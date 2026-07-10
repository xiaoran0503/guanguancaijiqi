using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;
using NovelSpider.Local;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class HelpBatchCreate : DockContent
{
	private BackgroundWorker backgroundWorker_0;

	public bool bool_0;

	public bool bool_1;

	public bool bool_2;

	public bool bool_3;

	public bool bool_4;

	public bool bool_5;

	private Button button_0;

	private Button button_1;

	private Button button_2;

	private Button button_3;

	private Button button_4;

	private Button button1;

	public bool ChapterHtml;

	private CheckBox checkBox_0;

	private CheckBox checkBox_1;

	private CheckBox checkBox_2;

	private CheckBox checkBox_3;

	private CheckBox checkBox_4;

	private CheckBox checkBox_5;

	private CheckBox checkBox_6;

	private CheckBox checkBox_7;

	private CheckBox checkBox_8;

	private CheckBox checkBox_9;

	private CheckBox chkOPF;

	private CheckBox chkWAP;

	public string CmdText;

	private IContainer components;

	private DateTime dateTime_0;

	public bool FullHtml;

	private GroupBox groupBox_0;

	private GroupBox groupBox_1;

	private IContainer icontainer_0;

	public bool IndexHtml;

	public bool isGenOPF;

	public bool isGenWAP;

	private Label label_0;

	private Label label_1;

	private Label label_10;

	private Label label_11;

	private Label label_13;

	private Label label_2;

	private Label label_3;

	private Label label_4;

	private Label label_5;

	private Label label_6;

	private Label label_7;

	private Label label_8;

	private Label label_9;

	private Label label1;

	private Label label2;

	public bool Log;

	public int m_Interval;

	private NumericUpDown numericUpDown_0;

	private NumericUpDown numericUpDown_1;

	private NumericUpDown numericUpDown_2;

	private NumericUpDown numericUpDown_3;

	private ProgressBar progressBar_0;

	private ProgressBar progressBar_1;

	private TextBox textBox_0;

	private TextBox textBox1;

	private Timer timer_0;

	public bool Timing;

	public HelpBatchCreate()
	{
		dateTime_0 = DateTime.Now;
		m_Interval = 1;
		InitializeComponent();
	}

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		backgroundWorker_0.ReportProgress(2, "正在获得小说列表");
		NovelInfo[] novelList;
		try
		{
			novelList = LocalProvider.GetInstance().GetNovelList(CmdText);
		}
		catch (Exception ex)
		{
			if (!Log)
			{
				MessageBox.Show("无法载入小说列表，有可能是SQL语句错误。\n\n" + ex.Message);
			}
			else
			{
				SpiderException.Debug("批量生成", ex.Message);
			}
			return;
		}
		backgroundWorker_0.ReportProgress(5, novelList.Length - 1);
		for (int i = 0; i < novelList.Length; i++)
		{
			if (backgroundWorker_0.CancellationPending)
			{
				e.Cancel = true;
				break;
			}
			backgroundWorker_0.ReportProgress(3, i);
			backgroundWorker_0.ReportProgress(0, novelList[i].PutID + " | " + novelList[i].Name);
			backgroundWorker_0.ReportProgress(2, "正在获得章节列表");
			if (ChapterHtml || isGenWAP)
			{
				try
				{
					ChapterInfo[] chapterList = LocalProvider.GetInstance().GetChapterList(novelList[i].PutID);
					backgroundWorker_0.ReportProgress(6, chapterList.Length - 1);
					for (int j = 0; j < chapterList.Length; j++)
					{
						if (!backgroundWorker_0.CancellationPending)
						{
							backgroundWorker_0.ReportProgress(4, j);
							backgroundWorker_0.ReportProgress(1, chapterList[j].VolumeName + " " + chapterList[j].ChapterName);
							backgroundWorker_0.ReportProgress(2, "正在生成章节内容HTML");
							int int_ = 0;
							int int_2 = 0;
							string string_ = "";
							string string_2 = "";
							string text = "";
							if (j != 0)
							{
								string_ = chapterList[j - 1].ChapterName;
								int_ = chapterList[j - 1].PutID;
							}
							if (chapterList.Length > j + 1)
							{
								string_2 = chapterList[j + 1].ChapterName;
								int_2 = chapterList[j + 1].PutID;
							}
							text = chapterList[j].VolumeName;
							try
							{
								if (isGenWAP && ChapterHtml)
								{
									LocalProvider.GetInstance().CreateSingleChapter(novelList[i], chapterList[j], bool_0: true, int_, int_2, string_, string_2, text);
									LocalProvider.GetInstance().CreateWapChapter(novelList[i], chapterList[j], bool_0: true, int_, int_2, string_, string_2, text);
								}
								else if (isGenWAP)
								{
									LocalProvider.GetInstance().CreateWapChapter(novelList[i], chapterList[j], bool_0: true, int_, int_2, string_, string_2, text);
								}
								else if (ChapterHtml)
								{
									LocalProvider.GetInstance().CreateNoWapChapter(novelList[i], chapterList[j], bool_0: true, int_, int_2, string_, string_2, text);
								}
							}
							catch (Exception ex2)
							{
								if (!Log)
								{
									MessageBox.Show(ex2.Message);
								}
								else
								{
									SpiderException.Debug("批量生成", ex2.Message);
								}
							}
							continue;
						}
						e.Cancel = true;
						break;
					}
				}
				catch (Exception ex3)
				{
					if (!Log)
					{
						MessageBox.Show(ex3.Message);
					}
					else
					{
						SpiderException.Debug("批量生成", ex3.Message);
					}
				}
			}
			backgroundWorker_0.ReportProgress(1, "--");
			backgroundWorker_0.ReportProgress(2, "正在生成目录页,全文页及电子书");
			try
			{
				if (isGenOPF && !IndexHtml && !FullHtml && !bool_2 && !bool_5 && !bool_3 && !bool_4 && !bool_1 && !bool_0)
				{
					LocalProvider.GetInstance().CreateOPF(novelList[i]);
				}
				else
				{
					LocalProvider.GetInstance().CreateIndex(novelList[i], IndexHtml, FullHtml, bool_2, bool_5, bool_3, bool_4, bool_1, bool_0, bool_8: false, bool_9: false, 0);
				}
			}
			catch (Exception ex4)
			{
				if (!Log)
				{
					MessageBox.Show(ex4.Message);
				}
				else
				{
					SpiderException.Debug("批量生成", ex4.Message);
				}
			}
		}
	}

	private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		switch (e.ProgressPercentage)
		{
		case 0:
			label_5.Text = e.UserState.ToString();
			break;
		case 1:
			label_4.Text = e.UserState.ToString();
			break;
		case 2:
			label_3.Text = e.UserState.ToString();
			break;
		case 3:
		{
			int num2 = Convert.ToInt32(e.UserState);
			if (num2 <= progressBar_1.Maximum && num2 >= progressBar_1.Minimum)
			{
				progressBar_1.Value = Convert.ToInt32(e.UserState);
			}
			break;
		}
		case 4:
		{
			int num4 = Convert.ToInt32(e.UserState);
			if (num4 <= progressBar_0.Maximum && num4 >= progressBar_0.Minimum)
			{
				progressBar_0.Value = Convert.ToInt32(e.UserState);
			}
			break;
		}
		case 5:
		{
			int num3 = Convert.ToInt32(e.UserState);
			if (num3 > 0)
			{
				progressBar_1.Maximum = num3;
			}
			break;
		}
		case 6:
		{
			int num = Convert.ToInt32(e.UserState);
			if (num > 0)
			{
				progressBar_0.Maximum = num;
			}
			break;
		}
		}
	}

	private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			label_3.Text = "错误提示：" + e.Error.Message;
			button_1.Enabled = true;
			groupBox_0.Enabled = true;
			button_0.Enabled = false;
			return;
		}
		if (e.Cancelled)
		{
			label_3.Text = "操作取消";
			button_1.Enabled = true;
			groupBox_0.Enabled = true;
			button_0.Enabled = false;
			return;
		}
		label_3.Text = "操作完成";
		if (Timing)
		{
			timer_0.Start();
			dateTime_0 = DateTime.Now.AddMinutes(m_Interval);
			return;
		}
		label_3.Text = "操作完成";
		groupBox_0.Enabled = true;
		button_1.Enabled = true;
		button_0.Enabled = false;
	}

	private void button_0_Click(object sender, EventArgs e)
	{
		if (backgroundWorker_0.IsBusy)
		{
			button_0.Enabled = false;
			backgroundWorker_0.CancelAsync();
			timer_0.Stop();
		}
	}

	private void button_1_Click(object sender, EventArgs e)
	{
		if (textBox_0.Text == "")
		{
			MessageBox.Show("请输入自定义SQL，选择单本或批量方式的请先生成自定义SQL。");
		}
		else if (!backgroundWorker_0.IsBusy)
		{
			button_1.Enabled = false;
			groupBox_0.Enabled = false;
			button_0.Enabled = true;
			IndexHtml = checkBox_7.Checked;
			ChapterHtml = checkBox_6.Checked;
			FullHtml = checkBox_5.Checked;
			bool_2 = true;
			bool_5 = checkBox_4.Checked;
			bool_3 = checkBox_3.Checked;
			bool_4 = checkBox_2.Checked;
			bool_1 = checkBox_0.Checked;
			bool_0 = checkBox_1.Checked;
			isGenWAP = chkWAP.Checked;
			isGenOPF = chkOPF.Checked;
			CmdText = textBox_0.Text;
			dateTime_0 = DateTime.Now.AddMinutes(m_Interval);
			backgroundWorker_0.RunWorkerAsync();
		}
	}

	private void button_2_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "UnsupportedCms")
		{
			textBox_0.Text = "SELECT * FROM [Ws_BookList] WHERE bookupdatetime BETWEEN '" + DateTime.Today.ToShortDateString() + "' AND '" + DateTime.Today.AddDays(1.0).ToShortDateString() + "' ORDER BY bookupdatetime ASC";
		}
		else if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `lastupdate` BETWEEN '" + FormatText.GetTime(DateTime.Today) + "' AND '" + FormatText.GetTime(DateTime.Today.AddDays(1.0)) + "' ORDER BY `lastupdate` ASC";
		}
		else if (Configs.CmsName == "Cnend")
		{
			textBox_0.Text = "SELECT * FROM [list_book] WHERE list_gxdate BETWEEN " + numericUpDown_1.Value + " AND " + numericUpDown_0.Value + " ORDER BY list_gxdate ASC";
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void button_3_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "UnsupportedCms")
		{
			textBox_0.Text = "SELECT * FROM [Ws_BookList] WHERE id BETWEEN " + numericUpDown_1.Value + " AND " + numericUpDown_0.Value + " ORDER BY id ASC";
		}
		else if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `articleid` BETWEEN '" + numericUpDown_1.Value + "' AND '" + numericUpDown_0.Value + "' ORDER BY `articleid` ASC";
		}
		else if (Configs.CmsName == "Cnend")
		{
			textBox_0.Text = "SELECT * FROM [list_book] WHERE id BETWEEN " + numericUpDown_1.Value + " AND " + numericUpDown_0.Value + " ORDER BY id ASC";
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void button_4_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "UnsupportedCms")
		{
			textBox_0.Text = "SELECT TOP 1 * FROM [Ws_BookList] WHERE id = " + numericUpDown_2.Value;
		}
		else if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `articleid` = '" + numericUpDown_2.Value + "'";
		}
		else if (Configs.CmsName == "Cnend")
		{
			textBox_0.Text = "SELECT TOP 1 * FROM [list_book] WHERE id = " + numericUpDown_2.Value;
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "UnsupportedCms")
		{
			textBox_0.Text = "SELECT TOP 1 * FROM [Ws_BookList] WHERE id in (" + textBox1.Text.ToString() + ")";
		}
		else if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `articleid` in (" + textBox1.Text.ToString() + ")";
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void checkBox_8_CheckedChanged(object sender, EventArgs e)
	{
		if (checkBox_8.Checked)
		{
			Log = false;
		}
		else
		{
			Log = true;
		}
	}

	private void checkBox_9_CheckedChanged(object sender, EventArgs e)
	{
		if (checkBox_9.Checked)
		{
			Timing = true;
		}
		else
		{
			Timing = false;
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

	private void HelpBatchCreate_Load(object sender, EventArgs e)
	{
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.HelpBatchCreate));
		this.groupBox_0 = new System.Windows.Forms.GroupBox();
		this.chkWAP = new System.Windows.Forms.CheckBox();
		this.chkOPF = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label_11 = new System.Windows.Forms.Label();
		this.checkBox_6 = new System.Windows.Forms.CheckBox();
		this.checkBox_7 = new System.Windows.Forms.CheckBox();
		this.button_2 = new System.Windows.Forms.Button();
		this.checkBox_5 = new System.Windows.Forms.CheckBox();
		this.button_3 = new System.Windows.Forms.Button();
		this.button_4 = new System.Windows.Forms.Button();
		this.label_0 = new System.Windows.Forms.Label();
		this.label_1 = new System.Windows.Forms.Label();
		this.label_2 = new System.Windows.Forms.Label();
		this.checkBox_0 = new System.Windows.Forms.CheckBox();
		this.checkBox_1 = new System.Windows.Forms.CheckBox();
		this.checkBox_2 = new System.Windows.Forms.CheckBox();
		this.checkBox_3 = new System.Windows.Forms.CheckBox();
		this.checkBox_4 = new System.Windows.Forms.CheckBox();
		this.numericUpDown_0 = new System.Windows.Forms.NumericUpDown();
		this.numericUpDown_1 = new System.Windows.Forms.NumericUpDown();
		this.numericUpDown_2 = new System.Windows.Forms.NumericUpDown();
		this.textBox_0 = new System.Windows.Forms.TextBox();
		this.button_0 = new System.Windows.Forms.Button();
		this.button_1 = new System.Windows.Forms.Button();
		this.groupBox_1 = new System.Windows.Forms.GroupBox();
		this.label_13 = new System.Windows.Forms.Label();
		this.numericUpDown_3 = new System.Windows.Forms.NumericUpDown();
		this.checkBox_9 = new System.Windows.Forms.CheckBox();
		this.checkBox_8 = new System.Windows.Forms.CheckBox();
		this.label_3 = new System.Windows.Forms.Label();
		this.label_4 = new System.Windows.Forms.Label();
		this.label_5 = new System.Windows.Forms.Label();
		this.label_6 = new System.Windows.Forms.Label();
		this.label_7 = new System.Windows.Forms.Label();
		this.label_8 = new System.Windows.Forms.Label();
		this.progressBar_0 = new System.Windows.Forms.ProgressBar();
		this.label_9 = new System.Windows.Forms.Label();
		this.label_10 = new System.Windows.Forms.Label();
		this.progressBar_1 = new System.Windows.Forms.ProgressBar();
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		this.timer_0 = new System.Windows.Forms.Timer(this.components);
		this.groupBox_0.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_2).BeginInit();
		this.groupBox_1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).BeginInit();
		base.SuspendLayout();
		this.groupBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_0.Controls.Add(this.chkWAP);
		this.groupBox_0.Controls.Add(this.chkOPF);
		this.groupBox_0.Controls.Add(this.label2);
		this.groupBox_0.Controls.Add(this.button1);
		this.groupBox_0.Controls.Add(this.textBox1);
		this.groupBox_0.Controls.Add(this.label1);
		this.groupBox_0.Controls.Add(this.label_11);
		this.groupBox_0.Controls.Add(this.checkBox_6);
		this.groupBox_0.Controls.Add(this.checkBox_7);
		this.groupBox_0.Controls.Add(this.button_2);
		this.groupBox_0.Controls.Add(this.checkBox_5);
		this.groupBox_0.Controls.Add(this.button_3);
		this.groupBox_0.Controls.Add(this.button_4);
		this.groupBox_0.Controls.Add(this.label_0);
		this.groupBox_0.Controls.Add(this.label_1);
		this.groupBox_0.Controls.Add(this.label_2);
		this.groupBox_0.Controls.Add(this.checkBox_0);
		this.groupBox_0.Controls.Add(this.checkBox_1);
		this.groupBox_0.Controls.Add(this.checkBox_2);
		this.groupBox_0.Controls.Add(this.checkBox_3);
		this.groupBox_0.Controls.Add(this.checkBox_4);
		this.groupBox_0.Controls.Add(this.numericUpDown_0);
		this.groupBox_0.Controls.Add(this.numericUpDown_1);
		this.groupBox_0.Controls.Add(this.numericUpDown_2);
		this.groupBox_0.Controls.Add(this.textBox_0);
		this.groupBox_0.Location = new System.Drawing.Point(12, 12);
		this.groupBox_0.Name = "groupBox_0";
		this.groupBox_0.Size = new System.Drawing.Size(758, 202);
		this.groupBox_0.TabIndex = 0;
		this.groupBox_0.TabStop = false;
		this.groupBox_0.Text = "生成设置";
		this.chkWAP.AutoSize = true;
		this.chkWAP.Checked = true;
		this.chkWAP.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkWAP.Location = new System.Drawing.Point(554, 130);
		this.chkWAP.Name = "chkWAP";
		this.chkWAP.Size = new System.Drawing.Size(42, 16);
		this.chkWAP.TabIndex = 23;
		this.chkWAP.Text = "WAP";
		this.chkWAP.UseVisualStyleBackColor = true;
		this.chkOPF.AutoSize = true;
		this.chkOPF.Location = new System.Drawing.Point(600, 130);
		this.chkOPF.Name = "chkOPF";
		this.chkOPF.Size = new System.Drawing.Size(42, 16);
		this.chkOPF.TabIndex = 23;
		this.chkOPF.Text = "OPF";
		this.chkOPF.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.ForeColor = System.Drawing.Color.Blue;
		this.label2.Location = new System.Drawing.Point(6, 177);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(287, 12);
		this.label2.TabIndex = 22;
		this.label2.Text = "请先“生成自定义SQL”，再开始执行批量生成任务。";
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button1.Location = new System.Drawing.Point(627, 73);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(111, 21);
		this.button1.TabIndex = 21;
		this.button1.Text = "生成自定义SQL";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox1.Location = new System.Drawing.Point(111, 73);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(510, 21);
		this.textBox1.TabIndex = 20;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(7, 76);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(107, 12);
		this.label1.TabIndex = 19;
		this.label1.Text = "自定义ID(,分割)：";
		this.label_11.AutoSize = true;
		this.label_11.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label_11.ForeColor = System.Drawing.Color.Blue;
		this.label_11.Location = new System.Drawing.Point(6, 155);
		this.label_11.Name = "label_11";
		this.label_11.Size = new System.Drawing.Size(419, 12);
		this.label_11.TabIndex = 18;
		this.label_11.Text = "批量生成最终是按“自定义SQL”执行，选择“单本ID”或“批量ID”方式的。";
		this.checkBox_6.AutoSize = true;
		this.checkBox_6.Checked = true;
		this.checkBox_6.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBox_6.Location = new System.Drawing.Point(111, 130);
		this.checkBox_6.Name = "checkBox_6";
		this.checkBox_6.Size = new System.Drawing.Size(96, 16);
		this.checkBox_6.TabIndex = 5;
		this.checkBox_6.Text = "章节内容HTML";
		this.checkBox_6.UseVisualStyleBackColor = true;
		this.checkBox_7.AutoSize = true;
		this.checkBox_7.Checked = true;
		this.checkBox_7.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBox_7.Location = new System.Drawing.Point(9, 130);
		this.checkBox_7.Name = "checkBox_7";
		this.checkBox_7.Size = new System.Drawing.Size(96, 16);
		this.checkBox_7.TabIndex = 4;
		this.checkBox_7.Text = "章节目录HTML";
		this.checkBox_7.UseVisualStyleBackColor = true;
		this.button_2.Location = new System.Drawing.Point(341, 20);
		this.button_2.Name = "button_2";
		this.button_2.Size = new System.Drawing.Size(120, 21);
		this.button_2.TabIndex = 17;
		this.button_2.Text = "今日更新书籍SQL";
		this.button_2.UseVisualStyleBackColor = true;
		this.button_2.Click += new System.EventHandler(button_2_Click);
		this.checkBox_5.AutoSize = true;
		this.checkBox_5.Location = new System.Drawing.Point(213, 130);
		this.checkBox_5.Name = "checkBox_5";
		this.checkBox_5.Size = new System.Drawing.Size(96, 16);
		this.checkBox_5.TabIndex = 6;
		this.checkBox_5.Text = "全文阅读HTML";
		this.checkBox_5.UseVisualStyleBackColor = true;
		this.button_3.Location = new System.Drawing.Point(341, 47);
		this.button_3.Name = "button_3";
		this.button_3.Size = new System.Drawing.Size(120, 21);
		this.button_3.TabIndex = 16;
		this.button_3.Text = "生成自定义SQL";
		this.button_3.UseVisualStyleBackColor = true;
		this.button_3.Click += new System.EventHandler(button_3_Click);
		this.button_4.Location = new System.Drawing.Point(215, 20);
		this.button_4.Name = "button_4";
		this.button_4.Size = new System.Drawing.Size(120, 21);
		this.button_4.TabIndex = 15;
		this.button_4.Text = "生成自定义SQL";
		this.button_4.UseVisualStyleBackColor = true;
		this.button_4.Click += new System.EventHandler(button_4_Click);
		this.label_0.AutoSize = true;
		this.label_0.Location = new System.Drawing.Point(7, 103);
		this.label_0.Name = "label_0";
		this.label_0.Size = new System.Drawing.Size(77, 12);
		this.label_0.TabIndex = 14;
		this.label_0.Text = "自定义SQL ：";
		this.label_1.AutoSize = true;
		this.label_1.Location = new System.Drawing.Point(7, 49);
		this.label_1.Name = "label_1";
		this.label_1.Size = new System.Drawing.Size(77, 12);
		this.label_1.TabIndex = 13;
		this.label_1.Text = "批量生成ID：";
		this.label_2.AutoSize = true;
		this.label_2.Location = new System.Drawing.Point(6, 22);
		this.label_2.Name = "label_2";
		this.label_2.Size = new System.Drawing.Size(77, 12);
		this.label_2.TabIndex = 12;
		this.label_2.Text = "单本生成ID：";
		this.checkBox_0.AutoSize = true;
		this.checkBox_0.Location = new System.Drawing.Point(459, 130);
		this.checkBox_0.Name = "checkBox_0";
		this.checkBox_0.Size = new System.Drawing.Size(42, 16);
		this.checkBox_0.TabIndex = 11;
		this.checkBox_0.Text = "JAR";
		this.checkBox_0.UseVisualStyleBackColor = true;
		this.checkBox_1.AutoSize = true;
		this.checkBox_1.Location = new System.Drawing.Point(507, 130);
		this.checkBox_1.Name = "checkBox_1";
		this.checkBox_1.Size = new System.Drawing.Size(42, 16);
		this.checkBox_1.TabIndex = 10;
		this.checkBox_1.Text = "CHM";
		this.checkBox_1.UseVisualStyleBackColor = true;
		this.checkBox_2.AutoSize = true;
		this.checkBox_2.Location = new System.Drawing.Point(411, 130);
		this.checkBox_2.Name = "checkBox_2";
		this.checkBox_2.Size = new System.Drawing.Size(42, 16);
		this.checkBox_2.TabIndex = 9;
		this.checkBox_2.Text = "UMD";
		this.checkBox_2.UseVisualStyleBackColor = true;
		this.checkBox_3.AutoSize = true;
		this.checkBox_3.Location = new System.Drawing.Point(363, 130);
		this.checkBox_3.Name = "checkBox_3";
		this.checkBox_3.Size = new System.Drawing.Size(42, 16);
		this.checkBox_3.TabIndex = 8;
		this.checkBox_3.Text = "TXT";
		this.checkBox_3.UseVisualStyleBackColor = true;
		this.checkBox_4.AutoSize = true;
		this.checkBox_4.Location = new System.Drawing.Point(315, 130);
		this.checkBox_4.Name = "checkBox_4";
		this.checkBox_4.Size = new System.Drawing.Size(42, 16);
		this.checkBox_4.TabIndex = 7;
		this.checkBox_4.Text = "ZIP";
		this.checkBox_4.UseVisualStyleBackColor = true;
		this.numericUpDown_0.Location = new System.Drawing.Point(215, 47);
		this.numericUpDown_0.Maximum = new decimal(new int[4] { 1215752192, 23, 0, 0 });
		this.numericUpDown_0.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.numericUpDown_0.Name = "numericUpDown_0";
		this.numericUpDown_0.Size = new System.Drawing.Size(120, 21);
		this.numericUpDown_0.TabIndex = 3;
		this.numericUpDown_0.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.numericUpDown_1.Location = new System.Drawing.Point(89, 47);
		this.numericUpDown_1.Maximum = new decimal(new int[4] { 1215752192, 23, 0, 0 });
		this.numericUpDown_1.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.numericUpDown_1.Name = "numericUpDown_1";
		this.numericUpDown_1.Size = new System.Drawing.Size(120, 21);
		this.numericUpDown_1.TabIndex = 2;
		this.numericUpDown_1.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.numericUpDown_2.Location = new System.Drawing.Point(89, 20);
		this.numericUpDown_2.Maximum = new decimal(new int[4] { 1215752192, 23, 0, 0 });
		this.numericUpDown_2.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.numericUpDown_2.Name = "numericUpDown_2";
		this.numericUpDown_2.Size = new System.Drawing.Size(120, 21);
		this.numericUpDown_2.TabIndex = 1;
		this.numericUpDown_2.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.textBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_0.Location = new System.Drawing.Point(89, 100);
		this.textBox_0.Name = "textBox_0";
		this.textBox_0.Size = new System.Drawing.Size(658, 21);
		this.textBox_0.TabIndex = 0;
		this.button_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button_0.Location = new System.Drawing.Point(672, 98);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 21);
		this.button_0.TabIndex = 19;
		this.button_0.Text = "停止";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(button_0_Click);
		this.button_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button_1.Location = new System.Drawing.Point(591, 99);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(75, 21);
		this.button_1.TabIndex = 18;
		this.button_1.Text = "开始";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.groupBox_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_1.Controls.Add(this.label_13);
		this.groupBox_1.Controls.Add(this.numericUpDown_3);
		this.groupBox_1.Controls.Add(this.checkBox_9);
		this.groupBox_1.Controls.Add(this.checkBox_8);
		this.groupBox_1.Controls.Add(this.button_0);
		this.groupBox_1.Controls.Add(this.label_3);
		this.groupBox_1.Controls.Add(this.label_4);
		this.groupBox_1.Controls.Add(this.button_1);
		this.groupBox_1.Controls.Add(this.label_5);
		this.groupBox_1.Controls.Add(this.label_6);
		this.groupBox_1.Controls.Add(this.label_7);
		this.groupBox_1.Controls.Add(this.label_8);
		this.groupBox_1.Controls.Add(this.progressBar_0);
		this.groupBox_1.Controls.Add(this.label_9);
		this.groupBox_1.Controls.Add(this.label_10);
		this.groupBox_1.Controls.Add(this.progressBar_1);
		this.groupBox_1.Location = new System.Drawing.Point(12, 220);
		this.groupBox_1.Name = "groupBox_1";
		this.groupBox_1.Size = new System.Drawing.Size(758, 191);
		this.groupBox_1.TabIndex = 4;
		this.groupBox_1.TabStop = false;
		this.groupBox_1.Text = "生成进度";
		this.label_13.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label_13.AutoSize = true;
		this.label_13.Location = new System.Drawing.Point(718, 65);
		this.label_13.Name = "label_13";
		this.label_13.Size = new System.Drawing.Size(29, 12);
		this.label_13.TabIndex = 23;
		this.label_13.Text = "分钟";
		this.numericUpDown_3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.numericUpDown_3.Location = new System.Drawing.Point(662, 63);
		this.numericUpDown_3.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown_3.Name = "numericUpDown_3";
		this.numericUpDown_3.Size = new System.Drawing.Size(50, 21);
		this.numericUpDown_3.TabIndex = 22;
		this.numericUpDown_3.ValueChanged += new System.EventHandler(numericUpDown_3_ValueChanged);
		this.checkBox_9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.checkBox_9.AutoSize = true;
		this.checkBox_9.Location = new System.Drawing.Point(554, 64);
		this.checkBox_9.Name = "checkBox_9";
		this.checkBox_9.Size = new System.Drawing.Size(102, 16);
		this.checkBox_9.TabIndex = 21;
		this.checkBox_9.Text = "定时采集 间隔";
		this.checkBox_9.UseVisualStyleBackColor = true;
		this.checkBox_9.CheckedChanged += new System.EventHandler(checkBox_9_CheckedChanged);
		this.checkBox_8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.checkBox_8.AutoSize = true;
		this.checkBox_8.Checked = true;
		this.checkBox_8.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBox_8.Location = new System.Drawing.Point(452, 64);
		this.checkBox_8.Name = "checkBox_8";
		this.checkBox_8.Size = new System.Drawing.Size(96, 16);
		this.checkBox_8.TabIndex = 20;
		this.checkBox_8.Text = "弹出异常提示";
		this.checkBox_8.UseVisualStyleBackColor = true;
		this.checkBox_8.CheckedChanged += new System.EventHandler(checkBox_8_CheckedChanged);
		this.label_3.AutoSize = true;
		this.label_3.Location = new System.Drawing.Point(89, 108);
		this.label_3.Name = "label_3";
		this.label_3.Size = new System.Drawing.Size(17, 12);
		this.label_3.TabIndex = 16;
		this.label_3.Text = "--";
		this.label_4.AutoSize = true;
		this.label_4.Location = new System.Drawing.Point(89, 86);
		this.label_4.Name = "label_4";
		this.label_4.Size = new System.Drawing.Size(17, 12);
		this.label_4.TabIndex = 15;
		this.label_4.Text = "--";
		this.label_5.AutoSize = true;
		this.label_5.Location = new System.Drawing.Point(89, 64);
		this.label_5.Name = "label_5";
		this.label_5.Size = new System.Drawing.Size(17, 12);
		this.label_5.TabIndex = 14;
		this.label_5.Text = "--";
		this.label_6.AutoSize = true;
		this.label_6.Location = new System.Drawing.Point(18, 107);
		this.label_6.Name = "label_6";
		this.label_6.Size = new System.Drawing.Size(65, 12);
		this.label_6.TabIndex = 13;
		this.label_6.Text = "当前动作：";
		this.label_7.AutoSize = true;
		this.label_7.Location = new System.Drawing.Point(18, 86);
		this.label_7.Name = "label_7";
		this.label_7.Size = new System.Drawing.Size(65, 12);
		this.label_7.TabIndex = 12;
		this.label_7.Text = "当前章节：";
		this.label_8.AutoSize = true;
		this.label_8.Location = new System.Drawing.Point(18, 64);
		this.label_8.Name = "label_8";
		this.label_8.Size = new System.Drawing.Size(65, 12);
		this.label_8.TabIndex = 11;
		this.label_8.Text = "当前小说：";
		this.progressBar_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.progressBar_0.Location = new System.Drawing.Point(89, 43);
		this.progressBar_0.Name = "progressBar_0";
		this.progressBar_0.Size = new System.Drawing.Size(658, 12);
		this.progressBar_0.TabIndex = 10;
		this.label_9.AutoSize = true;
		this.label_9.Location = new System.Drawing.Point(19, 43);
		this.label_9.Name = "label_9";
		this.label_9.Size = new System.Drawing.Size(65, 12);
		this.label_9.TabIndex = 9;
		this.label_9.Text = "章节进度：";
		this.label_10.AutoSize = true;
		this.label_10.Location = new System.Drawing.Point(18, 20);
		this.label_10.Name = "label_10";
		this.label_10.Size = new System.Drawing.Size(65, 12);
		this.label_10.TabIndex = 8;
		this.label_10.Text = "小说进度：";
		this.progressBar_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.progressBar_1.Location = new System.Drawing.Point(89, 20);
		this.progressBar_1.Name = "progressBar_1";
		this.progressBar_1.Size = new System.Drawing.Size(658, 12);
		this.progressBar_1.TabIndex = 7;
		this.backgroundWorker_0.WorkerReportsProgress = true;
		this.backgroundWorker_0.WorkerSupportsCancellation = true;
		this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
		this.backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_0_ProgressChanged);
		this.backgroundWorker_0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_0_RunWorkerCompleted);
		this.timer_0.Tick += new System.EventHandler(timer_0_Tick);
		base.ClientSize = new System.Drawing.Size(782, 423);
		base.Controls.Add(this.groupBox_1);
		base.Controls.Add(this.groupBox_0);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "HelpBatchCreate";
		this.Text = "批量生成";
		base.Load += new System.EventHandler(HelpBatchCreate_Load);
		this.groupBox_0.ResumeLayout(false);
		this.groupBox_0.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_2).EndInit();
		this.groupBox_1.ResumeLayout(false);
		this.groupBox_1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).EndInit();
		base.ResumeLayout(false);
	}

	private void numericUpDown_3_ValueChanged(object sender, EventArgs e)
	{
		m_Interval = Convert.ToInt32(numericUpDown_3.Value);
	}

	private void timer_0_Tick(object sender, EventArgs e)
	{
		if (!backgroundWorker_0.IsBusy)
		{
			if (dateTime_0 < DateTime.Now)
			{
				dateTime_0 = DateTime.Now.AddMinutes(m_Interval);
				backgroundWorker_0.RunWorkerAsync();
				timer_0.Stop();
				return;
			}
			TimeSpan timeSpan = new TimeSpan(dateTime_0.Ticks).Subtract(new TimeSpan(DateTime.Now.Ticks)).Duration();
			label_3.Text = "距离下次循环开始还有 " + timeSpan.Days + "天" + timeSpan.Hours + "小时" + timeSpan.Minutes + "分钟" + timeSpan.Seconds + "秒";
		}
	}
}
