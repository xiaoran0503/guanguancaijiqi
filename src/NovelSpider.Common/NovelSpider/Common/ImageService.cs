#nullable enable
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace NovelSpider.Common;

public static class ImageService
{
	public static bool HasUsableImage(Image image)
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

	public static void SaveJpeg(Image image, string path)
	{
		if (!HasUsableImage(image))
		{
			return;
		}

		string? directory = Path.GetDirectoryName(path);
		if (!string.IsNullOrEmpty(directory))
		{
			Directory.CreateDirectory(directory);
		}

		image.Save(path, ImageFormat.Jpeg);
	}

	public static void SaveSmallCover(Image image, string directory, int articleId)
	{
		SaveJpeg(image, Path.Combine(directory, articleId + "s.jpg"));
	}
}
