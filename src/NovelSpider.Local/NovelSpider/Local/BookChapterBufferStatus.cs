using System;

namespace NovelSpider.Local;

public sealed class BookChapterBufferStatus
{
	public bool Enabled { get; set; }
	public int CurrentArticleId { get; set; }
	public string CurrentNovelName { get; set; } = string.Empty;
	public int PendingChapterCount { get; set; }
	public string DatabasePath { get; set; } = string.Empty;
	public string LastError { get; set; } = string.Empty;
	public DateTime LastFlushTime { get; set; }
}
