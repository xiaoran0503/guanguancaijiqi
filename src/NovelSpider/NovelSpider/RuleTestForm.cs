using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;
using NovelSpider.Target;

namespace NovelSpider;

public class RuleTestForm : Form
{
	private BackgroundWorker backgroundWorker_0;

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

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		taskConfigInfo_0.Proxy = false;
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		Page page = new Page(ruleConfigInfo_0, taskConfigInfo_0);
		bool hasSingleNovelId = string_0 != "0" && string_0 != "";
		NovelInfo[] novelList = Array.Empty<NovelInfo>();
		if (!hasSingleNovelId || !string.IsNullOrWhiteSpace(ruleConfigInfo_0.NovelListUrl.Pattern))
		{
			backgroundWorker.ReportProgress(0, "====== 开始测试获得最新列表 ======");
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
			backgroundWorker.ReportProgress(0, text);
			backgroundWorker.ReportProgress(0, "");
		}
		else
		{
			backgroundWorker.ReportProgress(0, "====== 已填写单本小说ID，跳过最新列表测试 ======");
		}
		backgroundWorker.ReportProgress(0, "====== 开始测试小说信息页 ======");
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
		NovelInfo novelInfo2 = page.GetNovelInfo(novelInfo_);
		string text2 = "获取失败";
		if (novelInfo2.Cover != null)
		{
			text2 = "获取成功";
		}
		backgroundWorker.ReportProgress(0, "RuleID(站点ID):\t" + novelInfo2.RuleID + "\nNovelUrl(信息页URL):\t" + novelInfo2.NovelUrl.AbsolutePath + "\nNovelName(书名):\t" + novelInfo2.Name + "\nNovelAuthor(作者):\t" + novelInfo2.Author + "\nIsboy(频道):\t" + novelInfo2.Isboy + "\nLagerSort(大类):\t" + novelInfo2.LagerSort + "\nSmallSort(小类):\t" + novelInfo2.SmallSort + "\nNovelIntro(简介):\t" + novelInfo2.Intro + "\nNovelKeyword(关键词):\t" + novelInfo2.Keyword + "\nNovelCover(封面):\t" + text2 + "\nNovelDegree(连载状态):\t" + novelInfo2.Degree.ToString());
		backgroundWorker.ReportProgress(0, "");
		backgroundWorker.ReportProgress(0, "====== 开始测试章节目录页 ======");
		Thread.Sleep(100);
		backgroundWorker.ReportProgress(0, "PubIndexUrl\t" + novelInfo2.IndexUrl);
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
		backgroundWorker.ReportProgress(0, text3);
		backgroundWorker.ReportProgress(0, "");
		backgroundWorker.ReportProgress(0, "====== 开始测试章节内容页 ======");
		Thread.Sleep(100);
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
			backgroundWorker.ReportProgress(0, "PubContentUrl:\t" + novelInfo2.LastChapter.GetID);
			chapterInfo = page.GetChapterInfo(novelInfo2, isvip: false);
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
			backgroundWorker.ReportProgress(0, "PubContentUrl:\t" + novelInfo2.LastChapter.GetID);
			chapterInfo = page.GetChapterInfo(novelInfo2, isvip: false);
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
		backgroundWorker.ReportProgress(0, string.Concat(array3));
		backgroundWorker.ReportProgress(0, "PubContentText:\t" + chapterInfo.LastChapter.ChapterText);
	}
	private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		TestResult.AppendText(e.UserState.ToString() + "\n");
		TestResult.Focus();
		TestResult.Select(TestResult.TextLength, 0);
		TestResult.ScrollToCaret();
	}

	private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			TestResult.AppendText("发生错误：" + e.Error.Message + "\n");
			TestResult.Focus();
			TestResult.Select(TestResult.TextLength, 0);
			TestResult.ScrollToCaret();
		}
		else
		{
			TestResult.AppendText("测试结束！");
			TestResult.Focus();
			TestResult.Select(TestResult.TextLength, 0);
			TestResult.ScrollToCaret();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && icontainer_0 != null)
		{
			icontainer_0.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.RuleTestForm));
		this.TestResult = new System.Windows.Forms.RichTextBox();
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		base.SuspendLayout();
		this.TestResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TestResult.Location = new System.Drawing.Point(0, 0);
		this.TestResult.Name = "TestResult";
		this.TestResult.Size = new System.Drawing.Size(530, 302);
		this.TestResult.TabIndex = 3;
		this.TestResult.Text = "";
		this.backgroundWorker_0.WorkerReportsProgress = true;
		this.backgroundWorker_0.WorkerSupportsCancellation = true;
		this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
		this.backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_0_ProgressChanged);
		this.backgroundWorker_0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_0_RunWorkerCompleted);
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

	private void RuleTestForm_Load(object sender, EventArgs e)
	{
		TestResult.Text = "";
		if (!backgroundWorker_0.IsBusy)
		{
			backgroundWorker_0.RunWorkerAsync();
		}
	}
}
