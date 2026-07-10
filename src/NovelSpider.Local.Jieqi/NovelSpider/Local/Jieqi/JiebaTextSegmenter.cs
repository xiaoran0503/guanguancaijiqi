using System.Collections.Generic;
using JiebaNet.Segmenter;

namespace NovelSpider.Local.Jieqi;

public class JiebaTextSegmenter
{
	private static JiebaSegmenter _segmenter;

	public static void Init()
	{
		_segmenter ??= new JiebaSegmenter();
	}

	public IEnumerable<SegmentedWord> Segment(string text)
	{
		Init();
		foreach (string word in _segmenter.Cut(text ?? string.Empty))
		{
			yield return new SegmentedWord { Word = word };
		}
	}
}

public class SegmentedWord
{
	public string Word { get; set; }
}
