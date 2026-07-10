using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NovelSpider.Config;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class NovelSpiderUpdate : DockContent
{
	private BackgroundWorker backgroundWorker_0;

	private ProgressBar CheckprogressBar;

	private IContainer icontainer_0;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label NowVersion;

	private PictureBox pictureBox1;

	private ProgressBar progressBar1;

	private ProgressBar progressBar2;

	private Label RemoteVersion;

	private Button StartUpdateButton;

	private Button StopUpdateButton;

	private TextBox UpdateLog;

	public NovelSpiderUpdate()
	{
		InitializeComponent();
	}

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		backgroundWorker_0.ReportProgress(10, "开始检查采集器最新版本");
	}

	private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		switch (e.ProgressPercentage)
		{
		case 10:
			CheckprogressBar.Value = 10;
			UpdateLog.Text = UpdateLog.Text + "\r\n" + e.UserState.ToString();
			break;
		case 20:
			CheckprogressBar.Value = 20;
			UpdateLog.Text = UpdateLog.Text + "\r\n" + e.UserState.ToString();
			break;
		}
	}

	private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			UpdateLog.Text = UpdateLog.Text + "\r\n出现错误：" + e.Error.Message;
		}
		else if (e.Cancelled)
		{
			UpdateLog.Text += "\r\n操作被用户取消";
		}
		else
		{
			UpdateLog.Text += "\r\n版本检测完成";
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.NovelSpiderUpdate));
		this.UpdateLog = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CheckprogressBar = new System.Windows.Forms.ProgressBar();
		this.progressBar1 = new System.Windows.Forms.ProgressBar();
		this.label2 = new System.Windows.Forms.Label();
		this.progressBar2 = new System.Windows.Forms.ProgressBar();
		this.label3 = new System.Windows.Forms.Label();
		this.StartUpdateButton = new System.Windows.Forms.Button();
		this.StopUpdateButton = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.NowVersion = new System.Windows.Forms.Label();
		this.RemoteVersion = new System.Windows.Forms.Label();
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.UpdateLog.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.UpdateLog.Location = new System.Drawing.Point(174, 12);
		this.UpdateLog.Multiline = true;
		this.UpdateLog.Name = "UpdateLog";
		this.UpdateLog.ReadOnly = true;
		this.UpdateLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.UpdateLog.Size = new System.Drawing.Size(436, 230);
		this.UpdateLog.TabIndex = 0;
		this.UpdateLog.Text = "欢迎使用关关采集更新管理器";
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(172, 255);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(77, 12);
		this.label1.TabIndex = 2;
		this.label1.Text = "最新版本检测";
		this.CheckprogressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.CheckprogressBar.Location = new System.Drawing.Point(255, 255);
		this.CheckprogressBar.Name = "CheckprogressBar";
		this.CheckprogressBar.Size = new System.Drawing.Size(355, 12);
		this.CheckprogressBar.TabIndex = 11;
		this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.progressBar1.Location = new System.Drawing.Point(255, 282);
		this.progressBar1.Name = "progressBar1";
		this.progressBar1.Size = new System.Drawing.Size(355, 12);
		this.progressBar1.TabIndex = 13;
		this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(172, 282);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(77, 12);
		this.label2.TabIndex = 12;
		this.label2.Text = "文件下载进度";
		this.progressBar2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.progressBar2.Location = new System.Drawing.Point(255, 311);
		this.progressBar2.Name = "progressBar2";
		this.progressBar2.Size = new System.Drawing.Size(355, 12);
		this.progressBar2.TabIndex = 15;
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(172, 311);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(77, 12);
		this.label3.TabIndex = 14;
		this.label3.Text = "版本更新状态";
		this.StartUpdateButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.StartUpdateButton.Location = new System.Drawing.Point(453, 329);
		this.StartUpdateButton.Name = "StartUpdateButton";
		this.StartUpdateButton.Size = new System.Drawing.Size(75, 23);
		this.StartUpdateButton.TabIndex = 16;
		this.StartUpdateButton.Text = "开始更新";
		this.StartUpdateButton.UseVisualStyleBackColor = true;
		this.StartUpdateButton.Click += new System.EventHandler(StartUpdateButton_Click);
		this.StopUpdateButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.StopUpdateButton.Location = new System.Drawing.Point(534, 329);
		this.StopUpdateButton.Name = "StopUpdateButton";
		this.StopUpdateButton.Size = new System.Drawing.Size(75, 23);
		this.StopUpdateButton.TabIndex = 17;
		this.StopUpdateButton.Text = "取消";
		this.StopUpdateButton.UseVisualStyleBackColor = true;
		this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.label4.AutoSize = true;
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(221, 370);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(389, 12);
		this.label4.TabIndex = 18;
		this.label4.Text = "请在提示更新完成后关闭所有正在进行的采集并重新打开采集器完成更新";
		this.NowVersion.AutoSize = true;
		this.NowVersion.BackColor = System.Drawing.Color.Transparent;
		this.NowVersion.ForeColor = System.Drawing.Color.White;
		this.NowVersion.Location = new System.Drawing.Point(12, 355);
		this.NowVersion.Name = "NowVersion";
		this.NowVersion.Size = new System.Drawing.Size(59, 12);
		this.NowVersion.TabIndex = 19;
		this.NowVersion.Text = "当前版本:";
		this.RemoteVersion.AutoSize = true;
		this.RemoteVersion.BackColor = System.Drawing.Color.Transparent;
		this.RemoteVersion.ForeColor = System.Drawing.Color.White;
		this.RemoteVersion.Location = new System.Drawing.Point(12, 374);
		this.RemoteVersion.Name = "RemoteVersion";
		this.RemoteVersion.Size = new System.Drawing.Size(59, 12);
		this.RemoteVersion.TabIndex = 20;
		this.RemoteVersion.Text = "最新版本:";
		this.backgroundWorker_0.WorkerReportsProgress = true;
		this.backgroundWorker_0.WorkerSupportsCancellation = true;
		this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
		this.backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_0_ProgressChanged);
		this.backgroundWorker_0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_0_RunWorkerCompleted);
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.pictureBox1.InitialImage = null;
		this.pictureBox1.Location = new System.Drawing.Point(0, -1);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(166, 425);
		this.pictureBox1.TabIndex = 1;
		this.pictureBox1.TabStop = false;
		base.ClientSize = new System.Drawing.Size(622, 393);
		base.Controls.Add(this.RemoteVersion);
		base.Controls.Add(this.NowVersion);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.StopUpdateButton);
		base.Controls.Add(this.StartUpdateButton);
		base.Controls.Add(this.progressBar2);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.progressBar1);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.CheckprogressBar);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.UpdateLog);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "NovelSpiderUpdate";
		this.Text = "關關更新器";
		base.Load += new System.EventHandler(NovelSpiderUpdate_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void NovelSpiderUpdate_Load(object sender, EventArgs e)
	{
		pictureBox1.Controls.Add(NowVersion);
		pictureBox1.Controls.Add(RemoteVersion);
		NowVersion.Text = NowVersion.Text + "V" + Configs.DisplayVersion;
		backgroundWorker_0.RunWorkerAsync();
	}

	private void StartUpdateButton_Click(object sender, EventArgs e)
	{
	}
}
