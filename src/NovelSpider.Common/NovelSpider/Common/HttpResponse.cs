using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelSpider.Common;

public class HttpResponse
{
	public int ContentLength;

	public string ContentType;

	public CookieCollection Cookies;

	public string CookieString;

	public string Header;

	public WebHeaderCollection Headers;

	public bool KeepAlive;

	public Uri ResponseUri;

	public Socket socket;

	public int StatusCode;

	public HttpResponse()
	{
		Cookies = new CookieCollection();
	}

	public void Close()
	{
		socket.Close();
	}

	public void Connect(HttpRequest httpRequest_0)
	{
		ResponseUri = httpRequest_0.RequestUri;
		socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		socket.Connect(new IPEndPoint(Dns.GetHostEntry(ResponseUri.Host).AddressList[0], ResponseUri.Port));
	}

	public void ReceiveHeader()
	{
		Header = "";
		Headers = new WebHeaderCollection();
		byte[] array = new byte[10];
		while (socket.Receive(array, 0, 1, SocketFlags.None) > 0)
		{
			string header = Header + Encoding.ASCII.GetString(array, 0, 1);
			Header = header;
			if (array[0] == 10 && Header.EndsWith("\r\n\r\n"))
			{
				break;
			}
		}
		MatchCollection matchCollection = new Regex("[^\r\n]+").Matches(Header.TrimEnd('\r', '\n'));
		for (int i = 1; i < matchCollection.Count; i++)
		{
			string[] array2 = matchCollection[i].Value.Split(new char[1] { ':' }, 2);
			if (array2.Length > 1)
			{
				if (array2[0].Trim() == "Set-Cookie")
				{
					Cookies.Add(HttpClient.ToCookies(array2[1].Replace("Set-Cookie:", "")));
				}
				Headers[array2[0].Trim()] = array2[1].Trim();
			}
		}
		try
		{
			StatusCode = Convert.ToInt32(matchCollection[0].Value.Split(' ')[1]);
		}
		catch
		{
		}
		if (matchCollection.Count > 0 && (matchCollection[0].Value.IndexOf(" 307 ") != -1 || matchCollection[0].Value.IndexOf(" 303 ") != -1 || matchCollection[0].Value.IndexOf(" 302 ") != -1 || matchCollection[0].Value.IndexOf(" 301 ") != -1) && Headers["Location"] != null)
		{
			try
			{
				ResponseUri = new Uri(Headers["Location"]);
			}
			catch
			{
				ResponseUri = new Uri(ResponseUri, Headers["Location"]);
			}
		}
		ContentType = Headers["Content-Type"];
		if (Headers["Content-Length"] != null)
		{
			ContentLength = int.Parse(Headers["Content-Length"]);
		}
		KeepAlive = (Headers["Connection"] != null && Headers["Connection"].ToLower() == "keep-alive") || (Headers["Proxy-Connection"] != null && Headers["Proxy-Connection"].ToLower() == "keep-alive");
	}

	public void SendRequest(HttpRequest httpRequest_0)
	{
		ResponseUri = httpRequest_0.RequestUri;
		httpRequest_0.Header = httpRequest_0.Method + " " + ResponseUri.PathAndQuery + " HTTP/1.0\r\n" + httpRequest_0.Headers;
		socket.Send(Encoding.ASCII.GetBytes(httpRequest_0.Header));
	}

	public void SetTimeout(int int_0)
	{
		socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, int_0 * 1000);
		socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, int_0 * 1000);
	}
}
