using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NtrSharp.NtrObject
{
	[DataContract(IsReference=true)]
	public class MemoryRegion
	{
		[DataMember]
		public UInt32 Start { get; set; }
		[IgnoreDataMember]
		public UInt32 End { get { return Start + Length - 1; } }
		[DataMember]
		public UInt32 Length { get; set; }

		public MemoryRegion(UInt32 Start, UInt32 Length)
		{
			this.Start = Start;
			this.Length = Length;
		}
	}
}
