using FNAEngine2D.Network.Commands;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Object simulating physic
    /// </summary>
    public class RigidBody: GameComponent
    {
        /// <summary>
        /// If the GameObject is a NetworkGameObject
        /// </summary>
        private NetworkGameObject _networkGameObject;

        /// <summary>
        /// Time at which the object begun to fall
        /// </summary>
        private float _timeBeginFall;

        /// <summary>
        /// List of forces applied
        /// </summary>
        private List<Force> _forces = new List<Force>();

        /// <summary>
        /// Current movement
        /// </summary>
        private Vector2 _movement = Vector2.Zero;

        /// <summary>
        /// Next movement to to
        /// </summary>
        private Vector2? _nextMovement = null;

        /// <summary>
        /// Next start location
        /// </summary>
        private Vector2? _nextStartPosition = null;

        /// <summary>
        /// Use gravity
        /// </summary>  
        public bool UseGravity { get; set; }

        /// <summary>
        /// The gravity (meter per second) (default Earth gravirty is 9.81 m/s)
        /// It's a acceleration force
        /// </summary>
        public float GravityMps { get; set; } = 9.81f;

        /// <summary>
        /// Moving speed meter per second
        /// </summary>
        public float SpeedMps { get; set; } = 1;

        /// <summary>
        /// Normalized vector for the movement
        /// </summary>
        public Vector2 Movement
        {
            get { return _movement; }
            set
            {
                SetMovement(value);
            }
        }

        /// <summary>
        /// Type with which the rigid body collide. If null, then all collider will be used.
        /// </summary>
        public Type[] ColliderTypes { get; set; } = null;

        /// <summary>
        /// Previous location
        /// </summary>
        public Vector2 LastLocation { get; private set; }

        /// <summary>
        /// Current collision
        /// </summary>
        public Collision Collistion { get; private set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public RigidBody()
        {
        }

        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            LastLocation = this.GameObject.Location;

            //_networkGameObject will be set if the game object is a network game object
            _networkGameObject = this.GameObject as NetworkGameObject;
        }

        /// <summary>
        /// Update each frame
        /// </summary>
        protected override void Update()
        {
            //Applying next movement and location...
            if (_nextMovement != null)
                ApplyNextMovement();

            //Applying physics...
            Vector2 nextPosition = GetNextLocation();


            //Check for collision...
            this.Collistion = this.GameObject.GetCollision(nextPosition, this.ColliderTypes);
            if (this.Collistion != null)
            {
                nextPosition = this.Collistion.StopLocation;
            }

            LastLocation = this.GameObject.Location;
            this.GameObject.TranslateTo(nextPosition);

            
        }

        /// <summary>
        /// Add a force
        /// </summary>
        public Force AddForce(Vector2 target, float speedMps)
        {
            Force force = new Force(target, speedMps, this.GameObject);

            _forces.Add(force);

            return force;
        }

        /// <summary>
        /// Set the current movement
        /// </summary>
        public void SetMovement(Vector2 movement)
        {
            //Be sure to normalize... for hackers maybe if triing to send longer movement or if caller did not normalized
            if (movement != Vector2.Zero)
                movement.Normalize();

            if (_movement != movement)
            {
                //Sending movement to the server or if we are on the server... to all the other clients
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
        internal void SetNextMovement(Vector2 movement, Vector2 startPosition)
        {
            //We queue... because we are already moving...
            lock (this)
            {
                _nextMovement = movement;
                _nextStartPosition = startPosition;
            }
        }

        /// <summary>
        /// Apply the physics
        /// </summary>
        public Vector2 GetNextLocation()
        {
            //-------------
            //Moving left/right
            Vector2 delta = Movement * (SpeedMps * this.GameObject.ElapsedGameTimeSeconds * this.GameObject.NbPixelPerMeter);



            //-------------
            //Gavity...
            if (this.UseGravity)
            {
                if (LastLocation.Y >= this.GameObject.Location.Y)
                    //Did not fall...
                    _timeBeginFall = 0f;

                _timeBeginFall += this.GameObject.ElapsedGameTimeSeconds;
                float acceleration = GravityMps * _timeBeginFall;

                delta.Y = acceleration * this.GameObject.ElapsedGameTimeSeconds * this.GameObject.NbPixelPerMeter;
            }


            //------------
            //Other forces...
            if (_forces.Count > 0)
            {
                for (int index = _forces.Count - 1; index >= 0; index--)
                {
                    //Applying force...
                    delta = _forces[index].Apply(delta);

                    //If done, remove it..
                    if (_forces[index].IsCompleted)
                        _forces.RemoveAt(index);
                }
            }            

            return this.GameObject.Location + delta;

        }


        /// <summary>
        /// Update movement of an object
        /// </summary>
        private void ApplyNextMovement()
        {
            if (_nextMovement != null)
            {
                Vector2? nextMovement;
                Vector2? nextStartPosition;
                lock (this)
                {
                    nextMovement = _nextMovement;
                    nextStartPosition = _nextStartPosition;
                    _nextMovement = null;
                    _nextStartPosition = null;
                }

                if (nextMovement != null)
                {
                    this.GameObject.Location = nextStartPosition.Value;

                    if (_movement != nextMovement.Value)
                    {
                        //Update movement...
                        _movement = nextMovement.Value;
                    }
                }
            }
        }

    }
}
