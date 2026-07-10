using System.Collections;
using System.IO;
using System.Reflection;
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
		string text = string.Empty;
		for (int i = 0; i < string_0.Length; i++)
		{
			char ch = string_0[i];
			try
			{
				ChineseChar chineseChar = new ChineseChar(ch);
				string text2 = chineseChar.Pinyins[0].ToString();
				text += text2.Substring(0, 1);
			}
			catch
			{
				text += ch;
			}
		}
		return text;
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
		ChineseChar chineseChar = new ChineseChar(string_0[0]);
		string_1 = chineseChar.Pinyins[0].ToString();
		string_1 = string_1.Substring(0, string_1.Length - 1);
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
