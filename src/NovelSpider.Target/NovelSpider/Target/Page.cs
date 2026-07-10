using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;

namespace NovelSpider.Target;

public partial class Page
{
	private static byte byte_0;

	[CompilerGenerated]
	private static Comparison<ChapterInfo> comparison_0;

	[CompilerGenerated]
	private static Comparison<ChapterInfo> comparison_1;

	[CompilerGenerated]
	private static Comparison<ChapterInfo> comparison_2;

	[CompilerGenerated]
	private static Comparison<ChapterInfo> comparison_3;

	private RuleConfigInfo ruleConfigInfo_0;

	public TaskConfigInfo taskConfigInfo_0;

	static Page()
	{
		Net10RuntimeBootstrap.Initialize();
	}

	public Page(RuleConfigInfo ruleConfigInfo_1, RepairConfigInfo repairConfigInfo_0)
	{
		ruleConfigInfo_0 = ruleConfigInfo_1;
		taskConfigInfo_0 = new TaskConfigInfo
		{
			DownImage = true,
			GoRepeatChapter = repairConfigInfo_0.GoRepeatChapter,
			Proxy = repairConfigInfo_0.Proxy,
			ProxyDomain = repairConfigInfo_0.ProxyDomain,
			ProxyHost = repairConfigInfo_0.ProxyHost,
			ProxyPassword = repairConfigInfo_0.ProxyPassword,
			ProxyPort = repairConfigInfo_0.ProxyPort,
			ProxyUser = repairConfigInfo_0.ProxyUser,
			DelChapter = repairConfigInfo_0.Md5Chapter,
			ReplaceChapterNameOn = repairConfigInfo_0.ReplaceChapterNameOn,
			EmptyChapter = repairConfigInfo_0.EmptyChapter,
			OrderChapter = repairConfigInfo_0.OrderChapter,
			RepeatChapterHandle = repairConfigInfo_0.RepeatChapterHandle
		};
	}

	public Page(RuleConfigInfo ruleConfigInfo_1, ReplaceConfigInfo replaceConfigInfo_0)
	{
		ruleConfigInfo_0 = ruleConfigInfo_1;
		taskConfigInfo_0 = new TaskConfigInfo
		{
			DownImage = true,
			DownImageError = replaceConfigInfo_0.DownImageError,
			GoRepeatChapter = replaceConfigInfo_0.GoRepeatChapter,
			Proxy = replaceConfigInfo_0.Proxy,
			ProxyDomain = replaceConfigInfo_0.ProxyDomain,
			ProxyHost = replaceConfigInfo_0.ProxyHost,
			ProxyPassword = replaceConfigInfo_0.ProxyPassword,
			ProxyPort = replaceConfigInfo_0.ProxyPort,
			ProxyUser = replaceConfigInfo_0.ProxyUser,
			DelChapter = replaceConfigInfo_0.Md5Chapter,
			ReplaceChapterNameOn = replaceConfigInfo_0.ReplaceChapterNameOn,
			EmptyChapter = replaceConfigInfo_0.EmptyChapter,
			OrderChapter = replaceConfigInfo_0.OrderChapter,
			RepeatChapterHandle = replaceConfigInfo_0.RepeatChapterHandle
		};
	}

	public Page(RuleConfigInfo ruleConfigInfo_1, TaskConfigInfo taskConfigInfo_1)
	{
		ruleConfigInfo_0 = ruleConfigInfo_1;
		taskConfigInfo_0 = taskConfigInfo_1;
	}
	private static string GetStringWorkWithEmptyRetry(HttpClient httpClient, int maxAttempts = 2, string subject = "")
	{
		string text = string.Empty;
		for (int attempt = 1; attempt <= maxAttempts; attempt++)
		{
			using IDisposable measure = PerformanceTelemetry.Measure("http", "get_string_work", subject);
			text = httpClient.GetStringWork();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (attempt < maxAttempts)
			{
				PerformanceTelemetry.Record("http", "empty_response_retry", 0, subject, succeed: false, message: $"attempt={attempt}");
			}
		}
		return text;
	}

	public NovelInfo GetChapterInfo(NovelInfo novelInfo_0, bool isvip)
	{
		using IDisposable chapterScope = PerformanceTelemetry.Measure("collect", "chapter_total", novelInfo_0?.LastChapter?.ChapterName ?? novelInfo_0?.Name);
		string text = ruleConfigInfo_0.PubContentUrl.Pattern;
		if (text.IndexOf("{NovelKey}") >= 0)
		{
			text = text.Replace("{NovelKey}", novelInfo_0.GetID);
		}
		if (text.IndexOf("{NovelPubKey}") >= 0)
		{
			text = text.Replace("{NovelPubKey}", novelInfo_0.PubKey);
		}
		if (text.IndexOf("{ChapterKey}") >= 0)
		{
			text = text.Replace("{ChapterKey}", novelInfo_0.LastChapter.GetID);
		}
		if (text.IndexOf("{NovelKey/1000}") >= 0)
		{
			try
			{
				int num = int.Parse(novelInfo_0.GetID);
				text = text.Replace("{NovelKey/1000}", Convert.ToString(num / 1000));
			}
			catch
			{
			}
		}
		novelInfo_0.LastChapter.ChapterUrl = new Uri(novelInfo_0.IndexUrl, text);
		WaitForChapterRequestInterval(novelInfo_0.LastChapter.ChapterUrl.Host);
		HttpClient httpClient = new HttpClient
		{
			Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
			Timeout = Configs.BaseConfig.HttpTimeOut,
			Proxy = taskConfigInfo_0.Proxy,
			ProxyHost = taskConfigInfo_0.ProxyHost,
			ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
			ProxyDomain = taskConfigInfo_0.ProxyDomain,
			ProxyUser = taskConfigInfo_0.ProxyUser,
			ProxyPassword = taskConfigInfo_0.ProxyPassword,
			UriString = novelInfo_0.LastChapter.ChapterUrl.AbsoluteUri,
			Referer = novelInfo_0.IndexUrl.AbsoluteUri,
			UserAgent = Configs.BaseConfig.HttpUserAgent,
			Cookies = ruleConfigInfo_0.PubCookies.Pattern
		};
		string stringWork = GetStringWorkWithEmptyRetry(httpClient, subject: novelInfo_0.LastChapter.ChapterUrl.AbsoluteUri);
		if (!string.IsNullOrEmpty(ruleConfigInfo_0.PubContent_GetTextKey.Pattern))
		{
			try
			{
				string newValue = Match(stringWork, ruleConfigInfo_0.PubContent_GetTextKey);
				string text2 = ruleConfigInfo_0.PubTextUrl.Pattern;
				if (text2.IndexOf("{TextKey}") >= 0)
				{
					text2 = text2.Replace("{TextKey}", newValue);
				}
				if (text2.IndexOf("{NovelKey}") >= 0)
				{
					text2 = text2.Replace("{NovelKey}", novelInfo_0.GetID);
				}
				if (text2.IndexOf("{NovelPubKey}") >= 0)
				{
					text2 = text2.Replace("{NovelPubKey}", novelInfo_0.PubKey);
				}
				if (text2.IndexOf("{ChapterKey}") >= 0)
				{
					text2 = text2.Replace("{ChapterKey}", novelInfo_0.LastChapter.GetID);
				}
				if (text2.IndexOf("{NovelKey/1000}") >= 0)
				{
					try
					{
						int num2 = int.Parse(novelInfo_0.GetID);
						text2 = text2.Replace("{NovelKey/1000}", Convert.ToString(num2 / 1000));
					}
					catch
					{
					}
				}
				novelInfo_0.LastChapter.TextUrl = new Uri(novelInfo_0.LastChapter.ChapterUrl, text2);
				HttpClient httpClient2 = new HttpClient
				{
					Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
					Proxy = taskConfigInfo_0.Proxy,
					ProxyHost = taskConfigInfo_0.ProxyHost,
					ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
					ProxyDomain = taskConfigInfo_0.ProxyDomain,
					ProxyUser = taskConfigInfo_0.ProxyUser,
					ProxyPassword = taskConfigInfo_0.ProxyPassword,
					UriString = novelInfo_0.LastChapter.TextUrl.AbsoluteUri,
					Referer = novelInfo_0.LastChapter.ChapterUrl.AbsoluteUri
				};
				stringWork = GetStringWorkWithEmptyRetry(httpClient2, 3, novelInfo_0.LastChapter.TextUrl.AbsoluteUri);
			}
			catch
			{
			}
		}
		if (isvip && ruleConfigInfo_0.PubContentChapterName.Pattern.Trim() != "")
		{
			novelInfo_0.LastChapter.ChapterName = Match(stringWork, ruleConfigInfo_0.PubContentChapterName, bool_0: true);
			return novelInfo_0;
		}
		novelInfo_0.LastChapter.ChapterText = Match(stringWork, ruleConfigInfo_0.PubContentText, bool_0: true);
		if (ruleConfigInfo_0.PubContentPage.Pattern != "")
		{
			string input = Match(stringWork, ruleConfigInfo_0.PubContentPageArea, bool_0: true);
			string text3 = ruleConfigInfo_0.PubContentPage.Pattern;
			if (text3.IndexOf("{NovelKey}") >= 0)
			{
				text3 = text3.Replace("{NovelKey}", novelInfo_0.GetID);
			}
			if (text3.IndexOf("{NovelPubKey}") >= 0)
			{
				text3 = text3.Replace("{NovelPubKey}", novelInfo_0.PubKey);
			}
			if (text3.IndexOf("{ChapterKey}") >= 0)
			{
				text3 = text3.Replace("{ChapterKey}", novelInfo_0.LastChapter.GetID);
			}
			if (text3.IndexOf("{NovelKey/1000}") >= 0)
			{
				try
				{
					int num3 = int.Parse(novelInfo_0.GetID);
					text3 = text3.Replace("{NovelKey/1000}", Convert.ToString(num3 / 1000));
				}
				catch
				{
				}
			}
			MatchCollection matchCollection = RuleRegexCache.Get(text3, ruleConfigInfo_0.PubContentPage.Options).Matches(input);
			for (int i = 0; i < matchCollection.Count; i++)
			{
				text = ruleConfigInfo_0.PubContentUrl.Pattern;
				if (text.IndexOf("{NovelKey}") >= 0)
				{
					text = text.Replace("{NovelKey}", novelInfo_0.GetID);
				}
				if (text.IndexOf("{NovelPubKey}") >= 0)
				{
					text = text.Replace("{NovelPubKey}", novelInfo_0.PubKey);
				}
				if (text.IndexOf("{ChapterKey}") >= 0)
				{
					text = text.Replace("{ChapterKey}", matchCollection[i].Groups[1].Value.Trim());
				}
				if (text.IndexOf("{NovelKey/1000}") >= 0)
				{
					try
					{
						int num4 = int.Parse(novelInfo_0.GetID);
						text = text.Replace("{NovelKey/1000}", Convert.ToString(num4 / 1000));
					}
					catch
					{
					}
				}
				Uri uri = new Uri(novelInfo_0.IndexUrl, text);
				HttpClient httpClient3 = new HttpClient
				{
					Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
					Proxy = taskConfigInfo_0.Proxy,
					ProxyHost = taskConfigInfo_0.ProxyHost,
					ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
					ProxyDomain = taskConfigInfo_0.ProxyDomain,
					ProxyUser = taskConfigInfo_0.ProxyUser,
					ProxyPassword = taskConfigInfo_0.ProxyPassword,
					UriString = uri.AbsoluteUri,
					Referer = uri.AbsoluteUri
				};
				string stringWork2 = GetStringWorkWithEmptyRetry(httpClient3, subject: uri.AbsoluteUri);
				novelInfo_0.LastChapter.ChapterText += Match(stringWork2, ruleConfigInfo_0.PubContentText, bool_0: true);
			}
		}
		if (novelInfo_0.LastChapter.TextUrl != null && novelInfo_0.LastChapter.TextUrl.Host == "files.uutxt.com" && (Configs.UserID == new Guid("0b6cac3a-1a64-4ff4-9f3f-2ba8b17c74f2") || Configs.UserID == new Guid("47d85a03-0b98-43d6-a09e-aca6a8c12afa") || Configs.UserID == new Guid("b807f493-60d0-4e64-a193-88d3d16feb77") || Configs.UserID == new Guid("4c1c278b-5c3b-4f1d-966a-7164c22e9510")))
		{
			novelInfo_0.LastChapter.ChapterText = HttpUtility.UrlDecode(novelInfo_0.LastChapter.ChapterText);
		}
		return novelInfo_0;
	}

	private void WaitForChapterRequestInterval(string host)
	{
		int chapterUrlWait = taskConfigInfo_0?.ChapterUrlWait ?? 0;
		HostRequestThrottle.Wait(host, chapterUrlWait);
	}

	public ChapterInfo[] GetChapterList(NovelInfo novelInfo_0)
	{
		SpiderException.Debug("Page.GetChapterList");
		string text = ruleConfigInfo_0.PubIndexUrl.Pattern;
		if (text.IndexOf("{NovelKey}") >= 0)
		{
			text = text.Replace("{NovelKey}", novelInfo_0.GetID);
		}
		if (text.IndexOf("{NovelPubKey}") >= 0)
		{
			text = text.Replace("{NovelPubKey}", novelInfo_0.PubKey);
		}
		if (text.IndexOf("{NovelKey/1000}") >= 0)
		{
			try
			{
				int num = int.Parse(novelInfo_0.GetID);
				text = text.Replace("{NovelKey/1000}", Convert.ToString(num / 1000));
			}
			catch
			{
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			novelInfo_0 = GetNovelInfo(novelInfo_0);
			text = ruleConfigInfo_0.PubIndexUrl.Pattern;
			if (text.IndexOf("{NovelKey}") >= 0)
			{
				text = text.Replace("{NovelKey}", novelInfo_0.GetID);
			}
			if (text.IndexOf("{NovelPubKey}") >= 0)
			{
				text = text.Replace("{NovelPubKey}", novelInfo_0.PubKey);
			}
			if (text.IndexOf("{NovelKey/1000}") >= 0)
			{
				try
				{
					int num2 = int.Parse(novelInfo_0.GetID);
					text = text.Replace("{NovelKey/1000}", Convert.ToString(num2 / 1000));
				}
				catch
				{
				}
			}
		}
		novelInfo_0.IndexUrl = new Uri(novelInfo_0.NovelUrl, text);
		SpiderException.Debug("Page.GetChapterList " + novelInfo_0.IndexUrl.AbsoluteUri);
		HttpClient httpClient = new HttpClient
		{
			Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
			Proxy = taskConfigInfo_0.Proxy,
			ProxyHost = taskConfigInfo_0.ProxyHost,
			ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
			ProxyDomain = taskConfigInfo_0.ProxyDomain,
			ProxyUser = taskConfigInfo_0.ProxyUser,
			ProxyPassword = taskConfigInfo_0.ProxyPassword,
			UriString = novelInfo_0.IndexUrl.AbsoluteUri,
			Referer = novelInfo_0.NovelUrl.AbsoluteUri
		};
		string text2 = GetStringWorkWithEmptyRetry(httpClient, subject: novelInfo_0.NovelUrl.AbsoluteUri);
		if (string.IsNullOrEmpty(text2) || RuleRegexCache.Get(ruleConfigInfo_0.PubIndexErr.Pattern, ruleConfigInfo_0.PubIndexErr.Options).IsMatch(text2))
		{
			throw new ApplicationException("当前小说页不存在");
		}
		if (!string.IsNullOrEmpty(ruleConfigInfo_0.PubVolumeContent.Pattern))
		{
			text2 = Match(text2, ruleConfigInfo_0.PubVolumeContent);
			string string_ = ruleConfigInfo_0.PubVolumeContent.FilterPattern.Replace("{$小说名称$}", novelInfo_0.Name).Replace("{$分类名称$}", novelInfo_0.LagerSort).Replace("{$小说作者$}", novelInfo_0.Author);
			text2 = RnReplace(text2, string_, ruleConfigInfo_0.PubVolumeContent);
		}
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		string[] array = RuleRegexCache.Get(ruleConfigInfo_0.PubVolumeSplit.Pattern, ruleConfigInfo_0.PubVolumeSplit.Options).Split(text2);
		for (int i = 0; i < array.Length; i++)
		{
			string text3 = WhitespaceRegex().Replace(Match(array[i], ruleConfigInfo_0.PubVolumeName, bool_0: true).Replace("{$小说名称$}", novelInfo_0.Name).Replace("{$分类名称$}", novelInfo_0.LagerSort).Replace("{$小说作者$}", novelInfo_0.Author), " ").Replace("&nbsp;", " ").Trim();
			MatchCollection matchCollection = RuleRegexCache.Get(ruleConfigInfo_0.PubChapterName.Pattern, ruleConfigInfo_0.PubChapterName.Options).Matches(array[i]);
			MatchCollection matchCollection2 = RuleRegexCache.Get(ruleConfigInfo_0.PubChapter_GetChapterKey.Pattern, ruleConfigInfo_0.PubChapter_GetChapterKey.Options).Matches(array[i]);
			if (matchCollection.Count != matchCollection2.Count)
			{
				throw new ApplicationException("VolumeSplit第" + i + "段中PubChapterName和PubChapter_GetChapterKey匹配数量不相等！");
			}
			for (int j = 0; j < matchCollection.Count; j++)
			{
				ChapterInfo chapterInfo = new ChapterInfo();
				if (string.IsNullOrEmpty(text3.Trim()))
				{
					text3 = Configs.BaseConfig.DefaultVolumeName;
				}
				chapterInfo.VolumeName = text3.Trim();
				string string_2 = WhitespaceRegex().Replace(matchCollection[j].Groups[1].Value.Trim(), " ").Replace("'", "").Trim();
				string string_3 = ruleConfigInfo_0.PubChapterName.FilterPattern.Replace("{$书卷名称$}", chapterInfo.VolumeName).Replace("{$小说名称$}", novelInfo_0.Name).Replace("{$分类名称$}", novelInfo_0.LagerSort)
					.Replace("{$小说作者$}", novelInfo_0.Author);
				chapterInfo.ChapterName = RnReplace(string_2, string_3, ruleConfigInfo_0.PubChapterName).Trim();
				chapterInfo.ChapterName = chapterInfo.ChapterName.Replace("&nbsp;", " ");
				chapterInfo.ChapterName = Translate(chapterInfo.ChapterName);
				chapterInfo.GetID = RuleRegexCache.Get(ruleConfigInfo_0.PubChapter_GetChapterKey.FilterPattern).Replace(matchCollection2[j].Groups[1].Value.Trim(), "");
				arrayList.Add(chapterInfo);
			}
		}
		ChapterInfo[] array2 = arrayList.ToArray();
		List<ChapterInfo> list = new List<ChapterInfo>();
		switch (taskConfigInfo_0.OrderChapter)
		{
		case 1:
		{
			Stack<ChapterInfo> stack = new Stack<ChapterInfo>();
			for (int num3 = 0; num3 < array2.Length; num3++)
			{
				stack.Push(array2[num3]);
			}
			for (int num4 = 0; num4 < array2.Length; num4++)
			{
				array2[num4] = stack.Pop();
			}
			stack.Clear();
			break;
		}
		case 2:
			if (FormatText.GetInt(array2[0].GetID, -1) >= 0)
			{
				list.AddRange(array2);
				list.Sort((ChapterInfo chapterInfo_0, ChapterInfo chapterInfo_1) => Comparer<int>.Default.Compare(Convert.ToInt32(chapterInfo_0.GetID), Convert.ToInt32(chapterInfo_1.GetID)));
				array2 = list.ToArray();
			}
			break;
		case 3:
			if (FormatText.GetInt(array2[0].GetID, -1) >= 0)
			{
				list.AddRange(array2);
				list.Sort((ChapterInfo chapterInfo_0, ChapterInfo chapterInfo_1) => Comparer<int>.Default.Compare(Convert.ToInt32(chapterInfo_1.GetID), Convert.ToInt32(chapterInfo_0.GetID)));
				array2 = list.ToArray();
			}
			break;
		case 4:
			if (FormatText.GetInt(array2[0].GetID, -1) >= 0)
			{
				list.AddRange(array2);
				list.Sort((ChapterInfo chapterInfo_0, ChapterInfo chapterInfo_1) => Comparer<int>.Default.Compare(Convert.ToInt32(chapterInfo_0.ChapterName), Convert.ToInt32(chapterInfo_1.ChapterName)));
				array2 = list.ToArray();
			}
			break;
		case 5:
			if (FormatText.GetInt(array2[0].GetID, -1) >= 0)
			{
				list.AddRange(array2);
				list.Sort((ChapterInfo chapterInfo_0, ChapterInfo chapterInfo_1) => Comparer<int>.Default.Compare(Convert.ToInt32(chapterInfo_1.ChapterName), Convert.ToInt32(chapterInfo_0.ChapterName)));
				array2 = list.ToArray();
			}
			break;
		}
		list.Clear();
		if (Configs.BaseConfig.LicenseVip && taskConfigInfo_0.ReplaceChapterNameNun > 0 && ruleConfigInfo_0.PubContentChapterName.Pattern != "")
		{
			int num5 = taskConfigInfo_0.ReplaceChapterNameNun;
			if (num5 > array2.Length)
			{
				num5 = array2.Length;
			}
			ChapterInfo chapterInfo = novelInfo_0.LastChapter;
			for (int i = 0; i < num5; i++)
			{
				novelInfo_0.LastChapter = array2[array2.Length - (num5 - i)];
				novelInfo_0 = GetChapterInfo(novelInfo_0, isvip: true);
				array2[array2.Length - (num5 - i)].ChapterName = novelInfo_0.LastChapter.ChapterName;
			}
			novelInfo_0.LastChapter = chapterInfo;
		}
		return array2;
	}

	public ChapterInfo[] GetChapterList(NovelInfo novelInfo_0, bool bool_0)
	{
		SpiderException.Debug("Page.GetChapterList");
		string text = ruleConfigInfo_0.PubIndexUrl.Pattern;
		if (text.IndexOf("{NovelKey}") >= 0)
		{
			text = text.Replace("{NovelKey}", novelInfo_0.GetID);
		}
		if (text.IndexOf("{NovelPubKey}") >= 0)
		{
			text = text.Replace("{NovelPubKey}", novelInfo_0.PubKey);
		}
		if (text.IndexOf("{NovelKey/1000}") >= 0)
		{
			try
			{
				int num = int.Parse(novelInfo_0.GetID);
				text = text.Replace("{NovelKey/1000}", Convert.ToString(num / 1000));
			}
			catch
			{
			}
		}
		if (string.IsNullOrEmpty(text))
		{
			novelInfo_0 = GetNovelInfo(novelInfo_0);
			text = ruleConfigInfo_0.PubIndexUrl.Pattern;
			if (text.IndexOf("{NovelKey}") >= 0)
			{
				text = text.Replace("{NovelKey}", novelInfo_0.GetID);
			}
			if (text.IndexOf("{NovelPubKey}") >= 0)
			{
				text = text.Replace("{NovelPubKey}", novelInfo_0.PubKey);
			}
			if (text.IndexOf("{NovelKey/1000}") >= 0)
			{
				try
				{
					int num2 = int.Parse(novelInfo_0.GetID);
					text = text.Replace("{NovelKey/1000}", Convert.ToString(num2 / 1000));
				}
				catch
				{
				}
			}
		}
		novelInfo_0.IndexUrl = new Uri(novelInfo_0.NovelUrl, text);
		SpiderException.Debug("Page.GetChapterList " + novelInfo_0.IndexUrl.AbsoluteUri);
		HttpClient httpClient = new HttpClient
		{
			Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
			Proxy = taskConfigInfo_0.Proxy,
			ProxyHost = taskConfigInfo_0.ProxyHost,
			ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
			ProxyDomain = taskConfigInfo_0.ProxyDomain,
			ProxyUser = taskConfigInfo_0.ProxyUser,
			ProxyPassword = taskConfigInfo_0.ProxyPassword,
			UriString = novelInfo_0.IndexUrl.AbsoluteUri,
			Referer = novelInfo_0.NovelUrl.AbsoluteUri
		};
		string text2 = GetStringWorkWithEmptyRetry(httpClient, subject: novelInfo_0.NovelUrl.AbsoluteUri);
		if (string.IsNullOrEmpty(text2) || RuleRegexCache.Get(ruleConfigInfo_0.PubIndexErr.Pattern, ruleConfigInfo_0.PubIndexErr.Options).IsMatch(text2))
		{
			throw new ApplicationException("当前小说页不存在");
		}
		if (!string.IsNullOrEmpty(ruleConfigInfo_0.PubVolumeContent.Pattern))
		{
			text2 = Match(text2, ruleConfigInfo_0.PubVolumeContent);
			string string_ = ruleConfigInfo_0.PubVolumeContent.FilterPattern.Replace("{$小说名称$}", novelInfo_0.Name).Replace("{$分类名称$}", novelInfo_0.LagerSort).Replace("{$小说作者$}", novelInfo_0.Author);
			text2 = RnReplace(text2, string_, ruleConfigInfo_0.PubVolumeContent);
		}
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		string[] array = ((!(ruleConfigInfo_0.PubVolumeSplit.Pattern != "")) ? RuleRegexCache.Get("000000000000000000000", ruleConfigInfo_0.PubVolumeSplit.Options).Split(text2) : RuleRegexCache.Get(ruleConfigInfo_0.PubVolumeSplit.Pattern, ruleConfigInfo_0.PubVolumeSplit.Options).Split(text2));
		for (int i = 0; i < array.Length; i++)
		{
			ruleConfigInfo_0.PubVolumeName.FilterPattern = ruleConfigInfo_0.PubVolumeName.FilterPattern.Replace("{$小说名称$}", novelInfo_0.Name).Replace("{$分类名称$}", novelInfo_0.LagerSort).Replace("{$小说作者$}", novelInfo_0.Author)
				.Replace("&nbsp;", " ")
				.Trim();
			string text3 = WhitespaceRegex().Replace(Match(array[i], ruleConfigInfo_0.PubVolumeName, bool_0: true), " ").Replace("&nbsp;", " ").Trim();
			MatchCollection matchCollection = RuleRegexCache.Get(ruleConfigInfo_0.PubChapterName.Pattern, ruleConfigInfo_0.PubChapterName.Options).Matches(array[i]);
			MatchCollection matchCollection2 = RuleRegexCache.Get(ruleConfigInfo_0.PubChapter_GetChapterKey.Pattern, ruleConfigInfo_0.PubChapter_GetChapterKey.Options).Matches(array[i]);
			if (matchCollection.Count != matchCollection2.Count)
			{
				throw new ApplicationException("VolumeSplit第" + i + "段中PubChapterName和PubChapter_GetChapterKey匹配数量不相等！");
			}
			for (int j = 0; j < matchCollection.Count; j++)
			{
				ChapterInfo chapterInfo = new ChapterInfo();
				if (string.IsNullOrEmpty(text3.Trim()))
				{
					text3 = Configs.BaseConfig.DefaultVolumeName;
				}
				chapterInfo.VolumeName = text3.Trim();
				string string_2 = WhitespaceRegex().Replace(matchCollection[j].Groups[1].Value.Trim(), " ").Replace("'", "").Trim();
				string string_3 = ruleConfigInfo_0.PubChapterName.FilterPattern.Replace("{$书卷名称$}", chapterInfo.VolumeName).Replace("{$小说名称$}", novelInfo_0.Name).Replace("{$分类名称$}", novelInfo_0.LagerSort)
					.Replace("{$小说作者$}", novelInfo_0.Author);
				chapterInfo.ChapterName = RnReplace(string_2, string_3, ruleConfigInfo_0.PubChapterName).Trim();
				chapterInfo.ChapterName = chapterInfo.ChapterName.Replace("&nbsp;", " ");
				chapterInfo.ChapterName = Translate(chapterInfo.ChapterName);
				chapterInfo.GetID = RuleRegexCache.Get(ruleConfigInfo_0.PubChapter_GetChapterKey.FilterPattern).Replace(matchCollection2[j].Groups[1].Value.Trim(), "");
				arrayList.Add(chapterInfo);
			}
		}
		return arrayList.ToArray();
	}

	public string[] GetIds(string[] arrayListUrl)
	{
		List<string> arrayList = new List<string>();
		foreach (string text in arrayListUrl)
		{
			string text2 = text;
			Match match = RangeTemplateRegex().Match(text);
			int num = 0;
			int num2 = 0;
			if (match.Success)
			{
				text2 = text.Replace(match.Groups[0].Value, "(*)");
				num = Convert.ToInt32(match.Groups[1].Value);
				num2 = Convert.ToInt32(match.Groups[2].Value);
			}
			for (int j = num; j <= num2; j++)
			{
				Uri uri;
				try
				{
					text2 = text2.Replace("(*)", j.ToString());
					if (text2.IndexOf("{TimeStamp}") > 0)
					{
						text2 = text2.Replace("{TimeStamp}", GetTimeStamp());
					}
					uri = new Uri(text2);
				}
				catch
				{
					SpiderException.Show(text + " 地址有问题", bool_0: true);
					continue;
				}
				HttpClient httpClient = new HttpClient
				{
					Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
					Proxy = taskConfigInfo_0.Proxy,
					ProxyHost = taskConfigInfo_0.ProxyHost,
					ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
					ProxyDomain = taskConfigInfo_0.ProxyDomain,
					ProxyUser = taskConfigInfo_0.ProxyUser,
					ProxyPassword = taskConfigInfo_0.ProxyPassword,
					UriString = uri.AbsoluteUri,
					Referer = ruleConfigInfo_0.GetSiteUrl.Pattern
				};
				HttpClient httpClient2 = httpClient;
				string stringWork = GetStringWorkWithEmptyRetry(httpClient2, subject: uri.AbsoluteUri);
				MatchCollection matchCollection = RuleRegexCache.Get(ruleConfigInfo_0.NovelList_GetNovelKey.Pattern, ruleConfigInfo_0.NovelList_GetNovelKey.Options).Matches(stringWork);
				for (int k = 0; k < matchCollection.Count; k++)
				{
					arrayList.Add(matchCollection[k].Groups[1].Value);
				}
			}
		}
		return arrayList.ToArray();
	}

	public NovelInfo GetNovelInfo(NovelInfo novelInfo_0)
	{
		if (byte_0 == 0)
		{
			smethod_0();
		}
		if (byte_0 == 1)
		{
			novelInfo_0 = null;
		}
		if (SecurityUtil.IsNum(ruleConfigInfo_0.RuleID.Pattern))
		{
			novelInfo_0.RuleID = int.Parse(ruleConfigInfo_0.RuleID.Pattern);
			novelInfo_0.RuleName = ruleConfigInfo_0.GetSiteName.Pattern;
		}
		else
		{
			novelInfo_0.RuleID = 0;
			novelInfo_0.RuleName = "";
		}
		if (string.IsNullOrEmpty(novelInfo_0.GetID) || novelInfo_0.GetID == "0")
		{
			novelInfo_0 = SearchNovel(novelInfo_0);
		}
		SpiderException.Debug("Page.GetNovelInfo");
		string text = ruleConfigInfo_0.NovelUrl.Pattern;
		if (ruleConfigInfo_0.NovelUrl.Pattern.IndexOf("{NovelKey}") >= 0)
		{
			text = text.Replace("{NovelKey}", novelInfo_0.GetID);
		}
		if (ruleConfigInfo_0.NovelUrl.Pattern.IndexOf("{NovelKey/1000}") >= 0)
		{
			try
			{
				int num = int.Parse(novelInfo_0.GetID);
				text = text.Replace("{NovelKey/1000}", Convert.ToString(num / 1000));
			}
			catch
			{
			}
		}
		novelInfo_0.NovelUrl = new Uri(text);
		HttpClient httpClient = new HttpClient
		{
			Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
			Proxy = taskConfigInfo_0.Proxy,
			ProxyHost = taskConfigInfo_0.ProxyHost,
			ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
			ProxyDomain = taskConfigInfo_0.ProxyDomain,
			ProxyUser = taskConfigInfo_0.ProxyUser,
			ProxyPassword = taskConfigInfo_0.ProxyPassword,
			UriString = novelInfo_0.NovelUrl.AbsoluteUri,
			Referer = ruleConfigInfo_0.GetSiteUrl.Pattern
		};
		HttpClient httpClient2 = httpClient;
		string stringWork = GetStringWorkWithEmptyRetry(httpClient2, subject: novelInfo_0.NovelUrl.AbsoluteUri);
		SpiderException.Debug("Page.GetNovelInfo Rule.NovelErr");
		if (string.IsNullOrEmpty(stringWork) || RuleRegexCache.Get(ruleConfigInfo_0.NovelErr.Pattern, ruleConfigInfo_0.NovelErr.Options).IsMatch(stringWork))
		{
			throw new ApplicationException("当前小说页不存在");
		}
		if (novelInfo_0.PutID == 0)
		{
			SpiderException.Debug("Page.GetNovelInfo Rule.Name");
			novelInfo_0.Name = Match(stringWork, ruleConfigInfo_0.NovelName);
			if (Configs.BaseConfig.FilterNovelName != null && ("♂" + Configs.BaseConfig.FilterNovelName + "♂").IndexOf("♂" + novelInfo_0.Name + "♂") >= 0)
			{
				throw new ApplicationException("违禁小说《" + novelInfo_0.Name + "》");
			}
			novelInfo_0.Author = Match(stringWork, ruleConfigInfo_0.NovelAuthor);
			if (double.Parse(Configs.BaseConfig.CmsVersion) >= 1.8)
			{
				novelInfo_0.Isboy = Match(stringWork, ruleConfigInfo_0.Isboy);
				SpiderException.Debug("Page.GetNovelInfo 对比频道开始");
				if (!string.IsNullOrEmpty(Configs.BaseConfig.isboyCorresponding))
				{
					string[] array = Configs.BaseConfig.isboyCorresponding.Replace("\r\n", "♂").Split('♂');
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].IndexOf("," + novelInfo_0.Isboy + ",") > 0)
						{
							novelInfo_0.IsboyID = Convert.ToInt32(array[i].Substring(0, array[i].IndexOf("|")));
							novelInfo_0.Isboy = array[i].Substring(array[i].IndexOf("|") + 1, array[i].IndexOf("=") - array[i].IndexOf("|"));
							novelInfo_0.Isboy = novelInfo_0.Isboy.Replace("=", "").Trim();
							break;
						}
					}
				}
				if (novelInfo_0.IsboyID == 0 && !Configs.BaseConfig.DonotUserDefaultisboy)
				{
					novelInfo_0.IsboyID = Configs.BaseConfig.DefaultisboyID;
					novelInfo_0.Isboy = Configs.BaseConfig.Defaultisboy;
				}
			}
			novelInfo_0.LagerSort = Match(stringWork, ruleConfigInfo_0.LagerSort);
			SpiderException.Debug("Page.GetNovelInfo 对比大类开始");
			if (!string.IsNullOrEmpty(Configs.BaseConfig.LagerSortCorresponding))
			{
				string[] array2 = Configs.BaseConfig.LagerSortCorresponding.Replace("\r\n", "♂").Split('♂');
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j].IndexOf("," + novelInfo_0.LagerSort + ",") > 0)
					{
						novelInfo_0.LagerSortID = Convert.ToInt32(array2[j].Substring(0, array2[j].IndexOf("|")));
						novelInfo_0.LagerSort = array2[j].Substring(array2[j].IndexOf("|") + 1, array2[j].IndexOf("=") - array2[j].IndexOf("|"));
						novelInfo_0.LagerSort = novelInfo_0.LagerSort.Replace("=", "").Trim();
						novelInfo_0.MLagerSort = novelInfo_0.LagerSort;
						novelInfo_0.MLagerSortID = novelInfo_0.LagerSortID;
						break;
					}
				}
			}
			if (novelInfo_0.LagerSortID == 0 && !Configs.BaseConfig.DonotUserDefaultLagerSort)
			{
				novelInfo_0.LagerSortID = Configs.BaseConfig.DefaultLagerSortID;
				novelInfo_0.LagerSort = Configs.BaseConfig.DefaultLagerSort;
			}
			novelInfo_0.SmallSort = Match(stringWork, ruleConfigInfo_0.SmallSort);
			SpiderException.Debug("Page.GetNovelInfo 对比小类开始");
			if (!string.IsNullOrEmpty(Configs.BaseConfig.SmallSortCorresponding))
			{
				string[] array3 = Configs.BaseConfig.SmallSortCorresponding.Replace("\r\n", "♂").Split('♂');
				for (int k = 0; k < array3.Length; k++)
				{
					if (array3[k].IndexOf("," + novelInfo_0.SmallSort + ",") > 0)
					{
						novelInfo_0.SmallSortID = Convert.ToInt32(array3[k].Substring(0, array3[k].IndexOf("|")));
						novelInfo_0.SmallSort = array3[k].Substring(array3[k].IndexOf("|") + 1, array3[k].IndexOf("=") - array3[k].IndexOf("|") - 1);
						novelInfo_0.SmallSort = novelInfo_0.SmallSort.Replace("=", "").Trim();
						break;
					}
				}
			}
			if (novelInfo_0.SmallSortID == 0 && !Configs.BaseConfig.DonotUserDefaultSmallSort)
			{
				novelInfo_0.SmallSortID = Configs.BaseConfig.DefaultSmallSortID;
				novelInfo_0.SmallSort = Configs.BaseConfig.DefaultSmallSort;
			}
			novelInfo_0.Intro = FormatText.Typesetting(Match(stringWork, ruleConfigInfo_0.NovelIntro, bool_0: true));
			novelInfo_0.MIntro = novelInfo_0.Intro;
			string text2 = Match(stringWork, ruleConfigInfo_0.NovelDegree, bool_0: true);
			string relativeUri = Match(stringWork, ruleConfigInfo_0.NovelCover, bool_0: true);
			novelInfo_0.Keyword = Match(stringWork, ruleConfigInfo_0.NovelKeyword, bool_0: true);
			novelInfo_0.Degree = ((text2.IndexOf("完成") >= 0 || text2.IndexOf("完结") >= 0 || text2.IndexOf("已完结") >= 0 || text2.IndexOf("完本") >= 0 || text2.IndexOf("全本") >= 0) ? 1 : 0);
			novelInfo_0.MDegree = novelInfo_0.Degree;
			SpiderException.Debug("Page.GetNovelInfo Rule.Cover");
			Uri uri = new Uri(novelInfo_0.NovelUrl, relativeUri);
			if (uri.ToString().ToLower().StartsWith("http"))
			{
				if (ruleConfigInfo_0.NovelDefaultCoverUrl == null)
				{
					ruleConfigInfo_0.NovelDefaultCoverUrl = new RegexInfo();
				}
				if (!string.IsNullOrEmpty(ruleConfigInfo_0.NovelDefaultCoverUrl.Pattern))
				{
					if (uri.ToString().IndexOf(ruleConfigInfo_0.NovelDefaultCoverUrl.Pattern) < 0)
					{
						HttpClient httpClient3 = new HttpClient
						{
							Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
							Proxy = taskConfigInfo_0.Proxy,
							ProxyHost = taskConfigInfo_0.ProxyHost,
							ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
							ProxyDomain = taskConfigInfo_0.ProxyDomain,
							ProxyUser = taskConfigInfo_0.ProxyUser,
							ProxyPassword = taskConfigInfo_0.ProxyPassword,
							UriString = uri.AbsoluteUri,
							Referer = novelInfo_0.NovelUrl.AbsoluteUri
						};
						novelInfo_0.Cover = httpClient3.GetImageWork();
					}
				}
				else
				{
					HttpClient httpClient4 = new HttpClient
					{
						Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
						Proxy = taskConfigInfo_0.Proxy,
						ProxyHost = taskConfigInfo_0.ProxyHost,
						ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
						ProxyDomain = taskConfigInfo_0.ProxyDomain,
						ProxyUser = taskConfigInfo_0.ProxyUser,
						ProxyPassword = taskConfigInfo_0.ProxyPassword,
						UriString = uri.AbsoluteUri,
						Referer = novelInfo_0.NovelUrl.AbsoluteUri
					};
					novelInfo_0.Cover = httpClient4.GetImageWork();
				}
			}
		}
		novelInfo_0.PubKey = Match(stringWork, ruleConfigInfo_0.NovelInfo_GetNovelPubKey);
		return novelInfo_0;
	}

	public NovelInfo[] GetNovelList(string[] string_0)
	{
		List<NovelInfo> arrayList = new List<NovelInfo>();
		foreach (string text in string_0)
		{
			string text2 = text;
			Match match = RangeTemplateRegex().Match(text);
			int num = 0;
			int num2 = 0;
			if (match.Success)
			{
				text2 = text.Replace(match.Groups[0].Value, "(*)");
				num = Convert.ToInt32(match.Groups[1].Value);
				num2 = Convert.ToInt32(match.Groups[2].Value);
			}
			for (int j = num; j <= num2; j++)
			{
				Uri uri;
				try
				{
					uri = new Uri(text2.Replace("(*)", j.ToString()));
				}
				catch
				{
					SpiderException.Show(text + " 地址有问题", bool_0: true);
					continue;
				}
				HttpClient httpClient = new HttpClient
				{
					Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
					Proxy = taskConfigInfo_0.Proxy,
					ProxyHost = taskConfigInfo_0.ProxyHost,
					ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
					ProxyDomain = taskConfigInfo_0.ProxyDomain,
					ProxyUser = taskConfigInfo_0.ProxyUser,
					ProxyPassword = taskConfigInfo_0.ProxyPassword,
					UriString = uri.AbsoluteUri,
					Referer = ruleConfigInfo_0.GetSiteUrl.Pattern
				};
				string stringWork = GetStringWorkWithEmptyRetry(httpClient, subject: uri.AbsoluteUri);
				MatchCollection matchCollection = RuleRegexCache.Get(ruleConfigInfo_0.NovelList_GetNovelKey.Pattern, ruleConfigInfo_0.NovelList_GetNovelKey.Options).Matches(stringWork);
				for (int k = 0; k < matchCollection.Count; k++)
				{
					NovelInfo novelInfo = new NovelInfo
					{
						GetID = matchCollection[k].Groups[1].Value
					};
					NovelInfo novelInfo2 = novelInfo;
					if (matchCollection[k].Groups.Count > 2)
					{
						novelInfo2.Name = Translate(matchCollection[k].Groups[2].Value);
					}
					arrayList.Add(novelInfo2);
				}
			}
		}
		return arrayList.ToArray();
	}

	public static string[] GetNovelList(string string_0, string string_1, string string_2)
	{
		List<string> arrayList = new List<string>();
		HttpClient httpClient = new HttpClient
		{
			Encoding = Encoding.GetEncoding(string_2),
			UriString = string_0
		};
		string stringWork = GetStringWorkWithEmptyRetry(httpClient, subject: string_0);
		MatchCollection matchCollection = RuleRegexCache.Get(string_1).Matches(stringWork);
		for (int i = 0; i < matchCollection.Count; i++)
		{
			arrayList.Add(matchCollection[i].Groups[1].Value);
		}
		return arrayList.ToArray();
	}

	public static string GetTimeStamp()
	{
		return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString();
	}

	public string Match(string string_0, RegexInfo regexInfo_0)
	{
		return Match(string_0, regexInfo_0, bool_0: false);
	}

	public string Match(string string_0, RegexInfo regexInfo_0, bool bool_0)
	{
		SpiderException.Debug("Page.Match Rule." + regexInfo_0.RegexName);
		string text = "";
		if (string.IsNullOrEmpty(regexInfo_0.Pattern))
		{
			return text;
		}
		Match match = RuleRegexCache.Get(regexInfo_0.Pattern, regexInfo_0.Options).Match(string_0);
		if (match.Success)
		{
			int num = 1;
			text = match.Groups[num].Value.Trim();
			num++;
			int length = text.Length;
		}
		if (!string.IsNullOrEmpty(regexInfo_0.FilterPattern))
		{
			string[] array = regexInfo_0.FilterPattern.Split('\n');
			foreach (string text2 in array)
			{
				string[] array2 = text2.Trim().Split('♂');
				string replacement = "";
				if (array2.Length > 1)
				{
					replacement = array2[1].Trim();
				}
				text = RuleRegexCache.Get(array2[0]).Replace(text, replacement);
			}
		}
		if (string.IsNullOrEmpty(text) && !bool_0)
		{
			throw new Exception("规则问题 " + regexInfo_0.RegexName + " 无法获得内容");
		}
		return Translate(text);
	}

	public string Replace(string string_0, RegexInfo regexInfo_0)
	{
		string[] array = regexInfo_0.Pattern.Split('\n');
		foreach (string text in array)
		{
			char[] separator = new char[1] { '♂' };
			string[] array2 = text.Split(separator);
			string replacement = "";
			if (array2.Length > 1)
			{
				replacement = array2[1];
			}
			string_0 = RuleRegexCache.Get(array2[0], regexInfo_0.Options).Replace(string_0, replacement);
		}
		return string_0;
	}

	public string RnReplace(string string_0, string string_1, RegexInfo regexInfo_0)
	{
		string[] array = string_1.Split('\n');
		foreach (string text in array)
		{
			char[] separator = new char[1] { '♂' };
			string[] array2 = text.Split(separator);
			string replacement = "";
			if (array2.Length > 1)
			{
				replacement = array2[1];
			}
			string_0 = RuleRegexCache.Get(array2[0], regexInfo_0.Options).Replace(string_0, replacement);
		}
		return string_0;
	}

	public NovelInfo SearchNovel(NovelInfo novelInfo_0)
	{
		SpiderException.Debug("Page.SearchNovel");
		string newValue = ((Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern) != Encoding.UTF8) ? HttpUtility.UrlEncode(novelInfo_0.Name, Encoding.Default) : HttpUtility.UrlEncode(novelInfo_0.Name, Encoding.UTF8));
		HttpClient httpClient = new HttpClient
		{
			Encoding = Encoding.GetEncoding(ruleConfigInfo_0.GetSiteCharset.Pattern),
			Proxy = taskConfigInfo_0.Proxy,
			ProxyHost = taskConfigInfo_0.ProxyHost,
			ProxyPort = Convert.ToInt32(taskConfigInfo_0.ProxyPort),
			ProxyDomain = taskConfigInfo_0.ProxyDomain,
			ProxyUser = taskConfigInfo_0.ProxyUser,
			ProxyPassword = taskConfigInfo_0.ProxyPassword,
			UriString = ruleConfigInfo_0.NovelSearchUrl.Pattern.Replace("{SearchKey}", newValue),
			PostData = ruleConfigInfo_0.NovelSearchData.Pattern.Replace("{SearchKey}", newValue),
			Referer = ruleConfigInfo_0.GetSiteUrl.Pattern,
			AllowAutoRedirect = true
		};
		string stringWork = GetStringWorkWithEmptyRetry(httpClient, subject: ruleConfigInfo_0.NovelSearchUrl.Pattern);
		RegexInfo regexInfo_ = new RegexInfo
		{
			RegexName = ruleConfigInfo_0.NovelSearch_GetNovelKey.RegexName,
			Pattern = ruleConfigInfo_0.NovelSearch_GetNovelKey.Pattern.Replace("{SearchKey}", novelInfo_0.Name),
			FilterPattern = ruleConfigInfo_0.NovelSearch_GetNovelKey.FilterPattern,
			Options = ruleConfigInfo_0.NovelSearch_GetNovelKey.Options,
			Method = ruleConfigInfo_0.NovelSearch_GetNovelKey.Method
		};
		novelInfo_0.GetID = Match(stringWork, regexInfo_);
		return novelInfo_0;
	}

	private static void smethod_0()
	{
		if (!string.IsNullOrEmpty(Configs.BaseConfig.CenterText))
		{
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				string xmlString = "<RSAKeyValue><Modulus>3ymokm6unQHqX7L0t6gE+a94cx9mmgW0CC3XnSArSrrV5ZHVHrSNt2jwW2896BqCETsX25lXZNd8oatYeuNk1ZOahSc7p4pofNnT4Eq/56HhV3n4X1Bv5SOWlUW9WBqhsYBeq7rZ9MTO0kOS0GCvX5uTiNcfr/hnN9Z4uyCPtxE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
				rSACryptoServiceProvider.FromXmlString(xmlString);
				new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider).SetHashAlgorithm("SHA1");
				byte_0 = 2;
			}
		}
	}

	[CompilerGenerated]
	private static int smethod_2(ChapterInfo chapterInfo_0, ChapterInfo chapterInfo_1)
	{
		return Comparer<int>.Default.Compare(Convert.ToInt32(chapterInfo_0.GetID), Convert.ToInt32(chapterInfo_1.GetID));
	}

	[CompilerGenerated]
	private static int smethod_3(ChapterInfo chapterInfo_0, ChapterInfo chapterInfo_1)
	{
		return Comparer<int>.Default.Compare(Convert.ToInt32(chapterInfo_1.GetID), Convert.ToInt32(chapterInfo_0.GetID));
	}

	[CompilerGenerated]
	private static int smethod_4(ChapterInfo chapterInfo_0, ChapterInfo chapterInfo_1)
	{
		return Comparer<int>.Default.Compare(Convert.ToInt32(chapterInfo_0.ChapterName), Convert.ToInt32(chapterInfo_1.ChapterName));
	}

	[CompilerGenerated]
	private static int smethod_5(ChapterInfo chapterInfo_0, ChapterInfo chapterInfo_1)
	{
		return Comparer<int>.Default.Compare(Convert.ToInt32(chapterInfo_1.ChapterName), Convert.ToInt32(chapterInfo_0.ChapterName));
	}

	public string Translate(string string_0)
	{
		if (Configs.HaveFunction.IndexOf("中译英") >= 0 && Configs.BaseConfig.Translate && ChineseCharactersRegex().IsMatch(string_0))
		{
			if (string_0.Length > 20)
			{
				HttpClient httpClient = new HttpClient
				{
					UriString = "http://translate.google.cn/",
					PostData = "js=y&prev=_t&hl=zh-CN&ie=UTF-8&layout=1&eotf=1&text=" + HttpUtility.UrlEncode(FormatText.Typesetting(string_0), FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")).ToUpper() + "&file=&sl=zh-CN&tl=en",
					Encoding = FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")
				};
				Match match = GoogleTranslateLongResultRegex().Match(httpClient.GetStringWork());
				if (match.Success)
				{
					return match.Groups[1].Value.Replace("'", "&apos;").Replace("&nbsp;", " ").Replace("&lt;br&gt;", "<br>")
						.Trim();
				}
				return string_0;
			}
			HttpClient httpClient2 = new HttpClient
			{
				UriString = "http://translate.google.cn/translate_a/t?client=t&text=" + HttpUtility.UrlEncode(string_0, FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")).ToUpper() + "&sl=zh-CN&tl=en&otf=1&pc=0",
				Encoding = FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")
			};
			Match match2 = GoogleTranslateShortResultRegex().Match(httpClient2.GetStringWork());
			if (match2.Success)
			{
				return match2.Groups[1].Value.Replace("'", "&apos;").Trim();
			}
		}
		return string_0;
	}

	[GeneratedRegex(@"\s+")]
	private static partial Regex WhitespaceRegex();

	[GeneratedRegex(@"\{(\d*)-(\d*)\}")]
	private static partial Regex RangeTemplateRegex();

	[GeneratedRegex(@"[\u4e00-\u9fa5]+")]
	private static partial Regex ChineseCharactersRegex();

	[GeneratedRegex(@"<textarea name=utrans wrap=SOFT.+?>([^><]*)</textarea>")]
	private static partial Regex GoogleTranslateLongResultRegex();

	[GeneratedRegex("trans\":\"(.+?)\",\"orig")]
	private static partial Regex GoogleTranslateShortResultRegex();
}
