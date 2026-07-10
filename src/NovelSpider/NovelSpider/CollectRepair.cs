using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;
using NovelSpider.Local;
using NovelSpider.Target;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class CollectRepair : DockContent
{
	private BackgroundWorker backgroundWorker_0;

	private bool bool_0;

	private Button button_0;

	private Button button_1;

	private Button button_2;

	private CheckBox checkBox_0;

	private CheckBox checkBox_12;

	private CheckBox checkBox_13;

	private CheckBox checkBox_14;

	private CheckBox checkBox_16;

	private CheckBox checkBox_17;

	private CheckBox checkBox_21;

	private CheckBox checkBox_22;

	private CheckBox checkBox_4;

	private CheckBox checkBox_5;

	private CheckBox checkBox_6;

	private CheckBox checkBox_8;

	private CheckBox checkBox_9;

	private ComboBox comboBox_0;

	private ComboBox comboBox_1;

	private ComboBox comboBox_2;

	private ComboBox comboBox_5;

	private ComboBox comboBox_7;

	private ComboBox comboBox1;

	private IContainer components;

	private DateTime dateTime_0 = DateTime.Now;

	private DateTimePicker dateTimePicker_0;

	private DateTimePicker dateTimePicker_1;

	private CheckBox DelForTxt;

	private CheckBox DelForTxtHtml;

	private NumericUpDown DonotCollectChapterNum;

	private TextBox FilterNovel;

	private TextBox FilterVolume;

	private GroupBox groupBox_0;

	private GroupBox groupBox_1;

	private GroupBox groupBox_2;

	private GroupBox groupBox_3;

	private GroupBox groupBox_4;

	private GroupBox groupBox_7;

	private GroupBox groupBox_8;

	private GroupBox groupBox_9;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private GroupBox groupBox3;

	private IContainer icontainer_0;

	private Label label_1;

	private Label label_10;

	private Label label_11;

	private Label label_12;

	private Label label_13;

	private Label label_14;

	private Label label_15;

	private Label label_16;

	private Label label_17;

	private Label label_18;

	private Label label_19;

	private Label label_2;

	private Label label_20;

	private Label label_21;

	private Label label_22;

	private Label label_25;

	private Label label_27;

	private Label label_3;

	private Label label_32;

	private Label label_4;

	private Label label_5;

	private Label label_6;

	private Label label_7;

	private Label label_8;

	private Label label_9;

	private Label label1;

	private Label label10;

	private Label label11;

	private Label label12;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label label9;

	private NumericUpDown numericUpDown_0;

	private NumericUpDown numericUpDown_1;

	private NumericUpDown numericUpDown_2;

	private NumericUpDown numericUpDown_3;

	private NumericUpDown numericUpDown_4;

	private NumericUpDown numericUpDown_5;

	private NumericUpDown numericUpDown1;

	private ProgressBar progressBar_0;

	private ProgressBar progressBar_1;

	private CheckBox ReplaceChapterNameOn;

	private RichTextBox richTextBox_0;

	public RuleConfigInfo rInfo = new RuleConfigInfo();

	private string string_2 = "";

	private TabControl tabControl_0;

	private TabPage tabPage_1;

	private TabPage tabPage_2;

	private TabPage tabPage_4;

	private TabPage tabPage1;

	private TextBox textBox_12;

	private TextBox textBox_13;

	private TextBox textBox_14;

	private TextBox textBox_15;

	private TextBox textBox_16;

	private System.Windows.Forms.Timer timer_0;

	public RepairConfigInfo tInfo = new RepairConfigInfo();

	public TaskConfigInfo uInfo = new TaskConfigInfo();

	public CollectRepair()
	{
		InitializeComponent();
	}

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		backgroundWorker_0.ReportProgress(2, "获得错误日志小说列表");
		int num = 0;
		string string_ = "";
		if (num == 0)
		{
			string[] array = IO.LoadLogs();
			if (array.Length != 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (backgroundWorker_0.CancellationPending)
					{
						break;
					}
					if (array[i].EndsWith("db3") && !array[i].EndsWith("BaiduPush.db3"))
					{
						collectStart(array[i], bool_1: false);
					}
				}
			}
		}
		else
		{
			collectStart(string_, bool_1: false);
		}
		if (backgroundWorker_0.CancellationPending)
		{
			e.Cancel = true;
		}
	}

	private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		if (Configs.CmdModel)
		{
			switch (e.ProgressPercentage)
			{
			case 0:
				Console.WriteLine("当前小说：" + e.UserState.ToString());
				break;
			case 1:
				Console.WriteLine("当前章节：" + e.UserState.ToString());
				break;
			case 2:
				Console.WriteLine("当前动作：" + e.UserState.ToString());
				break;
			}
		}
		switch (e.ProgressPercentage)
		{
		case 0:
			label_17.Text = e.UserState.ToString();
			break;
		case 1:
			label_16.Text = e.UserState.ToString();
			break;
		case 2:
			label_15.Text = e.UserState.ToString();
			break;
		case 3:
			if (!tInfo.NoPBar)
			{
				int num = Convert.ToInt32(e.UserState);
				if (num <= progressBar_1.Maximum && num >= progressBar_1.Minimum)
				{
					progressBar_1.Value = Convert.ToInt32(e.UserState);
				}
			}
			break;
		case 4:
			if (!tInfo.NoPBar)
			{
				int num2 = Convert.ToInt32(e.UserState);
				if (num2 <= progressBar_0.Maximum && num2 >= progressBar_0.Minimum)
				{
					progressBar_0.Value = Convert.ToInt32(e.UserState);
				}
			}
			break;
		case 5:
			if (!tInfo.NoPBar)
			{
				progressBar_1.Maximum = Convert.ToInt32(e.UserState);
			}
			break;
		case 6:
			if (!tInfo.NoPBar)
			{
				progressBar_0.Maximum = Convert.ToInt32(e.UserState);
			}
			break;
		case 8:
		{
			if (tInfo.Log)
			{
				richTextBox_0.AppendText(e.UserState.ToString() + "\n");
				richTextBox_0.Focus();
				richTextBox_0.Select(richTextBox_0.TextLength, 0);
				richTextBox_0.ScrollToCaret();
			}
			string[] array2 = e.UserState.ToString().Split('|');
			if (array2.Length == 5)
			{
				string string_ = "Data Source=" + array2[1].Trim();
				string string_2 = "update [TaskLog] set ERROROK=1,ERRORTEXT='已修复' Where GETID='" + array2[3].Trim() + "' And RULEFILE='" + array2[2].Trim() + "'";
				SQLiteHelper.ExecuteNonQuery(string_, string_2);
			}
			break;
		}
		case 9:
		{
			if (tInfo.Log)
			{
				richTextBox_0.AppendText(e.UserState.ToString() + "\n");
				richTextBox_0.Focus();
				richTextBox_0.Select(richTextBox_0.TextLength, 0);
				richTextBox_0.ScrollToCaret();
			}
			string[] array = e.UserState.ToString().Split('|');
			if (array.Length == 5)
			{
				string string_ = "Data Source=" + array[1].Trim();
				string string_2 = "update [TaskLog] set ERRORNUM=ERRORNUM+1,ERRORTEXT='未修复' Where GETID='" + array[3].Trim() + "' And RULEFILE='" + array[2].Trim() + "'";
				SQLiteHelper.ExecuteNonQuery(string_, string_2);
			}
			break;
		}
		case 101:
			label7.Text = e.UserState.ToString();
			break;
		case 102:
			label5.Text = e.UserState.ToString();
			break;
		case 103:
			label9.Text = e.UserState.ToString();
			break;
		}
	}

	private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			SpiderException.Debug(tInfo.ID, "严重错误：" + e.Error.Message);
			if (Configs.CmdModel)
			{
				Console.WriteLine("严重错误：" + e.Error.Message);
			}
			if (tInfo.Timing)
			{
				if (tInfo.Log)
				{
					label_15.Text = "错误提示：" + e.Error.Message;
					timer_0.Start();
					dateTime_0 = DateTime.Now.AddMinutes(tInfo.Interval);
				}
				else
				{
					MessageForm messageForm = new MessageForm
					{
						Text = "错误提示"
					};
					MessageForm messageForm2 = messageForm;
					messageForm2.MessageText.Text = e.Error.Message;
					messageForm2.ShowDialog();
					button_1.Enabled = true;
					button_0.Enabled = false;
				}
			}
			else
			{
				MessageForm messageForm3 = new MessageForm
				{
					Text = "错误提示"
				};
				MessageForm messageForm4 = messageForm3;
				messageForm4.MessageText.Text = e.Error.Message;
				messageForm4.ShowDialog();
				button_1.Enabled = true;
				button_0.Enabled = false;
			}
		}
		else if (e.Cancelled)
		{
			label_15.Text = "操作取消";
			button_1.Enabled = true;
			button_0.Enabled = false;
		}
		else if (tInfo.Timing)
		{
			timer_0.Start();
			dateTime_0 = DateTime.Now.AddMinutes(tInfo.Interval);
		}
		else
		{
			label_15.Text = "操作完成";
			button_1.Enabled = true;
			button_0.Enabled = false;
		}
		Configs.TaskNovelInfo[string_2.ToString()] = null;
	}

	private void button_0_Click(object sender, EventArgs e)
	{
		button_0.Enabled = false;
		if (backgroundWorker_0.IsBusy)
		{
			backgroundWorker_0.CancelAsync();
			return;
		}
		if (timer_0.Enabled)
		{
			timer_0.Stop();
			label_15.Text = "操作取消";
		}
		button_1.Enabled = true;
	}

	private void button_1_Click(object sender, EventArgs e)
	{
		MessageBox.Show("功能暂未开放！");
	}

	private void button_2_Click(object sender, EventArgs e)
	{
		writeConfigToFiles();
		ConfigFileManager.SaveConfig("ReplaceConfig.xml", tInfo);
	}

	private void CollectReplace_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (backgroundWorker_0.IsBusy)
		{
			backgroundWorker_0.CancelAsync();
			e.Cancel = true;
			MessageBox.Show("检查到采集进程正在运行，现在正在自动停止采集进程\n\n请在采集进程停止后关闭窗口！", "信息提示");
		}
		else
		{
			Configs.TaskNovelInfo.Remove(string_2);
		}
	}

	private void CollectReplace_Load(object sender, EventArgs e)
	{
		string_2 = Guid.NewGuid().ToString().ToUpper();
		Configs.TaskNovelInfo.Add(string_2, null);
		string text = Text + " " + string_2;
		Text = text;
		tInfo = (RepairConfigInfo)ConfigFileManager.LoadConfig("RepairConfig.xml", tInfo);
		comboBox_0.BeginUpdate();
		comboBox_0.Items.Add("循环执行所有采集错误日志");
		string[] array = IO.LoadLogs();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].EndsWith("db3") && !array[i].EndsWith("BaiduPush.db3"))
				{
					comboBox_0.Items.Add(array[i]);
				}
			}
		}
		comboBox_0.EndUpdate();
		comboBox_0.SelectedIndex = 0;
		loadConfigs();
	}

	private void collectStart(string string_3, bool bool_1)
	{
		backgroundWorker_0.ReportProgress(101, string_3);
		FileInfo fileInfo = new FileInfo(string_3 ?? "");
		if (!fileInfo.Exists)
		{
			return;
		}
		string string_4 = "Data Source=" + fileInfo.FullName;
		string string_5 = "Select * From [TaskLog] Where GETID<>'0' And GETID<>'' And RULEFILE<>''";
		DataSet dataSet = SQLiteHelper.ExecuteDataset(string_4, string_5);
		if (dataSet == null || dataSet.Tables[0].Rows.Count < 1)
		{
			return;
		}
		backgroundWorker_0.ReportProgress(5, dataSet.Tables[0].Rows.Count);
		for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
		{
			if (backgroundWorker_0.CancellationPending)
			{
				break;
			}
			backgroundWorker_0.ReportProgress(3, i + 1);
			backgroundWorker_0.ReportProgress(102, (i + 1).ToString() + " / " + dataSet.Tables[0].Rows.Count);
			NovelInfo novelInfo = new NovelInfo
			{
				GetID = dataSet.Tables[0].Rows[i]["GETID"].ToString(),
				RuleName = dataSet.Tables[0].Rows[i]["RULEFILE"].ToString()
			};
			NovelInfo novelInfo2 = novelInfo;
			backgroundWorker_0.ReportProgress(103, novelInfo2.RuleName);
			try
			{
				rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(novelInfo2.RuleName, rInfo);
			}
			catch
			{
				backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + novelInfo2.RuleName + " | " + novelInfo2.GetID + " | 无法载入。");
			}
			repairCollect(novelInfo2, string_3, novelInfo2.RuleName);
		}
	}

	private void comboBox_0_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!bool_0)
		{
			if (comboBox_0.SelectedIndex == 0)
			{
				Text = "多日志 超级修复模式";
			}
			else
			{
				Text = comboBox_0.SelectedItem.ToString() + " 超级修复模式";
			}
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.CollectRepair));
		this.comboBox_0 = new System.Windows.Forms.ComboBox();
		this.button_0 = new System.Windows.Forms.Button();
		this.button_1 = new System.Windows.Forms.Button();
		this.button_2 = new System.Windows.Forms.Button();
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		this.timer_0 = new System.Windows.Forms.Timer(this.components);
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.richTextBox_0 = new System.Windows.Forms.RichTextBox();
		this.groupBox_7 = new System.Windows.Forms.GroupBox();
		this.checkBox_12 = new System.Windows.Forms.CheckBox();
		this.label_6 = new System.Windows.Forms.Label();
		this.textBox_12 = new System.Windows.Forms.TextBox();
		this.label_7 = new System.Windows.Forms.Label();
		this.label_8 = new System.Windows.Forms.Label();
		this.textBox_13 = new System.Windows.Forms.TextBox();
		this.textBox_14 = new System.Windows.Forms.TextBox();
		this.textBox_15 = new System.Windows.Forms.TextBox();
		this.textBox_16 = new System.Windows.Forms.TextBox();
		this.tabPage_4 = new System.Windows.Forms.TabPage();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.checkBox_13 = new System.Windows.Forms.CheckBox();
		this.label_22 = new System.Windows.Forms.Label();
		this.progressBar_1 = new System.Windows.Forms.ProgressBar();
		this.label_21 = new System.Windows.Forms.Label();
		this.progressBar_0 = new System.Windows.Forms.ProgressBar();
		this.groupBox_9 = new System.Windows.Forms.GroupBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label_15 = new System.Windows.Forms.Label();
		this.label_16 = new System.Windows.Forms.Label();
		this.label_17 = new System.Windows.Forms.Label();
		this.label_18 = new System.Windows.Forms.Label();
		this.label_19 = new System.Windows.Forms.Label();
		this.label_20 = new System.Windows.Forms.Label();
		this.groupBox_8 = new System.Windows.Forms.GroupBox();
		this.label_9 = new System.Windows.Forms.Label();
		this.label_10 = new System.Windows.Forms.Label();
		this.label_11 = new System.Windows.Forms.Label();
		this.label_12 = new System.Windows.Forms.Label();
		this.numericUpDown_3 = new System.Windows.Forms.NumericUpDown();
		this.label_13 = new System.Windows.Forms.Label();
		this.numericUpDown_4 = new System.Windows.Forms.NumericUpDown();
		this.label_14 = new System.Windows.Forms.Label();
		this.numericUpDown_5 = new System.Windows.Forms.NumericUpDown();
		this.tabPage_2 = new System.Windows.Forms.TabPage();
		this.groupBox_4 = new System.Windows.Forms.GroupBox();
		this.FilterVolume = new System.Windows.Forms.TextBox();
		this.groupBox_3 = new System.Windows.Forms.GroupBox();
		this.FilterNovel = new System.Windows.Forms.TextBox();
		this.comboBox_2 = new System.Windows.Forms.ComboBox();
		this.groupBox_2 = new System.Windows.Forms.GroupBox();
		this.label3 = new System.Windows.Forms.Label();
		this.DonotCollectChapterNum = new System.Windows.Forms.NumericUpDown();
		this.label4 = new System.Windows.Forms.Label();
		this.label_2 = new System.Windows.Forms.Label();
		this.numericUpDown_1 = new System.Windows.Forms.NumericUpDown();
		this.label_3 = new System.Windows.Forms.Label();
		this.label_4 = new System.Windows.Forms.Label();
		this.label_5 = new System.Windows.Forms.Label();
		this.numericUpDown_2 = new System.Windows.Forms.NumericUpDown();
		this.tabPage_1 = new System.Windows.Forms.TabPage();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.checkBox_0 = new System.Windows.Forms.CheckBox();
		this.ReplaceChapterNameOn = new System.Windows.Forms.CheckBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
		this.groupBox_1 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.label_32 = new System.Windows.Forms.Label();
		this.comboBox_7 = new System.Windows.Forms.ComboBox();
		this.label_27 = new System.Windows.Forms.Label();
		this.comboBox_5 = new System.Windows.Forms.ComboBox();
		this.label2 = new System.Windows.Forms.Label();
		this.comboBox_1 = new System.Windows.Forms.ComboBox();
		this.groupBox_0 = new System.Windows.Forms.GroupBox();
		this.DelForTxtHtml = new System.Windows.Forms.CheckBox();
		this.checkBox_22 = new System.Windows.Forms.CheckBox();
		this.checkBox_21 = new System.Windows.Forms.CheckBox();
		this.checkBox_17 = new System.Windows.Forms.CheckBox();
		this.checkBox_16 = new System.Windows.Forms.CheckBox();
		this.DelForTxt = new System.Windows.Forms.CheckBox();
		this.checkBox_8 = new System.Windows.Forms.CheckBox();
		this.checkBox_9 = new System.Windows.Forms.CheckBox();
		this.checkBox_4 = new System.Windows.Forms.CheckBox();
		this.dateTimePicker_1 = new System.Windows.Forms.DateTimePicker();
		this.label_25 = new System.Windows.Forms.Label();
		this.checkBox_14 = new System.Windows.Forms.CheckBox();
		this.dateTimePicker_0 = new System.Windows.Forms.DateTimePicker();
		this.checkBox_5 = new System.Windows.Forms.CheckBox();
		this.label_1 = new System.Windows.Forms.Label();
		this.checkBox_6 = new System.Windows.Forms.CheckBox();
		this.numericUpDown_0 = new System.Windows.Forms.NumericUpDown();
		this.tabControl_0 = new System.Windows.Forms.TabControl();
		this.tabPage1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox_7.SuspendLayout();
		this.tabPage_4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox_9.SuspendLayout();
		this.groupBox_8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_5).BeginInit();
		this.tabPage_2.SuspendLayout();
		this.groupBox_4.SuspendLayout();
		this.groupBox_3.SuspendLayout();
		this.groupBox_2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DonotCollectChapterNum).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_2).BeginInit();
		this.tabPage_1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).BeginInit();
		this.groupBox_1.SuspendLayout();
		this.groupBox_0.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).BeginInit();
		this.tabControl_0.SuspendLayout();
		base.SuspendLayout();
		this.comboBox_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.comboBox_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_0.FormattingEnabled = true;
		this.comboBox_0.Location = new System.Drawing.Point(12, 376);
		this.comboBox_0.Name = "comboBox_0";
		this.comboBox_0.Size = new System.Drawing.Size(261, 20);
		this.comboBox_0.TabIndex = 31;
		this.comboBox_0.SelectedIndexChanged += new System.EventHandler(comboBox_0_SelectedIndexChanged);
		this.button_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button_0.Enabled = false;
		this.button_0.Location = new System.Drawing.Point(666, 374);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 23);
		this.button_0.TabIndex = 47;
		this.button_0.Text = "停止";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(button_0_Click);
		this.button_1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button_1.Location = new System.Drawing.Point(585, 374);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(75, 23);
		this.button_1.TabIndex = 46;
		this.button_1.Text = "开始";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.button_2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button_2.Location = new System.Drawing.Point(747, 374);
		this.button_2.Name = "button_2";
		this.button_2.Size = new System.Drawing.Size(75, 23);
		this.button_2.TabIndex = 49;
		this.button_2.Text = "保存设置";
		this.button_2.UseVisualStyleBackColor = true;
		this.button_2.Click += new System.EventHandler(button_2_Click);
		this.backgroundWorker_0.WorkerReportsProgress = true;
		this.backgroundWorker_0.WorkerSupportsCancellation = true;
		this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
		this.backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_0_ProgressChanged);
		this.backgroundWorker_0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_0_RunWorkerCompleted);
		this.timer_0.Interval = 1000;
		this.timer_0.Tick += new System.EventHandler(timer_0_Tick);
		this.tabPage1.Controls.Add(this.groupBox1);
		this.tabPage1.Controls.Add(this.groupBox_7);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(802, 330);
		this.tabPage1.TabIndex = 5;
		this.tabPage1.Text = "代理/记录";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.Controls.Add(this.richTextBox_0);
		this.groupBox1.Location = new System.Drawing.Point(6, 116);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(790, 204);
		this.groupBox1.TabIndex = 6;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "修复记录";
		this.richTextBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.richTextBox_0.BackColor = System.Drawing.SystemColors.Window;
		this.richTextBox_0.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.richTextBox_0.Location = new System.Drawing.Point(6, 20);
		this.richTextBox_0.Name = "richTextBox_0";
		this.richTextBox_0.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
		this.richTextBox_0.Size = new System.Drawing.Size(778, 176);
		this.richTextBox_0.TabIndex = 18;
		this.richTextBox_0.Text = "";
		this.groupBox_7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_7.Controls.Add(this.checkBox_12);
		this.groupBox_7.Controls.Add(this.label_6);
		this.groupBox_7.Controls.Add(this.textBox_12);
		this.groupBox_7.Controls.Add(this.label_7);
		this.groupBox_7.Controls.Add(this.label_8);
		this.groupBox_7.Controls.Add(this.textBox_13);
		this.groupBox_7.Controls.Add(this.textBox_14);
		this.groupBox_7.Controls.Add(this.textBox_15);
		this.groupBox_7.Controls.Add(this.textBox_16);
		this.groupBox_7.Location = new System.Drawing.Point(6, 6);
		this.groupBox_7.Name = "groupBox_7";
		this.groupBox_7.Size = new System.Drawing.Size(790, 104);
		this.groupBox_7.TabIndex = 5;
		this.groupBox_7.TabStop = false;
		this.groupBox_7.Text = "代理IP";
		this.checkBox_12.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.checkBox_12.AutoSize = true;
		this.checkBox_12.Location = new System.Drawing.Point(666, 50);
		this.checkBox_12.Name = "checkBox_12";
		this.checkBox_12.Size = new System.Drawing.Size(96, 16);
		this.checkBox_12.TabIndex = 20;
		this.checkBox_12.Text = "启用代理功能";
		this.checkBox_12.UseVisualStyleBackColor = true;
		this.label_6.AutoSize = true;
		this.label_6.Location = new System.Drawing.Point(30, 50);
		this.label_6.Name = "label_6";
		this.label_6.Size = new System.Drawing.Size(53, 12);
		this.label_6.TabIndex = 19;
		this.label_6.Text = "代理域：";
		this.textBox_12.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_12.Location = new System.Drawing.Point(89, 47);
		this.textBox_12.Name = "textBox_12";
		this.textBox_12.Size = new System.Drawing.Size(571, 21);
		this.textBox_12.TabIndex = 17;
		this.label_7.AutoSize = true;
		this.label_7.Location = new System.Drawing.Point(18, 77);
		this.label_7.Name = "label_7";
		this.label_7.Size = new System.Drawing.Size(65, 12);
		this.label_7.TabIndex = 16;
		this.label_7.Text = "帐户密码：";
		this.label_8.AutoSize = true;
		this.label_8.Location = new System.Drawing.Point(18, 23);
		this.label_8.Name = "label_8";
		this.label_8.Size = new System.Drawing.Size(65, 12);
		this.label_8.TabIndex = 15;
		this.label_8.Text = "ＩＰ端口：";
		this.textBox_13.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_13.Location = new System.Drawing.Point(666, 74);
		this.textBox_13.Name = "textBox_13";
		this.textBox_13.Size = new System.Drawing.Size(118, 21);
		this.textBox_13.TabIndex = 12;
		this.textBox_14.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_14.Location = new System.Drawing.Point(89, 74);
		this.textBox_14.Name = "textBox_14";
		this.textBox_14.Size = new System.Drawing.Size(571, 21);
		this.textBox_14.TabIndex = 11;
		this.textBox_15.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_15.Location = new System.Drawing.Point(666, 20);
		this.textBox_15.Name = "textBox_15";
		this.textBox_15.Size = new System.Drawing.Size(118, 21);
		this.textBox_15.TabIndex = 10;
		this.textBox_15.Text = "80";
		this.textBox_16.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_16.Location = new System.Drawing.Point(89, 20);
		this.textBox_16.Name = "textBox_16";
		this.textBox_16.Size = new System.Drawing.Size(571, 21);
		this.textBox_16.TabIndex = 9;
		this.tabPage_4.Controls.Add(this.groupBox2);
		this.tabPage_4.Controls.Add(this.groupBox_9);
		this.tabPage_4.Controls.Add(this.groupBox_8);
		this.tabPage_4.Location = new System.Drawing.Point(4, 22);
		this.tabPage_4.Name = "tabPage_4";
		this.tabPage_4.Size = new System.Drawing.Size(802, 330);
		this.tabPage_4.TabIndex = 4;
		this.tabPage_4.Text = "修复进度";
		this.tabPage_4.UseVisualStyleBackColor = true;
		this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox2.Controls.Add(this.checkBox_13);
		this.groupBox2.Controls.Add(this.label_22);
		this.groupBox2.Controls.Add(this.progressBar_1);
		this.groupBox2.Controls.Add(this.label_21);
		this.groupBox2.Controls.Add(this.progressBar_0);
		this.groupBox2.Location = new System.Drawing.Point(6, 8);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(625, 100);
		this.groupBox2.TabIndex = 53;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "修复进度";
		this.checkBox_13.AutoSize = true;
		this.checkBox_13.Location = new System.Drawing.Point(83, 72);
		this.checkBox_13.Name = "checkBox_13";
		this.checkBox_13.Size = new System.Drawing.Size(96, 16);
		this.checkBox_13.TabIndex = 17;
		this.checkBox_13.Text = "不绘图进度条";
		this.checkBox_13.UseVisualStyleBackColor = true;
		this.label_22.AutoSize = true;
		this.label_22.Location = new System.Drawing.Point(6, 23);
		this.label_22.Name = "label_22";
		this.label_22.Size = new System.Drawing.Size(77, 12);
		this.label_22.TabIndex = 8;
		this.label_22.Text = "修复总进度：";
		this.progressBar_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.progressBar_1.Location = new System.Drawing.Point(83, 19);
		this.progressBar_1.Name = "progressBar_1";
		this.progressBar_1.Size = new System.Drawing.Size(536, 18);
		this.progressBar_1.TabIndex = 7;
		this.label_21.AutoSize = true;
		this.label_21.Location = new System.Drawing.Point(6, 48);
		this.label_21.Name = "label_21";
		this.label_21.Size = new System.Drawing.Size(77, 12);
		this.label_21.TabIndex = 9;
		this.label_21.Text = "修复分进度：";
		this.progressBar_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.progressBar_0.Location = new System.Drawing.Point(83, 45);
		this.progressBar_0.Name = "progressBar_0";
		this.progressBar_0.Size = new System.Drawing.Size(536, 18);
		this.progressBar_0.TabIndex = 10;
		this.groupBox_9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_9.Controls.Add(this.label9);
		this.groupBox_9.Controls.Add(this.label10);
		this.groupBox_9.Controls.Add(this.label7);
		this.groupBox_9.Controls.Add(this.label8);
		this.groupBox_9.Controls.Add(this.label5);
		this.groupBox_9.Controls.Add(this.label6);
		this.groupBox_9.Controls.Add(this.label_15);
		this.groupBox_9.Controls.Add(this.label_16);
		this.groupBox_9.Controls.Add(this.label_17);
		this.groupBox_9.Controls.Add(this.label_18);
		this.groupBox_9.Controls.Add(this.label_19);
		this.groupBox_9.Controls.Add(this.label_20);
		this.groupBox_9.Location = new System.Drawing.Point(6, 116);
		this.groupBox_9.Name = "groupBox_9";
		this.groupBox_9.Size = new System.Drawing.Size(793, 207);
		this.groupBox_9.TabIndex = 52;
		this.groupBox_9.TabStop = false;
		this.groupBox_9.Text = "操作详情";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(81, 78);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(17, 12);
		this.label9.TabIndex = 22;
		this.label9.Text = "--";
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(18, 78);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(65, 12);
		this.label10.TabIndex = 21;
		this.label10.Text = "使用规则：";
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(81, 28);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(17, 12);
		this.label7.TabIndex = 20;
		this.label7.Text = "--";
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(18, 28);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(65, 12);
		this.label8.TabIndex = 19;
		this.label8.Text = "当前日志：";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(81, 52);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(17, 12);
		this.label5.TabIndex = 18;
		this.label5.Text = "--";
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(6, 52);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(77, 12);
		this.label6.TabIndex = 17;
		this.label6.Text = "错误总数量：";
		this.label_15.AutoSize = true;
		this.label_15.Location = new System.Drawing.Point(81, 156);
		this.label_15.Name = "label_15";
		this.label_15.Size = new System.Drawing.Size(17, 12);
		this.label_15.TabIndex = 16;
		this.label_15.Text = "--";
		this.label_16.AutoSize = true;
		this.label_16.Location = new System.Drawing.Point(81, 130);
		this.label_16.Name = "label_16";
		this.label_16.Size = new System.Drawing.Size(17, 12);
		this.label_16.TabIndex = 15;
		this.label_16.Text = "--";
		this.label_17.AutoSize = true;
		this.label_17.Location = new System.Drawing.Point(81, 105);
		this.label_17.Name = "label_17";
		this.label_17.Size = new System.Drawing.Size(17, 12);
		this.label_17.TabIndex = 14;
		this.label_17.Text = "--";
		this.label_18.AutoSize = true;
		this.label_18.Location = new System.Drawing.Point(18, 156);
		this.label_18.Name = "label_18";
		this.label_18.Size = new System.Drawing.Size(65, 12);
		this.label_18.TabIndex = 13;
		this.label_18.Text = "当前动作：";
		this.label_19.AutoSize = true;
		this.label_19.Location = new System.Drawing.Point(18, 130);
		this.label_19.Name = "label_19";
		this.label_19.Size = new System.Drawing.Size(65, 12);
		this.label_19.TabIndex = 12;
		this.label_19.Text = "当前章节：";
		this.label_20.AutoSize = true;
		this.label_20.Location = new System.Drawing.Point(18, 105);
		this.label_20.Name = "label_20";
		this.label_20.Size = new System.Drawing.Size(65, 12);
		this.label_20.TabIndex = 11;
		this.label_20.Text = "当前小说：";
		this.groupBox_8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_8.Controls.Add(this.label_9);
		this.groupBox_8.Controls.Add(this.label_10);
		this.groupBox_8.Controls.Add(this.label_11);
		this.groupBox_8.Controls.Add(this.label_12);
		this.groupBox_8.Controls.Add(this.numericUpDown_3);
		this.groupBox_8.Controls.Add(this.label_13);
		this.groupBox_8.Controls.Add(this.numericUpDown_4);
		this.groupBox_8.Controls.Add(this.label_14);
		this.groupBox_8.Controls.Add(this.numericUpDown_5);
		this.groupBox_8.Location = new System.Drawing.Point(637, 6);
		this.groupBox_8.Name = "groupBox_8";
		this.groupBox_8.Size = new System.Drawing.Size(162, 104);
		this.groupBox_8.TabIndex = 6;
		this.groupBox_8.TabStop = false;
		this.groupBox_8.Text = "延时等待";
		this.label_9.AutoSize = true;
		this.label_9.Location = new System.Drawing.Point(124, 77);
		this.label_9.Name = "label_9";
		this.label_9.Size = new System.Drawing.Size(29, 12);
		this.label_9.TabIndex = 28;
		this.label_9.Text = "毫秒";
		this.label_10.AutoSize = true;
		this.label_10.Location = new System.Drawing.Point(124, 50);
		this.label_10.Name = "label_10";
		this.label_10.Size = new System.Drawing.Size(29, 12);
		this.label_10.TabIndex = 27;
		this.label_10.Text = "毫秒";
		this.label_11.AutoSize = true;
		this.label_11.Location = new System.Drawing.Point(124, 23);
		this.label_11.Name = "label_11";
		this.label_11.Size = new System.Drawing.Size(29, 12);
		this.label_11.TabIndex = 26;
		this.label_11.Text = "毫秒";
		this.label_12.AutoSize = true;
		this.label_12.Location = new System.Drawing.Point(12, 77);
		this.label_12.Name = "label_12";
		this.label_12.Size = new System.Drawing.Size(53, 12);
		this.label_12.TabIndex = 25;
		this.label_12.Text = "章节页：";
		this.numericUpDown_3.Location = new System.Drawing.Point(71, 74);
		this.numericUpDown_3.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		this.numericUpDown_3.Name = "numericUpDown_3";
		this.numericUpDown_3.Size = new System.Drawing.Size(46, 21);
		this.numericUpDown_3.TabIndex = 24;
		this.label_13.AutoSize = true;
		this.label_13.Location = new System.Drawing.Point(12, 50);
		this.label_13.Name = "label_13";
		this.label_13.Size = new System.Drawing.Size(53, 12);
		this.label_13.TabIndex = 23;
		this.label_13.Text = "目录页：";
		this.numericUpDown_4.Location = new System.Drawing.Point(71, 47);
		this.numericUpDown_4.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		this.numericUpDown_4.Name = "numericUpDown_4";
		this.numericUpDown_4.Size = new System.Drawing.Size(46, 21);
		this.numericUpDown_4.TabIndex = 22;
		this.label_14.AutoSize = true;
		this.label_14.Location = new System.Drawing.Point(12, 23);
		this.label_14.Name = "label_14";
		this.label_14.Size = new System.Drawing.Size(53, 12);
		this.label_14.TabIndex = 21;
		this.label_14.Text = "信息页：";
		this.numericUpDown_5.Location = new System.Drawing.Point(71, 20);
		this.numericUpDown_5.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		this.numericUpDown_5.Name = "numericUpDown_5";
		this.numericUpDown_5.Size = new System.Drawing.Size(46, 21);
		this.numericUpDown_5.TabIndex = 0;
		this.tabPage_2.Controls.Add(this.groupBox_4);
		this.tabPage_2.Controls.Add(this.groupBox_3);
		this.tabPage_2.Controls.Add(this.groupBox_2);
		this.tabPage_2.Location = new System.Drawing.Point(4, 22);
		this.tabPage_2.Name = "tabPage_2";
		this.tabPage_2.Size = new System.Drawing.Size(802, 330);
		this.tabPage_2.TabIndex = 2;
		this.tabPage_2.Text = "过滤设置";
		this.tabPage_2.UseVisualStyleBackColor = true;
		this.groupBox_4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_4.Controls.Add(this.FilterVolume);
		this.groupBox_4.Location = new System.Drawing.Point(199, 111);
		this.groupBox_4.Name = "groupBox_4";
		this.groupBox_4.Size = new System.Drawing.Size(597, 212);
		this.groupBox_4.TabIndex = 4;
		this.groupBox_4.TabStop = false;
		this.groupBox_4.Text = "过滤分卷";
		this.FilterVolume.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FilterVolume.Location = new System.Drawing.Point(3, 20);
		this.FilterVolume.Multiline = true;
		this.FilterVolume.Name = "FilterVolume";
		this.FilterVolume.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.FilterVolume.Size = new System.Drawing.Size(588, 186);
		this.FilterVolume.TabIndex = 0;
		this.groupBox_3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.groupBox_3.Controls.Add(this.FilterNovel);
		this.groupBox_3.Controls.Add(this.comboBox_2);
		this.groupBox_3.Location = new System.Drawing.Point(6, 111);
		this.groupBox_3.Name = "groupBox_3";
		this.groupBox_3.Size = new System.Drawing.Size(190, 212);
		this.groupBox_3.TabIndex = 3;
		this.groupBox_3.TabStop = false;
		this.groupBox_3.Text = "限制小说";
		this.FilterNovel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FilterNovel.Location = new System.Drawing.Point(8, 46);
		this.FilterNovel.Multiline = true;
		this.FilterNovel.Name = "FilterNovel";
		this.FilterNovel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.FilterNovel.Size = new System.Drawing.Size(176, 160);
		this.FilterNovel.TabIndex = 8;
		this.comboBox_2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboBox_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_2.FormattingEnabled = true;
		this.comboBox_2.Items.AddRange(new object[3] { "不限制", "不采集限制小说", "只采集限制小说" });
		this.comboBox_2.Location = new System.Drawing.Point(8, 20);
		this.comboBox_2.Name = "comboBox_2";
		this.comboBox_2.Size = new System.Drawing.Size(176, 20);
		this.comboBox_2.TabIndex = 7;
		this.groupBox_2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_2.Controls.Add(this.label3);
		this.groupBox_2.Controls.Add(this.DonotCollectChapterNum);
		this.groupBox_2.Controls.Add(this.label4);
		this.groupBox_2.Controls.Add(this.label_2);
		this.groupBox_2.Controls.Add(this.numericUpDown_1);
		this.groupBox_2.Controls.Add(this.label_3);
		this.groupBox_2.Controls.Add(this.label_4);
		this.groupBox_2.Controls.Add(this.label_5);
		this.groupBox_2.Controls.Add(this.numericUpDown_2);
		this.groupBox_2.Location = new System.Drawing.Point(6, 6);
		this.groupBox_2.Name = "groupBox_2";
		this.groupBox_2.Size = new System.Drawing.Size(790, 99);
		this.groupBox_2.TabIndex = 1;
		this.groupBox_2.TabStop = false;
		this.groupBox_2.Text = "章节限制";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(188, 73);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(197, 12);
		this.label3.TabIndex = 9;
		this.label3.Text = "字的章节(根据目标站章节字节总数)";
		this.DonotCollectChapterNum.Location = new System.Drawing.Point(125, 71);
		this.DonotCollectChapterNum.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.DonotCollectChapterNum.Name = "DonotCollectChapterNum";
		this.DonotCollectChapterNum.Size = new System.Drawing.Size(57, 21);
		this.DonotCollectChapterNum.TabIndex = 7;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(6, 73);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(113, 12);
		this.label4.TabIndex = 8;
		this.label4.Text = "不替换章节字数小于";
		this.label_2.AutoSize = true;
		this.label_2.Location = new System.Drawing.Point(212, 49);
		this.label_2.Name = "label_2";
		this.label_2.Size = new System.Drawing.Size(161, 12);
		this.label_2.TabIndex = 6;
		this.label_2.Text = "的小说(根据目标站章节总数)";
		this.numericUpDown_1.Location = new System.Drawing.Point(149, 47);
		this.numericUpDown_1.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown_1.Name = "numericUpDown_1";
		this.numericUpDown_1.Size = new System.Drawing.Size(57, 21);
		this.numericUpDown_1.TabIndex = 1;
		this.label_3.AutoSize = true;
		this.label_3.Location = new System.Drawing.Point(6, 49);
		this.label_3.Name = "label_3";
		this.label_3.Size = new System.Drawing.Size(137, 12);
		this.label_3.TabIndex = 4;
		this.label_3.Text = "不修复需要更新章节超过";
		this.label_4.AutoSize = true;
		this.label_4.Location = new System.Drawing.Point(176, 25);
		this.label_4.Name = "label_4";
		this.label_4.Size = new System.Drawing.Size(161, 12);
		this.label_4.TabIndex = 3;
		this.label_4.Text = "的小说(根据目标站章节总数)";
		this.label_5.AutoSize = true;
		this.label_5.Location = new System.Drawing.Point(6, 25);
		this.label_5.Name = "label_5";
		this.label_5.Size = new System.Drawing.Size(101, 12);
		this.label_5.TabIndex = 2;
		this.label_5.Text = "不修复章节数小于";
		this.numericUpDown_2.Location = new System.Drawing.Point(113, 20);
		this.numericUpDown_2.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown_2.Name = "numericUpDown_2";
		this.numericUpDown_2.Size = new System.Drawing.Size(57, 21);
		this.numericUpDown_2.TabIndex = 0;
		this.tabPage_1.Controls.Add(this.groupBox3);
		this.tabPage_1.Controls.Add(this.groupBox_1);
		this.tabPage_1.Controls.Add(this.groupBox_0);
		this.tabPage_1.Controls.Add(this.checkBox_5);
		this.tabPage_1.Controls.Add(this.label_1);
		this.tabPage_1.Controls.Add(this.checkBox_6);
		this.tabPage_1.Controls.Add(this.numericUpDown_0);
		this.tabPage_1.Location = new System.Drawing.Point(4, 22);
		this.tabPage_1.Name = "tabPage_1";
		this.tabPage_1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage_1.Size = new System.Drawing.Size(802, 330);
		this.tabPage_1.TabIndex = 1;
		this.tabPage_1.Text = "修复设置";
		this.tabPage_1.UseVisualStyleBackColor = true;
		this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox3.Controls.Add(this.checkBox_0);
		this.groupBox3.Controls.Add(this.ReplaceChapterNameOn);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.numericUpDown1);
		this.groupBox3.Location = new System.Drawing.Point(6, 232);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(790, 63);
		this.groupBox3.TabIndex = 51;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "高级设置";
		this.checkBox_0.AutoSize = true;
		this.checkBox_0.Location = new System.Drawing.Point(231, 24);
		this.checkBox_0.Name = "checkBox_0";
		this.checkBox_0.Size = new System.Drawing.Size(282, 16);
		this.checkBox_0.TabIndex = 32;
		this.checkBox_0.Text = "自动对比章节MD5（如果章节内容相同则不修复）";
		this.checkBox_0.UseVisualStyleBackColor = true;
		this.ReplaceChapterNameOn.AutoSize = true;
		this.ReplaceChapterNameOn.ForeColor = System.Drawing.SystemColors.HotTrack;
		this.ReplaceChapterNameOn.Location = new System.Drawing.Point(521, 24);
		this.ReplaceChapterNameOn.Name = "ReplaceChapterNameOn";
		this.ReplaceChapterNameOn.Size = new System.Drawing.Size(228, 16);
		this.ReplaceChapterNameOn.TabIndex = 31;
		this.ReplaceChapterNameOn.Text = "使用修正列表规则（突破列表防采集）";
		this.ReplaceChapterNameOn.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(176, 25);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(41, 12);
		this.label11.TabIndex = 6;
		this.label11.Text = "的小说";
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(6, 25);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(113, 12);
		this.label12.TabIndex = 5;
		this.label12.Text = "只修复错误次数小于";
		this.numericUpDown1.Location = new System.Drawing.Point(125, 23);
		this.numericUpDown1.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown1.Name = "numericUpDown1";
		this.numericUpDown1.Size = new System.Drawing.Size(45, 21);
		this.numericUpDown1.TabIndex = 4;
		this.groupBox_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_1.Controls.Add(this.label1);
		this.groupBox_1.Controls.Add(this.comboBox1);
		this.groupBox_1.Controls.Add(this.label_32);
		this.groupBox_1.Controls.Add(this.comboBox_7);
		this.groupBox_1.Controls.Add(this.label_27);
		this.groupBox_1.Controls.Add(this.comboBox_5);
		this.groupBox_1.Controls.Add(this.label2);
		this.groupBox_1.Controls.Add(this.comboBox_1);
		this.groupBox_1.Location = new System.Drawing.Point(6, 128);
		this.groupBox_1.Name = "groupBox_1";
		this.groupBox_1.Size = new System.Drawing.Size(790, 98);
		this.groupBox_1.TabIndex = 50;
		this.groupBox_1.TabStop = false;
		this.groupBox_1.Text = "其他设置";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(6, 55);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(149, 12);
		this.label1.TabIndex = 31;
		this.label1.Text = "目标站重复章节判断方式：";
		this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Items.AddRange(new object[2] { "停止本书，跳入下一本", "跳过本章，继续采集下一个章" });
		this.comboBox1.Location = new System.Drawing.Point(221, 70);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(207, 20);
		this.comboBox1.TabIndex = 30;
		this.label_32.AutoSize = true;
		this.label_32.Location = new System.Drawing.Point(219, 17);
		this.label_32.Name = "label_32";
		this.label_32.Size = new System.Drawing.Size(89, 12);
		this.label_32.TabIndex = 29;
		this.label_32.Text = "章节排序方式：";
		this.comboBox_7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_7.FormattingEnabled = true;
		this.comboBox_7.Items.AddRange(new object[6] { "目标站顺序", "目标站倒序", "按章节ID顺序", "按章节ID倒序", "按章节名顺序", "按章节名倒序" });
		this.comboBox_7.Location = new System.Drawing.Point(221, 32);
		this.comboBox_7.Name = "comboBox_7";
		this.comboBox_7.Size = new System.Drawing.Size(207, 20);
		this.comboBox_7.TabIndex = 28;
		this.label_27.AutoSize = true;
		this.label_27.Location = new System.Drawing.Point(219, 55);
		this.label_27.Name = "label_27";
		this.label_27.Size = new System.Drawing.Size(113, 12);
		this.label_27.TabIndex = 25;
		this.label_27.Text = "重复章节处理方式：";
		this.comboBox_5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_5.FormattingEnabled = true;
		this.comboBox_5.Items.AddRange(new object[2] { "只对比章节名", "对比分卷名+章节名" });
		this.comboBox_5.Location = new System.Drawing.Point(8, 70);
		this.comboBox_5.Name = "comboBox_5";
		this.comboBox_5.Size = new System.Drawing.Size(207, 20);
		this.comboBox_5.TabIndex = 24;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(6, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(101, 12);
		this.label2.TabIndex = 23;
		this.label2.Text = "空章节处理方式：";
		this.comboBox_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_1.FormattingEnabled = true;
		this.comboBox_1.Items.AddRange(new object[3] { "停止本书，跳入下一本", "跳过本章，继续采集下一个章", "入库一个章节名，继续采集下一个章" });
		this.comboBox_1.Location = new System.Drawing.Point(8, 32);
		this.comboBox_1.Name = "comboBox_1";
		this.comboBox_1.Size = new System.Drawing.Size(207, 20);
		this.comboBox_1.TabIndex = 22;
		this.groupBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_0.Controls.Add(this.DelForTxtHtml);
		this.groupBox_0.Controls.Add(this.checkBox_22);
		this.groupBox_0.Controls.Add(this.checkBox_21);
		this.groupBox_0.Controls.Add(this.checkBox_17);
		this.groupBox_0.Controls.Add(this.checkBox_16);
		this.groupBox_0.Controls.Add(this.DelForTxt);
		this.groupBox_0.Controls.Add(this.checkBox_8);
		this.groupBox_0.Controls.Add(this.checkBox_9);
		this.groupBox_0.Controls.Add(this.checkBox_4);
		this.groupBox_0.Controls.Add(this.dateTimePicker_1);
		this.groupBox_0.Controls.Add(this.label_25);
		this.groupBox_0.Controls.Add(this.checkBox_14);
		this.groupBox_0.Controls.Add(this.dateTimePicker_0);
		this.groupBox_0.Location = new System.Drawing.Point(6, 6);
		this.groupBox_0.Name = "groupBox_0";
		this.groupBox_0.Size = new System.Drawing.Size(790, 116);
		this.groupBox_0.TabIndex = 49;
		this.groupBox_0.TabStop = false;
		this.groupBox_0.Text = "替换设置";
		this.DelForTxtHtml.AutoSize = true;
		this.DelForTxtHtml.Location = new System.Drawing.Point(202, 64);
		this.DelForTxtHtml.Name = "DelForTxtHtml";
		this.DelForTxtHtml.Size = new System.Drawing.Size(144, 16);
		this.DelForTxtHtml.TabIndex = 72;
		this.DelForTxtHtml.Text = "清理无用HTML页面文件";
		this.DelForTxtHtml.UseVisualStyleBackColor = true;
		this.checkBox_22.AutoSize = true;
		this.checkBox_22.Location = new System.Drawing.Point(428, 20);
		this.checkBox_22.Name = "checkBox_22";
		this.checkBox_22.Size = new System.Drawing.Size(132, 16);
		this.checkBox_22.TabIndex = 71;
		this.checkBox_22.Text = "检测目标站重复章节";
		this.checkBox_22.UseVisualStyleBackColor = true;
		this.checkBox_21.AutoSize = true;
		this.checkBox_21.Location = new System.Drawing.Point(202, 42);
		this.checkBox_21.Name = "checkBox_21";
		this.checkBox_21.Size = new System.Drawing.Size(198, 16);
		this.checkBox_21.TabIndex = 70;
		this.checkBox_21.Text = "遇到“１一1壹”才判断添加分卷";
		this.checkBox_21.UseVisualStyleBackColor = true;
		this.checkBox_17.AutoSize = true;
		this.checkBox_17.Location = new System.Drawing.Point(428, 42);
		this.checkBox_17.Name = "checkBox_17";
		this.checkBox_17.Size = new System.Drawing.Size(150, 16);
		this.checkBox_17.TabIndex = 68;
		this.checkBox_17.Text = "以\"书名+作者\"识别书籍";
		this.checkBox_17.UseVisualStyleBackColor = true;
		this.checkBox_16.AutoSize = true;
		this.checkBox_16.Location = new System.Drawing.Point(8, 64);
		this.checkBox_16.Name = "checkBox_16";
		this.checkBox_16.Size = new System.Drawing.Size(96, 16);
		this.checkBox_16.TabIndex = 62;
		this.checkBox_16.Text = "隐藏更新小说";
		this.checkBox_16.UseVisualStyleBackColor = true;
		this.DelForTxt.AutoSize = true;
		this.DelForTxt.Location = new System.Drawing.Point(428, 64);
		this.DelForTxt.Name = "DelForTxt";
		this.DelForTxt.Size = new System.Drawing.Size(150, 16);
		this.DelForTxt.TabIndex = 60;
		this.DelForTxt.Text = "清理无用的TXT文本文件";
		this.DelForTxt.UseVisualStyleBackColor = true;
		this.checkBox_8.AutoSize = true;
		this.checkBox_8.Location = new System.Drawing.Point(202, 20);
		this.checkBox_8.Name = "checkBox_8";
		this.checkBox_8.Size = new System.Drawing.Size(120, 16);
		this.checkBox_8.TabIndex = 61;
		this.checkBox_8.Text = "不处理已完成小说";
		this.checkBox_8.UseVisualStyleBackColor = true;
		this.checkBox_9.AutoSize = true;
		this.checkBox_9.Location = new System.Drawing.Point(8, 20);
		this.checkBox_9.Name = "checkBox_9";
		this.checkBox_9.Size = new System.Drawing.Size(96, 16);
		this.checkBox_9.TabIndex = 67;
		this.checkBox_9.Text = "是否加添新书";
		this.checkBox_9.UseVisualStyleBackColor = true;
		this.checkBox_4.AutoSize = true;
		this.checkBox_4.Location = new System.Drawing.Point(8, 42);
		this.checkBox_4.Name = "checkBox_4";
		this.checkBox_4.Size = new System.Drawing.Size(96, 16);
		this.checkBox_4.TabIndex = 65;
		this.checkBox_4.Text = "禁止添加分卷";
		this.checkBox_4.UseVisualStyleBackColor = true;
		this.dateTimePicker_1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
		this.dateTimePicker_1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dateTimePicker_1.Location = new System.Drawing.Point(318, 85);
		this.dateTimePicker_1.Name = "dateTimePicker_1";
		this.dateTimePicker_1.Size = new System.Drawing.Size(153, 21);
		this.dateTimePicker_1.TabIndex = 55;
		this.label_25.AutoSize = true;
		this.label_25.Location = new System.Drawing.Point(295, 88);
		this.label_25.Name = "label_25";
		this.label_25.Size = new System.Drawing.Size(17, 12);
		this.label_25.TabIndex = 54;
		this.label_25.Text = "至";
		this.checkBox_14.AutoSize = true;
		this.checkBox_14.Location = new System.Drawing.Point(8, 86);
		this.checkBox_14.Name = "checkBox_14";
		this.checkBox_14.Size = new System.Drawing.Size(120, 16);
		this.checkBox_14.TabIndex = 49;
		this.checkBox_14.Text = "章节入库时间限制";
		this.checkBox_14.UseVisualStyleBackColor = true;
		this.dateTimePicker_0.CustomFormat = "yyyy/MM/dd HH:mm:ss";
		this.dateTimePicker_0.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dateTimePicker_0.Location = new System.Drawing.Point(136, 85);
		this.dateTimePicker_0.Name = "dateTimePicker_0";
		this.dateTimePicker_0.Size = new System.Drawing.Size(153, 21);
		this.dateTimePicker_0.TabIndex = 52;
		this.checkBox_5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.checkBox_5.AutoSize = true;
		this.checkBox_5.Location = new System.Drawing.Point(459, 304);
		this.checkBox_5.Name = "checkBox_5";
		this.checkBox_5.Size = new System.Drawing.Size(72, 16);
		this.checkBox_5.TabIndex = 45;
		this.checkBox_5.Text = "修复记录";
		this.checkBox_5.UseVisualStyleBackColor = true;
		this.label_1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label_1.AutoSize = true;
		this.label_1.Location = new System.Drawing.Point(615, 304);
		this.label_1.Name = "label_1";
		this.label_1.Size = new System.Drawing.Size(125, 12);
		this.label_1.TabIndex = 47;
		this.label_1.Text = "循环间隔时间(分钟)：";
		this.checkBox_6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.checkBox_6.AutoSize = true;
		this.checkBox_6.Location = new System.Drawing.Point(537, 304);
		this.checkBox_6.Name = "checkBox_6";
		this.checkBox_6.Size = new System.Drawing.Size(72, 16);
		this.checkBox_6.TabIndex = 46;
		this.checkBox_6.Text = "循环修复";
		this.checkBox_6.UseVisualStyleBackColor = true;
		this.numericUpDown_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.numericUpDown_0.Location = new System.Drawing.Point(746, 301);
		this.numericUpDown_0.Name = "numericUpDown_0";
		this.numericUpDown_0.Size = new System.Drawing.Size(50, 21);
		this.numericUpDown_0.TabIndex = 48;
		this.tabControl_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tabControl_0.Controls.Add(this.tabPage_1);
		this.tabControl_0.Controls.Add(this.tabPage_2);
		this.tabControl_0.Controls.Add(this.tabPage_4);
		this.tabControl_0.Controls.Add(this.tabPage1);
		this.tabControl_0.Location = new System.Drawing.Point(12, 12);
		this.tabControl_0.Name = "tabControl_0";
		this.tabControl_0.SelectedIndex = 0;
		this.tabControl_0.Size = new System.Drawing.Size(810, 356);
		this.tabControl_0.TabIndex = 48;
		base.ClientSize = new System.Drawing.Size(834, 409);
		base.Controls.Add(this.button_2);
		base.Controls.Add(this.tabControl_0);
		base.Controls.Add(this.comboBox_0);
		base.Controls.Add(this.button_0);
		base.Controls.Add(this.button_1);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "CollectRepair";
		this.Text = "超级修复模式";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CollectReplace_FormClosing);
		base.Load += new System.EventHandler(CollectReplace_Load);
		this.tabPage1.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox_7.ResumeLayout(false);
		this.groupBox_7.PerformLayout();
		this.tabPage_4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox_9.ResumeLayout(false);
		this.groupBox_9.PerformLayout();
		this.groupBox_8.ResumeLayout(false);
		this.groupBox_8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_5).EndInit();
		this.tabPage_2.ResumeLayout(false);
		this.groupBox_4.ResumeLayout(false);
		this.groupBox_4.PerformLayout();
		this.groupBox_3.ResumeLayout(false);
		this.groupBox_3.PerformLayout();
		this.groupBox_2.ResumeLayout(false);
		this.groupBox_2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DonotCollectChapterNum).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_2).EndInit();
		this.tabPage_1.ResumeLayout(false);
		this.tabPage_1.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown1).EndInit();
		this.groupBox_1.ResumeLayout(false);
		this.groupBox_1.PerformLayout();
		this.groupBox_0.ResumeLayout(false);
		this.groupBox_0.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).EndInit();
		this.tabControl_0.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	private void loadConfigs()
	{
		try
		{
			checkBox_5.Checked = tInfo.Log;
			checkBox_6.Checked = tInfo.Timing;
			numericUpDown_0.Value = tInfo.Interval;
			checkBox_14.Checked = tInfo.AddTime;
			dateTimePicker_0.Value = tInfo.MinAddTime;
			dateTimePicker_1.Value = tInfo.MaxAddTime;
			checkBox_4.Checked = tInfo.ProhibitionVolume;
			numericUpDown_2.Value = tInfo.MinChapterNum;
			numericUpDown_1.Value = tInfo.FindMaxChapterNum;
			DonotCollectChapterNum.Value = tInfo.ReMoteChapterNum;
			comboBox_2.SelectedIndex = tInfo.FilterNovelType;
			if (tInfo.FilterVolume != null)
			{
				FilterVolume.Text = string.Join("\r\n", tInfo.FilterVolume);
			}
			if (tInfo.FilterNovel != null)
			{
				FilterNovel.Text = string.Join("\r\n", tInfo.FilterNovel);
			}
			textBox_16.Text = tInfo.ProxyHost;
			textBox_15.Text = tInfo.ProxyPort.ToString();
			textBox_12.Text = tInfo.ProxyDomain;
			textBox_14.Text = tInfo.ProxyUser;
			textBox_13.Text = tInfo.ProxyPassword;
			checkBox_12.Checked = tInfo.Proxy;
			numericUpDown_5.Value = tInfo.NovelUrlWait;
			numericUpDown_4.Value = tInfo.IndexUrlWait;
			numericUpDown_3.Value = tInfo.ChapterUrlWait;
			checkBox_13.Checked = tInfo.NoPBar;
			checkBox_22.Checked = tInfo.CheckRepeat;
			DelForTxt.Checked = tInfo.DelForTxt;
			checkBox_8.Checked = tInfo.FilterFinish;
			checkBox_9.Checked = tInfo.NewBook;
			checkBox_16.Checked = tInfo.Hidebook;
			checkBox_17.Checked = tInfo.NameAndAuthor;
			checkBox_21.Checked = tInfo.CheckVolume;
			checkBox_4.Checked = tInfo.ProhibitionVolume;
			comboBox_1.SelectedIndex = tInfo.EmptyChapter;
			comboBox_7.SelectedIndex = tInfo.OrderChapter;
			comboBox1.SelectedIndex = tInfo.RepeatChapterHandle;
			comboBox_5.SelectedIndex = tInfo.GoRepeatChapter;
			DelForTxtHtml.Checked = tInfo.DelForTxtHtml;
			numericUpDown1.Value = tInfo.ErrorNum;
			ReplaceChapterNameOn.Checked = tInfo.ReplaceChapterNameOn;
			if (!Configs.BaseConfig.LicenseVip)
			{
				numericUpDown1.Value = 0m;
				checkBox_0.Checked = false;
				ReplaceChapterNameOn.Checked = false;
				groupBox3.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageForm messageForm = new MessageForm
			{
				Text = "错误提示"
			};
			MessageForm messageForm2 = messageForm;
			messageForm2.MessageText.Text = ex.Message;
			messageForm2.ShowDialog();
		}
	}

	internal static void pvuWPiSIY2EwXi7iKgC(Form form_0, bool bool_1)
	{
	}

	private void repairCollect(NovelInfo novelInfo_0, string string_3, string string_2)
	{
		if (novelInfo_0.GetID == null)
		{
			novelInfo_0.GetID = "0";
		}
		if (novelInfo_0.Name == null)
		{
			novelInfo_0.Name = "";
		}
		backgroundWorker_0.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
		backgroundWorker_0.ReportProgress(1, "--");
		backgroundWorker_0.ReportProgress(2, "获得小说信息");
		backgroundWorker_0.ReportProgress(4, 0);
		SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 获得小说信息");
		Page page = new Page(rInfo, tInfo);
		try
		{
			if (novelInfo_0.PutID == 0)
			{
				if (novelInfo_0.NovelUrl == null)
				{
					novelInfo_0 = page.GetNovelInfo(novelInfo_0);
				}
				novelInfo_0 = LocalProvider.GetInstance().GetNovelInfo(novelInfo_0, tInfo.NameAndAuthor);
			}
			else if (novelInfo_0.GetID == "" || novelInfo_0.GetID == "0")
			{
				novelInfo_0 = LocalProvider.GetInstance().GetNovelInfo(novelInfo_0, tInfo.NameAndAuthor);
				novelInfo_0 = page.GetNovelInfo(novelInfo_0);
			}
		}
		catch (Exception ex)
		{
			backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | " + ex.Message);
			return;
		}
		Thread.Sleep(tInfo.NovelUrlWait);
		try
		{
			ICollection keys = Configs.TaskNovelInfo.Keys;
			if (Configs.TaskNovelInfo.Count != 0)
			{
				IEnumerator enumerator = keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Current;
					if (text != this.string_2 && Configs.TaskNovelInfo[text] != null && ((NovelInfo)Configs.TaskNovelInfo[text]).Name == novelInfo_0.Name)
					{
						backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | 子窗口冲突");
						return;
					}
				}
			}
		}
		catch
		{
			backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | 检查子窗口冲突失败");
			return;
		}
		Configs.TaskNovelInfo[this.string_2.ToString()] = novelInfo_0;
		backgroundWorker_0.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
		SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 过滤小说");
		if (tInfo.FilterNovelType == 1)
		{
			if (tInfo.FilterNovel != null)
			{
				string[] filterNovel = tInfo.FilterNovel;
				foreach (string text2 in filterNovel)
				{
					if (novelInfo_0.Name.Trim() == text2.Trim())
					{
						backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | " + novelInfo_0.Name + " 限制小说(黑名单)");
						return;
					}
				}
			}
		}
		else if (tInfo.FilterNovelType == 2)
		{
			bool flag = false;
			if (tInfo.FilterNovel != null)
			{
				string[] filterNovel2 = tInfo.FilterNovel;
				foreach (string text3 in filterNovel2)
				{
					if (novelInfo_0.Name.Trim() == text3.Trim())
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | " + novelInfo_0.Name + " 限制小说(不在白名单)");
				return;
			}
		}
		if (backgroundWorker_0.CancellationPending)
		{
			return;
		}
		SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 获得小说的章节目录");
		ChapterInfo[] chapterList;
		try
		{
			backgroundWorker_0.ReportProgress(2, "获得小说的章节目录");
			chapterList = page.GetChapterList(novelInfo_0);
		}
		catch (Exception ex2)
		{
			backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | " + novelInfo_0.Name + " " + ex2.Message);
			return;
		}
		if (chapterList == null || chapterList.Length == 0)
		{
			backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | " + novelInfo_0.Name + " 章节组为空");
			return;
		}
		backgroundWorker_0.ReportProgress(6, chapterList.Length);
		if (backgroundWorker_0.CancellationPending)
		{
			return;
		}
		bool flag2 = false;
		if (novelInfo_0.PutID == 0 && novelInfo_0.IsNew)
		{
			backgroundWorker_0.ReportProgress(2, "处理新书");
			if (!tInfo.Finish || novelInfo_0.Degree != 1 || !tInfo.NewBook)
			{
				SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 是否添加新书判断");
				if (!tInfo.NewBook)
				{
					backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | " + novelInfo_0.Name + " 设置不添加新书");
					return;
				}
				SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 章节数量限制判断");
				if (chapterList.Length > tInfo.FindMaxChapterNum && tInfo.FindMaxChapterNum != 0)
				{
					backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | " + novelInfo_0.Name + " 章节数量大于限制");
					return;
				}
				if (chapterList.Length < tInfo.MinChapterNum && tInfo.MinChapterNum != 0)
				{
					backgroundWorker_0.ReportProgress(9, "失败 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | " + novelInfo_0.Name + " 章节数量小于限制");
					return;
				}
			}
			SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 正式入库小说信息");
			novelInfo_0 = LocalProvider.GetInstance().InsertNovel(novelInfo_0);
			flag2 = true;
			backgroundWorker_0.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
		}
		ChapterInfo[] chapterList2 = LocalProvider.GetInstance().GetChapterList(novelInfo_0.PutID);
		int num = 0;
		num = chapterList2.Length - chapterList.Length;
		if (num < 0)
		{
			num = 0;
		}
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		Thread.Sleep(tInfo.IndexUrlWait);
		int num5 = chapterList.Length;
		backgroundWorker_0.ReportProgress(2, "章节循环完毕");
		if (flag2)
		{
			LocalProvider.GetInstance().UpdateLastChapter(novelInfo_0);
			LocalProvider.GetInstance().CreateIndex(novelInfo_0, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
			backgroundWorker_0.ReportProgress(8, "成功 | " + string_3 + " | " + string_2 + " | " + novelInfo_0.GetID + " | " + novelInfo_0.Name + " 替换" + num2 + "章节，新增" + num3 + "章节，跳过" + num4 + "章节");
		}
	}

	private void timer_0_Tick(object sender, EventArgs e)
	{
		if (!backgroundWorker_0.IsBusy)
		{
			if (dateTime_0 < DateTime.Now)
			{
				dateTime_0 = DateTime.Now.AddMinutes(tInfo.Interval);
				backgroundWorker_0.RunWorkerAsync();
				timer_0.Stop();
				return;
			}
			TimeSpan timeSpan = new TimeSpan(dateTime_0.Ticks).Subtract(new TimeSpan(DateTime.Now.Ticks)).Duration();
			label_15.Text = "距离下次循环开始还有 " + timeSpan.Days + "天" + timeSpan.Hours + "小时" + timeSpan.Minutes + "分钟" + timeSpan.Seconds + "秒";
		}
	}

	private void writeConfigToFiles()
	{
		try
		{
			tInfo.Log = checkBox_5.Checked;
			tInfo.Timing = checkBox_6.Checked;
			tInfo.Interval = Convert.ToInt32(numericUpDown_0.Value);
			tInfo.ProhibitionVolume = checkBox_4.Checked;
			tInfo.AddTime = checkBox_14.Checked;
			tInfo.MinAddTime = dateTimePicker_0.Value;
			tInfo.MaxAddTime = dateTimePicker_1.Value;
			tInfo.MinChapterNum = Convert.ToInt32(numericUpDown_2.Value);
			tInfo.FindMaxChapterNum = Convert.ToInt32(numericUpDown_1.Value);
			tInfo.ReMoteChapterNum = Convert.ToInt32(DonotCollectChapterNum.Value);
			tInfo.FilterNovelType = comboBox_2.SelectedIndex;
			tInfo.FilterVolume = FilterVolume.Text.Replace("\r\n", "♂").Split('♂');
			tInfo.FilterNovel = FilterNovel.Text.Replace("\r\n", "♂").Split('♂');
			tInfo.ProxyHost = textBox_16.Text;
			tInfo.ProxyPort = Convert.ToInt32(textBox_15.Text);
			tInfo.ProxyDomain = textBox_12.Text;
			tInfo.ProxyUser = textBox_14.Text;
			tInfo.ProxyPassword = textBox_13.Text;
			tInfo.Proxy = checkBox_12.Checked;
			tInfo.NoPBar = checkBox_13.Checked;
			tInfo.NovelUrlWait = Convert.ToInt32(numericUpDown_5.Value);
			tInfo.IndexUrlWait = Convert.ToInt32(numericUpDown_4.Value);
			tInfo.ChapterUrlWait = Convert.ToInt32(numericUpDown_3.Value);
			tInfo.CheckRepeat = checkBox_22.Checked;
			tInfo.DelForTxt = DelForTxt.Checked;
			tInfo.FilterFinish = checkBox_8.Checked;
			tInfo.NewBook = checkBox_9.Checked;
			tInfo.Hidebook = checkBox_16.Checked;
			tInfo.NameAndAuthor = checkBox_17.Checked;
			tInfo.CheckVolume = checkBox_21.Checked;
			tInfo.ProhibitionVolume = checkBox_4.Checked;
			tInfo.EmptyChapter = comboBox_1.SelectedIndex;
			tInfo.OrderChapter = comboBox_7.SelectedIndex;
			tInfo.RepeatChapterHandle = comboBox1.SelectedIndex;
			tInfo.GoRepeatChapter = comboBox_5.SelectedIndex;
			tInfo.DelForTxtHtml = DelForTxtHtml.Checked;
			tInfo.ErrorNum = Convert.ToInt32(numericUpDown1.Value);
			tInfo.ReplaceChapterNameOn = ReplaceChapterNameOn.Checked;
			if (!Configs.BaseConfig.LicenseVip)
			{
				tInfo.ReplaceChapterNameOn = false;
				tInfo.ErrorNum = 0;
			}
		}
		catch (Exception ex)
		{
			MessageForm messageForm = new MessageForm
			{
				Text = "错误提示"
			};
			MessageForm messageForm2 = messageForm;
			messageForm2.MessageText.Text = ex.Message;
			messageForm2.ShowDialog();
		}
	}
}
