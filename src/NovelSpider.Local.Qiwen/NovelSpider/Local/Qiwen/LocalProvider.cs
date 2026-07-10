using System;
using System.Collections;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;

namespace NovelSpider.Local.Qiwen;

public class LocalProvider : ILocalProvider
{
	private static object object_0 = new object();

	public LocalProvider()
	{
		Config.LoadConfig();
		SpiderException.Debug("Qiwen LocalProvider");
	}

	public void ClearNovel(NovelInfo novelInfo)
	{
		int putID = novelInfo.PutID;
		string cmdText = "SELECT id From Ws_BookChapter_" + putID % 10 + " where bookid=" + putID;
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		while (sqlDataReader.Read())
		{
			if (File.Exists(Config.WaterSoftPath + "\\files\\article\\txt\\" + putID / 1000 + "\\" + putID + "\\" + sqlDataReader["id"].ToString() + ".txt"))
			{
				File.Delete(Config.WaterSoftPath + "\\files\\article\\txt\\" + putID / 1000 + "\\" + putID + "\\" + sqlDataReader["id"].ToString() + ".txt");
			}
		}
		sqlDataReader.Close();
		cmdText = "Delete Ws_BookChapter_" + putID % 10 + " where bookid=" + putID;
		SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		cmdText = "Delete Ws_BookVolume where bookid=" + putID;
		SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		cmdText = "SELECT booktxt From Ws_BookList where id = " + putID;
		Convert.ToBoolean(SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null));
		if (File.Exists(Config.WaterSoftPath + "\\files\\article\\txtfull\\" + putID / 1000 + "\\" + novelInfo.Name + ".txt"))
		{
			File.Delete(Config.WaterSoftPath + "\\files\\article\\txtfull\\" + putID / 1000 + "\\" + novelInfo.Name + ".txt");
		}
		if (File.Exists(Config.WaterSoftPath + "\\files\\article\\image\\" + putID / 1000 + "\\" + putID + "\\" + putID + "s.jpg"))
		{
			File.Delete(Config.WaterSoftPath + "\\files\\article\\image\\" + putID / 1000 + "\\" + putID + "\\" + putID + "s.jpg");
		}
	}

	public void CreateChapter(NovelInfo novelInfo_0)
	{
		CreateChapter(novelInfo_0, novelInfo_0.LastChapter, bool_0: true);
	}

	public void CreateChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0)
	{
		CreateChapter(novelInfo_0, chapterInfo_0, bool_0: true);
	}

	private void CreateChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, bool bool_0)
	{
		SpiderException.Debug("CreateChapter 初始化模板");
		string text = Config.TempletsContent;
		SpiderException.Debug("CreateChapter 翻页数据 Ws_BookChapter_AndVolume_SelectAll");
		string cmdText = "Ws_BookChapter_AndVolume_SelectAll";
		SqlParameter[] commandParameters = new SqlParameter[1]
		{
			new SqlParameter("@bookid ", novelInfo_0.PutID)
		};
		DataTable dataTable = SqlHelper.ExecuteDataTable(SqlHelper.ConnectionString, CommandType.StoredProcedure, cmdText, commandParameters);
		int num = 0;
		string newValue = string.Empty;
		string newValue2 = string.Empty;
		int num2 = 0;
		string newValue3 = string.Empty;
		string newValue4 = string.Empty;
		string newValue5 = string.Empty;
		string newValue6 = string.Empty;
		for (int i = 0; i < dataTable.Rows.Count; i++)
		{
			if (Convert.ToInt32(dataTable.Rows[i][0]) == chapterInfo_0.PutID)
			{
				if (i < dataTable.Rows.Count - 1)
				{
					newValue5 = "下一章：<a href=\"" + dataTable.Rows[i + 1]["id"].ToString() + ".{WSExtName}\">" + dataTable.Rows[i + 1]["booktexttitle"].ToString() + "</a>";
					newValue2 = dataTable.Rows[i + 1]["id"].ToString() + ".{WSExtName}";
					newValue = "<a href=\"" + dataTable.Rows[i + 1]["id"].ToString() + ".{WSExtName}\">翻下页</a>";
					num = Convert.ToInt32(dataTable.Rows[i + 1]["id"]);
					text = text.Replace("{WsDownID}", dataTable.Rows[i + 1]["id"].ToString());
				}
				else
				{
					newValue5 = "至尾页";
					newValue2 = Config.SysInfo["RootFolder"] + "Book/ReadEnd.aspx?id=" + novelInfo_0.PutID;
					newValue = "<a href=\"" + Config.SysInfo["RootFolder"] + "Book/ReadEnd.aspx?id=" + novelInfo_0.PutID + "\">至尾页</a>";
					num = 0;
					text = text.Replace("{WsDownID}", Config.BookInfo["BookHtmlNclassName"] + "." + Config.BookInfo["BookHtmlExtName"]);
				}
				if (i > 0)
				{
					newValue6 = "上一章：<a href=\"" + dataTable.Rows[i - 1]["id"].ToString() + ".{WSExtName}\">" + dataTable.Rows[i - 1]["booktexttitle"].ToString() + "</a>";
					newValue4 = dataTable.Rows[i - 1]["id"].ToString() + ".{WSExtName}";
					newValue3 = "<a href=\"" + dataTable.Rows[i - 1]["id"].ToString() + ".{WSExtName}\">翻上页</a>";
					num2 = Convert.ToInt32(dataTable.Rows[i - 1]["id"]);
					text = text.Replace("{WsUpID}", dataTable.Rows[i - 1]["id"].ToString());
				}
				else
				{
					newValue6 = "无上页";
					newValue4 = Config.BookInfo["BookHtmlNclassName"] + ".{WSExtName}";
					newValue3 = "无上页";
					num2 = 0;
					text = text.Replace("{WsUpID}", Config.BookInfo["BookHtmlNclassName"] + "." + Config.BookInfo["BookHtmlExtName"]);
				}
				break;
			}
		}
		dataTable.Clear();
		dataTable.Dispose();
		SpiderException.Debug("CreateChapter 小说数据 Ws_BookList_Select_A_MakeHtml");
		string text2 = "";
		cmdText = "Ws_BookList_Select_A_MakeHtml";
		commandParameters = new SqlParameter[1]
		{
			new SqlParameter("@id ", novelInfo_0.PutID)
		};
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.StoredProcedure, cmdText, commandParameters);
		if (sqlDataReader.Read())
		{
			text2 = sqlDataReader["bookhtml"].ToString();
			novelInfo_0.Name = sqlDataReader["booktitle"].ToString();
			novelInfo_0.Author = sqlDataReader["bookwriter"].ToString();
			novelInfo_0.LagerSort = sqlDataReader["tclass"].ToString();
			novelInfo_0.LagerSortID = Convert.ToInt32(sqlDataReader["tclassid"]);
			novelInfo_0.SmallSort = sqlDataReader["nclass"].ToString();
			novelInfo_0.SmallSortID = Convert.ToInt32(sqlDataReader["nclassid"]);
		}
		sqlDataReader.Close();
		SpiderException.Debug("CreateChapter 章节数据 Ws_BookList_Select_A_MakeHtml");
		cmdText = "Ws_BookChapter_Select_A_MakeHtml";
		commandParameters = new SqlParameter[1]
		{
			new SqlParameter("@id ", chapterInfo_0.PutID)
		};
		sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.StoredProcedure, cmdText, commandParameters);
		if (sqlDataReader.Read())
		{
			chapterInfo_0.ChapterName = sqlDataReader["booktexttitle"].ToString();
			chapterInfo_0.VolumeName = sqlDataReader["nclass"].ToString();
			string text3 = sqlDataReader["booktexttype"].ToString();
			string value = text3;
			if (!string.IsNullOrEmpty(value))
			{
				if (File.Exists(Config.WaterSoftPath + text3))
				{
					chapterInfo_0.ChapterText = File.ReadAllText(Config.WaterSoftPath + text3, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
				}
				else
				{
					SpiderException.Show(chapterInfo_0.ChapterName + " 缺少war文件 " + text3, bool_0: true);
					chapterInfo_0.ChapterText = "";
				}
			}
			else
			{
				SpiderException.Show(chapterInfo_0.ChapterName + " 数据库中没有记录对应的war文件位置", bool_0: true);
				chapterInfo_0.ChapterText = "";
			}
		}
		sqlDataReader.Close();
		SpiderException.Debug("CreateChapter 模板替换");
		text = text.Replace("{WsHomeTitle}", Config.SysInfo["HomeTitle"]).Replace("{WsPageText}", Config.SysInfo["HomePageText"]).Replace("{WsKeywords}", Config.SysInfo["HomeKeyWords"])
			.Replace("{WsDescription}", Config.SysInfo["HomeDescription"])
			.Replace("{WsBookTitle}", novelInfo_0.Name)
			.Replace("{WsBookUrl}", "/Book/" + novelInfo_0.PutID + "/Index." + Config.SysInfo["UrlReWriteExtName"])
			.Replace("{WsBookTclass}", novelInfo_0.LagerSort)
			.Replace("{WsBookNclass}", novelInfo_0.SmallSort)
			.Replace("{WsRootUrl}", Config.SysInfo["RootFolder"])
			.Replace("{WsAllUrl}", Config.BookInfo["BookHtmlIndexName"] + "." + Config.BookInfo["BookHtmlExtName"])
			.Replace("{WsListUrl}", Config.BookInfo["BookHtmlNclassName"] + "." + Config.BookInfo["BookHtmlExtName"])
			.Replace("{WsComUrl}", "/User/BR" + novelInfo_0.PutID + "." + Config.SysInfo["UrlReWriteExtName"])
			.Replace("{WsSaveUrl}", "/User/BS" + novelInfo_0.PutID + "." + Config.SysInfo["UrlReWriteExtName"])
			.Replace("{WsMarkUrl}", "/User/B" + novelInfo_0.PutID + "C" + chapterInfo_0.PutID + "." + Config.SysInfo["UrlReWriteExtName"])
			.Replace("{WsTranLoad}", "")
			.Replace("{WsUpUrl}", newValue4)
			.Replace("{WsDownUrl}", newValue2)
			.Replace("{WsUpUrl2}", newValue6)
			.Replace("{WsDownUrl2}", newValue5)
			.Replace("{WsChapterTitle}", chapterInfo_0.ChapterName)
			.Replace("{WsNclass}", chapterInfo_0.VolumeName);
		string text4 = chapterInfo_0.ChapterText;
		string[] array = Config.SysInfo["CheckContent"].ToString().Split(',');
		foreach (string text5 in array)
		{
			if (text5 != "" && text5.IndexOf("|", StringComparison.Ordinal) > 0)
			{
				string[] array2 = text5.Split('|');
				text4 = text4.Replace(array2[0], array2[1]);
			}
		}
		text = text.Replace("{WsTextType}", text4).Replace("{WsUpText}", newValue3).Replace("{WsListUrl}", Config.SysInfo["BookHtmlNclassName"] + "." + Config.BookInfo["BookHtmlExtName"])
			.Replace("{WsDownText}", newValue)
			.Replace("{WsTranStart}", "")
			.Replace("{WsBookID}", novelInfo_0.PutID.ToString())
			.Replace("{WsChapterID}", chapterInfo_0.PutID.ToString())
			.Replace("{WSExtName}", Config.BookInfo["BookHtmlExtName"]);
		SpiderException.Debug("CreateChapter 生成文件");
		string text6 = text2 + novelInfo_0.PutID + "/";
		text = null;
		if (bool_0)
		{
			SpiderException.Debug("CreateChapter 前后两页");
			if (num != 0)
			{
				ChapterInfo chapterInfo = new ChapterInfo
				{
					PutID = num
				};
				ChapterInfo chapterInfo_1 = chapterInfo;
				CreateChapter(novelInfo_0, chapterInfo_1, bool_0: false);
			}
			if (num2 != 0)
			{
				ChapterInfo chapterInfo2 = new ChapterInfo
				{
					PutID = num2
				};
				ChapterInfo chapterInfo_2 = chapterInfo2;
				CreateChapter(novelInfo_0, chapterInfo_2, bool_0: false);
			}
		}
	}

	public void CreateIndex(NovelInfo novelInfo, bool indexHtml, bool fullHtml, bool bool_2, bool bool_3, bool bool_4, bool bool_5, bool bool_6, bool bool_7, bool delTxt, bool delHtml, int delnum)
	{
		string text = "";
		if (delnum > 0)
		{
			SpiderException.Debug("DeleteChapter 删除多余章节");
			text = "DELETE  TOP " + delnum + " FROM Ws_BookVolume WHERE bookid =" + novelInfo.PutID.ToString() + " Order By id DESC";
			SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, text, (SqlParameter[])null);
		}
		SpiderException.Debug("UpdateChapterCount 更新书籍章节总数");
		text = "UPDATE Ws_BookList SET chaptercount = (SELECT COUNT(*) FROM Ws_BookChapter_" + novelInfo.PutID % 10 + " WHERE bookid=" + novelInfo.PutID + "), isweb=1, booktxt = '" + fullHtml + "' where id = " + novelInfo.PutID;
		SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, text, (SqlParameter[])null);
		text = "SELECT * FROM Ws_BookVolume WHERE bookid =" + novelInfo.PutID + " Order By id ASC";
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, text, (SqlParameter[])null);
		while (sqlDataReader.Read())
		{
			string text2 = sqlDataReader["id"].ToString();
			text = "UPDATE Ws_BookVolume SET booknclassnum = (SELECT COUNT(*) FROM Ws_BookChapter_" + novelInfo.PutID % 10 + " WHERE nclassid=" + text2 + ") where id = " + text2;
			SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, text, (SqlParameter[])null);
		}
		if (novelInfo.IsNew)
		{
			SpiderException.Debug("CreateSiteNewBook 奇文分站增加新书");
			text = "SELECT * FROM Ws_BookWeb WHERE isweb=1 Order By id ASC";
			sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, text, (SqlParameter[])null);
			while (sqlDataReader.Read())
			{
				int num = int.Parse(sqlDataReader["id"].ToString());
				string text3 = sqlDataReader["webaddtclass"].ToString();
				if (text3.IndexOf("." + novelInfo.LagerSortID + ".", StringComparison.Ordinal) >= 0 || text3.IndexOf(".0.", StringComparison.Ordinal) >= 0)
				{
					text = "Ws_BookWebRelevant_Insert";
					SqlParameter[] array = new SqlParameter[2]
					{
						new SqlParameter("@webid", SqlDbType.Int),
						null
					};
					array[0].SqlValue = num;
					array[1] = new SqlParameter("@bookid", novelInfo.PutID);
					SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.StoredProcedure, text, array);
				}
			}
		}
		string text4 = "";
		string text5 = "<spine>";
		SpiderException.Debug("CreateOPF 生成OPF索引文件");
		if (bool_2)
		{
			if (novelInfo.Keyword == null)
			{
				novelInfo.Keyword = novelInfo.Name;
			}
			object obj = text4 + "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>";
			object obj2 = string.Concat(obj, "<package unique-identifier=\"", novelInfo.PutID, "\">", "<metadata><dc-metadata><dc:Title>", novelInfo.Name.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</dc:Title><dc:Creator>", novelInfo.Author.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</dc:Creator><dc:Subject>", novelInfo.Keyword.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</dc:Subject><dc:Description>", novelInfo.Intro.Replace("&", "&amp;").Replace("<", "&gt;").Replace(">", "&lt;")
				.Replace("\n", "")
				.Replace("\r", ""), "</dc:Description>");
			object obj3 = string.Concat(obj2, "<dc:Publisher>", "小说阅读索引", "</dc:Publisher>", "<dc:Contributorid>1</dc:Contributorid><dc:Contributor>admin</dc:Contributor>");
			object obj4 = string.Concat(obj3, "<dc:Sortid>", novelInfo.LagerSortID, "</dc:Sortid>", "<dc:Typeid>0</dc:Typeid><dc:Articletype>0</dc:Articletype><dc:Permission>0</dc:Permission><dc:Firstflag>0</dc:Firstflag><dc:Fullflag>0</dc:Fullflag><dc:Imgflag>0</dc:Imgflag><dc:Power>0</dc:Power><dc:Display>0</dc:Display>");
			text4 = string.Concat(obj4, "<dc:Date>", DateTime.Today, "</dc:Date>", "<dc:Type>Text</dc:Type><dc:Format>text</dc:Format><dc:Language>ZH</dc:Language></dc-metadata></metadata><manifest>");
		}
		string empty = string.Empty;
		if (!string.IsNullOrEmpty(Configs.BaseConfig.EBookHead))
		{
			empty = empty + Configs.BaseConfig.EBookHead + "\n\n\n";
		}
		string text6 = string.Empty;
		if (!string.IsNullOrEmpty(Configs.BaseConfig.EBookHead))
		{
			text6 = text6 + Configs.BaseConfig.EBookHead + "\r\n\r\n\r\n";
		}
		text6 = text6 + "《" + novelInfo.Name + "》\r\n\r\n";
		string text7 = "0";
		string text8 = text6;
		string cmdText = "SELECT * FROM Ws_BookChapter_" + novelInfo.PutID % 10 + " WHERE bookid =" + novelInfo.PutID.ToString() + " Order By booktextorder ASC";
		sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		int num2 = 0;
		while (sqlDataReader.Read())
		{
			num2++;
			text5 = text5 + "<itemref idref=\"" + sqlDataReader["booktexttitle"].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
				.Replace(">", "&gt;") + "\" />";
			sqlDataReader["booktexttitle"].ToString();
			text7 = sqlDataReader["id"].ToString();
			if (fullHtml)
			{
				string path = string.Concat(Config.WaterSoftPath, "/files/article/txt/", novelInfo.PutID / 1000, "/", novelInfo.PutID.ToString(), "/" + text7 + ".txt");
				object obj5 = text8;
				text8 = string.Concat(obj5, "-----(", num2, ").", sqlDataReader["booktexttitle"].ToString(), "-----\r\n\r\n");
				if (File.Exists(path))
				{
					path = File.ReadAllText(path, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")).Replace("\n", "\r\n").Replace("<br/><br/>", "\r\n");
					text8 = text8 + path + "\r\n\r\n";
				}
				else
				{
					text8 += "该章节正在准备中，请稍候下载.....\r\n\r\n";
				}
			}
			object obj = text4;
			text4 = string.Concat(obj, "<item id=\"", sqlDataReader["booktexttitle"].ToString().Replace("&", "&amp;").Replace("<", "&lt;")
				.Replace(">", "&gt;"), "\" href=\"", sqlDataReader["id"], ".txt\" media-type=\"text/html\" content-type=\"chapter\" />");
		}
		sqlDataReader.Close();
		if (bool_2)
		{
			text4 = text4 + "</manifest>" + text5 + "</spine></package>";
			string path2 = Config.WaterSoftPath + "/files/article/txt/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID.ToString();
			if (!Directory.Exists(path2))
			{
				Directory.CreateDirectory(path2);
			}
			File.WriteAllText(Config.WaterSoftPath + "/files/article/txt/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID.ToString() + "/index.opf", text4, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		}
		if (fullHtml)
		{
			SpiderException.Debug("CreateTxt 生成全文阅读Txt");
			if (!string.IsNullOrEmpty(Configs.BaseConfig.EBookFoot))
			{
				text8 = text8 + "\r\n\r\n\r\n" + Configs.BaseConfig.EBookFoot;
			}
			text8 = NoHtml(text8);
			string text9 = Config.WaterSoftPath + "/files/article/txtfull/" + novelInfo.PutID / 1000;
			if (!Directory.Exists(text9))
			{
				Directory.CreateDirectory(text9);
			}
			string text10 = ".txt";
			string path3 = text9 + "/" + novelInfo.Name + text10;
			if (File.Exists(path3))
			{
				File.Delete(path3);
			}
			File.WriteAllText(path3, text8, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		}
	}

	public void CreateNoWapChapter(NovelInfo novelInfo, ChapterInfo chapterInfo, bool a, int b, int c, string d, string e, string f)
	{
	}

	public void CreateOPF(NovelInfo novelinfo)
	{
	}

	public void CreateSingleChapter(NovelInfo novelInfo)
	{
	}

	public void CreateSingleChapter(NovelInfo novelInfo, ChapterInfo chapterInfo, bool a, int b, int c, string d, string e, string f)
	{
	}

	public void CreateTagTable()
	{
		throw new NotImplementedException();
	}

	public void CreateWapChapter(NovelInfo novelInfo, ChapterInfo chapterInfo, bool a, int b, int c, string d, string e, string f)
	{
	}

	public void DeleteChapter(NovelInfo novelInfo, int bookid, int chapterid, bool ischapter, bool isnoio)
	{
		if (ischapter)
		{
			SpiderException.Debug("DeleteChapter 删除章节 CID=" + chapterid);
			string cmdText = "Delete From Ws_BookChapter_" + bookid % 10 + " where id=" + chapterid.ToString();
			SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
			if (!isnoio)
			{
				string path = Config.WaterSoftPath + "/files/article/txt/" + bookid / 1000 + "/" + bookid.ToString() + "/" + chapterid.ToString() + ".txt";
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
		}
		else
		{
			SpiderException.Debug("DeleteChapter 删除分卷 VID=" + chapterid);
			string cmdText = "Delete From Ws_BookVolume where id=" + chapterid;
			SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
			SpiderException.Debug("DeleteChapter 删除分卷下章节 VID=" + chapterid);
			cmdText = "Delete From Ws_BookChapter_" + bookid % 10 + " where nclassid=" + chapterid.ToString();
			SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		}
	}

	public void DeleteVolume(NovelInfo novelInfo, int bookid)
	{
	}

	public void DeteleNovel(int bookid)
	{
		string cmdText = "Delete Ws_BookList where id=" + bookid;
		SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
	}

	public NovelInfo GetChapterInfo(NovelInfo novelInfo)
	{
		ChapterInfo chapterInfo = GetChapterInfo(novelInfo.PutID, novelInfo.LastChapter.PutID);
		novelInfo.LastChapter.ChapterName = chapterInfo.ChapterName;
		novelInfo.LastChapter.ChapterText = chapterInfo.ChapterText;
		novelInfo.LastChapter.VolumeName = chapterInfo.VolumeName;
		return novelInfo;
	}

	public ChapterInfo GetChapterInfo(int putid, int chapterid)
	{
		NovelInfo novelInfo = new NovelInfo
		{
			PutID = putid
		};
		NovelInfo novelInfo2 = novelInfo;
		ChapterInfo chapterInfo = new ChapterInfo
		{
			PutID = chapterid
		};
		ChapterInfo chapterInfo2 = chapterInfo;
		string cmdText = "select * from Ws_BookChapter_" + novelInfo2.PutID % 10 + " where id=@id";
		SqlParameter[] commandParameters = new SqlParameter[1]
		{
			new SqlParameter("@id ", chapterInfo2.PutID)
		};
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, commandParameters);
		if (sqlDataReader.Read())
		{
			chapterInfo2.ChapterName = sqlDataReader["booktexttitle"].ToString().Trim();
			chapterInfo2.VolumeName = sqlDataReader["nclass"].ToString();
			int.Parse(sqlDataReader["booktextnum"].ToString());
			string text = "/files/article/txt/" + novelInfo2.PutID / 1000 + "/" + novelInfo2.PutID + "/" + chapterInfo2.PutID + ".txt";
			if (File.Exists(Config.WaterSoftPath + text))
			{
				chapterInfo2.ChapterText = File.ReadAllText(Config.WaterSoftPath + text, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
			}
			else
			{
				SpiderException.Show(chapterInfo2.ChapterName + " 缺少txt文件 " + text, bool_0: true);
				chapterInfo2.ChapterText = "";
			}
		}
		sqlDataReader.Close();
		return chapterInfo2;
	}

	public ChapterInfo[] GetChapterList(int bookid)
	{
		ArrayList arrayList = new ArrayList();
		int num = bookid % 10;
		string cmdText = "SELECT a.id, a.nclassid, a.nclass, a.booktexttitle, a.booktextorder,a.booktexttime,a.booktextnum FROM Ws_BookChapter_" + num + " a INNER JOIN Ws_BookVolume b ON b.id = a.nclassid WHERE (a.bookid = " + bookid.ToString() + ") ORDER BY b.booknclassnum ASC, b.id ASC, a.booktextorder ASC, a.id ASC";
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		while (sqlDataReader.Read())
		{
			ChapterInfo value = new ChapterInfo
			{
				PutID = Convert.ToInt32(sqlDataReader["id"]),
				VolumeName = sqlDataReader["nclass"].ToString(),
				ChapterName = sqlDataReader["booktexttitle"].ToString().Trim(),
				LastTime = Convert.ToDateTime(sqlDataReader["booktexttime"]),
				Size = Convert.ToInt32(sqlDataReader["booktextnum"]),
				ItemIndex = Convert.ToInt32(sqlDataReader["booktextorder"])
			};
			arrayList.Add(value);
		}
		sqlDataReader.Close();
		return (ChapterInfo[])arrayList.ToArray(typeof(ChapterInfo));
	}

	public string GetChapterText(NovelInfo novelInfo_0, bool on)
	{
		string result = string.Empty;
		if (on)
		{
			result = FormatText.Typesetting(Regex.Replace(novelInfo_0.LastChapter.ChapterText.Replace("[img]", "").Replace("[/img]", ""), "【图片下载标记\\d*】", "")).Replace("\n", "\r\n\r\n");
			if (result.Length > 300 && !result.StartsWith("    "))
			{
				result = "    " + result;
			}
			return result;
		}
		string path = Config.WaterSoftPath + "/files/article/txt/" + novelInfo_0.PutID / 1000 + "/" + novelInfo_0.PutID.ToString() + "/" + novelInfo_0.LastChapter.PutID.ToString() + ".txt";
		if (File.Exists(path))
		{
			result = File.ReadAllText(path, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"));
		}
		return result;
	}

	public NovelInfo GetNovelInfo(NovelInfo novelInfo)
	{
		string text = "";
		if (novelInfo.PutID != 0)
		{
			text = "SELECT TOP 1 * FROM [Ws_BookList] WHERE ID=" + novelInfo.PutID;
		}
		else
		{
			if (novelInfo.Name == "" || novelInfo.Author == "")
			{
				throw new ApplicationException("无法从数据库获得小说信息");
			}
			text = "SELECT TOP 1 * FROM [Ws_BookList] WHERE BOOKTITLE='" + novelInfo.Name.Replace("'", "") + "'";
		}
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, text, (SqlParameter[])null);
		if (sqlDataReader.Read())
		{
			novelInfo.PutID = Convert.ToInt32(sqlDataReader["id"]);
			novelInfo.Name = sqlDataReader["booktitle"].ToString();
			novelInfo.Author = sqlDataReader["bookwriter"].ToString();
			novelInfo.LagerSortID = Convert.ToInt32(sqlDataReader["tclassid"]);
			novelInfo.LagerSort = sqlDataReader["tclass"].ToString();
			novelInfo.SmallSortID = Convert.ToInt32(sqlDataReader["nclassid"]);
			novelInfo.SmallSort = sqlDataReader["nclass"].ToString();
			novelInfo.Intro = sqlDataReader["booksum"].ToString();
			novelInfo.Degree = Convert.ToInt32(sqlDataReader["bookstate"]);
			novelInfo.Keyword = sqlDataReader["bookrole"].ToString();
			novelInfo.LastupDate = method_0(DateTime.Parse(sqlDataReader["updatetime"].ToString()));
			novelInfo.IsNew = false;
		}
		else
		{
			novelInfo.IsNew = true;
		}
		sqlDataReader.Close();
		if (!novelInfo.IsNew)
		{
			text = "SELECT TOP 1 a.id, a.nclassid, a.nclass, a.booktexttitle, a.booktextorder FROM Ws_BookChapter_" + novelInfo.PutID % 10 + " a INNER JOIN Ws_BookVolume b ON b.id = a.nclassid WHERE (a.bookid = " + novelInfo.PutID.ToString() + ") ORDER BY b.booknclassnum DESC, b.id DESC, a.booktextorder DESC, a.id DESC";
			sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, text, (SqlParameter[])null);
			if (sqlDataReader.Read())
			{
				string chapterName = Regex.Replace(sqlDataReader["booktexttitle"].ToString(), "\\s+", " ").Trim();
				novelInfo.LastChapter.PutID = Convert.ToInt32(sqlDataReader["id"]);
				novelInfo.LastChapter.ChapterName = chapterName;
				novelInfo.LastChapter.VolumeName = sqlDataReader["nclass"].ToString();
			}
			else
			{
				novelInfo.LastChapter.PutID = 0;
				novelInfo.LastChapter.ChapterName = "";
				novelInfo.LastChapter.VolumeName = "";
			}
			sqlDataReader.Close();
		}
		return novelInfo;
	}

	public NovelInfo GetNovelInfo(NovelInfo novelInfo, bool a)
	{
		return GetNovelInfo(novelInfo);
	}

	public NovelInfo[] GetNovelList(string sqlString)
	{
		ArrayList arrayList = new ArrayList();
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, sqlString, (SqlParameter[])null);
		while (sqlDataReader.Read())
		{
			NovelInfo value = new NovelInfo
			{
				PutID = Convert.ToInt32(sqlDataReader["id"]),
				Name = sqlDataReader["booktitle"].ToString(),
				Author = sqlDataReader["bookwriter"].ToString(),
				LagerSortID = Convert.ToInt32(sqlDataReader["tclassid"]),
				LagerSort = sqlDataReader["tclass"].ToString(),
				SmallSortID = Convert.ToInt32(sqlDataReader["nclassid"]),
				SmallSort = sqlDataReader["nclass"].ToString(),
				Intro = sqlDataReader["booksum"].ToString(),
				Degree = Convert.ToInt32(sqlDataReader["bookstate"]),
				Keyword = sqlDataReader["bookrole"].ToString(),
				LastupDate = FormatText.GetTime(DateTime.Parse(sqlDataReader["updatetime"].ToString())),
				PostDate = FormatText.GetTime(DateTime.Parse(sqlDataReader["updatetime"].ToString()))
			};
			arrayList.Add(value);
		}
		sqlDataReader.Close();
		NovelInfo[] array = (NovelInfo[])arrayList.ToArray(typeof(NovelInfo));
		for (int i = 0; i < array.Length; i++)
		{
			int num = array[i].PutID % 10;
			sqlString = "SELECT TOP 1 a.id, a.nclassid, a.nclass, a.booktexttitle, a.booktextorder FROM Ws_BookChapter_" + num + " a INNER JOIN Ws_BookVolume b ON b.id = a.nclassid WHERE (a.bookid = " + array[i].PutID.ToString() + ") ORDER BY b.booknclassnum DESC, b.id DESC, a.booktextorder DESC, a.id DESC";
			sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, sqlString, (SqlParameter[])null);
			if (sqlDataReader.Read())
			{
				string chapterName = Regex.Replace(sqlDataReader["booktexttitle"].ToString(), "\\s+", " ").Trim();
				array[i].LastChapter.PutID = Convert.ToInt32(sqlDataReader["id"]);
				array[i].LastChapter.ChapterName = chapterName;
				array[i].LastChapter.VolumeName = sqlDataReader["nclass"].ToString();
			}
			else
			{
				array[i].LastChapter.PutID = 0;
				array[i].LastChapter.ChapterName = "";
				array[i].LastChapter.VolumeName = "";
			}
			sqlDataReader.Close();
		}
		return array;
	}

	public ChapterInfo[] GetVolumeNameList(int bookid)
	{
		ArrayList arrayList = new ArrayList();
		string cmdText = "SELECT * FROM  Ws_BookVolume  WHERE (bookid = " + bookid + ") ORDER BY id ASC";
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		int num = 0;
		while (sqlDataReader.Read())
		{
			num++;
			ChapterInfo value = new ChapterInfo
			{
				ItemIndex = num,
				PutID = Convert.ToInt32(sqlDataReader["id"]),
				VolumeName = sqlDataReader["booknclass"].ToString(),
				ChapterName = sqlDataReader["booktitle"].ToString().Trim()
			};
			arrayList.Add(value);
		}
		sqlDataReader.Close();
		return (ChapterInfo[])arrayList.ToArray(typeof(ChapterInfo));
	}

	public ChapterInfo InsertChapter(NovelInfo novelInfo, TaskConfigInfo taskConfigInfo)
	{
		if (novelInfo.PutID == 0)
		{
			novelInfo = InsertNovel(novelInfo);
		}
		SpiderException.Debug("InsertChapter 获得本书最新分卷");
		int num = 0;
		string text = "";
		int num2 = 0;
		string cmdText = "select top 1 id,booknclass,booknclassnum from Ws_BookVolume where bookid = " + novelInfo.PutID + " order by booknclassnum desc,id desc";
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		if (sqlDataReader.Read())
		{
			num = Convert.ToInt32(sqlDataReader["id"]);
			text = sqlDataReader["booknclass"].ToString();
			num2 = Convert.ToInt32(sqlDataReader["booknclassnum"]);
			if (novelInfo.LastChapter.VolumeName == string.Empty)
			{
				novelInfo.LastChapter.VolumeName = text;
			}
		}
		else
		{
			text = novelInfo.LastChapter.VolumeName;
			if (text.Trim() == string.Empty)
			{
				text = Config.TempletsVolume;
				novelInfo.LastChapter.VolumeName = text;
			}
		}
		sqlDataReader.Close();
		sqlDataReader.Dispose();
		SpiderException.Debug("InsertChapter 比较分卷");
		if (!taskConfigInfo.ProhibitionVolume)
		{
			bool flag = false;
			flag = !taskConfigInfo.CheckVolume || !Regex.IsMatch(novelInfo.LastChapter.ChapterName, "([第]*[两|一|二|三|四|五|六|七|八|九|〇|壹|贰|叁|肆|伍|陆|柒|捌|玖|零|十|拾|百|佰|千|万|1|2|3|4|5|6|7|8|9|0]*[章|节|回])") || ((novelInfo.LastChapter.VolumeName.IndexOf("第") >= 0) ? (Regex.IsMatch(novelInfo.LastChapter.ChapterName, "([第][零|一|〇|壹|1|0]?[章|节|回])") ? true : false) : (Regex.IsMatch(novelInfo.LastChapter.ChapterName, "([第]*[零|一|〇|壹|1|0]?[章|节|回])") ? true : false));
			if (flag && (text.Trim() != novelInfo.LastChapter.VolumeName.Trim() || num == 0))
			{
				cmdText = "INSERT INTO Ws_BookVolume (bookid,booktitle,booknclass,booknclassnum) Values (" + novelInfo.PutID.ToString() + ",'" + novelInfo.Name + "','" + novelInfo.LastChapter.VolumeName + "'," + num2 + ")";
				SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				cmdText = "Select Max(id) From Ws_BookVolume Where bookid = " + novelInfo.PutID;
				object obj = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
				if (obj != DBNull.Value)
				{
					num = Convert.ToInt32(obj);
				}
			}
		}
		SpiderException.Debug("InsertChapter 添加章节数据");
		cmdText = "Ws_BookChapter_" + novelInfo.PutID % 10 + "_Insert";
		SqlParameter[] commandParameters = new SqlParameter[7]
		{
			new SqlParameter("@bookid", novelInfo.PutID),
			new SqlParameter("@booktitle", novelInfo.Name),
			new SqlParameter("@nclassid", num),
			new SqlParameter("@nclass", text),
			new SqlParameter("@booktexttitle", novelInfo.LastChapter.ChapterName),
			new SqlParameter("@booktextnum", novelInfo.LastChapter.ChapterText.Length),
			new SqlParameter("@booktextorder", novelInfo.LastChapter.ItemIndex)
		};
		object value = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.StoredProcedure, cmdText, commandParameters);
		novelInfo.LastChapter.PutID = Convert.ToInt32(value);
		if (Configs.BaseConfig.TextMarkOfData)
		{
			novelInfo.LastChapter = FormatText.TextMark(novelInfo.LastChapter);
		}
		novelInfo.LastChapter.ChapterText = novelInfo.LastChapter.ChapterText.Replace("<br>", "\r\n").Replace("<br />", "\r\n").Replace("<BR />", "\r\n")
			.Replace("<BR>", "\r\n");
		novelInfo.LastChapter.ChapterText = novelInfo.LastChapter.ChapterText.Replace("&nbsp;", " ");
		novelInfo.LastChapter.ChapterText = FormatText.Typesetting(novelInfo.LastChapter.ChapterText);
		if (novelInfo.LastChapter.ChapterText.Length > 300 && !novelInfo.LastChapter.ChapterText.StartsWith("    "))
		{
			novelInfo.LastChapter.ChapterText = "    " + novelInfo.LastChapter.ChapterText;
		}
		novelInfo.LastChapter.ChapterText = novelInfo.LastChapter.ChapterText.Replace("\\n", "\n");
		novelInfo.LastChapter.ChapterText = novelInfo.LastChapter.ChapterText.Replace("\n", "\r\n");
		string text2 = Config.WaterSoftPath + "/files/article/txt/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID;
		bool flag2 = true;
		try
		{
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			File.WriteAllText(text2 + "/" + novelInfo.LastChapter.PutID + ".txt", novelInfo.LastChapter.ChapterText, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		}
		catch
		{
			flag2 = false;
		}
		if (flag2)
		{
			cmdText = string.Concat(new object[2]
			{
				"Update Ws_BookChapter_" + novelInfo.PutID % 10 + " Set isweb=1 Where id = ",
				novelInfo.LastChapter.PutID.ToString()
			});
			SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		}
		SpiderException.Debug("InsertChapter 更新小说最新章节 Ws_BookList_Update_UpdateInfo");
		cmdText = "Ws_BookList_Update_UpdateInfo";
		commandParameters = new SqlParameter[4]
		{
			new SqlParameter("@id", novelInfo.PutID),
			new SqlParameter("@lastchapterid", novelInfo.LastChapter.PutID),
			null,
			null
		};
		string text3 = novelInfo.LastChapter.ChapterName;
		if (string.IsNullOrEmpty(novelInfo.LastChapter.VolumeName))
		{
			text3 = novelInfo.LastChapter.VolumeName + " " + text3;
		}
		commandParameters[2] = new SqlParameter("@lastchaptertitle", text3);
		commandParameters[3] = new SqlParameter("@updatetime", DateTime.Now);
		SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.StoredProcedure, cmdText, commandParameters);
		return novelInfo.LastChapter;
	}

	public ChapterInfo InsertChapterByOrder(NovelInfo novelInfo_0, TaskConfigInfo Ol1O11l10lO10lOO, int OllO000)
	{
		return new ChapterInfo();
	}

	public NovelInfo InsertNovel(NovelInfo novelInfo)
	{
		if (novelInfo.LagerSort.Trim() == string.Empty)
		{
			novelInfo.LagerSort = "其他类型";
		}
		if (novelInfo.SmallSort.Trim() == string.Empty)
		{
			novelInfo.SmallSort = "其他类型";
		}
		SqlParameter[] array = new SqlParameter[15];
		SpiderException.Debug("Local.Novel.Sort 入库 Ws_BookList_Insert");
		novelInfo = Sort(novelInfo);
		array[0] = new SqlParameter("@tclassid", novelInfo.LagerSortID);
		array[1] = new SqlParameter("@nclassid", novelInfo.SmallSortID);
		array[2] = new SqlParameter("@booktitle", novelInfo.Name);
		string text = FormatText.ToSpell(novelInfo.Name);
		array[3] = new SqlParameter("@bookentitle", text);
		string value = "*";
		if (text.Length > 0)
		{
			value = text.Substring(0, 1);
		}
		array[4] = new SqlParameter("@bookabc", value);
		array[5] = new SqlParameter("@bookwriter", novelInfo.Author);
		array[6] = new SqlParameter("@bookrole", novelInfo.Keyword);
		array[7] = new SqlParameter("@bookimg", novelInfo.Cover != null);
		array[8] = new SqlParameter("@booksum", novelInfo.Intro);
		array[9] = new SqlParameter("@bookstate", novelInfo.Degree)
		{
			SqlValue = novelInfo.Degree
		};
		array[10] = new SqlParameter("@tclass", novelInfo.LagerSort);
		array[11] = new SqlParameter("@nclass", novelInfo.SmallSort);
		array[12] = new SqlParameter("@getid", Convert.ToInt32(novelInfo.GetID));
		array[13] = new SqlParameter("@ruleid", novelInfo.RuleID);
		array[14] = new SqlParameter("@rulename", novelInfo.RuleName);
		string cmdText = "Ws_BookList_Insert";
		object value2 = SqlHelper.ExecuteScalar(SqlHelper.ConnectionString, CommandType.StoredProcedure, cmdText, array);
		novelInfo.PutID = Convert.ToInt32(value2);
		novelInfo.IsNew = true;
		if (novelInfo.Cover != null)
		{
			string text2 = Config.WaterSoftPath + "/files/article/image/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID;
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
			}
			novelInfo.Cover.Save(text2 + "/" + novelInfo.PutID + "s.jpg", ImageFormat.Jpeg);
		}
		return novelInfo;
	}

	public int InsertVolume(NovelInfo novelInfo, string str)
	{
		return 0;
	}

	private int method_0(DateTime dateTime_0)
	{
		return (int)new DateTimeOffset(dateTime_0).ToUnixTimeSeconds();
	}

	public string NoHtml(string html)
	{
		html = Regex.Replace(html, "<.+?>", "").Trim();
		return html;
	}

	public void PinyinHua(string string_0)
	{
	}

	private static void smethod_0()
	{
		object_0 = new object();
	}

	public NovelInfo Sort(NovelInfo novelInfo)
	{
		bool flag = false;
		bool flag2 = false;
		SpiderException.Debug("Local.Novel.Sort 处理分类");
		string cmdText = "Select id,tid From Ws_BookClass Where tid > 0 And title = '" + novelInfo.SmallSort + "'";
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		if (sqlDataReader.Read())
		{
			novelInfo.SmallSortID = Convert.ToInt32(sqlDataReader["id"]);
			novelInfo.LagerSortID = Convert.ToInt32(sqlDataReader["tid"]);
			flag2 = true;
		}
		else
		{
			flag2 = false;
		}
		sqlDataReader.Close();
		if (!flag2)
		{
			cmdText = "Select id,tid From Ws_BookClass Where tid=0 And title = '" + novelInfo.LagerSort + "'";
			sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
			if (sqlDataReader.Read())
			{
				novelInfo.LagerSortID = Convert.ToInt32(sqlDataReader["id"]);
				flag = true;
			}
			else
			{
				flag = false;
			}
			sqlDataReader.Close();
			object value;
			if (!flag)
			{
				cmdText = "Ws_BookClass_Insert";
				value = SqlHelper.ExecuteScalar(commandParameters: new SqlParameter[4]
				{
					new SqlParameter("@tid", SqlDbType.BigInt)
					{
						SqlValue = 0
					},
					new SqlParameter("@title", novelInfo.LagerSort),
					new SqlParameter("@onoff", false),
					new SqlParameter("@collecttext", novelInfo.LagerSort)
				}, connectionString: SqlHelper.ConnectionString, cmdType: CommandType.StoredProcedure, cmdText: cmdText);
				novelInfo.LagerSortID = Convert.ToInt32(value);
			}
			cmdText = "Ws_BookClass_Insert";
			value = SqlHelper.ExecuteScalar(commandParameters: new SqlParameter[4]
			{
				new SqlParameter("@tid", novelInfo.LagerSortID)
				{
					SqlValue = novelInfo.LagerSortID
				},
				new SqlParameter("@title", novelInfo.SmallSort),
				new SqlParameter("@onoff", true),
				new SqlParameter("@collecttext", novelInfo.SmallSort)
			}, connectionString: SqlHelper.ConnectionString, cmdType: CommandType.StoredProcedure, cmdText: cmdText);
			novelInfo.SmallSortID = Convert.ToInt32(value);
			return novelInfo;
		}
		cmdText = "Select id,title From Ws_BookClass Where id = " + novelInfo.LagerSortID + "";
		sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		if (sqlDataReader.Read())
		{
			novelInfo.LagerSort = Convert.ToString(sqlDataReader["title"]);
		}
		sqlDataReader.Close();
		return novelInfo;
	}

	public void UpdateChapter(NovelInfo novelInfo, ReplaceConfigInfo replaceConfigInfo)
	{
		SpiderException.Debug("UpdateChapter 文字广告");
		if (Configs.BaseConfig.TextMarkOfData)
		{
			novelInfo.LastChapter = FormatText.TextMark(novelInfo.LastChapter);
		}
		SpiderException.Debug("UpdateChapter 整理小说");
		novelInfo.LastChapter.ChapterText = FormatText.Typesetting(novelInfo.LastChapter.ChapterText);
		novelInfo.LastChapter.ChapterText = novelInfo.LastChapter.ChapterText.Replace("<br>", "\r\n").Replace("<br />", "\r\n").Replace("<BR />", "\r\n")
			.Replace("<BR>", "\r\n");
		novelInfo.LastChapter.ChapterText = novelInfo.LastChapter.ChapterText.Replace("&nbsp;", " ");
		novelInfo.LastChapter.ChapterText = FormatText.Typesetting(novelInfo.LastChapter.ChapterText);
		if (novelInfo.LastChapter.ChapterText.Length > 300 && !novelInfo.LastChapter.ChapterText.StartsWith("    "))
		{
			novelInfo.LastChapter.ChapterText = "    " + novelInfo.LastChapter.ChapterText;
		}
		novelInfo.LastChapter.ChapterText = novelInfo.LastChapter.ChapterText.Replace("\\n", "\n");
		novelInfo.LastChapter.ChapterText = novelInfo.LastChapter.ChapterText.Replace("\n", "\r\n");
		SpiderException.Debug("UpdateChapter 更新章节数据1");
		SqlHelper.ExecuteNonQuery(cmdText: (!replaceConfigInfo.UpdateChapterName) ? string.Concat("Update Ws_BookChapter_" + novelInfo.PutID % 10 + " Set isweb=0,booktextnum=", novelInfo.LastChapter.ChapterText.Length, " Where id = ", novelInfo.LastChapter.PutID.ToString()) : string.Concat("Update Ws_BookChapter_" + novelInfo.PutID % 10 + " Set isweb=0,booktexttitle='", novelInfo.LastChapter.ChapterName, "',booktextnum=", novelInfo.LastChapter.ChapterText.Length, " Where id = ", novelInfo.LastChapter.PutID.ToString()), connectionString: SqlHelper.ConnectionString, cmdType: CommandType.Text, commandParameters: (SqlParameter[])null);
		SpiderException.Debug("UpdateChapter 更新txt");
		string text = "/files/article/txt/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID.ToString() + "/" + novelInfo.LastChapter.PutID.ToString() + ".txt";
		FileInfo fileInfo = new FileInfo(Config.WaterSoftPath + text);
		bool flag = true;
		try
		{
			if (!fileInfo.Exists && !fileInfo.Directory.Exists)
			{
				Directory.CreateDirectory(fileInfo.Directory.FullName);
			}
			StreamWriter streamWriter = new StreamWriter(Config.WaterSoftPath + text, append: false, Encoding.GetEncoding("utf-8"));
			streamWriter.Write(novelInfo.LastChapter.ChapterText);
			streamWriter.Flush();
			streamWriter.Close();
		}
		catch
		{
			flag = false;
		}
		if (flag)
		{
			string cmdText = string.Concat(new object[2]
			{
				"Update Ws_BookChapter_" + novelInfo.PutID % 10 + " Set isweb=1 Where id = ",
				novelInfo.LastChapter.PutID.ToString()
			});
			SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		}
		SpiderException.Debug("UpdateChapter 结束");
	}

	public int[] UpdateChapterOrder(NovelInfo novelInfo_0, int Ol00llOO01O001O0, int OO1lOOO0OOO0)
	{
		return new int[5];
	}

	public void UpdateLastChapter(NovelInfo novelInfo)
	{
		ChapterInfo chapterInfo = new ChapterInfo();
		string cmdText = "SELECT top 1 * FROM Ws_BookChapter_" + novelInfo.PutID % 10 + " WHERE (bookid = " + novelInfo.PutID.ToString() + ") ORDER BY id Desc, booktextorder Desc";
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		if (sqlDataReader.Read())
		{
			chapterInfo.PutID = Convert.ToInt32(sqlDataReader["id"]);
			chapterInfo.VolumeName = sqlDataReader["nclass"].ToString();
			chapterInfo.ChapterName = sqlDataReader["booktexttitle"].ToString().Trim();
			chapterInfo.LastTime = Convert.ToDateTime(sqlDataReader["booktexttime"]);
		}
		sqlDataReader.Close();
		SpiderException.Debug("InsertChapter 更新小说最新章节 Ws_BookList_Update_UpdateInfo");
		cmdText = "Ws_BookList_Update_UpdateInfo";
		SqlParameter[] commandParameters = new SqlParameter[4]
		{
			new SqlParameter("@id", novelInfo.PutID),
			new SqlParameter("@lastchapterid", chapterInfo.PutID),
			new SqlParameter("@lastchaptertitle", chapterInfo.ChapterName),
			new SqlParameter("@updatetime", DateTime.Now)
		};
		SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.StoredProcedure, cmdText, commandParameters);
	}

	public void UpdateLastChapter(NovelInfo novelInfo, ChapterInfo chapterInfo)
	{
	}

	public void UpdateNovel(NovelInfo novelInfo, bool a1, bool a2, bool a3, bool a4, bool a5, bool a6, bool a7)
	{
		if (novelInfo.IsNew)
		{
			return;
		}
		string cmdText = "SELECT TOP 1 id FROM [Ws_BookList] WHERE BOOKTITLE='" + novelInfo.Name.Replace("'", "") + "'";
		SqlDataReader sqlDataReader = SqlHelper.ExecuteReader(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
		if (sqlDataReader.Read())
		{
			novelInfo.PutID = Convert.ToInt32(sqlDataReader["id"]);
		}
		sqlDataReader.Close();
		if (novelInfo.PutID == 0)
		{
			return;
		}
		string empty = string.Empty;
		bool flag = true;
		if (novelInfo.Cover != null)
		{
			try
			{
				SpiderException.Debug("UpdateNovel 下载封面");
				new Random();
				empty = Config.WaterSoftPath + "/files/article/image/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID;
				string text = empty + "/" + novelInfo.PutID + "s.jpg";
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				if (!Directory.Exists(empty))
				{
					Directory.CreateDirectory(empty);
				}
				novelInfo.Cover.Save(text);
				novelInfo.Cover.Dispose();
			}
			catch
			{
				SpiderException.Debug("UpdateNovel 下载封面失败");
				flag = false;
			}
		}
		cmdText = "UPDATE [Ws_BookList] SET BOOKTITLE='" + novelInfo.Name.Replace("'", "") + "'";
		if (novelInfo.Author != null)
		{
			cmdText = cmdText + ",bookwriter='" + novelInfo.Author.ToString() + "'";
		}
		if (novelInfo.Intro != null)
		{
			cmdText = cmdText + ",booksum='" + novelInfo.Intro.Replace("'", "") + "'";
		}
		if (novelInfo.LagerSort != null)
		{
			string text2 = cmdText;
			cmdText = text2 + ",tclassid=" + novelInfo.LagerSortID + ",tclass='" + novelInfo.LagerSort + "'";
		}
		if (novelInfo.SmallSort != null)
		{
			string text3 = cmdText;
			cmdText = text3 + ",nclassid=" + novelInfo.SmallSortID + ",nclass='" + novelInfo.SmallSort + "'";
		}
		if (novelInfo.Degree != -1)
		{
			cmdText = cmdText + ",bookstate=" + novelInfo.Degree;
		}
		if (novelInfo.Keyword != null)
		{
			cmdText = cmdText + ",bookrole='" + novelInfo.Keyword.ToString() + "'";
		}
		if (novelInfo.Cover != null)
		{
			cmdText = cmdText + ",bookimg='" + flag + "'";
		}
		cmdText = cmdText + " WHERE id = " + novelInfo.PutID;
		SpiderException.Debug(cmdText);
		SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString, CommandType.Text, cmdText, (SqlParameter[])null);
	}

	public void UpdateVolume(NovelInfo novelInfo, int O0O, string Ol0Oll)
	{
	}
}
