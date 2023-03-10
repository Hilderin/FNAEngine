using FNAEngine2D.Physics;
using FNAEngine2D.Network.Commands;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.CodeDom;

namespace FNAEngine2D.Network
{
    public class ClientWorker
    {
        /// <summary>
        /// Last id assigned
        /// </summary>
        private static int _lastID = 0;

        /// <summary>
        /// Static lock obj
        /// </summary>
        private static object _lockObj = new object();

        /// <summary>
        /// Current server
        /// </summary>
        private NetworkServer _server;

        /// <summary>
        /// Communication channel
        /// </summary>
        private ICommunicationChannel _clientChannel;

        /// <summary>
        /// Dictionary of the game objects sent to the client
        /// </summary>
        private ConcurrentDictionary<Guid, ClientWorkerGameObject> _dictGameObjects = new ConcurrentDictionary<Guid, ClientWorkerGameObject>();

        /// <summary>
        /// Objects to add
        /// </summary>
        private Queue<ClientWorkerGameObject> _objectsToAdd = new Queue<ClientWorkerGameObject>();

        /// <summary>
        /// Objects to remove
        /// </summary>
        private Queue<NetworkGameObject> _objectsToRemove = new Queue<NetworkGameObject>();

        /// <summary>
        /// Default server command args that we will reuse
        /// </summary>
        private ServerCommandArgs _serverCommandArgs;

        /// <summary>
        /// GameLoop
        /// </summary>
        private GameLoop _gameLoop;

        /// <summary>
        /// Connection ID
        /// </summary>
        public Guid ConnectionID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Id du client
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Indicate if server running
        /// </summary>
        public bool IsRunning { get { return _gameLoop.IsRunning; } set { _gameLoop.IsRunning = value; } }

        /// <summary>
        /// Client still running and player ok
        /// </summary>
        public bool IsReady { get; set; } = false;

        /// <summary>
        /// User ID
        /// </summary>
        public Guid UserID { get; set; } = Guid.Empty;

        /// <summary>
        /// Player object
        /// </summary>
        public NetworkGameObject Player { get; set; }

        /// <summary>
        /// Server
        /// </summary>
        public NetworkServer Server { get { return _server; } }

        /// <summary>
        /// Channel of communication
        /// </summary>
        public ICommunicationChannel Channel { get { return _clientChannel; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public ClientWorker(ICommunicationChannel clientChannel, NetworkServer server)
        {
            _clientChannel = clientChannel;
            _clientChannel.OnError = ex => LogError("SocketError", ex);

            _serverCommandArgs = new ServerCommandArgs(this.ConnectionID);

            _server = server;
            _gameLoop = new GameLoop(Update);

            lock (_lockObj)
            {
                this.ID = --_lastID;
            }
        }


        /// <summary>
        /// Start for a new channel
        /// </summary>
        public void Start()
        {
            Thread thread = new Thread(DoItClientWorker);
            thread.IsBackground = true;
            thread.Start();
        }


        /// <summary>
        /// Assure that a game object is followed by the client worker
        /// </summary>
        public void AssureGameObjectPresent(NetworkGameObject gameObject, bool isLocalPlayer = false)
        {
            if (!_dictGameObjects.ContainsKey(gameObject.ID))
            {
                ClientWorkerGameObject cwgo = new ClientWorkerGameObject() { GameObject = gameObject, IsLocalPlayer = isLocalPlayer };
                _dictGameObjects[gameObject.ID] = cwgo;

                lock (_objectsToAdd)
                    _objectsToAdd.Enqueue(cwgo);
            }
        }

        /// <summary>
        /// Assure that a game object is followed by the client worker
        /// </summary>
        public void UnspawnGameObject(NetworkGameObject gameObject)
        {
            if (_dictGameObjects.TryRemove(gameObject.ID, out ClientWorkerGameObject cwgo))
            {
                lock (_objectsToRemove)
                    _objectsToRemove.Enqueue(gameObject);
            }
        }


        /// <summary>
        /// Send command
        /// </summary>
        public void SendCommand(IClientCommand command)
        {
            LogInfo("Command sent: " + command.ToString());

            _clientChannel.Send(command);
        }


        /// <summary>
        /// Send a command to all other clients
        /// </summary>
        public void SendCommandToAllClients(IClientCommand command)
        {
            _server.SendCommandToAllClients(command, this);
        }

        /// <summary>
        /// Thread for a ClientWorker
        /// </summary>
        private void DoItClientWorker()
        {
            try
            {
                //Process initlization...
                _gameLoop.GameTimer.Restart();

                this.IsReady = true;

                //Sending the Connection ID to the client...
                SendCommand(new NewConnectionCommand() { ConnectionID = this.ConnectionID });

                //Managing the client...                
                _gameLoop.RunLoop();

                LogInfo("ClientWorker.Done");

            }
            catch (Exception ex)
            {
                LogError("ClientWorker.DoItClientWorker", ex);
            }
            finally
            {
                IsRunning = false;
                IsReady = false;
            }
        }

        /// <summary>
        /// Update each frame
        /// </summary>
        private void Update()
        {
            //Process input command...
            while (_clientChannel.Available)
            {
                ServerCommand cmd = _clientChannel.Read<ServerCommand>();
                if (cmd != null)
                {
                    try
                    {
                        NetworkGameObject gameObject;
                        if (cmd.ID != Guid.Empty)
                        {
                            gameObject = (NetworkGameObject)_server.GameObject.Game.RootGameObject.Find(o => o is NetworkGameObject && ((NetworkGameObject)o).ID == cmd.ID);
                            if (gameObject == null)
                            {
                                LogError("GameObject not found: " + cmd.ID);
                                continue;
                            }
                        }
                        else
                        {
                            //Directly on the server..
                            gameObject = (NetworkGameObject)_server.GameObject;
                        }


                        _serverCommandArgs.GameObject = gameObject;
                        cmd.ExecuteServer(_serverCommandArgs);

                        LogInfo("Command executed: " + cmd.ToString());
                    }
                    catch (Exception ex)
                    {
                        LogInfo("Command error " + cmd.ToString() + ": " + ex.ToString());
                    }
                }
            }

            //Send new objects...
            SendNewObjects();

            //Send destroyed objects...
            SendDestroyedObjects();
        }

        /// <summary>
        /// Send new objects to the client
        /// </summary>
        private void SendNewObjects()
        {
            //Add object in the scene...
            while (_objectsToAdd.Count > 0)
            {
                ClientWorkerGameObject cwgo;
                lock (_objectsToAdd)
                    cwgo = _objectsToAdd.Dequeue();

                //Spawning...
                if (cwgo.IsLocalPlayer)
                {
                    //Oh, this is the local player object...
                    this.Player = cwgo.GameObject;
                    SendCommand(GetSpawnCommand(cwgo.GameObject, true));
                }
                else
                {
                    //Normal object...
                    SendCommand(GetSpawnCommand(cwgo.GameObject, false));
                }

                RigidBody rigidBody = cwgo.GameObject.GetComponent<RigidBody>();
                if (rigidBody != null)
                    SendCommand(new MovementCommand() { ID = cwgo.GameObject.ID, Movement = rigidBody.Movement, StartPosition = cwgo.GameObject.Location });
                
            }
        }

        /// <summary>
        /// Get the command for a gameobject
        /// </summary>
        private ClientCommand GetSpawnCommand(NetworkGameObject gameObject, bool isLocalPlayer)
        {
            if (isLocalPlayer)
                return new SpawnPlayerObjectCommand() { GameObject = gameObject };
            else
                return new SpawnObjectCommand() { GameObject = gameObject };

        }

        /// <summary>
        /// Send destroyed objects to the client
        /// </summary>
        private void SendDestroyedObjects()
        {
            //Remove object from the scene...
            while (_objectsToRemove.Count > 0)
            {
                NetworkGameObject gameObject;
                lock (_objectsToRemove)
                    gameObject = _objectsToRemove.Dequeue();

                if (this.Player == gameObject)
                    this.Player = null;

                SendCommand(new UnspawnObjectCommand() { ID = gameObject.ID });

            }
        }


        ///// <summary>
        ///// Process the initialisation
        ///// </summary>
        //private void ProcessInitialization()
        //{
        //    //First thing we should receive is a login command...
        //    LoginCommand loginCommand = _clientChannel.WaitNext<LoginCommand>();

        //    this.UserID = loginCommand.UserID;

        //    Logguer.Info("Login: " + this.UserID.ToString());

        //    Guid characterID;

        //    if (this.UserID.ToString() == "00000000-0000-0000-0000-000000000001")
        //    {
        //        characterID = Guid.Parse("f3d40172-d4c1-4723-8fc8-195c28a49587");
        //    }
        //    else if (this.UserID.ToString() == "00000000-0000-0000-0000-000000000002")
        //    {
        //        characterID = Guid.Parse("89c95f68-9368-4cec-a3db-ea82b3a0790c");
        //    }
        //    else
        //    {
        //        characterID = Guid.NewGuid();
        //    }


        //    this.Character = DataAccess.Load<Character>(characterID);
        //    if (this.Character == null)
        //    {
        //        this.Character = new Character();

        //        if (this.UserID.ToString() == "00000000-0000-0000-0000-000000000001")
        //        {
        //            this.Character.Color = Color.Orange;
        //            this.Character.CharacterName = "Character 1";
        //        }
        //        else if (this.UserID.ToString() == "00000000-0000-0000-0000-000000000002")
        //        {
        //            this.Character.Color = Color.Blue;
        //            this.Character.CharacterName = "Character 2";
        //        }
        //        else
        //        {
        //            this.Character.Color = Color.Pink;
        //            this.Character.CharacterName = "Character 3";
        //        }
        //    }


        //    this.Character.ID = characterID;


        //    //Sending the player...
        //    SendCommand(new LoadCharacterCommand() { Character = this.Character });


        //    Logguer.Info("AddGameObject: " + this.Character.ID + " " + this.UserID);

        //    _server.SpawnObject(this.Character);

        //    //Send objects on the scene...
        //    SendNewObjects();


        //    //Game is ready to go...
        //    SendCommand(new GameReadyCommand());




        //}

        /// <summary>
        /// Get prefix for logging
        /// </summary>
        private string GetPrefixLog()
        {
            long elapsedMs = _gameLoop.GameTimer.ElapsedMilliseconds;

            return elapsedMs.ToString("0000000") + " CW" + this.ID;
        }

        /// <summary>
        /// Log info
        /// </summary>
        public void LogInfo(string info)
        {
            Logguer.Info(GetPrefixLog() + " " + info);
        }

        /// <summary>
        /// Log error
        /// </summary>
        public void LogError(string detail)
        {
            Logguer.Error(GetPrefixLog() + " ERROR: " + detail);
        }

        /// <summary>
        /// Log error
        /// </summary>
        public void LogError(string detail, Exception ex)
        {
            Logguer.Error(GetPrefixLog() + " ERROR: " + detail, ex);
        }


        /// <summary>
        /// Information on an object sent to the client
        /// </summary>
        private class ClientWorkerGameObject
        {
            //public int Version = -1;
            public NetworkGameObject GameObject;
            public bool IsLocalPlayer;
        }

    }
}
