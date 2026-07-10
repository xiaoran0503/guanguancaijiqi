using System;
using System.IO;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;

namespace NovelSpider;

internal static class Program
{
	[STAThread]
	private static void Main(string[] args)
	{
		// 授权验证已移除 - 全功能开放
		try
		{
			NetworkCompatibility.Initialize();
			if (!Directory.Exists("Log"))
			{
				Directory.CreateDirectory("Log");
			}
			Configs.LoadConfigs();
			Configs.BaseConfig.LicenseAd = "";
			Configs.BaseConfig.LicenseOk = true;
			Configs.BaseConfig.LicenseVip = true;
			Configs.BaseConfig.LicenseTime = DateTime.MaxValue;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "错误提示");
			return;
		}
		try
		{
			NativeMethods.FreeConsole();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Application.Run(new MdiForm());
		}
		finally
		{
			NativeMethods.FreeConsole();
		}
	}
}
