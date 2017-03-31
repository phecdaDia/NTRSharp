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

#if DEBUG
		private readonly Boolean IsDebug = true;
#else
		private readonly Boolean IsDebug = false;
#endif

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
			if (IsDebug) AllocConsole();
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
			if (txtOutput.IsDisposed) return;
			txtOutput.TryInvoke(new Action(() => txtOutput.AppendText(String.Format(Message, Format) + Environment.NewLine)));
			
		}

		// Form Events
		private void MainWindow_Load(object sender, EventArgs e)
		{
			try
			{
				LogLine("NTR by cell9");
				LogLine("NTRSharp by imthe666st");

				Log("Setup");
				this.cmbEditModeType.SelectedIndex = 0;
				this.cmbMemlayout.SelectedIndex = 0;
				this.cmbProcesses.SelectedIndex = 0;

				this.NtrClient = new NtrClient();

				this.NtrClient.EvtMessageReceived += (s, e_) => { LogLine(e_.Message); };
				this.NtrClient.EvtNtrStringReceived += (s, e_) => { HandleMessageReceived(e_.Message); };
				this.NtrClient.EvtReadMemoryReceived += (s, e_) => { HandleReadMemory(e_.Buffer); };

				this.NtrClient.EvtDisconnect += (s, e_) => { EnableConnect(); };

				this.NtrClient.EvtProgress += (s, e_) => { SetProgress(this.NtrClient?.Progress ?? 0); };

				Log(".");

				txtEditorByte.ContextMenu = new ContextMenu(new MenuItem[] {
					new MenuItem("Save to file", (s, e_) => {
						using(SaveFileDialog sfd = new SaveFileDialog())
						{
							sfd.DefaultExt = "bin";
							sfd.Filter = "Binary|*.bin";
							if (sfd.ShowDialog() == DialogResult.OK)
							{
								List<byte> byteCode = new List<byte>();
								String k = txtEditorByte.Text;
								k = String.Join(null, k.Split(' '));
								for (int i = 0; i <= k.Length - 2; i+=2)
								{

									byteCode.Add(Convert.ToByte(k.Substring(i, 2), 16));
								}

								File.WriteAllBytes(sfd.FileName, byteCode.ToArray());
							}
						}
					}),
					new MenuItem("Load from file", (s, e_) => {
						using (OpenFileDialog ofd = new OpenFileDialog())
						{
							ofd.Filter = "All|*.*";
							ofd.Multiselect = false;
							if (ofd.ShowDialog() == DialogResult.OK)
							{
								if (File.Exists(ofd.FileName))
								{
									byte[] data = File.ReadAllBytes(ofd.FileName);
									txtEditorByte.Text = ByteArrayToHexString(data);
								}
							}
						}
					}),

					new MenuItem("Clear", (s, e_) => txtEditorByte.Text = null),
					new MenuItem("Copy", (s, e_) => Clipboard.SetText(txtEditorByte.Text)),
					new MenuItem("Paste", (s, e_) => {
						String t = Clipboard.GetText();
						t = String.Join(null, t.Split(' '));
						t = String.Join(null, t.Split(Environment.NewLine));
						t = t.ToUpper();

						if (!new Regex("[0-9A-F]*").IsMatch(t)) return;

						//t = String.Join(" ", t.Split(2, true));
						txtEditorByte.Text = t;

					}),
				});


				txtEditorBase.ContextMenu = new ContextMenu(new MenuItem[] {
					new MenuItem("Save to file", (s, e_) => {
						using(SaveFileDialog sfd = new SaveFileDialog())
						{
							sfd.DefaultExt = "txt";
							sfd.Filter = "Text|*.txt";
							if (sfd.ShowDialog() == DialogResult.OK)
							{


								File.WriteAllText(sfd.FileName, txtEditorBase.Text);
							}
						}
					}),
					new MenuItem("Load from file", (s, e_) => {
						using (OpenFileDialog ofd = new OpenFileDialog())
						{
							ofd.Filter = "All|*.*";
							ofd.Multiselect = false;
							if (ofd.ShowDialog() == DialogResult.OK)
							{
								if (File.Exists(ofd.FileName))
								{
									byte[] data = File.ReadAllBytes(ofd.FileName);
									txtEditorBase.Text = ByteArrayToHexString(data);
								}
							}
						}
					}),

					new MenuItem("Clear", (s, e_) => txtEditorBase.Text = null),
					new MenuItem("Copy", (s, e_) => Clipboard.SetText(txtEditorBase.Text)),
					new MenuItem("Paste", (s, e_) => txtEditorBase.Text = Clipboard.GetText()),
				});
				Log(".");

				LogLine(Environment.NewLine + "Finished setup");
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
				Console.WriteLine("Derpy");
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
				byte[] baseCode = Compression.Decompress(Convert.FromBase64String(txtBaseCode.Text), COMPRESSION_MODE);
				//LogLine(ByteArrayToHexString(baseCode));
				Int32 Index = 0;
				//LogLine(ByteArrayToHexString(baseCode));
				UInt32 magic = BitConverter.ToUInt32(baseCode, Index);
				Index += 4;
				if (magic != BitConverter.ToUInt32(Encoding.ASCII.GetBytes("BASE"), 0))
				{
					LogLine("Received {0:X08}, expected {1:X08}", magic, BitConverter.ToUInt32(Encoding.ASCII.GetBytes("BASE"), 0));
					LogLine("Invalid Magic. It might be a broken code or an outdated one");

					return;
				}



				UInt32 Address = BitConverter.ToUInt32(baseCode, Index);
				Index += 4;
				//LogLine("{0:X08}", Address);
				Int32 ProcessNameLength = BitConverter.ToInt32(baseCode, Index);
				Index += 4;
				String ProcessName = Encoding.ASCII.GetString(baseCode, Index, ProcessNameLength);
				Index += ProcessNameLength;

				Int32 DataLength = BitConverter.ToInt32(baseCode, Index);
				Index += 4;
				//LogLine("{0:X}", DataLength);

				byte[] DataBuffer = baseCode.SubArray(Index, DataLength);
				Index += DataLength;

				//LogLine("{0} {1:X08} => {2}", ProcessName, Address, ByteArrayToHexString(DataBuffer));

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
				if (IsDebug) LogLine(ex.Message + Environment.NewLine + ex.StackTrace);	// good enough
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

		private void buttonBaseHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show(
				"How to use Base64 Codes:" + Environment.NewLine +
				 Environment.NewLine +
				 "If you want to create Base64 Codes simply specify an address you want to copy from and the length. You can then share the Base64 code for other people to use. Please beware that this won't work with dynamic pointer." + Environment.NewLine +
				 Environment.NewLine +
				 "If you want to use Base64 Codes simply paste the code you received into the textbox below or use the \"Paste\" button and click \"Use Code\""
				,
			"Help", MessageBoxButtons.OK);
		}

		private void txtEditorByte_KeyPress(object sender, KeyPressEventArgs e)
		{
			//Log("Derp: ");
			TextBox txt = sender as TextBox;
			char[] AllowedChars = new char[]
			{
				'0', '1', '2', '3',
				'4', '5', '6', '7',
				'8', '9',

				'A', 'B',
				'C', 'D', 'E', 'F',

				'a', 'b',			// figure out how to cast "toupper" and remove this
				'c', 'd', 'e', 'f',

				(char)Keys.Back,

			};
			char Key = e.KeyChar;
			if (!AllowedChars.Contains(Key))
			{
				e.Handled = true;
				return;
			}

			if (txt.SelectionStart >= 1 && txt.Text[txt.SelectionStart - 1] == ' ') txt.SelectionStart--;
		}

		Boolean AllowTxtEditByteTextChanged = true;
		private void txtEditorByte_TextChanged(object sender, EventArgs e)
		{
			if (!AllowTxtEditByteTextChanged)
			{
				AllowTxtEditByteTextChanged = true;
				return;
			}
			TextBox txt = sender as TextBox;
			String Code = txt.Text;
			int Index = txt.SelectionStart;
			Index -= Code.Substring(0, Index).Split(' ').Length;
			Index += (Index + 1) / 2;
			//LogLine("Derp?");

			Code = Code.Replace(Environment.NewLine, String.Empty);
			Code = Code.Replace(" ", String.Empty);


			// Now split every 2
			String[] CodeSplit = Code.Split(2, true).ToArray();



			Code = String.Join(" ", CodeSplit);

			//LogLine(Code);
			AllowTxtEditByteTextChanged = false;
			txt.Text = Code;
			txt.SelectionStart = Index + 1;
			AllowTxtEditByteTextChanged = true;
		}

		private void buttonEditorEncrypt_Click(object sender, EventArgs e)
		{
			List<byte> byteCode = new List<byte>();
			String k = txtEditorByte.Text;
			k = String.Join(null, k.Split(' '));
			k = String.Join(null, k.Split(Environment.NewLine));

			//Console.WriteLine(k.Length);
			for (int i = 0; i <= k.Length - 2; i+=2)
			{
				String j = k.Substring(i, 2);
				//Console.WriteLine(j);
				byte s = Convert.ToByte(j, 16);
				byteCode.Add(s);
			}

			//Console.WriteLine(byteCode.Count);

			//LogLine(ByteArrayToHexString(byteCode.ToArray()));

			UInt32 Address = Convert.ToUInt32(txtEditorAddress.Text, 16);

			txtEditorBase.Text = ToBase64Code(Address, GetProcessName(), byteCode);
		}

		private void buttonEditorDecrypt_Click(object sender, EventArgs e)
		{
			try
			{
				byte[] baseCode = Compression.Decompress(Convert.FromBase64String(txtEditorBase.Text), COMPRESSION_MODE);
				//LogLine(ByteArrayToHexString(baseCode));
				Int32 Index = 0;
				//LogLine(ByteArrayToHexString(baseCode));
				UInt32 magic = BitConverter.ToUInt32(baseCode, Index);
				Index += 4;
				if (magic != BitConverter.ToUInt32(Encoding.ASCII.GetBytes("BASE"), 0))
				{
					LogLine("Received {0:X08}, expected {1:X08}", magic, BitConverter.ToUInt32(Encoding.ASCII.GetBytes("BASE"), 0));
					LogLine("Invalid Magic. It might be a broken code or an outdated one");

					return;
				}



				UInt32 Address = BitConverter.ToUInt32(baseCode, Index);
				Index += 4;
				//LogLine("{0:X08}", Address);
				Int32 ProcessNameLength = BitConverter.ToInt32(baseCode, Index);
				Index += 4;
				String ProcessName = Encoding.ASCII.GetString(baseCode, Index, ProcessNameLength);
				Index += ProcessNameLength;

				Int32 DataLength = BitConverter.ToInt32(baseCode, Index);
				Index += 4;
				//LogLine("{0:X}", DataLength);

				byte[] DataBuffer = baseCode.SubArray(Index, DataLength);
				Index += DataLength;

				if (ProcessName != GetProcessName())
				{
					LogLine("You can't use this code on this process. Expected: {0}", ProcessName);
					return;
				}

				String t = Convert.ToString(Address, 16).ToString();
				for (int i = t.Length; i < 8; i++)
				{
					t = "0" + t;
				}
				txtEditorAddress.Text = t;
				

				txtEditorByte.Text = ByteArrayToHexString(DataBuffer);



			}
			catch (Exception ex)
			{
				LogLine("Not a valid Base64 Code");
				if (IsDebug) LogLine(ex.Message + Environment.NewLine + ex.StackTrace); // good enough
				return;
			}
		}

		private void buttonEditorUse_Click(object sender, EventArgs e)
		{
			try
			{
				UInt32 Address = Convert.ToUInt32(txtEditorAddress.Text, 16);
				List<byte> byteCode = new List<byte>();
				String k = txtEditorByte.Text;
				k = String.Join(null, k.Split(' '));
				for (int i = 0; i <= k.Length - 2; i += 2)
				{
					byte s = Convert.ToByte(k.Substring(i, 2), 16);
					byteCode.Add(s);
				}

				if (!IsValidMemregion(Address, (uint)byteCode.Count))
				{

					LogLine("Invalid Address / Length. No valid memregions found!");
					return;
				}


				this.NtrClient?.SendWriteMemPacket(Address, GetPid(), byteCode.ToArray());


			}
			catch (Exception ex)
			{
				LogLine("Not a valid Base64 Code");
				if (IsDebug) LogLine(ex.Message + Environment.NewLine + ex.StackTrace); // good enough
				return;
			}
		}

		private void buttonEditorClear_Click(object sender, EventArgs e)
		{
			txtEditorAddress.Text = "00000000";
			txtEditorLength.Text = "00000000";

			txtEditorByte.Text = null;
			txtEditorBase.Text = null;
		}

		// toolstrip

		private void disconnectfalseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.NtrClient?.Disconnect(false);
		}

		private void disconnecttrueToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.NtrClient.Disconnect(true);
		}

		private void buttonEditorCreate_Click(object sender, EventArgs e)
		{


			UInt32 Address = Convert.ToUInt32(txtEditorAddress.Text, 16);
			UInt32 Length = Convert.ToUInt32(txtEditorLength.Text, 16);


			if (!IsValidMemregion(Address, Length))
			{
				LogLine("Invalid Address / Length. No valid memregions found!");
				return;
			}

			this.ReadMemoryType = ReadMemoryType.CreateEditorCode;

			this.NtrClient?.SendReadMemPacket(Address, Length, GetPid());
		}


		// Validating

		private void ValidateHex32(object sender, CancelEventArgs e)
		{
			TextBox txt = sender as TextBox;
			Regex ValidRegex = new Regex(@"^[0-9A-F]{8}$");
			String Validator = txt.Text.ToUpper();

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

		private void txtEditorByte_Validating(object sender, CancelEventArgs e)
		{
			TextBox txt = sender as TextBox;
			String k = txt.Text;
			k = String.Join(null, k.Split(" "));
			if (!(k.Length % 2 == 0)) e.Cancel = true;
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


				if (String.IsNullOrEmpty(GetProcessName()))
				{
					LogLine("Process name is null or empty. WHY?!??");
					return;
				}

				//byteCode.AddRange(Encoding.ASCII.GetBytes("BASE")); // magic
				//byteCode.AddRange(BitConverter.GetBytes(Address));
				//byteCode.AddRange(BitConverter.GetBytes(GetProcessName().Length));
				//byteCode.AddRange(Encoding.ASCII.GetBytes(GetProcessName()));
				////byte[] DataBuffer = Compression.Compress(Buffer, COMPRESSION_MODE);
				//byteCode.AddRange(BitConverter.GetBytes(Buffer.Length));
				//byteCode.AddRange(Buffer);
				

				//String Base64 = Convert.ToBase64String(Compression.Compress(byteCode.ToArray(), COMPRESSION_MODE));

				String Base64 = ToBase64Code(Address, GetProcessName(), Buffer);
				this.txtBaseCode.TryInvoke(new Action(() =>
				{
					this.txtBaseCode.Text = Base64;
				}));
			} 
			else if (Rmt == ReadMemoryType.CreateEditorCode)
			{
				List<byte> byteCode = new List<byte>();

				UInt32 Address = Convert.ToUInt32(txtBaseAddress.Text, 16);
				UInt32 Length = Convert.ToUInt32(txtBaseLength.Text, 16);
				

				String Base64 = ToBase64Code(Address, GetProcessName(), Buffer);
				this.txtEditorBase.TryInvoke(new Action(() =>
				{
					this.txtEditorBase.Text = Base64;
				}));

				this.txtEditorByte.TryInvoke(new Action(() => this.txtEditorByte.Text = ByteArrayToHexString(Buffer)));
			}
			else if (Rmt == ReadMemoryType.EditMode)
			{
				UInt32 l = GetEditModeLength();
				if (Buffer.Length != GetEditModeLength())
				{
					LogLine("Expected {0:X}, received {1:X} bytes", GetEditModeLength(), Buffer.Length);
					return;
				}

				//LogLine(ByteArrayToHexString(Buffer));

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
					String k = Convert.ToString(Hex, 16).ToUpper();

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

		private String ToBase64Code(UInt32 Address, String Process, IEnumerable<byte> Buffer)
		{
			if (String.IsNullOrEmpty(Process)) return null;

			List<byte> byteCode = new List<byte>();

			byteCode.AddRange(Encoding.ASCII.GetBytes("BASE")); // magic
			byteCode.AddRange(BitConverter.GetBytes(Address));
			byteCode.AddRange(BitConverter.GetBytes(Process.Length));
			byteCode.AddRange(Encoding.ASCII.GetBytes(Process));
			//byte[] DataBuffer = Compression.Compress(Buffer, COMPRESSION_MODE);
			byteCode.AddRange(BitConverter.GetBytes(Buffer.ToArray().Length));
			byteCode.AddRange(Buffer);


			String Base64 = Convert.ToBase64String(Compression.Compress(byteCode.ToArray(), COMPRESSION_MODE));
			return Base64;
		}
	}

	public enum ReadMemoryType
	{
		None,
		DumpAsFile,
		DumpAsConsole,
		CreateCode,
		CreateEditorCode,
		EditMode,
	}

	public enum ReadNtrStringType
	{
		None,
		Process,
		MemLayout,
	}
}
