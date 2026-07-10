using System;
using System.Runtime.CompilerServices;

namespace NovelSpider.Config;

[Serializable]
public class TaskConfigInfo : IConfigInfo
{
	private bool bool_0;

	private bool bool_1;

	private bool bool_12;

	private bool bool_16;

	private bool bool_17;

	private bool bool_18;

	private bool bool_19;

	private bool bool_2;

	private bool bool_20;

	private bool bool_21;

	private bool bool_22;

	private bool bool_23;

	private bool bool_24;

	private bool bool_25;

	private bool bool_26;

	private bool boolChkMD5;

	private bool forceReplace;

	private bool bool_28;

	private bool bool_29;

	private bool bool_3;

	private bool bool_30;

	private bool bool_32;

	private bool bool_33;

	private bool bool_34;

	private bool bool_35;

	private bool bool_36;

	private bool bool_37;

	private bool bool_38;

	private bool bool_39;

	private bool bool_4;

	private bool bool_5;

	private bool bool_6;

	private bool bool_7;

	private bool bool_8;

	private int int_0;

	private int int_1;

	private int int_10;

	private int int_11;

	private int int_12;

	private int int_13;

	private int int_15;

	private int int_16;

	private int int_19;

	private int int_2;

	private int int_20;

	private int int_21;

	private int int_22;

	private int int_23;

	private int int_24;

	private int int_25;

	private int int_26;

	private int int_3;

	private int int_4;

	private int int_5;

	private int int_7;

	private int int_8;

	private int int_9;

	private string string_0;

	private string string_1;

	private string[] string_10;

	private string string_11;

	private string string_12;

	private string string_13;

	private string string_14;

	private string string_15;

	private string[] string_16;

	private string[] string_17;

	private string[] string_18;

	private string[] string_2;

	private string[] string_3;

	private string[] string_4;

	private string string_5;

	private string string_6;

	private string string_7;

	private string string_8;

	private string[] string_9;

	private bool isboy;

	private int int_vip标示;

	private int int_上架标示;

	private bool bool_ReplaceChapterIndex;

	private bool bool_ReplaceChapterTime;

	private bool bool_11;

	private bool thdskpulRH;

	private bool bool_10;

	private bool bool_13;

	public bool Isboy
	{
		get
		{
			return isboy;
		}
		set
		{
			isboy = value;
		}
	}

	public int ChapterUrlWait
	{
		get
		{
			return int_10;
		}
		set
		{
			int_10 = value;
		}
	}

	public bool CheckRepeat
	{
		get
		{
			return bool_7;
		}
		set
		{
			bool_7 = value;
		}
	}

	public bool CheckVolume
	{
		get
		{
			return bool_3;
		}
		set
		{
			bool_3 = value;
		}
	}

	public bool CompulsoryDeleteChapter
	{
		get
		{
			return bool_37;
		}
		set
		{
			bool_37 = value;
		}
	}

	public bool isChkMD5
	{
		get
		{
			return boolChkMD5;
		}
		set
		{
			boolChkMD5 = value;
		}
	}

	public bool ForceReplace
	{
		get
		{
			return forceReplace;
		}
		set
		{
			forceReplace = value;
		}
	}

	public bool DeleteChapter
	{
		get
		{
			return bool_38;
		}
		set
		{
			bool_38 = value;
		}
	}

	public bool DelForHtml
	{
		get
		{
			return bool_33;
		}
		set
		{
			bool_33 = value;
		}
	}

	public bool DelForTxt
	{
		get
		{
			return bool_34;
		}
		set
		{
			bool_34 = value;
		}
	}

	public int DonnotCollectLastChapterNo
	{
		get
		{
			return int_11;
		}
		set
		{
			int_11 = value;
		}
	}

	public bool DownImage
	{
		get
		{
			return bool_39;
		}
		set
		{
			bool_39 = value;
		}
	}

	public int DownImageError
	{
		get
		{
			return int_25;
		}
		set
		{
			int_25 = value;
		}
	}

	public bool DuanImage
	{
		get
		{
			return bool_19;
		}
		set
		{
			bool_19 = value;
		}
	}

	public bool DuanImageCheck
	{
		get
		{
			return bool_32;
		}
		set
		{
			bool_32 = value;
		}
	}

	public int EmptyChapter
	{
		get
		{
			return int_24;
		}
		set
		{
			int_24 = value;
		}
	}

	public int EqualsChapter
	{
		get
		{
			return int_23;
		}
		set
		{
			int_23 = value;
		}
	}

	public string[] FilterChapterName
	{
		get
		{
			return string_9;
		}
		set
		{
			string_9 = value;
		}
	}

	public string[] FilterContinueChapterName
	{
		get
		{
			return string_10;
		}
		set
		{
			string_10 = value;
		}
	}

	public string[] FilterContinueVolume
	{
		get
		{
			return string_4;
		}
		set
		{
			string_4 = value;
		}
	}

	public bool FilterFinish
	{
		get
		{
			return bool_35;
		}
		set
		{
			bool_35 = value;
		}
	}

	public string[] FilterNovel
	{
		get
		{
			return string_3;
		}
		set
		{
			string_3 = value;
		}
	}

	public int FilterNovelType
	{
		get
		{
			return int_5;
		}
		set
		{
			int_5 = value;
		}
	}

	public string[] FilterVolume
	{
		get
		{
			return string_2;
		}
		set
		{
			string_2 = value;
		}
	}

	public int FindMaxChapterNum
	{
		get
		{
			return int_1;
		}
		set
		{
			int_1 = value;
		}
	}

	public bool Finish
	{
		get
		{
			return bool_8;
		}
		set
		{
			bool_8 = value;
		}
	}

	public string[] GetListUrl
	{
		get
		{
			return string_16;
		}
		set
		{
			string_16 = value;
		}
	}

	public int GetOrderMaxId
	{
		get
		{
			return int_19;
		}
		set
		{
			int_19 = value;
		}
	}

	public int GetOrderMinId
	{
		get
		{
			return int_12;
		}
		set
		{
			int_12 = value;
		}
	}

	public string[] GetSinceId
	{
		get
		{
			return string_17;
		}
		set
		{
			string_17 = value;
		}
	}

	public bool Hidebook
	{
		get
		{
			return bool_18;
		}
		set
		{
			bool_18 = value;
		}
	}

	public string ID
	{
		get
		{
			return string_13;
		}
		set
		{
			string_13 = value;
		}
	}

	public int IndexUrlWait
	{
		get
		{
			return int_9;
		}
		set
		{
			int_9 = value;
		}
	}

	public int Interval
	{
		get
		{
			return int_22;
		}
		set
		{
			int_22 = value;
		}
	}

	public bool Log
	{
		get
		{
			return bool_0;
		}
		set
		{
			bool_0 = value;
		}
	}

	public int MinChapterNum
	{
		get
		{
			return int_4;
		}
		set
		{
			int_4 = value;
		}
	}

	public int MinChapterTextLength
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

	public bool NameAndAuthor
	{
		get
		{
			return bool_6;
		}
		set
		{
			bool_6 = value;
		}
	}

	public bool NewBook
	{
		get
		{
			return bool_12;
		}
		set
		{
			bool_12 = value;
		}
	}

	public bool NoPBar
	{
		get
		{
			return bool_17;
		}
		set
		{
			bool_17 = value;
		}
	}

	public int NovelUrlWait
	{
		get
		{
			return int_8;
		}
		set
		{
			int_8 = value;
		}
	}

	public bool OldBook
	{
		get
		{
			return bool_23;
		}
		set
		{
			bool_23 = value;
		}
	}

	public bool OnlyReplaceSort
	{
		get
		{
			return bool_25;
		}
		set
		{
			bool_25 = value;
		}
	}

	public bool OnlyText
	{
		get
		{
			return bool_5;
		}
		set
		{
			bool_5 = value;
		}
	}

	public int OrderChapter
	{
		get
		{
			return int_3;
		}
		set
		{
			int_3 = value;
		}
	}

	public string OtherEncoding
	{
		get
		{
			return string_11;
		}
		set
		{
			string_11 = value;
		}
	}

	public string OtherListUrl
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
		}
	}

	public string OtherRegex
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
		}
	}

	public bool ProhibitionVolume
	{
		get
		{
			return bool_2;
		}
		set
		{
			bool_2 = value;
		}
	}

	public bool Proxy
	{
		get
		{
			return bool_16;
		}
		set
		{
			bool_16 = value;
		}
	}

	public string ProxyDomain
	{
		get
		{
			return string_6;
		}
		set
		{
			string_6 = value;
		}
	}

	public string ProxyHost
	{
		get
		{
			return string_5;
		}
		set
		{
			string_5 = value;
		}
	}

	public string ProxyPassword
	{
		get
		{
			return string_8;
		}
		set
		{
			string_8 = value;
		}
	}

	public int ProxyPort
	{
		get
		{
			return int_7;
		}
		set
		{
			int_7 = value;
		}
	}

	public string ProxyUser
	{
		get
		{
			return string_7;
		}
		set
		{
			string_7 = value;
		}
	}

	public int PutOrderMaxId
	{
		get
		{
			return int_21;
		}
		set
		{
			int_21 = value;
		}
	}

	public int PutOrderMinId
	{
		get
		{
			return int_20;
		}
		set
		{
			int_20 = value;
		}
	}

	public string[] PutSinceId
	{
		get
		{
			return string_18;
		}
		set
		{
			string_18 = value;
		}
	}

	public string RadioBy
	{
		get
		{
			return string_15;
		}
		set
		{
			string_15 = value;
		}
	}

	public int RepeatChapter
	{
		get
		{
			return int_2;
		}
		set
		{
			int_2 = value;
		}
	}

	public int RepeatChapterHandle
	{
		get
		{
			return int_13;
		}
		set
		{
			int_13 = value;
		}
	}

	public bool ReplaceChapter
	{
		get
		{
			return bool_26;
		}
		set
		{
			bool_26 = value;
		}
	}

	public int ReplaceChapterNameNun
	{
		get
		{
			return int_26;
		}
		set
		{
			int_26 = value;
		}
	}

	public int ReplaceChapterNun
	{
		get
		{
			return int_16;
		}
		set
		{
			int_16 = value;
		}
	}

	public bool ReplaceFullflag
	{
		get
		{
			return bool_21;
		}
		set
		{
			bool_21 = value;
		}
	}

	public bool ReplaceImgflag
	{
		get
		{
			return bool_20;
		}
		set
		{
			bool_20 = value;
		}
	}

	public bool ReplaceIntro
	{
		get
		{
			return bool_22;
		}
		set
		{
			bool_22 = value;
		}
	}

	public bool ReplaceSort
	{
		get
		{
			return bool_24;
		}
		set
		{
			bool_24 = value;
		}
	}

	public int ReplaceSortId
	{
		get
		{
			return int_15;
		}
		set
		{
			int_15 = value;
		}
	}

	public string RuleFile
	{
		get
		{
			return string_14;
		}
		set
		{
			string_14 = value;
		}
	}

	public bool StrongReplaceFullflag
	{
		get
		{
			return bool_29;
		}
		set
		{
			bool_29 = value;
		}
	}

	public bool StrongReplaceImgflag
	{
		get
		{
			return bool_28;
		}
		set
		{
			bool_28 = value;
		}
	}

	public bool StrongReplaceIntro
	{
		get
		{
			return bool_30;
		}
		set
		{
			bool_30 = value;
		}
	}

	public bool Timing
	{
		get
		{
			return bool_1;
		}
		set
		{
			bool_1 = value;
		}
	}

	public bool Typesetting
	{
		get
		{
			return bool_36;
		}
		set
		{
			bool_36 = value;
		}
	}

	public string UnDownUrl
	{
		get
		{
			return string_12;
		}
		set
		{
			string_12 = value;
		}
	}

	public bool UpdateDefault
	{
		get
		{
			return bool_4;
		}
		set
		{
			bool_4 = value;
		}
	}

	public int vip标示
	{
		get
		{
			return int_vip标示;
		}
		set
		{
			int_vip标示 = value;
		}
	}

	public int 上架标示
	{
		get
		{
			return int_上架标示;
		}
		set
		{
			int_上架标示 = value;
		}
	}

	public bool ReplaceChapterIndex
	{
		get
		{
			return bool_ReplaceChapterIndex;
		}
		set
		{
			bool_ReplaceChapterIndex = value;
		}
	}

	public bool ReplaceChapterTime
	{
		get
		{
			return bool_ReplaceChapterTime;
		}
		set
		{
			bool_ReplaceChapterTime = value;
		}
	}

	public int ReplaceChapterTimeMin { get; set; }

	public int ReplaceChapterTimeMax { get; set; }

	public bool PubContentUrlProxy
	{
		get
		{
			return bool_11;
		}
		set
		{
			bool_11 = value;
		}
	}

	public bool PubIndexUrlProxy
	{
		get
		{
			return thdskpulRH;
		}
		set
		{
			thdskpulRH = value;
		}
	}

	public bool NovelListUrlProxy
	{
		get
		{
			return bool_13;
		}
		set
		{
			bool_13 = value;
		}
	}

	public bool NovelUrlProxy
	{
		get
		{
			return bool_10;
		}
		set
		{
			bool_10 = value;
		}
	}

	public int GoRepeatChapter
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			return 0;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
		}
	}

	public bool DelChapter
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

	public bool ReplaceChapterNameOn
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

	public bool IpStatic { get; set; }

	public int IpStaticNum { get; set; }

	public int IpTimeNum { get; set; }

	public TaskConfigInfo()
	{
		int_0 = 30;
		int_1 = 10000;
		string_0 = "http://www.qidian.com/Book/NewVipBookChapterList.aspx";
		string_1 = "bs\\[\\d*\\] = new Book\\('\\d*','(.+?)','\\d*'";
		string_11 = "gb2312";
		string_12 = "";
	}
}
