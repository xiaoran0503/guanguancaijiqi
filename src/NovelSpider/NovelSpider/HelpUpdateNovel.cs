using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;
using NovelSpider.Local;
using NovelSpider.Target;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class HelpUpdateNovel : DockContent
{
	private BackgroundWorker backgroundWorker_0;

	public bool BAuthor;

	public bool BCover;

	public bool BDegree;

	public bool BIntro;

	public bool BKeyword;

	public bool bool_0;

	public bool bool_1;

	private Button button_0;

	private Button button_1;

	private RadioButton 按本站编号顺序;

	private CheckBox 更新作者checkBox;

	private CheckBox 更新大类checkBox;

	private CheckBox 更新小类checkBox;

	private CheckBox 更新写作进程checkBox;

	private CheckBox 更新简介checkBox;

	private CheckBox 更新关键词checkBox;

	private CheckBox 更新封面checkBox;

	private ComboBox 规则选项comboBox;

	private GroupBox 更新生成进度groupBox;

	private GroupBox 更新设置;

	private IContainer icontainer_0;

	private Label label_0;

	private Label label_1;

	private Label label1;

	public int MaxID;

	private TextBox 本站结束ID;

	public int memaxID;

	private TextBox 本站开始ID;

	public int meminID;

	public int MinID;

	private NumericUpDown 更新循环搜索时间;

	private RadioButton 按目标站编号顺序;

	public RuleConfigInfo rInfo;

	private TextBox 目标站结束ID;

	private TextBox 目标站起ID;

	public TaskConfigInfo tInfo;

	public HelpUpdateNovel()
	{
		rInfo = new RuleConfigInfo();
		tInfo = new TaskConfigInfo();
		InitializeComponent();
	}

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		bool flag = false;
		if (按本站编号顺序.Checked)
		{
			MinID = meminID;
			MaxID = memaxID;
			flag = true;
		}
		int num = MinID;
		while (true)
		{
			if (num > MaxID)
			{
				return;
			}
			if (backgroundWorker_0.CancellationPending)
			{
				break;
			}
			NovelInfo novelInfo = new NovelInfo();
			if (flag)
			{
				Thread.Sleep(Convert.ToInt32(更新循环搜索时间.Value) * 1000);
				NovelInfo novelInfo2 = new NovelInfo();
				novelInfo2.PutID = num;
				NovelInfo novelInfo3 = novelInfo2;
				try
				{
					SpiderException.Debug("开始按本站ID获取信息并搜索目标站");
					Page page = new Page(rInfo, tInfo);
					novelInfo3 = LocalProvider.GetInstance().GetNovelInfo(novelInfo3, tInfo.NameAndAuthor);
					novelInfo3 = page.GetNovelInfo(novelInfo3);
					novelInfo.GetID = novelInfo3.GetID;
				}
				catch (Exception ex)
				{
					SpiderException.Show("根据本站ID:" + novelInfo3.PutID + "获取目标站ID时错误,请确认规则支持搜索 | " + ex.Message, bool_0: true);
					backgroundWorker_0.ReportProgress(0, novelInfo3.PutID + " | " + ex.Message);
				}
			}
			else
			{
				novelInfo.GetID = num.ToString();
			}
			try
			{
				backgroundWorker_0.ReportProgress(0, novelInfo.GetID);
				Page page2 = new Page(rInfo, tInfo);
				SpiderException.Debug("开始按目标站ID获取小说信息");
				novelInfo = page2.GetNovelInfo(novelInfo);
				backgroundWorker_0.ReportProgress(0, novelInfo.GetID + " | " + novelInfo.Name);
				if (!BCover)
				{
					novelInfo.Cover = null;
				}
				SpiderException.Debug("更新小说信息完成!");
				LocalProvider.GetInstance().UpdateNovel(novelInfo, BAuthor, BIntro, BDegree, bool_0, bool_1, BCover, BKeyword);
			}
			catch (Exception ex2)
			{
				SpiderException.Show("更新小说信息：" + novelInfo.GetID + " | " + ex2.Message, bool_0: true);
				backgroundWorker_0.ReportProgress(0, novelInfo.GetID + " | " + ex2.Message);
			}
			num++;
		}
		e.Cancel = true;
	}

	private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		label_0.Text = e.UserState.ToString();
	}

	private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			MessageBox.Show(e.Error.Message, "错误提示");
		}
		else if (e.Cancelled)
		{
			MessageBox.Show("操作取消", "信息提示");
		}
		else
		{
			MessageBox.Show("操作完成", "信息提示");
		}
		button_0.Enabled = false;
		button_1.Enabled = true;
		更新设置.Enabled = true;
	}

	private void button_0_Click(object sender, EventArgs e)
	{
		button_0.Enabled = false;
		if (backgroundWorker_0.IsBusy)
		{
			backgroundWorker_0.CancelAsync();
		}
	}

	private void button_1_Click(object sender, EventArgs e)
	{
		rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(规则选项comboBox.Text, rInfo);
		BAuthor = 更新作者checkBox.Checked;
		BIntro = 更新简介checkBox.Checked;
		BDegree = 更新写作进程checkBox.Checked;
		bool_0 = 更新大类checkBox.Checked;
		bool_1 = 更新小类checkBox.Checked;
		BCover = 更新封面checkBox.Checked;
		BKeyword = 更新关键词checkBox.Checked;
		MinID = int.Parse(目标站起ID.Text);
		MaxID = int.Parse(目标站结束ID.Text);
		meminID = int.Parse(本站开始ID.Text);
		memaxID = int.Parse(本站结束ID.Text);
		button_0.Enabled = true;
		button_1.Enabled = false;
		更新设置.Enabled = false;
		backgroundWorker_0.RunWorkerAsync();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && icontainer_0 != null)
		{
			icontainer_0.Dispose();
		}
		base.Dispose(disposing);
	}

	private void HelpUpdateNovel_Load(object sender, EventArgs e)
	{
		tInfo = (TaskConfigInfo)ConfigFileManager.LoadConfig("TaskConfig.xml", tInfo);
		规则选项comboBox.BeginUpdate();
		string[] array = IO.LoadRules();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				规则选项comboBox.Items.Add(array[i]);
				if (array[i] == tInfo.RuleFile)
				{
					规则选项comboBox.Text = tInfo.RuleFile;
				}
			}
		}
		规则选项comboBox.EndUpdate();
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.HelpUpdateNovel));
		this.更新作者checkBox = new System.Windows.Forms.CheckBox();
		this.更新大类checkBox = new System.Windows.Forms.CheckBox();
		this.更新小类checkBox = new System.Windows.Forms.CheckBox();
		this.更新写作进程checkBox = new System.Windows.Forms.CheckBox();
		this.更新简介checkBox = new System.Windows.Forms.CheckBox();
		this.更新关键词checkBox = new System.Windows.Forms.CheckBox();
		this.更新封面checkBox = new System.Windows.Forms.CheckBox();
		this.目标站结束ID = new System.Windows.Forms.TextBox();
		this.目标站起ID = new System.Windows.Forms.TextBox();
		this.按目标站编号顺序 = new System.Windows.Forms.RadioButton();
		this.更新生成进度groupBox = new System.Windows.Forms.GroupBox();
		this.label_0 = new System.Windows.Forms.Label();
		this.label_1 = new System.Windows.Forms.Label();
		this.button_0 = new System.Windows.Forms.Button();
		this.button_1 = new System.Windows.Forms.Button();
		this.更新设置 = new System.Windows.Forms.GroupBox();
		this.更新循环搜索时间 = new System.Windows.Forms.NumericUpDown();
		this.label1 = new System.Windows.Forms.Label();
		this.规则选项comboBox = new System.Windows.Forms.ComboBox();
		this.按本站编号顺序 = new System.Windows.Forms.RadioButton();
		this.本站结束ID = new System.Windows.Forms.TextBox();
		this.本站开始ID = new System.Windows.Forms.TextBox();
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		this.更新生成进度groupBox.SuspendLayout();
		this.更新设置.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.更新循环搜索时间).BeginInit();
		base.SuspendLayout();
		this.更新作者checkBox.AutoSize = true;
		this.更新作者checkBox.Location = new System.Drawing.Point(7, 144);
		this.更新作者checkBox.Name = "更新作者checkBox";
		this.更新作者checkBox.Size = new System.Drawing.Size(48, 16);
		this.更新作者checkBox.TabIndex = 0;
		this.更新作者checkBox.Text = "作者";
		this.更新作者checkBox.UseVisualStyleBackColor = true;
		this.更新大类checkBox.AutoSize = true;
		this.更新大类checkBox.Location = new System.Drawing.Point(61, 144);
		this.更新大类checkBox.Name = "更新大类checkBox";
		this.更新大类checkBox.Size = new System.Drawing.Size(48, 16);
		this.更新大类checkBox.TabIndex = 1;
		this.更新大类checkBox.Text = "大类";
		this.更新大类checkBox.UseVisualStyleBackColor = true;
		this.更新小类checkBox.AutoSize = true;
		this.更新小类checkBox.Location = new System.Drawing.Point(115, 144);
		this.更新小类checkBox.Name = "更新小类checkBox";
		this.更新小类checkBox.Size = new System.Drawing.Size(48, 16);
		this.更新小类checkBox.TabIndex = 2;
		this.更新小类checkBox.Text = "小类";
		this.更新小类checkBox.UseVisualStyleBackColor = true;
		this.更新写作进程checkBox.AutoSize = true;
		this.更新写作进程checkBox.Checked = true;
		this.更新写作进程checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.更新写作进程checkBox.Location = new System.Drawing.Point(7, 166);
		this.更新写作进程checkBox.Name = "更新写作进程checkBox";
		this.更新写作进程checkBox.Size = new System.Drawing.Size(72, 16);
		this.更新写作进程checkBox.TabIndex = 3;
		this.更新写作进程checkBox.Text = "写作进程";
		this.更新写作进程checkBox.UseVisualStyleBackColor = true;
		this.更新简介checkBox.AutoSize = true;
		this.更新简介checkBox.Location = new System.Drawing.Point(85, 166);
		this.更新简介checkBox.Name = "更新简介checkBox";
		this.更新简介checkBox.Size = new System.Drawing.Size(72, 16);
		this.更新简介checkBox.TabIndex = 4;
		this.更新简介checkBox.Text = "内容简介";
		this.更新简介checkBox.UseVisualStyleBackColor = true;
		this.更新关键词checkBox.AutoSize = true;
		this.更新关键词checkBox.Location = new System.Drawing.Point(163, 166);
		this.更新关键词checkBox.Name = "更新关键词checkBox";
		this.更新关键词checkBox.Size = new System.Drawing.Size(96, 16);
		this.更新关键词checkBox.TabIndex = 5;
		this.更新关键词checkBox.Text = "关键词(主角)";
		this.更新关键词checkBox.UseVisualStyleBackColor = true;
		this.更新封面checkBox.AutoSize = true;
		this.更新封面checkBox.Location = new System.Drawing.Point(169, 144);
		this.更新封面checkBox.Name = "更新封面checkBox";
		this.更新封面checkBox.Size = new System.Drawing.Size(48, 16);
		this.更新封面checkBox.TabIndex = 6;
		this.更新封面checkBox.Text = "封面";
		this.更新封面checkBox.UseVisualStyleBackColor = true;
		this.目标站结束ID.Location = new System.Drawing.Point(115, 68);
		this.目标站结束ID.Name = "目标站结束ID";
		this.目标站结束ID.Size = new System.Drawing.Size(104, 21);
		this.目标站结束ID.TabIndex = 9;
		this.目标站结束ID.Text = "1";
		this.目标站起ID.Location = new System.Drawing.Point(9, 68);
		this.目标站起ID.Name = "目标站起ID";
		this.目标站起ID.Size = new System.Drawing.Size(100, 21);
		this.目标站起ID.TabIndex = 8;
		this.目标站起ID.Text = "1";
		this.按目标站编号顺序.AutoSize = true;
		this.按目标站编号顺序.Checked = true;
		this.按目标站编号顺序.Location = new System.Drawing.Point(9, 46);
		this.按目标站编号顺序.Name = "按目标站编号顺序";
		this.按目标站编号顺序.Size = new System.Drawing.Size(119, 16);
		this.按目标站编号顺序.TabIndex = 10;
		this.按目标站编号顺序.TabStop = true;
		this.按目标站编号顺序.Text = "按目标站编号顺序";
		this.按目标站编号顺序.UseVisualStyleBackColor = true;
		this.更新生成进度groupBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.更新生成进度groupBox.Controls.Add(this.label_0);
		this.更新生成进度groupBox.Controls.Add(this.label_1);
		this.更新生成进度groupBox.Location = new System.Drawing.Point(12, 210);
		this.更新生成进度groupBox.Name = "更新生成进度groupBox";
		this.更新生成进度groupBox.Size = new System.Drawing.Size(381, 85);
		this.更新生成进度groupBox.TabIndex = 11;
		this.更新生成进度groupBox.TabStop = false;
		this.更新生成进度groupBox.Text = "生成进度";
		this.label_0.AutoSize = true;
		this.label_0.Location = new System.Drawing.Point(77, 21);
		this.label_0.Name = "label_0";
		this.label_0.Size = new System.Drawing.Size(17, 12);
		this.label_0.TabIndex = 14;
		this.label_0.Text = "--";
		this.label_1.AutoSize = true;
		this.label_1.Location = new System.Drawing.Point(6, 21);
		this.label_1.Name = "label_1";
		this.label_1.Size = new System.Drawing.Size(65, 12);
		this.label_1.TabIndex = 11;
		this.label_1.Text = "当前小说：";
		this.button_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.button_0.Location = new System.Drawing.Point(205, 300);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 21);
		this.button_0.TabIndex = 19;
		this.button_0.Text = "停止";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(button_0_Click);
		this.button_1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
		this.button_1.Location = new System.Drawing.Point(124, 301);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(75, 21);
		this.button_1.TabIndex = 18;
		this.button_1.Text = "开始";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.更新设置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.更新设置.Controls.Add(this.更新循环搜索时间);
		this.更新设置.Controls.Add(this.label1);
		this.更新设置.Controls.Add(this.规则选项comboBox);
		this.更新设置.Controls.Add(this.按本站编号顺序);
		this.更新设置.Controls.Add(this.按目标站编号顺序);
		this.更新设置.Controls.Add(this.更新作者checkBox);
		this.更新设置.Controls.Add(this.更新大类checkBox);
		this.更新设置.Controls.Add(this.更新小类checkBox);
		this.更新设置.Controls.Add(this.本站结束ID);
		this.更新设置.Controls.Add(this.更新写作进程checkBox);
		this.更新设置.Controls.Add(this.目标站结束ID);
		this.更新设置.Controls.Add(this.本站开始ID);
		this.更新设置.Controls.Add(this.更新简介checkBox);
		this.更新设置.Controls.Add(this.目标站起ID);
		this.更新设置.Controls.Add(this.更新关键词checkBox);
		this.更新设置.Controls.Add(this.更新封面checkBox);
		this.更新设置.Location = new System.Drawing.Point(12, 12);
		this.更新设置.Name = "更新设置";
		this.更新设置.Size = new System.Drawing.Size(381, 192);
		this.更新设置.TabIndex = 20;
		this.更新设置.TabStop = false;
		this.更新设置.Text = "更新设置";
		this.更新循环搜索时间.Location = new System.Drawing.Point(226, 117);
		this.更新循环搜索时间.Name = "更新循环搜索时间";
		this.更新循环搜索时间.Size = new System.Drawing.Size(102, 21);
		this.更新循环搜索时间.TabIndex = 33;
		this.更新循环搜索时间.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(227, 97);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(101, 12);
		this.label1.TabIndex = 32;
		this.label1.Text = "循环搜索间隔(秒)";
		this.规则选项comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.规则选项comboBox.FormattingEnabled = true;
		this.规则选项comboBox.Location = new System.Drawing.Point(9, 20);
		this.规则选项comboBox.Name = "规则选项comboBox";
		this.规则选项comboBox.Size = new System.Drawing.Size(210, 20);
		this.规则选项comboBox.TabIndex = 31;
		this.按本站编号顺序.AutoSize = true;
		this.按本站编号顺序.Checked = true;
		this.按本站编号顺序.Location = new System.Drawing.Point(9, 95);
		this.按本站编号顺序.Name = "按本站编号顺序";
		this.按本站编号顺序.Size = new System.Drawing.Size(107, 16);
		this.按本站编号顺序.TabIndex = 10;
		this.按本站编号顺序.TabStop = true;
		this.按本站编号顺序.Text = "按本站编号顺序";
		this.按本站编号顺序.UseVisualStyleBackColor = true;
		this.本站结束ID.Location = new System.Drawing.Point(115, 117);
		this.本站结束ID.Name = "本站结束ID";
		this.本站结束ID.Size = new System.Drawing.Size(104, 21);
		this.本站结束ID.TabIndex = 9;
		this.本站结束ID.Text = "1";
		this.本站开始ID.Location = new System.Drawing.Point(9, 117);
		this.本站开始ID.Name = "本站开始ID";
		this.本站开始ID.Size = new System.Drawing.Size(100, 21);
		this.本站开始ID.TabIndex = 8;
		this.本站开始ID.Text = "1";
		this.backgroundWorker_0.WorkerReportsProgress = true;
		this.backgroundWorker_0.WorkerSupportsCancellation = true;
		this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
		this.backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_0_ProgressChanged);
		this.backgroundWorker_0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_0_RunWorkerCompleted);
		base.ClientSize = new System.Drawing.Size(405, 334);
		base.Controls.Add(this.更新设置);
		base.Controls.Add(this.button_0);
		base.Controls.Add(this.更新生成进度groupBox);
		base.Controls.Add(this.button_1);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "HelpUpdateNovel";
		this.Text = "更新小说信息";
		base.Load += new System.EventHandler(HelpUpdateNovel_Load);
		this.更新生成进度groupBox.ResumeLayout(false);
		this.更新生成进度groupBox.PerformLayout();
		this.更新设置.ResumeLayout(false);
		this.更新设置.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.更新循环搜索时间).EndInit();
		base.ResumeLayout(false);
	}
}
