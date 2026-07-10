using System;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelSpider.Common;

public static class Net10RuntimeBootstrap
{
	private static bool initialized;

	public static void Initialize()
	{
		if (initialized)
		{
			return;
		}

		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		Regex.CacheSize = Math.Max(Regex.CacheSize, 256);
		initialized = true;
	}
}
