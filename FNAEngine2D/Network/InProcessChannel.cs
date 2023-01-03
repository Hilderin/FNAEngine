using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{
    /// <summary>
    /// In process communication channel
    /// </summary>
    public class InProcessChannel : CommunicationChannel
    {
        /// <summary>
        /// Queue of objects to send
        /// </summary>
        private Queue<ICommand> _sendQueue = new Queue<ICommand>();

        /// <summary>
        /// Queue of objects to read
        /// </summary>
        private Queue<ICommand> _readQueue = new Queue<ICommand>();

        /// <summary>
        /// Server connected to
        /// </summary>
        private NetworkServer _server;

        /// <summary>
        /// The other end of the communication
        /// </summary>
        private InProcessChannel _otherInProcessChannel;

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
        public InProcessChannel()
        {

        }

        /// <summary>
        /// Constructor for the channel for the server
        /// </summary>
        private InProcessChannel(Queue<ICommand> clientSendQueue, Queue<ICommand> clientReadQueue)
        {
            _readQueue = clientSendQueue;
            _sendQueue = clientReadQueue;
        }

        /// <summary>
        /// Connect
        /// </summary>
        public void Connect(Action actionAfterConnect)
        {
            _server = NetworkServer.Current;

            if (_server == null)
                throw new InvalidOperationException("Server not running.");

            InProcessChannel serverInProcessChannel = new InProcessChannel(_sendQueue, _readQueue);
            serverInProcessChannel.State = ChannelState.Connected;
            _otherInProcessChannel = serverInProcessChannel;

            _server.ConnectInProcess(serverInProcessChannel);

            this.State = ChannelState.Connected;

            if (actionAfterConnect != null)
                actionAfterConnect();
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        public void Disconnect()
        {
            if (_otherInProcessChannel != null)
                _otherInProcessChannel.State = ChannelState.Disconnected;

            this.State = ChannelState.Disconnected;
            _server = null;
        }

        /// <summary>
        /// Read the next object
        /// </summary>
        public T Read<T>() where T : ICommand
        {
            object obj = ReadObject();

            if (obj == null)
                return default(T);

            if (!(obj is T))
                throw new InvalidCastException("Invalid command type received. Expected: " + typeof(T).FullName + ", Received: " + obj.GetType().FullName);

            return (T)obj;
        }

        /// <summary>
        /// Read the next object
        /// </summary>
        public ICommand ReadObject()
        {
            lock (_readQueue)
            {
                if (_readQueue.Count > 0)
                    return _readQueue.Dequeue();
            }

            return null;
        }

        /// <summary>
        /// Send the object
        /// </summary>
        public void Send(ICommand data)
        {
            lock (_sendQueue)
            {
                _sendQueue.Enqueue(data);
            }
        }

        /// <summary>
        /// Wait and read the next object
        /// </summary>
        public T WaitNext<T>() where T : ICommand
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
    }
}
