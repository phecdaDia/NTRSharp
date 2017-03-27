using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewNtrClient
{
	public static class WinFormsExtention
	{
		public static void AppendLine(this TextBox Source, string Value)
		{
			try
			{
				if (Source.IsDisposed) return;
				else if (Source.InvokeRequired) Source.Invoke(new Action(() => Source.AppendLine(Value)));
				else
				{
					if (Source.Text.Length == 0) Source.Text = Value;
					else Source.AppendText(Environment.NewLine + Value);
				}


			} catch	(Exception) { }
		}

		public static void TryInvoke(this Control Source, Delegate Method)
		{
			try
			{
				if (Source.InvokeRequired) Source.Invoke(Method);
				else Method.DynamicInvoke();
			} catch (Exception) { }

		}


	}
}
