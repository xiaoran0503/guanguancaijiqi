using NovelSpider.Config;
using NovelSpider.Entity;

namespace NovelSpider.Local;

public interface ILocalProvider
{
	void ClearNovel(NovelInfo novelInfo_0);

	void CreateChapter(NovelInfo novelInfo_0);

	void CreateChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0);

	void CreateOPF(NovelInfo novelinfo);

	void CreateIndex(NovelInfo novelInfo_0, bool bool_0, bool bool_1, bool bool_2, bool bool_3, bool bool_4, bool bool_5, bool bool_6, bool bool_7, bool bool_8, bool bool_9, int delnum);

	void CreateSingleChapter(NovelInfo novelInfo_0);

	void CreateSingleChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, bool bool_0, int int_0, int int_1, string string_0, string string_1, string string_2);

	void CreateWapChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, bool bool_0, int int_0, int int_1, string string_0, string string_1, string string_2);

	void CreateNoWapChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0, bool bool_0, int int_0, int int_1, string string_0, string string_1, string string_2);

	void DeleteChapter(NovelInfo novelInfo_0, int int_0, int int_1, bool bool_0, bool bool_1);

	void DeleteVolume(NovelInfo novelInfo_0, int int_0);

	void DeteleNovel(int int_0);

	NovelInfo GetChapterInfo(NovelInfo novelInfo_0);

	ChapterInfo GetChapterInfo(int int_0, int int_1);

	ChapterInfo[] GetChapterList(int int_0);

	string GetChapterText(NovelInfo novelInfo_0, bool on);

	NovelInfo GetNovelInfo(NovelInfo novelInfo_0);

	NovelInfo GetNovelInfo(NovelInfo novelInfo_0, bool bool_0);

	NovelInfo[] GetNovelList(string string_0);

	ChapterInfo[] GetVolumeNameList(int int_0);

	ChapterInfo InsertChapter(NovelInfo novelInfo_0, TaskConfigInfo taskConfigInfo_0);

	ChapterInfo InsertChapterByOrder(NovelInfo novelInfo_0, TaskConfigInfo taskConfigInfo_0, int int_0);

	NovelInfo InsertNovel(NovelInfo novelInfo_0);

	int InsertVolume(NovelInfo novelInfo_0, string string_0);

	void PinyinHua(string string_0);

	void CreateTagTable();

	void UpdateChapter(NovelInfo novelInfo_0, ReplaceConfigInfo replaceConfigInfo_0);

	int[] UpdateChapterOrder(NovelInfo novelInfo_0, int int_0, int int_1);

	void UpdateLastChapter(NovelInfo novelInfo_0);

	void UpdateLastChapter(NovelInfo novelInfo_0, ChapterInfo chapterInfo_0);

	void UpdateNovel(NovelInfo novelInfo_0, bool bool_0, bool bool_1, bool bool_2, bool bool_3, bool bool_4, bool bool_5, bool bool_6);

	void UpdateVolume(NovelInfo novelInfo_0, int int_0, string string_0);
}
