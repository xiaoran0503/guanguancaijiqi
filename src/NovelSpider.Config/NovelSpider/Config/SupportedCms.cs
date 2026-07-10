namespace NovelSpider.Config;

public static class SupportedCms
{
	public const string SupportedCmsName = "Jieqi";

	public static bool IsSupported(string cmsName)
	{
		return string.Equals(cmsName, SupportedCmsName, System.StringComparison.OrdinalIgnoreCase);
	}

	public static string NormalizeCmsName(string cmsName)
	{
		if (IsSupported(cmsName))
		{
			return SupportedCmsName;
		}
		return SupportedCmsName;
	}
}
