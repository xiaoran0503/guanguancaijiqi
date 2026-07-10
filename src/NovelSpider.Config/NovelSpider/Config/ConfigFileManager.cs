using System;
using System.IO;
using System.Xml.Serialization;

namespace NovelSpider.Config;

public class ConfigFileManager
{
	private static IConfigInfo iconfigInfo_0;

	private static object object_0;

	private static string string_0;

	static ConfigFileManager()
	{
		object_0 = new object();
	}

	public static IConfigInfo DeserializeInfo(string string_0, Type type_0)
	{
		if (string.IsNullOrWhiteSpace(string_0))
		{
			throw new ArgumentException("配置文件路径不能为空。", nameof(string_0));
		}
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(string_0, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			return (IConfigInfo)new XmlSerializer(type_0).Deserialize(fileStream);
		}
		catch
		{
			throw;
		}
		finally
		{
			fileStream?.Close();
		}
	}

	public static IConfigInfo LoadConfig(string string_0, IConfigInfo iconfigInfo_0)
	{
		if (string.IsNullOrWhiteSpace(string_0) || !File.Exists(string_0))
		{
			return iconfigInfo_0;
		}
		ConfigFileManager.string_0 = string_0;
		ConfigFileManager.iconfigInfo_0 = iconfigInfo_0;
		lock (object_0)
		{
			ConfigFileManager.iconfigInfo_0 = DeserializeInfo(string_0, iconfigInfo_0.GetType());
		}
		return ConfigFileManager.iconfigInfo_0;
	}

	public static bool SaveConfig(string string_0, IConfigInfo iconfigInfo_0)
	{
		FileStream fileStream = null;
		try
		{
			fileStream = new FileStream(string_0, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
			new XmlSerializer(iconfigInfo_0.GetType()).Serialize(fileStream, iconfigInfo_0);
			return true;
		}
		catch
		{
			throw;
		}
		finally
		{
			fileStream?.Close();
		}
	}
}


