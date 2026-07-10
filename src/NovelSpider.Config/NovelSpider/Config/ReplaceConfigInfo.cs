using System;

namespace NovelSpider.Config;

[Serializable]
public class ReplaceConfigInfo : IConfigInfo
{
	private DateTime dateTime_0;

	private DateTime dateTime_1;

	private int int_0;

	public bool AddTime { get; set; }

	public int ChapterNum { get; set; }

	public int ChapterUrlWait { get; set; }

	public bool ClearRectMark { get; set; }

	public int DownImageError { get; set; }

	public int EqualsChapter { get; set; }

	public string[] FilterNovel { get; set; }

	public int FilterNovelType { get; set; }

	public string[] FilterVolume { get; set; }

	public int FindMaxChapterNum
	{
		get
		{
			return int_0;
		}
		set
		{
			int_0 = value;
		}
	}

	public bool Finish { get; set; }

	public string[] GetListUrl { get; set; }

	public int GetOrderMaxId { get; set; }

	public int GetOrderMinId { get; set; }

	public string[] GetSinceId { get; set; }

	public string ID { get; set; }

	public int IndexUrlWait { get; set; }

	public int Interval { get; set; }

	public bool Log { get; set; }

	public DateTime MaxAddTime
	{
		get
		{
			return dateTime_1;
		}
		set
		{
			dateTime_1 = value;
		}
	}

	public DateTime MinAddTime
	{
		get
		{
			return dateTime_0;
		}
		set
		{
			dateTime_0 = value;
		}
	}

	public int MinChapterNum { get; set; }

	public bool NameAndAuthor { get; set; }

	public bool NoPBar { get; set; }

	public int NovelUrlWait { get; set; }

	public bool Proxy { get; set; }

	public string ProxyDomain { get; set; }

	public string ProxyHost { get; set; }

	public string ProxyPassword { get; set; }

	public int ProxyPort { get; set; }

	public string ProxyUser { get; set; }

	public int PutOrderMaxId { get; set; }

	public int PutOrderMinId { get; set; }

	public string[] PutSinceId { get; set; }

	public string PutSql { get; set; }

	public string RadioBy { get; set; }

	public int ReMoteChapterNum { get; set; }

	public bool ReplaceNA { get; set; }

	public bool ReplacePP { get; set; }

	public bool ReplacePT { get; set; }

	public bool ReplaceTP { get; set; }

	public bool ReplaceTT { get; set; }

	public string RuleFile { get; set; }

	public bool Timing { get; set; }

	public bool UpdateChapterName { get; set; }

	public int GoRepeatChapter { get; set; }

	public bool Md5Chapter { get; set; }

	public bool ReplaceChapterNameOn { get; set; }

	public int EmptyChapter { get; set; }

	public int OrderChapter { get; set; }

	public int RepeatChapterHandle { get; set; }

	public ReplaceConfigInfo()
	{
		dateTime_0 = DateTime.Now;
		dateTime_1 = DateTime.Now;
		int_0 = 10000;
	}
}
