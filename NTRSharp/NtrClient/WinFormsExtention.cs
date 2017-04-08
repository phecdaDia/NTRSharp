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

		// Extention Methods
		// 

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

		public static IEnumerable<String> Split(this String Source, String Splitter, StringSplitOptions Options = StringSplitOptions.None)
		{
			return Source.Split(new string[] { Splitter }, Options);
		}

		public static T[] SubArray<T>(this T[] data, int index, int length)
		{
			T[] result = new T[length];
			Array.Copy(data, index, result, 0, length);
			return result;
		}
		public static IEnumerable<string> Split(this string str, int chunkSize, Boolean allow)
		{
			List<string> k = new List<string>();
			//Console.WriteLine(str);
			//Console.WriteLine("Len: {0} Chunks: {1}", str.Length, chunkSize);

			int i = 0;
			while (i < str.Length - chunkSize)
			{
				k.Add(str.Substring(i, chunkSize));
				i += chunkSize;
				//Console.WriteLine("Added String: {0}", k.LastOrDefault());
			}

			if (allow) k.Add(str.Substring(i, str.Length - i));

			return k;
		}

	}
}
