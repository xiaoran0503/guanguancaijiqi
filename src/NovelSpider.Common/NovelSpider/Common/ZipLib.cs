using System.IO;
using System.IO.Compression;

namespace NovelSpider.Common;

public class ZipLib
{
	public void ZipToFile(string string_1, string string_2)
	{
		string sourceDirectory = Path.GetFullPath(string_1);
		if (!Directory.Exists(sourceDirectory))
		{
			return;
		}

		string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(string_2));
		if (!string.IsNullOrEmpty(outputDirectory))
		{
			Directory.CreateDirectory(outputDirectory);
		}

		if (File.Exists(string_2))
		{
			File.Delete(string_2);
		}

		using FileStream fileStream = File.Create(string_2);
		using ZipArchive archive = new ZipArchive(fileStream, ZipArchiveMode.Create);
		foreach (string file in Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories))
		{
			string entryName = Path.GetRelativePath(sourceDirectory, file).Replace(Path.DirectorySeparatorChar, '/');
			archive.CreateEntryFromFile(file, entryName, CompressionLevel.Optimal);
		}
	}
}
