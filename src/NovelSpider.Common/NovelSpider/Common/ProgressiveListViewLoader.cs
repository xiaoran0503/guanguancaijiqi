using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NovelSpider.Common;

public static class ProgressiveListViewLoader
{
	private const int BatchSize = 200;

	private static readonly ConditionalWeakTable<ListView, LoadState> States = new();

	public static void ReplaceItems(ListView listView, IEnumerable<ListViewItem> items, ToolStripStatusLabel statusLabel = null, string label = "")
	{
		if (listView == null)
		{
			return;
		}

		List<ListViewItem> pendingItems = items?.Where(static item => item != null).ToList() ?? [];
		void Start()
		{
			LoadState state = States.GetOrCreateValue(listView);
			int generation = state.NextGeneration();
			listView.BeginUpdate();
			try
			{
				listView.Items.Clear();
			}
			finally
			{
				listView.EndUpdate();
			}
			AppendBatch(listView, pendingItems, statusLabel, label, state, generation, 0);
		}

		if (listView.IsHandleCreated && listView.InvokeRequired)
		{
			listView.BeginInvoke((MethodInvoker)Start);
		}
		else
		{
			Start();
		}
	}

	private static void AppendBatch(ListView listView, List<ListViewItem> items, ToolStripStatusLabel statusLabel, string label, LoadState state, int generation, int index)
	{
		if (listView.IsDisposed || state.Generation != generation)
		{
			return;
		}

		Stopwatch stopwatch = Stopwatch.StartNew();
		int count = Math.Min(BatchSize, items.Count - index);
		if (count > 0)
		{
			ListViewItem[] batch = new ListViewItem[count];
			items.CopyTo(index, batch, 0, count);
			listView.BeginUpdate();
			try
			{
				listView.Items.AddRange(batch);
			}
			finally
			{
				listView.EndUpdate();
			}
		}
		stopwatch.Stop();

		int loaded = Math.Min(index + count, items.Count);
		if (statusLabel != null)
		{
			statusLabel.Text = string.IsNullOrEmpty(label) ? $"已加载 {loaded}/{items.Count}" : $"{label} {loaded}/{items.Count}";
		}
		PerformanceTelemetry.Record("ui", "list_batch", stopwatch.ElapsedMilliseconds, listView.Name, message: $"loaded={loaded};total={items.Count}");

		if (loaded < items.Count)
		{
			listView.BeginInvoke((MethodInvoker)delegate
			{
				AppendBatch(listView, items, statusLabel, label, state, generation, loaded);
			});
		}
	}

	private sealed class LoadState
	{
		private int generation;

		public int Generation => generation;

		public int NextGeneration()
		{
			return unchecked(++generation);
		}
	}
}