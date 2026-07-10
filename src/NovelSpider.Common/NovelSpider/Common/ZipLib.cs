using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksum;
using ICSharpCode.SharpZipLib.Zip;

namespace NovelSpider.Common;

public class ZipLib
{
	private Crc32 crc32_0;

	private string XrXaevcbd;

	private ZipOutputStream zipOutputStream_0;

	private void method_0(string string_0)
	{
		string[] files = Directory.GetFiles(string_0);
		string[] directories = Directory.GetDirectories(string_0);
		string[] array = files;
		foreach (string text in array)
		{
			FileStream fileStream = File.OpenRead(text);
			byte[] array2 = new byte[fileStream.Length];
			fileStream.Read(array2, 0, array2.Length);
			ZipEntry zipEntry = new ZipEntry(text.Replace(XrXaevcbd, ""))
			{
				DateTime = DateTime.Now,
				Size = fileStream.Length
			};
			fileStream.Close();
			crc32_0.Reset();
			crc32_0.Update(array2);
			zipEntry.Crc = crc32_0.Value;
			zipOutputStream_0.PutNextEntry(zipEntry);
			zipOutputStream_0.Write(array2, 0, array2.Length);
		}
		string[] array3 = directories;
		foreach (string text2 in array3)
		{
			zipOutputStream_0.PutNextEntry(new ZipEntry(text2.Replace(XrXaevcbd, "")));
			method_0(text2);
		}
	}

	public void ZipToFile(string string_1, string string_2)
	{
		if (!string_1.EndsWith("\\"))
		{
			string_1 += "\\";
		}
		XrXaevcbd = string_1;
		Directory.GetFiles(string_1);
		Directory.GetDirectories(string_1);
		crc32_0 = new Crc32();
		zipOutputStream_0 = new ZipOutputStream(File.Create(string_2));
		zipOutputStream_0.SetLevel(6);
		method_0(string_1);
		zipOutputStream_0.Finish();
		zipOutputStream_0.Close();
	}
}
