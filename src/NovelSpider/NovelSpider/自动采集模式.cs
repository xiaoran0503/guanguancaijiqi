using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;
using NovelSpider.Local;
using NovelSpider.Target;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class 自动采集模式 : DockContent
{
	private static T WaitForBackgroundAsync<T>(System.Threading.Tasks.Task<T> task)
	{
		try
		{
			task.Wait();
			return task.Result;
		}
		catch (AggregateException ex) when (ex.InnerExceptions.Count == 1)
		{
			System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
			throw;
		}
	}

	private static void WaitForBackgroundAsync(System.Threading.Tasks.Task task)
	{
		try
		{
			task.Wait();
		}
		catch (AggregateException ex) when (ex.InnerExceptions.Count == 1)
		{
			System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
			throw;
		}
	}

	private bool WaitOrCancel(int milliseconds)
	{
		int remaining = Math.Max(0, milliseconds);
		while (remaining > 0)
		{
			if (AutoWorker.CancellationPending)
			{
				return false;
			}
			int slice = Math.Min(remaining, 200);
			Thread.Sleep(slice);
			remaining -= slice;
		}
		return !AutoWorker.CancellationPending;
	}

	private void WaitForAutoWorker()
	{
		using ManualResetEventSlim completed = new ManualResetEventSlim(!AutoWorker.IsBusy);
		RunWorkerCompletedEventHandler handler = null;
		handler = (_, _) => completed.Set();
		AutoWorker.RunWorkerCompleted += handler;
		try
		{
			while (AutoWorker.IsBusy && !completed.Wait(200))
			{
			}
		}
		finally
		{
			AutoWorker.RunWorkerCompleted -= handler;
		}
	}
	public BackgroundWorker AutoWorker;

	private bool bool_0;

	private Button 开始;

	private Button 停止;

	private Button 测试网速;

	private Button 采集方案_1;

	private Button button1;

	private CheckBox 添加新书_0;

	private CheckBox 更新连载_1;

	private CheckBox 调用页面_10;

	private CheckBox 全本必采_15;

	private CheckBox 内容自动排版_16;

	private CheckBox 识别方式_17;

	private CheckBox 强制清空_18;

	private CheckBox 循环采集_2;

	private CheckBox checkBox_20;

	private CheckBox 添加分卷判断_21;

	private CheckBox 栓测重复章节_22;

	private CheckBox 日志记录_3;

	private CheckBox 禁止添加分卷_4;

	private CheckBox 下载图片章节_5;

	private CheckBox 清空重采_6;

	private CheckBox 不处理完结_8;

	private CheckBox 只采文字章_9;

	private CheckBox chkEnableProxy;

	private ColumnHeader columnHeader_12;

	private ColumnHeader columnHeader_13;

	private ColumnHeader columnHeader1;

	private ColumnHeader columnHeader2;

	private ColumnHeader columnHeader3;

	private ColumnHeader columnHeader4;

	private ComboBox 采集规则_0;

	private ComboBox comboBox_1;

	private ComboBox comboBox_2;

	private ComboBox 采集方案_4;

	private ComboBox comboBox_5;

	private ComboBox comboBox_6;

	private ComboBox comboBox_7;

	private ComboBox comboBox1;

	private IContainer components;

	private DateTime dateTime_0;

	private CheckBox DelForHtml;

	private CheckBox DelForTxt;

	private NumericUpDown DonnotCollectLastChapterNum;

	private CheckBox DuanImage;

	private CheckBox DuanImageCheck;

	private TextBox FilterChapterNameBox;

	private TextBox FilterChapterNameBox1;

	private TextBox FilterNovelTextBox;

	private ComboBox FilterNovelType;

	private TextBox FilterVolumeTextBox;

	private TextBox FilterVolumeTextBox1;

	private FontDialog fontDialog_0;

	private CheckBox forceReplace;

	private GroupBox groupBox_2;

	private GroupBox groupBox_3;

	private GroupBox groupBox_4;

	private GroupBox 设置2_5;

	private GroupBox 设置_6;

	private GroupBox groupBox_7;

	private GroupBox groupBox_9;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private GroupBox groupBox3;

	private GroupBox groupBox4;

	private GroupBox groupBox5;

	private GroupBox groupBox6;

	private GroupBox groupBox7;

	private GroupBox groupBox8;

	private CheckBox 隐藏更新小说;

	public BackgroundWorker HttpWorker;

	private IContainer icontainer_0;

	private CheckBox isChkMD5;

	private Label 循环间隔时间_0;

	private Label 采集规则_1;

	private Label label_10;

	private Label label_11;

	private Label label_12;

	private Label label_13;

	private Label label_14;

	private Label label_15;

	private Label 注意得示_16;

	private Label label_17;

	private Label label_18;

	private Label label_19;

	private Label label_2;

	private Label 采集方案_20;

	private Label label_21;

	private Label label_22;

	private Label label_23;

	private Label label_24;

	private Label label_25;

	private Label label_26;

	private Label label_27;

	private Label label_28;

	private Label label_29;

	private Label label_3;

	private Label 不采小于字符前_30;

	private Label label_31;

	private Label label_32;

	private Label 不采小于章节后段_4;

	private Label 不采集小于章节_5;

	private Label label_6;

	private Label label_7;

	private Label label_8;

	private Label label_9;

	private Label label1;

	private Label label10;

	private Label label11;

	private Label label12;

	private Label label13;

	private Label label14;

	private Label label15;

	private Label label16;

	private Label label17;

	private Label label18;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label label9;

	private ListView listView1;

	public BackgroundWorker LoginWorker;

	private NumericUpDown 循环间隔时间选择_0;

	private NumericUpDown numericUpDown_1;

	private NumericUpDown 不采小于章节设置_2;

	private NumericUpDown numericUpDown_3;

	private NumericUpDown numericUpDown_4;

	private NumericUpDown numericUpDown_5;
	private GroupBox requestScheduleGroup;

	private NumericUpDown requestListWaitMinBox;

	private NumericUpDown requestListWaitMaxBox;

	private NumericUpDown requestNovelWaitMinBox;

	private NumericUpDown requestNovelWaitMaxBox;

	private NumericUpDown requestIndexWaitMinBox;

	private NumericUpDown requestIndexWaitMaxBox;

	private NumericUpDown requestChapterWaitMinBox;

	private NumericUpDown requestChapterWaitMaxBox;

	private NumericUpDown sameHostConcurrencyBox;

	private ComboBox userAgentModeBox;

	private CheckBox requestBackoffBox;

	private NumericUpDown 不采小于字符设置_6;

	private CheckBox OnlyReplaceSort;

	private OpenFileDialog openFileDialog_0;

	private ProgressBar progressBar_0;

	private ProgressBar progressBar_1;

	private RadioButton 采集最新列表_0;

	private RadioButton 目标站自定义书号_1;

	private RadioButton 目标站书号_2;

	private RadioButton 本站自定义书号_3;

	private RadioButton 本站书号_4;

	private RadioButton 其他站列表搜索_5;

	private CheckBox ReplaceChapter;

	private NumericUpDown ReplaceChapterNameNun;

	private NumericUpDown ReplaceChapterNun;

	private CheckBox ReplaceFullflag;

	private CheckBox ReplaceImgflag;

	private CheckBox ReplaceIntro;

	private CheckBox ReplaceSort;

	private NumericUpDown ReplaceSortId;

	public RuleConfigInfo rInfo;

	private SaveFileDialog saveFileDialog_0;

	private ContextMenuStrip SaveMenuStrip;

	private string string_0;

	private string string_1;

	private string string_2;

	private CheckBox StrongReplaceFullflag;

	private CheckBox StrongReplaceImgflag;

	private CheckBox StrongReplaceIntro;

	private TabControl tabControl_0;

	private TabPage 采集模式_0;

	private TabPage 采集动作_1;

	private TabPage 过滤设置_2;

	private TabPage 采集进度_4;

	private TabPage 高级设置1;

	private TabPage 代理设定2;

	private ContextMenuStrip TargetMenuStrip;

	public BackgroundWorker TestWorker;

	private TextBox 本站自定义书号ID_0;

	private TextBox 本站书号结束_1;

	private TextBox textBox_10;

	private TextBox textBox_11;

	private TextBox textBox_12;

	private TextBox textBox_14;

	private TextBox 提取其他站小说名规则_16;

	private TextBox 其他站列表URL_17;

	private TextBox 其他站编码_18;

	private TextBox textBox_19;

	private TextBox 本站书号开始_2;

	private TextBox 采集最新列表框_3;

	private TextBox 目标站自定义书号ID_4;

	private TextBox 目标站书号结束_5;

	private TextBox 目标站书号开始_6;

	private TextBox textBox_9;

	private System.Windows.Forms.Timer timer_0;

	public TaskConfigInfo tInfo;

	private ToolStripMenuItem toolStripMenuItem_0;

	private ToolStripMenuItem toolStripMenuItem_1;

	private ToolStripMenuItem toolStripMenuItem_11;

	private ToolStripMenuItem toolStripMenuItem_2;

	private ToolStripMenuItem toolStripMenuItem_22;

	private ToolStripMenuItem toolStripMenuItem_23;

	private ToolStripMenuItem toolStripMenuItem_25;

	private ToolStripMenuItem toolStripMenuItem_3;

	private ToolStripMenuItem toolStripMenuItem_6;

	private ToolStripMenuItem toolStripMenuItem_8;

	private ToolStripMenuItem toolStripMenuItem_9;

	private ToolStripSeparator toolStripSeparator_0;

	private ToolStripSeparator toolStripSeparator_2;

	private ToolStripSeparator toolStripSeparator_3;

	private NumericUpDown ReplaceChapterTimeMax;

	private Label label19;

	private NumericUpDown ReplaceChapterTimeMin;

	private CheckBox 索引对比失败只修复;

	private CheckBox 索引对比判断修复;

	private Label label20;

	private ToolTip toolTip_0;

	public 自动采集模式()
	{
		rInfo = new RuleConfigInfo();
		tInfo = new TaskConfigInfo();
		dateTime_0 = DateTime.Now;
		string_0 = "";
		string_1 = "";
		string_2 = "";
		InitializeComponent();
	}

	public 自动采集模式(bool bool_1)
	{
		rInfo = new RuleConfigInfo();
		tInfo = new TaskConfigInfo();
		dateTime_0 = DateTime.Now;
		string_0 = "";
		string_1 = "";
		string_2 = "";
		InitializeComponent();
		bool_0 = bool_1;
	}

	private void AutoWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		AutoWorker.ReportProgress(2, "获得小说列表");
		switch (tInfo.RadioBy)
		{
		case "GetListUrl":
		{
			string[] ids = new Page(rInfo, tInfo).GetIds(tInfo.GetListUrl);
			AutoWorker.ReportProgress(5, ids.Length);
			method_1(ids, bool_1: true);
			break;
		}
		case "GetOrderId":
			AutoWorker.ReportProgress(5, tInfo.GetOrderMaxId - tInfo.GetOrderMinId);
			method_2(tInfo.GetOrderMinId, tInfo.GetOrderMaxId, bool_1: true);
			break;
		case "GetSinceId":
			AutoWorker.ReportProgress(5, tInfo.GetSinceId.Length);
			method_1(tInfo.GetSinceId, bool_1: true);
			break;
		case "PutOrderId":
			AutoWorker.ReportProgress(5, tInfo.PutOrderMaxId - tInfo.PutOrderMinId);
			method_2(tInfo.PutOrderMinId, tInfo.PutOrderMaxId, bool_1: false);
			break;
		case "OtherListUrl":
		{
			string[] novelList = Page.GetNovelList(tInfo.OtherListUrl, tInfo.OtherRegex, tInfo.OtherEncoding);
			AutoWorker.ReportProgress(5, tInfo.PutSinceId.Length);
			method_0(novelList);
			break;
		}
		case "PutSinceId":
			AutoWorker.ReportProgress(5, tInfo.PutSinceId.Length);
			method_1(tInfo.PutSinceId, bool_1: false);
			break;
		}
		if (AutoWorker.CancellationPending)
		{
			e.Cancel = true;
		}
		else
		{
			if (!tInfo.UpdateDefault)
			{
				return;
			}
			string[] updateDefaultUrls = Configs.BaseConfig.UpdateDefaultUrls;
			foreach (string text in updateDefaultUrls)
			{
				if (string.IsNullOrEmpty(text))
				{
					break;
				}
				HttpClient httpClient = new HttpClient
				{
					UriString = text
				};
				HttpClient httpClient2 = httpClient;
				AutoWorker.ReportProgress(2, "执行" + text);
				httpClient2.GetStringWork();
			}
		}
	}

	private void AutoWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
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
			label_15.Text = e.UserState.ToString();
			break;
		case 1:
			label_14.Text = e.UserState.ToString();
			break;
		case 2:
			label_13.Text = e.UserState.ToString();
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

	private void AutoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
					label_13.Text = "错误提示：" + e.Error.Message;
					SpiderException.Show(0, e.Error.Message, null, tInfo.Log, string_0, tInfo.RuleFile);
					timer_0.Start();
					dateTime_0 = DateTime.Now.AddMinutes(tInfo.Interval);
				}
				else
				{
					ShowErrorMessage(e.Error.Message);
					开始.Enabled = true;
					停止.Enabled = false;
				}
			}
			else
			{
				ShowErrorMessage(e.Error.Message);
				开始.Enabled = true;
				停止.Enabled = false;
			}
		}
		else if (e.Cancelled)
		{
			label_13.Text = "操作取消";
			开始.Enabled = true;
			停止.Enabled = false;
		}
		else if (tInfo.Timing)
		{
			timer_0.Start();
			dateTime_0 = DateTime.Now.AddMinutes(tInfo.Interval);
		}
		else
		{
			label_13.Text = "操作完成";
			开始.Enabled = true;
			停止.Enabled = false;
		}
		label7.Text = "-";
		Configs.TaskNovelInfo[string_2.ToString()] = null;
	}

	private void btnImportProxylistClick(object sender, EventArgs e)
	{
		OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = "txt文件|*.txt"
		};
		OpenFileDialog openFileDialog2 = openFileDialog;
		if (openFileDialog2.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		string[] array = File.ReadAllLines(openFileDialog2.FileName);
		if (array.Length == 0)
		{
			return;
		}
		ListViewItem[] array2 = new ListViewItem[array.Length];
		int num = 0;
		string[] array3 = array;
		foreach (string text in array3)
		{
			string text2 = text.Trim();
			if (string.IsNullOrEmpty(text2))
			{
				continue;
			}
			string[] array4 = text2.Split(':');
			if (array4.Length == 1)
			{
				if (SecurityUtil.IsIP(text2))
				{
					array2[num] = new ListViewItem(text2);
					array2[num].SubItems.Add("80");
					array2[num].SubItems.Add("未知");
					array2[num].SubItems.Add("-");
					array2[num].SubItems.Add("-");
				}
			}
			else if (SecurityUtil.IsIP(array4[0].Trim()) && SecurityUtil.IsNum(array4[1].Trim()))
			{
				array2[num] = new ListViewItem(array4[0].Trim());
				array2[num].SubItems.Add(array4[1].Trim());
				array2[num].SubItems.Add("未知");
				array2[num].SubItems.Add("-");
				array2[num].SubItems.Add("-");
			}
			num++;
		}
		listView1.Items.Clear();
		ProgressiveListViewLoader.ReplaceItems(listView1, array2);
	}

	private void btnSaveConfigClick(object sender, EventArgs e)
	{
		if (bool_0)
		{
			method_5();
			ConfigFileManager.SaveConfig("TaskConfig.xml", tInfo);
			base.DialogResult = DialogResult.OK;
		}
		else
		{
			SaveMenuStrip.Show(采集方案_1, 0, 23);
		}
	}

	private void btnStartClick(object sender, EventArgs e)
	{
		if (true) /* LicenseOk always true */
		{
			label6.Text = DateTime.Now.ToString();
			rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(采集规则_0.Text, rInfo);
			method_5();
			if (tInfo.Proxy && tInfo.ProxyHost.Trim() != string.Empty)
			{
				label7.Text = tInfo.ProxyHost.Trim() + ":" + tInfo.ProxyPort;
			}
			tInfo.ID = string_2;
			开始.Enabled = false;
			停止.Enabled = true;
			测试网速.Enabled = false;
			tabControl_0.SelectedIndex = tabControl_0.TabCount - 2;
			dateTime_0 = DateTime.Now.AddMinutes(tInfo.Interval);
			if (!AutoWorker.IsBusy)
			{
				AutoWorker.RunWorkerAsync();
			}
		}
	}

	private void btnStopClick(object sender, EventArgs e)
	{
		停止.Enabled = false;
		if (AutoWorker.IsBusy)
		{
			AutoWorker.CancelAsync();
			return;
		}
		if (timer_0.Enabled)
		{
			timer_0.Stop();
			label_13.Text = "操作取消";
		}
		开始.Enabled = true;
		测试网速.Enabled = true;
	}

	private void btnTestNetworkClick(object sender, EventArgs e)
	{
		label_13.Text = "正在网络测速...";
		rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(采集规则_0.Text, rInfo);
		method_5();
		tInfo.ID = string_2;
		开始.Enabled = false;
		测试网速.Enabled = false;
		停止.Enabled = false;
		tabControl_0.SelectedIndex = tabControl_0.TabCount - 2;
		if (!HttpWorker.IsBusy)
		{
			HttpWorker.RunWorkerAsync();
		}
	}

	public static bool CheckHost()
	{
		return true;
	}

	private void CollectAuto_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (AutoWorker.IsBusy)
		{
			AutoWorker.CancelAsync();
			e.Cancel = true;
			MessageBox.Show("检查到采集进程正在运行，现在正在自动停止采集进程\n\n请在采集进程停止后关闭窗口！", "信息提示");
		}
		else
		{
			Configs.TaskNovelInfo.Remove(string_2);
		}
	}

	private void CollectAuto_Load(object sender, EventArgs e)
	{
		注意得示_16.Text = "通知：" + Configs.BaseConfig.LicenseAd;
		string_2 = Guid.NewGuid().ToString().ToUpper();
		label11.Text = string_2;
		if (!LoginWorker.IsBusy)
		{
			LoginWorker.RunWorkerAsync();
		}
		Configs.TaskNovelInfo.Add(string_2, null);
		string text = Text + " " + string_2;
		Text = text;
		tInfo = (TaskConfigInfo)ConfigFileManager.LoadConfig("TaskConfig.xml", tInfo);
		采集规则_0.BeginUpdate();
		string[] array = IO.LoadRules();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				采集规则_0.Items.Add(array[i]);
				if (array[i] == tInfo.RuleFile)
				{
					采集规则_0.Text = tInfo.RuleFile;
					rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(tInfo.RuleFile, rInfo);
					采集最新列表框_3.Text = rInfo.NovelListUrl.Pattern;
					if (!bool_0)
					{
						Text = rInfo.GetSiteName.Pattern + " 标准采集模式";
					}
				}
			}
		}
		采集规则_0.EndUpdate();
		采集方案_4.BeginUpdate();
		采集方案_4.Items.Add("TaskConfig.xml");
		采集方案_4.Text = "TaskConfig.xml";
		string[] array2 = IO.LoadTasks();
		if (array2.Length != 0)
		{
			for (int j = 0; j < array2.Length; j++)
			{
				采集方案_4.Items.Add(array2[j]);
			}
		}
		采集方案_4.EndUpdate();
		method_4();
		if (bool_0)
		{
			采集方案_4.Enabled = false;
			注意得示_16.Visible = false;
			开始.Visible = false;
			停止.Visible = false;
			Text = "设置手动方案";
			采集方案_1.Text = "保存";
		}
	}

	private void CollectNovel_old(NovelInfo novelInfo_0)
	{
		try
		{
			NativeMethods.BookCount++;
			if (novelInfo_0.GetID == null)
			{
				novelInfo_0.GetID = "0";
			}
			if (novelInfo_0.Name == null)
			{
				novelInfo_0.Name = "";
			}
			AutoWorker.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
			AutoWorker.ReportProgress(1, "--");
			AutoWorker.ReportProgress(2, "获得小说信息");
			AutoWorker.ReportProgress(4, 0);
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
					AutoWorker.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
					novelInfo_0 = page.GetNovelInfo(novelInfo_0);
				}
			}
			catch (Exception ex)
			{
				WaitOrCancel(500);
				SpiderException.Show(200, ex.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
				return;
			}
			if (AutoWorker.CancellationPending)
			{
				return;
			}
			ApplyFriendlyDelay(tInfo, RequestKind.Novel);
			try
			{
				var keys = Configs.TaskNovelInfo.Keys;
				if (Configs.TaskNovelInfo.Count != 0)
				{
					foreach (string item in keys)
					{
						if (item != string_2 && Configs.TaskNovelInfo[item] != null && ((NovelInfo)Configs.TaskNovelInfo[item]).Name == novelInfo_0.Name && 0 == 0)
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
			AutoWorker.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
			SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 过滤小说");
			if (tInfo.FilterNovelType != 1)
			{
				if (tInfo.FilterNovelType == 2)
				{
					bool flag = false;
					if (tInfo.FilterNovel != null)
					{
						string[] filterNovel = tInfo.FilterNovel;
						foreach (string text2 in filterNovel)
						{
							if (novelInfo_0.Name.Trim() == text2.Trim())
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
			}
			else if (tInfo.FilterNovel != null)
			{
				string[] filterNovel2 = tInfo.FilterNovel;
				foreach (string text3 in filterNovel2)
				{
					if (novelInfo_0.Name.Trim() == text3.Trim())
					{
						SpiderException.Show(134, "限制小说_黑名单", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						return;
					}
				}
			}
			SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 获得小说的章节目录");
			int num = -1;
			bool flag2 = false;
			ChapterInfo[] chapterList;
			try
			{
				AutoWorker.ReportProgress(2, "获得小说的章节目录");
				chapterList = page.GetChapterList(novelInfo_0);
			}
			catch (Exception ex2)
			{
				SpiderException.Show(210, ex2.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
				return;
			}
			if (chapterList == null || chapterList.Length == 0)
			{
				SpiderException.Show(214, "章节组为空", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
				return;
			}
			AutoWorker.ReportProgress(6, chapterList.Length);
			if (AutoWorker.CancellationPending)
			{
				return;
			}
			if (novelInfo_0.PutID == 0 && novelInfo_0.IsNew)
			{
				AutoWorker.ReportProgress(2, "处理新书");
				if (!tInfo.Finish || novelInfo_0.Degree != 1 || !tInfo.NewBook)
				{
					SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 是否添加新书判断");
					if (!tInfo.NewBook)
					{
						SpiderException.Show(125, "设置不添加新书", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						return;
					}
					SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 章节数量限制判断");
					if (chapterList.Length < tInfo.MinChapterNum && tInfo.MinChapterNum != 0)
					{
						SpiderException.Show(131, "章节数量小于限制", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						return;
					}
				}
				SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 正式入库小说信息");
				novelInfo_0 = WaitForBackgroundAsync(LocalProviderAsyncDispatcher.InsertNovelAsync(LocalProvider.GetInstance(), novelInfo_0));
				flag2 = true;
				AutoWorker.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
			}
			if (AutoWorker.CancellationPending)
			{
				return;
			}
			if (novelInfo_0.LastChapter.PutID == 0)
			{
				flag2 = true;
			}
			if (tInfo.ReplaceFullflag || tInfo.ReplaceImgflag || tInfo.ReplaceIntro || tInfo.ReplaceSort)
			{
				if (tInfo.ReplaceImgflag)
				{
					if (!tInfo.StrongReplaceImgflag && novelInfo_0.ImgFlag == 1)
					{
						tInfo.ReplaceImgflag = false;
					}
					else
					{
						try
						{
							if (novelInfo_0.Cover.Height > 5)
							{
								tInfo.ReplaceImgflag = true;
							}
							else
							{
								tInfo.ReplaceImgflag = false;
							}
						}
						catch (Exception)
						{
							tInfo.ReplaceImgflag = false;
						}
					}
				}
				if (tInfo.ReplaceFullflag)
				{
					if (novelInfo_0.MDegree == 0 && !tInfo.StrongReplaceIntro)
					{
						tInfo.ReplaceIntro = false;
					}
					else
					{
						tInfo.ReplaceIntro = true;
					}
				}
				if (tInfo.ReplaceIntro)
				{
					if (novelInfo_0.Intro == "" && novelInfo_0.MIntro != "" && !tInfo.StrongReplaceFullflag)
					{
						tInfo.ReplaceFullflag = false;
					}
					else
					{
						tInfo.ReplaceFullflag = true;
					}
				}
				if (novelInfo_0.MLagerSortID != novelInfo_0.LagerSortID && tInfo.ReplaceSort && novelInfo_0.MLagerSortID != 0)
				{
					if (tInfo.OnlyReplaceSort)
					{
						if (novelInfo_0.LagerSortID != tInfo.ReplaceSortId && novelInfo_0.LagerSortID != 0)
						{
							tInfo.ReplaceSort = false;
						}
						else
						{
							tInfo.ReplaceSort = true;
						}
					}
				}
				else
				{
					tInfo.ReplaceSort = false;
				}
				if (tInfo.ReplaceImgflag || tInfo.ReplaceFullflag || tInfo.ReplaceIntro || tInfo.ReplaceSort)
				{
					LocalProvider.GetInstance().UpdateNovel(novelInfo_0, bool_0: false, tInfo.ReplaceIntro, tInfo.ReplaceFullflag, tInfo.ReplaceSort, tInfo.ReplaceSort, tInfo.ReplaceImgflag, bool_6: false);
				}
			}
			AutoWorker.ReportProgress(2, "判断是否开启超级模式");
			ChapterInfo[] chapterList2 = LocalProvider.GetInstance().GetChapterList(novelInfo_0.PutID);
			bool flag3 = false;
			novelInfo_0.LastChapter = new ChapterInfo();
			int num2 = 0;
			if (chapterList2.Length != 0)
			{
				num2 = chapterList2.Length - 1;
				if (tInfo.ReplaceChapter)
				{
					if (!tInfo.ReplaceChapterIndex)
					{
						num2 = chapterList2.Length - 1 - tInfo.ReplaceChapterNun;
						if (num2 < 0)
						{
							num2 = 0;
						}
					}
					else
					{
						if (novelInfo_0.MDegree > 0)
						{
							AutoWorker.ReportProgress(2, "超级修复，书籍已经完结，跳过本书。");
							WaitOrCancel(1000);
							return;
						}
						bool flag4 = false;
						for (int k = 0; k < chapterList2.Length; k++)
						{
							if (chapterList2[k].ChapterName != chapterList2[k].ChapterName)
							{
								num2 = k - 1;
								flag4 = true;
								if (num2 >= 0)
								{
									break;
								}
								AutoWorker.ReportProgress(2, "超级修复，书籍第一章节对比失败，跳过本书。");
								WaitOrCancel(1000);
								return;
							}
							num2 = chapterList2.Length - 1;
						}
						if (flag4 && tInfo.ReplaceChapterTime)
						{
							DateTime time = FormatText.GetTime(novelInfo_0.LastupDate);
							DateTime dateTime = DateTime.Now.AddDays(-tInfo.ReplaceChapterTimeMin);
							if (time <= DateTime.Now.AddDays(-tInfo.ReplaceChapterTimeMax) || !(time < dateTime))
							{
								num2 = chapterList2.Length - 1;
								flag2 = false;
							}
						}
					}
				}
				if (!flag2)
				{
					novelInfo_0.LastChapter = chapterList2[num2];
				}
			}
			int num3 = 0;
			bool flag5 = false;
			bool flag6 = false;
			if (tInfo.DuanImage && (chapterList.Length < chapterList2.Length || flag3))
			{
				flag5 = true;
				flag6 = true;
				num3 = chapterList2.Length - chapterList.Length;
				tInfo.ReplaceChapter = true;
				tInfo.ReplaceChapterNun = chapterList.Length;
			}
			int num4 = 0;
			bool flag7 = false;
			if (forceReplace.Checked)
			{
				flag7 = true;
			}
			while (true)
			{
				if (chapterList2.Length != 0)
				{
					num4 = chapterList2.Length - 1;
					if (tInfo.ReplaceChapter && flag7)
					{
						num4 = chapterList2.Length - 1 - tInfo.ReplaceChapterNun;
						if (num4 < 0)
						{
							num4 = 0;
						}
					}
					novelInfo_0.LastChapter = chapterList2[num4];
				}
				AutoWorker.ReportProgress(2, "对比最新章节开始");
				SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 对比最新章节开始");
				if (flag2)
				{
					break;
				}
				AutoWorker.ReportProgress(2, "正在对比最新章节");
				int num5 = 0;
				int num6 = 0;
				while (true)
				{
					if (AutoWorker.CancellationPending)
					{
						return;
					}
					if (num6 >= chapterList.Length - tInfo.DonnotCollectLastChapterNo)
					{
						break;
					}
					if (!tInfo.NoPBar)
					{
						AutoWorker.ReportProgress(1, chapterList[num6].ChapterName);
						AutoWorker.ReportProgress(4, num6 + 1);
						ApplyFriendlyDelay(tInfo, RequestKind.Chapter);
					}
					switch (tInfo.EqualsChapter)
					{
					case 0:
						if (chapterList[num6].ChapterName == novelInfo_0.LastChapter.ChapterName)
						{
							num = num6;
							flag2 = true;
						}
						break;
					case 1:
						if (chapterList[num6].ChapterName == novelInfo_0.LastChapter.ChapterName && chapterList[num6].VolumeName == novelInfo_0.LastChapter.VolumeName)
						{
							num = num6;
							flag2 = true;
						}
						break;
					case 2:
						if (FormatText.CompareToChapter(chapterList[num6].ChapterName, novelInfo_0.LastChapter.ChapterName))
						{
							num = num6;
							flag2 = true;
						}
						break;
					case 3:
					{
						int num8 = FormatText.CompareToChapter2(chapterList[num6].ChapterName, novelInfo_0.LastChapter.ChapterName, chapterList[num6].VolumeName, novelInfo_0.LastChapter.VolumeName);
						if (num8 > 3 && num8 >= num5)
						{
							num5 = num8;
							num = num6;
							flag2 = true;
						}
						break;
					}
					case 4:
					{
						int num9 = FormatText.CompareToChapter3(chapterList[num6].ChapterName, novelInfo_0.LastChapter.ChapterName, chapterList[num6].VolumeName, novelInfo_0.LastChapter.VolumeName);
						if (num9 > 0 && num9 >= num5)
						{
							num5 = num9;
							num = num6;
							flag2 = true;
						}
						break;
					}
					case 5:
					{
						int num7 = FormatText.CompareToChapter4(chapterList[num6].ChapterName, num6, novelInfo_0.LastChapter.ChapterName, novelInfo_0.Chapters, chapterList[num6].VolumeName, novelInfo_0.LastChapter.VolumeName);
						if (num7 > 0 && num7 >= num5)
						{
							num5 = num7;
							num = num6;
							flag2 = true;
						}
						break;
					}
					}
					num6++;
				}
				if (flag2 || !tInfo.ReplaceChapter || flag7)
				{
					break;
				}
				flag7 = true;
			}
			if (AutoWorker.CancellationPending)
			{
				return;
			}
			if (!flag2 && tInfo.DuanImage)
			{
				AutoWorker.ReportProgress(2, "章节对比失败，启用超级更新模式");
				flag5 = true;
				flag6 = true;
				num4 = -1;
				num3 = chapterList2.Length - chapterList.Length;
				if (num3 < 0)
				{
					num3 = 0;
				}
				tInfo.ReplaceChapter = true;
				tInfo.ReplaceChapterNun = chapterList2.Length;
			}
			else if (!flag2)
			{
				if (!tInfo.DeleteChapter)
				{
					SpiderException.Show(120, novelInfo_0.LastChapter.VolumeName + " | " + novelInfo_0.LastChapter.ChapterName, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
					return;
				}
				AutoWorker.ReportProgress(2, "正在清空章节");
				LocalProvider.GetInstance().ClearNovel(novelInfo_0);
				flag2 = true;
			}
			else if (tInfo.CompulsoryDeleteChapter)
			{
				AutoWorker.ReportProgress(2, "正在强制清空章节");
				LocalProvider.GetInstance().ClearNovel(novelInfo_0);
				flag2 = true;
				num = -1;
			}
			if (chapterList.Length - num > tInfo.FindMaxChapterNum)
			{
				SpiderException.Show(132, "对比最新章节成功！但需要采集到章节数超限。", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
			}
			else
			{
				if (!flag5 && (AutoWorker.CancellationPending || !tInfo.OldBook || !flag2))
				{
					return;
				}
				ApplyFriendlyDelay(tInfo, RequestKind.Index);
				bool flag8 = false;
				int num10 = num + 1;
				int num11 = 0;
				int num12 = 0;
				while (true)
				{
					if (tInfo.DonnotCollectLastChapterNo > 0)
					{
					}
					if (num10 >= chapterList.Length - tInfo.DonnotCollectLastChapterNo || AutoWorker.CancellationPending)
					{
						break;
					}
					num11++;
					num12 = num4 + num11;
					if (num12 < chapterList2.Length)
					{
						chapterList[num10].PutID = chapterList2[num12].PutID;
					}
					if (tInfo.FilterVolume != null && tInfo.FilterVolume[0].Replace(" ", "") != "")
					{
						if (!tInfo.NoPBar)
						{
							AutoWorker.ReportProgress(2, "过滤章节名");
							AutoWorker.ReportProgress(1, chapterList[num10].ChapterName);
							AutoWorker.ReportProgress(4, num10 + 1);
						}
						string pattern = "";
						string[] filterVolume = tInfo.FilterVolume;
						foreach (string text4 in filterVolume)
						{
							if (text4.Replace(" ", "") != "")
							{
								pattern = text4.Replace("{$书卷名称$}", chapterList[num10].VolumeName).Replace("{$小说作者$}", novelInfo_0.Author).Replace("{$小说名称$}", novelInfo_0.Name)
									.Replace("{$分类名称$}", novelInfo_0.LagerSort);
							}
						}
						if (Regex.Match(chapterList[num10].VolumeName, pattern, RegexOptions.IgnoreCase).Success)
						{
							SpiderException.Show(137, "限制分卷(" + chapterList[num10].VolumeName + ")_跳出本分卷章节", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						}
						goto IL_1cf4;
					}
					if (tInfo.FilterContinueChapterName != null && tInfo.FilterContinueChapterName[0].Replace(" ", "") != "")
					{
						if (!tInfo.NoPBar)
						{
							AutoWorker.ReportProgress(2, "过滤章节名");
							AutoWorker.ReportProgress(1, chapterList[num10].ChapterName);
							AutoWorker.ReportProgress(4, num10 + 1);
						}
						string pattern2 = "";
						string[] filterContinueChapterName = tInfo.FilterContinueChapterName;
						foreach (string text5 in filterContinueChapterName)
						{
							if (text5.Replace(" ", "") != "")
							{
								pattern2 = text5.Replace("{$书卷名称$}", chapterList[num10].VolumeName).Replace("{$小说作者$}", novelInfo_0.Author).Replace("{$小说名称$}", novelInfo_0.Name)
									.Replace("{$分类名称$}", novelInfo_0.LagerSort);
							}
						}
						if (Regex.Match(chapterList[num10].ChapterName, pattern2, RegexOptions.IgnoreCase).Success)
						{
							SpiderException.Show(137, "限制章节(" + chapterList[num10].ChapterName + ")_跳出本章节", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
							LocalProvider.GetInstance().CreateIndex(novelInfo_0, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
							goto IL_1cf4;
						}
					}
					if (tInfo.FilterChapterName != null && tInfo.FilterChapterName[0].Replace(" ", "") != "")
					{
						if (!tInfo.NoPBar)
						{
							AutoWorker.ReportProgress(2, "过滤章节名");
							AutoWorker.ReportProgress(1, chapterList[num10].ChapterName);
							AutoWorker.ReportProgress(4, num10 + 1);
						}
						string pattern3 = "";
						string[] filterChapterName = tInfo.FilterChapterName;
						foreach (string text6 in filterChapterName)
						{
							if (text6.Replace(" ", "") != "")
							{
								pattern3 = text6.Replace("{$书卷名称$}", chapterList[num10].VolumeName).Replace("{$小说作者$}", novelInfo_0.Author).Replace("{$小说名称$}", novelInfo_0.Name)
									.Replace("{$分类名称$}", novelInfo_0.LagerSort);
							}
						}
						if (Regex.Match(chapterList[num10].ChapterName, pattern3, RegexOptions.IgnoreCase).Success)
						{
							SpiderException.Show(137, "限制章节(" + chapterList[num10].ChapterName + ")_跳出本书", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
							LocalProvider.GetInstance().CreateIndex(novelInfo_0, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
							return;
						}
					}
					if (tInfo.FilterContinueVolume != null && tInfo.FilterContinueVolume[0].Replace(" ", "") != "")
					{
						if (!tInfo.NoPBar)
						{
							AutoWorker.ReportProgress(2, "限制章节《" + chapterList[num10].VolumeName + "》不入库分卷名");
							AutoWorker.ReportProgress(1, chapterList[num10].ChapterName);
							AutoWorker.ReportProgress(4, num10 + 1);
						}
						string pattern4 = "";
						string[] filterContinueVolume = tInfo.FilterContinueVolume;
						foreach (string text7 in filterContinueVolume)
						{
							if (text7.Replace(" ", "") != "")
							{
								pattern4 = text7.Replace("{$书卷名称$}", chapterList[num10].VolumeName).Replace("{$小说作者$}", novelInfo_0.Author).Replace("{$小说名称$}", novelInfo_0.Name)
									.Replace("{$分类名称$}", novelInfo_0.LagerSort);
							}
						}
						if (Regex.Match(chapterList[num10].VolumeName, pattern4, RegexOptions.IgnoreCase).Success && (num10 != 0 || chapterList[0].VolumeName != Configs.BaseConfig.DefaultVolumeName))
						{
							chapterList[num10].VolumeName = "";
						}
					}
					if (flag6)
					{
						AutoWorker.ReportProgress(2, "启用超级模式，跳过重复章节检查");
					}
					else if (tInfo.CheckRepeat)
					{
						if (!tInfo.NoPBar)
						{
							AutoWorker.ReportProgress(2, "重复章节检查");
							AutoWorker.ReportProgress(1, chapterList[num10].ChapterName);
							AutoWorker.ReportProgress(4, num10 + 1);
						}
						int num14 = 0;
						bool flag9 = false;
						for (int num15 = 0; num15 <= chapterList2.Length - 1; num15++)
						{
							switch (tInfo.RepeatChapter)
							{
							case 0:
								if (num15 <= num4 && chapterList[num10].ChapterName == chapterList2[num15].ChapterName)
								{
									flag9 = true;
								}
								break;
							case 1:
								if (num15 <= num4 && chapterList[num10].ChapterName == chapterList2[num15].ChapterName && chapterList[num10].VolumeName == chapterList2[num15].VolumeName)
								{
									flag9 = true;
								}
								break;
							case 2:
								if (num15 > num4)
								{
									chapterList[num10].PutID = chapterList2[num15].PutID;
								}
								else if (FormatText.CompareToChapter(chapterList[num10].ChapterName, chapterList2[num15].ChapterName))
								{
									flag9 = true;
								}
								break;
							case 3:
								if (num15 <= num4)
								{
									int num17 = FormatText.CompareToChapter2(chapterList[num10].ChapterName, chapterList2[num15].ChapterName, chapterList[num10].VolumeName, chapterList2[num15].VolumeName);
									if (num17 > 8 && num17 >= num14)
									{
										num14 = num17;
										flag9 = true;
									}
								}
								break;
							case 4:
								if (num15 <= num4)
								{
									int num18 = FormatText.CompareToChapter3(chapterList[num10].ChapterName, chapterList2[num15].ChapterName, chapterList[num10].VolumeName, chapterList2[num15].VolumeName);
									if (num18 > 0 && num18 >= num14)
									{
										num14 = num18;
										flag9 = true;
									}
								}
								break;
							case 5:
								if (num15 <= num4)
								{
									int num16 = FormatText.CompareToChapter4(chapterList[num10].ChapterName, num10, chapterList2[num15].ChapterName, num15, chapterList[num10].VolumeName, novelInfo_0.LastChapter.VolumeName);
									if (num16 > 0 && num16 >= num14)
									{
										num14 = num16;
										flag9 = true;
									}
								}
								break;
							}
							if (flag9)
							{
								SpiderException.Show(122, chapterList[num10].VolumeName + "|" + chapterList[num10].ChapterName + "@" + chapterList2[num15].VolumeName + "|" + chapterList2[num15].ChapterName, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
							}
						}
						if (tInfo.DuanImage && flag9)
						{
							AutoWorker.ReportProgress(2, "发现重复章节，启用超级更新模式");
							tInfo.ReplaceChapter = true;
							tInfo.ReplaceChapterNun = chapterList2.Length;
							flag6 = true;
							num10 = 0;
							num4 = -1;
							num11 = 0;
							num12 = 0;
							continue;
						}
						if (flag9 && tInfo.RepeatChapterHandle == 0)
						{
							break;
						}
						if (flag9 && tInfo.RepeatChapterHandle != 0)
						{
							goto IL_1cf4;
						}
					}
					AutoWorker.ReportProgress(1, chapterList[num10].ChapterName);
					AutoWorker.ReportProgress(4, num10 + 1);
					novelInfo_0.LastChapter = chapterList[num10];
					if (novelInfo_0.LastChapter.PutID > 0)
					{
						AutoWorker.ReportProgress(2, "开始采集替换章节第" + num11 + "个");
					}
					else
					{
						int num19 = num11;
						if (tInfo.ReplaceChapter && flag7)
						{
							num19 = num11 - tInfo.ReplaceChapterNun;
							if (chapterList2.Length < tInfo.ReplaceChapterNun)
							{
								num19 += tInfo.ReplaceChapterNun - chapterList2.Length;
							}
						}
						AutoWorker.ReportProgress(2, "开始采集新章节第" + num19 + "个");
					}
					try
					{
						novelInfo_0 = page.GetChapterInfo(novelInfo_0, isvip: false);
					}
					catch (Exception ex4)
					{
						SpiderException.Show(220, ex4.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						break;
					}
					bool flag10 = true;
					if (novelInfo_0.LastChapter.ChapterText == null || novelInfo_0.LastChapter.ChapterText.Trim() == "" || novelInfo_0.LastChapter.ChapterText.Length <= tInfo.MinChapterTextLength)
					{
						SpiderException.Show(121, novelInfo_0.LastChapter.ChapterName + "|" + novelInfo_0.LastChapter.ChapterUrl, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						switch (tInfo.EmptyChapter)
						{
						case 0:
							return;
						case 1:
							flag10 = false;
							break;
						case 2:
							flag10 = true;
							break;
						}
					}
					if (!Regex.Match(chapterList[num10].ChapterText, "<img", RegexOptions.IgnoreCase).Success || !tInfo.OnlyText)
					{
						if (flag10)
						{
							AutoWorker.ReportProgress(2, "正在入库章节");
							if (novelInfo_0.LastChapter.ChapterText.Length > tInfo.MinChapterTextLength || tInfo.EmptyChapter == 2)
							{
								try
								{
									novelInfo_0.LastChapter.ChapterName = chapterList[num10].ChapterName;
									novelInfo_0.LastChapter.ItemIndex = num10;
									if (novelInfo_0.LastChapter.PutID > 0)
									{
										bool flag11 = true;
										if (tInfo.isChkMD5)
										{
											string chapterText = LocalProvider.GetInstance().GetChapterText(novelInfo_0, on: false);
											string chapterText2 = LocalProvider.GetInstance().GetChapterText(novelInfo_0, on: true);
											if (SecurityUtil.ComputeMD5(chapterText) == SecurityUtil.ComputeMD5(chapterText2.Substring(0, chapterText2.Length - 4)))
											{
												flag11 = false;
											}
										}
										if (flag11)
										{
											ReplaceConfigInfo replaceConfigInfo = new ReplaceConfigInfo
											{
												UpdateChapterName = true
											};
											ReplaceConfigInfo replaceConfigInfo_ = replaceConfigInfo;
											novelInfo_0.LastChapter.LastTime = DateTime.Now;
											LocalProvider.GetInstance().UpdateChapter(novelInfo_0, replaceConfigInfo_);
											goto IL_1c53;
										}
										num10++;
										NativeMethods.ChapterCount++;
										continue;
									}
									WaitForBackgroundAsync(LocalProviderAsyncDispatcher.InsertChapterAsync(LocalProvider.GetInstance(), novelInfo_0, tInfo));
									goto IL_1c53;
									IL_1c53:
									NativeMethods.ChapterCount++;
									goto IL_1c93;
								}
								catch (Exception ex5)
								{
									SpiderException.Show(441, ex5.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
									goto IL_1c93;
								}
							}
							SpiderException.Show(130, "空章节或当前章节字数" + novelInfo_0.LastChapter.ChapterText.Length + "少于设置的" + tInfo.MinChapterTextLength + "字", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
							break;
						}
						goto IL_1cbe;
					}
					SpiderException.Show(124, novelInfo_0.LastChapter.ChapterName + "|" + novelInfo_0.LastChapter.ChapterUrl, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
					return;
					IL_1cbe:
					if (DateTime.Now.Second % 12 == 10 && !FormatDateTime.CheckHost())
					{
						WaitOrCancel(5000);
					}
					ApplyFriendlyDelay(tInfo, RequestKind.Chapter);
					goto IL_1cf4;
					IL_1c93:
					if (Configs.BaseConfig.ChapterHtml)
					{
						AutoWorker.ReportProgress(2, "正在生成章节Html");
						LocalProvider.GetInstance().CreateChapter(novelInfo_0);
					}
					flag8 = true;
					goto IL_1cbe;
					IL_1cf4:
					num10++;
				}
				if (flag8 && (Configs.BaseConfig.IndexHtml || Configs.BaseConfig.FullHtml || Configs.BaseConfig.CreateOPF || Configs.BaseConfig.CreateZIP || Configs.BaseConfig.CreateTXT || Configs.BaseConfig.CreateUMD || Configs.BaseConfig.CreateJAR || Configs.BaseConfig.CreateCHM))
				{
					AutoWorker.ReportProgress(2, "清理正在生成目录Html (此过程将同时生成OPF和其他格式)");
					LocalProvider.GetInstance().CreateIndex(novelInfo_0, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, tInfo.DelForTxt, tInfo.DelForHtml, num3);
				}
			}
		}
		catch (Exception ex6)
		{
			SpiderException.Show(0, novelInfo_0.LastChapter.ChapterName + "|" + novelInfo_0.LastChapter.ChapterUrl, novelInfo_0, tInfo.Log, ex6.Message, tInfo.RuleFile);
		}
	}

	private void CollectNovel(NovelInfo novelInfo_0)
	{
		try
		{
			NativeMethods.BookCount++;
			ChapterInfo[] array = null;
			if (novelInfo_0.GetID == null)
			{
				novelInfo_0.GetID = "0";
			}
			if (novelInfo_0.Name == null)
			{
				novelInfo_0.Name = "";
			}
			AutoWorker.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
			AutoWorker.ReportProgress(1, "-");
			AutoWorker.ReportProgress(2, "获得小说信息");
			AutoWorker.ReportProgress(4, 0);
			SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 获得小说信息");
			Page page = new Page(rInfo, tInfo);
			try
			{
				if (novelInfo_0.PutID == 0)
				{
					if (novelInfo_0.NovelUrl == null)
					{
						if (tInfo.IpStatic || (!tInfo.Proxy && !tInfo.NovelUrlProxy))
						{
							novelInfo_0 = page.GetNovelInfo(novelInfo_0);
						}
						else
						{
							DataTable sQLite = SpiderException.GetSQLite(tInfo.RuleFile, tInfo.IpStaticNum, tInfo.IpTimeNum);
							if (sQLite.Rows.Count <= 0)
							{
								AutoWorker.ReportProgress(7, "代理池已无可用IP地址，请开启 自动代理模式 进程");
							}
							else
							{
								int num = 0;
								foreach (DataRow row in sQLite.Rows)
								{
									num++;
									page.taskConfigInfo_0.ProxyHost = row["IP"].ToString();
									page.taskConfigInfo_0.ProxyPort = int.Parse(row["PORT"].ToString());
									AutoWorker.ReportProgress(7, page.taskConfigInfo_0.ProxyHost + ":" + page.taskConfigInfo_0.ProxyPort + " (" + num + "/" + sQLite.Rows.Count + ")");
									try
									{
										novelInfo_0 = page.GetNovelInfo(novelInfo_0);
										SpiderException.UpTimeSQLite(tInfo.RuleFile, page.taskConfigInfo_0.ProxyHost);
									}
									catch
									{
										SpiderException.UpLockSQLite(tInfo.RuleFile, page.taskConfigInfo_0.ProxyHost);
										continue;
									}
									break;
								}
								AutoWorker.ReportProgress(7, "-");
							}
						}
					}
					novelInfo_0 = LocalProvider.GetInstance().GetNovelInfo(novelInfo_0, tInfo.NameAndAuthor);
				}
				else if (novelInfo_0.GetID == "" || novelInfo_0.GetID == "0")
				{
					novelInfo_0 = LocalProvider.GetInstance().GetNovelInfo(novelInfo_0, tInfo.NameAndAuthor);
					AutoWorker.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
					if (tInfo.IpStatic || (!tInfo.Proxy && !tInfo.NovelUrlProxy))
					{
						novelInfo_0 = page.GetNovelInfo(novelInfo_0);
					}
					else
					{
						DataTable sQLite = SpiderException.GetSQLite(tInfo.RuleFile, tInfo.IpStaticNum, tInfo.IpTimeNum);
						if (sQLite.Rows.Count <= 0)
						{
							AutoWorker.ReportProgress(7, "代理池已无可用IP地址，请开启 自动代理模式 进程");
						}
						else
						{
							int num = 0;
							foreach (DataRow row2 in sQLite.Rows)
							{
								num++;
								page.taskConfigInfo_0.ProxyHost = row2["IP"].ToString();
								page.taskConfigInfo_0.ProxyPort = int.Parse(row2["PORT"].ToString());
								AutoWorker.ReportProgress(7, page.taskConfigInfo_0.ProxyHost + ":" + page.taskConfigInfo_0.ProxyPort + " (" + num + "/" + sQLite.Rows.Count + ")");
								try
								{
									novelInfo_0 = page.GetNovelInfo(novelInfo_0);
									SpiderException.UpTimeSQLite(tInfo.RuleFile, page.taskConfigInfo_0.ProxyHost);
								}
								catch
								{
									SpiderException.UpLockSQLite(tInfo.RuleFile, page.taskConfigInfo_0.ProxyHost);
									continue;
								}
								break;
							}
							AutoWorker.ReportProgress(7, "-");
						}
					}
				}
			}
			catch (Exception ex)
			{
				AutoWorker.ReportProgress(2, ex.Message);
				WaitOrCancel(500);
				SpiderException.Show(200, ex.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
				return;
			}
			if (AutoWorker.CancellationPending)
			{
				return;
			}
			ApplyFriendlyDelay(tInfo, RequestKind.Novel);
			try
			{
				var keys = Configs.TaskNovelInfo.Keys;
				if (Configs.TaskNovelInfo.Count != 0)
				{
					IEnumerator enumerator3 = keys.GetEnumerator();
					while (enumerator3.MoveNext())
					{
						string text = (string)enumerator3.Current;
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
			AutoWorker.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
			SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 过滤小说");
			if (tInfo.FilterNovelType != 1)
			{
				if (tInfo.FilterNovelType == 2)
				{
					bool flag = false;
					if (tInfo.FilterNovel != null)
					{
						string[] filterNovel = tInfo.FilterNovel;
						foreach (string text2 in filterNovel)
						{
							if (novelInfo_0.Name.Trim() == text2.Trim())
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
			}
			else if (tInfo.FilterNovel != null)
			{
				string[] filterNovel = tInfo.FilterNovel;
				foreach (string text3 in filterNovel)
				{
					if (novelInfo_0.Name.Trim() == text3.Trim())
					{
						SpiderException.Show(134, "限制小说_黑名单", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						return;
					}
				}
			}
			SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 获得小说的章节目录");
			int num2 = -1;
			bool flag2 = false;
			try
			{
				AutoWorker.ReportProgress(2, "获得小说的章节目录");
				if (tInfo.IpStatic || (!tInfo.Proxy && !tInfo.PubIndexUrlProxy))
				{
					array = page.GetChapterList(novelInfo_0);
				}
				else
				{
					DataTable sQLite = SpiderException.GetSQLite(tInfo.RuleFile, tInfo.IpStaticNum, tInfo.IpTimeNum);
					if (sQLite.Rows.Count <= 0)
					{
						AutoWorker.ReportProgress(7, "代理池已无可用IP地址，请开启 自动代理模式 进程");
					}
					else
					{
						int num = 0;
						foreach (DataRow row3 in sQLite.Rows)
						{
							num++;
							page.taskConfigInfo_0.ProxyHost = row3["IP"].ToString();
							page.taskConfigInfo_0.ProxyPort = int.Parse(row3["PORT"].ToString());
							AutoWorker.ReportProgress(7, page.taskConfigInfo_0.ProxyHost + ":" + page.taskConfigInfo_0.ProxyPort + " (" + num + "/" + sQLite.Rows.Count + ")");
							try
							{
								array = page.GetChapterList(novelInfo_0);
								SpiderException.UpTimeSQLite(tInfo.RuleFile, page.taskConfigInfo_0.ProxyHost);
							}
							catch
							{
								SpiderException.UpLockSQLite(tInfo.RuleFile, page.taskConfigInfo_0.ProxyHost);
								continue;
							}
							break;
						}
						AutoWorker.ReportProgress(7, "-");
					}
				}
			}
			catch (Exception ex2)
			{
				SpiderException.Show(210, ex2.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
				return;
			}
			if (array != null && array.Length != 0)
			{
				AutoWorker.ReportProgress(6, array.Length);
				if (AutoWorker.CancellationPending)
				{
					return;
				}
				if (novelInfo_0.PutID == 0 && novelInfo_0.IsNew)
				{
					AutoWorker.ReportProgress(2, "处理新书");
					if (!tInfo.Finish || novelInfo_0.Degree != 1 || !tInfo.NewBook)
					{
						SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 是否添加新书判断");
						if (!tInfo.NewBook)
						{
							SpiderException.Show(125, "设置不添加新书", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
							return;
						}
						SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 章节数量限制判断");
						if (array.Length < tInfo.MinChapterNum && tInfo.MinChapterNum != 0)
						{
							SpiderException.Show(131, "章节数量小于限制", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
							return;
						}
					}
					SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 正式入库小说信息");
					novelInfo_0 = WaitForBackgroundAsync(LocalProviderAsyncDispatcher.InsertNovelAsync(LocalProvider.GetInstance(), novelInfo_0));
					flag2 = true;
					AutoWorker.ReportProgress(0, novelInfo_0.GetID + " | " + novelInfo_0.Name + " | " + novelInfo_0.PutID);
				}
				if (AutoWorker.CancellationPending)
				{
					return;
				}
				if (novelInfo_0.LastChapter.PutID == 0)
				{
					flag2 = true;
				}
				if (tInfo.ReplaceFullflag || tInfo.ReplaceImgflag || tInfo.ReplaceIntro || tInfo.ReplaceSort)
				{
					if (tInfo.ReplaceImgflag)
					{
						if (tInfo.StrongReplaceImgflag || novelInfo_0.ImgFlag != 1)
						{
							try
							{
								if (novelInfo_0.Cover.Height <= 5)
								{
									tInfo.ReplaceImgflag = false;
								}
								else
								{
									tInfo.ReplaceImgflag = true;
								}
							}
							catch (Exception)
							{
								tInfo.ReplaceImgflag = false;
							}
						}
						else
						{
							tInfo.ReplaceImgflag = false;
						}
					}
					if (tInfo.ReplaceFullflag)
					{
						if (novelInfo_0.MDegree != 0 || tInfo.StrongReplaceIntro)
						{
							tInfo.ReplaceIntro = true;
						}
						else
						{
							tInfo.ReplaceIntro = false;
						}
					}
					if (tInfo.ReplaceIntro)
					{
						if (!(novelInfo_0.Intro == "") || !(novelInfo_0.MIntro != "") || tInfo.StrongReplaceFullflag)
						{
							tInfo.ReplaceFullflag = true;
						}
						else
						{
							tInfo.ReplaceFullflag = false;
						}
					}
					if (novelInfo_0.MLagerSortID == novelInfo_0.LagerSortID || !tInfo.ReplaceSort || novelInfo_0.MLagerSortID == 0)
					{
						tInfo.ReplaceSort = false;
					}
					else if (tInfo.OnlyReplaceSort)
					{
						if (novelInfo_0.LagerSortID == tInfo.ReplaceSortId || novelInfo_0.LagerSortID == 0)
						{
							tInfo.ReplaceSort = true;
						}
						else
						{
							tInfo.ReplaceSort = false;
						}
					}
					if (tInfo.ReplaceImgflag || tInfo.ReplaceFullflag || tInfo.ReplaceIntro || tInfo.ReplaceSort)
					{
						LocalProvider.GetInstance().UpdateNovel(novelInfo_0, bool_0: false, tInfo.ReplaceIntro, tInfo.ReplaceFullflag, tInfo.ReplaceSort, tInfo.ReplaceSort, tInfo.ReplaceImgflag, bool_6: false);
					}
				}
				AutoWorker.ReportProgress(2, "判断是否开启超级模式");
				ChapterInfo[] array2 = LocalProvider.GetInstance().GetChapterList(novelInfo_0.PutID);
				bool flag3 = false;
				novelInfo_0.LastChapter = new ChapterInfo();
				int num3 = 0;
				if (array2.Length != 0)
				{
					num3 = array2.Length - 1;
					if (tInfo.ReplaceChapter)
					{
						if (!tInfo.ReplaceChapterIndex)
						{
							num3 = array2.Length - 1 - tInfo.ReplaceChapterNun;
							if (num3 < 0)
							{
								num3 = 0;
							}
						}
						else
						{
							if (novelInfo_0.MDegree > 0)
							{
								AutoWorker.ReportProgress(2, "超级修复，书籍已经完结，跳过本书。");
								WaitOrCancel(1000);
								return;
							}
							bool flag4 = false;
							for (int j = 0; j < array2.Length; j++)
							{
								if (array2[j].ChapterName != array[j].ChapterName)
								{
									num3 = j - 1;
									flag4 = true;
									if (num3 >= 0)
									{
										break;
									}
									AutoWorker.ReportProgress(2, "超级修复，书籍第一章节对比失败，跳过本书。");
									WaitOrCancel(1000);
									return;
								}
								num3 = array2.Length - 1;
							}
							if (flag4 && tInfo.ReplaceChapterTime)
							{
								DateTime time = FormatText.GetTime(novelInfo_0.LastupDate);
								DateTime dateTime = DateTime.Now.AddDays(-tInfo.ReplaceChapterTimeMin);
								if (time <= DateTime.Now.AddDays(-tInfo.ReplaceChapterTimeMax) || !(time < dateTime))
								{
									num3 = array2.Length - 1;
									flag2 = false;
								}
							}
						}
					}
					if (!flag2)
					{
						novelInfo_0.LastChapter = array2[num3];
					}
				}
				AutoWorker.ReportProgress(2, "对比最新章节开始");
				SpiderException.Debug(tInfo.ID, "CollectAuto.Collect 对比最新章节开始");
				if (!flag2)
				{
					AutoWorker.ReportProgress(2, "正在对比最新章节");
					AutoWorker.ReportProgress(1, "-");
					int num4 = 0;
					int num5 = 0;
					while (true)
					{
						if (AutoWorker.CancellationPending)
						{
							return;
						}
						if (num5 >= array.Length - tInfo.DonnotCollectLastChapterNo)
						{
							break;
						}
						if (!tInfo.NoPBar)
						{
							AutoWorker.ReportProgress(4, num5 + 1);
						}
						switch (tInfo.EqualsChapter)
						{
						case 0:
							if (!(array[num5].ChapterName != novelInfo_0.LastChapter.ChapterName))
							{
								num2 = num5;
								flag2 = true;
							}
							break;
						case 1:
							if (!(array[num5].ChapterName != novelInfo_0.LastChapter.ChapterName) && array[num5].VolumeName == novelInfo_0.LastChapter.VolumeName)
							{
								num2 = num5;
								flag2 = true;
							}
							break;
						case 2:
							if (FormatText.CompareToChapter(array[num5].ChapterName, novelInfo_0.LastChapter.ChapterName))
							{
								num2 = num5;
								flag2 = true;
							}
							break;
						case 3:
						{
							int num7 = FormatText.CompareToChapter2(array[num5].ChapterName, novelInfo_0.LastChapter.ChapterName, array[num5].VolumeName, novelInfo_0.LastChapter.VolumeName);
							if (num7 > 3 && num7 >= num4)
							{
								num4 = num7;
								num2 = num5;
								flag2 = true;
							}
							break;
						}
						case 4:
						{
							int num8 = FormatText.CompareToChapter3(array[num5].ChapterName, novelInfo_0.LastChapter.ChapterName, array[num5].VolumeName, novelInfo_0.LastChapter.VolumeName);
							if (num8 > 0 && num8 >= num4)
							{
								num4 = num8;
								num2 = num5;
								flag2 = true;
							}
							break;
						}
						case 5:
						{
							int num6 = FormatText.CompareToChapter4(array[num5].ChapterName, num5, novelInfo_0.LastChapter.ChapterName, novelInfo_0.Chapters, array[num5].VolumeName, novelInfo_0.LastChapter.VolumeName);
							if (num6 > 0 && num6 >= num4)
							{
								num4 = num6;
								num2 = num5;
								flag2 = true;
							}
							break;
						}
						}
						num5++;
					}
				}
				if (AutoWorker.CancellationPending)
				{
					return;
				}
				if (!flag2)
				{
					if (!tInfo.DeleteChapter)
					{
						SpiderException.Show(120, novelInfo_0.LastChapter.VolumeName + " | " + novelInfo_0.LastChapter.ChapterName, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						return;
					}
					AutoWorker.ReportProgress(2, "章节对比失败，正在清空章节");
					LocalProvider.GetInstance().ClearNovel(novelInfo_0);
					array2 = new ChapterInfo[0];
					flag2 = true;
				}
				else if (tInfo.CompulsoryDeleteChapter)
				{
					AutoWorker.ReportProgress(2, "正在强制清空章节");
					LocalProvider.GetInstance().ClearNovel(novelInfo_0);
					array2 = new ChapterInfo[0];
					flag2 = true;
					num2 = -1;
				}
				if (array.Length - num2 > tInfo.FindMaxChapterNum)
				{
					SpiderException.Show(132, "对比最新章节成功！但需要采集到章节数超限。", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
				}
				else
				{
					if (AutoWorker.CancellationPending || !tInfo.OldBook || !flag2)
					{
						return;
					}
					ApplyFriendlyDelay(tInfo, RequestKind.Index);
					bool flag5 = false;
					int num9 = num2 + 1;
					int num10 = 0;
					int num11 = 0;
					while (num9 < array.Length - tInfo.DonnotCollectLastChapterNo && !AutoWorker.CancellationPending)
					{
						num10++;
						num11 = num3 + num10;
						if (num11 < array2.Length)
						{
							array[num9].PutID = array2[num11].PutID;
						}
						string[] filterNovel;
						if (tInfo.FilterVolume == null || !(tInfo.FilterVolume[0].Replace(" ", "") != ""))
						{
							if (tInfo.FilterContinueChapterName != null && tInfo.FilterContinueChapterName[0].Replace(" ", "") != "")
							{
								if (!tInfo.NoPBar)
								{
									AutoWorker.ReportProgress(2, "过滤章节名");
									AutoWorker.ReportProgress(1, array[num9].ChapterName);
									AutoWorker.ReportProgress(4, num9 + 1);
								}
								string pattern = "";
								filterNovel = tInfo.FilterContinueChapterName;
								foreach (string text4 in filterNovel)
								{
									if (text4.Replace(" ", "") != "")
									{
										pattern = text4.Replace("{$书卷名称$}", array[num9].VolumeName).Replace("{$小说作者$}", novelInfo_0.Author).Replace("{$小说名称$}", novelInfo_0.Name)
											.Replace("{$分类名称$}", novelInfo_0.LagerSort);
									}
								}
								if (Regex.Match(array[num9].ChapterName, pattern, RegexOptions.IgnoreCase).Success)
								{
									SpiderException.Show(137, "限制章节(" + array[num9].ChapterName + ")_跳出本章节", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
									LocalProvider.GetInstance().CreateIndex(novelInfo_0, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
									goto IL_25e7;
								}
							}
							if (tInfo.FilterChapterName != null && tInfo.FilterChapterName[0].Replace(" ", "") != "")
							{
								if (!tInfo.NoPBar)
								{
									AutoWorker.ReportProgress(2, "过滤章节名");
									AutoWorker.ReportProgress(1, array[num9].ChapterName);
									AutoWorker.ReportProgress(4, num9 + 1);
								}
								string pattern2 = "";
								filterNovel = tInfo.FilterChapterName;
								foreach (string text5 in filterNovel)
								{
									if (text5.Replace(" ", "") != "")
									{
										pattern2 = text5.Replace("{$书卷名称$}", array[num9].VolumeName).Replace("{$小说作者$}", novelInfo_0.Author).Replace("{$小说名称$}", novelInfo_0.Name)
											.Replace("{$分类名称$}", novelInfo_0.LagerSort);
									}
								}
								if (Regex.Match(array[num9].ChapterName, pattern2, RegexOptions.IgnoreCase).Success)
								{
									SpiderException.Show(137, "限制章节(" + array[num9].ChapterName + ")_跳出本书", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
									LocalProvider.GetInstance().CreateIndex(novelInfo_0, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
									return;
								}
							}
							if (tInfo.FilterContinueVolume != null && tInfo.FilterContinueVolume[0].Replace(" ", "") != "")
							{
								if (!tInfo.NoPBar)
								{
									AutoWorker.ReportProgress(2, "限制章节《" + array[num9].VolumeName + "》不入库分卷名");
									AutoWorker.ReportProgress(1, array[num9].ChapterName);
									AutoWorker.ReportProgress(4, num9 + 1);
								}
								string pattern3 = "";
								filterNovel = tInfo.FilterContinueVolume;
								foreach (string text6 in filterNovel)
								{
									if (text6.Replace(" ", "") != "")
									{
										pattern3 = text6.Replace("{$书卷名称$}", array[num9].VolumeName).Replace("{$小说作者$}", novelInfo_0.Author).Replace("{$小说名称$}", novelInfo_0.Name)
											.Replace("{$分类名称$}", novelInfo_0.LagerSort);
									}
								}
								if (Regex.Match(array[num9].VolumeName, pattern3, RegexOptions.IgnoreCase).Success && (num9 != 0 || array[0].VolumeName != Configs.BaseConfig.DefaultVolumeName))
								{
									array[num9].VolumeName = "";
								}
							}
							if (tInfo.CheckRepeat)
							{
								if (!tInfo.NoPBar)
								{
									AutoWorker.ReportProgress(2, "重复章节检查");
									AutoWorker.ReportProgress(1, array[num9].ChapterName);
									AutoWorker.ReportProgress(4, num9 + 1);
								}
								int num12 = 0;
								bool flag6 = false;
								for (int j = 0; j <= array2.Length - 1; j++)
								{
									switch (tInfo.RepeatChapter)
									{
									case 0:
										if ((!tInfo.ReplaceChapter || j <= num3) && array[num9].ChapterName == array2[j].ChapterName)
										{
											flag6 = true;
										}
										break;
									case 1:
										if ((!tInfo.ReplaceChapter || j <= num3) && !(array[num9].ChapterName != array2[j].ChapterName) && array[num9].VolumeName == array2[j].VolumeName)
										{
											flag6 = true;
										}
										break;
									case 2:
										if (!tInfo.ReplaceChapter || j <= num3)
										{
											if (FormatText.CompareToChapter(array[num9].ChapterName, array2[j].ChapterName))
											{
												flag6 = true;
											}
										}
										else
										{
											array[num9].PutID = array2[j].PutID;
										}
										break;
									case 3:
										if (!tInfo.ReplaceChapter || j <= num3)
										{
											int num14 = FormatText.CompareToChapter2(array[num9].ChapterName, array2[j].ChapterName, array[num9].VolumeName, array2[j].VolumeName);
											if (num14 > 8 && num14 >= num12)
											{
												num12 = num14;
												flag6 = true;
											}
										}
										break;
									case 4:
										if (!tInfo.ReplaceChapter || j <= num3)
										{
											int num15 = FormatText.CompareToChapter3(array[num9].ChapterName, array2[j].ChapterName, array[num9].VolumeName, array2[j].VolumeName);
											if (num15 > 0 && num15 >= num12)
											{
												num12 = num15;
												flag6 = true;
											}
										}
										break;
									case 5:
										if (!tInfo.ReplaceChapter || j <= num3)
										{
											int num13 = FormatText.CompareToChapter4(array[num9].ChapterName, num9, array2[j].ChapterName, j, array[num9].VolumeName, novelInfo_0.LastChapter.VolumeName);
											if (num13 > 0 && num13 >= num12)
											{
												num12 = num13;
												flag6 = true;
											}
										}
										break;
									}
									if (flag6)
									{
										SpiderException.Show(122, array[num9].VolumeName + "|" + array[num9].ChapterName + "@" + array2[j].VolumeName + "|" + array2[j].ChapterName, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
										break;
									}
								}
								if (flag6 && tInfo.RepeatChapterHandle == 0)
								{
									break;
								}
								if (flag6 && tInfo.RepeatChapterHandle != 0)
								{
									goto IL_25e7;
								}
							}
							AutoWorker.ReportProgress(1, array[num9].ChapterName);
							AutoWorker.ReportProgress(4, num9 + 1);
							novelInfo_0.LastChapter = array[num9];
							if (novelInfo_0.LastChapter.PutID <= 0)
							{
								int num16 = num10;
								if (tInfo.ReplaceChapter)
								{
									num16 = num10 - tInfo.ReplaceChapterNun;
									if (array2.Length < tInfo.ReplaceChapterNun)
									{
										num16 += tInfo.ReplaceChapterNun - array2.Length;
									}
								}
								AutoWorker.ReportProgress(2, "开始采集新章节第" + num16 + "个");
							}
							else
							{
								AutoWorker.ReportProgress(2, "开始采集替换章节第" + num10 + "个");
							}
							bool flag7 = false;
							string text7 = "";
							if (tInfo.IpStatic || (!tInfo.Proxy && !tInfo.PubContentUrlProxy))
							{
								try
								{
									novelInfo_0 = page.GetChapterInfo(novelInfo_0, isvip: false);
									flag7 = true;
								}
								catch (Exception ex4)
								{
									text7 = ex4.Message;
								}
							}
							else
							{
								DataTable sQLite = SpiderException.GetSQLite(tInfo.RuleFile, tInfo.IpStaticNum, tInfo.IpTimeNum);
								if (sQLite.Rows.Count <= 0)
								{
									AutoWorker.ReportProgress(7, "代理池已无可用IP地址，请开启 自动代理模式 进程");
								}
								else
								{
									int num = 0;
									foreach (DataRow row4 in sQLite.Rows)
									{
										num++;
										page.taskConfigInfo_0.ProxyHost = row4["IP"].ToString();
										page.taskConfigInfo_0.ProxyPort = int.Parse(row4["PORT"].ToString());
										AutoWorker.ReportProgress(7, page.taskConfigInfo_0.ProxyHost + ":" + page.taskConfigInfo_0.ProxyPort + " (" + num + "/" + sQLite.Rows.Count + ")");
										try
										{
											novelInfo_0 = page.GetChapterInfo(novelInfo_0, isvip: false);
											SpiderException.UpTimeSQLite(tInfo.RuleFile, page.taskConfigInfo_0.ProxyHost);
											flag7 = true;
										}
										catch (Exception ex5)
										{
											text7 = ex5.Message;
											SpiderException.UpLockSQLite(tInfo.RuleFile, page.taskConfigInfo_0.ProxyHost);
											continue;
										}
										break;
									}
									AutoWorker.ReportProgress(7, "-");
								}
							}
							if (!flag7)
							{
								SpiderException.Show(220, text7, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
								break;
							}
							bool flag8 = true;
							if (novelInfo_0.LastChapter.ChapterText == null || novelInfo_0.LastChapter.ChapterText.Trim() == "")
							{
								SpiderException.Show(121, novelInfo_0.LastChapter.ChapterName + "|" + novelInfo_0.LastChapter.ChapterUrl, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
								switch (tInfo.EmptyChapter)
								{
								case 0:
									return;
								case 1:
									flag8 = false;
									break;
								case 2:
									flag8 = true;
									break;
								}
							}
							if (Regex.Match(array[num9].ChapterText, "<img", RegexOptions.IgnoreCase).Success && tInfo.OnlyText)
							{
								SpiderException.Show(124, novelInfo_0.LastChapter.ChapterName + "|" + novelInfo_0.LastChapter.ChapterUrl, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
								return;
							}
							if (flag8)
							{
								AutoWorker.ReportProgress(2, "正在入库章节");
								if (novelInfo_0.LastChapter.ChapterText.Length <= tInfo.MinChapterTextLength)
								{
									object[] array3 = new object[5]
									{
										"空章节或当前章节字数",
										novelInfo_0.LastChapter.ChapterText.Length,
										"少于设置的",
										tInfo.MinChapterTextLength,
										"字"
									};
									SpiderException.Show(130, string.Concat(array3), novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
									break;
								}
								try
								{
									novelInfo_0.LastChapter.ChapterName = array[num9].ChapterName;
									novelInfo_0.LastChapter.ItemIndex = num9;
									if (novelInfo_0.LastChapter.PutID <= 0)
									{
										flag3 = true;
										WaitForBackgroundAsync(LocalProviderAsyncDispatcher.InsertChapterAsync(LocalProvider.GetInstance(), novelInfo_0, tInfo));
										goto IL_2405;
									}
									bool flag9 = true;
									if (tInfo.DelChapter)
									{
										string text8 = array2[num9].ChapterName + "|" + LocalProvider.GetInstance().GetChapterText(novelInfo_0, on: false);
										string text9 = novelInfo_0.LastChapter.ChapterName + "|" + LocalProvider.GetInstance().GetChapterText(novelInfo_0, on: true);
										if (SecurityUtil.ComputeMD5(text8) == SecurityUtil.ComputeMD5(text9.Substring(0, text9.Length - 4)))
										{
											flag9 = false;
										}
									}
									if (!flag9)
									{
										num9++;
										NativeMethods.ChapterCount++;
										continue;
									}
									flag3 = true;
									ReplaceConfigInfo replaceConfigInfo_ = new ReplaceConfigInfo
									{
										UpdateChapterName = true
									};
									novelInfo_0.LastChapter.LastTime = DateTime.Now;
									LocalProvider.GetInstance().UpdateChapter(novelInfo_0, replaceConfigInfo_);
									goto IL_2405;
									IL_2405:
									NativeMethods.ChapterCount++;
									goto IL_2449;
								}
								catch (Exception ex6)
								{
									SpiderException.Show(441, ex6.Message, novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
									goto IL_2449;
								}
							}
							goto IL_2474;
						}
						if (!tInfo.NoPBar)
						{
							AutoWorker.ReportProgress(2, "过滤章节名");
							AutoWorker.ReportProgress(1, array[num9].ChapterName);
							AutoWorker.ReportProgress(4, num9 + 1);
						}
						string pattern4 = "";
						filterNovel = tInfo.FilterVolume;
						foreach (string text10 in filterNovel)
						{
							if (text10.Replace(" ", "") != "")
							{
								pattern4 = text10.Replace("{$书卷名称$}", array[num9].VolumeName).Replace("{$小说作者$}", novelInfo_0.Author).Replace("{$小说名称$}", novelInfo_0.Name)
									.Replace("{$分类名称$}", novelInfo_0.LagerSort);
							}
						}
						if (Regex.Match(array[num9].VolumeName, pattern4, RegexOptions.IgnoreCase).Success)
						{
							SpiderException.Show(137, "限制分卷(" + array[num9].VolumeName + ")_跳出本分卷章节", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
						}
						goto IL_25e7;
						IL_2449:
						if (Configs.BaseConfig.ChapterHtml)
						{
							AutoWorker.ReportProgress(2, "正在生成章节Html");
							LocalProvider.GetInstance().CreateChapter(novelInfo_0);
						}
						flag5 = true;
						goto IL_2474;
						IL_25e7:
						num9++;
						continue;
						IL_2474:
						if (DateTime.Now.Second % 12 == 10 && !FormatDateTime.CheckHost())
						{
							WaitOrCancel(5000);
						}
						ApplyFriendlyDelay(tInfo, RequestKind.Chapter);
						goto IL_25e7;
					}
					if (flag5 && (Configs.BaseConfig.IndexHtml || Configs.BaseConfig.CreateOPF))
					{
						AutoWorker.ReportProgress(2, "清理正在生成目录Html (此过程将同时生成OPF和其他格式)");
						if (flag3)
						{
							WaitForBackgroundAsync(LocalProviderAsyncDispatcher.UpdateLastChapterAsync(LocalProvider.GetInstance(), novelInfo_0));
						}
						LocalProvider.GetInstance().CreateIndex(novelInfo_0, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
					}
				}
			}
			else
			{
				SpiderException.Show(214, "章节组为空", novelInfo_0, tInfo.Log, string_0, tInfo.RuleFile);
			}
		}
		catch (Exception ex7)
		{
			Exception ex8 = ex7;
			SpiderException.Show(0, novelInfo_0.LastChapter.ChapterName + "|" + novelInfo_0.LastChapter.ChapterUrl, novelInfo_0, tInfo.Log, ex8.Message, tInfo.RuleFile);
		}
	}

	private void comboBox_0_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (采集规则_0.Items.Count == 0 || string.IsNullOrWhiteSpace(采集规则_0.Text) || !File.Exists(采集规则_0.Text))
		{
			return;
		}
		rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(采集规则_0.Text, rInfo);
		采集最新列表框_3.Text = rInfo.NovelListUrl.Pattern;
		if (!bool_0)
		{
			Text = rInfo.GetSiteName.Pattern + " 标准采集模式";
		}
	}

	private void comboBox_4_SelectedIndexChanged(object sender, EventArgs e)
	{
		string_0 = 采集方案_4.Text;
		if (string.IsNullOrWhiteSpace(string_0) || !File.Exists(string_0))
		{
			return;
		}
		tInfo = (TaskConfigInfo)ConfigFileManager.LoadConfig(string_0, tInfo);
		method_4();
		if (string.IsNullOrWhiteSpace(采集规则_0.Text) || !File.Exists(采集规则_0.Text))
		{
			return;
		}
		rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(采集规则_0.Text, rInfo);
		if (!bool_0)
		{
			Text = rInfo.GetSiteName.Pattern + " 标准采集模式";
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

	private void HttpWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		HttpClient httpClient = new HttpClient
		{
			UriString = rInfo.NovelListUrl.Pattern,
			Encoding = Encoding.GetEncoding(rInfo.GetSiteCharset.Pattern),
			Proxy = tInfo.Proxy,
			ProxyHost = tInfo.ProxyHost,
			ProxyPort = tInfo.ProxyPort,
			ProxyDomain = tInfo.ProxyDomain,
			ProxyUser = tInfo.ProxyUser,
			ProxyPassword = tInfo.ProxyPassword
		};
		HttpClient httpClient2 = httpClient;
		string text = "";
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		string stringWork = httpClient2.GetStringWork();
		if (stringWork != string.Empty)
		{
			text = stopwatch.Elapsed.TotalSeconds.ToString("0.000") + "秒";
		}
		text = "测试完成，";
		text = ((!tInfo.Proxy) ? (text + "本机访问") : (text + "代理IP(" + tInfo.ProxyHost + ")访问"));
		if (text == string.Empty)
		{
			text += "不可用";
			return;
		}
		double num = (double)stringWork.Length / 1024.0;
		string text2 = text;
		text = text2 + "抓取" + num.ToString("0.000") + "k字节耗时" + text;
	}

	private void HttpWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
	}

	private void HttpWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		开始.Enabled = true;
		测试网速.Enabled = true;
		停止.Enabled = false;
		label6.Text = "--";
		label7.Text = "--";
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.自动采集模式));
		this.开始 = new System.Windows.Forms.Button();
		this.停止 = new System.Windows.Forms.Button();
		this.TargetMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripMenuItem_6 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_25 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_8 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_22 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_3 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_9 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_11 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_23 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolTip_0 = new System.Windows.Forms.ToolTip(this.components);
		this.groupBox_3 = new System.Windows.Forms.GroupBox();
		this.FilterVolumeTextBox = new System.Windows.Forms.TextBox();
		this.FilterNovelTextBox = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.FilterChapterNameBox = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.FilterChapterNameBox1 = new System.Windows.Forms.TextBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.FilterVolumeTextBox1 = new System.Windows.Forms.TextBox();
		this.禁止添加分卷_4 = new System.Windows.Forms.CheckBox();
		this.下载图片章节_5 = new System.Windows.Forms.CheckBox();
		this.清空重采_6 = new System.Windows.Forms.CheckBox();
		this.不处理完结_8 = new System.Windows.Forms.CheckBox();
		this.更新连载_1 = new System.Windows.Forms.CheckBox();
		this.内容自动排版_16 = new System.Windows.Forms.CheckBox();
		this.添加新书_0 = new System.Windows.Forms.CheckBox();
		this.强制清空_18 = new System.Windows.Forms.CheckBox();
		this.栓测重复章节_22 = new System.Windows.Forms.CheckBox();
		this.日志记录_3 = new System.Windows.Forms.CheckBox();
		this.循环采集_2 = new System.Windows.Forms.CheckBox();
		this.循环间隔时间选择_0 = new System.Windows.Forms.NumericUpDown();
		this.目标站书号开始_6 = new System.Windows.Forms.TextBox();
		this.目标站书号结束_5 = new System.Windows.Forms.TextBox();
		this.目标站自定义书号ID_4 = new System.Windows.Forms.TextBox();
		this.采集最新列表框_3 = new System.Windows.Forms.TextBox();
		this.本站书号开始_2 = new System.Windows.Forms.TextBox();
		this.本站书号结束_1 = new System.Windows.Forms.TextBox();
		this.本站自定义书号ID_0 = new System.Windows.Forms.TextBox();
		this.其他站列表URL_17 = new System.Windows.Forms.TextBox();
		this.提取其他站小说名规则_16 = new System.Windows.Forms.TextBox();
		this.其他站编码_18 = new System.Windows.Forms.TextBox();
		this.AutoWorker = new System.ComponentModel.BackgroundWorker();
		this.注意得示_16 = new System.Windows.Forms.Label();
		this.采集方案_1 = new System.Windows.Forms.Button();
		this.timer_0 = new System.Windows.Forms.Timer(this.components);
		this.SaveMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripMenuItem_3 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_0 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_0 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_1 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_2 = new System.Windows.Forms.ToolStripMenuItem();
		this.fontDialog_0 = new System.Windows.Forms.FontDialog();
		this.openFileDialog_0 = new System.Windows.Forms.OpenFileDialog();
		this.saveFileDialog_0 = new System.Windows.Forms.SaveFileDialog();
		this.测试网速 = new System.Windows.Forms.Button();
		this.LoginWorker = new System.ComponentModel.BackgroundWorker();
		this.TestWorker = new System.ComponentModel.BackgroundWorker();
		this.HttpWorker = new System.ComponentModel.BackgroundWorker();
		this.代理设定2 = new System.Windows.Forms.TabPage();
		this.groupBox8 = new System.Windows.Forms.GroupBox();
		this.listView1 = new System.Windows.Forms.ListView();
		this.columnHeader_12 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader_13 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
		this.groupBox_7 = new System.Windows.Forms.GroupBox();
		this.button1 = new System.Windows.Forms.Button();
		this.chkEnableProxy = new System.Windows.Forms.CheckBox();
		this.label_19 = new System.Windows.Forms.Label();
		this.textBox_14 = new System.Windows.Forms.TextBox();
		this.label_17 = new System.Windows.Forms.Label();
		this.label_18 = new System.Windows.Forms.Label();
		this.textBox_9 = new System.Windows.Forms.TextBox();
		this.textBox_10 = new System.Windows.Forms.TextBox();
		this.textBox_11 = new System.Windows.Forms.TextBox();
		this.textBox_12 = new System.Windows.Forms.TextBox();
		this.采集进度_4 = new System.Windows.Forms.TabPage();
		this.groupBox_4 = new System.Windows.Forms.GroupBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label_13 = new System.Windows.Forms.Label();
		this.label_14 = new System.Windows.Forms.Label();
		this.label_15 = new System.Windows.Forms.Label();
		this.label_8 = new System.Windows.Forms.Label();
		this.label_9 = new System.Windows.Forms.Label();
		this.label_10 = new System.Windows.Forms.Label();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.checkBox_20 = new System.Windows.Forms.CheckBox();
		this.progressBar_1 = new System.Windows.Forms.ProgressBar();
		this.progressBar_0 = new System.Windows.Forms.ProgressBar();
		this.label_11 = new System.Windows.Forms.Label();
		this.label_12 = new System.Windows.Forms.Label();
		this.groupBox_9 = new System.Windows.Forms.GroupBox();
		this.label_21 = new System.Windows.Forms.Label();
		this.label_22 = new System.Windows.Forms.Label();
		this.label_23 = new System.Windows.Forms.Label();
		this.label_24 = new System.Windows.Forms.Label();
		this.numericUpDown_3 = new System.Windows.Forms.NumericUpDown();
		this.label_25 = new System.Windows.Forms.Label();
		this.numericUpDown_4 = new System.Windows.Forms.NumericUpDown();
		this.label_26 = new System.Windows.Forms.Label();
		this.numericUpDown_5 = new System.Windows.Forms.NumericUpDown();
		this.高级设置1 = new System.Windows.Forms.TabPage();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.label20 = new System.Windows.Forms.Label();
		this.ReplaceChapterTimeMax = new System.Windows.Forms.NumericUpDown();
		this.label19 = new System.Windows.Forms.Label();
		this.ReplaceChapterTimeMin = new System.Windows.Forms.NumericUpDown();
		this.索引对比失败只修复 = new System.Windows.Forms.CheckBox();
		this.索引对比判断修复 = new System.Windows.Forms.CheckBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.ReplaceChapterNameNun = new System.Windows.Forms.NumericUpDown();
		this.label15 = new System.Windows.Forms.Label();
		this.isChkMD5 = new System.Windows.Forms.CheckBox();
		this.forceReplace = new System.Windows.Forms.CheckBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.ReplaceChapterNun = new System.Windows.Forms.NumericUpDown();
		this.ReplaceChapter = new System.Windows.Forms.CheckBox();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.StrongReplaceIntro = new System.Windows.Forms.CheckBox();
		this.StrongReplaceFullflag = new System.Windows.Forms.CheckBox();
		this.StrongReplaceImgflag = new System.Windows.Forms.CheckBox();
		this.label8 = new System.Windows.Forms.Label();
		this.ReplaceSortId = new System.Windows.Forms.NumericUpDown();
		this.OnlyReplaceSort = new System.Windows.Forms.CheckBox();
		this.ReplaceSort = new System.Windows.Forms.CheckBox();
		this.ReplaceIntro = new System.Windows.Forms.CheckBox();
		this.ReplaceFullflag = new System.Windows.Forms.CheckBox();
		this.ReplaceImgflag = new System.Windows.Forms.CheckBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label4 = new System.Windows.Forms.Label();
		this.DelForTxt = new System.Windows.Forms.CheckBox();
		this.DelForHtml = new System.Windows.Forms.CheckBox();
		this.DuanImageCheck = new System.Windows.Forms.CheckBox();
		this.DuanImage = new System.Windows.Forms.CheckBox();
		this.过滤设置_2 = new System.Windows.Forms.TabPage();
		this.groupBox_2 = new System.Windows.Forms.GroupBox();
		this.FilterNovelType = new System.Windows.Forms.ComboBox();
		this.采集动作_1 = new System.Windows.Forms.TabPage();
		this.设置2_5 = new System.Windows.Forms.GroupBox();
		this.label3 = new System.Windows.Forms.Label();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.label_32 = new System.Windows.Forms.Label();
		this.comboBox_7 = new System.Windows.Forms.ComboBox();
		this.comboBox_6 = new System.Windows.Forms.ComboBox();
		this.label_31 = new System.Windows.Forms.Label();
		this.label_28 = new System.Windows.Forms.Label();
		this.textBox_19 = new System.Windows.Forms.TextBox();
		this.label_27 = new System.Windows.Forms.Label();
		this.comboBox_5 = new System.Windows.Forms.ComboBox();
		this.label_6 = new System.Windows.Forms.Label();
		this.comboBox_1 = new System.Windows.Forms.ComboBox();
		this.label_7 = new System.Windows.Forms.Label();
		this.comboBox_2 = new System.Windows.Forms.ComboBox();
		this.设置_6 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.DonnotCollectLastChapterNum = new System.Windows.Forms.NumericUpDown();
		this.label_29 = new System.Windows.Forms.Label();
		this.不采小于字符设置_6 = new System.Windows.Forms.NumericUpDown();
		this.不采小于字符前_30 = new System.Windows.Forms.Label();
		this.label_2 = new System.Windows.Forms.Label();
		this.numericUpDown_1 = new System.Windows.Forms.NumericUpDown();
		this.label_3 = new System.Windows.Forms.Label();
		this.不采小于章节后段_4 = new System.Windows.Forms.Label();
		this.不采集小于章节_5 = new System.Windows.Forms.Label();
		this.不采小于章节设置_2 = new System.Windows.Forms.NumericUpDown();
		this.全本必采_15 = new System.Windows.Forms.CheckBox();
		this.隐藏更新小说 = new System.Windows.Forms.CheckBox();
		this.添加分卷判断_21 = new System.Windows.Forms.CheckBox();
		this.识别方式_17 = new System.Windows.Forms.CheckBox();
		this.调用页面_10 = new System.Windows.Forms.CheckBox();
		this.只采文字章_9 = new System.Windows.Forms.CheckBox();
		this.采集模式_0 = new System.Windows.Forms.TabPage();
		this.其他站列表搜索_5 = new System.Windows.Forms.RadioButton();
		this.采集方案_4 = new System.Windows.Forms.ComboBox();
		this.采集方案_20 = new System.Windows.Forms.Label();
		this.采集规则_1 = new System.Windows.Forms.Label();
		this.采集规则_0 = new System.Windows.Forms.ComboBox();
		this.本站自定义书号_3 = new System.Windows.Forms.RadioButton();
		this.本站书号_4 = new System.Windows.Forms.RadioButton();
		this.采集最新列表_0 = new System.Windows.Forms.RadioButton();
		this.目标站自定义书号_1 = new System.Windows.Forms.RadioButton();
		this.目标站书号_2 = new System.Windows.Forms.RadioButton();
		this.循环间隔时间_0 = new System.Windows.Forms.Label();
		this.tabControl_0 = new System.Windows.Forms.TabControl();
		this.TargetMenuStrip.SuspendLayout();
		this.groupBox_3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.循环间隔时间选择_0).BeginInit();
		this.SaveMenuStrip.SuspendLayout();
		this.代理设定2.SuspendLayout();
		this.groupBox8.SuspendLayout();
		this.groupBox_7.SuspendLayout();
		this.采集进度_4.SuspendLayout();
		this.groupBox_4.SuspendLayout();
		this.groupBox7.SuspendLayout();
		this.groupBox_9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_5).BeginInit();
		this.高级设置1.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ReplaceChapterTimeMax).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ReplaceChapterTimeMin).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ReplaceChapterNameNun).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ReplaceChapterNun).BeginInit();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ReplaceSortId).BeginInit();
		this.groupBox4.SuspendLayout();
		this.过滤设置_2.SuspendLayout();
		this.groupBox_2.SuspendLayout();
		this.采集动作_1.SuspendLayout();
		this.设置2_5.SuspendLayout();
		this.设置_6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DonnotCollectLastChapterNum).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.不采小于字符设置_6).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.不采小于章节设置_2).BeginInit();
		this.采集模式_0.SuspendLayout();
		this.tabControl_0.SuspendLayout();
		base.SuspendLayout();
		this.开始.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.开始.Location = new System.Drawing.Point(497, 391);
		this.开始.Name = "开始";
		this.开始.Size = new System.Drawing.Size(75, 23);
		this.开始.TabIndex = 14;
		this.开始.Text = "开始";
		this.开始.UseVisualStyleBackColor = true;
		this.开始.Click += new System.EventHandler(btnStartClick);
		this.停止.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.停止.Enabled = false;
		this.停止.Location = new System.Drawing.Point(578, 391);
		this.停止.Name = "停止";
		this.停止.Size = new System.Drawing.Size(75, 23);
		this.停止.TabIndex = 15;
		this.停止.Text = "停止";
		this.停止.UseVisualStyleBackColor = true;
		this.停止.Click += new System.EventHandler(btnStopClick);
		this.TargetMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripMenuItem_6, this.toolStripMenuItem_25, this.toolStripSeparator_2, this.toolStripMenuItem_8, this.toolStripMenuItem_22, this.toolStripSeparator_3, this.toolStripMenuItem_9, this.toolStripMenuItem_11, this.toolStripMenuItem_23 });
		this.TargetMenuStrip.Name = "TargetMenuStrip";
		this.TargetMenuStrip.Size = new System.Drawing.Size(149, 170);
		this.toolStripMenuItem_6.Name = "toolStripMenuItem_6";
		this.toolStripMenuItem_6.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_6.Text = "设为代理";
		this.toolStripMenuItem_6.Click += new System.EventHandler(toolStripMenuItem_6_Click);
		this.toolStripMenuItem_25.Name = "toolStripMenuItem_25";
		this.toolStripMenuItem_25.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_25.Text = "验证可用性";
		this.toolStripMenuItem_25.Click += new System.EventHandler(toolStripMenuItem_25_Click);
		this.toolStripSeparator_2.Name = "toolStripSeparator_2";
		this.toolStripSeparator_2.Size = new System.Drawing.Size(145, 6);
		this.toolStripMenuItem_8.Name = "toolStripMenuItem_8";
		this.toolStripMenuItem_8.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_8.Text = "全部选中";
		this.toolStripMenuItem_8.Click += new System.EventHandler(toolStripMenuItem_8_Click);
		this.toolStripMenuItem_22.Name = "toolStripMenuItem_22";
		this.toolStripMenuItem_22.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_22.Text = "全部不选中";
		this.toolStripMenuItem_22.Click += new System.EventHandler(toolStripMenuItem_22_Click);
		this.toolStripSeparator_3.Name = "toolStripSeparator_3";
		this.toolStripSeparator_3.Size = new System.Drawing.Size(145, 6);
		this.toolStripMenuItem_9.Name = "toolStripMenuItem_9";
		this.toolStripMenuItem_9.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_9.Text = "删除选中";
		this.toolStripMenuItem_9.Click += new System.EventHandler(toolStripMenuItem_9_Click);
		this.toolStripMenuItem_11.Name = "toolStripMenuItem_11";
		this.toolStripMenuItem_11.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_11.Text = "清空列表";
		this.toolStripMenuItem_11.Click += new System.EventHandler(toolStripMenuItem_11_Click);
		this.toolStripMenuItem_23.Name = "toolStripMenuItem_23";
		this.toolStripMenuItem_23.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_23.Text = "重新加载列表";
		this.toolStripMenuItem_23.Click += new System.EventHandler(toolStripMenuItem_23_Click);
		this.toolTip_0.AutomaticDelay = 100;
		this.toolTip_0.AutoPopDelay = 50000;
		this.toolTip_0.InitialDelay = 100;
		this.toolTip_0.ReshowDelay = 20;
		this.toolTip_0.ShowAlways = true;
		this.toolTip_0.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.toolTip_0.ToolTipTitle = "提示";
		this.groupBox_3.Controls.Add(this.FilterVolumeTextBox);
		this.groupBox_3.Location = new System.Drawing.Point(218, 13);
		this.groupBox_3.Name = "groupBox_3";
		this.groupBox_3.Size = new System.Drawing.Size(188, 145);
		this.groupBox_3.TabIndex = 1;
		this.groupBox_3.TabStop = false;
		this.groupBox_3.Text = "过滤分卷（跳过本分卷）";
		this.toolTip_0.SetToolTip(this.groupBox_3, "过滤格式：\n分卷名\u3000\u3000\u3000\u3000这种过滤所有书的这个分卷\n书名♂分卷\u3000\u3000只过滤某本书的某个分卷");
		this.FilterVolumeTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FilterVolumeTextBox.Location = new System.Drawing.Point(3, 20);
		this.FilterVolumeTextBox.Multiline = true;
		this.FilterVolumeTextBox.Name = "FilterVolumeTextBox";
		this.FilterVolumeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.FilterVolumeTextBox.Size = new System.Drawing.Size(179, 119);
		this.FilterVolumeTextBox.TabIndex = 0;
		this.toolTip_0.SetToolTip(this.FilterVolumeTextBox, "请输入分卷名称，一行一个。");
		this.FilterNovelTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FilterNovelTextBox.Location = new System.Drawing.Point(8, 46);
		this.FilterNovelTextBox.Multiline = true;
		this.FilterNovelTextBox.Name = "FilterNovelTextBox";
		this.FilterNovelTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.FilterNovelTextBox.Size = new System.Drawing.Size(192, 275);
		this.FilterNovelTextBox.TabIndex = 8;
		this.toolTip_0.SetToolTip(this.FilterNovelTextBox, "请输入小说名称，一行一个。");
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.Controls.Add(this.FilterChapterNameBox);
		this.groupBox1.Location = new System.Drawing.Point(412, 13);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(387, 145);
		this.groupBox1.TabIndex = 3;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "过滤章节名（跳过本书）";
		this.toolTip_0.SetToolTip(this.groupBox1, "过滤格式：\r\n章节名\u3000\u3000\u3000\u3000这种过滤所有书的这个章节\r\n书名♂章节名\u3000\u3000只过滤某本书的某个章节");
		this.FilterChapterNameBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FilterChapterNameBox.Location = new System.Drawing.Point(3, 20);
		this.FilterChapterNameBox.Multiline = true;
		this.FilterChapterNameBox.Name = "FilterChapterNameBox";
		this.FilterChapterNameBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.FilterChapterNameBox.Size = new System.Drawing.Size(378, 119);
		this.FilterChapterNameBox.TabIndex = 0;
		this.toolTip_0.SetToolTip(this.FilterChapterNameBox, "请输入章节名称，一行一个。");
		this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox2.Controls.Add(this.FilterChapterNameBox1);
		this.groupBox2.Location = new System.Drawing.Point(412, 164);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(384, 176);
		this.groupBox2.TabIndex = 4;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "过滤章节名（继续采集）";
		this.toolTip_0.SetToolTip(this.groupBox2, "过滤格式：\r\n章节名\u3000\u3000\u3000\u3000这种过滤所有书的这个章节\r\n书名♂章节名\u3000\u3000只过滤某本书的某个章节");
		this.FilterChapterNameBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FilterChapterNameBox1.Location = new System.Drawing.Point(3, 20);
		this.FilterChapterNameBox1.Multiline = true;
		this.FilterChapterNameBox1.Name = "FilterChapterNameBox1";
		this.FilterChapterNameBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.FilterChapterNameBox1.Size = new System.Drawing.Size(375, 150);
		this.FilterChapterNameBox1.TabIndex = 0;
		this.toolTip_0.SetToolTip(this.FilterChapterNameBox1, "请输入章节名称，一行一个。");
		this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.groupBox3.Controls.Add(this.FilterVolumeTextBox1);
		this.groupBox3.Location = new System.Drawing.Point(218, 164);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(188, 176);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "过滤分卷（继续采集）";
		this.toolTip_0.SetToolTip(this.groupBox3, "过滤格式：\n分卷名\u3000\u3000\u3000\u3000这种过滤所有书的这个分卷\n书名♂分卷\u3000\u3000只过滤某本书的某个分卷");
		this.FilterVolumeTextBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FilterVolumeTextBox1.Location = new System.Drawing.Point(3, 20);
		this.FilterVolumeTextBox1.Multiline = true;
		this.FilterVolumeTextBox1.Name = "FilterVolumeTextBox1";
		this.FilterVolumeTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.FilterVolumeTextBox1.Size = new System.Drawing.Size(179, 150);
		this.FilterVolumeTextBox1.TabIndex = 0;
		this.toolTip_0.SetToolTip(this.FilterVolumeTextBox1, "请输入分卷名称，一行一个。");
		this.禁止添加分卷_4.AutoSize = true;
		this.禁止添加分卷_4.Location = new System.Drawing.Point(426, 64);
		this.禁止添加分卷_4.Name = "禁止添加分卷_4";
		this.禁止添加分卷_4.Size = new System.Drawing.Size(96, 16);
		this.禁止添加分卷_4.TabIndex = 7;
		this.禁止添加分卷_4.Text = "禁止添加分卷";
		this.toolTip_0.SetToolTip(this.禁止添加分卷_4, "入库章节的时候不考虑分卷问题，直接追加在最后一个分卷后面。");
		this.禁止添加分卷_4.UseVisualStyleBackColor = true;
		this.下载图片章节_5.AutoSize = true;
		this.下载图片章节_5.Location = new System.Drawing.Point(426, 42);
		this.下载图片章节_5.Name = "下载图片章节_5";
		this.下载图片章节_5.Size = new System.Drawing.Size(108, 16);
		this.下载图片章节_5.TabIndex = 6;
		this.下载图片章节_5.Text = "下载图片本地化";
		this.toolTip_0.SetToolTip(this.下载图片章节_5, "是否下载图片本地化，不勾则为盗链模式。");
		this.下载图片章节_5.UseVisualStyleBackColor = true;
		this.清空重采_6.AutoSize = true;
		this.清空重采_6.ForeColor = System.Drawing.Color.Gray;
		this.清空重采_6.Location = new System.Drawing.Point(6, 86);
		this.清空重采_6.Name = "清空重采_6";
		this.清空重采_6.Size = new System.Drawing.Size(168, 16);
		this.清空重采_6.TabIndex = 5;
		this.清空重采_6.Text = "对比章节失败自动清空重采";
		this.toolTip_0.SetToolTip(this.清空重采_6, "对比最新章节失败的时候自动清空所有旧的章节重新采集，不建议使用，不利于SEO");
		this.清空重采_6.UseVisualStyleBackColor = true;
		this.不处理完结_8.AutoSize = true;
		this.不处理完结_8.Location = new System.Drawing.Point(426, 20);
		this.不处理完结_8.Name = "不处理完结_8";
		this.不处理完结_8.Size = new System.Drawing.Size(120, 16);
		this.不处理完结_8.TabIndex = 2;
		this.不处理完结_8.Text = "不处理已完成小说";
		this.toolTip_0.SetToolTip(this.不处理完结_8, "本站标记已完成的小说不进行任何操作。");
		this.不处理完结_8.UseVisualStyleBackColor = true;
		this.更新连载_1.AutoSize = true;
		this.更新连载_1.Location = new System.Drawing.Point(200, 20);
		this.更新连载_1.Name = "更新连载_1";
		this.更新连载_1.Size = new System.Drawing.Size(96, 16);
		this.更新连载_1.TabIndex = 1;
		this.更新连载_1.Text = "更新连载章节";
		this.toolTip_0.SetToolTip(this.更新连载_1, "采集过程中需要更新旧书章节吗？");
		this.更新连载_1.UseVisualStyleBackColor = true;
		this.内容自动排版_16.AutoSize = true;
		this.内容自动排版_16.Location = new System.Drawing.Point(200, 42);
		this.内容自动排版_16.Name = "内容自动排版_16";
		this.内容自动排版_16.Size = new System.Drawing.Size(120, 16);
		this.内容自动排版_16.TabIndex = 4;
		this.内容自动排版_16.Text = "自动排版章节内容";
		this.toolTip_0.SetToolTip(this.内容自动排版_16, "章节内容自动重新排版。");
		this.内容自动排版_16.UseVisualStyleBackColor = true;
		this.添加新书_0.AutoSize = true;
		this.添加新书_0.Location = new System.Drawing.Point(6, 20);
		this.添加新书_0.Name = "添加新书_0";
		this.添加新书_0.Size = new System.Drawing.Size(72, 16);
		this.添加新书_0.TabIndex = 0;
		this.添加新书_0.Text = "添加新书";
		this.toolTip_0.SetToolTip(this.添加新书_0, "采集过程中遇到新书是否添加？");
		this.添加新书_0.UseVisualStyleBackColor = true;
		this.强制清空_18.AutoSize = true;
		this.强制清空_18.ForeColor = System.Drawing.Color.Gray;
		this.强制清空_18.Location = new System.Drawing.Point(200, 86);
		this.强制清空_18.Name = "强制清空_18";
		this.强制清空_18.Size = new System.Drawing.Size(132, 16);
		this.强制清空_18.TabIndex = 11;
		this.强制清空_18.Text = "强制清空重采(慎用)";
		this.toolTip_0.SetToolTip(this.强制清空_18, "对比最新章节失败的时候自动清空所有旧的章节重新采集，不建议使用，不利于SEO");
		this.强制清空_18.UseVisualStyleBackColor = true;
		this.栓测重复章节_22.AutoSize = true;
		this.栓测重复章节_22.Location = new System.Drawing.Point(426, 108);
		this.栓测重复章节_22.Name = "栓测重复章节_22";
		this.栓测重复章节_22.Size = new System.Drawing.Size(96, 16);
		this.栓测重复章节_22.TabIndex = 13;
		this.栓测重复章节_22.Text = "检测重复章节";
		this.toolTip_0.SetToolTip(this.栓测重复章节_22, "只检查需要入库的第一个章节\n判断方式调用下面设置的对比方式。");
		this.栓测重复章节_22.UseVisualStyleBackColor = true;
		this.日志记录_3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.日志记录_3.AutoSize = true;
		this.日志记录_3.Location = new System.Drawing.Point(459, 320);
		this.日志记录_3.Name = "日志记录_3";
		this.日志记录_3.Size = new System.Drawing.Size(72, 16);
		this.日志记录_3.TabIndex = 15;
		this.日志记录_3.Text = "日志记录";
		this.toolTip_0.SetToolTip(this.日志记录_3, "当发生错误是，以日志形式记录，不弹出错误提示框。");
		this.日志记录_3.UseVisualStyleBackColor = true;
		this.循环采集_2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.循环采集_2.AutoSize = true;
		this.循环采集_2.Location = new System.Drawing.Point(537, 320);
		this.循环采集_2.Name = "循环采集_2";
		this.循环采集_2.Size = new System.Drawing.Size(72, 16);
		this.循环采集_2.TabIndex = 16;
		this.循环采集_2.Text = "循环采集";
		this.toolTip_0.SetToolTip(this.循环采集_2, "当一次采集完成后，等待N分钟间隔时间，开始下次循环。");
		this.循环采集_2.UseVisualStyleBackColor = true;
		this.循环间隔时间选择_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.循环间隔时间选择_0.Location = new System.Drawing.Point(746, 319);
		this.循环间隔时间选择_0.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.循环间隔时间选择_0.Name = "循环间隔时间选择_0";
		this.循环间隔时间选择_0.Size = new System.Drawing.Size(50, 21);
		this.循环间隔时间选择_0.TabIndex = 19;
		this.toolTip_0.SetToolTip(this.循环间隔时间选择_0, "2次采集中间的间隔时间。");
		this.目标站书号开始_6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.目标站书号开始_6.Location = new System.Drawing.Point(144, 183);
		this.目标站书号开始_6.Name = "目标站书号开始_6";
		this.目标站书号开始_6.Size = new System.Drawing.Size(212, 21);
		this.目标站书号开始_6.TabIndex = 4;
		this.toolTip_0.SetToolTip(this.目标站书号开始_6, "开始编号");
		this.目标站书号结束_5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.目标站书号结束_5.Location = new System.Drawing.Point(362, 183);
		this.目标站书号结束_5.Name = "目标站书号结束_5";
		this.目标站书号结束_5.Size = new System.Drawing.Size(434, 21);
		this.目标站书号结束_5.TabIndex = 5;
		this.toolTip_0.SetToolTip(this.目标站书号结束_5, "结束编号");
		this.目标站自定义书号ID_4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.目标站自定义书号ID_4.Location = new System.Drawing.Point(144, 210);
		this.目标站自定义书号ID_4.Name = "目标站自定义书号ID_4";
		this.目标站自定义书号ID_4.Size = new System.Drawing.Size(652, 21);
		this.目标站自定义书号ID_4.TabIndex = 7;
		this.toolTip_0.SetToolTip(this.目标站自定义书号ID_4, "自定义编号，用\",\"分割(英文半角)");
		this.采集最新列表框_3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.采集最新列表框_3.Location = new System.Drawing.Point(6, 54);
		this.采集最新列表框_3.Multiline = true;
		this.采集最新列表框_3.Name = "采集最新列表框_3";
		this.采集最新列表框_3.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.采集最新列表框_3.Size = new System.Drawing.Size(790, 123);
		this.采集最新列表框_3.TabIndex = 2;
		this.toolTip_0.SetToolTip(this.采集最新列表框_3, "一行一个地址。");
		this.本站书号开始_2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.本站书号开始_2.Location = new System.Drawing.Point(144, 237);
		this.本站书号开始_2.Name = "本站书号开始_2";
		this.本站书号开始_2.Size = new System.Drawing.Size(212, 21);
		this.本站书号开始_2.TabIndex = 10;
		this.toolTip_0.SetToolTip(this.本站书号开始_2, "开始编号");
		this.本站书号结束_1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.本站书号结束_1.Location = new System.Drawing.Point(362, 237);
		this.本站书号结束_1.Name = "本站书号结束_1";
		this.本站书号结束_1.Size = new System.Drawing.Size(434, 21);
		this.本站书号结束_1.TabIndex = 11;
		this.toolTip_0.SetToolTip(this.本站书号结束_1, "结束编号");
		this.本站自定义书号ID_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.本站自定义书号ID_0.Location = new System.Drawing.Point(144, 264);
		this.本站自定义书号ID_0.Name = "本站自定义书号ID_0";
		this.本站自定义书号ID_0.Size = new System.Drawing.Size(652, 21);
		this.本站自定义书号ID_0.TabIndex = 13;
		this.toolTip_0.SetToolTip(this.本站自定义书号ID_0, "自定义编号，用\",\"分割(英文半角)");
		this.其他站列表URL_17.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.其他站列表URL_17.Location = new System.Drawing.Point(144, 290);
		this.其他站列表URL_17.Name = "其他站列表URL_17";
		this.其他站列表URL_17.Size = new System.Drawing.Size(233, 21);
		this.其他站列表URL_17.TabIndex = 34;
		this.toolTip_0.SetToolTip(this.其他站列表URL_17, "其他站URL地址");
		this.提取其他站小说名规则_16.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.提取其他站小说名规则_16.Location = new System.Drawing.Point(444, 291);
		this.提取其他站小说名规则_16.Name = "提取其他站小说名规则_16";
		this.提取其他站小说名规则_16.Size = new System.Drawing.Size(352, 21);
		this.提取其他站小说名规则_16.TabIndex = 35;
		this.toolTip_0.SetToolTip(this.提取其他站小说名规则_16, "提取小说名规则");
		this.其他站编码_18.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.其他站编码_18.Location = new System.Drawing.Point(383, 290);
		this.其他站编码_18.Name = "其他站编码_18";
		this.其他站编码_18.Size = new System.Drawing.Size(55, 21);
		this.其他站编码_18.TabIndex = 36;
		this.toolTip_0.SetToolTip(this.其他站编码_18, "其他站编码");
		this.AutoWorker.WorkerReportsProgress = true;
		this.AutoWorker.WorkerSupportsCancellation = true;
		this.AutoWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(AutoWorker_DoWork);
		this.AutoWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(AutoWorker_ProgressChanged);
		this.AutoWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(AutoWorker_RunWorkerCompleted);
		this.注意得示_16.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.注意得示_16.AutoSize = true;
		this.注意得示_16.Location = new System.Drawing.Point(10, 396);
		this.注意得示_16.Name = "注意得示_16";
		this.注意得示_16.Size = new System.Drawing.Size(197, 12);
		this.注意得示_16.TabIndex = 19;
		this.注意得示_16.Text = "注意：采集过程中，改动设置无效。";
		this.采集方案_1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.采集方案_1.Location = new System.Drawing.Point(743, 391);
		this.采集方案_1.Name = "采集方案_1";
		this.采集方案_1.Size = new System.Drawing.Size(75, 23);
		this.采集方案_1.TabIndex = 20;
		this.采集方案_1.Text = "采集方案";
		this.采集方案_1.UseVisualStyleBackColor = true;
		this.采集方案_1.Click += new System.EventHandler(btnSaveConfigClick);
		this.timer_0.Interval = 1000;
		this.timer_0.Tick += new System.EventHandler(timer_0_Tick);
		this.SaveMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripMenuItem_3, this.toolStripMenuItem_0, this.toolStripSeparator_0, this.toolStripMenuItem_1, this.toolStripMenuItem_2 });
		this.SaveMenuStrip.Name = "SaveMenuStrip";
		this.SaveMenuStrip.ShowImageMargin = false;
		this.SaveMenuStrip.Size = new System.Drawing.Size(136, 98);
		this.toolStripMenuItem_3.Name = "toolStripMenuItem_3";
		this.toolStripMenuItem_3.Size = new System.Drawing.Size(135, 22);
		this.toolStripMenuItem_3.Text = "打开方案";
		this.toolStripMenuItem_3.Click += new System.EventHandler(toolStripMenuItem_3_Click);
		this.toolStripMenuItem_0.Name = "toolStripMenuItem_0";
		this.toolStripMenuItem_0.Size = new System.Drawing.Size(135, 22);
		this.toolStripMenuItem_0.Text = "保存方案";
		this.toolStripMenuItem_0.Click += new System.EventHandler(toolStripMenuItem_0_Click);
		this.toolStripSeparator_0.Name = "toolStripSeparator_0";
		this.toolStripSeparator_0.Size = new System.Drawing.Size(132, 6);
		this.toolStripMenuItem_1.Name = "toolStripMenuItem_1";
		this.toolStripMenuItem_1.Size = new System.Drawing.Size(135, 22);
		this.toolStripMenuItem_1.Text = "另存方案";
		this.toolStripMenuItem_1.Click += new System.EventHandler(toolStripMenuItem_1_Click);
		this.toolStripMenuItem_2.Name = "toolStripMenuItem_2";
		this.toolStripMenuItem_2.Size = new System.Drawing.Size(135, 22);
		this.toolStripMenuItem_2.Text = "保存为手动方案";
		this.toolStripMenuItem_2.Click += new System.EventHandler(toolStripMenuItem_2_Click);
		this.openFileDialog_0.DefaultExt = "xml";
		this.openFileDialog_0.Filter = "规则文件(*.xml)|*.xml|所有文件(*.*)|*.*";
		this.openFileDialog_0.InitialDirectory = "Tasks";
		this.openFileDialog_0.RestoreDirectory = true;
		this.saveFileDialog_0.DefaultExt = "xml";
		this.saveFileDialog_0.Filter = "规则文件(*.xml)|*.xml|所有文件(*.*)|*.*";
		this.saveFileDialog_0.InitialDirectory = "Tasks";
		this.saveFileDialog_0.RestoreDirectory = true;
		this.测试网速.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.测试网速.Location = new System.Drawing.Point(662, 391);
		this.测试网速.Name = "测试网速";
		this.测试网速.Size = new System.Drawing.Size(75, 23);
		this.测试网速.TabIndex = 21;
		this.测试网速.Text = "测试网速";
		this.测试网速.UseVisualStyleBackColor = true;
		this.测试网速.Click += new System.EventHandler(btnTestNetworkClick);
		this.LoginWorker.WorkerReportsProgress = true;
		this.LoginWorker.WorkerSupportsCancellation = true;
		this.LoginWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(LoginWorker_DoWork);
		this.LoginWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(LoginWorker_ProgressChanged);
		this.LoginWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(LoginWorker_RunWorkerCompleted);
		this.TestWorker.WorkerReportsProgress = true;
		this.TestWorker.WorkerSupportsCancellation = true;
		this.TestWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(TestWorker_DoWork);
		this.TestWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(TestWorker_RunWorkerCompleted);
		this.HttpWorker.WorkerReportsProgress = true;
		this.HttpWorker.WorkerSupportsCancellation = true;
		this.HttpWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(HttpWorker_DoWork);
		this.HttpWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(HttpWorker_ProgressChanged);
		this.HttpWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(HttpWorker_RunWorkerCompleted);
		this.代理设定2.Controls.Add(this.groupBox8);
		this.代理设定2.Controls.Add(this.groupBox_7);
		this.代理设定2.Location = new System.Drawing.Point(4, 22);
		this.代理设定2.Name = "代理设定2";
		this.代理设定2.Padding = new System.Windows.Forms.Padding(3);
		this.代理设定2.Size = new System.Drawing.Size(802, 347);
		this.代理设定2.TabIndex = 6;
		this.代理设定2.Text = "代理设定";
		this.代理设定2.UseVisualStyleBackColor = true;
		this.groupBox8.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox8.Controls.Add(this.listView1);
		this.groupBox8.Location = new System.Drawing.Point(9, 116);
		this.groupBox8.Name = "groupBox8";
		this.groupBox8.Size = new System.Drawing.Size(787, 225);
		this.groupBox8.TabIndex = 5;
		this.groupBox8.TabStop = false;
		this.groupBox8.Text = "获取代理列表";
		this.listView1.CheckBoxes = true;
		this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[6] { this.columnHeader_12, this.columnHeader_13, this.columnHeader1, this.columnHeader2, this.columnHeader3, this.columnHeader4 });
		this.listView1.ContextMenuStrip = this.TargetMenuStrip;
		this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listView1.FullRowSelect = true;
		this.listView1.GridLines = true;
		this.listView1.HideSelection = false;
		this.listView1.Location = new System.Drawing.Point(3, 17);
		this.listView1.Name = "listView1";
		this.listView1.Size = new System.Drawing.Size(781, 205);
		this.listView1.TabIndex = 4;
		this.listView1.UseCompatibleStateImageBehavior = false;
		this.listView1.View = System.Windows.Forms.View.Details;
		this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(listView1_MouseDoubleClick);
		this.columnHeader_12.Text = "IP地址";
		this.columnHeader_12.Width = 150;
		this.columnHeader_13.Text = "端口";
		this.columnHeader_13.Width = 80;
		this.columnHeader1.Text = "位置";
		this.columnHeader1.Width = 200;
		this.columnHeader2.Text = "连接速度";
		this.columnHeader2.Width = 90;
		this.columnHeader3.Text = "抓取速度";
		this.columnHeader3.Width = 90;
		this.columnHeader4.Text = "验证时间";
		this.columnHeader4.Width = 130;
		this.groupBox_7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_7.Controls.Add(this.button1);
		this.groupBox_7.Controls.Add(this.chkEnableProxy);
		this.groupBox_7.Controls.Add(this.label_19);
		this.groupBox_7.Controls.Add(this.textBox_14);
		this.groupBox_7.Controls.Add(this.label_17);
		this.groupBox_7.Controls.Add(this.label_18);
		this.groupBox_7.Controls.Add(this.textBox_9);
		this.groupBox_7.Controls.Add(this.textBox_10);
		this.groupBox_7.Controls.Add(this.textBox_11);
		this.groupBox_7.Controls.Add(this.textBox_12);
		this.groupBox_7.Location = new System.Drawing.Point(9, 6);
		this.groupBox_7.Name = "groupBox_7";
		this.groupBox_7.Size = new System.Drawing.Size(787, 104);
		this.groupBox_7.TabIndex = 4;
		this.groupBox_7.TabStop = false;
		this.groupBox_7.Text = "代理IP";
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button1.Location = new System.Drawing.Point(663, 72);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(118, 23);
		this.button1.TabIndex = 21;
		this.button1.Text = "手动导入代理";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(btnImportProxylistClick);
		this.chkEnableProxy.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkEnableProxy.AutoSize = true;
		this.chkEnableProxy.Location = new System.Drawing.Point(663, 50);
		this.chkEnableProxy.Name = "chkEnableProxy";
		this.chkEnableProxy.Size = new System.Drawing.Size(96, 16);
		this.chkEnableProxy.TabIndex = 20;
		this.chkEnableProxy.Text = "启用代理功能";
		this.chkEnableProxy.UseVisualStyleBackColor = true;
		this.label_19.AutoSize = true;
		this.label_19.Location = new System.Drawing.Point(30, 50);
		this.label_19.Name = "label_19";
		this.label_19.Size = new System.Drawing.Size(53, 12);
		this.label_19.TabIndex = 19;
		this.label_19.Text = "代理域：";
		this.textBox_14.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_14.Location = new System.Drawing.Point(89, 47);
		this.textBox_14.Name = "textBox_14";
		this.textBox_14.Size = new System.Drawing.Size(568, 21);
		this.textBox_14.TabIndex = 17;
		this.label_17.AutoSize = true;
		this.label_17.Location = new System.Drawing.Point(18, 77);
		this.label_17.Name = "label_17";
		this.label_17.Size = new System.Drawing.Size(65, 12);
		this.label_17.TabIndex = 16;
		this.label_17.Text = "帐户密码：";
		this.label_18.AutoSize = true;
		this.label_18.Location = new System.Drawing.Point(18, 23);
		this.label_18.Name = "label_18";
		this.label_18.Size = new System.Drawing.Size(65, 12);
		this.label_18.TabIndex = 15;
		this.label_18.Text = "ＩＰ端口：";
		this.textBox_9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_9.Location = new System.Drawing.Point(313, 74);
		this.textBox_9.Name = "textBox_9";
		this.textBox_9.Size = new System.Drawing.Size(344, 21);
		this.textBox_9.TabIndex = 12;
		this.textBox_10.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_10.Location = new System.Drawing.Point(89, 74);
		this.textBox_10.Name = "textBox_10";
		this.textBox_10.Size = new System.Drawing.Size(218, 21);
		this.textBox_10.TabIndex = 11;
		this.textBox_11.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_11.Location = new System.Drawing.Point(663, 20);
		this.textBox_11.Name = "textBox_11";
		this.textBox_11.Size = new System.Drawing.Size(118, 21);
		this.textBox_11.TabIndex = 10;
		this.textBox_11.Text = "80";
		this.textBox_12.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_12.Location = new System.Drawing.Point(89, 20);
		this.textBox_12.Name = "textBox_12";
		this.textBox_12.Size = new System.Drawing.Size(568, 21);
		this.textBox_12.TabIndex = 9;
		this.采集进度_4.Controls.Add(this.groupBox_4);
		this.采集进度_4.Controls.Add(this.groupBox7);
		this.groupBox_4.Controls.Add(this.groupBox_9);
		this.采集进度_4.Location = new System.Drawing.Point(4, 22);
		this.采集进度_4.Name = "采集进度_4";
		this.采集进度_4.Size = new System.Drawing.Size(802, 347);
		this.采集进度_4.TabIndex = 4;
		this.采集进度_4.Text = "采集进度";
		this.采集进度_4.UseVisualStyleBackColor = true;
		this.groupBox_4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox_4.AutoSize = true;
		this.groupBox_4.Controls.Add(this.label16);
		this.groupBox_4.Controls.Add(this.label17);
		this.groupBox_4.Controls.Add(this.label6);
		this.groupBox_4.Controls.Add(this.label7);
		this.groupBox_4.Controls.Add(this.label11);
		this.groupBox_4.Controls.Add(this.label12);
		this.groupBox_4.Controls.Add(this.label13);
		this.groupBox_4.Controls.Add(this.label14);
		this.groupBox_4.Controls.Add(this.label_13);
		this.groupBox_4.Controls.Add(this.label_14);
		this.groupBox_4.Controls.Add(this.label_15);
		this.groupBox_4.Controls.Add(this.label_8);
		this.groupBox_4.Controls.Add(this.label_9);
		this.groupBox_4.Controls.Add(this.label_10);
		this.groupBox_4.Location = new System.Drawing.Point(6, 116);
		this.groupBox_4.Name = "groupBox_4";
		this.groupBox_4.Size = new System.Drawing.Size(790, 224);
		this.groupBox_4.TabIndex = 3;
		this.groupBox_4.TabStop = false;
		this.groupBox_4.Text = "操作详情";
		this.label16.AutoSize = true;
		this.label16.Location = new System.Drawing.Point(78, 22);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(89, 12);
		this.label16.TabIndex = 24;
		this.label16.Text = "普通版本无期限";
		this.label17.AutoSize = true;
		this.label17.Location = new System.Drawing.Point(18, 22);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(65, 12);
		this.label17.TabIndex = 23;
		this.label17.Text = "版本授权：";
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(78, 96);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(17, 12);
		this.label6.TabIndex = 22;
		this.label6.Text = "--";
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(78, 71);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(17, 12);
		this.label7.TabIndex = 21;
		this.label7.Text = "--";
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(78, 46);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(17, 12);
		this.label11.TabIndex = 20;
		this.label11.Text = "--";
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(18, 96);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(65, 12);
		this.label12.TabIndex = 19;
		this.label12.Text = "开始时间：";
		this.label13.AutoSize = true;
		this.label13.Location = new System.Drawing.Point(6, 71);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(77, 12);
		this.label13.TabIndex = 18;
		this.label13.Text = "代理IP地址：";
		this.label14.AutoSize = true;
		this.label14.Location = new System.Drawing.Point(6, 46);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(77, 12);
		this.label14.TabIndex = 17;
		this.label14.Text = "子窗口标识：";
		this.label_13.AutoSize = true;
		this.label_13.Location = new System.Drawing.Point(78, 169);
		this.label_13.Name = "label_13";
		this.label_13.Size = new System.Drawing.Size(17, 12);
		this.label_13.TabIndex = 16;
		this.label_13.Text = "--";
		this.label_14.AutoSize = true;
		this.label_14.Location = new System.Drawing.Point(78, 144);
		this.label_14.Name = "label_14";
		this.label_14.Size = new System.Drawing.Size(17, 12);
		this.label_14.TabIndex = 15;
		this.label_14.Text = "--";
		this.label_15.AutoSize = true;
		this.label_15.Location = new System.Drawing.Point(78, 119);
		this.label_15.Name = "label_15";
		this.label_15.Size = new System.Drawing.Size(17, 12);
		this.label_15.TabIndex = 14;
		this.label_15.Text = "--";
		this.label_8.AutoSize = true;
		this.label_8.Location = new System.Drawing.Point(18, 169);
		this.label_8.Name = "label_8";
		this.label_8.Size = new System.Drawing.Size(65, 12);
		this.label_8.TabIndex = 13;
		this.label_8.Text = "当前状态：";
		this.label_9.AutoSize = true;
		this.label_9.Location = new System.Drawing.Point(18, 144);
		this.label_9.Name = "label_9";
		this.label_9.Size = new System.Drawing.Size(65, 12);
		this.label_9.TabIndex = 12;
		this.label_9.Text = "当前章节：";
		this.label_10.AutoSize = true;
		this.label_10.Location = new System.Drawing.Point(18, 119);
		this.label_10.Name = "label_10";
		this.label_10.Size = new System.Drawing.Size(65, 12);
		this.label_10.TabIndex = 11;
		this.label_10.Text = "当前小说：";
		this.groupBox7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox7.Controls.Add(this.checkBox_20);
		this.groupBox7.Controls.Add(this.progressBar_1);
		this.groupBox7.Controls.Add(this.progressBar_0);
		this.groupBox7.Controls.Add(this.label_11);
		this.groupBox7.Controls.Add(this.label_12);
		this.groupBox7.Location = new System.Drawing.Point(6, 6);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(622, 104);
		this.groupBox7.TabIndex = 6;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "采集进度";
		this.checkBox_20.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.checkBox_20.Location = new System.Drawing.Point(80, 75);
		this.checkBox_20.Name = "checkBox_20";
		this.checkBox_20.Size = new System.Drawing.Size(96, 16);
		this.checkBox_20.TabIndex = 17;
		this.checkBox_20.Text = "不绘图进度条";
		this.checkBox_20.UseVisualStyleBackColor = true;
		this.progressBar_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.progressBar_1.Location = new System.Drawing.Point(80, 23);
		this.progressBar_1.Name = "progressBar_1";
		this.progressBar_1.Size = new System.Drawing.Size(536, 18);
		this.progressBar_1.TabIndex = 7;
		this.progressBar_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.progressBar_0.Location = new System.Drawing.Point(80, 47);
		this.progressBar_0.Name = "progressBar_0";
		this.progressBar_0.Size = new System.Drawing.Size(536, 18);
		this.progressBar_0.TabIndex = 10;
		this.label_11.AutoSize = true;
		this.label_11.Location = new System.Drawing.Point(6, 50);
		this.label_11.Name = "label_11";
		this.label_11.Size = new System.Drawing.Size(77, 12);
		this.label_11.TabIndex = 9;
		this.label_11.Text = "采集分进度：";
		this.label_12.AutoSize = true;
		this.label_12.Location = new System.Drawing.Point(6, 28);
		this.label_12.Name = "label_12";
		this.label_12.Size = new System.Drawing.Size(77, 12);
		this.label_12.TabIndex = 8;
		this.label_12.Text = "采集总进度：";
		this.groupBox_9.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
		this.groupBox_9.Controls.Add(this.label_21);
		this.groupBox_9.Controls.Add(this.label_22);
		this.groupBox_9.Controls.Add(this.label_23);
		this.groupBox_9.Controls.Add(this.label_24);
		this.groupBox_9.Controls.Add(this.numericUpDown_3);
		this.groupBox_9.Controls.Add(this.label_25);
		this.groupBox_9.Controls.Add(this.numericUpDown_4);
		this.groupBox_9.Controls.Add(this.label_26);
		this.groupBox_9.Controls.Add(this.numericUpDown_5);
		this.groupBox_9.Location = new System.Drawing.Point(548, 22);
		this.groupBox_9.Name = "groupBox_9";
		this.groupBox_9.Size = new System.Drawing.Size(232, 192);
		this.groupBox_9.TabIndex = 5;
		this.groupBox_9.TabStop = false;
		this.groupBox_9.Text = "请求调度";
		this.label_21.AutoSize = true;
		this.label_21.Location = new System.Drawing.Point(121, 77);
		this.label_21.Name = "label_21";
		this.label_21.Size = new System.Drawing.Size(29, 12);
		this.label_21.TabIndex = 28;
		this.label_21.Text = "毫秒";
		this.label_22.AutoSize = true;
		this.label_22.Location = new System.Drawing.Point(121, 50);
		this.label_22.Name = "label_22";
		this.label_22.Size = new System.Drawing.Size(29, 12);
		this.label_22.TabIndex = 27;
		this.label_22.Text = "毫秒";
		this.label_23.AutoSize = true;
		this.label_23.Location = new System.Drawing.Point(121, 23);
		this.label_23.Name = "label_23";
		this.label_23.Size = new System.Drawing.Size(29, 12);
		this.label_23.TabIndex = 26;
		this.label_23.Text = "毫秒";
		this.label_24.AutoSize = true;
		this.label_24.Location = new System.Drawing.Point(12, 77);
		this.label_24.Name = "label_24";
		this.label_24.Size = new System.Drawing.Size(53, 12);
		this.label_24.TabIndex = 25;
		this.label_24.Text = "章节页：";
		this.numericUpDown_3.Location = new System.Drawing.Point(71, 74);
		this.numericUpDown_3.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown_3.Name = "numericUpDown_3";
		this.numericUpDown_3.Size = new System.Drawing.Size(44, 21);
		this.numericUpDown_3.TabIndex = 24;
		this.label_25.AutoSize = true;
		this.label_25.Location = new System.Drawing.Point(12, 50);
		this.label_25.Name = "label_25";
		this.label_25.Size = new System.Drawing.Size(53, 12);
		this.label_25.TabIndex = 23;
		this.label_25.Text = "目录页：";
		this.numericUpDown_4.Location = new System.Drawing.Point(71, 47);
		this.numericUpDown_4.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown_4.Name = "numericUpDown_4";
		this.numericUpDown_4.Size = new System.Drawing.Size(44, 21);
		this.numericUpDown_4.TabIndex = 22;
		this.label_26.AutoSize = true;
		this.label_26.Location = new System.Drawing.Point(12, 23);
		this.label_26.Name = "label_26";
		this.label_26.Size = new System.Drawing.Size(53, 12);
		this.label_26.TabIndex = 21;
		this.label_26.Text = "信息页：";
		this.numericUpDown_5.Location = new System.Drawing.Point(71, 20);
		this.numericUpDown_5.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown_5.Name = "numericUpDown_5";
		this.numericUpDown_5.Size = new System.Drawing.Size(44, 21);
		this.numericUpDown_5.TabIndex = 0;
		this.高级设置1.Controls.Add(this.groupBox6);
		this.高级设置1.Controls.Add(this.groupBox5);
		this.高级设置1.Controls.Add(this.groupBox4);
		this.高级设置1.Location = new System.Drawing.Point(4, 22);
		this.高级设置1.Name = "高级设置1";
		this.高级设置1.Size = new System.Drawing.Size(802, 347);
		this.高级设置1.TabIndex = 5;
		this.高级设置1.Text = "高级设置";
		this.高级设置1.UseVisualStyleBackColor = true;
		this.groupBox6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox6.Controls.Add(this.label20);
		this.groupBox6.Controls.Add(this.ReplaceChapterTimeMax);
		this.groupBox6.Controls.Add(this.label19);
		this.groupBox6.Controls.Add(this.ReplaceChapterTimeMin);
		this.groupBox6.Controls.Add(this.索引对比失败只修复);
		this.groupBox6.Controls.Add(this.索引对比判断修复);
		this.groupBox6.Controls.Add(this.label5);
		this.groupBox6.Controls.Add(this.label18);
		this.groupBox6.Controls.Add(this.ReplaceChapterNameNun);
		this.groupBox6.Controls.Add(this.label15);
		this.groupBox6.Controls.Add(this.isChkMD5);
		this.groupBox6.Controls.Add(this.forceReplace);
		this.groupBox6.Controls.Add(this.label10);
		this.groupBox6.Controls.Add(this.label9);
		this.groupBox6.Controls.Add(this.ReplaceChapterNun);
		this.groupBox6.Controls.Add(this.ReplaceChapter);
		this.groupBox6.Location = new System.Drawing.Point(3, 120);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(796, 138);
		this.groupBox6.TabIndex = 3;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "章节自动修复(高级服务)";
		this.label20.AutoSize = true;
		this.label20.Location = new System.Drawing.Point(313, 112);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(101, 12);
		this.label20.TabIndex = 35;
		this.label20.Text = "天内更新的书籍。";
		this.ReplaceChapterTimeMax.Location = new System.Drawing.Point(247, 107);
		this.ReplaceChapterTimeMax.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		this.ReplaceChapterTimeMax.Name = "ReplaceChapterTimeMax";
		this.ReplaceChapterTimeMax.Size = new System.Drawing.Size(59, 21);
		this.ReplaceChapterTimeMax.TabIndex = 34;
		this.label19.AutoSize = true;
		this.label19.Location = new System.Drawing.Point(219, 112);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(17, 12);
		this.label19.TabIndex = 33;
		this.label19.Text = "到";
		this.ReplaceChapterTimeMin.Location = new System.Drawing.Point(153, 107);
		this.ReplaceChapterTimeMin.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		this.ReplaceChapterTimeMin.Name = "ReplaceChapterTimeMin";
		this.ReplaceChapterTimeMin.Size = new System.Drawing.Size(60, 21);
		this.ReplaceChapterTimeMin.TabIndex = 32;
		this.索引对比失败只修复.AutoSize = true;
		this.索引对比失败只修复.Checked = true;
		this.索引对比失败只修复.CheckState = System.Windows.Forms.CheckState.Checked;
		this.索引对比失败只修复.ForeColor = System.Drawing.Color.Red;
		this.索引对比失败只修复.Location = new System.Drawing.Point(6, 108);
		this.索引对比失败只修复.Name = "索引对比失败只修复";
		this.索引对比失败只修复.Size = new System.Drawing.Size(144, 16);
		this.索引对比失败只修复.TabIndex = 31;
		this.索引对比失败只修复.Text = "索引对比失败，只修复";
		this.索引对比失败只修复.UseVisualStyleBackColor = true;
		this.索引对比判断修复.AutoSize = true;
		this.索引对比判断修复.Checked = true;
		this.索引对比判断修复.CheckState = System.Windows.Forms.CheckState.Checked;
		this.索引对比判断修复.ForeColor = System.Drawing.Color.Red;
		this.索引对比判断修复.Location = new System.Drawing.Point(6, 86);
		this.索引对比判断修复.Name = "索引对比判断修复";
		this.索引对比判断修复.Size = new System.Drawing.Size(360, 16);
		this.索引对比判断修复.TabIndex = 30;
		this.索引对比判断修复.Text = "用索引对比判断修复的的章节数（从对比失败的章节开始覆盖）";
		this.索引对比判断修复.UseVisualStyleBackColor = true;
		this.label5.AutoSize = true;
		this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
		this.label5.Location = new System.Drawing.Point(518, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(149, 12);
		this.label5.TabIndex = 29;
		this.label5.Text = "章节名称(突破列表防采集)";
		this.label18.AutoSize = true;
		this.label18.ForeColor = System.Drawing.SystemColors.HotTrack;
		this.label18.Location = new System.Drawing.Point(371, 21);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(89, 12);
		this.label18.TabIndex = 28;
		this.label18.Text = "获取列表时修复";
		this.ReplaceChapterNameNun.Location = new System.Drawing.Point(466, 16);
		this.ReplaceChapterNameNun.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.ReplaceChapterNameNun.Name = "ReplaceChapterNameNun";
		this.ReplaceChapterNameNun.Size = new System.Drawing.Size(46, 21);
		this.ReplaceChapterNameNun.TabIndex = 27;
		this.label15.AutoSize = true;
		this.label15.Location = new System.Drawing.Point(25, 43);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(353, 12);
		this.label15.TabIndex = 26;
		this.label15.Text = "(说明：自动修复错误，即覆盖章节名和内容，无视一切防盗章节)";
		this.isChkMD5.AutoSize = true;
		this.isChkMD5.Enabled = false;
		this.isChkMD5.Location = new System.Drawing.Point(6, 63);
		this.isChkMD5.Name = "isChkMD5";
		this.isChkMD5.Size = new System.Drawing.Size(282, 16);
		this.isChkMD5.TabIndex = 25;
		this.isChkMD5.Text = "自动对比章节MD5（如果章节内容相同则不替换）";
		this.isChkMD5.UseVisualStyleBackColor = true;
		this.forceReplace.AutoSize = true;
		this.forceReplace.Enabled = false;
		this.forceReplace.Location = new System.Drawing.Point(371, 63);
		this.forceReplace.Name = "forceReplace";
		this.forceReplace.Size = new System.Drawing.Size(378, 16);
		this.forceReplace.TabIndex = 25;
		this.forceReplace.Text = "强制替换最后N章节（如果不勾选只有最新章节对比不上时才替换）";
		this.forceReplace.UseVisualStyleBackColor = true;
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(302, 20);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(29, 12);
		this.label10.TabIndex = 24;
		this.label10.Text = "章节";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(148, 21);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(77, 12);
		this.label9.TabIndex = 23;
		this.label9.Text = "覆盖修复最后";
		this.ReplaceChapterNun.Location = new System.Drawing.Point(231, 17);
		this.ReplaceChapterNun.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.ReplaceChapterNun.Name = "ReplaceChapterNun";
		this.ReplaceChapterNun.Size = new System.Drawing.Size(63, 21);
		this.ReplaceChapterNun.TabIndex = 1;
		this.ReplaceChapter.AutoSize = true;
		this.ReplaceChapter.Checked = true;
		this.ReplaceChapter.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ReplaceChapter.Location = new System.Drawing.Point(6, 20);
		this.ReplaceChapter.Name = "ReplaceChapter";
		this.ReplaceChapter.Size = new System.Drawing.Size(120, 16);
		this.ReplaceChapter.TabIndex = 0;
		this.ReplaceChapter.Text = "启用自动修复错误";
		this.ReplaceChapter.UseVisualStyleBackColor = true;
		this.groupBox5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox5.Controls.Add(this.StrongReplaceIntro);
		this.groupBox5.Controls.Add(this.StrongReplaceFullflag);
		this.groupBox5.Controls.Add(this.StrongReplaceImgflag);
		this.groupBox5.Controls.Add(this.label8);
		this.groupBox5.Controls.Add(this.ReplaceSortId);
		this.groupBox5.Controls.Add(this.OnlyReplaceSort);
		this.groupBox5.Controls.Add(this.ReplaceSort);
		this.groupBox5.Controls.Add(this.ReplaceIntro);
		this.groupBox5.Controls.Add(this.ReplaceFullflag);
		this.groupBox5.Controls.Add(this.ReplaceImgflag);
		this.groupBox5.Location = new System.Drawing.Point(3, 3);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(796, 111);
		this.groupBox5.TabIndex = 2;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "信息自动更新";
		this.StrongReplaceIntro.AutoSize = true;
		this.StrongReplaceIntro.Location = new System.Drawing.Point(396, 63);
		this.StrongReplaceIntro.Name = "StrongReplaceIntro";
		this.StrongReplaceIntro.Size = new System.Drawing.Size(288, 16);
		this.StrongReplaceIntro.TabIndex = 25;
		this.StrongReplaceIntro.Text = "强制更新小说简介(目标站无简介时不更新，慎用)";
		this.StrongReplaceIntro.UseVisualStyleBackColor = true;
		this.StrongReplaceIntro.CheckedChanged += new System.EventHandler(StrongReplaceIntro_CheckedChanged);
		this.StrongReplaceFullflag.AutoSize = true;
		this.StrongReplaceFullflag.Location = new System.Drawing.Point(396, 41);
		this.StrongReplaceFullflag.Name = "StrongReplaceFullflag";
		this.StrongReplaceFullflag.Size = new System.Drawing.Size(120, 16);
		this.StrongReplaceFullflag.TabIndex = 24;
		this.StrongReplaceFullflag.Text = "强制更新连载状态";
		this.StrongReplaceFullflag.UseVisualStyleBackColor = true;
		this.StrongReplaceFullflag.CheckedChanged += new System.EventHandler(StrongReplaceFullflag_CheckedChanged);
		this.StrongReplaceImgflag.AutoSize = true;
		this.StrongReplaceImgflag.Location = new System.Drawing.Point(396, 20);
		this.StrongReplaceImgflag.Name = "StrongReplaceImgflag";
		this.StrongReplaceImgflag.Size = new System.Drawing.Size(288, 16);
		this.StrongReplaceImgflag.TabIndex = 23;
		this.StrongReplaceImgflag.Text = "强制更新小说封面（目标站无封面不更新，慎用）";
		this.StrongReplaceImgflag.UseVisualStyleBackColor = true;
		this.StrongReplaceImgflag.CheckedChanged += new System.EventHandler(StrongReplaceImgflag_CheckedChanged);
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(518, 87);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(239, 12);
		this.label8.TabIndex = 22;
		this.label8.Text = "的小说(如只更新本站无分类小说请设置为0)";
		this.ReplaceSortId.Location = new System.Drawing.Point(466, 83);
		this.ReplaceSortId.Name = "ReplaceSortId";
		this.ReplaceSortId.Size = new System.Drawing.Size(46, 21);
		this.ReplaceSortId.TabIndex = 21;
		this.OnlyReplaceSort.AutoSize = true;
		this.OnlyReplaceSort.Location = new System.Drawing.Point(300, 86);
		this.OnlyReplaceSort.Name = "OnlyReplaceSort";
		this.OnlyReplaceSort.Size = new System.Drawing.Size(168, 16);
		this.OnlyReplaceSort.TabIndex = 4;
		this.OnlyReplaceSort.Text = "无分类小说和本站分类ID为";
		this.OnlyReplaceSort.UseVisualStyleBackColor = true;
		this.OnlyReplaceSort.CheckedChanged += new System.EventHandler(OnlyReplaceSort_CheckedChanged);
		this.ReplaceSort.AutoSize = true;
		this.ReplaceSort.Location = new System.Drawing.Point(6, 86);
		this.ReplaceSort.Name = "ReplaceSort";
		this.ReplaceSort.Size = new System.Drawing.Size(288, 16);
		this.ReplaceSort.TabIndex = 3;
		this.ReplaceSort.Text = "自动更新分类（后面未选择时强制更新所有小说）";
		this.ReplaceSort.UseVisualStyleBackColor = true;
		this.ReplaceSort.CheckedChanged += new System.EventHandler(ReplaceSort_CheckedChanged);
		this.ReplaceIntro.AutoSize = true;
		this.ReplaceIntro.Location = new System.Drawing.Point(6, 64);
		this.ReplaceIntro.Name = "ReplaceIntro";
		this.ReplaceIntro.Size = new System.Drawing.Size(396, 16);
		this.ReplaceIntro.TabIndex = 2;
		this.ReplaceIntro.Text = "自动更新小说简介（只更新本站无简介的小说目标站无简介时不更新）";
		this.ReplaceIntro.UseVisualStyleBackColor = true;
		this.ReplaceIntro.CheckedChanged += new System.EventHandler(ReplaceIntro_CheckedChanged);
		this.ReplaceFullflag.AutoSize = true;
		this.ReplaceFullflag.Location = new System.Drawing.Point(6, 42);
		this.ReplaceFullflag.Name = "ReplaceFullflag";
		this.ReplaceFullflag.Size = new System.Drawing.Size(372, 16);
		this.ReplaceFullflag.TabIndex = 1;
		this.ReplaceFullflag.Text = "自动更新连载状态（只更新本站状态为连载的小说，连载到完结）";
		this.ReplaceFullflag.UseVisualStyleBackColor = true;
		this.ReplaceFullflag.CheckedChanged += new System.EventHandler(ReplaceFullflag_CheckedChanged);
		this.ReplaceImgflag.AutoSize = true;
		this.ReplaceImgflag.Location = new System.Drawing.Point(6, 20);
		this.ReplaceImgflag.Name = "ReplaceImgflag";
		this.ReplaceImgflag.Size = new System.Drawing.Size(384, 16);
		this.ReplaceImgflag.TabIndex = 0;
		this.ReplaceImgflag.Text = "自动更新封面（只更新本站无封面的小说，目标站无封面时不更新）";
		this.ReplaceImgflag.UseVisualStyleBackColor = true;
		this.ReplaceImgflag.CheckedChanged += new System.EventHandler(ReplaceImgflag_CheckedChanged);
		this.groupBox4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox4.Controls.Add(this.label4);
		this.groupBox4.Controls.Add(this.DelForTxt);
		this.groupBox4.Controls.Add(this.DelForHtml);
		this.groupBox4.Controls.Add(this.DuanImageCheck);
		this.groupBox4.Controls.Add(this.DuanImage);
		this.groupBox4.Location = new System.Drawing.Point(3, 264);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(796, 87);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "超级同步修复(高级服务)";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(25, 42);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(719, 12);
		this.label4.TabIndex = 27;
		this.label4.Text = "(说明：适用于站群同步，开启后当目标站章节数小于本地章节数或章节对比失败以及发现重复章节时，自动全章节覆盖，避免404错误)";
		this.DelForTxt.AutoSize = true;
		this.DelForTxt.Enabled = false;
		this.DelForTxt.Location = new System.Drawing.Point(150, 64);
		this.DelForTxt.Name = "DelForTxt";
		this.DelForTxt.Size = new System.Drawing.Size(138, 16);
		this.DelForTxt.TabIndex = 3;
		this.DelForTxt.Text = "清理无用txt文本文件";
		this.DelForTxt.UseVisualStyleBackColor = true;
		this.DelForHtml.AutoSize = true;
		this.DelForHtml.Enabled = false;
		this.DelForHtml.Location = new System.Drawing.Point(6, 64);
		this.DelForHtml.Name = "DelForHtml";
		this.DelForHtml.Size = new System.Drawing.Size(120, 16);
		this.DelForHtml.TabIndex = 2;
		this.DelForHtml.Text = "清理无用html页面";
		this.DelForHtml.UseVisualStyleBackColor = true;
		this.DuanImageCheck.AutoSize = true;
		this.DuanImageCheck.Location = new System.Drawing.Point(150, 20);
		this.DuanImageCheck.Name = "DuanImageCheck";
		this.DuanImageCheck.Size = new System.Drawing.Size(324, 16);
		this.DuanImageCheck.TabIndex = 1;
		this.DuanImageCheck.Text = "对比本地重复章节(本地有重复章节，自动启用超级同步)";
		this.DuanImageCheck.UseVisualStyleBackColor = true;
		this.DuanImage.AutoSize = true;
		this.DuanImage.Location = new System.Drawing.Point(6, 20);
		this.DuanImage.Name = "DuanImage";
		this.DuanImage.Size = new System.Drawing.Size(96, 16);
		this.DuanImage.TabIndex = 0;
		this.DuanImage.Text = "启用同步修复";
		this.DuanImage.UseVisualStyleBackColor = true;
		this.过滤设置_2.Controls.Add(this.groupBox3);
		this.过滤设置_2.Controls.Add(this.groupBox2);
		this.过滤设置_2.Controls.Add(this.groupBox1);
		this.过滤设置_2.Controls.Add(this.groupBox_2);
		this.过滤设置_2.Controls.Add(this.groupBox_3);
		this.过滤设置_2.Location = new System.Drawing.Point(4, 22);
		this.过滤设置_2.Name = "过滤设置_2";
		this.过滤设置_2.Size = new System.Drawing.Size(802, 347);
		this.过滤设置_2.TabIndex = 2;
		this.过滤设置_2.Text = "过滤设置";
		this.过滤设置_2.UseVisualStyleBackColor = true;
		this.groupBox_2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.groupBox_2.Controls.Add(this.FilterNovelTextBox);
		this.groupBox_2.Controls.Add(this.FilterNovelType);
		this.groupBox_2.Location = new System.Drawing.Point(6, 13);
		this.groupBox_2.Name = "groupBox_2";
		this.groupBox_2.Size = new System.Drawing.Size(206, 327);
		this.groupBox_2.TabIndex = 2;
		this.groupBox_2.TabStop = false;
		this.groupBox_2.Text = "限制小说";
		this.FilterNovelType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.FilterNovelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.FilterNovelType.FormattingEnabled = true;
		this.FilterNovelType.Items.AddRange(new object[3] { "不限制", "不采集限制小说", "只采集限制小说" });
		this.FilterNovelType.Location = new System.Drawing.Point(8, 20);
		this.FilterNovelType.Name = "FilterNovelType";
		this.FilterNovelType.Size = new System.Drawing.Size(192, 20);
		this.FilterNovelType.TabIndex = 7;
		this.采集动作_1.Controls.Add(this.设置2_5);
		this.采集动作_1.Controls.Add(this.设置_6);
		this.采集动作_1.Location = new System.Drawing.Point(4, 22);
		this.采集动作_1.Name = "采集动作_1";
		this.采集动作_1.Padding = new System.Windows.Forms.Padding(3);
		this.采集动作_1.Size = new System.Drawing.Size(802, 347);
		this.采集动作_1.TabIndex = 1;
		this.采集动作_1.Text = "采集动作";
		this.采集动作_1.UseVisualStyleBackColor = true;
		this.设置2_5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.设置2_5.Controls.Add(this.label3);
		this.设置2_5.Controls.Add(this.comboBox1);
		this.设置2_5.Controls.Add(this.label_32);
		this.设置2_5.Controls.Add(this.comboBox_7);
		this.设置2_5.Controls.Add(this.comboBox_6);
		this.设置2_5.Controls.Add(this.label_31);
		this.设置2_5.Controls.Add(this.label_28);
		this.设置2_5.Controls.Add(this.textBox_19);
		this.设置2_5.Controls.Add(this.label_27);
		this.设置2_5.Controls.Add(this.comboBox_5);
		this.设置2_5.Controls.Add(this.label_6);
		this.设置2_5.Controls.Add(this.comboBox_1);
		this.设置2_5.Controls.Add(this.label_7);
		this.设置2_5.Controls.Add(this.comboBox_2);
		this.设置2_5.Location = new System.Drawing.Point(6, 190);
		this.设置2_5.Name = "设置2_5";
		this.设置2_5.Size = new System.Drawing.Size(790, 150);
		this.设置2_5.TabIndex = 13;
		this.设置2_5.TabStop = false;
		this.设置2_5.Text = "章节处理设置";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(411, 55);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(113, 12);
		this.label3.TabIndex = 21;
		this.label3.Text = "下载图片失败处理：";
		this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Items.AddRange(new object[2] { "停止本书，跳入下一本", "跳过本章，继续采集下一个章" });
		this.comboBox1.Location = new System.Drawing.Point(200, 70);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(207, 20);
		this.comboBox1.TabIndex = 20;
		this.label_32.AutoSize = true;
		this.label_32.Location = new System.Drawing.Point(411, 17);
		this.label_32.Name = "label_32";
		this.label_32.Size = new System.Drawing.Size(89, 12);
		this.label_32.TabIndex = 19;
		this.label_32.Text = "章节排序方式：";
		this.comboBox_7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_7.FormattingEnabled = true;
		this.comboBox_7.Items.AddRange(new object[6] { "目标站顺序", "目标站倒序", "按章节ID顺序", "按章节ID倒序", "按章节名顺序", "按章节名倒序" });
		this.comboBox_7.Location = new System.Drawing.Point(413, 32);
		this.comboBox_7.Name = "comboBox_7";
		this.comboBox_7.Size = new System.Drawing.Size(120, 20);
		this.comboBox_7.TabIndex = 18;
		this.comboBox_6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_6.FormattingEnabled = true;
		this.comboBox_6.Items.AddRange(new object[5] { "只对比章节名", "对比分卷名+章节名", "智能对比 V1.2 Beta", "智能对比 V2.6 Beta", "智能对比 V3.2 Beta" });
		this.comboBox_6.Location = new System.Drawing.Point(6, 70);
		this.comboBox_6.Name = "comboBox_6";
		this.comboBox_6.Size = new System.Drawing.Size(188, 20);
		this.comboBox_6.TabIndex = 17;
		this.label_31.AutoSize = true;
		this.label_31.Location = new System.Drawing.Point(4, 55);
		this.label_31.Name = "label_31";
		this.label_31.Size = new System.Drawing.Size(113, 12);
		this.label_31.TabIndex = 16;
		this.label_31.Text = "重复章节判断方式：";
		this.label_28.AutoSize = true;
		this.label_28.Location = new System.Drawing.Point(538, 55);
		this.label_28.Name = "label_28";
		this.label_28.Size = new System.Drawing.Size(83, 12);
		this.label_28.TabIndex = 15;
		this.label_28.Text = "强制盗链URL：";
		this.textBox_19.Location = new System.Drawing.Point(540, 70);
		this.textBox_19.Name = "textBox_19";
		this.textBox_19.Size = new System.Drawing.Size(86, 21);
		this.textBox_19.TabIndex = 14;
		this.label_27.AutoSize = true;
		this.label_27.Location = new System.Drawing.Point(198, 55);
		this.label_27.Name = "label_27";
		this.label_27.Size = new System.Drawing.Size(113, 12);
		this.label_27.TabIndex = 12;
		this.label_27.Text = "重复章节处理方式：";
		this.comboBox_5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_5.FormattingEnabled = true;
		this.comboBox_5.Items.AddRange(new object[2] { "启用盗链模式", "跳入下一本" });
		this.comboBox_5.Location = new System.Drawing.Point(413, 70);
		this.comboBox_5.Name = "comboBox_5";
		this.comboBox_5.Size = new System.Drawing.Size(120, 20);
		this.comboBox_5.TabIndex = 11;
		this.label_6.AutoSize = true;
		this.label_6.Location = new System.Drawing.Point(198, 17);
		this.label_6.Name = "label_6";
		this.label_6.Size = new System.Drawing.Size(101, 12);
		this.label_6.TabIndex = 10;
		this.label_6.Text = "空章节处理方式：";
		this.comboBox_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_1.FormattingEnabled = true;
		this.comboBox_1.Items.AddRange(new object[3] { "停止本书，跳入下一本", "跳过本章，继续采集下一个章", "入库一个章节名，继续采集下一个章" });
		this.comboBox_1.Location = new System.Drawing.Point(200, 32);
		this.comboBox_1.Name = "comboBox_1";
		this.comboBox_1.Size = new System.Drawing.Size(207, 20);
		this.comboBox_1.TabIndex = 1;
		this.label_7.AutoSize = true;
		this.label_7.Location = new System.Drawing.Point(4, 17);
		this.label_7.Name = "label_7";
		this.label_7.Size = new System.Drawing.Size(113, 12);
		this.label_7.TabIndex = 9;
		this.label_7.Text = "最新章节对比方式：";
		this.comboBox_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_2.FormattingEnabled = true;
		this.comboBox_2.Items.AddRange(new object[5] { "只对比章节名", "对比分卷名+章节名", "智能对比 V1.2 Beta", "得分对比 V2.6 Beta", "得分对比 V3.2 Beta" });
		this.comboBox_2.Location = new System.Drawing.Point(6, 32);
		this.comboBox_2.Name = "comboBox_2";
		this.comboBox_2.Size = new System.Drawing.Size(188, 20);
		this.comboBox_2.TabIndex = 0;
		this.设置_6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.设置_6.Controls.Add(this.label1);
		this.设置_6.Controls.Add(this.label2);
		this.设置_6.Controls.Add(this.DonnotCollectLastChapterNum);
		this.设置_6.Controls.Add(this.label_29);
		this.设置_6.Controls.Add(this.不采小于字符设置_6);
		this.设置_6.Controls.Add(this.不采小于字符前_30);
		this.设置_6.Controls.Add(this.label_2);
		this.设置_6.Controls.Add(this.numericUpDown_1);
		this.设置_6.Controls.Add(this.label_3);
		this.设置_6.Controls.Add(this.不采小于章节后段_4);
		this.设置_6.Controls.Add(this.不采集小于章节_5);
		this.设置_6.Controls.Add(this.不采小于章节设置_2);
		this.设置_6.Controls.Add(this.全本必采_15);
		this.设置_6.Controls.Add(this.隐藏更新小说);
		this.设置_6.Controls.Add(this.栓测重复章节_22);
		this.设置_6.Controls.Add(this.添加分卷判断_21);
		this.设置_6.Controls.Add(this.强制清空_18);
		this.设置_6.Controls.Add(this.识别方式_17);
		this.设置_6.Controls.Add(this.添加新书_0);
		this.设置_6.Controls.Add(this.内容自动排版_16);
		this.设置_6.Controls.Add(this.更新连载_1);
		this.设置_6.Controls.Add(this.调用页面_10);
		this.设置_6.Controls.Add(this.不处理完结_8);
		this.设置_6.Controls.Add(this.只采文字章_9);
		this.设置_6.Controls.Add(this.清空重采_6);
		this.设置_6.Controls.Add(this.下载图片章节_5);
		this.设置_6.Controls.Add(this.禁止添加分卷_4);
		this.设置_6.Location = new System.Drawing.Point(6, 6);
		this.设置_6.Name = "设置_6";
		this.设置_6.Size = new System.Drawing.Size(790, 178);
		this.设置_6.TabIndex = 12;
		this.设置_6.TabStop = false;
		this.设置_6.Text = "设置";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(467, 154);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(29, 12);
		this.label1.TabIndex = 27;
		this.label1.Text = "章节";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(333, 154);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(65, 12);
		this.label2.TabIndex = 26;
		this.label2.Text = "不采集倒数";
		this.DonnotCollectLastChapterNum.Location = new System.Drawing.Point(404, 151);
		this.DonnotCollectLastChapterNum.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.DonnotCollectLastChapterNum.Name = "DonnotCollectLastChapterNum";
		this.DonnotCollectLastChapterNum.Size = new System.Drawing.Size(57, 21);
		this.DonnotCollectLastChapterNum.TabIndex = 25;
		this.label_29.AutoSize = true;
		this.label_29.Location = new System.Drawing.Point(467, 132);
		this.label_29.Name = "label_29";
		this.label_29.Size = new System.Drawing.Size(77, 12);
		this.label_29.TabIndex = 24;
		this.label_29.Text = "个字符的章节";
		this.不采小于字符设置_6.Location = new System.Drawing.Point(404, 127);
		this.不采小于字符设置_6.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.不采小于字符设置_6.Name = "不采小于字符设置_6";
		this.不采小于字符设置_6.Size = new System.Drawing.Size(57, 21);
		this.不采小于字符设置_6.TabIndex = 22;
		this.不采小于字符前_30.AutoSize = true;
		this.不采小于字符前_30.Location = new System.Drawing.Point(333, 132);
		this.不采小于字符前_30.Name = "不采小于字符前_30";
		this.不采小于字符前_30.Size = new System.Drawing.Size(65, 12);
		this.不采小于字符前_30.TabIndex = 23;
		this.不采小于字符前_30.Text = "不采集小于";
		this.label_2.AutoSize = true;
		this.label_2.Location = new System.Drawing.Point(176, 154);
		this.label_2.Name = "label_2";
		this.label_2.Size = new System.Drawing.Size(137, 12);
		this.label_2.TabIndex = 21;
		this.label_2.Text = "的小说(需要更新的小说)";
		this.numericUpDown_1.Location = new System.Drawing.Point(113, 151);
		this.numericUpDown_1.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numericUpDown_1.Name = "numericUpDown_1";
		this.numericUpDown_1.Size = new System.Drawing.Size(57, 21);
		this.numericUpDown_1.TabIndex = 16;
		this.label_3.AutoSize = true;
		this.label_3.Location = new System.Drawing.Point(6, 154);
		this.label_3.Name = "label_3";
		this.label_3.Size = new System.Drawing.Size(101, 12);
		this.label_3.TabIndex = 20;
		this.label_3.Text = "不更新章节数超过";
		this.不采小于章节后段_4.AutoSize = true;
		this.不采小于章节后段_4.Location = new System.Drawing.Point(176, 132);
		this.不采小于章节后段_4.Name = "不采小于章节后段_4";
		this.不采小于章节后段_4.Size = new System.Drawing.Size(125, 12);
		this.不采小于章节后段_4.TabIndex = 19;
		this.不采小于章节后段_4.Text = "的小说(只对新书而言)";
		this.不采集小于章节_5.AutoSize = true;
		this.不采集小于章节_5.Location = new System.Drawing.Point(6, 132);
		this.不采集小于章节_5.Name = "不采集小于章节_5";
		this.不采集小于章节_5.Size = new System.Drawing.Size(101, 12);
		this.不采集小于章节_5.TabIndex = 18;
		this.不采集小于章节_5.Text = "不采集章节数小于";
		this.不采小于章节设置_2.Location = new System.Drawing.Point(113, 127);
		this.不采小于章节设置_2.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.不采小于章节设置_2.Name = "不采小于章节设置_2";
		this.不采小于章节设置_2.Size = new System.Drawing.Size(57, 21);
		this.不采小于章节设置_2.TabIndex = 15;
		this.全本必采_15.AutoSize = true;
		this.全本必采_15.ForeColor = System.Drawing.Color.Red;
		this.全本必采_15.Location = new System.Drawing.Point(6, 108);
		this.全本必采_15.Name = "全本必采_15";
		this.全本必采_15.Size = new System.Drawing.Size(168, 16);
		this.全本必采_15.TabIndex = 17;
		this.全本必采_15.Text = "全本必采(不考虑以下情况)";
		this.全本必采_15.UseVisualStyleBackColor = true;
		this.隐藏更新小说.AutoSize = true;
		this.隐藏更新小说.Location = new System.Drawing.Point(426, 86);
		this.隐藏更新小说.Name = "隐藏更新小说";
		this.隐藏更新小说.Size = new System.Drawing.Size(96, 16);
		this.隐藏更新小说.TabIndex = 14;
		this.隐藏更新小说.Text = "隐藏更新小说";
		this.隐藏更新小说.UseVisualStyleBackColor = true;
		this.添加分卷判断_21.AutoSize = true;
		this.添加分卷判断_21.Location = new System.Drawing.Point(200, 64);
		this.添加分卷判断_21.Name = "添加分卷判断_21";
		this.添加分卷判断_21.Size = new System.Drawing.Size(198, 16);
		this.添加分卷判断_21.TabIndex = 12;
		this.添加分卷判断_21.Text = "遇到“１一1壹”才判断添加分卷";
		this.添加分卷判断_21.UseVisualStyleBackColor = true;
		this.识别方式_17.AutoSize = true;
		this.识别方式_17.Location = new System.Drawing.Point(6, 64);
		this.识别方式_17.Name = "识别方式_17";
		this.识别方式_17.Size = new System.Drawing.Size(150, 16);
		this.识别方式_17.TabIndex = 10;
		this.识别方式_17.Text = "以\"书名+作者\"识别书籍";
		this.识别方式_17.UseVisualStyleBackColor = true;
		this.调用页面_10.AutoSize = true;
		this.调用页面_10.Location = new System.Drawing.Point(200, 108);
		this.调用页面_10.Name = "调用页面_10";
		this.调用页面_10.Size = new System.Drawing.Size(132, 16);
		this.调用页面_10.TabIndex = 8;
		this.调用页面_10.Text = "单次循环后调用页面";
		this.调用页面_10.UseVisualStyleBackColor = true;
		this.只采文字章_9.AutoSize = true;
		this.只采文字章_9.Location = new System.Drawing.Point(6, 42);
		this.只采文字章_9.Name = "只采文字章_9";
		this.只采文字章_9.Size = new System.Drawing.Size(108, 16);
		this.只采文字章_9.TabIndex = 9;
		this.只采文字章_9.Text = "只采集文字章节";
		this.只采文字章_9.UseVisualStyleBackColor = true;
		this.采集模式_0.AllowDrop = true;
		this.采集模式_0.Controls.Add(this.其他站编码_18);
		this.采集模式_0.Controls.Add(this.提取其他站小说名规则_16);
		this.采集模式_0.Controls.Add(this.其他站列表URL_17);
		this.采集模式_0.Controls.Add(this.本站自定义书号ID_0);
		this.采集模式_0.Controls.Add(this.本站书号结束_1);
		this.采集模式_0.Controls.Add(this.本站书号开始_2);
		this.采集模式_0.Controls.Add(this.采集最新列表框_3);
		this.采集模式_0.Controls.Add(this.目标站自定义书号ID_4);
		this.采集模式_0.Controls.Add(this.目标站书号结束_5);
		this.采集模式_0.Controls.Add(this.目标站书号开始_6);
		this.采集模式_0.Controls.Add(this.其他站列表搜索_5);
		this.采集模式_0.Controls.Add(this.采集方案_4);
		this.采集模式_0.Controls.Add(this.采集方案_20);
		this.采集模式_0.Controls.Add(this.采集规则_1);
		this.采集模式_0.Controls.Add(this.采集规则_0);
		this.采集模式_0.Controls.Add(this.本站自定义书号_3);
		this.采集模式_0.Controls.Add(this.本站书号_4);
		this.采集模式_0.Controls.Add(this.采集最新列表_0);
		this.采集模式_0.Controls.Add(this.目标站自定义书号_1);
		this.采集模式_0.Controls.Add(this.目标站书号_2);
		this.采集模式_0.Controls.Add(this.循环间隔时间_0);
		this.采集模式_0.Controls.Add(this.循环间隔时间选择_0);
		this.采集模式_0.Controls.Add(this.循环采集_2);
		this.采集模式_0.Controls.Add(this.日志记录_3);
		this.采集模式_0.Location = new System.Drawing.Point(4, 22);
		this.采集模式_0.Name = "采集模式_0";
		this.采集模式_0.Padding = new System.Windows.Forms.Padding(3);
		this.采集模式_0.Size = new System.Drawing.Size(802, 347);
		this.采集模式_0.TabIndex = 0;
		this.采集模式_0.Text = "采集模式";
		this.采集模式_0.UseVisualStyleBackColor = true;
		this.其他站列表搜索_5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.其他站列表搜索_5.AutoSize = true;
		this.其他站列表搜索_5.Location = new System.Drawing.Point(8, 291);
		this.其他站列表搜索_5.Name = "其他站列表搜索_5";
		this.其他站列表搜索_5.Size = new System.Drawing.Size(119, 16);
		this.其他站列表搜索_5.TabIndex = 33;
		this.其他站列表搜索_5.TabStop = true;
		this.其他站列表搜索_5.Text = "按其他站列表搜索";
		this.其他站列表搜索_5.UseVisualStyleBackColor = true;
		this.采集方案_4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.采集方案_4.FormattingEnabled = true;
		this.采集方案_4.Items.AddRange(new object[1] { "TaskConfig.xml" });
		this.采集方案_4.Location = new System.Drawing.Point(77, 6);
		this.采集方案_4.Name = "采集方案_4";
		this.采集方案_4.Size = new System.Drawing.Size(180, 20);
		this.采集方案_4.TabIndex = 32;
		this.采集方案_4.SelectedIndexChanged += new System.EventHandler(comboBox_4_SelectedIndexChanged);
		this.采集方案_20.AutoSize = true;
		this.采集方案_20.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.采集方案_20.Location = new System.Drawing.Point(6, 9);
		this.采集方案_20.Name = "采集方案_20";
		this.采集方案_20.Size = new System.Drawing.Size(65, 12);
		this.采集方案_20.TabIndex = 31;
		this.采集方案_20.Text = "采集方案：";
		this.采集规则_1.AutoSize = true;
		this.采集规则_1.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.采集规则_1.Location = new System.Drawing.Point(263, 9);
		this.采集规则_1.Name = "采集规则_1";
		this.采集规则_1.Size = new System.Drawing.Size(65, 12);
		this.采集规则_1.TabIndex = 30;
		this.采集规则_1.Text = "采集规则：";
		this.采集规则_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.采集规则_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.采集规则_0.FormattingEnabled = true;
		this.采集规则_0.Location = new System.Drawing.Point(334, 6);
		this.采集规则_0.Name = "采集规则_0";
		this.采集规则_0.Size = new System.Drawing.Size(462, 20);
		this.采集规则_0.TabIndex = 0;
		this.采集规则_0.SelectedIndexChanged += new System.EventHandler(comboBox_0_SelectedIndexChanged);
		this.本站自定义书号_3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.本站自定义书号_3.AutoSize = true;
		this.本站自定义书号_3.Location = new System.Drawing.Point(6, 265);
		this.本站自定义书号_3.Name = "本站自定义书号_3";
		this.本站自定义书号_3.Size = new System.Drawing.Size(131, 16);
		this.本站自定义书号_3.TabIndex = 12;
		this.本站自定义书号_3.TabStop = true;
		this.本站自定义书号_3.Text = "按自己站自定义编号";
		this.本站自定义书号_3.UseVisualStyleBackColor = true;
		this.本站书号_4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.本站书号_4.AutoSize = true;
		this.本站书号_4.Location = new System.Drawing.Point(6, 238);
		this.本站书号_4.Name = "本站书号_4";
		this.本站书号_4.Size = new System.Drawing.Size(119, 16);
		this.本站书号_4.TabIndex = 9;
		this.本站书号_4.TabStop = true;
		this.本站书号_4.Text = "按自己站编号顺序";
		this.本站书号_4.UseVisualStyleBackColor = true;
		this.采集最新列表_0.AutoSize = true;
		this.采集最新列表_0.Location = new System.Drawing.Point(6, 32);
		this.采集最新列表_0.Name = "采集最新列表_0";
		this.采集最新列表_0.Size = new System.Drawing.Size(335, 16);
		this.采集最新列表_0.TabIndex = 1;
		this.采集最新列表_0.TabStop = true;
		this.采集最新列表_0.Text = "按目标站页面获得编号，一般监控最新列表，一个地址一行";
		this.采集最新列表_0.UseVisualStyleBackColor = true;
		this.目标站自定义书号_1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.目标站自定义书号_1.AutoSize = true;
		this.目标站自定义书号_1.Location = new System.Drawing.Point(6, 211);
		this.目标站自定义书号_1.Name = "目标站自定义书号_1";
		this.目标站自定义书号_1.Size = new System.Drawing.Size(131, 16);
		this.目标站自定义书号_1.TabIndex = 6;
		this.目标站自定义书号_1.TabStop = true;
		this.目标站自定义书号_1.Text = "按目标站自定义编号";
		this.目标站自定义书号_1.UseVisualStyleBackColor = true;
		this.目标站书号_2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.目标站书号_2.AutoSize = true;
		this.目标站书号_2.Location = new System.Drawing.Point(6, 184);
		this.目标站书号_2.Name = "目标站书号_2";
		this.目标站书号_2.Size = new System.Drawing.Size(119, 16);
		this.目标站书号_2.TabIndex = 3;
		this.目标站书号_2.TabStop = true;
		this.目标站书号_2.Text = "按目标站编号顺序";
		this.目标站书号_2.UseVisualStyleBackColor = true;
		this.循环间隔时间_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.循环间隔时间_0.AutoSize = true;
		this.循环间隔时间_0.Location = new System.Drawing.Point(615, 321);
		this.循环间隔时间_0.Name = "循环间隔时间_0";
		this.循环间隔时间_0.Size = new System.Drawing.Size(125, 12);
		this.循环间隔时间_0.TabIndex = 18;
		this.循环间隔时间_0.Text = "循环间隔时间(分钟)：";
		this.tabControl_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tabControl_0.Controls.Add(this.采集模式_0);
		this.tabControl_0.Controls.Add(this.采集动作_1);
		this.tabControl_0.Controls.Add(this.过滤设置_2);
		this.tabControl_0.Controls.Add(this.高级设置1);
		this.tabControl_0.Controls.Add(this.采集进度_4);
		this.tabControl_0.Controls.Add(this.代理设定2);
		this.tabControl_0.Location = new System.Drawing.Point(12, 12);
		this.tabControl_0.Name = "tabControl_0";
		this.tabControl_0.SelectedIndex = 0;
		this.tabControl_0.Size = new System.Drawing.Size(810, 373);
		this.tabControl_0.TabIndex = 17;
		base.ClientSize = new System.Drawing.Size(834, 426);
		base.Controls.Add(this.测试网速);
		base.Controls.Add(this.注意得示_16);
		base.Controls.Add(this.采集方案_1);
		base.Controls.Add(this.tabControl_0);
		base.Controls.Add(this.开始);
		base.Controls.Add(this.停止);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "自动采集模式";
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "标准采集模式";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CollectAuto_FormClosing);
		base.Load += new System.EventHandler(CollectAuto_Load);
		this.TargetMenuStrip.ResumeLayout(false);
		this.groupBox_3.ResumeLayout(false);
		this.groupBox_3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.循环间隔时间选择_0).EndInit();
		this.SaveMenuStrip.ResumeLayout(false);
		this.代理设定2.ResumeLayout(false);
		this.groupBox8.ResumeLayout(false);
		this.groupBox_7.ResumeLayout(false);
		this.groupBox_7.PerformLayout();
		this.采集进度_4.ResumeLayout(false);
		this.采集进度_4.PerformLayout();
		this.groupBox_4.ResumeLayout(false);
		this.groupBox_4.PerformLayout();
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		this.UpgradeVisibleDelayGroup();
		this.groupBox_9.ResumeLayout(false);
		this.groupBox_9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_5).EndInit();
		this.高级设置1.ResumeLayout(false);
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ReplaceChapterTimeMax).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ReplaceChapterTimeMin).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ReplaceChapterNameNun).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ReplaceChapterNun).EndInit();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ReplaceSortId).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.过滤设置_2.ResumeLayout(false);
		this.groupBox_2.ResumeLayout(false);
		this.groupBox_2.PerformLayout();
		this.采集动作_1.ResumeLayout(false);
		this.设置2_5.ResumeLayout(false);
		this.设置2_5.PerformLayout();
		this.设置_6.ResumeLayout(false);
		this.设置_6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DonnotCollectLastChapterNum).EndInit();
		((System.ComponentModel.ISupportInitialize)this.不采小于字符设置_6).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.不采小于章节设置_2).EndInit();
		this.采集模式_0.ResumeLayout(false);
		this.采集模式_0.PerformLayout();
		this.tabControl_0.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		string text = "";
		int num = 80;
		if (listView1.SelectedItems.Count > 0)
		{
			text = listView1.SelectedItems[0].SubItems[0].Text.Trim();
			num = int.Parse(listView1.SelectedItems[0].SubItems[1].Text.Trim());
			chkEnableProxy.Checked = true;
			textBox_11.Text = num.ToString();
			textBox_12.Text = text;
			tInfo.Proxy = true;
			tInfo.ProxyHost = text;
			tInfo.ProxyPort = num;
		}
	}

	private void LoginWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		if (!tInfo.Proxy)
		{
			return;
		}
		Random random = new Random();
		string text = random.Next(10000000, 99999999).ToString();
		HttpClient httpClient = new HttpClient
		{
			UriString = "http://www.xicidaili.com/wn/" + text,
			Encoding = FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk")
		};
		HttpClient httpClient2 = httpClient;
		string stringWork = httpClient2.GetStringWork();
		string text2 = string.Format("{0}(?<getcontent>[\\s|\\S]+?){1}", "<table id=\"ip_list\">", "</table>");
		Match match = SecurityUtil.RegexsMatch(stringWork, text2);
		if (match.Success)
		{
			stringWork = match.Groups["getcontent"].Value;
			httpClient2.UriString = "http://www.xicidaili.com/nn/" + text;
			string stringWork2 = httpClient2.GetStringWork();
			text2 = string.Format("{0}(?<getcontent>[\\s|\\S]+?){1}", "<table id=\"ip_list\">", "</table>");
			match = SecurityUtil.RegexsMatch(stringWork2, text2);
			if (!match.Success)
			{
				MessageBox.Show("获取代理ip错误,请联系管理员");
				return;
			}
			stringWork2 = match.Groups["getcontent"].Value;
			stringWork += stringWork2;
			MatchCollection matchCollection = null;
			if (stringWork != string.Empty)
			{
				text2 = string.Format("{0}(?<getcontent>[\\s|\\S]+?){1}", "<tr class=\"\\w+\">", "</tr>");
				matchCollection = SecurityUtil.RegexsMatches(stringWork, text2);
			}
			if (matchCollection.Count <= 0)
			{
				return;
			}
			ListViewItem[] items = new ListViewItem[matchCollection.Count];
			int num = 0;
			foreach (Match item in matchCollection)
			{
				stringWork = item.Value.Replace("\r", "").Replace("\n", "");
				text2 = string.Format("{0}(?<getcontent>[\\s|\\S]+?){1}", "<td\\w+\\>", "</td>");
				matchCollection = SecurityUtil.RegexsMatches(stringWork, text2);
				if (matchCollection.Count == 9 && SecurityUtil.IsIP(SecurityUtil.NoHtml(matchCollection[1].Groups["getcontent"].Value.Trim())))
				{
					items[num] = new ListViewItem(SecurityUtil.NoHtml(matchCollection[1].Groups["getcontent"].Value.Trim()));
					items[num].SubItems.Add(SecurityUtil.NoHtml(matchCollection[2].Groups["getcontent"].Value.Trim()));
					items[num].SubItems.Add(SecurityUtil.NoHtml(matchCollection[3].Groups["getcontent"].Value.Trim()));
					text2 = string.Format("{0}(?<getcontent>[\\s|\\S]+?){1}", "<div title=\"", "\"");
					match = SecurityUtil.RegexsMatch(matchCollection[6].Groups["getcontent"].Value.Trim(), text2);
					if (match.Success)
					{
						items[num].SubItems.Add(SecurityUtil.NoHtml(match.Groups["getcontent"].Value.Trim()));
					}
					else
					{
						items[num].SubItems.Add("未知");
					}
					items[num].SubItems.Add("-");
					items[num].SubItems.Add("-");
				}
				num++;
			}
			Invoke((MethodInvoker)delegate
			{
				listView1.Items.Clear();
				ProgressiveListViewLoader.ReplaceItems(listView1, items);
			});
		}
		else
		{
			MessageBox.Show("获取代理ip错误,请联系管理员");
		}
	}

	private void LoginWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
	}

	private void LoginWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
	}

	private void method_0(string[] string_3)
	{
		for (int i = 0; i < string_3.Length; i++)
		{
			if (AutoWorker.CancellationPending)
			{
				break;
			}
			AutoWorker.ReportProgress(3, i + 1);
			NovelInfo novelInfo = new NovelInfo
			{
				Name = string_3[i]
			};
			NovelInfo novelInfo_ = novelInfo;
			CollectNovel(novelInfo_);
		}
	}

	private void method_1(string[] string_3, bool bool_1)
	{
		for (int i = 0; i < string_3.Length; i++)
		{
			if (AutoWorker.CancellationPending)
			{
				break;
			}
			AutoWorker.ReportProgress(3, i + 1);
			NovelInfo novelInfo = new NovelInfo();
			if (bool_1)
			{
				novelInfo.GetID = string_3[i];
			}
			else
			{
				novelInfo.PutID = Convert.ToInt32(string_3[i]);
			}
			CollectNovel(novelInfo);
		}
	}

	private void method_2(int int_0, int int_1, bool bool_1)
	{
		for (int i = int_0; i <= int_1; i++)
		{
			if (AutoWorker.CancellationPending)
			{
				break;
			}
			AutoWorker.ReportProgress(3, i - int_0 + 1);
			NovelInfo novelInfo = new NovelInfo();
			if (bool_1)
			{
				novelInfo.GetID = i.ToString();
			}
			else
			{
				novelInfo.PutID = i;
			}
			CollectNovel(novelInfo);
		}
	}

	private void InitializeRequestSchedulingControls()
	{
		// Kept for compatibility with old constructor flow; the visible controls now live in groupBox_9.
	}

	private void UpgradeVisibleDelayGroup()
	{
		Label header = new Label { Text = "类型      最小      最大", Location = new Point(10, 20), Size = new Size(210, 12) };
		groupBox_9.Controls.Add(header);
		requestListWaitMinBox = CreateScheduleNumber(72, 40);
		requestListWaitMaxBox = CreateScheduleNumber(145, 40);
		requestNovelWaitMinBox = numericUpDown_5;
		requestNovelWaitMaxBox = CreateScheduleNumber(145, 65);
		requestIndexWaitMinBox = numericUpDown_4;
		requestIndexWaitMaxBox = CreateScheduleNumber(145, 90);
		requestChapterWaitMinBox = numericUpDown_3;
		requestChapterWaitMaxBox = CreateScheduleNumber(145, 115);
		AddScheduleRow("列表", 40, requestListWaitMinBox, requestListWaitMaxBox);
		RelocateExistingDelayRow(label_26, numericUpDown_5, label_23, "信息", 65);
		RelocateExistingDelayRow(label_25, numericUpDown_4, label_22, "目录", 90);
		RelocateExistingDelayRow(label_24, numericUpDown_3, label_21, "正文", 115);
		groupBox_9.Controls.Add(requestNovelWaitMaxBox);
		groupBox_9.Controls.Add(requestIndexWaitMaxBox);
		groupBox_9.Controls.Add(requestChapterWaitMaxBox);
		requestBackoffBox = new CheckBox { Text = "失败退避", Location = new Point(10, 143), Size = new Size(78, 18), Checked = true };
		groupBox_9.Controls.Add(requestBackoffBox);
		groupBox_9.Controls.Add(new Label { Text = "并发", Location = new Point(98, 146), Size = new Size(34, 12) });
		sameHostConcurrencyBox = CreateScheduleNumber(132, 141);
		sameHostConcurrencyBox.Minimum = 1;
		sameHostConcurrencyBox.Maximum = 16;
		sameHostConcurrencyBox.Value = 1;
		groupBox_9.Controls.Add(sameHostConcurrencyBox);
		groupBox_9.Controls.Add(new Label { Text = "UA", Location = new Point(10, 169), Size = new Size(20, 12) });
		userAgentModeBox = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(34, 165), Size = new Size(185, 20) };
		userAgentModeBox.Items.AddRange(new object[] { "固定全局 UA", "随机 PC 浏览器 UA", "随机手机浏览器 UA", "随机爬虫 UA" });
		userAgentModeBox.SelectedIndex = 0;
		groupBox_9.Controls.Add(userAgentModeBox);
	}

	private NumericUpDown CreateScheduleNumber(int x, int y)
	{
		return new NumericUpDown
		{
			Location = new Point(x, y),
			Size = new Size(60, 21),
			Maximum = 600000,
			Minimum = 0
		};
	}

	private void AddScheduleRow(string label, int y, NumericUpDown minBox, NumericUpDown maxBox)
	{
		groupBox_9.Controls.Add(new Label { Text = label, Location = new Point(10, y + 4), Size = new Size(48, 12) });
		groupBox_9.Controls.Add(minBox);
		groupBox_9.Controls.Add(maxBox);
	}

	private static void RelocateExistingDelayRow(Label label, NumericUpDown minBox, Label unitLabel, string text, int y)
	{
		label.Text = text;
		label.Location = new Point(10, y + 4);
		label.Size = new Size(48, 12);
		minBox.Location = new Point(72, y);
		minBox.Size = new Size(60, 21);
		minBox.Maximum = 600000;
		unitLabel.Text = "";
		unitLabel.Visible = false;
	}

	private static void NormalizeDelayBoxes(NumericUpDown minBox, NumericUpDown maxBox)
	{
		if (minBox.Value > maxBox.Value)
		{
			var value = minBox.Value;
			minBox.Value = maxBox.Value;
			maxBox.Value = value;
		}
	}

	private void LoadRequestSchedulingToUi(TaskConfigInfo task)
	{
		requestListWaitMinBox.Value = ClampScheduleValue(task.RequestListWaitMin);
		requestListWaitMaxBox.Value = ClampScheduleValue(task.RequestListWaitMax);
		requestNovelWaitMinBox.Value = ClampScheduleValue(task.RequestNovelWaitMin);
		requestNovelWaitMaxBox.Value = ClampScheduleValue(task.RequestNovelWaitMax);
		requestIndexWaitMinBox.Value = ClampScheduleValue(task.RequestIndexWaitMin);
		requestIndexWaitMaxBox.Value = ClampScheduleValue(task.RequestIndexWaitMax);
		requestChapterWaitMinBox.Value = ClampScheduleValue(task.RequestChapterWaitMin);
		requestChapterWaitMaxBox.Value = ClampScheduleValue(task.RequestChapterWaitMax);
		sameHostConcurrencyBox.Value = ClampScheduleValue(task.SameHostConcurrencyLimit, 1, 16);
		requestBackoffBox.Checked = task.RequestBackoffEnabled;
		userAgentModeBox.SelectedIndex = UserAgentModeToUi(task.UserAgentMode);
	}

	private void SaveRequestSchedulingFromUi(TaskConfigInfo task)
	{
		NormalizeRequestScheduleUi();
		task.RequestListWaitMin = Convert.ToInt32(requestListWaitMinBox.Value);
		task.RequestListWaitMax = Convert.ToInt32(requestListWaitMaxBox.Value);
		task.RequestNovelWaitMin = Convert.ToInt32(requestNovelWaitMinBox.Value);
		task.RequestNovelWaitMax = Convert.ToInt32(requestNovelWaitMaxBox.Value);
		task.RequestIndexWaitMin = Convert.ToInt32(requestIndexWaitMinBox.Value);
		task.RequestIndexWaitMax = Convert.ToInt32(requestIndexWaitMaxBox.Value);
		task.RequestChapterWaitMin = Convert.ToInt32(requestChapterWaitMinBox.Value);
		task.RequestChapterWaitMax = Convert.ToInt32(requestChapterWaitMaxBox.Value);
		task.NovelUrlWait = task.RequestNovelWaitMin;
		task.IndexUrlWait = task.RequestIndexWaitMin;
		task.ChapterUrlWait = task.RequestChapterWaitMin;
		task.SameHostConcurrencyLimit = Convert.ToInt32(sameHostConcurrencyBox.Value);
		task.RequestBackoffEnabled = requestBackoffBox.Checked;
		task.UserAgentMode = UiToUserAgentMode(userAgentModeBox);
	}

	private void NormalizeRequestScheduleUi()
	{
		NormalizeDelayBoxes(requestListWaitMinBox, requestListWaitMaxBox);
		NormalizeDelayBoxes(requestNovelWaitMinBox, requestNovelWaitMaxBox);
		NormalizeDelayBoxes(requestIndexWaitMinBox, requestIndexWaitMaxBox);
		NormalizeDelayBoxes(requestChapterWaitMinBox, requestChapterWaitMaxBox);
	}

	private static decimal ClampScheduleValue(int value, int minimum = 0, int maximum = 600000)
	{
		if (value < minimum)
		{
			return minimum;
		}
		if (value > maximum)
		{
			return maximum;
		}
		return value;
	}

	private static string UiToUserAgentMode(ComboBox box)
	{
		return box.SelectedIndex switch
		{
			1 => "DesktopBrowserRandom",
			2 => "MobileBrowserRandom",
			3 => "CrawlerRandom",
			_ => "Fixed"
		};
	}

	private static int UserAgentModeToUi(string mode)
	{
		if (string.Equals(mode, "DesktopBrowserRandom", StringComparison.OrdinalIgnoreCase))
		{
			return 1;
		}
		if (string.Equals(mode, "MobileBrowserRandom", StringComparison.OrdinalIgnoreCase))
		{
			return 2;
		}
		if (string.Equals(mode, "CrawlerRandom", StringComparison.OrdinalIgnoreCase))
		{
			return 3;
		}
		return 0;
	}

	private static void ApplyFriendlyDelay(TaskConfigInfo config, RequestKind kind)
	{
		(int minDelay, int maxDelay) = RequestDelayProfile.GetDelay(config, kind);
		HostRequestThrottle.Wait("*", minDelay, maxDelay, kind.ToString());
	}

	private static void ShowErrorMessage(string message)
	{
		using var messageForm = new MessageForm
		{
			Text = "错误提示"
		};
		messageForm.MessageText.Text = message;
		messageForm.ShowDialog();
	}

	private void method_4()
	{
		try
		{
			采集规则_0.Text = tInfo.RuleFile;
			switch (tInfo.RadioBy)
			{
			case "GetListUrl":
				采集最新列表_0.Checked = true;
				break;
			case "GetOrderId":
				目标站书号_2.Checked = true;
				break;
			case "GetSinceId":
				目标站自定义书号_1.Checked = true;
				break;
			case "PutOrderId":
				本站书号_4.Checked = true;
				break;
			case "OtherListUrl":
				其他站列表搜索_5.Checked = true;
				break;
			case "PutSinceId":
				本站自定义书号_3.Checked = true;
				break;
			}
			if (tInfo.GetListUrl != null)
			{
				采集最新列表框_3.Text = string.Join("\r\n", tInfo.GetListUrl);
			}
			目标站书号开始_6.Text = tInfo.GetOrderMinId.ToString();
			目标站书号结束_5.Text = tInfo.GetOrderMaxId.ToString();
			if (tInfo.GetSinceId != null)
			{
				目标站自定义书号ID_4.Text = string.Join(",", tInfo.GetSinceId);
			}
			本站书号开始_2.Text = tInfo.PutOrderMinId.ToString();
			本站书号结束_1.Text = tInfo.PutOrderMaxId.ToString();
			if (tInfo.PutSinceId != null)
			{
				本站自定义书号ID_0.Text = string.Join(",", tInfo.PutSinceId);
			}
			其他站列表URL_17.Text = tInfo.OtherListUrl;
			提取其他站小说名规则_16.Text = tInfo.OtherRegex;
			其他站编码_18.Text = tInfo.OtherEncoding;
			日志记录_3.Checked = tInfo.Log;
			循环采集_2.Checked = tInfo.Timing;
			循环间隔时间选择_0.Value = tInfo.Interval;
			添加新书_0.Checked = tInfo.NewBook;
			更新连载_1.Checked = tInfo.OldBook;
			不处理完结_8.Checked = tInfo.FilterFinish;
			内容自动排版_16.Checked = tInfo.Typesetting;
			强制清空_18.Checked = tInfo.CompulsoryDeleteChapter;
			清空重采_6.Checked = tInfo.DeleteChapter;
			下载图片章节_5.Checked = tInfo.DownImage;
			禁止添加分卷_4.Checked = tInfo.ProhibitionVolume;
			识别方式_17.Checked = tInfo.NameAndAuthor;
			添加分卷判断_21.Checked = tInfo.CheckVolume;
			调用页面_10.Checked = tInfo.UpdateDefault;
			只采文字章_9.Checked = tInfo.OnlyText;
			栓测重复章节_22.Checked = tInfo.CheckRepeat;
			comboBox_2.SelectedIndex = tInfo.EqualsChapter;
			comboBox_6.SelectedIndex = tInfo.RepeatChapter;
			comboBox1.SelectedIndex = tInfo.RepeatChapterHandle;
			comboBox_1.SelectedIndex = tInfo.EmptyChapter;
			comboBox_5.SelectedIndex = tInfo.DownImageError;
			textBox_19.Text = tInfo.UnDownUrl;
			comboBox_7.SelectedIndex = tInfo.OrderChapter;
			不采小于字符设置_6.Value = tInfo.MinChapterTextLength;
			不采小于章节设置_2.Value = tInfo.MinChapterNum;
			numericUpDown_1.Value = tInfo.FindMaxChapterNum;
			全本必采_15.Checked = tInfo.Finish;
			隐藏更新小说.Checked = tInfo.Hidebook;
			DonnotCollectLastChapterNum.Value = tInfo.DonnotCollectLastChapterNo;
			FilterNovelType.SelectedIndex = tInfo.FilterNovelType;
			if (tInfo.FilterVolume != null)
			{
				FilterVolumeTextBox.Text = string.Join("\r\n", tInfo.FilterVolume);
			}
			if (tInfo.FilterContinueVolume != null)
			{
				FilterVolumeTextBox1.Text = string.Join("\r\n", tInfo.FilterContinueVolume);
			}
			if (tInfo.FilterChapterName != null)
			{
				FilterChapterNameBox.Text = string.Join("\r\n", tInfo.FilterChapterName);
			}
			if (tInfo.FilterContinueChapterName != null)
			{
				FilterChapterNameBox1.Text = string.Join("\r\n", tInfo.FilterContinueChapterName);
			}
			if (tInfo.FilterNovel != null)
			{
				FilterNovelTextBox.Text = string.Join("\r\n", tInfo.FilterNovel);
			}
			DuanImage.Checked = tInfo.DuanImage;
			DuanImageCheck.Checked = tInfo.DuanImageCheck;
			ReplaceImgflag.Checked = tInfo.ReplaceImgflag;
			DelForHtml.Checked = tInfo.DelForHtml;
			DelForTxt.Checked = tInfo.DelForTxt;
			ReplaceFullflag.Checked = tInfo.ReplaceFullflag;
			ReplaceIntro.Checked = tInfo.ReplaceIntro;
			ReplaceSort.Checked = tInfo.ReplaceSort;
			OnlyReplaceSort.Checked = tInfo.OnlyReplaceSort;
			ReplaceChapter.Checked = tInfo.ReplaceChapter;
			isChkMD5.Checked = tInfo.isChkMD5;
			forceReplace.Checked = tInfo.ForceReplace;
			ReplaceSortId.Value = tInfo.ReplaceSortId;
			ReplaceChapterNun.Value = tInfo.ReplaceChapterNun;
			ReplaceChapterNameNun.Value = tInfo.ReplaceChapterNameNun;
			StrongReplaceImgflag.Checked = tInfo.StrongReplaceImgflag;
			StrongReplaceFullflag.Checked = tInfo.StrongReplaceFullflag;
			StrongReplaceIntro.Checked = tInfo.StrongReplaceIntro;
			label16.Text = "高级版本无期限(" + Configs.BaseConfig.LicenseTime.ToString("yyyy-MM-dd HH:mm:ss") + "验证)";
			textBox_12.Text = tInfo.ProxyHost;
			textBox_11.Text = tInfo.ProxyPort.ToString();
			textBox_14.Text = tInfo.ProxyDomain;
			textBox_10.Text = tInfo.ProxyUser;
			textBox_9.Text = tInfo.ProxyPassword;
			chkEnableProxy.Checked = tInfo.Proxy;
			LoadRequestSchedulingToUi(tInfo);
			checkBox_20.Checked = tInfo.NoPBar;
			索引对比判断修复.Checked = tInfo.ReplaceChapterIndex;
			索引对比失败只修复.Checked = tInfo.ReplaceChapterTime;
			ReplaceChapterTimeMin.Value = tInfo.ReplaceChapterTimeMin;
			ReplaceChapterTimeMax.Value = tInfo.ReplaceChapterTimeMax;
		}
		catch (Exception ex)
		{
			ShowErrorMessage(ex.Message);
		}
	}

	private void method_5()
	{
		try
		{
			tInfo.RuleFile = 采集规则_0.Text;
			if (采集最新列表_0.Checked)
			{
				tInfo.RadioBy = "GetListUrl";
			}
			if (目标站书号_2.Checked)
			{
				tInfo.RadioBy = "GetOrderId";
			}
			if (目标站自定义书号_1.Checked)
			{
				tInfo.RadioBy = "GetSinceId";
			}
			if (本站书号_4.Checked)
			{
				tInfo.RadioBy = "PutOrderId";
			}
			if (本站自定义书号_3.Checked)
			{
				tInfo.RadioBy = "PutSinceId";
			}
			if (其他站列表搜索_5.Checked)
			{
				tInfo.RadioBy = "OtherListUrl";
			}
			tInfo.GetListUrl = 采集最新列表框_3.Text.Trim().Replace("\r\n", "♂").Split('♂');
			tInfo.GetOrderMinId = Convert.ToInt32(目标站书号开始_6.Text);
			tInfo.GetOrderMaxId = Convert.ToInt32(目标站书号结束_5.Text);
			tInfo.GetSinceId = 目标站自定义书号ID_4.Text.Split(',');
			tInfo.OtherListUrl = 其他站列表URL_17.Text;
			tInfo.OtherRegex = 提取其他站小说名规则_16.Text;
			tInfo.OtherEncoding = 其他站编码_18.Text;
			tInfo.PutOrderMinId = Convert.ToInt32(本站书号开始_2.Text);
			tInfo.PutOrderMaxId = Convert.ToInt32(本站书号结束_1.Text);
			tInfo.PutSinceId = 本站自定义书号ID_0.Text.Split(',');
			tInfo.Log = 日志记录_3.Checked;
			tInfo.Timing = 循环采集_2.Checked;
			tInfo.Interval = Convert.ToInt32(循环间隔时间选择_0.Value);
			tInfo.NewBook = 添加新书_0.Checked;
			tInfo.OldBook = 更新连载_1.Checked;
			tInfo.FilterFinish = 不处理完结_8.Checked;
			tInfo.Typesetting = 内容自动排版_16.Checked;
			tInfo.CompulsoryDeleteChapter = 强制清空_18.Checked;
			tInfo.DeleteChapter = 清空重采_6.Checked;
			tInfo.DownImage = 下载图片章节_5.Checked;
			tInfo.ProhibitionVolume = 禁止添加分卷_4.Checked;
			tInfo.CheckVolume = 添加分卷判断_21.Checked;
			tInfo.UpdateDefault = 调用页面_10.Checked;
			tInfo.OnlyText = 只采文字章_9.Checked;
			tInfo.CheckRepeat = 栓测重复章节_22.Checked;
			tInfo.EqualsChapter = comboBox_2.SelectedIndex;
			tInfo.RepeatChapter = comboBox_6.SelectedIndex;
			tInfo.RepeatChapterHandle = comboBox1.SelectedIndex;
			tInfo.EmptyChapter = comboBox_1.SelectedIndex;
			tInfo.DownImageError = comboBox_5.SelectedIndex;
			tInfo.UnDownUrl = textBox_19.Text;
			tInfo.OrderChapter = comboBox_7.SelectedIndex;
			tInfo.NameAndAuthor = 识别方式_17.Checked;
			tInfo.MinChapterTextLength = Convert.ToInt32(不采小于字符设置_6.Value);
			tInfo.MinChapterNum = Convert.ToInt32(不采小于章节设置_2.Value);
			tInfo.FindMaxChapterNum = Convert.ToInt32(numericUpDown_1.Value);
			tInfo.Finish = 全本必采_15.Checked;
			tInfo.Hidebook = 隐藏更新小说.Checked;
			tInfo.DonnotCollectLastChapterNo = Convert.ToInt32(DonnotCollectLastChapterNum.Value);
			tInfo.FilterNovelType = FilterNovelType.SelectedIndex;
			tInfo.FilterVolume = FilterVolumeTextBox.Text.Replace("\r\n", "♂").Split('♂');
			tInfo.FilterContinueVolume = FilterVolumeTextBox1.Text.Replace("\r\n", "♂").Split('♂');
			tInfo.FilterNovel = FilterNovelTextBox.Text.Replace("\r\n", "♂").Split('♂');
			tInfo.FilterChapterName = FilterChapterNameBox.Text.Replace("\r\n", "♂").Split('♂');
			tInfo.FilterContinueChapterName = FilterChapterNameBox1.Text.Replace("\r\n", "♂").Split('♂');
			tInfo.DuanImage = DuanImage.Checked;
			tInfo.DuanImageCheck = DuanImageCheck.Checked;
			tInfo.ReplaceImgflag = ReplaceImgflag.Checked;
			tInfo.DelForHtml = DelForHtml.Checked;
			tInfo.DelForTxt = DelForTxt.Checked;
			tInfo.ReplaceFullflag = ReplaceFullflag.Checked;
			tInfo.ReplaceIntro = ReplaceIntro.Checked;
			tInfo.ReplaceSort = ReplaceSort.Checked;
			tInfo.OnlyReplaceSort = OnlyReplaceSort.Checked;
			tInfo.ReplaceChapter = ReplaceChapter.Checked;
			tInfo.isChkMD5 = isChkMD5.Checked;
			tInfo.ForceReplace = forceReplace.Checked;
			tInfo.ReplaceSortId = Convert.ToInt32(ReplaceSortId.Value);
			tInfo.ReplaceChapterNun = Convert.ToInt32(ReplaceChapterNun.Value);
			tInfo.ReplaceChapterNameNun = Convert.ToInt32(ReplaceChapterNameNun.Value);
			tInfo.StrongReplaceImgflag = StrongReplaceImgflag.Checked;
			tInfo.StrongReplaceFullflag = StrongReplaceFullflag.Checked;
			tInfo.StrongReplaceIntro = StrongReplaceIntro.Checked;
			tInfo.ProxyHost = textBox_12.Text;
			tInfo.ProxyPort = Convert.ToInt32(textBox_11.Text);
			tInfo.ProxyDomain = textBox_14.Text;
			tInfo.ProxyUser = textBox_10.Text;
			tInfo.ProxyPassword = textBox_9.Text;
			tInfo.Proxy = chkEnableProxy.Checked;
			tInfo.NoPBar = checkBox_20.Checked;
			SaveRequestSchedulingFromUi(tInfo);
			tInfo.ReplaceChapterIndex = 索引对比判断修复.Checked;
			tInfo.ReplaceChapterTime = 索引对比失败只修复.Checked;
			tInfo.ReplaceChapterTimeMin = Convert.ToInt32(ReplaceChapterTimeMin.Value);
			tInfo.ReplaceChapterTimeMax = Convert.ToInt32(ReplaceChapterTimeMax.Value);
		}
		catch (Exception ex)
		{
			ShowErrorMessage(ex.Message);
		}
	}

	private void OnlyReplaceSort_CheckedChanged(object sender, EventArgs e)
	{
		if (OnlyReplaceSort.Checked)
		{
			ReplaceSort.Checked = true;
		}
	}

	private void ReplaceFullflag_CheckedChanged(object sender, EventArgs e)
	{
		if (!ReplaceFullflag.Checked)
		{
			StrongReplaceFullflag.Checked = false;
		}
	}

	private void ReplaceImgflag_CheckedChanged(object sender, EventArgs e)
	{
		if (!ReplaceImgflag.Checked)
		{
			StrongReplaceImgflag.Checked = false;
		}
	}

	private void ReplaceIntro_CheckedChanged(object sender, EventArgs e)
	{
		if (!ReplaceIntro.Checked)
		{
			StrongReplaceIntro.Checked = false;
		}
	}

	private void ReplaceSort_CheckedChanged(object sender, EventArgs e)
	{
		if (!ReplaceSort.Checked)
		{
			OnlyReplaceSort.Checked = false;
		}
	}

	public void Run()
	{
		Console.WriteLine("开始执行");
		if (!tInfo.Timing)
		{
			if (!AutoWorker.IsBusy)
			{
				AutoWorker.RunWorkerAsync();
				WaitForAutoWorker();
			}
			return;
		}
		while (true)
		{
			if (!AutoWorker.IsBusy)
			{
				AutoWorker.RunWorkerAsync();
				WaitForAutoWorker();
			}
			for (int num = tInfo.Interval * 60; num > 0; num--)
			{
				Console.WriteLine("距离下次启动还有 " + num + " 秒");
				if (!WaitOrCancel(1000))
				{
					return;
				}
			}
		}
	}

	private void StrongReplaceFullflag_CheckedChanged(object sender, EventArgs e)
	{
		if (StrongReplaceFullflag.Checked)
		{
			ReplaceFullflag.Checked = true;
		}
	}

	private void StrongReplaceImgflag_CheckedChanged(object sender, EventArgs e)
	{
		if (StrongReplaceImgflag.Checked)
		{
			ReplaceImgflag.Checked = true;
		}
	}

	private void StrongReplaceIntro_CheckedChanged(object sender, EventArgs e)
	{
		if (StrongReplaceIntro.Checked)
		{
			ReplaceIntro.Checked = true;
		}
	}

	private void TestWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		ListViewItem[] array = (ListViewItem[])e.Argument;
		ListViewItem[] array2 = array;
		foreach (ListViewItem item in array2)
		{
			string proxyHost = item.Text.ToString();
			int proxyPort = int.Parse(item.SubItems[1].Text.ToString());
			HttpClient httpClient = new HttpClient
			{
				UriString = "http://www.baidu.com/",
				Encoding = FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk"),
				Proxy = true,
				ProxyHost = proxyHost,
				ProxyPort = proxyPort
			};
			HttpClient httpClient2 = httpClient;
			string time = "不可用";
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			if (httpClient2.GetStringWork() != string.Empty)
			{
				time = stopwatch.Elapsed.TotalSeconds.ToString("0.000") + "秒";
			}
			Invoke((MethodInvoker)delegate
			{
				if (time == "不可用")
				{
					listView1.Items[item.Index].ForeColor = Color.Red;
				}
				else
				{
					listView1.Items[item.Index].ForeColor = Color.Green;
				}
				listView1.Items[item.Index].SubItems[4].Text = time;
				listView1.Items[item.Index].SubItems[5].Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			});
		}
	}

	private void TestWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
	}

	private void timer_0_Tick(object sender, EventArgs e)
	{
		if (!AutoWorker.IsBusy)
		{
			if (dateTime_0 < DateTime.Now)
			{
				dateTime_0 = DateTime.Now.AddMinutes(tInfo.Interval);
				AutoWorker.RunWorkerAsync();
				timer_0.Stop();
				return;
			}
			TimeSpan timeSpan = new TimeSpan(dateTime_0.Ticks).Subtract(new TimeSpan(DateTime.Now.Ticks)).Duration();
			label_13.Text = "距离下次循环开始还有 " + timeSpan.Days + "天" + timeSpan.Hours + "小时" + timeSpan.Minutes + "分钟" + timeSpan.Seconds + "秒";
		}
	}

	private void toolStripMenuItem_0_Click(object sender, EventArgs e)
	{
		if (string_0 != "")
		{
			method_5();
			ConfigFileManager.SaveConfig(string_0, tInfo);
		}
		else
		{
			toolStripMenuItem_1_Click(sender, e);
		}
	}

	private void toolStripMenuItem_1_Click(object sender, EventArgs e)
	{
		if (saveFileDialog_0.ShowDialog() == DialogResult.OK)
		{
			string_0 = saveFileDialog_0.FileName;
			new FileInfo(string_0);
			method_5();
			ConfigFileManager.SaveConfig(string_0, tInfo);
		}
		采集方案_4.BeginUpdate();
		string[] array = IO.LoadTasks();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!采集方案_4.Items.Contains(array[i]))
				{
					采集方案_4.Items.Add(array[i]);
					采集方案_4.Text = array[i];
				}
			}
		}
		采集方案_4.EndUpdate();
	}

	private void toolStripMenuItem_11_Click(object sender, EventArgs e)
	{
		listView1.Items.Clear();
	}

	private void toolStripMenuItem_2_Click(object sender, EventArgs e)
	{
		method_5();
		ConfigFileManager.SaveConfig("TaskConfig.xml", tInfo);
	}

	private void toolStripMenuItem_22_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView1.Items.Count; i++)
		{
			listView1.Items[i].Checked = false;
		}
	}

	private void toolStripMenuItem_23_Click(object sender, EventArgs e)
	{
		if (!LoginWorker.IsBusy)
		{
			LoginWorker.RunWorkerAsync();
		}
	}

	private void toolStripMenuItem_25_Click(object sender, EventArgs e)
	{
		if (listView1.CheckedItems.Count == 0 || TestWorker.IsBusy)
		{
			return;
		}
		ListViewItem[] array = new ListViewItem[listView1.CheckedItems.Count];
		int num = 0;
		foreach (ListViewItem checkedItem in listView1.CheckedItems)
		{
			array[num] = checkedItem;
			num++;
		}
		TestWorker.RunWorkerAsync(array);
	}

	private void toolStripMenuItem_3_Click(object sender, EventArgs e)
	{
		if (openFileDialog_0.ShowDialog() == DialogResult.OK)
		{
			string_0 = openFileDialog_0.FileName;
			new FileInfo(string_0);
			tInfo = (TaskConfigInfo)ConfigFileManager.LoadConfig(string_0, tInfo);
			method_4();
			rInfo = (RuleConfigInfo)ConfigFileManager.LoadConfig(采集规则_0.Text, rInfo);
			if (!bool_0)
			{
				Text = rInfo.GetSiteName.Pattern + " 标准采集模式";
			}
		}
	}

	private void toolStripMenuItem_6_Click(object sender, EventArgs e)
	{
		string text = "";
		int num = 80;
		if (listView1.SelectedItems.Count > 0)
		{
			text = listView1.SelectedItems[0].SubItems[0].Text.Trim();
			num = int.Parse(listView1.SelectedItems[0].SubItems[1].Text.Trim());
			chkEnableProxy.Checked = true;
			textBox_11.Text = num.ToString();
			textBox_12.Text = text;
			tInfo.Proxy = true;
			tInfo.ProxyHost = text;
			tInfo.ProxyPort = num;
		}
	}

	private void toolStripMenuItem_8_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView1.Items.Count; i++)
		{
			listView1.Items[i].Checked = true;
		}
	}

	public static int BoolToInt(object object_0)
	{
		if (object_0 != null && Convert.ToBoolean(object_0))
		{
			return 1;
		}
		return 0;
	}

	private void toolStripMenuItem_9_Click(object sender, EventArgs e)
	{
		if (listView1.CheckedItems.Count == 0 || LoginWorker.IsBusy)
		{
			return;
		}
		foreach (ListViewItem checkedItem in listView1.CheckedItems)
		{
			listView1.Items.Remove(checkedItem);
		}
	}
}








