using System.Threading;
using System.Threading.Tasks;
using NovelSpider.Config;
using NovelSpider.Entity;

namespace NovelSpider.Local;

public static class LocalProviderAsyncDispatcher
{
	public static Task<NovelInfo> InsertNovelAsync(ILocalProvider provider, NovelInfo novelInfo, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			return asyncProvider.InsertNovelAsync(novelInfo, cancellationToken);
		}
		return Task.FromResult(provider.InsertNovel(novelInfo));
	}

	public static Task<ChapterInfo> InsertChapterAsync(ILocalProvider provider, NovelInfo novelInfo, TaskConfigInfo taskConfigInfo, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			return asyncProvider.InsertChapterAsync(novelInfo, taskConfigInfo, cancellationToken);
		}
		return Task.FromResult(provider.InsertChapter(novelInfo, taskConfigInfo));
	}

	public static Task<ChapterInfo> InsertChapterByOrderAsync(ILocalProvider provider, NovelInfo novelInfo, TaskConfigInfo taskConfigInfo, int order, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			return asyncProvider.InsertChapterByOrderAsync(novelInfo, taskConfigInfo, order, cancellationToken);
		}
		return Task.FromResult(provider.InsertChapterByOrder(novelInfo, taskConfigInfo, order));
	}

	public static Task UpdateLastChapterAsync(ILocalProvider provider, NovelInfo novelInfo, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			return asyncProvider.UpdateLastChapterAsync(novelInfo, cancellationToken);
		}
		provider.UpdateLastChapter(novelInfo);
		return Task.CompletedTask;
	}

	public static Task UpdateLastChapterAsync(ILocalProvider provider, NovelInfo novelInfo, ChapterInfo chapterInfo, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			return asyncProvider.UpdateLastChapterAsync(novelInfo, chapterInfo, cancellationToken);
		}
		provider.UpdateLastChapter(novelInfo, chapterInfo);
		return Task.CompletedTask;
	}
}

