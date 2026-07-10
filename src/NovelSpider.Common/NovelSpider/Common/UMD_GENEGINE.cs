using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace NovelSpider.Common;

public class UMD_GENEGINE
{
	private struct Struct0
	{
		public byte byte_0;

		public ushort ushort_0;

		public ushort ushort_1;

		public uint uint_0;

		public byte[] byte_1;
	}

	private ArrayList arrayList_0;

	private ArrayList arrayList_1;

	private ArrayList arrayList_2;

	private ArrayList arrayList_3;

	private byte byte_0;

	private byte byte_1;

	private byte byte_2;

	private byte[] byte_3;

	private byte[][] byte_4;

	private int[] int_0;

	private int int_1;

	private int int_2;

	private short short_0;

	private string string_0;

	private string string_1;

	private string string_10;

	private string string_2;

	private string string_3;

	private string string_4;

	private string string_5;

	private string string_6;

	private string string_7;

	private string string_8;

	private string string_9;

	public UMD_GENEGINE()
	{
		arrayList_0 = new ArrayList();
		arrayList_1 = new ArrayList();
		arrayList_2 = new ArrayList();
		arrayList_3 = new ArrayList();
		byte_0 = 204;
		byte_1 = 172;
		byte_2 = 166;
		short_0 = 0;
	}

	public bool Initialize(string string_11, string string_12, string string_13, string string_14, string string_15, string string_16, string string_17, string string_18, string string_19, int int_3, string string_20, ref ArrayList arrayList_4, ref ArrayList arrayList_5, out string string_21)
	{
		string_21 = "true";
		if (string_11.Length <= 0)
		{
			string_21 = "标题不能为空";
			return false;
		}
		if (string_12.Length <= 0)
		{
			string_21 = "作者不能为空";
			return false;
		}
		if (string_13.Length > 4 || string_14.Length > 2 || string_15.Length > 2)
		{
			string_21 = "日期非法";
			return false;
		}
		if (arrayList_4.Count <= 0)
		{
			string_21 = "内容数量不能小于0";
			return false;
		}
		if (arrayList_4.Count != arrayList_5.Count)
		{
			string_21 = "章节标题数量和章节内容数量不符";
			return false;
		}
		if (!Directory.Exists(string_20))
		{
			Directory.CreateDirectory(string_20);
		}
		string_9 = string_11;
		string_0 = string_12;
		string_2 = string_13;
		string_6 = string_14;
		string_4 = string_15;
		string_5 = string_16;
		string_7 = string_17;
		string_10 = string_18;
		int_1 = int_3;
		string_3 = string_19;
		string_8 = string_20;
		for (int i = 0; i < arrayList_4.Count; i++)
		{
			arrayList_1.Add((string)arrayList_4[i] + "\u2029");
			arrayList_0.Add((string)arrayList_5[i]);
		}
		int_0 = new int[arrayList_1.Count];
		return true;
	}

	private void lWyIquqUn9()
	{
		arrayList_3.Clear();
		for (int i = 16; i < 17; i++)
		{
			string path = Path.Combine(AppContext.BaseDirectory, "FontWidthData", "sunfon.s" + i + ".wdt");
			if (!File.Exists(path))
			{
				continue;
			}
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
			{
				ushort ushort_ = binaryReader.ReadUInt16();
				ushort ushort_2 = binaryReader.ReadUInt16();
				uint num = binaryReader.ReadUInt16();
				byte[] array = new byte[num];
				for (uint num2 = 0u; num2 < num; num2++)
				{
					array[(int)(IntPtr)num2] = binaryReader.ReadByte();
				}
				Struct0 @struct = new Struct0
				{
					byte_0 = (byte)i,
					ushort_0 = ushort_,
					ushort_1 = ushort_2,
					uint_0 = num,
					byte_1 = array
				};
				arrayList_3.Add(@struct);
			}
			binaryReader.Close();
			fileStream.Close();
		}
	}

	public bool Make(ref ProgressBar progressBar_0, out string string_11, int int_3)
	{
		method_6();
		Random random = new Random();
		if (string_8.EndsWith("\\"))
		{
			string_8.Remove(string_8.Length - 1, 1);
		}
		string path = string_8 + "\\" + int_3 + ".umd";
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
		BinaryWriter binaryWriter_ = new BinaryWriter(fileStream);
		byte b = Encoding.Unicode.GetBytes((string)arrayList_1[0])[0];
		binaryWriter_.Write(3734674313u);
		binaryWriter_.Write('#');
		binaryWriter_.Write(1);
		binaryWriter_.Write(0);
		binaryWriter_.Write(8);
		binaryWriter_.Write(1);
		binaryWriter_.Write(short_0);
		binaryWriter_.Write('#');
		binaryWriter_.Write(2);
		binaryWriter_.Write(0);
		binaryWriter_.Write((byte)(5 + string_9.Length * 2));
		binaryWriter_.Write(Encoding.Unicode.GetBytes(string_9));
		binaryWriter_.Write('#');
		binaryWriter_.Write(3);
		binaryWriter_.Write(0);
		binaryWriter_.Write((byte)(5 + string_0.Length * 2));
		binaryWriter_.Write(Encoding.Unicode.GetBytes(string_0));
		if (string_2.Length > 0)
		{
			binaryWriter_.Write('#');
			binaryWriter_.Write(4);
			binaryWriter_.Write(0);
			binaryWriter_.Write((byte)(5 + string_2.Length * 2));
			binaryWriter_.Write(Encoding.Unicode.GetBytes(string_2));
		}
		if (string_6.Length > 0)
		{
			binaryWriter_.Write('#');
			binaryWriter_.Write(5);
			binaryWriter_.Write(0);
			binaryWriter_.Write((byte)(5 + string_6.Length * 2));
			binaryWriter_.Write(Encoding.Unicode.GetBytes(string_6));
		}
		if (string_4.Length > 0)
		{
			binaryWriter_.Write('#');
			binaryWriter_.Write(6);
			binaryWriter_.Write(0);
			binaryWriter_.Write((byte)(5 + string_4.Length * 2));
			binaryWriter_.Write(Encoding.Unicode.GetBytes(string_4));
		}
		if (string_5.Length > 0)
		{
			binaryWriter_.Write('#');
			binaryWriter_.Write(7);
			binaryWriter_.Write(0);
			binaryWriter_.Write((byte)(5 + string_5.Length * 2));
			binaryWriter_.Write(Encoding.Unicode.GetBytes(string_5));
		}
		if (string_7.Length > 0)
		{
			binaryWriter_.Write('#');
			binaryWriter_.Write(8);
			binaryWriter_.Write(0);
			binaryWriter_.Write((byte)(5 + string_7.Length * 2));
			binaryWriter_.Write(Encoding.Unicode.GetBytes(string_7));
		}
		if (string_10.Length > 0)
		{
			binaryWriter_.Write('#');
			binaryWriter_.Write(9);
			binaryWriter_.Write(0);
			binaryWriter_.Write((byte)(5 + string_10.Length * 2));
			binaryWriter_.Write(Encoding.Unicode.GetBytes(string_10));
		}
		binaryWriter_.Write('#');
		binaryWriter_.Write(11);
		binaryWriter_.Write(0);
		binaryWriter_.Write(9);
		binaryWriter_.Write(byte_3.Length);
		uint value = (uint)(12288 + random.Next(4095));
		binaryWriter_.Write('#');
		binaryWriter_.Write(131);
		binaryWriter_.Write(1);
		binaryWriter_.Write(9);
		binaryWriter_.Write(value);
		binaryWriter_.Write('$');
		binaryWriter_.Write(value);
		binaryWriter_.Write((uint)(9 + int_0.Length * 4));
		int[] array = int_0;
		foreach (int value2 in array)
		{
			binaryWriter_.Write(value2);
		}
		uint value3 = (uint)(16384 + random.Next(4095));
		binaryWriter_.Write('#');
		binaryWriter_.Write(132);
		binaryWriter_.Write(1);
		binaryWriter_.Write(9);
		binaryWriter_.Write(value3);
		int num = 0;
		foreach (object item in arrayList_0)
		{
			num += ((string)item).Length * 2 + 1;
		}
		binaryWriter_.Write('$');
		binaryWriter_.Write(value3);
		binaryWriter_.Write((uint)(9 + num));
		foreach (object item2 in arrayList_0)
		{
			binaryWriter_.Write((byte)(((string)item2).Length * 2));
			binaryWriter_.Write(Encoding.Unicode.GetBytes((string)item2));
		}
		int num2 = 0;
		int num3 = 0;
		uint[] array2 = new uint[byte_4.Length];
		if (byte_4.Length > 1)
		{
			num2 = random.Next(0, byte_4.Length - 1);
			num3 = random.Next(0, byte_4.Length - 1);
		}
		for (int j = 0; j < byte_4.Length; j++)
		{
			array2[j] = (uint)(random.Next(1, 268435455) * -1);
			binaryWriter_.Write('$');
			binaryWriter_.Write(array2[j]);
			binaryWriter_.Write((uint)(9 + byte_4[j].Length));
			binaryWriter_.Write(byte_4[j]);
			if (j == num2)
			{
				binaryWriter_.Write('#');
				binaryWriter_.Write(241);
				binaryWriter_.Write(0);
				binaryWriter_.Write(21);
				binaryWriter_.Write(Encoding.ASCII.GetBytes("\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0"));
			}
			if (j == num3)
			{
				binaryWriter_.Write('#');
				binaryWriter_.Write(10);
				binaryWriter_.Write(0);
				binaryWriter_.Write(9);
				binaryWriter_.Write(int_1);
			}
		}
		uint value4 = (uint)(8192 + random.Next(4095));
		binaryWriter_.Write('#');
		binaryWriter_.Write(129);
		binaryWriter_.Write(1);
		binaryWriter_.Write(9);
		binaryWriter_.Write(value4);
		binaryWriter_.Write('$');
		binaryWriter_.Write(value4);
		binaryWriter_.Write((uint)(9 + array2.Length * 4));
		uint[] array3 = array2;
		foreach (uint value5 in array3)
		{
			binaryWriter_.Write(value5);
		}
		if (File.Exists(string_3))
		{
			FileStream fileStream2 = new FileStream(string_3, FileMode.Open);
			byte[] array4 = new byte[fileStream2.Length];
			fileStream2.Read(array4, 0, (int)fileStream2.Length);
			fileStream2.Close();
			uint value6 = (uint)(4096 + random.Next(4095));
			binaryWriter_.Write('#');
			binaryWriter_.Write(130);
			binaryWriter_.Write(1);
			binaryWriter_.Write(10);
			binaryWriter_.Write(1);
			binaryWriter_.Write(value6);
			binaryWriter_.Write('$');
			binaryWriter_.Write(value6);
			binaryWriter_.Write((uint)(9 + array4.Length));
			binaryWriter_.Write(array4);
		}
		method_2(16, byte_0, ref progressBar_0, out var uint_);
		method_7(16, (byte)(byte_0 + 4), ref uint_, ref binaryWriter_, 1);
		uint_.Initialize();
		method_2(16, byte_1, ref progressBar_0, out uint_);
		method_7(16, (byte)(byte_1 + 4), ref uint_, ref binaryWriter_, 1);
		uint_.Initialize();
		method_2(12, byte_0, ref progressBar_0, out uint_);
		method_7(12, (byte)(byte_0 + 4), ref uint_, ref binaryWriter_, 1);
		uint_.Initialize();
		method_2(12, byte_1, ref progressBar_0, out uint_);
		method_7(12, (byte)(byte_1 + 4), ref uint_, ref binaryWriter_, 1);
		uint_.Initialize();
		method_3(10, byte_2, ref progressBar_0, out uint_);
		method_7(10, byte_2, ref uint_, ref binaryWriter_, 5);
		binaryWriter_.Write('#');
		binaryWriter_.Write(12);
		binaryWriter_.Write(1);
		binaryWriter_.Write(9);
		binaryWriter_.Write((uint)((int)binaryWriter_.BaseStream.Position + 4));
		binaryWriter_.Close();
		fileStream.Close();
		string_11 = "true";
		return true;
	}

	private byte method_0(string string_11, byte byte_5)
	{
		ushort num = string_11[0];
		for (int i = 0; i < arrayList_2.Count; i++)
		{
			Struct0 @struct = (Struct0)arrayList_2[i];
			if (@struct.byte_0 == byte_5 && num >= @struct.ushort_0 && num <= @struct.ushort_1)
			{
				if (@struct.uint_0 == 1)
				{
					return @struct.byte_1[0];
				}
				return @struct.byte_1[num - @struct.ushort_0];
			}
		}
		return byte_5;
	}

	private byte method_1(string string_11, byte byte_5)
	{
		ushort num = string_11[0];
		for (int i = 0; i < arrayList_3.Count; i++)
		{
			Struct0 @struct = (Struct0)arrayList_3[i];
			if (@struct.byte_0 == byte_5 && num >= @struct.ushort_0 && num <= @struct.ushort_1)
			{
				if (@struct.uint_0 == 1)
				{
					return @struct.byte_1[0];
				}
				return @struct.byte_1[num - @struct.ushort_0];
			}
		}
		return byte_5;
	}

	private bool method_2(byte byte_5, uint uint_0, ref ProgressBar progressBar_0, out uint[] uint_1)
	{
		ArrayList arrayList_ = new ArrayList();
		if (byte_5 != 16 && byte_5 != 12)
		{
			uint_1 = new uint[0];
			return false;
		}
		method_4();
		arrayList_.Add(1);
		while ((ulong)Convert.ToUInt32(arrayList_[arrayList_.Count - 1]) < (ulong)string_1.Length)
		{
			method_5((uint)(arrayList_.Count - 1), byte_5, uint_0, ref arrayList_, 1);
		}
		uint_1 = new uint[arrayList_.Count];
		for (int i = 0; i < arrayList_.Count; i++)
		{
			uint_1[i] = Convert.ToUInt32(arrayList_[i]) * 2;
		}
		return true;
	}

	private bool method_3(byte byte_5, uint uint_0, ref ProgressBar progressBar_0, out uint[] uint_1)
	{
		ArrayList arrayList_ = new ArrayList();
		if (byte_5 >= 6 && byte_5 <= 16)
		{
			lWyIquqUn9();
			arrayList_.Add(0);
			while ((ulong)Convert.ToUInt32(arrayList_[arrayList_.Count - 1]) < (ulong)string_1.Length)
			{
				method_5((uint)(arrayList_.Count - 1), byte_5, uint_0, ref arrayList_, 5);
			}
			uint_1 = new uint[arrayList_.Count];
			for (int i = 0; i < arrayList_.Count; i++)
			{
				uint_1[i] = Convert.ToUInt32(arrayList_[i]);
			}
			return true;
		}
		uint_1 = new uint[0];
		return false;
	}

	private void method_4()
	{
		arrayList_2.Clear();
		for (int i = 0; i < 2; i++)
		{
			string path = "";
			switch (i)
			{
			case 0:
				path = Path.Combine(AppContext.BaseDirectory, "FontWidthData", "S60CHS.S16.wdt");
				break;
			case 1:
				path = Path.Combine(AppContext.BaseDirectory, "FontWidthData", "S60CHS.S12.wdt");
				break;
			}
			if (!File.Exists(path))
			{
				continue;
			}
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
			{
				ushort ushort_ = binaryReader.ReadUInt16();
				ushort ushort_2 = binaryReader.ReadUInt16();
				uint num = binaryReader.ReadUInt16();
				byte[] array = new byte[num];
				for (uint num2 = 0u; num2 < num; num2++)
				{
					array[(int)(IntPtr)num2] = binaryReader.ReadByte();
				}
				Struct0 @struct = default(Struct0);
				switch (i)
				{
				case 0:
					@struct.byte_0 = 16;
					break;
				case 1:
					@struct.byte_0 = 12;
					break;
				}
				@struct.ushort_0 = ushort_;
				@struct.ushort_1 = ushort_2;
				@struct.uint_0 = num;
				@struct.byte_1 = array;
				arrayList_2.Add(@struct);
			}
			binaryReader.Close();
			fileStream.Close();
		}
	}

	private void method_5(uint uint_0, byte byte_5, uint uint_1, ref ArrayList arrayList_4, byte byte_6)
	{
		if ((ulong)uint_0 >= (ulong)arrayList_4.Count)
		{
			return;
		}
		string text = "";
		int num = 0;
		uint num2;
		try
		{
			num2 = (uint)arrayList_4[(int)uint_0];
		}
		catch
		{
			num2 = 0u;
		}
		ArrayList arrayList = new ArrayList();
		byte b = 0;
		if (byte_6 == 1)
		{
			b = 50;
		}
		if (byte_6 == 5)
		{
			b = 25;
		}
		for (byte b2 = 0; b2 < b; b2++)
		{
			text = text.Remove(0, text.Length);
			byte b3 = 0;
			while ((ulong)num2 >= (ulong)string_1.Length)
			{
				string text2 = "\0";
				switch (text2)
				{
				case "\t":
				case "\0":
					text2 = "\u3000";
					break;
				}
				byte b4 = method_0(text2, byte_5);
				if (text2 == "\u2029")
				{
					b4 = 0;
				}
				if (b4 + b3 > uint_1)
				{
					break;
				}
				b3 += b4;
				num2++;
				if (!(text2 != "\u2029"))
				{
					break;
				}
				text += text2;
			}
			num += (int)arrayList[b2];
			if (b2 == b - 1)
			{
				uint num3;
				try
				{
					num3 = (uint)arrayList_4[arrayList_4.Count - 1];
				}
				catch
				{
					num3 = 0u;
				}
				if ((ulong)num2 < (ulong)string_1.Length && num2 > num3)
				{
					arrayList_4.Add(num3 + (uint)num);
				}
				if ((ulong)num2 >= (ulong)string_1.Length)
				{
					arrayList_4.Add((uint)string_1.Length);
				}
			}
		}
	}

	private void method_6()
	{
		short_0 = (short)(new Random().Next(1025, 32767) % 65535);
		int_2 = 0;
		for (int i = 0; i < arrayList_1.Count; i++)
		{
			arrayList_1[i] = ((string)arrayList_1[i]).Replace("\r\n", "\u2029");
			string_1 += (string)arrayList_1[i];
			int_0[i] = int_2;
			int_2 += ((string)arrayList_1[i]).Length * 2;
		}
		byte_3 = new byte[int_2];
		int num = 0;
		for (int j = 0; j < arrayList_1.Count; j++)
		{
			int length = ((string)arrayList_1[j]).Length;
			byte[] bytes = Encoding.Unicode.GetBytes((string)arrayList_1[j]);
			bytes.CopyTo(byte_3, num);
			num += bytes.Length;
		}
		byte_4 = new byte[(int_2 % 32768 != 0) ? (int_2 / 32768 + 1) : (int_2 / 32768)][];
		byte[] array = new byte[32768];
		int num2 = 0;
		int num3 = 0;
		for (int k = 0; k < byte_3.Length; k++)
		{
			array[num2] = byte_3[k];
			if (num2 != 32767 && k != byte_3.Length - 1)
			{
				num2++;
				continue;
			}
			byte[] output = new byte[32768];
			num2 = 0;
			Deflater deflater = new Deflater(9, noZlibHeaderOrFooter: false);
			if (deflater.IsNeedingInput)
			{
				deflater.SetInput(array, 0, array.Length);
			}
			deflater.Finish();
			deflater.Deflate(output);
			byte_4[num3] = new byte[deflater.TotalOut];
			Deflater deflater2 = new Deflater(9, noZlibHeaderOrFooter: false);
			if (deflater2.IsNeedingInput)
			{
				deflater2.SetInput(array, 0, array.Length);
			}
			deflater2.Finish();
			deflater2.Deflate(byte_4[num3]);
			num3++;
			Encoding.Unicode.GetBytes("");
			array = new byte[32768];
		}
	}

	private void method_7(byte byte_5, byte byte_6, ref uint[] uint_0, ref BinaryWriter binaryWriter_0, byte byte_7)
	{
		uint value = (uint)(28672 + new Random().Next(4095));
		binaryWriter_0.Write('#');
		binaryWriter_0.Write(135);
		binaryWriter_0.Write(byte_7);
		binaryWriter_0.Write(11);
		binaryWriter_0.Write(byte_5);
		binaryWriter_0.Write(byte_6);
		binaryWriter_0.Write(value);
		binaryWriter_0.Write('$');
		binaryWriter_0.Write(value);
		binaryWriter_0.Write((uint)(9 + uint_0.Length * 4));
		uint[] array = uint_0;
		foreach (uint value2 in array)
		{
			binaryWriter_0.Write(value2);
		}
	}
}
