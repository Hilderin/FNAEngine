using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Force applied in a RigidBody
    /// </summary>
    public class Force
    {
        /// <summary>
        /// Total travel time
        /// </summary>
        private float _totalTravelTime;

        /// <summary>
        /// Total elapsed time
        /// </summary>
        private float _totalElapsedTimeSeconds = 0f;

        /// <summary>
        /// Parent game object
        /// </summary>
        private GameObject _gameObject;

        /// <summary>
        /// Last vector applied
        /// </summary>
        private Vector2 _lastVectorApplied = Vector2.Zero;

        /// <summary>
        /// Target
        /// </summary>
        public Vector2 Target { get; set; }

        /// <summary>
        /// Speed (m/s)
        /// </summary>
        public float SpeedMps { get; set; }

        /// <summary>
        /// Completed?
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Force(Vector2 target, float speedMps, GameObject gameObject)
        {
            this.Target = target;
            this.SpeedMps = speedMps;

            _gameObject = gameObject;

            //If we need to move for 100 pixels
            //with 30 pixels per seconds
            //it shound take 3.33 seconds
            //each second, we should move for 1 / 3.33 of the total path

            float totalDistanceInMeter = target.Length() / _gameObject.NbPixelPerMeter;
            _totalTravelTime = totalDistanceInMeter / this.SpeedMps;

        }

        /// <summary>
        /// Apply the force (with Game.ElapsedGameTimeSeconds)
        /// </summary>
        public Vector2 Apply(Vector2 vector)
        {
            return Apply(vector, _gameObject.ElapsedGameTimeSeconds);
        }

        /// <summary>
        /// Apply the force
        /// </summary>
        public Vector2 Apply(Vector2 vector, float elapsedGameTimeSeconds)
        {
            if (this.IsCompleted)
                return vector;

            
            _totalElapsedTimeSeconds += elapsedGameTimeSeconds;

            
            float amount = _totalElapsedTimeSeconds * (1 / _totalTravelTime);
            if (amount >= 1f)
            {
                amount = 1f;
                this.IsCompleted = true;
            }

            Vector2 newVector = Vector2.Lerp(Vector2.Zero, this.Target, amount);

            Vector2 ret = vector + (newVector - _lastVectorApplied);

            _lastVectorApplied = newVector;

            return ret;

        }



    }
}
