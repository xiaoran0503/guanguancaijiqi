using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;
using NovelSpider.Target;

namespace NovelSpider;

public class RuleTestForm : Form
{
	private CancellationTokenSource cancellationTokenSource;

	private bool isRunning;

	private IContainer icontainer_0;

	private RuleConfigInfo ruleConfigInfo_0;

	private string string_0;

	private string string_1;

	private TaskConfigInfo taskConfigInfo_0;

	private RichTextBox TestResult;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ChapterID
	{
		get
		{
			return string_1;
		}
		set
		{
			string_1 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string NovelID
	{
		get
		{
			return string_0;
		}
		set
		{
			string_0 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public RuleConfigInfo Rule
	{
		get
		{
			return ruleConfigInfo_0;
		}
		set
		{
			ruleConfigInfo_0 = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public TaskConfigInfo Task
	{
		get
		{
			return taskConfigInfo_0;
		}
		set
		{
			taskConfigInfo_0 = value;
		}
	}

	public RuleTestForm()
	{
		InitializeComponent();
	}

	private async Task RunRuleTestAsync(Action<string> report, CancellationToken cancellationToken)
	{
		taskConfigInfo_0.Proxy = false;
		Page page = new Page(ruleConfigInfo_0, taskConfigInfo_0);
		bool hasSingleNovelId = string_0 != "0" && string_0 != "";
		NovelInfo[] novelList = Array.Empty<NovelInfo>();
		if (!hasSingleNovelId || !string.IsNullOrWhiteSpace(ruleConfigInfo_0.NovelListUrl.Pattern))
		{
			report("====== 开始测试获得最新列表 ======");
			string[] array = ruleConfigInfo_0.NovelListUrl.Pattern.Replace("\r\n", "♂").Split('♂');
			novelList = page.GetNovelList(array);
			if (novelList.Length == 0 && !hasSingleNovelId)
			{
				throw new ApplicationException("没有获得小说列表");
			}
			string text = "";
			for (int i = 0; i < novelList.Length; i++)
			{
				text = text + novelList[i].GetID + "\t" + novelList[i].Name + "\n";
			}
			report(text);
			report("");
		}
		else
		{
			report("====== 已填写单本小说ID，跳过最新列表测试 ======");
		}
		report("====== 开始测试小说信息页 ======");
		Random random = new Random();
		NovelInfo novelInfo_;
		if (hasSingleNovelId)
		{
			NovelInfo novelInfo = new NovelInfo
			{
				GetID = string_0
			};
			novelInfo_ = novelInfo;
		}
		else
		{
			novelInfo_ = novelList[random.Next(novelList.Length)];
		}
		cancellationToken.ThrowIfCancellationRequested();
		NovelInfo novelInfo2 = page.GetNovelInfo(novelInfo_);
		string text2 = "获取失败";
		if (novelInfo2.Cover != null)
		{
			text2 = "获取成功";
		}
		report("RuleID(站点ID):\t" + novelInfo2.RuleID + "\nNovelUrl(信息页URL):\t" + novelInfo2.NovelUrl.AbsolutePath + "\nNovelName(书名):\t" + novelInfo2.Name + "\nNovelAuthor(作者):\t" + novelInfo2.Author + "\nIsboy(频道):\t" + novelInfo2.Isboy + "\nLagerSort(大类):\t" + novelInfo2.LagerSort + "\nSmallSort(小类):\t" + novelInfo2.SmallSort + "\nNovelIntro(简介):\t" + novelInfo2.Intro + "\nNovelKeyword(关键词):\t" + novelInfo2.Keyword + "\nNovelCover(封面):\t" + text2 + "\nNovelDegree(连载状态):\t" + novelInfo2.Degree.ToString());
		report("");
		report("====== 开始测试章节目录页 ======");
		report("PubIndexUrl\t" + novelInfo2.IndexUrl);
		cancellationToken.ThrowIfCancellationRequested();
		ChapterInfo[] chapterList = page.GetChapterList(novelInfo2, bool_0: true);
		if (chapterList.Length == 0)
		{
			throw new ApplicationException("没有获得章节列表");
		}
		string text3 = "";
		int num = FormatText.GetInt(ruleConfigInfo_0.PubContentChapterNum.Pattern, 0);
		for (int j = 0; j < chapterList.Length; j++)
		{
			string[] array2 = new string[6]
			{
				text3,
				chapterList[j].GetID,
				"\t",
				chapterList[j].VolumeName,
				"\t",
				chapterList[j].ChapterName
			};
			text3 = string.Concat(array2);
			text3 = ((chapterList.Length - j > num || !(ruleConfigInfo_0.PubContentChapterName.Pattern != "")) ? (text3 + "\n") : (text3 + "\t[新]\n"));
		}
		report(text3);
		report("");
		report("====== 开始测试章节内容页 ======");
		NovelInfo chapterInfo;
		object[] array3;
		if (string_1 != "0")
		{
			for (int k = 0; k < chapterList.Length; k++)
			{
				if (chapterList[k].GetID == string_1)
				{
					novelInfo2.LastChapter = chapterList[k];
					break;
				}
			}
			if (novelInfo2.LastChapter == null)
			{
				throw new ApplicationException("没有找到指定章节ID：" + string_1);
			}
			report("PubContentUrl:\t" + novelInfo2.LastChapter.GetID);
			cancellationToken.ThrowIfCancellationRequested();
			cancellationToken.ThrowIfCancellationRequested();
			chapterInfo = await page.GetChapterInfoAsync(novelInfo2, isvip: false, cancellationToken).ConfigureAwait(false);
			array3 = new object[6]
			{
				"ChapterName:\t",
				chapterInfo.LastChapter.ChapterName,
				"\nChapterUrl:\t",
				chapterInfo.LastChapter.ChapterUrl,
				"\nChapterText:\t",
				null
			};
		}
		else
		{
			novelInfo2.LastChapter = chapterList[random.Next(chapterList.Length)];
			report("PubContentUrl:\t" + novelInfo2.LastChapter.GetID);
			chapterInfo = await page.GetChapterInfoAsync(novelInfo2, isvip: false, cancellationToken).ConfigureAwait(false);
			array3 = new object[6]
			{
				"ChapterName:\t",
				chapterInfo.LastChapter.ChapterName,
				"\nChapterUrl:\t",
				chapterInfo.LastChapter.ChapterUrl,
				"\nChapterText:\t",
				null
			};
		}
		chapterInfo.LastChapter.ChapterText = page.Replace(chapterInfo.LastChapter.ChapterText, ruleConfigInfo_0.PubContentReplace);
		array3[5] = chapterInfo.LastChapter.ChapterText;
		report(string.Concat(array3));
		report("PubContentText:\t" + chapterInfo.LastChapter.ChapterText);
	}
	private void AppendResult(string text)
	{
		if (IsDisposed)
		{
			return;
		}
		if (InvokeRequired)
		{
			BeginInvoke(new Action<string>(AppendResult), text);
			return;
		}
		TestResult.AppendText(text + "\n");
		TestResult.Focus();
		TestResult.Select(TestResult.TextLength, 0);
		TestResult.ScrollToCaret();
	}

	private async Task StartRuleTestAsync()
	{
		if (isRunning)
		{
			return;
		}
		isRunning = true;
		cancellationTokenSource?.Dispose();
		cancellationTokenSource = new CancellationTokenSource();
		CancellationToken cancellationToken = cancellationTokenSource.Token;
		try
		{
			await System.Threading.Tasks.Task.Run(async () => await RunRuleTestAsync(AppendResult, cancellationToken).ConfigureAwait(false), cancellationToken).ConfigureAwait(true);
			AppendResult("测试结束！");
		}
		catch (OperationCanceledException)
		{
			AppendResult("测试已取消");
		}
		catch (Exception ex)
		{
			AppendResult("发生错误：" + ex.Message);
		}
		finally
		{
			isRunning = false;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			cancellationTokenSource?.Cancel();
			cancellationTokenSource?.Dispose();
			icontainer_0?.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.RuleTestForm));
		this.TestResult = new System.Windows.Forms.RichTextBox();
		base.SuspendLayout();
		this.TestResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TestResult.Location = new System.Drawing.Point(0, 0);
		this.TestResult.Name = "TestResult";
		this.TestResult.Size = new System.Drawing.Size(530, 302);
		this.TestResult.TabIndex = 3;
		this.TestResult.Text = "";
		base.ClientSize = new System.Drawing.Size(530, 302);
		base.Controls.Add(this.TestResult);
		base.Icon = AppIconProvider.Icon;
		base.Name = "RuleTestForm";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "规则测试";
		base.Load += new System.EventHandler(RuleTestForm_Load);
		base.ResumeLayout(false);
	}

	private async void RuleTestForm_Load(object sender, EventArgs e)
	{
		TestResult.Text = "";
		await StartRuleTestAsync();
	}
}
