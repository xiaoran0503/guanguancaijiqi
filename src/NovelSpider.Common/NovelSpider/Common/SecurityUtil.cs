using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using NovelSpider.Config;

namespace NovelSpider.Common;

public class SecurityUtil
{
	public static string Base64Decode(string Message)
	{
		byte[] bytes = Convert.FromBase64String(Message);
		return FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk").GetString(bytes);
	}

	public static string Base64Encode(string Message)
	{
		return Convert.ToBase64String(FormatText.GetCharset(Configs.BaseConfig.CmsEncoding, "gbk").GetBytes(Message));
	}

	public static int ConvertDateTimeInt(DateTime time)
	{
		return (int)new DateTimeOffset(time).ToUnixTimeSeconds();
	}

	public static string DesDecode(string data, string key)
	{
		string s = key.Substring(0, 8);
		string s2 = key.Substring(8, 8);
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		byte[] bytes2 = Encoding.ASCII.GetBytes(s2);
		byte[] buffer;
		try
		{
			buffer = Convert.FromBase64String(data);
		}
		catch
		{
			return null;
		}
		using DES dESCryptoServiceProvider = DES.Create();
		MemoryStream stream = new MemoryStream(buffer);
		CryptoStream stream2 = new CryptoStream(stream, dESCryptoServiceProvider.CreateDecryptor(bytes, bytes2), CryptoStreamMode.Read);
		StreamReader streamReader = new StreamReader(stream2);
		string empty = string.Empty;
		try
		{
			return streamReader.ReadToEnd();
		}
		catch
		{
			return null;
		}
	}

	public static string DesEncode(string data, string key)
	{
		string s = key.Substring(0, 8);
		string s2 = key.Substring(8, 8);
		byte[] bytes = Encoding.ASCII.GetBytes(s);
		byte[] bytes2 = Encoding.ASCII.GetBytes(s2);
		using DES dESCryptoServiceProvider = DES.Create();
		MemoryStream memoryStream = new MemoryStream();
		CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, bytes2), CryptoStreamMode.Write);
		StreamWriter streamWriter = new StreamWriter(cryptoStream);
		streamWriter.Write(data);
		streamWriter.Flush();
		cryptoStream.FlushFinalBlock();
		streamWriter.Flush();
		return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
	}

	public static DateTime GetTime(string timeStamp)
	{
		return DateTimeOffset.FromUnixTimeSeconds(long.Parse(timeStamp)).LocalDateTime;
	}

	public static bool IsDate(string text)
	{
		return Regex.IsMatch(text, "[\\d]{4}-[\\d]{1,2}-[\\d]{1,2}");
	}

	public static bool IsEmail(string text)
	{
		if (!(text == ""))
		{
			return Regex.IsMatch(text, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
		}
		return true;
	}

	public static bool IsIP(string text)
	{
		IPAddress address;
		return IPAddress.TryParse(text, out address);
	}

	public static bool IsNum(string text)
	{
		int result = 0;
		return int.TryParse(text, out result);
	}

	public static string ComputeMD5(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return "00000000000000000000000000000000";
		}
		return ComputeMd5Hash(text, "MD5");
	}

	public static string NoHtml(string html)
	{
		html = Regex.Replace(html, "<.+?>", "").Replace(" ", "");
		return html;
	}

	public static Match RegexsMatch(string string_0, string string_1)
	{
		Regex regex = new Regex(string_1, RegexOptions.IgnoreCase | RegexOptions.Multiline);
		return regex.Match(string_0);
	}

	public static MatchCollection RegexsMatches(string string_0, string string_1)
	{
		Regex regex = new Regex(string_1, RegexOptions.IgnoreCase | RegexOptions.Multiline);
		return regex.Matches(string_0);
	}
	private static string ComputeMd5Hash(string input, string algorithm)
	{
		byte[] bytes = System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes(input));
		return Convert.ToHexString(bytes);
	}
}
