using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NovelSpider.Common;

internal static class DnsEndpointCache
{
	private const int MaximumEntries = 512;

	private static readonly TimeSpan TimeToLive = TimeSpan.FromMinutes(30);

	private static readonly ConcurrentDictionary<string, CacheEntry> Entries = new(StringComparer.OrdinalIgnoreCase);

	public static async ValueTask<Stream> ConnectAsync(SocketsHttpConnectionContext context, CancellationToken cancellationToken)
	{
		DnsEndPoint endpoint = context.DnsEndPoint;
		string host = endpoint.Host;
		if (!RequiresCache(host))
		{
			return await ConnectUncachedAsync(host, endpoint.Port, cancellationToken).ConfigureAwait(false);
		}

		string key = host.Trim().ToUpperInvariant();
		if (Entries.TryGetValue(key, out CacheEntry cached))
		{
			if (!cached.IsExpired)
			{
				PerformanceTelemetry.Record("dns", "cache_hit", 0, host);
				try
				{
					return await ConnectToAddressesAsync(cached.GetNextAddresses(), endpoint.Port, cancellationToken).ConfigureAwait(false);
				}
				catch (Exception ex) when (IsConnectionFailure(ex))
				{
					Remove(key, cached);
					PerformanceTelemetry.Record("dns", "cache_invalidated", 0, host, succeed: false, message: "cached_addresses_failed");
				}
			}
			else
			{
				Remove(key, cached);
				PerformanceTelemetry.Record("dns", "cache_expired", 0, host);
			}
		}

		CacheEntry refreshed = await ResolveAsync(host, "cache_miss", cancellationToken).ConfigureAwait(false);
		Add(key, refreshed);
		try
		{
			return await ConnectToAddressesAsync(refreshed.GetNextAddresses(), endpoint.Port, cancellationToken).ConfigureAwait(false);
		}
		catch (Exception ex) when (IsConnectionFailure(ex))
		{
			Remove(key, refreshed);
			PerformanceTelemetry.Record("dns", "cache_invalidated", 0, host, succeed: false, message: "refreshed_addresses_failed");
			CacheEntry resolvedAgain = await ResolveAsync(host, "resolve_retry", cancellationToken).ConfigureAwait(false);
			Add(key, resolvedAgain);
			return await ConnectToAddressesAsync(resolvedAgain.GetNextAddresses(), endpoint.Port, cancellationToken).ConfigureAwait(false);
		}
	}

	private static async ValueTask<Stream> ConnectUncachedAsync(string host, int port, CancellationToken cancellationToken)
	{
		if (IPAddress.TryParse(host, out IPAddress address))
		{
			return await ConnectToAddressesAsync([address], port, cancellationToken).ConfigureAwait(false);
		}

		Stopwatch stopwatch = Stopwatch.StartNew();
		IPAddress[] addresses = await Dns.GetHostAddressesAsync(host).WaitAsync(cancellationToken).ConfigureAwait(false);
		stopwatch.Stop();
		PerformanceTelemetry.Record("dns", "bypass", stopwatch.ElapsedMilliseconds, host, message: "localhost");
		return await ConnectToAddressesAsync(addresses, port, cancellationToken).ConfigureAwait(false);
	}

	private static async Task<CacheEntry> ResolveAsync(string host, string operation, CancellationToken cancellationToken)
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			IPAddress[] addresses = await Dns.GetHostAddressesAsync(host).WaitAsync(cancellationToken).ConfigureAwait(false);
			if (addresses.Length == 0)
			{
				throw new SocketException((int)SocketError.HostNotFound);
			}
			stopwatch.Stop();
			PerformanceTelemetry.Record("dns", operation, stopwatch.ElapsedMilliseconds, host, message: "addresses=" + addresses.Length);
			return new CacheEntry(addresses, DateTimeOffset.UtcNow.Add(TimeToLive));
		}
		catch
		{
			PerformanceTelemetry.Record("dns", operation, stopwatch.ElapsedMilliseconds, host, succeed: false, message: "resolve_failed");
			throw;
		}
		finally
		{
			if (stopwatch.IsRunning)
			{
				stopwatch.Stop();
			}
		}
	}

	private static async ValueTask<Stream> ConnectToAddressesAsync(IPAddress[] addresses, int port, CancellationToken cancellationToken)
	{
		Exception lastError = null;
		foreach (IPAddress address in addresses)
		{
			Socket socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				await socket.ConnectAsync(new IPEndPoint(address, port), cancellationToken).ConfigureAwait(false);
				return new NetworkStream(socket, ownsSocket: true);
			}
			catch (Exception ex) when (IsConnectionFailure(ex))
			{
				lastError = ex;
				socket.Dispose();
			}
		}

		throw lastError ?? new SocketException((int)SocketError.HostUnreachable);
	}

	private static bool RequiresCache(string host)
	{
		return !string.IsNullOrWhiteSpace(host)
			&& !string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
			&& !IPAddress.TryParse(host, out _);
	}

	private static bool IsConnectionFailure(Exception exception)
	{
		return exception is SocketException || exception is IOException;
	}

	private static void Add(string key, CacheEntry entry)
	{
		if (Entries.Count >= MaximumEntries)
		{
			foreach (KeyValuePair<string, CacheEntry> item in Entries)
			{
				if (item.Value.IsExpired || Entries.Count >= MaximumEntries)
				{
					Entries.TryRemove(item.Key, out _);
					break;
				}
			}
		}
		Entries[key] = entry;
	}

	private static void Remove(string key, CacheEntry entry)
	{
		if (Entries.TryGetValue(key, out CacheEntry current) && ReferenceEquals(current, entry))
		{
			Entries.TryRemove(key, out _);
		}
	}

	private sealed class CacheEntry
	{
		private readonly IPAddress[] addresses;

		private int nextAddress;

		public CacheEntry(IPAddress[] addresses, DateTimeOffset expiresAt)
		{
			this.addresses = addresses;
			ExpiresAt = expiresAt;
		}

		public DateTimeOffset ExpiresAt { get; }

		public bool IsExpired => DateTimeOffset.UtcNow >= ExpiresAt;

		public IPAddress[] GetNextAddresses()
		{
			if (addresses.Length < 2)
			{
				return addresses;
			}

			IPAddress[] result = new IPAddress[addresses.Length];
			int start = Math.Abs(Interlocked.Increment(ref nextAddress)) % addresses.Length;
			for (int index = 0; index < addresses.Length; index++)
			{
				result[index] = addresses[(start + index) % addresses.Length];
			}
			return result;
		}
	}
}
