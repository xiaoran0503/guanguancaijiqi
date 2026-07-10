using System;
using System.Windows.Forms;
using NovelSpider.Common;

namespace NovelVip;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Net10RuntimeBootstrap.Initialize();
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new form1());
	}
}

