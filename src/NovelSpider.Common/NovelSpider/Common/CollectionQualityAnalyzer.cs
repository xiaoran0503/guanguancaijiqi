using System;
using System.Text.RegularExpressions;

namespace NovelSpider.Common;

public static class CollectionQualityAnalyzer
{
	public static string ClassifyException(Exception exception)
	{
		if (exception == null)
		{
			return "unknown";
		}
		string name = exception.GetType().Name;
		string message = exception.Message ?? string.Empty;
		if (message.Contains("DNS", StringComparison.OrdinalIgnoreCase) || message.Contains("NameResolution", StringComparison.OrdinalIgnoreCase))
		{
			return "dns";
		}
		if (message.Contains("timeout", StringComparison.OrdinalIgnoreCase) || exception is TimeoutException)
		{
			return "timeout";
		}
		if (message.Contains("404") || message.Contains("500") || message.Contains("403"))
		{
			return "http_status";
		}
		return name;
	}

	public static int ScoreChapterText(string htmlOrText)
	{
		if (string.IsNullOrWhiteSpace(htmlOrText))
		{
			return 0;
		}
		string text = Regex.Replace(htmlOrText, "<[^>]+>", " ");
		text = Regex.Replace(text, "\\s+", " ").Trim();
		int score = Math.Min(60, text.Length / 40);
		int linkCount = Regex.Matches(htmlOrText, "<a\\b", RegexOptions.IgnoreCase).Count;
		int tagCount = Regex.Matches(htmlOrText, "<[^>]+>").Count;
		if (text.Length > 1000)
		{
			score += 20;
		}
		if (tagCount > 0 && linkCount * 100 / Math.Max(tagCount, 1) > 30)
		{
			score -= 25;
		}
		if (Regex.IsMatch(text, "(最新网址|请收藏|手机阅读|广告|推荐票)"))
		{
			score -= 10;
		}
		return Math.Clamp(score, 0, 100);
	}
}
