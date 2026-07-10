using System.Collections.Generic;
using System.IO;

namespace NovelSpider.Common;

public class IO
{
	public static void CopyFiles(string string_0, string string_1, bool bool_0, bool bool_1)
	{
		IEnumerable<string> files = Directory.EnumerateFiles(string_0);
		if (bool_0)
		{
			IEnumerable<string> array = files;
			foreach (string text in array)
			{
				if (!Directory.Exists(string_1))
				{
					Directory.CreateDirectory(string_1);
				}
				File.Copy(text, string_1 + text.Substring(text.LastIndexOf("\\")), overwrite: true);
			}
		}
		else
		{
			IEnumerable<string> array2 = files;
			foreach (string text2 in array2)
			{
				if (!File.Exists(string_1 + text2.Substring(text2.LastIndexOf("\\"))))
				{
					File.Copy(text2, string_1 + text2.Substring(text2.LastIndexOf("\\")));
				}
			}
		}
		if (!bool_1)
		{
			return;
		}
		IEnumerable<string> directories = Directory.EnumerateDirectories(string_0);
		foreach (string text3 in directories)
		{
			string text4 = string_1 + text3.Substring(text3.LastIndexOf("\\"));
			if (!Directory.Exists(text4))
			{
				Directory.CreateDirectory(text4);
			}
			CopyFiles(text3, text4, bool_0, bool_1);
		}
	}

	public static string[] LoadLogs()
	{
		return LoadFilesOrEmpty("Log");
	}

	public static string[] LoadRules()
	{
		return LoadFilesOrEmpty("Rules");
	}

	public static string[] LoadTasks()
	{
		return LoadFilesOrEmpty("Tasks");
	}

	private static string[] LoadFilesOrEmpty(string directory)
	{
		if (!Directory.Exists(directory))
		{
			return [];
		}
		return Directory.GetFiles(directory);
	}
}
