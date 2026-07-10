using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace NovelSpider.Local.Jieqi;

internal static class CoverImageService
{
	public static bool HasUsableCover(Image image)
	{
		try
		{
			return image != null && image.Height > 5;
		}
		catch
		{
			return false;
		}
	}

	public static void SaveSmallCover(Image image, string directory, int articleId)
	{
		if (!HasUsableCover(image))
		{
			return;
		}
		Directory.CreateDirectory(directory);
		image.Save(Path.Combine(directory, articleId + "s.jpg"), ImageFormat.Jpeg);
	}
}
