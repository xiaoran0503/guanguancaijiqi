using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using NovelSpider.Local.Jieqi;
using NovelSpider.Target;
using WeifenLuo.WinFormsUI.Docking;

namespace NovelSpider;

public class CollectManual : DockContent
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
	private TextBox articlenameBox;

	private BackgroundWorker backgroundWorker_0;

	private BackgroundWorker backgroundWorker_1;

	private BackgroundWorker backgroundWorker_10;

	private BackgroundWorker backgroundWorker_11;

	private BackgroundWorker backgroundWorker_12;

	private BackgroundWorker backgroundWorker_2;

	private BackgroundWorker backgroundWorker_3;

	private BackgroundWorker backgroundWorker_4;

	private BackgroundWorker backgroundWorker_5;

	private BackgroundWorker backgroundWorker_6;

	private BackgroundWorker backgroundWorker_7;

	private BackgroundWorker backgroundWorker_8;

	private BackgroundWorker backgroundWorker_9;

	private BackgroundWorker backgroundWorker1;

	private bool bool_0;

	private Button button_0;

	private Button button_1;

	private Button button_2;

	private Button button_3;

	private Button button1;

	private Button button2;

	private ChapterInfo[] chapterInfo_0;

	private ChapterInfo[] chapterInfo_1;

	private TextBox chapterNameBox;

	private TextBox chapterTXTBox;

	private ColumnHeader 左手动分卷名;

	private ColumnHeader 左手动章节名;

	private ColumnHeader 左手动索引;

	private ColumnHeader 右手动索引;

	private ColumnHeader columnHeader_12;

	private ColumnHeader columnHeader_13;

	private ColumnHeader 手动信息更新时间;

	private ColumnHeader 右手动更新时间;

	private ColumnHeader 右手动分卷名;

	private ColumnHeader 右手动章节名;

	private ColumnHeader 左手动内容;

	private ColumnHeader 右手动内容;

	private ColumnHeader 手动信息目标站ID;

	private ColumnHeader 手动信息小说名称;

	private ColumnHeader 手动信息本站ID;

	private ColumnHeader 手动信息本站最新章节情况;

	private ComboBox comboBox_0;

	private ComboBox comboBox_1;

	private ComboBox comboBox_2;

	private ComboBox comboBox_3;

	private ComboBox comboBox_4;

	private IContainer components;

	private Button Db3InsertButton;

	private ToolStripMenuItem DelSelectLog;

	private ComboBox ErrIdcomboBox;

	private GroupBox groupBox1;

	private IContainer icontainer_0;

	private int int_0;

	private int int_1;

	private int int_2;

	private int int_3;

	private Label label_0;

	private Label label_1;

	private Label label1;

	private Label label10;

	private Label label11;

	private Label label12;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label label9;

	private ListView listView_1;

	private ListView listView_2;

	private ListView listView1;

	private ContextMenuStrip LocalMenuStrip;

	private ContextMenuStrip LocalMenuStrip_1;

	private NovelInfo novelInfo_0;

	private ContextMenuStrip NovelMenuStrip;

	private Page page_0;

	private Panel panel_0;

	private Panel panel_1;

	private Panel panel_2;

	private TextBox posterBox;

	private RadioButton radioButton_0;

	private RadioButton radioButton_1;

	private RadioButton radioButton_2;

	private RadioButton radioButton_3;

	private RadioButton radioButton_4;

	private ReplaceConfigInfo replaceConfigInfo_0;

	private Panel ReviseChapter;

	private RuleConfigInfo ruleConfigInfo_0;

	private ComboBox sortBox;

	private SplitContainer splitContainer_0;

	private SplitContainer splitContainer_1;

	private SplitContainer splitContainer1;

	private StatusStrip statusStrip_0;

	private string string_0;

	private string string_1;

	private ListView target_list_view;

	private ContextMenuStrip TargetMenuStrip;

	private TaskConfigInfo taskConfigInfo_0;

	private Thread thr;

	private ToolStrip toolStrip_0;

	private ToolStripButton toolStripButton_0;

	private ToolStripButton toolStripButton_1;

	private ToolStripButton toolStripButton_3;

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

	private ToolStripMenuItem toolStripMenuItem_29;

	private ToolStripMenuItem toolStripMenuItem_3;

	private ToolStripMenuItem toolStripMenuItem_30;

	private ToolStripMenuItem toolStripMenuItem_31;

	private ToolStripMenuItem toolStripMenuItem_32;

	private ToolStripMenuItem toolStripMenuItem_33;

	private ToolStripMenuItem toolStripMenuItem_34;

	private ToolStripMenuItem toolStripMenuItem_35;

	private ToolStripMenuItem toolStripMenuItem_36;

	private ToolStripMenuItem toolStripMenuItem_37;

	private ToolStripMenuItem toolStripMenuItem_4;

	private ToolStripMenuItem toolStripMenuItem_5;

	private ToolStripMenuItem toolStripMenuItem_6;

	private ToolStripMenuItem toolStripMenuItem_7;

	private ToolStripMenuItem toolStripMenuItem_8;

	private ToolStripMenuItem toolStripMenuItem_9;

	private ToolStripSeparator toolStripSeparator_0;

	private ToolStripSeparator toolStripSeparator_1;

	private ToolStripSeparator toolStripSeparator_2;

	private ToolStripSeparator toolStripSeparator_3;

	private ToolStripSeparator toolStripSeparator_4;

	private ToolStripSeparator toolStripSeparator_5;

	private ToolStripSeparator toolStripSeparator_6;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripStatusLabel toolStripStatusLabel_0;

	private ToolStripStatusLabel toolStripStatusLabel_1;

	private ToolTip toolTip_0;

	public CollectManual()
	{
		replaceConfigInfo_0 = new ReplaceConfigInfo();
		ruleConfigInfo_0 = new RuleConfigInfo();
		string_0 = "GetId";
		string_1 = "";
		taskConfigInfo_0 = new TaskConfigInfo();
		InitializeComponent();
	}

	private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
		NovelInfo novelInfo = novelInfo_0;
		ChapterInfo[] array = (ChapterInfo[])e.Argument;
		for (int i = 0; i < array.Length; i++)
		{
			if (backgroundWorker.CancellationPending)
			{
				break;
			}
			bool flag = array[i].chaptertype == 0;
			NovelSpider.Local.LocalProvider.GetInstance().DeleteChapter(novelInfo, novelInfo.PutID, array[i].PutID, flag, bool_1: true);
		}
		NovelSpider.Local.LocalProvider.GetInstance().CreateIndex(novelInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
		ChapterInfo[] chapterList = NovelSpider.Local.LocalProvider.GetInstance().GetChapterList(novelInfo.PutID);
		ChapterInfo[] volumeNameList = NovelSpider.Local.LocalProvider.GetInstance().GetVolumeNameList(novelInfo.PutID);
		backgroundWorker.ReportProgress(30, chapterList);
		backgroundWorker.ReportProgress(34, volumeNameList);
		if (chapterList.Length != 0)
		{
			novelInfo.LastChapter = chapterList[chapterList.Length - 1];
		}
		backgroundWorker.ReportProgress(12, novelInfo);
		backgroundWorker.ReportProgress(13, novelInfo);
	}

	private void backgroundWorker_1_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		NovelInfo novelInfo = novelInfo_0;
		ChapterInfo[] array = (ChapterInfo[])e.Argument;
		for (int i = 0; i < array.Length; i++)
		{
			if (backgroundWorker.CancellationPending)
			{
				break;
			}
			ChapterInfo chapterInfo = NovelSpider.Local.LocalProvider.GetInstance().GetChapterInfo(novelInfo.PutID, array[i].PutID);
			chapterInfo.ItemIndex = array[i].ItemIndex;
			chapterInfo.VolumeName = array[i].VolumeName;
			backgroundWorker.ReportProgress(32, chapterInfo);
		}
	}

	private void backgroundWorker_10_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		NovelInfo novelInfo = (NovelInfo)e.Argument;
		if (novelInfo.NovelUrl == null)
		{
			novelInfo = page_0.GetNovelInfo(novelInfo);
		}
		int_0 = 0;
		backgroundWorker.ReportProgress(11, novelInfo);
		if (novelInfo.PutID == 0)
		{
			novelInfo = NovelSpider.Local.LocalProvider.GetInstance().GetNovelInfo(novelInfo, taskConfigInfo_0.NameAndAuthor);
		}
		NovelSpider.Local.LocalProvider.GetInstance().UpdateNovel(novelInfo, bool_0: false, bool_1: false, bool_2: true, bool_3: false, bool_4: false, bool_5: false, bool_6: false);
	}

	private void backgroundWorker_11_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		string[] array = (string[])e.Argument;
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('^');
			if (array2.Length != 5)
			{
				backgroundWorker.ReportProgress(35, "参数错误,请确认您选中的小说来自错误日志");
				continue;
			}
			try
			{
				SpiderException.removeSqlite(Convert.ToInt32(array2[1]), Convert.ToInt32(array2[2]), array2[3], array2[4]);
			}
			catch (Exception ex)
			{
				backgroundWorker.ReportProgress(35, ex.Message);
			}
		}
		backgroundWorker.ReportProgress(36, "");
		backgroundWorker.ReportProgress(35, "删除成功");
	}

	private void backgroundWorker_12_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		NovelInfo novelInfo = novelInfo_0;
		ChapterInfo[] array = (ChapterInfo[])e.Argument;
		for (int i = 0; i < array.Length; i++)
		{
			if (backgroundWorker.CancellationPending)
			{
				break;
			}
			NovelSpider.Local.LocalProvider.GetInstance().DeleteChapter(novelInfo, novelInfo.PutID, array[i].PutID, bool_0: true, bool_1: false);
		}
		NovelSpider.Local.LocalProvider.GetInstance().CreateIndex(novelInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
		ChapterInfo[] chapterList = NovelSpider.Local.LocalProvider.GetInstance().GetChapterList(novelInfo.PutID);
		ChapterInfo[] volumeNameList = NovelSpider.Local.LocalProvider.GetInstance().GetVolumeNameList(novelInfo.PutID);
		backgroundWorker.ReportProgress(30, chapterList);
		backgroundWorker.ReportProgress(34, volumeNameList);
		if (chapterList.Length != 0)
		{
			novelInfo.LastChapter = chapterList[chapterList.Length - 1];
		}
		backgroundWorker.ReportProgress(12, novelInfo);
		backgroundWorker.ReportProgress(13, novelInfo);
	}

	private void backgroundWorker_12_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		switch (e.ProgressPercentage)
		{
		case 10:
		{
			NovelInfo novelInfo3 = (NovelInfo)e.UserState;
			novelInfo3.ItemIndex = listView_2.Items.Count;
			DateTime now = DateTimeOffset.FromUnixTimeSeconds(novelInfo3.LastupDate).LocalDateTime;
			string text = now.ToString("yyyy-MM-dd HH:mm:ss");
			string[] items2 = new string[5]
			{
				novelInfo3.GetID,
				((novelInfo3.Degree == 0) ? "[载]" : "[完]") + novelInfo3.Name,
				novelInfo3.PutID.ToString(),
				novelInfo3.LastChapter.ChapterName,
				(novelInfo3.PutID != 0) ? text : "新作品"
			};
			listView_2.Items.Insert(novelInfo3.ItemIndex, new ListViewItem(items2));
			listView_2.Items[novelInfo3.ItemIndex].Tag = novelInfo3;
			listView_2.Items[novelInfo3.ItemIndex].UseItemStyleForSubItems = false;
			if (novelInfo3.Degree != 0)
			{
				listView_2.Items[novelInfo3.ItemIndex].SubItems[1].ForeColor = Color.Red;
			}
			TimeSpan timeSpan = DateTime.Now - now;
			if (timeSpan.Days < 1)
			{
				listView_2.Items[novelInfo3.ItemIndex].SubItems[4].BackColor = Color.Orange;
			}
			if (timeSpan.Days < 4 && timeSpan.Days > 0)
			{
				listView_2.Items[novelInfo3.ItemIndex].SubItems[4].BackColor = Color.SeaGreen;
			}
			if (timeSpan.Days > 3 && timeSpan.Days < 8)
			{
				listView_2.Items[novelInfo3.ItemIndex].SubItems[4].BackColor = Color.DeepPink;
			}
			if (timeSpan.Days > 7)
			{
				listView_2.Items[novelInfo3.ItemIndex].SubItems[4].BackColor = Color.Red;
			}
			break;
		}
		case 11:
		{
			NovelInfo novelInfo2 = (NovelInfo)e.UserState;
			listView_2.Items[novelInfo2.ItemIndex].SubItems[1].Text = ((novelInfo2.Degree == 0) ? "[载]" : "[完]") + novelInfo2.Name;
			listView_2.Items[novelInfo2.ItemIndex].UseItemStyleForSubItems = true;
			listView_2.Items[novelInfo2.ItemIndex].BackColor = Color.MistyRose;
			listView_2.Items[novelInfo2.ItemIndex].EnsureVisible();
			break;
		}
		case 12:
		{
			NovelInfo novelInfo4 = (NovelInfo)e.UserState;
			listView_2.Items[novelInfo4.ItemIndex].UseItemStyleForSubItems = true;
			listView_2.Items[novelInfo4.ItemIndex].SubItems[0].Text = novelInfo4.GetID;
			listView_2.Items[novelInfo4.ItemIndex].SubItems[1].Text = ((novelInfo4.Degree == 0) ? "[载]" : "[完]") + novelInfo4.Name;
			listView_2.Items[novelInfo4.ItemIndex].SubItems[2].Text = novelInfo4.PutID.ToString();
			listView_2.Items[novelInfo4.ItemIndex].SubItems[3].Text = novelInfo4.LastChapter.ChapterName;
			listView_2.Items[novelInfo4.ItemIndex].Tag = novelInfo4;
			break;
		}
		case 13:
		{
			NovelInfo tag = (NovelInfo)e.UserState;
			target_list_view.Tag = tag;
			listView_1.Tag = tag;
			break;
		}
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
			break;
		case 20:
		{
			ChapterInfo[] array2 = (ChapterInfo[])e.UserState;
			NovelInfo novelInfo6 = (NovelInfo)target_list_view.Tag;
			if (array2 == null)
			{
				target_list_view.Items.Clear();
				break;
			}
			bool flag = false;
			target_list_view.BeginUpdate();
			target_list_view.Items.Clear();
			int num = 0;
			int num2 = -1;
			for (int l = 0; l < array2.Length; l++)
			{
				array2[l].ItemIndex = l;
				if (!string.IsNullOrEmpty(taskConfigInfo_0.FilterContinueVolume[0].Trim()) && Regex.Match(array2[l].VolumeName, taskConfigInfo_0.FilterContinueVolume[0].ToString(), RegexOptions.IgnoreCase).Success)
				{
					if (l == 0 && !string.IsNullOrEmpty(array2[0].VolumeName.Trim()))
					{
						array2[0].VolumeName = Configs.BaseConfig.DefaultVolumeName;
					}
					else
					{
						array2[l].VolumeName = "";
					}
				}
				string[] array3 = new string[4]
				{
					(Convert.ToDouble(array2[l].ItemIndex) + Convert.ToDouble(1)).ToString(),
					array2[l].VolumeName,
					array2[l].ChapterName,
					""
				};
				string[] items4 = array3;
				target_list_view.Items.Insert(l, new ListViewItem(items4));
				target_list_view.Items[l].Tag = array2[l];
				switch (taskConfigInfo_0.EqualsChapter)
				{
				case 0:
					if (array2[l].ChapterName == novelInfo6.LastChapter.ChapterName)
					{
						num2 = l;
						target_list_view.Items[l].BackColor = Color.Red;
						flag = true;
					}
					break;
				case 1:
					if (array2[l].ChapterName == novelInfo6.LastChapter.ChapterName && array2[l].VolumeName == novelInfo6.LastChapter.VolumeName)
					{
						num2 = l;
						target_list_view.Items[l].BackColor = Color.Red;
						flag = true;
					}
					break;
				case 2:
					if (FormatText.CompareToChapter(array2[l].ChapterName, novelInfo6.LastChapter.ChapterName))
					{
						num2 = l;
						target_list_view.Items[l].BackColor = Color.Red;
						flag = true;
					}
					break;
				case 3:
				{
					int num4 = FormatText.CompareToChapter2(array2[l].ChapterName, novelInfo6.LastChapter.ChapterName, array2[l].VolumeName, novelInfo6.LastChapter.VolumeName);
					if (num4 > 6)
					{
						if (num4 >= num)
						{
							num = num4;
							num2 = l;
						}
						SpiderException.Debug(taskConfigInfo_0.ID, array2[l].ChapterName + " " + num4);
						target_list_view.Items[l].BackColor = Color.Pink;
						flag = true;
					}
					break;
				}
				case 4:
				{
					int num3 = FormatText.CompareToChapter3(array2[l].ChapterName, novelInfo6.LastChapter.ChapterName, array2[l].VolumeName, novelInfo6.LastChapter.VolumeName);
					if (num3 > 0)
					{
						if (num3 >= num)
						{
							num = num3;
							num2 = l;
						}
						SpiderException.Debug(taskConfigInfo_0.ID, array2[l].ChapterName + " " + num3);
						target_list_view.Items[l].BackColor = Color.Pink;
						flag = true;
					}
					break;
				}
				}
			}
			if (num2 >= 0)
			{
				target_list_view.Items[num2].BackColor = Color.Red;
				if (num2 > 5 && target_list_view.Items.Count > num2)
				{
					if (target_list_view.Items.Count - num2 > 3)
					{
						target_list_view.Items[num2 + 2].EnsureVisible();
					}
					else
					{
						target_list_view.Items[target_list_view.Items.Count - 1].EnsureVisible();
					}
				}
				else
				{
					target_list_view.Items[num2].EnsureVisible();
				}
			}
			if (!flag && array2.Length != 0)
			{
				target_list_view.Items[array2.Length - 1].EnsureVisible();
			}
			target_list_view.EndUpdate();
			break;
		}
		case 21:
		{
			ChapterInfo chapterInfo2 = (ChapterInfo)e.UserState;
			target_list_view.Items[chapterInfo2.ItemIndex].BackColor = Color.MistyRose;
			target_list_view.Items[chapterInfo2.ItemIndex].EnsureVisible();
			break;
		}
		case 22:
		{
			ChapterInfo chapterInfo = (ChapterInfo)e.UserState;
			target_list_view.Items[chapterInfo.ItemIndex].SubItems[1].Text = chapterInfo.VolumeName;
			target_list_view.Items[chapterInfo.ItemIndex].SubItems[2].Text = chapterInfo.ChapterName;
			target_list_view.Items[chapterInfo.ItemIndex].SubItems[3].Text = "文";
			target_list_view.Items[chapterInfo.ItemIndex].Checked = false;
			break;
		}
		case 30:
		{
			ChapterInfo[] array4 = (ChapterInfo[])e.UserState;
			NovelInfo novelInfo7 = (NovelInfo)target_list_view.Tag;
			if (array4 == null || array4.Length == 0)
			{
				listView_1.Items.Clear();
				break;
			}
			listView_1.BeginUpdate();
			listView_1.Items.Clear();
			for (int m = 0; m < array4.Length; m++)
			{
				array4[m].ItemIndex = m;
				string[] array3 = new string[5]
				{
					(Convert.ToDouble(array4[m].ItemIndex) + Convert.ToDouble(1)).ToString(),
					array4[m].VolumeName,
					array4[m].ChapterName,
					array4[m].LastTime.ToString("yyyy-MM-dd HH:mm:ss"),
					""
				};
				string[] items5 = array3;
				listView_1.Items.Insert(m, new ListViewItem(items5));
				listView_1.Items[m].UseItemStyleForSubItems = false;
				TimeSpan timeSpan2 = DateTime.Now - array4[m].LastTime;
				if (timeSpan2.Days < 1)
				{
					listView_1.Items[m].SubItems[3].BackColor = Color.Orange;
				}
				if (timeSpan2.Days < 4 && timeSpan2.Days > 0)
				{
					listView_1.Items[m].SubItems[3].BackColor = Color.SeaGreen;
				}
				if (timeSpan2.Days > 3 && timeSpan2.Days < 8)
				{
					listView_1.Items[m].SubItems[3].BackColor = Color.DeepPink;
				}
				if (timeSpan2.Days > 7)
				{
					listView_1.Items[m].SubItems[3].BackColor = Color.Red;
				}
				listView_1.Items[m].Tag = array4[m];
				listView_1.Items[m].EnsureVisible();
			}
			listView_1.Items[array4.Length - 1].EnsureVisible();
			listView_1.EndUpdate();
			break;
		}
		case 31:
		{
			ChapterInfo chapterInfo4 = (ChapterInfo)e.UserState;
			listView_1.Items[chapterInfo4.ItemIndex].BackColor = Color.MistyRose;
			listView_1.Items[chapterInfo4.ItemIndex].EnsureVisible();
			break;
		}
		case 32:
		{
			ChapterInfo chapterInfo3 = (ChapterInfo)e.UserState;
			listView_1.Items[chapterInfo3.ItemIndex].UseItemStyleForSubItems = true;
			listView_1.Items[chapterInfo3.ItemIndex].SubItems[1].Text = chapterInfo3.VolumeName;
			listView_1.Items[chapterInfo3.ItemIndex].SubItems[2].Text = chapterInfo3.ChapterName;
			listView_1.Items[chapterInfo3.ItemIndex].SubItems[3].Text = chapterInfo3.LastTime.ToString("yyyy-MM-dd HH:mm:ss");
			listView_1.Items[chapterInfo3.ItemIndex].SubItems[4].Text = "文";
			listView_1.Items[chapterInfo3.ItemIndex].Tag = chapterInfo3;
			listView_1.Items[chapterInfo3.ItemIndex].Checked = false;
			break;
		}
		case 33:
			listView_1.Items[int_3].EnsureVisible();
			break;
		case 34:
		{
			ChapterInfo[] array = (ChapterInfo[])e.UserState;
			NovelInfo novelInfo5 = (NovelInfo)target_list_view.Tag;
			if (array != null)
			{
				listView1.BeginUpdate();
				listView1.Items.Clear();
				for (int k = 0; k < array.Length; k++)
				{
					array[k].ItemIndex = k;
					string[] items3 = new string[2]
					{
						array[k].ItemIndex.ToString(),
						array[k].VolumeName
					};
					listView1.Items.Insert(k, new ListViewItem(items3));
					listView1.Items[k].Tag = array[k];
				}
				listView1.EndUpdate();
			}
			break;
		}
		case 35:
			MessageBox.Show((string)e.UserState);
			break;
		case 36:
		{
			List<NovelInfo> arrayList = new List<NovelInfo>();
			for (int i = 0; i < listView_2.Items.Count; i++)
			{
				if (!listView_2.Items[i].Checked)
				{
					arrayList.Add((NovelInfo)listView_2.Items[i].Tag);
				}
			}
			listView_2.Items.Clear();
			for (int j = 0; j < arrayList.Count; j++)
			{
				NovelInfo novelInfo = arrayList[j];
				novelInfo.ItemIndex = j;
				string[] items = new string[4]
				{
					novelInfo.GetID,
					novelInfo.Name,
					novelInfo.PutID.ToString(),
					novelInfo.LastChapter.ChapterName
				};
				listView_2.Items.Insert(novelInfo.ItemIndex, new ListViewItem(items));
				listView_2.Items[novelInfo.ItemIndex].Tag = novelInfo;
			}
			target_list_view.Items.Clear();
			listView_1.Items.Clear();
			listView1.Items.Clear();
			break;
		}
		}
	}

	private void backgroundWorker_12_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error != null)
		{
			toolStripStatusLabel_0.Text = "发生错误";
			MessageBox.Show(e.Error.Message);
		}
		else if (e.Cancelled)
		{
			toolStripStatusLabel_0.Text = "取消操作";
		}
		else
		{
			toolStripStatusLabel_0.Text = "操作完成";
		}
		panel_0.Visible = false;
		panel_1.Visible = false;
		button_0.Enabled = true;
		button_3.Enabled = true;
		bool_0 = false;
		panel_2.Enabled = true;
	}

	private void backgroundWorker_2_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		NovelInfo chapterInfo = novelInfo_0;
		for (int i = 0; i < chapterInfo_0.Length; i++)
		{
			if (backgroundWorker.CancellationPending)
			{
				break;
			}
			chapterInfo.LastChapter = chapterInfo_0[i];
			backgroundWorker.ReportProgress(21, chapterInfo.LastChapter);
			chapterInfo = page_0.GetChapterInfo(chapterInfo, isvip: false);
			backgroundWorker.ReportProgress(22, chapterInfo.LastChapter);
			if (chapterInfo.LastChapter.ChapterText == null || chapterInfo.LastChapter.ChapterText.Trim() == "")
			{
				string strTask = comboBox_2.Text + " | " + comboBox_3.Text;
				SpiderException.Show("发现空章节", chapterInfo, taskConfigInfo_0.Log, strTask);
				break;
			}
			if (Regex.Match(chapterInfo.LastChapter.ChapterText, "<img", RegexOptions.IgnoreCase).Success && taskConfigInfo_0.OnlyText)
			{
				string strTask2 = comboBox_2.Text + " | " + comboBox_3.Text;
				SpiderException.Show("发现图片章节", chapterInfo, taskConfigInfo_0.Log, strTask2);
				break;
			}
			chapterInfo.LastChapter.PutID = chapterInfo_1[i].PutID;
			chapterInfo.LastChapter.LastTime = DateTime.Now;
			chapterInfo_1[i].ChapterName = chapterInfo.LastChapter.ChapterName;
			chapterInfo_1[i].ChapterText = chapterInfo.LastChapter.ChapterText;
			backgroundWorker.ReportProgress(31, chapterInfo_1[i]);
			NovelSpider.Local.LocalProvider.GetInstance().UpdateChapter(chapterInfo, replaceConfigInfo_0);
			if (Configs.BaseConfig.ChapterHtml)
			{
				NovelSpider.Local.LocalProvider.GetInstance().CreateChapter(chapterInfo);
			}
			backgroundWorker.ReportProgress(32, chapterInfo_1[i]);
		}
		WaitForBackgroundAsync(LocalProviderAsyncDispatcher.UpdateLastChapterAsync(NovelSpider.Local.LocalProvider.GetInstance(), chapterInfo));
		NovelSpider.Local.LocalProvider.GetInstance().CreateIndex(chapterInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
	}

	private void backgroundWorker_3_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
		Page page = new Page(ruleConfigInfo_0, taskConfigInfo_0);
		NovelInfo[] array = null;
		switch (string_0)
		{
		case "Put":
		{
			array = NovelSpider.Local.LocalProvider.GetInstance().GetNovelList(string_1);
			for (int j = 0; j < array.Length; j++)
			{
				backgroundWorker.ReportProgress(10, array[j]);
			}
			break;
		}
		case "NovelName":
		{
			array = new NovelInfo[1]
			{
				new NovelInfo()
			};
			array[0].Name = string_1;
			array[0] = page.GetNovelInfo(array[0]);
			array[0] = NovelSpider.Local.LocalProvider.GetInstance().GetNovelInfo(array[0], taskConfigInfo_0.NameAndAuthor);
			for (int l = 0; l < array.Length; l++)
			{
				backgroundWorker.ReportProgress(10, array[l]);
			}
			break;
		}
		case "GetId":
			array = new NovelInfo[1]
			{
				new NovelInfo()
			};
			if (string_1.IndexOf(',') > 0)
			{
				string[] array3 = string_1.Split(',');
				array = new NovelInfo[array3.Length];
				for (int m = 0; m < array3.Length; m++)
				{
					array[m] = new NovelInfo
					{
						GetID = array3[m]
					};
					try
					{
						array[m] = page.GetNovelInfo(array[m]);
					}
					catch (Exception)
					{
						continue;
					}
					array[m] = NovelSpider.Local.LocalProvider.GetInstance().GetNovelInfo(array[m], taskConfigInfo_0.NameAndAuthor);
					if (array[m] != null)
					{
						backgroundWorker.ReportProgress(10, array[m]);
					}
				}
			}
			else
			{
				array[0].GetID = string_1;
				array[0] = page.GetNovelInfo(array[0]);
				array[0] = NovelSpider.Local.LocalProvider.GetInstance().GetNovelInfo(array[0], taskConfigInfo_0.NameAndAuthor);
				for (int n = 0; n < array.Length; n++)
				{
					backgroundWorker.ReportProgress(10, array[n]);
				}
			}
			break;
		case "PutId":
		{
			array = new NovelInfo[1]
			{
				new NovelInfo()
			};
			array[0].PutID = Convert.ToInt32(string_1);
			array[0] = NovelSpider.Local.LocalProvider.GetInstance().GetNovelInfo(array[0], taskConfigInfo_0.NameAndAuthor);
			array[0] = page.GetNovelInfo(array[0]);
			for (int k = 0; k < array.Length; k++)
			{
				backgroundWorker.ReportProgress(10, array[k]);
			}
			break;
		}
		case "Get":
		{
			string[] array2 = string_1.Split(',');
			array = page.GetNovelList(array2);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = page_0.GetNovelInfo(array[i]);
				array[i] = NovelSpider.Local.LocalProvider.GetInstance().GetNovelInfo(array[i], taskConfigInfo_0.NameAndAuthor);
				if (array[i] != null)
				{
					backgroundWorker.ReportProgress(10, array[i]);
				}
			}
			break;
		}
		}
		e.Result = array;
	}

	private void backgroundWorker_5_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
		NovelInfo[] array = (NovelInfo[])e.Argument;
		for (int i = 0; i < array.Length; i++)
		{
			backgroundWorker.ReportProgress(11, array[i]);
			if (array[i].PutID == 0)
			{
				array[i] = NovelSpider.Local.LocalProvider.GetInstance().GetNovelInfo(array[i], taskConfigInfo_0.NameAndAuthor);
			}
			else if (array[i].GetID == "" || array[i].GetID == null)
			{
				array[i] = new Page(ruleConfigInfo_0, taskConfigInfo_0).GetNovelInfo(array[i]);
			}
			backgroundWorker.ReportProgress(12, array[i]);
		}
	}

	private void backgroundWorker_6_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		NovelInfo chapterInfo = novelInfo_0;
		ChapterInfo[] array = (ChapterInfo[])e.Argument;
		int i = 0;
		try
		{
			for (; i < array.Length; i++)
			{
				if (backgroundWorker.CancellationPending)
				{
					break;
				}
				chapterInfo.LastChapter = null;
				chapterInfo.LastChapter = array[i];
				backgroundWorker.ReportProgress(21, chapterInfo.LastChapter);
				chapterInfo = page_0.GetChapterInfo(chapterInfo, isvip: false);
				backgroundWorker.ReportProgress(22, chapterInfo.LastChapter);
				if (chapterInfo.LastChapter.ChapterText == null || chapterInfo.LastChapter.ChapterText.Trim() == "")
				{
					string str3 = string.Empty;
					Invoke((MethodInvoker)delegate
					{
						str3 = comboBox_2.Text + " | " + comboBox_3.Text;
					});
					SpiderException.Show("发现空章节", chapterInfo, taskConfigInfo_0.Log, str3);
					break;
				}
				if (Regex.IsMatch(chapterInfo.LastChapter.ChapterText, "<img", RegexOptions.IgnoreCase) && taskConfigInfo_0.OnlyText)
				{
					string str4 = string.Empty;
					Invoke((MethodInvoker)delegate
					{
						str4 = comboBox_2.Text + " | " + comboBox_3.Text;
					});
					SpiderException.Show("发现图片章节", chapterInfo, taskConfigInfo_0.Log, str4);
					break;
				}
				if (chapterInfo.LastChapter.ChapterText.Length <= taskConfigInfo_0.MinChapterTextLength)
				{
					string str5 = string.Empty;
					Invoke((MethodInvoker)delegate
					{
						str5 = comboBox_2.Text + " | " + comboBox_3.Text;
					});
					SpiderException.Show("空章节或字数过少", chapterInfo, taskConfigInfo_0.Log, str5);
					break;
				}
				WaitForBackgroundAsync(LocalProviderAsyncDispatcher.InsertChapterAsync(NovelSpider.Local.LocalProvider.GetInstance(), chapterInfo, taskConfigInfo_0));
				if (Configs.BaseConfig.ChapterHtml)
				{
					NovelSpider.Local.LocalProvider.GetInstance().CreateChapter(chapterInfo);
				}
			}
			NovelSpider.Local.LocalProvider.GetInstance().CreateIndex(chapterInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
			ChapterInfo[] chapterList = NovelSpider.Local.LocalProvider.GetInstance().GetChapterList(chapterInfo.PutID);
			backgroundWorker.ReportProgress(30, chapterList);
			if (chapterList.Length != 0)
			{
				chapterInfo.LastChapter = chapterList[chapterList.Length - 1];
			}
			backgroundWorker.ReportProgress(12, chapterInfo);
			backgroundWorker.ReportProgress(13, chapterInfo);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void backgroundWorker_7_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		NovelInfo novelInfo = (NovelInfo)e.Argument;
		if (novelInfo.NovelUrl == null)
		{
			novelInfo = page_0.GetNovelInfo(novelInfo);
		}
		int_0 = 0;
		backgroundWorker.ReportProgress(11, novelInfo);
		if (novelInfo.PutID == 0)
		{
			novelInfo = NovelSpider.Local.LocalProvider.GetInstance().GetNovelInfo(novelInfo, taskConfigInfo_0.NameAndAuthor);
		}
		else if (novelInfo.GetID == "" || novelInfo.GetID == null)
		{
			novelInfo = page_0.GetNovelInfo(novelInfo);
		}
		ChapterInfo[] chapterList = NovelSpider.Local.LocalProvider.GetInstance().GetChapterList(novelInfo.PutID);
		backgroundWorker.ReportProgress(30, chapterList);
		if (chapterList != null && chapterList.Length != 0)
		{
			novelInfo.LastChapter = chapterList[chapterList.Length - 1];
			backgroundWorker.ReportProgress(12, novelInfo);
			backgroundWorker.ReportProgress(13, novelInfo);
		}
		ChapterInfo[] chapterList2 = page_0.GetChapterList(novelInfo);
		ChapterInfo[] volumeNameList = NovelSpider.Local.LocalProvider.GetInstance().GetVolumeNameList(novelInfo.PutID);
		backgroundWorker.ReportProgress(20, chapterList2);
		backgroundWorker.ReportProgress(34, volumeNameList);
	}

	private void backgroundWorker_8_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
		NovelInfo[] array = (NovelInfo[])e.Argument;
		for (int i = 0; i < array.Length; i++)
		{
			backgroundWorker.ReportProgress(11, array[i]);
			if (array[i].PutID == 0)
			{
				array[i] = NovelSpider.Local.LocalProvider.GetInstance().GetNovelInfo(array[i], taskConfigInfo_0.NameAndAuthor);
			}
			if (array[i].PutID == 0)
			{
				array[i] = new Page(ruleConfigInfo_0, taskConfigInfo_0).GetNovelInfo(array[i]);
				array[i] = WaitForBackgroundAsync(LocalProviderAsyncDispatcher.InsertNovelAsync(NovelSpider.Local.LocalProvider.GetInstance(), array[i]));
			}
			backgroundWorker.ReportProgress(12, array[i]);
		}
	}

	private void backgroundWorker_9_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorker backgroundWorker = sender as BackgroundWorker;
		NovelInfo chapterInfo = novelInfo_0;
		int num = chapterInfo_0.Length;
		int[] array = NovelSpider.Local.LocalProvider.GetInstance().UpdateChapterOrder(chapterInfo, num, int_1);
		if (array[1] == -1 || (array[1] == -1 && num > 1) || array[0] == 0)
		{
			return;
		}
		for (int i = 0; i < chapterInfo_0.Length; i++)
		{
			if (backgroundWorker.CancellationPending)
			{
				break;
			}
			chapterInfo.LastChapter = chapterInfo_0[i];
			backgroundWorker.ReportProgress(21, chapterInfo.LastChapter);
			chapterInfo = page_0.GetChapterInfo(chapterInfo, isvip: false);
			backgroundWorker.ReportProgress(22, chapterInfo.LastChapter);
			if (chapterInfo.LastChapter.ChapterText == null || chapterInfo.LastChapter.ChapterText.Trim() == "")
			{
				string strTask = comboBox_2.Text + " | " + comboBox_3.Text;
				SpiderException.Show("发现空章节", chapterInfo, taskConfigInfo_0.Log, strTask);
				break;
			}
			if (Regex.Match(chapterInfo.LastChapter.ChapterText, "<img", RegexOptions.IgnoreCase).Success && taskConfigInfo_0.OnlyText)
			{
				string strTask2 = comboBox_2.Text + " | " + comboBox_3.Text;
				SpiderException.Show("发现图片章节", chapterInfo, taskConfigInfo_0.Log, strTask2);
				break;
			}
			WaitForBackgroundAsync(LocalProviderAsyncDispatcher.InsertChapterByOrderAsync(NovelSpider.Local.LocalProvider.GetInstance(), chapterInfo, taskConfigInfo_0, array[0] + i));
			if (Configs.BaseConfig.ChapterHtml)
			{
				NovelSpider.Local.LocalProvider.GetInstance().CreateChapter(chapterInfo);
			}
			ChapterInfo[] chapterList = NovelSpider.Local.LocalProvider.GetInstance().GetChapterList(chapterInfo.PutID);
			backgroundWorker.ReportProgress(30, chapterList);
			ChapterInfo[] volumeNameList = NovelSpider.Local.LocalProvider.GetInstance().GetVolumeNameList(chapterInfo.PutID);
			backgroundWorker.ReportProgress(34, volumeNameList);
			backgroundWorker.ReportProgress(33, null);
		}
		ChapterInfo chapterInfo2 = new ChapterInfo();
		chapterInfo2.PutID = array[1];
		ChapterInfo chapterInfo3 = chapterInfo2;
		ChapterInfo chapterInfo4 = new ChapterInfo();
		chapterInfo4.PutID = int_1;
		ChapterInfo chapterInfo5 = chapterInfo4;
		NovelSpider.Local.LocalProvider.GetInstance().CreateChapter(chapterInfo, chapterInfo3);
		NovelSpider.Local.LocalProvider.GetInstance().CreateChapter(chapterInfo, chapterInfo5);
		WaitForBackgroundAsync(LocalProviderAsyncDispatcher.UpdateLastChapterAsync(NovelSpider.Local.LocalProvider.GetInstance(), chapterInfo));
		NovelSpider.Local.LocalProvider.GetInstance().CreateIndex(chapterInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
	}

	private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
	{
		NovelInfo novelInfo = (NovelInfo)e.Argument;
		NovelSpider.Local.LocalProvider.GetInstance().UpdateNovel(novelInfo, bool_0: true, bool_1: false, bool_2: false, bool_3: true, bool_4: false, bool_5: false, bool_6: false);
		NovelSpider.Local.LocalProvider.GetInstance().CreateIndex(novelInfo, Configs.BaseConfig.IndexHtml, Configs.BaseConfig.FullHtml, Configs.BaseConfig.CreateOPF, Configs.BaseConfig.CreateZIP, Configs.BaseConfig.CreateTXT, Configs.BaseConfig.CreateUMD, Configs.BaseConfig.CreateJAR, Configs.BaseConfig.CreateCHM, bool_8: false, bool_9: false, 0);
	}

	private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		ReviseChapter.Visible = false;
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			listView_1.Items[i].Checked = false;
		}
		sortBox.Enabled = true;
		button2.Enabled = true;
		button1.Enabled = true;
		toolStripStatusLabel_0.Text = "操作完成";
	}

	private void button_0_Click(object sender, EventArgs e)
	{
		if (!Configs.BaseConfig.LicenseOk || bool_0 || backgroundWorker_3.IsBusy)
		{
			return;
		}
		toolStripStatusLabel_0.Text = "正在单本载入.请勿进行其他操作..";
		bool_0 = true;
		panel_2.Enabled = false;
		listView_1.Items.Clear();
		target_list_view.Items.Clear();
		button_0.Enabled = false;
		if (comboBox_0.Text != "" && !backgroundWorker_3.IsBusy)
		{
			if (radioButton_2.Checked)
			{
				string_0 = "GetId";
			}
			if (radioButton_1.Checked)
			{
				string_0 = "NovelName";
			}
			if (radioButton_0.Checked)
			{
				string_0 = "PutId";
			}
			string_1 = comboBox_0.Text;
			listView_2.Items.Clear();
			backgroundWorker_3.RunWorkerAsync();
		}
	}

	private void button_1_Click(object sender, EventArgs e)
	{
		panel_0.Visible = false;
	}

	private void button_2_Click(object sender, EventArgs e)
	{
		panel_1.Visible = false;
	}

	private void button_3_Click(object sender, EventArgs e)
	{
		if (!Configs.BaseConfig.LicenseOk || bool_0 || backgroundWorker_3.IsBusy)
		{
			return;
		}
		toolStripStatusLabel_0.Text = "正在批量载入.请勿进行其他操作..";
		bool_0 = true;
		panel_2.Enabled = false;
		listView_1.Items.Clear();
		target_list_view.Items.Clear();
		button_3.Enabled = false;
		if (comboBox_1.Text != "" && !backgroundWorker_3.IsBusy)
		{
			if (radioButton_4.Checked)
			{
				string_0 = "Get";
			}
			if (radioButton_3.Checked)
			{
				string_0 = "Put";
			}
			string_1 = comboBox_1.Text;
			listView_2.Items.Clear();
			backgroundWorker_3.RunWorkerAsync();
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		button2.Enabled = false;
		button1.Enabled = false;
		toolStripStatusLabel_0.Text = "正在执行";
		if (Configs.CmsName != "UnsupportedCms")
		{
			if (articlenameBox.Text != "" && posterBox.Text != "" && chapterNameBox.Text != "" && chapterTXTBox.Text != "")
			{
				
				for (int i = 0; i < listView_1.Items.Count; i++)
				{
					if (!listView_1.Items[i].Checked)
					{
						continue;
					}
					NovelInfo novelInfo = (NovelInfo)target_list_view.Tag;
					ChapterInfo chapterInfo = (ChapterInfo)listView_1.Items[i].Tag;
					novelInfo.ReviseChapterID = chapterInfo.PutID;
					novelInfo.LastChapter.ChapterText = chapterTXTBox.Text;
					novelInfo.ReviseChapter = chapterNameBox.Text;
					novelInfo.Name = articlenameBox.Text;
					listView_1.Items[i].SubItems[2].Text = novelInfo.ReviseChapter;
					novelInfo.Author = posterBox.Text;
					for (int j = 0; j < NovelSpider.Local.Jieqi.Config.JieqiSort.Length; j++)
					{
						if (sortBox.SelectedItem.ToString().Equals(NovelSpider.Local.Jieqi.Config.JieqiSort[j]))
						{
							novelInfo.MLagerSortID = j;
							novelInfo.MLagerSort = sortBox.SelectedItem.ToString();
							break;
						}
					}
					if (novelInfo.LastChapter.ChapterText != "恭喜中奖了！又碰到无TXT文本的章节！或些章节为图片章节！")
					{
						string chapterPath = NovelSpider.Local.Jieqi.Config.TxtDir + "/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID.ToString() + "/" + chapterInfo.PutID.ToString() + ".txt";
						File.WriteAllText(chapterPath, NovelSpider.Local.Jieqi.TextEncodingPolicy.NormalizeDatabaseText(novelInfo.LastChapter.ChapterText), NovelSpider.Local.Jieqi.TextEncodingPolicy.Utf8NoBom);
					}
					backgroundWorker1.RunWorkerAsync(novelInfo);
				}
			}
			else if (articlenameBox.Text == "")
			{
				MessageBox.Show("小说名不能为空");
			}
			else if (posterBox.Text == "")
			{
				MessageBox.Show("作者不能为空");
			}
			else if (chapterNameBox.Text == "")
			{
				MessageBox.Show("章节名不能为空");
			}
			else if (articlenameBox.Text == "")
			{
				MessageBox.Show("小说名不能为空");
			}
			else if (chapterTXTBox.Text == "")
			{
				MessageBox.Show("章节内容不能为空");
			}
		}
		else if (articlenameBox.Text != "" && posterBox.Text != "" && chapterNameBox.Text != "" && chapterTXTBox.Text != "")
		{
			
			for (int i = 0; i < listView_1.Items.Count; i++)
			{
				if (!listView_1.Items[i].Checked)
				{
					continue;
				}
				NovelInfo novelInfo = (NovelInfo)target_list_view.Tag;
				ChapterInfo chapterInfo = (ChapterInfo)listView_1.Items[i].Tag;
				novelInfo.ReviseChapterID = chapterInfo.PutID;
				novelInfo.LastChapter.ChapterText = chapterTXTBox.Text;
				novelInfo.ReviseChapter = chapterNameBox.Text;
				novelInfo.Name = articlenameBox.Text;
				listView_1.Items[i].SubItems[2].Text = novelInfo.ReviseChapter;
				novelInfo.Author = posterBox.Text;
				if (novelInfo.LastChapter.ChapterText != "恭喜中奖了！又碰到无TXT文本的章节！或些章节为图片章节！")
				{
					string chapterPath = NovelSpider.Local.Jieqi.Config.TxtDir + "/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID.ToString() + "/" + chapterInfo.PutID.ToString() + ".txt";
					File.WriteAllText(chapterPath, NovelSpider.Local.Jieqi.TextEncodingPolicy.NormalizeDatabaseText(novelInfo.LastChapter.ChapterText), NovelSpider.Local.Jieqi.TextEncodingPolicy.Utf8NoBom);
				}
				backgroundWorker1.RunWorkerAsync(novelInfo);
			}
		}
		else if (articlenameBox.Text == "")
		{
			MessageBox.Show("小说名不能为空");
		}
		else if (posterBox.Text == "")
		{
			MessageBox.Show("作者不能为空");
		}
		else if (chapterNameBox.Text == "")
		{
			MessageBox.Show("章节名不能为空");
		}
		else if (articlenameBox.Text == "")
		{
			MessageBox.Show("小说名不能为空");
		}
		else if (chapterTXTBox.Text == "")
		{
			MessageBox.Show("章节内容不能为空");
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("你确定要放弃当前操作？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
		{
			ReviseChapter.Visible = false;
		}
	}

	private void CollectManual_Load(object sender, EventArgs e)
	{
		taskConfigInfo_0 = (TaskConfigInfo)ConfigFileManager.LoadConfig("TaskConfig.xml", taskConfigInfo_0);
		try
		{
			comboBox_3.BeginUpdate();
			string[] array = IO.LoadRules();
			if (array.Length != 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					comboBox_3.Items.Add(array[i]);
					if (array[i] == taskConfigInfo_0.RuleFile)
					{
						comboBox_3.Text = taskConfigInfo_0.RuleFile;
						ruleConfigInfo_0 = (RuleConfigInfo)ConfigFileManager.LoadConfig(taskConfigInfo_0.RuleFile, ruleConfigInfo_0);
					}
				}
			}
			comboBox_3.EndUpdate();
			comboBox_2.BeginUpdate();
			comboBox_2.Items.Clear();
			comboBox_2.Items.Add("TaskConfig.xml");
			comboBox_2.Text = "TaskConfig.xml";
			string[] array2 = IO.LoadTasks();
			if (array2.Length != 0)
			{
				for (int j = 0; j < array2.Length; j++)
				{
					comboBox_2.Items.Add(array2[j]);
				}
			}
			comboBox_2.EndUpdate();
			replaceConfigInfo_0 = (ReplaceConfigInfo)ConfigFileManager.LoadConfig("ReplaceConfig.xml", replaceConfigInfo_0);
			page_0 = new Page(ruleConfigInfo_0, taskConfigInfo_0);
			method_0();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "程序错误");
		}
		comboBox_1.Text = ruleConfigInfo_0.NovelListUrl.Pattern;
	}

	private void comboBox_2_SelectedIndexChanged(object sender, EventArgs e)
	{
		string fileName = comboBox_2.Text;
		new FileInfo(fileName);
		taskConfigInfo_0 = (TaskConfigInfo)ConfigFileManager.LoadConfig(fileName, taskConfigInfo_0);
		comboBox_3.Text = taskConfigInfo_0.RuleFile;
		ruleConfigInfo_0 = (RuleConfigInfo)ConfigFileManager.LoadConfig(comboBox_3.Text, ruleConfigInfo_0);
		page_0 = new Page(ruleConfigInfo_0, taskConfigInfo_0);
		method_0();
	}

	private void comboBox_3_SelectedIndexChanged(object sender, EventArgs e)
	{
		taskConfigInfo_0.RuleFile = comboBox_3.Text;
		ruleConfigInfo_0 = (RuleConfigInfo)ConfigFileManager.LoadConfig(taskConfigInfo_0.RuleFile, ruleConfigInfo_0);
		comboBox_1.Text = ruleConfigInfo_0.NovelListUrl.Pattern;
		page_0 = new Page(ruleConfigInfo_0, taskConfigInfo_0);
		listView_2.Items.Clear();
		listView_1.Items.Clear();
		target_list_view.Items.Clear();
	}

	private void Db3InsertButton_Click(object sender, EventArgs e)
	{
		if (comboBox_4.Text.Length < 5)
		{
			return;
		}
		FileInfo fileInfo = new FileInfo("Log\\" + comboBox_4.Text);
		if (!fileInfo.Exists)
		{
			return;
		}
		listView_2.Items.Clear();
		string text = "Data Source=" + fileInfo.FullName;
		string text2 = "Select * From [TaskLog] Where RULEFILE='" + comboBox_3.Text + "' And NovelName<>''";
		if (ErrIdcomboBox.Text != "EXIT" && ErrIdcomboBox.Text != "")
		{
			if (ErrIdcomboBox.Text == "")
			{
				ErrIdcomboBox.Text = "EXID";
			}
			if (ErrIdcomboBox.Text != "EXID")
			{
				ErrIdcomboBox.Text = ErrIdcomboBox.Text.Split(' ')[0];
			}
			text2 = text2 + " And EXID= " + ErrIdcomboBox.Text;
		}
		DataSet dataSet = SQLiteHelper.ExecuteDataset(text, text2);
		if (dataSet == null || dataSet.Tables[0].Rows.Count < 1)
		{
			return;
		}
		string text3 = "";
		for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
		{
			NovelInfo novelInfo = new NovelInfo();
			novelInfo.GetID = dataSet.Tables[0].Rows[i]["GETID"].ToString();
			NovelInfo novelInfo2 = novelInfo;
			if (!text3.Contains("," + novelInfo2.GetID + ",") && novelInfo2.GetID != "0")
			{
				text3 = text3 + novelInfo2.GetID + ",";
			}
		}
		text3 = text3.Trim(',');
		panel_0.Visible = false;
		if (panel_0.Visible)
		{
			panel_0.Visible = false;
		}
		else
		{
			panel_0.Visible = true;
		}
		comboBox_0.Text = text3;
		if (!bool_0 && !backgroundWorker_3.IsBusy)
		{
			toolStripStatusLabel_0.Text = "正在单本载入.请勿进行其他操作..";
			bool_0 = true;
			panel_2.Enabled = false;
			listView_1.Items.Clear();
			target_list_view.Items.Clear();
			button_0.Enabled = false;
			if (comboBox_0.Text != "" && !backgroundWorker_3.IsBusy)
			{
				string_0 = "GetId";
				string_1 = comboBox_0.Text;
				listView_2.Items.Clear();
				backgroundWorker_3.RunWorkerAsync();
			}
		}
	}

	private void DelSelectLog_Click(object sender, EventArgs e)
	{
		if (listView_2.CheckedItems.Count < 1 || bool_0 || backgroundWorker_11.IsBusy)
		{
			bool_0 = true;
		}
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在从日志中删除.请勿进行其他操作..";
		List<string> arrayList = new List<string>();
		for (int i = 0; i < listView_2.Items.Count; i++)
		{
			if (listView_2.Items[i].Checked)
			{
				NovelInfo novelInfo = (NovelInfo)listView_2.Items[i].Tag;
				string value = i + "^" + novelInfo.PutID + "^" + novelInfo.GetID + "^" + novelInfo.Name + "^" + comboBox_4.Text;
				arrayList.Add(value);
			}
		}
		backgroundWorker_11.RunWorkerAsync(arrayList.ToArray());
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NovelSpider.CollectManual));
		this.toolStrip_0 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton_0 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton_1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton_3 = new System.Windows.Forms.ToolStripButton();
		this.statusStrip_0 = new System.Windows.Forms.StatusStrip();
		this.toolStripStatusLabel_0 = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolStripStatusLabel_1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.splitContainer_0 = new System.Windows.Forms.SplitContainer();
		this.panel_0 = new System.Windows.Forms.Panel();
		this.button_1 = new System.Windows.Forms.Button();
		this.label_0 = new System.Windows.Forms.Label();
		this.button_0 = new System.Windows.Forms.Button();
		this.comboBox_0 = new System.Windows.Forms.ComboBox();
		this.radioButton_0 = new System.Windows.Forms.RadioButton();
		this.radioButton_1 = new System.Windows.Forms.RadioButton();
		this.radioButton_2 = new System.Windows.Forms.RadioButton();
		this.panel_1 = new System.Windows.Forms.Panel();
		this.label_1 = new System.Windows.Forms.Label();
		this.button_2 = new System.Windows.Forms.Button();
		this.button_3 = new System.Windows.Forms.Button();
		this.comboBox_1 = new System.Windows.Forms.ComboBox();
		this.radioButton_3 = new System.Windows.Forms.RadioButton();
		this.radioButton_4 = new System.Windows.Forms.RadioButton();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.listView_2 = new System.Windows.Forms.ListView();
		this.手动信息目标站ID = new System.Windows.Forms.ColumnHeader();
		this.手动信息小说名称 = new System.Windows.Forms.ColumnHeader();
		this.手动信息本站ID = new System.Windows.Forms.ColumnHeader();
		this.手动信息本站最新章节情况 = new System.Windows.Forms.ColumnHeader();
		this.手动信息更新时间 = new System.Windows.Forms.ColumnHeader();
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
		this.toolStripMenuItem_32 = new System.Windows.Forms.ToolStripMenuItem();
		this.DelSelectLog = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_5 = new System.Windows.Forms.ToolStripMenuItem();
		this.listView1 = new System.Windows.Forms.ListView();
		this.columnHeader_12 = new System.Windows.Forms.ColumnHeader();
		this.columnHeader_13 = new System.Windows.Forms.ColumnHeader();
		this.LocalMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripMenuItem_31 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_29 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_28 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_0 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_35 = new System.Windows.Forms.ToolStripMenuItem();
		this.panel_2 = new System.Windows.Forms.Panel();
		this.Db3InsertButton = new System.Windows.Forms.Button();
		this.ErrIdcomboBox = new System.Windows.Forms.ComboBox();
		this.comboBox_4 = new System.Windows.Forms.ComboBox();
		this.comboBox_3 = new System.Windows.Forms.ComboBox();
		this.comboBox_2 = new System.Windows.Forms.ComboBox();
		this.splitContainer_1 = new System.Windows.Forms.SplitContainer();
		this.target_list_view = new System.Windows.Forms.ListView();
		this.左手动索引 = new System.Windows.Forms.ColumnHeader();
		this.左手动分卷名 = new System.Windows.Forms.ColumnHeader();
		this.左手动章节名 = new System.Windows.Forms.ColumnHeader();
		this.左手动内容 = new System.Windows.Forms.ColumnHeader();
		this.TargetMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripMenuItem_6 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_34 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_7 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_8 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_22 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_9 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_10 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_37 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_3 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_11 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_23 = new System.Windows.Forms.ToolStripMenuItem();
		this.listView_1 = new System.Windows.Forms.ListView();
		this.右手动索引 = new System.Windows.Forms.ColumnHeader();
		this.右手动分卷名 = new System.Windows.Forms.ColumnHeader();
		this.右手动章节名 = new System.Windows.Forms.ColumnHeader();
		this.右手动更新时间 = new System.Windows.Forms.ColumnHeader();
		this.右手动内容 = new System.Windows.Forms.ColumnHeader();
		this.LocalMenuStrip_1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.toolStripMenuItem_12 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_4 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_25 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_33 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_30 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_17 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_27 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_26 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_13 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator_5 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripMenuItem_14 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_24 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_15 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_16 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem_36 = new System.Windows.Forms.ToolStripMenuItem();
		this.backgroundWorker_3 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_4 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_5 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_6 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_7 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_8 = new System.ComponentModel.BackgroundWorker();
		this.toolTip_0 = new System.Windows.Forms.ToolTip(this.components);
		this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_1 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_2 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_9 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_10 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_11 = new System.ComponentModel.BackgroundWorker();
		this.backgroundWorker_12 = new System.ComponentModel.BackgroundWorker();
		this.ReviseChapter = new System.Windows.Forms.Panel();
		this.button2 = new System.Windows.Forms.Button();
		this.sortBox = new System.Windows.Forms.ComboBox();
		this.posterBox = new System.Windows.Forms.TextBox();
		this.articlenameBox = new System.Windows.Forms.TextBox();
		this.chapterNameBox = new System.Windows.Forms.TextBox();
		this.button1 = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.chapterTXTBox = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.toolStrip_0.SuspendLayout();
		this.statusStrip_0.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer_0).BeginInit();
		this.splitContainer_0.Panel1.SuspendLayout();
		this.splitContainer_0.Panel2.SuspendLayout();
		this.splitContainer_0.SuspendLayout();
		this.panel_0.SuspendLayout();
		this.panel_1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		this.NovelMenuStrip.SuspendLayout();
		this.LocalMenuStrip.SuspendLayout();
		this.panel_2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer_1).BeginInit();
		this.splitContainer_1.Panel1.SuspendLayout();
		this.splitContainer_1.Panel2.SuspendLayout();
		this.splitContainer_1.SuspendLayout();
		this.TargetMenuStrip.SuspendLayout();
		this.LocalMenuStrip_1.SuspendLayout();
		this.ReviseChapter.SuspendLayout();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip_0.ImageScalingSize = new System.Drawing.Size(32, 32);
		this.toolStrip_0.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripButton_0, this.toolStripButton_1, this.toolStripButton_3 });
		this.toolStrip_0.Location = new System.Drawing.Point(0, 0);
		this.toolStrip_0.Name = "toolStrip_0";
		this.toolStrip_0.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.toolStrip_0.Size = new System.Drawing.Size(964, 39);
		this.toolStrip_0.TabIndex = 0;
		this.toolStrip_0.Text = "工具栏";
		this.toolStripButton_0.Font = new System.Drawing.Font("宋体", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton_0.Image = null;
		this.toolStripButton_0.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton_0.Name = "toolStripButton_0";
		this.toolStripButton_0.Size = new System.Drawing.Size(99, 36);
		this.toolStripButton_0.Text = "单本载入";
		this.toolStripButton_0.Click += new System.EventHandler(toolStripButton_0_Click);
		this.toolStripButton_1.Font = new System.Drawing.Font("宋体", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton_1.Image = null;
		this.toolStripButton_1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton_1.Name = "toolStripButton_1";
		this.toolStripButton_1.Size = new System.Drawing.Size(99, 36);
		this.toolStripButton_1.Text = "批量载入";
		this.toolStripButton_1.Click += new System.EventHandler(toolStripButton_1_Click);
		this.toolStripButton_3.Font = new System.Drawing.Font("宋体", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton_3.Image = null;
		this.toolStripButton_3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton_3.Name = "toolStripButton_3";
		this.toolStripButton_3.Size = new System.Drawing.Size(99, 36);
		this.toolStripButton_3.Text = "中断采集";
		this.toolStripButton_3.Click += new System.EventHandler(toolStripButton_3_Click);
		this.statusStrip_0.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripStatusLabel_0, this.toolStripStatusLabel_1 });
		this.statusStrip_0.Location = new System.Drawing.Point(0, 539);
		this.statusStrip_0.Name = "statusStrip_0";
		this.statusStrip_0.Size = new System.Drawing.Size(964, 22);
		this.statusStrip_0.TabIndex = 1;
		this.statusStrip_0.Text = "statusStrip1";
		this.toolStripStatusLabel_0.Name = "toolStripStatusLabel_0";
		this.toolStripStatusLabel_0.Size = new System.Drawing.Size(56, 17);
		this.toolStripStatusLabel_0.Text = "准备就绪";
		this.toolStripStatusLabel_1.Name = "toolStripStatusLabel_1";
		this.toolStripStatusLabel_1.Size = new System.Drawing.Size(893, 17);
		this.toolStripStatusLabel_1.Spring = true;
		this.toolStripStatusLabel_1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.splitContainer_0.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer_0.Location = new System.Drawing.Point(0, 39);
		this.splitContainer_0.Name = "splitContainer_0";
		this.splitContainer_0.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer_0.Panel1.Controls.Add(this.panel_0);
		this.splitContainer_0.Panel1.Controls.Add(this.panel_1);
		this.splitContainer_0.Panel1.Controls.Add(this.splitContainer1);
		this.splitContainer_0.Panel1.Controls.Add(this.panel_2);
		this.splitContainer_0.Panel2.Controls.Add(this.splitContainer_1);
		this.splitContainer_0.Size = new System.Drawing.Size(964, 500);
		this.splitContainer_0.SplitterDistance = 258;
		this.splitContainer_0.TabIndex = 2;
		this.panel_0.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panel_0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel_0.Controls.Add(this.button_1);
		this.panel_0.Controls.Add(this.label_0);
		this.panel_0.Controls.Add(this.button_0);
		this.panel_0.Controls.Add(this.comboBox_0);
		this.panel_0.Controls.Add(this.radioButton_0);
		this.panel_0.Controls.Add(this.radioButton_1);
		this.panel_0.Controls.Add(this.radioButton_2);
		this.panel_0.Location = new System.Drawing.Point(373, 78);
		this.panel_0.Name = "panel_0";
		this.panel_0.Size = new System.Drawing.Size(264, 100);
		this.panel_0.TabIndex = 1;
		this.panel_0.Visible = false;
		this.button_1.Location = new System.Drawing.Point(87, 54);
		this.button_1.Name = "button_1";
		this.button_1.Size = new System.Drawing.Size(75, 23);
		this.button_1.TabIndex = 6;
		this.button_1.Text = "取消";
		this.button_1.UseVisualStyleBackColor = true;
		this.button_1.Click += new System.EventHandler(button_1_Click);
		this.label_0.AutoSize = true;
		this.label_0.Location = new System.Drawing.Point(4, 80);
		this.label_0.Name = "label_0";
		this.label_0.Size = new System.Drawing.Size(257, 12);
		this.label_0.TabIndex = 5;
		this.label_0.Text = "按小说名称和按本站ID，都需要目标站搜索支持";
		this.button_0.Location = new System.Drawing.Point(6, 54);
		this.button_0.Name = "button_0";
		this.button_0.Size = new System.Drawing.Size(75, 23);
		this.button_0.TabIndex = 4;
		this.button_0.Text = "载入";
		this.button_0.UseVisualStyleBackColor = true;
		this.button_0.Click += new System.EventHandler(button_0_Click);
		this.comboBox_0.FormattingEnabled = true;
		this.comboBox_0.Location = new System.Drawing.Point(6, 28);
		this.comboBox_0.Name = "comboBox_0";
		this.comboBox_0.Size = new System.Drawing.Size(249, 20);
		this.comboBox_0.TabIndex = 3;
		this.radioButton_0.AutoSize = true;
		this.radioButton_0.Location = new System.Drawing.Point(183, 6);
		this.radioButton_0.Name = "radioButton_0";
		this.radioButton_0.Size = new System.Drawing.Size(71, 16);
		this.radioButton_0.TabIndex = 2;
		this.radioButton_0.Text = "按本站ID";
		this.radioButton_0.UseVisualStyleBackColor = true;
		this.radioButton_1.AutoSize = true;
		this.radioButton_1.Location = new System.Drawing.Point(95, 6);
		this.radioButton_1.Name = "radioButton_1";
		this.radioButton_1.Size = new System.Drawing.Size(83, 16);
		this.radioButton_1.TabIndex = 1;
		this.radioButton_1.Text = "按小说名称";
		this.radioButton_1.UseVisualStyleBackColor = true;
		this.radioButton_2.AutoSize = true;
		this.radioButton_2.Checked = true;
		this.radioButton_2.Location = new System.Drawing.Point(6, 6);
		this.radioButton_2.Name = "radioButton_2";
		this.radioButton_2.Size = new System.Drawing.Size(83, 16);
		this.radioButton_2.TabIndex = 0;
		this.radioButton_2.TabStop = true;
		this.radioButton_2.Text = "按目标站ID";
		this.radioButton_2.UseVisualStyleBackColor = true;
		this.panel_1.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.panel_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel_1.Controls.Add(this.label_1);
		this.panel_1.Controls.Add(this.button_2);
		this.panel_1.Controls.Add(this.button_3);
		this.panel_1.Controls.Add(this.comboBox_1);
		this.panel_1.Controls.Add(this.radioButton_3);
		this.panel_1.Controls.Add(this.radioButton_4);
		this.panel_1.Location = new System.Drawing.Point(298, 83);
		this.panel_1.Name = "panel_1";
		this.panel_1.Size = new System.Drawing.Size(400, 86);
		this.panel_1.TabIndex = 2;
		this.panel_1.Visible = false;
		this.label_1.AutoSize = true;
		this.label_1.Location = new System.Drawing.Point(171, 59);
		this.label_1.Name = "label_1";
		this.label_1.Size = new System.Drawing.Size(221, 12);
		this.label_1.TabIndex = 5;
		this.label_1.Text = "不熟悉SQL的朋友，请勿更改默认SQL语句";
		this.button_2.Location = new System.Drawing.Point(87, 54);
		this.button_2.Name = "button_2";
		this.button_2.Size = new System.Drawing.Size(75, 23);
		this.button_2.TabIndex = 4;
		this.button_2.Text = "取消";
		this.button_2.UseVisualStyleBackColor = true;
		this.button_2.Click += new System.EventHandler(button_2_Click);
		this.button_3.Location = new System.Drawing.Point(6, 54);
		this.button_3.Name = "button_3";
		this.button_3.Size = new System.Drawing.Size(75, 23);
		this.button_3.TabIndex = 3;
		this.button_3.Text = "载入";
		this.button_3.UseVisualStyleBackColor = true;
		this.button_3.Click += new System.EventHandler(button_3_Click);
		this.comboBox_1.FormattingEnabled = true;
		this.comboBox_1.Location = new System.Drawing.Point(6, 28);
		this.comboBox_1.Name = "comboBox_1";
		this.comboBox_1.Size = new System.Drawing.Size(386, 20);
		this.comboBox_1.TabIndex = 2;
		this.radioButton_3.AutoSize = true;
		this.radioButton_3.Location = new System.Drawing.Point(155, 6);
		this.radioButton_3.Name = "radioButton_3";
		this.radioButton_3.Size = new System.Drawing.Size(137, 16);
		this.radioButton_3.TabIndex = 1;
		this.radioButton_3.Text = "通过本站SQL结果载入";
		this.radioButton_3.UseVisualStyleBackColor = true;
		this.radioButton_3.CheckedChanged += new System.EventHandler(radioButton_3_CheckedChanged);
		this.radioButton_4.AutoSize = true;
		this.radioButton_4.Checked = true;
		this.radioButton_4.Location = new System.Drawing.Point(6, 6);
		this.radioButton_4.Name = "radioButton_4";
		this.radioButton_4.Size = new System.Drawing.Size(143, 16);
		this.radioButton_4.TabIndex = 0;
		this.radioButton_4.TabStop = true;
		this.radioButton_4.Text = "从目标站列表进行载入";
		this.radioButton_4.UseVisualStyleBackColor = true;
		this.radioButton_4.CheckedChanged += new System.EventHandler(radioButton_4_CheckedChanged);
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 26);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Panel1.Controls.Add(this.listView_2);
		this.splitContainer1.Panel2.Controls.Add(this.listView1);
		this.splitContainer1.Size = new System.Drawing.Size(964, 232);
		this.splitContainer1.SplitterDistance = 722;
		this.splitContainer1.TabIndex = 5;
		this.listView_2.CheckBoxes = true;
		this.listView_2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[5] { this.手动信息目标站ID, this.手动信息小说名称, this.手动信息本站ID, this.手动信息本站最新章节情况, this.手动信息更新时间 });
		this.listView_2.ContextMenuStrip = this.NovelMenuStrip;
		this.listView_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listView_2.FullRowSelect = true;
		this.listView_2.GridLines = true;
		this.listView_2.HideSelection = false;
		this.listView_2.Location = new System.Drawing.Point(0, 0);
		this.listView_2.Name = "listView_2";
		this.listView_2.Size = new System.Drawing.Size(722, 232);
		this.listView_2.TabIndex = 4;
		this.listView_2.UseCompatibleStateImageBehavior = false;
		this.listView_2.View = System.Windows.Forms.View.Details;
		this.listView_2.SelectedIndexChanged += new System.EventHandler(listView_1_SelectedIndexChanged);
		this.listView_2.DoubleClick += new System.EventHandler(listView_2_DoubleClick);
		this.手动信息目标站ID.Text = "目标站ID";
		this.手动信息目标站ID.Width = 61;
		this.手动信息小说名称.Text = "小说名称";
		this.手动信息小说名称.Width = 144;
		this.手动信息本站ID.Text = "本站ID";
		this.手动信息本站ID.Width = 49;
		this.手动信息本站最新章节情况.Text = "本站最新章节情况(空表示新书)";
		this.手动信息本站最新章节情况.Width = 227;
		this.手动信息更新时间.Text = "更新时间";
		this.手动信息更新时间.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.手动信息更新时间.Width = 200;
		this.NovelMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.toolStripMenuItem_20, this.toolStripSeparator_0, this.toolStripMenuItem_19, this.toolStripMenuItem_1, this.toolStripMenuItem_2, this.toolStripSeparator_6, this.toolStripMenuItem_3, this.toolStripMenuItem_21, this.toolStripMenuItem_4, this.toolStripMenuItem_18,
			this.toolStripSeparator_1, this.toolStripMenuItem_32, this.DelSelectLog, this.toolStripMenuItem_5
		});
		this.NovelMenuStrip.Name = "NovelMenuStrip";
		this.NovelMenuStrip.Size = new System.Drawing.Size(157, 264);
		this.NovelMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(NovelMenuStrip_Opening);
		this.toolStripMenuItem_20.Name = "toolStripMenuItem_20";
		this.toolStripMenuItem_20.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_20.Text = "列出章节";
		this.toolStripMenuItem_20.Click += new System.EventHandler(toolStripMenuItem_20_Click);
		this.toolStripSeparator_0.Name = "toolStripSeparator_0";
		this.toolStripSeparator_0.Size = new System.Drawing.Size(153, 6);
		this.toolStripMenuItem_19.Name = "toolStripMenuItem_19";
		this.toolStripMenuItem_19.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_19.Text = "加载小说信息";
		this.toolStripMenuItem_19.Click += new System.EventHandler(toolStripMenuItem_19_Click);
		this.toolStripMenuItem_1.Name = "toolStripMenuItem_1";
		this.toolStripMenuItem_1.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_1.Text = "添加小说";
		this.toolStripMenuItem_1.Click += new System.EventHandler(toolStripMenuItem_1_Click);
		this.toolStripMenuItem_2.Name = "toolStripMenuItem_2";
		this.toolStripMenuItem_2.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_2.Text = "删除小说(真实)";
		this.toolStripMenuItem_2.Click += new System.EventHandler(toolStripMenuItem_2_Click);
		this.toolStripSeparator_6.Name = "toolStripSeparator_6";
		this.toolStripSeparator_6.Size = new System.Drawing.Size(153, 6);
		this.toolStripMenuItem_3.Name = "toolStripMenuItem_3";
		this.toolStripMenuItem_3.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_3.Text = "全选小说";
		this.toolStripMenuItem_3.Click += new System.EventHandler(toolStripMenuItem_3_Click);
		this.toolStripMenuItem_21.Name = "toolStripMenuItem_21";
		this.toolStripMenuItem_21.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_21.Text = "全不选小说";
		this.toolStripMenuItem_21.Click += new System.EventHandler(toolStripMenuItem_21_Click);
		this.toolStripMenuItem_4.Name = "toolStripMenuItem_4";
		this.toolStripMenuItem_4.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_4.Text = "反选小说";
		this.toolStripMenuItem_4.Click += new System.EventHandler(toolStripMenuItem_4_Click);
		this.toolStripMenuItem_18.Name = "toolStripMenuItem_18";
		this.toolStripMenuItem_18.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_18.Text = "选中后续小说";
		this.toolStripMenuItem_18.Click += new System.EventHandler(toolStripMenuItem_18_Click);
		this.toolStripSeparator_1.Name = "toolStripSeparator_1";
		this.toolStripSeparator_1.Size = new System.Drawing.Size(153, 6);
		this.toolStripMenuItem_32.Name = "toolStripMenuItem_32";
		this.toolStripMenuItem_32.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_32.Text = "更新连载状态";
		this.toolStripMenuItem_32.Click += new System.EventHandler(toolStripMenuItem_32_Click);
		this.DelSelectLog.Name = "DelSelectLog";
		this.DelSelectLog.Size = new System.Drawing.Size(156, 22);
		this.DelSelectLog.Text = "删除选中日志";
		this.DelSelectLog.Click += new System.EventHandler(DelSelectLog_Click);
		this.toolStripMenuItem_5.Name = "toolStripMenuItem_5";
		this.toolStripMenuItem_5.Size = new System.Drawing.Size(156, 22);
		this.toolStripMenuItem_5.Text = "清空列表";
		this.toolStripMenuItem_5.Click += new System.EventHandler(toolStripMenuItem_5_Click);
		this.listView1.CheckBoxes = true;
		this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[2] { this.columnHeader_12, this.columnHeader_13 });
		this.listView1.ContextMenuStrip = this.LocalMenuStrip;
		this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listView1.FullRowSelect = true;
		this.listView1.GridLines = true;
		this.listView1.HideSelection = false;
		this.listView1.Location = new System.Drawing.Point(0, 0);
		this.listView1.Name = "listView1";
		this.listView1.Size = new System.Drawing.Size(238, 232);
		this.listView1.TabIndex = 3;
		this.listView1.UseCompatibleStateImageBehavior = false;
		this.listView1.View = System.Windows.Forms.View.Details;
		this.columnHeader_12.Text = "索引";
		this.columnHeader_12.Width = 62;
		this.columnHeader_13.Text = "分卷名";
		this.columnHeader_13.Width = 142;
		this.LocalMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripMenuItem_31, this.toolStripSeparator1, this.toolStripMenuItem_29, this.toolStripMenuItem_28, this.toolStripMenuItem_0, this.toolStripMenuItem_35 });
		this.LocalMenuStrip.Name = "LocalMenuStrip";
		this.LocalMenuStrip.Size = new System.Drawing.Size(149, 120);
		this.toolStripMenuItem_31.Name = "toolStripMenuItem_31";
		this.toolStripMenuItem_31.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_31.Text = "删除分卷";
		this.toolStripMenuItem_31.Click += new System.EventHandler(toolStripMenuItem_31_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
		this.toolStripMenuItem_29.Name = "toolStripMenuItem_29";
		this.toolStripMenuItem_29.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_29.Text = "全选章节";
		this.toolStripMenuItem_29.Click += new System.EventHandler(toolStripMenuItem_29_Click);
		this.toolStripMenuItem_28.Name = "toolStripMenuItem_28";
		this.toolStripMenuItem_28.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_28.Text = "全不选章节";
		this.toolStripMenuItem_28.Click += new System.EventHandler(toolStripMenuItem_28_Click);
		this.toolStripMenuItem_0.Name = "toolStripMenuItem_0";
		this.toolStripMenuItem_0.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_0.Text = "反选章节";
		this.toolStripMenuItem_0.Click += new System.EventHandler(toolStripMenuItem_0_Click);
		this.toolStripMenuItem_35.Name = "toolStripMenuItem_35";
		this.toolStripMenuItem_35.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem_35.Text = "选中后续章节";
		this.toolStripMenuItem_35.Click += new System.EventHandler(toolStripMenuItem_35_Click);
		this.panel_2.Controls.Add(this.Db3InsertButton);
		this.panel_2.Controls.Add(this.ErrIdcomboBox);
		this.panel_2.Controls.Add(this.comboBox_4);
		this.panel_2.Controls.Add(this.comboBox_3);
		this.panel_2.Controls.Add(this.comboBox_2);
		this.panel_2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel_2.Location = new System.Drawing.Point(0, 0);
		this.panel_2.Name = "panel_2";
		this.panel_2.Size = new System.Drawing.Size(964, 26);
		this.panel_2.TabIndex = 3;
		this.Db3InsertButton.Location = new System.Drawing.Point(774, 1);
		this.Db3InsertButton.Name = "Db3InsertButton";
		this.Db3InsertButton.Size = new System.Drawing.Size(61, 23);
		this.Db3InsertButton.TabIndex = 14;
		this.Db3InsertButton.Text = "导入";
		this.Db3InsertButton.UseVisualStyleBackColor = true;
		this.Db3InsertButton.Click += new System.EventHandler(Db3InsertButton_Click);
		this.ErrIdcomboBox.FormattingEnabled = true;
		this.ErrIdcomboBox.Items.AddRange(new object[31]
		{
			"EXID", "0 未知错误", "", "101 子窗口冲突", "102 检查子窗口冲突失败", "", "120 对比最新章节失败", "121 空章节", "122 检查到重复章节", "124 只采集文字章节时发现图片章节",
			"125 设置不添加新书", "", "130 限制章节字数小于多少字的章节不采集", "131 章节数量小于限制", "132 对比最新章节成功！但需要采集到章节数超限。", "134 限制小说_黑名单", "135 限制小说_不在白名单", "136 过滤分卷名", "137 章节名过滤（章节名过滤作者名、自定义过滤）", "",
			"200 小说信息页发生问题", "210 小说目录页发生问题", "214 章节组为空", "220 小说内容页发生问题", "", "410 操作本站小说列表发生问题", "420 操作本站小说信息发生问题", "430 操作本站章节列表发生问题", "440 操作本站章节信息发生问题", "441 InsertChapter发生问题",
			"442 UpdateChapter发生问题"
		});
		this.ErrIdcomboBox.Location = new System.Drawing.Point(588, 3);
		this.ErrIdcomboBox.Name = "ErrIdcomboBox";
		this.ErrIdcomboBox.Size = new System.Drawing.Size(180, 20);
		this.ErrIdcomboBox.TabIndex = 12;
		this.ErrIdcomboBox.Text = "EXID";
		this.comboBox_4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_4.FormattingEnabled = true;
		this.comboBox_4.Location = new System.Drawing.Point(381, 3);
		this.comboBox_4.Name = "comboBox_4";
		this.comboBox_4.Size = new System.Drawing.Size(191, 20);
		this.comboBox_4.TabIndex = 11;
		this.comboBox_3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_3.FormattingEnabled = true;
		this.comboBox_3.Location = new System.Drawing.Point(206, 3);
		this.comboBox_3.Name = "comboBox_3";
		this.comboBox_3.Size = new System.Drawing.Size(169, 20);
		this.comboBox_3.TabIndex = 10;
		this.comboBox_3.SelectedIndexChanged += new System.EventHandler(comboBox_3_SelectedIndexChanged);
		this.comboBox_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox_2.FormattingEnabled = true;
		this.comboBox_2.Location = new System.Drawing.Point(12, 3);
		this.comboBox_2.Name = "comboBox_2";
		this.comboBox_2.Size = new System.Drawing.Size(188, 20);
		this.comboBox_2.TabIndex = 1;
		this.comboBox_2.SelectedIndexChanged += new System.EventHandler(comboBox_2_SelectedIndexChanged);
		this.splitContainer_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer_1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer_1.Name = "splitContainer_1";
		this.splitContainer_1.Panel1.Controls.Add(this.target_list_view);
		this.splitContainer_1.Panel2.Controls.Add(this.listView_1);
		this.splitContainer_1.Size = new System.Drawing.Size(964, 238);
		this.splitContainer_1.SplitterDistance = 462;
		this.splitContainer_1.TabIndex = 0;
		this.target_list_view.CheckBoxes = true;
		this.target_list_view.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.左手动索引, this.左手动分卷名, this.左手动章节名, this.左手动内容 });
		this.target_list_view.ContextMenuStrip = this.TargetMenuStrip;
		this.target_list_view.Dock = System.Windows.Forms.DockStyle.Fill;
		this.target_list_view.FullRowSelect = true;
		this.target_list_view.GridLines = true;
		this.target_list_view.HideSelection = false;
		this.target_list_view.Location = new System.Drawing.Point(0, 0);
		this.target_list_view.Name = "target_list_view";
		this.target_list_view.Size = new System.Drawing.Size(462, 238);
		this.target_list_view.TabIndex = 1;
		this.target_list_view.UseCompatibleStateImageBehavior = false;
		this.target_list_view.View = System.Windows.Forms.View.Details;
		this.target_list_view.SelectedIndexChanged += new System.EventHandler(listView_1_SelectedIndexChanged);
		this.target_list_view.DoubleClick += new System.EventHandler(target_listview_DoubleClick);
		this.左手动索引.Text = "索引";
		this.左手动索引.Width = 48;
		this.左手动分卷名.Text = "分卷名";
		this.左手动分卷名.Width = 66;
		this.左手动章节名.Text = "章节名(目标站)";
		this.左手动章节名.Width = 240;
		this.左手动内容.Text = "内容";
		this.左手动内容.Width = 59;
		this.TargetMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.toolStripMenuItem_6, this.toolStripMenuItem_34, this.toolStripMenuItem_7, this.toolStripSeparator_2, this.toolStripMenuItem_8, this.toolStripMenuItem_22, this.toolStripMenuItem_9, this.toolStripMenuItem_10, this.toolStripMenuItem_37, this.toolStripSeparator_3,
			this.toolStripMenuItem_11, this.toolStripMenuItem_23
		});
		this.TargetMenuStrip.Name = "TargetMenuStrip";
		this.TargetMenuStrip.Size = new System.Drawing.Size(161, 236);
		this.TargetMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(TargetMenuStrip_Opening);
		this.toolStripMenuItem_6.Name = "toolStripMenuItem_6";
		this.toolStripMenuItem_6.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_6.Text = "采集章节";
		this.toolStripMenuItem_6.Click += new System.EventHandler(toolStripMenuItem_6_Click);
		this.toolStripMenuItem_34.Name = "toolStripMenuItem_34";
		this.toolStripMenuItem_34.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_34.Text = "选中后续并采集";
		this.toolStripMenuItem_34.Click += new System.EventHandler(toolStripMenuItem_34_Click);
		this.toolStripMenuItem_7.Name = "toolStripMenuItem_7";
		this.toolStripMenuItem_7.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_7.Text = "预采集章节内容";
		this.toolStripMenuItem_7.Visible = false;
		this.toolStripSeparator_2.Name = "toolStripSeparator_2";
		this.toolStripSeparator_2.Size = new System.Drawing.Size(157, 6);
		this.toolStripMenuItem_8.Name = "toolStripMenuItem_8";
		this.toolStripMenuItem_8.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_8.Text = "全选章节";
		this.toolStripMenuItem_8.Click += new System.EventHandler(toolStripMenuItem_8_Click);
		this.toolStripMenuItem_22.Name = "toolStripMenuItem_22";
		this.toolStripMenuItem_22.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_22.Text = "全不选章节";
		this.toolStripMenuItem_22.Click += new System.EventHandler(toolStripMenuItem_22_Click);
		this.toolStripMenuItem_9.Name = "toolStripMenuItem_9";
		this.toolStripMenuItem_9.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_9.Text = "反选章节";
		this.toolStripMenuItem_9.Click += new System.EventHandler(toolStripMenuItem_9_Click);
		this.toolStripMenuItem_10.Name = "toolStripMenuItem_10";
		this.toolStripMenuItem_10.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_10.Text = "选中后续章节";
		this.toolStripMenuItem_10.Click += new System.EventHandler(toolStripMenuItem_10_Click);
		this.toolStripMenuItem_37.Name = "toolStripMenuItem_37";
		this.toolStripMenuItem_37.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_37.Text = "选中中间章节";
		this.toolStripMenuItem_37.Click += new System.EventHandler(toolStripMenuItem_37_Click);
		this.toolStripSeparator_3.Name = "toolStripSeparator_3";
		this.toolStripSeparator_3.Size = new System.Drawing.Size(157, 6);
		this.toolStripMenuItem_11.Name = "toolStripMenuItem_11";
		this.toolStripMenuItem_11.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_11.Text = "清空列表";
		this.toolStripMenuItem_11.Click += new System.EventHandler(toolStripMenuItem_11_Click);
		this.toolStripMenuItem_23.Name = "toolStripMenuItem_23";
		this.toolStripMenuItem_23.Size = new System.Drawing.Size(160, 22);
		this.toolStripMenuItem_23.Text = "复制章节名";
		this.toolStripMenuItem_23.Click += new System.EventHandler(toolStripMenuItem_23_Click);
		this.listView_1.CheckBoxes = true;
		this.listView_1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[5] { this.右手动索引, this.右手动分卷名, this.右手动章节名, this.右手动更新时间, this.右手动内容 });
		this.listView_1.ContextMenuStrip = this.LocalMenuStrip_1;
		this.listView_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.listView_1.FullRowSelect = true;
		this.listView_1.GridLines = true;
		this.listView_1.HideSelection = false;
		this.listView_1.Location = new System.Drawing.Point(0, 0);
		this.listView_1.Name = "listView_1";
		this.listView_1.Size = new System.Drawing.Size(498, 238);
		this.listView_1.TabIndex = 2;
		this.listView_1.UseCompatibleStateImageBehavior = false;
		this.listView_1.View = System.Windows.Forms.View.Details;
		this.listView_1.SelectedIndexChanged += new System.EventHandler(listView_1_SelectedIndexChanged);
		this.右手动索引.Text = "索引";
		this.右手动索引.Width = 53;
		this.右手动分卷名.Text = "分卷名";
		this.右手动分卷名.Width = 66;
		this.右手动章节名.Text = "章节名(自己站)";
		this.右手动章节名.Width = 140;
		this.右手动更新时间.Text = "更新时间";
		this.右手动更新时间.Width = 180;
		this.右手动内容.Text = "内容";
		this.右手动内容.Width = 42;
		this.LocalMenuStrip_1.Items.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.toolStripMenuItem_12, this.toolStripSeparator_4, this.toolStripMenuItem_25, this.toolStripMenuItem_33, this.toolStripMenuItem_30, this.toolStripMenuItem_17, this.toolStripMenuItem_27, this.toolStripMenuItem_26, this.toolStripMenuItem_13, this.toolStripSeparator_5,
			this.toolStripMenuItem_14, this.toolStripMenuItem_24, this.toolStripMenuItem_15, this.toolStripMenuItem_16, this.toolStripMenuItem_36
		});
		this.LocalMenuStrip_1.Name = "LocalMenuStrip";
		this.LocalMenuStrip_1.Size = new System.Drawing.Size(169, 302);
		this.LocalMenuStrip_1.Opening += new System.ComponentModel.CancelEventHandler(LocalMenuStrip_1_Opening);
		this.toolStripMenuItem_12.Name = "toolStripMenuItem_12";
		this.toolStripMenuItem_12.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_12.Text = "替换采集";
		this.toolStripMenuItem_12.Click += new System.EventHandler(toolStripMenuItem_12_Click);
		this.toolStripSeparator_4.Name = "toolStripSeparator_4";
		this.toolStripSeparator_4.Size = new System.Drawing.Size(165, 6);
		this.toolStripMenuItem_25.Name = "toolStripMenuItem_25";
		this.toolStripMenuItem_25.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_25.Text = "在选中前插入";
		this.toolStripMenuItem_25.Click += new System.EventHandler(toolStripMenuItem_25_Click);
		this.toolStripMenuItem_33.Name = "toolStripMenuItem_33";
		this.toolStripMenuItem_33.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_33.Text = "修改选中章节";
		this.toolStripMenuItem_33.Click += new System.EventHandler(toolStripMenuItem_33_Click);
		this.toolStripMenuItem_30.Name = "toolStripMenuItem_30";
		this.toolStripMenuItem_30.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_30.Text = "删除本章(数据库)";
		this.toolStripMenuItem_30.Click += new System.EventHandler(toolStripMenuItem_30_Click);
		this.toolStripMenuItem_17.Name = "toolStripMenuItem_17";
		this.toolStripMenuItem_17.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_17.Text = "删除本章(真实)";
		this.toolStripMenuItem_17.Click += new System.EventHandler(toolStripMenuItem_17_Click);
		this.toolStripMenuItem_27.Name = "toolStripMenuItem_27";
		this.toolStripMenuItem_27.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_27.Text = "检查重复章节";
		this.toolStripMenuItem_27.Click += new System.EventHandler(toolStripMenuItem_27_Click);
		this.toolStripMenuItem_26.Name = "toolStripMenuItem_26";
		this.toolStripMenuItem_26.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_26.Text = "检测TXT文本";
		this.toolStripMenuItem_26.Click += new System.EventHandler(toolStripMenuItem_26_Click);
		this.toolStripMenuItem_13.Name = "toolStripMenuItem_13";
		this.toolStripMenuItem_13.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_13.Text = "检查章节内容";
		this.toolStripMenuItem_13.Click += new System.EventHandler(toolStripMenuItem_13_Click);
		this.toolStripSeparator_5.Name = "toolStripSeparator_5";
		this.toolStripSeparator_5.Size = new System.Drawing.Size(165, 6);
		this.toolStripMenuItem_14.Name = "toolStripMenuItem_14";
		this.toolStripMenuItem_14.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_14.Text = "全选章节";
		this.toolStripMenuItem_14.Click += new System.EventHandler(toolStripMenuItem_14_Click);
		this.toolStripMenuItem_24.Name = "toolStripMenuItem_24";
		this.toolStripMenuItem_24.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_24.Text = "全不选章节";
		this.toolStripMenuItem_24.Click += new System.EventHandler(toolStripMenuItem_24_Click);
		this.toolStripMenuItem_15.Name = "toolStripMenuItem_15";
		this.toolStripMenuItem_15.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_15.Text = "反选章节";
		this.toolStripMenuItem_15.Click += new System.EventHandler(toolStripMenuItem_15_Click);
		this.toolStripMenuItem_16.Name = "toolStripMenuItem_16";
		this.toolStripMenuItem_16.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_16.Text = "选中后续章节";
		this.toolStripMenuItem_16.Click += new System.EventHandler(toolStripMenuItem_16_Click);
		this.toolStripMenuItem_36.Name = "toolStripMenuItem_36";
		this.toolStripMenuItem_36.Size = new System.Drawing.Size(168, 22);
		this.toolStripMenuItem_36.Text = "选中中间章节";
		this.toolStripMenuItem_36.Click += new System.EventHandler(toolStripMenuItem_36_Click);
		this.backgroundWorker_3.WorkerReportsProgress = true;
		this.backgroundWorker_3.WorkerSupportsCancellation = true;
		this.backgroundWorker_3.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_3_DoWork);
		this.backgroundWorker_3.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_4.WorkerReportsProgress = true;
		this.backgroundWorker_4.WorkerSupportsCancellation = true;
		this.backgroundWorker_5.WorkerReportsProgress = true;
		this.backgroundWorker_5.WorkerSupportsCancellation = true;
		this.backgroundWorker_5.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_5_DoWork);
		this.backgroundWorker_5.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_5.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_6.WorkerReportsProgress = true;
		this.backgroundWorker_6.WorkerSupportsCancellation = true;
		this.backgroundWorker_6.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_6_DoWork);
		this.backgroundWorker_6.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_6.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_7.WorkerReportsProgress = true;
		this.backgroundWorker_7.WorkerSupportsCancellation = true;
		this.backgroundWorker_7.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_7_DoWork);
		this.backgroundWorker_7.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_7.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_8.WorkerReportsProgress = true;
		this.backgroundWorker_8.WorkerSupportsCancellation = true;
		this.backgroundWorker_8.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_8_DoWork);
		this.backgroundWorker_8.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_8.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.toolTip_0.AutomaticDelay = 100;
		this.toolTip_0.AutoPopDelay = 50000;
		this.toolTip_0.InitialDelay = 100;
		this.toolTip_0.ReshowDelay = 20;
		this.toolTip_0.ShowAlways = true;
		this.toolTip_0.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
		this.toolTip_0.ToolTipTitle = "提示";
		this.backgroundWorker_0.WorkerReportsProgress = true;
		this.backgroundWorker_0.WorkerSupportsCancellation = true;
		this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_0_DoWork);
		this.backgroundWorker_0.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_0.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_1.WorkerReportsProgress = true;
		this.backgroundWorker_1.WorkerSupportsCancellation = true;
		this.backgroundWorker_1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_1_DoWork);
		this.backgroundWorker_1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_2.WorkerReportsProgress = true;
		this.backgroundWorker_2.WorkerSupportsCancellation = true;
		this.backgroundWorker_2.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_2_DoWork);
		this.backgroundWorker_2.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_9.WorkerReportsProgress = true;
		this.backgroundWorker_9.WorkerSupportsCancellation = true;
		this.backgroundWorker_9.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_9_DoWork);
		this.backgroundWorker_9.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_9.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_10.WorkerReportsProgress = true;
		this.backgroundWorker_10.WorkerSupportsCancellation = true;
		this.backgroundWorker_10.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_10_DoWork);
		this.backgroundWorker_10.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_10.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_11.WorkerReportsProgress = true;
		this.backgroundWorker_11.WorkerSupportsCancellation = true;
		this.backgroundWorker_11.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_11_DoWork);
		this.backgroundWorker_11.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_11.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.backgroundWorker_12.WorkerReportsProgress = true;
		this.backgroundWorker_12.WorkerSupportsCancellation = true;
		this.backgroundWorker_12.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_12_DoWork);
		this.backgroundWorker_12.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(backgroundWorker_12_ProgressChanged);
		this.backgroundWorker_12.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_12_RunWorkerCompleted);
		this.ReviseChapter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.ReviseChapter.Controls.Add(this.button2);
		this.ReviseChapter.Controls.Add(this.sortBox);
		this.ReviseChapter.Controls.Add(this.posterBox);
		this.ReviseChapter.Controls.Add(this.articlenameBox);
		this.ReviseChapter.Controls.Add(this.chapterNameBox);
		this.ReviseChapter.Controls.Add(this.button1);
		this.ReviseChapter.Controls.Add(this.label4);
		this.ReviseChapter.Controls.Add(this.label3);
		this.ReviseChapter.Controls.Add(this.label2);
		this.ReviseChapter.Controls.Add(this.label1);
		this.ReviseChapter.Controls.Add(this.groupBox1);
		this.ReviseChapter.Location = new System.Drawing.Point(114, 239);
		this.ReviseChapter.Name = "ReviseChapter";
		this.ReviseChapter.Size = new System.Drawing.Size(630, 263);
		this.ReviseChapter.TabIndex = 5;
		this.ReviseChapter.Visible = false;
		this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button2.Location = new System.Drawing.Point(552, 237);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(75, 23);
		this.button2.TabIndex = 13;
		this.button2.Text = "取消";
		this.button2.UseVisualStyleBackColor = true;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.sortBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.sortBox.FormattingEnabled = true;
		this.sortBox.Location = new System.Drawing.Point(553, 9);
		this.sortBox.Name = "sortBox";
		this.sortBox.Size = new System.Drawing.Size(101, 20);
		this.sortBox.TabIndex = 12;
		this.posterBox.Location = new System.Drawing.Point(336, 9);
		this.posterBox.Name = "posterBox";
		this.posterBox.Size = new System.Drawing.Size(172, 21);
		this.posterBox.TabIndex = 10;
		this.articlenameBox.Location = new System.Drawing.Point(62, 9);
		this.articlenameBox.Name = "articlenameBox";
		this.articlenameBox.ReadOnly = true;
		this.articlenameBox.Size = new System.Drawing.Size(210, 21);
		this.articlenameBox.TabIndex = 9;
		this.chapterNameBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.chapterNameBox.Location = new System.Drawing.Point(62, 38);
		this.chapterNameBox.Name = "chapterNameBox";
		this.chapterNameBox.Size = new System.Drawing.Size(555, 21);
		this.chapterNameBox.TabIndex = 8;
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.button1.Location = new System.Drawing.Point(471, 237);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 6;
		this.button1.Text = "确认";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(514, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(41, 12);
		this.label4.TabIndex = 5;
		this.label4.Text = "分类：";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(296, 13);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(41, 12);
		this.label3.TabIndex = 4;
		this.label3.Text = "作者：";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(10, 13);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(53, 12);
		this.label2.TabIndex = 3;
		this.label2.Text = "小说名：";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(10, 41);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(53, 12);
		this.label1.TabIndex = 1;
		this.label1.Text = "章节名：";
		this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.groupBox1.Controls.Add(this.chapterTXTBox);
		this.groupBox1.Location = new System.Drawing.Point(3, 66);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(624, 165);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "章节内容";
		this.chapterTXTBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.chapterTXTBox.Location = new System.Drawing.Point(9, 20);
		this.chapterTXTBox.Multiline = true;
		this.chapterTXTBox.Name = "chapterTXTBox";
		this.chapterTXTBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.chapterTXTBox.Size = new System.Drawing.Size(605, 139);
		this.chapterTXTBox.TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("宋体", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label5.ForeColor = System.Drawing.Color.Orange;
		this.label5.Location = new System.Drawing.Point(457, 10);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(25, 16);
		this.label5.TabIndex = 6;
		this.label5.Text = "▆";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("宋体", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label6.ForeColor = System.Drawing.Color.SeaGreen;
		this.label6.Location = new System.Drawing.Point(547, 10);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(25, 16);
		this.label6.TabIndex = 6;
		this.label6.Text = "▆";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("宋体", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(739, 10);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(25, 16);
		this.label7.TabIndex = 6;
		this.label7.Text = "▆";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("宋体", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
		this.label8.ForeColor = System.Drawing.Color.DeepPink;
		this.label8.Location = new System.Drawing.Point(643, 10);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(25, 16);
		this.label8.TabIndex = 6;
		this.label8.Text = "▆";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(488, 14);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(53, 12);
		this.label9.TabIndex = 7;
		this.label9.Text = "当天更新";
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(578, 14);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(59, 12);
		this.label10.TabIndex = 7;
		this.label10.Text = "2~3天更新";
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(674, 14);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(59, 12);
		this.label11.TabIndex = 7;
		this.label11.Text = "3~7天更新";
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(770, 14);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(59, 12);
		this.label12.TabIndex = 7;
		this.label12.Text = "7天前更新";
		this.backgroundWorker1.WorkerReportsProgress = true;
		this.backgroundWorker1.WorkerSupportsCancellation = true;
		this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
		this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
		base.ClientSize = new System.Drawing.Size(964, 561);
		base.Controls.Add(this.label12);
		base.Controls.Add(this.label11);
		base.Controls.Add(this.label10);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.ReviseChapter);
		base.Controls.Add(this.splitContainer_0);
		base.Controls.Add(this.statusStrip_0);
		base.Controls.Add(this.toolStrip_0);
		this.Font = new System.Drawing.Font("宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
		base.Icon = AppIconProvider.Icon;
		base.Name = "CollectManual";
		this.Text = "手动控制";
		base.Load += new System.EventHandler(CollectManual_Load);
		this.toolStrip_0.ResumeLayout(false);
		this.toolStrip_0.PerformLayout();
		this.statusStrip_0.ResumeLayout(false);
		this.statusStrip_0.PerformLayout();
		this.splitContainer_0.Panel1.ResumeLayout(false);
		this.splitContainer_0.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer_0).EndInit();
		this.splitContainer_0.ResumeLayout(false);
		this.panel_0.ResumeLayout(false);
		this.panel_0.PerformLayout();
		this.panel_1.ResumeLayout(false);
		this.panel_1.PerformLayout();
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		this.NovelMenuStrip.ResumeLayout(false);
		this.LocalMenuStrip.ResumeLayout(false);
		this.panel_2.ResumeLayout(false);
		this.splitContainer_1.Panel1.ResumeLayout(false);
		this.splitContainer_1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer_1).EndInit();
		this.splitContainer_1.ResumeLayout(false);
		this.TargetMenuStrip.ResumeLayout(false);
		this.LocalMenuStrip_1.ResumeLayout(false);
		this.ReviseChapter.ResumeLayout(false);
		this.ReviseChapter.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void listView_1_SelectedIndexChanged(object sender, EventArgs e)
	{
		ListView listView = (ListView)sender;
		if (listView.SelectedItems.Count > 0)
		{
			int_2 = listView.SelectedItems[0].Index;
			switch (listView.Name)
			{
			case "listView_0":
			case "listView_1":
				toolStripStatusLabel_1.Text = listView.SelectedItems[0].SubItems[1].Text + " " + listView.SelectedItems[0].SubItems[2].Text;
				break;
			default:
				toolStripStatusLabel_1.Text = listView.SelectedItems[0].Text + " " + listView.SelectedItems[0].SubItems[1].Text;
				break;
			}
			listView.SelectedItems[0].Selected = false;
		}
	}

	private void listView_2_DoubleClick(object sender, EventArgs e)
	{
		if (!bool_0 && !backgroundWorker_7.IsBusy && int_2 != -1)
		{
			bool_0 = true;
			panel_2.Enabled = false;
			toolStripStatusLabel_0.Text = "正在列出章节.请勿进行其他操作..";
			listView_2.Items[int_2].Checked = !listView_2.Items[int_2].Checked;
			NovelInfo novelInfo = (NovelInfo)listView_2.Items[int_2].Tag;
			target_list_view.Tag = novelInfo;
			backgroundWorker_7.RunWorkerAsync(novelInfo);
		}
	}

	private void LocalMenuStrip_1_Opening(object sender, CancelEventArgs e)
	{
	}

	private void method_0()
	{
		comboBox_3.BeginUpdate();
		string[] array = IO.LoadRules();
		comboBox_3.Items.Clear();
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				comboBox_3.Items.Add(array[i]);
				if (array[i] == taskConfigInfo_0.RuleFile)
				{
					comboBox_3.Text = taskConfigInfo_0.RuleFile;
					ruleConfigInfo_0 = (RuleConfigInfo)ConfigFileManager.LoadConfig(taskConfigInfo_0.RuleFile, ruleConfigInfo_0);
					comboBox_1.Text = ruleConfigInfo_0.NovelListUrl.Pattern;
				}
			}
		}
		comboBox_3.EndUpdate();
		comboBox_3.Text = taskConfigInfo_0.RuleFile;
		comboBox_4.BeginUpdate();
		comboBox_4.Items.Clear();
		string[] array2 = IO.LoadLogs();
		int num = -1;
		string[] array3 = array2;
		foreach (string text in array3)
		{
			if (text.EndsWith("db3"))
			{
				num++;
				comboBox_4.Items.Insert(num, text.Replace("Log\\", ""));
			}
		}
		if (num >= 0)
		{
			comboBox_4.SelectedIndex = num;
		}
		comboBox_4.EndUpdate();
	}

	private void method_1(object sender, EventArgs e)
	{
		if (new 自动采集模式(bool_1: true).ShowDialog() == DialogResult.OK)
		{
			taskConfigInfo_0 = (TaskConfigInfo)ConfigFileManager.LoadConfig("TaskConfig.xml", taskConfigInfo_0);
			ruleConfigInfo_0 = (RuleConfigInfo)ConfigFileManager.LoadConfig(taskConfigInfo_0.RuleFile, ruleConfigInfo_0);
			page_0 = new Page(ruleConfigInfo_0, taskConfigInfo_0);
			method_0();
		}
	}

	private void NovelMenuStrip_Opening(object sender, CancelEventArgs e)
	{
	}

	private void radioButton_3_CheckedChanged(object sender, EventArgs e)
	{
		if (Configs.CmsName == "UnsupportedCms")
		{
			comboBox_1.Text = "SELECT TOP 30 * FROM [Ws_BookList]";
		}
		else if (Configs.CmsName == "Jieqi")
		{
			comboBox_1.Text = "SELECT * FROM `jieqi_article_article` LIMIT  0,30";
		}
		else
		{
			comboBox_1.Text = "请输入SQL查询语句";
		}
	}

	private void radioButton_4_CheckedChanged(object sender, EventArgs e)
	{
		comboBox_1.Text = ruleConfigInfo_0.NovelListUrl.Pattern;
	}

	private void target_listview_DoubleClick(object sender, EventArgs e)
	{
		bool flag = false;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			if (target_list_view.Items[i].BackColor == Color.Red)
			{
				flag = true;
				num2 = i + 1;
				num++;
			}
		}
		if (num > 1)
		{
			MessageBox.Show("当前超过一个匹配项目，请通过手动选择采集。");
			return;
		}
		for (int j = 0; j < num2; j++)
		{
			target_list_view.Items[j].Checked = false;
		}
		for (int k = num2; k < target_list_view.Items.Count; k++)
		{
			target_list_view.Items[k].Checked = flag;
			if (target_list_view.Items[k].BackColor == Color.Red)
			{
				flag = true;
			}
		}
		if (target_list_view.CheckedItems.Count == 0 || bool_0 || backgroundWorker_6.IsBusy)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在采集章节.请勿进行其他操作..";
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		for (int l = 0; l < target_list_view.Items.Count; l++)
		{
			if (target_list_view.Items[l].Checked)
			{
				ChapterInfo value = (ChapterInfo)target_list_view.Items[l].Tag;
				arrayList.Add(value);
			}
		}
		novelInfo_0 = (NovelInfo)target_list_view.Tag;
		ChapterInfo[] argument = arrayList.ToArray();
		arrayList.Clear();
		backgroundWorker_6.RunWorkerAsync(argument);
	}

	private void TargetMenuStrip_Opening(object sender, CancelEventArgs e)
	{
	}

	private void toolStripButton_0_Click(object sender, EventArgs e)
	{
		panel_1.Visible = false;
		if (panel_0.Visible)
		{
			panel_0.Visible = false;
		}
		else
		{
			panel_0.Visible = true;
		}
	}

	private void toolStripButton_1_Click(object sender, EventArgs e)
	{
		panel_0.Visible = false;
		if (panel_1.Visible)
		{
			panel_1.Visible = false;
		}
		else
		{
			panel_1.Visible = true;
		}
	}

	private void toolStripButton_3_Click(object sender, EventArgs e)
	{
		if (backgroundWorker_6.IsBusy)
		{
			backgroundWorker_6.CancelAsync();
		}
		if (backgroundWorker_4.IsBusy)
		{
			backgroundWorker_4.CancelAsync();
		}
	}

	private void toolStripMenuItem_0_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView1.Items.Count; i++)
		{
			if (listView1.Items[i].Checked)
			{
				listView1.Items[i].Checked = false;
			}
			else
			{
				listView1.Items[i].Checked = true;
			}
		}
	}

	private void toolStripMenuItem_1_Click(object sender, EventArgs e)
	{
		if (listView_2.CheckedItems.Count == 0 || bool_0 || backgroundWorker_8.IsBusy)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在添加小说.请勿进行其他操作..";
		NovelInfo[] array = new NovelInfo[listView_2.CheckedItems.Count];
		int num = 0;
		for (int i = 0; i < listView_2.Items.Count; i++)
		{
			if (listView_2.Items[i].Checked)
			{
				array[num] = (NovelInfo)listView_2.Items[i].Tag;
				num++;
			}
		}
		backgroundWorker_8.RunWorkerAsync(array);
	}

	private void toolStripMenuItem_10_Click(object sender, EventArgs e)
	{
		bool flag = false;
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			if (target_list_view.Items[i].Checked)
			{
				flag = true;
			}
			if (flag)
			{
				target_list_view.Items[i].Checked = true;
			}
		}
	}

	private void toolStripMenuItem_11_Click(object sender, EventArgs e)
	{
		target_list_view.Items.Clear();
	}

	private void toolStripMenuItem_12_Click(object sender, EventArgs e)
	{
		if (target_list_view.CheckedItems.Count != listView_1.CheckedItems.Count || target_list_view.CheckedItems.Count == 0 || bool_0 || backgroundWorker_2.IsBusy)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在替换章节.请勿进行其他操作..";
		novelInfo_0 = (NovelInfo)target_list_view.Tag;
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			if (target_list_view.Items[i].Checked)
			{
				ChapterInfo value = (ChapterInfo)target_list_view.Items[i].Tag;
				arrayList.Add(value);
			}
		}
		chapterInfo_0 = arrayList.ToArray();
		arrayList.Clear();
		for (int j = 0; j < listView_1.Items.Count; j++)
		{
			if (listView_1.Items[j].Checked)
			{
				ChapterInfo value2 = (ChapterInfo)listView_1.Items[j].Tag;
				arrayList.Add(value2);
			}
		}
		chapterInfo_1 = arrayList.ToArray();
		backgroundWorker_2.RunWorkerAsync();
	}

	private void toolStripMenuItem_13_Click(object sender, EventArgs e)
	{
		if (listView_1.CheckedItems.Count == 0 || bool_0 || backgroundWorker_1.IsBusy)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在检查章节.请勿进行其他操作..";
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			if (listView_1.Items[i].Checked)
			{
				ChapterInfo value = (ChapterInfo)listView_1.Items[i].Tag;
				arrayList.Add(value);
			}
		}
		novelInfo_0 = (NovelInfo)target_list_view.Tag;
		backgroundWorker_1.RunWorkerAsync(arrayList.ToArray());
	}

	private void toolStripMenuItem_14_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			listView_1.Items[i].Checked = true;
		}
	}

	private void toolStripMenuItem_15_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			listView_1.Items[i].Checked = !listView_1.Items[i].Checked;
		}
	}

	private void toolStripMenuItem_16_Click(object sender, EventArgs e)
	{
		bool flag = false;
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			if (listView_1.Items[i].Checked)
			{
				flag = true;
			}
			if (flag)
			{
				listView_1.Items[i].Checked = true;
			}
		}
	}

	private void toolStripMenuItem_17_Click(object sender, EventArgs e)
	{
		if (listView_1.CheckedItems.Count == 0 || bool_0 || backgroundWorker_0.IsBusy)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在删除章节.请勿进行其他操作..";
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			if (listView_1.Items[i].Checked)
			{
				ChapterInfo chapterInfo = (ChapterInfo)listView_1.Items[i].Tag;
				chapterInfo.chaptertype = 0;
				arrayList.Add(chapterInfo);
			}
		}
		novelInfo_0 = (NovelInfo)target_list_view.Tag;
		NovelInfo novelInfo = (NovelInfo)target_list_view.Tag;
		backgroundWorker_0.RunWorkerAsync(arrayList.ToArray());
	}

	private void toolStripMenuItem_18_Click(object sender, EventArgs e)
	{
		bool flag = false;
		for (int i = 0; i < listView_2.Items.Count; i++)
		{
			if (listView_2.Items[i].Checked)
			{
				flag = true;
			}
			if (flag)
			{
				listView_2.Items[i].Checked = true;
			}
		}
	}

	private void toolStripMenuItem_19_Click(object sender, EventArgs e)
	{
		if (listView_2.CheckedItems.Count == 0)
		{
			return;
		}
		NovelInfo[] array = new NovelInfo[listView_2.CheckedItems.Count];
		int num = 0;
		for (int i = 0; i < listView_2.Items.Count; i++)
		{
			if (listView_2.Items[i].Checked)
			{
				array[num] = (NovelInfo)listView_2.Items[i].Tag;
				num++;
			}
		}
		backgroundWorker_5.RunWorkerAsync(array);
	}

	private void toolStripMenuItem_2_Click(object sender, EventArgs e)
	{
		if (listView_2.CheckedItems.Count > 1)
		{
			MessageBox.Show("本功能只支持单本删除，你确认你只选择了一本小说！");
			return;
		}
		if (listView_2.CheckedItems.Count == 1)
		{
			bool_0 = true;
		}
		panel_2.Enabled = false;
		List<string> arrayList = new List<string>();
		string text = "";
		for (int i = 0; i < listView_2.Items.Count; i++)
		{
			if (listView_2.Items[i].Checked)
			{
				NovelInfo novelInfo = (NovelInfo)listView_2.Items[i].Tag;
				string value = i + "^" + novelInfo.PutID + "^" + novelInfo.GetID + "^" + novelInfo.Name + "^" + comboBox_4.Text;
				NovelSpider.Local.LocalProvider.GetInstance().ClearNovel(novelInfo);
				NovelSpider.Local.LocalProvider.GetInstance().DeteleNovel(novelInfo.PutID);
				text = novelInfo.Name;
				arrayList.Add(value);
			}
		}
		toolStripStatusLabel_0.Text = "正在删除《" + text + "》";
		backgroundWorker_11.RunWorkerAsync(arrayList.ToArray());
	}

	private void toolStripMenuItem_20_Click(object sender, EventArgs e)
	{
		if (listView_2.CheckedItems.Count == 1 && !bool_0 && !backgroundWorker_7.IsBusy)
		{
			bool_0 = true;
			panel_2.Enabled = false;
			toolStripStatusLabel_0.Text = "正在列出章节.请勿进行其他操作..";
			NovelInfo novelInfo = (NovelInfo)listView_2.CheckedItems[0].Tag;
			target_list_view.Tag = novelInfo;
			backgroundWorker_7.RunWorkerAsync(novelInfo);
		}
	}

	private void toolStripMenuItem_21_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView_2.Items.Count; i++)
		{
			listView_2.Items[i].Checked = false;
		}
	}

	private void toolStripMenuItem_22_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			target_list_view.Items[i].Checked = false;
		}
	}

	private void toolStripMenuItem_23_Click(object sender, EventArgs e)
	{
		NovelInfo novelInfo = (NovelInfo)target_list_view.Tag;
		string text = "===对比信息===\n" + novelInfo.GetID + " | " + novelInfo.Name + " | " + novelInfo.PutID + "\n";
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			if (target_list_view.Items[i].BackColor == Color.Red)
			{
				ChapterInfo chapterInfo = (ChapterInfo)target_list_view.Items[i].Tag;
				text = text + chapterInfo.VolumeName + "\n" + chapterInfo.ChapterName + "\n";
			}
		}
		WinFormsRuntime.SetClipboardText(text + novelInfo.LastChapter.VolumeName + "\n" + novelInfo.LastChapter.ChapterName + "\n");
		MessageBox.Show("复制成功，可直接Ctrl+V复制到QQ中。");
	}

	private void toolStripMenuItem_24_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			listView_1.Items[i].Checked = false;
		}
	}

	private void toolStripMenuItem_25_Click(object sender, EventArgs e)
	{
		if (target_list_view.CheckedItems.Count == 0 || listView_1.CheckedItems.Count != 1 || bool_0 || backgroundWorker_9.IsBusy)
		{
			MessageBox.Show("远程章节未选择/本地章节选择数量不等于1/后台线程正忙");
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在插入章节.请勿进行其他操作..";
		novelInfo_0 = (NovelInfo)target_list_view.Tag;
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			if (target_list_view.Items[i].Checked)
			{
				ChapterInfo value = (ChapterInfo)target_list_view.Items[i].Tag;
				arrayList.Add(value);
			}
		}
		chapterInfo_0 = arrayList.ToArray();
		arrayList.Clear();
		for (int j = 0; j < listView_1.Items.Count; j++)
		{
			if (listView_1.Items[j].Checked)
			{
				ChapterInfo chapterInfo = (ChapterInfo)listView_1.Items[j].Tag;
				int_1 = chapterInfo.PutID;
				int_3 = j;
				break;
			}
		}
		backgroundWorker_9.RunWorkerAsync();
	}

	private void toolStripMenuItem_26_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("章节数过多时检测TXT可能出现卡顿、无响现象，请务进行其他的操作！", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在检测重新章节请误进行其他操作...";
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			listView_1.Items[i].Checked = false;
		}
		for (int j = 0; j < target_list_view.Items.Count; j++)
		{
			target_list_view.Items[j].Checked = false;
		}
		string text = "";
		int num = 0;
		for (int k = 0; k < listView_1.Items.Count; k++)
		{
			NovelInfo novelInfo = (NovelInfo)target_list_view.Tag;
			ChapterInfo chapterInfo = (ChapterInfo)listView_1.Items[k].Tag;
			if (File.Exists(NovelSpider.Local.Jieqi.Config.TxtDir + "/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID.ToString() + "/" + chapterInfo.PutID.ToString() + ".txt"))
			{
				continue;
			}
			listView_1.Items[k].Checked = true;
			if (listView_1.Items[k].Checked)
			{
				num++;
			}
			for (int l = 0; l < target_list_view.Items.Count; l++)
			{
				if (!(listView_1.Items[k].SubItems[2].Text == target_list_view.Items[l].SubItems[2].Text))
				{
					if (listView_1.Items[k].SubItems[2].Text != target_list_view.Items[l].SubItems[2].Text && target_list_view.Items.Count - 1 == l)
					{
						listView_1.Items[k].Checked = false;
						object obj = text;
						text = string.Concat(obj, "\n索引 ", k, "、", listView_1.Items[k].SubItems[2].Text);
						break;
					}
					continue;
				}
				target_list_view.Items[l].Checked = true;
				break;
			}
		}
		if (num > 0)
		{
			MessageBox.Show("检测到本站" + num + "个章节无TXT文本,与目标站对比成功" + target_list_view.CheckedItems.Count + "个章节，" + (num - target_list_view.CheckedItems.Count) + "个章节对比失败！\n失败章节如下：" + text);
		}
		else
		{
			MessageBox.Show("当前小说章节TXT完整！");
		}
		toolStripStatusLabel_0.Text = "操作完成";
		panel_0.Visible = false;
		panel_1.Visible = false;
		button_0.Enabled = true;
		button_3.Enabled = true;
		bool_0 = false;
		panel_2.Enabled = true;
	}

	private void toolStripMenuItem_27_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("章节数过多时检测重复章节可能出现卡顿、无响现象，请务进行其他的操作！", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在检测重复章节请误进行其他操作...";
		for (int num = listView_1.Items.Count - 1; num >= 0; num--)
		{
			if (num < listView_1.Items.Count && num > 0)
			{
				string text = listView_1.Items[num].SubItems[2].Text;
				for (int num2 = num; num2 >= 0; num2--)
				{
					if (text == listView_1.Items[num2].SubItems[2].Text && num != num2)
					{
						listView_1.Items[num].Checked = true;
					}
				}
			}
		}
		if (listView_1.CheckedItems.Count > 0)
		{
			MessageBox.Show("检测到当前小说存在" + listView_1.CheckedItems.Count + "个重复章节，此数据只供参考，请自己认真后再删除！");
		}
		else
		{
			MessageBox.Show("当前小说无重复章节！");
		}
		toolStripStatusLabel_0.Text = "操作完成";
		panel_0.Visible = false;
		panel_1.Visible = false;
		button_0.Enabled = true;
		button_3.Enabled = true;
		bool_0 = false;
		panel_2.Enabled = true;
	}

	private void toolStripMenuItem_28_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView1.Items.Count; i++)
		{
			listView1.Items[i].Checked = false;
		}
	}

	private void toolStripMenuItem_29_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView1.Items.Count; i++)
		{
			listView1.Items[i].Checked = true;
		}
	}

	private void toolStripMenuItem_3_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView_2.Items.Count; i++)
		{
			listView_2.Items[i].Checked = true;
		}
	}

	private void toolStripMenuItem_30_Click(object sender, EventArgs e)
	{
		if (listView_1.CheckedItems.Count == 0 || bool_0 || backgroundWorker_12.IsBusy)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在删除章节.请勿进行其他操作..";
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			if (listView_1.Items[i].Checked)
			{
				ChapterInfo chapterInfo = (ChapterInfo)listView_1.Items[i].Tag;
				chapterInfo.chaptertype = 0;
				arrayList.Add(chapterInfo);
			}
		}
		novelInfo_0 = (NovelInfo)target_list_view.Tag;
		NovelInfo novelInfo = (NovelInfo)target_list_view.Tag;
		backgroundWorker_12.RunWorkerAsync(arrayList.ToArray());
	}

	private void toolStripMenuItem_31_Click(object sender, EventArgs e)
	{
		if (listView1.CheckedItems.Count == 0 || bool_0 || backgroundWorker_0.IsBusy)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在删除分卷.请勿进行其他操作..";
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		for (int i = 0; i < listView1.Items.Count; i++)
		{
			if (listView1.Items[i].Checked)
			{
				ChapterInfo value = (ChapterInfo)listView1.Items[i].Tag;
				arrayList.Add(value);
			}
		}
		novelInfo_0 = (NovelInfo)target_list_view.Tag;
		backgroundWorker_0.RunWorkerAsync(arrayList.ToArray());
	}

	private void toolStripMenuItem_32_Click(object sender, EventArgs e)
	{
		if (bool_0 || backgroundWorker_10.IsBusy || listView_2.CheckedItems.Count < 1)
		{
			return;
		}
		if (listView_2.CheckedItems.Count > 1)
		{
			MessageBox.Show("请确认你只选择了一本小说！");
			return;
		}
		toolStripStatusLabel_0.Text = "正准备设为全本..";
		NovelInfo novelInfo = (NovelInfo)listView_2.CheckedItems[0].Tag;
		switch (MessageBox.Show("是否把《" + novelInfo.Name + "》设置为完结小说！是为全本，否为连载！", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
		{
		case DialogResult.Yes:
			bool_0 = true;
			panel_2.Enabled = false;
			target_list_view.Tag = novelInfo;
			novelInfo.MDegree = 1;
			novelInfo.Degree = 1;
			backgroundWorker_10.RunWorkerAsync(novelInfo);
			break;
		case DialogResult.No:
			bool_0 = true;
			panel_2.Enabled = false;
			target_list_view.Tag = novelInfo;
			novelInfo.MDegree = 0;
			novelInfo.Degree = 0;
			backgroundWorker_10.RunWorkerAsync(novelInfo);
			break;
		}
		for (int i = 0; i < listView_2.Items.Count; i++)
		{
			listView_2.Items[i].Checked = false;
		}
	}

	private void toolStripMenuItem_33_Click(object sender, EventArgs e)
	{
		ReviseChapter.Dock = DockStyle.Fill;
		if (Configs.CmsName != "UnsupportedCms")
		{
			if (listView_1.CheckedItems.Count != 1 && !bool_0 && !backgroundWorker_0.IsBusy)
			{
				if (listView_1.CheckedItems.Count > 1)
				{
					MessageBox.Show("我干你选择这么多章节让准备让我修改那个章节！");
				}
				else
				{
					MessageBox.Show("你妹的，你不选择章节我应该怎么处理？");
				}
				return;
			}
			ReviseChapter.Visible = true;
			
			for (int i = 0; i < listView_1.Items.Count; i++)
			{
				if (listView_1.Items[i].Checked)
				{
					NovelInfo novelInfo = (NovelInfo)target_list_view.Tag;
					ChapterInfo chapterInfo = (ChapterInfo)listView_1.Items[i].Tag;
					articlenameBox.Text = novelInfo.Name;
					posterBox.Text = novelInfo.Author;
					sortBox.Items.Clear();
					for (int j = 1; j < NovelSpider.Local.Jieqi.Config.JieqiSort.Length; j++)
					{
						sortBox.Items.Add(NovelSpider.Local.Jieqi.Config.JieqiSort[j]);
					}
					sortBox.SelectedItem = novelInfo.LagerSort;
					chapterNameBox.Text = chapterInfo.ChapterName;
					string path = NovelSpider.Local.Jieqi.Config.TxtDir + "/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID.ToString() + "/" + chapterInfo.PutID.ToString() + ".txt";
					if (File.Exists(path))
					{
						chapterInfo.ChapterText = NovelSpider.Local.Jieqi.TextEncodingPolicy.ReadLegacyChapterText(path);
					}
					else
					{
						chapterInfo.ChapterText = "恭喜中奖了！又碰到无TXT文本的章节！或些章节为图片章节！";
					}
					chapterTXTBox.Text = chapterInfo.ChapterText;
				}
			}
			return;
		}
		if (listView_1.CheckedItems.Count != 1 && !bool_0 && !backgroundWorker_0.IsBusy)
		{
			if (listView_1.CheckedItems.Count > 1)
			{
				MessageBox.Show("我干你选择这么多章节让准备让我修改那个章节！");
			}
			else
			{
				MessageBox.Show("你妹的，你不选择章节我应该怎么处理？");
			}
			return;
		}
		ReviseChapter.Visible = true;
		
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			if (listView_1.Items[i].Checked)
			{
				NovelInfo novelInfo = (NovelInfo)target_list_view.Tag;
				ChapterInfo chapterInfo = (ChapterInfo)listView_1.Items[i].Tag;
				articlenameBox.Text = novelInfo.Name;
				posterBox.Text = novelInfo.Author;
				sortBox.Items.Clear();
				sortBox.Items.Add(novelInfo.LagerSort);
				sortBox.SelectedItem = novelInfo.LagerSort;
				sortBox.Enabled = false;
				chapterNameBox.Text = chapterInfo.ChapterName;
				string path = NovelSpider.Local.Jieqi.Config.TxtDir + "/" + novelInfo.PutID / 1000 + "/" + novelInfo.PutID.ToString() + "/" + chapterInfo.PutID.ToString() + ".txt";
				if (File.Exists(path))
				{
					chapterInfo.ChapterText = NovelSpider.Local.Jieqi.TextEncodingPolicy.ReadLegacyChapterText(path);
				}
				else
				{
					chapterInfo.ChapterText = "恭喜中奖了！又碰到无TXT文本的章节！或些章节为图片章节！";
				}
				chapterTXTBox.Text = chapterInfo.ChapterText;
			}
		}
	}

	private void toolStripMenuItem_34_Click(object sender, EventArgs e)
	{
		bool flag = false;
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			if (target_list_view.Items[i].Checked)
			{
				flag = true;
			}
			if (flag)
			{
				target_list_view.Items[i].Checked = true;
			}
		}
		if (target_list_view.CheckedItems.Count == 0 || bool_0 || backgroundWorker_6.IsBusy)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在采集章节.请勿进行其他操作..";
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		for (int j = 0; j < target_list_view.Items.Count; j++)
		{
			if (target_list_view.Items[j].Checked)
			{
				ChapterInfo value = (ChapterInfo)target_list_view.Items[j].Tag;
				arrayList.Add(value);
			}
		}
		novelInfo_0 = (NovelInfo)target_list_view.Tag;
		ChapterInfo[] argument = arrayList.ToArray();
		arrayList.Clear();
		backgroundWorker_6.RunWorkerAsync(argument);
	}

	private void toolStripMenuItem_35_Click(object sender, EventArgs e)
	{
		bool flag = false;
		for (int i = 0; i < listView1.Items.Count; i++)
		{
			if (listView1.Items[i].Checked)
			{
				flag = true;
			}
			if (flag)
			{
				listView1.Items[i].Checked = true;
			}
		}
	}

	private void toolStripMenuItem_36_Click(object sender, EventArgs e)
	{
		int num = 0;
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < listView_1.Items.Count; i++)
		{
			if (listView_1.Items[i].Checked)
			{
				num++;
				if (num2 == -1)
				{
					num2 = i;
				}
				else
				{
					num3 = i;
				}
			}
		}
		if (num != 2)
		{
			MessageBox.Show("只能选择前后两项");
			return;
		}
		for (int j = num2 + 1; j < num3; j++)
		{
			listView_1.Items[j].Checked = true;
		}
	}

	private void toolStripMenuItem_37_Click(object sender, EventArgs e)
	{
		int num = 0;
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			if (target_list_view.Items[i].Checked)
			{
				num++;
				if (num2 == -1)
				{
					num2 = i;
				}
				else
				{
					num3 = i;
				}
			}
		}
		if (num != 2)
		{
			MessageBox.Show("只能选择前后两项");
			return;
		}
		for (int j = num2 + 1; j < num3; j++)
		{
			target_list_view.Items[j].Checked = true;
		}
	}

	private void toolStripMenuItem_4_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < listView_2.Items.Count; i++)
		{
			listView_2.Items[i].Checked = !listView_2.Items[i].Checked;
		}
	}

	private void toolStripMenuItem_5_Click(object sender, EventArgs e)
	{
		listView_2.Items.Clear();
	}

	private void toolStripMenuItem_6_Click(object sender, EventArgs e)
	{
		if (target_list_view.CheckedItems.Count == 0 || bool_0 || backgroundWorker_6.IsBusy)
		{
			return;
		}
		bool_0 = true;
		panel_2.Enabled = false;
		toolStripStatusLabel_0.Text = "正在采集章节.请勿进行其他操作..";
		List<ChapterInfo> arrayList = new List<ChapterInfo>();
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			if (target_list_view.Items[i].Checked)
			{
				ChapterInfo value = (ChapterInfo)target_list_view.Items[i].Tag;
				arrayList.Add(value);
			}
		}
		novelInfo_0 = (NovelInfo)target_list_view.Tag;
		ChapterInfo[] argument = arrayList.ToArray();
		arrayList.Clear();
		backgroundWorker_6.RunWorkerAsync(argument);
	}

	private void toolStripMenuItem_8_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			target_list_view.Items[i].Checked = true;
		}
	}

	private void toolStripMenuItem_9_Click(object sender, EventArgs e)
	{
		for (int i = 0; i < target_list_view.Items.Count; i++)
		{
			target_list_view.Items[i].Checked = !target_list_view.Items[i].Checked;
		}
	}
}







