using System;
using System.IO;
using System.Text;
using NovelSpider.Common;
using NovelSpider.Config;

namespace NovelSpider.Local.Jieqi;

public static class TextEncodingPolicy
{
	public static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: false);

	public static string NormalizeDatabaseText(string text)
	{
		return text ?? string.Empty;
	}

	public static string ReadLegacyChapterText(string path)
	{
		if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
		{
			return string.Empty;
		}
		byte[] bytes = File.ReadAllBytes(path);
		if (bytes.Length == 0)
		{
			return string.Empty;
		}
		if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
		{
			return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);
		}
		try
		{
			return new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true).GetString(bytes);
		}
		catch (DecoderFallbackException)
		{
			Encoding legacyEncoding = FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, Config.JieqiCharset);
			return legacyEncoding.GetString(bytes);
		}
	}
}

