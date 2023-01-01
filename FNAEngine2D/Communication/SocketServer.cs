using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace FNAEngine2D.Communication
{
    /// <summary>
    /// Socket server
    /// </summary>
    public class SocketServer
    {
        /// <summary>
        /// Listener
        /// </summary>
        private Socket _listener;

        /// <summary>
        /// New connexions
        /// </summary>
        public ConcurrentQueue<SocketChannel> NewChannels { get; private set; } = new ConcurrentQueue<SocketChannel>();

        /// <summary>
        /// Current connexions
        /// </summary>
        public ConcurrentBag<SocketChannel> Channels { get; private set; } = new ConcurrentBag<SocketChannel>();

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start(bool localOnly, int port)
        {
            EndPoint ipEndPoint = new IPEndPoint((localOnly ? IPAddress.Loopback : IPAddress.Any), port);
            _listener = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _listener.Bind(ipEndPoint);
            _listener.Listen(port);

            _listener.BeginAccept(BeginAcceptCallback, null);

        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public void Stop()
        {
            if (_listener != null)
            {
                _listener.Close();
                _listener = null;
            }

            foreach (SocketChannel channel in this.Channels)
            {
                try
                {
                    channel.Disconnect();
                }
                catch { }
            }

        }

        /// <summary>
        /// Accept callback
        /// </summary>
        private void BeginAcceptCallback(IAsyncResult ar)
        {
            if (_listener == null)
                return;

            Socket clientSocket = _listener.EndAccept(ar);

            _listener.BeginAccept(BeginAcceptCallback, null);

            SocketChannel socketChannel = new SocketChannel(clientSocket, this);
            this.Channels.Add(socketChannel);
            NewChannels.Enqueue(socketChannel);

        }

    }
}
