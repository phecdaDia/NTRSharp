using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NtrSharp.NtrObject
{
	[DataContract(IsReference=true)]
	public class Process
	{
		[DataMember]
		public String Name { get; set; }
		[DataMember]
		public MemoryRegion[] Memory { get; set; }

		public Process(String Name, params MemoryRegion[] Memory)
		{
			this.Name = Name;
			this.Memory = Memory;
		}
	}
}
