using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NovelSpider.Common;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class HelpLog : DockContent
{
	private Button button_0;

	private ComboBox comboBox_0;

	private ComboBox comboBox_1;

	private ComboBox comboBox_2;

	private ComboBox comboBox_3;

	private DataGridView dataGridView_0;

	private IContainer icontainer_0;

	public HelpLog()
	{
		InitializeComponent();
	}

	private void btnShowLog_Click(object sender, EventArgs e)
	{
		FileInfo fileInfo = new FileInfo(comboBox_0.Text);
		if (!fileInfo.Exists)
		{
			return;
		}
		string string_ = "Data Source=" + fileInfo.FullName;
		string text = "Select * From [TaskLog] Where 1=1";
		if (comboBox_1.Text != "EXIT" && comboBox_1.Text != "")
		{
			if (comboBox_1.Text == "")
			{
				comboBox_1.Text = "EXID";
			}
			if (comboBox_1.Text != "EXID")
			{
				comboBox_1.Text = comboBox_1.Text.Split(' ')[0];
			}
			text = text + " And EXID= " + comboBox_1.Text;
		}
		if (comboBox_2.Text != "书名" && comboBox_2.Text != "")
		{
			text = text + " And NovelName= '" + comboBox_2.Text + "'";
		}
		if (comboBox_3.Text != "提示信息" && comboBox_3.Text != "")
		{
			text = text + " And Exmsg Like '%" + comboBox_3.Text + "%'";
		}
		dataGridView_0.DataSource = SQLiteHelper.ExecuteDataset(string_, text).Tables[0];
		dataGridView_0.AutoResizeColumns();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && icontainer_0 != null)
		{
			icontainer_0.Dispose();
		}
		base.Dispose(disposing);
	}

	private void HelpLog_Load(object sender, EventArgs e)
	{
		string[] array = IO.LoadLogs();
		int num = -1;
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (text.EndsWith("db3"))
			{
				num++;
				comboBox_0.Items.Insert(num, text);
			}
		}
		if (num >= 0)
		{
			comboBox_0.SelectedIndex = num;
		}
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.HelpLog));
		this.dataGridView_0 = new System.Windows.Forms.DataGridView();
		this.comboBox_0 = new System.Windows.Forms.ComboBox();
		this.comboBox_1 = new System.Windows.Forms.ComboBox();
		this.comboBox_2 = new System.Windows.Forms.ComboBox();
		this.comboBox_3 = new System.Windows.Forms.ComboBox();
		this.button_0 = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.dataGridView_0).BeginInit();
		base.SuspendLayout();
		this.dataGridView_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.dataGridView_0.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dataGridView_0.Location = new System.Drawing.Point(12, 38);
		this.dataGridView_0.Name = "dataGridView_0";
		this.dataGridView_0.RowTemplate.Height = 23;
		this.dataGridView_0.Size = new System.Drawing.Size(598, 373);
		this.dataGridView_0.TabIndex = 0;
		this.comboBox_0.FormattingEnabled = true;
		this.comboBox_0.Location = new System.Drawing.Point(12, 12);
		this.comboBox_0.Name = "comboBox_0";
		this.comboBox_0.Size = new System.Drawing.Size(150, 20);
		this.comboBox_0.TabIndex = 1;
		this.comboBox_1.DropDownWidth = 300;
		this.comboBox_1.FormattingEnabled = true;
		this.comboBox_1.Items.AddRange(new object[32]
		{
			"EXID", "0 未知错误", "", "21 FTP负载失败", "", "101 子窗口冲突", "102 检查子窗口冲突失败", "", "120 对比最新章节失败", "121 空章节",
			"122 检查到重复章节", "124 只采集文字章节时发现图片章节", "125 设置不添加新书", "", "130 限制章节字数小于多少字的章节不采集", "131 章节数量小于限制", "132 对比最新章节成功！但需要采集到章节数超限。", "134 限制小说_黑名单", "135 限制小说_不在白名单", "136 过滤分卷名",
			"", "200 小说信息页发生问题", "210 小说目录页发生问题", "214 章节组为空", "220 小说内容页发生问题", "", "410 操作本站小说列表发生问题", "420 操作本站小说信息发生问题", "430 操作本站章节列表发生问题", "440 操作本站章节信息发生问题",
			"441 InsertChapter发生问题", "442 UpdateChapter发生问题"
		});
		this.comboBox_1.Location = new System.Drawing.Point(168, 12);
		this.comboBox_1.Name = "comboBox_1";
		this.comboBox_1.Size = new System.Drawing.Size(50, 20);
		this.comboBox_1.TabIndex = 2;
		this.comboBox_1.Text = "EXID";
		this.comboBox_2.FormattingEnabled = true;
		this.comboBox_2.Items.AddRange(new object[1] { "书名" });
		this.comboBox_2.Location = new System.Drawing.Point(224, 12);
		this.comboBox_2.Name = "comboBox_2";
		this.comboBox_2.Size = new System.Drawing.Size(108, 20);
		this.comboBox_2.TabIndex = 3;
		this.comboBox_2.Text = "书名";
		this.comboBox_3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.comboBox_3.FormattingEnabled = true;
		this.comboBox_3.Items.AddRange(new object[1] { "提示信息" });
		this.comboBox_3.Location = new System.Drawing.Point(338, 12);
		this.comboBox_3.Name = "comboBox_3";
		this.comboBox_3.Size = new System.Drawing.Size(191, 20);
		this.comboBox_3.TabIndex = 4;
		this.comboBox_3.Text = "提示信息";
		this.button_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button_0.Location = new System.Drawing.Point(535, 12);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 20);
		this.button_0.TabIndex = 5;
		this.button_0.Text = "列出日志";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(btnShowLog_Click);
		base.AcceptButton = this.button_0;
		base.ClientSize = new System.Drawing.Size(622, 423);
		base.Controls.Add(this.button_0);
		base.Controls.Add(this.comboBox_3);
		base.Controls.Add(this.comboBox_2);
		base.Controls.Add(this.comboBox_1);
		base.Controls.Add(this.comboBox_0);
		base.Controls.Add(this.dataGridView_0);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "HelpLog";
		this.Text = "查看日志";
		base.Load += new System.EventHandler(HelpLog_Load);
		((System.ComponentModel.ISupportInitialize)this.dataGridView_0).EndInit();
		base.ResumeLayout(false);
	}
}
