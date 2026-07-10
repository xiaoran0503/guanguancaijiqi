using System;
using System.Windows.Forms;
using NovelSpider.Common;
using NovelSpider.Config;

namespace NovelAdmin;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Net10RuntimeBootstrap.Initialize();
		Configs.LoadConfigs();
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new 数据管理台());
	}
}

