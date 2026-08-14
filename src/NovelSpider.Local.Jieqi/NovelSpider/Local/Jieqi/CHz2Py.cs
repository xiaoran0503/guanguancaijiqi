using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;

namespace NovelSpider.Local.Jieqi;

public class CHz2Py
{
	private static bool bool_0;

	private static Hashtable hashtable_0;

	private static string string_0;

	private static string[] string_1;

	static CHz2Py()
	{
		smethod_1();
	}

	public static string Convert4Hz2Py(string string_0)
	{
		if (string.IsNullOrEmpty(string_0))
		{
			return "";
		}
		string_0 = Regex.Replace(string_0, "[^一-龥]", "");
		string text = string.Empty;
		int num = 0;
		string text2 = "";
		string text3 = string_0;
		while (text3.Length >= 1)
		{
			num = Hz2Py(text3, ref text2);
			text3 = text3.Substring(num);
			text += text2;
		}
		if (string_0.EndsWith("传"))
		{
			text = text.Substring(0, text.Length - 5) + "zhuan";
		}
		return text.ToLower();
	}

	public static string GetFirstPinyin(string string_0)
	{
		if (string.IsNullOrEmpty(string_0))
		{
			return "";
		}
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in string_0)
			{
				stringBuilder.Append(FirstLetterOf(c));
			}
			return stringBuilder.ToString();
		}
		catch
		{
			return "";
		}
	}

	public static int Hz2Py(string string_0, ref string string_1)
	{
		if (hashtable_0.Contains(string_0))
		{
			string_1 = hashtable_0[string_0].ToString();
			return string_0.Length;
		}
		if (string_0.Length > 1)
		{
			return Hz2Py(string_0.Substring(0, string_0.Length - 1), ref string_1);
		}
		if (CHz2Py.string_0.Contains(string_0))
		{
			int num = CHz2Py.string_0.IndexOf(string_0, 0);
			string_1 = CHz2Py.string_1[num];
			return string_0.Length;
		}
		string_1 = FullPinyinOf(string_0[0]);
		return string_0.Length;
	}

	public static bool IsZB(string string_0)
	{
		if (string_0 != "6.6")
		{
			return false;
		}
		return true;
	}

	private static string FullPinyinOf(char char_0)
	{
		try
		{
			ChineseChar chineseChar = new ChineseChar(char_0);
			return StripPinyin(chineseChar.Pinyins[0]);
		}
		catch
		{
			return char_0.ToString();
		}
	}

	private static string FirstLetterOf(char char_0)
	{
		try
		{
			ChineseChar chineseChar = new ChineseChar(char_0);
			foreach (char c in chineseChar.Pinyins[0])
			{
				if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
				{
					return char.ToLower(c).ToString();
				}
			}
		}
		catch
		{
		}
		return char_0.ToString();
	}

	private static string StripPinyin(string string_0)
	{
		if (string.IsNullOrEmpty(string_0))
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in string_0)
		{
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
			{
				stringBuilder.Append(char.ToLower(c));
			}
		}
		return stringBuilder.ToString();
	}

	public static void smethod_0()
	{
		try
		{
			Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("pinyin.TXT");
			bool flag = false;
			string text = "";
			string text2 = "";
			string text3 = "";
			using (StreamReader streamReader = new StreamReader(manifestResourceStream))
			{
				while (!streamReader.EndOfStream)
				{
					text = "";
					text2 = "";
					text3 = "";
					text = streamReader.ReadLine();
					if (flag)
					{
						for (int i = 0; i < text.Length; i++)
						{
							if (text[i] > '\u007f')
							{
								text2 += text[i];
							}
							else
							{
								text3 += text[i];
							}
						}
						if (!hashtable_0.Contains(text2) && text2.Length > 1)
						{
							hashtable_0.Add(text2, text3);
						}
					}
					if (text.Contains("[Text]"))
					{
						flag = true;
					}
				}
			}
			bool_0 = true;
		}
		catch
		{
			bool_0 = false;
		}
	}

	private static void smethod_1()
	{
		string_0 = "无行万系说召盛厂塔种合乾家强区校略奇伯单都落级骑若红拂";
		string_1 = new string[27]
		{
			"wu", "xing", "wan", "xi", "shuo", "zhao", "sheng", "chang", "ta", "zhong",
			"he", "qian", "jia", "qiang", "qu", "xiao", "lue", "qi", "bo", "dan",
			"du", "luo", "ji", "qi", "ruo", "hong", "fu"
		};
		bool_0 = false;
		hashtable_0 = new Hashtable();
	}
}
