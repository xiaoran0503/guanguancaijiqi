using System.Drawing;
using System.Runtime.InteropServices;

internal static class Class2
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class Class3
	{
		[MarshalAs(UnmanagedType.U4)]
		public int int_0;

		[MarshalAs(UnmanagedType.U2)]
		public short short_0;

		[MarshalAs(UnmanagedType.U2)]
		public short short_1;

		[MarshalAs(UnmanagedType.U2)]
		public short short_2;

		[MarshalAs(UnmanagedType.U2)]
		public short short_3;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class Class4
	{
		public int int_0;

		public int int_1;

		public int int_2;

		public int int_3;

		public Class4()
		{
		}

		public Class4(Rectangle rectangle_0)
		{
			int_0 = rectangle_0.X;
			int_1 = rectangle_0.Y;
			int_2 = rectangle_0.Right;
			int_3 = rectangle_0.Bottom;
		}

		public Class4(int int_4, int int_5, int int_6, int int_7)
		{
			int_0 = int_4;
			int_1 = int_5;
			int_2 = int_6;
			int_3 = int_7;
		}

		public static Class4 smethod_0(int int_4, int int_5, int int_6, int int_7)
		{
			return new Class4(int_4, int_5, int_4 + int_6, int_5 + int_7);
		}

		public override string ToString()
		{
			return "Left = " + int_0 + " Top " + int_1 + " Right = " + int_2 + " Bottom = " + int_3;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public sealed class Class5
	{
		[MarshalAs(UnmanagedType.U2)]
		public short short_0;

		[MarshalAs(UnmanagedType.U2)]
		public short short_1;
	}
}
