using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace NovelSpider.Common;

public class GifPalette
{
	[StructLayout(LayoutKind.Explicit)]
	public struct Color32
	{
		[FieldOffset(3)]
		public byte Alpha;

		[FieldOffset(0)]
		public int ARGB;

		[FieldOffset(0)]
		public byte Blue;

		[FieldOffset(1)]
		public byte Green;

		[FieldOffset(2)]
		public byte Red;

		public Color Color => Color.FromArgb(Alpha, Red, Green, Blue);
	}

	private static ArrayList arrayList_0;

	public bool AutoWaterMark;

	public bool ClearWaterMark;

	private Color[] color_0;

	private Hashtable hashtable_0;

	public int[] WaterMarkColors;

	public GifPalette()
	{
		ArrayList arrayList = SetPalette();
		hashtable_0 = new Hashtable();
		color_0 = new Color[arrayList.Count];
		arrayList.CopyTo(color_0);
	}

	public GifPalette(ArrayList arrayList_0)
	{
		hashtable_0 = new Hashtable();
		color_0 = new Color[arrayList_0.Count];
		arrayList_0.CopyTo(color_0);
	}

	private ColorPalette method_0(ColorPalette colorPalette_0)
	{
		for (int i = 0; i < color_0.Length; i++)
		{
			colorPalette_0.Entries[i] = color_0[i];
		}
		return colorPalette_0;
	}

	private void method_1(BitmapData bitmapData_0, Bitmap bitmap_0, int int_0, int int_1, Rectangle rectangle_0)
	{
		BitmapData bitmapdata = null;
		try
		{
		}
		finally
		{
			bitmap_0.UnlockBits(bitmapdata);
		}
	}

	private byte method_2(Color32 color32_0)
	{
		return 0;
	}

	private bool method_3(int int_0, int int_1, int int_2)
	{
		if (int_0 > 235 && int_1 > 235 && int_2 > 235)
		{
			return true;
		}
		if (ClearWaterMark && WaterMarkColors != null)
		{
			int[] waterMarkColors = WaterMarkColors;
			foreach (int argb in waterMarkColors)
			{
				Color color = Color.FromArgb(argb);
				if (int_0 == color.R && int_1 == color.G && int_2 == color.B)
				{
					return true;
				}
			}
		}
		return false;
	}

	public Bitmap Quantize(Image image_0)
	{
		int height = image_0.Height;
		int width = image_0.Width;
		Rectangle rectangle = new Rectangle(0, 0, width, height);
		Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
		Bitmap bitmap2 = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			graphics.PageUnit = GraphicsUnit.Pixel;
			graphics.DrawImageUnscaled(image_0, rectangle);
		}
		BitmapData bitmapData = null;
		try
		{
			bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			bitmap2.Palette = method_0(bitmap2.Palette);
			method_1(bitmapData, bitmap2, width, height, rectangle);
			return bitmap2;
		}
		finally
		{
			bitmap.UnlockBits(bitmapData);
		}
	}

	public static ArrayList SetPalette()
	{
		if (arrayList_0 == null)
		{
			arrayList_0 = new ArrayList();
			arrayList_0.Add(Color.FromArgb(255, 255, 255, 255));
			arrayList_0.Add(Color.FromArgb(255, 0, 0, 0));
			arrayList_0.Add(Color.FromArgb(255, 51, 51, 51));
			arrayList_0.Add(Color.FromArgb(255, 102, 102, 102));
			arrayList_0.Add(Color.FromArgb(255, 153, 153, 153));
			arrayList_0.Add(Color.FromArgb(255, 204, 204, 204));
		}
		return arrayList_0;
	}
}
