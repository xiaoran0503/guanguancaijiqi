using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NovelSpider.Config;

[Serializable]
public class BaseConfigInfo : IConfigInfo
{
	public bool AddJsRealTxt;

	public string BadWords;

	public string[] BadwordsReplaceImages;

	private bool bool_0;

	public bool bool_10;

	public bool bool_11;

	public bool bool_12;

	public bool bool_13;

	public bool bool_14;

	public bool bool_15;

	private bool bool_9;

	public string CenterText;

	public bool ChapterHtml;

	public string ChapterHtmlDir;

	public string ChapterHtmlUrl;

	public string ChmDir;

	public string ChmUrl;

	public string CoverDir;

	public string CoverUrl;

	public bool CustomCover;

	public bool CustomCreatePath;

	public bool CustomHtmlTemplets;

	public bool CustomImage;

	public bool CustomRealTxt;

	public bool DelFtpImage;

	public string FilterNovelName;

	public bool FullHtml;

	public string FullHtmlDir;

	public string FullHtmlUrl;

	private int gcPvCioq6;

	public string ImageDir;

	public string ImageUrl;

	public bool IndexHtml;

	public string IndexHtmlDir;

	public string IndexHtmlUrl;

	private string InnerTagLinkUrl;

	private int int_00;

	private int int_0;

	private int int_1;

	private int int_4;

	private int int_5;

	private int int_6;

	public string JarDir;

	public string JarUrl;

	public string LicenseAd = ""; // 全量开放

	public bool LicenseOk = true; // 全量开放

	public DateTime LicenseTime = DateTime.MaxValue; // 全量开放

	public bool LicenseVip = true; // 全量开放

	public string NextEndHtmlUrl;

	public string OpfDir;

	public string OpfUrl;

	public string PrevFirstHtmlUrl;

	public string RealTxtDir;

	public string RealTxtUrl;

	public string ReplaceBadWords;

	public string SelectLog;

	public string string_0;

	public string string_1;

	public string string_15;

	public string string_2;

	[CompilerGenerated]
	private string[] string_25;

	private string string_26;

	private string string_27;

	private string string_28;

	private string string_00;

	private string string_29;

	private string string_3;

	private string string_30;

	private string string_31;

	private string string_32;

	private string string_33;

	[CompilerGenerated]
	private string string_34;

	private string string_4;

	private string string_5;

	private string string_6;

	private string string_7;

	private string string_8;

	private string string_9;

	public string[] TextMarkOfArrayText;

	public string TextMarkOfBottom;

	public bool TextMarkOfData;

	public bool TextMarkOfEBook;

	public bool TextMarkOfHtml;

	public int TextMarkOfNumber;

	public string TextMarkOfTop;

	public string[] TextMarkOfVulmeName;

	public string TxtDir;

	public string TxtUrl;

	public string UmdDir;

	public string UmdUrl;

	private string UxOxnqUtFs;

	public string ZipDir;

	public string ZipUrl;

	public bool CreateOPF;

	public bool CreateZIP;

	public bool CreateTXT;

	public bool CreateUMD;

	public bool CreateJAR;

	public bool CreateCHM;

	public string EBookHead;

	public string EBookFoot;

	private bool innerTagLink;

	private bool isEnableBaiduPush;

	private string strBaiduPushDomain;

	private string strBaiduPushToken;

	private string strBaiduPushURL;

	private string strBaiduPushType;

	private int intBaiduPushNum;

	private bool isEnableWapGen;

	private string strWapDomain;

	private string strWapIndexTemplate;

	private string strWapChapterTemplate;

	private string strWapHtmlDir;

	private string _strlist29;

	private string _strcon29;

	private string _strinfo29;

	private string _strindex29;

	public string Indextmp
	{
		get
		{
			return _strindex29;
		}
		set
		{
			_strindex29 = value;
		}
	}

	public string Infotmp
	{
		get
		{
			return _strinfo29;
		}
		set
		{
			_strinfo29 = value;
		}
	}

	public string Listtmp
	{
		get
		{
			return _strlist29;
		}
		set
		{
			_strlist29 = value;
		}
	}

	public string Contmp
	{
		get
		{
			return _strcon29;
		}
		set
		{
			_strcon29 = value;
		}
	}

	public bool ChapterName2Num { get; set; }

	public int ChapterNeighbor { get; set; }

	public int ChapterPagingNum
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

	public int ChapterTuijian { get; set; }

	public string CmsName
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

	public string CmsVersion
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

	public string ConnectionString
	{
		get
		{
			return string_28;
		}
		set
		{
			string_28 = value;
		}
	}

	public string DatabaseServerType { get; set; }

	public string DatabaseServerVersion { get; set; }

	public int DatabaseServerMajorVersion { get; set; }

	public string DatabaseServerComment { get; set; }

	public bool Debug { get; set; }

	public string DefaultLagerSort
	{
		get
		{
			return string_29;
		}
		set
		{
			string_29 = value;
		}
	}

	public int DefaultLagerSortID
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

	public string DefaultSmallSort
	{
		get
		{
			return string_30;
		}
		set
		{
			string_30 = value;
		}
	}

	public int DefaultSmallSortID
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

	public string DefaultVolumeName { get; set; }

	public bool DonotUserDefaultisboy { get; set; }

	public int DefaultisboyID
	{
		get
		{
			return int_00;
		}
		set
		{
			int_00 = value;
		}
	}

	public string Defaultisboy
	{
		get
		{
			return string_00;
		}
		set
		{
			string_00 = value;
		}
	}

	public bool DonotUserDefaultLagerSort { get; set; }

	public bool DonotUserDefaultSmallSort { get; set; }

	public int HttpTimeOut
	{
		get
		{
			return gcPvCioq6;
		}
		set
		{
			gcPvCioq6 = value;
		}
	}

	public string HttpUserAgent
	{
		get
		{
			return UxOxnqUtFs;
		}
		set
		{
			UxOxnqUtFs = value;
		}
	}

	public int IndexAntiCollectNum { get; set; }

	public int IndexNeighbor { get; set; }

	public int IndexTuijian { get; set; }

	public bool InternalLink { get; set; }

	public int InternalLinkDensity
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

	public string InternalLinkFoot { get; set; }

	public string InternalLinkHead { get; set; }

	public string InternalLinkUrl
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

	public string isboyCorresponding { get; set; }

	public string LagerSortCorresponding { get; set; }

	public int LogType { get; set; }

	public string MailPass { get; set; }

	public string MailSmtp { get; set; }

	public int MailTimeNum { get; set; }

	public string MailTitle { get; set; }

	public string MailUser { get; set; }

	public int NewAntiCollectNum { get; set; }

	public string NullChapter
	{
		get
		{
			return string_26;
		}
		set
		{
			string_26 = value;
		}
	}

	public string NumOrPinyin
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

	public string NumOrPinyinDir
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

	public string OnAntiCollectDir
	{
		get
		{
			return string_32;
		}
		set
		{
			string_32 = value;
		}
	}

	public int OnAntiCollectNum { get; set; }

	public bool OpenImageChapter
	{
		get
		{
			return bool_9;
		}
		set
		{
			bool_9 = value;
		}
	}

	public bool OpenNullChapter
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

	public string PrevNextPageSuffix
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

	public string SerialServer
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

	public int SizeNullChapter
	{
		get
		{
			return int_6;
		}
		set
		{
			int_6 = value;
		}
	}

	public string SmallSortCorresponding { get; set; }

	public string sqliteTime
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

	public bool Translate { get; set; }

	public string TuijianTemplates
	{
		get
		{
			return string_34;
		}
		set
		{
			string_34 = value;
		}
	}

	public string TuijianType
	{
		get
		{
			return string_33;
		}
		set
		{
			string_33 = value;
		}
	}

	public string[] UpdateDefaultUrls
	{
		get
		{
			return string_25;
		}
		set
		{
			string_25 = value;
		}
	}

	public string WebSiteCookies { get; set; }

	public string WebSiteName { get; set; }

	public string WebSitePath
	{
		get
		{
			return string_27;
		}
		set
		{
			string_27 = value;
		}
	}

	public bool ZhanQun { get; set; }

	public bool InnerTagLink
	{
		get
		{
			return innerTagLink;
		}
		set
		{
			innerTagLink = value;
		}
	}

	public string InnerTagLinkUrl1
	{
		get
		{
			return InnerTagLinkUrl;
		}
		set
		{
			InnerTagLinkUrl = value;
		}
	}

	public bool IsEnableBaiduPush
	{
		get
		{
			return isEnableBaiduPush;
		}
		set
		{
			isEnableBaiduPush = value;
		}
	}

	public string StrBaiduPushDomain
	{
		get
		{
			return strBaiduPushDomain;
		}
		set
		{
			strBaiduPushDomain = value;
		}
	}

	public string StrBaiduPushToken
	{
		get
		{
			return strBaiduPushToken;
		}
		set
		{
			strBaiduPushToken = value;
		}
	}

	public string StrWapDomain
	{
		get
		{
			return strWapDomain;
		}
		set
		{
			strWapDomain = value;
		}
	}

	public string StrWapIndexTemplate
	{
		get
		{
			return strWapIndexTemplate;
		}
		set
		{
			strWapIndexTemplate = value;
		}
	}

	public string StrWapChapterTemplate
	{
		get
		{
			return strWapChapterTemplate;
		}
		set
		{
			strWapChapterTemplate = value;
		}
	}

	public string StrWapHtmlDir
	{
		get
		{
			return strWapHtmlDir;
		}
		set
		{
			strWapHtmlDir = value;
		}
	}

	public bool IsEnableWapGen
	{
		get
		{
			return isEnableWapGen;
		}
		set
		{
			isEnableWapGen = value;
		}
	}

	public string StrBaiduPushURL
	{
		get
		{
			return strBaiduPushURL;
		}
		set
		{
			strBaiduPushURL = value;
		}
	}

	public int IntBaiduPushNum
	{
		get
		{
			return intBaiduPushNum;
		}
		set
		{
			intBaiduPushNum = value;
		}
	}

	public string StrBaiduPushType
	{
		get
		{
			return strBaiduPushType;
		}
		set
		{
			strBaiduPushType = value;
		}
	}

	public string CmsEncoding { get; set; }

	public BaseConfigInfo()
	{
		int_0 = 1;
		int_00 = 1;
		int_1 = 1;
		gcPvCioq6 = 20;
		int_4 = 1500;
		int_5 = 1000;
		int_6 = 30;
		SelectLog = ",0,21,101,102,120,121,122,124,125,131,132,134,135,136,200,210,214,220,410,420,430,440,441,442,";
		string_2 = "http://vip.datahelper.cn";
		string_3 = "200806";
		string_4 = "jieqi";
		string_5 = "数字ID目录";
		string_6 = "ID除1000/ID";
		string_7 = "后台默认";
		string_8 = "/{NovelId}_{PagingId}.html";
		string_9 = "http://www.xxx.com/{NovelId/1000}/{NovelId}/";
		string_15 = "1";
		string_26 = "{?$jieqi_sitename?}提醒您本章节内容空白,这也许是作者设置的防采集章节或本站获取内容错误,您可以继续尝试阅读下一章或者点击{?postErr(/newmessage.php?tosys=1&title={?$article_title?}-{?$jieqi_chapter?}-错误&content=错误章节:{?$jieqi_chapter?}错误地址:{?$jieqi_url?}/{?$article_id?}/{?$chapter_id?}/错误原因:章节空白)?}快速报错{?/postErr?}提交给管理员处理。";
		string_27 = "E:\\My WebSite\\5857.NET0";
		string_28 = "Server=(local);User id=sa;Pwd=;Database=WanerSoft5857";
		DatabaseServerType = "MySQL";
		DatabaseServerVersion = "";
		DatabaseServerMajorVersion = 0;
		DatabaseServerComment = "";
		string_00 = "男生";
		string_29 = "玄幻";
		string_30 = "玄幻";
		string_31 = "图$|\\[图\\]$|t$|T$|Ｔ$|ｔ$";
		UxOxnqUtFs = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
		LicenseTime = DateTime.Now;
		LicenseAd = "";
		LicenseOk = true;
		LicenseVip = true;
		EnsureDefaults();
	}

	public void EnsureDefaults()
	{
		foreach (FieldInfo fieldInfo in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public))
		{
			if (fieldInfo.FieldType == typeof(string) && fieldInfo.GetValue(this) == null)
			{
				fieldInfo.SetValue(this, string.Empty);
			}
		}
		foreach (PropertyInfo propertyInfo in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
		{
			if (propertyInfo.PropertyType == typeof(string) && propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.GetValue(this) == null)
			{
				propertyInfo.SetValue(this, string.Empty);
			}
		}
		if (string.IsNullOrWhiteSpace(CmsName))
		{
			CmsName = "jieqi";
		}
		if (string.IsNullOrWhiteSpace(CmsVersion))
		{
			CmsVersion = "200806";
		}
		if (string.IsNullOrWhiteSpace(NumOrPinyin))
		{
			NumOrPinyin = "数字ID目录";
		}
		if (string.IsNullOrWhiteSpace(NumOrPinyinDir))
		{
			NumOrPinyinDir = "ID除1000/ID";
		}
		if (string.IsNullOrWhiteSpace(Defaultisboy))
		{
			Defaultisboy = "男生";
		}
		if (string.IsNullOrWhiteSpace(DefaultLagerSort))
		{
			DefaultLagerSort = "玄幻";
		}
		if (string.IsNullOrWhiteSpace(DefaultSmallSort))
		{
			DefaultSmallSort = "玄幻";
		}
		if (string.IsNullOrWhiteSpace(HttpUserAgent))
		{
			HttpUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
		}
		if (string.IsNullOrWhiteSpace(SelectLog))
		{
			SelectLog = ",0,21,101,102,120,121,122,124,125,131,132,134,135,136,200,210,214,220,410,420,430,440,441,442,";
		}
		if (HttpTimeOut <= 0)
		{
			HttpTimeOut = 20;
		}
	}
}
