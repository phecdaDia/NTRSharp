using NtrSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrTest
{
	class Program
	{
		static String IpAddress = @"192.168.0.17";

		static void Main(string[] args)
		{
			NtrClient ntr = new NtrClient(IpAddress);
			Console.WriteLine("NTR TEST - plz work");
			Console.ReadLine();
			do
			{
				Console.Write("Trying to connect... ");
				ntr.ConnectToServer();
				Console.WriteLine(ntr.IsConnected);
			} while (!ntr.IsConnected);
			ntr.SendProcessPacket();
			Console.WriteLine("\n\n");
			Console.ReadLine();
			ntr.SendMemLayoutPacket(0x29);
			Console.ReadLine();
			ntr.Disconnect();
		}
	}
}
