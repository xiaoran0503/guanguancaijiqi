using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using NovelSpider.Common;
using NovelSpider.Config;

namespace NovelSpider.Local.Jieqi;

public static class Config
{
	public static string AttachDir;

	public static string AttachUrl;

	public static string ConnectionString;

	public static string FullDir;

	public static string FullUrl;

	public static string HtmlDir;

	public static string HtmlUrl;

	public static string ImageDir;

	public static string ImageUrl;

	public static string JarDir;

	public static string JarUrl;

	public static Hashtable JieqiArticleConfigs;

	public static string JieqiAuthor;

	public static string JieqiCharset;

	public static Hashtable JieqiDefine;

	public static string[] JieqiSort;

	public static string[] JieqiSortcode;

	public static string[][] JieqiSorttypes;

	public static string[] Isboy;

	public static string[] IsboyID;

	public static string OpfDir;

	public static string OpfUrl;

	public static string RssDir;

	public static string RssUrl;

	public static string Templets_fullbottom;

	public static string Templets_fullchapter;

	public static Hashtable Templets_fullchapterlist;

	public static string Templets_fullchapterloop;

	public static string Templets_fullhtml;

	public static string Templets_fulltop;

	public static string Templets_fullvolume;

	public static string Templets_indexbottom;

	public static string Templets_indexchapter;

	public static Hashtable Templets_indexchapterlist;

	public static string Templets_indexhtml;

	public static string Templets_indextop;

	public static string Templets_indexvolume;

	public static string TempletsContent;

	public static string TempletsFullHtml;

	public static string TempletsIndexHtml;

	public static string TxtDir;

	public static string TxtFullDir;

	public static string TxtFullUrl;

	public static string TxtUrl;

	public static string UmdDir;

	public static string UmdUrl;

	public static string WebSitePath;

	public static string ZipDir;

	public static string ZipUrl;

	static Config()
	{
		JieqiArticleConfigs = new Hashtable();
		JieqiDefine = new Hashtable();
		Templets_fullchapterlist = new Hashtable();
		Templets_indexchapterlist = new Hashtable();
	}

	public static void LoadConfig()
	{
		WebSitePath = Configs.BaseConfig.WebSitePath;
		ConnectionString = DatabaseCompatibilityProfile.NormalizeConnectionString(Configs.BaseConfig.ConnectionString, Configs.BaseConfig.DatabaseServerType, Configs.BaseConfig.DatabaseServerMajorVersion);
		LoadJieqiConfig();
		if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
		{
			LoadJieqiSkins();
		}
		else
		{
			LoadJieqiSkinsOld();
		}
	}

	public static void LoadJieqiConfig()
	{
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig JieqiDefine");
		JieqiDefine = new Hashtable();
		MatchCollection matchCollection = new Regex("define\\('(.+?)','(.*)'\\);").Matches(File.ReadAllText(WebSitePath + "/configs/define.php", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")).Replace("\n", "").Replace("\r", "")
			.Replace("define", "\ndefine"));
		for (int i = 0; i < matchCollection.Count; i++)
		{
			JieqiDefine[matchCollection[i].Groups[1].Value] = matchCollection[i].Groups[2].Value;
		}
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig JieqiSort");
		string text = File.ReadAllText(WebSitePath + "/configs/article/sort.php", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		MatchCollection matchCollection2;
		if (Configs.BaseConfig.CmsVersion == "1.8")
		{
			matchCollection2 = new Regex("jieqiSort\\['article'\\]\\[(\\d*)\\]\\s*.+?'code' => '(.+?)',\\s*.+?caption' \\=> '(.+?)'").Matches(text);
		}
		else if (double.Parse(Configs.BaseConfig.CmsVersion) >= 2.1)
		{
			matchCollection2 = new Regex("jieqiSort\\['article'\\]\\[(\\d*)\\] = array\\('code' => '(.+?)',\\s*'caption' => '(.+?)'").Matches(text);
			if (text.IndexOf("types") > 0)
			{
				matchCollection2 = new Regex("jieqiSort\\['article'\\]\\[(\\d*)\\] = array\\('code' => '(.+?)', 'caption' => '(.+?)','group' => '(\\d*)','types' => array\\((.+?)\\)\\)").Matches(text);
			}
		}
		else
		{
			matchCollection2 = new Regex("jieqiSort\\['article'\\]\\[(\\d*)\\]\\s*.+?caption' \\=> '(.+?)'").Matches(text);
		}
		JieqiSort = new string[matchCollection2.Count + 1];
		JieqiSortcode = new string[matchCollection2.Count + 1];
		JieqiSort[0] = "未知分类";
		JieqiSortcode[0] = "Unknown";
		Isboy = new string[matchCollection2.Count + 1];
		IsboyID = new string[matchCollection2.Count + 1];
		Isboy[0] = "未知频道";
		IsboyID[0] = "";
		JieqiSorttypes = new string[matchCollection2.Count + 1][];
		for (int j = 0; j < JieqiSorttypes.Length; j++)
		{
			JieqiSorttypes[j] = new string[100];
		}
		JieqiSorttypes[0][0] = "";
		if (double.Parse(Configs.BaseConfig.CmsVersion) >= 2.1)
		{
			for (int k = 0; k < matchCollection2.Count; k++)
			{
				int num = Convert.ToInt32(matchCollection2[k].Groups[1].Value);
				JieqiSort[num] = matchCollection2[k].Groups[3].Value;
				JieqiSortcode[num] = matchCollection2[k].Groups[2].Value;
				Isboy[num] = matchCollection2[k].Groups[4].Value;
				if (matchCollection2[k].Groups.Count > 4)
				{
					string value = matchCollection2[k].Groups[4].Value;
					MatchCollection matchCollection3 = new Regex("'(\\d*)' =>'(.+?)'").Matches(value);
					if (matchCollection3.Count > 0)
					{
						for (int l = 0; l < matchCollection3.Count; l++)
						{
							int num2 = Convert.ToInt32(matchCollection3[l].Groups[1].Value);
							JieqiSorttypes[num][num2] = matchCollection3[l].Groups[2].Value;
						}
					}
					else
					{
						JieqiSorttypes[num][0] = "";
					}
				}
				else
				{
					JieqiSorttypes[num][0] = "";
				}
			}
		}
		else if (Configs.BaseConfig.CmsVersion == "1.8")
		{
			matchCollection2 = new Regex("jieqiSort\\['article'\\]\\[(\\d*)\\]\\s*.+?'code'\\s*=>\\s*'(.+?)',\\s*.+?caption' \\=> '(.+?)'").Matches(File.ReadAllText(WebSitePath + "/configs/article/sort.php", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")));
			for (int m = 0; m < matchCollection2.Count; m++)
			{
				int num3 = Convert.ToInt32(matchCollection2[m].Groups[1].Value);
				JieqiSortcode[num3] = matchCollection2[m].Groups[2].Value;
				JieqiSort[num3] = matchCollection2[m].Groups[3].Value;
			}
		}
		else
		{
			for (int n = 0; n < matchCollection2.Count; n++)
			{
				int num4 = Convert.ToInt32(matchCollection2[n].Groups[1].Value);
				JieqiSort[num4] = matchCollection2[n].Groups[2].Value;
				JieqiSortcode[num4] = matchCollection2[n].Groups[3].Value;
			}
		}
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig JieqiArticleConfigs");
		JieqiArticleConfigs = new Hashtable();
		MatchCollection matchCollection4 = new Regex("jieqiConfigs\\['article'\\]\\['(.+?)'\\] \\= '(.*)';").Matches(File.ReadAllText(WebSitePath + "/configs/article/configs.php", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")).Replace("\n", "").Replace("\r", "")
			.Replace("$jieqiConfigs", "\n$jieqiConfigs"));
		if (matchCollection4.Count == 0)
		{
			matchCollection4 = new Regex("'([^']*?)'\\s*\\=>\\s*'([^']*)',").Matches(File.ReadAllText(WebSitePath + "/configs/article/configs.php", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")).Replace("> ", "> '").Replace(",\n", "',\n")
				.Replace("''", "'")
				.Replace("\n", "")
				.Replace("\r", ""));
		}
		for (int num5 = 0; num5 < matchCollection4.Count; num5++)
		{
			JieqiArticleConfigs[matchCollection4[num5].Groups[1].Value] = matchCollection4[num5].Groups[2].Value;
		}
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig HtmlDir");
		HtmlDir = ((JieqiArticleConfigs["htmldir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["htmldir"].ToString()) : JieqiArticleConfigs["htmldir"].ToString());
		HtmlUrl = ((!(JieqiArticleConfigs["htmlurl"].ToString() != "")) ? ((string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["htmldir"].ToString()) : JieqiArticleConfigs["htmlurl"].ToString());
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig TxtDir");
		TxtDir = ((JieqiArticleConfigs["txtdir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["txtdir"].ToString()) : JieqiArticleConfigs["txtdir"].ToString());
		TxtUrl = ((!(JieqiArticleConfigs["txturl"].ToString() != "")) ? ((string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["txtdir"].ToString()) : JieqiArticleConfigs["txturl"].ToString());
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig OpfDir");
		OpfDir = ((JieqiArticleConfigs["opfdir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["opfdir"].ToString()) : JieqiArticleConfigs["opfdir"].ToString());
		OpfUrl = ((!(JieqiArticleConfigs["opfurl"].ToString() != "")) ? ((string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["opfdir"].ToString()) : JieqiArticleConfigs["opfurl"].ToString());
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig FullDir");
		FullDir = ((JieqiArticleConfigs["fulldir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["fulldir"].ToString()) : JieqiArticleConfigs["fulldir"].ToString());
		FullUrl = ((!(JieqiArticleConfigs["fullurl"].ToString() != "")) ? ((string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["fulldir"].ToString()) : JieqiArticleConfigs["fullurl"].ToString());
		if (double.Parse(Configs.BaseConfig.CmsVersion) < 2.1)
		{
			SpiderException.Debug("Jieqi.Config.LoadWanerConfig ZipDir");
			ZipDir = ((JieqiArticleConfigs["zipdir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["zipdir"].ToString()) : JieqiArticleConfigs["zipdir"].ToString());
			ZipUrl = ((!(JieqiArticleConfigs["zipurl"].ToString() != "")) ? ((string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["zipdir"].ToString()) : JieqiArticleConfigs["zipurl"].ToString());
			SpiderException.Debug("Jieqi.Config.LoadWanerConfig UmdDir");
			try
			{
				UmdDir = ((JieqiArticleConfigs["umddir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["umddir"].ToString()) : JieqiArticleConfigs["umddir"].ToString());
			}
			catch
			{
				UmdDir = WebSitePath + "/files/article/umd";
			}
			try
			{
				UmdUrl = ((!(JieqiArticleConfigs["umdurl"].ToString() != "")) ? ((string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["umddir"].ToString()) : JieqiArticleConfigs["umdurl"].ToString());
			}
			catch
			{
				UmdUrl = (string)JieqiDefine["JIEQI_URL"] + "/files/article/umd";
			}
			SpiderException.Debug("Jieqi.Config.LoadWanerConfig JarDir");
			try
			{
				JarDir = ((JieqiArticleConfigs["jardir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["jardir"].ToString()) : JieqiArticleConfigs["jardir"].ToString());
			}
			catch
			{
				JarDir = WebSitePath + "/files/article/jar";
			}
			try
			{
				JarUrl = ((!(JieqiArticleConfigs["jarurl"].ToString() != "")) ? ((string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["jardir"].ToString()) : JieqiArticleConfigs["jarurl"].ToString());
			}
			catch
			{
				JarUrl = (string)JieqiDefine["JIEQI_URL"] + "/files/article/jar";
			}
		}
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig TxtFullDir");
		try
		{
			TxtFullDir = ((JieqiArticleConfigs["txtfulldir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["txtfulldir"].ToString()) : JieqiArticleConfigs["txtfulldir"].ToString());
		}
		catch
		{
			TxtFullDir = WebSitePath + "/files/article/txtfull";
		}
		try
		{
			TxtFullUrl = ((!(JieqiArticleConfigs["txtfullurl"].ToString() != "")) ? ((string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["txtfulldir"].ToString()) : JieqiArticleConfigs["txtfullurl"].ToString());
		}
		catch
		{
			TxtFullUrl = (string)JieqiDefine["JIEQI_URL"] + "/files/article/txtfull";
		}
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig ImageDir");
		ImageDir = ((JieqiArticleConfigs["imagedir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["imagedir"].ToString()) : JieqiArticleConfigs["imagedir"].ToString());
		ImageUrl = ((!(JieqiArticleConfigs["imageurl"].ToString() != "")) ? ((string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["imagedir"].ToString()) : JieqiArticleConfigs["imageurl"].ToString());
		SpiderException.Debug("Jieqi.Config.LoadWanerConfig AttachDir");
		AttachDir = ((JieqiArticleConfigs["attachdir"].ToString().IndexOf(":") <= 0) ? (WebSitePath + "/files/article/" + JieqiArticleConfigs["attachdir"].ToString()) : JieqiArticleConfigs["attachdir"].ToString());
		if (JieqiArticleConfigs["attachurl"].ToString() != "")
		{
			AttachUrl = JieqiArticleConfigs["attachurl"].ToString();
		}
		else
		{
			AttachUrl = (string)JieqiDefine["JIEQI_URL"] + "/files/article/" + JieqiArticleConfigs["attachdir"].ToString();
		}
		JieqiCharset = (string)JieqiDefine["JIEQI_DB_CHARSET"];
	}

	public static void LoadJieqiSkins()
	{
		SpiderException.Debug("Jieqi.Config.LoadJieqiSkins");
		if (Configs.BaseConfig.Listtmp != "")
		{
			TempletsIndexHtml = File.ReadAllText(Configs.BaseConfig.Listtmp, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		}
		else
		{
			TempletsIndexHtml = File.ReadAllText(WebSitePath + "/modules/article/templates/index.html", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		}
		if (Configs.BaseConfig.Contmp != "")
		{
			TempletsContent = File.ReadAllText(Configs.BaseConfig.Contmp, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		}
		else
		{
			TempletsContent = File.ReadAllText(WebSitePath + "/modules/article/templates/style.html", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		}
		TempletsFullHtml = File.ReadAllText(WebSitePath + "/modules/article/templates/fulltext.html", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		TempletsIndexHtml = TempletsIndexHtml.Replace("?}{?else?}", "?}&nbsp;{?else?}");
		SpiderException.Debug("Jieqi.Config.LoadJieqiSkins 分析目录页模板");
		Templets_indexchapterlist.Clear();
		MatchCollection matchCollection = Regex.Matches(TempletsIndexHtml, "\\{\\?if\\s+\\$i\\[\\'order\\'\\]\\s*([\\>\\<\\=]*)\\s*(.+?)\\?\\}(.+?)\\{\\?/if\\?\\}", RegexOptions.Singleline);
		for (int i = 0; i < matchCollection.Count; i++)
		{
			string[] value = new string[2]
			{
				matchCollection[i].Groups[2].Value.Replace("&nbsp;", " "),
				matchCollection[i].Groups[3].Value.Replace("&nbsp;", " ")
			};
			Templets_indexchapterlist.Add(matchCollection[i].Groups[1].Value + matchCollection[i].Groups[2].Value.Replace("&nbsp;", " "), value);
		}
		TempletsIndexHtml = Regex.Replace(TempletsIndexHtml, "\\{\\?if\\s+\\$chapterrows\\[i\\]\\.isvip\\s*==\\s*0\\?\\}.+?\\{\\?\\/if\\?\\}", "", RegexOptions.Singleline);
		TempletsIndexHtml = Regex.Replace(TempletsIndexHtml, "\\{\\?if\\s*\\$chapterrows\\[i\\]\\.display\\s*!=\\s*0\\s*\\?\\}\\s*class=\"gray\"\\{\\?\\/if\\?\\}", "", RegexOptions.Singleline);
		TempletsIndexHtml = Regex.Replace(TempletsIndexHtml, "\\{\\?if\\s*\\$chapterrows\\[i\\]\\.isvip\\s*>\\s*0\\?\\}\\s*.+?\\s*\\{\\?else\\?\\}\\s*(.+?)\\s*\\{\\?\\/if\\?\\}", "$1", RegexOptions.Singleline);
		Templets_indexhtml = Regex.Replace(TempletsIndexHtml, "\\{\\?if\\s+\\$i\\[\\'order\\'\\]\\s*([\\>\\<\\=]*)\\s*(.+?)\\?\\}(.+?)\\{\\?/if\\?\\}", "判断", RegexOptions.Singleline);
		Match match = Regex.Match(Templets_indexhtml, "\\{\\?section name=i loop=\\$chapterrows?\\?\\}(.*)\\{\\?if \\$chapterrows?\\[i\\]\\.chaptertype > 0\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.*)\\{\\?/section\\?\\}", RegexOptions.Singleline);
		if (!match.Success)
		{
			throw new ApplicationException("分析目录页模板发生错误");
		}
		Templets_indextop = match.Groups[1].Value;
		Templets_indexvolume = match.Groups[2].Value;
		Templets_indexchapter = match.Groups[3].Value;
		Templets_indexbottom = match.Groups[4].Value;
		Templets_indexhtml = Regex.Replace(Templets_indexhtml, "\\{\\?section name=i loop=\\$chapterrows?\\?\\}(.*)\\{\\?if \\$chapterrows?\\[i\\]\\.chaptertype > 0\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.*)\\{\\?/section\\?\\}", "目录循环部分", RegexOptions.Singleline);
		SpiderException.Debug("Jieqi.Config.LoadJieqiSkins 分析全文页模板");
		match = Regex.Match(TempletsFullHtml, "\\{\\?section name=i loop=\\$chapterrows\\?\\}(.+?)\\{\\?/section\\?\\}", RegexOptions.Singleline);
		if (!match.Success)
		{
			throw new ApplicationException("分析全文阅读页模板发生错误1");
		}
		Templets_fullchapterloop = match.Groups[1].Value;
		Templets_fullhtml = Regex.Replace(TempletsFullHtml, "\\{\\?section name=i loop=\\$chapterrows\\?\\}(.+?)\\{\\?/section\\?\\}", "章节循环部分", RegexOptions.Singleline);
		Templets_fullchapterlist.Clear();
		matchCollection = Regex.Matches(Templets_fullhtml, "\\{\\?if\\s+\\$chapterrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", RegexOptions.Singleline);
		for (int j = 0; j < matchCollection.Count; j++)
		{
			string[] value2 = new string[2]
			{
				matchCollection[j].Groups[2].Value,
				matchCollection[j].Groups[3].Value
			};
			Templets_fullchapterlist.Add(matchCollection[j].Groups[1].Value, value2);
		}
		Templets_fullhtml = Regex.Replace(Templets_fullhtml, "\\{\\?if\\s+\\$chapterrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", "章节$1", RegexOptions.Singleline);
		match = Regex.Match(Templets_fullhtml, "\\{\\?section name=i loop=\\$chapters\\?\\}(.+?)\\{\\?/section\\?\\}", RegexOptions.Singleline);
		if (!match.Success)
		{
			throw new ApplicationException("分析全文阅读页模板发生错误2");
		}
		Templets_fulltop = match.Groups[1].Value;
		Templets_fullvolume = match.Groups[2].Value;
		Templets_fullchapter = match.Groups[3].Value;
		Templets_fullbottom = match.Groups[4].Value;
		Templets_fullhtml = Regex.Replace(Templets_fullhtml, "\\{\\?section name=i loop=\\$chapterrows?\\?\\}(.*)\\{\\?if \\$chapterrows?\\[i\\]\\.chaptertype > \"0\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.*)\\{\\?/section\\?\\}", "目录循环部分", RegexOptions.Singleline);
	}

	public static void LoadJieqiSkinsOld()
	{
		SpiderException.Debug("Jieqi.Config.LoadWanerSkins");
		TempletsIndexHtml = File.ReadAllText(WebSitePath + "/modules/article/templates/index.html", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		TempletsContent = File.ReadAllText(WebSitePath + "/modules/article/templates/style.html", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		TempletsFullHtml = File.ReadAllText(WebSitePath + "/modules/article/templates/fulltext.html", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		TempletsIndexHtml = TempletsIndexHtml.Replace("?}{?else?}", "?}&nbsp;{?else?}");
		SpiderException.Debug("Jieqi.Config.LoadWanerSkins 分析目录页模板");
		Templets_indexchapterlist.Clear();
		MatchCollection matchCollection = Regex.Matches(TempletsIndexHtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", RegexOptions.Singleline);
		for (int i = 0; i < matchCollection.Count; i++)
		{
			string[] value = new string[2]
			{
				matchCollection[i].Groups[2].Value.Replace("&nbsp;", " "),
				matchCollection[i].Groups[3].Value.Replace("&nbsp;", " ")
			};
			Templets_indexchapterlist.Add(matchCollection[i].Groups[1].Value, value);
		}
		Templets_indexhtml = Regex.Replace(TempletsIndexHtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", "章节$1", RegexOptions.Singleline);
		Match match = Regex.Match(Templets_indexhtml, "\\{\\?section name=i loop=\\$indexrows\\?\\}(.*)\\{\\?if \\$indexrows\\[i\\]\\.ctype == \"volume\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.*)\\{\\?/section\\?\\}", RegexOptions.Singleline);
		if (!match.Success)
		{
			throw new ApplicationException("分析目录页模板发生错误");
		}
		Templets_indextop = match.Groups[1].Value;
		Templets_indexvolume = match.Groups[2].Value;
		Templets_indexchapter = match.Groups[3].Value;
		Templets_indexbottom = match.Groups[4].Value;
		Templets_indexhtml = Regex.Replace(Templets_indexhtml, "\\{\\?section name=i loop=\\$indexrows\\?\\}(.*)\\{\\?if \\$indexrows\\[i\\]\\.ctype == \"volume\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.*)\\{\\?/section\\?\\}", "目录循环部分", RegexOptions.Singleline);
		Templets_indexhtml = Regex.Replace(Templets_indexhtml, "\\{\\?section name=i loop=\\$lastrows\\?\\}(.*)\\{\\?/section\\?\\}", "最新章节循环部分", RegexOptions.Singleline);
		SpiderException.Debug("Jieqi.Config.LoadWanerSkins 分析全文页模板");
		match = Regex.Match(TempletsFullHtml, "\\{\\?section name=i loop=\\$chapters\\?\\}(.+?)\\{\\?/section\\?\\}", RegexOptions.Singleline);
		if (!match.Success)
		{
			throw new ApplicationException("分析全文阅读页模板发生错误1");
		}
		Templets_fullchapterloop = match.Groups[1].Value;
		Templets_fullhtml = Regex.Replace(TempletsFullHtml, "\\{\\?section name=i loop=\\$chapters\\?\\}(.+?)\\{\\?/section\\?\\}", "章节循环部分", RegexOptions.Singleline);
		Templets_fullchapterlist.Clear();
		matchCollection = Regex.Matches(Templets_fullhtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", RegexOptions.Singleline);
		for (int j = 0; j < matchCollection.Count; j++)
		{
			string[] value2 = new string[2]
			{
				matchCollection[j].Groups[2].Value,
				matchCollection[j].Groups[3].Value
			};
			Templets_fullchapterlist.Add(matchCollection[j].Groups[1].Value, value2);
		}
		Templets_fullhtml = Regex.Replace(Templets_fullhtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", "章节$1", RegexOptions.Singleline);
		match = Regex.Match(Templets_fullhtml, "\\{\\?section name=i loop=\\$indexrows\\?\\}(.+?)\\{\\?if \\$indexrows\\[i\\]\\.ctype == \"volume\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.+?)\\{\\?/section\\?\\}", RegexOptions.Singleline);
		if (!match.Success)
		{
			throw new ApplicationException("分析全文阅读页模板发生错误2");
		}
		Templets_fulltop = match.Groups[1].Value;
		Templets_fullvolume = match.Groups[2].Value;
		Templets_fullchapter = match.Groups[3].Value;
		Templets_fullbottom = match.Groups[4].Value;
		Templets_fullhtml = Regex.Replace(Templets_fullhtml, "\\{\\?section name=i loop=\\$indexrows\\?\\}(.+?)\\{\\?if \\$indexrows\\[i\\]\\.ctype == \"volume\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.+?)\\{\\?/section\\?\\}", "目录循环部分", RegexOptions.Singleline);
	}

	public static void LoadWanerSkins()
	{
		SpiderException.Debug("Jieqi.Config.LoadWanerSkins");
		TempletsIndexHtml = File.ReadAllText(WebSitePath + "/modules/article/templates/index.html", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		TempletsContent = File.ReadAllText(WebSitePath + "/modules/article/templates/style.html", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		TempletsFullHtml = File.ReadAllText(WebSitePath + "/modules/article/templates/fulltext.html", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		TempletsIndexHtml = TempletsIndexHtml.Replace("?}{?else?}", "?}&nbsp;{?else?}");
		SpiderException.Debug("Jieqi.Config.LoadWanerSkins 分析目录页模板");
		Templets_indexchapterlist.Clear();
		if (Configs.BaseConfig.CmsVersion != "1.8")
		{
			MatchCollection matchCollection = Regex.Matches(TempletsIndexHtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", RegexOptions.Singleline);
			for (int i = 0; i < matchCollection.Count; i++)
			{
				string[] value = new string[2]
				{
					matchCollection[i].Groups[2].Value.Replace("&nbsp;", " "),
					matchCollection[i].Groups[3].Value.Replace("&nbsp;", " ")
				};
				Templets_indexchapterlist.Add(matchCollection[i].Groups[1].Value, value);
			}
			Templets_indexhtml = Regex.Replace(TempletsIndexHtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", "章节$1", RegexOptions.Singleline);
		}
		else
		{
			MatchCollection matchCollection = Regex.Matches(TempletsIndexHtml, "\\{\\?if \\$i\\['order'\\] > 1\\?\\}[\\S\\s]*\\{\\?/if\\?\\}([\\S\\s]*)(\\{\\?\\$chapterrows\\[i\\]\\.chaptername\\?\\})([\\S\\s]*)\\{\\?if \\$i\\['order'\\] < \\$i\\['count'\\]\\?\\}", RegexOptions.Singleline);
			for (int j = 0; j < matchCollection.Count; j++)
			{
				string[] value2 = new string[2]
				{
					"",
					matchCollection[j].Groups[2].Value.Replace("&nbsp;", " ")
				};
				Templets_indexchapterlist.Add("vname", value2);
				Templets_indexhtml = Regex.Replace(TempletsIndexHtml, "\\{\\?if \\$i\\['order'\\] > 1\\?\\}[\\S\\s]*\\{\\?/if\\?\\}([\\S\\s]*)(\\{\\?\\$chapterrows\\[i\\]\\.chaptername\\?\\})([\\S\\s]*)\\{\\?if \\$i\\['order'\\] < \\$i\\['count'\\]\\?\\}[\\S\\s]*\\{\\?/if\\?\\}[\\S\\s]*\\{\\?else\\?\\}", "$1章节vname$3", RegexOptions.Singleline);
			}
			matchCollection = Regex.Matches(TempletsIndexHtml, "\\{\\?if \\$i\\['order'\\] == 1\\?\\}([\\S\\s]*)\\{\\?/if\\?\\}([\\S\\s]*)\\{\\?if \\$i\\['order'\\] == \\$i\\['count'\\]\\?\\}([\\S\\s]*)\\{\\?/if\\?\\}\\s*\\{\\?/if\\?\\}", RegexOptions.Singleline);
			for (int k = 0; k < matchCollection.Count; k++)
			{
				string[] value3 = new string[2]
				{
					"",
					matchCollection[k].Groups[2].Value.Replace("&nbsp;", " ")
				};
				Templets_indexchapterlist.Add("cname1", value3);
				Templets_indexhtml = Regex.Replace(Templets_indexhtml, "(\\{\\?if \\$i\\['order'\\] == 1\\?\\}[\\S\\s]*\\{\\?/if\\?\\})([\\S\\s]*)(\\{\\?if \\$i\\['order'\\] == \\$i\\['count'\\]\\?\\}[\\S\\s]*\\{\\?/if\\?\\}\\s*\\{\\?/if\\?\\})", "$1章节cname1$3", RegexOptions.Singleline);
			}
		}
		Match match;
		if (Configs.BaseConfig.CmsVersion != "1.8")
		{
			match = Regex.Match(Templets_indexhtml, "\\{\\?section name=i loop=\\$indexrows\\?\\}(.*)\\{\\?if \\$indexrows\\[i\\]\\.ctype == \"volume\"\\?\\}(.*)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.*)\\{\\?/section\\?\\}", RegexOptions.Singleline);
			if (!match.Success)
			{
				throw new ApplicationException("分析目录页模板发生错误");
			}
			Templets_indextop = match.Groups[1].Value;
			Templets_indexvolume = match.Groups[2].Value;
			Templets_indexchapter = match.Groups[3].Value;
			Templets_indexbottom = match.Groups[4].Value;
			Templets_indexhtml = Regex.Replace(Templets_indexhtml, "\\{\\?section name=i loop=\\$indexrows\\?\\}(.*)\\{\\?if \\$indexrows\\[i\\]\\.ctype == \"volume\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.*)\\{\\?/section\\?\\}", "目录循环部分", RegexOptions.Singleline);
		}
		else
		{
			match = Regex.Match(Templets_indexhtml, "\\{\\?if \\$chapterrows\\[i\\].chaptertype > 0\\?\\}([\\S\\s]*)\\{\\?if \\$i\\['order'\\] == 1\\?\\}([\\S\\s]*)\\{\\?/if\\?\\}([\\S\\s]*)\\{\\?if \\$i\\['order'\\] == \\$i\\['count'\\]\\?\\}([\\S\\s]*)\\{\\?/if\\?\\}\\s*\\{\\?/if\\?\\}", RegexOptions.Singleline);
			if (!match.Success)
			{
				throw new ApplicationException("分析目录页模板发生错误");
			}
			Templets_indextop = match.Groups[2].Value;
			Templets_indexvolume = match.Groups[1].Value;
			Templets_indexchapter = match.Groups[3].Value;
			Templets_indexbottom = match.Groups[4].Value;
			Templets_indexhtml = Regex.Replace(Templets_indexhtml, "\\{\\?section name=i loop=\\$chapterrows\\?\\}([\\S\\s]*)\\{\\?/section\\?\\}", "目录循环部分", RegexOptions.Singleline);
		}
		SpiderException.Debug("Jieqi.Config.LoadWanerSkins 分析全文页模板");
		Match match2 = Regex.Match(TempletsFullHtml, "\\{\\?section name=i loop=\\$chapters\\?\\}(.+?)\\{\\?/section\\?\\}", RegexOptions.Singleline);
		if (!match2.Success)
		{
			throw new ApplicationException("分析全文阅读页模板发生错误1");
		}
		Templets_fullchapterloop = match2.Groups[1].Value;
		Templets_fullhtml = Regex.Replace(TempletsFullHtml, "\\{\\?section name=i loop=\\$chapters\\?\\}(.+?)\\{\\?/section\\?\\}", "章节循环部分", RegexOptions.Singleline);
		Templets_fullchapterlist.Clear();
		if (Configs.BaseConfig.CmsVersion != "1.8")
		{
			MatchCollection matchCollection2 = Regex.Matches(TempletsFullHtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", RegexOptions.Singleline);
			for (int l = 0; l < matchCollection2.Count; l++)
			{
				string[] value4 = new string[2]
				{
					matchCollection2[l].Groups[2].Value.Replace("&nbsp;", " "),
					matchCollection2[l].Groups[3].Value.Replace("&nbsp;", " ")
				};
				Templets_fullchapterlist.Add(matchCollection2[l].Groups[1].Value, value4);
			}
			Templets_fullhtml = Regex.Replace(TempletsFullHtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", "章节$1", RegexOptions.Singleline);
		}
		else
		{
			MatchCollection matchCollection2 = Regex.Matches(TempletsFullHtml, "\\{\\?if \\$i\\['order'\\] > 1\\?\\}[\\S\\s]*\\{\\?/if\\?\\}([\\S\\s]*)(\\{\\?\\$chapterrows\\[i\\]\\.chaptername\\?\\})([\\S\\s]*)\\{\\?if \\$i\\['order'\\] < \\$i\\['count'\\]\\?\\}", RegexOptions.Singleline);
			for (int m = 0; m < matchCollection2.Count; m++)
			{
				string[] value5 = new string[2]
				{
					"",
					matchCollection2[m].Groups[2].Value.Replace("&nbsp;", " ")
				};
				Templets_fullchapterlist.Add("vname", value5);
				Templets_fullhtml = Regex.Replace(TempletsFullHtml, "\\{\\?if \\$i\\['order'\\] > 1\\?\\}[\\S\\s]*\\{\\?/if\\?\\}([\\S\\s]*)(\\{\\?\\$chapterrows\\[i\\]\\.chaptername\\?\\})([\\S\\s]*)\\{\\?if \\$i\\['order'\\] < \\$i\\['count'\\]\\?\\}[\\S\\s]\\{\\?/if\\?\\}", "$1章节vname$3", RegexOptions.Singleline);
			}
			matchCollection2 = Regex.Matches(TempletsFullHtml, "\\{\\?if \\$i\\['order'\\] == 1\\?\\}([\\S\\s]*)\\{\\?/if\\?\\}([\\S\\s]*)\\{\\?if \\$i\\['order'\\] == \\$i\\['count'\\]\\?\\}([\\S\\s]*)\\{\\?/if\\?\\}\\s*\\{\\?/if\\?\\}", RegexOptions.Singleline);
			for (int n = 0; n < matchCollection2.Count; n++)
			{
				string[] value6 = new string[2]
				{
					"",
					matchCollection2[n].Groups[2].Value.Replace("&nbsp;", " ")
				};
				Templets_fullchapterlist.Add("cname1", value6);
				Templets_fullhtml = Regex.Replace(Templets_fullhtml, "(\\{\\?if \\$i\\['order'\\] == 1\\?\\}[\\S\\s]*\\{\\?/if\\?\\})([\\S\\s]*)(\\{\\?if \\$i\\['order'\\] == \\$i\\['count'\\]\\?\\}[\\S\\s]*\\{\\?/if\\?\\}\\s*\\{\\?/if\\?\\})", "$1章节cname1$3", RegexOptions.Singleline);
			}
		}
		if (Configs.BaseConfig.CmsVersion != "1.8")
		{
			SpiderException.Debug("Jieqi.Config.LoadWanerSkins 分析全文页模板");
			match2 = Regex.Match(TempletsFullHtml, "\\{\\?section name=i loop=\\$chapters\\?\\}(.+?)\\{\\?/section\\?\\}", RegexOptions.Singleline);
			if (!match2.Success)
			{
				throw new ApplicationException("分析全文阅读页模板发生错误1");
			}
			Templets_fullchapterloop = match2.Groups[1].Value;
			Templets_fullhtml = Regex.Replace(TempletsFullHtml, "\\{\\?section name=i loop=\\$chapters\\?\\}(.+?)\\{\\?/section\\?\\}", "章节循环部分", RegexOptions.Singleline);
			Templets_fullchapterlist.Clear();
			MatchCollection matchCollection2 = Regex.Matches(Templets_fullhtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", RegexOptions.Singleline);
			for (int num = 0; num < matchCollection2.Count; num++)
			{
				string[] value7 = new string[2]
				{
					matchCollection2[num].Groups[2].Value,
					matchCollection2[num].Groups[3].Value
				};
				Templets_fullchapterlist.Add(matchCollection2[num].Groups[1].Value, value7);
			}
			Templets_fullhtml = Regex.Replace(Templets_fullhtml, "\\{\\?if\\s+\\$indexrows\\[i\\]\\.([^=]+?)\\s*==\\s*\"\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}", "章节$1", RegexOptions.Singleline);
			Match match3 = Regex.Match(Templets_fullhtml, "\\{\\?section name=i loop=\\$indexrows\\?\\}(.+?)\\{\\?if \\$indexrows\\[i\\]\\.ctype == \"volume\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.+?)\\{\\?/section\\?\\}", RegexOptions.Singleline);
			if (!match3.Success)
			{
				throw new ApplicationException("分析全文阅读页模板发生错误2");
			}
			Templets_fulltop = match3.Groups[1].Value;
			Templets_fullvolume = match3.Groups[2].Value;
			Templets_fullchapter = match3.Groups[3].Value;
			Templets_fullbottom = match3.Groups[4].Value;
			Templets_fullhtml = Regex.Replace(Templets_fullhtml, "\\{\\?section name=i loop=\\$indexrows\\?\\}(.+?)\\{\\?if \\$indexrows\\[i\\]\\.ctype == \"volume\"\\?\\}(.+?)\\{\\?else\\?\\}(.+?)\\{\\?/if\\?\\}(.+?)\\{\\?/section\\?\\}", "目录循环部分", RegexOptions.Singleline);
		}
		else
		{
			if (!Regex.Match(Templets_fullhtml, "\\{\\?if \\$chapterrows\\[i\\].chaptertype > 0\\?\\}([\\S\\s]*)\\{\\?if \\$i\\['order'\\] == 1\\?\\}([\\S\\s]*)\\{\\?/if\\?\\}([\\S\\s]*)\\{\\?if \\$i\\['order'\\] == \\$i\\['count'\\]\\?\\}([\\S\\s]*)\\{\\?/if\\?\\}\\s*\\{\\?/if\\?\\}", RegexOptions.Singleline).Success)
			{
				throw new ApplicationException("分析全文阅读页模板发生错误3");
			}
			Templets_fulltop = match.Groups[2].Value;
			Templets_fullvolume = match.Groups[1].Value;
			Templets_fullchapter = match.Groups[3].Value;
			Templets_fullbottom = match.Groups[4].Value;
			Templets_fullhtml = Regex.Replace(Templets_indexhtml, "\\{\\?section name=i loop=\\$chapterrows\\?\\}([\\S\\s]*)\\{\\?/section\\?\\}", "目录循环部分", RegexOptions.Singleline);
		}
	}
}
