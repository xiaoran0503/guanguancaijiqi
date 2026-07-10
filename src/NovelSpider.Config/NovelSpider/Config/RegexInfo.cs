using System.Text.RegularExpressions;

namespace NovelSpider.Config;

public class RegexInfo
{
	public string FilterPattern;

	public string Method;

	public RegexOptions Options;

	public string Pattern;

	public string RegexName;

	public RegexInfo()
	{
		FilterPattern = "";
		Method = "Match";
		Pattern = "";
		RegexName = "";
	}
}
