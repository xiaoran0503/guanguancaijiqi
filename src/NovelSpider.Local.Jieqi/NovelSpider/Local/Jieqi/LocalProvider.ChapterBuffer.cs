using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NovelSpider.Config;
using NovelSpider.Entity;

namespace NovelSpider.Local.Jieqi;

public partial class LocalProvider
{
	private sealed class BufferedChapterEntry
	{
		public long Id { get; set; }
		public int ArticleId { get; set; }
		public string NovelName { get; set; } = string.Empty;
		public string ChapterName { get; set; } = string.Empty;
		public string VolumeName { get; set; } = string.Empty;
		public string ChapterText { get; set; } = string.Empty;
		public string ChapterUrl { get; set; } = string.Empty;
		public string GetId { get; set; } = string.Empty;
		public int ItemIndex { get; set; }
		public int InsertOrder { get; set; }
		public bool ByOrder { get; set; }
	}

	private static string ChapterBufferDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NovelSpider", "ChapterWriteBuffer");

	private static string ChapterBufferPath => Path.Combine(ChapterBufferDirectory, "jieqi-chapter-buffer.db3");

	public async Task BeginBookSessionAsync(NovelInfo novelInfo, TaskConfigInfo taskConfigInfo, CancellationToken cancellationToken = default)
	{
		if (!IsChapterBufferEnabled(taskConfigInfo) || novelInfo == null || novelInfo.PutID <= 0)
		{
			return;
		}
		await ChapterBufferGate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			EnsureChapterBufferStore();
			currentBufferedArticleId = novelInfo.PutID;
			currentBufferedNovelName = novelInfo.Name ?? string.Empty;
		}
		finally
		{
			ChapterBufferGate.Release();
		}
	}

	public async Task FlushBookSessionAsync(NovelInfo novelInfo, TaskConfigInfo taskConfigInfo, CancellationToken cancellationToken = default)
	{
		if (novelInfo == null || novelInfo.PutID <= 0)
		{
			return;
		}
		await FlushBufferedArticleAsync(novelInfo.PutID, taskConfigInfo, cancellationToken).ConfigureAwait(false);
	}

	public async Task FlushAllBookSessionsAsync(CancellationToken cancellationToken = default)
	{
		EnsureChapterBufferStore();
		foreach (int articleId in LoadPendingArticleIds())
		{
			await FlushBufferedArticleAsync(articleId, new TaskConfigInfo(), cancellationToken).ConfigureAwait(false);
		}
	}

	public Task RetryPendingSessionsAsync(CancellationToken cancellationToken = default)
	{
		return FlushAllBookSessionsAsync(cancellationToken);
	}

	public BookChapterBufferStatus GetBufferStatus()
	{
		EnsureChapterBufferStore();
		return new BookChapterBufferStatus
		{
			Enabled = true,
			CurrentArticleId = currentBufferedArticleId,
			CurrentNovelName = currentBufferedNovelName,
			PendingChapterCount = CountPendingChapters(currentBufferedArticleId),
			DatabasePath = ChapterBufferPath,
			LastError = chapterBufferLastError,
			LastFlushTime = chapterBufferLastFlush
		};
	}

	private static bool IsChapterBufferEnabled(TaskConfigInfo taskConfigInfo)
	{
		return taskConfigInfo == null || taskConfigInfo.ChapterWriteBufferEnabled;
	}

	private async Task<ChapterInfo> BufferChapterAsync(NovelInfo novelInfo, TaskConfigInfo taskConfigInfo, int insertOrder, bool byOrder, CancellationToken cancellationToken)
	{
		if (!IsChapterBufferEnabled(taskConfigInfo) || novelInfo?.LastChapter == null || novelInfo.PutID <= 0)
		{
			return byOrder ? InsertChapterByOrder(novelInfo, taskConfigInfo, insertOrder) : InsertChapter(novelInfo, taskConfigInfo);
		}
		await ChapterBufferGate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			EnsureChapterBufferStore();
			currentBufferedArticleId = novelInfo.PutID;
			currentBufferedNovelName = novelInfo.Name ?? string.Empty;
			using SQLiteConnection connection = OpenChapterBufferConnection();
			using SQLiteCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO PendingChapter (ArticleId,NovelName,ChapterName,VolumeName,ChapterText,ChapterUrl,GetId,ItemIndex,InsertOrder,ByOrder,Status,CreatedUtc) VALUES (@ArticleId,@NovelName,@ChapterName,@VolumeName,@ChapterText,@ChapterUrl,@GetId,@ItemIndex,@InsertOrder,@ByOrder,0,@CreatedUtc)";
			command.Parameters.AddWithValue("@ArticleId", novelInfo.PutID);
			command.Parameters.AddWithValue("@NovelName", novelInfo.Name ?? string.Empty);
			command.Parameters.AddWithValue("@ChapterName", novelInfo.LastChapter.ChapterName ?? string.Empty);
			command.Parameters.AddWithValue("@VolumeName", novelInfo.LastChapter.VolumeName ?? string.Empty);
			command.Parameters.AddWithValue("@ChapterText", novelInfo.LastChapter.ChapterText ?? string.Empty);
			command.Parameters.AddWithValue("@ChapterUrl", novelInfo.LastChapter.ChapterUrl?.AbsoluteUri ?? string.Empty);
			command.Parameters.AddWithValue("@GetId", novelInfo.LastChapter.GetID ?? string.Empty);
			command.Parameters.AddWithValue("@ItemIndex", novelInfo.LastChapter.ItemIndex);
			command.Parameters.AddWithValue("@InsertOrder", insertOrder);
			command.Parameters.AddWithValue("@ByOrder", byOrder ? 1 : 0);
			command.Parameters.AddWithValue("@CreatedUtc", DateTime.UtcNow.ToString("O"));
			command.ExecuteNonQuery();
			return novelInfo.LastChapter;
		}
		finally
		{
			ChapterBufferGate.Release();
		}
	}

	private async Task FlushBufferedArticleAsync(int articleId, TaskConfigInfo taskConfigInfo, CancellationToken cancellationToken)
	{
		await ChapterBufferGate.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			EnsureChapterBufferStore();
			List<BufferedChapterEntry> entries = LoadPendingChapters(articleId, taskConfigInfo?.ChapterWriteBufferBatchSize ?? 100);
			if (entries.Count == 0)
			{
				return;
			}
			NovelInfo novel = new NovelInfo { PutID = articleId, Name = entries[0].NovelName };
			foreach (BufferedChapterEntry entry in entries)
			{
				cancellationToken.ThrowIfCancellationRequested();
				novel.LastChapter = new ChapterInfo
				{
					ChapterName = entry.ChapterName,
					VolumeName = entry.VolumeName,
					ChapterText = entry.ChapterText,
					GetID = entry.GetId,
					ItemIndex = entry.ItemIndex
				};
				if (Uri.TryCreate(entry.ChapterUrl, UriKind.Absolute, out Uri chapterUrl))
				{
					novel.LastChapter.ChapterUrl = chapterUrl;
				}
				if (entry.ByOrder)
				{
					InsertChapterByOrder(novel, taskConfigInfo ?? new TaskConfigInfo(), entry.InsertOrder);
				}
				else
				{
					InsertChapter(novel, taskConfigInfo ?? new TaskConfigInfo());
				}
				MarkBufferedChapterDone(entry.Id);
			}
			chapterBufferLastFlush = DateTime.Now;
			chapterBufferLastError = string.Empty;
		}
		catch (Exception ex)
		{
			chapterBufferLastError = ex.Message;
			throw;
		}
		finally
		{
			ChapterBufferGate.Release();
		}
	}

	private static void EnsureChapterBufferStore()
	{
		Directory.CreateDirectory(ChapterBufferDirectory);
		using SQLiteConnection connection = OpenChapterBufferConnection();
		using SQLiteCommand pragma = connection.CreateCommand();
		pragma.CommandText = "PRAGMA journal_mode=WAL;";
		pragma.ExecuteNonQuery();
		using SQLiteCommand command = connection.CreateCommand();
		command.CommandText = "CREATE TABLE IF NOT EXISTS PendingChapter (Id INTEGER PRIMARY KEY AUTOINCREMENT, ArticleId INTEGER NOT NULL, NovelName TEXT NOT NULL, ChapterName TEXT NOT NULL, VolumeName TEXT NOT NULL, ChapterText TEXT NOT NULL, ChapterUrl TEXT NOT NULL, GetId TEXT NOT NULL, ItemIndex INTEGER NOT NULL, InsertOrder INTEGER NOT NULL, ByOrder INTEGER NOT NULL, Status INTEGER NOT NULL, Error TEXT NOT NULL DEFAULT '', CreatedUtc TEXT NOT NULL, LastRetryUtc TEXT NOT NULL DEFAULT '')";
		command.ExecuteNonQuery();
	}

	private static SQLiteConnection OpenChapterBufferConnection()
	{
		SQLiteConnection connection = new SQLiteConnection("Data Source=" + ChapterBufferPath + ";Version=3;");
		connection.Open();
		return connection;
	}

	private static List<int> LoadPendingArticleIds()
	{
		List<int> result = new List<int>();
		using SQLiteConnection connection = OpenChapterBufferConnection();
		using SQLiteCommand command = connection.CreateCommand();
		command.CommandText = "SELECT DISTINCT ArticleId FROM PendingChapter WHERE Status=0 ORDER BY ArticleId";
		using SQLiteDataReader reader = command.ExecuteReader();
		while (reader.Read())
		{
			result.Add(Convert.ToInt32(reader["ArticleId"]));
		}
		return result;
	}

	private static List<BufferedChapterEntry> LoadPendingChapters(int articleId, int limit)
	{
		List<BufferedChapterEntry> result = new List<BufferedChapterEntry>();
		using SQLiteConnection connection = OpenChapterBufferConnection();
		using SQLiteCommand command = connection.CreateCommand();
		command.CommandText = "SELECT * FROM PendingChapter WHERE Status=0 AND ArticleId=@ArticleId ORDER BY Id LIMIT @Limit";
		command.Parameters.AddWithValue("@ArticleId", articleId);
		command.Parameters.AddWithValue("@Limit", Math.Max(1, limit));
		using SQLiteDataReader reader = command.ExecuteReader();
		while (reader.Read())
		{
			result.Add(new BufferedChapterEntry
			{
				Id = Convert.ToInt64(reader["Id"]),
				ArticleId = Convert.ToInt32(reader["ArticleId"]),
				NovelName = Convert.ToString(reader["NovelName"]),
				ChapterName = Convert.ToString(reader["ChapterName"]),
				VolumeName = Convert.ToString(reader["VolumeName"]),
				ChapterText = Convert.ToString(reader["ChapterText"]),
				ChapterUrl = Convert.ToString(reader["ChapterUrl"]),
				GetId = Convert.ToString(reader["GetId"]),
				ItemIndex = Convert.ToInt32(reader["ItemIndex"]),
				InsertOrder = Convert.ToInt32(reader["InsertOrder"]),
				ByOrder = Convert.ToInt32(reader["ByOrder"]) != 0
			});
		}
		return result;
	}

	private static void MarkBufferedChapterDone(long id)
	{
		using SQLiteConnection connection = OpenChapterBufferConnection();
		using SQLiteCommand command = connection.CreateCommand();
		command.CommandText = "DELETE FROM PendingChapter WHERE Id=@Id";
		command.Parameters.AddWithValue("@Id", id);
		command.ExecuteNonQuery();
		CompactChapterBufferIfEmpty(connection);
	}

	private static void CompactChapterBufferIfEmpty(SQLiteConnection connection)
	{
		using SQLiteCommand count = connection.CreateCommand();
		count.CommandText = "SELECT COUNT(*) FROM PendingChapter WHERE Status=0";
		if (Convert.ToInt32(count.ExecuteScalar()) != 0)
		{
			return;
		}
		using SQLiteCommand checkpoint = connection.CreateCommand();
		checkpoint.CommandText = "PRAGMA wal_checkpoint(TRUNCATE);";
		checkpoint.ExecuteNonQuery();
		FileInfo file = new FileInfo(ChapterBufferPath);
		if (file.Exists && file.Length > 16L * 1024L * 1024L)
		{
			using SQLiteCommand vacuum = connection.CreateCommand();
			vacuum.CommandText = "VACUUM;";
			vacuum.ExecuteNonQuery();
		}
	}

	private static int CountPendingChapters(int articleId)
	{
		using SQLiteConnection connection = OpenChapterBufferConnection();
		using SQLiteCommand command = connection.CreateCommand();
		command.CommandText = articleId > 0 ? "SELECT COUNT(*) FROM PendingChapter WHERE Status=0 AND ArticleId=@ArticleId" : "SELECT COUNT(*) FROM PendingChapter WHERE Status=0";
		if (articleId > 0)
		{
			command.Parameters.AddWithValue("@ArticleId", articleId);
		}
		return Convert.ToInt32(command.ExecuteScalar());
	}
}
