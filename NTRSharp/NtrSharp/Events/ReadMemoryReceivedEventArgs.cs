using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrSharp.Events
{
	public class ReadMemoryReceivedEventArgs
	{
		public byte[] Buffer { get; }

		public ReadMemoryReceivedEventArgs(byte[] Buffer)
		{
			this.Buffer = Buffer;
		}
	}
}
