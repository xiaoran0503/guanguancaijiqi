using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;
using NovelSpider.Local;
using NovelSpider.Target;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class CollectReplace : DockContent
{
	private BackgroundWorker backgroundWorker_0;

	private CheckBox BnameAndAuthorCheckBox;

	private bool bool_0;

	private Button button_0;

	private Button button_1;

	private Button button_2;

	private NumericUpDown ChapterNum;

	private CheckBox checkBox_0;

	private CheckBox checkBox_1;

	private CheckBox checkBox_12;

	private CheckBox checkBox_13;

	private CheckBox checkBox_14;

	private CheckBox checkBox_15;

	private CheckBox checkBox_2;

	private CheckBox checkBox_3;

	private CheckBox checkBox_4;

	private CheckBox checkBox_5;

	private CheckBox checkBox_6;

	private CheckBox checkBox_7;

	private ComboBox comboBox_0;

	private ComboBox comboBox_1;

	private ComboBox comboBox_2;

	private ComboBox comboBox_3;

	private IContainer components;

	private DateTime dateTime_0;

	private DateTimePicker dateTimePicker_0;

	private DateTimePicker dateTimePicker_1;

	private NumericUpDown DonotCollectChapterNum;

	private GroupBox groupBox_0;

	private GroupBox groupBox_1;

	private GroupBox groupBox_2;

	private GroupBox groupBox_3;

	private GroupBox groupBox_4;

	private GroupBox groupBox_7;

	private GroupBox groupBox_8;

	private GroupBox groupBox_9;

	private IContainer icontainer_0;

	private Label label_0;

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

	private Label label_23;

	private Label label_24;

	private Label label_25;

	private Label label_3;

	private Label label_4;

	private Label label_5;

	private Label label_6;

	private Label label_7;

	private Label label_8;

	private Label label_9;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private NumericUpDown numericUpDown_0;

	private NumericUpDown numericUpDown_1;

	private NumericUpDown numericUpDown_2;

	private NumericUpDown numericUpDown_3;

	private NumericUpDown numericUpDown_4;

	private NumericUpDown numericUpDown_5;

	private ProgressBar progressBar_0;

	private ProgressBar progressBar_1;

	private RadioButton radioButton_0;

	private RadioButton radioButton_1;

	private RadioButton radioButton_2;

	private RadioButton radioButton_3;

	private RadioButton radioButton_4;

	private RadioButton radioButton_5;

	private RichTextBox richTextBox_0;

	public RuleConfigInfo rInfo;

	private string string_0;

	private string string_1;

	private string string_2;

	private TabControl tabControl_0;

	private TabPage tabPage_0;

	private TabPage tabPage_1;

	private TabPage tabPage_2;

	private TabPage tabPage_4;

	private TextBox textBox_0;

	private TextBox textBox_1;

	private TextBox textBox_12;

	private TextBox textBox_13;

	private TextBox textBox_14;

	private TextBox textBox_15;

	private TextBox textBox_16;

	private TextBox textBox_2;

	private TextBox textBox_3;

	private TextBox textBox_4;

	private TextBox textBox_5;

	private TextBox textBox_6;

	private TextBox textBox_7;

	private TextBox textBox_8;

	private TextBox textBox_9;

	private Timer timer_0;

	public ReplaceConfigInfo tInfo;

	public CollectReplace()
	{
		dateTime_0 = DateTime.Now;
		rInfo = new RuleConfigInfo();
		string_0 = "";
		string_1 = "";
		string_2 = "";
		tInfo = new ReplaceConfigInfo();
		InitializeComponent();
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		string xmlString = "<RSAKeyValue><Modulus>phYjxmkE1tob0jMq5V9afVYwp7EP42ULX2A08wIHTECuQdfDB6Mff4DiEoCCH6ermcw8K9YCemr96tyzqtV5tkIrkDVuCDqpGboWrCXOfIivvKKhG5RCvlkgIgKbtKO977Ziv/2ZLdk75mqrSZJcaPSsyDxSG6+3L+X8x26rOuU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
		rSACryptoServiceProvider.FromXmlString(xmlString);
		new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider).SetHashAlgorithm("SHA1");
	}

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		backgroundWorker_0.ReportProgress(2, "获得小说列表");
		switch (tInfo.RadioBy)
		{
		case "GetListUrl":
		{
			string[] ids = new Page(rInfo, tInfo).GetIds(tInfo.GetListUrl);
			backgroundWorker_0.ReportProgress(5, ids.Length);
			method_2(ids, bool_1: true);
			break;
		}
		case "GetOrderId":
			backgroundWorker_0.ReportProgress(5, tInfo.GetOrderMaxId - tInfo.GetOrderMinId);
			method_3(tInfo.GetOrderMinId, tInfo.GetOrderMaxId, bool_1: true);
			break;
		case "GetSinceId":
			backgroundWorker_0.ReportProgress(5, tInfo.GetSinceId.Length);
			method_2(tInfo.GetSinceId, bool_1: true);
			break;
		case "PutSinceId":
			backgroundWorker_0.ReportProgress(5, tInfo.PutSinceId.Length);
			method_2(tInfo.PutSinceId, bool_1: false);
			break;
		case "PutOrderId":
			backgroundWorker_0.ReportProgress(5, tInfo.PutOrderMaxId - tInfo.PutOrderMinId);
			method_3(tInfo.PutOrderMinId, tInfo.PutOrderMaxId, bool_1: false);
			break;
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
			if (!tInfo.NoPBar)
			{
				richTextBox_0.Text = "";
				richTextBox_0.AppendText(e.UserState.ToString() + "\n");
				richTextBox_0.Focus();
				richTextBox_0.Select(richTextBox_0.TextLength, 0);
				richTextBox_0.ScrollToCaret();
			}
			break;
		case 1:
			label_16.Text = e.UserState.ToString();
			if (!tInfo.NoPBar)
			{
				richTextBox_0.AppendText(e.UserState.ToString() + "\n");
				richTextBox_0.Focus();
				richTextBox_0.Select(richTextBox_0.TextLength, 0);
				richTextBox_0.ScrollToCaret();
			}
			break;
		case 2:
			label_15.Text = e.UserState.ToString();
			if (!tInfo.NoPBar)
			{
				richTextBox_0.AppendText(e.UserState.ToString() + "\n");
				richTextBox_0.Focus();
				richTextBox_0.Select(richTextBox_0.TextLength, 0);
				richTextBox_0.ScrollToCaret();
			}
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
					SpiderException.Show(0, e.Error.Message, null, tInfo.Log, string_0, tInfo.RuleFile);
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
		if (Configs.BaseConfig.LicenseOk)
		{
			rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(comboBox_0.Text, rInfo);
			method_1();
			tInfo.ID = string_2;
			button_1.Enabled = false;
			button_0.Enabled = true;
			tabControl_0.SelectedIndex = tabControl_0.TabCount - 1;
			dateTime_0 = DateTime.Now.AddMinutes(tInfo.Interval);
			if (!backgroundWorker_0.IsBusy)
			{
				backgroundWorker_0.RunWorkerAsync();
			}
		}
	}

	private void button_2_Click(object sender, EventArgs e)
	{
		method_1();
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
		tInfo = (ReplaceConfigInfo)ConfigFileManager.LoadConfig("ReplaceConfig.xml", tInfo);
		comboBox_0.BeginUpdate();
		string[] array = IO.LoadRules();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				comboBox_0.Items.Add(array[i]);
				if (array[i] == tInfo.RuleFile)
				{
					comboBox_0.Text = tInfo.RuleFile;
					rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(comboBox_0.Text, rInfo);
					textBox_0.Text = rInfo.NovelListUrl.Pattern;
					if (!bool_0)
					{
						Text = rInfo.GetSiteName.Pattern + " 替换采集模式";
					}
				}
			}
		}
		comboBox_0.EndUpdate();
		method_0();
	}

	private void comboBox_0_SelectedIndexChanged(object sender, EventArgs e)
	{
		rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(comboBox_0.Text, rInfo);
		textBox_0.Text = rInfo.NovelListUrl.Pattern;
		if (!bool_0)
		{
			Text = rInfo.GetSiteName.Pattern + " 替换采集模式";
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.CollectReplace));
		this.checkBox_0 = new System.Windows.Forms.CheckBox();
		this.checkBox_1 = new System.Windows.Forms.CheckBox();
		this.checkBox_2 = new System.Windows.Forms.CheckBox();
		this.checkBox_3 = new System.Windows.Forms.CheckBox();
		this.checkBox_4 = new System.Windows.Forms.CheckBox();
		this.comboBox_0 = new System.Windows.Forms.ComboBox();
		this.textBox_0 = new System.Windows.Forms.TextBox();
		this.textBox_1 = new System.Windows.Forms.TextBox();
		this.textBox_2 = new System.Windows.Forms.TextBox();
		this.textBox_3 = new System.Windows.Forms.TextBox();
		this.radioButton_0 = new System.Windows.Forms.RadioButton();
		this.radioButton_1 = new System.Windows.Forms.RadioButton();
		this.radioButton_2 = new System.Windows.Forms.RadioButton();
		this.checkBox_5 = new System.Windows.Forms.CheckBox();
		this.button_0 = new System.Windows.Forms.Button();
		this.button_1 = new System.Windows.Forms.Button();
		this.tabControl_0 = new System.Windows.Forms.TabControl();
		this.tabPage_0 = new System.Windows.Forms.TabPage();
		this.label_0 = new System.Windows.Forms.Label();
		this.textBox_4 = new System.Windows.Forms.TextBox();
		this.radioButton_3 = new System.Windows.Forms.RadioButton();
		this.radioButton_4 = new System.Windows.Forms.RadioButton();
		this.textBox_5 = new System.Windows.Forms.TextBox();
		this.textBox_6 = new System.Windows.Forms.TextBox();
		this.textBox_7 = new System.Windows.Forms.TextBox();
		this.radioButton_5 = new System.Windows.Forms.RadioButton();
		this.tabPage_1 = new System.Windows.Forms.TabPage();
		this.groupBox_1 = new System.Windows.Forms.GroupBox();
		this.label_24 = new System.Windows.Forms.Label();
		this.label_23 = new System.Windows.Forms.Label();
		this.comboBox_3 = new System.Windows.Forms.ComboBox();
		this.comboBox_1 = new System.Windows.Forms.ComboBox();
		this.checkBox_6 = new System.Windows.Forms.CheckBox();
		this.numericUpDown_0 = new System.Windows.Forms.NumericUpDown();
		this.label_1 = new System.Windows.Forms.Label();
		this.groupBox_0 = new System.Windows.Forms.GroupBox();
		this.BnameAndAuthorCheckBox = new System.Windows.Forms.CheckBox();
		this.ChapterNum = new System.Windows.Forms.NumericUpDown();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.checkBox_15 = new System.Windows.Forms.CheckBox();
		this.dateTimePicker_1 = new System.Windows.Forms.DateTimePicker();
		this.label_25 = new System.Windows.Forms.Label();
		this.checkBox_14 = new System.Windows.Forms.CheckBox();
		this.dateTimePicker_0 = new System.Windows.Forms.DateTimePicker();
		this.tabPage_2 = new System.Windows.Forms.TabPage();
		this.groupBox_4 = new System.Windows.Forms.GroupBox();
		this.textBox_9 = new System.Windows.Forms.TextBox();
		this.groupBox_3 = new System.Windows.Forms.GroupBox();
		this.textBox_8 = new System.Windows.Forms.TextBox();
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
		this.checkBox_7 = new System.Windows.Forms.CheckBox();
		this.tabPage_4 = new System.Windows.Forms.TabPage();
		this.groupBox_9 = new System.Windows.Forms.GroupBox();
		this.richTextBox_0 = new System.Windows.Forms.RichTextBox();
		this.checkBox_13 = new System.Windows.Forms.CheckBox();
		this.label_15 = new System.Windows.Forms.Label();
		this.label_16 = new System.Windows.Forms.Label();
		this.label_17 = new System.Windows.Forms.Label();
		this.label_18 = new System.Windows.Forms.Label();
		this.label_19 = new System.Windows.Forms.Label();
		this.label_20 = new System.Windows.Forms.Label();
		this.progressBar_0 = new System.Windows.Forms.ProgressBar();
		this.label_21 = new System.Windows.Forms.Label();
		this.label_22 = new System.Windows.Forms.Label();
		this.progressBar_1 = new System.Windows.Forms.ProgressBar();
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
		this.button_2 = new System.Windows.Forms.Button();
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		this.timer_0 = new System.Windows.Forms.Timer(this.components);
		this.tabControl_0.SuspendLayout();
		this.tabPage_0.SuspendLayout();
		this.tabPage_1.SuspendLayout();
		this.groupBox_1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).BeginInit();
		this.groupBox_0.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ChapterNum).BeginInit();
		this.tabPage_2.SuspendLayout();
		this.groupBox_4.SuspendLayout();
		this.groupBox_3.SuspendLayout();
		this.groupBox_2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DonotCollectChapterNum).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_2).BeginInit();
		this.tabPage_4.SuspendLayout();
		this.groupBox_9.SuspendLayout();
		this.groupBox_8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_5).BeginInit();
		this.groupBox_7.SuspendLayout();
		base.SuspendLayout();
		this.checkBox_0.AutoSize = true;
		this.checkBox_0.Location = new System.Drawing.Point(6, 20);
		this.checkBox_0.Name = "checkBox_0";
		this.checkBox_0.Size = new System.Drawing.Size(96, 16);
		this.checkBox_0.TabIndex = 0;
		this.checkBox_0.Text = "图片替换文字";
		this.checkBox_0.UseVisualStyleBackColor = true;
		this.checkBox_1.AutoSize = true;
		this.checkBox_1.Location = new System.Drawing.Point(214, 20);
		this.checkBox_1.Name = "checkBox_1";
		this.checkBox_1.Size = new System.Drawing.Size(96, 16);
		this.checkBox_1.TabIndex = 1;
		this.checkBox_1.Text = "图片替换图片";
		this.checkBox_1.UseVisualStyleBackColor = true;
		this.checkBox_2.AutoSize = true;
		this.checkBox_2.Location = new System.Drawing.Point(6, 42);
		this.checkBox_2.Name = "checkBox_2";
		this.checkBox_2.Size = new System.Drawing.Size(96, 16);
		this.checkBox_2.TabIndex = 2;
		this.checkBox_2.Text = "文字替换文字";
		this.checkBox_2.UseVisualStyleBackColor = true;
		this.checkBox_3.AutoSize = true;
		this.checkBox_3.Location = new System.Drawing.Point(214, 42);
		this.checkBox_3.Name = "checkBox_3";
		this.checkBox_3.Size = new System.Drawing.Size(96, 16);
		this.checkBox_3.TabIndex = 3;
		this.checkBox_3.Text = "文字替换图片";
		this.checkBox_3.UseVisualStyleBackColor = true;
		this.checkBox_4.AutoSize = true;
		this.checkBox_4.Location = new System.Drawing.Point(214, 64);
		this.checkBox_4.Name = "checkBox_4";
		this.checkBox_4.Size = new System.Drawing.Size(96, 16);
		this.checkBox_4.TabIndex = 4;
		this.checkBox_4.Text = "空白替换任意";
		this.checkBox_4.UseVisualStyleBackColor = true;
		this.comboBox_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.comboBox_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_0.FormattingEnabled = true;
		this.comboBox_0.Location = new System.Drawing.Point(12, 332);
		this.comboBox_0.Name = "comboBox_0";
		this.comboBox_0.Size = new System.Drawing.Size(200, 20);
		this.comboBox_0.TabIndex = 31;
		this.comboBox_0.SelectedIndexChanged += new System.EventHandler(comboBox_0_SelectedIndexChanged);
		this.textBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_0.Location = new System.Drawing.Point(8, 28);
		this.textBox_0.Multiline = true;
		this.textBox_0.Name = "textBox_0";
		this.textBox_0.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox_0.Size = new System.Drawing.Size(786, 50);
		this.textBox_0.TabIndex = 33;
		this.textBox_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_1.Location = new System.Drawing.Point(143, 111);
		this.textBox_1.Name = "textBox_1";
		this.textBox_1.Size = new System.Drawing.Size(651, 21);
		this.textBox_1.TabIndex = 38;
		this.textBox_2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_2.Location = new System.Drawing.Point(279, 84);
		this.textBox_2.Name = "textBox_2";
		this.textBox_2.Size = new System.Drawing.Size(515, 21);
		this.textBox_2.TabIndex = 36;
		this.textBox_3.Location = new System.Drawing.Point(143, 84);
		this.textBox_3.Name = "textBox_3";
		this.textBox_3.Size = new System.Drawing.Size(130, 21);
		this.textBox_3.TabIndex = 35;
		this.radioButton_0.AutoSize = true;
		this.radioButton_0.Checked = true;
		this.radioButton_0.Location = new System.Drawing.Point(8, 6);
		this.radioButton_0.Name = "radioButton_0";
		this.radioButton_0.Size = new System.Drawing.Size(335, 16);
		this.radioButton_0.TabIndex = 32;
		this.radioButton_0.TabStop = true;
		this.radioButton_0.Text = "按目标站页面获得编号，一般监控最新列表，一个地址一行";
		this.radioButton_0.UseVisualStyleBackColor = true;
		this.radioButton_1.AutoSize = true;
		this.radioButton_1.Location = new System.Drawing.Point(8, 112);
		this.radioButton_1.Name = "radioButton_1";
		this.radioButton_1.Size = new System.Drawing.Size(131, 16);
		this.radioButton_1.TabIndex = 37;
		this.radioButton_1.Text = "按目标站自定义编号";
		this.radioButton_1.UseVisualStyleBackColor = true;
		this.radioButton_2.AutoSize = true;
		this.radioButton_2.Location = new System.Drawing.Point(8, 85);
		this.radioButton_2.Name = "radioButton_2";
		this.radioButton_2.Size = new System.Drawing.Size(119, 16);
		this.radioButton_2.TabIndex = 34;
		this.radioButton_2.Text = "按目标站编号顺序";
		this.radioButton_2.UseVisualStyleBackColor = true;
		this.checkBox_5.AutoSize = true;
		this.checkBox_5.Location = new System.Drawing.Point(194, 79);
		this.checkBox_5.Name = "checkBox_5";
		this.checkBox_5.Size = new System.Drawing.Size(72, 16);
		this.checkBox_5.TabIndex = 45;
		this.checkBox_5.Text = "日志记录";
		this.checkBox_5.UseVisualStyleBackColor = true;
		this.button_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button_0.Enabled = false;
		this.button_0.Location = new System.Drawing.Point(666, 330);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 23);
		this.button_0.TabIndex = 47;
		this.button_0.Text = "停止";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(button_0_Click);
		this.button_1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button_1.Location = new System.Drawing.Point(585, 330);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(75, 23);
		this.button_1.TabIndex = 46;
		this.button_1.Text = "开始";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.tabControl_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tabControl_0.Controls.Add(this.tabPage_0);
		this.tabControl_0.Controls.Add(this.tabPage_1);
		this.tabControl_0.Controls.Add(this.tabPage_2);
		this.tabControl_0.Controls.Add(this.tabPage_4);
		this.tabControl_0.Location = new System.Drawing.Point(12, 12);
		this.tabControl_0.Name = "tabControl_0";
		this.tabControl_0.SelectedIndex = 0;
		this.tabControl_0.Size = new System.Drawing.Size(810, 314);
		this.tabControl_0.TabIndex = 48;
		this.tabPage_0.Controls.Add(this.label_0);
		this.tabPage_0.Controls.Add(this.textBox_4);
		this.tabPage_0.Controls.Add(this.radioButton_3);
		this.tabPage_0.Controls.Add(this.radioButton_4);
		this.tabPage_0.Controls.Add(this.textBox_5);
		this.tabPage_0.Controls.Add(this.textBox_6);
		this.tabPage_0.Controls.Add(this.textBox_7);
		this.tabPage_0.Controls.Add(this.radioButton_5);
		this.tabPage_0.Controls.Add(this.radioButton_0);
		this.tabPage_0.Controls.Add(this.radioButton_2);
		this.tabPage_0.Controls.Add(this.radioButton_1);
		this.tabPage_0.Controls.Add(this.textBox_3);
		this.tabPage_0.Controls.Add(this.textBox_2);
		this.tabPage_0.Controls.Add(this.textBox_1);
		this.tabPage_0.Controls.Add(this.textBox_0);
		this.tabPage_0.Location = new System.Drawing.Point(4, 22);
		this.tabPage_0.Name = "tabPage_0";
		this.tabPage_0.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage_0.Size = new System.Drawing.Size(802, 288);
		this.tabPage_0.TabIndex = 0;
		this.tabPage_0.Text = "替换方式";
		this.tabPage_0.UseVisualStyleBackColor = true;
		this.label_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label_0.AutoSize = true;
		this.label_0.ForeColor = System.Drawing.Color.Maroon;
		this.label_0.Location = new System.Drawing.Point(579, 140);
		this.label_0.Name = "label_0";
		this.label_0.Size = new System.Drawing.Size(215, 12);
		this.label_0.TabIndex = 51;
		this.label_0.Text = "以下3种方式都需要使用目标站搜索功能";
		this.textBox_4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_4.Location = new System.Drawing.Point(8, 160);
		this.textBox_4.Multiline = true;
		this.textBox_4.Name = "textBox_4";
		this.textBox_4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox_4.Size = new System.Drawing.Size(786, 67);
		this.textBox_4.TabIndex = 50;
		this.radioButton_3.AutoSize = true;
		this.radioButton_3.Enabled = false;
		this.radioButton_3.Location = new System.Drawing.Point(8, 138);
		this.radioButton_3.Name = "radioButton_3";
		this.radioButton_3.Size = new System.Drawing.Size(137, 16);
		this.radioButton_3.TabIndex = 49;
		this.radioButton_3.TabStop = true;
		this.radioButton_3.Text = "按自己站SQL执行结果";
		this.radioButton_3.UseVisualStyleBackColor = true;
		this.radioButton_4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.radioButton_4.AutoSize = true;
		this.radioButton_4.Location = new System.Drawing.Point(8, 234);
		this.radioButton_4.Name = "radioButton_4";
		this.radioButton_4.Size = new System.Drawing.Size(119, 16);
		this.radioButton_4.TabIndex = 44;
		this.radioButton_4.TabStop = true;
		this.radioButton_4.Text = "按自己站编号顺序";
		this.radioButton_4.UseVisualStyleBackColor = true;
		this.textBox_5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.textBox_5.Location = new System.Drawing.Point(142, 233);
		this.textBox_5.Name = "textBox_5";
		this.textBox_5.Size = new System.Drawing.Size(130, 21);
		this.textBox_5.TabIndex = 45;
		this.textBox_6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_6.Location = new System.Drawing.Point(279, 233);
		this.textBox_6.Name = "textBox_6";
		this.textBox_6.Size = new System.Drawing.Size(515, 21);
		this.textBox_6.TabIndex = 46;
		this.textBox_7.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_7.Location = new System.Drawing.Point(142, 260);
		this.textBox_7.Name = "textBox_7";
		this.textBox_7.Size = new System.Drawing.Size(652, 21);
		this.textBox_7.TabIndex = 48;
		this.radioButton_5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.radioButton_5.AutoSize = true;
		this.radioButton_5.Location = new System.Drawing.Point(8, 261);
		this.radioButton_5.Name = "radioButton_5";
		this.radioButton_5.Size = new System.Drawing.Size(131, 16);
		this.radioButton_5.TabIndex = 47;
		this.radioButton_5.TabStop = true;
		this.radioButton_5.Text = "按自己站自定义编号";
		this.radioButton_5.UseVisualStyleBackColor = true;
		this.tabPage_1.Controls.Add(this.groupBox_1);
		this.tabPage_1.Controls.Add(this.groupBox_0);
		this.tabPage_1.Location = new System.Drawing.Point(4, 22);
		this.tabPage_1.Name = "tabPage_1";
		this.tabPage_1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage_1.Size = new System.Drawing.Size(802, 288);
		this.tabPage_1.TabIndex = 1;
		this.tabPage_1.Text = "替换设置";
		this.tabPage_1.UseVisualStyleBackColor = true;
		this.groupBox_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_1.Controls.Add(this.label_24);
		this.groupBox_1.Controls.Add(this.label_23);
		this.groupBox_1.Controls.Add(this.comboBox_3);
		this.groupBox_1.Controls.Add(this.comboBox_1);
		this.groupBox_1.Controls.Add(this.checkBox_6);
		this.groupBox_1.Controls.Add(this.numericUpDown_0);
		this.groupBox_1.Controls.Add(this.checkBox_5);
		this.groupBox_1.Controls.Add(this.label_1);
		this.groupBox_1.Location = new System.Drawing.Point(6, 178);
		this.groupBox_1.Name = "groupBox_1";
		this.groupBox_1.Size = new System.Drawing.Size(790, 104);
		this.groupBox_1.TabIndex = 50;
		this.groupBox_1.TabStop = false;
		this.groupBox_1.Text = "其他设置";
		this.label_24.AutoSize = true;
		this.label_24.Location = new System.Drawing.Point(6, 17);
		this.label_24.Name = "label_24";
		this.label_24.Size = new System.Drawing.Size(113, 12);
		this.label_24.TabIndex = 51;
		this.label_24.Text = "最新章节对比方式：";
		this.label_23.AutoSize = true;
		this.label_23.Location = new System.Drawing.Point(270, 17);
		this.label_23.Name = "label_23";
		this.label_23.Size = new System.Drawing.Size(137, 12);
		this.label_23.TabIndex = 50;
		this.label_23.Text = "下载图片失败处理方式：";
		this.comboBox_3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_3.FormattingEnabled = true;
		this.comboBox_3.Items.AddRange(new object[2] { "启用盗链模式", "跳入下一本" });
		this.comboBox_3.Location = new System.Drawing.Point(272, 32);
		this.comboBox_3.Name = "comboBox_3";
		this.comboBox_3.Size = new System.Drawing.Size(187, 20);
		this.comboBox_3.TabIndex = 49;
		this.comboBox_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_1.FormattingEnabled = true;
		this.comboBox_1.Items.AddRange(new object[5] { "只对比章节名", "对比分卷名+章节名", "智能对比 V1.2 Beta", "得分对比 V2.6 Beta", "智能对比 V3.2 Beta" });
		this.comboBox_1.Location = new System.Drawing.Point(8, 32);
		this.comboBox_1.Name = "comboBox_1";
		this.comboBox_1.Size = new System.Drawing.Size(182, 20);
		this.comboBox_1.TabIndex = 48;
		this.checkBox_6.AutoSize = true;
		this.checkBox_6.Location = new System.Drawing.Point(272, 79);
		this.checkBox_6.Name = "checkBox_6";
		this.checkBox_6.Size = new System.Drawing.Size(72, 16);
		this.checkBox_6.TabIndex = 46;
		this.checkBox_6.Text = "循环采集";
		this.checkBox_6.UseVisualStyleBackColor = true;
		this.numericUpDown_0.Location = new System.Drawing.Point(481, 76);
		this.numericUpDown_0.Name = "numericUpDown_0";
		this.numericUpDown_0.Size = new System.Drawing.Size(50, 21);
		this.numericUpDown_0.TabIndex = 48;
		this.label_1.AutoSize = true;
		this.label_1.Location = new System.Drawing.Point(350, 79);
		this.label_1.Name = "label_1";
		this.label_1.Size = new System.Drawing.Size(125, 12);
		this.label_1.TabIndex = 47;
		this.label_1.Text = "循环间隔时间(分钟)：";
		this.groupBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_0.Controls.Add(this.BnameAndAuthorCheckBox);
		this.groupBox_0.Controls.Add(this.ChapterNum);
		this.groupBox_0.Controls.Add(this.label2);
		this.groupBox_0.Controls.Add(this.label1);
		this.groupBox_0.Controls.Add(this.checkBox_15);
		this.groupBox_0.Controls.Add(this.dateTimePicker_1);
		this.groupBox_0.Controls.Add(this.label_25);
		this.groupBox_0.Controls.Add(this.checkBox_14);
		this.groupBox_0.Controls.Add(this.checkBox_1);
		this.groupBox_0.Controls.Add(this.checkBox_0);
		this.groupBox_0.Controls.Add(this.dateTimePicker_0);
		this.groupBox_0.Controls.Add(this.checkBox_2);
		this.groupBox_0.Controls.Add(this.checkBox_4);
		this.groupBox_0.Controls.Add(this.checkBox_3);
		this.groupBox_0.Location = new System.Drawing.Point(6, 6);
		this.groupBox_0.Name = "groupBox_0";
		this.groupBox_0.Size = new System.Drawing.Size(790, 166);
		this.groupBox_0.TabIndex = 49;
		this.groupBox_0.TabStop = false;
		this.groupBox_0.Text = "替换设置";
		this.BnameAndAuthorCheckBox.AutoSize = true;
		this.BnameAndAuthorCheckBox.Location = new System.Drawing.Point(6, 64);
		this.BnameAndAuthorCheckBox.Name = "BnameAndAuthorCheckBox";
		this.BnameAndAuthorCheckBox.Size = new System.Drawing.Size(162, 16);
		this.BnameAndAuthorCheckBox.TabIndex = 59;
		this.BnameAndAuthorCheckBox.Text = "以“作者+书名”识别书籍";
		this.BnameAndAuthorCheckBox.UseVisualStyleBackColor = true;
		this.ChapterNum.Location = new System.Drawing.Point(137, 110);
		this.ChapterNum.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.ChapterNum.Name = "ChapterNum";
		this.ChapterNum.Size = new System.Drawing.Size(69, 21);
		this.ChapterNum.TabIndex = 58;
		this.ChapterNum.Value = new decimal(new int[4] { 50, 0, 0, 0 });
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(212, 115);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(137, 12);
		this.label2.TabIndex = 57;
		this.label2.Text = "个字符的小说(文字章节)";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(4, 115);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(125, 12);
		this.label1.TabIndex = 57;
		this.label1.Text = "替换本地章节字数小于";
		this.checkBox_15.AutoSize = true;
		this.checkBox_15.Checked = true;
		this.checkBox_15.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBox_15.Location = new System.Drawing.Point(6, 86);
		this.checkBox_15.Name = "checkBox_15";
		this.checkBox_15.Size = new System.Drawing.Size(108, 16);
		this.checkBox_15.TabIndex = 56;
		this.checkBox_15.Text = "同时更新章节名";
		this.checkBox_15.UseVisualStyleBackColor = true;
		this.dateTimePicker_1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
		this.dateTimePicker_1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dateTimePicker_1.Location = new System.Drawing.Point(316, 142);
		this.dateTimePicker_1.Name = "dateTimePicker_1";
		this.dateTimePicker_1.Size = new System.Drawing.Size(153, 21);
		this.dateTimePicker_1.TabIndex = 55;
		this.label_25.AutoSize = true;
		this.label_25.Location = new System.Drawing.Point(293, 145);
		this.label_25.Name = "label_25";
		this.label_25.Size = new System.Drawing.Size(17, 12);
		this.label_25.TabIndex = 54;
		this.label_25.Text = "至";
		this.checkBox_14.AutoSize = true;
		this.checkBox_14.Location = new System.Drawing.Point(6, 144);
		this.checkBox_14.Name = "checkBox_14";
		this.checkBox_14.Size = new System.Drawing.Size(120, 16);
		this.checkBox_14.TabIndex = 49;
		this.checkBox_14.Text = "章节入库时间限制";
		this.checkBox_14.UseVisualStyleBackColor = true;
		this.dateTimePicker_0.CustomFormat = "yyyy/MM/dd HH:mm:ss";
		this.dateTimePicker_0.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dateTimePicker_0.Location = new System.Drawing.Point(134, 142);
		this.dateTimePicker_0.Name = "dateTimePicker_0";
		this.dateTimePicker_0.Size = new System.Drawing.Size(153, 21);
		this.dateTimePicker_0.TabIndex = 52;
		this.tabPage_2.Controls.Add(this.groupBox_4);
		this.tabPage_2.Controls.Add(this.groupBox_3);
		this.tabPage_2.Controls.Add(this.groupBox_2);
		this.tabPage_2.Location = new System.Drawing.Point(4, 22);
		this.tabPage_2.Name = "tabPage_2";
		this.tabPage_2.Size = new System.Drawing.Size(802, 288);
		this.tabPage_2.TabIndex = 2;
		this.tabPage_2.Text = "过滤设置";
		this.tabPage_2.UseVisualStyleBackColor = true;
		this.groupBox_4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_4.Controls.Add(this.textBox_9);
		this.groupBox_4.Location = new System.Drawing.Point(199, 111);
		this.groupBox_4.Name = "groupBox_4";
		this.groupBox_4.Size = new System.Drawing.Size(597, 170);
		this.groupBox_4.TabIndex = 4;
		this.groupBox_4.TabStop = false;
		this.groupBox_4.Text = "过滤分卷";
		this.textBox_9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_9.Location = new System.Drawing.Point(3, 20);
		this.textBox_9.Multiline = true;
		this.textBox_9.Name = "textBox_9";
		this.textBox_9.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox_9.Size = new System.Drawing.Size(588, 144);
		this.textBox_9.TabIndex = 0;
		this.groupBox_3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.groupBox_3.Controls.Add(this.textBox_8);
		this.groupBox_3.Controls.Add(this.comboBox_2);
		this.groupBox_3.Location = new System.Drawing.Point(6, 111);
		this.groupBox_3.Name = "groupBox_3";
		this.groupBox_3.Size = new System.Drawing.Size(190, 170);
		this.groupBox_3.TabIndex = 3;
		this.groupBox_3.TabStop = false;
		this.groupBox_3.Text = "限制小说";
		this.textBox_8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_8.Location = new System.Drawing.Point(8, 46);
		this.textBox_8.Multiline = true;
		this.textBox_8.Name = "textBox_8";
		this.textBox_8.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox_8.Size = new System.Drawing.Size(176, 118);
		this.textBox_8.TabIndex = 8;
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
		this.groupBox_2.Controls.Add(this.checkBox_7);
		this.groupBox_2.Location = new System.Drawing.Point(6, 6);
		this.groupBox_2.Name = "groupBox_2";
		this.groupBox_2.Size = new System.Drawing.Size(790, 99);
		this.groupBox_2.TabIndex = 1;
		this.groupBox_2.TabStop = false;
		this.groupBox_2.Text = "章节限制";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(188, 73);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(173, 12);
		this.label3.TabIndex = 9;
		this.label3.Text = "字的章节(只对需要更新的小说)";
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
		this.label_2.Text = "的小说(只对需要更新的小说)";
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
		this.label_3.Text = "不采集需要更新章节超过";
		this.label_4.AutoSize = true;
		this.label_4.Location = new System.Drawing.Point(176, 25);
		this.label_4.Name = "label_4";
		this.label_4.Size = new System.Drawing.Size(125, 12);
		this.label_4.TabIndex = 3;
		this.label_4.Text = "的小说(只对新书而言)";
		this.label_5.AutoSize = true;
		this.label_5.Location = new System.Drawing.Point(6, 25);
		this.label_5.Name = "label_5";
		this.label_5.Size = new System.Drawing.Size(101, 12);
		this.label_5.TabIndex = 2;
		this.label_5.Text = "不采集章节数小于";
		this.numericUpDown_2.Location = new System.Drawing.Point(113, 20);
		this.numericUpDown_2.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown_2.Name = "numericUpDown_2";
		this.numericUpDown_2.Size = new System.Drawing.Size(57, 21);
		this.numericUpDown_2.TabIndex = 0;
		this.checkBox_7.AutoSize = true;
		this.checkBox_7.Location = new System.Drawing.Point(369, 20);
		this.checkBox_7.Name = "checkBox_7";
		this.checkBox_7.Size = new System.Drawing.Size(168, 16);
		this.checkBox_7.TabIndex = 2;
		this.checkBox_7.Text = "全本必采(不考虑以上情况)";
		this.checkBox_7.UseVisualStyleBackColor = true;
		this.tabPage_4.Controls.Add(this.groupBox_9);
		this.tabPage_4.Controls.Add(this.groupBox_8);
		this.tabPage_4.Controls.Add(this.groupBox_7);
		this.tabPage_4.Location = new System.Drawing.Point(4, 22);
		this.tabPage_4.Name = "tabPage_4";
		this.tabPage_4.Size = new System.Drawing.Size(802, 288);
		this.tabPage_4.TabIndex = 4;
		this.tabPage_4.Text = "代理/进度";
		this.tabPage_4.UseVisualStyleBackColor = true;
		this.groupBox_9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_9.Controls.Add(this.richTextBox_0);
		this.groupBox_9.Controls.Add(this.checkBox_13);
		this.groupBox_9.Controls.Add(this.label_15);
		this.groupBox_9.Controls.Add(this.label_16);
		this.groupBox_9.Controls.Add(this.label_17);
		this.groupBox_9.Controls.Add(this.label_18);
		this.groupBox_9.Controls.Add(this.label_19);
		this.groupBox_9.Controls.Add(this.label_20);
		this.groupBox_9.Controls.Add(this.progressBar_0);
		this.groupBox_9.Controls.Add(this.label_21);
		this.groupBox_9.Controls.Add(this.label_22);
		this.groupBox_9.Controls.Add(this.progressBar_1);
		this.groupBox_9.Location = new System.Drawing.Point(6, 116);
		this.groupBox_9.Name = "groupBox_9";
		this.groupBox_9.Size = new System.Drawing.Size(793, 165);
		this.groupBox_9.TabIndex = 52;
		this.groupBox_9.TabStop = false;
		this.groupBox_9.Text = "替换进度";
		this.richTextBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.richTextBox_0.BackColor = System.Drawing.SystemColors.Window;
		this.richTextBox_0.Location = new System.Drawing.Point(381, 20);
		this.richTextBox_0.Name = "richTextBox_0";
		this.richTextBox_0.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
		this.richTextBox_0.Size = new System.Drawing.Size(406, 135);
		this.richTextBox_0.TabIndex = 18;
		this.richTextBox_0.Text = "";
		this.checkBox_13.AutoSize = true;
		this.checkBox_13.Location = new System.Drawing.Point(279, 107);
		this.checkBox_13.Name = "checkBox_13";
		this.checkBox_13.Size = new System.Drawing.Size(96, 16);
		this.checkBox_13.TabIndex = 17;
		this.checkBox_13.Text = "不绘图进度条";
		this.checkBox_13.UseVisualStyleBackColor = true;
		this.label_15.AutoSize = true;
		this.label_15.Location = new System.Drawing.Point(89, 108);
		this.label_15.Name = "label_15";
		this.label_15.Size = new System.Drawing.Size(17, 12);
		this.label_15.TabIndex = 16;
		this.label_15.Text = "--";
		this.label_16.AutoSize = true;
		this.label_16.Location = new System.Drawing.Point(89, 86);
		this.label_16.Name = "label_16";
		this.label_16.Size = new System.Drawing.Size(17, 12);
		this.label_16.TabIndex = 15;
		this.label_16.Text = "--";
		this.label_17.AutoSize = true;
		this.label_17.Location = new System.Drawing.Point(89, 64);
		this.label_17.Name = "label_17";
		this.label_17.Size = new System.Drawing.Size(17, 12);
		this.label_17.TabIndex = 14;
		this.label_17.Text = "--";
		this.label_18.AutoSize = true;
		this.label_18.Location = new System.Drawing.Point(18, 109);
		this.label_18.Name = "label_18";
		this.label_18.Size = new System.Drawing.Size(65, 12);
		this.label_18.TabIndex = 13;
		this.label_18.Text = "当前动作：";
		this.label_19.AutoSize = true;
		this.label_19.Location = new System.Drawing.Point(18, 87);
		this.label_19.Name = "label_19";
		this.label_19.Size = new System.Drawing.Size(65, 12);
		this.label_19.TabIndex = 12;
		this.label_19.Text = "当前章节：";
		this.label_20.AutoSize = true;
		this.label_20.Location = new System.Drawing.Point(18, 65);
		this.label_20.Name = "label_20";
		this.label_20.Size = new System.Drawing.Size(65, 12);
		this.label_20.TabIndex = 11;
		this.label_20.Text = "当前小说：";
		this.progressBar_0.Location = new System.Drawing.Point(89, 43);
		this.progressBar_0.Name = "progressBar_0";
		this.progressBar_0.Size = new System.Drawing.Size(286, 12);
		this.progressBar_0.TabIndex = 10;
		this.label_21.AutoSize = true;
		this.label_21.Location = new System.Drawing.Point(6, 43);
		this.label_21.Name = "label_21";
		this.label_21.Size = new System.Drawing.Size(77, 12);
		this.label_21.TabIndex = 9;
		this.label_21.Text = "采集分进度：";
		this.label_22.AutoSize = true;
		this.label_22.Location = new System.Drawing.Point(6, 21);
		this.label_22.Name = "label_22";
		this.label_22.Size = new System.Drawing.Size(77, 12);
		this.label_22.TabIndex = 8;
		this.label_22.Text = "采集总进度：";
		this.progressBar_1.Location = new System.Drawing.Point(89, 20);
		this.progressBar_1.Name = "progressBar_1";
		this.progressBar_1.Size = new System.Drawing.Size(286, 12);
		this.progressBar_1.TabIndex = 7;
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
		this.groupBox_7.Size = new System.Drawing.Size(625, 104);
		this.groupBox_7.TabIndex = 5;
		this.groupBox_7.TabStop = false;
		this.groupBox_7.Text = "代理IP";
		this.checkBox_12.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.checkBox_12.AutoSize = true;
		this.checkBox_12.Location = new System.Drawing.Point(501, 50);
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
		this.textBox_12.Size = new System.Drawing.Size(406, 21);
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
		this.textBox_13.Location = new System.Drawing.Point(501, 74);
		this.textBox_13.Name = "textBox_13";
		this.textBox_13.Size = new System.Drawing.Size(118, 21);
		this.textBox_13.TabIndex = 12;
		this.textBox_14.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_14.Location = new System.Drawing.Point(89, 74);
		this.textBox_14.Name = "textBox_14";
		this.textBox_14.Size = new System.Drawing.Size(406, 21);
		this.textBox_14.TabIndex = 11;
		this.textBox_15.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_15.Location = new System.Drawing.Point(501, 20);
		this.textBox_15.Name = "textBox_15";
		this.textBox_15.Size = new System.Drawing.Size(118, 21);
		this.textBox_15.TabIndex = 10;
		this.textBox_15.Text = "80";
		this.textBox_16.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_16.Location = new System.Drawing.Point(89, 20);
		this.textBox_16.Name = "textBox_16";
		this.textBox_16.Size = new System.Drawing.Size(406, 21);
		this.textBox_16.TabIndex = 9;
		this.button_2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button_2.Location = new System.Drawing.Point(747, 330);
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
		base.ClientSize = new System.Drawing.Size(834, 365);
		base.Controls.Add(this.button_2);
		base.Controls.Add(this.tabControl_0);
		base.Controls.Add(this.comboBox_0);
		base.Controls.Add(this.button_0);
		base.Controls.Add(this.button_1);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "CollectReplace";
		this.Text = "替换采集模式";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CollectReplace_FormClosing);
		base.Load += new System.EventHandler(CollectReplace_Load);
		this.tabControl_0.ResumeLayout(false);
		this.tabPage_0.ResumeLayout(false);
		this.tabPage_0.PerformLayout();
		this.tabPage_1.ResumeLayout(false);
		this.groupBox_1.ResumeLayout(false);
		this.groupBox_1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).EndInit();
		this.groupBox_0.ResumeLayout(false);
		this.groupBox_0.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ChapterNum).EndInit();
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
		this.tabPage_4.ResumeLayout(false);
		this.groupBox_9.ResumeLayout(false);
		this.groupBox_9.PerformLayout();
		this.groupBox_8.ResumeLayout(false);
		this.groupBox_8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_5).EndInit();
		this.groupBox_7.ResumeLayout(false);
		this.groupBox_7.PerformLayout();
		base.ResumeLayout(false);
	}

	private void method_0()
	{
		try
		{
			comboBox_0.Text = tInfo.RuleFile;
			switch (tInfo.RadioBy)
			{
			case "GetListUrl":
				radioButton_0.Checked = true;
				break;
			case "GetOrderId":
				radioButton_2.Checked = true;
				break;
			case "GetSinceId":
				radioButton_1.Checked = true;
				break;
			case "PutOrderId":
				radioButton_4.Checked = true;
				break;
			case "PutSql":
				radioButton_3.Checked = true;
				break;
			case "PutSinceId":
				radioButton_5.Checked = true;
				break;
			}
			if (tInfo.GetListUrl != null)
			{
				textBox_0.Text = string.Join("\r\n", tInfo.GetListUrl);
			}
			textBox_3.Text = tInfo.GetOrderMinId.ToString();
			textBox_2.Text = tInfo.GetOrderMaxId.ToString();
			if (tInfo.GetSinceId != null)
			{
				textBox_1.Text = string.Join(",", tInfo.GetSinceId);
			}
			textBox_4.Text = tInfo.PutSql;
			textBox_5.Text = tInfo.PutOrderMinId.ToString();
			textBox_6.Text = tInfo.PutOrderMaxId.ToString();
			if (tInfo.PutSinceId != null)
			{
				textBox_7.Text = string.Join(",", tInfo.PutSinceId);
			}
			checkBox_5.Checked = tInfo.Log;
			checkBox_6.Checked = tInfo.Timing;
			numericUpDown_0.Value = tInfo.Interval;
			checkBox_14.Checked = tInfo.AddTime;
			dateTimePicker_0.Value = tInfo.MinAddTime;
			dateTimePicker_1.Value = tInfo.MaxAddTime;
			checkBox_0.Checked = tInfo.ReplacePT;
			checkBox_1.Checked = tInfo.ReplacePP;
			checkBox_2.Checked = tInfo.ReplaceTT;
			checkBox_3.Checked = tInfo.ReplaceTP;
			BnameAndAuthorCheckBox.Checked = tInfo.NameAndAuthor;
			checkBox_4.Checked = tInfo.ReplaceNA;
			ChapterNum.Value = tInfo.ChapterNum;
			comboBox_1.SelectedIndex = tInfo.EqualsChapter;
			comboBox_3.SelectedIndex = tInfo.DownImageError;
			checkBox_15.Checked = tInfo.UpdateChapterName;
			numericUpDown_2.Value = tInfo.MinChapterNum;
			numericUpDown_1.Value = tInfo.FindMaxChapterNum;
			DonotCollectChapterNum.Value = tInfo.ReMoteChapterNum;
			checkBox_7.Checked = tInfo.Finish;
			comboBox_2.SelectedIndex = tInfo.FilterNovelType;
			if (tInfo.FilterVolume != null)
			{
				textBox_9.Text = string.Join("\r\n", tInfo.FilterVolume);
			}
			if (tInfo.FilterNovel != null)
			{
				textBox_8.Text = string.Join("\r\n", tInfo.FilterNovel);
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

	private void method_1()
	{
		try
		{
			tInfo.RuleFile = comboBox_0.Text;
			if (radioButton_0.Checked)
			{
				tInfo.RadioBy = "GetListUrl";
			}
			if (radioButton_2.Checked)
			{
				tInfo.RadioBy = "GetOrderId";
			}
			if (radioButton_1.Checked)
			{
				tInfo.RadioBy = "GetSinceId";
			}
			if (radioButton_4.Checked)
			{
				tInfo.RadioBy = "PutOrderId";
			}
			if (radioButton_5.Checked)
			{
				tInfo.RadioBy = "PutSinceId";
			}
			if (radioButton_3.Checked)
			{
				tInfo.RadioBy = "PutSql";
			}
			tInfo.GetListUrl = textBox_0.Text.Trim().Replace("\r\n", "♂").Split('♂');
			tInfo.GetOrderMinId = Convert.ToInt32(textBox_3.Text);
			tInfo.GetOrderMaxId = Convert.ToInt32(textBox_2.Text);
			tInfo.GetSinceId = textBox_1.Text.Split(',');
			tInfo.PutSql = textBox_4.Text;
			tInfo.PutOrderMinId = Convert.ToInt32(textBox_5.Text);
			tInfo.PutOrderMaxId = Convert.ToInt32(textBox_6.Text);
			tInfo.PutSinceId = textBox_7.Text.Split(',');
			tInfo.Log = checkBox_5.Checked;
			tInfo.Timing = checkBox_6.Checked;
			tInfo.Interval = Convert.ToInt32(numericUpDown_0.Value);
			tInfo.ReplacePT = checkBox_0.Checked;
			tInfo.ReplacePP = checkBox_1.Checked;
			tInfo.ReplaceTT = checkBox_2.Checked;
			tInfo.ReplaceTP = checkBox_3.Checked;
			tInfo.NameAndAuthor = BnameAndAuthorCheckBox.Checked;
			tInfo.ReplaceNA = checkBox_4.Checked;
			tInfo.EqualsChapter = comboBox_1.SelectedIndex;
			tInfo.ChapterNum = Convert.ToInt32(ChapterNum.Value);
			tInfo.DownImageError = comboBox_3.SelectedIndex;
			tInfo.UpdateChapterName = checkBox_15.Checked;
			tInfo.AddTime = checkBox_14.Checked;
			tInfo.MinAddTime = dateTimePicker_0.Value;
			tInfo.MaxAddTime = dateTimePicker_1.Value;
			tInfo.MinChapterNum = Convert.ToInt32(numericUpDown_2.Value);
			tInfo.FindMaxChapterNum = Convert.ToInt32(numericUpDown_1.Value);
			tInfo.ReMoteChapterNum = Convert.ToInt32(DonotCollectChapterNum.Value);
			tInfo.Finish = checkBox_7.Checked;
			tInfo.FilterNovelType = comboBox_2.SelectedIndex;
			tInfo.FilterVolume = textBox_9.Text.Replace("\r\n", "♂").Split('♂');
			tInfo.FilterNovel = textBox_8.Text.Replace("\r\n", "♂").Split('♂');
			tInfo.ChapterNum = Convert.ToInt32(ChapterNum.Value);
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

	private void method_2(string[] string_3, bool bool_1)
	{
		for (int i = 0; i < string_3.Length; i++)
		{
			if (backgroundWorker_0.CancellationPending)
			{
				break;
			}
			backgroundWorker_0.ReportProgress(3, i + 1);
			NovelInfo novelInfo = new NovelInfo();
			if (bool_1)
			{
				novelInfo.GetID = string_3[i];
			}
			else
			{
				novelInfo.PutID = Convert.ToInt32(string_3[i]);
			}
			method_4(novelInfo);
		}
	}

	private void method_3(int int_0, int int_1, bool bool_1)
	{
		for (int i = int_0; i <= int_1; i++)
		{
			if (backgroundWorker_0.CancellationPending)
			{
				break;
			}
			backgroundWorker_0.ReportProgress(3, i - int_0 + 1);
			NovelInfo novelInfo = new NovelInfo();
			if (bool_1)
			{
				novelInfo.GetID = i.ToString();
			}
			else
			{
				novelInfo.PutID = i;
			}
			method_4(novelInfo);
		}
	}

	private void method_4(NovelInfo novelInfo_0)
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
			SpiderException.Show(200, ex.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
			return;
		}
		try
		{
			var keys = Configs.TaskNovelInfo.Keys;
			if (Configs.TaskNovelInfo.Count != 0)
			{
				IEnumerator enumerator = keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Current;
					if (text != string_2 && Configs.TaskNovelInfo[text] != null && ((NovelInfo)Configs.TaskNovelInfo[text]).Name == novelInfo_0.Name)
					{
						SpiderException.Show(101, "子窗口冲突", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						return;
					}
				}
			}
		}
		catch
		{
			SpiderException.Show(102, "检查子窗口冲突失败", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
			return;
		}
		Configs.TaskNovelInfo[string_2.ToString()] = novelInfo_0;
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
						SpiderException.Show(134, "限制小说_黑名单", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
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
				SpiderException.Show(135, "限制小说_不在白名单", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
				return;
			}
		}
		if (backgroundWorker_0.CancellationPending)
		{
			return;
		}
		SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 获得小说的章节目录");
		ChapterInfo[] chapterList;
		ChapterInfo[] chapterList2;
		try
		{
			backgroundWorker_0.ReportProgress(2, "获得小说的章节目录");
			chapterList = page.GetChapterList(novelInfo_0);
			chapterList2 = LocalProvider.GetInstance().GetChapterList(novelInfo_0.PutID);
		}
		catch (Exception ex2)
		{
			SpiderException.Show(210, ex2.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
			return;
		}
		if (chapterList == null || chapterList2 == null || chapterList.Length == 0 || chapterList2.Length == 0)
		{
			SpiderException.Show(214, "章节组为空", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
			return;
		}
		backgroundWorker_0.ReportProgress(6, chapterList2.Length);
		if (backgroundWorker_0.CancellationPending)
		{
			return;
		}
		bool flag2 = false;
		for (int k = 0; k < chapterList2.Length; k++)
		{
			if (backgroundWorker_0.CancellationPending)
			{
				break;
			}
			backgroundWorker_0.ReportProgress(4, k + 1);
			if (tInfo.AddTime)
			{
				chapterList2[k] = LocalProvider.GetInstance().GetChapterInfo(novelInfo_0.PutID, chapterList2[k].PutID);
				if (chapterList2[k].LastTime <= tInfo.MinAddTime || chapterList2[k].LastTime >= tInfo.MaxAddTime)
				{
					continue;
				}
				bool flag3 = false;
				SpiderException.Debug("--------------------------\r\n" + chapterList2[k].ChapterName + "\r\n" + chapterList2[k].ChapterText + "\r\n");
				int num;
				if (!Regex.IsMatch(chapterList2[k].ChapterText, "\\[img\\]", RegexOptions.IgnoreCase) && !Regex.IsMatch(chapterList2[k].ChapterText, "<img", RegexOptions.IgnoreCase))
				{
					if (chapterList2[k].ChapterText != null && chapterList2[k].ChapterText != "")
					{
						num = 1;
						backgroundWorker_0.ReportProgress(1, chapterList2[k].ChapterName + " <本文>");
					}
					else
					{
						num = 0;
						backgroundWorker_0.ReportProgress(1, chapterList2[k].ChapterName + " <本空>");
					}
				}
				else if (chapterList2[k].ChapterText.Length <= tInfo.ChapterNum)
				{
					num = 0;
					backgroundWorker_0.ReportProgress(1, chapterList2[k].ChapterName + " <本少>");
				}
				else if (chapterList2[k].ChapterText.Length <= tInfo.ReMoteChapterNum)
				{
					num = 1;
					backgroundWorker_0.ReportProgress(1, chapterList2[k].ChapterName + " <远少>");
				}
				else
				{
					num = 2;
					backgroundWorker_0.ReportProgress(1, chapterList2[k].ChapterName + " <本图>");
				}
				if (num == 0 && tInfo.ReplaceNA)
				{
					flag3 = true;
				}
				if (num == 1 && (tInfo.ReplaceTP || tInfo.ReplaceTT))
				{
					flag3 = true;
				}
				if (num == 2 && (tInfo.ReplacePP || tInfo.ReplacePT))
				{
					flag3 = true;
				}
				if (backgroundWorker_0.CancellationPending)
				{
					break;
				}
				if (!flag3)
				{
					continue;
				}
				int num2 = -1;
				bool flag4 = false;
				int num3 = 0;
				backgroundWorker_0.ReportProgress(2, "正在对比");
				for (int l = 0; l < chapterList.Length; l++)
				{
					switch (tInfo.EqualsChapter)
					{
					case 0:
						if (chapterList[l].ChapterName == chapterList2[k].ChapterName)
						{
							num2 = l;
							flag4 = true;
						}
						break;
					case 1:
						if (chapterList[l].ChapterName == chapterList2[k].ChapterName && chapterList[l].VolumeName == chapterList2[k].VolumeName)
						{
							num2 = l;
							flag4 = true;
						}
						break;
					case 2:
						if (FormatText.CompareToChapter(chapterList[l].ChapterName, chapterList2[k].ChapterName))
						{
							num2 = l;
							flag4 = true;
						}
						break;
					case 3:
					{
						int num5 = FormatText.CompareToChapter2(chapterList[l].ChapterName, chapterList2[k].ChapterName, chapterList[l].VolumeName, chapterList2[k].VolumeName);
						if (num5 > 3 && num5 >= num3)
						{
							num3 = num5;
							num2 = l;
							flag4 = true;
						}
						break;
					}
					case 4:
					{
						int num4 = FormatText.CompareToChapter3(chapterList[l].ChapterName, chapterList2[k].ChapterName, chapterList[l].VolumeName, chapterList2[k].VolumeName);
						if (num4 > 0 && num4 >= num3)
						{
							num3 = num4;
							num2 = l;
							flag4 = true;
						}
						break;
					}
					}
					if (tInfo.EqualsChapter < 3 && flag4)
					{
						break;
					}
				}
				if (backgroundWorker_0.CancellationPending)
				{
					break;
				}
				if (!flag4)
				{
					backgroundWorker_0.ReportProgress(2, "对比失败");
					continue;
				}
				backgroundWorker_0.ReportProgress(2, "对比成功");
				novelInfo_0.LastChapter = chapterList[num2];
				try
				{
					novelInfo_0 = page.GetChapterInfo(novelInfo_0, isvip: false);
				}
				catch (Exception ex3)
				{
					SpiderException.Show(220, ex3.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
					break;
				}
				int num6;
				if (novelInfo_0.LastChapter.ChapterText != null && novelInfo_0.LastChapter.ChapterText != "")
				{
					backgroundWorker_0.ReportProgress(2, novelInfo_0.LastChapter.ChapterName + " <他文>");
					num6 = 1;
				}
				else
				{
					backgroundWorker_0.ReportProgress(2, novelInfo_0.LastChapter.ChapterName + " <他空>");
					num6 = 0;
				}
				bool flag5 = false;
				if ((num == 0) & tInfo.ReplaceNA)
				{
					flag5 = true;
				}
				if (num == 1 && num6 == 1 && tInfo.ReplaceTT)
				{
					flag5 = true;
				}
				if (num == 2 && num6 == 1 && tInfo.ReplacePT)
				{
					flag5 = true;
				}
				if (num == 1 && num6 == 2 && tInfo.ReplaceTP)
				{
					flag5 = true;
				}
				if (num == 2 && num6 == 2 && tInfo.ReplacePP)
				{
					flag5 = true;
				}
				if (flag5)
				{
					flag2 = true;
					backgroundWorker_0.ReportProgress(2, "执行替换" + novelInfo_0.LastChapter.ChapterName);
					novelInfo_0.LastChapter.PutID = chapterList2[k].PutID;
					try
					{
						LocalProvider.GetInstance().UpdateChapter(novelInfo_0, tInfo);
					}
					catch (Exception ex4)
					{
						SpiderException.Show(442, ex4.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
					}
					if (Configs.BaseConfig.ChapterHtml)
					{
						backgroundWorker_0.ReportProgress(2, "重新生成章节Html");
						LocalProvider.GetInstance().CreateChapter(novelInfo_0);
					}
				}
				continue;
			}
			bool flag6 = false;
			chapterList2[k] = LocalProvider.GetInstance().GetChapterInfo(novelInfo_0.PutID, chapterList2[k].PutID);
			SpiderException.Debug("--------------------------\r\n" + chapterList2[k].ChapterName + "\r\n" + chapterList2[k].ChapterText + "\r\n");
			int num7;
			if (chapterList2[k].ChapterText != null && chapterList2[k].ChapterText != "" && chapterList2[k].ChapterText.Length > tInfo.ChapterNum)
			{
				num7 = 1;
				backgroundWorker_0.ReportProgress(1, chapterList2[k].ChapterName + " <本文>");
			}
			else if (chapterList2[k].ChapterText.Length <= tInfo.ChapterNum)
			{
				num7 = 0;
				backgroundWorker_0.ReportProgress(1, chapterList2[k].ChapterName + " <本少>");
			}
			else if (chapterList2[k].ChapterText.Length <= tInfo.ReMoteChapterNum)
			{
				num7 = 1;
				backgroundWorker_0.ReportProgress(1, chapterList2[k].ChapterName + " <远少>");
			}
			else
			{
				num7 = 0;
				backgroundWorker_0.ReportProgress(1, chapterList2[k].ChapterName + " <本空>");
			}
			if (num7 == 0 && tInfo.ReplaceNA)
			{
				flag6 = true;
			}
			if (num7 == 1 && (tInfo.ReplaceTP || tInfo.ReplaceTT))
			{
				flag6 = true;
			}
			if (num7 == 2 && (tInfo.ReplacePP || tInfo.ReplacePT))
			{
				flag6 = true;
			}
			if (backgroundWorker_0.CancellationPending)
			{
				break;
			}
			if (!flag6)
			{
				continue;
			}
			int num8 = -1;
			bool flag7 = false;
			int num9 = 0;
			backgroundWorker_0.ReportProgress(2, "正在对比");
			for (int m = 0; m < chapterList.Length; m++)
			{
				switch (tInfo.EqualsChapter)
				{
				case 0:
					if (chapterList[m].ChapterName == chapterList2[k].ChapterName)
					{
						num8 = m;
						flag7 = true;
					}
					break;
				case 1:
					if (chapterList[m].ChapterName == chapterList2[k].ChapterName && chapterList[m].VolumeName == chapterList2[k].VolumeName)
					{
						num8 = m;
						flag7 = true;
					}
					break;
				case 2:
					if (FormatText.CompareToChapter(chapterList[m].ChapterName, chapterList2[k].ChapterName))
					{
						num8 = m;
						flag7 = true;
					}
					break;
				case 3:
				{
					int num11 = FormatText.CompareToChapter2(chapterList[m].ChapterName, chapterList2[k].ChapterName, chapterList[m].VolumeName, chapterList2[k].VolumeName);
					if (num11 > 3 && num11 >= num9)
					{
						num9 = num11;
						num8 = m;
						flag7 = true;
					}
					break;
				}
				case 4:
				{
					int num10 = FormatText.CompareToChapter3(chapterList[m].ChapterName, chapterList2[k].ChapterName, chapterList[m].VolumeName, chapterList2[k].VolumeName);
					if (num10 > 0 && num10 >= num9)
					{
						num9 = num10;
						num8 = m;
						flag7 = true;
					}
					break;
				}
				}
				if (tInfo.EqualsChapter < 3 && flag7)
				{
					break;
				}
			}
			if (backgroundWorker_0.CancellationPending)
			{
				break;
			}
			if (!flag7)
			{
				backgroundWorker_0.ReportProgress(2, "对比失败");
				continue;
			}
			backgroundWorker_0.ReportProgress(2, "对比成功");
			novelInfo_0.LastChapter = chapterList[num8];
			try
			{
				novelInfo_0 = page.GetChapterInfo(novelInfo_0, isvip: false);
			}
			catch (Exception ex5)
			{
				SpiderException.Show(220, ex5.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
				break;
			}
			int num12;
			if (novelInfo_0.LastChapter.ChapterText != null && novelInfo_0.LastChapter.ChapterText != "")
			{
				backgroundWorker_0.ReportProgress(2, novelInfo_0.LastChapter.ChapterName + " <他文>");
				num12 = 1;
			}
			else
			{
				backgroundWorker_0.ReportProgress(2, novelInfo_0.LastChapter.ChapterName + " <他空>");
				num12 = 0;
			}
			bool flag8 = false;
			if ((num7 == 0) & tInfo.ReplaceNA)
			{
				flag8 = true;
			}
			if (num7 == 1 && num12 == 1 && tInfo.ReplaceTT)
			{
				flag8 = true;
			}
			if (num7 == 2 && num12 == 1 && tInfo.ReplacePT)
			{
				flag8 = true;
			}
			if (num7 == 1 && num12 == 2 && tInfo.ReplaceTP)
			{
				flag8 = true;
			}
			if (num7 == 2 && num12 == 2 && tInfo.ReplacePP)
			{
				flag8 = true;
			}
			if (flag8)
			{
				flag2 = true;
				backgroundWorker_0.ReportProgress(2, "执行替换" + novelInfo_0.LastChapter.ChapterName);
				novelInfo_0.LastChapter.PutID = chapterList2[k].PutID;
				try
				{
					LocalProvider.GetInstance().UpdateChapter(novelInfo_0, tInfo);
				}
				catch (Exception ex6)
				{
					SpiderException.Show(442, ex6.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
				}
				if (Configs.BaseConfig.ChapterHtml)
				{
					backgroundWorker_0.ReportProgress(2, "重新生成章节Html");
					LocalProvider.GetInstance().CreateChapter(novelInfo_0);
				}
			}
		}
		if (flag2)
		{
			LocalProvider.GetInstance().UpdateLastChapter(novelInfo_0);
			LocalProvider.GetInstance().CreateIndex(novelInfo_0, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
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
}
