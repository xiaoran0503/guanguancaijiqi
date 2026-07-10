using System;
using System.Net;

namespace NovelSpider.Common;

public class HttpRequest
{
	public string Header;

	public WebHeaderCollection Headers;

	public bool KeepAlive;

	public string Method;

	public Uri RequestUri;

	public int Timeout;
}
