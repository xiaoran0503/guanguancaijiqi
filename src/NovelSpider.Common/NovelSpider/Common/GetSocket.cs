using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NovelSpider.Common;

public class GetSocket
{
	public static Socket ConnectSocket(string string_0, int int_0)
	{
		IPAddress[] addressList = Dns.GetHostEntry(string_0).AddressList;
		foreach (IPAddress address in addressList)
		{
			IPEndPoint iPEndPoint = new IPEndPoint(address, int_0);
			Socket socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(iPEndPoint);
			if (socket.Connected)
			{
				return socket;
			}
		}
		return null;
	}

	public static string SocketSendReceive(string string_0, int int_0, Encoding encoding_0)
	{
		string s = "GET /book/showbooklist.aspx HTTP/1.1\r\nHost: " + string_0 + "\r\nConnection: Keep-Alive\r\n\r\n";
		byte[] bytes = encoding_0.GetBytes(s);
		byte[] array = new byte[256];
		Socket socket = ConnectSocket(string_0, int_0);
		if (socket == null)
		{
			return "Connection failed";
		}
		socket.Send(bytes, bytes.Length, SocketFlags.None);
		string text = "Default HTML page on " + string_0 + ":\r\n";
		int num;
		do
		{
			num = socket.Receive(array, array.Length, SocketFlags.None);
			text += encoding_0.GetString(array, 0, num);
		}
		while (num > 0);
		return text;
	}
}
