using NewNtrClient.NtrObject;
using NtrSharp;
using NtrSharp.Utility;
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

		private readonly Int32 COMPRESSION_MODE = 2;
		private readonly UInt32 MAX_CONSOLE_DUMP = 0x400;

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

			// debugging
			//AllocConsole();
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
			try
			{
				LogLine("NTR by cell9");
				LogLine("NTRSharp by imthe666st");

				this.cmbEditModeType.SelectedIndex = 0;
				this.cmbMemlayout.SelectedIndex = 0;
				this.cmbProcesses.SelectedIndex = 0;

				this.NtrClient = new NtrClient();

				this.NtrClient.EvtMessageReceived += (s, e_) => { LogLine(e_.Message); };
				this.NtrClient.EvtNtrStringReceived += (s, e_) => { HandleMessageReceived(e_.Message); };
				this.NtrClient.EvtReadMemoryReceived += (s, e_) => { HandleReadMemory(e_.Buffer); };

				this.NtrClient.EvtDisconnect += (s, e_) => { EnableConnect(); };

				this.NtrClient.EvtProgress += (s, e_) => { SetProgress(this.NtrClient?.Progress ?? 0); };

				LogLine("Finished setup");
			}
			catch (Exception ex)
			{
				LogLine(ex.Message + Environment.NewLine + ex.StackTrace);
			}
		}

		// Component Events	

		private void buttonConnect_Click(object sender, EventArgs e)
		{
			//if (this.NtrClient? == null) this.NtrClient? = new NtrClient();

			if (this.NtrClient?.IsConnected ?? false) this.NtrClient?.Disconnect();
			this.buttonConnect.Enabled = false;
			this.txtIpAddress.Enabled = false;


			this.NtrClient?.SetServer(txtIpAddress.Text, 8000);

			this.ReadMemoryType = ReadMemoryType.None;
			this.ReadNtrStringType = ReadNtrStringType.None;

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
			this.NtrClient?.SendProcessPacket();
		}

		private void cmbProcesses_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ReadNtrStringType = ReadNtrStringType.MemLayout;
			UInt32 Pid = GetPid();
			this.NtrClient?.SendMemLayoutPacket(Pid);
		}

		private void buttonMemlayout_Click(object sender, EventArgs e)
		{
			this.ReadNtrStringType = ReadNtrStringType.MemLayout;
			UInt32 Pid = GetPid();
			this.NtrClient?.SendMemLayoutPacket(Pid);
		}

		private void buttonDumpMemoryFile_Click(object sender, EventArgs e)
		{
			UInt32 Address = Convert.ToUInt32(txtDumpMemAddrStart.Text, 16);
			UInt32 Length = Convert.ToUInt32(txtDumpMemAddrLength.Text, 16);

			if (!IsValidMemregion(Address, Length))
			{
				LogLine("Invalid Address / Length. No valid memregions found!");
				return;
			}

			

			this.ReadMemoryType = ReadMemoryType.DumpAsFile;
			this.NtrClient?.SendReadMemPacket(Address, Length, GetPid());
		}

		private void buttonDumpMemoryConsole_Click(object sender, EventArgs e)
		{
			UInt32 Address = Convert.ToUInt32(txtDumpMemAddrStart.Text, 16);
			UInt32 Length = Convert.ToUInt32(txtDumpMemAddrLength.Text, 16);


			if (Length > MAX_CONSOLE_DUMP)
			{
				Length = MAX_CONSOLE_DUMP;
				LogLine("Length exceeded 0x{0:X}, shortened to 0x{0:X}", MAX_CONSOLE_DUMP);
			}
			
			if (!IsValidMemregion(Address, Length))
			{
				LogLine("Invalid Address / Length. No valid memregions found!");
				return;
			}

			this.ReadMemoryType = ReadMemoryType.DumpAsConsole;
			this.NtrClient?.SendReadMemPacket(Address, Length, GetPid());
		}

		private void buttonUseBaseCode_Click(object sender, EventArgs e)
		{
			try
			{
				byte[] baseCode = Convert.FromBase64String(txtBaseCode.Text);
				//LogLine(ByteArrayToHexString(baseCode));
				UInt32 Address = BitConverter.ToUInt32(baseCode, 0);
				//LogLine("{0:X08}", Address);
				Int32 ProcessNameLength = BitConverter.ToInt32(baseCode, 4);
				String ProcessName = Encoding.ASCII.GetString(baseCode, 8, ProcessNameLength);

				Int32 DataLength = BitConverter.ToInt32(baseCode, 8 + ProcessNameLength);
				//LogLine("{0:X}", DataLength);

				byte[] DataBuffer = Compression.Decompress(baseCode.SubArray(12 + ProcessNameLength, DataLength), COMPRESSION_MODE);

				//LogLine("{0} {1:X08} => {2}", ProcessName, Address, ByteArrayToHexString(Buffer));

				if (!IsValidMemregion(Address, (uint)DataLength))
				{
					LogLine("Invalid Address / Length. No valid memregions found!");
					return;
				}
				else if (ProcessName != GetProcessName())
				{
					LogLine("Invalid process selected. Please select {0} to use this code!", ProcessName);
					return;
				}

				this.NtrClient?.SendWriteMemPacket(Address, GetPid(), DataBuffer);


			}
			catch (Exception ex)
			{
				LogLine("Not a valid Base64 Code");
				LogLine(ex.Message + Environment.NewLine + ex.StackTrace);
				return;
			}
		}

		private void buttonCreateBaseCode_Click(object sender, EventArgs e)
		{
			UInt32 Address = Convert.ToUInt32(txtBaseAddress.Text, 16);
			UInt32 Length = Convert.ToUInt32(txtBaseLength.Text, 16);

			if (!IsValidMemregion(Address, Length))
			{
				LogLine("Invalid Address / Length. No valid memregions found!");
				return;
			}

			this.ReadMemoryType = ReadMemoryType.CreateCode;
			this.NtrClient?.SendReadMemPacket(Address, Length, GetPid());
		}

		private void buttonBaseClipboardCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(txtBaseCode.Text);
		}

		private void buttonBaseClipboardPaste_Click(object sender, EventArgs e)
		{
			txtBaseCode.Text = Clipboard.GetText();
		}

		private void buttonEditModeRead_Click(object sender, EventArgs e)
		{
			UInt32 Address = Convert.ToUInt32(txtEditModeAddress.Text, 16);
			UInt32 Length = GetEditModeLength();

			if (!IsValidMemregion(Address, Length))
			{
				LogLine("Invalid Address / Length. No valid memregions found!");
				return;
			}

			this.ReadMemoryType = ReadMemoryType.EditMode;
			this.NtrClient?.SendReadMemPacket(Address, Length, GetPid());
		}

		private void buttonEditModeWriteHex_Click(object sender, EventArgs e)
		{
			UInt32 Address = Convert.ToUInt32(txtEditModeAddress.Text, 16);
			UInt32 Length = GetEditModeLength();

			byte[] b = BitConverter.GetBytes(Convert.ToUInt32(txtEditModeHex.Text, 16));
			if (cbEditModeLittleEndian.Checked) b.Reverse();
			b = b.SubArray(4 - (int)Length, (int)Length);

			if (!IsValidMemregion(Address, Length))
			{
				LogLine("Invalid Address / Length. No valid memregions found!");
				return;
			}

			this.NtrClient?.SendWriteMemPacket(Address, GetPid(), b);
		}

		private void buttonEditModeWriteDecimal_Click(object sender, EventArgs e)
		{
			try
			{
				UInt32 Address = Convert.ToUInt32(txtEditModeAddress.Text, 16);
				UInt32 Length = GetEditModeLength();

				TextBox txt = sender as TextBox;

				byte[] t = BitConverter.GetBytes(Convert.ToUInt32(txt.Text));
				t = t.SubArray(0, (int)Length);



				if (!IsValidMemregion(Address, Length))
				{
					LogLine("Invalid Address / Length. No valid memregions found!");
					return;
				}

				this.NtrClient.SendWriteMemPacket(Address, GetPid(), t);
			}
			catch (Exception)
			{
				LogLine("Unable to parse {0} as number.");
				//throw;
			}
		}

		// Validating

		private void ValidateHex32(object sender, CancelEventArgs e)
		{
			TextBox txt = sender as TextBox;
			Regex ValidRegex = new Regex(@"^[0-9A-F]{8}$");
			String Validator = txt.Text.ToUpper();
			for (int i = txt.Text.Length; i < 8; i++)
			{
				Validator = "0" + Validator;
			}

			if (!ValidRegex.IsMatch(Validator))
			{
				e.Cancel = true;
			}
			else
			{
				txt.Text = Validator;
			}
		}

		private void txtDumpMemFilename_Validating(object sender, CancelEventArgs e)
		{
			TextBox txt = sender as TextBox;
			Regex ValidRegex = new Regex(@"^[\w_\-\d]+\.[\w]+$");
			String Validator = txt.Text;

			if (String.IsNullOrEmpty(Validator)) return;
			
			if (Validator.Split(".", StringSplitOptions.RemoveEmptyEntries).ToArray().Length == 1)
			{
				Validator += @".bin";
			}

			if (!ValidRegex.IsMatch(Validator)) e.Cancel = true;
			else
			{
				txt.Text = Validator;
			}
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
			if (Rmt == ReadMemoryType.DumpAsFile)
			{
				File.WriteAllBytes(txtDumpMemFilename.Text, Buffer);
				LogLine("Saved 0x{0:X} bytes to {1}", Buffer.Length, txtDumpMemFilename.Text);
			}
			else if (Rmt == ReadMemoryType.DumpAsConsole)
			{
				LogLine(ByteArrayToHexString(Buffer));
			}
			else if (Rmt == ReadMemoryType.CreateCode)
			{
				List<byte> byteCode = new List<byte>();

				UInt32 Address = Convert.ToUInt32(txtBaseAddress.Text, 16);
				UInt32 Length = Convert.ToUInt32(txtBaseLength.Text, 16);

				String ProcessName;

				this.cmbProcesses.TryInvoke(new Action(() => ProcessName = GetProcessName()));


				if (String.IsNullOrEmpty(GetProcessName()))
				{
					LogLine("Process name is null or empty. WHY?!??");
					return;
				}

				byteCode.AddRange(BitConverter.GetBytes(Address));
				byteCode.AddRange(BitConverter.GetBytes(GetProcessName().Length));
				byteCode.AddRange(Encoding.ASCII.GetBytes(GetProcessName()));
				byte[] DataBuffer = Compression.Compress(Buffer, COMPRESSION_MODE);
				byteCode.AddRange(BitConverter.GetBytes(DataBuffer.Length));
				byteCode.AddRange(DataBuffer);

				String Base64 = Convert.ToBase64String(byteCode.ToArray());
				this.txtBaseCode.TryInvoke(new Action(() =>
				{
					this.txtBaseCode.Text = Base64;
				}));
			} 
			else if (Rmt == ReadMemoryType.EditMode)
			{
				UInt32 l = GetEditModeLength();
				if (Buffer.Length != GetEditModeLength())
				{
					LogLine("Expected {0:X}, received {1:X} bytes", GetEditModeLength(), Buffer.Length);
					return;
				}

				LogLine(ByteArrayToHexString(Buffer));

				Boolean IsLittleEndian = false;
				cbEditModeLittleEndian.TryInvoke(new Action(() =>
				{
					IsLittleEndian = cbEditModeLittleEndian.Checked;
				}));

				if (!IsLittleEndian) Buffer.Reverse();

				UInt32 Hex = 0u;
				for (int i = 0; i < Buffer.Length && i < 4; i++)
				{
					Hex |= (uint)(Buffer[i] << (int)((l-1)*8 - (8 * i)));
				}

				txtEditModeHex.TryInvoke(new Action(() =>
				{
					String k = Convert.ToString(Hex, 16);

					for (int i = k.Length; i < 8; i++)
					{
						k = "0" + k;
					}

					txtEditModeHex.Text = k;
				}));

				txtEditModeDecimal.TryInvoke(new Action(() =>
				{
					txtEditModeDecimal.Text = Hex.ToString();
				}));
			}
		}

		private void HandleMessageReceived(String Message)
		{
			ReadNtrStringType Rnst = this.ReadNtrStringType;
			this.ReadNtrStringType = ReadNtrStringType.None;

			LogLine(Message);
			
			if (Rnst == ReadNtrStringType.Process)
			{
				// Now replace regex:" {2,}" with a single space.
				Message = new Regex(@"[ |\t]{2,}").Replace(Message, " ");

				// probably going to rewrite this, I don't like this solution

				List<String> ProcessStringList = Message.Split(Environment.NewLine).ToList();
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

			}
			else if (Rnst == ReadNtrStringType.MemLayout)
			{
				/*
					@"00100000 - 00dd7fff , size: 00cd8000"
					  0        1 2        3 4     5	 
				 */
				
				
				List<String> MemoryLayoutList = Message.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).ToList();
				if (MemoryLayoutList.Count >= 3)
				{
					Console.WriteLine("MLL Count: {0}", MemoryLayoutList.Count);
					MemoryLayoutList.RemoveAt(0);
					MemoryLayoutList.RemoveAt(MemoryLayoutList.Count - 1);

					List<String> MemLayouts = new List<string>();
					foreach (String Entry in MemoryLayoutList)
					{
						try
						{
							String[] s = Entry.Split(' ');
							// split this later with @" | "
							String k = String.Format("{0} | {1} | {2}", s[0], s[2], s[5]);
							MemLayouts.Add(k);
							
						}
						catch (Exception) {}
					}

					cmbMemlayout.TryInvoke(new Action(() =>
					{
						cmbMemlayout.Items.Clear();
						cmbMemlayout.Items.AddRange(MemLayouts.ToArray());
						cmbMemlayout.SelectedIndex = 0;
					}));

				}

			}

		}

		public UInt32 GetPid()
		{
			try
			{
				UInt32 Output = 0u;
				this.cmbProcesses.TryInvoke(new Action(() =>
				{
					if (this.cmbProcesses.SelectedIndex >= 0 && this.cmbProcesses.Items.Count >= 1) Output = Convert.ToUInt32(this.cmbProcesses.Text.Split(' ')[0], 16);
				}));
				return Output;
			}
			catch (Exception) { return 0u; }
		}

		public String GetProcessName()
		{
			try
			{
				String Output = null;
				this.cmbProcesses.TryInvoke(new Action(() =>
				{
					if (this.cmbProcesses.SelectedIndex >= 0 && this.cmbProcesses.Items.Count >= 1) Output = this.cmbProcesses.Text.Split(' ')[2];
				}));
				return Output;
			}
			catch (Exception ex)
			{
				LogLine(ex.Message + Environment.NewLine + ex.StackTrace);
				return null;
			}
		}

		public Boolean IsValidMemregion(UInt32 Address, UInt32 Length)
		{
			Boolean output = false;

			this.cmbMemlayout.TryInvoke(new Action(() =>
			{

				foreach (var item in cmbMemlayout.Items)
				{
					String[] m = item.ToString().Split(" | ", StringSplitOptions.RemoveEmptyEntries).ToArray();
					if (m.Length == 3)
					{
						UInt32 Start = Convert.ToUInt32(m[0], 16);
						UInt32 Size = Convert.ToUInt32(m[2], 16);


						UInt32 End = Start + Size;

						if (Start <= Address && Address <= End)
						{
							if (Address + Length <= End)
							{
								output = true;
								return;
							}
							else return;
						}
					}
				}
			}));

			return output;
		}

		public void SetProgress(int k)
		{
			if (k < 0) k = 0;
			else if (k > 100) k = 100;

			this.pgbProgress.TryInvoke(new Action(() => this.pgbProgress.Value = k));
			//this.pgbProgress.Value = k;
		}

		private UInt32 GetEditModeLength()
		{
			UInt32 output = 0u;

			this.cmbEditModeType.TryInvoke(new Action(() =>
			{
				int index = cmbEditModeType.SelectedIndex;
				switch (index)
				{
					case 0: output = 1; return;   // byte
					case 1: output = 2; return;   // UInt16
					case 2: output = 4; return;   // UInt32
					default:
						output = 0; return;
				}
			}));

			return output;
		}

		private void disconnectfalseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.NtrClient?.Disconnect(false);
		}

		private void disconnecttrueToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.NtrClient.Disconnect(true);
		}
	}

	public enum ReadMemoryType
	{
		None,
		DumpAsFile,
		DumpAsConsole,
		CreateCode,
		EditMode,
	}

	public enum ReadNtrStringType
	{
		None,
		Process,
		MemLayout,
	}
}
