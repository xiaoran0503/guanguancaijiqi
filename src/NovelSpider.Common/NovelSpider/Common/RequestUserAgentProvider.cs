using System;
using System.Collections.Concurrent;
using NovelSpider.Config;

namespace NovelSpider.Common;

public static class RequestUserAgentProvider
{
	private static readonly string[] DesktopAgents =
	{
		"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36",
		"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:127.0) Gecko/20100101 Firefox/127.0",
		"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 Edg/126.0.0.0",
		"Mozilla/5.0 (Macintosh; Intel Mac OS X 14_5) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.5 Safari/605.1.15"
	};

	private static readonly string[] MobileAgents =
	{
		"Mozilla/5.0 (iPhone; CPU iPhone OS 17_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.5 Mobile/15E148 Safari/604.1",
		"Mozilla/5.0 (Linux; Android 14; Pixel 8) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Mobile Safari/537.36",
		"Mozilla/5.0 (Linux; Android 14; SM-S928B) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Mobile Safari/537.36",
		"Mozilla/5.0 (iPad; CPU OS 17_5 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/17.5 Mobile/15E148 Safari/604.1"
	};

	private static readonly string[] CrawlerAgents =
	{
		"Mozilla/5.0 compatible; NovelSpider/10.4; +https://github.com/xiaoran0503/guanguancaijiqi",
		"NovelSpider-Net10/10.4 (+https://github.com/xiaoran0503/guanguancaijiqi)",
		"Mozilla/5.0 (compatible; NovelSpiderBot/10.4; Windows NT 10.0; Win64; x64)",
		"NovelSpiderRuleTester/10.4"
	};

	private static readonly ConcurrentDictionary<string, string> StickyAgents = new(StringComparer.OrdinalIgnoreCase);

	public static string Resolve(TaskConfigInfo taskConfig, string host)
	{
		string mode = taskConfig?.UserAgentMode ?? "Fixed";
		if (mode.Equals("DesktopBrowserRandom", StringComparison.OrdinalIgnoreCase))
		{
			return ResolveSticky(mode, host, DesktopAgents);
		}
		if (mode.Equals("MobileBrowserRandom", StringComparison.OrdinalIgnoreCase))
		{
			return ResolveSticky(mode, host, MobileAgents);
		}
		if (mode.Equals("CrawlerRandom", StringComparison.OrdinalIgnoreCase))
		{
			return ResolveSticky(mode, host, CrawlerAgents);
		}
		return Configs.BaseConfig.HttpUserAgent;
	}

	private static string ResolveSticky(string mode, string host, string[] pool)
	{
		string key = mode + "|" + (string.IsNullOrWhiteSpace(host) ? "*" : host.Trim().ToLowerInvariant());
		return StickyAgents.GetOrAdd(key, static (item, agents) => agents[Math.Abs(item.GetHashCode()) % agents.Length], pool);
	}
}
