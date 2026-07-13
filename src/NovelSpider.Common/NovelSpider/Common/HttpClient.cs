using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using NovelSpider.Config;

namespace NovelSpider.Common;

public class HttpClient
{
	private bool bool_0;

	private bool bool_1;

	private bool bool_2;

	private CookieCollection cookieCollection_0;

	private CookieCollection cookieCollection_1;

	private Encoding encoding_0;

	private Image image_0;

	private int int_0;

	private string string_0;

	private string string_1;

	private string string_2;

	private string string_3;

	private string string_4;

	private string string_5;

	private string string_6;

	private string string_7;

	private string string_8;

	private string string_9;

	public int Timeout;

	public string UserAgent;

	public string Cookies;

	public bool AllowAutoRedirect
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
		}
	}

	public CookieCollection CookieGet => cookieCollection_1;

	public CookieCollection CookiePost
	{
		get
		{
			return cookieCollection_0;
		}
		set
		{
			cookieCollection_0 = value;
		}
	}

	public Encoding Encoding
	{
		get
		{
			return encoding_0;
		}
		set
		{
			encoding_0 = value;
		}
	}

	public string Err
	{
		get
		{
			return string_5;
		}
		set
		{
			string_5 = value;
		}
	}

	public string PostData
	{
		get
		{
			return string_2;
		}
		set
		{
			string_2 = value;
		}
	}

	public bool Proxy
	{
		get
		{
			return bool_2;
		}
		set
		{
			bool_2 = value;
		}
	}

	public string ProxyDomain
	{
		get
		{
			return string_7;
		}
		set
		{
			string_7 = value;
		}
	}

	public string ProxyHost
	{
		get
		{
			return string_6;
		}
		set
		{
			string_6 = value;
		}
	}

	public string ProxyPassword
	{
		get
		{
			return string_9;
		}
		set
		{
			string_9 = value;
		}
	}

	public int ProxyPort
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = Convert.ToInt32(value);
		}
	}

	public string ProxyUser
	{
		get
		{
			return string_8;
		}
		set
		{
			string_8 = value;
		}
	}

	public string Referer
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
		}
	}

	public string ResHtml => string_3;

	public Image ResImage => image_0;

	public string StatusCode
	{
		get
		{
			return string_4;
		}
		set
		{
			string_4 = value;
		}
	}

	public bool Succeed
	{
		get
		{
			return bool_1;
		}
		set
		{
			bool_1 = value;
		}
	}

	public string UriString
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value.Replace("&amp;", "&");
		}
	}

	public HttpClient()
	{
		Net10RuntimeBootstrap.Initialize();
		bool_0 = true;
		encoding_0 = Encoding.Default;
	}

	public Image GetImageWork()
	{
		return GetImageWorkAsync().GetAwaiter().GetResult();
	}

	public async Task<Image> GetImageWorkAsync(CancellationToken cancellationToken = default)
	{
		Image image = null;
		try
		{
			Uri requestUri = new Uri(string_0);
			CookieContainer cookieContainer = CreateCookieContainer(requestUri);
			using HttpResponseMessage response = await SendModernRequestAsync(requestUri, cookieContainer, cancellationToken).ConfigureAwait(false);
			using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
			image = Image.FromStream(responseStream);
			CookieCollection cookies = cookieContainer.GetCookies(response.RequestMessage?.RequestUri ?? requestUri);
			if (cookies.Count > 0)
			{
				cookieCollection_1 = cookies;
			}
		}
		catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
		{
			SpiderException.Debug(ex.Message + string_0);
			string_5 = ex.Message;
		}
		catch (Exception ex) when (ex is not OperationCanceledException)
		{
			SpiderException.Debug(ex.Message + string_0);
			string_5 = ex.Message;
		}
		image_0 = image;
		return image_0;
	}

	public string GetStringWork()
	{
		if (!IsLegacySocketSite(string_0))
		{
			return GetStringWorkAsync().GetAwaiter().GetResult();
		}
		return GetStringWorkLegacySocket();
	}

	private string GetStringWorkLegacySocket()
	{
		HttpRequest httpRequest = new HttpRequest
		{
			RequestUri = new Uri(string_0),
			Method = "GET",
			Timeout = 20,
			Headers = new WebHeaderCollection()
		};
		httpRequest.Headers["Host"] = httpRequest.RequestUri.Host;
		httpRequest.Headers["Accept"] = "*/*";
		httpRequest.Headers["UserAgent"] = Configs.BaseConfig.HttpUserAgent;
		httpRequest.Headers["Connection"] = "Keep-Alive";
		HttpResponse httpResponse = new HttpResponse();
		httpResponse.Connect(httpRequest);
		httpResponse.SetTimeout(20);
		httpResponse.SendRequest(httpRequest);
		httpResponse.ReceiveHeader();
		if (httpResponse.StatusCode == 307 || httpResponse.StatusCode == 303 || httpResponse.StatusCode == 302 || httpResponse.StatusCode == 301)
		{
			httpResponse.Close();
			httpRequest.RequestUri = httpResponse.ResponseUri;
			httpRequest.Headers["Cookie"] = ToString(httpResponse.Cookies);
			httpResponse.Connect(httpRequest);
			httpResponse.SendRequest(httpRequest);
			httpResponse.ReceiveHeader();
		}
		MemoryStream memoryStream = new MemoryStream();
		StreamReader streamReader2 = null;
		StringBuilder stringBuilder2 = new StringBuilder();
		byte[] array2 = new byte[65536];
		int num3 = 0;
		int num4;
		while ((num4 = httpResponse.socket.Receive(array2, 0, 65536, SocketFlags.None)) > 0)
		{
			num3 += num4;
			memoryStream.Write(array2, 0, num4);
			stringBuilder2.Append(encoding_0.GetString(array2, 0, num4));
			if (httpResponse.KeepAlive && num3 >= httpResponse.ContentLength && httpResponse.ContentLength > 0)
			{
				break;
			}
		}
		memoryStream.Seek(0L, SeekOrigin.Begin);
		if (httpResponse.Headers["ContentEncoding"] == "gzip")
		{
			streamReader2 = new StreamReader(new GZipStream(memoryStream, CompressionMode.Decompress), encoding_0);
		}
		else if (httpResponse.Headers["ContentEncoding"] == "deflate")
		{
			streamReader2 = new StreamReader(new DeflateStream(memoryStream, CompressionMode.Decompress), encoding_0);
		}
		if (streamReader2 == null)
		{
			streamReader2 = new StreamReader(memoryStream, encoding_0);
		}
		StringBuilder stringBuilder3 = new StringBuilder();
		long num5 = 0L;
		char[] array3 = new char[256];
		for (int num6 = streamReader2.Read(array3, 0, array3.Length); num6 > 0; num6 = streamReader2.Read(array3, 0, array3.Length))
		{
			num5 = num6 + num5;
			stringBuilder3.Append(new string(array3, 0, num6));
		}
		string_3 = stringBuilder3.ToString();
		memoryStream.Close();
		httpResponse.Close();
		return string_3;
	}

	public async Task<string> GetStringWorkAsync(CancellationToken cancellationToken = default)
	{
		if (IsLegacySocketSite(string_0))
		{
			return await Task.Run(GetStringWorkLegacySocket, cancellationToken).ConfigureAwait(false);
		}

		Uri requestUri = new Uri(string_0);
		CookieContainer cookieContainer = CreateCookieContainer(requestUri);
		try
		{
			using HttpResponseMessage modernResponse = await SendModernRequestAsync(requestUri, cookieContainer, cancellationToken).ConfigureAwait(false);
			string_3 = await ReadResponseTextAsync(modernResponse, cancellationToken).ConfigureAwait(false);
			string_4 = ((int)modernResponse.StatusCode).ToString();
			if (string_4 == "302")
			{
				string_0 = modernResponse.RequestMessage?.RequestUri?.ToString();
				string_3 = modernResponse.Headers.Location?.ToString();
			}
			CookieCollection cookies = cookieContainer.GetCookies(modernResponse.RequestMessage?.RequestUri ?? requestUri);
			if (cookies.Count > 0)
			{
				cookieCollection_1 = cookies;
			}
		}
		catch (OperationCanceledException ex) when (!cancellationToken.IsCancellationRequested)
		{
			SpiderException.Debug(ex.Message + string_0);
			string_5 = ex.Message;
			return "";
		}
		catch (Exception ex) when (ex is not OperationCanceledException)
		{
			SpiderException.Debug(ex.Message + string_0);
			string_5 = ex.Message;
			return "";
		}
		return ResHtml;
	}

	private static bool IsLegacySocketSite(string uri)
	{
		return uri.IndexOf("16k.cn", StringComparison.OrdinalIgnoreCase) > 0 ||
			uri.IndexOf("16kxs.com", StringComparison.OrdinalIgnoreCase) > 0;
	}

	private CookieContainer CreateCookieContainer(Uri uri)
	{
		CookieContainer cookieContainer = new CookieContainer();
		if (cookieCollection_0 != null)
		{
			foreach (Cookie item in cookieCollection_0)
			{
				item.Domain = uri.Host;
			}
			cookieContainer.Add(cookieCollection_0);
		}
		return cookieContainer;
	}

	private async Task<HttpResponseMessage> SendModernRequestAsync(Uri requestUri, CookieContainer cookieContainer, CancellationToken cancellationToken = default)
	{
		Uri currentUri = requestUri;
		bool forceGet = false;
		for (int i = 0; i < 10; i++)
		{
			HttpRequestMessage request = CreateModernHttpRequest(currentUri, forceGet);
			ApplyRequestCookies(request, currentUri, cookieContainer);
			HttpResponseMessage response = await HttpTransportPool.SendAsync(request, CreateTransportOptions(), Timeout > 0 ? Timeout : Configs.BaseConfig.HttpTimeOut, cancellationToken).ConfigureAwait(false);
			StoreResponseCookies(response, currentUri, cookieContainer);
			if (!bool_0 || !IsRedirect(response) || response.Headers.Location == null)
			{
				return response;
			}
			Uri nextUri = response.Headers.Location.IsAbsoluteUri ? response.Headers.Location : new Uri(currentUri, response.Headers.Location);
			forceGet = response.StatusCode == HttpStatusCode.SeeOther ||
				((response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Redirect) && request.Method == HttpMethod.Post);
			response.Dispose();
			request.Dispose();
			currentUri = nextUri;
		}
		HttpRequestMessage finalRequest = CreateModernHttpRequest(currentUri, forceGet: true);
		ApplyRequestCookies(finalRequest, currentUri, cookieContainer);
		HttpResponseMessage finalResponse = await HttpTransportPool.SendAsync(finalRequest, CreateTransportOptions(), Timeout > 0 ? Timeout : Configs.BaseConfig.HttpTimeOut, cancellationToken).ConfigureAwait(false);
		StoreResponseCookies(finalResponse, currentUri, cookieContainer);
		return finalResponse;
	}

	private HttpTransportOptions CreateTransportOptions()
	{
		return new HttpTransportOptions
		{
			UseProxy = bool_2,
			ProxyHost = string_6 ?? string.Empty,
			ProxyPort = int_0,
			ProxyDomain = string_7 ?? string.Empty,
			ProxyUser = string_8 ?? string.Empty,
			ProxyPassword = string_9 ?? string.Empty
		};
	}

	private static bool IsRedirect(HttpResponseMessage response)
	{
		return response.StatusCode == HttpStatusCode.Moved ||
			response.StatusCode == HttpStatusCode.Redirect ||
			response.StatusCode == HttpStatusCode.RedirectMethod ||
			response.StatusCode == HttpStatusCode.SeeOther ||
			response.StatusCode == HttpStatusCode.TemporaryRedirect ||
			response.StatusCode == HttpStatusCode.PermanentRedirect;
	}

	private static void ApplyRequestCookies(HttpRequestMessage request, Uri uri, CookieContainer cookieContainer)
	{
		string cookieHeader = cookieContainer.GetCookieHeader(uri);
		if (!string.IsNullOrEmpty(cookieHeader))
		{
			request.Headers.TryAddWithoutValidation("Cookie", cookieHeader);
		}
	}

	private static void StoreResponseCookies(HttpResponseMessage response, Uri uri, CookieContainer cookieContainer)
	{
		if (!response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
		{
			return;
		}
		foreach (string value in values)
		{
			try
			{
				cookieContainer.SetCookies(uri, value);
			}
			catch (CookieException)
			{
			}
		}
	}

	private async Task<string> ReadResponseTextAsync(HttpResponseMessage response, CancellationToken cancellationToken = default)
	{
		using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
		using StreamReader streamReader = new StreamReader(responseStream, encoding_0);
		StringBuilder stringBuilder = new StringBuilder();
		char[] array = new char[4096];
		for (int count = await streamReader.ReadAsync(array.AsMemory(0, array.Length), cancellationToken).ConfigureAwait(false); count > 0; count = await streamReader.ReadAsync(array.AsMemory(0, array.Length), cancellationToken).ConfigureAwait(false))
		{
			stringBuilder.Append(array, 0, count);
		}
		return stringBuilder.ToString();
	}

	private HttpRequestMessage CreateModernHttpRequest(Uri uri, bool forceGet)
	{
		bool hasPostData = !forceGet && string_2 != null && string_2.Length > 0;
		HttpRequestMessage request = new HttpRequestMessage(hasPostData ? HttpMethod.Post : HttpMethod.Get, uri);
		request.Headers.TryAddWithoutValidation("Accept", "*/*");
		request.Headers.TryAddWithoutValidation("User-Agent", string.IsNullOrEmpty(UserAgent) ? Configs.BaseConfig.HttpUserAgent : UserAgent);
		if (!string.IsNullOrEmpty(string_1))
		{
			request.Headers.TryAddWithoutValidation("Referer", string_1);
		}
		request.Headers.ExpectContinue = false;
		if (hasPostData)
		{
			request.Content = new ByteArrayContent(encoding_0.GetBytes(string_2));
			request.Content.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
		}
		return request;
	}

	public static string ToString(CookieCollection cookieCollection)
	{
		if (cookieCollection == null || cookieCollection.Count == 0)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Cookie cookie in cookieCollection)
		{
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append("; ");
			}
			stringBuilder.Append(cookie.Name).Append('=').Append(cookie.Value);
		}
		return stringBuilder.ToString();
	}

	public static Cookie ToCookies(string cookieText)
	{
		string[] parts = cookieText.Split(';');
		string[] nameValue = parts[0].Split(new char[1] { '=' }, 2);
		Cookie cookie = new Cookie(nameValue[0].Trim(), nameValue.Length > 1 ? nameValue[1].Trim() : string.Empty);
		for (int i = 1; i < parts.Length; i++)
		{
			string[] attribute = parts[i].Split(new char[1] { '=' }, 2);
			string name = attribute[0].Trim();
			string value = attribute.Length > 1 ? attribute[1].Trim() : string.Empty;
			if (name.Equals("path", StringComparison.OrdinalIgnoreCase))
			{
				cookie.Path = value;
			}
			else if (name.Equals("domain", StringComparison.OrdinalIgnoreCase))
			{
				cookie.Domain = value;
			}
		}
		return cookie;
	}

}

