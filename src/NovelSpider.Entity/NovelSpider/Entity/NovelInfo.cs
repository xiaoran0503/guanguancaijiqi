using System;
using System.Drawing;

namespace NovelSpider.Entity;

public class NovelInfo
{
	public string Author;

	private ChapterInfo _lastChapter;

	public int Chapters;

	public Image Cover;

	public int Degree;

	public string GetID;

	public int ImgFlag;

	public Uri IndexUrl;

	private int _itemIndex;

	private int _ruleId;

	public string Intro;

	public bool IsErr;

	public bool IsNew;

	public string Keyword;

	public string LagerSort;

	public int LagerSortID;

	public int LastupDate;

	public Image MCover;

	public int MDegree;

	public string MIntro;

	public int MLagerSortID;

	public string MLagerSort;

	public string Name;

	public string[] novelLj;

	public string[] novelTj;

	public Uri NovelUrl;

	public string PinYin;

	public string PinYinSan;

	public int PostDate;

	public string PubKey;

	public int PutID;

	public string ReviseChapter;

	public int ReviseChapterID;

	public int Size;

	public string SmallSort;

	public int SmallSortID;

	public string string_0;

	private string _ruleName;

	public int TopDate;

	public string VipKey;

	public string Articlecode;

	public string Lastsummary;

	public int IsboyID;

	public string Isboy;

	public string Isvip;

	public string Issign;

	public int ItemIndex
	{
		get
		{
			return _itemIndex;
		}
		set
		{
			_itemIndex = value;
		}
	}

	public ChapterInfo LastChapter
	{
		get
		{
			return _lastChapter;
		}
		set
		{
			_lastChapter = value;
		}
	}

	public int RuleID
	{
		get
		{
			return _ruleId;
		}
		set
		{
			_ruleId = value;
		}
	}

	public string RuleName
	{
		get
		{
			return _ruleName;
		}
		set
		{
			_ruleName = value;
		}
	}

	public NovelInfo()
	{
		_lastChapter = new ChapterInfo();
	}
}
