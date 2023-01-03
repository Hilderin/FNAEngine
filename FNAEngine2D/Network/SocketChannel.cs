using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAEngine2D.Network
{
    /// <summary>
    /// Socket communication channel
    /// </summary>
    public class SocketChannel : CommunicationChannel
    {
        /// <summary>
        /// Buffer size
        /// </summary>
        private const int BUFFER_SIZE = ushort.MaxValue;

        /// <summary>
        /// Header size
        /// </summary>
        private const int HEADER_SIZE = 2;

        /// <summary>
        /// Buffer for the reception
        /// </summary>
        private byte[] _bufferReceive = new byte[BUFFER_SIZE];

        /// <summary>
        /// Offset in the buffer
        /// </summary>
        private int _offsetBufferReceive = 0;

        /// <summary>
        /// Buffer for the send
        /// </summary>
        private byte[] _bufferSend = new byte[BUFFER_SIZE];

        /// <summary>
        /// Send in progress?
        /// </summary>
        private bool _sendInProgress = false;

        /// <summary>
        /// Server
        /// </summary>
        private SocketServer _server;

        /// <summary>
        /// Socket
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// Port number
        /// </summary>
        private int _port;

        /// <summary>
        /// Queue of objects to send
        /// </summary>
        private Queue<ICommand> _sendQueue = new Queue<ICommand>();

        /// <summary>
        /// Queue of objects to read
        /// </summary>
        private Queue<ICommand> _readQueue = new Queue<ICommand>();


        /// <summary>
        /// Indicate the state of the channel
        /// </summary>
        public ChannelState State { get; private set; } = ChannelState.NotConnected;

        /// <summary>
        /// Error detail
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Check if commands available for reading
        /// </summary>
        public bool Available { get { return _readQueue.Count > 0; } }

        /// <summary>
        /// Action on error
        /// </summary>
        public Action<Exception> OnError { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public SocketChannel(Socket socket, SocketServer server)
        {
            if (!socket.Connected)
                throw new InvalidOperationException("The socket must be connected.");

            _socket = socket;
            _server = server;

            this.State = ChannelState.Connected;

            //Already wait for something...
            BeginReceive();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SocketChannel(int port)
        {
            _port = port;
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        public void Connect(Action actionAfterConnect)
        {
            
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            this.State = ChannelState.Connecting;

            var result = socket.BeginConnect(new IPEndPoint(IPAddress.Loopback, _port), BeginConnectCallback, new object[] { socket, actionAfterConnect });

            if (result.IsCompleted)
                BeginConnectCallback(result);

        }

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {

            if (_socket != null && _socket.Connected)
            {
                _socket.Close();
            }

            _socket = null;
            this.State = ChannelState.Disconnected;

            if (_server != null)
                _server.Channels.TryTake(out var bidon);

        }

        /// <summary>
        /// Connect callback
        /// </summary>
        private void BeginConnectCallback(IAsyncResult ar)
        {
            try
            {
                object[] args = (object[])ar.AsyncState;
                Socket socket = (Socket)args[0];
                Action actionAfterConnect = (Action)args[1];
                
                socket.EndConnect(ar);

                //We are connected...
                _socket = socket;

                //Already wait for something...
                BeginReceive();

                this.State = ChannelState.Connected;

                if (actionAfterConnect != null)
                    actionAfterConnect();

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                this.State = ChannelState.Error;

                if (OnError != null)
                    OnError(ex);
            }
        }

        /// <summary>
        /// Wait and read the next object
        /// </summary>
        public ICommand WaitNextObject()
        {
            //We will wait the next command...
            if (_readQueue.Count == 0)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (_readQueue.Count == 0)
                {
                    System.Threading.Thread.Sleep(16);

                    if (stopwatch.ElapsedMilliseconds >= 30000)
                        throw new TimeoutException("Timeout while receving a command.");
                }
            }

            return ReadObject();

        }

        /// <summary>
        /// Wait and read the next object
        /// </summary>
        public T WaitNext<T>() where T: ICommand
        {
            //We will wait the next command...
            if (_readQueue.Count == 0)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (_readQueue.Count == 0)
                {
                    System.Threading.Thread.Sleep(16);

                    if (stopwatch.ElapsedMilliseconds >= 30000)
                        throw new TimeoutException("Timeout while receving a command.");
                }
            }

            return Read<T>();

        }


        /// <summary>
        /// Read the next object
        /// </summary>
        public ICommand ReadObject()
        {
            if (_readQueue.Count > 0)
            {
                lock (_readQueue)
                {
                    if (_readQueue.Count > 0)
                        return _readQueue.Dequeue();
                }
            }

            return null;

        }

        /// <summary>
        /// Read the next object
        /// </summary>
        public T Read<T>() where T: ICommand
        {
            object obj = ReadObject();

            if (obj == null)
                return default(T);

            if (!(obj is T))
                throw new InvalidCastException("Invalid command type received. Expected: " + typeof(T).FullName + ", Received: " + obj.GetType().FullName);

            return (T)obj;

        }



        /// <summary>
        /// Send the object
        /// </summary>
        public void Send(ICommand command)
        {
            bool mustSend = true;

            lock (_sendQueue)
            {
                if (_sendInProgress)
                {
                    mustSend = false;
                    _sendQueue.Enqueue(command);
                }
                else
                {
                    _sendInProgress = true;
                }
            }

            if (mustSend)
                SendCommand(command);
        }

        /// <summary>
        /// Send a command
        /// </summary>
        private void SendCommand(ICommand command)
        {
            int len = CommandHelper.Serialize(command, _bufferSend, HEADER_SIZE);

            //We include the header size in the lent...
            len += HEADER_SIZE;

            _bufferSend[0] = (byte)len;
            _bufferSend[1] = (byte)(len >> 8);

            try
            {
                _socket.BeginSend(_bufferSend, 0, len, SocketFlags.None, SendCallback, null);


            }
            catch (Exception ex)
            {
                //Probably the connection was closed...
                Disconnect();

                this.Error = ex.Message;
                this.State = ChannelState.Error;

                if (OnError != null)
                    OnError(ex);
            }
        }

        /// <summary>
        /// Callback on send
        /// </summary>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                _socket.EndSend(ar);

                ICommand nextCommandToSend = null;
                lock (_sendQueue)
                {
                    if (_sendQueue.Count > 0)
                    {
                        nextCommandToSend = _sendQueue.Dequeue();
                    }

                    if (nextCommandToSend == null)
                        _sendInProgress = false;

                }

                if (nextCommandToSend != null)
                    SendCommand(nextCommandToSend);
            }
            catch (Exception ex)
            {
                //Probably the connection was closed...
                Disconnect();

                this.Error = ex.Message;
                this.State = ChannelState.Error;

                if (OnError != null)
                    OnError(ex);
            }
        }

        /// <summary>
        /// Begin a receive
        /// </summary>
        private void BeginReceive()
        {
            _socket.BeginReceive(_bufferReceive, _offsetBufferReceive, BUFFER_SIZE - _offsetBufferReceive, SocketFlags.None, ReceiveCallback, null);

        }

        /// <summary>
        /// Callback on receive
        /// </summary>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = _socket;

                if (socket == null)
                    //We are disconnected..
                    return;

                int read = socket.EndReceive(ar);

                if (read == 0)
                {
                    //Socket closed...
                    this.State = ChannelState.Disconnected;
                }
                else
                {
                    //Lock so we can will not screwup _offsetBufferReceive on multithread...
                    lock (_readQueue)
                    {
                        _offsetBufferReceive += read;

                        ProcessBuffer();
                    }

                    //Next receive...
                    BeginReceive();
                }
            }
            catch (Exception ex)
            {
                //Probably the connection was closed...
                Disconnect();

                this.Error = ex.Message;
                this.State = ChannelState.Error;

                if (OnError != null)
                    OnError(ex);


            }
        }

        /// <summary>
        /// Process the buffer
        /// </summary>
        private void ProcessBuffer()
        {
            //Structure of the data...
            //2 bytes: length of the packet
            //Command serialized
            int totalLenProcessed = 0;
            while (_offsetBufferReceive - totalLenProcessed >= HEADER_SIZE)
            {
                //We have the length of the next command...
                int len = (_bufferReceive[totalLenProcessed + 1] << 8) + _bufferReceive[totalLenProcessed];

                if (_offsetBufferReceive >= len + totalLenProcessed)
                {
                    //We have the packet...
                    _readQueue.Enqueue(DeserializeNextPacket(totalLenProcessed, len));

                    totalLenProcessed += len;
                }
                else
                {
                    //Not enough data...
                    break;
                }
            }


            if (totalLenProcessed > 0)
            {
                //Is there anything else in the buffer... we move it on the front...
                if (_offsetBufferReceive > totalLenProcessed)
                {
                    Buffer.BlockCopy(_bufferReceive, totalLenProcessed, _bufferReceive, 0, _offsetBufferReceive - totalLenProcessed);
                }

                _offsetBufferReceive -= totalLenProcessed;
            }
        }

        /// <summary>
        /// Deserialize the next packet
        /// </summary>
        private ICommand DeserializeNextPacket(int offset, int len)
        {
            //We have the hole packet...
            return CommandHelper.Deserialize(_bufferReceive, offset + HEADER_SIZE, offset + len - HEADER_SIZE);
        }
    }
}
