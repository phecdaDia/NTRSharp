using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewNtrClient.NtrObject
{
	public class NtrProcess
	{
		public UInt32 ProcessId;
		public UInt32 KernelObjectPointer;

		public String Name;
		public UInt64 TitleId;

		public NtrProcess(String ProcessString)
		{
			Console.WriteLine(ProcessString);
			//		UINT32			String			UINT64/String			UINT32
			//pid: 0x00000029, pname: Rabbit_R, tid: 0004000000197100, kpobj: fff7bb40
			// Split on , => Regex?
			String[] CommaSplit = ProcessString.Split(',');
			if (CommaSplit.Length != 4)
			{
				return; // If there are not exactly 4 strings, return;
			}
			// GET PID
			this.ProcessId = Convert.ToUInt32(CommaSplit[0].Substring(7), 16);
			this.Name = CommaSplit[1].Substring(8);
			this.TitleId = Convert.ToUInt64(CommaSplit[2].Substring(6), 16);
			this.KernelObjectPointer = Convert.ToUInt32(CommaSplit[3].Substring(8), 16);

			//Console.WriteLine("{0} => {1:X} TID: {2:X016} {3:X08}", Name, ProcessId, TitleId, KernelObjectPointer);
        }

		public NtrProcess(UInt32 ProcessId, String Name, UInt64 TitleId, UInt32 KernelObjectPointer)
		{
			this.ProcessId = ProcessId;
			this.Name = Name;
			this.TitleId = TitleId;
			this.KernelObjectPointer = KernelObjectPointer;
		}

		public override String ToString()
		{
			return String.Format("{1} - {0:X}", this.Name, this.ProcessId);
		}
	}
}
