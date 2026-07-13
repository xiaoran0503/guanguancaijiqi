#nullable enable
using System.Drawing;
using NovelSpider.Common;

namespace NovelSpider.Local.Jieqi;

internal static class CoverImageService
{
	public static bool HasUsableCover(Image image)
	{
		return ImageService.HasUsableImage(image);
	}

	public static void SaveSmallCover(Image image, string directory, int articleId)
	{
		ImageService.SaveSmallCover(image, directory, articleId);
	}
}
