using NtrSharp.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NtrSharp
{
    public class NtrClient
    {
		/// <summary>
		/// Ip Address the client tries to connect to
		/// </summary>
		public String ServerHost;
		/// <summary>
		/// Port to connect
		/// Should be 8000 for normal debugging
		/// </summary>
		public int ServerPort;  // Should be 8000
		/// <summary>
		/// Can the Heartbeat package be send
		/// </summary>
		public Boolean HeartbeatSendable;

		/// <summary>
		/// Used for larger data transfers
		/// </summary>
		public int Progress { get; private set; }
		/// <summary>
		/// returns true if there's a connection. Otherwise returns false
		/// </summary>
		public Boolean IsConnected { get { return Tcp?.Connected ?? false; } }

		/// <summary>
		/// Current packet sequence
		/// </summary>
		private UInt32 CurrentSequence;
		/// <summary>
		/// Last packet sequence when we send a ReadMem Packet
		/// </summary>
		private UInt32 LastReadMemorySequence;
		/// <summary>
		/// SyncLock
		/// </summary>
		private Object SyncLock = new object();

		/// <summary>
		/// TcpClient to connect to the 3ds
		/// </summary>
		private TcpClient Tcp;
		/// <summary>
		/// Thread used to handle basic communication
		/// </summary>
		private Thread PacketRecvThread;
		/// <summary>
		/// Sends a heartbeat when HeartbeatSendable is true and the delay passed
		/// </summary>
		private Thread HeartbeatThread;
		/// <summary>
		/// Network stream used for reading/writing
		/// </summary>
		private NetworkStream NetStream;

		/// <summary>
		/// How long shall the HeartbeatThread wait after every beat.
		/// </summary>
		private readonly int HEARTBEAT_INTERVAL = 1000;

		// Output Vars
		public byte[] ReadMemory;

		// Events
		/// <summary>
		/// Event for handling received messages
		/// </summary>
		public event MessageReceivedEventHandler EvtMessageReceived;
		public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
		protected void OnMessageReceived(MessageReceivedEventArgs e) { EvtMessageReceived?.Invoke(this, e); }
		protected void Log(String Message, params object[] Format) { OnMessageReceived(new MessageReceivedEventArgs(String.Format(Message, Format))); }
		
		/// <summary>
		/// Event for handing process and memregion strings
		/// </summary>
		public event NtrStringReceivedEventHandler EvtNtrStringReceived;
		public delegate void NtrStringReceivedEventHandler(object sender, MessageReceivedEventArgs e);
		protected void OnNtrStringReceived(MessageReceivedEventArgs e) { EvtNtrStringReceived?.Invoke(this, e); }

		// Read Memory
		/// <summary>
		/// Event for handling read memory
		/// </summary>
		public event ReadMemoryReceivedEventHandler EvtReadMemoryReceived;
		public delegate void ReadMemoryReceivedEventHandler(object sender, ReadMemoryReceivedEventArgs e);
		protected void OnReadMemoryReceived(ReadMemoryReceivedEventArgs e) { EvtReadMemoryReceived?.Invoke(this, e); }

		// Connect
		/// <summary>
		/// Event that fires once the debugger is connected
		/// </summary>
		public event ConnectEventHandler EvtConnect;
		public delegate void ConnectEventHandler(object sender, EventArgs e);
		protected void OnConnection() { EvtConnect?.Invoke(this, new EventArgs()); }

		// Disconnect
		/// <summary>
		/// Event that fires once the debugger is disconnected
		/// </summary>
		public event DisconnectEventHandler EvtDisconnect;
		public delegate void DisconnectEventHandler(object sender, EventArgs e);
		protected void OnDisconnect() { EvtDisconnect?.Invoke(this, new EventArgs()); }

		// Progress changed
		/// <summary>
		/// Event that fires once the progress changed
		/// </summary>
		public event ProgressEventHandler EvtProgress;
		public delegate void ProgressEventHandler(object sender, EventArgs e);
		protected void OnProgressChanged() { EvtProgress?.Invoke(this, new EventArgs()); }

		/// <summary>
		/// Constructor which also sets the ServerHost and Port
		/// </summary>
		/// <param name="ServerHost"></param>
		public NtrClient(string ServerHost, int ServerPort)
		{
			SetServer(ServerHost, ServerPort);
		}

		/// <summary>
		/// Default Constructor
		/// </summary>
		public NtrClient() { }

		/// <summary>
		/// Set ServerHost and ServerPort of the NTRClient
		/// ServerPort should be 8000 for normal debugging
		/// </summary>
		/// <param name="ServerHost"></param>
		/// <param name="ServerPort"></param>
		public void SetServer(string ServerHost, int ServerPort)
		{
			this.ServerHost = ServerHost;
			this.ServerPort = ServerPort;
		}

		/// <summary>
		/// Tries to connect to the 3ds.
		/// If there's already an established connection, it will disconnect.
		/// </summary>
		public void ConnectToServer()
		{
			try
			{
				if (Tcp != null) Disconnect(false);
				Tcp = new TcpClient();
				Tcp.NoDelay = true;
				Tcp.Connect(ServerHost, ServerPort);

				CurrentSequence = 0;
				NetStream = Tcp.GetStream();
				HeartbeatSendable = true;

				PacketRecvThread = new Thread(new ThreadStart(PacketRecvThreadStart));
				PacketRecvThread.Start();

				HeartbeatThread = new Thread(new ThreadStart(HeartbeatThreadStart));
				HeartbeatThread.Start();

				Log("Server Connected");
				OnConnection();
			}
			catch (Exception ex)
			{
				Log("Failed to connect: " + ex.Message);
			}

		}

		/// <summary>
		/// Disconnects the NTRClient.
		/// </summary>
		/// <param name="WaitPacketThread"></param>
		public void Disconnect(Boolean WaitPacketThread = true)
		{
			try
			{
				Tcp?.Close();
				Tcp = null;
				if (WaitPacketThread)
				{
					//PacketRecvThread?.Join();
					PacketRecvThread?.Abort();
				}
			}
			finally
			{
				Tcp = null;
			}
		}

		/// <summary>
		/// Handle received packets
		/// </summary>
		private void PacketRecvThreadStart()
		{
			byte[] Buffer = new byte[84];
			UInt32[] Args = new UInt32[16];

			int Ret;

			NetworkStream Stream = NetStream;

			while (IsConnected)
			{
				try
				{
					Ret = ReadNetworkStream(Stream, Buffer, Buffer.Length);
					if (Ret == 0) return; // Unable to read network stream

					int t = 0;
					UInt32 Magic = BitConverter.ToUInt32(Buffer, t);
					t += 4;
                    UInt32 Sequence = BitConverter.ToUInt32(Buffer, t);
					t += 4;
					UInt32 Type = BitConverter.ToUInt32(Buffer, t);
					t += 4;
					UInt32 Command = BitConverter.ToUInt32(Buffer, t);

					// Get Args
					for (int i = 0; i < Args.Length; i++)
					{
						t += 4;
						Args[i] = BitConverter.ToUInt32(Buffer, t);
					}
					t += 4;
					UInt32 DataLength = BitConverter.ToUInt32(Buffer, t);

					if (Command != 0) Log("Packet: CMD = {0}, DataLen = {1}", Command, DataLength);

					if (Magic != 0x12345678)
					{
						Log("Broken Protocol: Magic = {0:X08}, Seq = {1}", Magic, Sequence);
						break;
					}

					if (Command == 0)
					{
						if (DataLength != 0)
						{
							byte[] DataBuffer = new byte[DataLength];
							ReadNetworkStream(Stream, DataBuffer, DataBuffer.Length);

							// fix \n -> Environment.NewLine. A "replace" doesn't work for whatever reason.
							String Message = Encoding.UTF8.GetString(DataBuffer);
							Message = String.Join(Environment.NewLine, Message.Split('\n'));
							this.OnNtrStringReceived(new MessageReceivedEventArgs(Message)); // used for memlayout / processes
						}

						lock (SyncLock)
						{
							HeartbeatSendable = true;
						}
					}
					else if (DataLength != 0)
					{
						byte[] DataBuffer = new byte[DataLength];
						ReadNetworkStream(Stream, DataBuffer, DataBuffer.Length);
						HandlePacket(Command, Sequence, DataBuffer);
                    }

				}
				catch (ThreadAbortException tex)
				{
					Log("Thread aborted");
					Log(tex.Message + Environment.NewLine + tex.StackTrace);
					return;
				}
				catch (Exception ex)
				{
					Log(ex.Message + Environment.NewLine + ex.StackTrace);
					break;
				}
			}

			Log("Server Disconnected.");
			Disconnect(false);
		}

		/// <summary>
		/// Handles heartbeat packets
		/// </summary>
		private void HeartbeatThreadStart()
		{
			while (IsConnected)
			{
				SendHeartbeatPacket();
				//Console.WriteLine("Trying to send HeartbeatPacket");
				Thread.Sleep(HEARTBEAT_INTERVAL);
			}
			OnDisconnect();
		}

		/// <summary>
		/// Copies the readable Inputstream of the Networkstream to Buffer
		/// </summary>
		/// <param name="Stream"></param>
		/// <param name="Buffer"></param>
		/// <param name="Length"></param>
		/// <returns></returns>
		private int ReadNetworkStream(NetworkStream Stream, byte[] Buffer, int Length)
		{
			int Index = 0;
			Boolean UseProgress = (Length > 0x100000);

			do
			{
				if (UseProgress)
				{
					Progress = (int)(((double)(Index) / Length) * 100);
					OnProgressChanged();
				}

				int Len = Stream.Read(Buffer, Index, Length - Index);
				if (Len == 0) return 0;

				Index += Len;
			} while (Index < Length);

			Progress = -1;
			OnProgressChanged();

			return Length;
		}

		/// <summary>
		/// Handles received packets
		/// </summary>
		/// <param name="Command"></param>
		/// <param name="Sequence"></param>
		/// <param name="DataBuffer"></param>
		private void HandlePacket(UInt32 Command, UInt32 Sequence, byte[] DataBuffer)
		{
			if (Command == 9)
			{
				HandleReadMemory(Sequence, DataBuffer);
			}
		}

		/// <summary>
		/// Handles Read Memory
		/// </summary>
		/// <param name="Sequence"></param>
		/// <param name="DataBuffer"></param>
		private void HandleReadMemory(UInt32 Sequence, byte[] DataBuffer)
		{
			if (Sequence != LastReadMemorySequence)
			{
				Log("{0} != {1} ignored", Sequence, LastReadMemorySequence);
				return;
			}

			LastReadMemorySequence = 0;
			ReadMemory = DataBuffer;
			OnReadMemoryReceived(new ReadMemoryReceivedEventArgs(ReadMemory));
		}

		// Send Packets

		/// <summary>
		/// Sends a packet with the specified arguments
		/// </summary>
		/// <param name="Type"></param>
		/// <param name="Command"></param>
		/// <param name="Args"></param>
		/// <param name="DataLen"></param>
		public void SendPacket(UInt32 Type, UInt32 Command, UInt32[] Args, UInt32 DataLen)
		{
			if (!IsConnected) return;
			int t = 0;
			CurrentSequence += 1000;
			byte[] Buffer = new byte[84];
			BitConverter.GetBytes(0x12345678).CopyTo(Buffer, t);
			t += 4;
			BitConverter.GetBytes(CurrentSequence).CopyTo(Buffer, t);
			t += 4;
			BitConverter.GetBytes(Type).CopyTo(Buffer, t);
			t += 4;
			BitConverter.GetBytes(Command).CopyTo(Buffer, t);
			for (int i = 0; i < 16; i++)
			{
				t += 4;
				if (Args?.Length > i) BitConverter.GetBytes(Args[i]).CopyTo(Buffer, t);
				else BitConverter.GetBytes(0).CopyTo(Buffer, t); // remove this? Test it later TODO
				// This can be improved by a few ns
				// not really worth it though
			}
			t += 4;
			BitConverter.GetBytes(DataLen).CopyTo(Buffer, t);
			NetStream.Write(Buffer, 0, Buffer.Length);
		}

		/// <summary>
		/// Sends an empty packet with the specified command and arguments
		/// </summary>
		/// <param name="Command"></param>
		/// <param name="Args"></param>
		public void SendEmptyPacket(UInt32 Command, params UInt32[] Args)
		{
			SendPacket(0, Command, Args, 0);
		}

		// Sending Packets

		/// <summary>
		/// Sends a "Heartbeat" packet
		/// </summary>
		public void SendHeartbeatPacket()
		{
			if (Tcp != null)
			{
				lock (SyncLock)
				{
					if (HeartbeatSendable)
					{
						HeartbeatSendable = false;
						SendPacket(0, 0, null, 0);
					}
				}
			}
		}

		/// <summary>
		/// Sends a "Hello" Packet
		/// </summary>
		public void SendHelloPacket()
		{
			Log("Sending 'HELLO' Packet");
			SendPacket(0, 3, null, 0);
		}

		/// <summary>
		/// Sends a "Reload" Packet
		/// </summary>
		public void SendReloadPacket()
		{
			Log("Sending 'RELOAD' Packet");
			SendPacket(0, 4, null, 0);
		}

		/// <summary>
		/// Sends a "Process" Packet
		/// Lists all processes
		/// </summary>
		public void SendProcessPacket()
		{
			Log("Sending 'PROCESS' Packet");
			SendEmptyPacket(5);
		}

		/// <summary>
		/// Sends a "MemLayout" Packet
		/// Returns an String with the Memlayout of the process with the specified PID
		/// </summary>
		public void SendMemLayoutPacket(UInt32 Pid)
		{
			Log("Sending 'MEMLAYOUT' Packet (0x{0:X})", Pid);
			SendEmptyPacket(8, Pid);
		}

		/// <summary>
		/// Sends a "ReadMem" Packet
		/// Reads data from Address on the process with the specified PID
		/// </summary>
		public void SendReadMemPacket(UInt32 Address, UInt32 Size, UInt32 Pid)
		{
			Log("Sending 'READMEM' Packet ({0:X08} {1:X08} @0x{2:X})", Address, Size, Pid);
			SendEmptyPacket(9, Pid, Address, Size);
			LastReadMemorySequence = CurrentSequence;
		}

		/// <summary>
		/// Sends a "WriteMem" Packet
		/// Writes Buffer at the specified address on the process with the specified PID
		/// </summary>
		public void SendWriteMemPacket(UInt32 Address, UInt32 Pid, byte[] Buffer)
		{
			Log("Sending 'WRITEMEM' Packet ({0:X08} @0x{1:X} 0x{2:X})", Address, Pid, Buffer.Length);
			UInt32[] Args = new UInt32[] { Pid, Address, (UInt32) Buffer.Length };
			SendPacket(1, 10, Args, Args[2]);
			NetStream.Write(Buffer, 0, Buffer.Length);
		}


    }
}
