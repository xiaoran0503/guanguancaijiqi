using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace NovelSpider.Common;

public class CenterPalette
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

	private Color[] color_0;

	private Hashtable hashtable_0;

	public CenterPalette()
	{
		ArrayList arrayList = SetPalette();
		hashtable_0 = new Hashtable();
		color_0 = new Color[arrayList.Count];
		arrayList.CopyTo(color_0);
	}

	public CenterPalette(ArrayList arrayList_0)
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

	private int[] method_1(BitmapData bitmapData_0, Bitmap bitmap_0, int int_0, int int_1, Rectangle rectangle_0)
	{
		BitmapData bitmapdata = null;
		ArrayList arrayList = new ArrayList();
		try
		{
		}
		finally
		{
			bitmap_0.UnlockBits(bitmapdata);
		}
		return (int[])arrayList.ToArray(typeof(int));
	}

	private byte method_2(Color32 color32_0)
	{
		return 0;
	}

	private bool method_3(int int_0, int int_1, int int_2)
	{
		if ((int_0 != int_1 || int_0 != int_2 || int_0 != 0) && (int_0 != int_1 || int_0 != int_2 || int_0 != 51) && (int_0 != int_1 || int_0 != int_2 || int_0 != 102))
		{
			if (int_0 == int_1 && int_0 == int_2)
			{
				return int_0 != 153;
			}
			return true;
		}
		return false;
	}

	public int[] Quantize(Image image_0)
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
			return method_1(bitmapData, bitmap2, width, height, rectangle);
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
			arrayList_0.Add(Color.FromArgb(255, 0, 0, 0));
			arrayList_0.Add(Color.FromArgb(255, 128, 0, 0));
			arrayList_0.Add(Color.FromArgb(255, 0, 128, 0));
			arrayList_0.Add(Color.FromArgb(255, 128, 128, 0));
			arrayList_0.Add(Color.FromArgb(255, 0, 0, 128));
			arrayList_0.Add(Color.FromArgb(255, 128, 0, 128));
			arrayList_0.Add(Color.FromArgb(255, 0, 128, 128));
			arrayList_0.Add(Color.FromArgb(255, 192, 192, 192));
			arrayList_0.Add(Color.FromArgb(255, 192, 220, 192));
			arrayList_0.Add(Color.FromArgb(255, 166, 202, 240));
			arrayList_0.Add(Color.FromArgb(255, 1, 25, 83));
			arrayList_0.Add(Color.FromArgb(255, 1, 37, 92));
			arrayList_0.Add(Color.FromArgb(255, 2, 51, 103));
			arrayList_0.Add(Color.FromArgb(255, 18, 66, 114));
			arrayList_0.Add(Color.FromArgb(255, 39, 78, 123));
			arrayList_0.Add(Color.FromArgb(255, 101, 63, 107));
			arrayList_0.Add(Color.FromArgb(255, 72, 92, 119));
			arrayList_0.Add(Color.FromArgb(255, 89, 74, 121));
			arrayList_0.Add(Color.FromArgb(255, 85, 101, 122));
			arrayList_0.Add(Color.FromArgb(255, 122, 89, 127));
			arrayList_0.Add(Color.FromArgb(255, 101, 108, 106));
			arrayList_0.Add(Color.FromArgb(255, 111, 116, 111));
			arrayList_0.Add(Color.FromArgb(255, 109, 118, 122));
			arrayList_0.Add(Color.FromArgb(255, 120, 119, 97));
			arrayList_0.Add(Color.FromArgb(255, 121, 124, 114));
			arrayList_0.Add(Color.FromArgb(255, 1, 52, 154));
			arrayList_0.Add(Color.FromArgb(255, 16, 61, 156));
			arrayList_0.Add(Color.FromArgb(255, 15, 63, 160));
			arrayList_0.Add(Color.FromArgb(255, 37, 55, 131));
			arrayList_0.Add(Color.FromArgb(255, 24, 69, 158));
			arrayList_0.Add(Color.FromArgb(255, 21, 68, 162));
			arrayList_0.Add(Color.FromArgb(255, 35, 71, 137));
			arrayList_0.Add(Color.FromArgb(255, 33, 71, 152));
			arrayList_0.Add(Color.FromArgb(255, 43, 93, 130));
			arrayList_0.Add(Color.FromArgb(255, 51, 68, 139));
			arrayList_0.Add(Color.FromArgb(255, 48, 79, 159));
			arrayList_0.Add(Color.FromArgb(255, 53, 85, 131));
			arrayList_0.Add(Color.FromArgb(255, 49, 81, 151));
			arrayList_0.Add(Color.FromArgb(255, 34, 78, 167));
			arrayList_0.Add(Color.FromArgb(255, 41, 84, 170));
			arrayList_0.Add(Color.FromArgb(255, 50, 91, 173));
			arrayList_0.Add(Color.FromArgb(255, 54, 94, 176));
			arrayList_0.Add(Color.FromArgb(255, 53, 101, 136));
			arrayList_0.Add(Color.FromArgb(255, 60, 97, 168));
			arrayList_0.Add(Color.FromArgb(255, 59, 99, 177));
			arrayList_0.Add(Color.FromArgb(255, 68, 93, 134));
			arrayList_0.Add(Color.FromArgb(255, 67, 91, 150));
			arrayList_0.Add(Color.FromArgb(255, 86, 93, 151));
			arrayList_0.Add(Color.FromArgb(255, 64, 102, 142));
			arrayList_0.Add(Color.FromArgb(255, 76, 105, 153));
			arrayList_0.Add(Color.FromArgb(255, 75, 116, 150));
			arrayList_0.Add(Color.FromArgb(255, 81, 103, 140));
			arrayList_0.Add(Color.FromArgb(255, 84, 106, 147));
			arrayList_0.Add(Color.FromArgb(255, 91, 113, 146));
			arrayList_0.Add(Color.FromArgb(255, 75, 107, 172));
			arrayList_0.Add(Color.FromArgb(255, 65, 103, 180));
			arrayList_0.Add(Color.FromArgb(255, 77, 113, 184));
			arrayList_0.Add(Color.FromArgb(255, 90, 104, 162));
			arrayList_0.Add(Color.FromArgb(255, 90, 115, 160));
			arrayList_0.Add(Color.FromArgb(255, 90, 123, 189));
			arrayList_0.Add(Color.FromArgb(255, 101, 87, 130));
			arrayList_0.Add(Color.FromArgb(255, 106, 108, 158));
			arrayList_0.Add(Color.FromArgb(255, 101, 115, 130));
			arrayList_0.Add(Color.FromArgb(255, 103, 121, 149));
			arrayList_0.Add(Color.FromArgb(255, 112, 99, 139));
			arrayList_0.Add(Color.FromArgb(255, 122, 110, 148));
			arrayList_0.Add(Color.FromArgb(255, 101, 122, 165));
			arrayList_0.Add(Color.FromArgb(255, 116, 124, 172));
			arrayList_0.Add(Color.FromArgb(255, 93, 126, 192));
			arrayList_0.Add(Color.FromArgb(255, 96, 127, 192));
			arrayList_0.Add(Color.FromArgb(0, 72, 254, 42));
			arrayList_0.Add(Color.FromArgb(255, 90, 128, 160));
			arrayList_0.Add(Color.FromArgb(255, 97, 130, 159));
			arrayList_0.Add(Color.FromArgb(255, 119, 129, 138));
			arrayList_0.Add(Color.FromArgb(255, 118, 133, 154));
			arrayList_0.Add(Color.FromArgb(255, 107, 131, 169));
			arrayList_0.Add(Color.FromArgb(255, 105, 132, 186));
			arrayList_0.Add(Color.FromArgb(255, 118, 138, 170));
			arrayList_0.Add(Color.FromArgb(255, 117, 137, 180));
			arrayList_0.Add(Color.FromArgb(255, 118, 145, 173));
			arrayList_0.Add(Color.FromArgb(255, 124, 152, 183));
			arrayList_0.Add(Color.FromArgb(255, 95, 128, 192));
			arrayList_0.Add(Color.FromArgb(255, 102, 133, 195));
			arrayList_0.Add(Color.FromArgb(255, 112, 141, 199));
			arrayList_0.Add(Color.FromArgb(255, 120, 147, 202));
			arrayList_0.Add(Color.FromArgb(255, 154, 53, 53));
			arrayList_0.Add(Color.FromArgb(255, 131, 72, 91));
			arrayList_0.Add(Color.FromArgb(255, 143, 87, 104));
			arrayList_0.Add(Color.FromArgb(255, 129, 123, 92));
			arrayList_0.Add(Color.FromArgb(255, 156, 124, 68));
			arrayList_0.Add(Color.FromArgb(255, 129, 126, 101));
			arrayList_0.Add(Color.FromArgb(255, 154, 105, 120));
			arrayList_0.Add(Color.FromArgb(255, 169, 83, 82));
			arrayList_0.Add(Color.FromArgb(255, 165, 125, 70));
			arrayList_0.Add(Color.FromArgb(255, 160, 125, 80));
			arrayList_0.Add(Color.FromArgb(255, 176, 109, 114));
			arrayList_0.Add(Color.FromArgb(255, 205, 53, 2));
			arrayList_0.Add(Color.FromArgb(255, 209, 67, 19));
			arrayList_0.Add(Color.FromArgb(255, 210, 85, 47));
			arrayList_0.Add(Color.FromArgb(255, 230, 67, 21));
			arrayList_0.Add(Color.FromArgb(255, 243, 88, 46));
			arrayList_0.Add(Color.FromArgb(255, 255, 97, 53));
			arrayList_0.Add(Color.FromArgb(255, 201, 92, 71));
			arrayList_0.Add(Color.FromArgb(255, 214, 107, 79));
			arrayList_0.Add(Color.FromArgb(255, 205, 117, 105));
			arrayList_0.Add(Color.FromArgb(255, 245, 112, 74));
			arrayList_0.Add(Color.FromArgb(255, 131, 101, 136));
			arrayList_0.Add(Color.FromArgb(255, 139, 110, 144));
			arrayList_0.Add(Color.FromArgb(255, 130, 120, 156));
			arrayList_0.Add(Color.FromArgb(255, 146, 124, 155));
			arrayList_0.Add(Color.FromArgb(255, 162, 117, 131));
			arrayList_0.Add(Color.FromArgb(255, 141, 133, 94));
			arrayList_0.Add(Color.FromArgb(255, 156, 131, 72));
			arrayList_0.Add(Color.FromArgb(255, 151, 133, 83));
			arrayList_0.Add(Color.FromArgb(255, 157, 144, 92));
			arrayList_0.Add(Color.FromArgb(255, 137, 132, 102));
			arrayList_0.Add(Color.FromArgb(255, 135, 136, 120));
			arrayList_0.Add(Color.FromArgb(255, 147, 139, 102));
			arrayList_0.Add(Color.FromArgb(255, 148, 143, 115));
			arrayList_0.Add(Color.FromArgb(255, 156, 146, 106));
			arrayList_0.Add(Color.FromArgb(255, 148, 145, 122));
			arrayList_0.Add(Color.FromArgb(255, 168, 136, 73));
			arrayList_0.Add(Color.FromArgb(255, 168, 138, 88));
			arrayList_0.Add(Color.FromArgb(255, 172, 147, 90));
			arrayList_0.Add(Color.FromArgb(255, 178, 138, 82));
			arrayList_0.Add(Color.FromArgb(255, 186, 153, 69));
			arrayList_0.Add(Color.FromArgb(255, 179, 150, 91));
			arrayList_0.Add(Color.FromArgb(255, 174, 139, 100));
			arrayList_0.Add(Color.FromArgb(255, 166, 154, 107));
			arrayList_0.Add(Color.FromArgb(255, 161, 151, 114));
			arrayList_0.Add(Color.FromArgb(255, 182, 154, 101));
			arrayList_0.Add(Color.FromArgb(255, 190, 162, 81));
			arrayList_0.Add(Color.FromArgb(255, 172, 160, 117));
			arrayList_0.Add(Color.FromArgb(255, 183, 161, 103));
			arrayList_0.Add(Color.FromArgb(255, 182, 163, 119));
			arrayList_0.Add(Color.FromArgb(255, 205, 168, 63));
			arrayList_0.Add(Color.FromArgb(255, 218, 174, 52));
			arrayList_0.Add(Color.FromArgb(255, 221, 177, 53));
			arrayList_0.Add(Color.FromArgb(255, 255, 154, 1));
			arrayList_0.Add(Color.FromArgb(255, 255, 161, 18));
			arrayList_0.Add(Color.FromArgb(255, 235, 184, 44));
			arrayList_0.Add(Color.FromArgb(255, 228, 182, 52));
			arrayList_0.Add(Color.FromArgb(255, 247, 190, 36));
			arrayList_0.Add(Color.FromArgb(255, 200, 155, 94));
			arrayList_0.Add(Color.FromArgb(255, 192, 153, 104));
			arrayList_0.Add(Color.FromArgb(255, 223, 130, 103));
			arrayList_0.Add(Color.FromArgb(255, 215, 132, 118));
			arrayList_0.Add(Color.FromArgb(255, 201, 167, 68));
			arrayList_0.Add(Color.FromArgb(255, 196, 167, 87));
			arrayList_0.Add(Color.FromArgb(255, 209, 173, 70));
			arrayList_0.Add(Color.FromArgb(255, 212, 169, 91));
			arrayList_0.Add(Color.FromArgb(255, 213, 177, 73));
			arrayList_0.Add(Color.FromArgb(255, 198, 166, 102));
			arrayList_0.Add(Color.FromArgb(255, 196, 168, 123));
			arrayList_0.Add(Color.FromArgb(255, 219, 172, 112));
			arrayList_0.Add(Color.FromArgb(255, 219, 183, 106));
			arrayList_0.Add(Color.FromArgb(255, 217, 185, 115));
			arrayList_0.Add(Color.FromArgb(255, 255, 131, 91));
			arrayList_0.Add(Color.FromArgb(255, 249, 143, 109));
			arrayList_0.Add(Color.FromArgb(255, 227, 186, 108));
			arrayList_0.Add(Color.FromArgb(255, 229, 186, 112));
			arrayList_0.Add(Color.FromArgb(255, 255, 164, 123));
			arrayList_0.Add(Color.FromArgb(255, 207, 195, 127));
			arrayList_0.Add(Color.FromArgb(255, 253, 204, 94));
			arrayList_0.Add(Color.FromArgb(255, 235, 194, 108));
			arrayList_0.Add(Color.FromArgb(255, 233, 197, 117));
			arrayList_0.Add(Color.FromArgb(255, 252, 204, 104));
			arrayList_0.Add(Color.FromArgb(255, 249, 204, 115));
			arrayList_0.Add(Color.FromArgb(255, 251, 208, 106));
			arrayList_0.Add(Color.FromArgb(255, 253, 209, 117));
			arrayList_0.Add(Color.FromArgb(255, 128, 137, 140));
			arrayList_0.Add(Color.FromArgb(255, 136, 144, 145));
			arrayList_0.Add(Color.FromArgb(255, 151, 150, 130));
			arrayList_0.Add(Color.FromArgb(255, 144, 149, 148));
			arrayList_0.Add(Color.FromArgb(255, 137, 131, 165));
			arrayList_0.Add(Color.FromArgb(255, 137, 140, 180));
			arrayList_0.Add(Color.FromArgb(255, 140, 154, 168));
			arrayList_0.Add(Color.FromArgb(255, 133, 151, 180));
			arrayList_0.Add(Color.FromArgb(255, 149, 137, 167));
			arrayList_0.Add(Color.FromArgb(255, 149, 152, 188));
			arrayList_0.Add(Color.FromArgb(255, 138, 168, 188));
			arrayList_0.Add(Color.FromArgb(255, 175, 135, 148));
			arrayList_0.Add(Color.FromArgb(255, 169, 154, 129));
			arrayList_0.Add(Color.FromArgb(255, 187, 135, 134));
			arrayList_0.Add(Color.FromArgb(255, 178, 138, 148));
			arrayList_0.Add(Color.FromArgb(255, 184, 146, 156));
			arrayList_0.Add(Color.FromArgb(255, 168, 149, 173));
			arrayList_0.Add(Color.FromArgb(255, 170, 163, 131));
			arrayList_0.Add(Color.FromArgb(255, 184, 165, 128));
			arrayList_0.Add(Color.FromArgb(255, 181, 166, 157));
			arrayList_0.Add(Color.FromArgb(255, 162, 175, 186));
			arrayList_0.Add(Color.FromArgb(255, 189, 185, 170));
			arrayList_0.Add(Color.FromArgb(255, 130, 155, 206));
			arrayList_0.Add(Color.FromArgb(255, 134, 158, 208));
			arrayList_0.Add(Color.FromArgb(255, 146, 155, 196));
			arrayList_0.Add(Color.FromArgb(255, 142, 169, 193));
			arrayList_0.Add(Color.FromArgb(255, 140, 163, 209));
			arrayList_0.Add(Color.FromArgb(255, 153, 168, 199));
			arrayList_0.Add(Color.FromArgb(255, 148, 170, 213));
			arrayList_0.Add(Color.FromArgb(255, 157, 177, 216));
			arrayList_0.Add(Color.FromArgb(255, 164, 166, 193));
			arrayList_0.Add(Color.FromArgb(255, 171, 179, 198));
			arrayList_0.Add(Color.FromArgb(255, 167, 185, 220));
			arrayList_0.Add(Color.FromArgb(255, 181, 179, 205));
			arrayList_0.Add(Color.FromArgb(255, 181, 188, 211));
			arrayList_0.Add(Color.FromArgb(255, 173, 190, 224));
			arrayList_0.Add(Color.FromArgb(255, 178, 196, 217));
			arrayList_0.Add(Color.FromArgb(255, 174, 192, 224));
			arrayList_0.Add(Color.FromArgb(255, 174, 212, 224));
			arrayList_0.Add(Color.FromArgb(255, 184, 198, 227));
			arrayList_0.Add(Color.FromArgb(255, 185, 218, 229));
			arrayList_0.Add(Color.FromArgb(255, 202, 145, 142));
			arrayList_0.Add(Color.FromArgb(255, 196, 172, 134));
			arrayList_0.Add(Color.FromArgb(255, 203, 161, 158));
			arrayList_0.Add(Color.FromArgb(255, 203, 177, 134));
			arrayList_0.Add(Color.FromArgb(255, 207, 185, 151));
			arrayList_0.Add(Color.FromArgb(255, 210, 186, 132));
			arrayList_0.Add(Color.FromArgb(255, 215, 189, 148));
			arrayList_0.Add(Color.FromArgb(255, 213, 168, 167));
			arrayList_0.Add(Color.FromArgb(255, 234, 155, 132));
			arrayList_0.Add(Color.FromArgb(255, 253, 172, 140));
			arrayList_0.Add(Color.FromArgb(255, 193, 191, 193));
			arrayList_0.Add(Color.FromArgb(255, 206, 194, 135));
			arrayList_0.Add(Color.FromArgb(255, 214, 198, 136));
			arrayList_0.Add(Color.FromArgb(255, 219, 204, 145));
			arrayList_0.Add(Color.FromArgb(255, 218, 209, 141));
			arrayList_0.Add(Color.FromArgb(255, 215, 209, 154));
			arrayList_0.Add(Color.FromArgb(255, 231, 196, 135));
			arrayList_0.Add(Color.FromArgb(255, 225, 213, 143));
			arrayList_0.Add(Color.FromArgb(255, 231, 217, 148));
			arrayList_0.Add(Color.FromArgb(255, 251, 212, 129));
			arrayList_0.Add(Color.FromArgb(255, 255, 199, 172));
			arrayList_0.Add(Color.FromArgb(255, 202, 205, 219));
			arrayList_0.Add(Color.FromArgb(255, 193, 205, 230));
			arrayList_0.Add(Color.FromArgb(255, 200, 211, 233));
			arrayList_0.Add(Color.FromArgb(255, 211, 220, 237));
			arrayList_0.Add(Color.FromArgb(255, 213, 222, 240));
			arrayList_0.Add(Color.FromArgb(255, 201, 226, 228));
			arrayList_0.Add(Color.FromArgb(255, 201, 231, 242));
			arrayList_0.Add(Color.FromArgb(255, 216, 227, 232));
			arrayList_0.Add(Color.FromArgb(255, 219, 227, 241));
			arrayList_0.Add(Color.FromArgb(255, 228, 233, 237));
			arrayList_0.Add(Color.FromArgb(255, 229, 234, 244));
			arrayList_0.Add(Color.FromArgb(255, 236, 241, 248));
			arrayList_0.Add(Color.FromArgb(255, 240, 239, 243));
			arrayList_0.Add(Color.FromArgb(255, 253, 253, 254));
			arrayList_0.Add(Color.FromArgb(255, 255, 251, 240));
			arrayList_0.Add(Color.FromArgb(255, 160, 160, 164));
			arrayList_0.Add(Color.FromArgb(255, 128, 128, 128));
			arrayList_0.Add(Color.FromArgb(255, 255, 0, 0));
			arrayList_0.Add(Color.FromArgb(255, 0, 255, 0));
			arrayList_0.Add(Color.FromArgb(255, 255, 255, 0));
			arrayList_0.Add(Color.FromArgb(255, 0, 0, 255));
			arrayList_0.Add(Color.FromArgb(255, 255, 0, 255));
			arrayList_0.Add(Color.FromArgb(255, 0, 255, 255));
			arrayList_0.Add(Color.FromArgb(255, 255, 255, 255));
		}
		return arrayList_0;
	}
}
