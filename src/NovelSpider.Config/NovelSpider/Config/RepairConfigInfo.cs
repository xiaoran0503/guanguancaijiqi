using System;
using System.Runtime.CompilerServices;

namespace NovelSpider.Config;

[Serializable]
public class RepairConfigInfo : IConfigInfo
{
	private DateTime dateTime_0;

	private DateTime dateTime_1;

	private int int_0;

	public bool AddTime { get; set; }

	public bool Boolean_0 { get; set; }

	public int ChapterNum { get; set; }

	public int ChapterUrlWait { get; set; }

	public bool CheckRepeat { get; set; }

	public bool CheckVolume { get; set; }

	public bool ClearRectMark { get; set; }

	public bool DelForTxt { get; set; }

	public bool DelForTxtHtml { get; set; }

	public int EmptyChapter { get; set; }

	public int ErrorNum { get; set; }

	public bool FilterFinish { get; set; }

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

	public int GoRepeatChapter { get; set; }

	public bool Hidebook { get; set; }

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

	public bool NewBook { get; set; }

	public bool NoPBar { get; set; }

	public int NovelUrlWait { get; set; }

	public int OrderChapter { get; set; }

	public bool ProhibitionVolume { get; set; }

	public bool Proxy { get; set; }

	public string ProxyDomain { get; set; }

	public string ProxyHost { get; set; }

	public string ProxyPassword { get; set; }

	public int ProxyPort { get; set; }

	public string ProxyUser { get; set; }

	public int ReMoteChapterNum { get; set; }

	public int RepeatChapterHandle { get; set; }

	public bool ReplaceChapterNameOn { get; set; }

	public bool Timing { get; set; }

	public bool UpdateChapterName { get; set; }

	public bool Md5Chapter
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return true;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	public RepairConfigInfo()
	{
		dateTime_0 = DateTime.Now;
		dateTime_1 = DateTime.Now;
		int_0 = 10000;
	}
}
