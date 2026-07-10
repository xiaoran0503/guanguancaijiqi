using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.mxd;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class MdiForm : Form
{
	private IContainer components;

	private DockContent activeDockContent;

	private ConfigForm configForm_0;

	private WelcomeForm welcomeForm_0;

	private DockPanel dockPanel;

	private IContainer icontainer_0;

	public string KeyPassWd;

	private MenuStrip MainMenu;

	private static Thread thr;

	private static DateTime time;

	private System.Windows.Forms.Timer timer_0;

	private ToolStripMenuItem toolStripMenuItem_0;

	private ToolStripMenuItem toolStripMenuItem_1;

	private ToolStripMenuItem toolStripMenuItem_10;

	private ToolStripMenuItem toolStripMenuItem_11;

	private ToolStripMenuItem toolStripMenuItem_12;

	private ToolStripMenuItem toolStripMenuItem_13;

	private ToolStripMenuItem toolStripMenuItem_14;

	private ToolStripMenuItem toolStripMenuItem_15;

	private ToolStripMenuItem toolStripMenuItem_16;

	private ToolStripMenuItem toolStripMenuItem_17;

	private ToolStripMenuItem toolStripMenuItem_18;

	private ToolStripMenuItem toolStripMenuItem_19;

	private ToolStripMenuItem toolStripMenuItem_2;

	private ToolStripMenuItem toolStripMenuItem_20;

	private ToolStripMenuItem toolStripMenuItem_21;

	private ToolStripMenuItem toolStripMenuItem_22;

	private ToolStripMenuItem toolStripMenuItem_23;

	private ToolStripMenuItem toolStripMenuItem_24;

	private ToolStripMenuItem toolStripMenuItem_25;

	private ToolStripMenuItem toolStripMenuItem_26;

	private ToolStripMenuItem toolStripMenuItem_27;

	private ToolStripMenuItem toolStripMenuItem_28;

	private ToolStripMenuItem toolStripMenuItem_3;

	private ToolStripMenuItem toolStripMenuItem_4;

	private ToolStripMenuItem toolStripMenuItem_5;

	private ToolStripMenuItem toolStripMenuItem_6;

	private ToolStripMenuItem toolStripMenuItem_7;

	private ToolStripMenuItem toolStripMenuItem_8;

	private ToolStripMenuItem toolStripMenuItem_9;

	private ToolStripMenuItem toolStripMenuItem1;

	private ToolStripMenuItem toolStripMenuItem17;

	private ToolStripMenuItem toolStripMenuItem2;

	public MdiForm()
	{
		KeyPassWd = "";
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && icontainer_0 != null)
		{
			icontainer_0.Dispose();
		}
		base.Dispose(disposing);
	}

	public static string GetMail()
	{
		string text = NativeMethods.BookMsg;
		if (Configs.BaseConfig.LogType == 0)
		{
			text += "<br /><br />由于你使用的是文本log模式，无法给你提供错误详单，如果需要，请设置为SQLite模式。";
		}
		else
		{
			FileInfo fileInfo = new FileInfo("Log\\" + DateTime.Now.ToString("yyyyMMdd") + ".db3");
			if (fileInfo.Exists)
			{
				string string_ = "Data Source=" + fileInfo.FullName;
				string string_2 = "Select * From [TaskLog] Where LASTNUM >" + SecurityUtil.ConvertDateTimeInt(time.AddMinutes(-Configs.BaseConfig.MailTimeNum)) + " AND LASTNUM<=" + SecurityUtil.ConvertDateTimeInt(time) + " And NOVELNAME<>''";
				DataSet dataSet = SQLiteHelper.ExecuteDataset(string_, string_2);
				if (dataSet != null && dataSet.Tables[0].Rows.Count >= 1)
				{
					object obj = text;
					text = string.Concat(obj, "<br /><br />错误共计<font style=color:red>", dataSet.Tables[0].Rows.Count, "</font>条。");
					for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
					{
						string text2 = text;
						text = text2 + "<br /><br />" + dataSet.Tables[0].Rows[i]["LASTTIME"].ToString() + " 规则" + dataSet.Tables[0].Rows[i]["RULEFILE"].ToString() + "操作书籍[<font style=color:green>" + dataSet.Tables[0].Rows[i]["NOVELNAME"].ToString() + "</font>]时候发生(" + dataSet.Tables[0].Rows[i]["EXID"].ToString() + ")错误。<br />错误详情：" + dataSet.Tables[0].Rows[i]["EXMSG"].ToString() + "<br />相关网址：" + dataSet.Tables[0].Rows[i]["INDEXURL"].ToString();
					}
				}
				else
				{
					text += "没有发现错误。<br />";
				}
			}
		}
		return text + "<br /><br />就这样完了，没了!";
	}

	public static void GoMail()
	{
		EmailSendServer emailSendServer = new EmailSendServer();
		emailSendServer.SenderEmail = Configs.BaseConfig.MailUser;
		emailSendServer.SmtpServer = Configs.BaseConfig.MailSmtp;
		emailSendServer.SmtpServerAccount = Configs.BaseConfig.MailUser;
		emailSendServer.SmtpServerPassword = Configs.BaseConfig.MailPass;
		EmailSendServer emailSendServer2 = emailSendServer;
		Random random = new Random();
		string text = SecurityUtil.ComputeMD5(random.Next(0, 99999).ToString()).Substring(8, 5);
		EmailBase emailBase = new EmailBase();
		emailBase.Subject = "【" + Configs.BaseConfig.WebSiteName + "】即时采集报告(" + text + ")";
		emailBase.Content = GetMail();
		EmailBase emailBase2 = emailBase;
		string[] array = Configs.BaseConfig.MailTitle.Trim().Split(',');
		foreach (string text2 in array)
		{
			if (text2 == string.Empty)
			{
				break;
			}
			emailBase2.ToEmail = text2;
			emailSendServer2.SendMail(emailBase2);
		}
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
		WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
		WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
		WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient = new WeifenLuo.WinFormsUI.Docking.TabGradient();
		WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
		WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
		WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
		WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
		WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
		WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
		WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
		WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
		WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
		WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
		WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.MdiForm));
		this.MainMenu = new System.Windows.Forms.MenuStrip();
		this.toolStripMenuItem_0 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_1 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_2 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_3 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_4 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_23 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_8 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_7 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_9 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_25 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_26 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_27 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_5 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_13 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_12 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_17 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_22 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_14 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_15 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_16 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_10 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_11 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_6 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_18 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_19 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_24 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_20 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_21 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_28 = new System.Windows.Forms.ToolStripMenuItem();
		this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
		this.timer_0 = new System.Windows.Forms.Timer(this.components);
		this.MainMenu.SuspendLayout();
		base.SuspendLayout();
		this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripMenuItem_0, this.toolStripMenuItem_4, this.toolStripMenuItem_5, this.toolStripMenuItem_10, this.toolStripMenuItem_6, this.toolStripMenuItem_18 });
		this.MainMenu.Location = new System.Drawing.Point(0, 0);
		this.MainMenu.MdiWindowListItem = this.toolStripMenuItem_6;
		this.MainMenu.Name = "MainMenu";
		this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.MainMenu.Size = new System.Drawing.Size(984, 25);
		this.MainMenu.TabIndex = 0;
		this.MainMenu.Text = "主菜单";
		this.toolStripMenuItem_0.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripMenuItem_1, this.toolStripMenuItem_2, this.toolStripMenuItem_3, this.toolStripMenuItem2 });
		this.toolStripMenuItem_0.Name = "toolStripMenuItem_0";
		this.toolStripMenuItem_0.Size = new System.Drawing.Size(60, 21);
		this.toolStripMenuItem_0.Text = "采集(&C)";
		this.toolStripMenuItem_1.Name = "toolStripMenuItem_1";
		this.toolStripMenuItem_1.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_1.Text = "标准采集模式";
		this.toolStripMenuItem_1.Click += new System.EventHandler(toolStripMenuItem_1_Click);
		this.toolStripMenuItem_2.ForeColor = System.Drawing.Color.Maroon;
		this.toolStripMenuItem_2.Name = "toolStripMenuItem_2";
		this.toolStripMenuItem_2.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_2.Text = "替换采集模式";
		this.toolStripMenuItem_2.Click += new System.EventHandler(toolStripMenuItem_2_Click);
		this.toolStripMenuItem_3.Name = "toolStripMenuItem_3";
		this.toolStripMenuItem_3.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_3.Text = "手动控制模式";
		this.toolStripMenuItem_3.Click += new System.EventHandler(toolStripMenuItem_3_Click);
		this.toolStripMenuItem2.Name = "toolStripMenuItem2";
		this.toolStripMenuItem2.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem2.Text = "日志修复模式";
		this.toolStripMenuItem2.Click += new System.EventHandler(toolStripMenuItem2_Click);
		this.toolStripMenuItem_4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripMenuItem_23, this.toolStripMenuItem_8, this.toolStripMenuItem1, this.toolStripMenuItem_7, this.toolStripMenuItem_9, this.toolStripMenuItem_25, this.toolStripMenuItem_26, this.toolStripMenuItem_27 });
		this.toolStripMenuItem_4.Name = "toolStripMenuItem_4";
		this.toolStripMenuItem_4.Size = new System.Drawing.Size(60, 21);
		this.toolStripMenuItem_4.Text = "辅助(&A)";
		this.toolStripMenuItem_23.Name = "toolStripMenuItem_23";
		this.toolStripMenuItem_23.Size = new System.Drawing.Size(166, 22);
		this.toolStripMenuItem_23.Text = "子窗口冲突监控";
		this.toolStripMenuItem_23.Click += new System.EventHandler(toolStripMenuItem_23_Click);
		this.toolStripMenuItem_8.Enabled = false;
		this.toolStripMenuItem_8.Name = "toolStripMenuItem_8";
		this.toolStripMenuItem_8.Size = new System.Drawing.Size(166, 22);
		this.toolStripMenuItem_8.Text = "盗链本地化";
		this.toolStripMenuItem_8.Visible = false;
		this.toolStripMenuItem1.Name = "toolStripMenuItem1";
		this.toolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
		this.toolStripMenuItem1.Text = "数据库维护";
		this.toolStripMenuItem1.Visible = false;
		this.toolStripMenuItem1.Click += new System.EventHandler(toolStripMenuItem1_Click);
		this.toolStripMenuItem_7.Name = "toolStripMenuItem_7";
		this.toolStripMenuItem_7.Size = new System.Drawing.Size(166, 22);
		this.toolStripMenuItem_7.Text = "批量生成";
		this.toolStripMenuItem_7.Click += new System.EventHandler(toolStripMenuItem_7_Click);
		this.toolStripMenuItem_9.Name = "toolStripMenuItem_9";
		this.toolStripMenuItem_9.Size = new System.Drawing.Size(166, 22);
		this.toolStripMenuItem_9.Text = "批量删除小说";
		this.toolStripMenuItem_9.Click += new System.EventHandler(toolStripMenuItem_9_Click);
		this.toolStripMenuItem_25.Enabled = false;
		this.toolStripMenuItem_25.Name = "toolStripMenuItem_25";
		this.toolStripMenuItem_25.Size = new System.Drawing.Size(166, 22);
		this.toolStripMenuItem_25.Text = "批量删除章节";
		this.toolStripMenuItem_25.Visible = false;
		this.toolStripMenuItem_26.Name = "toolStripMenuItem_26";
		this.toolStripMenuItem_26.Size = new System.Drawing.Size(166, 22);
		this.toolStripMenuItem_26.Text = "更新小说信息";
		this.toolStripMenuItem_26.Click += new System.EventHandler(toolStripMenuItem_26_Click);
		this.toolStripMenuItem_27.Name = "toolStripMenuItem_27";
		this.toolStripMenuItem_27.Size = new System.Drawing.Size(166, 22);
		this.toolStripMenuItem_27.Text = "MYSQL时间换算";
		this.toolStripMenuItem_27.Click += new System.EventHandler(toolStripMenuItem_27_Click);
		this.toolStripMenuItem_5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripMenuItem_13, this.toolStripMenuItem_12, this.toolStripMenuItem_17, this.toolStripMenuItem17, this.toolStripMenuItem_22, this.toolStripMenuItem_14, this.toolStripMenuItem_15, this.toolStripMenuItem_16 });
		this.toolStripMenuItem_5.Name = "toolStripMenuItem_5";
		this.toolStripMenuItem_5.Size = new System.Drawing.Size(59, 21);
		this.toolStripMenuItem_5.Text = "设置(&S)";
		this.toolStripMenuItem_13.Name = "toolStripMenuItem_13";
		this.toolStripMenuItem_13.Size = new System.Drawing.Size(136, 22);
		this.toolStripMenuItem_13.Text = "系统设置";
		this.toolStripMenuItem_13.Click += new System.EventHandler(toolStripMenuItem_13_Click);
		this.toolStripMenuItem_12.Name = "toolStripMenuItem_12";
		this.toolStripMenuItem_12.Size = new System.Drawing.Size(136, 22);
		this.toolStripMenuItem_12.Text = "分类对应";
		this.toolStripMenuItem_12.Click += new System.EventHandler(toolStripMenuItem_12_Click);
		this.toolStripMenuItem_17.Name = "toolStripMenuItem_17";
		this.toolStripMenuItem_17.Size = new System.Drawing.Size(136, 22);
		this.toolStripMenuItem_17.Text = "高级设置";
		this.toolStripMenuItem_17.Click += new System.EventHandler(toolStripMenuItem_17_Click_1);
		this.toolStripMenuItem17.Name = "toolStripMenuItem17";
		this.toolStripMenuItem17.Size = new System.Drawing.Size(136, 22);
		this.toolStripMenuItem17.Text = "超级设置";
		this.toolStripMenuItem17.Click += new System.EventHandler(toolStripMenuItem17_Click_1);
		this.toolStripMenuItem_22.Name = "toolStripMenuItem_22";
		this.toolStripMenuItem_22.Size = new System.Drawing.Size(136, 22);
		this.toolStripMenuItem_22.Text = "图转文设置";
		this.toolStripMenuItem_22.Click += new System.EventHandler(toolStripMenuItem_22_Click);
		this.toolStripMenuItem_14.Name = "toolStripMenuItem_14";
		this.toolStripMenuItem_14.Size = new System.Drawing.Size(136, 22);
		this.toolStripMenuItem_14.Text = "生成设置";
		this.toolStripMenuItem_14.Click += new System.EventHandler(toolStripMenuItem_14_Click);
		this.toolStripMenuItem_15.Name = "toolStripMenuItem_15";
		this.toolStripMenuItem_15.Size = new System.Drawing.Size(136, 22);
		this.toolStripMenuItem_15.Text = "文字广告";
		this.toolStripMenuItem_15.Click += new System.EventHandler(toolStripMenuItem_15_Click);
		this.toolStripMenuItem_16.Name = "toolStripMenuItem_16";
		this.toolStripMenuItem_16.Size = new System.Drawing.Size(136, 22);
		this.toolStripMenuItem_16.Text = "过滤替换";
		this.toolStripMenuItem_16.Click += new System.EventHandler(toolStripMenuItem_16_Click);
		this.toolStripMenuItem_10.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.toolStripMenuItem_11 });
		this.toolStripMenuItem_10.Name = "toolStripMenuItem_10";
		this.toolStripMenuItem_10.Size = new System.Drawing.Size(60, 21);
		this.toolStripMenuItem_10.Text = "规则(&R)";
		this.toolStripMenuItem_11.Name = "toolStripMenuItem_11";
		this.toolStripMenuItem_11.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_11.Text = "采集规则管理";
		this.toolStripMenuItem_11.Click += new System.EventHandler(toolStripMenuItem_11_Click);
		this.toolStripMenuItem_6.Name = "toolStripMenuItem_6";
		this.toolStripMenuItem_6.Size = new System.Drawing.Size(64, 21);
		this.toolStripMenuItem_6.Text = "窗口(&W)";
		this.toolStripMenuItem_18.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripMenuItem_19, this.toolStripMenuItem_24, this.toolStripMenuItem_20, this.toolStripMenuItem_21, this.toolStripMenuItem_28 });
		this.toolStripMenuItem_18.Name = "toolStripMenuItem_18";
		this.toolStripMenuItem_18.Size = new System.Drawing.Size(61, 21);
		this.toolStripMenuItem_18.Text = "帮助(&H)";
		this.toolStripMenuItem_19.Name = "toolStripMenuItem_19";
		this.toolStripMenuItem_19.Size = new System.Drawing.Size(124, 22);
		this.toolStripMenuItem_19.Text = "帮助内容";
		this.toolStripMenuItem_19.Click += new System.EventHandler(toolStripMenuItem_19_Click);
		this.toolStripMenuItem_24.Name = "toolStripMenuItem_24";
		this.toolStripMenuItem_24.Size = new System.Drawing.Size(124, 22);
		this.toolStripMenuItem_24.Text = "查看日志";
		this.toolStripMenuItem_24.Click += new System.EventHandler(toolStripMenuItem_24_Click);
		this.toolStripMenuItem_20.Name = "toolStripMenuItem_20";
		this.toolStripMenuItem_20.Size = new System.Drawing.Size(124, 22);
		this.toolStripMenuItem_20.Text = "检查更新";
		this.toolStripMenuItem_20.Visible = false;
		this.toolStripMenuItem_20.Click += new System.EventHandler(toolStripMenuItem_20_Click);
		this.toolStripMenuItem_21.Name = "toolStripMenuItem_21";
		this.toolStripMenuItem_21.Size = new System.Drawing.Size(124, 22);
		this.toolStripMenuItem_21.Text = "更新日志";
		this.toolStripMenuItem_21.Click += new System.EventHandler(toolStripMenuItem_21_Click);
		this.toolStripMenuItem_28.Name = "toolStripMenuItem_28";
		this.toolStripMenuItem_28.Size = new System.Drawing.Size(124, 22);
		this.toolStripMenuItem_28.Text = "高级功能";
		this.toolStripMenuItem_28.Click += new System.EventHandler(toolStripMenuItem_28_Click);
		this.dockPanel.ActiveAutoHideContent = null;
		this.dockPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.dockPanel.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
		this.dockPanel.Location = new System.Drawing.Point(0, 25);
		this.dockPanel.Name = "dockPanel";
		this.dockPanel.Size = new System.Drawing.Size(984, 566);
		dockPanelGradient.EndColor = System.Drawing.SystemColors.ControlLight;
		dockPanelGradient.StartColor = System.Drawing.SystemColors.ControlLight;
		autoHideStripSkin.DockStripGradient = dockPanelGradient;
		tabGradient.EndColor = System.Drawing.SystemColors.Control;
		tabGradient.StartColor = System.Drawing.SystemColors.Control;
		tabGradient.TextColor = System.Drawing.SystemColors.ControlDarkDark;
		autoHideStripSkin.TabGradient = tabGradient;
		dockPanelSkin.AutoHideStripSkin = autoHideStripSkin;
		tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
		tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
		tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
		dockPaneStripGradient.ActiveTabGradient = tabGradient2;
		dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
		dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
		dockPaneStripGradient.DockStripGradient = dockPanelGradient2;
		tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
		tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
		tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
		dockPaneStripGradient.InactiveTabGradient = tabGradient3;
		dockPaneStripSkin.DocumentGradient = dockPaneStripGradient;
		tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
		tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
		tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
		tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
		dockPaneStripToolWindowGradient.ActiveCaptionGradient = tabGradient4;
		tabGradient5.EndColor = System.Drawing.SystemColors.Control;
		tabGradient5.StartColor = System.Drawing.SystemColors.Control;
		tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
		dockPaneStripToolWindowGradient.ActiveTabGradient = tabGradient5;
		dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
		dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
		dockPaneStripToolWindowGradient.DockStripGradient = dockPanelGradient3;
		tabGradient6.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
		tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
		tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
		tabGradient6.TextColor = System.Drawing.SystemColors.ControlText;
		dockPaneStripToolWindowGradient.InactiveCaptionGradient = tabGradient6;
		tabGradient7.EndColor = System.Drawing.Color.Transparent;
		tabGradient7.StartColor = System.Drawing.Color.Transparent;
		tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
		dockPaneStripToolWindowGradient.InactiveTabGradient = tabGradient7;
		dockPaneStripSkin.ToolWindowGradient = dockPaneStripToolWindowGradient;
		dockPanelSkin.DockPaneStripSkin = dockPaneStripSkin;
		this.dockPanel.Theme = DockThemeFactory.CreateTheme(dockPanelSkin);
		this.dockPanel.TabIndex = 2;
		this.timer_0.Interval = 1000;
		this.timer_0.Tick += new System.EventHandler(timer_0_Tick);
		this.BackColor = System.Drawing.SystemColors.Control;
		base.ClientSize = new System.Drawing.Size(984, 591);
		base.Controls.Add(this.MainMenu);
		base.Controls.Add(this.dockPanel);
		base.Icon = AppIconProvider.Icon;
		base.IsMdiContainer = true;
		base.MainMenuStrip = this.MainMenu;
		base.Name = "MdiForm";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "数据采集器 V2";
		base.Load += new System.EventHandler(MdiForm_Load);
		base.Shown += new System.EventHandler(MdiForm_Shown);
		this.MainMenu.ResumeLayout(false);
		this.MainMenu.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void MdiForm_Load(object sender, EventArgs e)
	{
		using IDisposable loadScope = PerformanceTelemetry.Measure("ui", "main_form_load");
		NovelSpider.Common.Keys.LoadText();
		if (!string.IsNullOrEmpty(Configs.BaseConfig.WebSiteName))
		{
			Text = Configs.BaseConfig.WebSiteName.ToString() + "（ FOR 杰奇全版本通用编码版）V" + Configs.DisplayVersion;
		}
		if (Configs.BaseConfig.LicenseOk)
		{
			time = DateTime.Now;
			System.Timers.Timer timer = new System.Timers.Timer(60000.0);
			timer.Elapsed += myTimer_Elapsed;
			timer.Enabled = true;
			timer.AutoReset = true;
		}
	}

	private WelcomeForm EnsureWelcomeForm()
	{
		if (welcomeForm_0 == null || welcomeForm_0.IsDisposed)
		{
			welcomeForm_0 = new WelcomeForm();
		}
		return welcomeForm_0;
	}

	private void ShowWelcomeForm()
	{
		using IDisposable welcomeScope = PerformanceTelemetry.Measure("ui", "welcome_form_open");
		ShowDockContent(EnsureWelcomeForm());
	}

	private ConfigForm EnsureConfigForm()
	{
		if (configForm_0 == null || configForm_0.IsDisposed)
		{
			configForm_0 = new ConfigForm();
		}
		return configForm_0;
	}

	private void ShowConfigForm(int logPageIndex)
	{
		using IDisposable configScope = PerformanceTelemetry.Measure("ui", "config_form_open", logPageIndex.ToString());
		ConfigForm configForm = EnsureConfigForm();
		configForm.日志记录.SelectedIndex = logPageIndex;
		ShowDockContent(configForm);
	}

	private void ShowDockContent(DockContent content)
	{
		if (content == null || content.IsDisposed)
		{
			return;
		}

		if (content.MdiParent != null)
		{
			content.MdiParent = null;
		}

		if (content.DockPanel == dockPanel)
		{
			content.Show();
			content.Focus();
			return;
		}

		activeDockContent = content;
		content.Show(dockPanel, DockState.Document);
	}

	private void MdiForm_Shown(object sender, EventArgs e)
	{
		BeginInvoke(new MethodInvoker(() =>
		{
			if (!IsDisposed)
			{
				ShowWelcomeForm();
			}
		}));
	}

	private static void myTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		try
		{
			YourTask();
		}
		catch
		{
		}
	}

	private void timer_0_Tick(object sender, EventArgs e)
	{
		if (DateTime.Now.Hour == 3 && Text.IndexOf("V8.0") < 0)
		{
			Application.Exit();
		}
	}

	private void toolStripMenuItem_1_Click(object sender, EventArgs e)
	{
		自动采集模式 自动采集模式2 = new 自动采集模式();
		ShowDockContent(自动采集模式2);
	}

	private void toolStripMenuItem_11_Click(object sender, EventArgs e)
	{
		RuleForm ruleForm = new RuleForm();
		ShowDockContent(ruleForm);
	}

	private void toolStripMenuItem_12_Click(object sender, EventArgs e)
	{
		ShowConfigForm(1);
	}

	private void toolStripMenuItem_13_Click(object sender, EventArgs e)
	{
		ShowConfigForm(0);
	}

	private void toolStripMenuItem_14_Click(object sender, EventArgs e)
	{
		ShowConfigForm(5);
	}

	private void toolStripMenuItem_15_Click(object sender, EventArgs e)
	{
		ShowConfigForm(8);
	}

	private void toolStripMenuItem_16_Click(object sender, EventArgs e)
	{
		ShowConfigForm(9);
	}

	private void toolStripMenuItem_17_Click(object sender, EventArgs e)
	{
		ShowConfigForm(7);
	}

	private void toolStripMenuItem_17_Click_1(object sender, EventArgs e)
	{
		ShowConfigForm(2);
	}

	private void toolStripMenuItem_19_Click(object sender, EventArgs e)
	{
		Help.ShowHelp(this, "http://127.0.0.1");
	}

	private void toolStripMenuItem_2_Click(object sender, EventArgs e)
	{
		CollectReplace collectReplace = new CollectReplace();
		ShowDockContent(collectReplace);
	}

	private void toolStripMenuItem_20_Click(object sender, EventArgs e)
	{
		NovelSpiderUpdate novelSpiderUpdate = new NovelSpiderUpdate();
		ShowDockContent(novelSpiderUpdate);
	}

	private void toolStripMenuItem_21_Click(object sender, EventArgs e)
	{
		ShowWelcomeForm();
	}

	private void toolStripMenuItem_22_Click(object sender, EventArgs e)
	{
		ShowConfigForm(4);
	}

	private void toolStripMenuItem_23_Click(object sender, EventArgs e)
	{
		HelpTaskNovelInfo helpTaskNovelInfo = new HelpTaskNovelInfo();
		ShowDockContent(helpTaskNovelInfo);
	}

	private void toolStripMenuItem_24_Click(object sender, EventArgs e)
	{
		if (Configs.BaseConfig.LogType == 0)
		{
			string text = Application.StartupPath + "\\Log\\" + DateTime.Today.ToString("yyyyMMdd") + ".Log";
			if (!File.Exists(text))
			{
				File.Create(text);
			}
			Process.Start(text);
		}
		else
		{
			HelpLog helpLog = new HelpLog();
			ShowDockContent(helpLog);
		}
	}

	private void toolStripMenuItem_26_Click(object sender, EventArgs e)
	{
		HelpUpdateNovel helpUpdateNovel = new HelpUpdateNovel();
		ShowDockContent(helpUpdateNovel);
	}

	private void toolStripMenuItem_27_Click(object sender, EventArgs e)
	{
		HelpConversion helpConversion = new HelpConversion();
		ShowDockContent(helpConversion);
	}

	private void toolStripMenuItem_28_Click(object sender, EventArgs e)
	{
		WebService webService = new WebService();
		try
		{
			// IP authorization removed
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripMenuItem_29_Click(object sender, EventArgs e)
	{
		HelpUpdateNovelBySelf helpUpdateNovelBySelf = new HelpUpdateNovelBySelf();
		ShowDockContent(helpUpdateNovelBySelf);
	}

	private void toolStripMenuItem_3_Click(object sender, EventArgs e)
	{
		CollectManual collectManual = new CollectManual();
		ShowDockContent(collectManual);
	}

	private void toolStripMenuItem_7_Click(object sender, EventArgs e)
	{
		HelpBatchCreate helpBatchCreate = new HelpBatchCreate();
		ShowDockContent(helpBatchCreate);
	}

	private void toolStripMenuItem_9_Click(object sender, EventArgs e)
	{
		HelpDeleteNovel helpDeleteNovel = new HelpDeleteNovel();
		ShowDockContent(helpDeleteNovel);
	}

	private void toolStripMenuItem1_Click(object sender, EventArgs e)
	{
		MysqlForm mysqlForm = new MysqlForm();
		ShowDockContent(mysqlForm);
	}

	private void toolStripMenuItem17_Click_1(object sender, EventArgs e)
	{
		ShowConfigForm(3);
	}

	private void toolStripMenuItem2_Click(object sender, EventArgs e)
	{
		CollectRepair collectRepair = new CollectRepair();
		ShowDockContent(collectRepair);
	}

	private static void YourTask()
	{
		if (time.AddMinutes(Configs.BaseConfig.MailTimeNum) <= DateTime.Now)
		{
			time = DateTime.Now;
			NativeMethods.BookMsg = time.AddMinutes(-Configs.BaseConfig.MailTimeNum).ToString("s") + " 至 " + time.ToString("s") + " 采集【" + Configs.BaseConfig.WebSiteName + "】所有规则，共执行书籍<font style=color:red>" + NativeMethods.BookCount + "</font>本/次，成功入库<font style=color:red>" + NativeMethods.ChapterCount + "</font>章节。";
			NativeMethods.ChapterCount = 0;
			NativeMethods.BookCount = 0;
			if (Configs.BaseConfig.MailPass != string.Empty)
			{
				thr = new Thread(GoMail);
				thr.Start();
			}
		}
	}
}



