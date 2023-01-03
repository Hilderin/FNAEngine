using FNAEngine2D.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// A GameObject that is managed by the server
    /// </summary>
    public class NetworkGameObject: GameObject
    {

        /// <summary>
        /// NetworkClient object
        /// </summary>
        private NetworkClient _client = NetworkClient.Current;

        /// <summary>
        /// NetworkServer object
        /// </summary>
        private NetworkServer _server = NetworkServer.Current;


        /// <summary>
        /// Unique ID of the GameObject
        /// </summary>
        [Browsable(false)]
        public Guid ID { get; set; } = Guid.NewGuid();

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

        /// <summary>
        /// NetworkClient object
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public NetworkClient Client
        {
            get { return _client; }
        }

        /// <summary>
        /// NetworkServer object
        /// </summary>
        [Browsable(false)]
        [JsonIgnore]
        public NetworkServer Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Override of the DoDraw
        /// </summary>
        internal override void DoDraw()
        {
            //No drawing on the client...
            if (!this.IsClient)
                return;

            base.DoDraw();
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
