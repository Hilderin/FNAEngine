using FNAEngine2D;
using FNAEngine2D.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;

namespace FNAEngine2D.Network
{
    /// <summary>
    /// NetworkClient
    /// </summary>
    public class NetworkClient : GameObject
    {
        /// <summary>
        /// Unique id of the client
        /// </summary>
        public Guid ID { get; private set; } = Guid.NewGuid();
        
        /// <summary>
        /// Dictionary of game objects
        /// </summary>
        private Dictionary<Guid, NetworkGameObject> _dictObjects = new Dictionary<Guid, NetworkGameObject>();

        /// <summary>
        /// Communication channel
        /// </summary>
        private CommunicationChannel _channel;

        /// <summary>
        /// Should invoke connected
        /// </summary>
        private bool _shouldInvokeConnected = false;

        /// <summary>
        /// Client ready?
        /// </summary>
        private bool _ready = false;

        /// <summary>
        /// Current Network client
        /// </summary>
        public static NetworkClient Current { get; private set; }

        /// <summary>
        /// Port number
        /// </summary>
        public int PortNumber { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public ChannelState State
        {
            get { return _channel.State; }
        }

        /// <summary>
        /// Error
        /// </summary>
        public string Error { get { return _channel.Error; } }

        /// <summary>
        /// Action on connected
        /// </summary>
        public Action OnConnected { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkClient(int portNumber)
        {
            this.PortNumber = portNumber;
        }

        /// <summary>
        /// Loading
        /// </summary>
        public override void Load()
        {
            Logguer.Info("New NetworkClient " + this.ID);

            //Creation of the communication channel
            if (this.PortNumber <= 0)
                _channel = new InProcessChannel();
            else
                _channel = new SocketChannel(this.PortNumber);
            _channel.OnError = ex => Logguer.Error(this.ID.ToString() + " SocketError", ex);


        }

        /// <summary>
        /// Added
        /// </summary>
        public override void OnAdded()
        {
            Current = this;

            _ready = false;

            Logguer.Info("Client connecting...");
            _channel.Connect(AfterConnect);

        }

        /// <summary>
        /// Removed
        /// </summary>
        public override void OnRemoved()
        {
            _shouldInvokeConnected = false;

            _channel.Disconnect();
            
            Current = null;

        }


        /// <summary>
        /// Update on the client
        /// </summary>
        public override void Update()
        {
            //Newly connected?
            if (_shouldInvokeConnected)
            {
                _shouldInvokeConnected = false;
                if (OnConnected != null)
                    OnConnected();
            }


            //Process received commands...
            while (_channel.Available)
            {
                IClientCommand cmd = _channel.Read<IClientCommand>();
                if (cmd != null)
                {
                    cmd.ExecuteClient(this);
                    Logguer.Info("Command executed: " + cmd.ToString() + " NetworkClient: " + this.ID);

                }
            }

        }

        /// <summary>
        /// Spawn an object
        /// </summary>
        public void AddGameObject(NetworkGameObject obj)
        {
            Add(obj);
            _dictObjects[obj.ID] = obj;
        }

        /// <summary>
        /// Remove a game object
        /// </summary>
        public void RemoveGameObject(Guid id)
        {
            if (_dictObjects.TryGetValue(id, out var gameObject))
            {
                _dictObjects.Remove(id);
                Remove(gameObject);
            }
        }

        /// <summary>
        /// Remove a game object
        /// </summary>
        public NetworkGameObject GetGameObject(Guid id)
        {
            if (_dictObjects.TryGetValue(id, out var gameObject))
                return gameObject;

            return null;
        }


        /// <summary>
        /// Send a command to the server
        /// </summary>
        public void SendCommand(IServerCommand command)
        {
            Logguer.Info("Command sent: " + command.ToString());

            _channel.Send(command);
        }

        /// <summary>
        /// The client is ready!
        /// </summary>
        internal void SetReadyState()
        {
            _ready = true;
        }

        /// <summary>
        /// Todo after the connection is established..
        /// </summary>
        private void AfterConnect()
        {
            Logguer.Info("Client connected!");

            _shouldInvokeConnected = true;
        }

    }
}
