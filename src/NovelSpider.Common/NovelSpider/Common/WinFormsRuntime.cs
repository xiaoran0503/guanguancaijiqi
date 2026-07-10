using System.Windows.Forms;

namespace NovelSpider.Common;

public static class WinFormsRuntime
{
	public static void SetClipboardText(string text)
	{
		Clipboard.SetText(text ?? string.Empty, TextDataFormat.UnicodeText);
	}
}