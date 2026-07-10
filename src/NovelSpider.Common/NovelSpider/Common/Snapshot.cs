using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace NovelSpider.Common;

public class Snapshot
{
	public Bitmap TakeSnapshot(object pUnknown, Rectangle bmpRect, int xWidth, int yHeight)
	{
		if (pUnknown == null)
		{
			return null;
		}
		if (!Marshal.IsComObject(pUnknown))
		{
			return null;
		}
		IntPtr ppv = IntPtr.Zero;
		Bitmap bitmap = new Bitmap(bmpRect.Width, bmpRect.Height);
		Graphics graphics = Graphics.FromImage(bitmap);
		Marshal.QueryInterface(Marshal.GetIUnknownForObject(pUnknown), in Class1.GrmiApou0, out ppv);
		try
		{
			(Marshal.GetTypedObjectForIUnknown(ppv, typeof(Class1.Interface0)) as Class1.Interface0).imethod_0(1, -1, IntPtr.Zero, null, IntPtr.Zero, graphics.GetHdc(), new Class2.Class4(bmpRect), null, IntPtr.Zero, 0);
			Marshal.Release(ppv);
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			throw;
		}
		graphics.Dispose();
		Rectangle srcRect = new Rectangle(0, 0, xWidth, yHeight);
		Bitmap bitmap2 = new Bitmap(xWidth, yHeight);
		using (Graphics graphics2 = Graphics.FromImage(bitmap2))
		{
			graphics2.Clear(Color.White);
			graphics2.SmoothingMode = SmoothingMode.HighQuality;
			graphics2.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics2.DrawImage(bitmap, new Rectangle(0, 0, xWidth, yHeight), srcRect, GraphicsUnit.Pixel);
		}
		bitmap.Dispose();
		return bitmap2;
	}
}



