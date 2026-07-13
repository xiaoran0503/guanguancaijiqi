using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using NovelSpider.Config;
using NovelSpider.Entity;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class HelpTaskNovelInfo : DockContent
{
	private ColumnHeader columnHeader_0;

	private ColumnHeader columnHeader_1;

	private ColumnHeader columnHeader_2;

	private IContainer components;

	private IContainer icontainer_0;

	private ListView listView_0;

	private Timer timer_0;

	public HelpTaskNovelInfo()
	{
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

	private void HelpTaskNovelInfo_FormClosing(object sender, FormClosingEventArgs e)
	{
		timer_0.Stop();
	}

	private void HelpTaskNovelInfo_Load(object sender, EventArgs e)
	{
		timer_0.Start();
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.HelpTaskNovelInfo));
		this.listView_0 = new System.Windows.Forms.ListView();
		this.columnHeader_0 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader_1 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader_2 = new System.Windows.Forms.ColumnHeader();
		this.timer_0 = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.listView_0.Columns.AddRange(new System.Windows.Forms.ColumnHeader[3] { this.columnHeader_0, this.columnHeader_1, this.columnHeader_2 });
		this.listView_0.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listView_0.FullRowSelect = true;
		this.listView_0.Location = new System.Drawing.Point(0, 0);
		this.listView_0.Name = "listView_0";
		this.listView_0.Size = new System.Drawing.Size(452, 169);
		this.listView_0.TabIndex = 0;
		this.listView_0.UseCompatibleStateImageBehavior = false;
		this.listView_0.View = System.Windows.Forms.View.Details;
		this.columnHeader_0.Text = "子窗口ID";
		this.columnHeader_0.Width = 200;
		this.columnHeader_1.Text = "小说编号";
		this.columnHeader_2.Text = "小说名称";
		this.columnHeader_2.Width = 150;
		this.timer_0.Interval = 1000;
		this.timer_0.Tick += new System.EventHandler(timer_0_Tick);
		base.ClientSize = new System.Drawing.Size(452, 169);
		base.Controls.Add(this.listView_0);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "HelpTaskNovelInfo";
		base.ShowInTaskbar = false;
		this.Text = "子窗口冲突监控";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(HelpTaskNovelInfo_FormClosing);
		base.Load += new System.EventHandler(HelpTaskNovelInfo_Load);
		base.ResumeLayout(false);
	}

	private void timer_0_Tick(object sender, EventArgs e)
	{
		listView_0.Items.Clear();
		int num = 0;
		var keys = Configs.TaskNovelInfo.Keys;
		if (Configs.TaskNovelInfo.Count == 0)
		{
			return;
		}
		foreach (string item in keys)
		{
			if (Configs.TaskNovelInfo[item] != null)
			{
				NovelInfo novelInfo = (NovelInfo)Configs.TaskNovelInfo[item];
				string[] items = new string[3]
				{
					item,
					novelInfo.PutID.ToString(),
					novelInfo.Name
				};
				listView_0.Items.Insert(num, new ListViewItem(items));
			}
			else
			{
				listView_0.Items.Insert(num, item);
			}
			num++;
		}
	}
}
