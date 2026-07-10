using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;
using PanGu;

namespace NovelSpider.Local.Jieqi;

public class LocalProvider : ILocalProvider
{
	private List<string> keywordlist = new List<string>();

	private static ProgressBar progressBar_0;

	public LocalProvider()
	{
		Config.LoadConfig();
		SpiderException.Debug("Jieqi LocalProvider");
	}

	private static DateTime FromUnixTimestamp(string timestamp)
	{
		return DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp)).LocalDateTime;
	}

	private static DateTime FromUnixTimestamp(int timestamp)
	{
		return DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
	}

	private static int GetLegacyChapterSize(string chapterText)
	{
		return chapterText.Replace("<br/>", "").Replace("<br />", "").Replace("&nbsp;", "")
			.Replace("</p>", "")
			.Replace("<p>", "")
			.Length * 2;
	}

	private static int GetJieqiWordCount(string chapterText, bool strictChapterInsert)
	{
		string text = chapterText.Replace("<br/>", "").Replace("<br />", "").Replace("&nbsp;", "")
			.Replace("</p>", "")
			.Replace("<p>", "");
		if (strictChapterInsert)
		{
			text = text.Replace("<br>", "").Replace("</br>", "").Replace(" ", "");
		}
		return text.Length;
	}

	private static bool IsCms18OrNewer()
	{
		return double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8;
	}

	private static bool IsCms24()
	{
		return Configs.BaseConfig.CmsVersion == "2.4";
	}

	private static string GetChapterSizeColumn()
	{
		return IsCms24() ? "words" : "size";
	}

	private static string BuildChapterInsertSql()
	{
		string sizeColumn = GetChapterSizeColumn();
		if (!IsCms18OrNewer())
		{
			return "INSERT INTO `jieqi_article_chapter` (`chapterid`,`articleid`,`articlename`,`volumeid`,`posterid`,`poster`,`postdate`,`lastupdate`,`chaptername`,`chapterorder`,`size`,`saleprice`,`salenum`,`totalcost`,`attachment`,`isvip`,`chaptertype`,`power`,`display`) VALUES (NULL,@articleid,@articlename,0,1,'admin',@postdate,@lastupdate,@chaptername,@chapterorder,@chapterSize,0,0,0,'',0,@chaptertype,0,0);";
		}
		return "INSERT INTO `jieqi_article_chapter` (`chapterid`,`articleid`,`articlename`,`volumeid`,`posterid`,`poster`,`postdate`,`lastupdate`,`chaptername`,`chapterorder`,`" + sizeColumn + "`,`saleprice`,`salenum`,`totalcost`,`attachment`,`isvip`,`chaptertype`,`power`,`display`,`preface`,`notice`,`foreword`) VALUES (NULL,@articleid,@articlename,0,1,'admin',@postdate,@lastupdate,@chaptername,@chapterorder,@chapterSize,0,0,0,'',0,@chaptertype,0,0,'','','');";
	}

	private static MySqlParameter[] CreateChapterParameters(NovelInfo novelInfo, string chapterName, int chapterOrder, int chapterSize, int chapterType, int timestamp)
	{
		return new MySqlParameter[]
		{
			new MySqlParameter("@articleid", novelInfo.PutID),
			new MySqlParameter("@articlename", novelInfo.Name),
			new MySqlParameter("@postdate", timestamp),
			new MySqlParameter("@lastupdate", timestamp),
			new MySqlParameter("@chaptername", chapterName),
			new MySqlParameter("@chapterorder", chapterOrder),
			new MySqlParameter("@chapterSize", chapterSize),
			new MySqlParameter("@chaptertype", chapterType)
		};
	}

	private static Encoding GetCmsTextEncoding()
	{
		return Configs.BaseConfig.CmsEncoding == "utf-8"
			? new UTF8Encoding(encoderShouldEmitUTF8Identifier: false)
			: FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset);
	}

	private static void WriteChapterTextFile(string directory, int chapterId, string chapterText)
	{
		ChapterFileWriter.WriteChapterText(directory, chapterId, chapterText, GetCmsTextEncoding());
	}

	private static void WriteGeneratedTextFile(string path, string content)
	{
		ChapterFileWriter.WriteTextAtomic(path, content, GetCmsTextEncoding());
	}

	private static object DbValue(object value)
	{
		return value ?? DBNull.Value;
	}

	private static bool UsePinyinDirectory()
	{
		return Configs.BaseConfig.NumOrPinyin == "拼音目录" && Configs.HaveFunction.IndexOf("PinyinDir") >= 0;
	}

	private static string BuildNovelInsertSql(bool usePinyinDirectory)
	{
		if (!IsCms18OrNewer())
		{
			if (usePinyinDirectory)
			{
				return "INSERT INTO `jieqi_article_article` (`articleid`,`postdate`,`lastupdate`,`articlename`,`articlecode`,`initial`,`author`,`posterid`,`poster`,`agentid`,`agent`,`sortid`,`intro`,`notice`,`setting`,`lastvolumeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`size`,`lastvisit`,`permission`,`fullflag`,`imgflag`,`chapters`,`keywords`,`display`) VALUES (NULL,@now,@now,@articlename,@articlecode,@initial,@author,1,'admin',0,0,@sortid,@intro,'','',0,'最新分卷',0,'最新章节',1,@now,0,@fullflag,@imgflag,1,@keywords,2);";
			}
			return "INSERT INTO `jieqi_article_article` (`articleid`,`postdate`,`lastupdate`,`articlename`,`initial`,`author`,`posterid`,`poster`,`agentid`,`agent`,`sortid`,`intro`,`notice`,`setting`,`lastvolumeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`size`,`lastvisit`,`permission`,`fullflag`,`imgflag`,`chapters`,`keywords`,`display`) VALUES (NULL,@now,@now,@articlename,@initial,@author,1,'admin',0,'',@sortid,@intro,'','',0,'最新分卷',0,'最新章节',1,@now,0,@fullflag,@imgflag,1,@keywords,2);";
		}
		if (IsCms24())
		{
			return "INSERT INTO `jieqi_article_article` (`articleid`,`postdate`,`lastupdate`,`infoupdate`,`articlename`,`articlecode`,`initial`,`author`,`posterid`,`poster`,`agentid`,`agent`,`sortid`,`typeid`,`intro`,`notice`,`setting`,`lastvolumeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`words`,`lastvisit`,`permission`,`fullflag`,`imgflag`,`chapters`,`keywords`,`rgroup`,`display`,`freetime`,`freewords`,`isvip`,`issign`,`siteid`,`pubinfo`,`buyinfo`,`vipsummary`) VALUES (NULL,@now,@now,@now,@articlename,@articlecode,@initial,@author,1,'admin',0,0,@sortid,@typeid,@intro,'','',0,'最新分卷',0,'最新章节',1,@now,0,@fullflag,@imgflag,1,@keywords,@rgroup,2,@now,1,0,0,@siteid,'','','');";
		}
		return "INSERT INTO `jieqi_article_article` (`articleid`,`postdate`,`lastupdate`,`infoupdate`,`articlename`,`articlecode`,`initial`,`author`,`posterid`,`poster`,`agentid`,`agent`,`sortid`,`typeid`,`intro`,`notice`,`setting`,`lastvolumeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`size`,`lastvisit`,`permission`,`fullflag`,`imgflag`,`chapters`,`keywords`,`rgroup`,`display`,`freetime`,`freesize`,`isvip`,`issign`,`siteid`) VALUES (NULL,@now,@now,@now,@articlename,@articlecode,@initial,@author,1,'admin',0,0,@sortid,@typeid,@intro,'','',0,'最新分卷',0,'最新章节',1,@now,0,@fullflag,@imgflag,1,@keywords,@rgroup,2,@now,1,0,0,@siteid);";
	}

	private static MySqlParameter[] CreateNovelInsertParameters(NovelInfo novelInfo, int coverFlag, int now)
	{
		string articleCode = CHz2Py.Convert4Hz2Py(novelInfo.Name ?? string.Empty);
		return new MySqlParameter[]
		{
			new MySqlParameter("@now", now),
			new MySqlParameter("@articlename", DbValue(novelInfo.Name)),
			new MySqlParameter("@articlecode", articleCode),
			new MySqlParameter("@initial", FormatText.ToChineseSpell(novelInfo.Name ?? string.Empty)),
			new MySqlParameter("@author", DbValue(novelInfo.Author)),
			new MySqlParameter("@sortid", novelInfo.LagerSortID),
			new MySqlParameter("@typeid", novelInfo.SmallSortID),
			new MySqlParameter("@intro", DbValue(novelInfo.Intro)),
			new MySqlParameter("@fullflag", novelInfo.Degree),
			new MySqlParameter("@imgflag", coverFlag),
			new MySqlParameter("@keywords", DbValue(novelInfo.Keyword)),
			new MySqlParameter("@rgroup", novelInfo.IsboyID),
			new MySqlParameter("@siteid", DbValue(novelInfo.RuleID))
		};
	}

	private static void ReadInsertedNovel(MySqlTransaction transaction, NovelInfo novelInfo, bool includeArticleCode)
	{
		object articleId = MySqlHelper.ExecuteScalar(transaction, CommandType.Text, "SELECT LAST_INSERT_ID()");
		novelInfo.PutID = Convert.ToInt32(articleId);
		string sql = includeArticleCode
			? "SELECT `articleid`,`articlecode`,`lastchapter` FROM `jieqi_article_article` WHERE `articleid`=@articleid"
			: "SELECT `articleid`,`lastchapter` FROM `jieqi_article_article` WHERE `articleid`=@articleid";
		DataTable dataTable = MySqlHelper.ExecuteDataTable(transaction, CommandType.Text, sql, new MySqlParameter("@articleid", novelInfo.PutID));
		if (dataTable.Rows.Count == 0)
		{
			return;
		}
		DataRow row = dataTable.Rows[0];
		if (includeArticleCode)
		{
			novelInfo.PinYin = row["articlecode"].ToString();
			novelInfo.PinYinSan = novelInfo.PinYin.Length >= 3 ? novelInfo.PinYin.Substring(0, 3) : novelInfo.PinYin;
		}
		novelInfo.LastChapter.ChapterName = row["lastchapter"].ToString();
		novelInfo.LastChapter.VolumeName = "书籍正在生成中,请稍等几分钟后刷新即可阅读";
	}

	private void InsertInnerTags(NovelInfo novelInfo)
	{
		if (!Configs.BaseConfig.InnerTagLink)
		{
			return;
		}
		Segment.Init();
		foreach (WordInfo item in new Segment().DoSegment(novelInfo.Name))
		{
			if (item != null && item.Word.Length > 1)
			{
				if (keywordlist != null && keywordlist.Count > 0 && !keywordlist.Contains(item.Word))
				{
					keywordlist.Add(item.Word);
				}
				MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, "INSERT INTO `jieqi_article_tag` (`tagid`,`articleid`,`tagname`,`length`,`hits`) VALUES (NULL,@articleid,@tagname,@length,0)",
					new MySqlParameter("@articleid", novelInfo.PutID),
					new MySqlParameter("@tagname", item.Word),
					new MySqlParameter("@length", item.Word.Length));
			}
		}
	}

	public static void BaiduPush(string urlNovelPush)
	{
		try
		{
			if (!Configs.BaseConfig.IsEnableBaiduPush)
			{
				return;
			}
			string strBaiduPushDomain = Configs.BaseConfig.StrBaiduPushDomain;
			string strBaiduPushToken = Configs.BaseConfig.StrBaiduPushToken;
			if (strBaiduPushDomain.Length == 0 || strBaiduPushToken.Length == 0)
			{
				return;
			}
			bool flag = false;
			FileInfo fileInfo = new FileInfo(Application.StartupPath + "/Log/BaiduPush.db3");
			string string_ = "Data Source=" + fileInfo.FullName;
			bool flag2 = false;
			if (!fileInfo.Exists)
			{
				SQLiteConnection.CreateFile(fileInfo.FullName);
				string string_2 = "CREATE TABLE [PushLog] ([SUCCESS] INT,[REMAIN] INT,[LASTTIME] NVARCHAR(100));";
				SQLiteHelper.ExecuteNonQuery(string_, string_2, (IDataParameter[])null);
			}
			else
			{
				string string_3 = "SELECT REMAIN,LASTTIME FROM [PushLog] ";
				DataSet dataSet = SQLiteHelper.ExecuteDataset(string_, string_3);
				if (dataSet != null && dataSet.Tables[0].Rows.Count >= 1)
				{
					flag = true;
					if (int.Parse(dataSet.Tables[0].Rows[0]["REMAIN"].ToString()) <= 0)
					{
						if (DateTime.Parse(dataSet.Tables[0].Rows[0]["LASTTIME"].ToString()).Date == DateTime.Now.Date)
						{
							return;
						}
						flag2 = true;
					}
				}
			}
			string requestUriString = "http://data.zz.baidu.com/urls?site=" + strBaiduPushDomain + "&token=" + strBaiduPushToken;
			byte[] bytes = FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk").GetBytes(urlNovelPush);
			Encoding charset = FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk");
			using System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
			using ByteArrayContent content = new ByteArrayContent(bytes);
			content.Headers.TryAddWithoutValidation("Content-Type", "text/plain");
			httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "curl/7.12.1");
			using HttpResponseMessage httpResponse = httpClient.PostAsync(requestUriString, content).GetAwaiter().GetResult();
			string value = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
			JObject jObject = (JObject)JsonConvert.DeserializeObject(value);
			int num = int.Parse(jObject["success"].ToString());
			int num2 = int.Parse(jObject["remain"].ToString());
			if (!flag)
			{
				string string_4 = string.Concat(new object[1] { "INSERT INTO [PushLog] (SUCCESS,REMAIN,LASTTIME) values ( " + num + "," + num2 + ",'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')" });
				SQLiteHelper.ExecuteNonQuery(string_, string_4, (IDataParameter[])null);
			}
			else
			{
				string text = "";
				text = ((!flag2) ? string.Concat(new object[1] { "UPDATE [PushLog] set SUCCESS=SUCCESS+1,REMAIN=" + num2 + ",LASTTIME='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" }) : string.Concat(new object[1] { "UPDATE [PushLog] set SUCCESS=1,REMAIN=" + num2 + ",LASTTIME='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" }));
				SQLiteHelper.ExecuteNonQuery(string_, text, (IDataParameter[])null);
			}
		}
		catch (Exception)
		{
		}
	}

	public void ClearNovel(NovelInfo novelInfo_0)
	{
		if (Configs.BaseConfig.NumOrPinyin == "拼音目录" && Configs.HaveFunction.IndexOf("PinyinDir") >= 0 && novelInfo_0.PinYin == "")
		{
			string string_ = "SELECT `articleid`,`articlecode` FROM `jieqi_article_article` WHERE `articleid` = '" + novelInfo_0.PutID + "'";
			MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
			if (mySqlDataReader.Read())
			{
				novelInfo_0.PinYin = mySqlDataReader["articlecode"].ToString();
			}
			if (!string.IsNullOrEmpty(novelInfo_0.PinYin))
			{
				novelInfo_0.PinYinSan = novelInfo_0.PinYin.Substring(0, 3).ToString();
			}
			mySqlDataReader.Close();
		}
		string string_2 = "Delete From `jieqi_article_chapter` WHERE `articleid`='" + novelInfo_0.PutID + "'";
		MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
		if (novelInfo_0.PinYinSan != null && novelInfo_0.PinYin != null && novelInfo_0.PinYinSan != "" && novelInfo_0.PinYin != "")
		{
			int num = novelInfo_0.PutID / 1000;
			string path = Config.HtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num.ToString()).Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
				.Replace("{Pinyin}", novelInfo_0.PinYin.ToString());
			if (Directory.Exists(path))
			{
				Directory.Delete(path, recursive: true);
			}
		}
		string path2 = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString();
		if (Directory.Exists(path2))
		{
			Directory.Delete(path2, recursive: true);
		}
		string path3 = Config.AttachDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString();
		if (Directory.Exists(path3))
		{
			Directory.Delete(path3, recursive: true);
		}
		string path4 = Config.FullDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + Config.JieqiArticleConfigs["htmlfile"].ToString();
		if (File.Exists(path4))
		{
			File.Delete(path4);
		}
		string path5 = Config.ZipDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + ".zip";
		if (File.Exists(path5))
		{
			File.Delete(path5);
			string string_3 = ((Configs.BaseConfig.CmsVersion == "2.4") ? ("UPDATE `jieqi_article_article` SET  `lastvolumeid`='0',`lastvolume`='最新分卷',`lastchapterid`='0',`lastchapter`='最新章节',`chapters`='0',`words`='0' WHERE `articleid`='" + novelInfo_0.PutID + "'") : ("UPDATE `jieqi_article_article` SET  `lastvolumeid`='0',`lastvolume`='最新分卷',`lastchapterid`='0',`lastchapter`='最新章节',`chapters`='0',`size`='0' WHERE `articleid`='" + novelInfo_0.PutID + "'"));
			MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_3, (MySqlParameter[])null);
		}
	}

	private void createAntiChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, bool bool_0)
	{
		if (chapterInfo_0.ChapterName == null)
		{
			return;
		}
		int num = chapterInfo_0.PutID - 1;
		int num2 = chapterInfo_0.PutID + 1;
		if (bool_0)
		{
			num2 = 0;
		}
		string text = "";
		string text2 = "";
		SpiderException.Debug("CreateChapter 替换模板");
		string text3 = Config.TempletsContent.Replace("<{if $authorid > 0}><a href=\"<{$article_dynamic_url}>/userinfo.php?id=<{$authorid}>\" target=\"_blank\"><{$author}></a><{else}><{$author}><{/if}>", "<{$author}>").Replace("<{$author}>", novelInfo_0.Author).Replace("<{$article_title}>", novelInfo_0.Name)
			.Replace("<{$jieqi_title}>", chapterInfo_0.ChapterName)
			.Replace("<{$sortname}>", novelInfo_0.LagerSort)
			.Replace("<{$jieqi_sitename}>", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString())
			.Replace("<{$jieqi_charset}>", Config.JieqiCharset)
			.Replace("<{$meta_keywords}>", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString())
			.Replace("<{$meta_description}>", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString())
			.Replace("<{$meta_author}>", Config.JieqiAuthor)
			.Replace("<{$meta_copyright}>", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString())
			.Replace("<{$new_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("{?$indexrows[i].vname?}", chapterInfo_0.VolumeName)
			.Replace("<{$articlename}>", novelInfo_0.Name)
			.Replace("<{$sort}>", novelInfo_0.LagerSort)
			.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
			.Replace("{?$articlesubdir?}", "/" + Convert.ToString(novelInfo_0.PutID / 1000));
		if (Configs.BaseConfig.LicenseVip && Configs.BaseConfig.ChapterNeighbor != 0 && Config.TempletsContent.IndexOf("{?$linju?}", StringComparison.Ordinal) > 0)
		{
			if (novelInfo_0.novelLj == null || novelInfo_0.novelLj.Length != Configs.BaseConfig.ChapterNeighbor)
			{
				novelInfo_0.novelLj = GetNovelLj(novelInfo_0, Configs.BaseConfig.ChapterNeighbor);
			}
			string text4 = "";
			string text5 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>\r\n";
			if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
			{
				text5 = Configs.BaseConfig.TuijianTemplates;
			}
			string[] novelLj = novelInfo_0.novelLj;
			foreach (string text6 in novelLj)
			{
				string text7 = text6.Split('^')[0];
				string text8 = text6.Split('^')[1];
				string text9 = text6.Split('^')[2];
				string text10 = text4;
				string newValue = Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", text7.ToString()).Replace("{Pinyin/3}", text7.Substring(0, 3).ToString()).Replace("{NovelId}", text8.ToString())
					.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text8) / 1000).ToString());
				string newValue2 = text9.ToString();
				string newValue3 = Config.ImageUrl + "/" + Convert.ToInt32(int.Parse(text8) / 1000) + "/" + text8.ToString() + "/" + text8.ToString() + "s.jpg";
				text4 = text10 + text5.Replace("{NovelUrl}", newValue).Replace("{NovelTitle}", newValue2).Replace("{NovelPic}", newValue3);
			}
			text3 = text3.Replace("{?$linju?}", text4);
		}
		else
		{
			text3 = text3.Replace("{?$linju?}", "");
		}
		if (Configs.BaseConfig.LicenseVip && Configs.BaseConfig.ChapterTuijian != 0 && Config.TempletsContent.IndexOf("{?$tuijian?}", StringComparison.Ordinal) > 0)
		{
			if (novelInfo_0.novelTj == null)
			{
				novelInfo_0.novelTj = GetNovelTj(novelInfo_0, Configs.BaseConfig.ChapterTuijian);
			}
			string text11 = "";
			int num3 = Configs.BaseConfig.ChapterTuijian;
			string[] array = novelInfo_0.novelTj;
			if (novelInfo_0.novelTj.Length < Configs.BaseConfig.ChapterTuijian)
			{
				num3 = novelInfo_0.novelTj.Length;
			}
			string text5 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>\r\n";
			if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
			{
				text5 = Configs.BaseConfig.TuijianTemplates;
			}
			for (int j = 0; j < num3; j++)
			{
				int num4 = new Random().Next(array.Length - 1);
				string text12 = array[num4];
				string text13 = text12.Split('^')[0];
				string text14 = text12.Split('^')[1];
				string text15 = text12.Split('^')[2];
				string text10 = text11;
				string newValue = Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", text13.ToString()).Replace("{Pinyin/3}", text13.Substring(0, 3).ToString()).Replace("{NovelId}", text14.ToString())
					.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text14) / 1000).ToString());
				string newValue2 = text15.ToString();
				string newValue3 = Config.ImageUrl + "/" + Convert.ToInt32(int.Parse(text14) / 1000) + "/" + text14.ToString() + "/" + text14.ToString() + "s.jpg";
				text11 = text10 + text5.Replace("{NovelUrl}", newValue).Replace("{NovelTitle}", newValue2).Replace("{NovelPic}", newValue3);
				array = DeleteStr(array, num4);
			}
			text3 = text3.Replace("{?$tuijian?}", text11);
		}
		else
		{
			text3 = text3.Replace("{?$tuijian?}", "");
		}
		text3 = text3.Replace("{?$url_preview?}", "{?$preview_page?}").Replace("{?$url_next?}", "{?$next_page?}").Replace("{?$url_index?}", "{?$index_page?}");
		text3 = (string.IsNullOrEmpty(text) ? text3.Replace("{?$preview_pageName?}", "返回目录") : text3.Replace("{?$preview_pageName?}", text));
		text3 = (string.IsNullOrEmpty(text2) ? text3.Replace("{?$next_pageName?}", "返回目录") : text3.Replace("{?$next_pageName?}", text2));
		int num5 = novelInfo_0.PutID / 1000;
		string newValue4 = Configs.BaseConfig.PrevFirstHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num5.ToString());
		num5 = novelInfo_0.PutID / 1000;
		string newValue5 = Configs.BaseConfig.NextEndHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num5.ToString());
		string text16 = ((num == 0) ? text3.Replace("{?$preview_page?}", newValue4).Replace("{?$preview_chapterid?}", "").Replace("{?$preview_chapteridd?}", "") : text3.Replace("{?$preview_page?}", num + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$preview_chapterid?}", num.ToString()).Replace("{?$preview_chapteridd?}", num + "/"));
		string string_ = ((num2 == 0) ? text16.Replace("{?$next_page?}", newValue5).Replace("{?$next_chapterid?}", "").Replace("{?$next_chapteridd?}", "") : text16.Replace("{?$next_page?}", num2 + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$next_chapterid?}", num2.ToString()).Replace("{?$next_chapteridd?}", num2 + "/")).Replace("<{$index_page}>", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$url_indexpage?}", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$index_page?}", Configs.BaseConfig.NextEndHtmlUrl)
			.Replace("<{$article_id}>", novelInfo_0.PutID.ToString())
			.Replace("<{$chapter_id}>", chapterInfo_0.PutID.ToString())
			.Replace("<{$dynamic_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("<{$url_bookroom}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/")
			.Replace("<{$url_articleinfo}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/articleinfo.php?id=" + novelInfo_0.PutID)
			.Replace("{?$navcode?}", CreateNav(novelInfo_0.LagerSortID))
			.Replace("{?$url_index?}", Configs.BaseConfig.NextEndHtmlUrl);
		SpiderException.Debug("CreateChapter 文字广告");
		if (Configs.BaseConfig.TextMarkOfHtml)
		{
			chapterInfo_0 = FormatText.TextMark(chapterInfo_0);
		}
		string value = ReplaceContents(string_, novelInfo_0, chapterInfo_0).Replace("{?$jieqi_content?}", chapterInfo_0.ChapterText);
		SpiderException.Debug("CreateChapter 生成文件");
		int num6 = novelInfo_0.PutID / 1000;
		string text17 = Config.HtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num6.ToString()).Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
			.Replace("{Pinyin}", novelInfo_0.PinYin.ToString());
		if (!Directory.Exists(text17))
		{
			Directory.CreateDirectory(text17);
		}
		WriteGeneratedTextFile(text17 + "/" + chapterInfo_0.PutID + Config.JieqiArticleConfigs["htmlfile"].ToString(), value);
	}

	private void createChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, bool bool_0)
	{
		if (!File.Exists(Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + chapterInfo_0.PutID.ToString() + ".txt") && chapterInfo_0.ChapterName == null)
		{
			return;
		}
		SpiderException.Debug("CreateChapter 获取当前章节");
		int num = 0;
		string text = "";
		string text2 = "";
		int num2 = 0;
		string text3 = "";
		int num3 = 0;
		string value = "";
		bool isEnableWapGen = Configs.BaseConfig.IsEnableWapGen;
		string string_ = ((Configs.BaseConfig.CmsVersion == "2.4") ? ("SELECT `chaptername`,`chapterorder`,`postdate`,`words` FROM `jieqi_article_chapter` WHERE `chapterid`='" + chapterInfo_0.PutID + "'") : ("SELECT `chaptername`,`chapterorder`,`postdate`,`size` FROM `jieqi_article_chapter` WHERE `chapterid`='" + chapterInfo_0.PutID + "'"));
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
		if (mySqlDataReader.Read())
		{
			chapterInfo_0.ChapterName = mySqlDataReader["chaptername"].ToString();
			Convert.ToInt32(mySqlDataReader["chapterorder"]);
			mySqlDataReader["postdate"].ToString();
			value = mySqlDataReader["postdate"].ToString();
			text3 = ((Configs.BaseConfig.CmsVersion == "2.4") ? mySqlDataReader["words"].ToString() : mySqlDataReader["size"].ToString());
		}
		mySqlDataReader.Close();
		string string_2 = "SELECT `chaptername`,`chapterorder` FROM `jieqi_article_chapter` WHERE `chapterid`='" + chapterInfo_0.PutID + "'";
		MySqlDataReader mySqlDataReader2 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
		if (mySqlDataReader2.Read())
		{
			chapterInfo_0.ChapterName = mySqlDataReader2["chaptername"].ToString();
			num3 = Convert.ToInt32(mySqlDataReader2["chapterorder"]);
		}
		mySqlDataReader2.Close();
		SpiderException.Debug("CreateChapter 读取上一页");
		string string_3 = "SELECT `chapterid`,`chaptername` FROM `jieqi_article_chapter` WHERE `chapterorder`<'" + num3 + "' AND `articleid`='" + novelInfo_0.PutID.ToString() + "' And `chaptertype` = '0' ORDER BY `chapterorder` DESC LIMIT 1";
		MySqlDataReader mySqlDataReader3 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_3, (MySqlParameter[])null);
		if (mySqlDataReader3.Read())
		{
			text = mySqlDataReader3["chaptername"].ToString();
			num = Convert.ToInt32(mySqlDataReader3["chapterid"]);
		}
		else
		{
			num = 0;
			text = "当前已是第一章节";
		}
		mySqlDataReader3.Close();
		SpiderException.Debug("CreateChapter 读取下一页");
		string string_4 = "SELECT `chapterid`,`chaptername` FROM `jieqi_article_chapter` WHERE `chapterorder`>'" + num3 + "' AND `articleid`='" + novelInfo_0.PutID.ToString() + "' And `chaptertype` = '0' ORDER BY `chapterorder` ASC LIMIT 1";
		MySqlDataReader mySqlDataReader4 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_4, (MySqlParameter[])null);
		if (mySqlDataReader4.Read())
		{
			text2 = mySqlDataReader4["chaptername"].ToString();
			num2 = Convert.ToInt32(mySqlDataReader4["chapterid"]);
		}
		else
		{
			num2 = 0;
			text2 = "暂无更多更新章节";
		}
		mySqlDataReader4.Close();
		SpiderException.Debug("CreateChapter 替换模板");
		int num4 = Convert.ToInt32(text3) / 2;
		string text4 = Config.TempletsContent.Replace("<{if $authorid > 0}><a href=\"<{$article_dynamic_url}>/userinfo.php?id=<{$authorid}>\" target=\"_blank\"><{$author}></a><{else}><{$author}><{/if}>", "<{$author}>").Replace("<{$author}>", novelInfo_0.Author).Replace("<{$article_title}>", novelInfo_0.Name)
			.Replace("<{$articlename}>", novelInfo_0.Name)
			.Replace("<{$jieqi_title}>", chapterInfo_0.ChapterName)
			.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
			.Replace("<{$sortname}>", novelInfo_0.LagerSort)
			.Replace("<{$sort}>", novelInfo_0.LagerSort)
			.Replace("<{$jieqi_sitename}>", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString())
			.Replace("<{$jieqi_charset}>", Config.JieqiCharset)
			.Replace("<{$meta_keywords}>", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString())
			.Replace("<{$meta_description}>", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString())
			.Replace("<{$meta_author}>", Config.JieqiAuthor)
			.Replace("<{$meta_copyright}>", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString())
			.Replace("<{$new_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("{?$indexrows[i].vname?}", chapterInfo_0.VolumeName)
			.Replace("{?$chaptertime|date:'Y-m-d H:i'?}", FormatText.GetTime(Convert.ToInt32(value)).ToString())
			.Replace("{?$chaptertime?}", FormatText.GetTime(Convert.ToInt32(value)).ToString())
			.Replace("{?$chaptersize_c?}", num4.ToString())
			.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
			.Replace("{?$articlesubdir?}", "/" + Convert.ToString(novelInfo_0.PutID / 1000));
		if (Configs.BaseConfig.ChapterNeighbor != 0 && Config.TempletsContent.IndexOf("{?$linju?}", StringComparison.Ordinal) > 0)
		{
			if (novelInfo_0.novelLj == null || novelInfo_0.novelLj.Length != Configs.BaseConfig.ChapterNeighbor)
			{
				novelInfo_0.novelLj = GetNovelLj(novelInfo_0, Configs.BaseConfig.ChapterNeighbor);
			}
			string text5 = "";
			string text6 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>\r\n";
			if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
			{
				text6 = Configs.BaseConfig.TuijianTemplates;
			}
			string[] novelLj = novelInfo_0.novelLj;
			foreach (string text7 in novelLj)
			{
				string text8 = text7.Split('^')[0];
				string text9 = text7.Split('^')[1];
				string text10 = text7.Split('^')[2];
				string text11 = text5;
				string newValue = Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", text8.ToString()).Replace("{Pinyin/3}", text8.Substring(0, 3).ToString()).Replace("{NovelId}", text9.ToString())
					.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text9) / 1000).ToString());
				string newValue2 = text10.ToString();
				string newValue3 = Config.ImageUrl + "/" + Convert.ToInt32(int.Parse(text9) / 1000) + "/" + text9.ToString() + "/" + text9.ToString() + "s.jpg";
				text5 = text11 + text6.Replace("{NovelUrl}", newValue).Replace("{NovelTitle}", newValue2).Replace("{NovelPic}", newValue3);
			}
			text4 = text4.Replace("{?$linju?}", text5);
		}
		else
		{
			text4 = text4.Replace("{?$linju?}", "");
		}
		if (Configs.BaseConfig.LicenseVip && Configs.BaseConfig.ChapterTuijian != 0 && Config.TempletsContent.IndexOf("{?$tuijian?}") > 0)
		{
			if (novelInfo_0.novelTj == null)
			{
				novelInfo_0.novelTj = GetNovelTj(novelInfo_0, Configs.BaseConfig.ChapterTuijian);
			}
			string text12 = "";
			int num5 = Configs.BaseConfig.ChapterTuijian;
			string[] array = novelInfo_0.novelTj;
			if (novelInfo_0.novelTj.Length < Configs.BaseConfig.ChapterTuijian)
			{
				num5 = novelInfo_0.novelTj.Length;
			}
			string text6 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>\r\n";
			if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
			{
				text6 = Configs.BaseConfig.TuijianTemplates;
			}
			for (int j = 0; j < num5; j++)
			{
				int num6 = new Random().Next(array.Length - 1);
				string text13 = array[num6];
				string text14 = text13.Split('^')[0];
				string text15 = text13.Split('^')[1];
				string text16 = text13.Split('^')[2];
				string text11 = text12;
				string newValue = Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", text14.ToString()).Replace("{Pinyin/3}", text14.Substring(0, 3).ToString()).Replace("{NovelId}", text15.ToString())
					.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text15) / 1000).ToString());
				string newValue2 = text16.ToString();
				string newValue3 = Config.ImageUrl + "/" + Convert.ToInt32(int.Parse(text15) / 1000) + "/" + text15.ToString() + "/" + text15.ToString() + "s.jpg";
				text12 = text11 + text6.Replace("{NovelUrl}", newValue).Replace("{NovelTitle}", newValue2).Replace("{NovelPic}", newValue3);
				array = DeleteStr(array, num6);
			}
			text4 = text4.Replace("{?$tuijian?}", text12);
		}
		else
		{
			text4 = text4.Replace("{?$tuijian?}", "");
		}
		text4 = text4.Replace("{?$url_preview?}", "{?$preview_page?}").Replace("{?$url_previous?}", "{?$preview_page?}").Replace("{?$url_next?}", "{?$next_page?}")
			.Replace("{?$url_next?}", "{?$next_page?}")
			.Replace("{?$url_index?}", "{?$index_page?}");
		text4 = (string.IsNullOrEmpty(text) ? text4.Replace("{?$preview_pageName?}", "返回目录") : text4.Replace("{?$preview_pageName?}", text));
		text4 = (string.IsNullOrEmpty(text2) ? text4.Replace("{?$next_pageName?}", "返回目录") : text4.Replace("{?$next_pageName?}", text2));
		int num7 = novelInfo_0.PutID / 1000;
		string newValue4 = Configs.BaseConfig.PrevFirstHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num7.ToString());
		num7 = novelInfo_0.PutID / 1000;
		string newValue5 = Configs.BaseConfig.NextEndHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num7.ToString());
		string text17 = ((num == 0) ? text4.Replace("{?$preview_page?}", newValue4).Replace("{?$url_previous?}", newValue4).Replace("{?$preview_chapteridd?}", "")
			.Replace("{?$preview_chapteridd?}", "") : text4.Replace("{?$preview_page?}", num + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$url_previous?}", num + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$preview_chapterid?}", num.ToString())
			.Replace("{?$preview_chapteridd?}", num + "/"));
		string text18 = ((num2 == 0) ? text17.Replace("{?$next_page?}", newValue5).Replace("{?$url_next?}", newValue5).Replace("{?$next_chapteridd?}", "")
			.Replace("{?$next_chapteridd?}", "") : text17.Replace("{?$next_page?}", num2 + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$url_next?}", num2 + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$next_chapterid?}", num2.ToString())
			.Replace("{?$next_chapteridd?}", num2 + "/")).Replace("<{$index_page}>", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$url_indexpage?}", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$index_page?}", Configs.BaseConfig.NextEndHtmlUrl)
			.Replace("{?$url_index?}", Configs.BaseConfig.NextEndHtmlUrl)
			.Replace("<{$article_id}>", novelInfo_0.PutID.ToString())
			.Replace("<{$chapter_id}>", chapterInfo_0.PutID.ToString())
			.Replace("<{$dynamic_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("<{$url_bookroom}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/")
			.Replace("<{$url_articleinfo}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/articleinfo.php?id=" + novelInfo_0.PutID)
			.Replace("{?$preChapterName?}", text)
			.Replace("{?$nextChapterName?}", text2);
		text18 = ((!(Config.JieqiArticleConfigs["dynamicurl"].ToString() == "")) ? text18.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiArticleConfigs["dynamicurl"].ToString()) : text18.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article"));
		SpiderException.Debug("CreateChapter 文字广告");
		if (Configs.BaseConfig.TextMarkOfHtml)
		{
			chapterInfo_0 = FormatText.TextMark(chapterInfo_0);
		}
		if (chapterInfo_0.ChapterText == null)
		{
			string path = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + chapterInfo_0.PutID.ToString() + ".txt";
			if (File.Exists(path))
			{
				chapterInfo_0.ChapterText = File.ReadAllText(path, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset));
			}
			else
			{
				chapterInfo_0.ChapterText = "本章节为空章节！";
			}
		}
		SpiderException.Debug("CreateChapter 盗链图片处理");
		string text19 = chapterInfo_0.ChapterText.Replace("%>_<%", "").Replace("%\\s*>", "").Replace("<\\s*%", "");
		string text20 = text19;
		if (!Configs.BaseConfig.Translate)
		{
			text19 = text19.Replace("  ", "&nbsp;&nbsp;");
			text20 = text20.Replace("  ", "&nbsp;&nbsp;");
		}
		string text21 = FormatText.Badwords(text19.Replace("\r", "").Replace("\n\n", "\n").Replace("\n\n", "\n")
			.Replace("\n\n", "\n")
			.Replace("\n", "<br /><br />"));
		if (text21.ToLower().IndexOf("<img src=") > 0)
		{
			text21 = text21.Replace("本章节为空章节！", "").Replace("<br /><br />", "");
		}
		if (Configs.BaseConfig.InnerTagLink)
		{
			SpiderException.Debug("CreateChapter 内容标签替换处理");
			text21 = InnerLinkReplace(text21);
		}
		if (novelInfo_0.novelTj == null && Configs.BaseConfig.InternalLink)
		{
			novelInfo_0.novelTj = GetNovelTj(novelInfo_0, Configs.BaseConfig.ChapterTuijian);
		}
		int internalLinkDensity = Configs.BaseConfig.InternalLinkDensity;
		if (text21.ToLower().IndexOf("本章节为空章节！") == 0 && text21.ToLower().IndexOf("<img src=") < 1)
		{
			SpiderException.EmptyTXT(novelInfo_0.Name, novelInfo_0.PutID, chapterInfo_0.ChapterName, chapterInfo_0.PutID);
		}
		if ((Configs.BaseConfig.OpenNullChapter && Configs.BaseConfig.NullChapter.Replace("\r\n", "").Trim() != "" && text21.ToLower().IndexOf("本章节为空章节！") == 0) || (chapterInfo_0.ChapterText.Length <= Configs.BaseConfig.SizeNullChapter && Configs.BaseConfig.NullChapter.Replace("\r\n", "").Trim() != ""))
		{
			FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset).GetBytes(chapterInfo_0.ChapterText);
			text21 = Configs.BaseConfig.NullChapter.ToString().Replace("{?$articleid|subdirectory?}", Convert.ToString(novelInfo_0.PutID / 1000)).Replace("<{$author}>", novelInfo_0.Author)
				.Replace("<{$article_title}>", novelInfo_0.Name)
				.Replace("<{$jieqi_title}>", chapterInfo_0.ChapterName)
				.Replace("<{$sortname}>", novelInfo_0.LagerSort)
				.Replace("<{$jieqi_sitename}>", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString())
				.Replace("<{$jieqi_charset}>", Config.JieqiCharset)
				.Replace("<{$meta_keywords}>", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString())
				.Replace("<{$meta_description}>", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString())
				.Replace("<{$meta_author}>", Config.JieqiAuthor)
				.Replace("<{$meta_copyright}>", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString())
				.Replace("<{$new_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
				.Replace("<{$articlename}>", novelInfo_0.Name)
				.Replace("<{$sort}>", novelInfo_0.LagerSort)
				.Replace("{?$chaptertime|date:'Y-m-d H:i'?}", FormatText.GetTime(Convert.ToInt32(value)).ToString())
				.Replace("{?$chaptertime?}", FormatText.GetTime(Convert.ToInt32(value)).ToString())
				.Replace("{?$chaptersize_c?}", num4.ToString())
				.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
				.Replace("{?$articlesubdir?}", "/" + Convert.ToString(novelInfo_0.PutID / 1000));
		}
		int length = text20.Length;
		text20 = ((length <= 70) ? text20.Substring(0, length) : text20.Substring(0, 70));
		string string_5 = text18.Replace("{?$jieqi_content?}", text21.Replace("{?postErr(", "<a href='").Replace(")?}", "' >").Replace("{?/postErr?}", "</a>")).Replace("{?$aaabc?}", text20 + "......").Replace("{?$size?}", text3)
			.Replace("{?$words?}", text3);
		string text22 = "";
		if (isEnableWapGen)
		{
			string strWapChapterTemplate = Configs.BaseConfig.StrWapChapterTemplate;
			string strWapHtmlDir = Configs.BaseConfig.StrWapHtmlDir;
			string strWapDomain = Configs.BaseConfig.StrWapDomain;
			text22 = File.ReadAllText(strWapChapterTemplate, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")).Replace("{?$wap_domain?}", strWapDomain).Replace("{?$jieqi_content?}", text21.Replace("{?postErr(", "<a href='").Replace(")?}", "' >").Replace("{?/postErr?}", "</a>"))
				.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
				.Replace("{?$aaabc?}", text20 + "......")
				.Replace("{?$size?}", text3)
				.Replace("{?$words?}", text3);
			text22 = ((num == 0) ? text22.Replace("{?$preview_page?}", newValue4).Replace("{?$preview_chapteridd?}", "") : text22.Replace("{?$preview_page?}", num + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$preview_chapterid?}", num.ToString()).Replace("{?$preview_chapteridd?}", num + "/"));
			text22 = ((num2 == 0) ? text22.Replace("{?$next_page?}", newValue5).Replace("{?$next_chapteridd?}", "") : text22.Replace("{?$next_page?}", num2 + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$next_chapterid?}", num2.ToString()).Replace("{?$next_chapteridd?}", num2 + "/"));
			text22 = ReplaceContents(text22, novelInfo_0, chapterInfo_0);
			int num8 = novelInfo_0.PutID / 1000;
			string text23 = strWapHtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num8.ToString()).Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
				.Replace("{Pinyin}", novelInfo_0.PinYin.ToString());
			if (!Directory.Exists(text23))
			{
				Directory.CreateDirectory(text23);
			}
			WriteGeneratedTextFile(text23 + "/" + chapterInfo_0.PutID + Config.JieqiArticleConfigs["htmlfile"].ToString(), text22);
		}
		SpiderException.Debug("CreateChapter 通用替换处理");
		string value2 = ReplaceContents(string_5, novelInfo_0, chapterInfo_0);
		SpiderException.Debug("CreateChapter 生成文件");
		int num9 = novelInfo_0.PutID / 1000;
		string text24 = Config.HtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num9.ToString()).Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
			.Replace("{Pinyin}", novelInfo_0.PinYin.ToString());
		if (!Directory.Exists(text24))
		{
			Directory.CreateDirectory(text24);
		}
		WriteGeneratedTextFile(text24 + "/" + chapterInfo_0.PutID + Config.JieqiArticleConfigs["htmlfile"].ToString(), value2);
		if (bool_0)
		{
			if (num2 != 0)
			{
				SpiderException.Debug("CreateChapter 开始处理下一页");
				ChapterInfo chapterInfo = new ChapterInfo
				{
					PutID = num2
				};
				ChapterInfo chapterInfo_1 = chapterInfo;
				createChapter(novelInfo_0, chapterInfo_1, bool_0: false);
			}
			if (num != 0)
			{
				SpiderException.Debug("CreateChapter 开始处理上一页");
				ChapterInfo chapterInfo2 = new ChapterInfo
				{
					PutID = num
				};
				ChapterInfo chapterInfo_2 = chapterInfo2;
				createChapter(novelInfo_0, chapterInfo_2, bool_0: false);
			}
		}
	}

	public void CreateChapter(NovelInfo novelInfo_0)
	{
		createChapter(novelInfo_0, novelInfo_0.LastChapter, bool_0: true);
	}

	public void CreateChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0)
	{
		createChapter(novelInfo_0, chapterInfo_0, bool_0: true);
	}

	public void CreateIndex(NovelInfo currentNovel, bool isGenIndexHtml, bool isGenFullHtml, bool isGenOPF, bool isGenZip, bool isGenFullTXT, bool isGenUMD, bool isGenJar, bool isGenCHM, bool isDeleteTXT, bool isDeleteHTML, int delnum)
	{
		if (!isGenIndexHtml)
		{
			CreateOPF(currentNovel);
			return;
		}
		bool flag = false;
		flag = Configs.BaseConfig.IsEnableWapGen;
		SpiderException.Debug("CreateIndex 生成章节目录页");
		int num = ((!(double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)) ? 1 : Convert.ToInt32(Config.JieqiArticleConfigs["indexcols"]));
		int num2 = 1;
		string text = "";
		string text2 = "<spine>\n";
		if (isGenOPF)
		{
			SpiderException.Debug("CreateIndex 生成OPF");
			if (currentNovel.Keyword == null)
			{
				currentNovel.Keyword = currentNovel.Name;
			}
			if (double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)
			{
				object obj = text + "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\n";
				object obj2 = string.Concat(obj, "<package unique-identifier=\"", Config.JieqiDefine["JIEQI_URL"], "/modules/article/-", currentNovel.PutID, "\">\n", "<metadata>\n<dc-metadata>\n<dc:Id>", currentNovel.PutID, "</dc:Id>\n<dc:Title>", currentNovel.Name.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
					.Replace("\n", "")
					.Replace("\r", ""), "</dc:Title>\n<dc:Creator>", currentNovel.Author.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
					.Replace("\n", "")
					.Replace("\r", ""), "</dc:Creator>\n<dc:Subject>", currentNovel.Keyword.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
					.Replace("\n", "")
					.Replace("\r", ""), "</dc:Subject>\n<dc:Description>", currentNovel.Intro.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
					.Replace("\n", "")
					.Replace("\r", ""), "</dc:Description>\n");
				object obj3 = string.Concat(obj2, "<dc:Publisher>", Config.JieqiDefine["JIEQI_SITE_NAME"], "</dc:Publisher>\n", "<dc:Contributorid>1</dc:Contributorid>\n<dc:Contributor>admin</dc:Contributor>\n");
				object obj4 = string.Concat(obj3, "<dc:Sortid>", currentNovel.LagerSortID, "</dc:Sortid>\n", "<dc:Typeid>0</dc:Typeid>\n<dc:Articletype>0</dc:Articletype>\n<dc:Permission>0</dc:Permission>\r\n<dc:Firstflag>0</dc:Firstflag>\n<dc:Fullflag>0</dc:Fullflag>\n<dc:Imgflag>0</dc:Imgflag>\n<dc:Power>0</dc:Power>\n<dc:Display>0</dc:Display>\n");
				text = string.Concat(obj4, "<dc:Date>", DateTime.Today, "</dc:Date>\n", "<dc:Type>Text</dc:Type>\n<dc:Format>text</dc:Format>\n<dc:Language>ZH</dc:Language>\n</dc-metadata>\n</metadata>\n<manifest>\n");
			}
			else if (Configs.BaseConfig.CmsVersion == "2.4")
			{
				int num3 = 0;
				StringBuilder stringBuilder = new StringBuilder();
				string string_ = "SELECT * FROM `jieqi_article_article` WHERE `articleid` = '" + currentNovel.PutID + "'";
				MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
				if (mySqlDataReader.Read())
				{
					try
					{
						num3 = Convert.ToInt32(mySqlDataReader["imgflag"]);
					}
					catch
					{
					}
					object obj = text + "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\r\n";
					object obj6 = string.Concat(new object[281]
					{
						string.Concat(obj, "<packageid=\"", currentNovel.PutID.ToString(), "\">\r\n<articleinfo>\r\n"),
						"<articleid>",
						HttpUtility.HtmlEncode(currentNovel.PutID.ToString()),
						"</articleid>\r\n<siteid>",
						HttpUtility.HtmlEncode(mySqlDataReader["siteid"].ToString()),
						"</siteid>\r\n<sourceid>",
						HttpUtility.HtmlEncode(mySqlDataReader["sourceid"].ToString()),
						"</sourceid>\r\n<postdate>",
						HttpUtility.HtmlEncode(mySqlDataReader["postdate"].ToString()),
						"</postdate>\r\n<lastupdate>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastupdate"].ToString()),
						"</lastupdate>\r\n<infoupdate>",
						HttpUtility.HtmlEncode(mySqlDataReader["infoupdate"].ToString()),
						"</infoupdate>\r\n<articlename>",
						HttpUtility.HtmlEncode(mySqlDataReader["articlename"].ToString()),
						"</articlename>\r\n<articlecode>",
						HttpUtility.HtmlEncode(mySqlDataReader["articlecode"].ToString()),
						"</articlecode>\r\n<backupname>",
						HttpUtility.HtmlEncode(mySqlDataReader["backupname"].ToString()),
						"</backupname>\r\n<keywords>",
						HttpUtility.HtmlEncode(mySqlDataReader["keywords"].ToString()),
						"</keywords>\r\n<roles>",
						HttpUtility.HtmlEncode(mySqlDataReader["roles"].ToString()),
						"</roles>\r\n<initial>",
						HttpUtility.HtmlEncode(mySqlDataReader["initial"].ToString()),
						"</initial>\r\n<authorid>",
						HttpUtility.HtmlEncode(mySqlDataReader["authorid"].ToString()),
						"</authorid>\r\n<author>",
						HttpUtility.HtmlEncode(mySqlDataReader["author"].ToString()),
						"</author>\r\n<posterid>",
						HttpUtility.HtmlEncode(mySqlDataReader["posterid"].ToString()),
						"</posterid>\r\n<poster>",
						HttpUtility.HtmlEncode(mySqlDataReader["poster"].ToString()),
						"</poster>\r\n<agentid>",
						HttpUtility.HtmlEncode(mySqlDataReader["agentid"].ToString()),
						"</agentid>\r\n<agent>",
						HttpUtility.HtmlEncode(mySqlDataReader["agent"].ToString()),
						"</agent>\r\n<reviewerid>",
						HttpUtility.HtmlEncode(mySqlDataReader["reviewerid"].ToString()),
						"</masterid>\r\n<reviewer>",
						HttpUtility.HtmlEncode(mySqlDataReader["reviewer"].ToString()),
						"</master>\r\n<sortid>",
						HttpUtility.HtmlEncode(mySqlDataReader["sortid"].ToString()),
						"</sortid>\r\n<typeid>",
						HttpUtility.HtmlEncode(mySqlDataReader["typeid"].ToString()),
						"</typeid>\r\n<libid>",
						HttpUtility.HtmlEncode(mySqlDataReader["libid"].ToString()),
						"</libid>\r\n<intro>",
						HttpUtility.HtmlEncode(mySqlDataReader["intro"].ToString()),
						"</intro>\r\n<notice>",
						HttpUtility.HtmlEncode(mySqlDataReader["notice"].ToString()),
						"</notice>\r\n<foreword>",
						HttpUtility.HtmlEncode(mySqlDataReader["foreword"].ToString()),
						"</foreword>\r\n<setting>",
						HttpUtility.HtmlEncode(mySqlDataReader["setting"].ToString()),
						"</setting>\r\n<lastvolumeid>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastvolumeid"].ToString()),
						"</lastvolumeid>\r\n<lastvolume>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastvolume"].ToString()),
						"</lastvolume>\r\n<lastchapterid>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastchapterid"].ToString()),
						"</lastchapterid>\r\n<lastchapter>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastchapter"].ToString()),
						"</lastchapter>\r\n<lastsummary>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastsummary"].ToString()),
						"</lastsummary>\r\n<chapters>",
						HttpUtility.HtmlEncode(mySqlDataReader["chapters"].ToString()),
						"</chapters>\r\n<words>",
						HttpUtility.HtmlEncode(mySqlDataReader["words"].ToString()),
						"</words>\r\n<daywords>",
						HttpUtility.HtmlEncode(mySqlDataReader["daywords"].ToString()),
						"</daywords>\r\n<weekwords>",
						HttpUtility.HtmlEncode(mySqlDataReader["weekwords"].ToString()),
						"</weekwords>\r\n<monthwords>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthwords"].ToString()),
						"</monthwords>\r\n<prewords>",
						HttpUtility.HtmlEncode(mySqlDataReader["prewords"].ToString()),
						"</prewords>\r\n<monthupds>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthupds"].ToString()),
						"</monthupds>\r\n<preupds>",
						HttpUtility.HtmlEncode(mySqlDataReader["preupds"].ToString()),
						"</preupds>\r\n<monthupdt>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthupdt"].ToString()),
						"</monthupdt>\r\n<preupdt>",
						HttpUtility.HtmlEncode(mySqlDataReader["preupdt"].ToString()),
						"</preupdt>\r\n<lastvisit>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastvisit"].ToString()),
						"</lastvisit>\r\n<dayvisit>",
						HttpUtility.HtmlEncode(mySqlDataReader["dayvisit"].ToString()),
						"</dayvisit>\r\n<weekvisit>",
						HttpUtility.HtmlEncode(mySqlDataReader["weekvisit"].ToString()),
						"</weekvisit>\r\n<monthvisit>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthvisit"].ToString()),
						"</monthvisit>\r\n<allvisit>",
						HttpUtility.HtmlEncode(mySqlDataReader["allvisit"].ToString()),
						"</allvisit>\r\n<previsit>",
						HttpUtility.HtmlEncode(mySqlDataReader["previsit"].ToString()),
						"</previsit>\r\n<lastvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastvote"].ToString()),
						"</lastvote>\r\n<dayvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["dayvote"].ToString()),
						"</dayvote>\r\n<weekvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["weekvote"].ToString()),
						"</weekvote>\r\n<monthvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthvote"].ToString()),
						"</monthvote>\r\n<allvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["allvote"].ToString()),
						"</allvote>\r\n<prevote>",
						HttpUtility.HtmlEncode(mySqlDataReader["prevote"].ToString()),
						"</prevote>\r\n<lastdown>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastdown"].ToString()),
						"</lastdown>\r\n<daydown>",
						HttpUtility.HtmlEncode(mySqlDataReader["daydown"].ToString()),
						"</daydown>\r\n<weekdown>",
						HttpUtility.HtmlEncode(mySqlDataReader["weekdown"].ToString()),
						"</weekdown>\r\n<monthdown>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthdown"].ToString()),
						"</monthdown>\r\n<alldown>",
						HttpUtility.HtmlEncode(mySqlDataReader["alldown"].ToString()),
						"</alldown>\r\n<predown>",
						HttpUtility.HtmlEncode(mySqlDataReader["predown"].ToString()),
						"</predown>\r\n<lastflower>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastflower"].ToString()),
						"</lastflower>\r\n<dayflower>",
						HttpUtility.HtmlEncode(mySqlDataReader["dayflower"].ToString()),
						"</dayflower>\r\n<weekflower>",
						HttpUtility.HtmlEncode(mySqlDataReader["weekflower"].ToString()),
						"</weekflower>\r\n<monthflower>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthflower"].ToString()),
						"</monthflower>\r\n<allflower>",
						HttpUtility.HtmlEncode(mySqlDataReader["allflower"].ToString()),
						"</allflower>\r\n<preflower>",
						HttpUtility.HtmlEncode(mySqlDataReader["preflower"].ToString()),
						"</preflower>\r\n<lastegg>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastegg"].ToString()),
						"</lastegg>\r\n<dayegg>",
						HttpUtility.HtmlEncode(mySqlDataReader["dayegg"].ToString()),
						"</dayegg>\r\n<weekegg>",
						HttpUtility.HtmlEncode(mySqlDataReader["weekegg"].ToString()),
						"</weekegg>\r\n<monthegg>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthegg"].ToString()),
						"</monthegg>\r\n<allegg>",
						HttpUtility.HtmlEncode(mySqlDataReader["allegg"].ToString()),
						"</allegg>\r\n<preegg>",
						HttpUtility.HtmlEncode(mySqlDataReader["preegg"].ToString()),
						"</preegg>\r\n<lastvipvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["lastvipvote"].ToString()),
						"</lastvipvote>\r\n<dayvipvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["dayvipvote"].ToString()),
						"</dayvipvote>\r\n<weekvipvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["weekvipvote"].ToString()),
						"</weekvipvote>\r\n<monthvipvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthvipvote"].ToString()),
						"</monthvipvote>\r\n<allvipvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["allvipvote"].ToString()),
						"</allvipvote>\r\n<previpvote>",
						HttpUtility.HtmlEncode(mySqlDataReader["previpvote"].ToString()),
						"</previpvote>\r\n<hotnum>",
						HttpUtility.HtmlEncode(mySqlDataReader["hotnum"].ToString()),
						"</hotnum>\r\n<goodnum>",
						HttpUtility.HtmlEncode(mySqlDataReader["goodnum"].ToString()),
						"</goodnum>\r\n<reviewsnum>",
						HttpUtility.HtmlEncode(mySqlDataReader["reviewsnum"].ToString()),
						"</reviewsnum>\r\n<ratenum>",
						HttpUtility.HtmlEncode(mySqlDataReader["ratenum"].ToString()),
						"</ratenum>\r\n<ratesum>",
						HttpUtility.HtmlEncode(mySqlDataReader["ratesum"].ToString()),
						"</ratesum>\r\n<rate1>",
						HttpUtility.HtmlEncode(mySqlDataReader["rate1"].ToString()),
						"</rate1>\r\n<rate2>",
						HttpUtility.HtmlEncode(mySqlDataReader["rate2"].ToString()),
						"</rate2>\r\n<rate3>",
						HttpUtility.HtmlEncode(mySqlDataReader["rate3"].ToString()),
						"</rate3>\r\n<rate4>",
						HttpUtility.HtmlEncode(mySqlDataReader["rate4"].ToString()),
						"</rate4>\r\n<rate5>",
						HttpUtility.HtmlEncode(mySqlDataReader["rate5"].ToString()),
						"</rate5>\r\n<toptime>",
						HttpUtility.HtmlEncode(mySqlDataReader["toptime"].ToString()),
						"</toptime>\r\n<saleprice>",
						HttpUtility.HtmlEncode(mySqlDataReader["saleprice"].ToString()),
						"</saleprice>\r\n<salenum>",
						HttpUtility.HtmlEncode(mySqlDataReader["salenum"].ToString()),
						"</salenum>\r\n<totalcost>",
						HttpUtility.HtmlEncode(mySqlDataReader["totalcost"].ToString()),
						"</totalcost>\r\n<unionid>",
						HttpUtility.HtmlEncode(mySqlDataReader["unionid"].ToString()),
						"</unionid>\r\n<permission>",
						HttpUtility.HtmlEncode(mySqlDataReader["permission"].ToString()),
						"</permission>\r\n<firstflag>",
						HttpUtility.HtmlEncode(mySqlDataReader["firstflag"].ToString()),
						"</firstflag>\r\n<fullflag>",
						HttpUtility.HtmlEncode(mySqlDataReader["fullflag"].ToString()),
						"</fullflag>\r\n<imgflag>",
						HttpUtility.HtmlEncode(mySqlDataReader["imgflag"].ToString()),
						"</imgflag>\r\n<upaudit>",
						HttpUtility.HtmlEncode(mySqlDataReader["upaudit"].ToString()),
						"</upaudit>\r\n<power>",
						HttpUtility.HtmlEncode(mySqlDataReader["power"].ToString()),
						"</power>\r\n<display>",
						HttpUtility.HtmlEncode(mySqlDataReader["display"].ToString()),
						"</display>\r\n<progress>",
						HttpUtility.HtmlEncode(mySqlDataReader["progress"].ToString()),
						"</progress>\r\n<issign>",
						HttpUtility.HtmlEncode(mySqlDataReader["issign"].ToString()),
						"</issign>\r\n<signtime>",
						HttpUtility.HtmlEncode(mySqlDataReader["signtime"].ToString()),
						"</signtime>\r\n<buyout>",
						HttpUtility.HtmlEncode(mySqlDataReader["buyout"].ToString()),
						"</buyout>\r\n<monthly>",
						HttpUtility.HtmlEncode(mySqlDataReader["monthly"].ToString()),
						"</monthly>\r\n<discount>",
						HttpUtility.HtmlEncode(mySqlDataReader["discount"].ToString()),
						"</discount>\r\n<quality>",
						HttpUtility.HtmlEncode(mySqlDataReader["quality"].ToString()),
						"</quality>\r\n<isshort>",
						HttpUtility.HtmlEncode(mySqlDataReader["isshort"].ToString()),
						"</isshort>\r\n<inmatch>",
						HttpUtility.HtmlEncode(mySqlDataReader["inmatch"].ToString()),
						"</inmatch>\r\n<isshare>",
						HttpUtility.HtmlEncode(mySqlDataReader["isshare"].ToString()),
						"</isshare>\r\n<rgroup>",
						HttpUtility.HtmlEncode(mySqlDataReader["rgroup"].ToString()),
						"</rgroup>\r\n<ispub>",
						HttpUtility.HtmlEncode(mySqlDataReader["ispub"].ToString()),
						"</ispub>\r\n<pubtime>",
						HttpUtility.HtmlEncode(mySqlDataReader["pubtime"].ToString()),
						"</pubtime>\r\n<pubid>",
						HttpUtility.HtmlEncode(mySqlDataReader["pubid"].ToString()),
						"</pubid>\r\n<pubhouse>",
						HttpUtility.HtmlEncode(mySqlDataReader["pubhouse"].ToString()),
						"</pubhouse>\r\n<pubprice>",
						HttpUtility.HtmlEncode(mySqlDataReader["pubprice"].ToString()),
						"</pubprice>\r\n<pubpages>",
						HttpUtility.HtmlEncode(mySqlDataReader["pubpages"].ToString()),
						"</pubpages>\r\n<pubisbn>",
						HttpUtility.HtmlEncode(mySqlDataReader["pubisbn"].ToString()),
						"</pubisbn>\r\n<pubinfo>",
						HttpUtility.HtmlEncode(mySqlDataReader["pubinfo"].ToString()),
						"</pubinfo>\r\n<buysid>",
						HttpUtility.HtmlEncode(mySqlDataReader["buysid"].ToString()),
						"</buysid>\r\n<buysite>",
						HttpUtility.HtmlEncode(mySqlDataReader["buysite"].ToString()),
						"</buysite>\r\n<buyurl>",
						HttpUtility.HtmlEncode(mySqlDataReader["buyurl"].ToString()),
						"</buyurl>\r\n<buyprice>",
						HttpUtility.HtmlEncode(mySqlDataReader["buyprice"].ToString()),
						"</buyprice>\r\n<buyinfo>",
						HttpUtility.HtmlEncode(mySqlDataReader["buyinfo"].ToString()),
						"</buyinfo>\r\n<freetime>",
						HttpUtility.HtmlEncode(mySqlDataReader["freetime"].ToString()),
						"</freetime>\r\n<freewords>",
						HttpUtility.HtmlEncode(mySqlDataReader["freewords"].ToString()),
						"</freewords>\r\n<freestart>",
						HttpUtility.HtmlEncode(mySqlDataReader["freestart"].ToString()),
						"</freetime>\r\n<freeend>",
						HttpUtility.HtmlEncode(mySqlDataReader["freeend"].ToString()),
						"</freewords>\r\n<isvip>",
						HttpUtility.HtmlEncode(mySqlDataReader["isvip"].ToString()),
						"</isvip>\r\n<viptime>",
						HttpUtility.HtmlEncode(mySqlDataReader["viptime"].ToString()),
						"</viptime>\r\n<vipid>",
						HttpUtility.HtmlEncode(mySqlDataReader["vipid"].ToString()),
						"</vipid>\r\n<vippubid>",
						HttpUtility.HtmlEncode(mySqlDataReader["vippubid"].ToString()),
						"</vippubid>\r\n<vipchapters>",
						HttpUtility.HtmlEncode(mySqlDataReader["vipchapters"].ToString()),
						"</vipchapters>\r\n<vipwords>",
						HttpUtility.HtmlEncode(mySqlDataReader["vipwords"].ToString()),
						"</vipwords>\r\n<vipvolumeid>",
						HttpUtility.HtmlEncode(mySqlDataReader["vipvolumeid"].ToString()),
						"</vipvolumeid>\r\n<vipvolume>",
						HttpUtility.HtmlEncode(mySqlDataReader["vipvolume"].ToString()),
						"</vipvolume>\r\n<vipchapterid>",
						HttpUtility.HtmlEncode(mySqlDataReader["vipchapterid"].ToString()),
						"</vipchapterid>",
						"\r\n<vipchapter>",
						HttpUtility.HtmlEncode(mySqlDataReader["vipchapter"].ToString()),
						"</vipchapter>\r\n<vipsummary>",
						HttpUtility.HtmlEncode(mySqlDataReader["vipsummary"].ToString()),
						"</vipsummary>\r\n"
					});
					text = string.Concat(new object[2] { obj6, "</articleinfo>\r\n<chapters>\r\n" });
				}
				mySqlDataReader.Close();
			}
			else
			{
				string string_2 = "SELECT * FROM `jieqi_article_article` WHERE `articleid`='" + currentNovel.PutID + "'";
				MySqlDataReader mySqlDataReader2 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
				string text3 = "";
				string text4 = "";
				string text5 = "";
				string text6 = "";
				if (mySqlDataReader2.Read())
				{
					text3 = mySqlDataReader2["imgflag"].ToString();
					text4 = mySqlDataReader2["lastchapterid"].ToString();
					text5 = mySqlDataReader2["lastchapter"].ToString();
					text6 = mySqlDataReader2["lastsummary"].ToString();
				}
				mySqlDataReader2.Close();
				object obj = text + "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\n";
				object obj7 = string.Concat(string.Concat(new object[2]
				{
					obj,
					"<package id=\"" + currentNovel.PutID + "\">\n<articleinfo>\n"
				}), "<articleid>", currentNovel.PutID, "</articleid>\n<postdate>", currentNovel.PostDate, "</postdate>\n<lastupdate>", currentNovel.LastupDate, "</lastupdate>\n<infoupdate>", currentNovel.PostDate, "</infoupdate>\n<articlename>", currentNovel.Name.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
					.Replace("\n", "")
					.Replace("\r", ""), "</articlename>\n<articlecode>", CHz2Py.Convert4Hz2Py(currentNovel.Name), "</articlecode>\n<author>", currentNovel.Author.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
					.Replace("\n", "")
					.Replace("\r", ""), "</author>\n<keywords>", currentNovel.Keyword.Replace("&", "").Replace("<", "").Replace(">", "")
					.Replace("\n", "")
					.Replace("\r", ""), "</keywords>\n<intro>", currentNovel.Intro.Replace("&", "").Replace("<", "").Replace(">", "")
					.Replace("\n", "")
					.Replace("\r", ""), "</intro>\n");
				object obj8 = string.Concat(string.Concat(obj7, "<sortid>", currentNovel.LagerSortID, "</sortid>\n"), "<typeid>", currentNovel.SmallSortID, "</typeid>\n<size>", currentNovel.Size, "</size>\n<firstflag>0</firstflag>\n<fullflag>", currentNovel.Degree, "</fullflag>\n<imgflag>", text3, "</imgflag>\n");
				text = string.Concat(obj8, "\n<lastchapterid>", text4, "</lastchapterid>\n<lastchapter>", text5, "</lastchapter>\n<lastsummary>", text6.Replace("&nbsp;", "").Replace("<", "").Replace(">", "")
					.Replace("\n", "")
					.Replace("\r", ""), "</lastsummary>\n<rgroup>", currentNovel.IsboyID, "</rgroup>\n", "</articleinfo>\n<chapters>\n");
			}
		}
		ArrayList arrayList_ = new ArrayList();
		ArrayList arrayList_2 = new ArrayList();
		if (isGenUMD)
		{
			if (!string.IsNullOrEmpty(Configs.BaseConfig.EBookHead))
			{
				arrayList_.Add("引子");
				arrayList_2.Add(Configs.BaseConfig.EBookHead);
			}
			arrayList_.Add("作品介绍");
			arrayList_2.Add("书名：" + currentNovel.Name + "\r\n作者：" + currentNovel.Author + "\r\n分类：" + currentNovel.LagerSort + "\r\n制作：" + Config.JieqiDefine["JIEQI_SITE_NAME"].ToString() + "\r\n网址：" + Config.JieqiDefine["JIEQI_URL"].ToString());
		}
		string text7 = string.Empty;
		if (!string.IsNullOrEmpty(Configs.BaseConfig.EBookHead))
		{
			text7 = text7 + Configs.BaseConfig.EBookHead + "\n\n\n";
		}
		string text8 = string.Empty;
		if (!string.IsNullOrEmpty(Configs.BaseConfig.EBookHead))
		{
			text8 = text8 + Configs.BaseConfig.EBookHead + "\r\n\r\n\r\n";
		}
		string text9 = text8 + "《" + currentNovel.Name + "》\r\n\r\n\r\n";
		string text10 = "";
		string text11 = Config.Templets_indextop + Config.Templets_indexchapter + Config.Templets_indexbottom;
		string text12 = "";
		string text13 = Config.Templets_fulltop + Config.Templets_fullchapter + Config.Templets_fullbottom;
		string text14 = "";
		string text15 = "";
		string text16 = "0";
		string text17 = Config.JieqiArticleConfigs["htmlfile"].ToString();
		ArrayList arrayList = new ArrayList();
		DateTime now = DateTime.Now;
		string text18 = "";
		MySqlDataReader mySqlDataReader3;
		if (delnum > 0)
		{
			text18 = "SELECT * FROM `jieqi_article_chapter` WHERE `articleid` = '" + currentNovel.PutID.ToString() + "' Order By `chapterorder` DESC  limit " + delnum;
			mySqlDataReader3 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, text18, (MySqlParameter[])null);
			while (mySqlDataReader3.Read())
			{
				string text19 = mySqlDataReader3["chapterid"].ToString();
				string text20 = "";
				if (isDeleteTXT)
				{
					text20 = Config.TxtDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString() + "/" + text19.ToString() + ".txt";
					if (File.Exists(text20))
					{
						File.Delete(text20);
					}
				}
				if (isDeleteHTML)
				{
					text20 = Config.HtmlDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString() + "/" + text19.ToString() + ".html";
					if (File.Exists(text20))
					{
						File.Delete(text20);
					}
				}
			}
			text18 = "DELETE FROM `jieqi_article_chapter` WHERE `articleid` = '" + currentNovel.PutID.ToString() + "' Order By `chapterorder` DESC  limit " + delnum;
			MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, text18, (MySqlParameter[])null);
		}
		text18 = "SELECT * FROM `jieqi_article_chapter` WHERE `articleid` = '" + currentNovel.PutID + "' Order By `chapterorder` ASC";
		mySqlDataReader3 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, text18, (MySqlParameter[])null);
		string[] array = null;
		while (mySqlDataReader3.Read())
		{
			string text21 = mySqlDataReader3["lastupdate"].ToString();
			string text22 = FromUnixTimestamp(text21).ToString("yyyy-MM-dd hh:mm:ss");
			int num4 = ((Configs.BaseConfig.CmsVersion == "2.4") ? Convert.ToInt32(mySqlDataReader3["words"]) : Convert.ToInt32(mySqlDataReader3["size"]));
			int num5 = ((Configs.BaseConfig.CmsVersion == "2.4") ? num4 : (num4 / 2));
			string[] array2 = ((!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? new string[10]
			{
				mySqlDataReader3["chaptertype"].ToString(),
				mySqlDataReader3["chaptername"].ToString(),
				mySqlDataReader3["chapterid"].ToString(),
				text22.ToString(),
				num4.ToString(),
				num5.ToString(),
				mySqlDataReader3["chapterorder"].ToString(),
				mySqlDataReader3["chaptertype"].ToString(),
				mySqlDataReader3["postdate"].ToString(),
				mySqlDataReader3["lastupdate"].ToString()
			} : new string[11]
			{
				mySqlDataReader3["chaptertype"].ToString(),
				mySqlDataReader3["chaptername"].ToString(),
				mySqlDataReader3["chapterid"].ToString(),
				text22.ToString(),
				num4.ToString(),
				num5.ToString(),
				mySqlDataReader3["chapterorder"].ToString(),
				mySqlDataReader3["chaptertype"].ToString(),
				mySqlDataReader3["postdate"].ToString(),
				mySqlDataReader3["lastupdate"].ToString(),
				mySqlDataReader3["summary"].ToString()
			});
			array = array2;
			arrayList.Add(array2);
		}
		if (Configs.BaseConfig.OnAntiCollectNum > 0 && array != null)
		{
			string path = Application.StartupPath + "\\Txtdir";
			int num6 = currentNovel.LastChapter.PutID;
			if (Directory.Exists(path))
			{
				string[] array3 = Directory.GetFiles(path);
				for (int i = 0; i < Configs.BaseConfig.OnAntiCollectNum; i++)
				{
					num6++;
					int num7 = new Random().Next(array3.Length - 1);
					int num8 = array3[num7].LastIndexOf("\\");
					ChapterInfo chapterInfo = new ChapterInfo
					{
						PutID = num6,
						ChapterName = array3[num7].Substring(num8 + 1).Replace(".txt", ""),
						ChapterText = File.ReadAllText(array3[num7], FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset))
					};
					string[] array4 = new string[11]
					{
						"0",
						chapterInfo.ChapterName,
						chapterInfo.PutID.ToString(),
						array[3],
						"0",
						"0",
						(int.Parse(array[6]) + 1 + i).ToString(),
						"0",
						array[8],
						array[8],
						null
					};
					if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
					{
						array4[10] = array[10];
					}
					arrayList.Add(array4);
					array3 = DeleteStr(array3, num7);
					bool flag2 = false;
					if (array3.Length < 1)
					{
						flag2 = true;
					}
					createAntiChapter(currentNovel, chapterInfo, flag2);
					if (flag2)
					{
						break;
					}
				}
			}
		}
		mySqlDataReader3.Close();
		for (int j = 0; j < arrayList.Count; j++)
		{
			string[] array5 = null;
			string[] array6 = (string[])arrayList[j];
			if (j >= arrayList.Count - Configs.BaseConfig.IndexAntiCollectNum)
			{
				text15 = array6[1];
				string text23 = "";
				if (text15.Length > 0)
				{
					long num9 = DateTime.Now.Ticks + j;
					Random random = new Random((int)(num9 & 0xFFFFFFFFu) | (int)(num9 >> 32));
					int num10 = random.Next(0, text15.Length);
					int num11 = random.Next(0, text15.Length - 1);
					if (num10 == num11)
					{
						num10 = (num11 + random.Next(0, 1000)) % text15.Length;
					}
					text23 = text15.Substring(0, num10) + text15[num11] + text15.Substring(num10, text15.Length - num10);
				}
				array5 = ((!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? new string[10]
				{
					array6[0],
					text23,
					array6[2],
					array6[3],
					array6[4],
					array6[5],
					array6[6],
					array6[7],
					array6[8],
					array6[9]
				} : new string[11]
				{
					array6[0],
					text23,
					array6[2],
					array6[3],
					array6[4],
					array6[5],
					array6[6],
					array6[7],
					array6[8],
					array6[9],
					array6[10]
				});
			}
			else
			{
				array5 = (string[])arrayList[j];
			}
			int num12 = Convert.ToInt32(array5[0]);
			if (j < arrayList.Count - Configs.BaseConfig.OnAntiCollectNum)
			{
				text2 = text2 + "<itemref idref=\"" + array5[1].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
					.Replace(">", "&gt;") + "\" />";
			}
			if (num12 == 1)
			{
				text = ((!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? (text + "<item id=\"" + array5[1].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
					.Replace(">", "&gt;") + "\" href=\"" + array5[2] + ".txt\" media-type=\"text/html\" content-type=\"volume\" />\n") : ((!(Configs.BaseConfig.CmsVersion == "2.4")) ? (text + "<item chapterid=\"" + array5[2] + "\" siteid=\"0\" articleid=\"" + currentNovel.PutID + "\" articlename=\"" + currentNovel.Name.ToString() + "\" volumeid=\"0\" posterid=\"1\" poster=\"admin\" postdate=\"" + array5[8] + "\" lastupdate=\"" + array5[9] + "\" chaptername=\"" + array5[1].ToString().Replace("&nbsp;", "").Replace("&", "")
					.Replace("<", "")
					.Replace(">", "") + "\" chapterorder=\"" + array5[6] + "\" size=\"" + array5[4] + "\" saleprice=\"0\" salenum=\"0\" totalcost=\"0\" attachment=\"a:0:{}\" summary=\"" + array5[10].ToString().Replace("&nbsp;", "").Replace("&", "")
					.Replace("<", "")
					.Replace(">", "")
					.Replace("\r", "")
					.Replace("\n", "")
					.Replace("\r\n", "") + "\" isimage=\"0\" isvip=\"0\" chaptertype=\"" + array5[7] + "\" power=\"0\" display=\"0\" />\n") : string.Concat(text, "<item chapterid=\"", array5[2], "\" siteid=\"", mySqlDataReader3["siteid"], "\" sourceid=\"", HttpUtility.HtmlEncode(mySqlDataReader3["sourceid"].ToString()), "\" sourcecid=\"", HttpUtility.HtmlEncode(mySqlDataReader3["sourcecid"].ToString()), "\" sourcecorder=\"", HttpUtility.HtmlEncode(mySqlDataReader3["sourcecorder"].ToString()), "\" articleid=\"", currentNovel.PutID.ToString(), "\" articlename=\"", currentNovel.Name.ToString().Replace("&", "&amp;").Replace("'", "&#39;")
					.Replace("\"", "&quot;")
					.Replace("<", "&lt;")
					.Replace(">", "&gt;"), "\" volumeid=\"", HttpUtility.HtmlEncode(mySqlDataReader3["volumeid"].ToString()), "\" posterid=\"", HttpUtility.HtmlEncode(mySqlDataReader3["posterid"].ToString()), "\" poster=\"", HttpUtility.HtmlEncode(mySqlDataReader3["poster"].ToString()), "\" postdate=\"", array5[8], "\" lastupdate=\"", array5[9], "\" chaptername=\"", array5[1].ToString().Replace("&nbsp;", "").Replace("&", "")
					.Replace("<", "")
					.Replace(">", "")
					.Replace("'", "&#39;")
					.Replace("\"", "&quot;"), "\" chapterorder=\"", array5[6], "\" words=\"", array5[4], "\" saleprice=\"", HttpUtility.HtmlEncode(mySqlDataReader3["saleprice"].ToString()), "\" salenum=\"", HttpUtility.HtmlEncode(mySqlDataReader3["salenum"].ToString()), "\" totalcost=\"", HttpUtility.HtmlEncode(mySqlDataReader3["totalcost"].ToString()), "\" attachment=\"", HttpUtility.HtmlEncode(mySqlDataReader3["attachment"].ToString()), "\" summary=\"", array5[10].ToString().Replace("&nbsp;", "").Replace("&", "")
					.Replace("<", "")
					.Replace(">", "")
					.Replace("\r", "")
					.Replace("\n", "")
					.Replace("\r\n", ""), "\" preface=\"", HttpUtility.HtmlEncode(mySqlDataReader3["preface"].ToString()), "\" notice=\"", HttpUtility.HtmlEncode(mySqlDataReader3["notice"].ToString()), "\" foreword=\"", HttpUtility.HtmlEncode(mySqlDataReader3["foreword"].ToString()), "\" isbody=\"", HttpUtility.HtmlEncode(mySqlDataReader3["isbody"].ToString()), "\" isimage=\"", HttpUtility.HtmlEncode(mySqlDataReader3["isimage"].ToString()), "\" isvip=\"", HttpUtility.HtmlEncode(mySqlDataReader3["isvip"].ToString()), "\" pages=\"", HttpUtility.HtmlEncode(mySqlDataReader3["pages"].ToString()), "\" chaptertype=\"", array5[7], "\" power=\"", HttpUtility.HtmlEncode(mySqlDataReader3["power"].ToString()), "\" display=\"", HttpUtility.HtmlEncode(mySqlDataReader3["display"].ToString()), "\"  />\n")));
				if (!isGenIndexHtml)
				{
					continue;
				}
				if (num2 != 1)
				{
					text10 += text11;
					text12 += text13;
				}
				text10 += Config.Templets_indextop;
				text12 += Config.Templets_fulltop;
				if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
				{
					if (array5[1].ToString() == "")
					{
						string text24 = "";
						text24 = Config.Templets_indexvolume.Replace("判断", "").Replace("{?$chapterrows[i].chaptername?}", array5[1].ToString());
						text10 += text24;
						text24 = Config.Templets_fullvolume.Replace("判断", "").Replace("{?$chapterrows[i].chaptername?}", array5[1].ToString());
						text12 += text24;
					}
					else
					{
						string text25 = "";
						text25 = Config.Templets_indexvolume.Replace("判断", "").Replace("{?$chapterrows[i].chaptername?}", array5[1].ToString()).Replace("{?$chapterrows[i].chapterid?}", array5[2].ToString())
							.Replace("{?if $chapterrows[i].isvip == 0?}", "")
							.Replace("{?/if?}", "");
						text10 += text25;
						text25 = Config.Templets_fullvolume.Replace("判断", "").Replace("{?$chapterrows[i].chaptername?}", array5[1].ToString());
						text12 += text25;
					}
					if (j > 0)
					{
						string[] array7 = (string[])Config.Templets_indexchapterlist[">1"];
						if (array7 != null)
						{
							text10 += array7[1];
						}
					}
					if (j < arrayList.Count - 1)
					{
						string[] array7 = (string[])Config.Templets_indexchapterlist["<$i['count']"];
						if (array7 != null)
						{
							text10 += array7[1];
						}
					}
				}
				else
				{
					string[] array7 = (string[])Config.Templets_indexchapterlist["vname"];
					if (array5[1].ToString() == "")
					{
						string text26 = "";
						text26 = Config.Templets_indexvolume.Replace("章节vname", array7[0]).Replace("{?$indexrows[i].vname?}", array5[1].ToString()).Replace("{?$indexrows[i].vid?}", array5[2].ToString());
						text10 += text26;
						text26 = Config.Templets_fullvolume.Replace("章节vname", array7[1]).Replace("{?$indexrows[i].vname?}", array5[1].ToString()).Replace("{?$indexrows[i].vid?}", array5[2].ToString());
						text12 += text26;
					}
					else
					{
						string text27 = "";
						text27 = Config.Templets_indexvolume.Replace("章节vname", array7[1]).Replace("{?$indexrows[i].vname?}", array5[1].ToString()).Replace("{?$indexrows[i].vid?}", array5[2].ToString());
						text10 += text27;
						text27 = Config.Templets_fullvolume.Replace("章节vname", array7[1]).Replace("{?$indexrows[i].vname?}", array5[1].ToString()).Replace("{?$indexrows[i].vid?}", array5[2].ToString());
						text12 += text27;
					}
				}
				num2 = 1;
				continue;
			}
			text15 = array5[1].ToString();
			text16 = array5[2].ToString();
			string text28 = "";
			if (isGenUMD || isGenJar || isGenFullTXT || isGenFullHtml)
			{
				string path2 = Config.TxtDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString() + "/" + array5[2] + ".txt";
				if (File.Exists(path2))
				{
					text28 = File.ReadAllText(path2, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset));
				}
				text28 = text28.Replace("\r", "").Replace("\n\n", "\n").Replace("\n", "<br /><br />");
			}
			if (isGenFullTXT)
			{
				string text29 = text9 + array5[1].ToString() + "\r\n\r\n";
				if (Configs.BaseConfig.TextMarkOfEBook)
				{
					text28 = FormatText.TextMark(text28);
				}
				text9 = text29 + text28.Replace("<br/><br/>", "\r\n") + "\r\n\r\n";
			}
			if (isGenUMD)
			{
				arrayList_.Add(array5[1].ToString());
				if (Configs.BaseConfig.TextMarkOfEBook)
				{
					text28 = FormatText.TextMark(text28);
				}
				arrayList_2.Add(text28.Replace("<br/><br/>", "\n"));
			}
			if (isGenJar)
			{
				if (Configs.BaseConfig.TextMarkOfEBook)
				{
					text28 = FormatText.TextMark(text28);
				}
				text7 += text28;
			}
			if (isGenFullHtml)
			{
				string text30 = Config.Templets_fullchapterloop.Replace("{?$chapters[i].title?}", "<a name=\"" + array5[1].ToString() + "\">" + array5[1].ToString() + "</a>").Replace("{?$chapters[i].content?}", text28);
				text14 += text30;
			}
			if (j < arrayList.Count - Configs.BaseConfig.OnAntiCollectNum)
			{
				text = ((!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? (text + "<item id=\"" + array5[1].ToString().Replace("&", "").Replace("<", "")
					.Replace(">", "") + "\" href=\"" + array5[2] + ".txt\" media-type=\"text/html\" content-type=\"chapter\" />\n") : ((!(Configs.BaseConfig.CmsVersion == "2.4")) ? (text + "<item chapterid=\"" + array5[2] + "\" siteid=\"0\" articleid=\"" + currentNovel.PutID + "\" articlename=\"" + currentNovel.Name.ToString() + "\" volumeid=\"0\" posterid=\"1\" poster=\"admin\" postdate=\"" + array5[8] + "\" lastupdate=\"" + array5[9] + "\" chaptername=\"" + array5[1].ToString().Replace("&nbsp;", "").Replace("&", "")
					.Replace("<", "")
					.Replace(">", "") + "\" chapterorder=\"" + array5[6] + "\" size=\"" + array5[4] + "\" saleprice=\"0\" salenum=\"0\" totalcost=\"0\" attachment=\"a:0:{}\" summary=\"" + array5[10].ToString().Replace("&nbsp;", "").Replace("&", "")
					.Replace("<", "")
					.Replace(">", "")
					.Replace("\r", "")
					.Replace("\n", "")
					.Replace("\r\n", "") + " \" isimage=\"0\" isvip=\"0\" chaptertype=\"" + array5[7] + "\" power=\"0\" display=\"0\" />\n") : (text + "<item chapterid=\"" + array5[2] + "\" siteid=\"0\" articleid=\"" + currentNovel.PutID + "\" articlename=\"" + currentNovel.Name.ToString() + "\" volumeid=\"0\" posterid=\"1\" poster=\"admin\" postdate=\"" + array5[8] + "\" lastupdate=\"" + array5[9] + "\" chaptername=\"" + array5[1].ToString().Replace("&nbsp;", "").Replace("&", "")
					.Replace("<", "")
					.Replace(">", "") + "\" chapterorder=\"" + array5[6] + "\" words=\"" + array5[4] + "\" saleprice=\"0\" salenum=\"0\" totalcost=\"0\" attachment=\"a:0:{}\" summary=\"" + array5[10].ToString().Replace("&nbsp;", "").Replace("&", "")
					.Replace("<", "")
					.Replace(">", "")
					.Replace("\r", "")
					.Replace("\n", "")
					.Replace("\r\n", "") + " \" isimage=\"0\" isvip=\"0\" chaptertype=\"" + array5[7] + "\" power=\"0\" display=\"0\" />\n")));
			}
			if (!isGenIndexHtml)
			{
				continue;
			}
			if (num2 == 1)
			{
				if (double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)
				{
					text11 = Config.Templets_indextop + Config.Templets_indexchapter + Config.Templets_indexbottom;
					text13 = Config.Templets_fulltop + Config.Templets_fullchapter + Config.Templets_fullbottom;
				}
				else if (j == 1)
				{
					text11 = Config.Templets_indextop + Config.Templets_indexchapter;
					text13 = Config.Templets_fullchapter;
				}
				else
				{
					text11 = Config.Templets_indexchapter;
					text13 = Config.Templets_fullchapter;
				}
			}
			if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
			{
				switch (j)
				{
				case 1:
					text11 = Config.Templets_indextop + Config.Templets_indexchapter + Config.Templets_indexbottom;
					text13 = Config.Templets_fulltop + Config.Templets_fullchapter + Config.Templets_fullbottom;
					break;
				case 0:
				{
					string[] array7 = (string[])Config.Templets_indexchapterlist["==1"];
					if (array7 != null)
					{
						text11 = array7[1] + text11;
					}
					break;
				}
				}
				if (j == arrayList.Count - 1)
				{
					string[] array7 = (string[])Config.Templets_indexchapterlist["==$i['count']"];
					if (array7 != null)
					{
						text11 += array7[1];
					}
				}
				if (array5[1].ToString() == "")
				{
					int num13 = Convert.ToInt32(array5[4].ToString()) / 2;
					text11 = text11.Replace("判断", "").Replace("{?$chapterrows[i].chaptername?}", "未知章节名" + array5[2].ToString()).Replace("{?$chapterrows[i].url_chapter?}", array5[2].ToString() + text17)
						.Replace("{?$indexrows[i].cid?}", array5[2].ToString())
						.Replace("{?$chapterrows[i].cid?}", array5[2].ToString())
						.Replace("{?$chapterrows[i].chapterid?}", array5[2].ToString())
						.Replace("{?$chapterrows[i].lastupdate?}", array5[3].ToString())
						.Replace("{?$chapterrows[i].lastupdate|date:'Y-m-d H:i'?}", array5[3].ToString())
						.Replace("{?$chapterrows[i].size_c?}", num13.ToString());
					text13 = text13.Replace("判断", "");
				}
				else
				{
					int num13 = Convert.ToInt32(array5[4].ToString()) / 2;
					text11 = text11.Replace("判断", "").Replace("{?$chapterrows[i].chaptername?}", array5[1].ToString()).Replace("{?$chapterrows[i].url_chapter?}", array5[2].ToString() + text17)
						.Replace("{?$indexrows[i].cid?}", array5[2].ToString())
						.Replace("{?$chapterrows[i].cid?}", array5[2].ToString())
						.Replace("{?$chapterrows[i].chapterid?}", array5[2].ToString())
						.Replace("{?$chapterrows[i].lastupdate?}", array5[3].ToString())
						.Replace("{?$chapterrows[i].lastupdate|date:'Y-m-d H:i'?}", array5[3].ToString())
						.Replace("{?$chapterrows[i].size_c?}", num13.ToString());
					text13 = text13.Replace("判断", "");
				}
				text10 += text11;
				text12 += text13;
			}
			else
			{
				string[] array8 = (string[])Config.Templets_indexchapterlist["cname" + num2];
				if (array8 == null)
				{
					num2 = 1;
					text10 += text11;
					text12 += text13;
					array8 = (string[])Config.Templets_indexchapterlist["cname" + num2];
				}
				if (num2 == 1)
				{
					text11 = Config.Templets_indextop + Config.Templets_indexchapter + Config.Templets_indexbottom;
					text13 = Config.Templets_fulltop + Config.Templets_fullchapter + Config.Templets_fullbottom;
				}
				if (array5[1].ToString() == "")
				{
					text11 = text11.Replace("章节cname" + num2, array8[0]).Replace("{?$indexrows[i].cname" + num2 + "?}", array5[1].ToString()).Replace("{?$indexrows[i].curl" + num2 + "?}", array5[2].ToString() + text17)
						.Replace("{?$indexrows[i].cid" + num2 + "?}", array5[2].ToString());
					text13 = text13.Replace("章节cname" + num2, array8[0]);
				}
				else
				{
					text11 = text11.Replace("章节cname" + num2, array8[1]).Replace("{?$indexrows[i].cname" + num2 + "?}", array5[1].ToString()).Replace("{?$indexrows[i].curl" + num2 + "?}", array5[2].ToString() + text17)
						.Replace("{?$indexrows[i].cid" + num2 + "?}", array5[2].ToString());
					text13 = text13.Replace("章节cname" + num2, array8[1]);
				}
				num2++;
				if (num2 > num || j == arrayList.Count - 1)
				{
					text10 += text11;
					text12 += text13;
					num2 = 1;
				}
			}
		}
		if (isGenIndexHtml)
		{
			if (num2 != 1)
			{
				text10 += text11;
				text12 += text13;
			}
			for (int k = 1; k <= num; k++)
			{
				text10 = text10.Replace("章节cname" + k, "&nbsp;");
			}
			text10 = ((!(double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)) ? ReplaceContents(Config.Templets_indexhtml.Replace("目录循环部分", text10 + Config.Templets_indexbottom), currentNovel, currentNovel.LastChapter).Replace("{?$lastchapter?}", text15).Replace("{?$url_lastchapter?}", text16 + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$permission?}", "暂未授权") : ReplaceContents(Config.Templets_indexhtml.Replace("目录循环部分", text10), currentNovel, currentNovel.LastChapter).Replace("{?$lastchapter?}", text15).Replace("{?$url_lastchapter?}", text16 + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$permission?}", "暂未授权"));
			text10 = text10.Replace("{?$chapters?}", arrayList.Count.ToString()).Replace("{?$lastchapterid?}", currentNovel.LastChapter.PutID.ToString()).Replace("{?$navcode?}", CreateNav(currentNovel.LagerSortID));
			text10 = ((!(Config.JieqiArticleConfigs["dynamicurl"].ToString() == "")) ? text10.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiArticleConfigs["dynamicurl"].ToString()) : text10.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article"));
			if (Configs.BaseConfig.IndexNeighbor > 0 && Config.TempletsIndexHtml.IndexOf("{?$linju?}") > 0)
			{
				if (currentNovel.novelLj == null || currentNovel.novelLj.Length != Configs.BaseConfig.IndexNeighbor)
				{
					currentNovel.novelLj = GetNovelLj(currentNovel, Configs.BaseConfig.IndexNeighbor);
				}
				string text31 = "";
				string text32 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>";
				if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
				{
					text32 = Configs.BaseConfig.TuijianTemplates;
				}
				string[] novelLj = currentNovel.novelLj;
				foreach (string text33 in novelLj)
				{
					string text34 = text33.Split('^')[0];
					string text35 = text33.Split('^')[1];
					string text36 = text33.Split('^')[2];
					string text37 = text31;
					string newValue = Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", text34.ToString()).Replace("{Pinyin/3}", text34.Substring(0, 3).ToString()).Replace("{NovelId}", text35.ToString())
						.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text35) / 1000).ToString());
					string newValue2 = text36.ToString();
					string newValue3 = Config.ImageUrl + "/" + Convert.ToInt32(int.Parse(text35) / 1000) + "/" + text35.ToString() + "/" + text35.ToString() + "s.jpg";
					text31 = text37 + text32.Replace("{NovelUrl}", newValue).Replace("{NovelTitle}", newValue2).Replace("{NovelPic}", newValue3);
				}
				text10 = text10.Replace("{?$linju?}", text31);
			}
			if (Configs.BaseConfig.IndexTuijian > 0 && Config.TempletsIndexHtml.IndexOf("{?$tuijian?}") > 0)
			{
				if (currentNovel.novelTj == null)
				{
					currentNovel.novelTj = GetNovelTj(currentNovel, Configs.BaseConfig.IndexTuijian);
				}
				string path = "";
				string text32 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>";
				if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
				{
					text32 = Configs.BaseConfig.TuijianTemplates;
				}
				int num14 = Configs.BaseConfig.IndexTuijian;
				string[] array9 = currentNovel.novelTj;
				if (currentNovel.novelTj.Length < Configs.BaseConfig.IndexTuijian)
				{
					num14 = currentNovel.novelTj.Length;
				}
				for (int i = 0; i < num14; i++)
				{
					int num7 = new Random().Next(array9.Length - 1);
					string text38 = array9[num7];
					string text39 = text38.Split('^')[0];
					string text40 = text38.Split('^')[1];
					string text41 = text38.Split('^')[2];
					string text37 = path;
					string newValue = Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", text39.ToString()).Replace("{Pinyin/3}", text39.Substring(0, 3).ToString()).Replace("{NovelId}", text40.ToString())
						.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text40) / 1000).ToString());
					string newValue2 = text41.ToString();
					string newValue3 = Config.ImageUrl + "/" + Convert.ToInt32(int.Parse(text40) / 1000) + "/" + text40.ToString() + "/" + text40.ToString() + "s.jpg";
					path = text37 + text32.Replace("{NovelUrl}", newValue).Replace("{NovelTitle}", newValue2).Replace("{NovelPic}", newValue3);
					array9 = DeleteStr(array9, num7);
				}
				text10 = text10.Replace("{?$tuijian?}", path);
			}
			if (Config.TempletsIndexHtml.IndexOf("{?$new9?}") > 0 && Configs.BaseConfig.NewAntiCollectNum > 0)
			{
				string text42 = "";
				string text43 = "";
				int num15 = 0;
				string string_3 = ((Configs.BaseConfig.CmsVersion == "2.4") ? ("SELECT `chapterid`,`chaptername`,`lastupdate`,`words` FROM `jieqi_article_chapter` WHERE `articleid` = '" + currentNovel.PutID.ToString() + "'  And `chaptertype` < '1' Order By `chapterorder` DESC limit 0 , " + Configs.BaseConfig.NewAntiCollectNum) : ("SELECT `chapterid`,`chaptername`,`lastupdate`,`size` FROM `jieqi_article_chapter` WHERE `articleid` = '" + currentNovel.PutID.ToString() + "'  And `chaptertype` < '1' Order By `chapterorder` DESC limit 0 , " + Configs.BaseConfig.NewAntiCollectNum));
				MySqlDataReader mySqlDataReader4 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_3, (MySqlParameter[])null);
				int j = 0;
				string text44 = "";
				while (mySqlDataReader4.Read())
				{
					j++;
					if (j > Configs.BaseConfig.NewAntiCollectNum)
					{
						break;
					}
					string text45 = mySqlDataReader4["chapterid"].ToString();
					string newValue4 = mySqlDataReader4["chaptername"].ToString();
					string text21 = mySqlDataReader4["lastupdate"].ToString();
					string newValue5 = FromUnixTimestamp(text21).ToString("yyyy-MM-dd hh:mm:ss");
					int num16 = ((Configs.BaseConfig.CmsVersion == "2.4") ? int.Parse(mySqlDataReader4["words"].ToString()) : (int.Parse(mySqlDataReader4["size"].ToString()) / 2));
					num15++;
					if (num15 == 1)
					{
						if (double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)
						{
							text42 = Config.Templets_indextop + Config.Templets_indexchapter + Config.Templets_indexbottom;
							text43 = Config.Templets_fulltop + Config.Templets_fullchapter + Config.Templets_fullbottom;
						}
						else if (num2 == 1)
						{
							text42 = Config.Templets_indextop + Config.Templets_indexchapter;
							text43 = Config.Templets_fullchapter;
						}
						else
						{
							text42 = Config.Templets_indexchapter;
							text43 = Config.Templets_fullchapter;
						}
					}
					if (double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)
					{
						string[] array10 = (string[])Config.Templets_indexchapterlist["cname" + num15];
						text44 += text42.Replace("章节cname" + num15, array10[1]).Replace("{?$indexrows[i].cname" + num15 + "?}", newValue4).Replace("{?$indexrows[i].curl" + num15 + "?}", text45 + Config.JieqiArticleConfigs["htmlfile"].ToString())
							.Replace("{?$indexrows[i].cid" + num15 + "?}", text45);
						text43 = text43.Replace("章节cname" + num15, array10[1]).Replace("{?$indexrows[i].cname" + num15 + "?}", newValue4).Replace("{?$indexrows[i].curl" + num15 + "?}", "#" + text45)
							.Replace("{?$indexrows[i].cid" + num15 + "?}", "#" + text45);
						continue;
					}
					string newValue6 = text45.ToString() + Config.JieqiArticleConfigs["htmlfile"].ToString();
					if (Config.JieqiArticleConfigs["fakechapter"].ToString() != "")
					{
						newValue6 = Config.JieqiArticleConfigs["fakechapter"].ToString().Replace("<{$aid}>", currentNovel.PutID.ToString()).Replace("<{$cid}>", text45.ToString())
							.Replace("<{$aid|subdirectory}>", "/" + Convert.ToInt32(currentNovel.PutID / 1000));
					}
					text44 += text42.Replace("判断", "").Replace("{?$chapterrows[i].chaptername?}", newValue4).Replace("{?$chapterrows[i].url_chapter?}", newValue6)
						.Replace("{?$chapterrows[i].chapterid?}", text45.ToString())
						.Replace("{?$chapterrows[i].lastupdate?}", newValue5)
						.Replace("{?$chapterrows[i].lastupdate|date:'Y-m-d H:i'?}", newValue5)
						.Replace("{?$chapterrows[i].size?}", j.ToString())
						.Replace("{?$chapterrows[i].size_c?}", num16.ToString());
					text43 = text43.Replace("判断", "").Replace("{?$chapterrows[i].chaptername?}", newValue4).Replace("{?$chapterrows[i].url_chapter?}", "#" + text45)
						.Replace("{?$chapterrows[i].chapterid?}", "#" + text45)
						.Replace("{?$chapterrows[i].chapterid?}", "#" + text45.ToString())
						.Replace("{?$chapterrows[i].lastupdate?}", newValue5)
						.Replace("{?$chapterrows[i].lastupdate|date:'Y-m-d H:i'?}", newValue5)
						.Replace("{?$chapterrows[i].lastupdate|date:'Y-m-d H:i'?}", newValue5)
						.Replace("{?$chapterrows[i].size?}", j.ToString())
						.Replace("{?$chapterrows[i].size_c?}", num16.ToString())
						.Replace("{?$chapterrows[i].words?}", num16.ToString());
				}
				text10 = text10.Replace("{?$new9?}", text44);
				mySqlDataReader4.Close();
			}
			int num17 = currentNovel.PutID / 1000;
			string text46 = Config.HtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num17.ToString()).Replace("{NovelId}", currentNovel.PutID.ToString()).Replace("{Pinyin/3}", currentNovel.PinYinSan.ToString())
				.Replace("{Pinyin}", currentNovel.PinYin.ToString());
			if (!Directory.Exists(text46))
			{
				Directory.CreateDirectory(text46);
			}
			WriteGeneratedTextFile(text46 + "/index" + Config.JieqiArticleConfigs["htmlfile"].ToString(), text10);
		}
		if (flag && Configs.BaseConfig.StrWapIndexTemplate != "")
		{
			string input = File.ReadAllText(Configs.BaseConfig.StrWapIndexTemplate, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset));
			string strWapHtmlDir = Configs.BaseConfig.StrWapHtmlDir;
			string strWapDomain = Configs.BaseConfig.StrWapDomain;
			Match match = Regex.Match(input, "\\{\\?section name=i loop=\\$articlerows?\\?\\}(.*)\\{\\?/section\\?\\}", RegexOptions.Singleline);
			if (!match.Success)
			{
				throw new ApplicationException("分析目录页模板发生错误");
			}
			string value2 = match.Groups[1].Value;
			input = Regex.Replace(input, "\\{\\?section name=i loop=\\$articlerows\\?\\}([\\S\\s]*)\\{\\?/section\\?\\}", "目录循环部分", RegexOptions.Singleline);
			string text47 = "";
			string string_4 = (Configs.BaseConfig.CmsVersion == "2.4") ? "SELECT `chapterid`,`chaptername`,`lastupdate`,`words` FROM `jieqi_article_chapter` WHERE `articleid`=@articleid And `chaptertype` < '1' Order By `chapterorder` DESC limit 0,@limit" : "SELECT `chapterid`,`chaptername`,`lastupdate`,`size` FROM `jieqi_article_chapter` WHERE `articleid`=@articleid And `chaptertype` < '1' Order By `chapterorder` DESC limit 0,@limit";
			MySqlDataReader mySqlDataReader5 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_4, new MySqlParameter("@articleid", currentNovel.PutID), new MySqlParameter("@limit", Configs.BaseConfig.NewAntiCollectNum));
			int j = 0;
			while (mySqlDataReader5.Read())
			{
				string text48 = mySqlDataReader5["chapterid"].ToString();
				string newValue7 = mySqlDataReader5["chaptername"].ToString();
				text47 += value2.Replace("{?$articlerows[i].chaptername?}", newValue7).Replace("{?$articlerows[i].chapterid?}", text48.ToString()).Replace("{?$articlerows[i].articleid?}", currentNovel.PutID.ToString());
			}
			mySqlDataReader5.Close();
			text47 = input.Replace("目录循环部分", text47).Replace("{?$wap_domain?}", strWapDomain);
			text47 = ReplaceContents(text47, currentNovel, currentNovel.LastChapter).Replace("{?$lastchapter?}", text15).Replace("{?$url_lastchapter?}", text16 + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$permission?}", "暂未授权")
				.Replace("{?$chapters?}", arrayList.Count.ToString())
				.Replace("{?$lastchapterid?}", currentNovel.LastChapter.PutID.ToString())
				.Replace("{?$navcode?}", CreateNav(currentNovel.LagerSortID));
			text47 = ((!(Config.JieqiArticleConfigs["dynamicurl"].ToString() == "")) ? text47.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiArticleConfigs["dynamicurl"].ToString()) : text47.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article"));
			int num18 = currentNovel.PutID / 1000;
			string text49 = strWapHtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num18.ToString()).Replace("{NovelId}", currentNovel.PutID.ToString()).Replace("{Pinyin/3}", currentNovel.PinYinSan.ToString())
				.Replace("{Pinyin}", currentNovel.PinYin.ToString());
			if (!Directory.Exists(text49))
			{
				Directory.CreateDirectory(text49);
			}
			WriteGeneratedTextFile(text49 + "/index" + Config.JieqiArticleConfigs["htmlfile"].ToString(), text47);
		}
		if (isGenOPF)
		{
			string value3 = ((!(double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)) ? (text + "</chapters>\n</package>") : (text + "</manifest>\n" + text2 + "</spine>\n</package>"));
			string path3 = Config.OpfDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString();
			if (!Directory.Exists(path3))
			{
				Directory.CreateDirectory(path3);
			}
			WriteGeneratedTextFile(Config.OpfDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString() + "/index.opf", value3);
		}
		if (isGenFullHtml)
		{
			for (int m = 1; m <= num; m++)
			{
				text12 = text12.Replace("章节cname" + m, "&nbsp;");
			}
			string value4 = ReplaceContents(Config.Templets_fullhtml.Replace("目录循环部分", text12).Replace("章节循环部分", text14), currentNovel, currentNovel.LastChapter);
			string path4 = Config.FullDir + "/" + currentNovel.PutID / 1000;
			if (!Directory.Exists(path4))
			{
				Directory.CreateDirectory(path4);
			}
			WriteGeneratedTextFile(Config.FullDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString() + Config.JieqiArticleConfigs["htmlfile"].ToString(), value4);
		}
		if (isGenFullTXT && double.Parse(Configs.BaseConfig.CmsVersion) >= 1.5)
		{
			if (!string.IsNullOrEmpty(Configs.BaseConfig.EBookFoot))
			{
				text9 = text9 + "\r\n\r\n\r\n" + Configs.BaseConfig.EBookFoot;
			}
			string contents = text9.Replace("<br />", "\r\n");
			string text50 = Config.TxtFullDir + "/" + currentNovel.PutID / 1000;
			if (!Directory.Exists(text50))
			{
				Directory.CreateDirectory(text50);
			}
			string text51 = ".txt";
			if (Config.JieqiArticleConfigs["txtfullfile"] != null)
			{
				text51 = Config.JieqiArticleConfigs["txtfullfile"].ToString();
			}
			string path5 = text50 + "/" + currentNovel.PutID + text51;
			if (File.Exists(path5))
			{
				File.Delete(path5);
			}
			if (Configs.BaseConfig.CmsEncoding == "utf-8")
			{
				ChapterFileWriter.WriteTextAtomic(path5, contents, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			}
			else
			{
				ChapterFileWriter.WriteTextAtomic(path5, contents, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset));
			}
		}
		if (isGenZip)
		{
			string string_5 = Config.HtmlDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString();
			new ZipLib().ZipToFile(string_5, currentNovel.PutID + ".zip");
			string text52 = Config.ZipDir + "/" + currentNovel.PutID / 1000;
			if (!Directory.Exists(text52))
			{
				Directory.CreateDirectory(text52);
			}
			string path6 = text52 + "/" + currentNovel.PutID + ".zip";
			if (File.Exists(path6))
			{
				File.Delete(path6);
			}
			string text53 = ".zip";
			if (Config.JieqiArticleConfigs["zipfile"] != null)
			{
				text53 = ".zip";
			}
			File.Move(currentNovel.PutID + ".zip", Config.ZipDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString() + text53);
		}
		if (isGenUMD)
		{
			if (!string.IsNullOrEmpty(Configs.BaseConfig.EBookFoot))
			{
				arrayList_.Add("结尾");
				arrayList_2.Add(Configs.BaseConfig.EBookFoot);
			}
			string string_6 = DateTime.Now.Year.ToString();
			string string_7 = DateTime.Now.Month.ToString();
			string string_8 = DateTime.Now.Day.ToString();
			UMD_GENEGINE uMD_GENEGINE = new UMD_GENEGINE();
			int int_ = new Random().Next(100000001, 1000000000);
			string strTask = "";
			if (!uMD_GENEGINE.Initialize(currentNovel.Name.ToString(), currentNovel.Author, string_6, string_7, string_8, currentNovel.LagerSort, Config.JieqiDefine["JIEQI_SITE_NAME"].ToString() ?? "", "DIY_GENERATED", "Cover.jpg", int_, Config.UmdDir + "\\" + currentNovel.PutID / 1000 + "\\", ref arrayList_2, ref arrayList_, out var string_9))
			{
				SpiderException.Show("UMD1 初始化生成引擎错误：" + string_9, currentNovel, bool_0: true, strTask);
			}
			else if (!uMD_GENEGINE.Make(ref progressBar_0, out string_9, currentNovel.PutID))
			{
				SpiderException.Show("UMD3 生成文件错误：：" + string_9, currentNovel, bool_0: true, strTask);
			}
		}
		if (isGenJar)
		{
			if (!string.IsNullOrEmpty(Configs.BaseConfig.EBookFoot))
			{
				text7 = text7 + "\n\n\n" + Configs.BaseConfig.EBookFoot;
			}
			string text54 = text7.Replace("\r", "").Replace("<br /><br />", "\n");
			IO.CopyFiles("Jar", Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\Jar", bool_0: true, bool_1: true);
			int length = currentNovel.PutID.ToString().Length;
			byte[] array11 = new byte[6 + length];
			array11[0] = 0;
			array11[1] = 0;
			array11[2] = 0;
			array11[3] = 1;
			array11[4] = 0;
			array11[5] = Convert.ToByte(length);
			char[] array12 = currentNovel.PutID.ToString().ToCharArray();
			for (int n = 0; n < length; n++)
			{
				array11[n + 6] = Convert.ToByte(array12[n]);
			}
			File.WriteAllBytes(Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\/Jar\\data\\library", array11);
			Directory.CreateDirectory(Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\Jar\\data\\" + currentNovel.PutID.ToString());
			byte[] array13 = new byte[10];
			int num19 = text54.Length;
			for (int num20 = 3; num20 >= 0; num20--)
			{
				array13[num20] = Convert.ToByte(num19 % 256);
				num19 /= 256;
			}
			array13[4] = 0;
			array13[5] = 0;
			array13[6] = 19;
			array13[7] = 136;
			array13[8] = 0;
			array13[9] = 0;
			File.WriteAllBytes(Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\Jar\\data\\" + currentNovel.PutID.ToString() + "\\info", array13);
			int num21 = text54.Length / 5000;
			for (int num22 = 0; num22 < num21; num22++)
			{
				ChapterFileWriter.WriteTextAtomic(Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\Jar\\data\\" + currentNovel.PutID.ToString() + "\\/part" + num22.ToString(), text54.Substring(num22 * 5000, 5000), Encoding.UTF8);
			}
			ChapterFileWriter.WriteTextAtomic(Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\Jar\\data\\" + currentNovel.PutID.ToString() + "\\part" + num21.ToString(), text54.Substring(num21 * 5000), Encoding.UTF8);
			string contents2 = "Manifest-Version: 1.0\nMIDlet-Delete-Confirm: Please don't kill me!i will change you live.\nParamVid: null\nMIDlet-1: MBookME, /icon.png, cn.com.mbook.mbookme.MBookMEMIDlet\nParamAud: null\nMicroEdition-Configuration: CLDC-1.0\nCreated-By: 1.5.0_04 (Sun Microsystems Inc.)\nMIDlet-Version: 1.0.0\nMIDlet-Description: " + currentNovel.Name + "\nMIDlet-Name: " + currentNovel.PutID + "\nMIDlet-Vendor: " + Config.JieqiDefine["JIEQI_SITE_NAME"].ToString() + "\nMicroEdition-Profile: MIDP-1.0\nParamTxt: C:\\" + currentNovel.PutID + ".txt\nParamImg: null\n";
			ChapterFileWriter.WriteTextAtomic(Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\Jar\\META-INF\\MANIFEST.MF", contents2, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			FastZip fastZip = new FastZip
			{
				CreateEmptyDirectories = true
			};
			fastZip.CreateZip(Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\" + currentNovel.PutID.ToString() + ".jar", Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\Jar\\", recurse: true, "");
			Directory.Delete(Config.JarDir + "\\" + currentNovel.PutID / 1000 + "\\Jar", recursive: true);
		}
	}

	public string CreateNav(int novelid)
	{
		return "";
	}

	public void CreateNoWapChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, bool pre_next, int preCID, int nextCID, string strChapterName, string string_1, string strVolumeName)
	{
		string newValue = "";
		string newValue2 = "";
		string newValue3 = "";
		if (pre_next)
		{
			string string_2 = (Configs.BaseConfig.CmsVersion == "2.4") ? "SELECT `chaptername`,`chapterorder`,`postdate`,`words` FROM `jieqi_article_chapter` WHERE `chapterid`=@chapterid" : "SELECT `chaptername`,`chapterorder`,`postdate`,`size` FROM `jieqi_article_chapter` WHERE `chapterid`=@chapterid";
			MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, new MySqlParameter("@chapterid", chapterInfo_0.PutID));
			if (mySqlDataReader.Read())
			{
				chapterInfo_0.ChapterName = mySqlDataReader["chaptername"].ToString();
				Convert.ToInt32(mySqlDataReader["chapterorder"]);
				mySqlDataReader["postdate"].ToString();
				newValue3 = ((Configs.BaseConfig.CmsVersion == "2.4") ? mySqlDataReader["words"].ToString() : mySqlDataReader["size"].ToString());
			}
			mySqlDataReader.Close();
			SpiderException.Debug("CreateChapter 读取上一页");
			string string_3 = "SELECT `chapterid`,`chaptername` FROM `jieqi_article_chapter` WHERE `chapterid`<@chapterid AND `articleid`=@articleid AND `chaptertype` = '0' ORDER BY `chapterid` DESC LIMIT 1";
			MySqlDataReader mySqlDataReader2 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_3, new MySqlParameter("@chapterid", chapterInfo_0.PutID), new MySqlParameter("@articleid", novelInfo_0.PutID));
			if (mySqlDataReader2.Read())
			{
				Convert.ToInt32(mySqlDataReader2["chapterid"]);
				newValue = mySqlDataReader2["chaptername"].ToString();
			}
			else
			{
				newValue = "当前已是第一章节";
			}
			mySqlDataReader2.Close();
			SpiderException.Debug("CreateChapter 读取下一页");
			string string_4 = "SELECT `chapterid`,`chaptername` FROM `jieqi_article_chapter` WHERE `chapterid`>@chapterid AND `articleid`=@articleid AND `chaptertype` = '0' ORDER BY `chapterid` ASC LIMIT 1";
			MySqlDataReader mySqlDataReader3 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_4, new MySqlParameter("@chapterid", chapterInfo_0.PutID), new MySqlParameter("@articleid", novelInfo_0.PutID));
			if (mySqlDataReader3.Read())
			{
				Convert.ToInt32(mySqlDataReader3["chapterid"]);
				newValue2 = mySqlDataReader3["chaptername"].ToString();
			}
			else
			{
				newValue2 = "暂无更多更新章节";
			}
			mySqlDataReader3.Close();
		}
		if (!File.Exists(Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + chapterInfo_0.PutID.ToString() + ".txt") && chapterInfo_0.ChapterName == null)
		{
			return;
		}
		SpiderException.Debug("CreateChapter 替换模板");
		string text = Config.TempletsContent.Replace("<{if $authorid > 0}><a href=\"<{$article_dynamic_url}>/userinfo.php?id=<{$authorid}>\" target=\"_blank\"><{$author}></a><{else}><{$author}><{/if}>", "<{$author}>").Replace("<{$author}>", novelInfo_0.Author).Replace("<{$article_title}>", novelInfo_0.Name)
			.Replace("<{$jieqi_title}>", chapterInfo_0.ChapterName)
			.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
			.Replace("<{$sortname}>", novelInfo_0.LagerSort)
			.Replace("<{$jieqi_sitename}>", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString())
			.Replace("<{$jieqi_charset}>", Config.JieqiCharset)
			.Replace("<{$meta_keywords}>", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString())
			.Replace("<{$meta_description}>", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString())
			.Replace("<{$meta_author}>", Config.JieqiAuthor)
			.Replace("<{$meta_copyright}>", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString())
			.Replace("<{$new_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("{?$indexrows[i].vname?}", chapterInfo_0.VolumeName);
		if (Configs.BaseConfig.ChapterNeighbor != 0 && Config.TempletsContent.IndexOf("{?$linju?}") > 0)
		{
			if (novelInfo_0.novelLj == null || novelInfo_0.novelLj.Length != Configs.BaseConfig.ChapterNeighbor)
			{
				novelInfo_0.novelLj = GetNovelLj(novelInfo_0, Configs.BaseConfig.ChapterNeighbor);
			}
			string text2 = "";
			string[] novelLj = novelInfo_0.novelLj;
			foreach (string text3 in novelLj)
			{
				string string_3 = text3.Split('^')[0];
				string text4 = text3.Split('^')[1];
				string text5 = text3.Split('^')[2];
				string text6 = text2;
				string text7 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>\r\n";
				if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
				{
					text7 = Configs.BaseConfig.TuijianTemplates;
				}
				text2 = text6 + text7.Replace("{NovelUrl}", Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", string_3.ToString()).Replace("{Pinyin/3}", string_3.Substring(0, 3).ToString()).Replace("{NovelId}", text4.ToString())
					.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text4) / 1000).ToString())).Replace("{NovelTitle}", text5.ToString());
			}
			text = text.Replace("{?$linju?}", text2);
		}
		else
		{
			text = text.Replace("{?$linju?}", "");
		}
		if (Configs.BaseConfig.LicenseVip && Configs.BaseConfig.ChapterTuijian != 0 && Config.TempletsContent.IndexOf("{?$tuijian?}") > 0)
		{
			if (novelInfo_0.novelTj == null)
			{
				novelInfo_0.novelTj = GetNovelTj(novelInfo_0, Configs.BaseConfig.ChapterTuijian);
			}
			string text8 = "";
			int num = Configs.BaseConfig.ChapterTuijian;
			string[] array = novelInfo_0.novelTj;
			if (novelInfo_0.novelTj.Length < Configs.BaseConfig.ChapterTuijian)
			{
				num = novelInfo_0.novelTj.Length;
			}
			for (int j = 0; j < num; j++)
			{
				int num2 = new Random().Next(array.Length - 1);
				string text9 = array[num2];
				string text10 = text9.Split('^')[0];
				string text11 = text9.Split('^')[1];
				string text12 = text9.Split('^')[2];
				string text6 = text8;
				string text7 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>\r\n";
				if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
				{
					text7 = Configs.BaseConfig.TuijianTemplates;
				}
				text8 = text6 + text7.Replace("{NovelUrl}", Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", text10.ToString()).Replace("{Pinyin/3}", text10.Substring(0, 3).ToString()).Replace("{NovelId}", text11.ToString())
					.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text11) / 1000).ToString())).Replace("{NovelTitle}", text12.ToString());
				array = DeleteStr(array, num2);
			}
			text = text.Replace("{?$tuijian?}", text8);
		}
		else
		{
			text = text.Replace("{?$tuijian?}", "");
		}
		text = text.Replace("{?$url_preview?}", "{?$preview_page?}").Replace("{?$url_previous?}", "{?$preview_page?}").Replace("{?$url_next?}", "{?$next_page?}")
			.Replace("{?$url_index?}", "{?$index_page?}");
		text = (string.IsNullOrEmpty(strChapterName) ? text.Replace("{?$preview_pageName?}", "返回目录") : text.Replace("{?$preview_pageName?}", strChapterName));
		text = (string.IsNullOrEmpty(string_1) ? text.Replace("{?$next_pageName?}", "返回目录") : text.Replace("{?$next_pageName?}", string_1));
		int num3 = novelInfo_0.PutID / 1000;
		string newValue4 = Configs.BaseConfig.PrevFirstHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num3.ToString());
		num3 = novelInfo_0.PutID / 1000;
		string newValue5 = Configs.BaseConfig.NextEndHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num3.ToString());
		string text13 = ((preCID == 0) ? text.Replace("{?$preview_page?}", newValue4).Replace("{?$preview_chapteridd?}", "") : text.Replace("{?$preview_page?}", preCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$preview_chapterid?}", preCID.ToString()).Replace("{?$preview_chapteridd?}", preCID + "/"));
		string text14 = ((nextCID == 0) ? text13.Replace("{?$next_page?}", newValue5).Replace("{?$next_chapteridd?}", "") : text13.Replace("{?$next_page?}", nextCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$next_chapterid?}", nextCID.ToString()).Replace("{?$next_chapteridd?}", nextCID + "/")).Replace("<{$index_page}>", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$url_indexpage?}", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$index_page?}", Configs.BaseConfig.NextEndHtmlUrl)
			.Replace("{?$url_index?}", Configs.BaseConfig.NextEndHtmlUrl)
			.Replace("<{$article_id}>", novelInfo_0.PutID.ToString())
			.Replace("<{$chapter_id}>", chapterInfo_0.PutID.ToString())
			.Replace("<{$dynamic_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("<{$url_bookroom}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/")
			.Replace("<{$url_articleinfo}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/articleinfo.php?id=" + novelInfo_0.PutID)
			.Replace("{?$preChapterName?}", newValue)
			.Replace("{?$nextChapterName?}", newValue2)
			.Replace("{?$navcode?}", CreateNav(novelInfo_0.LagerSortID));
		text14 = ((!(Config.JieqiArticleConfigs["dynamicurl"].ToString() == "")) ? text14.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiArticleConfigs["dynamicurl"].ToString()) : text14.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article"));
		SpiderException.Debug("CreateChapter 文字广告");
		if (Configs.BaseConfig.TextMarkOfHtml)
		{
			chapterInfo_0 = FormatText.TextMark(chapterInfo_0);
		}
		if (chapterInfo_0.ChapterText == null)
		{
			string path = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + chapterInfo_0.PutID.ToString() + ".txt";
			if (File.Exists(path))
			{
				chapterInfo_0.ChapterText = File.ReadAllText(path, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset));
			}
			else
			{
				chapterInfo_0.ChapterText = "本章节为空章节！";
			}
		}
		string text15 = chapterInfo_0.ChapterText;
		string text16 = chapterInfo_0.ChapterText;
		if (!Configs.BaseConfig.Translate)
		{
			text15 = text15.Replace("  ", "&nbsp;&nbsp;");
			text16 = text16.Replace(" ", "");
		}
		string text17 = FormatText.Badwords(text15.Replace("\r", "").Replace("\n\n", "\n").Replace("\n\n", "\n")
			.Replace("\n\n", "\n")
			.Replace("\n", "<br /><br />"));
		if (text17.ToLower().IndexOf("本章节为空章节！") == 0)
		{
			SpiderException.EmptyTXT(novelInfo_0.Name, novelInfo_0.PutID, chapterInfo_0.ChapterName, chapterInfo_0.PutID);
		}
		if ((Configs.BaseConfig.OpenNullChapter && Configs.BaseConfig.NullChapter.Replace("\r\n", "").Trim() != "") || (chapterInfo_0.ChapterText.Length <= Configs.BaseConfig.SizeNullChapter && Configs.BaseConfig.NullChapter.Replace("\r\n", "").Trim() != ""))
		{
			FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset).GetBytes(chapterInfo_0.ChapterText);
			text17 = Configs.BaseConfig.NullChapter.ToString().Replace("{?$articleid|subdirectory?}", Convert.ToString(novelInfo_0.PutID / 1000)).Replace("<{$author}>", novelInfo_0.Author)
				.Replace("<{$article_title}>", novelInfo_0.Name)
				.Replace("<{$jieqi_title}>", chapterInfo_0.ChapterName)
				.Replace("<{$sortname}>", novelInfo_0.LagerSort)
				.Replace("<{$jieqi_sitename}>", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString())
				.Replace("<{$jieqi_charset}>", Config.JieqiCharset)
				.Replace("<{$meta_keywords}>", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString())
				.Replace("<{$meta_description}>", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString())
				.Replace("<{$meta_author}>", Config.JieqiAuthor)
				.Replace("<{$meta_copyright}>", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString())
				.Replace("<{$new_url}>", Config.JieqiDefine["JIEQI_URL"].ToString());
		}
		int length = text16.Length;
		text16 = ((length <= 70) ? text16.Substring(0, length) : text16.Substring(0, 70));
		string string_5 = text14.Replace("{?$jieqi_content?}", text17.Replace("{?postErr(", "<a href='").Replace(")?}", "' >").Replace("{?/postErr?}", "</a>")).Replace("{?$aaabc?}", text16 + "......").Replace("{?$size?}", newValue3)
			.Replace("{?$words?}", newValue3);
		bool isEnableWapGen = Configs.BaseConfig.IsEnableWapGen;
		SpiderException.Debug("CreateChapter 通用替换处理");
		string value = ReplaceContents(string_5, novelInfo_0, chapterInfo_0);
		SpiderException.Debug("CreateChapter 生成文件");
		int num4 = novelInfo_0.PutID / 1000;
		string text18 = Config.HtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num4.ToString()).Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
			.Replace("{Pinyin}", novelInfo_0.PinYin.ToString());
		if (!Directory.Exists(text18))
		{
			Directory.CreateDirectory(text18);
		}
		WriteGeneratedTextFile(text18 + "/" + chapterInfo_0.PutID + Config.JieqiArticleConfigs["htmlfile"].ToString(), value);
	}

	public void CreateOPF(NovelInfo currentNovel)
	{
		string text = "";
		string text2 = "<spine>\n";
		SpiderException.Debug("CreateIndex 生成OPF");
		if (currentNovel.Keyword == null)
		{
			currentNovel.Keyword = currentNovel.Name;
		}
		if (double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)
		{
			object obj = text + "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\n";
			object obj2 = string.Concat(obj, "<package unique-identifier=\"", Config.JieqiDefine["JIEQI_URL"], "/modules/article/-", currentNovel.PutID, "\">\n", "<metadata>\n<dc-metadata>\n<dc:Id>", currentNovel.PutID, "</dc:Id>\n<dc:Title>", currentNovel.Name.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</dc:Title>\n<dc:Creator>", currentNovel.Author.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</dc:Creator>\n<dc:Subject>", currentNovel.Keyword.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</dc:Subject>\n<dc:Description>", currentNovel.Intro.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</dc:Description>\n");
			object obj3 = string.Concat(obj2, "<dc:Publisher>", Config.JieqiDefine["JIEQI_SITE_NAME"], "</dc:Publisher>\n", "<dc:Contributorid>1</dc:Contributorid>\n<dc:Contributor>admin</dc:Contributor>\n");
			object obj4 = string.Concat(obj3, "<dc:Sortid>", currentNovel.LagerSortID, "</dc:Sortid>\n", "<dc:Typeid>0</dc:Typeid>\n<dc:Articletype>0</dc:Articletype>\n<dc:Permission>0</dc:Permission>\n<dc:Firstflag>0</dc:Firstflag>\n<dc:Fullflag>0</dc:Fullflag>\n<dc:Imgflag>0</dc:Imgflag>\n<dc:Power>0</dc:Power>\n<dc:Display>0</dc:Display>\n");
			text = string.Concat(obj4, "<dc:Date>", DateTime.Today, "</dc:Date>\n", "<dc:Type>Text</dc:Type>\n<dc:Format>text</dc:Format>\n<dc:Language>ZH</dc:Language>\n</dc-metadata>\n</metadata>\n<manifest>\n");
		}
		else
		{
			string string_ = "SELECT * FROM `jieqi_article_article` WHERE `articleid`='" + currentNovel.PutID + "'";
			MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			if (mySqlDataReader.Read())
			{
				text3 = mySqlDataReader["imgflag"].ToString();
				text4 = mySqlDataReader["lastchapterid"].ToString();
				text5 = mySqlDataReader["lastchapter"].ToString();
				text6 = mySqlDataReader["lastsummary"].ToString();
			}
			mySqlDataReader.Close();
			object obj = text + "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\n";
			object obj5 = string.Concat(string.Concat(new object[2]
			{
				obj,
				"<package id=\"" + currentNovel.PutID + "\">\n<articleinfo>\n"
			}), "<articleid>", currentNovel.PutID, "</articleid>\n<postdate>", currentNovel.PostDate, "</postdate>\n<lastupdate>", currentNovel.LastupDate, "</lastupdate>\n<infoupdate>", currentNovel.PostDate, "</infoupdate>\n<articlename>", currentNovel.Name.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</articlename>\n<articlecode>", CHz2Py.Convert4Hz2Py(currentNovel.Name), "</articlecode>\n<author>", currentNovel.Author.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</author>\n<keywords>", currentNovel.Keyword.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</keywords>\n<intro>", currentNovel.Intro.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</intro>\n");
			object obj6 = ((Configs.BaseConfig.CmsVersion == "2.4") ? string.Concat(string.Concat(obj5, "<sortid>", currentNovel.LagerSortID, "</sortid>\n"), "<typeid>0</typeid>\n<words>", currentNovel.Size, "</words>\n<firstflag>0</firstflag>\n<fullflag>", currentNovel.Degree, "</fullflag>\n<imgflag>", text3, "</imgflag>\n") : string.Concat(string.Concat(obj5, "<sortid>", currentNovel.LagerSortID, "</sortid>\n"), "<typeid>0</typeid>\n<size>", currentNovel.Size, "</size>\n<firstflag>0</firstflag>\n<fullflag>", currentNovel.Degree, "</fullflag>\n<imgflag>", text3, "</imgflag>\n"));
			text = string.Concat(obj6, "<lastchapterid>", text4, "</lastchapterid>\n<lastchapter>", text5, "</lastchapter>\n<lastsummary>", text6.Replace("\n", "").Replace("\r", "").Replace("\r\n", "")
				.Replace("\n\r", "")
				.Replace("&", "")
				.Replace("<", "")
				.Replace(">", "")
				.Replace("nbsp", ""), "</lastsummary>\n", "</articleinfo>\n<chapters>\n");
		}
		string string_2 = "SELECT * FROM `jieqi_article_chapter` WHERE `articleid` = '" + currentNovel.PutID + "' Order By `chapterorder` ASC";
		MySqlDataReader mySqlDataReader2 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
		ArrayList arrayList = new ArrayList();
		while (mySqlDataReader2.Read())
		{
			string text7 = mySqlDataReader2["lastupdate"].ToString();
			string text8 = FromUnixTimestamp(text7).ToString("yyyy-MM-dd hh:mm:ss");
			int num = ((Configs.BaseConfig.CmsVersion == "2.4") ? Convert.ToInt32(mySqlDataReader2["words"]) : Convert.ToInt32(mySqlDataReader2["size"]));
			int num2 = num / 2;
			string[] array = null;
			array = ((!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? new string[10]
			{
				mySqlDataReader2["chaptertype"].ToString(),
				mySqlDataReader2["chaptername"].ToString(),
				mySqlDataReader2["chapterid"].ToString(),
				text8.ToString(),
				num.ToString(),
				num2.ToString(),
				mySqlDataReader2["chapterorder"].ToString(),
				mySqlDataReader2["chaptertype"].ToString(),
				mySqlDataReader2["postdate"].ToString(),
				mySqlDataReader2["lastupdate"].ToString()
			} : new string[11]
			{
				mySqlDataReader2["chaptertype"].ToString(),
				mySqlDataReader2["chaptername"].ToString(),
				mySqlDataReader2["chapterid"].ToString(),
				text8.ToString(),
				num.ToString(),
				num2.ToString(),
				mySqlDataReader2["chapterorder"].ToString(),
				mySqlDataReader2["chaptertype"].ToString(),
				mySqlDataReader2["postdate"].ToString(),
				mySqlDataReader2["lastupdate"].ToString(),
				mySqlDataReader2["summary"].ToString()
			});
			arrayList.Add(array);
		}
		mySqlDataReader2.Close();
		for (int i = 0; i < arrayList.Count; i++)
		{
			string[] array2 = null;
			string[] array3 = (string[])arrayList[i];
			array2 = (string[])arrayList[i];
			int num3 = Convert.ToInt32(array2[0]);
			if (i < arrayList.Count - Configs.BaseConfig.OnAntiCollectNum)
			{
				text2 = text2 + "<itemref idref=\"" + array2[1].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
					.Replace(">", "&gt;") + "\" />\n";
			}
			text = ((num3 != 1) ? ((!(double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)) ? ((Configs.BaseConfig.CmsVersion == "2.4") ? (text + "<item chapterid=\"" + array2[2] + "\" siteid=\"0\" articleid=\"" + currentNovel.PutID + "\" articlename=\"" + currentNovel.Name.ToString() + "\" volumeid=\"0\" posterid=\"1\" poster=\"admin\" postdate=\"" + array2[8] + "\" lastupdate=\"" + array2[9] + "\" chaptername=\"" + array2[1].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
				.Replace(">", "&gt;") + "\" chapterorder=\"" + array2[6] + "\" words=\"" + array2[4] + "\" saleprice=\"0\" salenum=\"0\" totalcost=\"0\" attachment=\"a:0:{}\" summary=\"" + array2[10].ToString().Replace("&", "").Replace("<", "")
				.Replace(">", "")
				.Replace("\r", "")
				.Replace("\n", "")
				.Replace("\r\n", "") + "\" isimage=\"0\" isvip=\"0\" chaptertype=\"" + array2[7] + "\" power=\"0\" display=\"0\" />\n") : (text + "<item chapterid=\"" + array2[2] + "\" siteid=\"0\" articleid=\"" + currentNovel.PutID + "\" articlename=\"" + currentNovel.Name.ToString() + "\" volumeid=\"0\" posterid=\"1\" poster=\"admin\" postdate=\"" + array2[8] + "\" lastupdate=\"" + array2[9] + "\" chaptername=\"" + array2[1].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
				.Replace(">", "&gt;") + "\" chapterorder=\"" + array2[6] + "\" size=\"" + array2[4] + "\" saleprice=\"0\" salenum=\"0\" totalcost=\"0\" attachment=\"a:0:{}\" summary=\"" + array2[10].ToString().Replace("&", "").Replace("<", "")
				.Replace(">", "")
				.Replace("\r", "")
				.Replace("\n", "")
				.Replace("\r\n", "") + "\" isimage=\"0\" isvip=\"0\" chaptertype=\"" + array2[7] + "\" power=\"0\" display=\"0\" />\n")) : (text + "<item id=\"" + array2[1].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
				.Replace(">", "&gt;") + "\" href=\"" + array2[2] + ".txt\" media-type=\"text/html\" content-type=\"chapter\" />\n")) : ((!(double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)) ? ((Configs.BaseConfig.CmsVersion == "2.4") ? (text + "<item chapterid=\"" + array2[2] + "\" siteid=\"0\" articleid=\"" + currentNovel.PutID + "\" articlename=\"" + currentNovel.Name.ToString() + "\" volumeid=\"0\" posterid=\"1\" poster=\"admin\" postdate=\"" + array2[8] + "\" lastupdate=\"" + array2[9] + "\" chaptername=\"" + array2[1].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
				.Replace(">", "&gt;") + "\" chapterorder=\"" + array2[6] + "\" words=\"" + array2[4] + "\" saleprice=\"0\" salenum=\"0\" totalcost=\"0\" attachment=\"a:0:{}\" summary=\"" + array2[10].ToString().Replace("&", "").Replace("<", "")
				.Replace(">", "")
				.Replace("\r", "")
				.Replace("\n", "")
				.Replace("\r\n", "") + "\" isimage=\"0\" isvip=\"0\" chaptertype=\"" + array2[7] + "\" power=\"0\" display=\"0\" />\n") : (text + "<item chapterid=\"" + array2[2] + "\" siteid=\"0\" articleid=\"" + currentNovel.PutID + "\" articlename=\"" + currentNovel.Name.ToString() + "\" volumeid=\"0\" posterid=\"1\" poster=\"admin\" postdate=\"" + array2[8] + "\" lastupdate=\"" + array2[9] + "\" chaptername=\"" + array2[1].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
				.Replace(">", "&gt;") + "\" chapterorder=\"" + array2[6] + "\" size=\"" + array2[4] + "\" saleprice=\"0\" salenum=\"0\" totalcost=\"0\" attachment=\"a:0:{}\" summary=\"" + array2[10].ToString().Replace("&", "").Replace("<", "")
				.Replace(">", "")
				.Replace("\r", "")
				.Replace("\n", "")
				.Replace("\r\n", "") + "\" isimage=\"0\" isvip=\"0\" chaptertype=\"" + array2[7] + "\" power=\"0\" display=\"0\" />\n")) : (text + "<item id=\"" + array2[1].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
				.Replace(">", "&gt;") + "\" href=\"" + array2[2] + ".txt\" media-type=\"text/html\" content-type=\"volume\" />\n")));
		}
		string value2 = ((!(double.Parse(Configs.BaseConfig.CmsVersion) < 1.8)) ? (text + "</chapters>\n</package>") : (text + "</manifest>\n" + text2 + "</spine>\n</package>"));
		string path = Config.OpfDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString();
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		WriteGeneratedTextFile(Config.OpfDir + "/" + currentNovel.PutID / 1000 + "/" + currentNovel.PutID.ToString() + "/index.opf", value2);
	}

	public void CreateSingleChapter(NovelInfo novelInfo_0)
	{
		createChapter(novelInfo_0, novelInfo_0.LastChapter, bool_0: false);
	}

	public void CreateSingleChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, bool pre_next, int preCID, int nextCID, string strChapterName, string string_1, string strVolumeName)
	{
		string newValue = "";
		string newValue2 = "";
		string newValue3 = "";
		if (pre_next)
		{
			string string_2 = ((Configs.BaseConfig.CmsVersion == "2.4") ? ("SELECT `chaptername`,`chapterorder`,`postdate`,`words` FROM `jieqi_article_chapter` WHERE `chapterid`='" + chapterInfo_0.PutID + "'") : ("SELECT `chaptername`,`chapterorder`,`postdate`,`size` FROM `jieqi_article_chapter` WHERE `chapterid`='" + chapterInfo_0.PutID + "'"));
			MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
			if (mySqlDataReader.Read())
			{
				chapterInfo_0.ChapterName = mySqlDataReader["chaptername"].ToString();
				Convert.ToInt32(mySqlDataReader["chapterorder"]);
				mySqlDataReader["postdate"].ToString();
				newValue3 = ((Configs.BaseConfig.CmsVersion == "2.4") ? mySqlDataReader["words"].ToString() : mySqlDataReader["size"].ToString());
			}
			mySqlDataReader.Close();
			SpiderException.Debug("CreateChapter 读取上一页");
			string string_3 = "SELECT `chapterid`,`chaptername` FROM `jieqi_article_chapter` WHERE `chapterid`<'" + chapterInfo_0.PutID + "' AND `articleid`='" + novelInfo_0.PutID.ToString() + "' AND `chaptertype` = '0' ORDER BY `chapterid` DESC LIMIT 1";
			MySqlDataReader mySqlDataReader2 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_3, (MySqlParameter[])null);
			if (mySqlDataReader2.Read())
			{
				Convert.ToInt32(mySqlDataReader2["chapterid"]);
				newValue = mySqlDataReader2["chaptername"].ToString();
			}
			else
			{
				newValue = "当前已是第一章节";
			}
			mySqlDataReader2.Close();
			SpiderException.Debug("CreateChapter 读取下一页");
			string string_4 = "SELECT `chapterid`,`chaptername` FROM `jieqi_article_chapter` WHERE `chapterid`>'" + chapterInfo_0.PutID + "' AND `articleid`='" + novelInfo_0.PutID.ToString() + "' AND `chaptertype` = '0' ORDER BY `chapterid` ASC LIMIT 1";
			MySqlDataReader mySqlDataReader3 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_4, (MySqlParameter[])null);
			if (mySqlDataReader3.Read())
			{
				Convert.ToInt32(mySqlDataReader3["chapterid"]);
				newValue2 = mySqlDataReader3["chaptername"].ToString();
			}
			else
			{
				newValue2 = "暂无更多更新章节";
			}
			mySqlDataReader3.Close();
		}
		if (!File.Exists(Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + chapterInfo_0.PutID.ToString() + ".txt") && chapterInfo_0.ChapterName == null)
		{
			return;
		}
		SpiderException.Debug("CreateChapter 替换模板");
		string text = Config.TempletsContent.Replace("<{if $authorid > 0}><a href=\"<{$article_dynamic_url}>/userinfo.php?id=<{$authorid}>\" target=\"_blank\"><{$author}></a><{else}><{$author}><{/if}>", "<{$author}>").Replace("<{$author}>", novelInfo_0.Author).Replace("<{$article_title}>", novelInfo_0.Name)
			.Replace("<{$jieqi_title}>", chapterInfo_0.ChapterName)
			.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
			.Replace("<{$sortname}>", novelInfo_0.LagerSort)
			.Replace("<{$jieqi_sitename}>", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString())
			.Replace("<{$jieqi_charset}>", Config.JieqiCharset)
			.Replace("<{$meta_keywords}>", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString())
			.Replace("<{$meta_description}>", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString())
			.Replace("<{$meta_author}>", Config.JieqiAuthor)
			.Replace("<{$meta_copyright}>", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString())
			.Replace("<{$new_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("{?$indexrows[i].vname?}", chapterInfo_0.VolumeName)
			.Replace("<{$articlename}>", novelInfo_0.Name)
			.Replace("<{$sort}>", novelInfo_0.LagerSort)
			.Replace("{?$articlesubdir?}", "/" + Convert.ToString(novelInfo_0.PutID / 1000));
		if (Configs.BaseConfig.ChapterNeighbor != 0 && Config.TempletsContent.IndexOf("{?$linju?}") > 0)
		{
			if (novelInfo_0.novelLj == null || novelInfo_0.novelLj.Length != Configs.BaseConfig.ChapterNeighbor)
			{
				novelInfo_0.novelLj = GetNovelLj(novelInfo_0, Configs.BaseConfig.ChapterNeighbor);
			}
			string text2 = "";
			string[] novelLj = novelInfo_0.novelLj;
			foreach (string text3 in novelLj)
			{
				string string_3 = text3.Split('^')[0];
				string text4 = text3.Split('^')[1];
				string text5 = text3.Split('^')[2];
				string text6 = text2;
				string text7 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>\r\n";
				if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
				{
					text7 = Configs.BaseConfig.TuijianTemplates;
				}
				text2 = text6 + text7.Replace("{NovelUrl}", Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", string_3.ToString()).Replace("{Pinyin/3}", string_3.Substring(0, 3).ToString()).Replace("{NovelId}", text4.ToString())
					.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text4) / 1000).ToString())).Replace("{NovelTitle}", text5.ToString());
			}
			text = text.Replace("{?$linju?}", text2);
		}
		else
		{
			text = text.Replace("{?$linju?}", "");
		}
		if (Configs.BaseConfig.LicenseVip && Configs.BaseConfig.ChapterTuijian != 0 && Config.TempletsContent.IndexOf("{?$tuijian?}") > 0)
		{
			if (novelInfo_0.novelTj == null)
			{
				novelInfo_0.novelTj = GetNovelTj(novelInfo_0, Configs.BaseConfig.ChapterTuijian);
			}
			string text8 = "";
			int num = Configs.BaseConfig.ChapterTuijian;
			string[] array = novelInfo_0.novelTj;
			if (novelInfo_0.novelTj.Length < Configs.BaseConfig.ChapterTuijian)
			{
				num = novelInfo_0.novelTj.Length;
			}
			for (int j = 0; j < num; j++)
			{
				int num2 = new Random().Next(array.Length - 1);
				string text9 = array[num2];
				string text10 = text9.Split('^')[0];
				string text11 = text9.Split('^')[1];
				string text12 = text9.Split('^')[2];
				string text6 = text8;
				string text7 = "<a href=\"{NovelUrl}\" target=\"_blank\">{NovelTitle}</a>\r\n";
				if (Configs.BaseConfig.TuijianTemplates.Trim() != string.Empty)
				{
					text7 = Configs.BaseConfig.TuijianTemplates;
				}
				text8 = text6 + text7.Replace("{NovelUrl}", Configs.BaseConfig.InternalLinkUrl.Replace("{Pinyin}", text10.ToString()).Replace("{Pinyin/3}", text10.Substring(0, 3).ToString()).Replace("{NovelId}", text11.ToString())
					.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text11) / 1000).ToString())).Replace("{NovelTitle}", text12.ToString());
				array = DeleteStr(array, num2);
			}
			text = text.Replace("{?$tuijian?}", text8);
		}
		else
		{
			text = text.Replace("{?$tuijian?}", "");
		}
		text = text.Replace("{?$url_preview?}", "{?$preview_page?}").Replace("{?$url_previous?}", "{?$preview_page?}").Replace("{?$url_next?}", "{?$next_page?}")
			.Replace("{?$url_index?}", "{?$index_page?}");
		text = (string.IsNullOrEmpty(strChapterName) ? text.Replace("{?$preview_pageName?}", "返回目录") : text.Replace("{?$preview_pageName?}", strChapterName));
		text = (string.IsNullOrEmpty(string_1) ? text.Replace("{?$next_pageName?}", "返回目录") : text.Replace("{?$next_pageName?}", string_1));
		int num3 = novelInfo_0.PutID / 1000;
		string newValue4 = Configs.BaseConfig.PrevFirstHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num3.ToString());
		num3 = novelInfo_0.PutID / 1000;
		string newValue5 = Configs.BaseConfig.NextEndHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num3.ToString());
		string text13 = ((preCID == 0) ? text.Replace("{?$preview_page?}", newValue4).Replace("{?$preview_chapteridd?}", "") : text.Replace("{?$preview_page?}", preCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$preview_chapterid?}", preCID.ToString()).Replace("{?$preview_chapteridd?}", preCID + "/"));
		string text14 = ((nextCID == 0) ? text13.Replace("{?$next_page?}", newValue5).Replace("{?$next_chapteridd?}", "") : text13.Replace("{?$next_page?}", nextCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$next_chapterid?}", nextCID.ToString()).Replace("{?$next_chapteridd?}", nextCID + "/")).Replace("<{$index_page}>", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$url_indexpage?}", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$index_page?}", Configs.BaseConfig.NextEndHtmlUrl)
			.Replace("{?$url_index?}", Configs.BaseConfig.NextEndHtmlUrl)
			.Replace("<{$article_id}>", novelInfo_0.PutID.ToString())
			.Replace("<{$chapter_id}>", chapterInfo_0.PutID.ToString())
			.Replace("<{$dynamic_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("<{$url_bookroom}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/")
			.Replace("<{$url_articleinfo}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/articleinfo.php?id=" + novelInfo_0.PutID)
			.Replace("{?$preChapterName?}", newValue)
			.Replace("{?$nextChapterName?}", newValue2)
			.Replace("{?$navcode?}", CreateNav(novelInfo_0.LagerSortID));
		text14 = ((!(Config.JieqiArticleConfigs["dynamicurl"].ToString() == "")) ? text14.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiArticleConfigs["dynamicurl"].ToString()) : text14.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article"));
		SpiderException.Debug("CreateChapter 文字广告");
		if (Configs.BaseConfig.TextMarkOfHtml)
		{
			chapterInfo_0 = FormatText.TextMark(chapterInfo_0);
		}
		if (chapterInfo_0.ChapterText == null)
		{
			string path = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + chapterInfo_0.PutID.ToString() + ".txt";
			if (File.Exists(path))
			{
				chapterInfo_0.ChapterText = File.ReadAllText(path, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset));
			}
			else
			{
				chapterInfo_0.ChapterText = "本章节为空章节！";
			}
		}
		string text15 = chapterInfo_0.ChapterText.Replace("%>_<%", "").Replace("%\\s*>", "").Replace("<\\s*%", "");
		string text16 = chapterInfo_0.ChapterText;
		if (!Configs.BaseConfig.Translate)
		{
			text15 = text15.Replace("  ", "&nbsp;&nbsp;");
			text16 = text16.Replace(" ", "");
		}
		string text17 = FormatText.Badwords(text15.Replace("\r", "").Replace("\n\n", "\n").Replace("\n\n", "\n")
			.Replace("\n\n", "\n")
			.Replace("\n", "<br /><br />"));
		if (text17.ToLower().IndexOf("本章节为空章节！") == 0)
		{
			SpiderException.EmptyTXT(novelInfo_0.Name, novelInfo_0.PutID, chapterInfo_0.ChapterName, chapterInfo_0.PutID);
		}
		if ((Configs.BaseConfig.OpenNullChapter && Configs.BaseConfig.NullChapter.Replace("\r\n", "").Trim() != "") || (chapterInfo_0.ChapterText.Length <= Configs.BaseConfig.SizeNullChapter && Configs.BaseConfig.NullChapter.Replace("\r\n", "").Trim() != ""))
		{
			FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset).GetBytes(chapterInfo_0.ChapterText);
			text17 = Configs.BaseConfig.NullChapter.ToString().Replace("{?$articleid|subdirectory?}", Convert.ToString(novelInfo_0.PutID / 1000)).Replace("<{$author}>", novelInfo_0.Author)
				.Replace("<{$article_title}>", novelInfo_0.Name)
				.Replace("<{$articlename}>", novelInfo_0.Name)
				.Replace("<{$jieqi_title}>", chapterInfo_0.ChapterName)
				.Replace("<{$sortname}>", novelInfo_0.LagerSort)
				.Replace("<{$sort}>", novelInfo_0.LagerSort)
				.Replace("<{$jieqi_sitename}>", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString())
				.Replace("<{$jieqi_charset}>", Config.JieqiCharset)
				.Replace("<{$meta_keywords}>", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString())
				.Replace("<{$meta_description}>", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString())
				.Replace("<{$meta_author}>", Config.JieqiAuthor)
				.Replace("<{$meta_copyright}>", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString())
				.Replace("<{$new_url}>", Config.JieqiDefine["JIEQI_URL"].ToString());
		}
		int length = text16.Length;
		text16 = ((length <= 70) ? text16.Substring(0, length) : text16.Substring(0, 70));
		string string_5 = text14.Replace("{?$jieqi_content?}", text17.Replace("{?postErr(", "<a href='").Replace(")?}", "' >").Replace("{?/postErr?}", "</a>")).Replace("{?$aaabc?}", text16 + "......").Replace("{?$size?}", newValue3)
			.Replace("{?$words?}", newValue3);
		bool isEnableWapGen = Configs.BaseConfig.IsEnableWapGen;
		string text18 = "";
		if (isEnableWapGen)
		{
			string strWapChapterTemplate = Configs.BaseConfig.StrWapChapterTemplate;
			string strWapHtmlDir = Configs.BaseConfig.StrWapHtmlDir;
			string strWapDomain = Configs.BaseConfig.StrWapDomain;
			text18 = File.ReadAllText(strWapChapterTemplate, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset)).Replace("{?$wap_domain?}", strWapDomain).Replace("{?$jieqi_content?}", text17.Replace("{?postErr(", "<a href='").Replace(")?}", "' >").Replace("{?/postErr?}", "</a>"))
				.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
				.Replace("{?$aaabc?}", text16 + "......")
				.Replace("{?$size?}", newValue3)
				.Replace("{?$words?}", newValue3);
			text18 = ((preCID == 0) ? text18.Replace("{?$preview_page?}", newValue4).Replace("{?$preview_chapteridd?}", "") : text18.Replace("{?$preview_page?}", preCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$preview_chapterid?}", preCID.ToString()).Replace("{?$preview_chapteridd?}", preCID + "/"));
			text18 = ((nextCID == 0) ? text18.Replace("{?$next_page?}", newValue5).Replace("{?$next_chapteridd?}", "") : text18.Replace("{?$next_page?}", nextCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$next_chapterid?}", nextCID.ToString()).Replace("{?$next_chapteridd?}", nextCID + "/"));
			text18 = ReplaceContents(text18, novelInfo_0, chapterInfo_0);
			int num4 = novelInfo_0.PutID / 1000;
			string text19 = strWapHtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num4.ToString()).Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
				.Replace("{Pinyin}", novelInfo_0.PinYin.ToString());
			if (!Directory.Exists(text19))
			{
				Directory.CreateDirectory(text19);
			}
			WriteGeneratedTextFile(text19 + "/" + chapterInfo_0.PutID + Config.JieqiArticleConfigs["htmlfile"].ToString(), text18);
		}
		SpiderException.Debug("CreateChapter 通用替换处理");
		string value = ReplaceContents(string_5, novelInfo_0, chapterInfo_0);
		SpiderException.Debug("CreateChapter 生成文件");
		int num5 = novelInfo_0.PutID / 1000;
		string text20 = Config.HtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num5.ToString()).Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
			.Replace("{Pinyin}", novelInfo_0.PinYin.ToString());
		if (!Directory.Exists(text20))
		{
			Directory.CreateDirectory(text20);
		}
		WriteGeneratedTextFile(text20 + "/" + chapterInfo_0.PutID + Config.JieqiArticleConfigs["htmlfile"].ToString(), value);
	}

	public void CreateTagTable()
	{
		try
		{
			string string_ = ((Configs.BaseConfig.CmsVersion == "2.4") ? "CREATE TABLE IF NOT EXISTS `jieqi_article_tag` (`tagid` int(10)  primary key not  null  auto_increment,`articleid` int(10) NOT NULL,`tagname` varchar(20) NOT NULL, `length` int(20) NOT NULL, `hits` int(50) DEFAULT 0 ) ENGINE = MyISAM DEFAULT CHARSET = utf8; " : "CREATE TABLE IF NOT EXISTS `jieqi_article_tag` (`tagid` int(10)  primary key not  null  auto_increment,`articleid` int(10) NOT NULL,`tagname` varchar(20) NOT NULL, `length` int(20) NOT NULL, `hits` int(50) DEFAULT 0 ) ENGINE = MyISAM DEFAULT CHARSET = gbk; ");
			MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
			if (MessageBox.Show("标签表初始化完成！，是否为数据库中小说生成标签？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
			{
				return;
			}
			string string_2 = "select `articleid`,`articlename` from `jieqi_article_article`";
			MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
			while (mySqlDataReader.Read())
			{
				string text = mySqlDataReader["articleid"].ToString();
				string text2 = mySqlDataReader["articlename"].ToString();
				Segment.Init();
				StringBuilder stringBuilder = new StringBuilder();
				foreach (WordInfo item in new Segment().DoSegment(text2))
				{
					if (item != null && item.Word.Length > 1)
					{
						if ((keywordlist == null || keywordlist.Count == 0) && !keywordlist.Contains(item.Word))
						{
							keywordlist.Add(item.Word);
						}
						string string_3 = "INSERT INTO `jieqi_article_tag` (`tagid`, `tagname`,`length`,`hits`) VALUES (NULL," + text + ",'" + item.Word + "'," + item.Word.Length + ",0)";
						MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_3, (MySqlParameter[])null);
					}
				}
			}
			mySqlDataReader.Close();
			MessageBox.Show("标签数据初始化完成");
		}
		catch (Exception)
		{
			MessageBox.Show("标签数据初始化失败！");
		}
	}

	public void CreateWapChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, bool pre_next, int preCID, int nextCID, string strChapterName, string string_1, string strVolumeName)
	{
		string newValue = "";
		string newValue2 = "";
		string newValue3 = "";
		if (pre_next)
		{
			string string_2 = ((Configs.BaseConfig.CmsVersion == "2.4") ? ("SELECT `chaptername`,`chapterorder`,`postdate`,`words` FROM `jieqi_article_chapter` WHERE `chapterid`='" + chapterInfo_0.PutID + "'") : ("SELECT `chaptername`,`chapterorder`,`postdate`,`size` FROM `jieqi_article_chapter` WHERE `chapterid`='" + chapterInfo_0.PutID + "'"));
			MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
			if (mySqlDataReader.Read())
			{
				chapterInfo_0.ChapterName = mySqlDataReader["chaptername"].ToString();
				Convert.ToInt32(mySqlDataReader["chapterorder"]);
				mySqlDataReader["postdate"].ToString();
				newValue3 = ((Configs.BaseConfig.CmsVersion == "2.4") ? mySqlDataReader["words"].ToString() : mySqlDataReader["size"].ToString());
			}
			mySqlDataReader.Close();
			SpiderException.Debug("CreateChapter 读取上一页");
			string string_3 = "SELECT `chapterid`,`chaptername` FROM `jieqi_article_chapter` WHERE `chapterid`<'" + chapterInfo_0.PutID + "' AND `articleid`='" + novelInfo_0.PutID.ToString() + "' And `chaptertype` = '0' ORDER BY `chapterid` DESC LIMIT 1";
			MySqlDataReader mySqlDataReader2 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_3, (MySqlParameter[])null);
			if (mySqlDataReader2.Read())
			{
				Convert.ToInt32(mySqlDataReader2["chapterid"]);
				newValue = mySqlDataReader2["chaptername"].ToString();
			}
			else
			{
				newValue = "当前已是第一章节";
			}
			mySqlDataReader2.Close();
			SpiderException.Debug("CreateChapter 读取下一页");
			string string_4 = "SELECT `chapterid`,`chaptername` FROM `jieqi_article_chapter` WHERE `chapterid`>'" + chapterInfo_0.PutID + "' AND `articleid`='" + novelInfo_0.PutID.ToString() + "' AND `chaptertype` = '0' ORDER BY `chapterid` ASC LIMIT 1";
			MySqlDataReader mySqlDataReader3 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_4, (MySqlParameter[])null);
			if (mySqlDataReader3.Read())
			{
				Convert.ToInt32(mySqlDataReader3["chapterid"]);
				newValue2 = mySqlDataReader3["chaptername"].ToString();
			}
			else
			{
				newValue2 = "暂无更多更新章节";
			}
			mySqlDataReader3.Close();
		}
		if (!File.Exists(Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + chapterInfo_0.PutID.ToString() + ".txt") && chapterInfo_0.ChapterName == null)
		{
			return;
		}
		SpiderException.Debug("CreateChapter 替换模板");
		string text = Config.TempletsContent.Replace("<{if $authorid > 0}><a href=\"<{$article_dynamic_url}>/userinfo.php?id=<{$authorid}>\" target=\"_blank\"><{$author}></a><{else}><{$author}><{/if}>", "<{$author}>").Replace("<{$author}>", novelInfo_0.Author).Replace("<{$article_title}>", novelInfo_0.Name)
			.Replace("<{$jieqi_title}>", chapterInfo_0.ChapterName)
			.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
			.Replace("<{$sortname}>", novelInfo_0.LagerSort)
			.Replace("<{$jieqi_sitename}>", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString())
			.Replace("<{$jieqi_charset}>", Config.JieqiCharset)
			.Replace("<{$meta_keywords}>", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString())
			.Replace("<{$meta_description}>", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString())
			.Replace("<{$meta_author}>", Config.JieqiAuthor)
			.Replace("<{$meta_copyright}>", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString())
			.Replace("<{$new_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("{?$indexrows[i].vname?}", chapterInfo_0.VolumeName)
			.Replace("{?$url_preview?}", "{?$preview_page?}")
			.Replace("{?$url_previous?}", "{?$preview_page?}")
			.Replace("{?$url_next?}", "{?$next_page?}")
			.Replace("{?$url_index?}", "{?$index_page?}");
		text = (string.IsNullOrEmpty(strChapterName) ? text.Replace("{?$preview_pageName?}", "返回目录") : text.Replace("{?$preview_pageName?}", strChapterName));
		text = (string.IsNullOrEmpty(string_1) ? text.Replace("{?$next_pageName?}", "返回目录") : text.Replace("{?$next_pageName?}", string_1));
		int num = novelInfo_0.PutID / 1000;
		string newValue4 = Configs.BaseConfig.PrevFirstHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num.ToString());
		num = novelInfo_0.PutID / 1000;
		string newValue5 = Configs.BaseConfig.NextEndHtmlUrl.Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{NovelId/1000}", num.ToString());
		string text2 = ((preCID == 0) ? text.Replace("{?$preview_page?}", newValue4).Replace("{?$preview_chapteridd?}", "") : text.Replace("{?$preview_page?}", preCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$preview_chapterid?}", preCID.ToString()).Replace("{?$preview_chapteridd?}", preCID + "/"));
		string text3 = ((nextCID == 0) ? text2.Replace("{?$next_page?}", newValue5).Replace("{?$next_chapteridd?}", "") : text2.Replace("{?$next_page?}", nextCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$next_chapterid?}", nextCID.ToString()).Replace("{?$next_chapteridd?}", nextCID + "/")).Replace("<{$index_page}>", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$url_indexpage?}", Configs.BaseConfig.NextEndHtmlUrl).Replace("{?$index_page?}", Configs.BaseConfig.NextEndHtmlUrl)
			.Replace("{?$url_index?}", Configs.BaseConfig.NextEndHtmlUrl)
			.Replace("<{$article_id}>", novelInfo_0.PutID.ToString())
			.Replace("<{$chapter_id}>", chapterInfo_0.PutID.ToString())
			.Replace("<{$dynamic_url}>", Config.JieqiDefine["JIEQI_URL"].ToString())
			.Replace("<{$url_bookroom}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/")
			.Replace("<{$url_articleinfo}>", (string)Config.JieqiDefine["JIEQI_URL"] + "/modules/article/articleinfo.php?id=" + novelInfo_0.PutID)
			.Replace("{?$preChapterName?}", newValue)
			.Replace("{?$nextChapterName?}", newValue2)
			.Replace("{?$navcode?}", CreateNav(novelInfo_0.LagerSortID));
		text3 = ((!(Config.JieqiArticleConfigs["dynamicurl"].ToString() == "")) ? text3.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiArticleConfigs["dynamicurl"].ToString()) : text3.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article"));
		SpiderException.Debug("CreateChapter 文字广告");
		if (Configs.BaseConfig.TextMarkOfHtml)
		{
			chapterInfo_0 = FormatText.TextMark(chapterInfo_0);
		}
		if (chapterInfo_0.ChapterText == null)
		{
			string path = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + chapterInfo_0.PutID.ToString() + ".txt";
			if (File.Exists(path))
			{
				chapterInfo_0.ChapterText = File.ReadAllText(path, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset));
			}
			else
			{
				chapterInfo_0.ChapterText = "本章节为空章节！";
			}
		}
		string text4 = chapterInfo_0.ChapterText;
		string text5 = chapterInfo_0.ChapterText;
		if (!Configs.BaseConfig.Translate)
		{
			text4 = text4.Replace("  ", "&nbsp;&nbsp;");
			text5 = text5.Replace(" ", "");
		}
		string text6 = FormatText.Badwords(text4.Replace("\r", "").Replace("\n\n", "\n").Replace("\n\n", "\n")
			.Replace("\n\n", "\n")
			.Replace("\n", "<br /><br />"));
		if (text6.ToLower().IndexOf("本章节为空章节！") == 0)
		{
			SpiderException.EmptyTXT(novelInfo_0.Name, novelInfo_0.PutID, chapterInfo_0.ChapterName, chapterInfo_0.PutID);
		}
		if ((Configs.BaseConfig.OpenNullChapter && Configs.BaseConfig.NullChapter.Replace("\r\n", "").Trim() != "") || (chapterInfo_0.ChapterText.Length <= Configs.BaseConfig.SizeNullChapter && Configs.BaseConfig.NullChapter.Replace("\r\n", "").Trim() != ""))
		{
			Encoding.GetEncoding("utf-8").GetBytes(chapterInfo_0.ChapterText);
			text6 = Configs.BaseConfig.NullChapter.ToString().Replace("{?$articleid|subdirectory?}", Convert.ToString(novelInfo_0.PutID / 1000)).Replace("<{$author}>", novelInfo_0.Author)
				.Replace("<{$article_title}>", novelInfo_0.Name)
				.Replace("<{$jieqi_title}>", chapterInfo_0.ChapterName)
				.Replace("<{$sortname}>", novelInfo_0.LagerSort)
				.Replace("<{$jieqi_sitename}>", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString())
				.Replace("<{$jieqi_charset}>", Config.JieqiCharset)
				.Replace("<{$meta_keywords}>", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString())
				.Replace("<{$meta_description}>", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString())
				.Replace("<{$meta_author}>", Config.JieqiAuthor)
				.Replace("<{$meta_copyright}>", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString())
				.Replace("<{$new_url}>", Config.JieqiDefine["JIEQI_URL"].ToString());
		}
		int length = text5.Length;
		text5 = ((length <= 70) ? text5.Substring(0, length) : text5.Substring(0, 70));
		text3.Replace("{?$jieqi_content?}", text6.Replace("{?postErr(", "<a href='").Replace(")?}", "' >").Replace("{?/postErr?}", "</a>")).Replace("{?$aaabc?}", text5 + "......").Replace("{?$size?}", newValue3)
			.Replace("{?$words?}", newValue3);
		bool isEnableWapGen = Configs.BaseConfig.IsEnableWapGen;
		string text7 = "";
		string strWapChapterTemplate = Configs.BaseConfig.StrWapChapterTemplate;
		string strWapHtmlDir = Configs.BaseConfig.StrWapHtmlDir;
		string strWapDomain = Configs.BaseConfig.StrWapDomain;
		text7 = File.ReadAllText(strWapChapterTemplate, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")).Replace("{?$wap_domain?}", strWapDomain).Replace("{?$jieqi_content?}", text6.Replace("{?postErr(", "<a href='").Replace(")?}", "' >").Replace("{?/postErr?}", "</a>"))
			.Replace("{?$chaptername?}", chapterInfo_0.ChapterName)
			.Replace("{?$aaabc?}", text5 + "......")
			.Replace("{?$size?}", newValue3)
			.Replace("{?$words?}", newValue3);
		text7 = ((preCID == 0) ? text7.Replace("{?$preview_page?}", newValue4).Replace("{?$preview_chapteridd?}", "") : text7.Replace("{?$preview_page?}", preCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$preview_chapterid?}", preCID.ToString()).Replace("{?$preview_chapteridd?}", preCID + "/"));
		text7 = ((nextCID == 0) ? text7.Replace("{?$next_page?}", newValue5).Replace("{?$next_chapteridd?}", "") : text7.Replace("{?$next_page?}", nextCID + ((Configs.BaseConfig.PrevNextPageSuffix == "/") ? "/" : Config.JieqiArticleConfigs["htmlfile"].ToString())).Replace("{?$next_chapterid?}", nextCID.ToString()).Replace("{?$next_chapteridd?}", nextCID + "/"));
		text7 = ReplaceContents(text7, novelInfo_0, chapterInfo_0);
		int num2 = novelInfo_0.PutID / 1000;
		string text8 = strWapHtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num2.ToString()).Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
			.Replace("{Pinyin}", novelInfo_0.PinYin.ToString());
		if (!Directory.Exists(text8))
		{
			Directory.CreateDirectory(text8);
		}
		WriteGeneratedTextFile(text8 + "/" + chapterInfo_0.PutID + Config.JieqiArticleConfigs["htmlfile"].ToString(), text7);
	}

	public void DeleteChapter(NovelInfo novelInfo_0, int int_0, int int_1, bool bool_0, bool bool_1)
	{
		if (!bool_0)
		{
			SpiderException.Debug("CreateChapter 删除分卷");
			string string_ = "Delete From `jieqi_article_chapter` WHERE `chapterid`='" + int_1 + "' AND `chaptertype`='1'";
			MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
			return;
		}
		if (!bool_1)
		{
			SpiderException.Debug("CreateChapter 删除文本");
			int num = int_0 / 1000;
			string path = Config.HtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num.ToString()).Replace("{NovelId}", int_0.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
				.Replace("{Pinyin}", novelInfo_0.PinYin.ToString()) + "/" + int_1 + Config.JieqiArticleConfigs["htmlfile"].ToString();
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			string path2 = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + int_1.ToString() + ".txt";
			if (File.Exists(path2))
			{
				File.Delete(path2);
			}
		}
		SpiderException.Debug("CreateChapter 读取上一页，下一页");
		int putID = 0;
		int putID2 = 0;
		int num2 = 0;
		string string_2 = "SELECT `chaptername`,`chapterorder` FROM `jieqi_article_chapter` WHERE `chapterid`='" + int_1 + "'";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
		if (mySqlDataReader.Read())
		{
			num2 = Convert.ToInt32(mySqlDataReader["chapterorder"]);
		}
		mySqlDataReader.Close();
		string string_3 = "SELECT `chapterid` FROM `jieqi_article_chapter` WHERE `chapterorder`<'" + num2 + "' AND `articleid`='" + int_0 + "' AND `chaptertype` = '0' ORDER BY `chapterorder` DESC LIMIT 1";
		object obj = MySqlHelper.ExecuteScalar(MySqlHelper.ConnectionString, CommandType.Text, string_3, (MySqlParameter[])null);
		if (obj != DBNull.Value)
		{
			putID = Convert.ToInt32(obj);
		}
		string string_4 = "SELECT `chapterid` FROM `jieqi_article_chapter` WHERE `chapterorder`>'" + num2 + "' AND `articleid`='" + int_0 + "' AND `chaptertype` = '0' ORDER BY `chapterorder` ASC LIMIT 1";
		object obj2 = MySqlHelper.ExecuteScalar(MySqlHelper.ConnectionString, CommandType.Text, string_4, (MySqlParameter[])null);
		if (obj2 != DBNull.Value)
		{
			putID2 = Convert.ToInt32(obj2);
		}
		string string_5 = "Delete From `jieqi_article_chapter` WHERE `chapterid`='" + int_1 + "'";
		MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_5, (MySqlParameter[])null);
		int num3 = 0;
		int num4 = 0;
		ChapterInfo chapterInfo = null;
		string string_6 = "SELECT * FROM `jieqi_article_chapter` WHERE `articleid`='" + int_0 + "' AND `chaptertype` = '0' ORDER BY `chapterorder` DESC";
		MySqlDataReader mySqlDataReader2 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_6, (MySqlParameter[])null);
		if (mySqlDataReader2.Read())
		{
			ChapterInfo chapterInfo2 = new ChapterInfo
			{
				ChapterName = mySqlDataReader2["chaptername"].ToString(),
				PutID = Convert.ToInt32(mySqlDataReader2["chapterid"]),
				Summary = mySqlDataReader2["summary"].ToString()
			};
			chapterInfo = chapterInfo2;
			num3 = Convert.ToInt32(mySqlDataReader2["chapterorder"]);
		}
		mySqlDataReader2.Close();
		if (chapterInfo != null)
		{
			string string_7 = "SELECT * FROM `jieqi_article_chapter` WHERE `articleid`='" + int_0 + "' AND `chaptertype` = '1' ORDER BY `chapterorder` DESC";
			MySqlDataReader mySqlDataReader3 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_7, (MySqlParameter[])null);
			if (mySqlDataReader3.Read())
			{
				chapterInfo.VolumeName = mySqlDataReader3["chaptername"].ToString();
				num4 = Convert.ToInt32(mySqlDataReader3["chapterid"]);
			}
			mySqlDataReader3.Close();
		}
		SpiderException.Debug("DeleteChapter 更新小说最新章节");
		if (chapterInfo != null)
		{
			int num5 = 0;
			string string_8 = ((Configs.BaseConfig.CmsVersion == "2.4") ? ("SELECT Sum(words) FROM `jieqi_article_chapter` WHERE `articleid` ='" + int_0 + "'") : ("SELECT Sum(size) FROM `jieqi_article_chapter` WHERE `articleid` ='" + int_0 + "'"));
			MySqlDataReader mySqlDataReader4 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_8, (MySqlParameter[])null);
			if (mySqlDataReader4.Read())
			{
				num5 = ((Configs.BaseConfig.CmsVersion == "2.4") ? Convert.ToInt32(mySqlDataReader4["Sum(words)"]) : Convert.ToInt32(mySqlDataReader4["Sum(size)"]));
			}
			mySqlDataReader4.Close();
			MySqlHelper.ExecuteNonQuery(string_1: (!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? ("UPDATE `jieqi_article_article` SET  `lastvolumeid`='" + num4 + "',`lastvolume`='" + chapterInfo.VolumeName + "',`lastchapterid`='" + chapterInfo.PutID + "',`lastchapter`='" + chapterInfo.ChapterName + "',`chapters`='" + num3 + "',`size`='" + Math.Floor((double)num5 * 0.8) + "' WHERE `articleid`='" + int_0 + "'") : ((Configs.BaseConfig.CmsVersion == "2.4") ? ("UPDATE `jieqi_article_article` SET  `lastvolumeid`='" + num4 + "',`lastvolume`='" + chapterInfo.VolumeName + "',`lastchapterid`='" + chapterInfo.PutID + "',`lastchapter`='" + chapterInfo.ChapterName + "',`chapters`='" + num3 + "',`lastsummary`='" + chapterInfo.Summary.Replace("\r", "").Replace("\n", "").Replace("\r\n", "")
				.Replace("\n\r", "")
				.Replace("&", "")
				.Replace("<", "")
				.Replace(">", "")
				.Replace("nbsp", "") + "',`words`='" + num5 + "' WHERE `articleid`='" + int_0 + "'") : ("UPDATE `jieqi_article_article` SET  `lastvolumeid`='" + num4 + "',`lastvolume`='" + chapterInfo.VolumeName + "',`lastchapterid`='" + chapterInfo.PutID + "',`lastchapter`='" + chapterInfo.ChapterName + "',`chapters`='" + num3 + "',`lastsummary`='" + chapterInfo.Summary.Replace("\r", "").Replace("\n", "").Replace("\r\n", "")
				.Replace("\n\r", "")
				.Replace("&", "")
				.Replace("<", "")
				.Replace(">", "")
				.Replace("nbsp", "") + "',`size`='" + Math.Floor((double)num5 * 0.8) + "' WHERE `articleid`='" + int_0 + "'")), string_0: MySqlHelper.ConnectionString, commandType_0: CommandType.Text, mySqlParameter_0: (MySqlParameter[])null);
		}
		else
		{
			string path2 = "Delete From `jieqi_article_chapter` WHERE `articleid`='" + int_0 + "'";
			MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, path2, (MySqlParameter[])null);
			MySqlHelper.ExecuteNonQuery(string_1: (!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? ("UPDATE `jieqi_article_article` SET  `lastvolumeid`='0',`lastvolume`='最新分卷',`lastchapterid`='0',`lastchapter`='最新章节',`chapters`='0',`size`='0' WHERE `articleid`='" + int_0 + "'") : ((Configs.BaseConfig.CmsVersion == "2.4") ? ("UPDATE `jieqi_article_article` SET  `lastvolumeid`='0',`lastvolume`='最新分卷',`lastchapterid`='0',`lastchapter`='最新章节',`chapters`='0',`lastsummary`='',`words`='0' WHERE `articleid`='" + int_0 + "'") : ("UPDATE `jieqi_article_article` SET  `lastvolumeid`='0',`lastvolume`='最新分卷',`lastchapterid`='0',`lastchapter`='最新章节',`chapters`='0',`lastsummary`='',`size`='0' WHERE `articleid`='" + int_0 + "'")), string_0: MySqlHelper.ConnectionString, commandType_0: CommandType.Text, mySqlParameter_0: (MySqlParameter[])null);
		}
		ChapterInfo chapterInfo3 = new ChapterInfo
		{
			PutID = putID
		};
		ChapterInfo chapterInfo_ = chapterInfo3;
		createChapter(novelInfo_0, chapterInfo_, bool_0: false);
		ChapterInfo chapterInfo4 = new ChapterInfo
		{
			PutID = putID2
		};
		ChapterInfo chapterInfo_2 = chapterInfo4;
		createChapter(novelInfo_0, chapterInfo_2, bool_0: false);
	}

	public string[] DeleteStr(string[] strArray, int index)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < strArray.Length; i++)
		{
			if (i != index)
			{
				arrayList.Add(strArray[i]);
			}
		}
		return (string[])arrayList.ToArray(typeof(string));
	}

	public void DeleteVolume(NovelInfo novelInfo_0, int int_0)
	{
	}

	public void DeteleNovel(int int_0)
	{
		string string_ = "Delete From `jieqi_article_article` WHERE `articleid`='" + int_0 + "'";
		MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
		string_ = "Delete From `jieqi_article_chapter` WHERE `articleid`='" + int_0 + "'";
		MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
		string_ = "Delete From `jieqi_article_attachs` WHERE `articleid`='" + int_0 + "'";
		MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
		SpiderException.Debug("CreateChapter 删除图片");
		string path = Config.ImageDir + "/" + int_0 / 1000 + "/" + int_0.ToString();
		if (Directory.Exists(path))
		{
			Directory.Delete(path, recursive: true);
		}
		SpiderException.Debug("CreateChapter 删除文本");
		int num = int_0 / 1000;
		string path2 = Config.HtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num.ToString()).Replace("{NovelId}", int_0.ToString());
		if (Directory.Exists(path2))
		{
			Directory.Delete(path2, recursive: true);
		}
		string path3 = Config.TxtDir + "/" + int_0 / 1000 + "/" + int_0.ToString();
		if (Directory.Exists(path3))
		{
			Directory.Delete(path3, recursive: true);
		}
	}

	public NovelInfo GetChapterInfo(NovelInfo novelInfo_0)
	{
		ChapterInfo chapterInfo = GetChapterInfo(novelInfo_0.PutID, novelInfo_0.LastChapter.PutID);
		novelInfo_0.LastChapter.ChapterName = chapterInfo.ChapterName;
		novelInfo_0.LastChapter.ChapterText = chapterInfo.ChapterText;
		return novelInfo_0;
	}

	public ChapterInfo GetChapterInfo(int int_0, int int_1)
	{
		NovelInfo novelInfo = new NovelInfo
		{
			PutID = int_0
		};
		NovelInfo novelInfo2 = novelInfo;
		ChapterInfo chapterInfo = new ChapterInfo
		{
			PutID = int_1
		};
		ChapterInfo chapterInfo2 = chapterInfo;
		string string_ = "SELECT * FROM `jieqi_article_chapter` WHERE `chapterid`=@chapterid";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, new MySqlParameter("@chapterid", chapterInfo2.PutID));
		if (mySqlDataReader.Read())
		{
			string text = mySqlDataReader["lastupdate"].ToString();
			DateTime now = FromUnixTimestamp(text);
			chapterInfo2.LastTime = now;
			chapterInfo2.ChapterName = mySqlDataReader["chaptername"].ToString();
			if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
			{
				chapterInfo2.Summary = mySqlDataReader["summary"].ToString();
			}
			if (!File.Exists(Config.TxtDir + "/" + novelInfo2.PutID / 1000 + "/" + novelInfo2.PutID.ToString() + "/" + chapterInfo2.PutID.ToString() + ".txt"))
			{
				chapterInfo2.ChapterText = "";
			}
			else
			{
				chapterInfo2.ChapterText = File.ReadAllText(Config.TxtDir + "/" + novelInfo2.PutID / 1000 + "/" + novelInfo2.PutID.ToString() + "/" + chapterInfo2.PutID.ToString() + ".txt", FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset));
			}
		}
		mySqlDataReader.Close();
		return chapterInfo2;
	}

	public ChapterInfo[] GetChapterList(int int_0)
	{
		ArrayList arrayList = new ArrayList();
		string volumeName = "";
		string string_ = "SELECT * FROM `jieqi_article_chapter` WHERE `articleid`=@articleid Order By `chapterorder` ASC";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, new MySqlParameter("@articleid", int_0));
		while (mySqlDataReader.Read())
		{
			if (Convert.ToInt32(mySqlDataReader["chaptertype"]) == 1)
			{
				volumeName = mySqlDataReader["chaptername"].ToString();
				continue;
			}
			ChapterInfo chapterInfo = new ChapterInfo
			{
				PutID = Convert.ToInt32(mySqlDataReader["chapterid"]),
				Size = ((double.Parse(Configs.BaseConfig.CmsVersion) >= 2.4) ? Convert.ToInt32(mySqlDataReader["words"]) : Convert.ToInt32(mySqlDataReader["size"])),
				ItemIndex = Convert.ToInt32(mySqlDataReader["chapterorder"]),
				Name = mySqlDataReader["articlename"].ToString(),
				VolumeName = volumeName,
				ChapterName = mySqlDataReader["chaptername"].ToString(),
				LastTime = FormatText.GetTime(Convert.ToInt32(mySqlDataReader["lastupdate"])),
				PostTime = FormatText.GetTime(Convert.ToInt32(mySqlDataReader["postdate"]))
			};
			ChapterInfo value = chapterInfo;
			arrayList.Add(value);
		}
		mySqlDataReader.Close();
		return (ChapterInfo[])arrayList.ToArray(typeof(ChapterInfo));
	}

	public string GetChapterText(NovelInfo novelInfo_0, bool on)
	{
		string result = string.Empty;
		if (on)
		{
			result = FormatText.Typesetting(Regex.Replace(novelInfo_0.LastChapter.ChapterText.Replace("[img]", "").Replace("[/img]", ""), "【图片下载标记\\d*】", "")).Replace("\n", "\r\n");
			if (result.Length > 300 && !result.StartsWith("    "))
			{
				result = "    " + result;
			}
			return result;
		}
		string path = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + novelInfo_0.LastChapter.PutID.ToString() + ".txt";
		if (File.Exists(path))
		{
			result = File.ReadAllText(path, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset));
		}
		return result;
	}

	public NovelInfo GetNovelInfo(NovelInfo novelInfo_0)
	{
		novelInfo_0.PinYin = "";
		novelInfo_0.PinYinSan = "";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(string_1: (Configs.BaseConfig.NumOrPinyin == "拼音目录" && Configs.HaveFunction.IndexOf("PinyinDir") >= 0) ? ((!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag`  FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'") : ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`  FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")) : ((!(Configs.BaseConfig.CmsVersion == "2.4")) ? ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag` ,`rgroup` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'") : ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag` ,`rgroup` FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")) : ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`words`,`imgflag` ,`rgroup` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'") : ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag` ,`rgroup` FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")))) : ((!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag`  FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'") : ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag` FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")) : ((!(Configs.BaseConfig.CmsVersion == "2.4")) ? ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag` ,`rgroup` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'") : ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag` ,`rgroup` FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")) : ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`words`,`imgflag` ,`rgroup` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'") : ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag` ,`rgroup` FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")))), string_0: MySqlHelper.ConnectionString, commandType_0: CommandType.Text, mySqlParameter_0: (MySqlParameter[])null);
		if (mySqlDataReader.Read())
		{
			if (Configs.BaseConfig.NumOrPinyin == "拼音目录" && Configs.HaveFunction.IndexOf("PinyinDir") >= 0)
			{
				novelInfo_0.PinYin = mySqlDataReader["articlecode"].ToString();
				novelInfo_0.PinYinSan = novelInfo_0.PinYin.ToString().Substring(0, 3);
			}
			novelInfo_0.PutID = Convert.ToInt32(mySqlDataReader["articleid"]);
			novelInfo_0.LastChapter.VolumeName = mySqlDataReader["lastvolume"].ToString();
			string input = mySqlDataReader["lastchapter"].ToString();
			novelInfo_0.Name = mySqlDataReader["articlename"].ToString();
			novelInfo_0.Author = mySqlDataReader["author"].ToString();
			novelInfo_0.Intro = mySqlDataReader["intro"].ToString();
			novelInfo_0.Degree = Convert.ToInt32(mySqlDataReader["fullflag"]);
			novelInfo_0.Keyword = mySqlDataReader["keywords"].ToString();
			novelInfo_0.Intro = mySqlDataReader["intro"].ToString();
			novelInfo_0.LagerSortID = Convert.ToInt32(mySqlDataReader["sortid"]);
			novelInfo_0.PostDate = Convert.ToInt32(mySqlDataReader["postdate"]);
			novelInfo_0.LastupDate = Convert.ToInt32(mySqlDataReader["lastupdate"]);
			novelInfo_0.Size = ((Configs.BaseConfig.CmsVersion == "2.4") ? Convert.ToInt32(mySqlDataReader["words"]) : Convert.ToInt32(mySqlDataReader["size"]));
			novelInfo_0.ImgFlag = Convert.ToInt32(mySqlDataReader["imgflag"]);
			novelInfo_0.Chapters = Convert.ToInt32(mySqlDataReader["chapters"]);
			if (novelInfo_0.LagerSortID >= Config.JieqiSort.Length || novelInfo_0.LagerSortID <= 0)
			{
				novelInfo_0.LagerSortID = 1;
			}
			novelInfo_0.LagerSort = Config.JieqiSort[novelInfo_0.LagerSortID];
			novelInfo_0.LastChapter.ChapterName = Regex.Replace(input, "\\s+", " ").Trim().Trim();
			novelInfo_0.LastChapter.PutID = Convert.ToInt32(mySqlDataReader["lastchapterid"]);
		}
		else
		{
			novelInfo_0.PutID = 0;
			novelInfo_0.IsNew = true;
		}
		mySqlDataReader.Close();
		return novelInfo_0;
	}

	public NovelInfo GetNovelInfo(NovelInfo novelInfo_0, bool bool_0)
	{
		string text = "";
		novelInfo_0.PinYin = "";
		novelInfo_0.PinYinSan = "";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(string_1: (Configs.BaseConfig.NumOrPinyin == "拼音目录" && Configs.HaveFunction.IndexOf("PinyinDir") >= 0) ? ((!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'" + (bool_0 ? (" and `author` = '" + novelInfo_0.Author + "'") : "")) : ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag`  FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")) : ((!(Configs.BaseConfig.CmsVersion == "2.4")) ? ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`typeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag`,`lastsummary` ,`rgroup` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'" + (bool_0 ? (" and `author` = '" + novelInfo_0.Author + "'") : "")) : ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`typeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag`,`lastsummary` ,`rgroup` FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")) : ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`typeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`words`,`imgflag`,`lastsummary` ,`rgroup` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'" + (bool_0 ? (" and `author` = '" + novelInfo_0.Author + "'") : "")) : ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`typeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`words`,`imgflag`,`lastsummary` ,`rgroup` FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")))) : ((!(double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)) ? ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'" + (bool_0 ? (" and `author` = '" + novelInfo_0.Author + "'") : "")) : ("SELECT `articlename`,`articleid`,`author`,`intro`,`sortid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag`  FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")) : ((!(Configs.BaseConfig.CmsVersion == "2.4")) ? ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`typeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag`,`lastsummary` ,`rgroup` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'" + (bool_0 ? (" and `author` = '" + novelInfo_0.Author + "'") : "")) : ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`typeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`size`,`imgflag`,`lastsummary` ,`rgroup`  FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")) : ((novelInfo_0.PutID == 0) ? ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`typeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`words`,`imgflag`,`lastsummary` ,`rgroup` FROM `jieqi_article_article` WHERE `articlename` = '" + novelInfo_0.Name + "'" + (bool_0 ? (" and `author` = '" + novelInfo_0.Author + "'") : "")) : ("SELECT `articlename`,`articlecode`,`articleid`,`author`,`intro`,`sortid`,`typeid`,`lastvolume`,`lastchapterid`,`lastchapter`,`chapters`,`keywords`,`fullflag`,`postdate`,`lastupdate`,`words`,`imgflag`,`lastsummary` ,`rgroup`  FROM `jieqi_article_article` WHERE `articleid`='" + novelInfo_0.PutID + "'")))), string_0: MySqlHelper.ConnectionString, commandType_0: CommandType.Text, mySqlParameter_0: (MySqlParameter[])null);
		if (mySqlDataReader.Read())
		{
			if (Configs.BaseConfig.NumOrPinyin == "拼音目录" && Configs.HaveFunction.IndexOf("PinyinDir") >= 0)
			{
				novelInfo_0.PinYin = mySqlDataReader["articlecode"].ToString();
				novelInfo_0.PinYinSan = novelInfo_0.PinYin.ToString().Substring(0, 3);
			}
			novelInfo_0.PutID = Convert.ToInt32(mySqlDataReader["articleid"]);
			novelInfo_0.LastChapter.VolumeName = mySqlDataReader["lastvolume"].ToString();
			string input = mySqlDataReader["lastchapter"].ToString();
			novelInfo_0.Name = mySqlDataReader["articlename"].ToString();
			novelInfo_0.Author = mySqlDataReader["author"].ToString();
			novelInfo_0.Chapters = Convert.ToInt32(mySqlDataReader["chapters"]);
			novelInfo_0.Intro = mySqlDataReader["intro"].ToString();
			novelInfo_0.Degree = Convert.ToInt32(mySqlDataReader["fullflag"]);
			novelInfo_0.Keyword = mySqlDataReader["keywords"].ToString();
			novelInfo_0.LagerSortID = Convert.ToInt32(mySqlDataReader["sortid"]);
			novelInfo_0.PostDate = Convert.ToInt32(mySqlDataReader["postdate"]);
			novelInfo_0.LastupDate = Convert.ToInt32(mySqlDataReader["lastupdate"]);
			novelInfo_0.Size = ((Configs.BaseConfig.CmsVersion == "2.4") ? Convert.ToInt32(mySqlDataReader["words"]) : Convert.ToInt32(mySqlDataReader["size"]));
			novelInfo_0.ImgFlag = Convert.ToInt32(mySqlDataReader["imgflag"]);
			if (novelInfo_0.LagerSortID >= Config.JieqiSort.Length || novelInfo_0.LagerSortID <= 0)
			{
				novelInfo_0.LagerSortID = 1;
			}
			novelInfo_0.LagerSort = Config.JieqiSort[novelInfo_0.LagerSortID];
			if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
			{
				novelInfo_0.Articlecode = mySqlDataReader["articlecode"].ToString();
				novelInfo_0.Lastsummary = mySqlDataReader["lastsummary"].ToString();
				novelInfo_0.SmallSortID = Convert.ToInt32(mySqlDataReader["typeid"]);
				if (novelInfo_0.SmallSortID >= Config.JieqiSorttypes[novelInfo_0.LagerSortID].Length || novelInfo_0.SmallSortID <= 0)
				{
					novelInfo_0.SmallSortID = 0;
				}
				novelInfo_0.SmallSort = Config.JieqiSorttypes[novelInfo_0.LagerSortID][novelInfo_0.SmallSortID];
				novelInfo_0.IsboyID = Convert.ToInt32(mySqlDataReader["rgroup"]);
			}
			novelInfo_0.LastChapter.ChapterName = Regex.Replace(input, "\\s+", " ").Trim().Trim();
			novelInfo_0.LastChapter.PutID = Convert.ToInt32(mySqlDataReader["lastchapterid"]);
			if (Configs.BaseConfig.LicenseVip)
			{
				SpiderException.Debug("DelNoChapter 删除防盗html文件");
				int num = novelInfo_0.PutID / 1000;
				string path = Config.HtmlDir + "/" + Configs.BaseConfig.NumOrPinyinDir.Replace("{NovelId/1000}", num.ToString()).Replace("{NovelId}", novelInfo_0.PutID.ToString()).Replace("{Pinyin/3}", novelInfo_0.PinYinSan.ToString())
					.Replace("{Pinyin}", novelInfo_0.PinYin.ToString());
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path);
					foreach (string text2 in files)
					{
						int num2 = text2.LastIndexOf("\\");
						string text3 = text2.Substring(num2 + 1).Replace(".html", "");
						if (SecurityUtil.IsNum(text3) && int.Parse(text3) > novelInfo_0.LastChapter.PutID)
						{
							File.Delete(text2);
						}
					}
				}
			}
		}
		else
		{
			novelInfo_0.PutID = 0;
			novelInfo_0.IsNew = true;
		}
		mySqlDataReader.Close();
		return novelInfo_0;
	}

	public NovelInfo[] GetNovelList(string string_0)
	{
		ArrayList arrayList = new ArrayList();
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_0, (MySqlParameter[])null);
		while (mySqlDataReader.Read())
		{
			NovelInfo novelInfo = new NovelInfo
			{
				PinYin = "",
				PinYinSan = ""
			};
			NovelInfo novelInfo2 = novelInfo;
			if (Configs.BaseConfig.NumOrPinyin == "拼音目录" && novelInfo2.PinYin == "")
			{
				novelInfo2.PinYin = mySqlDataReader["articlecode"].ToString();
				novelInfo2.PinYinSan = novelInfo2.PinYin.Substring(0, 3);
			}
			novelInfo2.PutID = Convert.ToInt32(mySqlDataReader["articleid"]);
			novelInfo2.LagerSortID = Convert.ToInt32(mySqlDataReader["sortid"]);
			novelInfo2.Name = mySqlDataReader["articlename"].ToString();
			novelInfo2.Degree = Convert.ToInt32(mySqlDataReader["fullflag"]);
			novelInfo2.Keyword = mySqlDataReader["keywords"].ToString();
			novelInfo2.Intro = mySqlDataReader["intro"].ToString();
			novelInfo2.LastChapter.VolumeName = mySqlDataReader["lastvolume"].ToString();
			string input = mySqlDataReader["lastchapter"].ToString();
			novelInfo2.Author = mySqlDataReader["author"].ToString();
			novelInfo2.PostDate = Convert.ToInt32(mySqlDataReader["postdate"]);
			novelInfo2.TopDate = Convert.ToInt32(mySqlDataReader["toptime"]);
			novelInfo2.LastupDate = Convert.ToInt32(mySqlDataReader["lastupdate"]);
			if (double.Parse(Configs.BaseConfig.CmsVersion) >= 2.4)
			{
				novelInfo2.Size = Convert.ToInt32(mySqlDataReader["words"]);
			}
			else
			{
				novelInfo2.Size = Convert.ToInt32(mySqlDataReader["size"]);
			}
			novelInfo2.Chapters = Convert.ToInt32(mySqlDataReader["chapters"]);
			novelInfo2.ImgFlag = Convert.ToInt32(mySqlDataReader["imgflag"]);
			if (novelInfo2.LagerSortID >= Config.JieqiSort.Length || novelInfo2.LagerSortID <= 0)
			{
				novelInfo2.LagerSortID = 1;
			}
			novelInfo2.LagerSort = Config.JieqiSort[novelInfo2.LagerSortID];
			if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
			{
				if (mySqlDataReader["typeid"] != null)
				{
					novelInfo2.SmallSortID = Convert.ToInt32(mySqlDataReader["typeid"]);
				}
				else
				{
					novelInfo2.SmallSortID = 0;
				}
				if (novelInfo2.SmallSortID >= Config.JieqiSorttypes[novelInfo2.LagerSortID].Length || novelInfo2.SmallSortID <= 0)
				{
					novelInfo2.SmallSortID = 0;
				}
				novelInfo2.SmallSort = Config.JieqiSorttypes[novelInfo2.LagerSortID][novelInfo2.SmallSortID];
				novelInfo2.IsboyID = Convert.ToInt32(mySqlDataReader["rgroup"]);
			}
			novelInfo2.LastChapter.ChapterName = Regex.Replace(input, "\\s+", " ").Trim().Trim();
			novelInfo2.LastChapter.PutID = Convert.ToInt32(mySqlDataReader["lastchapterid"]);
			novelInfo2.IsNew = false;
			arrayList.Add(novelInfo2);
		}
		mySqlDataReader.Close();
		NovelInfo[] array = new NovelInfo[arrayList.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (NovelInfo)arrayList[i];
		}
		return array;
	}

	public string[] GetNovelLj(NovelInfo novelInfo, int intLinjuNum)
	{
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		string string_ = string.Concat("SELECT * FROM `jieqi_article_article` WHERE `sortid`='" + novelInfo.LagerSortID + "' and display='0' and `articleid` < '", novelInfo.PutID, "' Order By `articleid` DESC limit 0,", intLinjuNum);
		string string_2 = string.Concat("SELECT * FROM `jieqi_article_article` WHERE `sortid`='" + novelInfo.LagerSortID + "' and display='0' and `articleid` > '", novelInfo.PutID, "' Order By articleid limit 0,", intLinjuNum);
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
		MySqlDataReader mySqlDataReader2 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
		while (mySqlDataReader.Read())
		{
			string text = "";
			string text2 = "pinyin";
			if (Configs.BaseConfig.NumOrPinyin == "拼音目录")
			{
				text2 = mySqlDataReader["articlecode"].ToString();
			}
			text = text2 + "^" + mySqlDataReader["articleid"].ToString() + "^" + mySqlDataReader["articlename"].ToString();
			arrayList.Add(text);
		}
		mySqlDataReader.Close();
		while (mySqlDataReader2.Read())
		{
			string text3 = "";
			string text4 = "pinyin";
			if (Configs.BaseConfig.NumOrPinyin == "拼音目录")
			{
				text4 = mySqlDataReader2["articlecode"].ToString();
			}
			text3 = text4 + "^" + mySqlDataReader2["articleid"].ToString() + "^" + mySqlDataReader2["articlename"].ToString();
			arrayList2.Add(text3);
		}
		mySqlDataReader2.Close();
		int num = arrayList2.Count + arrayList.Count;
		string text5 = "";
		if (num <= intLinjuNum)
		{
			intLinjuNum = num;
		}
		for (int i = 0; i < num; i++)
		{
			if (arrayList3.Count < intLinjuNum && arrayList.Count > i)
			{
				text5 = arrayList[i].ToString();
				arrayList3.Add(text5);
			}
		}
		for (int i = 0; i < num; i++)
		{
			if (arrayList3.Count < intLinjuNum * 2 && arrayList2.Count > i)
			{
				text5 = arrayList2[i].ToString();
				arrayList3.Add(text5);
			}
		}
		arrayList.Clear();
		arrayList2.Clear();
		string[] result = (string[])arrayList3.ToArray(typeof(string));
		arrayList3.Clear();
		return result;
	}

	public string[] GetNovelTj(NovelInfo novelInfo, int intTuijianNum)
	{
		string string_ = "SELECT * FROM `jieqi_article_article` where `articleid` <> '" + novelInfo.PutID + "' and `display`='0'  And `sortid`='" + novelInfo.LagerSortID + "' Order By `toptime` desc limit 0," + intTuijianNum;
		switch (Configs.BaseConfig.TuijianType)
		{
		case "日点击榜":
			string_ = string.Concat(new object[1] { "SELECT * FROM `jieqi_article_article` where `articleid` <> '" + novelInfo.PutID + "' and `display`='0'  And `sortid`='" + novelInfo.LagerSortID + "' Order By `dayvisit` desc limit 0," + intTuijianNum });
			break;
		case "周点击榜":
			string_ = string.Concat("SELECT * FROM `jieqi_article_article` where `articleid` <> '", novelInfo.PutID, "' and `display`='0'  And `sortid`='", novelInfo.LagerSortID, "' Order By `weekvisit` desc limit 0," + intTuijianNum);
			break;
		case "月点击榜":
			string_ = string.Concat("SELECT * FROM `jieqi_article_article` where `articleid` <> '", novelInfo.PutID, "' and `display`='0'  And `sortid`='", novelInfo.LagerSortID, "' Order By `monthvisit` desc limit 0," + intTuijianNum);
			break;
		case "总点击榜":
			string_ = string.Concat("SELECT * FROM `jieqi_article_article` where `articleid` <> '", novelInfo.PutID, "' and `display`='0'  And `sortid`='", novelInfo.LagerSortID, "' Order By `allvisit` desc limit 0," + intTuijianNum);
			break;
		case "日投票榜":
			string_ = string.Concat("SELECT * FROM `jieqi_article_article` where `articleid` <> '", novelInfo.PutID, "' and `display`='0'  And `sortid`='", novelInfo.LagerSortID, "' Order By `dayvote` desc limit 0," + intTuijianNum);
			break;
		case "周投票榜":
			string_ = string.Concat("SELECT * FROM `jieqi_article_article` where `articleid` <> '", novelInfo.PutID, "' and `display`='0'  And `sortid`='", novelInfo.LagerSortID, "' Order By `weekvote` desc limit 0," + intTuijianNum);
			break;
		case "月投票榜":
			string_ = string.Concat("SELECT * FROM `jieqi_article_article` where `articleid` <> '", novelInfo.PutID, "' and `display`='0'  And `sortid`='", novelInfo.LagerSortID, "' Order By `monthvote` desc limit 0," + intTuijianNum);
			break;
		case "总投票榜":
			string_ = string.Concat("SELECT * FROM `jieqi_article_article` where `articleid` <> '", novelInfo.PutID, "' and `display`='0'  And `sortid`='", novelInfo.LagerSortID, "' Order By `allvote` desc limit 0," + intTuijianNum);
			break;
		}
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
		ArrayList arrayList = new ArrayList();
		while (mySqlDataReader.Read())
		{
			string text = "";
			string text2 = "pinyin";
			if (Configs.BaseConfig.NumOrPinyin == "拼音目录")
			{
				text2 = mySqlDataReader["articlecode"].ToString();
			}
			text = text2 + "^" + mySqlDataReader["articleid"].ToString() + "^" + mySqlDataReader["articlename"].ToString();
			arrayList.Add(text);
		}
		mySqlDataReader.Close();
		string[] result = (string[])arrayList.ToArray(typeof(string));
		arrayList.Clear();
		return result;
	}

	public ChapterInfo[] GetVolumeNameList(int int_0)
	{
		ArrayList arrayList = new ArrayList();
		string text = "";
		string string_ = "SELECT * FROM `jieqi_article_chapter` WHERE `articleid` = '" + int_0 + "' Order By `chapterorder` ASC";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
		while (mySqlDataReader.Read())
		{
			if (Convert.ToInt32(mySqlDataReader["chaptertype"]) == 1)
			{
				text = mySqlDataReader["chaptername"].ToString();
				ChapterInfo chapterInfo = new ChapterInfo
				{
					PutID = Convert.ToInt32(mySqlDataReader["chapterid"]),
					VolumeName = text,
					ChapterName = mySqlDataReader["chaptername"].ToString().Trim(),
					LastTime = FormatText.GetTime(Convert.ToInt32(mySqlDataReader["lastupdate"]))
				};
				ChapterInfo value = chapterInfo;
				arrayList.Add(value);
			}
		}
		mySqlDataReader.Close();
		return (ChapterInfo[])arrayList.ToArray(typeof(ChapterInfo));
	}

	public string InnerLinkReplace(string chapterContent)
	{
		string text = chapterContent;
		string text2 = text;
		if (keywordlist.Count <= 0)
		{
			string string_ = " SELECT  DISTINCT `tagname` FROM `jieqi_article_tag` ORDER by `length` DESC ";
			MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
			while (mySqlDataReader.Read())
			{
				keywordlist.Add(mySqlDataReader["tagname"].ToString());
			}
			mySqlDataReader.Close();
		}
		foreach (string item in keywordlist)
		{
			string text3 = Configs.BaseConfig.InnerTagLinkUrl1.Replace("{Tag}", item).Replace("{Tag|urlencode}", UrlEncode(item));
			int num = text2.IndexOf(item);
			if (num >= 0)
			{
				int length = item.Length;
				text = text.Substring(0, num) + text3 + text.Substring(num + length);
				text2 = text2.Substring(0, num) + new string('*', text3.Length) + text2.Substring(num + length);
			}
		}
		return text;
	}

	public ChapterInfo InsertChapter(NovelInfo novelInfo_0, TaskConfigInfo taskConfigInfo_0)
	{
		SpiderException.Debug("InsertChapter 获得本书最新分卷");
		int num = 0;
		string string_ = "SELECT MAX(chapterorder) FROM `jieqi_article_chapter` WHERE `articleid`=@articleid";
		object obj = MySqlHelper.ExecuteScalar(MySqlHelper.ConnectionString, CommandType.Text, string_, new MySqlParameter("@articleid", novelInfo_0.PutID));
		if (obj != null && obj != DBNull.Value)
		{
			num = Convert.ToInt32(obj);
		}
		int num2 = num + 1;
		string text = "";
		int num3 = 0;
		string text2 = "";
		string string_2 = "SELECT `chaptername`,`chapterid` FROM `jieqi_article_chapter` WHERE `articleid`=@articleid AND `chaptertype` = '1' ORDER BY `chapterorder` DESC";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, new MySqlParameter("@articleid", novelInfo_0.PutID));
		while (mySqlDataReader.Read())
		{
			if (num3 == 0)
			{
				text = mySqlDataReader["chaptername"].ToString();
				num3 = Convert.ToInt32(mySqlDataReader["chapterid"]);
			}
			text2 = text2 + mySqlDataReader["chaptername"].ToString().Trim() + "`";
		}
		text2.TrimEnd('`');
		mySqlDataReader.Close();
		SpiderException.Debug("InsertChapter 比较分卷");
		bool shouldInsertVolume = !taskConfigInfo_0.ProhibitionVolume && (!taskConfigInfo_0.CheckVolume || !Regex.IsMatch(novelInfo_0.LastChapter.ChapterName, "([第]*[两|一|二|三|四|五|六|七|八|九|〇|壹|贰|叁|肆|伍|陆|柒|捌|玖|零|十|拾|百|佰|千|万|1|2|3|4|5|6|7|8|9|0]*[章|节|回|卷|话])") || Regex.IsMatch(novelInfo_0.LastChapter.ChapterName, "([第][零|一|〇|壹|1|0]?[章|节|回|卷|话])")) && !FormatText.CompareToChapter(text2, novelInfo_0.LastChapter.VolumeName) && !taskConfigInfo_0.ProhibitionVolume;
		int chapterInsertSize = IsCms24() ? GetJieqiWordCount(novelInfo_0.LastChapter.ChapterText, strictChapterInsert: true) : GetLegacyChapterSize(novelInfo_0.LastChapter.ChapterText);
		if (Configs.BaseConfig.TextMarkOfData)
		{
			novelInfo_0.LastChapter = FormatText.TextMark(novelInfo_0.LastChapter);
		}
		novelInfo_0.LastChapter.ChapterText = FormatText.Typesetting(novelInfo_0.LastChapter.ChapterText);
		novelInfo_0.LastChapter.ChapterText = novelInfo_0.LastChapter.ChapterText.Replace("\n", "\r\n");
		int articleSize = IsCms24() ? GetJieqiWordCount(novelInfo_0.LastChapter.ChapterText, strictChapterInsert: false) : GetLegacyChapterSize(novelInfo_0.LastChapter.ChapterText);
		string text3 = string.Empty;
		if (IsCms18OrNewer())
		{
			text3 = string.Empty;
			novelInfo_0.Lastsummary = string.Empty;
		}
		SpiderException.Debug("InsertChapter 添加章节数据");
		int now = FormatText.GetNow();
		int num4 = 0;
		if (taskConfigInfo_0.Hidebook)
		{
			num4 = 2;
		}
		using (MySqlConnection mySqlConnection = new MySqlConnection(MySqlHelper.ConnectionString))
		{
			mySqlConnection.Open();
			using MySqlTransaction mySqlTransaction = mySqlConnection.BeginTransaction();
			try
			{
				if (shouldInsertVolume)
				{
					MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, BuildChapterInsertSql(), CreateChapterParameters(novelInfo_0, novelInfo_0.LastChapter.VolumeName, num2, 0, 1, now));
					num3 = Convert.ToInt32(MySqlHelper.ExecuteScalar(mySqlTransaction, CommandType.Text, "SELECT LAST_INSERT_ID()"));
					num2++;
					text = novelInfo_0.LastChapter.VolumeName;
				}
				MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, BuildChapterInsertSql(), CreateChapterParameters(novelInfo_0, novelInfo_0.LastChapter.ChapterName, num2, chapterInsertSize, 0, now));
				novelInfo_0.LastChapter.PutID = Convert.ToInt32(MySqlHelper.ExecuteScalar(mySqlTransaction, CommandType.Text, "SELECT LAST_INSERT_ID()"));
				if (IsCms18OrNewer())
				{
					MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, "UPDATE `jieqi_article_chapter` SET `chaptername`=@chaptername WHERE `chapterid`=@chapterid", new MySqlParameter("@chaptername", novelInfo_0.LastChapter.ChapterName), new MySqlParameter("@chapterid", novelInfo_0.LastChapter.PutID));
				}
				string articleUpdateSql;
				MySqlParameter[] articleParameters;
				if (!IsCms18OrNewer())
				{
					articleUpdateSql = "UPDATE `jieqi_article_article` SET `lastupdate`=@now,`lastvolumeid`=@lastvolumeid,`lastvolume`=@lastvolume,`lastchapterid`=@lastchapterid,`lastchapter`=@lastchapter,`size`=`size`+@articleSize,`chapters`=@chapters,`display`=@display WHERE `articleid`=@articleid";
					articleParameters = new MySqlParameter[] { new MySqlParameter("@now", now), new MySqlParameter("@lastvolumeid", num3), new MySqlParameter("@lastvolume", text), new MySqlParameter("@lastchapterid", novelInfo_0.LastChapter.PutID), new MySqlParameter("@lastchapter", novelInfo_0.LastChapter.ChapterName), new MySqlParameter("@articleSize", articleSize), new MySqlParameter("@chapters", num2), new MySqlParameter("@display", num4), new MySqlParameter("@articleid", novelInfo_0.PutID) };
				}
				else if (IsCms24())
				{
					articleUpdateSql = "UPDATE `jieqi_article_article` SET `lastupdate`=@now,`lastvolumeid`=@lastvolumeid,`lastvolume`=@lastvolume,`lastchapterid`=@lastchapterid,`lastchapter`=@lastchapter,`lastsummary`=@lastsummary,`words`=`words`+@articleSize,`chapters`=@chapters,`freetime`=@now,`freewords`=`freewords`+@articleSize,`display`=@display WHERE `articleid`=@articleid";
					articleParameters = new MySqlParameter[] { new MySqlParameter("@now", now), new MySqlParameter("@lastvolumeid", num3), new MySqlParameter("@lastvolume", text), new MySqlParameter("@lastchapterid", novelInfo_0.LastChapter.PutID), new MySqlParameter("@lastchapter", novelInfo_0.LastChapter.ChapterName), new MySqlParameter("@lastsummary", text3), new MySqlParameter("@articleSize", articleSize), new MySqlParameter("@chapters", num2), new MySqlParameter("@display", num4), new MySqlParameter("@articleid", novelInfo_0.PutID) };
				}
				else
				{
					articleUpdateSql = "UPDATE `jieqi_article_article` SET `lastupdate`=@now,`lastvolumeid`=@lastvolumeid,`lastvolume`=@lastvolume,`lastchapterid`=@lastchapterid,`lastchapter`=@lastchapter,`lastsummary`=@lastsummary,`size`=`size`+@articleSize,`chapters`=@chapters,`freetime`=@now,`freesize`=`freesize`+@articleSize,`display`=@display WHERE `articleid`=@articleid";
					articleParameters = new MySqlParameter[] { new MySqlParameter("@now", now), new MySqlParameter("@lastvolumeid", num3), new MySqlParameter("@lastvolume", text), new MySqlParameter("@lastchapterid", novelInfo_0.LastChapter.PutID), new MySqlParameter("@lastchapter", novelInfo_0.LastChapter.ChapterName), new MySqlParameter("@lastsummary", text3), new MySqlParameter("@articleSize", articleSize), new MySqlParameter("@chapters", num2), new MySqlParameter("@display", num4), new MySqlParameter("@articleid", novelInfo_0.PutID) };
				}
				MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, articleUpdateSql, articleParameters);
				mySqlTransaction.Commit();
			}
			catch
			{
				mySqlTransaction.Rollback();
				throw;
			}
		}
		SpiderException.Debug("InsertChapter 生成章节内容Txt文件");
		novelInfo_0.LastChapter.ChapterText = novelInfo_0.LastChapter.ChapterText.Trim();
		if (novelInfo_0.LastChapter.ChapterText.Length > 300 && !novelInfo_0.LastChapter.ChapterText.StartsWith("    "))
		{
			novelInfo_0.LastChapter.ChapterText = "    " + novelInfo_0.LastChapter.ChapterText;
		}
		string text4 = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID;
		WriteChapterTextFile(text4, novelInfo_0.LastChapter.PutID, novelInfo_0.LastChapter.ChapterText);
		return novelInfo_0.LastChapter;
	}
	public ChapterInfo InsertChapterByOrder(NovelInfo novelInfo_0, TaskConfigInfo taskConfigInfo_0, int int_0)
	{
		int num = int_0;
		string text = "";
		int num2 = 0;
		string string_ = "SELECT `chaptername`,`chapterid` FROM `jieqi_article_chapter` WHERE `articleid`=@articleid AND `chaptertype` = '1' AND `chapterorder` < @chapterorder ORDER BY `chapterorder` DESC";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, new MySqlParameter("@articleid", novelInfo_0.PutID), new MySqlParameter("@chapterorder", int_0));
		if (mySqlDataReader.Read())
		{
			text = mySqlDataReader["chaptername"].ToString();
			num2 = Convert.ToInt32(mySqlDataReader["chapterid"]);
		}
		mySqlDataReader.Close();
		SpiderException.Debug("InsertChapter 比较分卷");
		bool shouldInsertVolume = !taskConfigInfo_0.ProhibitionVolume && (!taskConfigInfo_0.CheckVolume || !Regex.IsMatch(novelInfo_0.LastChapter.ChapterName, "([第]*[两|一|二|三|四|五|六|七|八|九|〇|壹|贰|叁|肆|伍|陆|柒|捌|玖|零|十|拾|百|佰|千|万|1|2|3|4|5|6|7|8|9|0]*[章|节|回|卷])") || Regex.IsMatch(novelInfo_0.LastChapter.ChapterName, "([第][零|一|〇|壹|1|0]?[章|节|回|卷])")) && (!FormatText.CompareToChapter(text, novelInfo_0.LastChapter.VolumeName) || num2 == 0) && !taskConfigInfo_0.ProhibitionVolume;
		int chapterInsertSize = IsCms24() ? GetJieqiWordCount(novelInfo_0.LastChapter.ChapterText, strictChapterInsert: false) : GetLegacyChapterSize(novelInfo_0.LastChapter.ChapterText);
		if (Configs.BaseConfig.TextMarkOfData)
		{
			novelInfo_0.LastChapter = FormatText.TextMark(novelInfo_0.LastChapter);
		}
		novelInfo_0.LastChapter.ChapterText = FormatText.Typesetting(novelInfo_0.LastChapter.ChapterText);
		novelInfo_0.LastChapter.ChapterText = novelInfo_0.LastChapter.ChapterText.Replace("\n", "\r\n\r\n");
		int articleSize = IsCms24() ? GetJieqiWordCount(novelInfo_0.LastChapter.ChapterText, strictChapterInsert: false) : GetLegacyChapterSize(novelInfo_0.LastChapter.ChapterText);
		string text2 = string.Empty;
		if (IsCms18OrNewer())
		{
			text2 = string.Empty;
			novelInfo_0.Lastsummary = string.Empty;
		}
		SpiderException.Debug("InsertChapter 添加章节数据");
		int now = FormatText.GetNow();
		using (MySqlConnection mySqlConnection = new MySqlConnection(MySqlHelper.ConnectionString))
		{
			mySqlConnection.Open();
			using MySqlTransaction mySqlTransaction = mySqlConnection.BeginTransaction();
			try
			{
				if (shouldInsertVolume)
				{
					MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, BuildChapterInsertSql(), CreateChapterParameters(novelInfo_0, novelInfo_0.LastChapter.VolumeName, num, 0, 1, now));
					num2 = Convert.ToInt32(MySqlHelper.ExecuteScalar(mySqlTransaction, CommandType.Text, "SELECT LAST_INSERT_ID()"));
					num++;
					text = novelInfo_0.LastChapter.VolumeName;
				}
				MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, BuildChapterInsertSql(), CreateChapterParameters(novelInfo_0, novelInfo_0.LastChapter.ChapterName, num, chapterInsertSize, 0, now));
				novelInfo_0.LastChapter.PutID = Convert.ToInt32(MySqlHelper.ExecuteScalar(mySqlTransaction, CommandType.Text, "SELECT LAST_INSERT_ID()"));
				if (IsCms18OrNewer())
				{
					MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, "UPDATE `jieqi_article_chapter` SET `chaptername`=@chaptername WHERE `chapterid`=@chapterid", new MySqlParameter("@chaptername", novelInfo_0.LastChapter.ChapterName), new MySqlParameter("@chapterid", novelInfo_0.LastChapter.PutID));
				}
				string articleUpdateSql;
				MySqlParameter[] articleParameters;
				if (!IsCms18OrNewer())
				{
					articleUpdateSql = "UPDATE `jieqi_article_article` SET `lastupdate`=@now,`lastvolumeid`=@lastvolumeid,`lastvolume`=@lastvolume,`lastchapterid`=@lastchapterid,`lastchapter`=@lastchapter,`size`=`size`+@articleSize,`chapters`=@chapters WHERE `articleid`=@articleid";
					articleParameters = new MySqlParameter[] { new MySqlParameter("@now", now), new MySqlParameter("@lastvolumeid", num2), new MySqlParameter("@lastvolume", text), new MySqlParameter("@lastchapterid", novelInfo_0.LastChapter.PutID), new MySqlParameter("@lastchapter", novelInfo_0.LastChapter.ChapterName), new MySqlParameter("@articleSize", articleSize), new MySqlParameter("@chapters", num), new MySqlParameter("@articleid", novelInfo_0.PutID) };
				}
				else if (IsCms24())
				{
					articleUpdateSql = "UPDATE `jieqi_article_article` SET `lastupdate`=@now,`lastvolumeid`=@lastvolumeid,`lastvolume`=@lastvolume,`lastchapterid`=@lastchapterid,`lastchapter`=@lastchapter,`lastsummary`=@lastsummary,`words`=`words`+@articleSize,`chapters`=@chapters,`freetime`=@now,`freewords`=`freewords`+@articleSize WHERE `articleid`=@articleid";
					articleParameters = new MySqlParameter[] { new MySqlParameter("@now", now), new MySqlParameter("@lastvolumeid", num2), new MySqlParameter("@lastvolume", text), new MySqlParameter("@lastchapterid", novelInfo_0.LastChapter.PutID), new MySqlParameter("@lastchapter", novelInfo_0.LastChapter.ChapterName), new MySqlParameter("@lastsummary", text2), new MySqlParameter("@articleSize", articleSize), new MySqlParameter("@chapters", num), new MySqlParameter("@articleid", novelInfo_0.PutID) };
				}
				else
				{
					articleUpdateSql = "UPDATE `jieqi_article_article` SET `lastupdate`=@now,`lastvolumeid`=@lastvolumeid,`lastvolume`=@lastvolume,`lastchapterid`=@lastchapterid,`lastchapter`=@lastchapter,`lastsummary`=@lastsummary,`size`=`size`+@articleSize,`chapters`=@chapters,`freetime`=@now,`freesize`=`freesize`+@articleSize WHERE `articleid`=@articleid";
					articleParameters = new MySqlParameter[] { new MySqlParameter("@now", now), new MySqlParameter("@lastvolumeid", num2), new MySqlParameter("@lastvolume", text), new MySqlParameter("@lastchapterid", novelInfo_0.LastChapter.PutID), new MySqlParameter("@lastchapter", novelInfo_0.LastChapter.ChapterName), new MySqlParameter("@lastsummary", text2), new MySqlParameter("@articleSize", articleSize), new MySqlParameter("@chapters", num), new MySqlParameter("@articleid", novelInfo_0.PutID) };
				}
				MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, articleUpdateSql, articleParameters);
				mySqlTransaction.Commit();
			}
			catch
			{
				mySqlTransaction.Rollback();
				throw;
			}
		}
		SpiderException.Debug("InsertChapter 生成章节内容Txt文件");
		novelInfo_0.LastChapter.ChapterText = novelInfo_0.LastChapter.ChapterText.Trim();
		if (novelInfo_0.LastChapter.ChapterText.Length > 300 && !novelInfo_0.LastChapter.ChapterText.StartsWith("    "))
		{
			novelInfo_0.LastChapter.ChapterText = "    " + novelInfo_0.LastChapter.ChapterText;
		}
		string text3 = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID;
		WriteChapterTextFile(text3, novelInfo_0.LastChapter.PutID, novelInfo_0.LastChapter.ChapterText);
		return novelInfo_0.LastChapter;
	}
	public NovelInfo InsertNovel(NovelInfo novelInfo_0)
	{
		int coverFlag = CoverImageService.HasUsableCover(novelInfo_0.Cover) ? 1 : 0;
		novelInfo_0.PinYin = "";
		novelInfo_0.PinYinSan = "";
		bool usePinyinDirectory = UsePinyinDirectory();
		bool includeArticleCode = usePinyinDirectory || IsCms18OrNewer();
		int now = FormatText.GetNow();
		MySqlHelper.ExecuteInTransaction(delegate(MySqlTransaction mySqlTransaction)
		{
			MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, BuildNovelInsertSql(usePinyinDirectory), CreateNovelInsertParameters(novelInfo_0, coverFlag, now));
			novelInfo_0.IsNew = false;
			ReadInsertedNovel(mySqlTransaction, novelInfo_0, includeArticleCode);
		});
		CreateIndex(novelInfo_0, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, isDeleteTXT: false, isDeleteHTML: false, 0);
		InsertInnerTags(novelInfo_0);
		if (coverFlag != 0)
		{
			string text2 = Config.ImageDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID;
			CoverImageService.SaveSmallCover(novelInfo_0.Cover, text2, novelInfo_0.PutID);
			novelInfo_0.Cover.Dispose();
		}
		return novelInfo_0;
	}

	public int InsertVolume(NovelInfo novelInfo_0, string string_0)
	{
		return 0;
	}

	private bool isBaiduPush(NovelInfo novelInfo)
	{
		if (!Configs.BaseConfig.IsEnableBaiduPush)
		{
			return false;
		}
		string strBaiduPushDomain = Configs.BaseConfig.StrBaiduPushDomain;
		string strBaiduPushToken = Configs.BaseConfig.StrBaiduPushToken;
		if (strBaiduPushDomain.Length == 0 || strBaiduPushToken.Length == 0)
		{
			return false;
		}
		int intBaiduPushNum = Configs.BaseConfig.IntBaiduPushNum;
		string string_ = "SELECT articleid FROM jieqi_article_article where display=0 Order By toptime desc limit 0," + intBaiduPushNum;
		switch (Configs.BaseConfig.TuijianType)
		{
		case "后台推荐":
			string_ = "SELECT articleid FROM jieqi_article_article where display=0 Order By toptime desc limit 0," + intBaiduPushNum;
			break;
		case "日点击榜":
			string_ = string.Concat(new object[1] { "SELECT articleid FROM jieqi_article_article  where display=0 Order By dayvisit desc limit 0," + intBaiduPushNum });
			break;
		case "周点击榜":
			string_ = string.Concat(new object[1] { "SELECT articleid FROM jieqi_article_article where display=0 Order By weekvisit desc limit 0," + intBaiduPushNum });
			break;
		case "月点击榜":
			string_ = string.Concat(new object[1] { "SELECT articleid FROM jieqi_article_article where display=0 Order By monthvisit desc limit 0," + intBaiduPushNum });
			break;
		case "总点击榜":
			string_ = string.Concat(new object[1] { "SELECT articleid FROM jieqi_article_article where display=0  Order By allvisit desc limit 0," + intBaiduPushNum });
			break;
		case "日投票榜":
			string_ = string.Concat(new object[1] { "SELECT articleid FROM jieqi_article_article where display=0  Order By dayvote desc limit 0," + intBaiduPushNum });
			break;
		case "周投票榜":
			string_ = string.Concat(new object[1] { "SELECT articleid FROM jieqi_article_article where display=0  Order By weekvote desc limit 0," + intBaiduPushNum });
			break;
		case "月投票榜":
			string_ = string.Concat(new object[1] { "SELECT articleid FROM jieqi_article_article where display=0  Order By monthvote desc limit 0," + intBaiduPushNum });
			break;
		case "总投票榜":
			string_ = string.Concat(new object[1] { "SELECT articleid FROM jieqi_article_article where display=0  Order By allvote desc limit 0," + intBaiduPushNum });
			break;
		default:
			string_ = "SELECT articleid FROM jieqi_article_article where display=0 Order By toptime desc limit 0," + intBaiduPushNum;
			break;
		case null:
			break;
		}
		MySqlDataReader mySqlDataReader = null;
		try
		{
			mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, (MySqlParameter[])null);
			ArrayList arrayList = new ArrayList();
			while (mySqlDataReader.Read())
			{
				arrayList.Add(mySqlDataReader["articleid"].ToString());
			}
			return arrayList.Contains(novelInfo.PutID);
		}
		catch (Exception)
		{
			return false;
		}
		finally
		{
			mySqlDataReader.Close();
		}
	}

	private string method_1(string string_0)
	{
		if (!(string_0 != ""))
		{
			return "";
		}
		string_0 = new Regex("(\\[IMG\\])(.[^\\[]*)(\\[\\/IMG\\])").Replace(string_0, "<div align=\"center\"><img src=\"$2\" border=0></div>");
		string_0 = new Regex("(\\[img\\])(.[^\\[]*)(\\[\\/img\\])").Replace(string_0, "<div align=\"center\"><img src=\"$2\" align=\"center\" border=0></div>");
		string_0 = new Regex("(\\[URL\\])(http:\\/\\/.[^\\[]*)(\\[\\/URL\\])").Replace(string_0, "<A HREF=\"$2\" TARGET=_blank>$2</A>");
		string_0 = new Regex("(\\[URL\\])(.[^\\[]*)(\\[\\/URL\\])").Replace(string_0, "<A HREF=\"http://$2\" TARGET=_blank>$2</A>");
		string_0 = new Regex("(\\[URL=(http:\\/\\/.[^\\[]*)\\])(.[^\\[]*)(\\[\\/URL\\])").Replace(string_0, "<A HREF=\"$2\" TARGET=_blank>$3</A>");
		string_0 = new Regex("(\\[URL=(.[^\\[]*)\\])(.[^\\[]*)(\\[\\/URL\\])").Replace(string_0, "<A HREF=\"http://$2\" TARGET=_blank>$3</A>");
		return string_0;
	}

	private string method_2(string string_0, int int_0, string string_1)
	{
		switch (int_0)
		{
		case 1:
			string_0 = string_0.Replace("<br />", "<br />" + method_3(""));
			string_0 = string_0.Replace("<br>", "<br />" + method_3(""));
			string_0 = string_0.Replace("<p>", "<p>" + method_3(""));
			return string_0;
		case 2:
			string_0 = string_0.Replace("<br />", "<br />" + method_3(string_1));
			string_0 = string_0.Replace("<br>", "<br />" + method_3(string_1));
			string_0 = string_0.Replace("<p>", "<p>" + method_3(string_1));
			return string_0;
		default:
			return string_0;
		}
	}

	private string method_3(string string_0)
	{
		string text = string.Empty;
		if (string_0.Equals(""))
		{
			string_0 = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,V,X,Y,Z,~,!,@,#,$,%,^,*,-,+,],[,'";
		}
		for (int i = 1; i < 6; i++)
		{
			string[] array = string_0.Split(',');
			text = text + " " + string_0.Split(',')[new Random().Next(array.Length - 1)];
		}
		return "<div style='display:none'>" + text + "</div>";
	}

	public void PinyinHua(string string_0)
	{
		string string_1 = "select * from `jieqi_article_article` Order By `lastupdate` desc limit 1";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_1, (MySqlParameter[])null);
		if (!mySqlDataReader.Read() || mySqlDataReader.Read())
		{
			try
			{
				string_0 = mySqlDataReader["articlecode"].ToString();
			}
			catch
			{
				if (MessageBox.Show("你的数据库没用对应的拼音字段，是否增加拼音字段到数据库中？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
				{
					string string_2 = "ALTER TABLE `jieqi_article_article` ADD `articlecode` varchar(200) NOT NULL DEFAULT '' AFTER `articlename` ;";
					MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_2, (MySqlParameter[])null);
				}
			}
		}
		mySqlDataReader.Close();
		if (MessageBox.Show("你的数据库中已经添加拼音字段，是否继续拼音化数据库里小说？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
		{
			string string_3 = "select * from jieqi_article_article";
			MySqlDataReader mySqlDataReader2 = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_3, (MySqlParameter[])null);
			while (mySqlDataReader2.Read())
			{
				string text = mySqlDataReader2["articleid"].ToString();
				string string_4 = mySqlDataReader2["articlename"].ToString();
				string string_5 = "UPDATE `jieqi_article_article` SET `articlecode`='" + CHz2Py.Convert4Hz2Py(string_4) + "' WHERE `articleid`=" + text.ToString();
				MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_5, (MySqlParameter[])null);
			}
			mySqlDataReader2.Close();
			MessageBox.Show("拼音化完成");
		}
	}

	public string ReplaceContents(string string_0, NovelInfo novelInfo_0, ChapterInfo chapterInfo_0)
	{
		string text = "";
		string strTask = "";
		try
		{
			string_0 = string_0.Replace("{?$jieqi_top_bar?}", Config.JieqiDefine["JIEQI_TOP_BAR"].ToString());
		}
		catch
		{
			SpiderException.Show("请检查 configs\\define.php 中的 jieqi_top_bar 。请不要包含;", novelInfo_0, bool_0: false, strTask);
		}
		try
		{
			string_0 = string_0.Replace("{?$jieqi_bottom_bar?}", Config.JieqiDefine["JIEQI_BOTTOM_BAR"].ToString());
		}
		catch
		{
			SpiderException.Show("请检查 configs\\define.php 中的 jieqi_bottom_bar 。请不要包含;", novelInfo_0, bool_0: false, strTask);
		}
		try
		{
			string_0 = string_0.Replace("{?$jieqi_banner?}", Config.JieqiDefine["JIEQI_BANNER"].ToString());
		}
		catch
		{
			SpiderException.Show("请检查 configs\\define.php 中的 jieqi_banner 。请不要包含;", novelInfo_0, bool_0: false, strTask);
		}
		try
		{
			string_0 = string_0.Replace("{?$meta_keywords?}", Config.JieqiDefine["JIEQI_META_KEYWORDS"].ToString());
		}
		catch
		{
			SpiderException.Show("请检查 configs\\define.php 中的 meta_keywords 。请不要包含;", novelInfo_0, bool_0: false, strTask);
		}
		try
		{
			string_0 = string_0.Replace("{?$meta_description?}", Config.JieqiDefine["JIEQI_META_DESCRIPTION"].ToString());
		}
		catch
		{
			SpiderException.Show("请检查 configs\\define.php 中的 meta_description 。请不要包含;", novelInfo_0, bool_0: false, strTask);
		}
		try
		{
			string_0 = string_0.Replace("{?$meta_copyright?}", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString());
		}
		catch
		{
			SpiderException.Show("请检查 configs\\define.php 中的 meta_copyright 。请不要包含;", novelInfo_0, bool_0: false, strTask);
		}
		try
		{
			string_0 = string_0.Replace("{?$copy_info?}", Config.JieqiDefine["JIEQI_META_COPYRIGHT"].ToString());
		}
		catch
		{
			SpiderException.Show("请检查 configs\\define.php 中的 copy_info 。请不要包含;", novelInfo_0, bool_0: false, strTask);
		}
		string string_1 = "SELECT lastupdate FROM `jieqi_article_article` WHERE `articleid` = " + novelInfo_0.PutID;
		if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
		{
			string_1 = "SELECT lastupdate,typeid,rgroup FROM `jieqi_article_article` WHERE `articleid` = " + novelInfo_0.PutID;
		}
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_1, (MySqlParameter[])null);
		if (mySqlDataReader.Read())
		{
			string text2 = mySqlDataReader["lastupdate"].ToString();
			string_0 = string_0.Replace("{?$lastupdate?}", FromUnixTimestamp(text2).ToString());
			if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
			{
				if (mySqlDataReader["typeid"] != null)
				{
					novelInfo_0.SmallSortID = Convert.ToInt32(mySqlDataReader["typeid"]);
				}
				else
				{
					novelInfo_0.SmallSortID = 0;
				}
				if (novelInfo_0.SmallSortID >= Config.JieqiSorttypes[novelInfo_0.LagerSortID].Length || novelInfo_0.SmallSortID <= 0)
				{
					novelInfo_0.SmallSortID = 0;
				}
				novelInfo_0.SmallSort = Config.JieqiSorttypes[novelInfo_0.LagerSortID][novelInfo_0.SmallSortID];
				novelInfo_0.IsboyID = Convert.ToInt32(mySqlDataReader["rgroup"]);
			}
		}
		mySqlDataReader.Close();
		string_0 = string_0.Replace("{?$jieqi_murl?}", Config.JieqiDefine["JIEQI_MOBILE_LOCATION"].ToString());
		string_0 = string_0.Replace("{?$jieqi_sitename?}", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString());
		string_0 = string_0.Replace("{?$sitename?}", Config.JieqiDefine["JIEQI_SITE_NAME"].ToString());
		string_0 = string_0.Replace("{?$jieqi_url?}", Config.JieqiDefine["JIEQI_URL"].ToString());
		string_0 = string_0.Replace("{?$jieqi_main_url?}", Config.JieqiDefine["JIEQI_URL"].ToString());
		string_0 = ((!(Config.JieqiArticleConfigs["staticurl"].ToString() == "")) ? string_0.Replace("{?$new_url?}", Config.JieqiArticleConfigs["staticurl"].ToString()) : string_0.Replace("{?$new_url?}", Config.JieqiDefine["JIEQI_URL"].ToString()));
		if (Configs.BaseConfig.CmsVersion != "1.5")
		{
			string_0 = ((!(Config.JieqiArticleConfigs["dynamicurl"].ToString() == "")) ? string_0.Replace("{?$dynamic_url?}", Config.JieqiArticleConfigs["dynamicurl"].ToString()) : string_0.Replace("{?$dynamic_url?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article"));
			string_0 = ((!(Config.JieqiArticleConfigs["staticurl"].ToString() == "")) ? string_0.Replace("{?$static_url?}", Config.JieqiArticleConfigs["staticurl"].ToString()) : string_0.Replace("{?$static_url?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article"));
		}
		string_0 = string_0.Replace("{?$jieqi_local_url?}", Config.JieqiDefine["JIEQI_URL"].ToString());
		string_0 = ((!(Config.JieqiArticleConfigs["dynamicurl"].ToString() == "")) ? string_0.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiArticleConfigs["dynamicurl"].ToString()) : string_0.Replace("{?$jieqi_modules['article']['url']?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article"));
		string_0 = string_0.Replace("{?$url_read?}", "");
		string_0 = string_0.Replace("{?$author|urlencode?}", UrlEncode(novelInfo_0.Author));
		string_0 = string_0.Replace("{?$author?}", novelInfo_0.Author);
		string_0 = string_0.Replace("{?$article_title?}", novelInfo_0.Name);
		string_0 = string_0.Replace("{?$book_title?}", novelInfo_0.Name);
		if (novelInfo_0.Intro != null)
		{
			string_0 = string_0.Replace("'..'?}", "\"..\"?}");
			string_0 = Regex.Replace(string_0, "\\{\\?\\$intro\\|truncate:(\\d*):\"(.+?)\"\\?\\}", "{?$intro|truncate:$1?}$2", RegexOptions.Singleline);
			MatchCollection matchCollection = new Regex("{\\?\\$intro\\|truncate:(\\d*)\\?\\}").Matches(string_0);
			if (matchCollection.Count > 0)
			{
				for (int i = 0; i < matchCollection.Count; i++)
				{
					int num = Convert.ToInt32(matchCollection[i].Groups[1].Value);
					int length = novelInfo_0.Intro.Length;
					int length2 = ((length >= num) ? num : length);
					string_0 = string_0.Replace("{?$intro|truncate:" + num + "?}", novelInfo_0.Intro.Replace("\\n", "").Substring(0, length2));
				}
			}
			string_0 = string_0.Replace("{?$intro?}", novelInfo_0.Intro);
		}
		string_0 = string_0.Replace("{?$jieqi_title?}", chapterInfo_0.VolumeName + " " + chapterInfo_0.ChapterName);
		string_0 = string_0.Replace("{?$chaptername?}", chapterInfo_0.ChapterName);
		string_0 = string_0.Replace("{?$jieqi_volume?}", chapterInfo_0.VolumeName);
		string_0 = string_0.Replace("{?$jieqi_chapter?}", chapterInfo_0.ChapterName);
		if (chapterInfo_0.ChapterName != null)
		{
			string_0 = Regex.Replace(string_0, "\\{\\?\\$jieqi_chapter\\|truncate:(\\d*):\"(.+?)\"\\?\\}", "{?$intro|truncate:$1?}$2", RegexOptions.Singleline);
			MatchCollection matchCollection2 = new Regex("{\\?\\$jieqi_chapter\\|truncate:(\\d*)\\?\\}").Matches(string_0);
			if (matchCollection2.Count > 0)
			{
				for (int j = 0; j < matchCollection2.Count; j++)
				{
					int num2 = Convert.ToInt32(matchCollection2[j].Groups[1].Value);
					string_0 = string_0.Replace("{?$jieqi_chapter|truncate:" + num2 + "?}", chapterInfo_0.ChapterName.Substring(0, num2));
				}
			}
		}
		string text3 = global::Class1.smethod_0(Regex.Replace(chapterInfo_0.ChapterName, "([第]*[两|一|二|三|四|五|六|七|八|九|〇|壹|贰|叁|肆|伍|陆|柒|捌|玖|零|十|拾|百|佰|千|万|1|2|3|4|5|6|7|8|9|0]*[卷])", "")).Replace("第", "").Replace("♀", "")
			.Replace("【Princess】", "")
			.Replace("（", "")
			.Replace("【", "")
			.Replace("chapter ", "")
			.Replace("Chapter ", "")
			.Replace("chapter", "")
			.Replace("Chapter", "")
			.Replace("NO.", "")
			.Replace("O.", "")
			.Replace("No.", "")
			.Replace("part ", "")
			.Replace("Part ", "")
			.Replace("part", "")
			.Replace("Part", "");
		Regex regex = new Regex("\\d");
		int index = regex.Match(text3).Index;
		if (index > 0)
		{
			text3 = text3.Remove(0, index);
		}
		Regex regex2 = new Regex("\\D");
		int index2 = regex2.Match(text3).Index;
		if (index2 > 0)
		{
			text3 = text3.Substring(0, index2);
		}
		text3 = Regex.Replace(text3, "[^0-9]", "");
		string_0 = string_0.Replace("{?$chaptersum?}", text3);
		if (novelInfo_0.LagerSortID == 0)
		{
			novelInfo_0.LagerSortID = 1;
		}
		if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
		{
			string_0 = string_0.Replace("{?function jieqi_geturl:'article':'articlelist':1:$sortid?}", Config.JieqiArticleConfigs["faketoplist"].ToString().Replace("<{$order}>", novelInfo_0.LagerSortID.ToString()).Replace("<{$page}>", "1"));
			string_0 = string_0.Replace("{?function jieqi_geturl:'article':'article':$articleid:'info'?}", Config.JieqiArticleConfigs["fakeinfo"].ToString().Replace("<{$id}>", novelInfo_0.PutID.ToString()).Replace("<{$aid|subdirectory}>", "/" + Convert.ToInt32(novelInfo_0.PutID / 1000)));
		}
		string_0 = string_0.Replace("{?$rgroup_n?}", novelInfo_0.IsboyID.ToString());
		string_0 = string_0.Replace("{?$rgroup?}", (novelInfo_0.IsboyID == 1) ? "男生" : "女生");
		string_0 = string_0.Replace("{?$rgroup_url?}", (novelInfo_0.IsboyID == 1) ? "gg" : "mm");
		string_0 = string_0.Replace("{?$sortcode?}", Config.JieqiSortcode[novelInfo_0.LagerSortID]);
		string_0 = string_0.Replace("{NovelId}", "{?$articleid?}");
		string_0 = string_0.Replace("{NovelId/1000}", Convert.ToInt32(novelInfo_0.PutID / 1000).ToString());
		string_0 = string_0.Replace("{?$sortname?}", Config.JieqiSort[novelInfo_0.LagerSortID]);
		string_0 = string_0.Replace("{?$sort?}", Config.JieqiSort[novelInfo_0.LagerSortID]);
		string_0 = string_0.Replace("{?$sortid?}", novelInfo_0.LagerSortID.ToString());
		string_0 = string_0.Replace("{?$jieqi_charset?}", Config.JieqiCharset);
		string_0 = string_0.Replace("{?$meta_author?}", Config.JieqiAuthor);
		string_0 = string_0.Replace("{?$url_fullpage?}", Config.FullUrl + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + Config.JieqiArticleConfigs["htmlfile"].ToString());
		string_0 = string_0.Replace("{?$url_download?}", Config.ZipUrl + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + ".zip");
		string_0 = string_0.Replace("{?$url_fullpage?}", novelInfo_0.PutID + Config.JieqiArticleConfigs["htmlfile"].ToString());
		string_0 = string_0.Replace("{?$articleid?}", novelInfo_0.PutID.ToString());
		string_0 = string_0.Replace("{?$article_id|subdirectory?}", "/" + Convert.ToString(novelInfo_0.PutID / 1000));
		string_0 = string_0.Replace("{?$articleid|subdirectory?}", "/" + Convert.ToString(novelInfo_0.PutID / 1000));
		string_0 = string_0.Replace("{?$url_preview?}", "{?$preview_page?}").Replace("{?$url_next?}", "{?$next_page?}").Replace("{?$url_index?}", "{?$index_page?}");
		string_0 = string_0.Replace("{?$preview_page?}", "index" + Config.JieqiArticleConfigs["htmlfile"].ToString());
		string_0 = string_0.Replace("{?$next_page?}", "index" + Config.JieqiArticleConfigs["htmlfile"].ToString());
		string_0 = string_0.Replace("{?$index_page?}", "index" + Config.JieqiArticleConfigs["htmlfile"].ToString());
		string_0 = string_0.Replace("{?$article_id?}", novelInfo_0.PutID.ToString());
		string_0 = string_0.Replace("{?$articleid?}", novelInfo_0.PutID.ToString());
		string_0 = string_0.Replace("{?$chapter_id?}", chapterInfo_0.PutID.ToString());
		string_0 = string_0.Replace("{?$chapterid?}", chapterInfo_0.PutID.ToString());
		string_0 = string_0.Replace("{?$pinyin?}", novelInfo_0.PinYin.ToString());
		string_0 = string_0.Replace("{?$pinyin|3?}", novelInfo_0.PinYinSan.ToString());
		string_0 = string_0.Replace("{?$fullflaga?}", (novelInfo_0.Degree == 0) ? "连载中" : "完结");
		string_0 = string_0.Replace("{?$fullflag?}", (novelInfo_0.Degree == 0) ? "连载中" : "完结");
		string_0 = string_0.Replace("{?if $fullflag==1?} wanjieIco{?/if?}", " wanjieIco");
		string_0 = string_0.Replace("{?$fullflagaid?}", novelInfo_0.Degree.ToString());
		string_0 = string_0.Replace("{?$url_simage?}", (novelInfo_0.ImgFlag == 0) ? (Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article/images/nocover.jpg") : (Config.ImageUrl + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID + "/" + novelInfo_0.PutID + "s.jpg"));
		string_0 = string_0.Replace("{?$simage?}", novelInfo_0.ImgFlag.ToString());
		string_0 = string_0.Replace("{?$size?}", novelInfo_0.Size.ToString());
		string_0 = string_0.Replace("{?$size_c?}", Convert.ToString(novelInfo_0.Size / 2));
		string_0 = string_0.Replace("{?$words?}", Convert.ToString(novelInfo_0.Size));
		if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
		{
			string_0 = string_0.Replace("{?$typeid?}", novelInfo_0.SmallSortID.ToString());
			string_0 = string_0.Replace("{?$typename?}", novelInfo_0.SmallSort);
			string_0 = string_0.Replace("{?$type?}", novelInfo_0.SmallSort);
			string_0 = string_0.Replace("{?$url_preview?}", "index" + Config.JieqiArticleConfigs["htmlfile"].ToString());
			string_0 = string_0.Replace("{?$url_next?}", "index" + Config.JieqiArticleConfigs["htmlfile"].ToString());
			string_0 = string_0.Replace("{?$url_index?}", "index" + Config.JieqiArticleConfigs["htmlfile"].ToString());
		}
		MatchCollection matchCollection3 = new Regex("\\{\\?\\$lastupdate(\\s*|.+?[^\\?])\\?\\}").Matches(string_0);
		if (matchCollection3.Count > 0)
		{
			for (int k = 0; k < matchCollection3.Count; k++)
			{
				string value2 = matchCollection3[k].Groups[1].Value;
				string text4 = value2.Replace("y", "yy").Replace("Y", "yyyy").Replace("m", "MM")
					.Replace("d", "dd")
					.Replace("H", "HH")
					.Replace("h", "hh")
					.Replace("i", "mm")
					.Replace("s", "ss")
					.Replace("|ddate:", "")
					.Replace("'", "");
				string_0 = string_0.Replace(newValue: FromUnixTimestamp(novelInfo_0.LastupDate).ToString((text4 == "") ? "MM-dd" : text4), oldValue: "{?$lastupdate" + value2 + "?}");
			}
		}
		MatchCollection matchCollection4 = new Regex("\\{\\?\\$postdate(\\s*|.+?[^\\?])\\?\\}").Matches(string_0);
		if (matchCollection4.Count > 0)
		{
			for (int l = 0; l < matchCollection4.Count; l++)
			{
				string value3 = matchCollection4[l].Groups[1].Value;
				string text5 = value3.Replace("y", "yy").Replace("Y", "yyyy").Replace("m", "MM")
					.Replace("d", "dd")
					.Replace("H", "HH")
					.Replace("h", "hh")
					.Replace("i", "mm")
					.Replace("s", "ss")
					.Replace("|ddate:", "")
					.Replace("'", "");
				string_0 = string_0.Replace(newValue: FromUnixTimestamp(novelInfo_0.PostDate).ToString((text5 == "") ? "MM-dd" : text5), oldValue: "{?$postdate" + value3 + "?}");
			}
		}
		string_0 = string_0.Replace("{?$articlesubdir?}", "/" + novelInfo_0.PutID / 1000);
		string_0 = string_0.Replace("{?$url_bookroom?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article/");
		string_0 = string_0.Replace("{?$articlename?}", novelInfo_0.Name);
		if (chapterInfo_0.PutID > 0)
		{
			MatchCollection matchCollection5 = new Regex("\\{\\?\\$chaptertime(\\s*|.+?[^\\?])\\?\\}").Matches(string_0);
			if (matchCollection5.Count > 0)
			{
				for (int l = 0; l < matchCollection5.Count; l++)
				{
					string value3 = matchCollection5[l].Groups[1].Value;
					string text5 = value3.Replace("y", "yy").Replace("Y", "yyyy").Replace("m", "MM")
						.Replace("d", "dd")
						.Replace("H", "HH")
						.Replace("h", "hh")
						.Replace("i", "mm")
						.Replace("s", "ss")
						.Replace("|ddate:", "")
						.Replace("'", "");
					string_0 = string_0.Replace(newValue: FromUnixTimestamp(novelInfo_0.LastupDate).ToString((text5 == "") ? "MM-dd" : text5), oldValue: "{?$chaptertime" + value3 + "?}");
				}
			}
			if (chapterInfo_0.ChapterText != null)
			{
				string_0 = string_0.Replace("{?$chaptersize_c?}", chapterInfo_0.ChapterText.Length.ToString()).Replace("{?$chapterwords?}", chapterInfo_0.ChapterText.Length.ToString());
			}
		}
		string_0 = ((Config.JieqiArticleConfigs["fakeinfo"].ToString() == "0") ? string_0.Replace("{?$url_articleinfo?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/modules/article/articleinfo.php?id=" + novelInfo_0.PutID) : ((!(double.Parse(Configs.BaseConfig.CmsVersion) < 1.6)) ? string_0.Replace("{?$url_articleinfo?}", Config.JieqiDefine["JIEQI_URL"].ToString() + Config.JieqiArticleConfigs["fakeinfo"].ToString().Replace("<{$id}>", novelInfo_0.PutID.ToString()).Replace("<{$aid|subdirectory}>", "/" + Convert.ToString(novelInfo_0.PutID / 1000))
			.Replace("<{$id|subdirectory}>", "/" + Convert.ToString(novelInfo_0.PutID / 1000))) : ((!(Config.JieqiArticleConfigs["fakeprefix"].ToString() == "")) ? string_0.Replace("{?$url_articleinfo?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/" + Config.JieqiArticleConfigs["fakeprefix"].ToString() + "info/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + (string)Config.JieqiArticleConfigs["fakefile"]) : string_0.Replace("{?$url_articleinfo?}", Config.JieqiDefine["JIEQI_URL"].ToString() + "/files/article/info/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + (string)Config.JieqiArticleConfigs["fakefile"]))));
		string_0 = string_0.Replace("{novelname}", novelInfo_0.Name);
		string_0 = string_0.Replace("{NovelName}", novelInfo_0.Name);
		string_0 = string_0.Replace("{?$articlename|urlencode?}", "");
		if (string.IsNullOrEmpty(novelInfo_0.Keyword) && !string.IsNullOrEmpty(novelInfo_0.Name))
		{
			string_0 = string_0.Replace("{?$novel_keywords?}", novelInfo_0.Name.ToString());
			return string_0;
		}
		if (!string.IsNullOrEmpty(novelInfo_0.Keyword))
		{
			string_0 = string_0.Replace("{?$novel_keywords?}", novelInfo_0.Keyword.ToString());
		}
		return string_0;
	}

	public NovelInfo Sort(NovelInfo novelInfo_0)
	{
		return novelInfo_0;
	}

	public void UpdateChapter(NovelInfo novelInfo_0, ReplaceConfigInfo replaceConfigInfo_0)
	{
		bool updateChapterName = replaceConfigInfo_0.UpdateChapterName;
		bool modernCms = double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8;
		if (updateChapterName || modernCms)
		{
			MySqlHelper.ExecuteInTransaction(delegate(MySqlTransaction mySqlTransaction)
			{
				if (updateChapterName)
				{
					MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, "UPDATE `jieqi_article_chapter` SET `chaptername`=@chaptername,`lastupdate`=@lastupdate WHERE `articleid`=@articleid AND `chapterid`=@chapterid",
						new MySqlParameter("@chaptername", novelInfo_0.LastChapter.ChapterName),
						new MySqlParameter("@lastupdate", FormatText.GetTime(novelInfo_0.LastChapter.LastTime)),
						new MySqlParameter("@articleid", novelInfo_0.PutID),
						new MySqlParameter("@chapterid", novelInfo_0.LastChapter.PutID));
					return;
				}
				MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, "UPDATE `jieqi_article_chapter` SET `chaptername`=@chaptername WHERE `chapterid`=@chapterid",
					new MySqlParameter("@chaptername", novelInfo_0.LastChapter.ChapterName),
					new MySqlParameter("@chapterid", novelInfo_0.LastChapter.PutID));
			});
		}
		if (Configs.BaseConfig.TextMarkOfData)
		{
			novelInfo_0.LastChapter = FormatText.TextMark(novelInfo_0.LastChapter);
		}
		novelInfo_0.LastChapter.ChapterText = FormatText.Typesetting(novelInfo_0.LastChapter.ChapterText);
		novelInfo_0.LastChapter.ChapterText = novelInfo_0.LastChapter.ChapterText.Replace("\n", "\r\n");
		string empty = string.Empty;
		if (modernCms)
		{
			empty = string.Empty;
			novelInfo_0.Lastsummary = string.Empty;
		}
		SpiderException.Debug("UpdateChapter 生成章节内容Txt文件");
		novelInfo_0.LastChapter.ChapterText = novelInfo_0.LastChapter.ChapterText.Trim();
		if (novelInfo_0.LastChapter.ChapterText.Length > 300 && !novelInfo_0.LastChapter.ChapterText.StartsWith("    "))
		{
			novelInfo_0.LastChapter.ChapterText = "    " + novelInfo_0.LastChapter.ChapterText;
		}
		string text = Config.TxtDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID;
		WriteChapterTextFile(text, novelInfo_0.LastChapter.PutID, novelInfo_0.LastChapter.ChapterText);
	}

	public int[] UpdateChapterOrder(NovelInfo novelInfo_0, int int_0, int int_1)
	{
		int[] array = new int[2];
		string string_ = "SELECT `chapterid`,`chaptername`,`chapterorder` FROM `jieqi_article_chapter` WHERE `chapterid`=@chapterid";
		MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionString, CommandType.Text, string_, new MySqlParameter("@chapterid", int_1));
		if (mySqlDataReader.Read())
		{
			array[0] = Convert.ToInt32(mySqlDataReader["chapterorder"].ToString());
		}
		else
		{
			array[0] = 0;
		}
		mySqlDataReader.Close();
		string string_2 = "SELECT `chapterid` FROM `jieqi_article_chapter` WHERE `chapterorder`<@chapterorder AND `articleid`=@articleid AND `chaptertype`='0' ORDER BY `chapterorder` DESC LIMIT 1";
		object obj = MySqlHelper.ExecuteScalar(MySqlHelper.ConnectionString, CommandType.Text, string_2, new MySqlParameter("@chapterorder", array[0]), new MySqlParameter("@articleid", novelInfo_0.PutID));
		if (obj != DBNull.Value)
		{
			array[1] = Convert.ToInt32(obj);
		}
		else
		{
			array[1] = -1;
		}
		if (array[0] > 0)
		{
			MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, "UPDATE `jieqi_article_chapter` SET `chapterorder`=`chapterorder`+@delta WHERE `articleid`=@articleid AND `chapterorder`>=@chapterorder",
				new MySqlParameter("@delta", int_0),
				new MySqlParameter("@articleid", novelInfo_0.PutID),
				new MySqlParameter("@chapterorder", array[0]));
		}
		return array;
	}

	public void UpdateLastChapter(NovelInfo novelInfo_0)
	{
		MySqlHelper.ExecuteInTransaction(delegate(MySqlTransaction mySqlTransaction)
		{
			DataTable dataTable = MySqlHelper.ExecuteDataTable(mySqlTransaction, CommandType.Text, "SELECT `chapterid`,`chaptername`,`chapterorder` FROM `jieqi_article_chapter` WHERE `articleid`=@articleid ORDER BY `chapterorder` DESC LIMIT 1", new MySqlParameter("@articleid", novelInfo_0.PutID));
			if (dataTable.Rows.Count == 0)
			{
				return;
			}
			DataRow dataRow = dataTable.Rows[0];
			MySqlHelper.ExecuteNonQuery(mySqlTransaction, CommandType.Text, "UPDATE `jieqi_article_article` SET `lastupdate`=@lastupdate,`lastvolumeid`=0,`lastvolume`='',`lastchapterid`=@lastchapterid,`lastchapter`=@lastchapter,`lastsummary`='',`chapters`=@chapters WHERE `articleid`=@articleid",
				new MySqlParameter("@lastupdate", FormatText.GetNow()),
				new MySqlParameter("@lastchapterid", dataRow["chapterid"]),
				new MySqlParameter("@lastchapter", dataRow["chaptername"]),
				new MySqlParameter("@chapters", dataRow["chapterorder"]),
				new MySqlParameter("@articleid", novelInfo_0.PutID));
		});
	}

	public void UpdateLastChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0)
	{
	}

	public void UpdateNovel(NovelInfo novelInfo_0, bool bool_0, bool bool_1, bool bool_2, bool bool_3, bool bool_4, bool bool_5, bool bool_6)
	{
		if (novelInfo_0.ReviseChapter == null)
		{
			string string_ = "SELECT `articleid` FROM `jieqi_article_article` WHERE `articlename`=@articlename AND `author`=@author";
			object obj = MySqlHelper.ExecuteScalar(MySqlHelper.ConnectionString, CommandType.Text, string_, new MySqlParameter("@articlename", novelInfo_0.Name), new MySqlParameter("@author", novelInfo_0.Author));
			if (obj == null || obj == DBNull.Value)
			{
				return;
			}
			novelInfo_0.PutID = Convert.ToInt32(obj);
		}
		int num = 0;
		if (novelInfo_0.Degree != 1 && bool_5)
		{
			string text = Config.ImageDir + "/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID;
			CoverImageService.SaveSmallCover(novelInfo_0.Cover, text, novelInfo_0.PutID);
			novelInfo_0.Cover.Dispose();
			num = 1;
		}
		StringBuilder updateBuilder = new StringBuilder("UPDATE `jieqi_article_article` SET `articlename`=@articlename");
		List<MySqlParameter> updateParameters = new List<MySqlParameter>
		{
			new MySqlParameter("@articlename", novelInfo_0.Name),
			new MySqlParameter("@articleid", novelInfo_0.PutID)
		};
		if (novelInfo_0.Author != null && bool_0)
		{
			updateBuilder.Append(",`author`=@author");
			updateParameters.Add(new MySqlParameter("@author", novelInfo_0.Author.ToString()));
		}
		if (novelInfo_0.MIntro != null && bool_1)
		{
			updateBuilder.Append(",`intro`=@intro");
			updateParameters.Add(new MySqlParameter("@intro", novelInfo_0.MIntro.ToString()));
		}
		if (novelInfo_0.LagerSort != null && bool_3)
		{
			updateBuilder.Append(",`sortid`=@sortid");
			updateParameters.Add(new MySqlParameter("@sortid", novelInfo_0.MLagerSortID));
			novelInfo_0.LagerSortID = novelInfo_0.MLagerSortID;
			novelInfo_0.LagerSort = novelInfo_0.MLagerSort;
		}
		if (bool_2)
		{
			updateBuilder.Append(",`fullflag`=@fullflag");
			updateParameters.Add(new MySqlParameter("@fullflag", novelInfo_0.MDegree));
		}
		if (novelInfo_0.Keyword != null && bool_6)
		{
			updateBuilder.Append(",`keywords`=@keywords");
			updateParameters.Add(new MySqlParameter("@keywords", novelInfo_0.Keyword.ToString()));
		}
		if (novelInfo_0.Cover != null && bool_5)
		{
			updateBuilder.Append(",`imgflag`=@imgflag");
			updateParameters.Add(new MySqlParameter("@imgflag", num));
		}
		updateBuilder.Append(" WHERE `articleid`=@articleid");
		MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, updateBuilder.ToString(), updateParameters.ToArray());
		if (novelInfo_0.ReviseChapter != null)
		{
			string string_3 = "UPDATE `jieqi_article_chapter` SET `chaptername`=@chaptername WHERE `chapterid`=@chapterid";
			MySqlHelper.ExecuteNonQuery(MySqlHelper.ConnectionString, CommandType.Text, string_3, new MySqlParameter("@chaptername", novelInfo_0.ReviseChapter), new MySqlParameter("@chapterid", novelInfo_0.ReviseChapterID));
			ChapterInfo chapterInfo = new ChapterInfo
			{
				PutID = novelInfo_0.ReviseChapterID
			};
			ChapterInfo chapterInfo_ = chapterInfo;
			createChapter(novelInfo_0, chapterInfo_, bool_0: true);
		}
		if (isBaiduPush(novelInfo_0))
		{
			string strBaiduPushURL = Configs.BaseConfig.StrBaiduPushURL;
			string pinYin = novelInfo_0.PinYin;
			string newValue = novelInfo_0.LastChapter.PutID.ToString();
			string text3 = novelInfo_0.PutID.ToString();
			BaiduPush(strBaiduPushURL.Replace("{LastChapterId}", newValue).Replace("{Pinyin}", pinYin.ToString()).Replace("{Pinyin/3}", pinYin.Substring(0, 3).ToString())
				.Replace("{NovelId}", text3)
				.Replace("{NovelId/1000}", Convert.ToInt32(int.Parse(text3) / 1000).ToString()));
		}
	}

	public void UpdateVolume(NovelInfo novelInfo_0, int int_0, string string_0)
	{
	}

	public static string UrlEncode(string str)
	{
		StringBuilder stringBuilder = new StringBuilder();
		byte[] bytes = FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk").GetBytes(str);
		for (int i = 0; i < bytes.Length; i++)
		{
			stringBuilder.Append("%" + Convert.ToString(bytes[i], 16));
		}
		return stringBuilder.ToString();
	}
}
