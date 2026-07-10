using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NovelSpider.Entity;
using NovelSpider.Local;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class HelpUpdateNovelBySelf : DockContent
{
	private BackgroundWorker backgroundWorker_0;

	private Button button_0;

	private Button button_1;

	private IContainer icontainer_0;

	private int int_0;

	private int int_1;

	private Label label_0;

	private Label label_1;

	private Label label_2;

	private TextBox textBox_0;

	private TextBox textBox_1;

	public HelpUpdateNovelBySelf()
	{
		InitializeComponent();
	}

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		int num = int_0;
		while (true)
		{
			if (num > int_1)
			{
				return;
			}
			if (backgroundWorker_0.CancellationPending)
			{
				break;
			}
			backgroundWorker_0.ReportProgress(0, num);
			try
			{
				NovelInfo novelInfo = new NovelInfo();
				novelInfo.PutID = num;
				NovelInfo novelInfo_ = novelInfo;
				LocalProvider.GetInstance().UpdateLastChapter(novelInfo_);
			}
			catch
			{
			}
			num++;
		}
		e.Cancel = true;
	}

	private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		label_2.Text = e.UserState.ToString();
	}

	private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		button_0.Enabled = true;
	}

	private void button_0_Click(object sender, EventArgs e)
	{
		int_0 = Convert.ToInt32(textBox_0.Text);
		int_1 = Convert.ToInt32(textBox_1.Text);
		if (!backgroundWorker_0.IsBusy)
		{
			button_0.Enabled = false;
			backgroundWorker_0.RunWorkerAsync();
		}
	}

	private void button_1_Click(object sender, EventArgs e)
	{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.HelpUpdateNovelBySelf));
		this.textBox_0 = new System.Windows.Forms.TextBox();
		this.label_0 = new System.Windows.Forms.Label();
		this.label_1 = new System.Windows.Forms.Label();
		this.textBox_1 = new System.Windows.Forms.TextBox();
		this.button_0 = new System.Windows.Forms.Button();
		this.button_1 = new System.Windows.Forms.Button();
		this.label_2 = new System.Windows.Forms.Label();
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		base.SuspendLayout();
		this.textBox_0.Location = new System.Drawing.Point(71, 12);
		this.textBox_0.Name = "textBox_0";
		this.textBox_0.Size = new System.Drawing.Size(156, 21);
		this.textBox_0.TabIndex = 0;
		this.label_0.AutoSize = true;
		this.label_0.Location = new System.Drawing.Point(12, 15);
		this.label_0.Name = "label_0";
		this.label_0.Size = new System.Drawing.Size(53, 12);
		this.label_0.TabIndex = 1;
		this.label_0.Text = "最小ID：";
		this.label_1.AutoSize = true;
		this.label_1.Location = new System.Drawing.Point(12, 42);
		this.label_1.Name = "label_1";
		this.label_1.Size = new System.Drawing.Size(53, 12);
		this.label_1.TabIndex = 3;
		this.label_1.Text = "最大ID：";
		this.textBox_1.Location = new System.Drawing.Point(71, 39);
		this.textBox_1.Name = "textBox_1";
		this.textBox_1.Size = new System.Drawing.Size(156, 21);
		this.textBox_1.TabIndex = 2;
		this.button_0.Location = new System.Drawing.Point(71, 66);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 23);
		this.button_0.TabIndex = 4;
		this.button_0.Text = "运行";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(button_0_Click);
		this.button_1.Location = new System.Drawing.Point(152, 66);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(75, 23);
		this.button_1.TabIndex = 5;
		this.button_1.Text = "停止";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.label_2.AutoSize = true;
		this.label_2.Location = new System.Drawing.Point(12, 111);
		this.label_2.Name = "label_2";
		this.label_2.Size = new System.Drawing.Size(41, 12);
		this.label_2.TabIndex = 6;
		this.label_2.Text = "label3";
		this.backgroundWorker_0.WorkerReportsProgress = true;
		this.backgroundWorker_0.WorkerSupportsCancellation = true;
		this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
		this.backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_0_ProgressChanged);
		this.backgroundWorker_0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_0_RunWorkerCompleted);
		base.ClientSize = new System.Drawing.Size(253, 144);
		base.Controls.Add(this.label_2);
		base.Controls.Add(this.button_1);
		base.Controls.Add(this.button_0);
		base.Controls.Add(this.label_1);
		base.Controls.Add(this.textBox_1);
		base.Controls.Add(this.label_0);
		base.Controls.Add(this.textBox_0);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "HelpUpdateNovelBySelf";
		this.Text = "更新小说信息";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
