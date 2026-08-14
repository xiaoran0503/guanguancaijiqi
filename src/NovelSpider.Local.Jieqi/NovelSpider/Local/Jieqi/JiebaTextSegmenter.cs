using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NovelSpider.Local.Jieqi;

/// <summary>
/// 轻量中文分词器，替代已停更的 jieba.NET。
/// 采用 n-gram（2~3 字）+ 停用词过滤，零外部依赖，用于书名关键词/标签提取。
/// </summary>
public class JiebaTextSegmenter
{
	private static readonly HashSet<string> Stopwords = new HashSet<string>(StringComparer.Ordinal)
	{
		"的", "了", "是", "在", "和", "我", "你", "他", "她", "它", "这", "那", "与", "及", "等", "也", "都", "就", "而",
		"把", "被", "让", "给", "从", "向", "对", "于", "以", "为", "之", "其", "或", "若", "但", "因", "故", "着",
		"个", "们", "中", "上", "下", "里", "后", "前", "又", "再", "更", "最", "很", "太", "已经", "正在", "将要",
		"一个", "没有", "可以", "这样", "那样", "什么", "怎么", "如何", "一些", "这些", "那些", "自己", "我们", "你们", "他们"
	};

	private static readonly Regex Cjk = new Regex("[一-龥]+", RegexOptions.Compiled);

	public static void Init()
	{
	}

	public IEnumerable<SegmentedWord> Segment(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			yield break;
		}
		foreach (Match match in Cjk.Matches(text))
		{
			string run = match.Value;
			if (run.Length < 2)
			{
				continue;
			}
			HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
			int maxGram = System.Math.Min(3, run.Length);
			for (int n = 2; n <= maxGram; n++)
			{
				for (int i = 0; i + n <= run.Length; i++)
				{
					string gram = run.Substring(i, n);
					if (seen.Add(gram) && !Stopwords.Contains(gram))
					{
						yield return new SegmentedWord { Word = gram };
					}
				}
			}
		}
	}
}

public class SegmentedWord
{
	public string Word { get; set; }
}
