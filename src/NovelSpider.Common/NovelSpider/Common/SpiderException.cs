using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Text;
using System.Windows.Forms;
using NovelSpider.Config;
using NovelSpider.Entity;

namespace NovelSpider.Common;

public class SpiderException
{
	private static object object_0;

	static SpiderException()
	{
		object_0 = new object();
	}

	public static void Debug(string string_0)
	{
		if (!Configs.BaseConfig.Debug)
		{
			return;
		}
		lock (object_0)
		{
			if (Configs.CmdModel)
			{
				Console.WriteLine("Debug：" + string_0);
			}
			if (Configs.BaseConfig.LogType == 0)
				{
					File.AppendAllText("Debug.Log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + string_0 + "\r\n", Encoding.GetEncoding("utf-8"));
				}
		}
	}

	public static void Debug(string string_0, string string_1)
	{
		if (!Configs.BaseConfig.Debug)
		{
			return;
		}
		lock (object_0)
		{
			if (Configs.CmdModel)
			{
				Console.WriteLine("Debug：" + string_1);
			}
			if (Configs.BaseConfig.LogType == 0)
				{
					File.AppendAllText("Debug.Log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + string_1 + "\r\n", Encoding.GetEncoding("utf-8"));
				}
		}
	}

	public static void Delhtml(string string_0, int int_0, int int_1, string string_1)
	{
		if (!Configs.BaseConfig.Debug)
		{
			return;
		}
		lock (object_0)
		{
			if (Configs.CmdModel)
			{
				Console.WriteLine("Debug：" + string_0);
			}
			if (Configs.BaseConfig.LogType == 0)
			{
				File.AppendAllText(Application.StartupPath + "/Log/DelHtml" + DateTime.Now.ToString("yyMMdd") + ".Log", ("------------------------------------------------------------\n时间：" + DateTime.Now.ToString("yyyy-MM-dd") + "|小说：" + string_0 + "|章节：" + string_1 + "\n").Replace("\n", "\r\n"), Encoding.GetEncoding("utf-8"));
			}
		}
	}

	public static void EmptyTXT(string string_0, int int_0, string string_1, int int_1)
	{
		if (!Configs.BaseConfig.Debug)
		{
			return;
		}
		lock (object_0)
		{
			if (Configs.CmdModel)
			{
				Console.WriteLine("Debug：" + string_0);
			}
			if (Configs.BaseConfig.LogType == 0)
			{
				File.AppendAllText(Application.StartupPath + "/Log/" + DateTime.Now.ToString("yyMMdd") + "EmptyTXT.Log", ("------------------------------------------------------------\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n小说：" + string_0 + "|" + int_0 + "\n章节：" + string_1 + "|" + int_1 + "\n").Replace("\n", "\r\n"), Encoding.GetEncoding("utf-8"));
			}
		}
	}

	public static string[] recursed(string string_0, string[] string_1)
	{
		string[] array = new string[0];
		if (Directory.Exists(Application.StartupPath + "/Log/"))
		{
			foreach (string searchPattern in string_1)
			{
				string[] files = Directory.GetFiles(Application.StartupPath + "/" + string_0, searchPattern, SearchOption.AllDirectories);
				if (files != null)
				{
					string[] array2 = array;
					array = new string[array.Length + files.Length];
					Array.Copy(array2, 0, array, 0, array2.Length);
					Array.Copy(files, 0, array, array2.Length, files.Length);
				}
			}
		}
		return array;
	}

	public static bool removeSqlite(int int_0, int int_1, string string_0, string string_1)
	{
		FileInfo fileInfo = new FileInfo(Application.StartupPath + "/Log/" + string_1);
		if (!fileInfo.Exists)
		{
			return false;
		}
		string string_2 = "Data Source=" + fileInfo.FullName;
		string string_3 = "delete FROM [TaskLog] WHERE NID=@nid and NOVELNAME=@novelname";
		return Convert.ToInt32(SQLiteHelper.ExecuteNonQuery(string_2, string_3, new SqliteParameter("@nid", int_0), new SqliteParameter("@novelname", string_0))) != 0;
	}

	public static void Show(string string_0, bool bool_0)
	{
		if (bool_0)
		{
			lock (object_0)
			{
				if (Configs.CmdModel)
				{
					Console.WriteLine("日志记录：" + string_0);
				}
				if (Configs.BaseConfig.LogType == 0)
				{
					File.AppendAllText(Application.StartupPath + "/Log/" + DateTime.Today.ToString("yyyyMMdd") + ".Log", ("------------------------------------------------------------\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n提示：" + string_0 + "\n").Replace("\n", "\r\n"), Encoding.GetEncoding("utf-8"));
				}
				else
				{
					SQLite(0, string_0, new NovelInfo { GetID = "0", Name = "未知", PutID = 0 }, string.Empty, string.Empty);
				}
				return;
			}
		}
		if (!Configs.CmdModel)
		{
			throw new ApplicationException(string_0);
		}
		throw new Exception(string_0);
	}

	public static void Show(string string_0, NovelInfo novelInfo_0, bool bool_0, string strTask)
	{
		if (!bool_0)
		{
			return;
		}
		lock (object_0)
		{
			if (novelInfo_0 == null)
			{
				novelInfo_0 = new NovelInfo { GetID = "0", Name = "未知", PutID = 0 };
			}
			if (Configs.CmdModel)
			{
				Console.WriteLine("日志记录：" + string_0 + "\t" + novelInfo_0.GetID + "|" + novelInfo_0.Name + "|" + novelInfo_0.PutID);
			}
			if (Configs.BaseConfig.LogType == 0)
			{
				File.AppendAllText(Application.StartupPath + "/Log/" + DateTime.Today.ToString("yyyyMMdd") + ".Log", ("------------------------------------------------------------\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n提示：" + string_0 + "\n小说：" + novelInfo_0.GetID + "|" + novelInfo_0.Name + "|" + novelInfo_0.PutID + "\n方案：" + strTask + "\n").Replace("\n", "\r\n"), Encoding.GetEncoding("utf-8"));
			}
			else
			{
				SQLite(0, string_0, novelInfo_0, strTask, string.Empty);
			}
		}
	}

	public static void Show(int int_0, string string_0, NovelInfo novelInfo_0, bool bool_0, string string_1, string string_2)
	{
		if (bool_0)
		{
			if (Configs.BaseConfig.SelectLog.IndexOf("," + int_0 + ",") < 0)
			{
				return;
			}
			lock (object_0)
			{
				if (novelInfo_0 == null)
				{
					novelInfo_0 = new NovelInfo();
					novelInfo_0.GetID = "0";
					novelInfo_0.Name = "未知";
					novelInfo_0.PutID = 0;
				}
				if (Configs.CmdModel)
				{
					Console.WriteLine("日志记录：" + int_0 + "|" + string_0 + "\t" + novelInfo_0.GetID + "|" + novelInfo_0.Name + "|" + novelInfo_0.PutID);
				}
				if (Configs.BaseConfig.LogType == 0)
				{
					File.AppendAllText(Application.StartupPath + "/Log/" + DateTime.Today.ToString("yyyyMMdd") + ".Log", ("------------------------------------------------------------\n时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n提示：" + int_0 + "|" + string_0 + "\n小说：" + novelInfo_0.GetID + "|" + novelInfo_0.Name + "|" + novelInfo_0.PutID + "\n方案：" + string_1 + "|" + string_2 + "\n").Replace("\n", "\r\n"), Encoding.GetEncoding("utf-8"));
					return;
				}
				SQLite(int_0, string_0, novelInfo_0, string_1, string_2);
			}
		}
		bool cmdModel = Configs.CmdModel;
	}

	public static void SQLite(int int_0, string string_0, NovelInfo novelInfo_0, string string_1, string string_2)
	{
		FileInfo fileInfo = new FileInfo(Application.StartupPath + "/Log/" + DateTime.Today.ToString("yyyyMMdd") + ".db3");
		int num = 1;
		try
		{
			num = Convert.ToInt32(Configs.BaseConfig.sqliteTime);
		}
		catch
		{
			num = 1;
		}
		DateTime now = DateTime.Now;
		string text = "";
		string[] array = recursed("Log/", new string[1] { "*.db3" });
		foreach (string text2 in array)
		{
			string text3 = text2.Replace(".db3", "").Replace(Application.StartupPath + "/Log/", "");
			if (text3.StartsWith("20") && text3.Length == 8)
			{
				string text4 = text3.Substring(0, 4);
				string text5 = text3.Substring(4, 2);
				string text6 = text3.Substring(6, 2);
				if (Convert.ToDateTime(text4 + "-" + text5 + "-" + text6).AddDays(num) >= now)
				{
					text = text + text2 + ",";
				}
			}
		}
		string[] array2 = text.TrimEnd(',').Split(',');
		fileInfo = ((array2.Length == 0 || string.IsNullOrEmpty(array2[array2.Length - 1])) ? new FileInfo(Application.StartupPath + "/Log/" + DateTime.Today.ToString("yyyyMMdd") + ".db3") : new FileInfo(array2[array2.Length - 1]));
		string string_3 = "Data Source=" + fileInfo.FullName;
		if (!fileInfo.Exists)
		{
			// Microsoft.Data.Sqlite 在首次打开连接时自动创建数据库文件，无需 CreateFile。
			string string_4 = "CREATE TABLE [TaskLog] ([EXID] INT,[TASKFILE] NVARCHAR(100),[RULEFILE] NVARCHAR(100),[EXMSG] NTEXT,[NID] INT,[NOVELNAME] NVARCHAR(200),[GETID] NVARCHAR(100),[NOVELURL] NVARCHAR(300),[INDEXURL] NVARCHAR(300),[LASTTIME] NVARCHAR(100),[LASTNUM] INT);";
			SQLiteHelper.ExecuteNonQuery(string_3, string_4, (IDataParameter[])null);
		}
		string string_5 = "SELECT Count(*) FROM [TaskLog] WHERE EXID=@exid AND RULEFILE=@rulefile AND NOVELNAME=@novelname AND EXMSG=@exmsg AND GETID=@getid";
		if (Convert.ToInt32(SQLiteHelper.ExecuteScalar(string_3, string_5,
			new SqliteParameter("@exid", int_0),
			new SqliteParameter("@rulefile", string_2),
			new SqliteParameter("@novelname", novelInfo_0.Name),
			new SqliteParameter("@exmsg", string_0),
			new SqliteParameter("@getid", novelInfo_0.GetID))) == 0)
		{
			string text7 = "";
			string text8 = "";
			if (novelInfo_0.NovelUrl != null)
			{
				text7 = novelInfo_0.NovelUrl.AbsoluteUri;
			}
			if (novelInfo_0.IndexUrl != null)
			{
				text8 = novelInfo_0.IndexUrl.AbsoluteUri;
			}
			int num2 = SecurityUtil.ConvertDateTimeInt(DateTime.Now);
			string string_6 = "INSERT INTO [TaskLog] (EXID,TASKFILE,RULEFILE,EXMSG,NOVELNAME,NOVELURL,INDEXURL,NID,GETID,LASTTIME,LASTNUM) values (@exid,@taskfile,@rulefile,@exmsg,@novelname,@novelurl,@indexurl,@nid,@getid,@lasttime,@lastnum)";
			SQLiteHelper.ExecuteNonQuery(string_3, string_6,
				new SqliteParameter("@exid", int_0),
				new SqliteParameter("@taskfile", string_1),
				new SqliteParameter("@rulefile", string_2),
				new SqliteParameter("@exmsg", string_0),
				new SqliteParameter("@novelname", novelInfo_0.Name),
				new SqliteParameter("@novelurl", text7),
				new SqliteParameter("@indexurl", text8),
				new SqliteParameter("@nid", novelInfo_0.PutID),
				new SqliteParameter("@getid", novelInfo_0.GetID),
				new SqliteParameter("@lasttime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
				new SqliteParameter("@lastnum", num2));
		}
	}

	public static DataTable GetSQLite(string ruleName, int countTime)
	{
		FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\" + ruleName.Replace("Rules", "Data") + ".db3");
		string string_ = "Data Source=" + fileInfo.FullName;
		Path.GetDirectoryName(fileInfo.FullName);
		if (File.Exists(fileInfo.FullName))
		{
			string string_2 = "SELECT * FROM [IpInfo] WHERE ISLOCK=0 AND ADDNUMTIME>@addnumtime ORDER BY ADDNUMTIME DESC";
			return SQLiteHelper.ExecuteDataset(string_, string_2, new SqliteParameter("@addnumtime", FormatText.GetTime(DateTime.Now) - countTime * 60)).Tables[0];
		}
		return new DataTable();
	}

	public static DataTable GetSQLite(string ruleName, int countTime, int goTime)
	{
		FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\" + ruleName.Replace("Rules", "Data") + ".db3");
		string string_ = "Data Source=" + fileInfo.FullName;
		Path.GetDirectoryName(fileInfo.FullName);
		if (File.Exists(fileInfo.FullName))
		{
			string string_2 = "SELECT * FROM [IpInfo] WHERE ISLOCK=0 AND ADDNUMTIME>@addnumtime AND GONUMTIME<@gonumtime ORDER BY RANDOM() LIMIT 0,10";
			return SQLiteHelper.ExecuteDataset(string_, string_2,
				new SqliteParameter("@addnumtime", FormatText.GetTime(DateTime.Now) - countTime * 60),
				new SqliteParameter("@gonumtime", FormatText.GetTime(DateTime.Now) - goTime)).Tables[0];
		}
		return new DataTable();
	}

	public static void UpLockSQLite(string ruleName, string ip)
	{
		FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\" + ruleName.Replace("Rules", "Data") + ".db3");
		string string_ = "Data Source=" + fileInfo.FullName;
		Path.GetDirectoryName(fileInfo.FullName);
		if (File.Exists(fileInfo.FullName))
		{
			string string_2 = "UPDATE [IpInfo] SET ISLOCK=1 WHERE IP=@ip";
			SQLiteHelper.ExecuteNonQuery(string_, string_2, new SqliteParameter("@ip", ip));
		}
	}

	public static void UpTimeSQLite(string ruleName, string ip)
	{
		FileInfo fileInfo = new FileInfo(Application.StartupPath + "\\" + ruleName.Replace("Rules", "Data") + ".db3");
		string string_ = "Data Source=" + fileInfo.FullName;
		Path.GetDirectoryName(fileInfo.FullName);
		if (File.Exists(fileInfo.FullName))
		{
			string string_2 = "UPDATE [IpInfo] SET ISLOCK=0, GONUMTIME=@gonumtime WHERE IP=@ip";
			SQLiteHelper.ExecuteNonQuery(string_, string_2,
				new SqliteParameter("@gonumtime", FormatText.GetTime(DateTime.Now)),
				new SqliteParameter("@ip", ip));
		}
	}
}
