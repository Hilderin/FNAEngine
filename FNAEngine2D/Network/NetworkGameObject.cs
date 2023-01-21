using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Remoting.Channels;

namespace FNAEngine2D.Network
{
    /// <summary>
    /// A GameObject that is managed by the server
    /// </summary>
    public class NetworkGameObject: GameObject
    {
        /// <summary>
        /// NetworkClient object
        /// </summary>
        internal NetworkClient _client = NetworkClient.Current;

        /// <summary>
        /// NetworkServer object
        /// </summary>
        internal NetworkServer _server = NetworkServer.Current;


        /// <summary>
        /// Unique ID of the GameObject
        /// </summary>
        [Browsable(false)]
        public Guid ID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Indicate if we are not in a multiplayer game
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public bool IsStandalone
        {
            get { return _client == null && _server == null; }
        }

        /// <summary>
        /// Indicate if we are the client
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public bool IsClient
        {
            get { return _client != null; }
        }


        /// <summary>
        /// Indicate if we are the server
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public bool IsServer
        {
            get { return _server != null; }
        }

        ///// <summary>
        ///// Indicate if we are the client and the server (host mode)
        ///// </summary>
        //[Browsable(false)]
        //[JsonIgnore]
        //public bool IsClientAndServer
        //{
        //    get { return _client != null && _server != null; }
        //}


        /// <summary>
        /// Indicate if it is our player
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public bool IsLocalPlayer { get; set; }

        ///// <summary>
        ///// NetworkClient object
        ///// </summary>
        //[Browsable(false)]
        //[JsonIgnore]
        //public NetworkClient Client
        //{
        //    get { return _client; }
        //}

        ///// <summary>
        ///// NetworkServer object
        ///// </summary>
        //[Browsable(false)]
        //[JsonIgnore]
        //public NetworkServer Server
        //{
        //    get { return _server; }
        //}

        ///// <summary>
        ///// Override of the DoDraw
        ///// </summary>
        //internal override void DoDraw()
        //{
        //    //No drawing on the client...
        //    if (!this.IsClient)
        //        return;

        //    base.DoDraw();
        //}


        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            
        }


        /// <summary>
        /// Send a command to the server
        /// </summary>
        public void SendCommandServer(IServerCommand command)
        {
            Logguer.Info("Command sent to server: " + command.ToString());

            if (_client != null)
                _client.SendCommand(command);
            else
                command.ExecuteServer(new ServerCommandArgs(Guid.Empty, this));

        }


        /// <summary>
        /// Send a command to the client
        /// </summary>
        public void SendCommandClient(ClientCommand command, Guid connectionID)
        {
            Logguer.Info("Command sent to client: " + command.ToString());

            if (_server != null)
                _server.SendCommand(command, connectionID);
            else if (_client != null)
                throw new InvalidOperationException("Cannot send command to client from the client.");

        }

        /// <summary>
        /// Spawn a game object on the server and on all clients
        /// </summary>
        public void SpawnObject(NetworkGameObject gameObject)
        {
            if (_server != null)
                _server.SpawnObject(gameObject);
            else
                Add(gameObject);
        }

        /// <summary>
        /// Spawn a game object on the server and on all clients
        /// </summary>
        public void SpawnObject(NetworkGameObject gameObject, Guid localPlayerConnectionID)
        {
            if (_server != null)
                _server.SpawnObject(gameObject, localPlayerConnectionID);
            else
                Add(gameObject);
        }

        /// <summary>
        /// Send a command to all clients
        /// </summary>
        public void SendCommandToAllClients(IClientCommand command)
        {
            if (_server != null)
                //We are the server!
                _server.SendCommandToAllClients(command);
            else if (_client != null)
                //We are the client...
                throw new InvalidOperationException("Cannot send command to all clients from client side.");

            //In standalone mode, we do nothing!

        }

        /// <summary>
        /// Destruction of an object
        /// </summary>
        public override void Destroy()
        {
            //If we are on the server, we destroy everywhere...
            if (_server != null)
                _server.UnspawnObject(this);

            base.Destroy();
        }

        /// <summary>
        /// Find a GameObjet by ID
        /// </summary>
        public GameObject FindByID(Guid id)
        {
            return Find(o => o is NetworkGameObject && ((NetworkGameObject)o).ID == id);
        }


        ///// <summary>
        ///// Spawn a object
        ///// </summary>
        //public void SpawnObject(NetworkGameObject gameObject)
        //{
        //    if (_server == null)
        //        throw new InvalidOperationException("Cannot spawn objects without a NetworkServer object.");

        //    _server.SpawnObject(gameObject);
        //}


    }
}
