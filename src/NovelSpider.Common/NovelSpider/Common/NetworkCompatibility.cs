using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelSpider.Common;

public static class NetworkCompatibility
{
	private static bool initialized;

	public static void Initialize()
	{
		if (initialized)
		{
			return;
		}

		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

		try
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
		}
		catch (NotSupportedException)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		}
		catch (ArgumentException)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		}

		ServicePointManager.Expect100Continue = false;
		Regex.CacheSize = Math.Max(Regex.CacheSize, 256);
		initialized = true;
	}
}
