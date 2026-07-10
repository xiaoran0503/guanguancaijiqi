using System.Collections.Generic;
using JiebaNet.Segmenter;

namespace PanGu;

public class Segment
{
	private static JiebaSegmenter _segmenter;

	public static void Init()
	{
		_segmenter ??= new JiebaSegmenter();
	}

	public IEnumerable<WordInfo> DoSegment(string text)
	{
		foreach (string word in _segmenter.Cut(text))
		{
			yield return new WordInfo { Word = word };
		}
	}
}

public class WordInfo
{
	public string Word { get; set; }
}
