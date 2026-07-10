using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NovelSpider.Common;

public static class PerformanceTelemetry
{
	private const string EnableEnvironmentVariable = "NOVELSPIDER_PERFORMANCE";

	private const int MaximumPendingEntries = 4096;

	private static readonly IDisposable DisabledScope = new NoopScope();

	private static readonly ConcurrentQueue<Entry> Entries = new();

	private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

	private static int flushScheduled;

	private static int pendingEntryCount;

	public static IDisposable Measure(string category, string operation, string subject = "")
	{
		if (!IsEnabled())
		{
			return DisabledScope;
		}
		return new Scope(category, operation, subject);
	}

	public static void Record(string category, string operation, long elapsedMilliseconds, string subject = "", bool succeed = true, string message = "")
	{
		if (!IsEnabled())
		{
			return;
		}
		try
		{
			if (Interlocked.Increment(ref pendingEntryCount) > MaximumPendingEntries)
			{
				Interlocked.Decrement(ref pendingEntryCount);
				return;
			}
			Entries.Enqueue(new Entry(DateTime.Now, Environment.CurrentManagedThreadId, category, operation, elapsedMilliseconds, subject, succeed, message));
			ScheduleFlush();
		}
		catch
		{
			Interlocked.Decrement(ref pendingEntryCount);
		}
	}

	public static bool IsEnabled()
	{
		string value = Environment.GetEnvironmentVariable(EnableEnvironmentVariable);
		return string.Equals(value, "1", StringComparison.OrdinalIgnoreCase)
			|| string.Equals(value, "true", StringComparison.OrdinalIgnoreCase)
			|| string.Equals(value, "on", StringComparison.OrdinalIgnoreCase)
			|| string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
	}

	private static void ScheduleFlush()
	{
		if (Interlocked.CompareExchange(ref flushScheduled, 1, 0) == 0)
		{
			_ = Task.Run(FlushAsync);
		}
	}

	private static async Task FlushAsync()
	{
		try
		{
			while (!Entries.IsEmpty)
			{
				Dictionary<string, StringBuilder> batches = new(StringComparer.Ordinal);
				while (Entries.TryDequeue(out Entry entry))
				{
					Interlocked.Decrement(ref pendingEntryCount);
					string directory = Path.Combine(AppContext.BaseDirectory, "Log", "Performance");
					string file = Path.Combine(directory, entry.Timestamp.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".csv");
					if (!batches.TryGetValue(file, out StringBuilder batch))
					{
						batch = new StringBuilder();
						batches.Add(file, batch);
					}
					batch.Append(Escape(entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture))).Append(',')
						.Append(entry.ThreadId.ToString(CultureInfo.InvariantCulture)).Append(',')
						.Append(Escape(entry.Category)).Append(',')
						.Append(Escape(entry.Operation)).Append(',')
						.Append(entry.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture)).Append(',')
						.Append(entry.Succeed ? '1' : '0').Append(',')
						.Append(Escape(entry.Subject)).Append(',')
						.Append(Escape(entry.Message)).Append("\r\n");
				}

				foreach (KeyValuePair<string, StringBuilder> batch in batches)
				{
					string directory = Path.GetDirectoryName(batch.Key);
					Directory.CreateDirectory(directory);
					bool includeHeader = !File.Exists(batch.Key);
					string contents = (includeHeader ? "time,thread,category,operation,elapsed_ms,succeed,subject,message\r\n" : string.Empty) + batch.Value;
					await File.AppendAllTextAsync(batch.Key, contents, Utf8NoBom).ConfigureAwait(false);
				}
			}
		}
		catch
		{
		}
		finally
		{
			Interlocked.Exchange(ref flushScheduled, 0);
			if (!Entries.IsEmpty)
			{
				ScheduleFlush();
			}
		}
	}

	private static string Escape(string value)
	{
		value ??= string.Empty;
		if (value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) < 0)
		{
			return value;
		}
		return "\"" + value.Replace("\"", "\"\"") + "\"";
	}

	private sealed class Scope : IDisposable
	{
		private readonly string category;

		private readonly string operation;

		private readonly string subject;

		private readonly Stopwatch stopwatch;

		private int disposed;

		public Scope(string category, string operation, string subject)
		{
			this.category = category;
			this.operation = operation;
			this.subject = subject;
			stopwatch = Stopwatch.StartNew();
		}

		public void Dispose()
		{
			if (Interlocked.Exchange(ref disposed, 1) == 0)
			{
				stopwatch.Stop();
				Record(category, operation, stopwatch.ElapsedMilliseconds, subject);
			}
		}
	}

	private sealed class NoopScope : IDisposable
	{
		public void Dispose()
		{
		}
	}

	private readonly record struct Entry(DateTime Timestamp, int ThreadId, string Category, string Operation, long ElapsedMilliseconds, string Subject, bool Succeed, string Message);
}
