using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace NovelSpider.Target;

internal static class RuleRegexCache
{
	private const int MaximumEntries = 512;

	private static readonly TimeSpan MatchTimeout = TimeSpan.FromSeconds(10);

	private static readonly ConcurrentDictionary<CacheKey, Regex> Cache = new();

	public static Regex Get(string pattern, RegexOptions options = RegexOptions.None)
	{
		CacheKey key = new(pattern, options);
		if (Cache.Count >= MaximumEntries && !Cache.ContainsKey(key))
		{
			Cache.Clear();
		}
		return Cache.GetOrAdd(key, static entry => new Regex(entry.Pattern, entry.Options, MatchTimeout));
	}

	private readonly record struct CacheKey(string Pattern, RegexOptions Options);
}
