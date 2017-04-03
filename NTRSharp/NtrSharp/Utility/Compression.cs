using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrSharp.Utility
{
	public static class Compression
	{
		public static byte[] Compress(byte[] Data, Int32 Cycle = 1)
		{
			if (Cycle <= 0) return Data;
			List<byte> lData = new List<byte>();

			for (int i = 0; i < Cycle; i++)
			{
				//lData.Clear();
				List<byte> _Adler32 = BitConverter.GetBytes(Adler32.CalculateAdler32(Data)).ToList();
				_Adler32.Reverse();
				using (MemoryStream ms = new MemoryStream())
				{
					using (DeflateStream gs = new DeflateStream(ms, CompressionMode.Compress))
					{
						gs.Write(Data, 0, Data.Length);

					}

					lData = ms.ToArray().ToList();
					// Add a zlib header. 

					lData.InsertRange(0, new byte[] { 0x78, 0x9C }); // I'll add a different header generation later

					// Add ADLER32
					lData.AddRange(_Adler32);
					Data = lData.ToArray();

				}
			}
			return lData.ToArray();
		}
		public static byte[] Decompress(byte[] Data, int Cycle)
		{
			if (Cycle <= 0) return Data;
			Console.WriteLine("DC >> {0:X}", Data.Length);
			for (int i = 0; i < Cycle; i++)
			{

				using (MemoryStream ms = new MemoryStream(Data))
				{
					MemoryStream msInner = new MemoryStream();

					// Read past the first two bytes of the zlib header
					ms.Seek(2, SeekOrigin.Begin);


					using (DeflateStream z = new DeflateStream(ms, CompressionMode.Decompress))
					{
						z.CopyTo(msInner);

					}
					Data = msInner.ToArray();
					Console.WriteLine("DC >> {0:X}", Data.Length);
				}
			}
			Console.WriteLine("OUT");
			return Data;
		}
	}
}