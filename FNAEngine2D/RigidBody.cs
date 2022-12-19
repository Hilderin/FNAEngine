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
    public class RigidBody
    {
        /// <summary>
        /// Attached game object
        /// </summary>
        private GameObject _gameObject;

        /// <summary>
        /// Last location
        /// </summary>
        private Vector2 _lastLocation;

        /// <summary>
        /// Time at which the object begun to fall
        /// </summary>
        private float _timeBeginFall;

        /// <summary>
        /// List of forces applied
        /// </summary>
        private List<Force> _forces = new List<Force>();

        /// <summary>
        /// Object attached to
        /// </summary>
        public GameObject GameObject { get { return _gameObject; } }

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
        /// Indicate if object is moving left
        /// </summary>
        public bool IsMovingLeft { get; set; }

        /// <summary>
        /// Indicate if object is moving right
        /// </summary>
        public bool IsMovingRight { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public RigidBody(GameObject gameObject)
        {
            _gameObject = gameObject;
            _lastLocation = gameObject.Location;
        }

        /// <summary>
        /// Add a force
        /// </summary>
        public Force AddForce(Vector2 target, float speedMps)
        {
            Force force = new Force(target, speedMps);

            _forces.Add(force);

            return force;
        }

        /// <summary>
        /// Apply the physics
        /// </summary>
        public Vector2 ApplyPhysics()
        {
            //-------------
            //Moving left/right
            Vector2 delta = Vector2.Zero;
            
            if(IsMovingLeft)
                delta.X -= SpeedMps * GameHost.ElapsedGameTimeSeconds * GameHost.NbPixelPerMeter;
            if (IsMovingRight)
                delta.X += SpeedMps * GameHost.ElapsedGameTimeSeconds * GameHost.NbPixelPerMeter;


            //-------------
            //Gavity...

            if (_lastLocation.Y >= _gameObject.Location.Y)
                //Did not fall...
                _timeBeginFall = 0f;

            _timeBeginFall += GameHost.ElapsedGameTimeSeconds;
            float acceleration = GravityMps * _timeBeginFall;

            delta.Y = acceleration * GameHost.ElapsedGameTimeSeconds * GameHost.NbPixelPerMeter;

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

            _lastLocation = _gameObject.Location;

            return _gameObject.Location + delta;

        }

    }
}
