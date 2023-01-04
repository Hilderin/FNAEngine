using FNAEngine2D.Network;
using FNAEngine2D.Network.Commands;
using Microsoft.Xna.Framework;

namespace FNAEngine2D.Components
{
    public class MovementComponent: GameComponent
    {
        /// <summary>
        /// If the GameObject is a NetworkGameObject
        /// </summary>
        private NetworkGameObject _networkGameObject;

        /// <summary>
        /// Next movement to to
        /// </summary>
        private MovementInfo _nextMovement = null;

        /// <summary>
        /// Current movement
        /// </summary>
        private Vector2 _movement = Vector2.Zero;

        /// <summary>
        /// Current movement
        /// </summary>
        public Vector2 Movement { get { return _movement; } }

        /// <summary>
        /// Loading
        /// </summary>
        public override void Load()
        {
            _networkGameObject = this.GameObject as NetworkGameObject;
        }

        /// <summary>
        /// Set the current movement
        /// </summary>
        public void SetMovement(Vector2 movement)
        {
            //Be sure to normalize... for hackers maybe if triing to send longer movement or if caller did not normalized
            if(movement != Vector2.Zero)
                movement.Normalize();

            if (_movement != movement)
            {
                if (_networkGameObject != null)
                {
                    if (_networkGameObject.IsServer)
                    {
                        //Sending movement to all clients...
                        _networkGameObject.Server.SendCommandToAllClients(new MovementCommand() { ID = _networkGameObject.ID, Movement = movement, StartPosition = this.GameObject.Location });
                    }
                    else if (_networkGameObject.IsLocalPlayer)
                    {
                        //Sending movement to server...
                        _networkGameObject.Client.SendCommand(new MovementCommand() { ID = _networkGameObject.ID, Movement = movement, StartPosition = this.GameObject.Location });
                    }
                }

                _movement = movement;
            }
        }

        /// <summary>
        /// Set the next movement
        /// </summary>
        public void SetNextMovement(Vector2 movement, Vector2 startPosition)
        {
            //We queue... because we are already moving...
            lock (this)
            {
                _nextMovement = new MovementInfo(movement, startPosition);
            }
        }

        /// <summary>
        /// Update movement of an object
        /// </summary>
        public Vector2 GetMovement()
        {
            if (_nextMovement != null)
            {
                MovementInfo nextMovement;
                lock (this)
                {
                    nextMovement = _nextMovement;
                    _nextMovement = null;
                }

                if (nextMovement != null)
                {
                    this.GameObject.Location = nextMovement.StartPosition;

                    if (_movement != nextMovement.Movement)
                    {
                        //Update movement...
                        _movement = nextMovement.Movement;

                        //To be saved!
                        //_gameObject.Version++;
                    }
                }
            }

            return _movement;
        }

    }

    /// <summary>
    /// Information on the next movement
    /// </summary>
    public class MovementInfo
    {
        /// <summary>
        /// Movement to do
        /// </summary>
        public Vector2 Movement { get; set; }

        /// <summary>
        /// Starting position
        /// </summary>
        public Vector2 StartPosition { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MovementInfo(Vector2 movement, Vector2 startPosition)
        {
            this.Movement = movement;
            this.StartPosition = startPosition;
        }
    }
}
