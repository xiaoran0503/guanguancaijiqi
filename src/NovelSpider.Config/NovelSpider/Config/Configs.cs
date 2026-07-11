using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace NovelSpider.Config;

public class Configs
{
	public static Version AssemblyVersion;

	public static string DisplayVersion;

	public static BaseConfigInfo BaseConfig;

	public static DateTime Build;

	public static bool CmdModel;

	public static string CmsName;

	public static string HaveFunction;

	public static bool IsDemo;

	public static string LoginPassword;

	public static string RuleVersion;

	public static TaskConfigInfo TaskConfig;

	public static Hashtable TaskNovelInfo;

	public static Guid UserID;

	static Configs()
	{
		smethod_0();
	}

	public static void LoadConfigs()
	{
		if (!File.Exists("BaseConfig.xml"))
		{
			ConfigFileManager.SaveConfig("BaseConfig.xml", BaseConfig);
		}
		else
		{
			BaseConfig = (BaseConfigInfo)ConfigFileManager.LoadConfig("BaseConfig.xml", BaseConfig);
		}
		BaseConfig.EnsureDefaults();
		if (!File.Exists("TaskConfig.xml"))
		{
			ConfigFileManager.SaveConfig("TaskConfig.xml", TaskConfig);
		}
		else
		{
			TaskConfig = (TaskConfigInfo)ConfigFileManager.LoadConfig("TaskConfig.xml", TaskConfig);
		}
		BaseConfig.CmsName = SupportedCms.NormalizeCmsName(BaseConfig.CmsName);
		CmsName = BaseConfig.CmsName;
	}

	private static void smethod_0()
	{
		BaseConfig = new BaseConfigInfo();
		TaskConfig = new TaskConfigInfo();
		RuleVersion = "5.1";
		LoginPassword = "";
		CmdModel = false;
		CmsName = "Cnend";
		HaveFunction = "ZhanQunPinyinDir中译英";
		UserID = Guid.NewGuid();
		AssemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
		DisplayVersion = "10.4.1-net10-test";
		TaskNovelInfo = new Hashtable();
		IsDemo = false;
		Build = new DateTime(2026, 7, 9);
	}
}







