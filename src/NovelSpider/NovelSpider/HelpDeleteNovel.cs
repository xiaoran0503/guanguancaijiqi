using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NovelSpider.Config;
using NovelSpider.Entity;
using NovelSpider.Local;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class HelpDeleteNovel : DockContent
{
	private BackgroundWorker backgroundWorker_0;

	private bool bool_0;

	private Button button_0;

	private Button button_1;

	private Button button_2;

	private CheckBox checkBox_0;

	private CheckBox checkBox_1;

	private CheckBox checkBox_2;

	private CheckBox checkBox_3;

	private CheckBox checkBox_4;

	private CheckBox checkBox_5;

	private CheckBox checkBox_6;

	private CheckBox checkBox_7;

	private CheckBox checkBox1;

	private DateTimePicker dateTimePicker_0;

	private DateTimePicker dateTimePicker_1;

	private GroupBox groupBox_0;

	private GroupBox groupBox_1;

	private IContainer icontainer_0;

	private int int_0;

	private Label label_0;

	private Label label_1;

	private Label label_2;

	private Label label_3;

	private MaskedTextBox maskedTextBox1;

	private NumericUpDown numericUpDown_0;

	private NumericUpDown numericUpDown_1;

	private NumericUpDown numericUpDown_2;

	private NumericUpDown numericUpDown_3;

	private NumericUpDown numericUpDown_4;

	private NumericUpDown numericUpDown_5;

	private TextBox textBox_0;

	public HelpDeleteNovel()
	{
		InitializeComponent();
	}

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		if (textBox_0.Text == "")
		{
			MessageBox.Show("请输入自定义SQL，选择单本或批量方式的请先生成自定义SQL。");
			return;
		}
		NovelInfo[] novelList = NovelSpider.Local.LocalProvider.GetInstance().GetNovelList(textBox_0.Text.ToString());
		for (int j = 0; j < novelList.Length; j++)
		{
			if (backgroundWorker_0.CancellationPending)
			{
				e.Cancel = true;
				break;
			}
			int putID = novelList[j].PutID;
			backgroundWorker_0.ReportProgress(0, putID + " | " + novelList[j].Name.ToString());
			backgroundWorker_0.ReportProgress(1, j + 1 + " / " + novelList.Length);
			if (!bool_0 || NovelSpider.Local.LocalProvider.GetInstance().GetChapterList(putID).Length < int_0)
			{
				NovelInfo novelInfo = new NovelInfo
				{
					PutID = putID
				};
				NovelInfo novelInfo_ = novelInfo;
				NovelSpider.Local.LocalProvider.GetInstance().ClearNovel(novelInfo_);
				NovelSpider.Local.LocalProvider.GetInstance().DeteleNovel(putID);
				File.AppendAllText("Delete.log", putID + " | " + novelList[j].Name.ToString() + "\r\n");
			}
		}
	}

	private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		if (e.ProgressPercentage == 0)
		{
			label_0.Text = "当前小说：" + e.UserState.ToString();
		}
		else
		{
			label_3.Text = "当前进度：" + e.UserState.ToString();
		}
	}

	private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			MessageBox.Show(e.Error.Message, "错误提示");
			label_1.Text = "当前动作：发生错误";
		}
		else if (e.Cancelled)
		{
			label_1.Text = "当前动作：操作取消";
		}
		else
		{
			label_1.Text = "当前动作：操作完成";
		}
		button_0.Enabled = true;
		button_1.Enabled = false;
		groupBox_0.Enabled = true;
	}

	private void button_0_Click(object sender, EventArgs e)
	{
		if (!(Configs.CmsName == "UnsupportedCms") && !backgroundWorker_0.IsBusy)
		{
			bool_0 = checkBox_5.Checked;
			int_0 = Convert.ToInt32(numericUpDown_1.Value);
			groupBox_0.Enabled = false;
			button_0.Enabled = false;
			button_1.Enabled = true;
			backgroundWorker_0.RunWorkerAsync(textBox_0.Text);
		}
		else
		{
			bool_0 = checkBox_5.Checked;
			int_0 = Convert.ToInt32(numericUpDown_1.Value);
			groupBox_0.Enabled = false;
			button_0.Enabled = false;
			button_1.Enabled = true;
			backgroundWorker_0.RunWorkerAsync(textBox_0.Text);
		}
	}

	private void button_1_Click(object sender, EventArgs e)
	{
		if (backgroundWorker_0.IsBusy)
		{
			backgroundWorker_0.CancelAsync();
			button_1.Enabled = false;
		}
	}

	private void button_2_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "UnsupportedCms")
		{
			bool flag = false;
			string text = "Select id,booktitle From WS_BookList Where";
			if (checkBox_4.Checked)
			{
				if (flag)
				{
					text += " And";
				}
				text = text + " id>=" + numericUpDown_4.Value;
				flag = true;
			}
			if (checkBox_3.Checked)
			{
				if (flag)
				{
					text += " And";
				}
				text = text + " id<=" + numericUpDown_3.Value;
				flag = true;
			}
			if (checkBox_6.Checked)
			{
				if (flag)
				{
					text += " And";
				}
				text = text + " bookhits<" + numericUpDown_0.Value;
				flag = true;
			}
			if (checkBox_2.Checked)
			{
				if (flag)
				{
					text += " And";
				}
				text = text + " bookrecom<" + numericUpDown_2.Value;
				flag = true;
			}
			if (checkBox_0.Checked)
			{
				if (flag)
				{
					text += " And";
				}
				text = text + " bookaddtime<'" + dateTimePicker_1.Value.ToShortDateString() + "'";
				flag = true;
			}
			if (checkBox_1.Checked)
			{
				if (flag)
				{
					text += " And";
				}
				text = text + " bookupdatetime<'" + dateTimePicker_0.Value.ToShortDateString() + "'";
			}
			textBox_0.Text = text + " Order By id Asc";
			return;
		}
		bool flag2 = false;
		string text2 = "SELECT * FROM `jieqi_article_article` WHERE";
		if (checkBox_4.Checked)
		{
			if (flag2)
			{
				text2 += " And";
			}
			text2 = text2 + " `articleid` >= '" + numericUpDown_4.Value + "'";
			flag2 = true;
		}
		if (checkBox_3.Checked)
		{
			if (flag2)
			{
				text2 += " And";
			}
			text2 = text2 + " `articleid` <= '" + numericUpDown_3.Value + "'";
			flag2 = true;
		}
		if (checkBox_7.Checked)
		{
			if (flag2)
			{
				text2 += " And";
			}
			text2 = text2 + " `articleid` = '" + numericUpDown_5.Value + "'";
			flag2 = true;
		}
		if (checkBox_5.Checked)
		{
			if (flag2)
			{
				text2 += " And";
			}
			text2 = text2 + " `chapters` < '" + numericUpDown_1.Value + "'";
			flag2 = true;
		}
		if (checkBox_6.Checked)
		{
			if (flag2)
			{
				text2 += " And";
			}
			text2 = text2 + " `allvisit` < '" + numericUpDown_0.Value + "'";
			flag2 = true;
		}
		if (checkBox_2.Checked)
		{
			if (flag2)
			{
				text2 += " And";
			}
			text2 = text2 + " `allvote` < '" + numericUpDown_2.Value + "'";
			flag2 = true;
		}
		if (checkBox_0.Checked)
		{
			if (flag2)
			{
				text2 += " And";
			}
			text2 = text2 + " `postdate` < '" + dateTimePicker_1.Value.ToShortDateString() + "'";
			flag2 = true;
		}
		if (checkBox_1.Checked)
		{
			if (flag2)
			{
				text2 += " And";
			}
			text2 = text2 + " `lastupdate` < '" + dateTimePicker_0.Value.ToShortDateString() + "'";
		}
		if (checkBox1.Checked)
		{
			if (flag2)
			{
				text2 += " And";
			}
			text2 = text2 + " `articleid` in(" + maskedTextBox1.Text.ToString() + ")";
			flag2 = true;
		}
		textBox_0.Text = text2 + " Order By `articleid` Asc";
	}

	private void checkBox_7_CheckedChanged(object sender, EventArgs e)
	{
		checkBox_0.Checked = true;
		checkBox_0.Checked = false;
		checkBox_1.Checked = false;
		checkBox_2.Checked = false;
		checkBox_3.Checked = false;
		checkBox_4.Checked = false;
		checkBox_5.Checked = false;
		checkBox_6.Checked = false;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.HelpDeleteNovel));
		this.groupBox_0 = new System.Windows.Forms.GroupBox();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.maskedTextBox1 = new System.Windows.Forms.MaskedTextBox();
		this.label_2 = new System.Windows.Forms.Label();
		this.button_2 = new System.Windows.Forms.Button();
		this.textBox_0 = new System.Windows.Forms.TextBox();
		this.checkBox_4 = new System.Windows.Forms.CheckBox();
		this.checkBox_5 = new System.Windows.Forms.CheckBox();
		this.checkBox_6 = new System.Windows.Forms.CheckBox();
		this.checkBox_0 = new System.Windows.Forms.CheckBox();
		this.checkBox_2 = new System.Windows.Forms.CheckBox();
		this.checkBox_1 = new System.Windows.Forms.CheckBox();
		this.checkBox_7 = new System.Windows.Forms.CheckBox();
		this.checkBox_3 = new System.Windows.Forms.CheckBox();
		this.dateTimePicker_0 = new System.Windows.Forms.DateTimePicker();
		this.dateTimePicker_1 = new System.Windows.Forms.DateTimePicker();
		this.numericUpDown_3 = new System.Windows.Forms.NumericUpDown();
		this.numericUpDown_4 = new System.Windows.Forms.NumericUpDown();
		this.numericUpDown_2 = new System.Windows.Forms.NumericUpDown();
		this.numericUpDown_5 = new System.Windows.Forms.NumericUpDown();
		this.numericUpDown_0 = new System.Windows.Forms.NumericUpDown();
		this.numericUpDown_1 = new System.Windows.Forms.NumericUpDown();
		this.groupBox_1 = new System.Windows.Forms.GroupBox();
		this.label_0 = new System.Windows.Forms.Label();
		this.label_1 = new System.Windows.Forms.Label();
		this.label_3 = new System.Windows.Forms.Label();
		this.button_0 = new System.Windows.Forms.Button();
		this.button_1 = new System.Windows.Forms.Button();
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		this.groupBox_0.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_5).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).BeginInit();
		this.groupBox_1.SuspendLayout();
		base.SuspendLayout();
		this.groupBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_0.Controls.Add(this.checkBox1);
		this.groupBox_0.Controls.Add(this.maskedTextBox1);
		this.groupBox_0.Controls.Add(this.label_2);
		this.groupBox_0.Controls.Add(this.button_2);
		this.groupBox_0.Controls.Add(this.textBox_0);
		this.groupBox_0.Controls.Add(this.checkBox_4);
		this.groupBox_0.Controls.Add(this.checkBox_5);
		this.groupBox_0.Controls.Add(this.checkBox_6);
		this.groupBox_0.Controls.Add(this.checkBox_0);
		this.groupBox_0.Controls.Add(this.checkBox_2);
		this.groupBox_0.Controls.Add(this.checkBox_1);
		this.groupBox_0.Controls.Add(this.checkBox_7);
		this.groupBox_0.Controls.Add(this.checkBox_3);
		this.groupBox_0.Controls.Add(this.dateTimePicker_0);
		this.groupBox_0.Controls.Add(this.dateTimePicker_1);
		this.groupBox_0.Controls.Add(this.numericUpDown_3);
		this.groupBox_0.Controls.Add(this.numericUpDown_4);
		this.groupBox_0.Controls.Add(this.numericUpDown_2);
		this.groupBox_0.Controls.Add(this.numericUpDown_5);
		this.groupBox_0.Controls.Add(this.numericUpDown_0);
		this.groupBox_0.Controls.Add(this.numericUpDown_1);
		this.groupBox_0.Location = new System.Drawing.Point(12, 12);
		this.groupBox_0.Name = "groupBox_0";
		this.groupBox_0.Size = new System.Drawing.Size(608, 248);
		this.groupBox_0.TabIndex = 16;
		this.groupBox_0.TabStop = false;
		this.groupBox_0.Text = "限制条件";
		this.checkBox1.AutoSize = true;
		this.checkBox1.Location = new System.Drawing.Point(8, 102);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(60, 16);
		this.checkBox1.TabIndex = 32;
		this.checkBox1.Text = "自定义";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.maskedTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.maskedTextBox1.Location = new System.Drawing.Point(98, 100);
		this.maskedTextBox1.Name = "maskedTextBox1";
		this.maskedTextBox1.Size = new System.Drawing.Size(498, 21);
		this.maskedTextBox1.TabIndex = 31;
		this.label_2.AutoSize = true;
		this.label_2.Location = new System.Drawing.Point(240, 132);
		this.label_2.Name = "label_2";
		this.label_2.Size = new System.Drawing.Size(155, 12);
		this.label_2.TabIndex = 30;
		this.label_2.Text = "PS:章节数在执行过程中判断";
		this.button_2.Location = new System.Drawing.Point(6, 127);
		this.button_2.Name = "button_2";
		this.button_2.Size = new System.Drawing.Size(126, 23);
		this.button_2.TabIndex = 29;
		this.button_2.Text = "↓生成SQL语句↓";
		this.button_2.UseVisualStyleBackColor = true;
		this.button_2.Click += new System.EventHandler(button_2_Click);
		this.textBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_0.Location = new System.Drawing.Point(8, 156);
		this.textBox_0.Multiline = true;
		this.textBox_0.Name = "textBox_0";
		this.textBox_0.Size = new System.Drawing.Size(588, 86);
		this.textBox_0.TabIndex = 28;
		this.checkBox_4.AutoSize = true;
		this.checkBox_4.Location = new System.Drawing.Point(8, 20);
		this.checkBox_4.Name = "checkBox_4";
		this.checkBox_4.Size = new System.Drawing.Size(84, 16);
		this.checkBox_4.TabIndex = 27;
		this.checkBox_4.Text = "最小小说ID";
		this.checkBox_4.UseVisualStyleBackColor = true;
		this.checkBox_5.AutoSize = true;
		this.checkBox_5.Location = new System.Drawing.Point(8, 47);
		this.checkBox_5.Name = "checkBox_5";
		this.checkBox_5.Size = new System.Drawing.Size(84, 16);
		this.checkBox_5.TabIndex = 26;
		this.checkBox_5.Text = "章节数小于";
		this.checkBox_5.UseVisualStyleBackColor = true;
		this.checkBox_6.AutoSize = true;
		this.checkBox_6.Location = new System.Drawing.Point(204, 51);
		this.checkBox_6.Name = "checkBox_6";
		this.checkBox_6.Size = new System.Drawing.Size(84, 16);
		this.checkBox_6.TabIndex = 25;
		this.checkBox_6.Text = "点击数小于";
		this.checkBox_6.UseVisualStyleBackColor = true;
		this.checkBox_0.AutoSize = true;
		this.checkBox_0.Location = new System.Drawing.Point(8, 76);
		this.checkBox_0.Name = "checkBox_0";
		this.checkBox_0.Size = new System.Drawing.Size(72, 16);
		this.checkBox_0.TabIndex = 24;
		this.checkBox_0.Text = "入库时间";
		this.checkBox_0.UseVisualStyleBackColor = true;
		this.checkBox_2.AutoSize = true;
		this.checkBox_2.Location = new System.Drawing.Point(401, 51);
		this.checkBox_2.Name = "checkBox_2";
		this.checkBox_2.Size = new System.Drawing.Size(84, 16);
		this.checkBox_2.TabIndex = 22;
		this.checkBox_2.Text = "推荐数小于";
		this.checkBox_2.UseVisualStyleBackColor = true;
		this.checkBox_1.AutoSize = true;
		this.checkBox_1.Location = new System.Drawing.Point(205, 76);
		this.checkBox_1.Name = "checkBox_1";
		this.checkBox_1.Size = new System.Drawing.Size(72, 16);
		this.checkBox_1.TabIndex = 23;
		this.checkBox_1.Text = "最后更新";
		this.checkBox_1.UseVisualStyleBackColor = true;
		this.checkBox_7.AutoSize = true;
		this.checkBox_7.Location = new System.Drawing.Point(401, 24);
		this.checkBox_7.Name = "checkBox_7";
		this.checkBox_7.Size = new System.Drawing.Size(84, 16);
		this.checkBox_7.TabIndex = 21;
		this.checkBox_7.Text = "小说ID等于";
		this.checkBox_7.UseVisualStyleBackColor = true;
		this.checkBox_7.CheckedChanged += new System.EventHandler(checkBox_7_CheckedChanged);
		this.checkBox_3.AutoSize = true;
		this.checkBox_3.Location = new System.Drawing.Point(205, 20);
		this.checkBox_3.Name = "checkBox_3";
		this.checkBox_3.Size = new System.Drawing.Size(84, 16);
		this.checkBox_3.TabIndex = 20;
		this.checkBox_3.Text = "最大小说ID";
		this.checkBox_3.UseVisualStyleBackColor = true;
		this.dateTimePicker_0.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dateTimePicker_0.Location = new System.Drawing.Point(295, 73);
		this.dateTimePicker_0.Name = "dateTimePicker_0";
		this.dateTimePicker_0.Size = new System.Drawing.Size(100, 21);
		this.dateTimePicker_0.TabIndex = 19;
		this.dateTimePicker_1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dateTimePicker_1.Location = new System.Drawing.Point(98, 73);
		this.dateTimePicker_1.Name = "dateTimePicker_1";
		this.dateTimePicker_1.Size = new System.Drawing.Size(100, 21);
		this.dateTimePicker_1.TabIndex = 17;
		this.numericUpDown_3.Location = new System.Drawing.Point(295, 19);
		this.numericUpDown_3.Maximum = new decimal(new int[4] { 100000000, 0, 0, 0 });
		this.numericUpDown_3.Name = "numericUpDown_3";
		this.numericUpDown_3.Size = new System.Drawing.Size(100, 21);
		this.numericUpDown_3.TabIndex = 11;
		this.numericUpDown_4.Location = new System.Drawing.Point(98, 19);
		this.numericUpDown_4.Maximum = new decimal(new int[4] { 100000000, 0, 0, 0 });
		this.numericUpDown_4.Name = "numericUpDown_4";
		this.numericUpDown_4.Size = new System.Drawing.Size(100, 21);
		this.numericUpDown_4.TabIndex = 9;
		this.numericUpDown_2.Location = new System.Drawing.Point(491, 50);
		this.numericUpDown_2.Maximum = new decimal(new int[4] { 100000000, 0, 0, 0 });
		this.numericUpDown_2.Name = "numericUpDown_2";
		this.numericUpDown_2.Size = new System.Drawing.Size(100, 21);
		this.numericUpDown_2.TabIndex = 7;
		this.numericUpDown_5.Location = new System.Drawing.Point(491, 23);
		this.numericUpDown_5.Maximum = new decimal(new int[4] { 100000000, 0, 0, 0 });
		this.numericUpDown_5.Name = "numericUpDown_5";
		this.numericUpDown_5.Size = new System.Drawing.Size(100, 21);
		this.numericUpDown_5.TabIndex = 5;
		this.numericUpDown_0.Location = new System.Drawing.Point(294, 47);
		this.numericUpDown_0.Maximum = new decimal(new int[4] { 100000000, 0, 0, 0 });
		this.numericUpDown_0.Name = "numericUpDown_0";
		this.numericUpDown_0.Size = new System.Drawing.Size(100, 21);
		this.numericUpDown_0.TabIndex = 3;
		this.numericUpDown_1.Location = new System.Drawing.Point(98, 46);
		this.numericUpDown_1.Maximum = new decimal(new int[4] { 100000000, 0, 0, 0 });
		this.numericUpDown_1.Name = "numericUpDown_1";
		this.numericUpDown_1.Size = new System.Drawing.Size(100, 21);
		this.numericUpDown_1.TabIndex = 1;
		this.groupBox_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_1.Controls.Add(this.label_0);
		this.groupBox_1.Controls.Add(this.label_1);
		this.groupBox_1.Controls.Add(this.label_3);
		this.groupBox_1.Location = new System.Drawing.Point(12, 266);
		this.groupBox_1.Name = "groupBox_1";
		this.groupBox_1.Size = new System.Drawing.Size(608, 94);
		this.groupBox_1.TabIndex = 17;
		this.groupBox_1.TabStop = false;
		this.groupBox_1.Text = "进度显示";
		this.label_0.AutoSize = true;
		this.label_0.Location = new System.Drawing.Point(6, 22);
		this.label_0.Name = "label_0";
		this.label_0.Size = new System.Drawing.Size(65, 12);
		this.label_0.TabIndex = 21;
		this.label_0.Text = "当前小说：";
		this.label_1.AutoSize = true;
		this.label_1.Location = new System.Drawing.Point(6, 71);
		this.label_1.Name = "label_1";
		this.label_1.Size = new System.Drawing.Size(65, 12);
		this.label_1.TabIndex = 19;
		this.label_1.Text = "当前动作：";
		this.label_3.AutoSize = true;
		this.label_3.Location = new System.Drawing.Point(6, 46);
		this.label_3.Name = "label_3";
		this.label_3.Size = new System.Drawing.Size(65, 12);
		this.label_3.TabIndex = 18;
		this.label_3.Text = "当前进度：";
		this.button_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button_0.Location = new System.Drawing.Point(464, 371);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 23);
		this.button_0.TabIndex = 18;
		this.button_0.Text = "开始";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(button_0_Click);
		this.button_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button_1.Location = new System.Drawing.Point(545, 371);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(75, 23);
		this.button_1.TabIndex = 19;
		this.button_1.Text = "停止";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.backgroundWorker_0.WorkerReportsProgress = true;
		this.backgroundWorker_0.WorkerSupportsCancellation = true;
		this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
		this.backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_0_ProgressChanged);
		this.backgroundWorker_0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_0_RunWorkerCompleted);
		base.ClientSize = new System.Drawing.Size(632, 406);
		base.Controls.Add(this.button_1);
		base.Controls.Add(this.button_0);
		base.Controls.Add(this.groupBox_1);
		base.Controls.Add(this.groupBox_0);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "HelpDeleteNovel";
		this.Text = "批量删除小说";
		this.groupBox_0.ResumeLayout(false);
		this.groupBox_0.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_5).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).EndInit();
		this.groupBox_1.ResumeLayout(false);
		this.groupBox_1.PerformLayout();
		base.ResumeLayout(false);
	}
}
