using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;
using NovelSpider.Entity;
using NovelSpider.Local;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelAdmin;

public class 数据管理台 : DockContent
{
	private BackgroundWorker backgroundWorker_0;

	private BackgroundWorker backgroundWorker1;

	private BackgroundWorker backgroundWorker2;

	private BackgroundWorker backgroundWorker3;

	private BackgroundWorker backgroundWorker4;

	private BackgroundWorker backgroundWorker5;

	private BackgroundWorker backgroundWorker6;

	private BackgroundWorker backgroundWorker7;

	private BackgroundWorker backgroundWorker8;

	private int bookindex;

	public bool bool_0;

	public bool bool_1;

	public bool bool_2;

	public bool bool_3;

	public bool bool_4;

	public bool bool_5;

	private Button button_1;

	private Button button_2;

	private Button button_3;

	private Button button_4;

	private Button button1;

	private Button button2;

	private Button button3;

	private Button button4;

	private Button button5;

	public bool ChapterHtml;

	private int chapterindex;

	private ContextMenuStrip ChapterMenuStrip;

	private CheckBox checkBox_8;

	public string CmdText;

	private ColumnHeader 总章节数;

	private ColumnHeader 最新章节;

	private ColumnHeader columnHeader11;

	private ColumnHeader columnHeader12;

	private ColumnHeader columnHeader13;

	private ColumnHeader columnHeader14;

	private ColumnHeader 总字数;

	private ColumnHeader columnHeader21;

	private ColumnHeader columnHeader22;

	private ColumnHeader columnHeader23;

	private ColumnHeader columnHeader24;

	private ColumnHeader columnHeader25;

	private ColumnHeader 添加时间;

	private ColumnHeader 更新时间;

	private ColumnHeader 推荐;

	private ColumnHeader 书籍ID;

	private ColumnHeader 书籍名称;

	private ColumnHeader 作者;

	private ColumnHeader 大类;

	private IContainer components;

	private DateTime dateTime_0;

	public bool FullHtml;

	private IContainer icontainer_0;

	public bool IndexHtml;

	private Label label_0;

	private Label label_1;

	private Label label_11;

	private Label label_2;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private ListView 小说信息list;

	private ListView 章节列表list;

	public bool Log;

	public int m_Interval;

	private ContextMenuStrip NovelMenuStrip;

	private NumericUpDown numericUpDown_0;

	private NumericUpDown numericUpDown_1;

	private NumericUpDown numericUpDown_2;

	private Panel SQL面板;

	private Panel panel1;

	private Panel panel2;

	private Panel 内容管理面板;

	private ProgressBar progressBar_1;

	private StatusStrip statusStrip_0;

	private TextBox textBox_0;

	private TextBox textBox1;

	private TextBox textBox2;

	private TextBox textBox3;

	private TextBox textBox4;

	private Timer timer_0;

	public bool Timing;

	private ToolStripMenuItem toolStripMenuItem_1;

	private ToolStripMenuItem toolStripMenuItem_18;

	private ToolStripMenuItem toolStripMenuItem_19;

	private ToolStripMenuItem toolStripMenuItem_2;

	private ToolStripMenuItem toolStripMenuItem_20;

	private ToolStripMenuItem toolStripMenuItem_21;

	private ToolStripMenuItem toolStripMenuItem_3;

	private ToolStripMenuItem toolStripMenuItem_4;

	private ToolStripMenuItem toolStripMenuItem_5;

	private ToolStripMenuItem toolStripMenuItem1;

	private ToolStripMenuItem toolStripMenuItem2;

	private ToolStripMenuItem toolStripMenuItem3;

	private ToolStripMenuItem toolStripMenuItem4;

	private ToolStripMenuItem toolStripMenuItem5;

	private ToolStripMenuItem toolStripMenuItem6;

	private ToolStripMenuItem toolStripMenuItem7;

	private ToolStripMenuItem toolStripMenuItem8;

	private ToolStripMenuItem toolStripMenuItem9;

	private ToolStripSeparator toolStripSeparator_0;

	private ToolStripSeparator toolStripSeparator_1;

	private ToolStripSeparator toolStripSeparator_6;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripStatusLabel toolStripStatusLabel_0;

	private ToolStripStatusLabel toolStripStatusLabel_1;

	public 数据管理台()
	{
		dateTime_0 = DateTime.Now;
		m_Interval = 1;
		bookindex = 0;
		chapterindex = 0;
		InitializeComponent();
	}

	private void AdminCreate_Load(object sender, EventArgs e)
	{
		base.ClientSize = new Size(1000, 730);
		panel1.Size = new Size(1000, 560);
		toolStripStatusLabel_1.Text = "正在连接" + Configs.BaseConfig.CmsName + "小说系统" + Configs.BaseConfig.CmsVersion + " [" + Configs.BaseConfig.WebSiteName + "]";
	}

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		backgroundWorker_0.ReportProgress(1, "正在执行");
		backgroundWorker_0.ReportProgress(1, "正在获得小说列表");
		backgroundWorker_0.ReportProgress(3, 1);
		NovelInfo[] novelList;
		try
		{
			novelList = LocalProvider.GetInstance().GetNovelList(CmdText);
		}
		catch (Exception ex)
		{
			if (Log)
			{
				SpiderException.Debug("管理器获得小说列表", ex.Message);
			}
			else
			{
				MessageBox.Show("无法载入小说列表，有可能是SQL语句错误。\n\n" + ex.Message);
			}
			Invoke((EventHandler)delegate
			{
				button_1.Enabled = true;
			});
			return;
		}
		Invoke((EventHandler)delegate
		{
			小说信息list.Items.Clear();
		});
		if (novelList.Length != 0)
		{
			ListViewItem[] items = new ListViewItem[novelList.Length];
			int num = 0;
			NovelInfo[] array = novelList;
			foreach (NovelInfo novelInfo in array)
			{
				items[num] = new ListViewItem(novelInfo.PutID.ToString())
				{
					Tag = novelInfo
				};
				items[num].SubItems.Add(novelInfo.Name);
				items[num].SubItems.Add(novelInfo.Author);
				items[num].SubItems.Add(novelInfo.LagerSort);
				items[num].SubItems.Add(novelInfo.LastChapter.ChapterName);
				items[num].SubItems.Add(novelInfo.Chapters.ToString());
				items[num].SubItems.Add(novelInfo.Size.ToString());
				ListViewItem.ListViewSubItemCollection subItems = items[num].SubItems;
				subItems.Add(FormatText.GetTime(novelInfo.PostDate).ToString("yyyy-MM-dd HH:mm:ss"));
				ListViewItem.ListViewSubItemCollection subItems2 = items[num].SubItems;
				subItems2.Add(FormatText.GetTime(novelInfo.LastupDate).ToString("yyyy-MM-dd HH:mm:ss"));
				if (novelInfo.TopDate > 0)
				{
					items[num].SubItems.Add("是");
				}
				else
				{
					items[num].SubItems.Add("否");
				}
				num++;
			}
			Invoke((EventHandler)delegate
			{
				ProgressiveListViewLoader.ReplaceItems(小说信息list, items, toolStripStatusLabel_1, "书籍加载");
			});
		}
		backgroundWorker_0.ReportProgress(2, 1);
		backgroundWorker_0.ReportProgress(1, "执行完毕");
		backgroundWorker_0.ReportProgress(1, "共找到" + novelList.Length + "本书籍");
		Invoke((EventHandler)delegate
		{
			button_1.Enabled = true;
		});
	}

	private void backgroundWorker_0_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetMesaages(e.ProgressPercentage, e.UserState);
	}

	private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			toolStripStatusLabel_0.Text = "错误提示";
			toolStripStatusLabel_1.Text = e.Error.Message;
			if (!Log)
			{
				MessageBox.Show(e.Error.Message);
			}
			else
			{
				SpiderException.Debug(e.Error.Message);
			}
		}
		else if (e.Cancelled)
		{
			toolStripStatusLabel_0.Text = "操作取消";
		}
		else
		{
			toolStripStatusLabel_0.Text = "操作完成";
		}
	}

	private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
	{
		NovelInfo argument = (NovelInfo)e.Argument;
		backgroundWorker1.ReportProgress(1, "正在执行");
		backgroundWorker1.ReportProgress(1, "正在获得章节列表");
		backgroundWorker1.ReportProgress(2, 0);
		ChapterInfo[] chapterList;
		try
		{
			chapterList = LocalProvider.GetInstance().GetChapterList(argument.PutID);
		}
		catch (Exception ex)
		{
			if (Log)
			{
				SpiderException.Debug("管理器获得章节列表", ex.Message);
			}
			else
			{
				MessageBox.Show("无法载入章节列表，请检查配置文件。\n\n" + ex.Message);
			}
			return;
		}
		backgroundWorker1.ReportProgress(3, chapterList.Length);
		Invoke((EventHandler)delegate
		{
			内容管理面板.Tag = argument;
			章节列表list.Items.Clear();
			章节列表list.Tag = chapterList;
		});
		if (chapterList.Length != 0)
		{
			ListViewItem[] items = new ListViewItem[chapterList.Length];
			int num = 0;
			ChapterInfo[] array = chapterList;
			foreach (ChapterInfo chapterInfo in array)
			{
				items[num] = new ListViewItem(chapterInfo.PutID.ToString());
				items[num].SubItems.Add(chapterInfo.ItemIndex.ToString());
				items[num].SubItems.Add(argument.Name);
				items[num].SubItems.Add(chapterInfo.VolumeName);
				items[num].SubItems.Add(chapterInfo.ChapterName);
				items[num].SubItems.Add(chapterInfo.PostTime.ToString("yyyy-MM-dd HH:mm:ss"));
				items[num].SubItems.Add(chapterInfo.LastTime.ToString("yyyy-MM-dd HH:mm:ss"));
				argument.LastChapter.PutID = chapterInfo.PutID;
				string chapterText = LocalProvider.GetInstance().GetChapterText(argument, on: false);
				if (string.IsNullOrEmpty(chapterText))
				{
					items[num].SubItems.Add("无法获取");
				}
				else if (chapterText.Length > 300)
				{
					items[num].SubItems.Add("文本正常");
				}
				else
				{
					items[num].SubItems.Add("文字较少");
				}
				items[num].SubItems.Add(chapterInfo.Size.ToString());
				num++;
				backgroundWorker1.ReportProgress(2, num);
			}
			Invoke((EventHandler)delegate
			{
				ProgressiveListViewLoader.ReplaceItems(章节列表list, items, toolStripStatusLabel_1, "章节加载");
			});
		}
		backgroundWorker1.ReportProgress(1, "执行完毕");
		backgroundWorker1.ReportProgress(1, "共找到" + chapterList.Length + "个章节");
	}

	private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetMesaages(e.ProgressPercentage, e.UserState);
	}

	private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
	{
		NovelInfo argument = (NovelInfo)e.Argument;
		try
		{
			argument.LastChapter.ChapterText = LocalProvider.GetInstance().GetChapterText(argument, on: false);
		}
		catch (Exception ex)
		{
			if (!Log)
			{
				MessageBox.Show("无法载入章节列表，请检查配置文件。\n\n" + ex.Message);
			}
			else
			{
				SpiderException.Debug("管理器获得章节列表", ex.Message);
			}
			return;
		}
		Invoke((EventHandler)delegate
		{
			内容管理面板.Tag = argument;
			textBox3.Text = argument.LastChapter.ChapterName;
			textBox4.Text = argument.LastChapter.ChapterText;
		});
	}

	private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
	{
		backgroundWorker3.ReportProgress(0, "正在执行");
		backgroundWorker3.ReportProgress(3, 4);
		backgroundWorker3.ReportProgress(2, 1);
		NovelInfo novelInfo = (NovelInfo)e.Argument;
		ReplaceConfigInfo replaceConfigInfo_ = new ReplaceConfigInfo
		{
			UpdateChapterName = true
		};
		LocalProvider.GetInstance().UpdateChapter(novelInfo, replaceConfigInfo_);
		backgroundWorker3.ReportProgress(0, "生成章节");
		backgroundWorker3.ReportProgress(3, 4);
		backgroundWorker3.ReportProgress(2, 2);
		LocalProvider.GetInstance().CreateSingleChapter(novelInfo, novelInfo.LastChapter, bool_0: true, 0, 0, "", "", novelInfo.LastChapter.VolumeName);
		backgroundWorker3.ReportProgress(0, "生成列表");
		backgroundWorker3.ReportProgress(3, 4);
		backgroundWorker3.ReportProgress(2, 3);
		try
		{
			LocalProvider.GetInstance().CreateIndex(novelInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
		}
		catch (Exception ex)
		{
			if (!Log)
			{
				MessageBox.Show(ex.Message);
			}
			else
			{
				SpiderException.Debug("管理器生成列表", ex.Message);
			}
		}
		backgroundWorker3.ReportProgress(0, "生成完成");
		backgroundWorker3.ReportProgress(3, 4);
		backgroundWorker3.ReportProgress(2, 4);
	}

	private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetMesaages(e.ProgressPercentage, e.UserState);
	}

	private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		SQL面板.Enabled = true;
		panel1.Visible = false;
		panel2.Visible = true;
		内容管理面板.Visible = false;
		button4.Enabled = true;
		button5.Enabled = true;
		章节列表list.Items[chapterindex].BackColor = Color.Azure;
		string text = textBox4.Text.Trim();
		if (string.IsNullOrEmpty(text))
		{
			章节列表list.Items[chapterindex].SubItems[7].Text = "空白章节";
		}
		else if (text.Length > 300)
		{
			章节列表list.Items[chapterindex].SubItems[7].Text = "文本正常";
		}
		else
		{
			章节列表list.Items[chapterindex].SubItems[7].Text = "文字较少";
		}
		章节列表list.Items[chapterindex].SubItems[4].Text = textBox3.Text.Trim();
	}

	private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
	{
		NovelInfo novelInfo = (NovelInfo)e.Argument;
		backgroundWorker4.ReportProgress(0, "准备生成");
		backgroundWorker4.ReportProgress(2, 0);
		try
		{
			ChapterInfo[] array = (ChapterInfo[])章节列表list.Tag;
			backgroundWorker4.ReportProgress(3, array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				if (backgroundWorker4.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				backgroundWorker4.ReportProgress(0, "生成章节");
				backgroundWorker4.ReportProgress(1, array[i].VolumeName + " " + array[i].ChapterName);
				backgroundWorker4.ReportProgress(2, i + 1);
				int int_ = 0;
				int int_2 = 0;
				string string_ = "";
				string string_2 = "";
				if (i != 0)
				{
					string_ = array[i - 1].ChapterName;
					int_ = array[i - 1].PutID;
				}
				if (array.Length > i + 1)
				{
					string_2 = array[i + 1].ChapterName;
					int_2 = array[i + 1].PutID;
				}
				string volumeName = array[i].VolumeName;
				LocalProvider.GetInstance().CreateSingleChapter(novelInfo, array[i], bool_0: false, int_, int_2, string_, string_2, volumeName);
			}
		}
		catch (Exception ex)
		{
			if (!Log)
			{
				MessageBox.Show(ex.Message);
			}
			else
			{
				SpiderException.Debug("管理器生成章节", ex.Message);
			}
		}
		backgroundWorker4.ReportProgress(0, "生成列表");
		backgroundWorker4.ReportProgress(1, novelInfo.Name + " " + novelInfo.Author);
		try
		{
			LocalProvider.GetInstance().CreateIndex(novelInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
		}
		catch (Exception ex2)
		{
			if (!Log)
			{
				MessageBox.Show(ex2.Message);
			}
			else
			{
				SpiderException.Debug("管理器生成列表", ex2.Message);
			}
			return;
		}
		backgroundWorker4.ReportProgress(0, "操作完成");
		backgroundWorker4.ReportProgress(1, novelInfo.Name + " " + novelInfo.Author + "  生成成功");
	}

	private void backgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetMesaages(e.ProgressPercentage, e.UserState);
	}

	private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		backgroundWorker5.ReportProgress(2, 0);
		NovelInfo novelInfo = (NovelInfo)内容管理面板.Tag;
		ChapterInfo[] array = (ChapterInfo[])e.Argument;
		backgroundWorker5.ReportProgress(3, array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			backgroundWorker5.ReportProgress(2, i);
			backgroundWorker5.ReportProgress(1, "删除 " + array[i].ChapterName);
			if (backgroundWorker.CancellationPending)
			{
				break;
			}
			LocalProvider.GetInstance().DeleteChapter(novelInfo, novelInfo.PutID, array[i].PutID, bool_0: true, bool_1: false);
		}
		LocalProvider.GetInstance().CreateIndex(novelInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
		ChapterInfo[] chapterList = LocalProvider.GetInstance().GetChapterList(novelInfo.PutID);
		章节列表list.Tag = chapterList;
		if (chapterList.Length != 0)
		{
			novelInfo.LastChapter = chapterList[chapterList.Length - 1];
		}
		backgroundWorker5.ReportProgress(0, "执行完成");
		backgroundWorker5.ReportProgress(2, array.Length);
		backgroundWorker5.ReportProgress(1, novelInfo.Name + " " + novelInfo.Author);
	}

	private void backgroundWorker5_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetMesaages(e.ProgressPercentage, e.UserState);
	}

	private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		backgroundWorker6.ReportProgress(2, 0);
		NovelInfo novelInfo = (NovelInfo)内容管理面板.Tag;
		ChapterInfo[] array = (ChapterInfo[])e.Argument;
		backgroundWorker6.ReportProgress(3, array.Length);
		for (int i = 0; i < array.Length; i++)
		{
			backgroundWorker6.ReportProgress(2, i);
			backgroundWorker6.ReportProgress(1, "删除 " + array[i].ChapterName);
			if (backgroundWorker.CancellationPending)
			{
				break;
			}
			LocalProvider.GetInstance().DeleteChapter(novelInfo, novelInfo.PutID, array[i].PutID, bool_0: true, bool_1: true);
		}
		LocalProvider.GetInstance().CreateIndex(novelInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
		ChapterInfo[] chapterList = LocalProvider.GetInstance().GetChapterList(novelInfo.PutID);
		章节列表list.Tag = chapterList;
		if (chapterList.Length != 0)
		{
			novelInfo.LastChapter = chapterList[chapterList.Length - 1];
		}
		backgroundWorker6.ReportProgress(0, "执行完成");
		backgroundWorker6.ReportProgress(2, array.Length);
		backgroundWorker6.ReportProgress(1, novelInfo.Name + " " + novelInfo.Author);
	}

	private void backgroundWorker6_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetMesaages(e.ProgressPercentage, e.UserState);
	}

	private void backgroundWorker7_DoWork(object sender, DoWorkEventArgs e)
	{
		NovelInfo[] array = (NovelInfo[])e.Argument;
		int num = 0;
		int num2 = 0;
		NovelInfo[] array2 = array;
		foreach (NovelInfo novelInfo in array2)
		{
			if (num >= array.Length)
			{
				return;
			}
			if (!backgroundWorker7.CancellationPending)
			{
				backgroundWorker7.ReportProgress(2, 0);
				backgroundWorker7.ReportProgress(1, novelInfo.PutID + " " + novelInfo.Name);
				backgroundWorker7.ReportProgress(0, "正在执行");
				try
				{
					ChapterInfo[] chapterList = LocalProvider.GetInstance().GetChapterList(novelInfo.PutID);
					num2 = chapterList.Length;
					backgroundWorker7.ReportProgress(3, num2);
					for (int j = 0; j < chapterList.Length; j++)
					{
						if (backgroundWorker7.CancellationPending)
						{
							e.Cancel = true;
							break;
						}
						backgroundWorker7.ReportProgress(2, j);
						backgroundWorker7.ReportProgress(1, chapterList[j].VolumeName + " " + chapterList[j].ChapterName);
						backgroundWorker7.ReportProgress(0, "生成章节");
						int int_ = 0;
						int int_2 = 0;
						string string_ = "";
						string string_2 = "";
						if (j != 0)
						{
							string_ = chapterList[j - 1].ChapterName;
							int_ = chapterList[j - 1].PutID;
						}
						if (chapterList.Length > j + 1)
						{
							string_2 = chapterList[j + 1].ChapterName;
							int_2 = chapterList[j + 1].PutID;
						}
						string volumeName = chapterList[j].VolumeName;
						LocalProvider.GetInstance().CreateSingleChapter(novelInfo, chapterList[j], bool_0: false, int_, int_2, string_, string_2, volumeName);
					}
				}
				catch (Exception ex)
				{
					if (!Log)
					{
						MessageBox.Show(ex.Message);
					}
					else
					{
						SpiderException.Debug("批量生成", ex.Message);
					}
					return;
				}
				try
				{
					backgroundWorker7.ReportProgress(1, novelInfo.PutID + " " + novelInfo.Name);
					backgroundWorker7.ReportProgress(0, "生成列表");
					LocalProvider.GetInstance().CreateIndex(novelInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
					if (backgroundWorker7.CancellationPending)
					{
						e.Cancel = true;
						break;
					}
				}
				catch (Exception ex2)
				{
					if (!Log)
					{
						MessageBox.Show(ex2.Message);
					}
					else
					{
						SpiderException.Debug("批量生成", ex2.Message);
					}
					return;
				}
				num++;
				continue;
			}
			e.Cancel = true;
			return;
		}
		backgroundWorker7.ReportProgress(2, num2);
		backgroundWorker7.ReportProgress(0, "执行完毕");
		backgroundWorker7.ReportProgress(1, "选中书籍生成完毕");
	}

	private void backgroundWorker7_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetMesaages(e.ProgressPercentage, e.UserState);
	}

	private void backgroundWorker7_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			MessageBox.Show(e.Error.Message, "错误提示");
			label_0.Text = "发生错误";
		}
		else if (e.Cancelled)
		{
			label_0.Text = "操作取消";
		}
		else
		{
			label_0.Text = "操作完成";
		}
		toolStripMenuItem_19.Enabled = true;
		toolStripMenuItem_1.Enabled = true;
		toolStripMenuItem_2.Enabled = false;
	}

	private void backgroundWorker8_DoWork(object sender, DoWorkEventArgs e)
	{
		if (backgroundWorker_0.IsBusy)
		{
			MessageBox.Show("正在删除小说请稍后操作");
			return;
		}
		NovelInfo[] array = (NovelInfo[])e.Argument;
		int num = array.Length;
		backgroundWorker8.ReportProgress(3, num);
		NovelInfo[] array2 = array;
		foreach (NovelInfo novelInfo in array2)
		{
			if (backgroundWorker8.CancellationPending)
			{
				e.Cancel = true;
				return;
			}
			backgroundWorker8.ReportProgress(0, "执行删除");
			backgroundWorker8.ReportProgress(1, novelInfo.Name + " " + novelInfo.Author);
			backgroundWorker8.ReportProgress(2, num);
			LocalProvider.GetInstance().DeteleNovel(novelInfo.PutID);
			File.AppendAllText("Delete.log", novelInfo.PutID + " | " + novelInfo.Name.ToString() + "\r\n");
		}
		backgroundWorker7.ReportProgress(2, num);
		backgroundWorker7.ReportProgress(0, "执行完毕");
		backgroundWorker7.ReportProgress(1, "选中书籍删除完毕");
	}

	private void backgroundWorker8_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetMesaages(e.ProgressPercentage, e.UserState);
	}

	private void backgroundWorker8_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			MessageBox.Show(e.Error.Message, "错误提示");
			label_0.Text = "发生错误";
		}
		else if (e.Cancelled)
		{
			label_0.Text = "操作取消";
		}
		else
		{
			label_0.Text = "操作完成";
		}
		toolStripMenuItem_19.Enabled = true;
		toolStripMenuItem_1.Enabled = true;
		toolStripMenuItem_2.Enabled = false;
	}

	private static string EscapeSqlLike(string value)
	{
		return (value ?? string.Empty).Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_").Replace("'", "''");
	}

	private static string BuildNumericIdList(string value)
	{
		string[] parts = (value ?? string.Empty).Split(new char[5] { ',', '，', ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
		List<string> ids = new List<string>();
		foreach (string part in parts)
		{
			if (int.TryParse(part.Trim(), out int id))
			{
				ids.Add(id.ToString());
			}
		}
		return ids.Count == 0 ? "0" : string.Join(",", ids);
	}

	private static bool IsDangerousSql(string sql)
	{
		if (string.IsNullOrWhiteSpace(sql))
		{
			return false;
		}
		string text = sql.TrimStart().ToUpperInvariant();
		return text.StartsWith("DELETE", StringComparison.Ordinal) || text.StartsWith("UPDATE", StringComparison.Ordinal) || text.StartsWith("INSERT", StringComparison.Ordinal) || text.StartsWith("DROP", StringComparison.Ordinal) || text.StartsWith("TRUNCATE", StringComparison.Ordinal) || text.StartsWith("ALTER", StringComparison.Ordinal);
	}

	private void button_1_Click(object sender, EventArgs e)
	{
		if (textBox_0.Text == "")
		{
			MessageBox.Show("请输入自定义SQL，选择单本或批量方式的请先生成自定义SQL。");
		}
		else if (!backgroundWorker_0.IsBusy)
		{
			if (IsDangerousSql(textBox_0.Text) && MessageBox.Show("当前 SQL 会修改数据库，请确认已备份并确实要执行。", "危险 SQL 确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
			{
				return;
			}
			button_1.Enabled = false;
			panel2.Visible = false;
			内容管理面板.Visible = false;
			panel1.Visible = true;
			bool_2 = true;
			CmdText = textBox_0.Text;
			dateTime_0 = DateTime.Now.AddMinutes(m_Interval);
			backgroundWorker_0.RunWorkerAsync();
		}
	}

	private void button_2_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `lastupdate` BETWEEN '" + FormatText.GetTime(DateTime.Today) + "' AND '" + FormatText.GetTime(DateTime.Today.AddDays(1.0)) + "' ORDER BY `lastupdate` ASC";
		}
		else if (Configs.CmsName == "Cnend")
		{
			textBox_0.Text = "SELECT * FROM [list_book] WHERE list_gxdate BETWEEN " + numericUpDown_1.Value + " AND " + numericUpDown_0.Value + " ORDER BY list_gxdate ASC";
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void button_3_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `articleid` BETWEEN '" + numericUpDown_1.Value + "' AND '" + numericUpDown_0.Value + "' ORDER BY `articleid` ASC";
		}
		else if (Configs.CmsName == "Cnend")
		{
			textBox_0.Text = "SELECT * FROM [list_book] WHERE id BETWEEN " + numericUpDown_1.Value + " AND " + numericUpDown_0.Value + " ORDER BY id ASC";
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void button_4_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `articleid` = '" + numericUpDown_2.Value + "'";
		}
		else if (Configs.CmsName == "Cnend")
		{
			textBox_0.Text = "SELECT TOP 1 * FROM [list_book] WHERE id = " + numericUpDown_2.Value;
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `articleid` in (" + BuildNumericIdList(textBox1.Text) + ")";
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `toptime` > '0' ORDER BY `toptime` DESC limit 1000";
		}
		else if (Configs.CmsName == "Cnend")
		{
			textBox_0.Text = "SELECT TOP 1000 * FROM [list_book] WHERE list_top > 0 ORDER BY list_gxdate ASC";
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		if (textBox2.Text.Trim() == "")
		{
			MessageBox.Show("请输入关键词");
		}
		else if (Configs.CmsName == "Jieqi")
		{
			textBox_0.Text = "SELECT * FROM `jieqi_article_article` WHERE `articlename` like '%" + EscapeSqlLike(textBox2.Text.Trim()) + "%' ESCAPE '\\' ORDER BY `lastupdate` ASC";
		}
		else
		{
			textBox_0.Text = "请输入SQL语句";
		}
	}

	private void button4_Click(object sender, EventArgs e)
	{
		if (!backgroundWorker3.IsBusy)
		{
			button4.Enabled = false;
			button5.Enabled = false;
			toolStripStatusLabel_0.Text = "更新章节";
			if (!backgroundWorker1.IsBusy)
			{
				NovelInfo novelInfo = (NovelInfo)内容管理面板.Tag;
				novelInfo.LastChapter.ChapterName = textBox3.Text.Trim();
				novelInfo.LastChapter.ChapterText = textBox4.Text.Trim();
				backgroundWorker3.RunWorkerAsync(novelInfo);
			}
		}
	}

	private void button5_Click(object sender, EventArgs e)
	{
		SQL面板.Enabled = true;
		panel1.Visible = false;
		panel2.Visible = true;
		内容管理面板.Visible = false;
	}

	private void checkBox_8_CheckedChanged(object sender, EventArgs e)
	{
		if (checkBox_8.Checked)
		{
			Log = false;
		}
		else
		{
			Log = true;
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
		this.button1 = new System.Windows.Forms.Button();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label_11 = new System.Windows.Forms.Label();
		this.button_2 = new System.Windows.Forms.Button();
		this.button_3 = new System.Windows.Forms.Button();
		this.button_4 = new System.Windows.Forms.Button();
		this.label_0 = new System.Windows.Forms.Label();
		this.label_1 = new System.Windows.Forms.Label();
		this.label_2 = new System.Windows.Forms.Label();
		this.numericUpDown_0 = new System.Windows.Forms.NumericUpDown();
		this.numericUpDown_1 = new System.Windows.Forms.NumericUpDown();
		this.numericUpDown_2 = new System.Windows.Forms.NumericUpDown();
		this.textBox_0 = new System.Windows.Forms.TextBox();
		this.button_1 = new System.Windows.Forms.Button();
		this.checkBox_8 = new System.Windows.Forms.CheckBox();
		this.progressBar_1 = new System.Windows.Forms.ProgressBar();
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		this.timer_0 = new System.Windows.Forms.Timer(this.components);
		this.statusStrip_0 = new System.Windows.Forms.StatusStrip();
		this.toolStripStatusLabel_0 = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolStripStatusLabel_1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.小说信息list = new System.Windows.Forms.ListView();
		this.书籍ID = new System.Windows.Forms.ColumnHeader();
		this.书籍名称 = new System.Windows.Forms.ColumnHeader();
		this.作者 = new System.Windows.Forms.ColumnHeader();
		this.大类 = new System.Windows.Forms.ColumnHeader();
		this.最新章节 = new System.Windows.Forms.ColumnHeader();
		this.总章节数 = new System.Windows.Forms.ColumnHeader();
		this.总字数 = new System.Windows.Forms.ColumnHeader();
		this.添加时间 = new System.Windows.Forms.ColumnHeader();
		this.更新时间 = new System.Windows.Forms.ColumnHeader();
		this.推荐 = new System.Windows.Forms.ColumnHeader();
		this.NovelMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripMenuItem_20 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_0 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_19 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_1 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_2 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_6 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_3 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_21 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_4 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_18 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_5 = new System.Windows.Forms.ToolStripMenuItem();
		this.panel2 = new System.Windows.Forms.Panel();
		this.章节列表list = new System.Windows.Forms.ListView();
		this.columnHeader21 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader22 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader23 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader24 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader25 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader12 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader13 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader14 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader11 = new System.Windows.Forms.ColumnHeader();
		this.ChapterMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
		this.SQL面板 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.button3 = new System.Windows.Forms.Button();
		this.button2 = new System.Windows.Forms.Button();
		this.内容管理面板 = new System.Windows.Forms.Panel();
		this.button5 = new System.Windows.Forms.Button();
		this.button4 = new System.Windows.Forms.Button();
		this.textBox4 = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.textBox3 = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker4 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker5 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker6 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker7 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker8 = new System.ComponentModel.BackgroundWorker();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_2).BeginInit();
		this.statusStrip_0.SuspendLayout();
		this.panel1.SuspendLayout();
		this.NovelMenuStrip.SuspendLayout();
		this.panel2.SuspendLayout();
		this.ChapterMenuStrip.SuspendLayout();
		this.SQL面板.SuspendLayout();
		this.内容管理面板.SuspendLayout();
		base.SuspendLayout();
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button1.Location = new System.Drawing.Point(1033, 42);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(69, 21);
		this.button1.TabIndex = 21;
		this.button1.Text = "生成SQL";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox1.Location = new System.Drawing.Point(70, 42);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(957, 21);
		this.textBox1.TabIndex = 20;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 45);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(41, 12);
		this.label1.TabIndex = 19;
		this.label1.Text = "多个ID";
		this.label_11.AutoSize = true;
		this.label_11.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		this.label_11.ForeColor = System.Drawing.Color.Blue;
		this.label_11.Location = new System.Drawing.Point(172, 112);
		this.label_11.Name = "label_11";
		this.label_11.Size = new System.Drawing.Size(419, 12);
		this.label_11.TabIndex = 18;
		this.label_11.Text = "批量生成最终是按“自定义SQL”执行，选择“单本ID”或“批量ID”方式的。";
		this.button_2.Location = new System.Drawing.Point(508, 15);
		this.button_2.Name = "button_2";
		this.button_2.Size = new System.Drawing.Size(91, 21);
		this.button_2.TabIndex = 17;
		this.button_2.Text = "今日更新SQL";
		this.button_2.UseVisualStyleBackColor = true;
		this.button_2.Click += new System.EventHandler(button_2_Click);
		this.button_3.Location = new System.Drawing.Point(428, 15);
		this.button_3.Name = "button_3";
		this.button_3.Size = new System.Drawing.Size(69, 21);
		this.button_3.TabIndex = 16;
		this.button_3.Text = "生成SQL";
		this.button_3.UseVisualStyleBackColor = true;
		this.button_3.Click += new System.EventHandler(button_3_Click);
		this.button_4.Location = new System.Drawing.Point(142, 15);
		this.button_4.Name = "button_4";
		this.button_4.Size = new System.Drawing.Size(69, 21);
		this.button_4.TabIndex = 15;
		this.button_4.Text = "生成SQL";
		this.button_4.UseVisualStyleBackColor = true;
		this.button_4.Click += new System.EventHandler(button_4_Click);
		this.label_0.AutoSize = true;
		this.label_0.Location = new System.Drawing.Point(12, 73);
		this.label_0.Name = "label_0";
		this.label_0.Size = new System.Drawing.Size(53, 12);
		this.label_0.TabIndex = 14;
		this.label_0.Text = "执行语句";
		this.label_1.AutoSize = true;
		this.label_1.Location = new System.Drawing.Point(224, 19);
		this.label_1.Name = "label_1";
		this.label_1.Size = new System.Drawing.Size(41, 12);
		this.label_1.TabIndex = 13;
		this.label_1.Text = "范围ID";
		this.label_2.AutoSize = true;
		this.label_2.Location = new System.Drawing.Point(12, 19);
		this.label_2.Name = "label_2";
		this.label_2.Size = new System.Drawing.Size(41, 12);
		this.label_2.TabIndex = 12;
		this.label_2.Text = "单本ID";
		this.numericUpDown_0.Location = new System.Drawing.Point(349, 15);
		this.numericUpDown_0.Maximum = new decimal(new int[4] { 1000000, 0, 0, 0 });
		this.numericUpDown_0.Name = "numericUpDown_0";
		this.numericUpDown_0.Size = new System.Drawing.Size(69, 21);
		this.numericUpDown_0.TabIndex = 3;
		this.numericUpDown_0.Value = new decimal(new int[4] { 1000, 0, 0, 0 });
		this.numericUpDown_1.Location = new System.Drawing.Point(272, 15);
		this.numericUpDown_1.Maximum = new decimal(new int[4] { 1000000, 0, 0, 0 });
		this.numericUpDown_1.Name = "numericUpDown_1";
		this.numericUpDown_1.Size = new System.Drawing.Size(69, 21);
		this.numericUpDown_1.TabIndex = 2;
		this.numericUpDown_2.Location = new System.Drawing.Point(70, 15);
		this.numericUpDown_2.Maximum = new decimal(new int[4] { 1000000, 0, 0, 0 });
		this.numericUpDown_2.Name = "numericUpDown_2";
		this.numericUpDown_2.Size = new System.Drawing.Size(66, 21);
		this.numericUpDown_2.TabIndex = 1;
		this.textBox_0.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox_0.Location = new System.Drawing.Point(70, 69);
		this.textBox_0.Multiline = true;
		this.textBox_0.Name = "textBox_0";
		this.textBox_0.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox_0.Size = new System.Drawing.Size(957, 33);
		this.textBox_0.TabIndex = 0;
		this.button_1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button_1.Location = new System.Drawing.Point(1033, 69);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(69, 33);
		this.button_1.TabIndex = 18;
		this.button_1.Text = "执行";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.checkBox_8.AutoSize = true;
		this.checkBox_8.Checked = true;
		this.checkBox_8.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBox_8.Location = new System.Drawing.Point(73, 111);
		this.checkBox_8.Name = "checkBox_8";
		this.checkBox_8.Size = new System.Drawing.Size(96, 16);
		this.checkBox_8.TabIndex = 20;
		this.checkBox_8.Text = "弹出异常提示";
		this.checkBox_8.UseVisualStyleBackColor = true;
		this.checkBox_8.CheckedChanged += new System.EventHandler(checkBox_8_CheckedChanged);
		this.progressBar_1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.progressBar_1.Location = new System.Drawing.Point(0, 695);
		this.progressBar_1.Name = "progressBar_1";
		this.progressBar_1.Size = new System.Drawing.Size(1121, 12);
		this.progressBar_1.TabIndex = 7;
		this.backgroundWorker_0.WorkerReportsProgress = true;
		this.backgroundWorker_0.WorkerSupportsCancellation = true;
		this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
		this.backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_0_ProgressChanged);
		this.backgroundWorker_0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_0_RunWorkerCompleted);
		this.statusStrip_0.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripStatusLabel_0, this.toolStripStatusLabel_1 });
		this.statusStrip_0.Location = new System.Drawing.Point(0, 708);
		this.statusStrip_0.Name = "statusStrip_0";
		this.statusStrip_0.Size = new System.Drawing.Size(1121, 22);
		this.statusStrip_0.TabIndex = 5;
		this.statusStrip_0.Text = "statusStrip1";
		this.toolStripStatusLabel_0.Name = "toolStripStatusLabel_0";
		this.toolStripStatusLabel_0.Size = new System.Drawing.Size(56, 17);
		this.toolStripStatusLabel_0.Text = "准备就绪";
		this.toolStripStatusLabel_1.Name = "toolStripStatusLabel_1";
		this.toolStripStatusLabel_1.Size = new System.Drawing.Size(1050, 17);
		this.toolStripStatusLabel_1.Spring = true;
		this.toolStripStatusLabel_1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel1.Controls.Add(this.小说信息list);
		this.panel1.Location = new System.Drawing.Point(0, 134);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(634, 295);
		this.panel1.TabIndex = 8;
		this.小说信息list.CheckBoxes = true;
		this.小说信息list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[10] { this.书籍ID, this.书籍名称, this.作者, this.大类, this.最新章节, this.总章节数, this.总字数, this.添加时间, this.更新时间, this.推荐 });
		this.小说信息list.ContextMenuStrip = this.NovelMenuStrip;
		this.小说信息list.Dock = System.Windows.Forms.DockStyle.Fill;
		this.小说信息list.FullRowSelect = true;
		this.小说信息list.GridLines = true;
		this.小说信息list.HideSelection = false;
		this.小说信息list.Location = new System.Drawing.Point(0, 0);
		this.小说信息list.Name = "小说信息list";
		this.小说信息list.Size = new System.Drawing.Size(634, 295);
		this.小说信息list.TabIndex = 5;
		this.小说信息list.UseCompatibleStateImageBehavior = false;
		this.小说信息list.View = System.Windows.Forms.View.Details;
		this.小说信息list.SelectedIndexChanged += new System.EventHandler(listView_0_SelectedIndexChanged);
		this.小说信息list.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(listView_0_MouseDoubleClick);
		this.书籍ID.Text = "书籍ID";
		this.书籍名称.Text = "书籍名称";
		this.书籍名称.Width = 140;
		this.作者.Text = "作者";
		this.作者.Width = 69;
		this.大类.Text = "类别";
		this.最新章节.Text = "最新章节(空表示新书)";
		this.最新章节.Width = 180;
		this.总章节数.Text = "章节数";
		this.总章节数.Width = 50;
		this.总字数.Text = "总字数";
		this.添加时间.Text = "添加时间";
		this.添加时间.Width = 126;
		this.更新时间.Text = "更新时间";
		this.更新时间.Width = 126;
		this.推荐.Text = "推荐";
		this.推荐.Width = 40;
		this.NovelMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.toolStripMenuItem_20, this.toolStripSeparator_0, this.toolStripMenuItem_19, this.toolStripMenuItem_1, this.toolStripMenuItem_2, this.toolStripSeparator_6, this.toolStripMenuItem_3, this.toolStripMenuItem_21, this.toolStripMenuItem_4, this.toolStripMenuItem_18,
			this.toolStripSeparator_1, this.toolStripMenuItem_5
		});
		this.NovelMenuStrip.Name = "NovelMenuStrip";
		this.NovelMenuStrip.Size = new System.Drawing.Size(205, 220);
		this.toolStripMenuItem_20.Name = "toolStripMenuItem_20";
		this.toolStripMenuItem_20.Size = new System.Drawing.Size(204, 22);
		this.toolStripMenuItem_20.Text = "列出当前书籍章节(双击)";
		this.toolStripMenuItem_20.Click += new System.EventHandler(toolStripMenuItem_20_Click);
		this.toolStripSeparator_0.Name = "toolStripSeparator_0";
		this.toolStripSeparator_0.Size = new System.Drawing.Size(201, 6);
		this.toolStripMenuItem_19.Name = "toolStripMenuItem_19";
		this.toolStripMenuItem_19.Size = new System.Drawing.Size(204, 22);
		this.toolStripMenuItem_19.Text = "重新生成所选书籍";
		this.toolStripMenuItem_19.Click += new System.EventHandler(toolStripMenuItem_19_Click);
		this.toolStripMenuItem_1.Name = "toolStripMenuItem_1";
		this.toolStripMenuItem_1.Size = new System.Drawing.Size(204, 22);
		this.toolStripMenuItem_1.Text = "删除所选书籍";
		this.toolStripMenuItem_1.Click += new System.EventHandler(toolStripMenuItem_1_Click);
		this.toolStripMenuItem_2.Enabled = false;
		this.toolStripMenuItem_2.Name = "toolStripMenuItem_2";
		this.toolStripMenuItem_2.Size = new System.Drawing.Size(204, 22);
		this.toolStripMenuItem_2.Text = "停止执行任务";
		this.toolStripMenuItem_2.Click += new System.EventHandler(toolStripMenuItem_2_Click);
		this.toolStripSeparator_6.Name = "toolStripSeparator_6";
		this.toolStripSeparator_6.Size = new System.Drawing.Size(201, 6);
		this.toolStripMenuItem_3.Name = "toolStripMenuItem_3";
		this.toolStripMenuItem_3.Size = new System.Drawing.Size(204, 22);
		this.toolStripMenuItem_3.Text = "全选书籍";
		this.toolStripMenuItem_3.Click += new System.EventHandler(toolStripMenuItem_3_Click);
		this.toolStripMenuItem_21.Name = "toolStripMenuItem_21";
		this.toolStripMenuItem_21.Size = new System.Drawing.Size(204, 22);
		this.toolStripMenuItem_21.Text = "全不选书籍";
		this.toolStripMenuItem_21.Click += new System.EventHandler(toolStripMenuItem_21_Click);
		this.toolStripMenuItem_4.Name = "toolStripMenuItem_4";
		this.toolStripMenuItem_4.Size = new System.Drawing.Size(204, 22);
		this.toolStripMenuItem_4.Text = "反选书籍";
		this.toolStripMenuItem_4.Click += new System.EventHandler(toolStripMenuItem_4_Click);
		this.toolStripMenuItem_18.Name = "toolStripMenuItem_18";
		this.toolStripMenuItem_18.Size = new System.Drawing.Size(204, 22);
		this.toolStripMenuItem_18.Text = "选中后续书籍";
		this.toolStripMenuItem_18.Click += new System.EventHandler(toolStripMenuItem_18_Click);
		this.toolStripSeparator_1.Name = "toolStripSeparator_1";
		this.toolStripSeparator_1.Size = new System.Drawing.Size(201, 6);
		this.toolStripMenuItem_5.Name = "toolStripMenuItem_5";
		this.toolStripMenuItem_5.Size = new System.Drawing.Size(204, 22);
		this.toolStripMenuItem_5.Text = "清空列表";
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel2.Controls.Add(this.章节列表list);
		this.panel2.Location = new System.Drawing.Point(0, 435);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(634, 254);
		this.panel2.TabIndex = 9;
		this.panel2.Visible = false;
		this.章节列表list.CheckBoxes = true;
		this.章节列表list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[9] { this.columnHeader21, this.columnHeader22, this.columnHeader23, this.columnHeader24, this.columnHeader25, this.columnHeader12, this.columnHeader13, this.columnHeader14, this.columnHeader11 });
		this.章节列表list.ContextMenuStrip = this.ChapterMenuStrip;
		this.章节列表list.Dock = System.Windows.Forms.DockStyle.Fill;
		this.章节列表list.FullRowSelect = true;
		this.章节列表list.GridLines = true;
		this.章节列表list.HideSelection = false;
		this.章节列表list.Location = new System.Drawing.Point(0, 0);
		this.章节列表list.Name = "章节列表list";
		this.章节列表list.Size = new System.Drawing.Size(634, 254);
		this.章节列表list.TabIndex = 3;
		this.章节列表list.UseCompatibleStateImageBehavior = false;
		this.章节列表list.View = System.Windows.Forms.View.Details;
		this.章节列表list.SelectedIndexChanged += new System.EventHandler(listView_1_SelectedIndexChanged);
		this.章节列表list.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(listView_1_MouseDoubleClick);
		this.columnHeader21.Text = "章节ID";
		this.columnHeader22.Text = "排序";
		this.columnHeader22.Width = 50;
		this.columnHeader23.Text = "书籍名称";
		this.columnHeader23.Width = 180;
		this.columnHeader24.Text = "分卷名称";
		this.columnHeader24.Width = 100;
		this.columnHeader25.Text = "章节名称";
		this.columnHeader25.Width = 200;
		this.columnHeader12.Text = "添加时间";
		this.columnHeader12.Width = 126;
		this.columnHeader13.Text = "更新时间";
		this.columnHeader13.Width = 126;
		this.columnHeader14.Text = "验证Txt";
		this.columnHeader14.Width = 70;
		this.columnHeader11.Text = "字数";
		this.ChapterMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.toolStripMenuItem1, this.toolStripSeparator1, this.toolStripMenuItem2, this.toolStripMenuItem3, this.toolStripMenuItem4, this.toolStripSeparator2, this.toolStripMenuItem5, this.toolStripMenuItem6, this.toolStripMenuItem7, this.toolStripMenuItem8,
			this.toolStripSeparator3, this.toolStripMenuItem9
		});
		this.ChapterMenuStrip.Name = "NovelMenuStrip";
		this.ChapterMenuStrip.Size = new System.Drawing.Size(209, 220);
		this.toolStripMenuItem1.Name = "toolStripMenuItem1";
		this.toolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
		this.toolStripMenuItem1.Text = "编辑章节(双击)";
		this.toolStripMenuItem1.Click += new System.EventHandler(toolStripMenuItem1_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(205, 6);
		this.toolStripMenuItem2.Name = "toolStripMenuItem2";
		this.toolStripMenuItem2.Size = new System.Drawing.Size(208, 22);
		this.toolStripMenuItem2.Text = "重新生成当前书籍";
		this.toolStripMenuItem2.Click += new System.EventHandler(toolStripMenuItem2_Click);
		this.toolStripMenuItem3.Name = "toolStripMenuItem3";
		this.toolStripMenuItem3.Size = new System.Drawing.Size(208, 22);
		this.toolStripMenuItem3.Text = "删除所选章节";
		this.toolStripMenuItem3.Click += new System.EventHandler(toolStripMenuItem3_Click);
		this.toolStripMenuItem4.Name = "toolStripMenuItem4";
		this.toolStripMenuItem4.Size = new System.Drawing.Size(208, 22);
		this.toolStripMenuItem4.Text = "删除所选章节及相关文件";
		this.toolStripMenuItem4.Click += new System.EventHandler(toolStripMenuItem4_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
		this.toolStripMenuItem5.Name = "toolStripMenuItem5";
		this.toolStripMenuItem5.Size = new System.Drawing.Size(208, 22);
		this.toolStripMenuItem5.Text = "全选章节";
		this.toolStripMenuItem5.Click += new System.EventHandler(toolStripMenuItem5_Click);
		this.toolStripMenuItem6.Name = "toolStripMenuItem6";
		this.toolStripMenuItem6.Size = new System.Drawing.Size(208, 22);
		this.toolStripMenuItem6.Text = "全不选章节";
		this.toolStripMenuItem6.Click += new System.EventHandler(toolStripMenuItem6_Click);
		this.toolStripMenuItem7.Name = "toolStripMenuItem7";
		this.toolStripMenuItem7.Size = new System.Drawing.Size(208, 22);
		this.toolStripMenuItem7.Text = "反选章节";
		this.toolStripMenuItem7.Click += new System.EventHandler(toolStripMenuItem7_Click);
		this.toolStripMenuItem8.Name = "toolStripMenuItem8";
		this.toolStripMenuItem8.Size = new System.Drawing.Size(208, 22);
		this.toolStripMenuItem8.Text = "选中后续章节";
		this.toolStripMenuItem8.Click += new System.EventHandler(toolStripMenuItem8_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(205, 6);
		this.toolStripMenuItem9.Name = "toolStripMenuItem9";
		this.toolStripMenuItem9.Size = new System.Drawing.Size(208, 22);
		this.toolStripMenuItem9.Text = "返回书籍列表";
		this.toolStripMenuItem9.Click += new System.EventHandler(toolStripMenuItem9_Click);
		this.SQL面板.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.SQL面板.Controls.Add(this.label2);
		this.SQL面板.Controls.Add(this.textBox2);
		this.SQL面板.Controls.Add(this.button3);
		this.SQL面板.Controls.Add(this.button2);
		this.SQL面板.Controls.Add(this.label_2);
		this.SQL面板.Controls.Add(this.button1);
		this.SQL面板.Controls.Add(this.label_1);
		this.SQL面板.Controls.Add(this.numericUpDown_0);
		this.SQL面板.Controls.Add(this.textBox1);
		this.SQL面板.Controls.Add(this.label_0);
		this.SQL面板.Controls.Add(this.numericUpDown_1);
		this.SQL面板.Controls.Add(this.label1);
		this.SQL面板.Controls.Add(this.button_4);
		this.SQL面板.Controls.Add(this.numericUpDown_2);
		this.SQL面板.Controls.Add(this.checkBox_8);
		this.SQL面板.Controls.Add(this.button_3);
		this.SQL面板.Controls.Add(this.label_11);
		this.SQL面板.Controls.Add(this.textBox_0);
		this.SQL面板.Controls.Add(this.button_2);
		this.SQL面板.Controls.Add(this.button_1);
		this.SQL面板.Location = new System.Drawing.Point(0, 1);
		this.SQL面板.Name = "SQL面板";
		this.SQL面板.Size = new System.Drawing.Size(1121, 128);
		this.SQL面板.TabIndex = 22;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(729, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(53, 12);
		this.label2.TabIndex = 25;
		this.label2.Text = "模糊查询";
		this.textBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox2.Location = new System.Drawing.Point(788, 16);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(239, 21);
		this.textBox2.TabIndex = 24;
		this.button3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button3.Location = new System.Drawing.Point(1033, 15);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(69, 21);
		this.button3.TabIndex = 23;
		this.button3.Text = "生成SQL";
		this.button3.UseVisualStyleBackColor = true;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.button2.Location = new System.Drawing.Point(610, 15);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(101, 21);
		this.button2.TabIndex = 22;
		this.button2.Text = "推荐书籍SQL";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.内容管理面板.Controls.Add(this.button5);
		this.内容管理面板.Controls.Add(this.button4);
		this.内容管理面板.Controls.Add(this.textBox4);
		this.内容管理面板.Controls.Add(this.label4);
		this.内容管理面板.Controls.Add(this.textBox3);
		this.内容管理面板.Controls.Add(this.label3);
		this.内容管理面板.Location = new System.Drawing.Point(640, 134);
		this.内容管理面板.Name = "内容管理面板";
		this.内容管理面板.Size = new System.Drawing.Size(480, 555);
		this.内容管理面板.TabIndex = 24;
		this.内容管理面板.Visible = false;
		this.button5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button5.Location = new System.Drawing.Point(397, 507);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(69, 33);
		this.button5.TabIndex = 28;
		this.button5.Text = "返回";
		this.button5.UseVisualStyleBackColor = true;
		this.button5.Click += new System.EventHandler(button5_Click);
		this.button4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.button4.Location = new System.Drawing.Point(296, 507);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(86, 33);
		this.button4.TabIndex = 27;
		this.button4.Text = "修改章节";
		this.button4.UseVisualStyleBackColor = true;
		this.button4.Click += new System.EventHandler(button4_Click);
		this.textBox4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox4.Font = new System.Drawing.Font("微软雅黑", 12f);
		this.textBox4.Location = new System.Drawing.Point(71, 40);
		this.textBox4.Multiline = true;
		this.textBox4.Name = "textBox4";
		this.textBox4.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox4.Size = new System.Drawing.Size(395, 448);
		this.textBox4.TabIndex = 26;
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(13, 42);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(53, 12);
		this.label4.TabIndex = 23;
		this.label4.Text = "章节内容";
		this.textBox3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.textBox3.Location = new System.Drawing.Point(71, 11);
		this.textBox3.Name = "textBox3";
		this.textBox3.Size = new System.Drawing.Size(395, 21);
		this.textBox3.TabIndex = 22;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(13, 14);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(53, 12);
		this.label3.TabIndex = 21;
		this.label3.Text = "章节名称";
		this.backgroundWorker1.WorkerReportsProgress = true;
		this.backgroundWorker1.WorkerSupportsCancellation = true;
		this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
		this.backgroundWorker2.WorkerReportsProgress = true;
		this.backgroundWorker2.WorkerSupportsCancellation = true;
		this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker2_DoWork);
		this.backgroundWorker3.WorkerReportsProgress = true;
		this.backgroundWorker3.WorkerSupportsCancellation = true;
		this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker3_DoWork);
		this.backgroundWorker3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker3_ProgressChanged);
		this.backgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker3_RunWorkerCompleted);
		this.backgroundWorker4.WorkerReportsProgress = true;
		this.backgroundWorker4.WorkerSupportsCancellation = true;
		this.backgroundWorker4.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker4_DoWork);
		this.backgroundWorker4.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker4_ProgressChanged);
		this.backgroundWorker5.WorkerReportsProgress = true;
		this.backgroundWorker5.WorkerSupportsCancellation = true;
		this.backgroundWorker5.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker5_DoWork);
		this.backgroundWorker5.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker5_ProgressChanged);
		this.backgroundWorker6.WorkerReportsProgress = true;
		this.backgroundWorker6.WorkerSupportsCancellation = true;
		this.backgroundWorker6.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker6_DoWork);
		this.backgroundWorker6.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker6_ProgressChanged);
		this.backgroundWorker7.WorkerReportsProgress = true;
		this.backgroundWorker7.WorkerSupportsCancellation = true;
		this.backgroundWorker7.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker7_DoWork);
		this.backgroundWorker7.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker7_ProgressChanged);
		this.backgroundWorker7.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker7_RunWorkerCompleted);
		this.backgroundWorker8.WorkerReportsProgress = true;
		this.backgroundWorker8.WorkerSupportsCancellation = true;
		this.backgroundWorker8.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker8_DoWork);
		this.backgroundWorker8.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker8_ProgressChanged);
		this.backgroundWorker8.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker8_RunWorkerCompleted);
		base.ClientSize = new System.Drawing.Size(1121, 730);
		base.Controls.Add(this.SQL面板);
		base.Controls.Add(this.内容管理面板);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.statusStrip_0);
		base.Controls.Add(this.progressBar_1);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Name = "数据管理台";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "数据管理台";
		base.Load += new System.EventHandler(AdminCreate_Load);
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_0).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.numericUpDown_2).EndInit();
		this.statusStrip_0.ResumeLayout(false);
		this.statusStrip_0.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.NovelMenuStrip.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.ChapterMenuStrip.ResumeLayout(false);
		this.SQL面板.ResumeLayout(false);
		this.SQL面板.PerformLayout();
		this.内容管理面板.ResumeLayout(false);
		this.内容管理面板.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void listView_0_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		panel2.Location = new Point(0, 134);
		panel2.Size = new Size(1000, 560);
		if (!backgroundWorker1.IsBusy)
		{
			panel1.Visible = false;
			panel2.Visible = true;
			toolStripStatusLabel_1.Text = "正在列出章节.请勿进行其他操作..";
			NovelInfo argument = (NovelInfo)小说信息list.Items[bookindex].Tag;
			backgroundWorker1.RunWorkerAsync(argument);
		}
	}

	private void listView_0_SelectedIndexChanged(object sender, EventArgs e)
	{
		ListView listView = (ListView)sender;
		if (listView.SelectedItems.Count > 0)
		{
			bookindex = listView.SelectedItems[0].Index;
			toolStripStatusLabel_1.Text = listView.SelectedItems[0].SubItems[1].Text + " " + listView.SelectedItems[0].SubItems[2].Text;
		}
	}

	private void listView_1_MouseDoubleClick(object sender, MouseEventArgs e)
	{
		内容管理面板.Location = new Point(0, 134);
		内容管理面板.Size = new Size(1000, 560);
		if (!backgroundWorker2.IsBusy)
		{
			toolStripStatusLabel_0.Text = "编辑章节";
			if (!backgroundWorker1.IsBusy)
			{
				panel1.Visible = false;
				panel2.Visible = false;
				内容管理面板.Visible = true;
				NovelInfo novelInfo = (NovelInfo)内容管理面板.Tag;
				novelInfo.LastChapter = ((ChapterInfo[])章节列表list.Tag)[chapterindex];
				backgroundWorker2.RunWorkerAsync(novelInfo);
			}
		}
	}

	private void listView_1_SelectedIndexChanged(object sender, EventArgs e)
	{
		ListView listView = (ListView)sender;
		if (listView.SelectedItems.Count > 0)
		{
			chapterindex = listView.SelectedItems[0].Index;
			toolStripStatusLabel_1.Text = listView.SelectedItems[0].SubItems[2].Text + " - " + listView.SelectedItems[0].SubItems[3].Text + " - " + listView.SelectedItems[0].SubItems[4].Text;
		}
	}

	protected void SetMesaages(int x, object e)
	{
		switch (x)
		{
		case 0:
			toolStripStatusLabel_0.Text = e.ToString();
			break;
		case 1:
			toolStripStatusLabel_1.Text = e.ToString();
			break;
		case 2:
		{
			int num2 = Convert.ToInt32(e);
			if (num2 <= progressBar_1.Maximum && num2 >= progressBar_1.Minimum)
			{
				progressBar_1.Value = Convert.ToInt32(e);
			}
			break;
		}
		case 3:
		{
			int num = Convert.ToInt32(e);
			if (num > 0)
			{
				progressBar_1.Maximum = num;
			}
			break;
		}
		}
	}

	private void toolStripMenuItem_1_Click(object sender, EventArgs e)
	{
		if (小说信息list.CheckedItems.Count == 0 || backgroundWorker8.IsBusy)
		{
			return;
		}
		toolStripMenuItem_19.Enabled = false;
		toolStripMenuItem_1.Enabled = false;
		toolStripMenuItem_2.Enabled = true;
		toolStripStatusLabel_0.Text = "删除书籍";
		ArrayList arrayList = new ArrayList();
		List<int> list = new List<int>();
		for (int i = 0; i < 小说信息list.Items.Count; i++)
		{
			if (小说信息list.Items[i].Checked)
			{
				NovelInfo value = (NovelInfo)小说信息list.Items[i].Tag;
				list.Add(i);
				arrayList.Add(value);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			小说信息list.Items.RemoveAt(list[list.Count - 1 - j]);
		}
		backgroundWorker8.RunWorkerAsync((NovelInfo[])arrayList.ToArray(typeof(NovelInfo)));
	}

	private void toolStripMenuItem_18_Click(object sender, EventArgs e)
	{
		bool flag = false;
		for (int i = 0; i < 小说信息list.Items.Count; i++)
		{
			if (小说信息list.Items[i].Checked)
			{
				flag = true;
			}
			if (flag)
			{
				小说信息list.Items[i].Checked = true;
			}
		}
	}

	private void toolStripMenuItem_19_Click(object sender, EventArgs e)
	{
		if (小说信息list.CheckedItems.Count == 0 || backgroundWorker7.IsBusy)
		{
			return;
		}
		toolStripMenuItem_19.Enabled = false;
		toolStripMenuItem_1.Enabled = false;
		toolStripMenuItem_2.Enabled = true;
		toolStripStatusLabel_0.Text = "生成书籍";
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < 小说信息list.Items.Count; i++)
		{
			if (小说信息list.Items[i].Checked)
			{
				NovelInfo value = (NovelInfo)小说信息list.Items[i].Tag;
				arrayList.Add(value);
			}
		}
		backgroundWorker7.RunWorkerAsync((NovelInfo[])arrayList.ToArray(typeof(NovelInfo)));
	}

	private void toolStripMenuItem_2_Click(object sender, EventArgs e)
	{
		button_1.Enabled = false;
		if (backgroundWorker7.IsBusy)
		{
			backgroundWorker7.CancelAsync();
		}
		if (backgroundWorker8.IsBusy)
		{
			backgroundWorker8.CancelAsync();
		}
	}

	private void toolStripMenuItem_20_Click(object sender, EventArgs e)
	{
		panel2.Location = new Point(0, 134);
		panel2.Size = new Size(1000, 560);
		if (!backgroundWorker1.IsBusy)
		{
			panel1.Visible = false;
			panel2.Visible = true;
			toolStripStatusLabel_1.Text = "正在列出章节.请勿进行其他操作..";
			NovelInfo argument = (NovelInfo)小说信息list.Items[bookindex].Tag;
			backgroundWorker1.RunWorkerAsync(argument);
		}
	}

	private void toolStripMenuItem_21_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < 小说信息list.Items.Count; i++)
		{
			小说信息list.Items[i].Checked = false;
		}
	}

	private void toolStripMenuItem_3_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < 小说信息list.Items.Count; i++)
		{
			小说信息list.Items[i].Checked = true;
		}
	}

	private void toolStripMenuItem_4_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < 小说信息list.Items.Count; i++)
		{
			小说信息list.Items[i].Checked = !小说信息list.Items[i].Checked;
		}
	}

	private void toolStripMenuItem1_Click(object sender, EventArgs e)
	{
		内容管理面板.Location = new Point(0, 134);
		内容管理面板.Size = new Size(1000, 560);
		if (!backgroundWorker2.IsBusy)
		{
			toolStripStatusLabel_0.Text = "编辑章节";
			if (!backgroundWorker1.IsBusy)
			{
				panel1.Visible = false;
				panel2.Visible = false;
				内容管理面板.Visible = true;
				NovelInfo novelInfo = (NovelInfo)内容管理面板.Tag;
				novelInfo.LastChapter = ((ChapterInfo[])章节列表list.Tag)[chapterindex];
				backgroundWorker2.RunWorkerAsync(novelInfo);
			}
		}
	}

	private void toolStripMenuItem2_Click(object sender, EventArgs e)
	{
		if (!backgroundWorker4.IsBusy)
		{
			NovelInfo argument = (NovelInfo)内容管理面板.Tag;
			backgroundWorker4.RunWorkerAsync(argument);
		}
	}

	private void toolStripMenuItem3_Click(object sender, EventArgs e)
	{
		if (章节列表list.CheckedItems.Count == 0 || backgroundWorker5.IsBusy)
		{
			return;
		}
		toolStripStatusLabel_0.Text = "删除章节";
		ArrayList arrayList = new ArrayList();
		List<int> list = new List<int>();
		for (int i = 0; i < 章节列表list.Items.Count; i++)
		{
			if (章节列表list.Items[i].Checked)
			{
				ChapterInfo chapterInfo = ((ChapterInfo[])章节列表list.Tag)[i];
				list.Add(i);
				chapterInfo.chaptertype = 0;
				arrayList.Add(chapterInfo);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			章节列表list.Items.RemoveAt(list[list.Count - 1 - j]);
		}
		backgroundWorker5.RunWorkerAsync((ChapterInfo[])arrayList.ToArray(typeof(ChapterInfo)));
	}

	private void toolStripMenuItem4_Click(object sender, EventArgs e)
	{
		if (章节列表list.CheckedItems.Count == 0 || backgroundWorker5.IsBusy)
		{
			return;
		}
		toolStripStatusLabel_0.Text = "删除章节";
		ArrayList arrayList = new ArrayList();
		List<int> list = new List<int>();
		for (int i = 0; i < 章节列表list.Items.Count; i++)
		{
			if (章节列表list.Items[i].Checked)
			{
				ChapterInfo chapterInfo = ((ChapterInfo[])章节列表list.Tag)[i];
				list.Add(i);
				chapterInfo.chaptertype = 0;
				arrayList.Add(chapterInfo);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			章节列表list.Items.RemoveAt(list[list.Count - 1 - j]);
		}
		backgroundWorker5.RunWorkerAsync((ChapterInfo[])arrayList.ToArray(typeof(ChapterInfo)));
	}

	private void toolStripMenuItem5_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < 章节列表list.Items.Count; i++)
		{
			章节列表list.Items[i].Checked = true;
		}
	}

	private void toolStripMenuItem6_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < 章节列表list.Items.Count; i++)
		{
			章节列表list.Items[i].Checked = false;
		}
	}

	private void toolStripMenuItem7_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < 章节列表list.Items.Count; i++)
		{
			章节列表list.Items[i].Checked = !章节列表list.Items[i].Checked;
		}
	}

	private void toolStripMenuItem8_Click(object sender, EventArgs e)
	{
		bool flag = false;
		for (int i = 0; i < 章节列表list.Items.Count; i++)
		{
			if (章节列表list.Items[i].Checked)
			{
				flag = true;
			}
			if (flag)
			{
				章节列表list.Items[i].Checked = true;
			}
		}
	}

	private void toolStripMenuItem9_Click(object sender, EventArgs e)
	{
		SQL面板.Enabled = true;
		panel1.Visible = true;
		panel2.Visible = false;
		内容管理面板.Visible = false;
	}
}
