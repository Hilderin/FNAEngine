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
        /// Time at which the object begun to fall
        /// </summary>
        private float _timeBeginFall;

        /// <summary>
        /// List of forces applied
        /// </summary>
        private List<Force> _forces = new List<Force>();


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
        public Vector2 Movement { get; set; }

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
        }

        /// <summary>
        /// Update each frame
        /// </summary>
        protected override void Update()
        {
            

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
        /// Apply the physics
        /// </summary>
        public Vector2 GetNextLocation()
        {
            //-------------
            //Moving left/right
            Vector2 delta = Movement * (SpeedMps * this.GameObject.ElapsedGameTimeSeconds * this.GameObject.NbPixelPerMeter);



            //-------------
            //Gavity...
            if (GravityMps != 0)
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

    }
}
