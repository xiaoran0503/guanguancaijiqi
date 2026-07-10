using System;

namespace NovelSpider.Config;

[Serializable]
public class RuleConfigInfo : IConfigInfo
{
	private RegexInfo ruleVersion;

	private RegexInfo ruleID;

	private RegexInfo novelDegree;

	private RegexInfo novelCover;

	private RegexInfo siteName;

	private RegexInfo novelDefaultCoverUrl;

	private RegexInfo novelInfo_getNovelPubKey;

	private RegexInfo pubCookies;

	private RegexInfo pubIndexUrl;

	private RegexInfo pubIndexErr;

	private RegexInfo pubVolumeContent;

	private RegexInfo pubVolumeSplit;

	private RegexInfo novelUrl;

	private RegexInfo pubVoluemeName;

	private RegexInfo pubChapterName;

	private RegexInfo pubChapter_GetChapterKey;

	private RegexInfo siteCharset;

	private RegexInfo pubContentUrl;

	private RegexInfo pubContentErr;

	private RegexInfo pubContent_GetTextKey;

	private RegexInfo pubTextUrl;

	private RegexInfo pubContentText;

	private RegexInfo pubContentChapterName;

	private RegexInfo novelErr;

	private RegexInfo pubContentImages;

	private RegexInfo pubContentReplace;

	private RegexInfo siteUrl;

	private RegexInfo novelSearchUrl;

	private RegexInfo novelSearchData;

	private RegexInfo novelKey;

	private RegexInfo novelListUrl;

	private RegexInfo novelList_novelKey;

	private RegexInfo novelName;

	private RegexInfo novelAuthor;

	private RegexInfo isboy;

	private RegexInfo lagerSort;

	private RegexInfo smallSort;

	private RegexInfo novelIntro;

	private RegexInfo novelKeyword;

	private RegexInfo pubContentPageArea;

	private RegexInfo pubContentPage;

	private RegexInfo novelListFilter;

	private RegexInfo pubContentChapterNum;

	private RegexInfo pubContentPageKey;

	private RegexInfo pubContentPageUrl;

	public RegexInfo RuleVersion
	{
		get
		{
			return ruleVersion;
		}
		set
		{
			ruleVersion = value;
		}
	}

	public RegexInfo RuleID
	{
		get
		{
			return ruleID;
		}
		set
		{
			ruleID = value;
		}
	}

	public RegexInfo GetSiteName
	{
		get
		{
			return siteName;
		}
		set
		{
			siteName = value;
		}
	}

	public RegexInfo GetSiteCharset
	{
		get
		{
			return siteCharset;
		}
		set
		{
			siteCharset = value;
		}
	}

	public RegexInfo GetSiteUrl
	{
		get
		{
			return siteUrl;
		}
		set
		{
			siteUrl = value;
		}
	}

	public RegexInfo NovelSearchUrl
	{
		get
		{
			return novelSearchUrl;
		}
		set
		{
			novelSearchUrl = value;
		}
	}

	public RegexInfo NovelSearchData
	{
		get
		{
			return novelSearchData;
		}
		set
		{
			novelSearchData = value;
		}
	}

	public RegexInfo NovelSearch_GetNovelKey
	{
		get
		{
			return novelKey;
		}
		set
		{
			novelKey = value;
		}
	}

	public RegexInfo NovelListUrl
	{
		get
		{
			return novelListUrl;
		}
		set
		{
			novelListUrl = value;
		}
	}

	public RegexInfo NovelListFilter
	{
		get
		{
			return novelListFilter;
		}
		set
		{
			novelListFilter = value;
		}
	}

	public RegexInfo NovelList_GetNovelKey
	{
		get
		{
			return novelList_novelKey;
		}
		set
		{
			novelList_novelKey = value;
		}
	}

	public RegexInfo NovelUrl
	{
		get
		{
			return novelUrl;
		}
		set
		{
			novelUrl = value;
		}
	}

	public RegexInfo NovelName
	{
		get
		{
			return novelName;
		}
		set
		{
			novelName = value;
		}
	}

	public RegexInfo NovelErr
	{
		get
		{
			return novelErr;
		}
		set
		{
			novelErr = value;
		}
	}

	public RegexInfo NovelAuthor
	{
		get
		{
			return novelAuthor;
		}
		set
		{
			novelAuthor = value;
		}
	}

	public RegexInfo Isboy
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

	public RegexInfo LagerSort
	{
		get
		{
			return lagerSort;
		}
		set
		{
			lagerSort = value;
		}
	}

	public RegexInfo SmallSort
	{
		get
		{
			return smallSort;
		}
		set
		{
			smallSort = value;
		}
	}

	public RegexInfo NovelIntro
	{
		get
		{
			return novelIntro;
		}
		set
		{
			novelIntro = value;
		}
	}

	public RegexInfo NovelKeyword
	{
		get
		{
			return novelKeyword;
		}
		set
		{
			novelKeyword = value;
		}
	}

	public RegexInfo NovelDegree
	{
		get
		{
			return novelDegree;
		}
		set
		{
			novelDegree = value;
		}
	}

	public RegexInfo NovelCover
	{
		get
		{
			return novelCover;
		}
		set
		{
			novelCover = value;
		}
	}

	public RegexInfo NovelDefaultCoverUrl
	{
		get
		{
			return novelDefaultCoverUrl;
		}
		set
		{
			novelDefaultCoverUrl = value;
		}
	}

	public RegexInfo NovelInfo_GetNovelPubKey
	{
		get
		{
			return novelInfo_getNovelPubKey;
		}
		set
		{
			novelInfo_getNovelPubKey = value;
		}
	}

	public RegexInfo PubCookies
	{
		get
		{
			return pubCookies;
		}
		set
		{
			pubCookies = value;
		}
	}

	public RegexInfo PubIndexUrl
	{
		get
		{
			return pubIndexUrl;
		}
		set
		{
			pubIndexUrl = value;
		}
	}

	public RegexInfo PubIndexErr
	{
		get
		{
			return pubIndexErr;
		}
		set
		{
			pubIndexErr = value;
		}
	}

	public RegexInfo PubVolumeContent
	{
		get
		{
			return pubVolumeContent;
		}
		set
		{
			pubVolumeContent = value;
		}
	}

	public RegexInfo PubVolumeSplit
	{
		get
		{
			return pubVolumeSplit;
		}
		set
		{
			pubVolumeSplit = value;
		}
	}

	public RegexInfo PubVolumeName
	{
		get
		{
			return pubVoluemeName;
		}
		set
		{
			pubVoluemeName = value;
		}
	}

	public RegexInfo PubChapterName
	{
		get
		{
			return pubChapterName;
		}
		set
		{
			pubChapterName = value;
		}
	}

	public RegexInfo PubChapter_GetChapterKey
	{
		get
		{
			return pubChapter_GetChapterKey;
		}
		set
		{
			pubChapter_GetChapterKey = value;
		}
	}

	public RegexInfo PubContentUrl
	{
		get
		{
			return pubContentUrl;
		}
		set
		{
			pubContentUrl = value;
		}
	}

	public RegexInfo PubContentErr
	{
		get
		{
			return pubContentErr;
		}
		set
		{
			pubContentErr = value;
		}
	}

	public RegexInfo PubContent_GetTextKey
	{
		get
		{
			return pubContent_GetTextKey;
		}
		set
		{
			pubContent_GetTextKey = value;
		}
	}

	public RegexInfo PubTextUrl
	{
		get
		{
			return pubTextUrl;
		}
		set
		{
			pubTextUrl = value;
		}
	}

	public RegexInfo PubContentText
	{
		get
		{
			return pubContentText;
		}
		set
		{
			pubContentText = value;
		}
	}

	public RegexInfo PubContentPageArea
	{
		get
		{
			return pubContentPageArea;
		}
		set
		{
			pubContentPageArea = value;
		}
	}

	public RegexInfo PubContentPage
	{
		get
		{
			return pubContentPage;
		}
		set
		{
			pubContentPage = value;
		}
	}

	public RegexInfo PubContentPageUrl
	{
		get
		{
			return pubContentPageUrl;
		}
		set
		{
			pubContentPageUrl = value;
		}
	}

	public RegexInfo PubContentPageKey
	{
		get
		{
			return pubContentPageKey;
		}
		set
		{
			pubContentPageKey = value;
		}
	}

	public RegexInfo PubContentChapterName
	{
		get
		{
			return pubContentChapterName;
		}
		set
		{
			pubContentChapterName = value;
		}
	}

	public RegexInfo PubContentChapterNum
	{
		get
		{
			return pubContentChapterNum;
		}
		set
		{
			pubContentChapterNum = value;
		}
	}

	public RegexInfo PubContentImages
	{
		get
		{
			return pubContentImages;
		}
		set
		{
			pubContentImages = value;
		}
	}

	public RegexInfo PubContentReplace
	{
		get
		{
			return pubContentReplace;
		}
		set
		{
			pubContentReplace = value;
		}
	}

	public RuleConfigInfo()
	{
		ruleVersion = new RegexInfo();
		ruleID = new RegexInfo();
		novelUrl = new RegexInfo();
		novelErr = new RegexInfo();
		novelName = new RegexInfo();
		novelAuthor = new RegexInfo();
		lagerSort = new RegexInfo();
		smallSort = new RegexInfo();
		novelIntro = new RegexInfo();
		novelKeyword = new RegexInfo();
		novelDegree = new RegexInfo();
		novelCover = new RegexInfo();
		siteName = new RegexInfo();
		novelDefaultCoverUrl = new RegexInfo();
		novelInfo_getNovelPubKey = new RegexInfo();
		pubCookies = new RegexInfo();
		pubIndexUrl = new RegexInfo();
		pubIndexErr = new RegexInfo();
		pubVolumeContent = new RegexInfo();
		pubVolumeSplit = new RegexInfo();
		pubVoluemeName = new RegexInfo();
		pubChapterName = new RegexInfo();
		pubChapter_GetChapterKey = new RegexInfo();
		siteCharset = new RegexInfo();
		pubContentUrl = new RegexInfo();
		pubContentErr = new RegexInfo();
		pubContent_GetTextKey = new RegexInfo();
		pubTextUrl = new RegexInfo();
		pubContentText = new RegexInfo();
		pubContentChapterName = new RegexInfo();
		pubContentChapterNum = new RegexInfo();
		pubContentImages = new RegexInfo();
		pubContentReplace = new RegexInfo();
		siteUrl = new RegexInfo();
		novelSearchUrl = new RegexInfo();
		novelSearchData = new RegexInfo();
		novelKey = new RegexInfo();
		novelListUrl = new RegexInfo();
		novelListFilter = new RegexInfo();
		novelList_novelKey = new RegexInfo();
		pubContentPageArea = new RegexInfo();
		pubContentPage = new RegexInfo();
		isboy = new RegexInfo();
		pubContentPageKey = new RegexInfo();
		pubContentPageUrl = new RegexInfo();
	}
}
