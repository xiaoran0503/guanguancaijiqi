using NovelSpider.Common;
using NovelSpider.Config;

namespace NovelSpider.Local.Qiwen;

public static class Config
{
	public static XmlConfig BookInfo;

	public static string ConnectionString;

	public static XmlConfig SysInfo;

	public static string TempletsContent;

	public static string TempletsList;

	public static string TempletsListCommon;

	public static string TempletsVolume;

	public static XmlConfig UrlInfo;

	public static string WaterSoftIISID;

	public static string WaterSoftPath;

	static Config()
	{
		BookInfo = null;
		ConnectionString = "Server=(local);User id=sa;Pwd=;Database=WanerSoft2007";
		SysInfo = null;
		TempletsVolume = "";
		TempletsList = "";
		TempletsListCommon = "";
		UrlInfo = null;
		WaterSoftIISID = "1";
		WaterSoftPath = "I:\\200708\\Waner2007";
	}

	public static void LoadConfig()
	{
		SpiderException.Debug("Qiwen LoadConfig");
		WaterSoftPath = Configs.BaseConfig.WebSitePath;
		ConnectionString = Configs.BaseConfig.ConnectionString;
		TempletsVolume = Configs.BaseConfig.DefaultVolumeName;
	}

	private static void smethod_0()
	{
		WaterSoftPath = "I:\\200708\\Waner2007";
		WaterSoftIISID = "1";
		ConnectionString = "Server=(local);User id=sa;Pwd=;Database=WanerSoft2007";
		BookInfo = null;
		SysInfo = null;
		UrlInfo = null;
		TempletsList = "";
		TempletsListCommon = "";
		TempletsVolume = "";
	}
}
