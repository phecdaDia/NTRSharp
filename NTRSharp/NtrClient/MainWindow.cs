using NewNtrClient.NtrObject;
using NtrSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewNtrClient
{
	public partial class MainWindow : Form
	{
		public NtrClient NtrClient;
		private ReadMemoryType ReadMemoryType = ReadMemoryType.None;
		private ReadNtrStringType ReadNtrStringType = ReadNtrStringType.None;

		private NtrProcess[] Processes = null;

		// Extern
		[System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint = "AllocConsole")]
		[return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
		public static extern bool AllocConsole();

		// Constructor
		public MainWindow()
		{
			InitializeComponent();
			this.Load += MainWindow_Load;

			this.FormClosing += (s, e_) => { this.NtrClient?.Disconnect(false); };
			AllocConsole();
		}

		// Static Methods

		private static void InvokeSetTextboxText(TextBox tb, object Text, params object[] Format)
		{
			String k = String.Format(Text.ToString(), Format);
			if (tb.InvokeRequired) tb.Invoke(new Action(() => { tb.Text = k; }));
			else tb.Text = k;
		}

		private static String ByteArrayToHexString(byte[] Buffer)
		{
			String k = "";

			for (int i = 0; i < Buffer.Length; i++)
			{
				k += String.Format("{0:X02} ", Buffer[i]);
				//if (i % 0x2c == 0x2b) k += Environment.NewLine;
			}
			return k;
		}

		// Logging

		public void Log(String Message, params object[] Format)
		{
			try
			{
				if (txtOutput.IsDisposed) return;

				if (txtOutput.InvokeRequired) txtOutput.Invoke(new Action(() => { Log(Message, Format); }));
				else txtOutput.AppendText(String.Format(Message, Format));
			}
			catch (Exception) { }
		}

		public void LogLine(String Message, params object[] Format)
		{
			txtOutput.AppendLine(String.Format(Message, Format));
		}

		// Form Events
		private void MainWindow_Load(object sender, EventArgs e)
		{
			LogLine("NTR by cell9");
			LogLine("NTRSharp by imthe666st");

			this.NtrClient = new NtrClient();

			this.NtrClient.EvtMessageReceived += (s, e_) => { LogLine(e_.Message); };
			this.NtrClient.EvtNtrStringReceived += (s, e_) => { HandleMessageReceived(e_.Message); };
			this.NtrClient.EvtReadMemoryReceived += (s, e_) =>	{ HandleReadMemory(e_.Buffer);  };

			this.NtrClient.EvtDisconnect += (s, e_) =>{ EnableConnect(); };
		}

		// Component Events	

		private void buttonConnect_Click(object sender, EventArgs e)
		{
			if (this.NtrClient.IsConnected) this.NtrClient.Disconnect();
			this.buttonConnect.Enabled = false;
			this.txtIpAddress.Enabled = false;


			this.NtrClient.SetServer(txtIpAddress.Text, 8000);
			new Task(() =>
			{
				int Retry = 0;
				LogLine("Trying to connect to {0}", txtIpAddress.Text);
				do
				{
					NtrClient.ConnectToServer();
				} while (!NtrClient.IsConnected && ++Retry < 3);
				if (!NtrClient.IsConnected)
				{
					LogLine("Unable to connect. :(");
					EnableConnect();
				}
			}).Start();

		}

		private void buttonProcesses_Click(object sender, EventArgs e)
		{
			this.ReadNtrStringType = ReadNtrStringType.Process;
			this.NtrClient.SendProcessPacket();
		}
		
		private void buttonMemlayout_Click(object sender, EventArgs e)
		{
			UInt32 Pid = GetPid();
			this.NtrClient.SendMemLayoutPacket(Pid);
		}

		// Handling stuff

		private void EnableConnect()
		{
			try
			{
				// enable the button
				buttonConnect.TryInvoke(new Action(() =>
				{
					buttonConnect.Enabled = true;
				}));

				txtIpAddress.TryInvoke(new Action(() =>
				{
					txtIpAddress.Enabled = true;
				}));
			}
			catch (Exception) { }
		}

		private void HandleReadMemory(byte[] Buffer)
		{
			ReadMemoryType Rmt = this.ReadMemoryType;
			this.ReadMemoryType = ReadMemoryType.None;
			if (Rmt == ReadMemoryType.None)
			{
				LogLine(ByteArrayToHexString(Buffer));
				return;
			}
		}

		private void HandleMessageReceived(String Message)
		{
			ReadNtrStringType Rnst = this.ReadNtrStringType;
			this.ReadNtrStringType = ReadNtrStringType.None;

			if (Rnst == ReadNtrStringType.None)
			{
				LogLine(Message);
				return;
			}
			else if (Rnst == ReadNtrStringType.Process)
			{
				// Now replace regex:" {2,}" with a single space.
				Message = new Regex(@"[ |\t]{2,}").Replace(Message, " ");

				List<String> ProcessStringList = Message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
				List<String> cList = new List<string>();
				List<NtrProcess> pList = new List<NtrProcess>();
				for (int i = 0; i < ProcessStringList.Count - 1; i++)
				{
					NtrProcess p = new NtrProcess(ProcessStringList[i]);
					if (!String.IsNullOrEmpty(p.Name))
					{
						cList.Add(p.ToString());
						pList.Add(p);
					}
				}

				this.Processes = pList.ToArray();

				this.cmbProcesses.TryInvoke(new Action(() =>
				{
					this.cmbProcesses.Items.Clear();
					this.cmbProcesses.Items.AddRange(cList.ToArray());
					this.cmbProcesses.SelectedIndex = 0;
				}));

			} else if (Rnst == ReadNtrStringType.MemLayout)
			{

			}

			LogLine(Message);
		}

		public UInt32 GetPid()
		{
			if (this.cmbProcesses.Items.Count > 1) return Convert.ToUInt32(this.cmbProcesses.Text.Split(' ')[0]);
			else return 0u;
		}
	}

	public enum ReadMemoryType
	{
		None,
	}

	public enum ReadNtrStringType
	{
		None,
		Process,
		MemLayout,
	}
}
