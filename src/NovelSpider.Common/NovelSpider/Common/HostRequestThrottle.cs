using System;
using System.Collections.Concurrent;
using System.Threading;
using NovelSpider.Config;

namespace NovelSpider.Common;

public sealed class HostConcurrencyLease : IDisposable
{
	private readonly SemaphoreSlim semaphore;

	internal HostConcurrencyLease(SemaphoreSlim semaphore)
	{
		this.semaphore = semaphore;
	}

	public void Dispose()
	{
		semaphore.Release();
	}
}

public static class HostRequestThrottle
{
	private sealed class HostState
	{
		public readonly object SyncRoot = new();

		public DateTime LastRequestUtc = DateTime.MinValue;
	}

	private sealed class FailureState
	{
		public int ConsecutiveFailures;
	}

	private static readonly ConcurrentDictionary<string, HostState> States = new(StringComparer.OrdinalIgnoreCase);

	private static readonly ConcurrentDictionary<string, FailureState> Failures = new(StringComparer.OrdinalIgnoreCase);

	private static readonly ConcurrentDictionary<string, SemaphoreSlim> Semaphores = new(StringComparer.OrdinalIgnoreCase);

	public static void Wait(string host, int intervalMilliseconds)
	{
		Wait(host, intervalMilliseconds, intervalMilliseconds, string.Empty);
	}

	public static void Wait(string host, int minMilliseconds, int maxMilliseconds, string requestKind)
	{
		(minMilliseconds, maxMilliseconds) = RequestDelayProfile.Normalize(minMilliseconds, maxMilliseconds);
		if (maxMilliseconds <= 0)
		{
			return;
		}
		host = NormalizeHost(host);
		int intervalMilliseconds = minMilliseconds == maxMilliseconds ? minMilliseconds : Random.Shared.Next(minMilliseconds, maxMilliseconds + 1);
		HostState state = States.GetOrAdd(host, static _ => new HostState());
		lock (state.SyncRoot)
		{
			DateTime now = DateTime.UtcNow;
			if (state.LastRequestUtc != DateTime.MinValue)
			{
				int remainingMilliseconds = intervalMilliseconds - (int)(now - state.LastRequestUtc).TotalMilliseconds;
				if (remainingMilliseconds > 0)
				{
					PerformanceTelemetry.Record("http", "request_delay", remainingMilliseconds, host, message: "kind=" + requestKind);
					Thread.Sleep(remainingMilliseconds);
					now = DateTime.UtcNow;
				}
			}
			state.LastRequestUtc = now;
		}
	}

	public static HostConcurrencyLease Enter(string host, int limit)
	{
		limit = limit <= 0 ? 1 : limit;
		host = NormalizeHost(host);
		SemaphoreSlim semaphore = Semaphores.GetOrAdd(host + "|" + limit, static (_, count) => new SemaphoreSlim(count, count), limit);
		semaphore.Wait();
		return new HostConcurrencyLease(semaphore);
	}

	public static void WaitForBackoff(string host, TaskConfigInfo taskConfig, string requestKind)
	{
		if (taskConfig == null || !taskConfig.RequestBackoffEnabled)
		{
			return;
		}
		host = NormalizeHost(host);
		FailureState state = Failures.GetOrAdd(host, static _ => new FailureState());
		int failures = Volatile.Read(ref state.ConsecutiveFailures);
		if (failures <= 0)
		{
			return;
		}
		int delay = Math.Min(30000, failures * failures * 500);
		PerformanceTelemetry.Record("http", "request_backoff", delay, host, succeed: false, message: "kind=" + requestKind + ";failures=" + failures);
		Thread.Sleep(delay);
	}

	public static void ReportResult(string host, TaskConfigInfo taskConfig, bool succeeded, string reason = "")
	{
		if (taskConfig == null || !taskConfig.RequestBackoffEnabled)
		{
			return;
		}
		host = NormalizeHost(host);
		FailureState state = Failures.GetOrAdd(host, static _ => new FailureState());
		if (succeeded)
		{
			Volatile.Write(ref state.ConsecutiveFailures, 0);
			return;
		}
		int failures = Interlocked.Increment(ref state.ConsecutiveFailures);
		PerformanceTelemetry.Record("http", "request_failure", 0, host, succeed: false, message: "reason=" + reason + ";failures=" + failures);
	}

	private static string NormalizeHost(string host)
	{
		return string.IsNullOrWhiteSpace(host) ? "*" : host.Trim();
	}
}
