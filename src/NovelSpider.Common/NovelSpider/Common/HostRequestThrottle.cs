using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
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

	public static int Wait(string host, int minMilliseconds, int maxMilliseconds, string requestKind)
	{
		int delay = GetDelayMilliseconds(host, minMilliseconds, maxMilliseconds, requestKind);
		if (delay > 0)
		{
			Thread.Sleep(delay);
		}
		return delay;
	}

	private static int GetDelayMilliseconds(string host, int minMilliseconds, int maxMilliseconds, string requestKind)
	{
		(minMilliseconds, maxMilliseconds) = RequestDelayProfile.Normalize(minMilliseconds, maxMilliseconds);
		if (maxMilliseconds <= 0)
		{
			return 0;
		}
		host = NormalizeHost(host);
		int intervalMilliseconds = minMilliseconds == maxMilliseconds ? minMilliseconds : Random.Shared.Next(minMilliseconds, maxMilliseconds + 1);
		HostState state = States.GetOrAdd(host, static _ => new HostState());
		lock (state.SyncRoot)
		{
			DateTime now = DateTime.UtcNow;
			int remainingMilliseconds = 0;
			if (state.LastRequestUtc != DateTime.MinValue)
			{
				remainingMilliseconds = intervalMilliseconds - (int)(now - state.LastRequestUtc).TotalMilliseconds;
				if (remainingMilliseconds > 0)
				{
					PerformanceTelemetry.Record("http", "request_delay", remainingMilliseconds, host, message: "kind=" + requestKind);
				}
			}
			state.LastRequestUtc = remainingMilliseconds > 0 ? now.AddMilliseconds(remainingMilliseconds) : now;
			return Math.Max(0, remainingMilliseconds);
		}
	}


	public static async Task<int> WaitAsync(string host, int intervalMilliseconds, CancellationToken cancellationToken = default)
	{
		return await WaitAsync(host, intervalMilliseconds, intervalMilliseconds, string.Empty, cancellationToken).ConfigureAwait(false);
	}

	public static async Task<int> WaitAsync(string host, int minMilliseconds, int maxMilliseconds, string requestKind, CancellationToken cancellationToken = default)
	{
		int delay = GetDelayMilliseconds(host, minMilliseconds, maxMilliseconds, requestKind);
		if (delay > 0)
		{
			await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
		}
		return delay;
	}

	public static HostConcurrencyLease Enter(string host, int limit)
	{
		limit = limit <= 0 ? 1 : limit;
		host = NormalizeHost(host);
		SemaphoreSlim semaphore = Semaphores.GetOrAdd(host + "|" + limit, static (_, count) => new SemaphoreSlim(count, count), limit);
		semaphore.Wait();
		return new HostConcurrencyLease(semaphore);
	}

	public static async Task<HostConcurrencyLease> EnterAsync(string host, int limit, CancellationToken cancellationToken = default)
	{
		limit = limit <= 0 ? 1 : limit;
		host = NormalizeHost(host);
		SemaphoreSlim semaphore = Semaphores.GetOrAdd(host + "|" + limit, static (_, count) => new SemaphoreSlim(count, count), limit);
		await semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
		return new HostConcurrencyLease(semaphore);
	}

	public static void WaitForBackoff(string host, TaskConfigInfo taskConfig, string requestKind)
	{
		int delay = GetBackoffDelay(host, taskConfig, requestKind);
		if (delay > 0)
		{
			Thread.Sleep(delay);
		}
	}

	public static async Task WaitForBackoffAsync(string host, TaskConfigInfo taskConfig, string requestKind, CancellationToken cancellationToken = default)
	{
		int delay = GetBackoffDelay(host, taskConfig, requestKind);
		if (delay > 0)
		{
			await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
		}
	}

	private static int GetBackoffDelay(string host, TaskConfigInfo taskConfig, string requestKind)
	{
		if (taskConfig == null || !taskConfig.RequestBackoffEnabled)
		{
			return 0;
		}
		host = NormalizeHost(host);
		FailureState state = Failures.GetOrAdd(host, static _ => new FailureState());
		int failures = Volatile.Read(ref state.ConsecutiveFailures);
		if (failures <= 0)
		{
			return 0;
		}
		int delay = Math.Min(30000, failures * failures * 500);
		PerformanceTelemetry.Record("http", "request_backoff", delay, host, succeed: false, message: "kind=" + requestKind + ";failures=" + failures);
		return delay;
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
