using System.Runtime.InteropServices;

namespace NovelSpider;

public class NativeMethods
{
	public static int BookCount;

	public static string BookMsg;

	public static int ChapterCount;

	static NativeMethods()
	{
		BookCount = 0;
		ChapterCount = 0;
		BookMsg = "";
	}

	[DllImport("kernel32.dll")]
	public static extern bool AllocConsole();

	[DllImport("kernel32.dll")]
	public static extern bool FreeConsole();
}
