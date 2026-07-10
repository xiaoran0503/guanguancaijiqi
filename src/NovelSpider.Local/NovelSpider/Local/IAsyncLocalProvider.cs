using System.Threading;
using System.Threading.Tasks;
using NovelSpider.Config;
using NovelSpider.Entity;

namespace NovelSpider.Local;

public interface IAsyncLocalProvider
{
	Task<ChapterInfo> InsertChapterAsync(NovelInfo novelInfo_0, TaskConfigInfo taskConfigInfo_0, CancellationToken cancellationToken = default);

	Task<ChapterInfo> InsertChapterByOrderAsync(NovelInfo novelInfo_0, TaskConfigInfo taskConfigInfo_0, int int_0, CancellationToken cancellationToken = default);

	Task<NovelInfo> InsertNovelAsync(NovelInfo novelInfo_0, CancellationToken cancellationToken = default);

	Task UpdateLastChapterAsync(NovelInfo novelInfo_0, CancellationToken cancellationToken = default);

	Task UpdateLastChapterAsync(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, CancellationToken cancellationToken = default);
}