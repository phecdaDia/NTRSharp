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
		//
		public String ServerHost;
		public int ServerPort;  // Should be 8000
		public Boolean HeartbeatSendable;

		public int Progress { get; private set; }
		public Boolean IsConnected { get { return Tcp?.Connected ?? false; } }

		private UInt32 CurrentSequence;
		private UInt32 LastReadMemorySequence;
		private Object SyncLock = new object();

		private TcpClient Tcp;
		private Thread PacketRecvThread;
		private Thread HeartbeatThread;
		private NetworkStream NetStream;

		private readonly int HEARTBEAT_INTERVAL = 1000; // How long shall the HeartbeatThread wait after every beat.

		// Output Vars
		public byte[] ReadMemory;

		// Events
		public event MessageReceivedEventHandler EvtMessageReceived;
		public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
		protected void OnMessageReceived(MessageReceivedEventArgs e) { EvtMessageReceived?.Invoke(this, e); }
		protected void Log(String Message, params object[] Format) { OnMessageReceived(new MessageReceivedEventArgs(String.Format(Message, Format))); }

		// UTF8 return
		// Temporary
		public event NtrStringReceivedEventHandler EvtNtrStringReceived;
		public delegate void NtrStringReceivedEventHandler(object sender, MessageReceivedEventArgs e);
		protected void OnNtrStringReceived(MessageReceivedEventArgs e) { EvtNtrStringReceived?.Invoke(this, e); }

		// Read Memory
		public event ReadMemoryReceivedEventHandler EvtReadMemoryReceived;
		public delegate void ReadMemoryReceivedEventHandler(object sender, ReadMemoryReceivedEventArgs e);
		protected void OnReadMemoryReceived(ReadMemoryReceivedEventArgs e) { EvtReadMemoryReceived?.Invoke(this, e); }

		// Connect
		public event ConnectEventHandler EvtConnect;
		public delegate void ConnectEventHandler(object sender, EventArgs e);
		protected void OnConnection() { EvtConnect?.Invoke(this, new EventArgs()); }

		// Disconnect
		public event DisconnectEventHandler EvtDisconnect;
		public delegate void DisconnectEventHandler(object sender, EventArgs e);
		protected void OnDisconnect() { EvtDisconnect?.Invoke(this, new EventArgs()); }

		// Progress changed
		public event ProgressEventHandler EvtProgress;
		public delegate void ProgressEventHandler(object sender, EventArgs e);
		protected void OnProgressChanged() { EvtProgress?.Invoke(this, new EventArgs()); }

		public NtrClient(String ServerHost)
		{
			SetServer(ServerHost, 8000);
		}

		public NtrClient() { }

		public void SetServer(String ServerHost, int ServerPort)
		{
			this.ServerHost = ServerHost;
			this.ServerPort = ServerPort;
		}

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

		public void Disconnect(Boolean WaitPacketThread = true)
		{
			try
			{
				Tcp?.Close();
				Tcp = null;
				if (WaitPacketThread)
				{
					//PacketRecvThread?.Join();
					PacketRecvThread.Abort();
				}
			}
			finally
			{
				Tcp = null;
			}
		}

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

		private void HandlePacket(UInt32 Command, UInt32 Sequence, byte[] DataBuffer)
		{
			if (Command == 9)
			{
				HandleReadMemory(Sequence, DataBuffer);
			}
		}

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

		public void SendEmptyPacket(UInt32 Command, params UInt32[] Args)
		{
			SendPacket(0, Command, Args, 0);
		}

		// Sending Packets

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

		public void SendHelloPacket()
		{
			Log("Sending 'HELLO' Packet");
			SendPacket(0, 3, null, 0);
		}

		public void SendReloadPacket()
		{
			Log("Sending 'RELOAD' Packet");
			SendPacket(0, 4, null, 0);
		}

		public void SendProcessPacket()
		{
			Log("Sending 'PROCESS' Packet");
			SendEmptyPacket(5);
		}

		public void SendMemLayoutPacket(UInt32 Pid)
		{
			Log("Sending 'MEMLAYOUT' Packet (0x{0:X})", Pid);
			SendEmptyPacket(8, Pid);
		}

		public void SendReadMemPacket(UInt32 Address, UInt32 Size, UInt32 Pid)
		{
			Log("Sending 'READMEM' Packet ({0:X08} {1:X08} @0x{2:X})", Address, Size, Pid);
			SendEmptyPacket(9, Pid, Address, Size);
			LastReadMemorySequence = CurrentSequence;
		}

		public void SendWriteMemPacket(UInt32 Address, UInt32 Pid, byte[] Buffer)
		{
			Log("Sending 'WRITEMEM' Packet ({0:X08} @0x{1:X} 0x{2:X})", Address, Pid, Buffer.Length);
			UInt32[] Args = new UInt32[] { Pid, Address, (UInt32) Buffer.Length };
			SendPacket(1, 10, Args, Args[2]);
			NetStream.Write(Buffer, 0, Buffer.Length);
		}


    }
}
