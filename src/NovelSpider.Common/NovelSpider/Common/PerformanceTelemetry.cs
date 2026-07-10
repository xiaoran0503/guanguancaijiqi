using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace NovelSpider.Common;

public static class PerformanceTelemetry
{
	private const string EnableEnvironmentVariable = "NOVELSPIDER_PERFORMANCE";

	private static readonly IDisposable DisabledScope = new NoopScope();

	private static readonly object SyncRoot = new();

	private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

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
			string directory = Path.Combine(AppContext.BaseDirectory, "Log", "Performance");
			Directory.CreateDirectory(directory);
			string file = Path.Combine(directory, DateTime.Today.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".csv");
			lock (SyncRoot)
			{
				if (!File.Exists(file))
				{
					File.AppendAllText(file, "time,thread,category,operation,elapsed_ms,succeed,subject,message\r\n", Utf8NoBom);
				}
				string line = string.Join(",",
					Escape(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)),
					Environment.CurrentManagedThreadId.ToString(CultureInfo.InvariantCulture),
					Escape(category),
					Escape(operation),
					elapsedMilliseconds.ToString(CultureInfo.InvariantCulture),
					succeed ? "1" : "0",
					Escape(subject),
					Escape(message));
				File.AppendAllText(file, line + "\r\n", Utf8NoBom);
			}
		}
		catch
		{
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
}
