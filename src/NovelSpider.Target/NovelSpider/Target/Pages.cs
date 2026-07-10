using NovelSpider.Config;

namespace NovelSpider.Target;

public class Pages
{
	public static Page GetInstance(RuleConfigInfo ruleConfigInfo_0, TaskConfigInfo taskConfigInfo_0)
	{
		return new Page(ruleConfigInfo_0, taskConfigInfo_0);
	}
}
