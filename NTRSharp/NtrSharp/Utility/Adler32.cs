using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrSharp.Utility
{
	public static class Adler32
	{
		//https://de.wikipedia.org/wiki/Adler-32
		private static readonly UInt16 MOD_ADLER = 65521;
		private static readonly UInt32 STANDART_ADLER = 0x00000001;

		/*
        public static UInt32 CalculateADLER32(byte[] Data)
        {
            UInt32 s1 = 1;
            UInt32 s2 = 0;

            for (int n = 0; n < Data.Length; n++)
            {
                s1 = (s1 + Data[n]) % MOD_ADLER;
                s2 = (s2 + s1) % MOD_ADLER;
            }

            return (s2 << 16) | s1;
        }
        */

		public static UInt32 CalculateAdler32(UInt32 Adler, byte[] Data, UInt32 Position = 0, Int32 Length = -1)
		{
			// Validating args
			if (Position >= Data.Length) throw new IndexOutOfRangeException();
			if (Length < 0) Length = (int)(Data.Length - Position);

			UInt32 s1 = (Adler & 0xffff) | 0;
			UInt32 s2 = ((Adler >> 16) & 0xffff) | 0;

			for (int n = 0; n < Length; n++)
			{
				s1 = (s1 + Data[Position + n]) % MOD_ADLER;
				s2 = (s1 + s2) % MOD_ADLER;
			}

			return (s1 | (s2 << 16)) | 0;

		}

		public static UInt32 CalculateAdler32(byte[] Data)
		{
			return CalculateAdler32(STANDART_ADLER, Data);
		}

	}
}