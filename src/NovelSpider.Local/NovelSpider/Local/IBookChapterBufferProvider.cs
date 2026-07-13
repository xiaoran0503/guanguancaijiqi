using System.Threading;
using System.Threading.Tasks;
using NovelSpider.Config;
using NovelSpider.Entity;

namespace NovelSpider.Local;

public interface IBookChapterBufferProvider
{
	Task BeginBookSessionAsync(NovelInfo novelInfo, TaskConfigInfo taskConfigInfo, CancellationToken cancellationToken = default);
	Task FlushBookSessionAsync(NovelInfo novelInfo, TaskConfigInfo taskConfigInfo, CancellationToken cancellationToken = default);
	Task FlushAllBookSessionsAsync(CancellationToken cancellationToken = default);
	Task RetryPendingSessionsAsync(CancellationToken cancellationToken = default);
	BookChapterBufferStatus GetBufferStatus();
}
