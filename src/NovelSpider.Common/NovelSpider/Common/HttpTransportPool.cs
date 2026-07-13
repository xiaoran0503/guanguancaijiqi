using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NovelSpider.Common;

internal sealed class HttpTransportOptions : IEquatable<HttpTransportOptions>
{
	public bool UseProxy { get; init; }

	public string ProxyHost { get; init; } = string.Empty;

	public int ProxyPort { get; init; }

	public string ProxyDomain { get; init; } = string.Empty;

	public string ProxyUser { get; init; } = string.Empty;

	public string ProxyPassword { get; init; } = string.Empty;

	public bool Equals(HttpTransportOptions other)
	{
		if (other == null)
		{
			return false;
		}
		return UseProxy == other.UseProxy &&
			ProxyPort == other.ProxyPort &&
			string.Equals(ProxyHost, other.ProxyHost, StringComparison.OrdinalIgnoreCase) &&
			string.Equals(ProxyDomain, other.ProxyDomain, StringComparison.OrdinalIgnoreCase) &&
			string.Equals(ProxyUser, other.ProxyUser, StringComparison.Ordinal) &&
			string.Equals(ProxyPassword, other.ProxyPassword, StringComparison.Ordinal);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as HttpTransportOptions);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(UseProxy, ProxyHost?.ToUpperInvariant(), ProxyPort, ProxyDomain, ProxyUser, ProxyPassword);
	}
}

internal static class HttpTransportPool
{
	private static readonly ConcurrentDictionary<HttpTransportOptions, Lazy<System.Net.Http.HttpClient>> Clients = new();

	public static HttpResponseMessage Send(HttpRequestMessage request, HttpTransportOptions options, int timeoutSeconds)
	{
		Net10RuntimeBootstrap.Initialize();
		bool reusedTransport = Clients.TryGetValue(options, out Lazy<System.Net.Http.HttpClient> existingClient) && existingClient.IsValueCreated;
		System.Net.Http.HttpClient client = Clients.GetOrAdd(options, static item => new Lazy<System.Net.Http.HttpClient>(() => CreateClient(item))).Value;
		using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds > 0 ? timeoutSeconds : 20));
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			HttpResponseMessage response = client.Send(request, HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token);
			stopwatch.Stop();
			PerformanceTelemetry.Record("http", "send", stopwatch.ElapsedMilliseconds, request.RequestUri?.Host ?? string.Empty, message: $"status={(int)response.StatusCode};transport={(reusedTransport ? "reused" : "new")}");
			return response;
		}
		catch (OperationCanceledException)
		{
			stopwatch.Stop();
			PerformanceTelemetry.Record("http", "send", stopwatch.ElapsedMilliseconds, request.RequestUri?.Host ?? string.Empty, succeed: false, message: "timeout");
			throw;
		}
		catch (HttpRequestException exception)
		{
			stopwatch.Stop();
			PerformanceTelemetry.Record("http", "send", stopwatch.ElapsedMilliseconds, request.RequestUri?.Host ?? string.Empty, succeed: false, message: "network_error:" + exception.Message);
			throw;
		}
	}

	public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpTransportOptions options, int timeoutSeconds, CancellationToken cancellationToken = default)
	{
		Net10RuntimeBootstrap.Initialize();
		bool reusedTransport = Clients.TryGetValue(options, out Lazy<System.Net.Http.HttpClient> existingClient) && existingClient.IsValueCreated;
		System.Net.Http.HttpClient client = Clients.GetOrAdd(options, static item => new Lazy<System.Net.Http.HttpClient>(() => CreateClient(item))).Value;
		using CancellationTokenSource timeoutTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds > 0 ? timeoutSeconds : 20));
		using CancellationTokenSource linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutTokenSource.Token);
		Stopwatch stopwatch = Stopwatch.StartNew();
		try
		{
			HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, linkedTokenSource.Token).ConfigureAwait(false);
			stopwatch.Stop();
			PerformanceTelemetry.Record("http", "send_async", stopwatch.ElapsedMilliseconds, request.RequestUri?.Host ?? string.Empty, message: $"status={(int)response.StatusCode};transport={(reusedTransport ? "reused" : "new")}");
			return response;
		}
		catch (OperationCanceledException)
		{
			stopwatch.Stop();
			PerformanceTelemetry.Record("http", "send_async", stopwatch.ElapsedMilliseconds, request.RequestUri?.Host ?? string.Empty, succeed: false, message: cancellationToken.IsCancellationRequested ? "cancelled" : "timeout");
			throw;
		}
		catch (HttpRequestException exception)
		{
			stopwatch.Stop();
			PerformanceTelemetry.Record("http", "send_async", stopwatch.ElapsedMilliseconds, request.RequestUri?.Host ?? string.Empty, succeed: false, message: "network_error:" + exception.Message);
			throw;
		}
	}

	private static System.Net.Http.HttpClient CreateClient(HttpTransportOptions options)
	{
		SocketsHttpHandler handler = new SocketsHttpHandler
		{
			AllowAutoRedirect = false,
			UseCookies = false,
			AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
			PooledConnectionLifetime = TimeSpan.FromMinutes(10),
			PooledConnectionIdleTimeout = TimeSpan.FromMinutes(2),
			MaxConnectionsPerServer = 16,
			Expect100ContinueTimeout = TimeSpan.Zero,
			ConnectTimeout = TimeSpan.FromSeconds(20)
		};
		if (options.UseProxy && !string.IsNullOrWhiteSpace(options.ProxyHost))
		{
			WebProxy webProxy = new WebProxy(options.ProxyHost, options.ProxyPort);
			if (!string.IsNullOrEmpty(options.ProxyUser) && !string.IsNullOrEmpty(options.ProxyPassword))
			{
				webProxy.Credentials = string.IsNullOrEmpty(options.ProxyDomain)
					? new NetworkCredential(options.ProxyUser, options.ProxyPassword)
					: new NetworkCredential(options.ProxyUser, options.ProxyPassword, options.ProxyDomain);
			}
			handler.Proxy = webProxy;
			handler.UseProxy = true;
		}
		else
		{
			handler.ConnectCallback = DnsEndpointCache.ConnectAsync;
		}
		return new System.Net.Http.HttpClient(handler)
		{
			Timeout = Timeout.InfiniteTimeSpan
		};
	}
}
