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
			string novelHtml = await FetchTextAsync(string.IsNullOrWhiteSpace(novelUrl) ? siteUrl : novelUrl);
			string indexHtml = string.IsNullOrWhiteSpace(indexUrl) ? novelHtml : await FetchTextAsync(indexUrl);
			string chapterHtml = string.IsNullOrWhiteSpace(chapterUrl) ? "" : await FetchTextAsync(chapterUrl);
			generatedRule = BuildLocalRule(siteUrl, novelUrl, indexUrl, chapterUrl, novelHtml, indexHtml, chapterHtml);
			if (!string.IsNullOrWhiteSpace(aiKeyBox.Text) && !string.IsNullOrWhiteSpace(aiModelBox.Text))
			{
				previewBox.Text = await BuildAiSuggestionAsync(generatedRule, novelHtml, indexHtml, chapterHtml);
			}
			else
			{
				previewBox.Text = SerializeRule(generatedRule) + Environment.NewLine + Environment.NewLine + "正文质量评分：" + CollectionQualityAnalyzer.ScoreChapterText(chapterHtml);
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

	private static RuleConfigInfo BuildLocalRule(string siteUrl, string novelUrl, string indexUrl, string chapterUrl, string novelHtml, string indexHtml, string chapterHtml)
	{
		Uri baseUri = CreateUri(siteUrl) ?? CreateUri(novelUrl) ?? CreateUri(indexUrl) ?? CreateUri(chapterUrl);
		string host = baseUri?.Host ?? "auto.local";
		RuleConfigInfo rule = new RuleConfigInfo();
		rule.RuleVersion = RegexInfo("RuleVersion", "10.3-auto");
		rule.RuleID = RegexInfo("RuleID", DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + host);
		rule.GetSiteName = RegexInfo("GetSiteName", host);
		rule.GetSiteCharset = RegexInfo("GetSiteCharset", "utf-8");
		rule.GetSiteUrl = RegexInfo("GetSiteUrl", baseUri?.GetLeftPart(UriPartial.Authority) ?? siteUrl);
		rule.NovelUrl = RegexInfo("NovelUrl", string.IsNullOrWhiteSpace(novelUrl) ? siteUrl : novelUrl);
		rule.PubIndexUrl = RegexInfo("PubIndexUrl", string.IsNullOrWhiteSpace(indexUrl) ? novelUrl : indexUrl);
		rule.PubContentUrl = RegexInfo("PubContentUrl", string.IsNullOrWhiteSpace(chapterUrl) ? "{ChapterKey}" : chapterUrl);
		rule.NovelName = RegexInfo("NovelName", BuildTitlePattern(novelHtml));
		rule.NovelAuthor = RegexInfo("NovelAuthor", "作者[：:]\\s*(?<str>[^<\\s]+)");
		rule.NovelIntro = RegexInfo("NovelIntro", "(?is)<meta[^>]+name=[\"']description[\"'][^>]+content=[\"'](?<str>.*?)[\"']");
		rule.PubChapterName = RegexInfo("PubChapterName", "(?is)<a[^>]+href=[\"'][^\"']+[\"'][^>]*>(?<str>[^<]{2,80})</a>");
		rule.PubChapter_GetChapterKey = RegexInfo("PubChapter_GetChapterKey", "(?is)<a[^>]+href=[\"'](?<str>[^\"']+)[\"'][^>]*>[^<]{2,80}</a>");
		rule.PubContentText = RegexInfo("PubContentText", BuildContentPattern(chapterHtml));
		return rule;
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
		if (Regex.IsMatch(html ?? string.Empty, "id=[\"']content[\"']", RegexOptions.IgnoreCase))
		{
			return "(?is)<div[^>]+id=[\"']content[\"'][^>]*>(?<str>.*?)</div>";
		}
		if (Regex.IsMatch(html ?? string.Empty, "class=[\"'][^\"']*(content|chapter)[^\"']*[\"']", RegexOptions.IgnoreCase))
		{
			return "(?is)<div[^>]+class=[\"'][^\"']*(content|chapter)[^\"']*[\"'][^>]*>(?<str>.*?)</div>";
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

