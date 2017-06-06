using NtrSharp.NtrObject;
using NtrSharp.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NewNtrClient
{
	[DataContract(IsReference=true)]
	public class ConfigFile : XmlManager<ConfigFile>
	{
		[DataMember]
		public String IpAddress { get; set; }
        public String port { get; set; }

        protected override void Init()
		{
			if (String.IsNullOrEmpty(IpAddress)) IpAddress = "192.168.1.45";
            if (String.IsNullOrEmpty(port)) port = "8000";
        }
	}
}
