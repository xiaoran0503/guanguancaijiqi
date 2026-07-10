using System;
using System.Data.SQLite;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using NovelSpider.Config;

namespace NovelSpider.Common;

public class Keys
{
	public static string[] Text;

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

	public static void LoadText()
	{
		FileInfo fileInfo = new FileInfo("Key.License");
		if (fileInfo.Exists)
		{
			SQLiteConnection sQLiteConnection = new SQLiteConnection("Data Source=" + fileInfo.FullName);
			sQLiteConnection.SetPassword(Configs.LoginPassword);
			try
			{
				sQLiteConnection.Open();
			}
			catch
			{
				return;
			}
			SQLiteDataReader sQLiteDataReader = new SQLiteCommand(sQLiteConnection)
			{
				CommandText = "Select * From SerialNumer"
			}.ExecuteReader();
			if (sQLiteDataReader.Read())
			{
				Text = new string[21];
				Text[1] = sQLiteDataReader["k01"].ToString();
				Text[2] = sQLiteDataReader["k02"].ToString();
				Text[3] = sQLiteDataReader["k03"].ToString();
				Text[4] = sQLiteDataReader["k04"].ToString();
				Text[5] = sQLiteDataReader["k05"].ToString();
				Text[6] = sQLiteDataReader["k06"].ToString();
				Text[7] = sQLiteDataReader["k07"].ToString();
				Text[8] = sQLiteDataReader["k08"].ToString();
				Text[9] = sQLiteDataReader["k09"].ToString();
				Text[10] = sQLiteDataReader["k10"].ToString();
				Text[11] = sQLiteDataReader["k11"].ToString();
				Text[12] = sQLiteDataReader["k12"].ToString();
				Text[13] = sQLiteDataReader["k13"].ToString();
				Text[14] = sQLiteDataReader["k14"].ToString();
				Text[15] = sQLiteDataReader["k15"].ToString();
				Text[16] = sQLiteDataReader["k16"].ToString();
				Text[17] = sQLiteDataReader["k17"].ToString();
				Text[18] = sQLiteDataReader["k18"].ToString();
				Text[19] = sQLiteDataReader["k19"].ToString();
				Text[20] = sQLiteDataReader["k20"].ToString();
			}
			sQLiteDataReader.Close();
			sQLiteConnection.Close();
		}
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
	private static string ComputeMd5Hash(string input, string algorithm)
	{
		byte[] bytes = System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes(input));
		return Convert.ToHexString(bytes);
	}
}
