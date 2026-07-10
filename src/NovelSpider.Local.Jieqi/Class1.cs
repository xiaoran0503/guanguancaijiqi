using System.Collections.Generic;
using System.Text.RegularExpressions;

internal class Class1
{
	public static string smethod_0(string string_0)
	{
		string_0 = string_0.Replace("〇", "零").Replace("壹", "一").Replace("贰", "二")
			.Replace("叁", "三")
			.Replace("肆", "四")
			.Replace("伍十", "五十")
			.Replace("伍百", "五百")
			.Replace("伍千", "五千")
			.Replace("陆十", "六十")
			.Replace("陆百", "六百")
			.Replace("陆千", "六千")
			.Replace("柒", "七")
			.Replace("捌", "八")
			.Replace("玖", "九")
			.Replace("拾", "十")
			.Replace("佰", "百")
			.Replace("仟", "千")
			.Replace("两百", "二百")
			.Replace("两千", "二千")
			.Replace("０", "0")
			.Replace("１", "1")
			.Replace("２", "2")
			.Replace("３", "3")
			.Replace("４", "4")
			.Replace("５", "5")
			.Replace("６", "6")
			.Replace("７", "7")
			.Replace("８", "8")
			.Replace("９", "9")
			.Replace("二选一", "");
		string[] array = new string[10] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
		string[] array2 = new string[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
		for (int i = 0; i < array.Length; i++)
		{
			if (string_0.IndexOf("千") < 1 && string_0.IndexOf("百") < 1 && string_0.IndexOf("十") < 1)
			{
				string_0 = string_0.Replace(array[i], array2[i]);
			}
		}
		string_0 = Regex.Replace(string_0, "(?<![零一二三四五六七八九])十", "一十");
		string_0 = Regex.Replace(string_0, "十(?![零一二三四五六七八九])", "十零");
		return Regex.Replace(string_0, "(?![千百])[零一二三四五六七八九十百千]+", smethod_1);
	}

	private static string smethod_1(Match match_0)
	{
		string value = match_0.Value;
		int num = 0;
		int num2 = -1;
		int num3 = -1;
		List<string> list = new List<string>(new string[10] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" });
		List<string> list2 = new List<string>(new string[3] { "十", "百", "千" });
		List<int> list3 = new List<int>(new int[3] { 10, 100, 1000 });
		for (int i = 0; i < value.Length; i++)
		{
			string item = value[i].ToString();
			if (list.IndexOf(item) > -1)
			{
				num3 = list.IndexOf(item);
			}
			if (list2.IndexOf(item) > -1)
			{
				if (list2.IndexOf(item) < num2)
				{
					num += num3 * list3[list2.IndexOf(item)];
				}
				else
				{
					num = (num + num3) * list3[list2.IndexOf(item)];
					num2 = list2.IndexOf(item);
				}
				num3 = -1;
			}
			if (i == value.Length - 1 && num3 != -1)
			{
				num += num3;
			}
		}
		return num.ToString();
	}
}
