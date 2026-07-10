using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using MySqlConnector;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Local;
using NovelSpider.Local.Jieqi;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class ConfigForm : DockContent
{
	public BackgroundWorker backgroundWorker1;

	private Button 初始化标签表;

	private Button 查看推送状态;

	private Button 保存;

	private Button 取消配置;

	private Button 拼音化已有小说;

	private Button 自动配置站群;

	private Button 测试发信;

	private Button 测试数据库;

	private Button 解析图片;

	private Button 选择图片;

	private CheckBox 是否生成全文;

	private CheckBox 是否生成内容;

	private CheckBox 是否生成HTML添加文字广告;

	private CheckBox 是否添加文字广告;

	private CheckBox 是否电子书添加文字广告;

	private CheckBox 小说封面位置;

	private CheckBox 章节图片位置;

	private CheckBox 是否启用章节实际内容;

	private CheckBox 是否启用自定义HTML模板;

	private CheckBox 是否启用自定义生成路径;

	private CheckBox 是否否生成OPF;

	private CheckBox 是否生成目录;

	private CheckBox 是否生成CHM;

	private CheckBox 是否生成JAR;

	private CheckBox 是否生成UMD;

	private CheckBox 是否生成TXT;

	private CheckBox 星否生成ZIP;

	private CheckBox 是否启用JS调用真实内容;

	private CheckBox 中译英;

	private CheckBox 是否使用默认小类;

	private CheckBox 是否使用默认大类;

	private CheckBox Debug模式;

	private CheckBox 是否启用章节名数字化;

	private CheckBox 是否启用内链;

	private CheckedListBox 日志记录Box;

	private CheckBox 是否启用百度推送;

	private CheckBox 是否生成WAP页面;

	private CheckBox 是否启用标签内链;

	private ComboBox 推送类型Box;

	private ComboBox 小说系统版本号;

	private ComboBox 小说系统名;

	private ComboBox 日志格式BOX;

	private ComboBox UA;

	private ComboBox 推荐榜获取形式Box;

	private ComboBox 内容上下页后缀Box;

	private IContainer components;

	private FolderBrowserDialog folderBrowserDialog_0;

	private FontDialog fontDialog_0;

	private GroupBox 文字广告集合;

	private GroupBox 添加文字广告;

	private GroupBox 违禁小说过滤;

	private GroupBox 章节内容非法字符过滤;

	private GroupBox 章节内容非法字符替换;

	private GroupBox 固定位置添加广告;

	private GroupBox 电子书广告;

	private GroupBox SEO优化;

	private GroupBox 内链设置;

	private GroupBox 拼音数字选择;

	private GroupBox 防采集及站群设置;

	private GroupBox 空章节自定义;

	private GroupBox 图版转文字设置;

	private GroupBox 标签内链设置;

	private GroupBox 推送设置;

	private GroupBox WAP生成设置;

	private IContainer icontainer_0;

	private int imgHeight;

	private string imgPath;

	private int imgWidth;

	private Label 小类;

	private Label 大类;

	private Label 网站路径;

	private Label 硬盘路径;

	private Label label_15;

	private Label label_16;

	private Label label_17;

	private Label label_18;

	private Label 每个章节添加几个广告;

	private Label 默认小类;

	private Label 限制只在以下分卷中添加文字广告;

	private Label 默认分卷名;

	private Label label_3;

	private Label 小说系统版本;

	private Label 默认替换字符;

	private Label HTTP超时设置;

	private Label 接收报告邮箱;

	private Label cookies设置;

	private Label label_4;

	private Label 留空表示不限制分卷;

	private Label 章节尾部广告;

	private Label label_5;

	private Label 章节头部广告;

	private Label 基本配置小提示;

	private Label 需要重启采集器;

	private Label 数据库需要重启采集器;

	private Label 日志记录模式;

	private Label 电子书尾广告;

	private Label 电子书头广告;

	private Label 电子书URL;

	private Label 电子书根目录;

	private Label 尾页的下一页;

	private Label 首页的上一页;

	private Label 选择需要记录的日志项;

	private Label UserAgent;

	private Label 入库小说系统;

	private Label 数据库连接字符串;

	private Label 本地网站目录;

	private Label 网站名称;

	private Label 内容邻居数;

	private Label 内容上下页后缀;

	private Label 将数据库已有小说进行拼音化;

	private Label 内链密度设置;

	private Label 生成目录样式;

	private Label 目录页防采集个章节;

	private Label 目录页最新章节数;

	private Label 日志保存周期;

	private Label 目录页最新个章节;

	private Label 数字拼音选择后自动升级数据库;

	private Label 前推荐词;

	private Label 空章节说明;

	private Label 目录推荐数;

	private Label label22;

	private Label 内容推荐数;

	private Label 章节字数小于字数;

	private Label 章节字数小于;

	private Label 单次循环后调用页面地址;

	private Label label27;

	private Label EMail用户名;

	private Label Smtp服务器地址;

	private Label 后长尾词;

	private Label 发送间隔时间;

	private Label EMail密码;

	private Label 推荐榜获取形式;

	private Label 内链接模版;

	private Label 模拟章节目录;

	private Label 目录页模拟章节数;

	private Label label36;

	private Label 目录最新章标签;

	private Label 必要组件;

	private Label PC域名;

	private Label 排行榜地址;

	private Label PCToken;

	private Label WAP目录模板;

	private Label label42;

	private Label WAP内容模板;

	private Label WAP域名;

	private Label 推送URL;

	private Label 推送类型;

	private Label 推送数量;

	private Label 拼音数字选择label;

	private Label 选择生成目录为拼音或ID;

	private Label 选择生成目录的格式;

	private Label 目录页防采集倒数;

	private Label 目录邻居数;

	private Label 最新推送情况;

	private Label 标签内链地址;

	private LinkLabel 获取COOKIES;

	private LinkLabel 查看配置;

	private LinkLabel 选择目录;

	private LinkLabel 载入设置;

	private LinkLabel MSSQL;

	private LinkLabel MYSQL;

	private LinkLabel 模拟章节目录选择;

	public BackgroundWorker MailWorker;

	private MaskedTextBox 前推荐词Box;

	private MaskedTextBox 后长尾词Box;

	private MaskedTextBox 排行榜地址Box;

	private TextBox 空章节替换内容Box;

	private NumericUpDown 推送数量Num;

	private NumericUpDown 添加文字广告个数;

	private NumericUpDown http超时;

	private NumericUpDown 目录页防采集倒数Box;

	private NumericUpDown 目录页模拟章节数Box;

	private NumericUpDown 目录邻居数BOX;

	private NumericUpDown 内容邻居数Box;

	private NumericUpDown 目录页最新章节数Box;

	private NumericUpDown 内链密度设置Box;

	private NumericUpDown 目录推荐数Box;

	private NumericUpDown 内容推荐数Box;

	private NumericUpDown 章节字数小于Box;

	private NumericUpDown 发送间隔box;

	private ComboBox 拼音数字选择Box;

	private ComboBox 生成目录样式Box;

	private OpenFileDialog openFileDialog_0;

	private CheckBox 是否启用图转文;

	private CheckBox 是否启用空章节替换;

	private ProgressBar 查看推送状态进度;

	private bool picEnd;

	private string picPath;

	private ComboBox 日志保存周期BOX;

	private string[] string_0;

	public TabControl 日志记录;

	private TabPage 分类对应;

	private TabPage 生成设置;

	private TabPage 基本设置;

	private TabPage 文字广告;

	private TabPage 过滤替换;

	private TabPage 电子书设置;

	private TabPage 日志选择;

	private TabPage 高级设置;

	private TabPage 附加设置;

	private TabPage 图转文设置;

	private TabPage 超级功能;

	private TextBox WAP内容模板Box;

	private TextBox WAP根目录Box;

	private TextBox WAP目录模板Box;

	private TextBox WAP域名Box;

	private string text;

	private TextBox 小类对应BOX;

	private TextBox 大类对应BOX;

	private TextBox 全文硬盘路径;

	private TextBox 内容URL路径;

	private TextBox 内容硬盘路径;

	private TextBox 章节目录URL路径;

	private TextBox 章节目录硬盘路径;

	private TextBox 默认小类BOX;

	private TextBox 文字广告集合Box;

	private TextBox 添加文字广告分卷限制;

	private TextBox 默认分卷名字;

	private TextBox 默认大类Box;

	private TextBox 违禁小说过滤Box;

	private TextBox 章节内容非法字符过滤Box;

	private TextBox 默认替换字符Box;

	private TextBox 章节内容非法字符替换Box;

	private TextBox 接报告邮箱;

	private TextBox cookies;

	private TextBox textBox_4;

	private TextBox 章节尾部广告Box;

	private TextBox 章节头部广告Box;

	private TextBox 小说封面硬盘路径;

	private TextBox 章节图片硬盘路径;

	private TextBox 实际章节硬盘路径;

	private TextBox OPFURL路径;

	private TextBox OPF硬盘路径;

	private TextBox textBox_5;

	private TextBox CHM_URL;

	private TextBox CHM根目录;

	private TextBox JAR_URL;

	private TextBox JAR根目录;

	private TextBox UMD_URL;

	private TextBox UMD根目录;

	private TextBox TXT_URL;

	private TextBox TXT根目录;

	private TextBox ZIP_URL;

	private TextBox ZIP根目录;

	private TextBox 电子书头广告Box;

	private TextBox 电子书尾广告Box;

	private TextBox 封面URL路径;

	private TextBox 图片章节URL路径;

	private TextBox 实际章节内容URL路径;

	private TextBox 生成设置其它Box;

	private TextBox 尾页的下一页Box;

	private TextBox 首页的上一页Box;

	private TextBox 数据库配置地址;

	private TextBox 网站硬盘根目录;

	private TextBox 全文URL路径;

	private TextBox 单次循环后调用页面列表;

	private TextBox mail名称;

	private TextBox Smtp服务器;

	private TextBox mail密码;

	private TextBox 内链接模版Box;

	private TextBox 模拟章节目录Box;

	private TextBox 图转文内容结果;

	private TextBox 图片文件BOX;

	private ToolTip toolTip_0;

	private TextBox PC域名Box;

	private TextBox PCTokenBox;

	private TextBox 推送URLBox;

	private TextBox 标签内链地址Box;

	private WebBrowser webBrowser;

	private MaskedTextBox 网站名称TEXT;

	private TabPage 杰奇目录DIY;

	private Label 模板路径为相对主目录路径;

	private RichTextBox 杰奇DIY说明2;

	private RichTextBox 杰奇DIY说明1;

	private TextBox 信息页网站调用URL;

	private TextBox OPF网站调用URL;

	private TextBox TXT网站调用URL;

	private TextBox 章节内容网站调用URL;

	private TextBox 章节列表网站调用URL;

	private Label OPF文件硬盘路径;

	private Label TXT文件硬盘路径;

	private Label 章节硬盘路径;

	private Label 章节列表硬盘路径;

	private Label 文章信息硬盘路径;

	private TextBox 章节模板路径;

	private Label 章节页模板;

	private TextBox 目录模板路径;

	private Label 目录页模板;

	private TextBox 信息页模版路径;

	private Label 信息页模板;

	private TextBox 首页模板路径;

	private Label 首页模板;

	private TextBox OPF硬盘文件夹;

	private TextBox TXT硬盘文件夹;

	private TextBox 章节HTML硬盘文件夹;

	private TextBox 目录HTML硬盘文件夹;

	private TextBox 信息页HTML硬盘文件夹;

	private Label OPF文件URL;

	private Label TXT文件URL;

	private Label 章节内容页URL;

	private Label 章节列表页URL;

	private Label 文章信息页URL;

	private CheckBox 是否使用默认男女频;

	private Label 男女频对应;

	private TextBox 默认男女频对应列表;

	private TextBox 默认男女频text;

	private Label 默认男女频;

	private ComboBox 编码Box;

	private Label 编码label;

	private CheckBox 是否开启站群;

	public ConfigForm()
	{
		using IDisposable constructionScope = PerformanceTelemetry.Measure("ui", "config_form_construct");
		string_0 = new string[26]
		{
			"0 未知错误", "21 FTP负载失败", "101 子窗口冲突", "102 检查子窗口冲突失败", "120 对比最新章节失败", "121 空章节", "122 检查到重复章节", "124 只采集文字章节时发现图片章节", "125 设置不添加新书", "130 限制章节字数小于多少字的章节不采集",
			"131 章节数量小于限制", "132 对比最新章节成功！但需要采集到章节数超限。", "134 限制小说_黑名单", "135 限制小说_不在白名单", "136 过滤分卷名", "137 章节名过滤（章节名过滤作者名、自定义过滤）", "200 小说信息页发生问题", "210 小说目录页发生问题", "214 章节组为空", "220 小说内容页发生问题",
			"410 操作本站小说列表发生问题", "420 操作本站小说信息发生问题", "430 操作本站章节列表发生问题", "440 操作本站章节信息发生问题", "441 InsertChapter发生问题", "442 UpdateChapter发生问题"
		};
		picPath = Application.StartupPath + "\\images\\test.jpg";
		imgPath = string.Empty;
		picEnd = false;
		imgWidth = 0;
		imgHeight = 0;
		text = string.Empty;
		InitializeComponent();
	}

	private WebBrowser EnsureImageBrowser()
	{
		if (webBrowser != null && !webBrowser.IsDisposed)
		{
			return webBrowser;
		}

		webBrowser = new WebBrowser
		{
			Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
			Location = new Point(9, 790),
			Margin = new Padding(0),
			Name = "webBrowser",
			ScriptErrorsSuppressed = true,
			ScrollBarsEnabled = false,
			Size = new Size(592, 119),
			TabIndex = 13
		};
		webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
		Controls.Add(webBrowser);
		return webBrowser;
	}

	private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
	{
		if (backgroundWorker1.CancellationPending)
		{
			e.Cancel = true;
			return;
		}
		if (File.Exists(imgPath))
		{
			Image image = Image.FromFile(imgPath);
			imgWidth = image.Width;
			imgHeight = image.Height;
			image.Dispose();
		}
		BeginInvoke(new MethodInvoker(() =>
		{
			if (!IsDisposed)
			{
				EnsureImageBrowser().Navigate(imgPath);
			}
		}));
		for (int i = 0; i < 10000; i++)
		{
			Thread.Sleep(100);
			if (picEnd)
			{
				break;
			}
		}
		try
		{
			text = WaterMark.ImgToText(picPath);
		}
		catch (Exception ex)
		{
			text = ex.Message;
		}
	}

	private void btnInnitTagTable_Click(object sender, EventArgs e)
	{
		NovelSpider.Local.LocalProvider.GetInstance().CreateTagTable();
	}

	private void btnViewBaiduPush_Click(object sender, EventArgs e)
	{
		FileInfo fileInfo = new FileInfo(Application.StartupPath + "/Log/BaiduPush.db3");
		string text = "Data Source=" + fileInfo.FullName;
		if (fileInfo.Exists)
		{
			string string_ = "SELECT REMAIN,SUCCESS,LASTTIME FROM [PushLog] ";
			DataSet dataSet = SQLiteHelper.ExecuteDataset(text, string_);
			if (dataSet != null && dataSet.Tables[0].Rows.Count >= 1)
			{
				string text2 = dataSet.Tables[0].Rows[0]["REMAIN"].ToString();
				string text3 = dataSet.Tables[0].Rows[0]["SUCCESS"].ToString();
				string text4 = dataSet.Tables[0].Rows[0]["LASTTIME"].ToString();
				查看推送状态进度.Maximum = int.Parse(text3) + int.Parse(text2);
				查看推送状态进度.Minimum = 0;
				查看推送状态进度.Value = int.Parse(text3);
				string text5 = "今日已推送成功" + text3 + "条，剩余可推送条数为" + text2 + "，最后推送时间为" + text4;
				最新推送情况.Text = text5;
			}
		}
	}

	private void button_0_Click(object sender, EventArgs e)
	{
		writeToConfigsFile();
		NovelSpider.Local.LocalProvider.ResetProvider();
		Close();
	}

	private void button_1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		string text = "";
		NovelSpider.Local.LocalProvider.GetInstance().PinyinHua(text);
	}

	private void button3_Click(object sender, EventArgs e)
	{
		Configs.BaseConfig.MailSmtp = Smtp服务器.Text;
		Configs.BaseConfig.MailUser = mail名称.Text;
		Configs.BaseConfig.MailPass = mail密码.Text;
		Configs.BaseConfig.MailTitle = 接报告邮箱.Text;
		测试发信.Enabled = false;
		if (!MailWorker.IsBusy)
		{
			MailWorker.RunWorkerAsync();
		}
	}

	private void button4_Click(object sender, EventArgs e)
	{
		Configs.BaseConfig.ConnectionString = 数据库配置地址.Text.Trim();
		string cmsName = 小说系统名.Text.Trim();
		Configs.BaseConfig.CmsName = SupportedCms.NormalizeCmsName(cmsName);
		小说系统名.Text = Configs.BaseConfig.CmsName;
		try
		{
			Configs.BaseConfig.ConnectionString = DatabaseConnectionProfile.NormalizeConnectionString(Configs.BaseConfig.ConnectionString, Configs.BaseConfig.DatabaseServerType, Configs.BaseConfig.DatabaseServerMajorVersion);
			数据库配置地址.Text = Configs.BaseConfig.ConnectionString;
			DatabaseConnectionProfile databaseConnectionProfile = DatabaseConnectionProfile.Detect(Configs.BaseConfig.ConnectionString);
			Configs.BaseConfig.DatabaseServerType = databaseConnectionProfile.ServerType;
			Configs.BaseConfig.DatabaseServerVersion = databaseConnectionProfile.ServerVersion;
			Configs.BaseConfig.DatabaseServerMajorVersion = databaseConnectionProfile.MajorVersion;
			Configs.BaseConfig.DatabaseServerComment = databaseConnectionProfile.VersionComment;
			if (databaseConnectionProfile.ConnectionStringChanged)
			{
				Configs.BaseConfig.ConnectionString = databaseConnectionProfile.RecommendedConnectionString;
				数据库配置地址.Text = databaseConnectionProfile.RecommendedConnectionString;
			}
			MessageBox.Show(databaseConnectionProfile.ToDisplayText(), "MySQL/MariaDB/Percona 连接测试");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message + "\r\n\r\n如使用 MySQL 8/9，请确认连接串包含 AllowPublicKeyRetrieval=True；程序已在缺省时自动补齐该参数。", "MySQL/MariaDB/Percona 连接测试失败");
		}
	}

	private void button5_Click(object sender, EventArgs e)
	{
		if (图片文件BOX.Text == "")
		{
			MessageBox.Show("请选择文件");
			return;
		}
		imgPath = 图片文件BOX.Text;
		picEnd = false;
		EnsureImageBrowser();
		if (!backgroundWorker1.IsBusy)
		{
			解析图片.Enabled = false;
			backgroundWorker1.RunWorkerAsync();
		}
	}

	private void button6_Click(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Multiselect = true;
		openFileDialog.Title = "请选择文件";
		openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All TAG(*.TIF;*.TIFF)|*.TIF;*.TIFF";
		openFileDialog.InitialDirectory = Application.StartupPath;
		OpenFileDialog openFileDialog2 = openFileDialog;
		if (openFileDialog2.ShowDialog() == DialogResult.OK)
		{
			string fileName = openFileDialog2.FileName;
			图片文件BOX.Text = fileName;
		}
	}

	private void comboBox_1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (小说系统名.Text.ToLower() == "jieqi".ToLower())
		{
			小说系统版本号.Items.Clear();
			小说系统版本号.Items.Add("1.3");
			小说系统版本号.Items.Add("1.4");
			小说系统版本号.Items.Add("1.5");
			小说系统版本号.Items.Add("1.6");
			小说系统版本号.Items.Add("1.7");
			小说系统版本号.Items.Add("1.8");
			小说系统版本号.Items.Add("2.1");
			小说系统版本号.Items.Add("2.2");
			小说系统版本号.Items.Add("2.3");
			小说系统版本号.Items.Add("2.4");
			小说系统版本号.Items.Add("3.0");
			小说系统版本号.Items.Add("3.1");
		}
	}

	private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (e.CloseReason.ToString() == "MdiFormClosing")
		{
			Application.Exit();
		}
		else
		{
			Hide();
		}
		e.Cancel = true;
	}

	private void ConfigForm_Load(object sender, EventArgs e)
	{
		取消配置.DialogResult = DialogResult.Cancel;
		if (Configs.CmsName.ToLower() == "Jieqi".ToLower())
		{
			小说系统版本号.Items.Clear();
			小说系统版本号.Items.Add("1.3");
			小说系统版本号.Items.Add("1.4");
			小说系统版本号.Items.Add("1.5");
			小说系统版本号.Items.Add("1.6");
			小说系统版本号.Items.Add("1.7");
			小说系统版本号.Items.Add("1.8");
			小说系统版本号.Items.Add("2.1");
			小说系统版本号.Items.Add("2.2");
			小说系统版本号.Items.Add("2.3");
			小说系统版本号.Items.Add("2.4");
			小说系统版本号.Items.Add("3.0");
			小说系统版本号.Items.Add("3.1");
		}
		initFormData();
		if (!Configs.BaseConfig.LicenseVip)
		{
			SEO优化.Enabled = false;
			图版转文字设置.Enabled = false;
			防采集及站群设置.Enabled = false;
		}
		if (Configs.HaveFunction.IndexOf("ZhanQun") < 0)
		{
			是否开启站群.Enabled = false;
			自动配置站群.Enabled = false;
		}
		中译英.Checked = false;
		中译英.Enabled = true;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && icontainer_0 != null)
		{
			icontainer_0.Dispose();
		}
		base.Dispose(disposing);
	}

	private void initFormData()
	{
		Configs.BaseConfig.EnsureDefaults();
		网站名称TEXT.Text = Configs.BaseConfig.WebSiteName;
		Debug模式.Checked = Configs.BaseConfig.Debug;
		网站硬盘根目录.Text = Configs.BaseConfig.WebSitePath;
		数据库配置地址.Text = Configs.BaseConfig.ConnectionString;
		小说系统版本号.Text = Configs.BaseConfig.CmsVersion;
		小说系统名.Text = SupportedCms.NormalizeCmsName(Configs.BaseConfig.CmsName);
		日志保存周期BOX.Text = Configs.BaseConfig.sqliteTime;
		空章节替换内容Box.Text = Configs.BaseConfig.NullChapter;
		是否启用空章节替换.Checked = Configs.BaseConfig.OpenNullChapter;
		是否启用图转文.Checked = Configs.BaseConfig.OpenImageChapter;
		是否使用默认男女频.Checked = Configs.BaseConfig.DonotUserDefaultisboy;
		默认男女频text.Text = Configs.BaseConfig.DefaultisboyID + "|" + Configs.BaseConfig.Defaultisboy;
		默认男女频对应列表.Text = (Configs.BaseConfig.isboyCorresponding ?? string.Empty).Replace("♂", "\r\n");
		是否使用默认大类.Checked = Configs.BaseConfig.DonotUserDefaultLagerSort;
		是否使用默认小类.Checked = Configs.BaseConfig.DonotUserDefaultSmallSort;
		默认大类Box.Text = Configs.BaseConfig.DefaultLagerSortID + "|" + Configs.BaseConfig.DefaultLagerSort;
		默认小类BOX.Text = Configs.BaseConfig.DefaultSmallSortID + "|" + Configs.BaseConfig.DefaultSmallSort;
		大类对应BOX.Text = (Configs.BaseConfig.LagerSortCorresponding ?? string.Empty).Replace("♂", "\r\n");
		小类对应BOX.Text = (Configs.BaseConfig.SmallSortCorresponding ?? string.Empty).Replace("♂", "\r\n");
		默认分卷名字.Text = Configs.BaseConfig.DefaultVolumeName;
		SetNumericValue(http超时, Configs.BaseConfig.HttpTimeOut);
		UA.Text = Configs.BaseConfig.HttpUserAgent;
		cookies.Text = Configs.BaseConfig.WebSiteCookies;
		SetComboBoxIndex(日志格式BOX, Configs.BaseConfig.LogType);
		中译英.Checked = Configs.BaseConfig.Translate;
		小说系统名.Text = SupportedCms.NormalizeCmsName(Configs.CmsName);
		是否生成目录.Checked = Configs.BaseConfig.IndexHtml;
		是否生成内容.Checked = Configs.BaseConfig.ChapterHtml;
		是否否生成OPF.Checked = Configs.BaseConfig.CreateOPF;
		是否生成全文.Checked = Configs.BaseConfig.FullHtml;
		是否启用章节实际内容.Checked = Configs.BaseConfig.CustomRealTxt;
		章节图片位置.Checked = Configs.BaseConfig.CustomImage;
		小说封面位置.Checked = Configs.BaseConfig.CustomCover;
		是否启用JS调用真实内容.Checked = Configs.BaseConfig.AddJsRealTxt;
		是否启用自定义生成路径.Checked = Configs.BaseConfig.CustomCreatePath;
		是否启用自定义HTML模板.Checked = Configs.BaseConfig.CustomHtmlTemplets;
		星否生成ZIP.Checked = Configs.BaseConfig.CreateZIP;
		是否生成TXT.Checked = Configs.BaseConfig.CreateTXT;
		是否生成UMD.Checked = Configs.BaseConfig.CreateUMD;
		是否生成JAR.Checked = Configs.BaseConfig.CreateJAR;
		是否生成CHM.Checked = Configs.BaseConfig.CreateCHM;
		章节目录硬盘路径.Text = Configs.BaseConfig.IndexHtmlDir;
		章节目录URL路径.Text = Configs.BaseConfig.IndexHtmlUrl;
		内容硬盘路径.Text = Configs.BaseConfig.ChapterHtmlDir;
		内容URL路径.Text = Configs.BaseConfig.ChapterHtmlUrl;
		全文硬盘路径.Text = Configs.BaseConfig.FullHtmlDir;
		全文URL路径.Text = Configs.BaseConfig.FullHtmlUrl;
		实际章节硬盘路径.Text = Configs.BaseConfig.RealTxtDir;
		实际章节内容URL路径.Text = Configs.BaseConfig.RealTxtUrl;
		OPF硬盘路径.Text = Configs.BaseConfig.OpfDir;
		OPFURL路径.Text = Configs.BaseConfig.OpfUrl;
		章节图片硬盘路径.Text = Configs.BaseConfig.ImageDir;
		图片章节URL路径.Text = Configs.BaseConfig.ImageUrl;
		小说封面硬盘路径.Text = Configs.BaseConfig.CoverDir;
		封面URL路径.Text = Configs.BaseConfig.CoverUrl;
		首页的上一页Box.Text = Configs.BaseConfig.PrevFirstHtmlUrl;
		尾页的下一页Box.Text = Configs.BaseConfig.NextEndHtmlUrl;
		ZIP根目录.Text = Configs.BaseConfig.ZipDir;
		ZIP_URL.Text = Configs.BaseConfig.ZipUrl;
		UMD根目录.Text = Configs.BaseConfig.UmdDir;
		UMD_URL.Text = Configs.BaseConfig.UmdUrl;
		JAR根目录.Text = Configs.BaseConfig.JarDir;
		JAR_URL.Text = Configs.BaseConfig.JarUrl;
		CHM根目录.Text = Configs.BaseConfig.ChmDir;
		CHM_URL.Text = Configs.BaseConfig.ChmUrl;
		TXT根目录.Text = Configs.BaseConfig.TxtDir;
		TXT_URL.Text = Configs.BaseConfig.TxtUrl;
		if (Configs.BaseConfig.TextMarkOfVulmeName != null)
		{
			添加文字广告分卷限制.Text = string.Join("\r\n", Configs.BaseConfig.TextMarkOfVulmeName);
		}
		if (Configs.BaseConfig.TextMarkOfArrayText != null)
		{
			文字广告集合Box.Text = string.Join("\r\n", Configs.BaseConfig.TextMarkOfArrayText);
		}
		if (Configs.BaseConfig.UpdateDefaultUrls != null)
		{
			单次循环后调用页面列表.Text = string.Join("\r\n", Configs.BaseConfig.UpdateDefaultUrls);
		}
		SetNumericValue(添加文字广告个数, Configs.BaseConfig.TextMarkOfNumber);
		章节头部广告Box.Text = Configs.BaseConfig.TextMarkOfTop;
		章节尾部广告Box.Text = Configs.BaseConfig.TextMarkOfBottom;
		是否添加文字广告.Checked = Configs.BaseConfig.TextMarkOfData;
		是否生成HTML添加文字广告.Checked = Configs.BaseConfig.TextMarkOfHtml;
		是否电子书添加文字广告.Checked = Configs.BaseConfig.TextMarkOfEBook;
		if (Configs.BaseConfig.FilterNovelName != null)
		{
			违禁小说过滤Box.Text = Configs.BaseConfig.FilterNovelName.Replace("♂", "\r\n").Replace(" ", "");
		}
		章节内容非法字符过滤Box.Text = Configs.BaseConfig.BadWords;
		默认替换字符Box.Text = Configs.BaseConfig.ReplaceBadWords;
		if (Configs.BaseConfig.BadwordsReplaceImages != null)
		{
			章节内容非法字符替换Box.Text = string.Join("\r\n", Configs.BaseConfig.BadwordsReplaceImages);
		}
		电子书头广告Box.Text = Configs.BaseConfig.EBookHead;
		电子书尾广告Box.Text = Configs.BaseConfig.EBookFoot;
		for (int i = 0; i < string_0.Length; i++)
		{
			日志记录Box.Items.Add(string_0[i]);
			string value = "," + string_0[i].Split(' ')[0] + ",";
			if ((Configs.BaseConfig.SelectLog ?? string.Empty).IndexOf(value) >= 0)
			{
				日志记录Box.SetItemChecked(i, value: true);
			}
		}
		switch ((Configs.BaseConfig.NumOrPinyin ?? string.Empty).ToLower())
		{
		case "拼音目录":
			if (Configs.HaveFunction.IndexOf("PinyinDir") < 0)
			{
				拼音数字选择Box.Text = "数字ID目录";
				生成目录样式Box.Text = "拼音目录";
			}
			else
			{
				拼音数字选择Box.Text = Configs.BaseConfig.NumOrPinyin;
				生成目录样式Box.Text = (Configs.BaseConfig.NumOrPinyinDir ?? string.Empty).Replace("{NovelId/1000}/{NovelId}", "ID除1000/ID").Replace("{NovelId}", "ID").Replace("{Pinyin/3}/{Pinyin}", "拼音前三字母/拼音")
					.Replace("{Pinyin}", "拼音")
					.Replace("{Pinyinshouid}", "拼音首字母+书号");
			}
			break;
		case "数字id目录":
			拼音数字选择Box.Text = Configs.BaseConfig.NumOrPinyin;
			生成目录样式Box.Text = Configs.BaseConfig.NumOrPinyinDir;
			break;
		}
		内容上下页后缀Box.Text = Configs.BaseConfig.PrevNextPageSuffix;
		SetNumericValue(目录邻居数BOX, Configs.BaseConfig.IndexNeighbor);
		SetNumericValue(内容邻居数Box, Configs.BaseConfig.ChapterNeighbor);
		SetNumericValue(目录推荐数Box, Configs.BaseConfig.IndexTuijian);
		SetNumericValue(内容推荐数Box, Configs.BaseConfig.ChapterTuijian);
		是否启用章节名数字化.Checked = Configs.BaseConfig.ChapterName2Num;
		SetNumericValue(目录页防采集倒数Box, Configs.BaseConfig.IndexAntiCollectNum);
		是否开启站群.Checked = Configs.BaseConfig.ZhanQun;
		SetNumericValue(目录页最新章节数Box, Configs.BaseConfig.ChapterPagingNum);
		SetNumericValue(章节字数小于Box, Configs.BaseConfig.SizeNullChapter);
		排行榜地址Box.Text = Configs.BaseConfig.InternalLinkUrl;
		SetNumericValue(内链密度设置Box, Configs.BaseConfig.InternalLinkDensity);
		是否启用内链.Checked = Configs.BaseConfig.InternalLink;
		前推荐词Box.Text = Configs.BaseConfig.InternalLinkHead;
		后长尾词Box.Text = Configs.BaseConfig.InternalLinkFoot;
		Smtp服务器.Text = Configs.BaseConfig.MailSmtp;
		mail名称.Text = Configs.BaseConfig.MailUser;
		mail密码.Text = Configs.BaseConfig.MailPass;
		接报告邮箱.Text = Configs.BaseConfig.MailTitle;
		SetNumericValue(发送间隔box, Configs.BaseConfig.MailTimeNum);
		推荐榜获取形式Box.Text = Configs.BaseConfig.TuijianType;
		内链接模版Box.Text = Configs.BaseConfig.TuijianTemplates;
		SetNumericValue(目录页最新章节数Box, Configs.BaseConfig.NewAntiCollectNum);
		SetNumericValue(目录页模拟章节数Box, Configs.BaseConfig.OnAntiCollectNum);
		模拟章节目录Box.Text = Configs.BaseConfig.OnAntiCollectDir;
		是否启用标签内链.Checked = Configs.BaseConfig.InnerTagLink;
		标签内链地址Box.Text = Configs.BaseConfig.InnerTagLinkUrl1;
		是否启用百度推送.Checked = Configs.BaseConfig.IsEnableBaiduPush;
		PC域名Box.Text = Configs.BaseConfig.StrBaiduPushDomain;
		PCTokenBox.Text = Configs.BaseConfig.StrBaiduPushToken;
		推送URLBox.Text = Configs.BaseConfig.StrBaiduPushURL;
		推送类型Box.Text = Configs.BaseConfig.StrBaiduPushType;
		SetNumericValue(推送数量Num, Configs.BaseConfig.IntBaiduPushNum);
		是否生成WAP页面.Checked = Configs.BaseConfig.IsEnableWapGen;
		WAP域名Box.Text = Configs.BaseConfig.StrWapDomain;
		WAP目录模板Box.Text = Configs.BaseConfig.StrWapIndexTemplate;
		WAP内容模板Box.Text = Configs.BaseConfig.StrWapChapterTemplate;
		WAP根目录Box.Text = Configs.BaseConfig.StrWapHtmlDir;
		目录模板路径.Text = Configs.BaseConfig.Listtmp;
		章节模板路径.Text = Configs.BaseConfig.Contmp;
		信息页模版路径.Text = Configs.BaseConfig.Infotmp;
		首页模板路径.Text = Configs.BaseConfig.Indextmp;
		编码Box.Text = Configs.BaseConfig.CmsEncoding;
	}

	private static void SetNumericValue(NumericUpDown numericUpDown, int value)
	{
		decimal safeValue = value;
		if (safeValue < numericUpDown.Minimum)
		{
			safeValue = numericUpDown.Minimum;
		}
		if (safeValue > numericUpDown.Maximum)
		{
			safeValue = numericUpDown.Maximum;
		}
		numericUpDown.Value = safeValue;
	}

	private static void SetComboBoxIndex(ComboBox comboBox, int index)
	{
		if (comboBox.Items.Count == 0)
		{
			return;
		}
		if (index < 0)
		{
			index = 0;
		}
		if (index >= comboBox.Items.Count)
		{
			index = comboBox.Items.Count - 1;
		}
		comboBox.SelectedIndex = index;
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.ConfigForm));
		this.日志记录 = new System.Windows.Forms.TabControl();
		this.基本设置 = new System.Windows.Forms.TabPage();
		this.编码Box = new System.Windows.Forms.ComboBox();
		this.编码label = new System.Windows.Forms.Label();
		this.单次循环后调用页面列表 = new System.Windows.Forms.TextBox();
		this.单次循环后调用页面地址 = new System.Windows.Forms.Label();
		this.测试数据库 = new System.Windows.Forms.Button();
		this.测试发信 = new System.Windows.Forms.Button();
		this.EMail用户名 = new System.Windows.Forms.Label();
		this.mail名称 = new System.Windows.Forms.TextBox();
		this.Smtp服务器地址 = new System.Windows.Forms.Label();
		this.Smtp服务器 = new System.Windows.Forms.TextBox();
		this.发送间隔box = new System.Windows.Forms.NumericUpDown();
		this.发送间隔时间 = new System.Windows.Forms.Label();
		this.mail密码 = new System.Windows.Forms.TextBox();
		this.EMail密码 = new System.Windows.Forms.Label();
		this.日志保存周期BOX = new System.Windows.Forms.ComboBox();
		this.日志保存周期 = new System.Windows.Forms.Label();
		this.网站名称TEXT = new System.Windows.Forms.MaskedTextBox();
		this.中译英 = new System.Windows.Forms.CheckBox();
		this.UA = new System.Windows.Forms.ComboBox();
		this.UserAgent = new System.Windows.Forms.Label();
		this.日志记录模式 = new System.Windows.Forms.Label();
		this.日志格式BOX = new System.Windows.Forms.ComboBox();
		this.数据库需要重启采集器 = new System.Windows.Forms.Label();
		this.需要重启采集器 = new System.Windows.Forms.Label();
		this.获取COOKIES = new System.Windows.Forms.LinkLabel();
		this.接报告邮箱 = new System.Windows.Forms.TextBox();
		this.接收报告邮箱 = new System.Windows.Forms.Label();
		this.cookies设置 = new System.Windows.Forms.Label();
		this.cookies = new System.Windows.Forms.TextBox();
		this.http超时 = new System.Windows.Forms.NumericUpDown();
		this.HTTP超时设置 = new System.Windows.Forms.Label();
		this.MYSQL = new System.Windows.Forms.LinkLabel();
		this.MSSQL = new System.Windows.Forms.LinkLabel();
		this.Debug模式 = new System.Windows.Forms.CheckBox();
		this.载入设置 = new System.Windows.Forms.LinkLabel();
		this.查看配置 = new System.Windows.Forms.LinkLabel();
		this.选择目录 = new System.Windows.Forms.LinkLabel();
		this.默认分卷名字 = new System.Windows.Forms.TextBox();
		this.默认分卷名 = new System.Windows.Forms.Label();
		this.小说系统名 = new System.Windows.Forms.ComboBox();
		this.入库小说系统 = new System.Windows.Forms.Label();
		this.小说系统版本号 = new System.Windows.Forms.ComboBox();
		this.小说系统版本 = new System.Windows.Forms.Label();
		this.数据库连接字符串 = new System.Windows.Forms.Label();
		this.数据库配置地址 = new System.Windows.Forms.TextBox();
		this.网站名称 = new System.Windows.Forms.Label();
		this.本地网站目录 = new System.Windows.Forms.Label();
		this.网站硬盘根目录 = new System.Windows.Forms.TextBox();
		this.分类对应 = new System.Windows.Forms.TabPage();
		this.是否使用默认男女频 = new System.Windows.Forms.CheckBox();
		this.男女频对应 = new System.Windows.Forms.Label();
		this.默认男女频对应列表 = new System.Windows.Forms.TextBox();
		this.默认男女频text = new System.Windows.Forms.TextBox();
		this.默认男女频 = new System.Windows.Forms.Label();
		this.是否使用默认小类 = new System.Windows.Forms.CheckBox();
		this.是否使用默认大类 = new System.Windows.Forms.CheckBox();
		this.小类 = new System.Windows.Forms.Label();
		this.小类对应BOX = new System.Windows.Forms.TextBox();
		this.大类 = new System.Windows.Forms.Label();
		this.大类对应BOX = new System.Windows.Forms.TextBox();
		this.默认小类BOX = new System.Windows.Forms.TextBox();
		this.默认小类 = new System.Windows.Forms.Label();
		this.默认大类Box = new System.Windows.Forms.TextBox();
		this.label_3 = new System.Windows.Forms.Label();
		this.高级设置 = new System.Windows.Forms.TabPage();
		this.SEO优化 = new System.Windows.Forms.GroupBox();
		this.内链接模版Box = new System.Windows.Forms.TextBox();
		this.内链接模版 = new System.Windows.Forms.Label();
		this.推荐榜获取形式Box = new System.Windows.Forms.ComboBox();
		this.推荐榜获取形式 = new System.Windows.Forms.Label();
		this.内容推荐数Box = new System.Windows.Forms.NumericUpDown();
		this.内容推荐数 = new System.Windows.Forms.Label();
		this.目录推荐数Box = new System.Windows.Forms.NumericUpDown();
		this.目录推荐数 = new System.Windows.Forms.Label();
		this.内容上下页后缀Box = new System.Windows.Forms.ComboBox();
		this.内容邻居数Box = new System.Windows.Forms.NumericUpDown();
		this.目录邻居数BOX = new System.Windows.Forms.NumericUpDown();
		this.排行榜地址Box = new System.Windows.Forms.MaskedTextBox();
		this.目录邻居数 = new System.Windows.Forms.Label();
		this.排行榜地址 = new System.Windows.Forms.Label();
		this.内容上下页后缀 = new System.Windows.Forms.Label();
		this.内容邻居数 = new System.Windows.Forms.Label();
		this.防采集及站群设置 = new System.Windows.Forms.GroupBox();
		this.目录最新章标签 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.目录页最新章节数Box = new System.Windows.Forms.NumericUpDown();
		this.目录页最新个章节 = new System.Windows.Forms.Label();
		this.目录页最新章节数 = new System.Windows.Forms.Label();
		this.目录页模拟章节数Box = new System.Windows.Forms.NumericUpDown();
		this.目录页模拟章节数 = new System.Windows.Forms.Label();
		this.模拟章节目录 = new System.Windows.Forms.Label();
		this.模拟章节目录选择 = new System.Windows.Forms.LinkLabel();
		this.模拟章节目录Box = new System.Windows.Forms.TextBox();
		this.自动配置站群 = new System.Windows.Forms.Button();
		this.目录页防采集倒数Box = new System.Windows.Forms.NumericUpDown();
		this.是否开启站群 = new System.Windows.Forms.CheckBox();
		this.目录页防采集个章节 = new System.Windows.Forms.Label();
		this.目录页防采集倒数 = new System.Windows.Forms.Label();
		this.拼音数字选择 = new System.Windows.Forms.GroupBox();
		this.数字拼音选择后自动升级数据库 = new System.Windows.Forms.Label();
		this.拼音化已有小说 = new System.Windows.Forms.Button();
		this.将数据库已有小说进行拼音化 = new System.Windows.Forms.Label();
		this.选择生成目录的格式 = new System.Windows.Forms.Label();
		this.选择生成目录为拼音或ID = new System.Windows.Forms.Label();
		this.拼音数字选择label = new System.Windows.Forms.Label();
		this.生成目录样式Box = new System.Windows.Forms.ComboBox();
		this.拼音数字选择Box = new System.Windows.Forms.ComboBox();
		this.生成目录样式 = new System.Windows.Forms.Label();
		this.内链设置 = new System.Windows.Forms.GroupBox();
		this.是否启用内链 = new System.Windows.Forms.CheckBox();
		this.内链密度设置Box = new System.Windows.Forms.NumericUpDown();
		this.后长尾词Box = new System.Windows.Forms.MaskedTextBox();
		this.前推荐词Box = new System.Windows.Forms.MaskedTextBox();
		this.后长尾词 = new System.Windows.Forms.Label();
		this.是否启用章节名数字化 = new System.Windows.Forms.CheckBox();
		this.前推荐词 = new System.Windows.Forms.Label();
		this.内链密度设置 = new System.Windows.Forms.Label();
		this.超级功能 = new System.Windows.Forms.TabPage();
		this.WAP生成设置 = new System.Windows.Forms.GroupBox();
		this.WAP域名Box = new System.Windows.Forms.TextBox();
		this.WAP域名 = new System.Windows.Forms.Label();
		this.WAP内容模板Box = new System.Windows.Forms.TextBox();
		this.WAP内容模板 = new System.Windows.Forms.Label();
		this.WAP根目录Box = new System.Windows.Forms.TextBox();
		this.WAP目录模板Box = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.WAP目录模板 = new System.Windows.Forms.Label();
		this.是否生成WAP页面 = new System.Windows.Forms.CheckBox();
		this.推送设置 = new System.Windows.Forms.GroupBox();
		this.推送数量Num = new System.Windows.Forms.NumericUpDown();
		this.推送数量 = new System.Windows.Forms.Label();
		this.推送类型Box = new System.Windows.Forms.ComboBox();
		this.推送类型 = new System.Windows.Forms.Label();
		this.推送URLBox = new System.Windows.Forms.TextBox();
		this.推送URL = new System.Windows.Forms.Label();
		this.最新推送情况 = new System.Windows.Forms.Label();
		this.查看推送状态进度 = new System.Windows.Forms.ProgressBar();
		this.查看推送状态 = new System.Windows.Forms.Button();
		this.PCTokenBox = new System.Windows.Forms.TextBox();
		this.PCToken = new System.Windows.Forms.Label();
		this.是否启用百度推送 = new System.Windows.Forms.CheckBox();
		this.PC域名Box = new System.Windows.Forms.TextBox();
		this.PC域名 = new System.Windows.Forms.Label();
		this.标签内链设置 = new System.Windows.Forms.GroupBox();
		this.是否启用标签内链 = new System.Windows.Forms.CheckBox();
		this.初始化标签表 = new System.Windows.Forms.Button();
		this.标签内链地址Box = new System.Windows.Forms.TextBox();
		this.标签内链地址 = new System.Windows.Forms.Label();
		this.图转文设置 = new System.Windows.Forms.TabPage();
		this.图版转文字设置 = new System.Windows.Forms.GroupBox();
		this.选择图片 = new System.Windows.Forms.Button();
		this.是否启用图转文 = new System.Windows.Forms.CheckBox();
		this.图片文件BOX = new System.Windows.Forms.TextBox();
		this.图转文内容结果 = new System.Windows.Forms.TextBox();
		this.必要组件 = new System.Windows.Forms.Label();
		this.解析图片 = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.生成设置 = new System.Windows.Forms.TabPage();
		this.生成设置其它Box = new System.Windows.Forms.TextBox();
		this.尾页的下一页 = new System.Windows.Forms.Label();
		this.首页的上一页 = new System.Windows.Forms.Label();
		this.首页的上一页Box = new System.Windows.Forms.TextBox();
		this.尾页的下一页Box = new System.Windows.Forms.TextBox();
		this.封面URL路径 = new System.Windows.Forms.TextBox();
		this.图片章节URL路径 = new System.Windows.Forms.TextBox();
		this.实际章节内容URL路径 = new System.Windows.Forms.TextBox();
		this.是否启用JS调用真实内容 = new System.Windows.Forms.CheckBox();
		this.是否启用自定义生成路径 = new System.Windows.Forms.CheckBox();
		this.OPFURL路径 = new System.Windows.Forms.TextBox();
		this.OPF硬盘路径 = new System.Windows.Forms.TextBox();
		this.是否否生成OPF = new System.Windows.Forms.CheckBox();
		this.是否启用自定义HTML模板 = new System.Windows.Forms.CheckBox();
		this.小说封面硬盘路径 = new System.Windows.Forms.TextBox();
		this.小说封面位置 = new System.Windows.Forms.CheckBox();
		this.章节图片硬盘路径 = new System.Windows.Forms.TextBox();
		this.章节图片位置 = new System.Windows.Forms.CheckBox();
		this.实际章节硬盘路径 = new System.Windows.Forms.TextBox();
		this.是否启用章节实际内容 = new System.Windows.Forms.CheckBox();
		this.网站路径 = new System.Windows.Forms.Label();
		this.硬盘路径 = new System.Windows.Forms.Label();
		this.全文URL路径 = new System.Windows.Forms.TextBox();
		this.全文硬盘路径 = new System.Windows.Forms.TextBox();
		this.内容URL路径 = new System.Windows.Forms.TextBox();
		this.内容硬盘路径 = new System.Windows.Forms.TextBox();
		this.章节目录URL路径 = new System.Windows.Forms.TextBox();
		this.章节目录硬盘路径 = new System.Windows.Forms.TextBox();
		this.是否生成全文 = new System.Windows.Forms.CheckBox();
		this.是否生成内容 = new System.Windows.Forms.CheckBox();
		this.是否生成目录 = new System.Windows.Forms.CheckBox();
		this.附加设置 = new System.Windows.Forms.TabPage();
		this.空章节自定义 = new System.Windows.Forms.GroupBox();
		this.章节字数小于 = new System.Windows.Forms.Label();
		this.章节字数小于Box = new System.Windows.Forms.NumericUpDown();
		this.章节字数小于字数 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.是否启用空章节替换 = new System.Windows.Forms.CheckBox();
		this.空章节替换内容Box = new System.Windows.Forms.TextBox();
		this.空章节说明 = new System.Windows.Forms.Label();
		this.电子书设置 = new System.Windows.Forms.TabPage();
		this.电子书URL = new System.Windows.Forms.Label();
		this.电子书广告 = new System.Windows.Forms.GroupBox();
		this.电子书头广告Box = new System.Windows.Forms.TextBox();
		this.电子书尾广告Box = new System.Windows.Forms.TextBox();
		this.电子书尾广告 = new System.Windows.Forms.Label();
		this.电子书头广告 = new System.Windows.Forms.Label();
		this.电子书根目录 = new System.Windows.Forms.Label();
		this.CHM_URL = new System.Windows.Forms.TextBox();
		this.CHM根目录 = new System.Windows.Forms.TextBox();
		this.JAR_URL = new System.Windows.Forms.TextBox();
		this.JAR根目录 = new System.Windows.Forms.TextBox();
		this.UMD_URL = new System.Windows.Forms.TextBox();
		this.UMD根目录 = new System.Windows.Forms.TextBox();
		this.TXT_URL = new System.Windows.Forms.TextBox();
		this.TXT根目录 = new System.Windows.Forms.TextBox();
		this.ZIP_URL = new System.Windows.Forms.TextBox();
		this.ZIP根目录 = new System.Windows.Forms.TextBox();
		this.是否生成CHM = new System.Windows.Forms.CheckBox();
		this.是否生成JAR = new System.Windows.Forms.CheckBox();
		this.是否生成UMD = new System.Windows.Forms.CheckBox();
		this.是否生成TXT = new System.Windows.Forms.CheckBox();
		this.星否生成ZIP = new System.Windows.Forms.CheckBox();
		this.文字广告 = new System.Windows.Forms.TabPage();
		this.是否电子书添加文字广告 = new System.Windows.Forms.CheckBox();
		this.固定位置添加广告 = new System.Windows.Forms.GroupBox();
		this.章节尾部广告Box = new System.Windows.Forms.TextBox();
		this.章节尾部广告 = new System.Windows.Forms.Label();
		this.章节头部广告Box = new System.Windows.Forms.TextBox();
		this.章节头部广告 = new System.Windows.Forms.Label();
		this.是否生成HTML添加文字广告 = new System.Windows.Forms.CheckBox();
		this.是否添加文字广告 = new System.Windows.Forms.CheckBox();
		this.文字广告集合 = new System.Windows.Forms.GroupBox();
		this.文字广告集合Box = new System.Windows.Forms.TextBox();
		this.添加文字广告 = new System.Windows.Forms.GroupBox();
		this.留空表示不限制分卷 = new System.Windows.Forms.Label();
		this.添加文字广告个数 = new System.Windows.Forms.NumericUpDown();
		this.每个章节添加几个广告 = new System.Windows.Forms.Label();
		this.添加文字广告分卷限制 = new System.Windows.Forms.TextBox();
		this.限制只在以下分卷中添加文字广告 = new System.Windows.Forms.Label();
		this.过滤替换 = new System.Windows.Forms.TabPage();
		this.章节内容非法字符替换 = new System.Windows.Forms.GroupBox();
		this.章节内容非法字符替换Box = new System.Windows.Forms.TextBox();
		this.违禁小说过滤 = new System.Windows.Forms.GroupBox();
		this.违禁小说过滤Box = new System.Windows.Forms.TextBox();
		this.章节内容非法字符过滤 = new System.Windows.Forms.GroupBox();
		this.默认替换字符 = new System.Windows.Forms.Label();
		this.默认替换字符Box = new System.Windows.Forms.TextBox();
		this.章节内容非法字符过滤Box = new System.Windows.Forms.TextBox();
		this.日志选择 = new System.Windows.Forms.TabPage();
		this.选择需要记录的日志项 = new System.Windows.Forms.Label();
		this.日志记录Box = new System.Windows.Forms.CheckedListBox();
		this.杰奇目录DIY = new System.Windows.Forms.TabPage();
		this.模板路径为相对主目录路径 = new System.Windows.Forms.Label();
		this.杰奇DIY说明2 = new System.Windows.Forms.RichTextBox();
		this.杰奇DIY说明1 = new System.Windows.Forms.RichTextBox();
		this.信息页网站调用URL = new System.Windows.Forms.TextBox();
		this.OPF网站调用URL = new System.Windows.Forms.TextBox();
		this.TXT网站调用URL = new System.Windows.Forms.TextBox();
		this.章节内容网站调用URL = new System.Windows.Forms.TextBox();
		this.章节列表网站调用URL = new System.Windows.Forms.TextBox();
		this.OPF文件硬盘路径 = new System.Windows.Forms.Label();
		this.TXT文件硬盘路径 = new System.Windows.Forms.Label();
		this.章节硬盘路径 = new System.Windows.Forms.Label();
		this.章节列表硬盘路径 = new System.Windows.Forms.Label();
		this.文章信息硬盘路径 = new System.Windows.Forms.Label();
		this.章节模板路径 = new System.Windows.Forms.TextBox();
		this.章节页模板 = new System.Windows.Forms.Label();
		this.目录模板路径 = new System.Windows.Forms.TextBox();
		this.目录页模板 = new System.Windows.Forms.Label();
		this.信息页模版路径 = new System.Windows.Forms.TextBox();
		this.信息页模板 = new System.Windows.Forms.Label();
		this.首页模板路径 = new System.Windows.Forms.TextBox();
		this.首页模板 = new System.Windows.Forms.Label();
		this.OPF硬盘文件夹 = new System.Windows.Forms.TextBox();
		this.TXT硬盘文件夹 = new System.Windows.Forms.TextBox();
		this.章节HTML硬盘文件夹 = new System.Windows.Forms.TextBox();
		this.目录HTML硬盘文件夹 = new System.Windows.Forms.TextBox();
		this.信息页HTML硬盘文件夹 = new System.Windows.Forms.TextBox();
		this.OPF文件URL = new System.Windows.Forms.Label();
		this.TXT文件URL = new System.Windows.Forms.Label();
		this.章节内容页URL = new System.Windows.Forms.Label();
		this.章节列表页URL = new System.Windows.Forms.Label();
		this.文章信息页URL = new System.Windows.Forms.Label();
		this.textBox_4 = new System.Windows.Forms.TextBox();
		this.label_4 = new System.Windows.Forms.Label();
		this.textBox_5 = new System.Windows.Forms.TextBox();
		this.label_5 = new System.Windows.Forms.Label();
		this.保存 = new System.Windows.Forms.Button();
		this.取消配置 = new System.Windows.Forms.Button();
		this.label_15 = new System.Windows.Forms.Label();
		this.label_16 = new System.Windows.Forms.Label();
		this.label_17 = new System.Windows.Forms.Label();
		this.label_18 = new System.Windows.Forms.Label();
		this.fontDialog_0 = new System.Windows.Forms.FontDialog();
		this.openFileDialog_0 = new System.Windows.Forms.OpenFileDialog();
		this.toolTip_0 = new System.Windows.Forms.ToolTip(this.components);
		this.folderBrowserDialog_0 = new System.Windows.Forms.FolderBrowserDialog();
		this.基本配置小提示 = new System.Windows.Forms.Label();
		this.MailWorker = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.日志记录.SuspendLayout();
		this.基本设置.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.发送间隔box).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.http超时).BeginInit();
		this.分类对应.SuspendLayout();
		this.高级设置.SuspendLayout();
		this.SEO优化.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.内容推荐数Box).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.目录推荐数Box).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.内容邻居数Box).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.目录邻居数BOX).BeginInit();
		this.防采集及站群设置.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.目录页最新章节数Box).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.目录页模拟章节数Box).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.目录页防采集倒数Box).BeginInit();
		this.拼音数字选择.SuspendLayout();
		this.内链设置.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.内链密度设置Box).BeginInit();
		this.超级功能.SuspendLayout();
		this.WAP生成设置.SuspendLayout();
		this.推送设置.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.推送数量Num).BeginInit();
		this.标签内链设置.SuspendLayout();
		this.图转文设置.SuspendLayout();
		this.图版转文字设置.SuspendLayout();
		this.生成设置.SuspendLayout();
		this.附加设置.SuspendLayout();
		this.空章节自定义.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.章节字数小于Box).BeginInit();
		this.电子书设置.SuspendLayout();
		this.电子书广告.SuspendLayout();
		this.文字广告.SuspendLayout();
		this.固定位置添加广告.SuspendLayout();
		this.文字广告集合.SuspendLayout();
		this.添加文字广告.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.添加文字广告个数).BeginInit();
		this.过滤替换.SuspendLayout();
		this.章节内容非法字符替换.SuspendLayout();
		this.违禁小说过滤.SuspendLayout();
		this.章节内容非法字符过滤.SuspendLayout();
		this.日志选择.SuspendLayout();
		this.杰奇目录DIY.SuspendLayout();
		base.SuspendLayout();
		this.日志记录.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.日志记录.Controls.Add(this.基本设置);
		this.日志记录.Controls.Add(this.分类对应);
		this.日志记录.Controls.Add(this.高级设置);
		this.日志记录.Controls.Add(this.超级功能);
		this.日志记录.Controls.Add(this.图转文设置);
		this.日志记录.Controls.Add(this.生成设置);
		this.日志记录.Controls.Add(this.附加设置);
		this.日志记录.Controls.Add(this.电子书设置);
		this.日志记录.Controls.Add(this.文字广告);
		this.日志记录.Controls.Add(this.过滤替换);
		this.日志记录.Controls.Add(this.日志选择);
		this.日志记录.Controls.Add(this.杰奇目录DIY);
		this.日志记录.Location = new System.Drawing.Point(12, 12);
		this.日志记录.Name = "日志记录";
		this.日志记录.SelectedIndex = 0;
		this.日志记录.Size = new System.Drawing.Size(820, 404);
		this.日志记录.TabIndex = 0;
		this.基本设置.Controls.Add(this.编码Box);
		this.基本设置.Controls.Add(this.编码label);
		this.基本设置.Controls.Add(this.单次循环后调用页面列表);
		this.基本设置.Controls.Add(this.单次循环后调用页面地址);
		this.基本设置.Controls.Add(this.测试数据库);
		this.基本设置.Controls.Add(this.测试发信);
		this.基本设置.Controls.Add(this.EMail用户名);
		this.基本设置.Controls.Add(this.mail名称);
		this.基本设置.Controls.Add(this.Smtp服务器地址);
		this.基本设置.Controls.Add(this.Smtp服务器);
		this.基本设置.Controls.Add(this.发送间隔box);
		this.基本设置.Controls.Add(this.发送间隔时间);
		this.基本设置.Controls.Add(this.mail密码);
		this.基本设置.Controls.Add(this.EMail密码);
		this.基本设置.Controls.Add(this.日志保存周期BOX);
		this.基本设置.Controls.Add(this.日志保存周期);
		this.基本设置.Controls.Add(this.网站名称TEXT);
		this.基本设置.Controls.Add(this.中译英);
		this.基本设置.Controls.Add(this.UA);
		this.基本设置.Controls.Add(this.UserAgent);
		this.基本设置.Controls.Add(this.日志记录模式);
		this.基本设置.Controls.Add(this.日志格式BOX);
		this.基本设置.Controls.Add(this.数据库需要重启采集器);
		this.基本设置.Controls.Add(this.需要重启采集器);
		this.基本设置.Controls.Add(this.获取COOKIES);
		this.基本设置.Controls.Add(this.接报告邮箱);
		this.基本设置.Controls.Add(this.接收报告邮箱);
		this.基本设置.Controls.Add(this.cookies设置);
		this.基本设置.Controls.Add(this.cookies);
		this.基本设置.Controls.Add(this.http超时);
		this.基本设置.Controls.Add(this.HTTP超时设置);
		this.基本设置.Controls.Add(this.MYSQL);
		this.基本设置.Controls.Add(this.MSSQL);
		this.基本设置.Controls.Add(this.Debug模式);
		this.基本设置.Controls.Add(this.载入设置);
		this.基本设置.Controls.Add(this.查看配置);
		this.基本设置.Controls.Add(this.选择目录);
		this.基本设置.Controls.Add(this.默认分卷名字);
		this.基本设置.Controls.Add(this.默认分卷名);
		this.基本设置.Controls.Add(this.小说系统名);
		this.基本设置.Controls.Add(this.入库小说系统);
		this.基本设置.Controls.Add(this.小说系统版本号);
		this.基本设置.Controls.Add(this.小说系统版本);
		this.基本设置.Controls.Add(this.数据库连接字符串);
		this.基本设置.Controls.Add(this.数据库配置地址);
		this.基本设置.Controls.Add(this.网站名称);
		this.基本设置.Controls.Add(this.本地网站目录);
		this.基本设置.Controls.Add(this.网站硬盘根目录);
		this.基本设置.Location = new System.Drawing.Point(4, 22);
		this.基本设置.Name = "基本设置";
		this.基本设置.Padding = new System.Windows.Forms.Padding(3);
		this.基本设置.Size = new System.Drawing.Size(812, 378);
		this.基本设置.TabIndex = 0;
		this.基本设置.Text = "基本设置";
		this.基本设置.UseVisualStyleBackColor = true;
		this.编码Box.FormattingEnabled = true;
		this.编码Box.Items.AddRange(new object[3] { "跟随系统", "gbk", "utf-8" });
		this.编码Box.Location = new System.Drawing.Point(234, 99);
		this.编码Box.Name = "编码Box";
		this.编码Box.Size = new System.Drawing.Size(100, 20);
		this.编码Box.TabIndex = 62;
		this.toolTip_0.SetToolTip(this.编码Box, "必须准确选择编码，否则会引起乱码\r\n网站是UTF-8编码请选择：utf-8\r\nGBK请选择：“随系统”或“gbk”\r\n");
		this.编码label.AutoSize = true;
		this.编码label.ForeColor = System.Drawing.Color.Red;
		this.编码label.Location = new System.Drawing.Point(235, 84);
		this.编码label.Name = "编码label";
		this.编码label.Size = new System.Drawing.Size(53, 12);
		this.编码label.TabIndex = 61;
		this.编码label.Text = "网站编码";
		this.单次循环后调用页面列表.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.单次循环后调用页面列表.Location = new System.Drawing.Point(10, 297);
		this.单次循环后调用页面列表.Multiline = true;
		this.单次循环后调用页面列表.Name = "单次循环后调用页面列表";
		this.单次循环后调用页面列表.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.单次循环后调用页面列表.Size = new System.Drawing.Size(796, 76);
		this.单次循环后调用页面列表.TabIndex = 60;
		this.单次循环后调用页面地址.AutoSize = true;
		this.单次循环后调用页面地址.Location = new System.Drawing.Point(9, 282);
		this.单次循环后调用页面地址.Name = "单次循环后调用页面地址";
		this.单次循环后调用页面地址.Size = new System.Drawing.Size(329, 12);
		this.单次循环后调用页面地址.TabIndex = 59;
		this.单次循环后调用页面地址.Text = "单次循环后调用页面地址，一行一个(可用于更新网站地图等)";
		this.测试数据库.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.测试数据库.Location = new System.Drawing.Point(717, 58);
		this.测试数据库.Name = "测试数据库";
		this.测试数据库.Size = new System.Drawing.Size(89, 23);
		this.测试数据库.TabIndex = 58;
		this.测试数据库.Text = "测试数据库";
		this.测试数据库.UseVisualStyleBackColor = true;
		this.测试数据库.Click += new System.EventHandler(button4_Click);
		this.测试发信.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.测试发信.Location = new System.Drawing.Point(731, 255);
		this.测试发信.Name = "测试发信";
		this.测试发信.Size = new System.Drawing.Size(75, 23);
		this.测试发信.TabIndex = 57;
		this.测试发信.Text = "测试发信";
		this.测试发信.UseVisualStyleBackColor = true;
		this.测试发信.Click += new System.EventHandler(button3_Click);
		this.EMail用户名.AutoSize = true;
		this.EMail用户名.Location = new System.Drawing.Point(160, 201);
		this.EMail用户名.Name = "EMail用户名";
		this.EMail用户名.Size = new System.Drawing.Size(71, 12);
		this.EMail用户名.TabIndex = 56;
		this.EMail用户名.Text = "EMail用户名";
		this.mail名称.Location = new System.Drawing.Point(162, 216);
		this.mail名称.Name = "mail名称";
		this.mail名称.Size = new System.Drawing.Size(146, 21);
		this.mail名称.TabIndex = 55;
		this.Smtp服务器地址.AutoSize = true;
		this.Smtp服务器地址.Location = new System.Drawing.Point(7, 201);
		this.Smtp服务器地址.Name = "Smtp服务器地址";
		this.Smtp服务器地址.Size = new System.Drawing.Size(113, 12);
		this.Smtp服务器地址.TabIndex = 54;
		this.Smtp服务器地址.Text = "发送报告Smtp服务器";
		this.Smtp服务器.Location = new System.Drawing.Point(9, 216);
		this.Smtp服务器.Name = "Smtp服务器";
		this.Smtp服务器.Size = new System.Drawing.Size(145, 21);
		this.Smtp服务器.TabIndex = 53;
		this.发送间隔box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.发送间隔box.Location = new System.Drawing.Point(630, 256);
		this.发送间隔box.Maximum = new decimal(new int[4] { 3000, 0, 0, 0 });
		this.发送间隔box.Minimum = new decimal(new int[4] { 30, 0, 0, 0 });
		this.发送间隔box.Name = "发送间隔box";
		this.发送间隔box.Size = new System.Drawing.Size(94, 21);
		this.发送间隔box.TabIndex = 52;
		this.发送间隔box.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		this.发送间隔时间.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.发送间隔时间.AutoSize = true;
		this.发送间隔时间.Location = new System.Drawing.Point(630, 241);
		this.发送间隔时间.Name = "发送间隔时间";
		this.发送间隔时间.Size = new System.Drawing.Size(89, 12);
		this.发送间隔时间.TabIndex = 51;
		this.发送间隔时间.Text = "发送间隔(分钟)";
		this.mail密码.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.mail密码.Location = new System.Drawing.Point(318, 216);
		this.mail密码.Name = "mail密码";
		this.mail密码.Size = new System.Drawing.Size(305, 21);
		this.mail密码.TabIndex = 50;
		this.EMail密码.AutoSize = true;
		this.EMail密码.Location = new System.Drawing.Point(316, 201);
		this.EMail密码.Name = "EMail密码";
		this.EMail密码.Size = new System.Drawing.Size(143, 12);
		this.EMail密码.TabIndex = 49;
		this.EMail密码.Text = "EMail密码(不用邮件留空)";
		this.toolTip_0.SetToolTip(this.EMail密码, "当获取分卷名为空的时候替换此分卷名");
		this.日志保存周期BOX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.日志保存周期BOX.FormattingEnabled = true;
		this.日志保存周期BOX.Items.AddRange(new object[3] { "1", "7", "30" });
		this.日志保存周期BOX.Location = new System.Drawing.Point(629, 176);
		this.日志保存周期BOX.Name = "日志保存周期BOX";
		this.日志保存周期BOX.Size = new System.Drawing.Size(177, 20);
		this.日志保存周期BOX.TabIndex = 48;
		this.日志保存周期.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.日志保存周期.AutoSize = true;
		this.日志保存周期.Location = new System.Drawing.Point(627, 161);
		this.日志保存周期.Name = "日志保存周期";
		this.日志保存周期.Size = new System.Drawing.Size(77, 12);
		this.日志保存周期.TabIndex = 47;
		this.日志保存周期.Text = "日志保存周期";
		this.网站名称TEXT.Location = new System.Drawing.Point(8, 21);
		this.网站名称TEXT.Name = "网站名称TEXT";
		this.网站名称TEXT.Size = new System.Drawing.Size(113, 21);
		this.网站名称TEXT.TabIndex = 46;
		this.中译英.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.中译英.AutoSize = true;
		this.中译英.Location = new System.Drawing.Point(715, 100);
		this.中译英.Name = "中译英";
		this.中译英.Size = new System.Drawing.Size(60, 16);
		this.中译英.TabIndex = 45;
		this.中译英.Text = "中译英";
		this.中译英.UseVisualStyleBackColor = true;
		this.UA.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.UA.FormattingEnabled = true;
		this.UA.Items.AddRange(new object[11]
		{
			"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1)", "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)", "Mozilla/5.0 (compatible; Yahoo! Slurp; http://help.yahoo.com/help/us/ysearch/slurp)", "Mozilla/5.0 (Windows; U; Windows NT 5.1; sv-SE; rv:1.7.5) Gecko/20041108 Firefox/1.0", "Mozilla/5.0 (iPhone; U; CPU iPhone OS 3_0 like Mac OS X; en-us) AppleWebKit/528.18 (KHTML, like Gecko) Version/4.0 Mobile/7A341 Safari/528.16", "Mozilla/5.0 (Windows; U; Windows NT 5.1; de; rv:1.8.1.8) Gecko/20071008 SeaMonkey/1.0", "Mozilla/5.0 (Macintosh; U; PPC Mac OS X Mach-O; en-US; rv:1.7.2) Gecko/20040825 Camino/0.8.1", "msnbot/1.1 (+http://search.msn.com/msnbot.htm)",
			"Baiduspider+(+http://www.baidu.com/search/spider.htm)"
		});
		this.UA.Location = new System.Drawing.Point(8, 136);
		this.UA.Name = "UA";
		this.UA.Size = new System.Drawing.Size(615, 20);
		this.UA.TabIndex = 44;
		this.UserAgent.AutoSize = true;
		this.UserAgent.Location = new System.Drawing.Point(6, 121);
		this.UserAgent.Name = "UserAgent";
		this.UserAgent.Size = new System.Drawing.Size(89, 12);
		this.UserAgent.TabIndex = 43;
		this.UserAgent.Text = "Http UserAgent";
		this.日志记录模式.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.日志记录模式.AutoSize = true;
		this.日志记录模式.Location = new System.Drawing.Point(627, 199);
		this.日志记录模式.Name = "日志记录模式";
		this.日志记录模式.Size = new System.Drawing.Size(77, 12);
		this.日志记录模式.TabIndex = 42;
		this.日志记录模式.Text = "日志记录模式";
		this.日志格式BOX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.日志格式BOX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.日志格式BOX.FormattingEnabled = true;
		this.日志格式BOX.Items.AddRange(new object[2] { "Log文本模式", "SQLite模式" });
		this.日志格式BOX.Location = new System.Drawing.Point(629, 214);
		this.日志格式BOX.Name = "日志格式BOX";
		this.日志格式BOX.Size = new System.Drawing.Size(177, 20);
		this.日志格式BOX.TabIndex = 41;
		this.数据库需要重启采集器.AutoSize = true;
		this.数据库需要重启采集器.ForeColor = System.Drawing.Color.Red;
		this.数据库需要重启采集器.Location = new System.Drawing.Point(113, 45);
		this.数据库需要重启采集器.Name = "数据库需要重启采集器";
		this.数据库需要重启采集器.Size = new System.Drawing.Size(89, 12);
		this.数据库需要重启采集器.TabIndex = 36;
		this.数据库需要重启采集器.Text = "需要重启采集器";
		this.需要重启采集器.AutoSize = true;
		this.需要重启采集器.ForeColor = System.Drawing.Color.Red;
		this.需要重启采集器.Location = new System.Drawing.Point(208, 6);
		this.需要重启采集器.Name = "需要重启采集器";
		this.需要重启采集器.Size = new System.Drawing.Size(89, 12);
		this.需要重启采集器.TabIndex = 35;
		this.需要重启采集器.Text = "需要重启采集器";
		this.获取COOKIES.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.获取COOKIES.AutoSize = true;
		this.获取COOKIES.Location = new System.Drawing.Point(552, 177);
		this.获取COOKIES.Name = "获取COOKIES";
		this.获取COOKIES.Size = new System.Drawing.Size(71, 12);
		this.获取COOKIES.TabIndex = 34;
		this.获取COOKIES.TabStop = true;
		this.获取COOKIES.Text = "获取COOKIES";
		this.接报告邮箱.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.接报告邮箱.Location = new System.Drawing.Point(11, 255);
		this.接报告邮箱.Name = "接报告邮箱";
		this.接报告邮箱.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.接报告邮箱.Size = new System.Drawing.Size(612, 21);
		this.接报告邮箱.TabIndex = 33;
		this.接收报告邮箱.AutoSize = true;
		this.接收报告邮箱.Location = new System.Drawing.Point(7, 242);
		this.接收报告邮箱.Name = "接收报告邮箱";
		this.接收报告邮箱.Size = new System.Drawing.Size(461, 12);
		this.接收报告邮箱.TabIndex = 32;
		this.接收报告邮箱.Text = "接收报告的邮箱(多个用半角逗号分开，留空为不发送，接收邮箱不要与发信邮箱相同)";
		this.cookies设置.AutoSize = true;
		this.cookies设置.Location = new System.Drawing.Point(6, 159);
		this.cookies设置.Name = "cookies设置";
		this.cookies设置.Size = new System.Drawing.Size(329, 12);
		this.cookies设置.TabIndex = 31;
		this.cookies设置.Text = "本地网站后台COOKIES (调用后台功能时需要，一般不用设置)";
		this.cookies.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.cookies.Location = new System.Drawing.Point(9, 176);
		this.cookies.Name = "cookies";
		this.cookies.Size = new System.Drawing.Size(538, 21);
		this.cookies.TabIndex = 30;
		this.http超时.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.http超时.Location = new System.Drawing.Point(629, 137);
		this.http超时.Name = "http超时";
		this.http超时.Size = new System.Drawing.Size(177, 21);
		this.http超时.TabIndex = 29;
		this.HTTP超时设置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.HTTP超时设置.AutoSize = true;
		this.HTTP超时设置.Location = new System.Drawing.Point(629, 122);
		this.HTTP超时设置.Name = "HTTP超时设置";
		this.HTTP超时设置.Size = new System.Drawing.Size(77, 12);
		this.HTTP超时设置.TabIndex = 28;
		this.HTTP超时设置.Text = "HTTP超时(秒)";
		this.MYSQL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.MYSQL.AutoSize = true;
		this.MYSQL.Location = new System.Drawing.Point(674, 63);
		this.MYSQL.Name = "MYSQL";
		this.MYSQL.Size = new System.Drawing.Size(59, 12);
		this.MYSQL.TabIndex = 27;
		this.MYSQL.TabStop = true;
		this.MYSQL.Text = "MYSQL系列";
		this.MYSQL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel_9_LinkClicked);
		this.MSSQL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.MSSQL.AutoSize = true;
		this.MSSQL.Location = new System.Drawing.Point(631, 63);
		this.MSSQL.Name = "MSSQL";
		this.MSSQL.Size = new System.Drawing.Size(35, 12);
		this.MSSQL.TabIndex = 26;
		this.MSSQL.TabStop = true;
		this.MSSQL.Text = "MSSQL";
		this.MSSQL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel_8_LinkClicked);
		this.Debug模式.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.Debug模式.AutoSize = true;
		this.Debug模式.Location = new System.Drawing.Point(631, 101);
		this.Debug模式.Name = "Debug模式";
		this.Debug模式.Size = new System.Drawing.Size(78, 16);
		this.Debug模式.TabIndex = 25;
		this.Debug模式.Text = "Debug模式";
		this.Debug模式.UseVisualStyleBackColor = true;
		this.载入设置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.载入设置.AutoSize = true;
		this.载入设置.Enabled = false;
		this.载入设置.Location = new System.Drawing.Point(748, 24);
		this.载入设置.Name = "载入设置";
		this.载入设置.Size = new System.Drawing.Size(53, 12);
		this.载入设置.TabIndex = 24;
		this.载入设置.TabStop = true;
		this.载入设置.Text = "载入设置";
		this.查看配置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.查看配置.AutoSize = true;
		this.查看配置.Enabled = false;
		this.查看配置.Location = new System.Drawing.Point(689, 24);
		this.查看配置.Name = "查看配置";
		this.查看配置.Size = new System.Drawing.Size(53, 12);
		this.查看配置.TabIndex = 23;
		this.查看配置.TabStop = true;
		this.查看配置.Text = "查看配置";
		this.选择目录.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.选择目录.AutoSize = true;
		this.选择目录.Location = new System.Drawing.Point(629, 24);
		this.选择目录.Name = "选择目录";
		this.选择目录.Size = new System.Drawing.Size(53, 12);
		this.选择目录.TabIndex = 22;
		this.选择目录.TabStop = true;
		this.选择目录.Text = "选择目录";
		this.选择目录.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel_6_LinkClicked);
		this.默认分卷名字.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.默认分卷名字.Location = new System.Drawing.Point(351, 99);
		this.默认分卷名字.Name = "默认分卷名字";
		this.默认分卷名字.Size = new System.Drawing.Size(170, 21);
		this.默认分卷名字.TabIndex = 21;
		this.默认分卷名.AutoSize = true;
		this.默认分卷名.Location = new System.Drawing.Point(349, 84);
		this.默认分卷名.Name = "默认分卷名";
		this.默认分卷名.Size = new System.Drawing.Size(65, 12);
		this.默认分卷名.TabIndex = 20;
		this.默认分卷名.Text = "默认分卷名";
		this.toolTip_0.SetToolTip(this.默认分卷名, "当获取分卷名为空的时候替换此分卷名");
		this.小说系统名.FormattingEnabled = true;
		this.小说系统名.Items.AddRange(new object[1] { "Jieqi" });
		this.小说系统名.Location = new System.Drawing.Point(8, 99);
		this.小说系统名.Name = "小说系统名";
		this.小说系统名.Size = new System.Drawing.Size(100, 20);
		this.小说系统名.TabIndex = 19;
		this.小说系统名.SelectedIndexChanged += new System.EventHandler(comboBox_1_SelectedIndexChanged);
		this.入库小说系统.AutoSize = true;
		this.入库小说系统.Location = new System.Drawing.Point(6, 84);
		this.入库小说系统.Name = "入库小说系统";
		this.入库小说系统.Size = new System.Drawing.Size(77, 12);
		this.入库小说系统.TabIndex = 15;
		this.入库小说系统.Text = "入库小说系统";
		this.小说系统版本号.FormattingEnabled = true;
		this.小说系统版本号.Items.AddRange(new object[3] { "200806", "200809", "200812" });
		this.小说系统版本号.Location = new System.Drawing.Point(114, 99);
		this.小说系统版本号.Name = "小说系统版本号";
		this.小说系统版本号.Size = new System.Drawing.Size(100, 20);
		this.小说系统版本号.TabIndex = 14;
		this.小说系统版本.AutoSize = true;
		this.小说系统版本.Location = new System.Drawing.Point(112, 84);
		this.小说系统版本.Name = "小说系统版本";
		this.小说系统版本.Size = new System.Drawing.Size(77, 12);
		this.小说系统版本.TabIndex = 13;
		this.小说系统版本.Text = "小说系统版本";
		this.数据库连接字符串.AutoSize = true;
		this.数据库连接字符串.Location = new System.Drawing.Point(6, 45);
		this.数据库连接字符串.Name = "数据库连接字符串";
		this.数据库连接字符串.Size = new System.Drawing.Size(101, 12);
		this.数据库连接字符串.TabIndex = 11;
		this.数据库连接字符串.Text = "数据库连接字符串";
		this.数据库配置地址.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.数据库配置地址.Location = new System.Drawing.Point(8, 60);
		this.数据库配置地址.Name = "数据库配置地址";
		this.数据库配置地址.Size = new System.Drawing.Size(615, 21);
		this.数据库配置地址.TabIndex = 10;
		this.网站名称.AutoSize = true;
		this.网站名称.Location = new System.Drawing.Point(6, 6);
		this.网站名称.Name = "网站名称";
		this.网站名称.Size = new System.Drawing.Size(53, 12);
		this.网站名称.TabIndex = 3;
		this.网站名称.Text = "网站名称";
		this.本地网站目录.AutoSize = true;
		this.本地网站目录.Location = new System.Drawing.Point(125, 6);
		this.本地网站目录.Name = "本地网站目录";
		this.本地网站目录.Size = new System.Drawing.Size(77, 12);
		this.本地网站目录.TabIndex = 3;
		this.本地网站目录.Text = "本地网站目录";
		this.网站硬盘根目录.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.网站硬盘根目录.Location = new System.Drawing.Point(127, 21);
		this.网站硬盘根目录.Name = "网站硬盘根目录";
		this.网站硬盘根目录.Size = new System.Drawing.Size(496, 21);
		this.网站硬盘根目录.TabIndex = 2;
		this.分类对应.Controls.Add(this.是否使用默认男女频);
		this.分类对应.Controls.Add(this.男女频对应);
		this.分类对应.Controls.Add(this.默认男女频对应列表);
		this.分类对应.Controls.Add(this.默认男女频text);
		this.分类对应.Controls.Add(this.默认男女频);
		this.分类对应.Controls.Add(this.是否使用默认小类);
		this.分类对应.Controls.Add(this.是否使用默认大类);
		this.分类对应.Controls.Add(this.小类);
		this.分类对应.Controls.Add(this.小类对应BOX);
		this.分类对应.Controls.Add(this.大类);
		this.分类对应.Controls.Add(this.大类对应BOX);
		this.分类对应.Controls.Add(this.默认小类BOX);
		this.分类对应.Controls.Add(this.默认小类);
		this.分类对应.Controls.Add(this.默认大类Box);
		this.分类对应.Controls.Add(this.label_3);
		this.分类对应.Location = new System.Drawing.Point(4, 22);
		this.分类对应.Name = "分类对应";
		this.分类对应.Padding = new System.Windows.Forms.Padding(3);
		this.分类对应.Size = new System.Drawing.Size(812, 378);
		this.分类对应.TabIndex = 1;
		this.分类对应.Text = "分类对应";
		this.分类对应.UseVisualStyleBackColor = true;
		this.是否使用默认男女频.AutoSize = true;
		this.是否使用默认男女频.Location = new System.Drawing.Point(311, 7);
		this.是否使用默认男女频.Name = "是否使用默认男女频";
		this.是否使用默认男女频.Size = new System.Drawing.Size(120, 16);
		this.是否使用默认男女频.TabIndex = 15;
		this.是否使用默认男女频.Text = "不使用默认男女频";
		this.是否使用默认男女频.UseVisualStyleBackColor = true;
		this.男女频对应.AutoSize = true;
		this.男女频对应.Location = new System.Drawing.Point(7, 8);
		this.男女频对应.Name = "男女频对应";
		this.男女频对应.Size = new System.Drawing.Size(77, 12);
		this.男女频对应.TabIndex = 14;
		this.男女频对应.Text = "男女频对应：";
		this.默认男女频对应列表.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.默认男女频对应列表.Location = new System.Drawing.Point(7, 29);
		this.默认男女频对应列表.Multiline = true;
		this.默认男女频对应列表.Name = "默认男女频对应列表";
		this.默认男女频对应列表.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.默认男女频对应列表.Size = new System.Drawing.Size(799, 79);
		this.默认男女频对应列表.TabIndex = 13;
		this.默认男女频text.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.默认男女频text.Location = new System.Drawing.Point(656, 5);
		this.默认男女频text.Name = "默认男女频text";
		this.默认男女频text.Size = new System.Drawing.Size(150, 21);
		this.默认男女频text.TabIndex = 12;
		this.默认男女频.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.默认男女频.AutoSize = true;
		this.默认男女频.Location = new System.Drawing.Point(585, 8);
		this.默认男女频.Name = "默认男女频";
		this.默认男女频.Size = new System.Drawing.Size(77, 12);
		this.默认男女频.TabIndex = 11;
		this.默认男女频.Text = "默认男女频：";
		this.是否使用默认小类.AutoSize = true;
		this.是否使用默认小类.Location = new System.Drawing.Point(311, 253);
		this.是否使用默认小类.Name = "是否使用默认小类";
		this.是否使用默认小类.Size = new System.Drawing.Size(108, 16);
		this.是否使用默认小类.TabIndex = 10;
		this.是否使用默认小类.Text = "不使用默认小类";
		this.是否使用默认小类.UseVisualStyleBackColor = true;
		this.是否使用默认大类.AutoSize = true;
		this.是否使用默认大类.Location = new System.Drawing.Point(311, 116);
		this.是否使用默认大类.Name = "是否使用默认大类";
		this.是否使用默认大类.Size = new System.Drawing.Size(108, 16);
		this.是否使用默认大类.TabIndex = 9;
		this.是否使用默认大类.Text = "不使用默认大类";
		this.是否使用默认大类.UseVisualStyleBackColor = true;
		this.小类.AutoSize = true;
		this.小类.Location = new System.Drawing.Point(7, 254);
		this.小类.Name = "小类";
		this.小类.Size = new System.Drawing.Size(65, 12);
		this.小类.TabIndex = 8;
		this.小类.Text = "小类对应：";
		this.小类对应BOX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.小类对应BOX.Location = new System.Drawing.Point(7, 274);
		this.小类对应BOX.Multiline = true;
		this.小类对应BOX.Name = "小类对应BOX";
		this.小类对应BOX.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.小类对应BOX.Size = new System.Drawing.Size(799, 98);
		this.小类对应BOX.TabIndex = 7;
		this.大类.AutoSize = true;
		this.大类.Location = new System.Drawing.Point(7, 117);
		this.大类.Name = "大类";
		this.大类.Size = new System.Drawing.Size(65, 12);
		this.大类.TabIndex = 6;
		this.大类.Text = "大类对应：";
		this.大类对应BOX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.大类对应BOX.Location = new System.Drawing.Point(7, 137);
		this.大类对应BOX.Multiline = true;
		this.大类对应BOX.Name = "大类对应BOX";
		this.大类对应BOX.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.大类对应BOX.Size = new System.Drawing.Size(799, 108);
		this.大类对应BOX.TabIndex = 5;
		this.默认小类BOX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.默认小类BOX.Location = new System.Drawing.Point(656, 250);
		this.默认小类BOX.Name = "默认小类BOX";
		this.默认小类BOX.Size = new System.Drawing.Size(150, 21);
		this.默认小类BOX.TabIndex = 3;
		this.默认小类.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.默认小类.AutoSize = true;
		this.默认小类.Location = new System.Drawing.Point(585, 254);
		this.默认小类.Name = "默认小类";
		this.默认小类.Size = new System.Drawing.Size(65, 12);
		this.默认小类.TabIndex = 2;
		this.默认小类.Text = "默认小类：";
		this.默认大类Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.默认大类Box.Location = new System.Drawing.Point(656, 114);
		this.默认大类Box.Name = "默认大类Box";
		this.默认大类Box.Size = new System.Drawing.Size(150, 21);
		this.默认大类Box.TabIndex = 1;
		this.label_3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label_3.AutoSize = true;
		this.label_3.Location = new System.Drawing.Point(585, 118);
		this.label_3.Name = "label_3";
		this.label_3.Size = new System.Drawing.Size(65, 12);
		this.label_3.TabIndex = 0;
		this.label_3.Text = "默认大类：";
		this.高级设置.Controls.Add(this.SEO优化);
		this.高级设置.Controls.Add(this.防采集及站群设置);
		this.高级设置.Controls.Add(this.拼音数字选择);
		this.高级设置.Controls.Add(this.内链设置);
		this.高级设置.Location = new System.Drawing.Point(4, 22);
		this.高级设置.Name = "高级设置";
		this.高级设置.Size = new System.Drawing.Size(812, 378);
		this.高级设置.TabIndex = 10;
		this.高级设置.Text = "高级设置";
		this.高级设置.UseVisualStyleBackColor = true;
		this.SEO优化.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.SEO优化.Controls.Add(this.内链接模版Box);
		this.SEO优化.Controls.Add(this.内链接模版);
		this.SEO优化.Controls.Add(this.推荐榜获取形式Box);
		this.SEO优化.Controls.Add(this.推荐榜获取形式);
		this.SEO优化.Controls.Add(this.内容推荐数Box);
		this.SEO优化.Controls.Add(this.内容推荐数);
		this.SEO优化.Controls.Add(this.目录推荐数Box);
		this.SEO优化.Controls.Add(this.目录推荐数);
		this.SEO优化.Controls.Add(this.内容上下页后缀Box);
		this.SEO优化.Controls.Add(this.内容邻居数Box);
		this.SEO优化.Controls.Add(this.目录邻居数BOX);
		this.SEO优化.Controls.Add(this.排行榜地址Box);
		this.SEO优化.Controls.Add(this.目录邻居数);
		this.SEO优化.Controls.Add(this.排行榜地址);
		this.SEO优化.Controls.Add(this.内容上下页后缀);
		this.SEO优化.Controls.Add(this.内容邻居数);
		this.SEO优化.Location = new System.Drawing.Point(6, 79);
		this.SEO优化.Name = "SEO优化";
		this.SEO优化.Size = new System.Drawing.Size(800, 98);
		this.SEO优化.TabIndex = 69;
		this.SEO优化.TabStop = false;
		this.SEO优化.Text = "SEO优化（高级设置）";
		this.内链接模版Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.内链接模版Box.Location = new System.Drawing.Point(76, 67);
		this.内链接模版Box.Multiline = true;
		this.内链接模版Box.Name = "内链接模版Box";
		this.内链接模版Box.Size = new System.Drawing.Size(611, 24);
		this.内链接模版Box.TabIndex = 79;
		this.内链接模版Box.Text = "<li><a target=\"_blank\" href=\"{NovelUrl}\" title=\"{NovelTitle}\"><span>{NovelTitle}</span></a></li>";
		this.toolTip_0.SetToolTip(this.内链接模版Box, "模版设置说明：\r\n小说地址\t\t{NovelUrl}\r\n\r\n小说名称\t\t{NovelTitle}\r\n小说封面\t            {NovelPic}\t");
		this.内链接模版.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.内链接模版.AutoSize = true;
		this.内链接模版.Location = new System.Drawing.Point(9, 70);
		this.内链接模版.Name = "内链接模版";
		this.内链接模版.Size = new System.Drawing.Size(65, 12);
		this.内链接模版.TabIndex = 78;
		this.内链接模版.Text = "内链接模版";
		this.推荐榜获取形式Box.AutoCompleteCustomSource.AddRange(new string[3] { "默认", "/", "无需后缀" });
		this.推荐榜获取形式Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.推荐榜获取形式Box.FormattingEnabled = true;
		this.推荐榜获取形式Box.Items.AddRange(new object[8] { "后台推荐", "日点击榜", "周点击榜", "总点击榜", "日投票榜", "周投票榜", "月投票榜", "总投票榜" });
		this.推荐榜获取形式Box.Location = new System.Drawing.Point(581, 41);
		this.推荐榜获取形式Box.Name = "推荐榜获取形式Box";
		this.推荐榜获取形式Box.Size = new System.Drawing.Size(106, 20);
		this.推荐榜获取形式Box.TabIndex = 77;
		this.toolTip_0.SetToolTip(this.推荐榜获取形式Box, "提取推荐榜的形式，自行推荐可在后台推荐。");
		this.推荐榜获取形式.AutoSize = true;
		this.推荐榜获取形式.Location = new System.Drawing.Point(486, 44);
		this.推荐榜获取形式.Name = "推荐榜获取形式";
		this.推荐榜获取形式.Size = new System.Drawing.Size(89, 12);
		this.推荐榜获取形式.TabIndex = 76;
		this.推荐榜获取形式.Text = "推荐榜获取形式";
		this.内容推荐数Box.Location = new System.Drawing.Point(428, 38);
		this.内容推荐数Box.Name = "内容推荐数Box";
		this.内容推荐数Box.Size = new System.Drawing.Size(41, 21);
		this.内容推荐数Box.TabIndex = 75;
		this.toolTip_0.SetToolTip(this.内容推荐数Box, "0为不推荐，调用使用{?$tuijian?}");
		this.内容推荐数.AutoSize = true;
		this.内容推荐数.Location = new System.Drawing.Point(363, 44);
		this.内容推荐数.Name = "内容推荐数";
		this.内容推荐数.Size = new System.Drawing.Size(65, 12);
		this.内容推荐数.TabIndex = 74;
		this.内容推荐数.Text = "内容推荐数";
		this.目录推荐数Box.Location = new System.Drawing.Point(314, 38);
		this.目录推荐数Box.Name = "目录推荐数Box";
		this.目录推荐数Box.Size = new System.Drawing.Size(41, 21);
		this.目录推荐数Box.TabIndex = 73;
		this.toolTip_0.SetToolTip(this.目录推荐数Box, "0为不推荐，调用使用{?$tuijian?}");
		this.目录推荐数.AutoSize = true;
		this.目录推荐数.Location = new System.Drawing.Point(249, 44);
		this.目录推荐数.Name = "目录推荐数";
		this.目录推荐数.Size = new System.Drawing.Size(65, 12);
		this.目录推荐数.TabIndex = 72;
		this.目录推荐数.Text = "目录推荐数";
		this.内容上下页后缀Box.AutoCompleteCustomSource.AddRange(new string[3] { "默认", "/", "无需后缀" });
		this.内容上下页后缀Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.内容上下页后缀Box.FormattingEnabled = true;
		this.内容上下页后缀Box.Items.AddRange(new object[2] { "后台默认", "/" });
		this.内容上下页后缀Box.Location = new System.Drawing.Point(581, 14);
		this.内容上下页后缀Box.Name = "内容上下页后缀Box";
		this.内容上下页后缀Box.Size = new System.Drawing.Size(106, 20);
		this.内容上下页后缀Box.TabIndex = 71;
		this.toolTip_0.SetToolTip(this.内容上下页后缀Box, "内容页上一页下一页后缀调说明：\r\n后台默认    直接读取网站后台的设置\r\n/               章节ID后面带/\r\n不需要       直接为章节ID");
		this.内容邻居数Box.Location = new System.Drawing.Point(196, 38);
		this.内容邻居数Box.Name = "内容邻居数Box";
		this.内容邻居数Box.Size = new System.Drawing.Size(42, 21);
		this.内容邻居数Box.TabIndex = 70;
		this.toolTip_0.SetToolTip(this.内容邻居数Box, "0为不推荐，调用使用{?$linju?}");
		this.目录邻居数BOX.Location = new System.Drawing.Point(76, 38);
		this.目录邻居数BOX.Name = "目录邻居数BOX";
		this.目录邻居数BOX.Size = new System.Drawing.Size(40, 21);
		this.目录邻居数BOX.TabIndex = 70;
		this.toolTip_0.SetToolTip(this.目录邻居数BOX, "0为不推荐，调用使用{?$linju?}");
		this.排行榜地址Box.Location = new System.Drawing.Point(76, 13);
		this.排行榜地址Box.Name = "排行榜地址Box";
		this.排行榜地址Box.Size = new System.Drawing.Size(390, 21);
		this.排行榜地址Box.TabIndex = 1;
		this.toolTip_0.SetToolTip(this.排行榜地址Box, "内链地址格式设置说明：\r\n小说编号\t\t{NovelId}\r\n小说编号除以1000\t{NovelId/1000}\r\n小说名称拼音\t\t{Pinyin}\r\n小说名称拼音前三\t{Pinyin/3}\r\n地址格式如http://www.xxx.com/{NovelId/1000}/{NovelId}/");
		this.目录邻居数.AutoSize = true;
		this.目录邻居数.Location = new System.Drawing.Point(9, 44);
		this.目录邻居数.Name = "目录邻居数";
		this.目录邻居数.Size = new System.Drawing.Size(65, 12);
		this.目录邻居数.TabIndex = 55;
		this.目录邻居数.Text = "目录邻居数";
		this.排行榜地址.AutoSize = true;
		this.排行榜地址.Location = new System.Drawing.Point(9, 17);
		this.排行榜地址.Name = "排行榜地址";
		this.排行榜地址.Size = new System.Drawing.Size(65, 12);
		this.排行榜地址.TabIndex = 0;
		this.排行榜地址.Text = "排行榜地址";
		this.内容上下页后缀.AutoSize = true;
		this.内容上下页后缀.Location = new System.Drawing.Point(486, 17);
		this.内容上下页后缀.Name = "内容上下页后缀";
		this.内容上下页后缀.Size = new System.Drawing.Size(89, 12);
		this.内容上下页后缀.TabIndex = 57;
		this.内容上下页后缀.Text = "内容上下页后缀";
		this.内容邻居数.AutoSize = true;
		this.内容邻居数.Location = new System.Drawing.Point(129, 44);
		this.内容邻居数.Name = "内容邻居数";
		this.内容邻居数.Size = new System.Drawing.Size(65, 12);
		this.内容邻居数.TabIndex = 56;
		this.内容邻居数.Text = "内容邻居数";
		this.防采集及站群设置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.防采集及站群设置.Controls.Add(this.目录最新章标签);
		this.防采集及站群设置.Controls.Add(this.label36);
		this.防采集及站群设置.Controls.Add(this.目录页最新章节数Box);
		this.防采集及站群设置.Controls.Add(this.目录页最新个章节);
		this.防采集及站群设置.Controls.Add(this.目录页最新章节数);
		this.防采集及站群设置.Controls.Add(this.目录页模拟章节数Box);
		this.防采集及站群设置.Controls.Add(this.目录页模拟章节数);
		this.防采集及站群设置.Controls.Add(this.模拟章节目录);
		this.防采集及站群设置.Controls.Add(this.模拟章节目录选择);
		this.防采集及站群设置.Controls.Add(this.模拟章节目录Box);
		this.防采集及站群设置.Controls.Add(this.自动配置站群);
		this.防采集及站群设置.Controls.Add(this.目录页防采集倒数Box);
		this.防采集及站群设置.Controls.Add(this.是否开启站群);
		this.防采集及站群设置.Controls.Add(this.目录页防采集个章节);
		this.防采集及站群设置.Controls.Add(this.目录页防采集倒数);
		this.防采集及站群设置.Location = new System.Drawing.Point(6, 183);
		this.防采集及站群设置.Name = "防采集及站群设置";
		this.防采集及站群设置.Size = new System.Drawing.Size(800, 94);
		this.防采集及站群设置.TabIndex = 68;
		this.防采集及站群设置.TabStop = false;
		this.防采集及站群设置.Text = "防采集及站群模式设置（高级设置）";
		this.目录最新章标签.AutoSize = true;
		this.目录最新章标签.ForeColor = System.Drawing.Color.Blue;
		this.目录最新章标签.Location = new System.Drawing.Point(225, 42);
		this.目录最新章标签.Name = "目录最新章标签";
		this.目录最新章标签.Size = new System.Drawing.Size(329, 12);
		this.目录最新章标签.TabIndex = 80;
		this.目录最新章标签.Text = "PS:目录页最新章节不参与防采集，调用使用标签{?$new9?}！";
		this.label36.AutoSize = true;
		this.label36.Location = new System.Drawing.Point(169, 72);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(41, 12);
		this.label36.TabIndex = 79;
		this.label36.Text = "个章节";
		this.目录页最新章节数Box.Location = new System.Drawing.Point(112, 42);
		this.目录页最新章节数Box.Maximum = new decimal(new int[4] { 30, 0, 0, 0 });
		this.目录页最新章节数Box.Name = "目录页最新章节数Box";
		this.目录页最新章节数Box.Size = new System.Drawing.Size(51, 21);
		this.目录页最新章节数Box.TabIndex = 78;
		this.toolTip_0.SetToolTip(this.目录页最新章节数Box, "提取目录也最新真实章节数，最多9个章节，调用使用参数{?$new9?}");
		this.目录页最新个章节.AutoSize = true;
		this.目录页最新个章节.Location = new System.Drawing.Point(169, 45);
		this.目录页最新个章节.Name = "目录页最新个章节";
		this.目录页最新个章节.Size = new System.Drawing.Size(41, 12);
		this.目录页最新个章节.TabIndex = 77;
		this.目录页最新个章节.Text = "个章节";
		this.目录页最新章节数.AutoSize = true;
		this.目录页最新章节数.Location = new System.Drawing.Point(9, 44);
		this.目录页最新章节数.Name = "目录页最新章节数";
		this.目录页最新章节数.Size = new System.Drawing.Size(101, 12);
		this.目录页最新章节数.TabIndex = 76;
		this.目录页最新章节数.Text = "目录页最新章节数";
		this.目录页模拟章节数Box.Location = new System.Drawing.Point(112, 70);
		this.目录页模拟章节数Box.Name = "目录页模拟章节数Box";
		this.目录页模拟章节数Box.Size = new System.Drawing.Size(51, 21);
		this.目录页模拟章节数Box.TabIndex = 75;
		this.目录页模拟章节数.AutoSize = true;
		this.目录页模拟章节数.Location = new System.Drawing.Point(9, 72);
		this.目录页模拟章节数.Name = "目录页模拟章节数";
		this.目录页模拟章节数.Size = new System.Drawing.Size(101, 12);
		this.目录页模拟章节数.TabIndex = 74;
		this.目录页模拟章节数.Text = "目录页模拟章节数";
		this.模拟章节目录.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.模拟章节目录.AutoSize = true;
		this.模拟章节目录.Location = new System.Drawing.Point(224, 72);
		this.模拟章节目录.Name = "模拟章节目录";
		this.模拟章节目录.Size = new System.Drawing.Size(77, 12);
		this.模拟章节目录.TabIndex = 73;
		this.模拟章节目录.Text = "模拟章节目录";
		this.模拟章节目录选择.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.模拟章节目录选择.AutoSize = true;
		this.模拟章节目录选择.Location = new System.Drawing.Point(590, 72);
		this.模拟章节目录选择.Name = "模拟章节目录选择";
		this.模拟章节目录选择.Size = new System.Drawing.Size(53, 12);
		this.模拟章节目录选择.TabIndex = 72;
		this.模拟章节目录选择.TabStop = true;
		this.模拟章节目录选择.Text = "选择目录";
		this.模拟章节目录选择.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.模拟章节目录Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.模拟章节目录Box.Location = new System.Drawing.Point(307, 69);
		this.模拟章节目录Box.Name = "模拟章节目录Box";
		this.模拟章节目录Box.Size = new System.Drawing.Size(274, 21);
		this.模拟章节目录Box.TabIndex = 71;
		this.toolTip_0.SetToolTip(this.模拟章节目录Box, "留空为程序根目录下 Txtdir");
		this.自动配置站群.Location = new System.Drawing.Point(424, 15);
		this.自动配置站群.Name = "自动配置站群";
		this.自动配置站群.Size = new System.Drawing.Size(97, 23);
		this.自动配置站群.TabIndex = 19;
		this.自动配置站群.Text = "自动配置站群系统";
		this.自动配置站群.UseVisualStyleBackColor = true;
		this.目录页防采集倒数Box.Location = new System.Drawing.Point(112, 18);
		this.目录页防采集倒数Box.Maximum = new decimal(new int[4] { 30, 0, 0, 0 });
		this.目录页防采集倒数Box.Name = "目录页防采集倒数Box";
		this.目录页防采集倒数Box.Size = new System.Drawing.Size(51, 21);
		this.目录页防采集倒数Box.TabIndex = 70;
		this.toolTip_0.SetToolTip(this.目录页防采集倒数Box, "些处设置章节列表页最后几个章节进行防采集\r\n如：设置2的话则倒数两个章节为防采集章节");
		this.是否开启站群.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.是否开启站群.AutoSize = true;
		this.是否开启站群.Location = new System.Drawing.Point(226, 20);
		this.是否开启站群.Name = "是否开启站群";
		this.是否开启站群.Size = new System.Drawing.Size(192, 16);
		this.是否开启站群.TabIndex = 64;
		this.是否开启站群.Text = "开启站群模式（只对新站使用）";
		this.是否开启站群.UseVisualStyleBackColor = true;
		this.目录页防采集个章节.AutoSize = true;
		this.目录页防采集个章节.Location = new System.Drawing.Point(169, 21);
		this.目录页防采集个章节.Name = "目录页防采集个章节";
		this.目录页防采集个章节.Size = new System.Drawing.Size(41, 12);
		this.目录页防采集个章节.TabIndex = 63;
		this.目录页防采集个章节.Text = "个章节";
		this.目录页防采集倒数.AutoSize = true;
		this.目录页防采集倒数.Location = new System.Drawing.Point(9, 21);
		this.目录页防采集倒数.Name = "目录页防采集倒数";
		this.目录页防采集倒数.Size = new System.Drawing.Size(101, 12);
		this.目录页防采集倒数.TabIndex = 54;
		this.目录页防采集倒数.Text = "目录页防采集倒数";
		this.拼音数字选择.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.拼音数字选择.Controls.Add(this.数字拼音选择后自动升级数据库);
		this.拼音数字选择.Controls.Add(this.拼音化已有小说);
		this.拼音数字选择.Controls.Add(this.将数据库已有小说进行拼音化);
		this.拼音数字选择.Controls.Add(this.选择生成目录的格式);
		this.拼音数字选择.Controls.Add(this.选择生成目录为拼音或ID);
		this.拼音数字选择.Controls.Add(this.拼音数字选择label);
		this.拼音数字选择.Controls.Add(this.生成目录样式Box);
		this.拼音数字选择.Controls.Add(this.拼音数字选择Box);
		this.拼音数字选择.Controls.Add(this.生成目录样式);
		this.拼音数字选择.Location = new System.Drawing.Point(6, 12);
		this.拼音数字选择.Name = "拼音数字选择";
		this.拼音数字选择.Size = new System.Drawing.Size(800, 63);
		this.拼音数字选择.TabIndex = 67;
		this.拼音数字选择.TabStop = false;
		this.拼音数字选择.Text = "拼音、数字选择（无需授权）";
		this.数字拼音选择后自动升级数据库.AutoSize = true;
		this.数字拼音选择后自动升级数据库.ForeColor = System.Drawing.Color.Blue;
		this.数字拼音选择后自动升级数据库.Location = new System.Drawing.Point(355, 43);
		this.数字拼音选择后自动升级数据库.Name = "数字拼音选择后自动升级数据库";
		this.数字拼音选择后自动升级数据库.Size = new System.Drawing.Size(203, 12);
		this.数字拼音选择后自动升级数据库.TabIndex = 18;
		this.数字拼音选择后自动升级数据库.Text = "PS:数字拼音选择后自动升级数据库！";
		this.拼音化已有小说.Location = new System.Drawing.Point(357, 14);
		this.拼音化已有小说.Name = "拼音化已有小说";
		this.拼音化已有小说.Size = new System.Drawing.Size(104, 23);
		this.拼音化已有小说.TabIndex = 17;
		this.拼音化已有小说.Text = "拼音化已有小说";
		this.拼音化已有小说.UseVisualStyleBackColor = true;
		this.拼音化已有小说.Click += new System.EventHandler(button1_Click);
		this.将数据库已有小说进行拼音化.AutoSize = true;
		this.将数据库已有小说进行拼音化.Location = new System.Drawing.Point(467, 20);
		this.将数据库已有小说进行拼音化.Name = "将数据库已有小说进行拼音化";
		this.将数据库已有小说进行拼音化.Size = new System.Drawing.Size(161, 12);
		this.将数据库已有小说进行拼音化.TabIndex = 16;
		this.将数据库已有小说进行拼音化.Text = "将数据库已有小说进行拼音化";
		this.选择生成目录的格式.AutoSize = true;
		this.选择生成目录的格式.Location = new System.Drawing.Point(199, 43);
		this.选择生成目录的格式.Name = "选择生成目录的格式";
		this.选择生成目录的格式.Size = new System.Drawing.Size(113, 12);
		this.选择生成目录的格式.TabIndex = 12;
		this.选择生成目录的格式.Text = "选择生成目录的格式";
		this.选择生成目录为拼音或ID.AutoSize = true;
		this.选择生成目录为拼音或ID.Location = new System.Drawing.Point(199, 20);
		this.选择生成目录为拼音或ID.Name = "选择生成目录为拼音或ID";
		this.选择生成目录为拼音或ID.Size = new System.Drawing.Size(137, 12);
		this.选择生成目录为拼音或ID.TabIndex = 11;
		this.选择生成目录为拼音或ID.Text = "选择生成目录为拼音或ID";
		this.拼音数字选择label.AutoSize = true;
		this.拼音数字选择label.Location = new System.Drawing.Point(6, 20);
		this.拼音数字选择label.Name = "拼音数字选择label";
		this.拼音数字选择label.Size = new System.Drawing.Size(89, 12);
		this.拼音数字选择label.TabIndex = 7;
		this.拼音数字选择label.Text = "数字、拼音选择";
		this.生成目录样式Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.生成目录样式Box.FormattingEnabled = true;
		this.生成目录样式Box.Items.AddRange(new object[2] { "ID除1000/ID", "ID" });
		this.生成目录样式Box.Location = new System.Drawing.Point(101, 40);
		this.生成目录样式Box.Name = "生成目录样式Box";
		this.生成目录样式Box.Size = new System.Drawing.Size(92, 20);
		this.生成目录样式Box.TabIndex = 10;
		this.拼音数字选择Box.AutoCompleteCustomSource.AddRange(new string[2] { "数字", "拼音" });
		this.拼音数字选择Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.拼音数字选择Box.FormattingEnabled = true;
		this.拼音数字选择Box.Items.AddRange(new object[2] { "数字ID目录", "拼音目录" });
		this.拼音数字选择Box.Location = new System.Drawing.Point(101, 17);
		this.拼音数字选择Box.Name = "拼音数字选择Box";
		this.拼音数字选择Box.Size = new System.Drawing.Size(92, 20);
		this.拼音数字选择Box.TabIndex = 9;
		this.拼音数字选择Box.SelectedIndexChanged += new System.EventHandler(NumOrPinyincomboBox_SelectedIndexChanged);
		this.生成目录样式.AutoSize = true;
		this.生成目录样式.Location = new System.Drawing.Point(6, 43);
		this.生成目录样式.Name = "生成目录样式";
		this.生成目录样式.Size = new System.Drawing.Size(89, 12);
		this.生成目录样式.TabIndex = 8;
		this.生成目录样式.Text = "生成目录样式：";
		this.内链设置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.内链设置.Controls.Add(this.是否启用内链);
		this.内链设置.Controls.Add(this.内链密度设置Box);
		this.内链设置.Controls.Add(this.后长尾词Box);
		this.内链设置.Controls.Add(this.前推荐词Box);
		this.内链设置.Controls.Add(this.后长尾词);
		this.内链设置.Controls.Add(this.是否启用章节名数字化);
		this.内链设置.Controls.Add(this.前推荐词);
		this.内链设置.Controls.Add(this.内链密度设置);
		this.内链设置.ForeColor = System.Drawing.Color.Red;
		this.内链设置.Location = new System.Drawing.Point(8, 280);
		this.内链设置.Name = "内链设置";
		this.内链设置.Size = new System.Drawing.Size(800, 95);
		this.内链设置.TabIndex = 53;
		this.内链设置.TabStop = false;
		this.内链设置.Text = "内链设置（生成HTML静态用）";
		this.是否启用内链.AutoSize = true;
		this.是否启用内链.Location = new System.Drawing.Point(12, 19);
		this.是否启用内链.Name = "是否启用内链";
		this.是否启用内链.Size = new System.Drawing.Size(72, 16);
		this.是否启用内链.TabIndex = 66;
		this.是否启用内链.Text = "启用内链";
		this.toolTip_0.SetToolTip(this.是否启用内链, "启用内链说明：\r\n这里设置的整个内链设置里的总开关\r\n如果关闭其他相关的内链设置将无效");
		this.是否启用内链.UseVisualStyleBackColor = true;
		this.内链密度设置Box.Location = new System.Drawing.Point(185, 15);
		this.内链密度设置Box.Maximum = new decimal(new int[4] { 30, 0, 0, 0 });
		this.内链密度设置Box.Name = "内链密度设置Box";
		this.内链密度设置Box.Size = new System.Drawing.Size(50, 21);
		this.内链密度设置Box.TabIndex = 70;
		this.toolTip_0.SetToolTip(this.内链密度设置Box, "内链密度设置说明：\r\n些处设置章节页内链的密度为多少字加入一个链接\r\n默认为1000字加一个链接");
		this.内链密度设置Box.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		this.后长尾词Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.后长尾词Box.Location = new System.Drawing.Point(85, 68);
		this.后长尾词Box.Name = "后长尾词Box";
		this.后长尾词Box.Size = new System.Drawing.Size(709, 21);
		this.后长尾词Box.TabIndex = 5;
		this.toolTip_0.SetToolTip(this.后长尾词Box, "长尾词设置说明：\r\n如HTML代码：\r\n其他书友正在看<a href=\"内链地址格式\">XXX最新章节</a>\r\n其中“最新章节”为长尾词\r\n多个推荐词中间可用半角“,”号分开\r\n[NoLink]不加链接的内容[/NoLink]\r\n");
		this.前推荐词Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.前推荐词Box.Location = new System.Drawing.Point(85, 41);
		this.前推荐词Box.Name = "前推荐词Box";
		this.前推荐词Box.Size = new System.Drawing.Size(709, 21);
		this.前推荐词Box.TabIndex = 4;
		this.toolTip_0.SetToolTip(this.前推荐词Box, "推荐词设置说明：\r\n如HTML代码：\r\n其他书友正在看<a href=\"内链地址格式\">XXX小说</a>\r\n其中“其他书友正在看”为推荐词\r\n多个推荐词中间可用半角“,”号分开");
		this.后长尾词.AutoSize = true;
		this.后长尾词.Location = new System.Drawing.Point(7, 72);
		this.后长尾词.Name = "后长尾词";
		this.后长尾词.Size = new System.Drawing.Size(77, 12);
		this.后长尾词.TabIndex = 3;
		this.后长尾词.Text = "长尾词（后）";
		this.是否启用章节名数字化.AutoSize = true;
		this.是否启用章节名数字化.Location = new System.Drawing.Point(261, 19);
		this.是否启用章节名数字化.Name = "是否启用章节名数字化";
		this.是否启用章节名数字化.Size = new System.Drawing.Size(120, 16);
		this.是否启用章节名数字化.TabIndex = 1;
		this.是否启用章节名数字化.Text = "启用章节名数字化";
		this.是否启用章节名数字化.UseVisualStyleBackColor = true;
		this.前推荐词.AutoSize = true;
		this.前推荐词.Location = new System.Drawing.Point(7, 46);
		this.前推荐词.Name = "前推荐词";
		this.前推荐词.Size = new System.Drawing.Size(77, 12);
		this.前推荐词.TabIndex = 2;
		this.前推荐词.Text = "推荐词（前）";
		this.内链密度设置.AutoSize = true;
		this.内链密度设置.Location = new System.Drawing.Point(102, 20);
		this.内链密度设置.Name = "内链密度设置";
		this.内链密度设置.Size = new System.Drawing.Size(77, 12);
		this.内链密度设置.TabIndex = 59;
		this.内链密度设置.Text = "内链密度设置";
		this.超级功能.Controls.Add(this.WAP生成设置);
		this.超级功能.Controls.Add(this.推送设置);
		this.超级功能.Controls.Add(this.标签内链设置);
		this.超级功能.Location = new System.Drawing.Point(4, 22);
		this.超级功能.Name = "超级功能";
		this.超级功能.Padding = new System.Windows.Forms.Padding(3);
		this.超级功能.Size = new System.Drawing.Size(812, 378);
		this.超级功能.TabIndex = 13;
		this.超级功能.Text = "超级功能";
		this.超级功能.UseVisualStyleBackColor = true;
		this.WAP生成设置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.WAP生成设置.Controls.Add(this.WAP域名Box);
		this.WAP生成设置.Controls.Add(this.WAP域名);
		this.WAP生成设置.Controls.Add(this.WAP内容模板Box);
		this.WAP生成设置.Controls.Add(this.WAP内容模板);
		this.WAP生成设置.Controls.Add(this.WAP根目录Box);
		this.WAP生成设置.Controls.Add(this.WAP目录模板Box);
		this.WAP生成设置.Controls.Add(this.label42);
		this.WAP生成设置.Controls.Add(this.WAP目录模板);
		this.WAP生成设置.Controls.Add(this.是否生成WAP页面);
		this.WAP生成设置.Location = new System.Drawing.Point(4, 222);
		this.WAP生成设置.Name = "WAP生成设置";
		this.WAP生成设置.Size = new System.Drawing.Size(802, 81);
		this.WAP生成设置.TabIndex = 8;
		this.WAP生成设置.TabStop = false;
		this.WAP生成设置.Text = "WAP生成设置";
		this.WAP域名Box.Location = new System.Drawing.Point(167, 16);
		this.WAP域名Box.Name = "WAP域名Box";
		this.WAP域名Box.Size = new System.Drawing.Size(233, 21);
		this.WAP域名Box.TabIndex = 8;
		this.WAP域名.AutoSize = true;
		this.WAP域名.Location = new System.Drawing.Point(109, 21);
		this.WAP域名.Name = "WAP域名";
		this.WAP域名.Size = new System.Drawing.Size(59, 12);
		this.WAP域名.TabIndex = 7;
		this.WAP域名.Text = "WAP域名：";
		this.WAP内容模板Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.WAP内容模板Box.Location = new System.Drawing.Point(499, 47);
		this.WAP内容模板Box.Name = "WAP内容模板Box";
		this.WAP内容模板Box.Size = new System.Drawing.Size(273, 21);
		this.WAP内容模板Box.TabIndex = 6;
		this.WAP内容模板.AutoSize = true;
		this.WAP内容模板.Location = new System.Drawing.Point(410, 50);
		this.WAP内容模板.Name = "WAP内容模板";
		this.WAP内容模板.Size = new System.Drawing.Size(83, 12);
		this.WAP内容模板.TabIndex = 5;
		this.WAP内容模板.Text = "WAP内容模板：";
		this.WAP根目录Box.Location = new System.Drawing.Point(499, 16);
		this.WAP根目录Box.Name = "WAP根目录Box";
		this.WAP根目录Box.Size = new System.Drawing.Size(273, 21);
		this.WAP根目录Box.TabIndex = 4;
		this.WAP目录模板Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.WAP目录模板Box.Location = new System.Drawing.Point(165, 47);
		this.WAP目录模板Box.Name = "WAP目录模板Box";
		this.WAP目录模板Box.Size = new System.Drawing.Size(237, 21);
		this.WAP目录模板Box.TabIndex = 3;
		this.label42.AutoSize = true;
		this.label42.Location = new System.Drawing.Point(422, 22);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(71, 12);
		this.label42.TabIndex = 2;
		this.label42.Text = "WAP根目录：";
		this.WAP目录模板.AutoSize = true;
		this.WAP目录模板.Location = new System.Drawing.Point(85, 50);
		this.WAP目录模板.Name = "WAP目录模板";
		this.WAP目录模板.Size = new System.Drawing.Size(83, 12);
		this.WAP目录模板.TabIndex = 1;
		this.WAP目录模板.Text = "WAP目录模版：";
		this.是否生成WAP页面.AutoSize = true;
		this.是否生成WAP页面.Location = new System.Drawing.Point(7, 21);
		this.是否生成WAP页面.Name = "是否生成WAP页面";
		this.是否生成WAP页面.Size = new System.Drawing.Size(90, 16);
		this.是否生成WAP页面.TabIndex = 0;
		this.是否生成WAP页面.Text = "是否生成WAP";
		this.是否生成WAP页面.UseVisualStyleBackColor = true;
		this.推送设置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.推送设置.Controls.Add(this.推送数量Num);
		this.推送设置.Controls.Add(this.推送数量);
		this.推送设置.Controls.Add(this.推送类型Box);
		this.推送设置.Controls.Add(this.推送类型);
		this.推送设置.Controls.Add(this.推送URLBox);
		this.推送设置.Controls.Add(this.推送URL);
		this.推送设置.Controls.Add(this.最新推送情况);
		this.推送设置.Controls.Add(this.查看推送状态进度);
		this.推送设置.Controls.Add(this.查看推送状态);
		this.推送设置.Controls.Add(this.PCTokenBox);
		this.推送设置.Controls.Add(this.PCToken);
		this.推送设置.Controls.Add(this.是否启用百度推送);
		this.推送设置.Controls.Add(this.PC域名Box);
		this.推送设置.Controls.Add(this.PC域名);
		this.推送设置.Location = new System.Drawing.Point(6, 81);
		this.推送设置.Name = "推送设置";
		this.推送设置.Size = new System.Drawing.Size(800, 135);
		this.推送设置.TabIndex = 7;
		this.推送设置.TabStop = false;
		this.推送设置.Text = "推送设置";
		this.推送数量Num.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.推送数量Num.Location = new System.Drawing.Point(711, 56);
		this.推送数量Num.Name = "推送数量Num";
		this.推送数量Num.Size = new System.Drawing.Size(59, 21);
		this.推送数量Num.TabIndex = 13;
		this.推送数量.AutoSize = true;
		this.推送数量.Location = new System.Drawing.Point(652, 60);
		this.推送数量.Name = "推送数量";
		this.推送数量.Size = new System.Drawing.Size(53, 12);
		this.推送数量.TabIndex = 12;
		this.推送数量.Text = "推送数量";
		this.推送类型Box.FormattingEnabled = true;
		this.推送类型Box.Items.AddRange(new object[8] { "后台推荐", "日点击榜", "周点击榜", "总点击榜", "日投票榜", "周投票榜", "月投票榜", "总投票榜" });
		this.推送类型Box.Location = new System.Drawing.Point(462, 57);
		this.推送类型Box.Name = "推送类型Box";
		this.推送类型Box.Size = new System.Drawing.Size(177, 20);
		this.推送类型Box.TabIndex = 11;
		this.推送类型.AutoSize = true;
		this.推送类型.Location = new System.Drawing.Point(399, 60);
		this.推送类型.Name = "推送类型";
		this.推送类型.Size = new System.Drawing.Size(53, 12);
		this.推送类型.TabIndex = 10;
		this.推送类型.Text = "推送类型";
		this.推送URLBox.Location = new System.Drawing.Point(59, 57);
		this.推送URLBox.Name = "推送URLBox";
		this.推送URLBox.Size = new System.Drawing.Size(311, 21);
		this.推送URLBox.TabIndex = 9;
		this.推送URL.AutoSize = true;
		this.推送URL.Location = new System.Drawing.Point(6, 60);
		this.推送URL.Name = "推送URL";
		this.推送URL.Size = new System.Drawing.Size(47, 12);
		this.推送URL.TabIndex = 8;
		this.推送URL.Text = "推送URL";
		this.最新推送情况.AutoSize = true;
		this.最新推送情况.Location = new System.Drawing.Point(151, 86);
		this.最新推送情况.Name = "最新推送情况";
		this.最新推送情况.Size = new System.Drawing.Size(185, 12);
		this.最新推送情况.TabIndex = 7;
		this.最新推送情况.Text = "请点击左侧按钮查看最新推送情况";
		this.查看推送状态进度.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.查看推送状态进度.Location = new System.Drawing.Point(146, 104);
		this.查看推送状态进度.Name = "查看推送状态进度";
		this.查看推送状态进度.Size = new System.Drawing.Size(647, 23);
		this.查看推送状态进度.Step = 1;
		this.查看推送状态进度.TabIndex = 6;
		this.查看推送状态.Location = new System.Drawing.Point(6, 104);
		this.查看推送状态.Name = "查看推送状态";
		this.查看推送状态.Size = new System.Drawing.Size(120, 23);
		this.查看推送状态.TabIndex = 5;
		this.查看推送状态.Text = "查看推送状态";
		this.查看推送状态.UseVisualStyleBackColor = true;
		this.查看推送状态.Click += new System.EventHandler(btnViewBaiduPush_Click);
		this.PCTokenBox.Location = new System.Drawing.Point(461, 20);
		this.PCTokenBox.Name = "PCTokenBox";
		this.PCTokenBox.Size = new System.Drawing.Size(177, 21);
		this.PCTokenBox.TabIndex = 4;
		this.PCToken.AutoSize = true;
		this.PCToken.Location = new System.Drawing.Point(404, 26);
		this.PCToken.Name = "PCToken";
		this.PCToken.Size = new System.Drawing.Size(59, 12);
		this.PCToken.TabIndex = 3;
		this.PCToken.Text = "PCToken：";
		this.是否启用百度推送.AutoSize = true;
		this.是否启用百度推送.Location = new System.Drawing.Point(6, 25);
		this.是否启用百度推送.Name = "是否启用百度推送";
		this.是否启用百度推送.Size = new System.Drawing.Size(120, 16);
		this.是否启用百度推送.TabIndex = 2;
		this.是否启用百度推送.Text = "是否启用百度推送";
		this.是否启用百度推送.UseVisualStyleBackColor = true;
		this.PC域名Box.Location = new System.Drawing.Point(195, 20);
		this.PC域名Box.Name = "PC域名Box";
		this.PC域名Box.Size = new System.Drawing.Size(179, 21);
		this.PC域名Box.TabIndex = 1;
		this.PC域名.AutoSize = true;
		this.PC域名.Location = new System.Drawing.Point(144, 26);
		this.PC域名.Name = "PC域名";
		this.PC域名.Size = new System.Drawing.Size(53, 12);
		this.PC域名.TabIndex = 0;
		this.PC域名.Text = "PC域名：";
		this.标签内链设置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.标签内链设置.Controls.Add(this.是否启用标签内链);
		this.标签内链设置.Controls.Add(this.初始化标签表);
		this.标签内链设置.Controls.Add(this.标签内链地址Box);
		this.标签内链设置.Controls.Add(this.标签内链地址);
		this.标签内链设置.ForeColor = System.Drawing.Color.Red;
		this.标签内链设置.Location = new System.Drawing.Point(6, 6);
		this.标签内链设置.Name = "标签内链设置";
		this.标签内链设置.Size = new System.Drawing.Size(800, 69);
		this.标签内链设置.TabIndex = 6;
		this.标签内链设置.TabStop = false;
		this.标签内链设置.Text = "标签内链设置（生成HTML静态使用）";
		this.是否启用标签内链.AutoSize = true;
		this.是否启用标签内链.Location = new System.Drawing.Point(6, 23);
		this.是否启用标签内链.Name = "是否启用标签内链";
		this.是否启用标签内链.Size = new System.Drawing.Size(120, 16);
		this.是否启用标签内链.TabIndex = 1;
		this.是否启用标签内链.Text = "是否启用标签内链";
		this.是否启用标签内链.UseVisualStyleBackColor = true;
		this.初始化标签表.Location = new System.Drawing.Point(541, 17);
		this.初始化标签表.Name = "初始化标签表";
		this.初始化标签表.Size = new System.Drawing.Size(92, 23);
		this.初始化标签表.TabIndex = 5;
		this.初始化标签表.Text = "初始化标签表";
		this.初始化标签表.UseVisualStyleBackColor = true;
		this.初始化标签表.Click += new System.EventHandler(btnInnitTagTable_Click);
		this.标签内链地址Box.Location = new System.Drawing.Point(217, 18);
		this.标签内链地址Box.Name = "标签内链地址Box";
		this.标签内链地址Box.Size = new System.Drawing.Size(305, 21);
		this.标签内链地址Box.TabIndex = 4;
		this.toolTip_0.SetToolTip(this.标签内链地址Box, "本内链设置只设置标签内链\n请使用{Tag}标签来代替标签名\n如果要urlencode过的请使用{Tag|urlencode}");
		this.标签内链地址.Location = new System.Drawing.Point(145, 24);
		this.标签内链地址.Name = "标签内链地址";
		this.标签内链地址.Size = new System.Drawing.Size(53, 12);
		this.标签内链地址.TabIndex = 3;
		this.标签内链地址.Text = "内链地址";
		this.图转文设置.Controls.Add(this.图版转文字设置);
		this.图转文设置.Controls.Add(this.label27);
		this.图转文设置.Location = new System.Drawing.Point(4, 22);
		this.图转文设置.Name = "图转文设置";
		this.图转文设置.Padding = new System.Windows.Forms.Padding(3);
		this.图转文设置.Size = new System.Drawing.Size(812, 378);
		this.图转文设置.TabIndex = 12;
		this.图转文设置.Text = "图转文设置";
		this.图转文设置.UseVisualStyleBackColor = true;
		this.图版转文字设置.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.图版转文字设置.Controls.Add(this.选择图片);
		this.图版转文字设置.Controls.Add(this.是否启用图转文);
		this.图版转文字设置.Controls.Add(this.图片文件BOX);
		this.图版转文字设置.Controls.Add(this.图转文内容结果);
		this.图版转文字设置.Controls.Add(this.必要组件);
		this.图版转文字设置.Controls.Add(this.解析图片);
		this.图版转文字设置.Location = new System.Drawing.Point(6, 6);
		this.图版转文字设置.Name = "图版转文字设置";
		this.图版转文字设置.Size = new System.Drawing.Size(800, 366);
		this.图版转文字设置.TabIndex = 49;
		this.图版转文字设置.TabStop = false;
		this.图版转文字设置.Text = "图转文设置";
		this.选择图片.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.选择图片.Location = new System.Drawing.Point(624, 42);
		this.选择图片.Name = "选择图片";
		this.选择图片.Size = new System.Drawing.Size(75, 23);
		this.选择图片.TabIndex = 50;
		this.选择图片.Text = "选择图片";
		this.选择图片.UseVisualStyleBackColor = true;
		this.选择图片.Click += new System.EventHandler(button6_Click);
		this.是否启用图转文.AutoSize = true;
		this.是否启用图转文.Location = new System.Drawing.Point(8, 20);
		this.是否启用图转文.Name = "是否启用图转文";
		this.是否启用图转文.Size = new System.Drawing.Size(132, 16);
		this.是否启用图转文.TabIndex = 53;
		this.是否启用图转文.Text = "启用图转文超级模式";
		this.是否启用图转文.UseVisualStyleBackColor = true;
		this.图片文件BOX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.图片文件BOX.Location = new System.Drawing.Point(8, 44);
		this.图片文件BOX.Name = "图片文件BOX";
		this.图片文件BOX.Size = new System.Drawing.Size(610, 21);
		this.图片文件BOX.TabIndex = 49;
		this.图转文内容结果.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.图转文内容结果.Location = new System.Drawing.Point(8, 77);
		this.图转文内容结果.Multiline = true;
		this.图转文内容结果.Name = "图转文内容结果";
		this.图转文内容结果.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.图转文内容结果.Size = new System.Drawing.Size(785, 280);
		this.图转文内容结果.TabIndex = 52;
		this.toolTip_0.SetToolTip(this.图转文内容结果, "这里是图转文测试单元，测试该站点文本是否支持图转文解析。");
		this.必要组件.AutoSize = true;
		this.必要组件.ForeColor = System.Drawing.Color.RoyalBlue;
		this.必要组件.Location = new System.Drawing.Point(146, 21);
		this.必要组件.Name = "必要组件";
		this.必要组件.Size = new System.Drawing.Size(605, 12);
		this.必要组件.TabIndex = 54;
		this.必要组件.Text = "必要组件 Microsoft Office Document Imaging，若是Office2007则需要打sp1补丁。支持常用字体，转换率99%。";
		this.解析图片.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.解析图片.Location = new System.Drawing.Point(718, 42);
		this.解析图片.Name = "解析图片";
		this.解析图片.Size = new System.Drawing.Size(75, 23);
		this.解析图片.TabIndex = 51;
		this.解析图片.Text = "执行解析";
		this.解析图片.UseVisualStyleBackColor = true;
		this.解析图片.Click += new System.EventHandler(button5_Click);
		this.label27.AutoSize = true;
		this.label27.Location = new System.Drawing.Point(17, 50);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(53, 12);
		this.label27.TabIndex = 7;
		this.label27.Text = "选择图：";
		this.生成设置.Controls.Add(this.生成设置其它Box);
		this.生成设置.Controls.Add(this.尾页的下一页);
		this.生成设置.Controls.Add(this.首页的上一页);
		this.生成设置.Controls.Add(this.首页的上一页Box);
		this.生成设置.Controls.Add(this.尾页的下一页Box);
		this.生成设置.Controls.Add(this.封面URL路径);
		this.生成设置.Controls.Add(this.图片章节URL路径);
		this.生成设置.Controls.Add(this.实际章节内容URL路径);
		this.生成设置.Controls.Add(this.是否启用JS调用真实内容);
		this.生成设置.Controls.Add(this.是否启用自定义生成路径);
		this.生成设置.Controls.Add(this.OPFURL路径);
		this.生成设置.Controls.Add(this.OPF硬盘路径);
		this.生成设置.Controls.Add(this.是否否生成OPF);
		this.生成设置.Controls.Add(this.是否启用自定义HTML模板);
		this.生成设置.Controls.Add(this.小说封面硬盘路径);
		this.生成设置.Controls.Add(this.小说封面位置);
		this.生成设置.Controls.Add(this.章节图片硬盘路径);
		this.生成设置.Controls.Add(this.章节图片位置);
		this.生成设置.Controls.Add(this.实际章节硬盘路径);
		this.生成设置.Controls.Add(this.是否启用章节实际内容);
		this.生成设置.Controls.Add(this.网站路径);
		this.生成设置.Controls.Add(this.硬盘路径);
		this.生成设置.Controls.Add(this.全文URL路径);
		this.生成设置.Controls.Add(this.全文硬盘路径);
		this.生成设置.Controls.Add(this.内容URL路径);
		this.生成设置.Controls.Add(this.内容硬盘路径);
		this.生成设置.Controls.Add(this.章节目录URL路径);
		this.生成设置.Controls.Add(this.章节目录硬盘路径);
		this.生成设置.Controls.Add(this.是否生成全文);
		this.生成设置.Controls.Add(this.是否生成内容);
		this.生成设置.Controls.Add(this.是否生成目录);
		this.生成设置.Location = new System.Drawing.Point(4, 22);
		this.生成设置.Name = "生成设置";
		this.生成设置.Size = new System.Drawing.Size(812, 378);
		this.生成设置.TabIndex = 2;
		this.生成设置.Text = "生成设置";
		this.生成设置.UseVisualStyleBackColor = true;
		this.生成设置其它Box.Location = new System.Drawing.Point(147, 210);
		this.生成设置其它Box.Multiline = true;
		this.生成设置其它Box.Name = "生成设置其它Box";
		this.生成设置其它Box.ReadOnly = true;
		this.生成设置其它Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.生成设置其它Box.Size = new System.Drawing.Size(347, 75);
		this.生成设置其它Box.TabIndex = 55;
		this.尾页的下一页.AutoSize = true;
		this.尾页的下一页.Location = new System.Drawing.Point(500, 249);
		this.尾页的下一页.Name = "尾页的下一页";
		this.尾页的下一页.Size = new System.Drawing.Size(101, 12);
		this.尾页的下一页.TabIndex = 54;
		this.尾页的下一页.Text = "最后一页的下一页";
		this.首页的上一页.AutoSize = true;
		this.首页的上一页.Location = new System.Drawing.Point(498, 210);
		this.首页的上一页.Name = "首页的上一页";
		this.首页的上一页.Size = new System.Drawing.Size(89, 12);
		this.首页的上一页.TabIndex = 53;
		this.首页的上一页.Text = "第一页的上一页";
		this.首页的上一页Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.首页的上一页Box.Location = new System.Drawing.Point(500, 225);
		this.首页的上一页Box.Name = "首页的上一页Box";
		this.首页的上一页Box.Size = new System.Drawing.Size(306, 21);
		this.首页的上一页Box.TabIndex = 52;
		this.toolTip_0.SetToolTip(this.首页的上一页Box, "自定义第一页的上一页的链接地址,可使用{NovelId}");
		this.尾页的下一页Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.尾页的下一页Box.Location = new System.Drawing.Point(500, 264);
		this.尾页的下一页Box.Name = "尾页的下一页Box";
		this.尾页的下一页Box.Size = new System.Drawing.Size(306, 21);
		this.尾页的下一页Box.TabIndex = 51;
		this.toolTip_0.SetToolTip(this.尾页的下一页Box, "自定义最后一页的下一页的链接地址,可使用{NovelId}");
		this.封面URL路径.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.封面URL路径.Location = new System.Drawing.Point(396, 183);
		this.封面URL路径.Name = "封面URL路径";
		this.封面URL路径.Size = new System.Drawing.Size(410, 21);
		this.封面URL路径.TabIndex = 49;
		this.图片章节URL路径.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.图片章节URL路径.Location = new System.Drawing.Point(396, 156);
		this.图片章节URL路径.Name = "图片章节URL路径";
		this.图片章节URL路径.Size = new System.Drawing.Size(410, 21);
		this.图片章节URL路径.TabIndex = 48;
		this.实际章节内容URL路径.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.实际章节内容URL路径.Location = new System.Drawing.Point(396, 129);
		this.实际章节内容URL路径.Name = "实际章节内容URL路径";
		this.实际章节内容URL路径.Size = new System.Drawing.Size(410, 21);
		this.实际章节内容URL路径.TabIndex = 47;
		this.是否启用JS调用真实内容.AutoSize = true;
		this.是否启用JS调用真实内容.Location = new System.Drawing.Point(9, 212);
		this.是否启用JS调用真实内容.Name = "是否启用JS调用真实内容";
		this.是否启用JS调用真实内容.Size = new System.Drawing.Size(132, 16);
		this.是否启用JS调用真实内容.TabIndex = 46;
		this.是否启用JS调用真实内容.Text = "启用JS调用真实内容";
		this.是否启用JS调用真实内容.UseVisualStyleBackColor = true;
		this.是否启用自定义生成路径.AutoSize = true;
		this.是否启用自定义生成路径.Location = new System.Drawing.Point(9, 239);
		this.是否启用自定义生成路径.Name = "是否启用自定义生成路径";
		this.是否启用自定义生成路径.Size = new System.Drawing.Size(132, 16);
		this.是否启用自定义生成路径.TabIndex = 45;
		this.是否启用自定义生成路径.Text = "启用自定义生成路径";
		this.是否启用自定义生成路径.UseVisualStyleBackColor = true;
		this.OPFURL路径.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.OPFURL路径.Location = new System.Drawing.Point(396, 102);
		this.OPFURL路径.Name = "OPFURL路径";
		this.OPFURL路径.Size = new System.Drawing.Size(410, 21);
		this.OPFURL路径.TabIndex = 44;
		this.OPF硬盘路径.Location = new System.Drawing.Point(147, 102);
		this.OPF硬盘路径.Name = "OPF硬盘路径";
		this.OPF硬盘路径.Size = new System.Drawing.Size(243, 21);
		this.OPF硬盘路径.TabIndex = 43;
		this.是否否生成OPF.AutoSize = true;
		this.是否否生成OPF.Location = new System.Drawing.Point(9, 104);
		this.是否否生成OPF.Name = "是否否生成OPF";
		this.是否否生成OPF.Size = new System.Drawing.Size(114, 16);
		this.是否否生成OPF.TabIndex = 42;
		this.是否否生成OPF.Text = "生成索引文件OPF";
		this.是否否生成OPF.UseVisualStyleBackColor = true;
		this.是否启用自定义HTML模板.AutoSize = true;
		this.是否启用自定义HTML模板.Location = new System.Drawing.Point(9, 266);
		this.是否启用自定义HTML模板.Name = "是否启用自定义HTML模板";
		this.是否启用自定义HTML模板.Size = new System.Drawing.Size(132, 16);
		this.是否启用自定义HTML模板.TabIndex = 41;
		this.是否启用自定义HTML模板.Text = "启用自定义HTML模板";
		this.是否启用自定义HTML模板.UseVisualStyleBackColor = true;
		this.小说封面硬盘路径.Location = new System.Drawing.Point(147, 183);
		this.小说封面硬盘路径.Name = "小说封面硬盘路径";
		this.小说封面硬盘路径.Size = new System.Drawing.Size(243, 21);
		this.小说封面硬盘路径.TabIndex = 40;
		this.小说封面位置.AutoSize = true;
		this.小说封面位置.Enabled = false;
		this.小说封面位置.Location = new System.Drawing.Point(9, 185);
		this.小说封面位置.Name = "小说封面位置";
		this.小说封面位置.Size = new System.Drawing.Size(72, 16);
		this.小说封面位置.TabIndex = 39;
		this.小说封面位置.Text = "封面位置";
		this.toolTip_0.SetToolTip(this.小说封面位置, "小说封面储存路径。");
		this.小说封面位置.UseVisualStyleBackColor = true;
		this.章节图片硬盘路径.Location = new System.Drawing.Point(147, 156);
		this.章节图片硬盘路径.Name = "章节图片硬盘路径";
		this.章节图片硬盘路径.Size = new System.Drawing.Size(243, 21);
		this.章节图片硬盘路径.TabIndex = 38;
		this.章节图片位置.AutoSize = true;
		this.章节图片位置.Enabled = false;
		this.章节图片位置.Location = new System.Drawing.Point(9, 158);
		this.章节图片位置.Name = "章节图片位置";
		this.章节图片位置.Size = new System.Drawing.Size(72, 16);
		this.章节图片位置.TabIndex = 37;
		this.章节图片位置.Text = "图片位置";
		this.toolTip_0.SetToolTip(this.章节图片位置, "章节内容中用到的图片的储存位置。");
		this.章节图片位置.UseVisualStyleBackColor = true;
		this.实际章节硬盘路径.Location = new System.Drawing.Point(147, 129);
		this.实际章节硬盘路径.Name = "实际章节硬盘路径";
		this.实际章节硬盘路径.Size = new System.Drawing.Size(243, 21);
		this.实际章节硬盘路径.TabIndex = 36;
		this.是否启用章节实际内容.AutoSize = true;
		this.是否启用章节实际内容.Enabled = false;
		this.是否启用章节实际内容.Location = new System.Drawing.Point(9, 131);
		this.是否启用章节实际内容.Name = "是否启用章节实际内容";
		this.是否启用章节实际内容.Size = new System.Drawing.Size(96, 16);
		this.是否启用章节实际内容.TabIndex = 35;
		this.是否启用章节实际内容.Text = "章节实际内容";
		this.toolTip_0.SetToolTip(this.是否启用章节实际内容, "杰奇的TXT，奇文的WAR，实际储存的章节内容的位置。");
		this.是否启用章节实际内容.UseVisualStyleBackColor = true;
		this.网站路径.AutoSize = true;
		this.网站路径.Location = new System.Drawing.Point(394, 6);
		this.网站路径.Name = "网站路径";
		this.网站路径.Size = new System.Drawing.Size(113, 12);
		this.网站路径.TabIndex = 32;
		this.网站路径.Text = "网站路径(高级服务)";
		this.硬盘路径.AutoSize = true;
		this.硬盘路径.Location = new System.Drawing.Point(145, 6);
		this.硬盘路径.Name = "硬盘路径";
		this.硬盘路径.Size = new System.Drawing.Size(113, 12);
		this.硬盘路径.TabIndex = 31;
		this.硬盘路径.Text = "硬盘路径(高级服务)";
		this.全文URL路径.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.全文URL路径.Location = new System.Drawing.Point(396, 75);
		this.全文URL路径.Name = "全文URL路径";
		this.全文URL路径.Size = new System.Drawing.Size(410, 21);
		this.全文URL路径.TabIndex = 15;
		this.全文硬盘路径.Location = new System.Drawing.Point(147, 75);
		this.全文硬盘路径.Name = "全文硬盘路径";
		this.全文硬盘路径.Size = new System.Drawing.Size(243, 21);
		this.全文硬盘路径.TabIndex = 14;
		this.内容URL路径.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.内容URL路径.Location = new System.Drawing.Point(396, 48);
		this.内容URL路径.Name = "内容URL路径";
		this.内容URL路径.Size = new System.Drawing.Size(410, 21);
		this.内容URL路径.TabIndex = 11;
		this.内容硬盘路径.Location = new System.Drawing.Point(147, 48);
		this.内容硬盘路径.Name = "内容硬盘路径";
		this.内容硬盘路径.Size = new System.Drawing.Size(243, 21);
		this.内容硬盘路径.TabIndex = 10;
		this.章节目录URL路径.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.章节目录URL路径.Location = new System.Drawing.Point(396, 21);
		this.章节目录URL路径.Name = "章节目录URL路径";
		this.章节目录URL路径.Size = new System.Drawing.Size(410, 21);
		this.章节目录URL路径.TabIndex = 9;
		this.章节目录硬盘路径.Location = new System.Drawing.Point(147, 21);
		this.章节目录硬盘路径.Name = "章节目录硬盘路径";
		this.章节目录硬盘路径.Size = new System.Drawing.Size(243, 21);
		this.章节目录硬盘路径.TabIndex = 8;
		this.是否生成全文.AutoSize = true;
		this.是否生成全文.Location = new System.Drawing.Point(9, 77);
		this.是否生成全文.Name = "是否生成全文";
		this.是否生成全文.Size = new System.Drawing.Size(96, 16);
		this.是否生成全文.TabIndex = 7;
		this.是否生成全文.Text = "生成全文阅读";
		this.是否生成全文.UseVisualStyleBackColor = true;
		this.是否生成内容.AutoSize = true;
		this.是否生成内容.Location = new System.Drawing.Point(9, 50);
		this.是否生成内容.Name = "是否生成内容";
		this.是否生成内容.Size = new System.Drawing.Size(108, 16);
		this.是否生成内容.TabIndex = 5;
		this.是否生成内容.Text = "生成内容页HTML";
		this.是否生成内容.UseVisualStyleBackColor = true;
		this.是否生成目录.AutoSize = true;
		this.是否生成目录.Location = new System.Drawing.Point(9, 23);
		this.是否生成目录.Name = "是否生成目录";
		this.是否生成目录.Size = new System.Drawing.Size(108, 16);
		this.是否生成目录.TabIndex = 4;
		this.是否生成目录.Text = "生成目录页HTML";
		this.是否生成目录.UseVisualStyleBackColor = true;
		this.附加设置.Controls.Add(this.空章节自定义);
		this.附加设置.ForeColor = System.Drawing.Color.Black;
		this.附加设置.Location = new System.Drawing.Point(4, 22);
		this.附加设置.Name = "附加设置";
		this.附加设置.Size = new System.Drawing.Size(812, 378);
		this.附加设置.TabIndex = 11;
		this.附加设置.Text = "附加设置";
		this.附加设置.UseVisualStyleBackColor = true;
		this.空章节自定义.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.空章节自定义.Controls.Add(this.章节字数小于);
		this.空章节自定义.Controls.Add(this.章节字数小于Box);
		this.空章节自定义.Controls.Add(this.章节字数小于字数);
		this.空章节自定义.Controls.Add(this.label22);
		this.空章节自定义.Controls.Add(this.是否启用空章节替换);
		this.空章节自定义.Controls.Add(this.空章节替换内容Box);
		this.空章节自定义.Controls.Add(this.空章节说明);
		this.空章节自定义.Location = new System.Drawing.Point(3, 3);
		this.空章节自定义.Name = "空章节自定义";
		this.空章节自定义.Size = new System.Drawing.Size(800, 372);
		this.空章节自定义.TabIndex = 0;
		this.空章节自定义.TabStop = false;
		this.空章节自定义.Text = "空章节自定义生成";
		this.章节字数小于.AutoSize = true;
		this.章节字数小于.Location = new System.Drawing.Point(183, 20);
		this.章节字数小于.Name = "章节字数小于";
		this.章节字数小于.Size = new System.Drawing.Size(77, 12);
		this.章节字数小于.TabIndex = 73;
		this.章节字数小于.Text = "章节字数小于";
		this.章节字数小于Box.Location = new System.Drawing.Point(266, 14);
		this.章节字数小于Box.Maximum = new decimal(new int[4] { 30, 0, 0, 0 });
		this.章节字数小于Box.Name = "章节字数小于Box";
		this.章节字数小于Box.Size = new System.Drawing.Size(48, 21);
		this.章节字数小于Box.TabIndex = 72;
		this.toolTip_0.SetToolTip(this.章节字数小于Box, "这里指的是章节内容TXT文本字数。");
		this.章节字数小于Box.Value = new decimal(new int[4] { 30, 0, 0, 0 });
		this.章节字数小于字数.AutoSize = true;
		this.章节字数小于字数.Location = new System.Drawing.Point(320, 19);
		this.章节字数小于字数.Name = "章节字数小于字数";
		this.章节字数小于字数.Size = new System.Drawing.Size(113, 12);
		this.章节字数小于字数.TabIndex = 71;
		this.章节字数小于字数.Text = "字数时使用替换生成";
		this.label22.AutoSize = true;
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(14, 353);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(113, 12);
		this.label22.TabIndex = 46;
		this.label22.Text = "这里改成你的保存下";
		this.是否启用空章节替换.AutoSize = true;
		this.是否启用空章节替换.Location = new System.Drawing.Point(9, 19);
		this.是否启用空章节替换.Name = "是否启用空章节替换";
		this.是否启用空章节替换.Size = new System.Drawing.Size(132, 16);
		this.是否启用空章节替换.TabIndex = 1;
		this.是否启用空章节替换.Text = "启用空章节替换生成";
		this.是否启用空章节替换.UseVisualStyleBackColor = true;
		this.空章节替换内容Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.空章节替换内容Box.Location = new System.Drawing.Point(9, 66);
		this.空章节替换内容Box.Multiline = true;
		this.空章节替换内容Box.Name = "空章节替换内容Box";
		this.空章节替换内容Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.空章节替换内容Box.Size = new System.Drawing.Size(785, 280);
		this.空章节替换内容Box.TabIndex = 45;
		this.toolTip_0.SetToolTip(this.空章节替换内容Box, "生成HTML的时候替换，不是替换章节实际内容。");
		this.空章节说明.AutoSize = true;
		this.空章节说明.ForeColor = System.Drawing.Color.RoyalBlue;
		this.空章节说明.Location = new System.Drawing.Point(6, 43);
		this.空章节说明.Name = "空章节说明";
		this.空章节说明.Size = new System.Drawing.Size(473, 12);
		this.空章节说明.TabIndex = 2;
		this.空章节说明.Text = "该设置生效后按设置会将少于设置字数、空章节、无TXT文本的章节内容规则为以下内容!";
		this.电子书设置.Controls.Add(this.电子书URL);
		this.电子书设置.Controls.Add(this.电子书广告);
		this.电子书设置.Controls.Add(this.电子书根目录);
		this.电子书设置.Controls.Add(this.CHM_URL);
		this.电子书设置.Controls.Add(this.CHM根目录);
		this.电子书设置.Controls.Add(this.JAR_URL);
		this.电子书设置.Controls.Add(this.JAR根目录);
		this.电子书设置.Controls.Add(this.UMD_URL);
		this.电子书设置.Controls.Add(this.UMD根目录);
		this.电子书设置.Controls.Add(this.TXT_URL);
		this.电子书设置.Controls.Add(this.TXT根目录);
		this.电子书设置.Controls.Add(this.ZIP_URL);
		this.电子书设置.Controls.Add(this.ZIP根目录);
		this.电子书设置.Controls.Add(this.是否生成CHM);
		this.电子书设置.Controls.Add(this.是否生成JAR);
		this.电子书设置.Controls.Add(this.是否生成UMD);
		this.电子书设置.Controls.Add(this.是否生成TXT);
		this.电子书设置.Controls.Add(this.星否生成ZIP);
		this.电子书设置.Location = new System.Drawing.Point(4, 22);
		this.电子书设置.Name = "电子书设置";
		this.电子书设置.Size = new System.Drawing.Size(812, 378);
		this.电子书设置.TabIndex = 8;
		this.电子书设置.Text = "电子书设置";
		this.电子书设置.UseVisualStyleBackColor = true;
		this.电子书URL.AutoSize = true;
		this.电子书URL.Location = new System.Drawing.Point(394, 6);
		this.电子书URL.Name = "电子书URL";
		this.电子书URL.Size = new System.Drawing.Size(53, 12);
		this.电子书URL.TabIndex = 46;
		this.电子书URL.Text = "网站路径";
		this.电子书广告.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.电子书广告.Controls.Add(this.电子书头广告Box);
		this.电子书广告.Controls.Add(this.电子书尾广告Box);
		this.电子书广告.Controls.Add(this.电子书尾广告);
		this.电子书广告.Controls.Add(this.电子书头广告);
		this.电子书广告.ForeColor = System.Drawing.Color.Black;
		this.电子书广告.Location = new System.Drawing.Point(6, 155);
		this.电子书广告.Name = "电子书广告";
		this.电子书广告.Size = new System.Drawing.Size(800, 217);
		this.电子书广告.TabIndex = 36;
		this.电子书广告.TabStop = false;
		this.电子书广告.Text = "电子书广告";
		this.电子书头广告Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.电子书头广告Box.Location = new System.Drawing.Point(8, 32);
		this.电子书头广告Box.Multiline = true;
		this.电子书头广告Box.Name = "电子书头广告Box";
		this.电子书头广告Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.电子书头广告Box.Size = new System.Drawing.Size(318, 179);
		this.电子书头广告Box.TabIndex = 44;
		this.toolTip_0.SetToolTip(this.电子书头广告Box, "生成HTML的时候替换，不是替换章节实际内容。");
		this.电子书尾广告Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.电子书尾广告Box.Location = new System.Drawing.Point(332, 32);
		this.电子书尾广告Box.Multiline = true;
		this.电子书尾广告Box.Name = "电子书尾广告Box";
		this.电子书尾广告Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.电子书尾广告Box.Size = new System.Drawing.Size(462, 179);
		this.电子书尾广告Box.TabIndex = 43;
		this.toolTip_0.SetToolTip(this.电子书尾广告Box, "生成HTML的时候替换，不是替换章节实际内容。");
		this.电子书尾广告.AutoSize = true;
		this.电子书尾广告.Location = new System.Drawing.Point(330, 17);
		this.电子书尾广告.Name = "电子书尾广告";
		this.电子书尾广告.Size = new System.Drawing.Size(125, 12);
		this.电子书尾广告.TabIndex = 42;
		this.电子书尾广告.Text = "电子书尾增加广告代码";
		this.电子书头广告.AutoSize = true;
		this.电子书头广告.Location = new System.Drawing.Point(6, 17);
		this.电子书头广告.Name = "电子书头广告";
		this.电子书头广告.Size = new System.Drawing.Size(125, 12);
		this.电子书头广告.TabIndex = 40;
		this.电子书头广告.Text = "电子书头增加广告代码";
		this.电子书根目录.AutoSize = true;
		this.电子书根目录.Location = new System.Drawing.Point(101, 6);
		this.电子书根目录.Name = "电子书根目录";
		this.电子书根目录.Size = new System.Drawing.Size(113, 12);
		this.电子书根目录.TabIndex = 45;
		this.电子书根目录.Text = "硬盘路径(无需授权)";
		this.CHM_URL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.CHM_URL.Location = new System.Drawing.Point(396, 128);
		this.CHM_URL.Name = "CHM_URL";
		this.CHM_URL.Size = new System.Drawing.Size(410, 21);
		this.CHM_URL.TabIndex = 48;
		this.CHM根目录.Location = new System.Drawing.Point(103, 128);
		this.CHM根目录.Name = "CHM根目录";
		this.CHM根目录.Size = new System.Drawing.Size(287, 21);
		this.CHM根目录.TabIndex = 47;
		this.JAR_URL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.JAR_URL.Location = new System.Drawing.Point(396, 102);
		this.JAR_URL.Name = "JAR_URL";
		this.JAR_URL.Size = new System.Drawing.Size(410, 21);
		this.JAR_URL.TabIndex = 46;
		this.JAR根目录.Location = new System.Drawing.Point(103, 102);
		this.JAR根目录.Name = "JAR根目录";
		this.JAR根目录.Size = new System.Drawing.Size(287, 21);
		this.JAR根目录.TabIndex = 45;
		this.UMD_URL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.UMD_URL.Location = new System.Drawing.Point(396, 75);
		this.UMD_URL.Name = "UMD_URL";
		this.UMD_URL.Size = new System.Drawing.Size(410, 21);
		this.UMD_URL.TabIndex = 44;
		this.UMD根目录.Location = new System.Drawing.Point(103, 75);
		this.UMD根目录.Name = "UMD根目录";
		this.UMD根目录.Size = new System.Drawing.Size(287, 21);
		this.UMD根目录.TabIndex = 43;
		this.TXT_URL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.TXT_URL.Location = new System.Drawing.Point(396, 48);
		this.TXT_URL.Name = "TXT_URL";
		this.TXT_URL.Size = new System.Drawing.Size(410, 21);
		this.TXT_URL.TabIndex = 42;
		this.TXT根目录.Location = new System.Drawing.Point(103, 48);
		this.TXT根目录.Name = "TXT根目录";
		this.TXT根目录.Size = new System.Drawing.Size(287, 21);
		this.TXT根目录.TabIndex = 41;
		this.ZIP_URL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ZIP_URL.Location = new System.Drawing.Point(396, 21);
		this.ZIP_URL.Name = "ZIP_URL";
		this.ZIP_URL.Size = new System.Drawing.Size(410, 21);
		this.ZIP_URL.TabIndex = 40;
		this.ZIP根目录.Location = new System.Drawing.Point(103, 21);
		this.ZIP根目录.Name = "ZIP根目录";
		this.ZIP根目录.Size = new System.Drawing.Size(287, 21);
		this.ZIP根目录.TabIndex = 39;
		this.是否生成CHM.AutoSize = true;
		this.是否生成CHM.Enabled = false;
		this.是否生成CHM.Location = new System.Drawing.Point(9, 130);
		this.是否生成CHM.Name = "是否生成CHM";
		this.是否生成CHM.Size = new System.Drawing.Size(66, 16);
		this.是否生成CHM.TabIndex = 38;
		this.是否生成CHM.Text = "生成CHM";
		this.是否生成CHM.UseVisualStyleBackColor = true;
		this.是否生成JAR.AutoSize = true;
		this.是否生成JAR.Location = new System.Drawing.Point(9, 104);
		this.是否生成JAR.Name = "是否生成JAR";
		this.是否生成JAR.Size = new System.Drawing.Size(66, 16);
		this.是否生成JAR.TabIndex = 37;
		this.是否生成JAR.Text = "生成JAR";
		this.是否生成JAR.UseVisualStyleBackColor = true;
		this.是否生成UMD.AutoSize = true;
		this.是否生成UMD.Location = new System.Drawing.Point(9, 77);
		this.是否生成UMD.Name = "是否生成UMD";
		this.是否生成UMD.Size = new System.Drawing.Size(66, 16);
		this.是否生成UMD.TabIndex = 36;
		this.是否生成UMD.Text = "生成UMD";
		this.是否生成UMD.UseVisualStyleBackColor = true;
		this.是否生成TXT.AutoSize = true;
		this.是否生成TXT.Location = new System.Drawing.Point(9, 50);
		this.是否生成TXT.Name = "是否生成TXT";
		this.是否生成TXT.Size = new System.Drawing.Size(66, 16);
		this.是否生成TXT.TabIndex = 35;
		this.是否生成TXT.Text = "生成TXT";
		this.是否生成TXT.UseVisualStyleBackColor = true;
		this.星否生成ZIP.AutoSize = true;
		this.星否生成ZIP.Location = new System.Drawing.Point(9, 23);
		this.星否生成ZIP.Name = "星否生成ZIP";
		this.星否生成ZIP.Size = new System.Drawing.Size(66, 16);
		this.星否生成ZIP.TabIndex = 34;
		this.星否生成ZIP.Text = "生成ZIP";
		this.星否生成ZIP.UseVisualStyleBackColor = true;
		this.文字广告.Controls.Add(this.是否电子书添加文字广告);
		this.文字广告.Controls.Add(this.固定位置添加广告);
		this.文字广告.Controls.Add(this.是否生成HTML添加文字广告);
		this.文字广告.Controls.Add(this.是否添加文字广告);
		this.文字广告.Controls.Add(this.文字广告集合);
		this.文字广告.Controls.Add(this.添加文字广告);
		this.文字广告.Location = new System.Drawing.Point(4, 22);
		this.文字广告.Name = "文字广告";
		this.文字广告.Size = new System.Drawing.Size(812, 378);
		this.文字广告.TabIndex = 5;
		this.文字广告.Text = "文字广告";
		this.文字广告.UseVisualStyleBackColor = true;
		this.是否电子书添加文字广告.AutoSize = true;
		this.是否电子书添加文字广告.Location = new System.Drawing.Point(375, 9);
		this.是否电子书添加文字广告.Name = "是否电子书添加文字广告";
		this.是否电子书添加文字广告.Size = new System.Drawing.Size(192, 16);
		this.是否电子书添加文字广告.TabIndex = 7;
		this.是否电子书添加文字广告.Text = "最后生成电子书时添加文字广告";
		this.是否电子书添加文字广告.UseVisualStyleBackColor = true;
		this.固定位置添加广告.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.固定位置添加广告.Controls.Add(this.章节尾部广告Box);
		this.固定位置添加广告.Controls.Add(this.章节尾部广告);
		this.固定位置添加广告.Controls.Add(this.章节头部广告Box);
		this.固定位置添加广告.Controls.Add(this.章节头部广告);
		this.固定位置添加广告.Location = new System.Drawing.Point(244, 53);
		this.固定位置添加广告.Name = "固定位置添加广告";
		this.固定位置添加广告.Size = new System.Drawing.Size(562, 158);
		this.固定位置添加广告.TabIndex = 6;
		this.固定位置添加广告.TabStop = false;
		this.固定位置添加广告.Text = "固定位置添加广告";
		this.章节尾部广告Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.章节尾部广告Box.Location = new System.Drawing.Point(6, 100);
		this.章节尾部广告Box.Multiline = true;
		this.章节尾部广告Box.Name = "章节尾部广告Box";
		this.章节尾部广告Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.章节尾部广告Box.Size = new System.Drawing.Size(550, 50);
		this.章节尾部广告Box.TabIndex = 16;
		this.章节尾部广告.AutoSize = true;
		this.章节尾部广告.Location = new System.Drawing.Point(6, 85);
		this.章节尾部广告.Name = "章节尾部广告";
		this.章节尾部广告.Size = new System.Drawing.Size(209, 12);
		this.章节尾部广告.TabIndex = 15;
		this.章节尾部广告.Text = "章节尾部(可以用回车控制是否分段落)";
		this.章节头部广告Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.章节头部广告Box.Location = new System.Drawing.Point(6, 32);
		this.章节头部广告Box.Multiline = true;
		this.章节头部广告Box.Name = "章节头部广告Box";
		this.章节头部广告Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.章节头部广告Box.Size = new System.Drawing.Size(550, 50);
		this.章节头部广告Box.TabIndex = 14;
		this.章节头部广告.AutoSize = true;
		this.章节头部广告.Location = new System.Drawing.Point(6, 17);
		this.章节头部广告.Name = "章节头部广告";
		this.章节头部广告.Size = new System.Drawing.Size(209, 12);
		this.章节头部广告.TabIndex = 0;
		this.章节头部广告.Text = "章节头部(可以用回车控制是否分段落)";
		this.是否生成HTML添加文字广告.AutoSize = true;
		this.是否生成HTML添加文字广告.Location = new System.Drawing.Point(9, 31);
		this.是否生成HTML添加文字广告.Name = "是否生成HTML添加文字广告";
		this.是否生成HTML添加文字广告.Size = new System.Drawing.Size(444, 16);
		this.是否生成HTML添加文字广告.TabIndex = 5;
		this.是否生成HTML添加文字广告.Text = "最后生成时添加文字广告(生成HTML的时候添加，使用后台生成还是没有广告的)";
		this.是否生成HTML添加文字广告.UseVisualStyleBackColor = true;
		this.是否添加文字广告.AutoSize = true;
		this.是否添加文字广告.Checked = true;
		this.是否添加文字广告.CheckState = System.Windows.Forms.CheckState.Checked;
		this.是否添加文字广告.Location = new System.Drawing.Point(9, 9);
		this.是否添加文字广告.Name = "是否添加文字广告";
		this.是否添加文字广告.Size = new System.Drawing.Size(360, 16);
		this.是否添加文字广告.TabIndex = 4;
		this.是否添加文字广告.Text = "入库章节时添加文字广告(真实入库，使用后台生成的时候也会)";
		this.是否添加文字广告.UseVisualStyleBackColor = true;
		this.文字广告集合.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.文字广告集合.Controls.Add(this.文字广告集合Box);
		this.文字广告集合.Location = new System.Drawing.Point(244, 217);
		this.文字广告集合.Name = "文字广告集合";
		this.文字广告集合.Size = new System.Drawing.Size(562, 155);
		this.文字广告集合.TabIndex = 3;
		this.文字广告集合.TabStop = false;
		this.文字广告集合.Text = "文字广告集合(一行一条广告语)";
		this.文字广告集合Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.文字广告集合Box.Location = new System.Drawing.Point(6, 20);
		this.文字广告集合Box.Multiline = true;
		this.文字广告集合Box.Name = "文字广告集合Box";
		this.文字广告集合Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.文字广告集合Box.Size = new System.Drawing.Size(550, 129);
		this.文字广告集合Box.TabIndex = 13;
		this.toolTip_0.SetToolTip(this.文字广告集合Box, "一行写一个广告条，可以很多个，会随机抽取的");
		this.添加文字广告.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.添加文字广告.Controls.Add(this.留空表示不限制分卷);
		this.添加文字广告.Controls.Add(this.添加文字广告个数);
		this.添加文字广告.Controls.Add(this.每个章节添加几个广告);
		this.添加文字广告.Controls.Add(this.添加文字广告分卷限制);
		this.添加文字广告.Controls.Add(this.限制只在以下分卷中添加文字广告);
		this.添加文字广告.Location = new System.Drawing.Point(6, 53);
		this.添加文字广告.Name = "添加文字广告";
		this.添加文字广告.Size = new System.Drawing.Size(232, 319);
		this.添加文字广告.TabIndex = 2;
		this.添加文字广告.TabStop = false;
		this.添加文字广告.Text = "添加文字广告";
		this.留空表示不限制分卷.AutoSize = true;
		this.留空表示不限制分卷.Location = new System.Drawing.Point(6, 32);
		this.留空表示不限制分卷.Name = "留空表示不限制分卷";
		this.留空表示不限制分卷.Size = new System.Drawing.Size(113, 12);
		this.留空表示不限制分卷.TabIndex = 12;
		this.留空表示不限制分卷.Text = "留空表示不限制分卷";
		this.添加文字广告个数.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.添加文字广告个数.Location = new System.Drawing.Point(6, 292);
		this.添加文字广告个数.Name = "添加文字广告个数";
		this.添加文字广告个数.Size = new System.Drawing.Size(220, 21);
		this.添加文字广告个数.TabIndex = 11;
		this.每个章节添加几个广告.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.每个章节添加几个广告.AutoSize = true;
		this.每个章节添加几个广告.Location = new System.Drawing.Point(6, 277);
		this.每个章节添加几个广告.Name = "每个章节添加几个广告";
		this.每个章节添加几个广告.Size = new System.Drawing.Size(125, 12);
		this.每个章节添加几个广告.TabIndex = 10;
		this.每个章节添加几个广告.Text = "每个章节添加几个广告";
		this.添加文字广告分卷限制.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.添加文字广告分卷限制.Location = new System.Drawing.Point(6, 47);
		this.添加文字广告分卷限制.Multiline = true;
		this.添加文字广告分卷限制.Name = "添加文字广告分卷限制";
		this.添加文字广告分卷限制.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.添加文字广告分卷限制.Size = new System.Drawing.Size(220, 227);
		this.添加文字广告分卷限制.TabIndex = 9;
		this.toolTip_0.SetToolTip(this.添加文字广告分卷限制, "一行写一个分卷名\n留空就表示在所有文字章节中都添加");
		this.限制只在以下分卷中添加文字广告.AutoSize = true;
		this.限制只在以下分卷中添加文字广告.Location = new System.Drawing.Point(6, 17);
		this.限制只在以下分卷中添加文字广告.Name = "限制只在以下分卷中添加文字广告";
		this.限制只在以下分卷中添加文字广告.Size = new System.Drawing.Size(185, 12);
		this.限制只在以下分卷中添加文字广告.TabIndex = 8;
		this.限制只在以下分卷中添加文字广告.Text = "限制只在以下分卷中添加文字广告";
		this.过滤替换.Controls.Add(this.章节内容非法字符替换);
		this.过滤替换.Controls.Add(this.违禁小说过滤);
		this.过滤替换.Controls.Add(this.章节内容非法字符过滤);
		this.过滤替换.Location = new System.Drawing.Point(4, 22);
		this.过滤替换.Name = "过滤替换";
		this.过滤替换.Size = new System.Drawing.Size(812, 378);
		this.过滤替换.TabIndex = 6;
		this.过滤替换.Text = "过滤替换";
		this.过滤替换.UseVisualStyleBackColor = true;
		this.章节内容非法字符替换.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.章节内容非法字符替换.Controls.Add(this.章节内容非法字符替换Box);
		this.章节内容非法字符替换.Location = new System.Drawing.Point(221, 154);
		this.章节内容非法字符替换.Name = "章节内容非法字符替换";
		this.章节内容非法字符替换.Size = new System.Drawing.Size(585, 218);
		this.章节内容非法字符替换.TabIndex = 16;
		this.章节内容非法字符替换.TabStop = false;
		this.章节内容非法字符替换.Text = "章节内容非法字符替换(仅在最后生成时)";
		this.章节内容非法字符替换Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.章节内容非法字符替换Box.Location = new System.Drawing.Point(6, 14);
		this.章节内容非法字符替换Box.Multiline = true;
		this.章节内容非法字符替换Box.Name = "章节内容非法字符替换Box";
		this.章节内容非法字符替换Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.章节内容非法字符替换Box.Size = new System.Drawing.Size(573, 198);
		this.章节内容非法字符替换Box.TabIndex = 13;
		this.toolTip_0.SetToolTip(this.章节内容非法字符替换Box, "生成HTML的时候替换，不是替换章节实际内容。\n一行一个，格式如下：\n101du.net|yfxsw.com\n性|<img src=\"images/xing.gif\">\n高潮|高氵朝");
		this.违禁小说过滤.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.违禁小说过滤.Controls.Add(this.违禁小说过滤Box);
		this.违禁小说过滤.Location = new System.Drawing.Point(6, 6);
		this.违禁小说过滤.Name = "违禁小说过滤";
		this.违禁小说过滤.Size = new System.Drawing.Size(209, 366);
		this.违禁小说过滤.TabIndex = 5;
		this.违禁小说过滤.TabStop = false;
		this.违禁小说过滤.Text = "违禁小说过滤(一行一个)";
		this.违禁小说过滤Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.违禁小说过滤Box.Location = new System.Drawing.Point(6, 14);
		this.违禁小说过滤Box.Multiline = true;
		this.违禁小说过滤Box.Name = "违禁小说过滤Box";
		this.违禁小说过滤Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.违禁小说过滤Box.Size = new System.Drawing.Size(197, 346);
		this.违禁小说过滤Box.TabIndex = 13;
		this.章节内容非法字符过滤.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.章节内容非法字符过滤.Controls.Add(this.默认替换字符);
		this.章节内容非法字符过滤.Controls.Add(this.默认替换字符Box);
		this.章节内容非法字符过滤.Controls.Add(this.章节内容非法字符过滤Box);
		this.章节内容非法字符过滤.Location = new System.Drawing.Point(221, 6);
		this.章节内容非法字符过滤.Name = "章节内容非法字符过滤";
		this.章节内容非法字符过滤.Size = new System.Drawing.Size(585, 142);
		this.章节内容非法字符过滤.TabIndex = 4;
		this.章节内容非法字符过滤.TabStop = false;
		this.章节内容非法字符过滤.Text = "章节内容非法字符过滤(正则过滤)(仅在最后生成时)";
		this.默认替换字符.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.默认替换字符.AutoSize = true;
		this.默认替换字符.Location = new System.Drawing.Point(6, 118);
		this.默认替换字符.Name = "默认替换字符";
		this.默认替换字符.Size = new System.Drawing.Size(89, 12);
		this.默认替换字符.TabIndex = 15;
		this.默认替换字符.Text = "默认替换字符：";
		this.默认替换字符Box.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.默认替换字符Box.Location = new System.Drawing.Point(101, 115);
		this.默认替换字符Box.Name = "默认替换字符Box";
		this.默认替换字符Box.Size = new System.Drawing.Size(478, 21);
		this.默认替换字符Box.TabIndex = 14;
		this.默认替换字符Box.Text = "**";
		this.章节内容非法字符过滤Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.章节内容非法字符过滤Box.Location = new System.Drawing.Point(6, 14);
		this.章节内容非法字符过滤Box.Multiline = true;
		this.章节内容非法字符过滤Box.Name = "章节内容非法字符过滤Box";
		this.章节内容非法字符过滤Box.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.章节内容非法字符过滤Box.Size = new System.Drawing.Size(573, 95);
		this.章节内容非法字符过滤Box.TabIndex = 13;
		this.toolTip_0.SetToolTip(this.章节内容非法字符过滤Box, "生成HTML的时候替换，不是替换章节实际内容。");
		this.日志选择.Controls.Add(this.选择需要记录的日志项);
		this.日志选择.Controls.Add(this.日志记录Box);
		this.日志选择.Location = new System.Drawing.Point(4, 22);
		this.日志选择.Name = "日志选择";
		this.日志选择.Size = new System.Drawing.Size(812, 378);
		this.日志选择.TabIndex = 9;
		this.日志选择.Text = "日志选择";
		this.日志选择.UseVisualStyleBackColor = true;
		this.选择需要记录的日志项.AutoSize = true;
		this.选择需要记录的日志项.Location = new System.Drawing.Point(6, 7);
		this.选择需要记录的日志项.Name = "选择需要记录的日志项";
		this.选择需要记录的日志项.Size = new System.Drawing.Size(161, 12);
		this.选择需要记录的日志项.TabIndex = 1;
		this.选择需要记录的日志项.Text = "请选择需要记录的日志项目：";
		this.日志记录Box.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.日志记录Box.CheckOnClick = true;
		this.日志记录Box.ColumnWidth = 300;
		this.日志记录Box.FormattingEnabled = true;
		this.日志记录Box.Location = new System.Drawing.Point(8, 25);
		this.日志记录Box.MultiColumn = true;
		this.日志记录Box.Name = "日志记录Box";
		this.日志记录Box.Size = new System.Drawing.Size(795, 340);
		this.日志记录Box.TabIndex = 0;
		this.杰奇目录DIY.BackColor = System.Drawing.Color.Transparent;
		this.杰奇目录DIY.Controls.Add(this.模板路径为相对主目录路径);
		this.杰奇目录DIY.Controls.Add(this.杰奇DIY说明2);
		this.杰奇目录DIY.Controls.Add(this.杰奇DIY说明1);
		this.杰奇目录DIY.Controls.Add(this.信息页网站调用URL);
		this.杰奇目录DIY.Controls.Add(this.OPF网站调用URL);
		this.杰奇目录DIY.Controls.Add(this.TXT网站调用URL);
		this.杰奇目录DIY.Controls.Add(this.章节内容网站调用URL);
		this.杰奇目录DIY.Controls.Add(this.章节列表网站调用URL);
		this.杰奇目录DIY.Controls.Add(this.OPF文件硬盘路径);
		this.杰奇目录DIY.Controls.Add(this.TXT文件硬盘路径);
		this.杰奇目录DIY.Controls.Add(this.章节硬盘路径);
		this.杰奇目录DIY.Controls.Add(this.章节列表硬盘路径);
		this.杰奇目录DIY.Controls.Add(this.文章信息硬盘路径);
		this.杰奇目录DIY.Controls.Add(this.章节模板路径);
		this.杰奇目录DIY.Controls.Add(this.章节页模板);
		this.杰奇目录DIY.Controls.Add(this.目录模板路径);
		this.杰奇目录DIY.Controls.Add(this.目录页模板);
		this.杰奇目录DIY.Controls.Add(this.信息页模版路径);
		this.杰奇目录DIY.Controls.Add(this.信息页模板);
		this.杰奇目录DIY.Controls.Add(this.首页模板路径);
		this.杰奇目录DIY.Controls.Add(this.首页模板);
		this.杰奇目录DIY.Controls.Add(this.OPF硬盘文件夹);
		this.杰奇目录DIY.Controls.Add(this.TXT硬盘文件夹);
		this.杰奇目录DIY.Controls.Add(this.章节HTML硬盘文件夹);
		this.杰奇目录DIY.Controls.Add(this.目录HTML硬盘文件夹);
		this.杰奇目录DIY.Controls.Add(this.信息页HTML硬盘文件夹);
		this.杰奇目录DIY.Controls.Add(this.OPF文件URL);
		this.杰奇目录DIY.Controls.Add(this.TXT文件URL);
		this.杰奇目录DIY.Controls.Add(this.章节内容页URL);
		this.杰奇目录DIY.Controls.Add(this.章节列表页URL);
		this.杰奇目录DIY.Controls.Add(this.文章信息页URL);
		this.杰奇目录DIY.ForeColor = System.Drawing.SystemColors.ControlText;
		this.杰奇目录DIY.Location = new System.Drawing.Point(4, 22);
		this.杰奇目录DIY.Name = "杰奇目录DIY";
		this.杰奇目录DIY.Size = new System.Drawing.Size(812, 378);
		this.杰奇目录DIY.TabIndex = 14;
		this.杰奇目录DIY.Text = "杰奇目录DIY";
		this.模板路径为相对主目录路径.AutoSize = true;
		this.模板路径为相对主目录路径.ForeColor = System.Drawing.Color.Red;
		this.模板路径为相对主目录路径.Location = new System.Drawing.Point(555, 357);
		this.模板路径为相对主目录路径.Name = "模板路径为相对主目录路径";
		this.模板路径为相对主目录路径.Size = new System.Drawing.Size(149, 12);
		this.模板路径为相对主目录路径.TabIndex = 97;
		this.模板路径为相对主目录路径.Text = "模板路径为相对主目录路径";
		this.杰奇DIY说明2.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.杰奇DIY说明2.ForeColor = System.Drawing.Color.Navy;
		this.杰奇DIY说明2.Location = new System.Drawing.Point(544, 210);
		this.杰奇DIY说明2.Name = "杰奇DIY说明2";
		this.杰奇DIY说明2.Size = new System.Drawing.Size(260, 144);
		this.杰奇DIY说明2.TabIndex = 96;
		this.杰奇DIY说明2.Text = resources.GetString("杰奇DIY说明2.Text");
		this.杰奇DIY说明1.BackColor = System.Drawing.SystemColors.Window;
		this.杰奇DIY说明1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.杰奇DIY说明1.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.杰奇DIY说明1.ForeColor = System.Drawing.Color.Navy;
		this.杰奇DIY说明1.Location = new System.Drawing.Point(544, 7);
		this.杰奇DIY说明1.Name = "杰奇DIY说明1";
		this.杰奇DIY说明1.ReadOnly = true;
		this.杰奇DIY说明1.Size = new System.Drawing.Size(260, 200);
		this.杰奇DIY说明1.TabIndex = 95;
		this.杰奇DIY说明1.Text = resources.GetString("杰奇DIY说明1.Text");
		this.信息页网站调用URL.Enabled = false;
		this.信息页网站调用URL.Location = new System.Drawing.Point(128, 4);
		this.信息页网站调用URL.Name = "信息页网站调用URL";
		this.信息页网站调用URL.Size = new System.Drawing.Size(410, 21);
		this.信息页网站调用URL.TabIndex = 94;
		this.信息页网站调用URL.Text = "2";
		this.OPF网站调用URL.Enabled = false;
		this.OPF网站调用URL.Location = new System.Drawing.Point(128, 109);
		this.OPF网站调用URL.Name = "OPF网站调用URL";
		this.OPF网站调用URL.Size = new System.Drawing.Size(410, 21);
		this.OPF网站调用URL.TabIndex = 93;
		this.TXT网站调用URL.Enabled = false;
		this.TXT网站调用URL.Location = new System.Drawing.Point(128, 81);
		this.TXT网站调用URL.Name = "TXT网站调用URL";
		this.TXT网站调用URL.Size = new System.Drawing.Size(410, 21);
		this.TXT网站调用URL.TabIndex = 92;
		this.章节内容网站调用URL.Enabled = false;
		this.章节内容网站调用URL.Location = new System.Drawing.Point(128, 55);
		this.章节内容网站调用URL.Name = "章节内容网站调用URL";
		this.章节内容网站调用URL.Size = new System.Drawing.Size(410, 21);
		this.章节内容网站调用URL.TabIndex = 91;
		this.章节列表网站调用URL.Enabled = false;
		this.章节列表网站调用URL.Location = new System.Drawing.Point(128, 28);
		this.章节列表网站调用URL.Name = "章节列表网站调用URL";
		this.章节列表网站调用URL.Size = new System.Drawing.Size(410, 21);
		this.章节列表网站调用URL.TabIndex = 90;
		this.OPF文件硬盘路径.AutoSize = true;
		this.OPF文件硬盘路径.Location = new System.Drawing.Point(16, 251);
		this.OPF文件硬盘路径.Name = "OPF文件硬盘路径";
		this.OPF文件硬盘路径.Size = new System.Drawing.Size(107, 12);
		this.OPF文件硬盘路径.TabIndex = 89;
		this.OPF文件硬盘路径.Text = "OPF文件硬盘路径：";
		this.TXT文件硬盘路径.AutoSize = true;
		this.TXT文件硬盘路径.Location = new System.Drawing.Point(16, 222);
		this.TXT文件硬盘路径.Name = "TXT文件硬盘路径";
		this.TXT文件硬盘路径.Size = new System.Drawing.Size(107, 12);
		this.TXT文件硬盘路径.TabIndex = 88;
		this.TXT文件硬盘路径.Text = "TXT文件硬盘路径：";
		this.章节硬盘路径.AutoSize = true;
		this.章节硬盘路径.Location = new System.Drawing.Point(33, 195);
		this.章节硬盘路径.Name = "章节硬盘路径";
		this.章节硬盘路径.Size = new System.Drawing.Size(89, 12);
		this.章节硬盘路径.TabIndex = 87;
		this.章节硬盘路径.Text = "章节硬盘路径：";
		this.章节列表硬盘路径.AutoSize = true;
		this.章节列表硬盘路径.Location = new System.Drawing.Point(9, 169);
		this.章节列表硬盘路径.Name = "章节列表硬盘路径";
		this.章节列表硬盘路径.Size = new System.Drawing.Size(113, 12);
		this.章节列表硬盘路径.TabIndex = 86;
		this.章节列表硬盘路径.Text = "章节列表硬盘路径：";
		this.文章信息硬盘路径.AutoSize = true;
		this.文章信息硬盘路径.Location = new System.Drawing.Point(9, 141);
		this.文章信息硬盘路径.Name = "文章信息硬盘路径";
		this.文章信息硬盘路径.Size = new System.Drawing.Size(113, 12);
		this.文章信息硬盘路径.TabIndex = 85;
		this.文章信息硬盘路径.Text = "文章信息硬盘路径：";
		this.章节模板路径.Location = new System.Drawing.Point(128, 354);
		this.章节模板路径.Name = "章节模板路径";
		this.章节模板路径.Size = new System.Drawing.Size(410, 21);
		this.章节模板路径.TabIndex = 84;
		this.toolTip_0.SetToolTip(this.章节模板路径, "自定义模板路径，默认为网站根目录\\modules\\article\\templates\\style.html\r\n");
		this.章节页模板.AutoSize = true;
		this.章节页模板.Location = new System.Drawing.Point(45, 356);
		this.章节页模板.Name = "章节页模板";
		this.章节页模板.Size = new System.Drawing.Size(77, 12);
		this.章节页模板.TabIndex = 83;
		this.章节页模板.Text = "章节页模板：";
		this.目录模板路径.Location = new System.Drawing.Point(128, 327);
		this.目录模板路径.Name = "目录模板路径";
		this.目录模板路径.Size = new System.Drawing.Size(410, 21);
		this.目录模板路径.TabIndex = 82;
		this.toolTip_0.SetToolTip(this.目录模板路径, "自定义模板路径，默认为网站根目录\\modules\\article\\templates\\index.html\r\n");
		this.目录页模板.AutoSize = true;
		this.目录页模板.Location = new System.Drawing.Point(45, 330);
		this.目录页模板.Name = "目录页模板";
		this.目录页模板.Size = new System.Drawing.Size(77, 12);
		this.目录页模板.TabIndex = 81;
		this.目录页模板.Text = "目录页模板：";
		this.信息页模版路径.Enabled = false;
		this.信息页模版路径.Location = new System.Drawing.Point(128, 298);
		this.信息页模版路径.Name = "信息页模版路径";
		this.信息页模版路径.Size = new System.Drawing.Size(410, 21);
		this.信息页模版路径.TabIndex = 80;
		this.toolTip_0.SetToolTip(this.信息页模版路径, "自定义模板路径，默认为网站根目录\\modules\\article\\templates\\articleinfo.html");
		this.信息页模板.AutoSize = true;
		this.信息页模板.Location = new System.Drawing.Point(45, 303);
		this.信息页模板.Name = "信息页模板";
		this.信息页模板.Size = new System.Drawing.Size(77, 12);
		this.信息页模板.TabIndex = 79;
		this.信息页模板.Text = "信息页模板：";
		this.首页模板路径.Enabled = false;
		this.首页模板路径.Location = new System.Drawing.Point(128, 273);
		this.首页模板路径.Name = "首页模板路径";
		this.首页模板路径.Size = new System.Drawing.Size(410, 21);
		this.首页模板路径.TabIndex = 78;
		this.toolTip_0.SetToolTip(this.首页模板路径, "自定义模板路径，默认为网站根目录\\templates\\index.html");
		this.首页模板.AutoSize = true;
		this.首页模板.Location = new System.Drawing.Point(57, 277);
		this.首页模板.Name = "首页模板";
		this.首页模板.Size = new System.Drawing.Size(65, 12);
		this.首页模板.TabIndex = 77;
		this.首页模板.Text = "首页模板：";
		this.OPF硬盘文件夹.Enabled = false;
		this.OPF硬盘文件夹.Location = new System.Drawing.Point(128, 246);
		this.OPF硬盘文件夹.Name = "OPF硬盘文件夹";
		this.OPF硬盘文件夹.Size = new System.Drawing.Size(410, 21);
		this.OPF硬盘文件夹.TabIndex = 76;
		this.toolTip_0.SetToolTip(this.OPF硬盘文件夹, "\\files\\article\\txt<{$id|subdirectory}>\\<{$id}>\\index.opf");
		this.TXT硬盘文件夹.Enabled = false;
		this.TXT硬盘文件夹.Location = new System.Drawing.Point(128, 219);
		this.TXT硬盘文件夹.Name = "TXT硬盘文件夹";
		this.TXT硬盘文件夹.Size = new System.Drawing.Size(410, 21);
		this.TXT硬盘文件夹.TabIndex = 75;
		this.toolTip_0.SetToolTip(this.TXT硬盘文件夹, "\\files\\article\\txt<{$id|subdirectory}>\\<{$id}>\\<{$cid}>.txt");
		this.章节HTML硬盘文件夹.Enabled = false;
		this.章节HTML硬盘文件夹.Location = new System.Drawing.Point(128, 192);
		this.章节HTML硬盘文件夹.Name = "章节HTML硬盘文件夹";
		this.章节HTML硬盘文件夹.Size = new System.Drawing.Size(410, 21);
		this.章节HTML硬盘文件夹.TabIndex = 74;
		this.toolTip_0.SetToolTip(this.章节HTML硬盘文件夹, "\\book<{$id|subdirectory}>\\<{$id}>\\<{$cid}>.html");
		this.目录HTML硬盘文件夹.Enabled = false;
		this.目录HTML硬盘文件夹.Location = new System.Drawing.Point(128, 165);
		this.目录HTML硬盘文件夹.Name = "目录HTML硬盘文件夹";
		this.目录HTML硬盘文件夹.Size = new System.Drawing.Size(410, 21);
		this.目录HTML硬盘文件夹.TabIndex = 73;
		this.toolTip_0.SetToolTip(this.目录HTML硬盘文件夹, "\\book<{$id|subdirectory}>\\<{$id}>\\index.html\r\n");
		this.信息页HTML硬盘文件夹.Enabled = false;
		this.信息页HTML硬盘文件夹.Location = new System.Drawing.Point(128, 138);
		this.信息页HTML硬盘文件夹.Name = "信息页HTML硬盘文件夹";
		this.信息页HTML硬盘文件夹.Size = new System.Drawing.Size(410, 21);
		this.信息页HTML硬盘文件夹.TabIndex = 72;
		this.toolTip_0.SetToolTip(this.信息页HTML硬盘文件夹, "例入\\book\\<{$pyh}>_<{$id}>.html\r\n");
		this.OPF文件URL.AutoSize = true;
		this.OPF文件URL.Location = new System.Drawing.Point(39, 112);
		this.OPF文件URL.Name = "OPF文件URL";
		this.OPF文件URL.Size = new System.Drawing.Size(77, 12);
		this.OPF文件URL.TabIndex = 71;
		this.OPF文件URL.Text = "OPF文件URL：";
		this.TXT文件URL.AutoSize = true;
		this.TXT文件URL.Location = new System.Drawing.Point(39, 84);
		this.TXT文件URL.Name = "TXT文件URL";
		this.TXT文件URL.Size = new System.Drawing.Size(77, 12);
		this.TXT文件URL.TabIndex = 70;
		this.TXT文件URL.Text = "TXT文件URL：";
		this.章节内容页URL.AutoSize = true;
		this.章节内容页URL.Location = new System.Drawing.Point(21, 58);
		this.章节内容页URL.Name = "章节内容页URL";
		this.章节内容页URL.Size = new System.Drawing.Size(95, 12);
		this.章节内容页URL.TabIndex = 69;
		this.章节内容页URL.Text = "章节内容页URL：";
		this.章节列表页URL.AutoSize = true;
		this.章节列表页URL.Location = new System.Drawing.Point(21, 34);
		this.章节列表页URL.Name = "章节列表页URL";
		this.章节列表页URL.Size = new System.Drawing.Size(95, 12);
		this.章节列表页URL.TabIndex = 68;
		this.章节列表页URL.Text = "章节列表页URL：";
		this.文章信息页URL.AutoSize = true;
		this.文章信息页URL.Location = new System.Drawing.Point(21, 9);
		this.文章信息页URL.Name = "文章信息页URL";
		this.文章信息页URL.Size = new System.Drawing.Size(95, 12);
		this.文章信息页URL.TabIndex = 67;
		this.文章信息页URL.Text = "文章信息页URL：";
		this.textBox_4.Location = new System.Drawing.Point(116, 78);
		this.textBox_4.Name = "textBox_4";
		this.textBox_4.Size = new System.Drawing.Size(100, 21);
		this.textBox_4.TabIndex = 1;
		this.label_4.AutoSize = true;
		this.label_4.Location = new System.Drawing.Point(45, 81);
		this.label_4.Name = "label_4";
		this.label_4.Size = new System.Drawing.Size(65, 12);
		this.label_4.TabIndex = 0;
		this.label_4.Text = "默认大类：";
		this.textBox_5.Location = new System.Drawing.Point(116, 78);
		this.textBox_5.Name = "textBox_5";
		this.textBox_5.Size = new System.Drawing.Size(100, 21);
		this.textBox_5.TabIndex = 1;
		this.label_5.AutoSize = true;
		this.label_5.Location = new System.Drawing.Point(45, 81);
		this.label_5.Name = "label_5";
		this.label_5.Size = new System.Drawing.Size(65, 12);
		this.label_5.TabIndex = 0;
		this.label_5.Text = "默认大类：";
		this.保存.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.保存.Location = new System.Drawing.Point(676, 422);
		this.保存.Name = "保存";
		this.保存.Size = new System.Drawing.Size(75, 23);
		this.保存.TabIndex = 1;
		this.保存.Text = "确定";
		this.保存.UseVisualStyleBackColor = true;
		this.保存.Click += new System.EventHandler(button_0_Click);
		this.取消配置.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.取消配置.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.取消配置.Location = new System.Drawing.Point(757, 422);
		this.取消配置.Name = "取消配置";
		this.取消配置.Size = new System.Drawing.Size(75, 23);
		this.取消配置.TabIndex = 2;
		this.取消配置.Text = "取消";
		this.取消配置.UseVisualStyleBackColor = true;
		this.取消配置.Click += new System.EventHandler(button_1_Click);
		this.label_15.AutoSize = true;
		this.label_15.Location = new System.Drawing.Point(6, 77);
		this.label_15.Name = "label_15";
		this.label_15.Size = new System.Drawing.Size(65, 12);
		this.label_15.TabIndex = 8;
		this.label_15.Text = "根 目 录：";
		this.label_16.AutoSize = true;
		this.label_16.Location = new System.Drawing.Point(6, 50);
		this.label_16.Name = "label_16";
		this.label_16.Size = new System.Drawing.Size(65, 12);
		this.label_16.TabIndex = 7;
		this.label_16.Text = "帐户密码：";
		this.label_17.AutoSize = true;
		this.label_17.Location = new System.Drawing.Point(6, 50);
		this.label_17.Name = "label_17";
		this.label_17.Size = new System.Drawing.Size(0, 12);
		this.label_17.TabIndex = 6;
		this.label_18.AutoSize = true;
		this.label_18.Location = new System.Drawing.Point(6, 23);
		this.label_18.Name = "label_18";
		this.label_18.Size = new System.Drawing.Size(65, 12);
		this.label_18.TabIndex = 5;
		this.label_18.Text = "ＩＰ端口：";
		this.openFileDialog_0.Filter = "水印图片|*.gif";
		this.openFileDialog_0.RestoreDirectory = true;
		this.toolTip_0.AutomaticDelay = 100;
		this.toolTip_0.AutoPopDelay = 50000;
		this.toolTip_0.InitialDelay = 100;
		this.toolTip_0.IsBalloon = true;
		this.toolTip_0.ReshowDelay = 20;
		this.toolTip_0.ShowAlways = true;
		this.toolTip_0.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.toolTip_0.ToolTipTitle = "提示";
		this.基本配置小提示.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.基本配置小提示.AutoSize = true;
		this.基本配置小提示.Location = new System.Drawing.Point(14, 427);
		this.基本配置小提示.Name = "基本配置小提示";
		this.基本配置小提示.Size = new System.Drawing.Size(197, 12);
		this.基本配置小提示.TabIndex = 3;
		this.基本配置小提示.Text = "小提示：鼠标停留会有相关帮助信息";
		this.MailWorker.WorkerReportsProgress = true;
		this.MailWorker.WorkerSupportsCancellation = true;
		this.MailWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(MailWorker_DoWork);
		this.MailWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(MailWorker_RunWorkerCompleted);
		this.backgroundWorker1.WorkerReportsProgress = true;
		this.backgroundWorker1.WorkerSupportsCancellation = true;
		this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		base.CancelButton = this.取消配置;
		base.ClientSize = new System.Drawing.Size(844, 457);
		base.Controls.Add(this.基本配置小提示);
		base.Controls.Add(this.取消配置);
		base.Controls.Add(this.保存);
		base.Controls.Add(this.日志记录);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "ConfigForm";
		this.Text = "系统设置";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ConfigForm_FormClosing);
		base.Load += new System.EventHandler(ConfigForm_Load);
		this.日志记录.ResumeLayout(false);
		this.基本设置.ResumeLayout(false);
		this.基本设置.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.发送间隔box).EndInit();
		((System.ComponentModel.ISupportInitialize)this.http超时).EndInit();
		this.分类对应.ResumeLayout(false);
		this.分类对应.PerformLayout();
		this.高级设置.ResumeLayout(false);
		this.SEO优化.ResumeLayout(false);
		this.SEO优化.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.内容推荐数Box).EndInit();
		((System.ComponentModel.ISupportInitialize)this.目录推荐数Box).EndInit();
		((System.ComponentModel.ISupportInitialize)this.内容邻居数Box).EndInit();
		((System.ComponentModel.ISupportInitialize)this.目录邻居数BOX).EndInit();
		this.防采集及站群设置.ResumeLayout(false);
		this.防采集及站群设置.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.目录页最新章节数Box).EndInit();
		((System.ComponentModel.ISupportInitialize)this.目录页模拟章节数Box).EndInit();
		((System.ComponentModel.ISupportInitialize)this.目录页防采集倒数Box).EndInit();
		this.拼音数字选择.ResumeLayout(false);
		this.拼音数字选择.PerformLayout();
		this.内链设置.ResumeLayout(false);
		this.内链设置.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.内链密度设置Box).EndInit();
		this.超级功能.ResumeLayout(false);
		this.WAP生成设置.ResumeLayout(false);
		this.WAP生成设置.PerformLayout();
		this.推送设置.ResumeLayout(false);
		this.推送设置.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.推送数量Num).EndInit();
		this.标签内链设置.ResumeLayout(false);
		this.标签内链设置.PerformLayout();
		this.图转文设置.ResumeLayout(false);
		this.图转文设置.PerformLayout();
		this.图版转文字设置.ResumeLayout(false);
		this.图版转文字设置.PerformLayout();
		this.生成设置.ResumeLayout(false);
		this.生成设置.PerformLayout();
		this.附加设置.ResumeLayout(false);
		this.空章节自定义.ResumeLayout(false);
		this.空章节自定义.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.章节字数小于Box).EndInit();
		this.电子书设置.ResumeLayout(false);
		this.电子书设置.PerformLayout();
		this.电子书广告.ResumeLayout(false);
		this.电子书广告.PerformLayout();
		this.文字广告.ResumeLayout(false);
		this.文字广告.PerformLayout();
		this.固定位置添加广告.ResumeLayout(false);
		this.固定位置添加广告.PerformLayout();
		this.文字广告集合.ResumeLayout(false);
		this.文字广告集合.PerformLayout();
		this.添加文字广告.ResumeLayout(false);
		this.添加文字广告.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.添加文字广告个数).EndInit();
		this.过滤替换.ResumeLayout(false);
		this.章节内容非法字符替换.ResumeLayout(false);
		this.章节内容非法字符替换.PerformLayout();
		this.违禁小说过滤.ResumeLayout(false);
		this.违禁小说过滤.PerformLayout();
		this.章节内容非法字符过滤.ResumeLayout(false);
		this.章节内容非法字符过滤.PerformLayout();
		this.日志选择.ResumeLayout(false);
		this.日志选择.PerformLayout();
		this.杰奇目录DIY.ResumeLayout(false);
		this.杰奇目录DIY.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void linkLabel_6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		if (folderBrowserDialog_0.ShowDialog() == DialogResult.OK)
		{
			网站硬盘根目录.Text = folderBrowserDialog_0.SelectedPath;
		}
	}

	private void linkLabel_8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		数据库配置地址.Text = "Server=(local);User id=用户名;Pwd=密码;Database=数据库名";
	}

	private void linkLabel_9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		数据库配置地址.Text = "Server=127.0.0.1;Port=3306;Database=数据库名;User ID=用户名;Password=密码;Charset=utf8mb4;SslMode=Preferred;AllowPublicKeyRetrieval=True";
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		if (folderBrowserDialog_0.ShowDialog() == DialogResult.OK)
		{
			模拟章节目录Box.Text = folderBrowserDialog_0.SelectedPath;
		}
	}

	private void MailWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		EmailSendServer emailSendServer = new EmailSendServer
		{
			SenderEmail = Configs.BaseConfig.MailUser,
			SmtpServer = Configs.BaseConfig.MailSmtp,
			SmtpServerAccount = Configs.BaseConfig.MailUser,
			SmtpServerPassword = Configs.BaseConfig.MailPass
		};
		EmailSendServer emailSendServer2 = emailSendServer;
		Random random = new Random();
		string text = SecurityUtil.ComputeMD5(random.Next(0, 99999).ToString()).Substring(8, 5);
		EmailBase emailBase = new EmailBase();
		emailBase.Subject = "【" + Configs.BaseConfig.WebSiteName + "】测试采集报告(" + text + ")";
		emailBase.Content = "这是一封测试邮件，收到此邮件表明你的邮箱可以接收采集器时段报告和错误日志。<br /><br />开发任何网站系统，任何软件，请联系QQ3120979";
		EmailBase emailBase2 = emailBase;
		EmailBase emailBase3 = emailBase2;
		string[] array = Configs.BaseConfig.MailTitle.Trim().Split(',');
		string text2 = "测试结果如下：";
		string[] array2 = array;
		foreach (string text3 in array2)
		{
			if (text3 == string.Empty)
			{
				return;
			}
			emailBase3.ToEmail = text3;
			string text4 = text2;
			text2 = text4 + "\r\n[" + text3 + "]" + emailSendServer2.SendMail(emailBase3);
		}
		MessageBox.Show(text2);
	}

	private void MailWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		测试发信.Enabled = true;
	}

	private void NumOrPinyincomboBox_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (拼音数字选择Box.Text.ToLower() == "数字id目录")
		{
			生成目录样式Box.Items.Clear();
			生成目录样式Box.Items.Add("ID");
			生成目录样式Box.Items.Add("ID除1000/ID");
			生成目录样式Box.Text = "ID除1000/ID";
		}
		else if (拼音数字选择Box.Text.ToLower() == "拼音目录")
		{
			生成目录样式Box.Items.Clear();
			生成目录样式Box.Items.Add("拼音");
			生成目录样式Box.Items.Add("拼音前三字母/拼音");
			生成目录样式Box.Items.Add("拼音首字母+书号");
			生成目录样式Box.Text = "拼音";
		}
	}

	private void SaveImage()
	{
		if (File.Exists(picPath))
		{
			File.Delete(picPath);
		}
		webBrowser.Width = imgWidth;
		webBrowser.Height = imgHeight;
		int xWidth = webBrowser.Width;
		int yHeight = webBrowser.Height;
		try
		{
			Snapshot snapshot = new Snapshot();
			using Bitmap bitmap = snapshot.TakeSnapshot(webBrowser.ActiveXInstance, new Rectangle(0, 0, xWidth, yHeight), xWidth, yHeight);
			bitmap.Save(picPath, ImageFormat.Jpeg);
			bitmap.Dispose();
		}
		catch (Exception)
		{
		}
		picEnd = true;
	}

	private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
	{
		((WebBrowser)sender).Document.Window.Error += Window_Error;
		if (webBrowser.ReadyState == WebBrowserReadyState.Complete && !webBrowser.IsBusy)
		{
			SaveImage();
		}
	}

	private void Window_Error(object sender, HtmlElementErrorEventArgs e)
	{
		e.Handled = true;
	}

	private void writeToConfigsFile()
	{
		Configs.BaseConfig.Indextmp = 首页模板路径.Text;
		Configs.BaseConfig.Infotmp = 信息页模版路径.Text;
		Configs.BaseConfig.Listtmp = 目录模板路径.Text;
		Configs.BaseConfig.Contmp = 章节模板路径.Text;
		Configs.BaseConfig.sqliteTime = 日志保存周期BOX.Text;
		Configs.BaseConfig.NullChapter = 空章节替换内容Box.Text;
		Configs.BaseConfig.OpenNullChapter = 是否启用空章节替换.Checked;
		Configs.BaseConfig.Debug = Debug模式.Checked;
		Configs.BaseConfig.WebSiteName = 网站名称TEXT.Text;
		Configs.BaseConfig.WebSitePath = 网站硬盘根目录.Text;
		Configs.BaseConfig.ConnectionString = DatabaseConnectionProfile.NormalizeConnectionString(数据库配置地址.Text, Configs.BaseConfig.DatabaseServerType, Configs.BaseConfig.DatabaseServerMajorVersion);
		数据库配置地址.Text = Configs.BaseConfig.ConnectionString;
		Configs.BaseConfig.CmsVersion = 小说系统版本号.Text;
		Configs.BaseConfig.CmsName = SupportedCms.NormalizeCmsName(小说系统名.Text);
		Configs.BaseConfig.DonotUserDefaultisboy = 是否使用默认男女频.Checked;
		Configs.BaseConfig.DefaultisboyID = Convert.ToInt32(默认男女频text.Text.Split('|')[0]);
		Configs.BaseConfig.Defaultisboy = 默认男女频text.Text.Split('|')[1];
		Configs.BaseConfig.DonotUserDefaultLagerSort = 是否使用默认大类.Checked;
		Configs.BaseConfig.DefaultLagerSortID = Convert.ToInt32(默认大类Box.Text.Split('|')[0]);
		Configs.BaseConfig.DefaultLagerSort = 默认大类Box.Text.Split('|')[1];
		Configs.BaseConfig.DonotUserDefaultSmallSort = 是否使用默认小类.Checked;
		Configs.BaseConfig.DefaultSmallSortID = Convert.ToInt32(默认小类BOX.Text.Split('|')[0]);
		Configs.BaseConfig.DefaultSmallSort = 默认小类BOX.Text.Split('|')[1];
		Configs.BaseConfig.isboyCorresponding = 默认男女频对应列表.Text.Replace("\r\n", "♂");
		Configs.BaseConfig.LagerSortCorresponding = 大类对应BOX.Text.Replace("\r\n", "♂");
		Configs.BaseConfig.SmallSortCorresponding = 小类对应BOX.Text.Replace("\r\n", "♂");
		Configs.BaseConfig.DefaultVolumeName = 默认分卷名字.Text;
		Configs.BaseConfig.HttpTimeOut = Convert.ToInt32(http超时.Value);
		Configs.BaseConfig.HttpUserAgent = UA.Text;
		Configs.BaseConfig.WebSiteCookies = cookies.Text;
		Configs.BaseConfig.OpenNullChapter = 是否启用空章节替换.Checked;
		Configs.BaseConfig.OpenImageChapter = 是否启用图转文.Checked;
		Configs.BaseConfig.MailSmtp = Smtp服务器.Text;
		Configs.BaseConfig.MailUser = mail名称.Text;
		Configs.BaseConfig.MailPass = mail密码.Text;
		Configs.BaseConfig.MailTitle = 接报告邮箱.Text;
		Configs.BaseConfig.MailTimeNum = Convert.ToInt32(发送间隔box.Value);
		if (Configs.BaseConfig.MailTimeNum < 10)
		{
			Configs.BaseConfig.MailTimeNum = 10;
		}
		Configs.BaseConfig.LogType = 日志格式BOX.SelectedIndex;
		Configs.BaseConfig.Translate = 中译英.Checked;
		Configs.BaseConfig.TextMarkOfVulmeName = 添加文字广告分卷限制.Text.Replace("\r\n", "♂").Split('♂');
		Configs.BaseConfig.TextMarkOfArrayText = 文字广告集合Box.Text.Replace("\r\n", "♂").Split('♂');
		Configs.BaseConfig.TextMarkOfNumber = Convert.ToInt32(添加文字广告个数.Value);
		Configs.BaseConfig.TextMarkOfTop = 章节头部广告Box.Text;
		Configs.BaseConfig.TextMarkOfBottom = 章节尾部广告Box.Text;
		Configs.BaseConfig.TextMarkOfData = 是否添加文字广告.Checked;
		Configs.BaseConfig.TextMarkOfHtml = 是否生成HTML添加文字广告.Checked;
		Configs.BaseConfig.TextMarkOfEBook = 是否电子书添加文字广告.Checked;
		Configs.BaseConfig.IndexHtml = 是否生成目录.Checked;
		Configs.BaseConfig.ChapterHtml = 是否生成内容.Checked;
		Configs.BaseConfig.FullHtml = 是否生成全文.Checked;
		Configs.BaseConfig.CreateOPF = 是否否生成OPF.Checked;
		Configs.BaseConfig.CustomRealTxt = 是否启用章节实际内容.Checked;
		Configs.BaseConfig.CustomImage = 章节图片位置.Checked;
		Configs.BaseConfig.CustomCover = 小说封面位置.Checked;
		Configs.BaseConfig.AddJsRealTxt = 是否启用JS调用真实内容.Checked;
		Configs.BaseConfig.CustomCreatePath = 是否启用自定义生成路径.Checked;
		Configs.BaseConfig.CustomHtmlTemplets = 是否启用自定义HTML模板.Checked;
		Configs.BaseConfig.CreateZIP = 星否生成ZIP.Checked;
		Configs.BaseConfig.CreateTXT = 是否生成TXT.Checked;
		Configs.BaseConfig.CreateUMD = 是否生成UMD.Checked;
		Configs.BaseConfig.CreateJAR = 是否生成JAR.Checked;
		Configs.BaseConfig.CreateCHM = 是否生成CHM.Checked;
		Configs.BaseConfig.IndexHtmlDir = 章节目录硬盘路径.Text;
		Configs.BaseConfig.IndexHtmlUrl = 章节目录URL路径.Text;
		Configs.BaseConfig.ChapterHtmlDir = 内容硬盘路径.Text;
		Configs.BaseConfig.ChapterHtmlUrl = 内容URL路径.Text;
		Configs.BaseConfig.FullHtmlDir = 全文硬盘路径.Text;
		Configs.BaseConfig.FullHtmlUrl = 全文URL路径.Text;
		Configs.BaseConfig.RealTxtDir = 实际章节硬盘路径.Text;
		Configs.BaseConfig.RealTxtUrl = 实际章节内容URL路径.Text;
		Configs.BaseConfig.OpfDir = OPF硬盘路径.Text;
		Configs.BaseConfig.OpfUrl = OPFURL路径.Text;
		Configs.BaseConfig.ImageDir = 章节图片硬盘路径.Text;
		Configs.BaseConfig.ImageUrl = 图片章节URL路径.Text;
		Configs.BaseConfig.CoverDir = 小说封面硬盘路径.Text;
		Configs.BaseConfig.CoverUrl = 封面URL路径.Text;
		Configs.BaseConfig.PrevFirstHtmlUrl = 首页的上一页Box.Text;
		Configs.BaseConfig.NextEndHtmlUrl = 尾页的下一页Box.Text;
		Configs.BaseConfig.ZipDir = ZIP根目录.Text;
		Configs.BaseConfig.ZipUrl = ZIP_URL.Text;
		Configs.BaseConfig.UmdDir = UMD根目录.Text;
		Configs.BaseConfig.UmdUrl = UMD_URL.Text;
		Configs.BaseConfig.JarDir = JAR根目录.Text;
		Configs.BaseConfig.JarUrl = JAR_URL.Text;
		Configs.BaseConfig.ChmDir = CHM根目录.Text;
		Configs.BaseConfig.ChmUrl = CHM_URL.Text;
		Configs.BaseConfig.TxtDir = TXT根目录.Text;
		Configs.BaseConfig.TxtUrl = TXT_URL.Text;
		Configs.BaseConfig.UpdateDefaultUrls = 单次循环后调用页面列表.Text.Replace("\r\n", "♂").Split('♂');
		Configs.BaseConfig.FilterNovelName = 违禁小说过滤Box.Text.Replace("\r\n", "♂");
		Configs.BaseConfig.BadWords = 章节内容非法字符过滤Box.Text;
		Configs.BaseConfig.ReplaceBadWords = 默认替换字符Box.Text;
		Configs.BaseConfig.BadwordsReplaceImages = 章节内容非法字符替换Box.Text.Replace("\r\n", "♂").Split('♂');
		Configs.BaseConfig.EBookHead = 电子书头广告Box.Text;
		Configs.BaseConfig.EBookFoot = 电子书尾广告Box.Text;
		Configs.BaseConfig.MailTimeNum = Convert.ToInt32(发送间隔box.Value);
		Configs.BaseConfig.MailSmtp = Smtp服务器.Text;
		Configs.BaseConfig.MailUser = mail名称.Text;
		Configs.BaseConfig.MailPass = mail密码.Text;
		Configs.BaseConfig.MailTitle = 接报告邮箱.Text;
		string text = ",";
		for (int i = 0; i < 日志记录Box.CheckedItems.Count; i++)
		{
			text = text + 日志记录Box.CheckedItems[i].ToString().Split(' ')[0] + ",";
		}
		Configs.BaseConfig.SelectLog = text;
		Configs.BaseConfig.NumOrPinyin = 拼音数字选择Box.Text;
		Configs.BaseConfig.NumOrPinyinDir = 生成目录样式Box.Text.Replace("ID除1000/ID", "{NovelId/1000}/{NovelId}").Replace("ID", "{NovelId}").Replace("拼音前三字母/拼音", "{Pinyin/3}/{Pinyin}")
			.Replace("拼音", "{Pinyin}")
			.Replace("拼音首字母+书号", "{Pinyinshouid}");
		Configs.BaseConfig.IndexNeighbor = Convert.ToInt32(目录邻居数BOX.Value);
		Configs.BaseConfig.ChapterNeighbor = Convert.ToInt32(内容邻居数Box.Value);
		Configs.BaseConfig.IndexTuijian = Convert.ToInt32(目录推荐数Box.Value);
		Configs.BaseConfig.ChapterTuijian = Convert.ToInt32(内容推荐数Box.Value);
		Configs.BaseConfig.PrevNextPageSuffix = 内容上下页后缀Box.Text;
		Configs.BaseConfig.ChapterName2Num = 是否启用章节名数字化.Checked;
		Configs.BaseConfig.IndexAntiCollectNum = Convert.ToInt32(目录页防采集倒数Box.Value);
		Configs.BaseConfig.ZhanQun = 是否开启站群.Checked;
		Configs.BaseConfig.ChapterPagingNum = Convert.ToInt32(目录页最新章节数Box.Value);
		Configs.BaseConfig.SizeNullChapter = Convert.ToInt32(章节字数小于Box.Value);
		Configs.BaseConfig.InternalLinkUrl = 排行榜地址Box.Text;
		Configs.BaseConfig.InternalLinkDensity = Convert.ToInt32(内链密度设置Box.Value);
		Configs.BaseConfig.InternalLink = 是否启用内链.Checked;
		Configs.BaseConfig.InternalLinkHead = 前推荐词Box.Text;
		Configs.BaseConfig.InternalLinkFoot = 后长尾词Box.Text;
		Configs.BaseConfig.TuijianType = 推荐榜获取形式Box.Text;
		Configs.BaseConfig.TuijianTemplates = 内链接模版Box.Text;
		Configs.BaseConfig.NewAntiCollectNum = Convert.ToInt32(目录页最新章节数Box.Value);
		Configs.BaseConfig.OnAntiCollectNum = Convert.ToInt32(目录页模拟章节数Box.Value);
		Configs.BaseConfig.OnAntiCollectDir = 模拟章节目录Box.Text;
		Configs.BaseConfig.InnerTagLink = 是否启用标签内链.Checked;
		Configs.BaseConfig.InnerTagLinkUrl1 = 标签内链地址Box.Text;
		Configs.BaseConfig.IsEnableBaiduPush = 是否启用百度推送.Checked;
		Configs.BaseConfig.StrBaiduPushDomain = PC域名Box.Text;
		Configs.BaseConfig.StrBaiduPushToken = PCTokenBox.Text;
		Configs.BaseConfig.StrBaiduPushURL = 推送URLBox.Text;
		Configs.BaseConfig.StrBaiduPushType = 推送类型Box.Text;
		Configs.BaseConfig.IntBaiduPushNum = Convert.ToInt32(推送数量Num.Value);
		Configs.BaseConfig.IsEnableWapGen = 是否生成WAP页面.Checked;
		Configs.BaseConfig.StrWapDomain = WAP域名Box.Text;
		Configs.BaseConfig.StrWapIndexTemplate = WAP目录模板Box.Text;
		Configs.BaseConfig.StrWapChapterTemplate = WAP内容模板Box.Text;
		Configs.BaseConfig.StrWapHtmlDir = WAP根目录Box.Text;
		Configs.BaseConfig.CmsEncoding = 编码Box.Text;
		ConfigFileManager.SaveConfig("BaseConfig.xml", Configs.BaseConfig);
	}
}
