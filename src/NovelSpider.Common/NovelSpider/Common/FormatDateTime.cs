using System;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using NovelSpider.Config;

namespace NovelSpider.Common;

public class FormatDateTime
{
	public static bool CheckHost()
	{
		return true;
	}

	public static string GetDiskInfo()
	{
		string text = string.Empty;
		using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_DiskDrive").GetInstances().GetEnumerator())
		{
			if (managementObjectEnumerator.MoveNext())
			{
				ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
				text = ComputeMd5Hash(managementObject.Properties["Model"].Value.ToString() + "@2288!ABC", "MD5");
			}
		}
		return text.Substring(8, 16);
	}

	public static string GetInfo()
	{
		string text = "";
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = new ManagementClass("Win32_Processor").GetInstances().GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
				text += ComputeMd5Hash(managementObject.Properties["ProcessorId"].Value.ToString() + "Qiwen", "MD5");
			}
		}
		catch
		{
			text = "@BF3CD3B458FC1FBD688F2E9137BBEACE139B646DED461D9";
		}
		string text2 = "";
		try
		{
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator2 = new ManagementClass("Win32_DiskDrive").GetInstances().GetEnumerator();
			if (managementObjectEnumerator2.MoveNext())
			{
				ManagementObject managementObject2 = (ManagementObject)managementObjectEnumerator2.Current;
				text2 += ComputeMd5Hash(managementObject2.Properties["Model"].Value.ToString() + "Jieqi", "MD5");
			}
		}
		catch
		{
			text2 = "@8182E42A3534AB268CA9573E4A66C17B85436F4BEC066FD";
		}
		string text3 = "";
		string text6;
		try
		{
			ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
			string text4 = "";
			foreach (ManagementObject item in instances)
			{
				if ((bool)item["IPEnabled"])
				{
					text4 = item["MacAddress"].ToString();
					break;
				}
			}
			string text5 = text4.Replace(":", "");
			text6 = text3 + ComputeMd5Hash(text5 + "Cnend", "MD5");
		}
		catch
		{
			text6 = text3 + ComputeMd5Hash("000000Cnend", "MD5");
		}
		return text + text2 + text6;
	}

	[DllImport("NovelSpider.Update.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
	public static extern string GetKeyPassWord(string string_0);

	public static bool IsDateTime(string string_0)
	{
		GetInfo();
		Guid.NewGuid();
		Configs.Build = new DateTime(2026, 7, 9);
		Configs.LoginPassword = Configs.LoginPassword.Split('_')[3];
		return true;
	}

	private static string smethod_0(string string_0)
	{
		string text = "";
		int length = string_0.Length;
		for (int i = 0; i < length / 8; i++)
		{
			string text2 = string_0.Substring(i * 8, 8);
			string text3 = ((i % 2 != 0) ? ComputeMd5Hash(text2 + DateTime.Today.Month, "MD5") : ComputeMd5Hash(text2 + DateTime.Now.ToString("yyyy-MM"), "MD5"));
			text += text3;
		}
		return text;
	}

	[DllImport("NovelSpider.Update.dll")]
	public static extern string Test(string string_0);

	public static void Today()
	{
		try
		{
			File.WriteAllText("Key.Machine", GetInfo());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "验证错误");
		}
	}
	private static string ComputeMd5Hash(string input, string algorithm)
	{
		byte[] bytes = System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes(input));
		return Convert.ToHexString(bytes);
	}
}
