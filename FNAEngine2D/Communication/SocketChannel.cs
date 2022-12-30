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

namespace FNAEngine2D.Communication
{
    /// <summary>
    /// Socket communication channel
    /// </summary>
    public class SocketChannel : CommunicationChannel
    {
        /// <summary>
        /// Buffer size
        /// </summary>
        private const int BUFFER_SIZE = short.MaxValue;

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
        /// Socket
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// Queue of objects to send
        /// </summary>
        private Queue<object> _sendQueue = new Queue<object>();

        /// <summary>
        /// Queue of objects to read
        /// </summary>
        private Queue<object> _readQueue = new Queue<object>();


        /// <summary>
        /// Indicate if the communication is opened
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Check if commands available for reading
        /// </summary>
        public bool Available { get { return _readQueue.Count > 0; } }


        /// <summary>
        /// Constructor
        /// </summary>
        public SocketChannel(Socket socket)
        {
            _socket = socket;

            //Already wait for something...
            BeginReceive();
        }

        /// <summary>
        /// Connect to the server
        /// </summary>
        public static SocketChannel Connect(int port)
        {
            Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            client.Connect(new IPEndPoint(IPAddress.Loopback, port));

            return new SocketChannel(client);
        }

        /// <summary>
        /// Wait and read the next object
        /// </summary>
        public object WaitNextObject()
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
        public T WaitNext<T>()
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
        public object ReadObject()
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
        public T Read<T>()
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
        public void Send(object command)
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
        private void SendCommand(object command)
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
            catch (SocketException)
            {
                //Probably the connection was closed...
                this.IsOpen = false;
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

                object nextCommandToSend = null;
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
            catch (SocketException)
            {
                //Probably the connection was closed...
                this.IsOpen = false;
            }
        }

        /// <summary>
        /// Begin a receive
        /// </summary>
        private void BeginReceive()
        {
            var result = _socket.BeginReceive(_bufferReceive, _offsetBufferReceive, BUFFER_SIZE - _offsetBufferReceive, SocketFlags.None, ReceiveCallback, null);

            if (result.CompletedSynchronously)
                ReceiveCallback(result);
        }

        /// <summary>
        /// Callback on receive
        /// </summary>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int read = _socket.EndReceive(ar);

                if (read == 0)
                {
                    //Socket closed...
                    this.IsOpen = false;
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
            catch (SocketException)
            {
                //Probably the connection was closed...
                this.IsOpen = false;
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
        private object DeserializeNextPacket(int offset, int len)
        {
            //We have the hole packet...
            return CommandHelper.Deserialize(_bufferReceive, offset + HEADER_SIZE, offset + len - HEADER_SIZE);
        }
    }
}
