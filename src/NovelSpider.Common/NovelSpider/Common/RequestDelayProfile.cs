using NovelSpider.Config;

namespace NovelSpider.Common;

public static class RequestDelayProfile
{
	public static (int Min, int Max) GetDelay(TaskConfigInfo taskConfig, RequestKind kind)
	{
		if (taskConfig == null)
		{
			return (0, 0);
		}
		return kind switch
		{
			RequestKind.List => Normalize(taskConfig.RequestListWaitMin, taskConfig.RequestListWaitMax),
			RequestKind.Novel => Normalize(taskConfig.RequestNovelWaitMin, taskConfig.RequestNovelWaitMax),
			RequestKind.Index => Normalize(taskConfig.RequestIndexWaitMin, taskConfig.RequestIndexWaitMax),
			RequestKind.Chapter => Normalize(taskConfig.RequestChapterWaitMin, taskConfig.RequestChapterWaitMax),
			_ => (0, 0)
		};
	}

	public static (int Min, int Max) Normalize(int min, int max)
	{
		min = min < 0 ? 0 : min;
		max = max < 0 ? 0 : max;
		return min <= max ? (min, max) : (max, min);
	}
}
