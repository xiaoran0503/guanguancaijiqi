using System.Drawing;
using System.Windows.Forms;

namespace NovelSpider;

internal static class AppIconProvider
{
	private static Icon icon;

	public static Icon Icon
	{
		get
		{
			if (icon == null)
			{
				icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath) ?? SystemIcons.Application;
			}
			return icon;
		}
	}
}
