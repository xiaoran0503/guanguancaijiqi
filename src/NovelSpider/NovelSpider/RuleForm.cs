using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class RuleForm : DockContent
{
	private Button button_0;

	private Button button_1;

	private Button button_2;

	private Button button_3;

	private CheckBox checkBox_0;

	private CheckBox checkBox_1;

	private ComboBox comboBox_0;

	private ComboBox comboBox_1;

	private IContainer components;

	private GroupBox groupBox_0;

	private GroupBox groupBox_1;

	private GroupBox groupBox_2;

	private IContainer icontainer_0;

	private int int_0;

	private ListBox listBox_0;

	private PropertyInfo[] propertyInfo_0;

	private RuleConfigInfo ruleConfigInfo_0;

	private Hashtable ruleHash;

	private RuleTestForm ruleTestForm_0;

	private TaskConfigInfo taskConfigInfo_0;

	private TextBox textBox_0;

	private TextBox textBox_1;

	private TextBox textBox_2;

	private TextBox textBox_3;

	private TextBox textBox_4;

	private TextBox textBox_5;

	private ToolTip toolTip_0;

	public RuleForm()
	{
		ruleHash = new Hashtable();
		int_0 = -1;
		ruleConfigInfo_0 = new RuleConfigInfo();
		ruleTestForm_0 = new RuleTestForm();
		taskConfigInfo_0 = new TaskConfigInfo();
		InitializeComponent();
		ruleHash.Add("RuleVersion", "规则版本");
		ruleHash.Add("RuleID", "规则编号");
		ruleHash.Add("GetSiteName", "站点名称");
		ruleHash.Add("GetSiteCharset", "站点编码");
		ruleHash.Add("GetSiteUrl", "站点地址");
		ruleHash.Add("NovelSearchUrl", "站点搜索地址");
		ruleHash.Add("NovelSearchData", "搜索提交内容\r\n{SearchKey} 表示搜索提交的内容");
		ruleHash.Add("NovelSearch_GetNovelKey", "从搜索结果中获得小说编号\r\n{SearchKey} 表示搜索提交的内容\r\n此获得结果存入{NovelKey}变量");
		ruleHash.Add("NovelListUrl", "站点最新列表地址");
		ruleHash.Add("NovelListFilter", "获得书籍列表部分关键HTML，一般可留空");
		ruleHash.Add("NovelList_GetNovelKey", "从最新列表中获得小说编号\r\n此规则中可以同时获得书名以方便手动时查看\r\n此获得结果存入{NovelKey}变量");
		ruleHash.Add("NovelUrl", "小说信息页地址 可调用{NovelKey}变量\r\n{NovelKey}一般情况表示小说编号");
		ruleHash.Add("NovelErr", "小说信息页错误识别标记");
		ruleHash.Add("NovelName", "获得小说名称正则，替换标签♂\r\n支持多模板");
		ruleHash.Add("NovelAuthor", "获得小说作者正则，替换标签♂\r\n支持多模板");
		ruleHash.Add("Isboy", "获得小说大类替换为男女频道正则，替换标签♂\r\n支持多模板");
		ruleHash.Add("LagerSort", "获得小说大类正则，替换标签♂\r\n支持多模板");
		ruleHash.Add("SmallSort", "获得小说小类正则，替换标签♂\r\n支持多模板\r\n如果目标站点没有小类，这里就重复输入一次大类规则");
		ruleHash.Add("NovelIntro", "获得小说简介正则，替换标签♂\r\n支持多模板");
		ruleHash.Add("NovelKeyword", "获得小说主角(关键字)正则，替换标签♂\r\n支持多模板");
		ruleHash.Add("NovelDegree", "获得写作进程正则(请把全本小说替换成完成、完结、完本)，替换标签♂\r\n支持多模板");
		ruleHash.Add("NovelCover", "获得小说封面正则，替换标签♂\r\n支持多模板");
		ruleHash.Add("NovelDefaultCoverUrl", "目标站默认封面地址\n遇到这个地址就不采集它的封面");
		ruleHash.Add("NovelInfo_GetNovelPubKey", "获得小说公众目录页地址正则\r\n支持多模板");
		ruleHash.Add("NovelInfo_GetNovelVipKey", "获得小说VIP目录页地址正则\r\n支持多模板\r\n这个一般无用，可留空");
		ruleHash.Add("PubCookies", "访问公众版需要登陆的Cookies\r\n一般无关，小说阅读网需要这个");
		ruleHash.Add("PubIndexUrl", "公众目录页地址 可调用{NovelPubKey} {NovelKey}变量");
		ruleHash.Add("PubIndexErr", "公众目录页错误识别标记");
		ruleHash.Add("PubVolumeContent", "获得目录部分关键HTML，一般可留空，替换标签♂\r\n替换支持{$分类名称$} {$小说名称$} {$小说作者$}变量");
		ruleHash.Add("PubVolumeSplit", "分割分卷");
		ruleHash.Add("PubVolumeName", "获得分卷名，替换标签♂\r\n支持多模板");
		ruleHash.Add("PubChapterName", "获得章节名，替换标签♂\r\n替换支持{$分类名称$} {$小说名称$} {$小说作者$} {$书卷名称$}变量");
		ruleHash.Add("PubChapter_GetChapterKey", "获得章节地址(章节编号)，所获得的数量必须和章节名相同。记录变量{ChapterKey}");
		ruleHash.Add("PubContentUrl", "章节内容页地址 可调用{ChapterKey} {NovelKey}变量");
		ruleHash.Add("PubContentErr", "章节内容页错误识别标记");
		ruleHash.Add("PubContent_GetTextKey", "内容页中真实内容有JS调用的，获得JS地址 记录变量{TextKey}");
		ruleHash.Add("PubTextUrl", "组合真实内容地址");
		ruleHash.Add("PubContentText", "获得章节内容的正则有((.|\n)+?)、(.+?)、(.*?)等，建议用：(?<str>[\\s|\\S]+?)\r\n替换标签用：♂\r\n支持多模板如：www.xinbiqi.com有两个以上内容模版，旧内容为\"<DIV id=content align=left>\"开始到\"<div id=\"page_ad_7up\">\"结束，新内容为\"<div class=\"articleDiv\">\"开始到\"<div class=\"ad960 - 90\" style=\"margin - top:10px; \">\"结束，我们可以表达为如下正则，两个模版中间用 | 隔开\r\n<DIV id=content align=left>(?<str>[\\s|\\S]+?)<div id=\"page_ad_7up\">|<div class=\"articleDiv\">(?<str>[\\s|\\S]+?)<div class=\"ad960 - 90\"");
		ruleHash.Add("PubContentPageArea", "获取章节内容中的分页区域");
		ruleHash.Add("PubContentPage", "获取章节内容中的分页编码的正则");
		ruleHash.Add("PubContentPageUrl", "内容页翻页地址，通常为第2页以后，可调用{NovelPubKey} {NovelKey} 以及 {Page}变量");
		ruleHash.Add("PubContentPageKey", "内容页翻页结束的标识，也就是说翻页的最后一页具备的特征，可为字符或者正则表达式，如果不填写，将会进行MD5智能判断是否到章节末");
		ruleHash.Add("PubContentChapterName", "获得章节修正名称的正则，自动修复列表名称，替换标签♂\r\n支持多模板\r\n(此为高级功能，用于防采集列表替换章节名称)");
		ruleHash.Add("PubContentChapterNum", "获得章节修正名称的数量(此为高级功能，用于防采集列表替换章节名称的数量，从尾章节向前数)");
		ruleHash.Add("PubContentImages", "章节内容中提取图片正则\r\n此功能已不是老版本的图片章节，而是用于图转文高级功能");
		ruleHash.Add("PubContentReplace", "章节内容替换规则(替换获取图片后)\r\n每行一个替换，格式如下\r\n需要替换的内容♂替换结果\r\n<div.+?>  这个表示过滤\r\n<div.+?>♂<br>  这个表示替换");
	}

	private void button_0_Click(object sender, EventArgs e)
	{
		ruleConfigInfo_0 = (RuleConfigInfo)ConfigFileManager.LoadConfig(comboBox_0.Text, ruleConfigInfo_0);
		propertyInfo_0 = ruleConfigInfo_0.GetType().GetProperties();
		for (int i = 0; i < propertyInfo_0.Length; i++)
		{
			listBox_0.Items.Add(propertyInfo_0[i].Name);
		}
		button_0.Enabled = false;
		comboBox_0.Enabled = false;
		button_1.Enabled = true;
		ConfigFileManager.SaveConfig(comboBox_0.Text, ruleConfigInfo_0);
	}

	private void button_1_Click(object sender, EventArgs e)
	{
		method_0();
		int_0 = -1;
		ConfigFileManager.SaveConfig(comboBox_0.Text, ruleConfigInfo_0);
		button_0.Enabled = true;
		comboBox_0.Enabled = true;
		listBox_0.Items.Clear();
		comboBox_0.Items.Clear();
		string[] array = IO.LoadRules();
		if (array.Length != 0)
		{
			string[] array2 = array;
			foreach (object item in array2)
			{
				comboBox_0.Items.Add(item);
			}
			comboBox_0.Text = comboBox_0.Items[0].ToString();
		}
	}

	private void button_2_Click(object sender, EventArgs e)
	{
		string[] array = IO.LoadRules();
		comboBox_0.Items.Clear();
		if (array.Length != 0)
		{
			string[] array2 = array;
			foreach (object item in array2)
			{
				comboBox_0.Items.Add(item);
			}
			comboBox_0.Text = comboBox_0.Items[0].ToString();
		}
		listBox_0.Items.Clear();
		button_0.Enabled = true;
		comboBox_0.Enabled = true;
	}

	private void button_3_Click(object sender, EventArgs e)
	{
		if (button_0.Enabled)
		{
			MessageBox.Show("我靠，你也得先把规则载入了再测试啊。");
			return;
		}
		method_0();
		ruleTestForm_0.Rule = ruleConfigInfo_0;
		ruleTestForm_0.Task = taskConfigInfo_0;
		ruleTestForm_0.NovelID = textBox_4.Text;
		ruleTestForm_0.ChapterID = textBox_5.Text;
		ruleTestForm_0.ShowDialog();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.RuleForm));
		this.listBox_0 = new System.Windows.Forms.ListBox();
		this.comboBox_0 = new System.Windows.Forms.ComboBox();
		this.textBox_0 = new System.Windows.Forms.TextBox();
		this.textBox_1 = new System.Windows.Forms.TextBox();
		this.comboBox_1 = new System.Windows.Forms.ComboBox();
		this.checkBox_0 = new System.Windows.Forms.CheckBox();
		this.button_0 = new System.Windows.Forms.Button();
		this.button_1 = new System.Windows.Forms.Button();
		this.button_2 = new System.Windows.Forms.Button();
		this.checkBox_1 = new System.Windows.Forms.CheckBox();
		this.toolTip_0 = new System.Windows.Forms.ToolTip(this.components);
		this.textBox_4 = new System.Windows.Forms.TextBox();
		this.textBox_5 = new System.Windows.Forms.TextBox();
		this.button_3 = new System.Windows.Forms.Button();
		this.textBox_2 = new System.Windows.Forms.TextBox();
		this.textBox_3 = new System.Windows.Forms.TextBox();
		this.groupBox_0 = new System.Windows.Forms.GroupBox();
		this.groupBox_1 = new System.Windows.Forms.GroupBox();
		this.groupBox_2 = new System.Windows.Forms.GroupBox();
		this.groupBox_0.SuspendLayout();
		this.groupBox_1.SuspendLayout();
		this.groupBox_2.SuspendLayout();
		base.SuspendLayout();
		this.listBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.listBox_0.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.listBox_0.FormattingEnabled = true;
		this.listBox_0.ItemHeight = 12;
		this.listBox_0.Location = new System.Drawing.Point(12, 38);
		this.listBox_0.Name = "listBox_0";
		this.listBox_0.Size = new System.Drawing.Size(200, 340);
		this.listBox_0.TabIndex = 0;
		this.listBox_0.DrawItem += new System.Windows.Forms.DrawItemEventHandler(listBox_0_DrawItem);
		this.listBox_0.SelectedIndexChanged += new System.EventHandler(listBox_0_SelectedIndexChanged);
		this.comboBox_0.FormattingEnabled = true;
		this.comboBox_0.Location = new System.Drawing.Point(12, 12);
		this.comboBox_0.Name = "comboBox_0";
		this.comboBox_0.Size = new System.Drawing.Size(200, 20);
		this.comboBox_0.TabIndex = 1;
		this.textBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_0.Location = new System.Drawing.Point(6, 42);
		this.textBox_0.Multiline = true;
		this.textBox_0.Name = "textBox_0";
		this.textBox_0.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox_0.Size = new System.Drawing.Size(308, 70);
		this.textBox_0.TabIndex = 2;
		this.textBox_0.Text = "采集规则框";
		this.textBox_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_1.Location = new System.Drawing.Point(6, 20);
		this.textBox_1.Name = "textBox_1";
		this.textBox_1.ReadOnly = true;
		this.textBox_1.Size = new System.Drawing.Size(166, 21);
		this.textBox_1.TabIndex = 3;
		this.textBox_1.Text = "规则名称框";
		this.comboBox_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.comboBox_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_1.FormattingEnabled = true;
		this.comboBox_1.Items.AddRange(new object[6] { "Url", "Match", "Matches", "Spilt", "Replace", "Other" });
		this.comboBox_1.Location = new System.Drawing.Point(196, 18);
		this.comboBox_1.Name = "comboBox_1";
		this.comboBox_1.Size = new System.Drawing.Size(118, 20);
		this.comboBox_1.TabIndex = 4;
		this.checkBox_0.AutoSize = true;
		this.checkBox_0.Location = new System.Drawing.Point(6, 20);
		this.checkBox_0.Name = "checkBox_0";
		this.checkBox_0.Size = new System.Drawing.Size(96, 16);
		this.checkBox_0.TabIndex = 5;
		this.checkBox_0.Text = "不区分大小写";
		this.toolTip_0.SetToolTip(this.checkBox_0, "指定不区分大小写的匹配");
		this.checkBox_0.UseVisualStyleBackColor = true;
		this.button_0.Location = new System.Drawing.Point(218, 12);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 20);
		this.button_0.TabIndex = 7;
		this.button_0.Text = "载入";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(button_0_Click);
		this.button_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button_1.Enabled = false;
		this.button_1.Location = new System.Drawing.Point(463, 12);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(75, 20);
		this.button_1.TabIndex = 8;
		this.button_1.Text = "保存";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.button_2.Location = new System.Drawing.Point(380, 12);
		this.button_2.Name = "button_2";
		this.button_2.Size = new System.Drawing.Size(75, 20);
		this.button_2.TabIndex = 9;
		this.button_2.Text = "刷新";
		this.button_2.UseVisualStyleBackColor = true;
		this.button_2.Click += new System.EventHandler(button_2_Click);
		this.checkBox_1.AutoSize = true;
		this.checkBox_1.Location = new System.Drawing.Point(108, 20);
		this.checkBox_1.Name = "checkBox_1";
		this.checkBox_1.Size = new System.Drawing.Size(72, 16);
		this.checkBox_1.TabIndex = 10;
		this.checkBox_1.Text = "单行模式";
		this.toolTip_0.SetToolTip(this.checkBox_1, "制定单行模式。使 . 匹配任意字符(包括换行字符)");
		this.checkBox_1.UseVisualStyleBackColor = true;
		this.textBox_4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_4.Location = new System.Drawing.Point(178, 20);
		this.textBox_4.Name = "textBox_4";
		this.textBox_4.Size = new System.Drawing.Size(65, 21);
		this.textBox_4.TabIndex = 14;
		this.textBox_4.Text = "0";
		this.toolTip_0.SetToolTip(this.textBox_4, "单本测试 - 小说ID");
		this.textBox_5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_5.Location = new System.Drawing.Point(249, 20);
		this.textBox_5.Name = "textBox_5";
		this.textBox_5.Size = new System.Drawing.Size(65, 21);
		this.textBox_5.TabIndex = 16;
		this.textBox_5.Text = "0";
		this.toolTip_0.SetToolTip(this.textBox_5, "单本测试 - 章节ID");
		this.button_3.Location = new System.Drawing.Point(299, 12);
		this.button_3.Name = "button_3";
		this.button_3.Size = new System.Drawing.Size(75, 20);
		this.button_3.TabIndex = 11;
		this.button_3.Text = "测试规则";
		this.button_3.UseVisualStyleBackColor = true;
		this.button_3.Click += new System.EventHandler(button_3_Click);
		this.textBox_2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_2.Location = new System.Drawing.Point(6, 47);
		this.textBox_2.Multiline = true;
		this.textBox_2.Name = "textBox_2";
		this.textBox_2.ReadOnly = true;
		this.textBox_2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox_2.Size = new System.Drawing.Size(308, 64);
		this.textBox_2.TabIndex = 13;
		this.textBox_2.Text = "规则说明框";
		this.textBox_3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_3.Location = new System.Drawing.Point(6, 20);
		this.textBox_3.Multiline = true;
		this.textBox_3.Name = "textBox_3";
		this.textBox_3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox_3.Size = new System.Drawing.Size(308, 68);
		this.textBox_3.TabIndex = 15;
		this.textBox_3.Text = "替换规则框";
		this.groupBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_0.Controls.Add(this.textBox_5);
		this.groupBox_0.Controls.Add(this.textBox_4);
		this.groupBox_0.Controls.Add(this.textBox_1);
		this.groupBox_0.Controls.Add(this.textBox_2);
		this.groupBox_0.Location = new System.Drawing.Point(218, 38);
		this.groupBox_0.Name = "groupBox_0";
		this.groupBox_0.Size = new System.Drawing.Size(320, 117);
		this.groupBox_0.TabIndex = 16;
		this.groupBox_0.TabStop = false;
		this.groupBox_0.Text = "规则名称";
		this.groupBox_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_1.Controls.Add(this.checkBox_0);
		this.groupBox_1.Controls.Add(this.comboBox_1);
		this.groupBox_1.Controls.Add(this.checkBox_1);
		this.groupBox_1.Controls.Add(this.textBox_0);
		this.groupBox_1.Location = new System.Drawing.Point(218, 161);
		this.groupBox_1.Name = "groupBox_1";
		this.groupBox_1.Size = new System.Drawing.Size(320, 118);
		this.groupBox_1.TabIndex = 17;
		this.groupBox_1.TabStop = false;
		this.groupBox_1.Text = "采集规则";
		this.groupBox_2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_2.Controls.Add(this.textBox_3);
		this.groupBox_2.Location = new System.Drawing.Point(218, 285);
		this.groupBox_2.Name = "groupBox_2";
		this.groupBox_2.Size = new System.Drawing.Size(320, 94);
		this.groupBox_2.TabIndex = 18;
		this.groupBox_2.TabStop = false;
		this.groupBox_2.Text = "替换规则";
		base.ClientSize = new System.Drawing.Size(550, 390);
		base.Controls.Add(this.groupBox_2);
		base.Controls.Add(this.groupBox_1);
		base.Controls.Add(this.groupBox_0);
		base.Controls.Add(this.button_3);
		base.Controls.Add(this.button_2);
		base.Controls.Add(this.button_1);
		base.Controls.Add(this.button_0);
		base.Controls.Add(this.comboBox_0);
		base.Controls.Add(this.listBox_0);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "RuleForm";
		this.Text = "规则管理器";
		base.Load += new System.EventHandler(RuleForm_Load);
		this.groupBox_0.ResumeLayout(false);
		this.groupBox_0.PerformLayout();
		this.groupBox_1.ResumeLayout(false);
		this.groupBox_1.PerformLayout();
		this.groupBox_2.ResumeLayout(false);
		this.groupBox_2.PerformLayout();
		base.ResumeLayout(false);
	}

	private void listBox_0_DrawItem(object sender, DrawItemEventArgs e)
	{
		Brush brush = Brushes.Black;
		if (e.Index > -1)
		{
			if (listBox_0.Items[e.Index].ToString() == "PubContentChapterName" || listBox_0.Items[e.Index].ToString() == "PubContentChapterNum")
			{
				brush = Brushes.Red;
			}
			if (listBox_0.Items[e.Index].ToString() == "PubChapterPageKey" || listBox_0.Items[e.Index].ToString() == "PubChapterPageUrl" || listBox_0.Items[e.Index].ToString() == "PubContentPageKey" || listBox_0.Items[e.Index].ToString() == "PubContentPageUrl" || listBox_0.Items[e.Index].ToString() == "NovelListFilter" || listBox_0.Items[e.Index].ToString() == "PubContentPageArea" || listBox_0.Items[e.Index].ToString() == "PubContentPage" || listBox_0.Items[e.Index].ToString() == "Isboy")
			{
				brush = Brushes.Green;
			}
			e.DrawBackground();
			e.Graphics.DrawString(listBox_0.Items[e.Index].ToString(), e.Font, brush, e.Bounds);
			e.DrawFocusRectangle();
		}
	}

	private void listBox_0_SelectedIndexChanged(object sender, EventArgs e)
	{
		method_0();
		if (button_0.Enabled)
		{
			return;
		}
		int selectedIndex = listBox_0.SelectedIndex;
		if (selectedIndex == -1)
		{
			return;
		}
		textBox_1.Text = propertyInfo_0[selectedIndex].Name;
		textBox_2.Text = ruleHash[propertyInfo_0[selectedIndex].Name].ToString();
		if (propertyInfo_0[selectedIndex].PropertyType == typeof(RegexInfo))
		{
			RegexInfo regexInfo = ((RegexInfo)propertyInfo_0[selectedIndex].GetValue(ruleConfigInfo_0, null)) ?? new RegexInfo();
			comboBox_1.Text = regexInfo.Method;
			textBox_0.Text = regexInfo.Pattern.Trim().Replace("\r", "").Replace("\n\n", "\n")
				.Replace("\n\n", "\n")
				.Replace("\n\n", "\n")
				.Replace("\n", "\r\n");
			textBox_3.Text = regexInfo.FilterPattern.Replace("\r", "").Replace("\n\n", "\n").Replace("\n\n", "\n")
				.Replace("\n\n", "\n")
				.Replace("\n", "\r\n");
			checkBox_0.Checked = false;
			checkBox_1.Checked = false;
			if (regexInfo.Options == RegexOptions.IgnoreCase)
			{
				checkBox_0.Checked = true;
			}
			if (regexInfo.Options == RegexOptions.Singleline)
			{
				checkBox_1.Checked = true;
			}
			if (regexInfo.Options == (RegexOptions.IgnoreCase | RegexOptions.Singleline))
			{
				checkBox_0.Checked = true;
				checkBox_1.Checked = true;
			}
			return;
		}
		comboBox_1.Text = "Other";
		try
		{
			textBox_0.Text = propertyInfo_0[selectedIndex].GetValue(ruleConfigInfo_0, null).ToString();
			textBox_3.Text = "";
		}
		catch
		{
			textBox_0.Text = "";
			textBox_3.Text = "";
		}
	}

	private void method_0()
	{
		if (int_0 >= 0)
		{
			int num = int_0;
			textBox_1.Text = propertyInfo_0[num].Name;
			if (propertyInfo_0[num].PropertyType == typeof(RegexInfo))
			{
				RegexInfo regexInfo = new RegexInfo
				{
					RegexName = textBox_1.Text,
					Method = comboBox_1.Text,
					Pattern = textBox_0.Text,
					FilterPattern = textBox_3.Text,
					Options = RegexOptions.None
				};
				RegexInfo regexInfo2 = regexInfo;
				if (checkBox_0.Checked)
				{
					regexInfo2.Options = RegexOptions.IgnoreCase;
				}
				if (checkBox_1.Checked)
				{
					regexInfo2.Options = RegexOptions.Singleline;
				}
				if (checkBox_0.Checked && checkBox_1.Checked)
				{
					regexInfo2.Options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
				}
				propertyInfo_0[num].SetValue(ruleConfigInfo_0, regexInfo2, null);
			}
			else
			{
				propertyInfo_0[num].SetValue(ruleConfigInfo_0, textBox_0.Text, null);
			}
		}
		int_0 = listBox_0.SelectedIndex;
	}

	private void RuleForm_Load(object sender, EventArgs e)
	{
		string[] array = IO.LoadRules();
		if (array.Length != 0)
		{
			string[] array2 = array;
			foreach (object item in array2)
			{
				comboBox_0.Items.Add(item);
			}
			comboBox_0.Text = comboBox_0.Items[0].ToString();
		}
		taskConfigInfo_0 = (TaskConfigInfo)ConfigFileManager.LoadConfig("TaskConfig.xml", taskConfigInfo_0);
		taskConfigInfo_0.Proxy = false;
		taskConfigInfo_0.PubContentUrlProxy = false;
		taskConfigInfo_0.PubIndexUrlProxy = false;
		taskConfigInfo_0.NovelListUrlProxy = false;
		taskConfigInfo_0.NovelUrlProxy = false;
	}
}
