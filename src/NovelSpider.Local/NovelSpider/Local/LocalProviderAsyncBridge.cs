using System.Threading;
using NovelSpider.Config;
using NovelSpider.Entity;

namespace NovelSpider.Local;

public static class LocalProviderAsyncBridge
{
	public static NovelInfo InsertNovel(ILocalProvider provider, NovelInfo novelInfo, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			return asyncProvider.InsertNovelAsync(novelInfo, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		return provider.InsertNovel(novelInfo);
	}

	public static ChapterInfo InsertChapter(ILocalProvider provider, NovelInfo novelInfo, TaskConfigInfo taskConfigInfo, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			return asyncProvider.InsertChapterAsync(novelInfo, taskConfigInfo, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		return provider.InsertChapter(novelInfo, taskConfigInfo);
	}

	public static ChapterInfo InsertChapterByOrder(ILocalProvider provider, NovelInfo novelInfo, TaskConfigInfo taskConfigInfo, int order, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			return asyncProvider.InsertChapterByOrderAsync(novelInfo, taskConfigInfo, order, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
		}
		return provider.InsertChapterByOrder(novelInfo, taskConfigInfo, order);
	}

	public static void UpdateLastChapter(ILocalProvider provider, NovelInfo novelInfo, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			asyncProvider.UpdateLastChapterAsync(novelInfo, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
			return;
		}
		provider.UpdateLastChapter(novelInfo);
	}

	public static void UpdateLastChapter(ILocalProvider provider, NovelInfo novelInfo, ChapterInfo chapterInfo, CancellationToken cancellationToken = default)
	{
		if (provider is IAsyncLocalProvider asyncProvider)
		{
			asyncProvider.UpdateLastChapterAsync(novelInfo, chapterInfo, cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();
			return;
		}
		provider.UpdateLastChapter(novelInfo, chapterInfo);
	}
}