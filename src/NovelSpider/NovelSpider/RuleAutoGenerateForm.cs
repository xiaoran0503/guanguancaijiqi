using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class RuleAutoGenerateForm : DockContent
{
	private readonly TextBox siteUrlBox = new TextBox();
	private readonly TextBox novelUrlBox = new TextBox();
	private readonly TextBox indexUrlBox = new TextBox();
	private readonly TextBox chapterUrlBox = new TextBox();
	private readonly TextBox hintBox = new TextBox();
	private readonly TextBox aiBaseUrlBox = new TextBox();
	private readonly TextBox aiKeyBox = new TextBox();
	private readonly TextBox aiModelBox = new TextBox();
	private readonly TextBox previewBox = new TextBox();
	private readonly TextBox saveNameBox = new TextBox();
	private readonly Button generateButton = new Button();
	private readonly Button saveButton = new Button();
	private RuleConfigInfo generatedRule;

	public RuleAutoGenerateForm()
	{
		Text = "自动生成采集规则";
		Width = 960;
		Height = 680;
		InitializeControls();
	}

	private void InitializeControls()
	{
		TableLayoutPanel root = new TableLayoutPanel
		{
			Dock = DockStyle.Fill,
			ColumnCount = 2,
			RowCount = 12,
			Padding = new Padding(8),
			AutoScroll = true
		};
		root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
		root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
		Controls.Add(root);

		AddRow(root, 0, "站点首页", siteUrlBox);
		AddRow(root, 1, "小说页", novelUrlBox);
		AddRow(root, 2, "目录页", indexUrlBox);
		AddRow(root, 3, "章节页", chapterUrlBox);
		AddRow(root, 4, "提示信息", hintBox);
		AddRow(root, 5, "AI BaseUrl", aiBaseUrlBox);
		AddRow(root, 6, "AI Key", aiKeyBox);
		AddRow(root, 7, "AI Model", aiModelBox);
		AddRow(root, 8, "保存文件名", saveNameBox);

		hintBox.Multiline = true;
		hintBox.Height = 48;
		aiKeyBox.UseSystemPasswordChar = true;
		aiBaseUrlBox.Text = "https://api.openai.com/v1/chat/completions";
		aiModelBox.Text = "";
		saveNameBox.Text = "auto-generated-rule.xml";

		FlowLayoutPanel buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
		generateButton.Text = "生成草稿";
		generateButton.Width = 100;
		generateButton.Click += async (_, _) => await GenerateAsync();
		saveButton.Text = "保存到 Rules";
		saveButton.Width = 120;
		saveButton.Enabled = false;
		saveButton.Click += (_, _) => SaveRule();
		buttons.Controls.Add(generateButton);
		buttons.Controls.Add(saveButton);
		root.Controls.Add(new Label { Text = "操作", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft }, 0, 9);
		root.Controls.Add(buttons, 1, 9);

		previewBox.Multiline = true;
		previewBox.ScrollBars = ScrollBars.Both;
		previewBox.WordWrap = false;
		previewBox.Dock = DockStyle.Fill;
		root.Controls.Add(new Label { Text = "规则预览", Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft }, 0, 10);
		root.Controls.Add(previewBox, 1, 10);
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
		root.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
		root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
	}

	private static void AddRow(TableLayoutPanel panel, int row, string label, TextBox textBox)
	{
		textBox.Dock = DockStyle.Fill;
		panel.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft }, 0, row);
		panel.Controls.Add(textBox, 1, row);
	}

	private async Task GenerateAsync()
	{
		generateButton.Enabled = false;
		try
		{
			string siteUrl = NormalizeUrl(siteUrlBox.Text);
			string novelUrl = NormalizeUrl(novelUrlBox.Text);
			string indexUrl = NormalizeUrl(string.IsNullOrWhiteSpace(indexUrlBox.Text) ? novelUrlBox.Text : indexUrlBox.Text);
			string chapterUrl = NormalizeUrl(chapterUrlBox.Text);
			if (string.IsNullOrWhiteSpace(siteUrl) && string.IsNullOrWhiteSpace(novelUrl) && string.IsNullOrWhiteSpace(chapterUrl))
			{
				MessageBox.Show("请至少填写一个可访问的网址。", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			string siteHtml = string.IsNullOrWhiteSpace(siteUrl) ? string.Empty : await FetchTextAsync(siteUrl);
			string novelHtml = await FetchTextAsync(string.IsNullOrWhiteSpace(novelUrl) ? siteUrl : novelUrl);
			string indexHtml = string.IsNullOrWhiteSpace(indexUrl) ? novelHtml : await FetchTextAsync(indexUrl);
			string chapterHtml = string.IsNullOrWhiteSpace(chapterUrl) ? "" : await FetchTextAsync(chapterUrl);
			generatedRule = BuildLocalRule(siteUrl, novelUrl, indexUrl, chapterUrl, siteHtml, novelHtml, indexHtml, chapterHtml);
			if (!string.IsNullOrWhiteSpace(aiKeyBox.Text) && !string.IsNullOrWhiteSpace(aiModelBox.Text))
			{
				previewBox.Text = await BuildAiSuggestionAsync(generatedRule, novelHtml, indexHtml, chapterHtml);
			}
			else
			{
				previewBox.Text = SerializeRule(generatedRule) + Environment.NewLine + Environment.NewLine + BuildPreviewNotes(generatedRule, chapterHtml);
			}
			saveButton.Enabled = generatedRule != null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		finally
		{
			generateButton.Enabled = true;
		}
	}

	private static RuleConfigInfo BuildLocalRule(string siteUrl, string novelUrl, string indexUrl, string chapterUrl, string siteHtml, string novelHtml, string indexHtml, string chapterHtml)
	{
		Uri baseUri = CreateUri(siteUrl) ?? CreateUri(novelUrl) ?? CreateUri(indexUrl) ?? CreateUri(chapterUrl);
		Uri indexUri = CreateUri(indexUrl) ?? CreateUri(novelUrl) ?? baseUri;
		string host = baseUri?.Host ?? "auto.local";
		ChapterLinkPatterns chapterLinks = BuildChapterLinkPatterns(indexHtml, chapterUrl, indexUri);
		RuleConfigInfo rule = new RuleConfigInfo();
		rule.RuleVersion = RegexInfo("RuleVersion", "10.3-auto");
		rule.RuleID = RegexInfo("RuleID", DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + host);
		rule.GetSiteName = RegexInfo("GetSiteName", host);
		rule.GetSiteCharset = RegexInfo("GetSiteCharset", "utf-8");
		rule.GetSiteUrl = RegexInfo("GetSiteUrl", baseUri?.GetLeftPart(UriPartial.Authority) ?? siteUrl);
		string novelUrlPattern = BuildNovelUrlPattern(novelUrl, siteUrl);
		rule.NovelListUrl = RegexInfo("NovelListUrl", siteUrl);
		rule.NovelList_GetNovelKey = RegexInfo("NovelList_GetNovelKey", BuildNovelListKeyPattern(siteHtml, novelUrl, baseUri));
		rule.NovelUrl = RegexInfo("NovelUrl", novelUrlPattern);
		rule.PubIndexUrl = RegexInfo("PubIndexUrl", string.IsNullOrWhiteSpace(indexUrl) || string.Equals(indexUrl, novelUrl, StringComparison.OrdinalIgnoreCase) ? novelUrlPattern : BuildNovelUrlPattern(indexUrl, siteUrl));
		rule.NovelErr = RegexInfo("NovelErr", "(?!)");
		rule.PubIndexErr = RegexInfo("PubIndexErr", "(?!)");
		rule.PubContentErr = RegexInfo("PubContentErr", "(?!)");
		rule.PubContentUrl = RegexInfo("PubContentUrl", chapterLinks.ContentUrlPattern);
		rule.NovelName = RegexInfo("NovelName", BuildTitlePattern(novelHtml));
		rule.NovelAuthor = RegexInfo("NovelAuthor", "作者[：:]\\s*(?<str>[^<\\s]+)");
		rule.NovelIntro = RegexInfo("NovelIntro", "(?is)<meta[^>]+name=[\"']description[\"'][^>]+content=[\"'](?<str>.*?)[\"']");
		rule.PubVolumeContent = RegexInfo("PubVolumeContent", BuildDirectoryContentPattern(indexHtml));
		rule.PubVolumeSplit = RegexInfo("PubVolumeSplit", "(?!)");
		rule.PubVolumeName = RegexInfo("PubVolumeName", string.Empty);
		rule.PubChapterName = RegexInfo("PubChapterName", chapterLinks.ChapterNamePattern);
		rule.PubChapter_GetChapterKey = RegexInfo("PubChapter_GetChapterKey", chapterLinks.ChapterKeyPattern);
		rule.PubContentText = RegexInfo("PubContentText", BuildContentPattern(chapterHtml));
		return rule;
	}

	private static string BuildNovelUrlPattern(string novelUrl, string siteUrl)
	{
		Uri novelUri = CreateUri(novelUrl);
		if (novelUri == null)
		{
			return string.IsNullOrWhiteSpace(novelUrl) ? siteUrl : novelUrl;
		}
		string path = novelUri.AbsolutePath;
		Match lastNumber = Regex.Match(path, @"(?<prefix>.*/)(?<id>\d+)/?$", RegexOptions.IgnoreCase);
		if (!lastNumber.Success)
		{
			return novelUri.AbsoluteUri;
		}
		string prefix = lastNumber.Groups["prefix"].Value;
		string id = lastNumber.Groups["id"].Value;
		string pathPattern = prefix + "{NovelKey}/";
		Match bucket = Regex.Match(prefix, @"^(?<before>.*/)(?<bucket>\d+)/$", RegexOptions.IgnoreCase);
		if (bucket.Success && int.TryParse(id, out int novelId) && int.TryParse(bucket.Groups["bucket"].Value, out int bucketId) && bucketId == novelId / 1000)
		{
			pathPattern = bucket.Groups["before"].Value + "{NovelKey/1000}/{NovelKey}/";
		}
		return novelUri.GetLeftPart(UriPartial.Authority) + pathPattern;
	}

	private static string BuildNovelListKeyPattern(string siteHtml, string novelUrl, Uri baseUri)
	{
		Uri novelUri = CreateUri(novelUrl);
		if (novelUri == null)
		{
			return string.Empty;
		}
		string href = ToPageHref(novelUri, baseUri);
		Match pathPattern = Regex.Match(href, @"^(?<prefix>.*/)(?<id>\d+)/?$", RegexOptions.IgnoreCase);
		if (!pathPattern.Success)
		{
			return string.Empty;
		}
		string prefix = pathPattern.Groups["prefix"].Value;
		string hrefPattern = Regex.Escape(prefix) + @"(?<str>\d+)/";
		int count = Regex.Matches(siteHtml ?? string.Empty, "href=[\"']" + hrefPattern + "[\"']", RegexOptions.IgnoreCase).Count;
		if (count == 0 && Uri.TryCreate(href, UriKind.Absolute, out Uri absoluteHref))
		{
			string absolutePrefix = absoluteHref.GetLeftPart(UriPartial.Authority) + prefix;
			hrefPattern = Regex.Escape(absolutePrefix) + @"(?<str>\d+)/";
		}
				string textLinkPattern = "(?is)<a[^>]+href=[\"']" + hrefPattern + "[\"'][^>]*>(?<name>[^<]{1,120})</a>";
		MatchCollection textLinks = Regex.Matches(siteHtml ?? string.Empty, textLinkPattern, RegexOptions.IgnoreCase);
		int usefulTextLinks = 0;
		foreach (Match link in textLinks)
		{
			string name = link.Groups["name"].Value.Trim();
			if (!string.IsNullOrWhiteSpace(name) && !Regex.IsMatch(name, "阅读本书|立即阅读|加入书架|更多|目录"))
			{
				usefulTextLinks++;
			}
		}
		return usefulTextLinks > 0 ? textLinkPattern : "(?is)<a[^>]+href=[\"']" + hrefPattern + "[\"'][^>]*>.*?</a>";
	}
	private sealed class ChapterLinkPatterns
	{
		public string ChapterNamePattern { get; init; } = "(?is)<a[^>]+href=[\"'][^\"']+[\"'][^>]*>(?<str>[^<]{2,120})</a>";

		public string ChapterKeyPattern { get; init; } = "(?is)<a[^>]+href=[\"'](?<str>[^\"']+)[\"'][^>]*>[^<]{2,120}</a>";

		public string ContentUrlPattern { get; init; } = "{ChapterKey}";
	}

	private static ChapterLinkPatterns BuildChapterLinkPatterns(string indexHtml, string chapterUrl, Uri indexUri)
	{
		string hrefPattern = BuildChapterHrefPattern(indexHtml, chapterUrl, indexUri);
		return new ChapterLinkPatterns
		{
			ChapterNamePattern = "(?is)<(?:li|dd)?[^>]*>\\s*<a[^>]+href=[\"']" + hrefPattern + "[\"'][^>]*>(?<str>[^<]{1,120})</a>",
			ChapterKeyPattern = "(?is)<(?:li|dd)?[^>]*>\\s*<a[^>]+href=[\"'](?<str>" + hrefPattern + ")[\"'][^>]*>[^<]{1,120}</a>",
			ContentUrlPattern = "{ChapterKey}"
		};
	}

	private static string BuildChapterHrefPattern(string indexHtml, string chapterUrl, Uri indexUri)
	{
		string sampleHref = FindBestChapterHref(indexHtml, chapterUrl, indexUri);
		if (string.IsNullOrWhiteSpace(sampleHref))
		{
			return "[^\"']+";
		}
		string escaped = Regex.Escape(sampleHref);
		return Regex.Replace(escaped, "\\d+", "\\d+");
	}

	private static string FindBestChapterHref(string indexHtml, string chapterUrl, Uri indexUri)
	{
		Uri chapterUri = CreateUri(chapterUrl);
		if (chapterUri != null)
		{
			return ToPageHref(chapterUri, indexUri);
		}
		MatchCollection links = Regex.Matches(indexHtml ?? string.Empty, "(?is)<a[^>]+href=[\"'](?<href>[^\"']+)[\"'][^>]*>(?<text>.*?)</a>");
		string bestHref = string.Empty;
		int bestScore = int.MinValue;
		foreach (Match link in links)
		{
			string href = link.Groups["href"].Value.Trim();
			string linkText = Regex.Replace(link.Groups["text"].Value, "<.*?>", string.Empty).Trim();
			int score = ScoreChapterLink(href, linkText);
			if (score > bestScore)
			{
				bestHref = href;
				bestScore = score;
			}
		}
		return bestScore > 0 ? bestHref : string.Empty;
	}

	private static int ScoreChapterLink(string href, string linkText)
	{
		if (string.IsNullOrWhiteSpace(href) || href.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase))
		{
			return -100;
		}
		int score = 0;
		if (Regex.IsMatch(href, @"/read/|/book/|/xs/|/chapter", RegexOptions.IgnoreCase))
		{
			score += 6;
		}
		if (Regex.IsMatch(href, @"\d+.*\.(html|htm)$", RegexOptions.IgnoreCase))
		{
			score += 5;
		}
		if (Regex.IsMatch(linkText, @"第.+[章节回卷]|序章|楔子|正文", RegexOptions.IgnoreCase))
		{
			score += 8;
		}
		if (linkText.Length is >= 2 and <= 80)
		{
			score += 2;
		}
		if (Regex.IsMatch(linkText, "最新|目录|首页|上一页|下一页|返回|书架|作者|排行|分类"))
		{
			score -= 8;
		}
		return score;
	}

	private static string ToPageHref(Uri chapterUri, Uri indexUri)
	{
		if (indexUri != null && string.Equals(chapterUri.Host, indexUri.Host, StringComparison.OrdinalIgnoreCase))
		{
			return chapterUri.PathAndQuery;
		}
		return chapterUri.AbsoluteUri;
	}

	private static string BuildDirectoryContentPattern(string html)
	{
		if (Regex.IsMatch(html ?? string.Empty, "全部章节", RegexOptions.IgnoreCase))
		{
			return "(?is)<h3>\\s*<a>\\s*全部章节\\s*</a>\\s*</h3>\\s*<ul[^>]*>(?<str>.*?)</ul>";
		}
		if (Regex.IsMatch(html ?? string.Empty, "class=[\"'][^\"']*ml_content[^\"']*[\"']", RegexOptions.IgnoreCase))
		{
			return "(?is)<div[^>]+class=[\"'][^\"']*ml_content[^\"']*[\"'][^>]*>(?<str>.*?)<div[^>]+class=[\"']clear[\"'][^>]*>";
		}
		return string.Empty;
	}
	private async Task<string> BuildAiSuggestionAsync(RuleConfigInfo localRule, string novelHtml, string indexHtml, string chapterHtml)
	{
		using System.Net.Http.HttpClient client = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(60) };
		using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, aiBaseUrlBox.Text.Trim());
		request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", aiKeyBox.Text.Trim());
		var payload = new
		{
			model = aiModelBox.Text.Trim(),
			messages = new[]
			{
				new { role = "system", content = "你是采集规则助手。请基于页面摘要生成 NovelSpider RuleConfigInfo XML 建议，不要输出 API Key、Cookie、数据库连接或账号密码。" },
				new { role = "user", content = BuildAiPrompt(localRule, novelHtml, indexHtml, chapterHtml) }
			}
		};
		request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
		using HttpResponseMessage response = await client.SendAsync(request);
		string body = await response.Content.ReadAsStringAsync();
		if (!response.IsSuccessStatusCode)
		{
			return SerializeRule(localRule) + Environment.NewLine + Environment.NewLine + "AI 调用失败：" + response.StatusCode + Environment.NewLine + body;
		}
		return SerializeRule(localRule) + Environment.NewLine + Environment.NewLine + "AI 建议：" + Environment.NewLine + body;
	}

	private static string BuildAiPrompt(RuleConfigInfo localRule, string novelHtml, string indexHtml, string chapterHtml)
	{
		return "本地草稿：" + Environment.NewLine + SerializeRule(localRule) + Environment.NewLine +
			"小说页摘要：" + Environment.NewLine + SummarizeHtml(novelHtml) + Environment.NewLine +
			"目录页摘要：" + Environment.NewLine + SummarizeHtml(indexHtml) + Environment.NewLine +
			"章节页摘要：" + Environment.NewLine + SummarizeHtml(chapterHtml);
	}

	private static string BuildPreviewNotes(RuleConfigInfo rule, string chapterHtml)
	{
		StringBuilder notes = new StringBuilder();
		notes.AppendLine("正文质量评分：" + CollectionQualityAnalyzer.ScoreChapterText(chapterHtml));
		if (string.IsNullOrWhiteSpace(rule.NovelListUrl.Pattern))
		{
			notes.AppendLine("提示：当前只根据小说页/目录页/章节页生成单本采集规则；未提供站点最新列表页时，NovelListUrl 不会自动生成，规则测试请优先测试小说信息、目录和章节正文。");
		}
		return notes.ToString();
	}
	private void SaveRule()
	{
		if (generatedRule == null)
		{
			return;
		}
		string fileName = string.IsNullOrWhiteSpace(saveNameBox.Text) ? "auto-generated-rule.xml" : saveNameBox.Text.Trim();
		foreach (char invalid in Path.GetInvalidFileNameChars())
		{
			fileName = fileName.Replace(invalid, '_');
		}
		if (!fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
		{
			fileName += ".xml";
		}
		string rulesDir = Path.Combine(AppContext.BaseDirectory, "Rules");
		Directory.CreateDirectory(rulesDir);
		string path = Path.Combine(rulesDir, fileName);
		ConfigFileManager.SaveConfig(path, generatedRule);
		MessageBox.Show("已保存：" + path, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
	}

	private static RegexInfo RegexInfo(string name, string pattern)
	{
		return new RegexInfo { RegexName = name, Pattern = pattern ?? string.Empty, Method = "Match" };
	}

	private static string BuildTitlePattern(string html)
	{
		return Regex.IsMatch(html ?? string.Empty, "<h1", RegexOptions.IgnoreCase)
			? "(?is)<h1[^>]*>(?<str>.*?)</h1>"
			: "(?is)<title[^>]*>(?<str>.*?)</title>";
	}

	private static string BuildContentPattern(string html)
	{
		if (Regex.IsMatch(html ?? string.Empty, "id=[\"']articlecontent[\"']", RegexOptions.IgnoreCase))
		{
			return "(?is)<div[^>]+id=[\"']articlecontent[\"'][^>]*>(?<str>.*?)</div>";
		}
		if (Regex.IsMatch(html ?? string.Empty, "id=[\"']content[\"']", RegexOptions.IgnoreCase))
		{
			return "(?is)<div[^>]+id=[\"']content[\"'][^>]*>(?<str>.*?)</div>";
		}
		if (Regex.IsMatch(html ?? string.Empty, "class=[\"'][^\"']*(articlecontent|novelcontent|content|chapter)[^\"']*[\"']", RegexOptions.IgnoreCase))
		{
			return "(?is)<div[^>]+class=[\"'][^\"']*(articlecontent|novelcontent|content|chapter)[^\"']*[\"'][^>]*>(?<str>.*?)</div>";
		}
		return "(?is)<body[^>]*>(?<str>.*?)</body>";
	}

	private static async Task<string> FetchTextAsync(string url)
	{
		if (string.IsNullOrWhiteSpace(url))
		{
			return string.Empty;
		}
		using System.Net.Http.HttpClient client = new System.Net.Http.HttpClient { Timeout = TimeSpan.FromSeconds(30) };
		client.DefaultRequestHeaders.UserAgent.ParseAdd("NovelSpider-Net10-RuleGenerator/10.3");
		return await client.GetStringAsync(url);
	}

	private static string NormalizeUrl(string value)
	{
		return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
	}

	private static Uri CreateUri(string value)
	{
		return Uri.TryCreate(value, UriKind.Absolute, out Uri uri) ? uri : null;
	}

	private static string SerializeRule(RuleConfigInfo rule)
	{
		using StringWriter writer = new StringWriter();
		new System.Xml.Serialization.XmlSerializer(typeof(RuleConfigInfo)).Serialize(writer, rule);
		return writer.ToString();
	}

	private static string SummarizeHtml(string html)
	{
		if (string.IsNullOrEmpty(html))
		{
			return string.Empty;
		}
		string text = Regex.Replace(html, "(?is)<script.*?</script>|<style.*?</style>", " ");
		text = Regex.Replace(text, "\\s+", " ").Trim();
		return text.Length <= 6000 ? text : text.Substring(0, 6000);
	}
}

