using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtrSharp.Events
{
	public class MessageReceivedEventArgs
	{
		public String Message { get; }
		public MessageReceivedEventArgs(String Message)
		{
			this.Message = Message;
		}
	}
}
