using System;
using System.IO;
using System.Text;
using NovelSpider.Common;

namespace NovelSpider.Local.Jieqi;

internal static class ChapterFileWriter
{
	private const int BufferSize = 64 * 1024;

	public static void WriteChapterText(string directory, int chapterId, string chapterText, Encoding encoding)
	{
		string fileName = chapterId + ".txt";
		WriteTextAtomic(directory, fileName, chapterText ?? string.Empty, encoding);
	}

	public static void WriteTextAtomic(string targetPath, string content, Encoding encoding)
	{
		string directory = Path.GetDirectoryName(targetPath);
		if (string.IsNullOrEmpty(directory))
		{
			directory = ".";
		}
		string fileName = Path.GetFileName(targetPath);
		WriteTextAtomic(directory, fileName, content, encoding);
	}

	public static void WriteTextAtomic(string directory, string fileName, string content, Encoding encoding)
	{
		using IDisposable scope = PerformanceTelemetry.Measure("file", "write_text", Path.Combine(directory, fileName));
		Directory.CreateDirectory(directory);
		string targetPath = Path.Combine(directory, fileName);
		string tempPath = targetPath + "." + Environment.ProcessId + "." + Guid.NewGuid().ToString("N") + ".tmp";
		try
		{
			FileStreamOptions options = new FileStreamOptions
			{
				Mode = FileMode.CreateNew,
				Access = FileAccess.Write,
				Share = FileShare.Read,
				BufferSize = BufferSize,
				Options = FileOptions.SequentialScan
			};
			using (FileStream stream = new FileStream(tempPath, options))
			using (StreamWriter writer = new StreamWriter(stream, encoding ?? Encoding.UTF8, BufferSize))
			{
				writer.Write(content ?? string.Empty);
			}
			File.Move(tempPath, targetPath, overwrite: true);
		}
		catch
		{
			try
			{
				if (File.Exists(tempPath))
				{
					File.Delete(tempPath);
				}
			}
			catch
			{
			}
			throw;
		}
	}
}
