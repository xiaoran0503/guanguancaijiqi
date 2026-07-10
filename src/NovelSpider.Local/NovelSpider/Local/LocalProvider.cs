using System;
using NovelSpider.Common;
using NovelSpider.Config;

namespace NovelSpider.Local;

public class LocalProvider
{
	private static ILocalProvider ilocalProvider_0;

	private static object object_0;

	static LocalProvider()
	{
		object_0 = new object();
	}

	public static ILocalProvider GetInstance()
	{
		if (ilocalProvider_0 == null)
		{
			lock (object_0)
			{
				if (ilocalProvider_0 == null)
				{
					smethod_0();
				}
			}
		}
		return ilocalProvider_0;
	}

	public static void ResetProvider()
	{
		ilocalProvider_0 = null;
	}

	private static void smethod_0()
	{
		try
		{
			Configs.CmsName = CmsCompatibility.NormalizeCmsName(Configs.CmsName);
			SpiderException.Debug("动态载入DLL " + string.Format("NovelSpider.Local.{0}.LocalProvider,NovelSpider.Local.{0}", Configs.CmsName));
			Type providerType = Type.GetType(string.Format("NovelSpider.Local.{0}.LocalProvider,NovelSpider.Local.{0}", Configs.CmsName), throwOnError: false, ignoreCase: true);
			if (providerType == null)
			{
				throw new InvalidOperationException("当前版本仅支持 Jieqi CMS。");
			}
			ilocalProvider_0 = (ILocalProvider)Activator.CreateInstance(providerType);
		}
		catch (Exception ex)
		{
			throw new Exception("载入小说基本配置信息出错。" + ex.Message);
		}
	}
}
