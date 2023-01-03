using FNAEngine2D.Network.Commands;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace FNAEngine2D.Network
{
    /// <summary>
    /// ServerMode
    /// </summary>
    public enum ServerMode
    {
        /// <summary>
        /// Didicated server only (no draw and open for external connexions)
        /// </summary>
        Dedicated,

        /// <summary>
        /// Local and server at the same time (open for external connexions)
        /// </summary>
        Host,

        /// <summary>
        /// Only local play (not open for external connexions)
        /// </summary>
        Local
    }

    /// <summary>
    /// Server
    /// </summary>
    public class NetworkServer: GameObject
    {
        /// <summary>
        /// Socket server
        /// </summary>
        private SocketServer _socketServer;
        
        /// <summary>
        /// List of clients
        /// </summary>
        private ConcurrentBag<ClientWorker> _clients = new ConcurrentBag<ClientWorker>();

        /// <summary>
        /// Dictionary of the game objects on the server
        /// </summary>
        private ConcurrentDictionary<Guid, NetworkGameObject> _gameObjects = new ConcurrentDictionary<Guid, NetworkGameObject>();

        /// <summary>
        /// List of the current game objects
        /// </summary>
        private List<NetworkGameObject> _listGameObjects = new List<NetworkGameObject>();

        /// <summary>
        /// In Process New Connections
        /// </summary>
        public ConcurrentQueue<InProcessChannel> _inProcessNewConnections = new ConcurrentQueue<InProcessChannel>();

        /// <summary>
        /// Objects to add
        /// </summary>
        private Queue<NetworkGameObject> _objectsToAdd = new Queue<NetworkGameObject>();

        /// <summary>
        /// Objects to remove
        /// </summary>
        private Queue<NetworkGameObject> _objectsToRemove = new Queue<NetworkGameObject>();

        /// <summary>
        /// New connections
        /// </summary>
        private Queue<ClientWorker> _newConnections = new Queue<ClientWorker>();

        /// <summary>
        /// Current Network client
        /// </summary>
        public static NetworkServer Current { get; private set; }


        /// <summary>
        /// Indicate 
        /// </summary>
        public bool IsRunning { get; set; } = false;


        /// <summary>
        /// Port number
        /// </summary>
        public int PortNumber { get; set; }

        /// <summary>
        /// ServerMode
        /// </summary>
        public ServerMode ServerMode { get; set; }

        ///// <summary>
        ///// Action on new connection
        ///// </summary>
        //public Action<ClientWorker> OnNewConnection { get; set; }

        /// <summary>
        /// Creation of the NetworkServer
        /// </summary>
        public NetworkServer(ServerMode serverMode, int portNumber)
        {
            this.ServerMode = serverMode;
            this.PortNumber = portNumber;

        }


        /// <summary>
        /// Add a game object
        /// </summary>
        public override void Load()
        {
        }

        /// <summary>
        /// Start the server
        /// </summary>
        public override void OnAdded()
        {
            Current = this;

            //Starting the socket server...
            _socketServer = new SocketServer();

            //If Isclient = true, we are in standalone mode... we will only listen on 127.0.0.1 (loopback)
            Logguer.Info("Starting server on port " + this.PortNumber);
            _socketServer.Start(this.ServerMode == ServerMode.Local, this.PortNumber);

            //Starting thread for new connexions...
            this.IsRunning = true;
            Thread thread = new Thread(DoItStartNewClients);
            thread.IsBackground = true;
            thread.Start();

            //SetupDebugCamera();


        }

        /// <summary>
        /// Stop the server
        /// </summary>
        public override void OnRemoved()
        {
            Logguer.Info("Stopping server.");

            this.IsRunning = false;
            _socketServer.Stop();

            Current = null;
        }

        /// <summary>
        /// Update each frame
        /// </summary>
        public override void Update()
        {
            try
            {
                ////Execution of the OnNewConnection event
                //while (_newConnections.Count > 0)
                //{
                //    ClientWorker clientWorker;
                //    lock (_newConnections)
                //    {
                //        clientWorker = _newConnections.Dequeue();
                //    }

                //    if (OnNewConnection != null)
                //        OnNewConnection(clientWorker);


                //}


                //Add object in the scene...
                while (_objectsToAdd.Count > 0)
                {
                    GameObject objToAdd;
                    lock (_objectsToAdd)
                        objToAdd = _objectsToAdd.Dequeue();
                    Add(objToAdd);
                }

                //remove object in the scene...
                while (_objectsToRemove.Count > 0)
                {
                    GameObject objToRemove;
                    lock (_objectsToRemove)
                        objToRemove = _objectsToRemove.Dequeue();
                    Remove(objToRemove);
                }


                //bool gameObjectAddedRemoved;

                ////Generate the ne
                //lock (_gameObjects)
                //{
                //    gameObjectAddedRemoved = _gameObjectAddedRemoved;
                //    _listGameObjects = new List<GameObject>(_gameObjects.Values);
                //    _gameObjectAddedRemoved = false;
                //}

                //foreach (ClientWorker clientWorker in _clients)
                //{
                //    try
                //    {
                //        //Ready?
                //        if (!clientWorker.IsReady)
                //            continue;

                //        //Spawn new objects...
                //        SendSpawnObjects(clientWorker);

                //    }
                //    catch (Exception exCW)
                //    {
                //        Logguer.Error("Server.Update (CW " + clientWorker.ID + ")", exCW);
                //    }
                //}


                ////Update locations...
                //if (_elapsedUpdateLocationsMilliseconds >= 5000)
                //{
                //    _elapsedUpdateLocationsMilliseconds = 0;
                //    UpdateObjectLocations();
                //}

                ////Save objects
                //if (_elapsedSaveMilliseconds >= 5000)
                //{
                //    _elapsedSaveMilliseconds = 0;
                //    SaveObjects();
                //}

            }
            catch (Exception ex)
            {
                Logguer.Error("Server.Update", ex);
            }
        }

        /// <summary>
        /// Spawn new objects...
        /// </summary>
        private void SendSpawnObjects(ClientWorker clientWorker)
        {
            foreach (NetworkGameObject gameObject in _listGameObjects)
            {
                clientWorker.AssureGameObjectPresent(gameObject);
            }
        }

        ///// <summary>
        ///// Update object locations
        ///// </summary>
        //private void UpdateObjectLocations()
        //{
        //    foreach (GameObject gameObject in _listGameObjects)
        //    {
        //        if (!_lastLocationGameObjects.TryGetValue(gameObject.ID, out Vector2 lastLocation) || lastLocation != gameObject.Location)
        //        {
        //            SendCommandToAllClients(new UpdateLocationCommand() { ID = gameObject.ID, Location = gameObject.Location }, null);
        //            _lastLocationGameObjects[gameObject.ID] = gameObject.Location;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Save objects in database
        ///// </summary>
        //private void SaveObjects()
        //{
        //    foreach (GameObject gameObject in _listGameObjects)
        //    {
        //        int version = gameObject.Version;
        //        if (!_lastSavedVersionGameObjects.TryGetValue(gameObject.ID, out int lastVersion) || lastVersion != version)
        //        {
        //            DataAccess.Save(gameObject.ID, gameObject);
        //            _lastSavedVersionGameObjects[gameObject.ID] = version;
        //        }
        //    }
        //}


        /// <summary>
        /// Spawn a game object on the server and on all clients
        /// </summary>
        public void SpawnObject(NetworkGameObject gameObject, ClientWorker localWorker = null)
        {
            
            lock (_gameObjects)
            {
                _gameObjects[gameObject.ID] = gameObject;
                _listGameObjects.Add(gameObject);
            }

            //Only in dedicated server mode we need to add the objet on the scene, otherwise, le object will be added twice!
            if (this.ServerMode == ServerMode.Dedicated)
            {
                lock (_objectsToAdd)
                    _objectsToAdd.Enqueue(gameObject);
            }


            //Add in clients...
            foreach (ClientWorker clientWorker in _clients)
            {
                clientWorker.AssureGameObjectPresent(gameObject, localWorker == clientWorker);

            }
        }

        /// <summary>
        /// Unspawn a game object on the server and on all clients
        /// </summary>
        public void UnspawnObject(NetworkGameObject gameObject, ClientWorker localWorker = null)
        {

            lock (_gameObjects)
            {
                _gameObjects.TryRemove(gameObject.ID, out var bidon);
                _listGameObjects.Remove(gameObject);
            }

            //Only in dedicated server mode we need to add the objet on the scene, otherwise, le object will be added twice!
            if (this.ServerMode == ServerMode.Dedicated)
            {
                lock (_objectsToRemove)
                    _objectsToRemove.Enqueue(gameObject);
            }


            //Add in clients...
            foreach (ClientWorker clientWorker in _clients)
            {
                clientWorker.UnspawnGameObject(gameObject);

            }
        }

        /// <summary>
        /// Send a command to all clients
        /// </summary>
        public void SendCommandToAllClients(IClientCommand command, ClientWorker clientToSkip = null)
        {
            foreach (ClientWorker clientWorker in _clients)
            {
                //Only if worker is ready and if it's not an in process channel, meaning we are working in Host mode or in Standalone mode
                if(clientWorker.IsReady && clientWorker != clientToSkip && !(clientWorker.Channel is InProcessChannel))
                    clientWorker.SendCommand(command);
            }
        }


        /// <summary>
        /// Thread for starting new clients
        /// </summary>
        private void DoItStartNewClients()
        {
            try
            {
                while (this.IsRunning)
                {
                    StartNewClients();

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Logguer.Error("Server.DoItStartNewClients", ex);
            }
        }

        /// <summary>
        /// Start new clients
        /// </summary>
        private void StartNewClients()
        {
            try
            {
                while (true)
                {
                    //New channels??
                    CommunicationChannel clientChannel = GetNewClientChannel();
                    if (clientChannel == null)
                        return;

                    Logguer.Info("New ClientWorker");

                    ClientWorker worker = new ClientWorker(clientChannel, this);
                    _clients.Add(worker);

                    worker.Start();

                    lock (_newConnections)
                        _newConnections.Enqueue(worker);

                    //Be sure that everthing is sent to the client...
                    SendSpawnObjects(worker);
                }
            }
            catch (Exception ex)
            {
                Logguer.Error("Server.StartNewClients", ex);
            }

        }

        /// <summary>
        /// Get the next client CommunicationChannel
        /// </summary>
        public CommunicationChannel GetNewClientChannel()
        {
            if (_socketServer.NewChannels.TryDequeue(out var clientChannel))
                return clientChannel;

            if(_inProcessNewConnections.TryDequeue(out var clientInProcessChannel))
                return clientInProcessChannel;

            return null;
        }

        /// <summary>
        /// Connection in process for the InProcessChannel
        /// </summary>
        internal void ConnectInProcess(InProcessChannel channel)
        {
            _inProcessNewConnections.Enqueue(channel);
        }
    }
}
