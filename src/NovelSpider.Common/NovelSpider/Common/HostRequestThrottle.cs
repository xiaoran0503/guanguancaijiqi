using System;
using System.Collections.Concurrent;
using System.Threading;

namespace NovelSpider.Common;

public static class HostRequestThrottle
{
	private sealed class HostState
	{
		public readonly object SyncRoot = new();

		public DateTime LastRequestUtc = DateTime.MinValue;
	}

	private static readonly ConcurrentDictionary<string, HostState> States = new(StringComparer.OrdinalIgnoreCase);

	public static void Wait(string host, int intervalMilliseconds)
	{
		if (intervalMilliseconds <= 0)
		{
			return;
		}
		host = string.IsNullOrWhiteSpace(host) ? "*" : host.Trim();
		HostState state = States.GetOrAdd(host, static _ => new HostState());
		lock (state.SyncRoot)
		{
			DateTime now = DateTime.UtcNow;
			if (state.LastRequestUtc != DateTime.MinValue)
			{
				int remainingMilliseconds = intervalMilliseconds - (int)(now - state.LastRequestUtc).TotalMilliseconds;
				if (remainingMilliseconds > 0)
				{
					Thread.Sleep(remainingMilliseconds);
					now = DateTime.UtcNow;
				}
			}
			state.LastRequestUtc = now;
		}
	}
}
