using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Circle Collider
    /// </summary>
    public class ColliderCircle : Collider
    {
        /// <summary>
        /// Moving location
        /// </summary>
        private Vector2 _movingLocation;

        /// <summary>
        /// Moving center location
        /// </summary>
        private Vector2 _centerMovingLocation;

        /// <summary>
        /// Location
        /// </summary>
        public override Vector2 Location { get { return this.GameObject.Location + this.CenterOffset; } }

        /// <summary>
        /// Offset location from the game object for the center
        /// </summary>
        public virtual Vector2 CenterOffset { get; set; }

        /// <summary>
        /// Next moving location
        /// </summary>
        public override Vector2 MovingLocation
        {
            get { return _movingLocation; }
            set
            {
                _movingLocation = value;
                _centerMovingLocation = value + this.CenterOffset;
            }
        }

        /// <summary>
        /// Offset location from the game object for the center
        /// </summary>
        public virtual Vector2 CenterMovingLocation { get { return _centerMovingLocation; } }

        /// <summary>
        /// Radius
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// Collider rectangle form
        /// </summary>
        public ColliderCircle(Vector2 centerOffset, float radius)
        {
            this.CenterOffset = centerOffset;
            this.Radius = radius;
        }

        ///// <summary>
        ///// Collider rectangle form
        ///// </summary>
        //public ColliderCircle(GameObject gameObject): base(gameObject)
        //{
        //}



        /// <summary>
        /// Check if the collider intersects with a rectangle
        /// </summary>
        public override bool Intersects(Collider movingCollider, out CollisionDirection direction, out Vector2 hitLocation)
        {
            if (movingCollider is ColliderRectangle)
            {
                //With another rectangle...
                return CollisionHelper.Intersects(this.Location, this.Radius, movingCollider.MovingLocation, ((ColliderRectangle)movingCollider).Size, out direction, out hitLocation);
            }
            else if (movingCollider is ColliderCircle)
            {
                //With a circle..
                if (CollisionHelper.Intersects(((ColliderCircle)movingCollider).CenterMovingLocation, ((ColliderCircle)movingCollider).Radius, this.Location, this.Radius, out direction, out hitLocation))
                {
                    hitLocation -= ((ColliderCircle)movingCollider).CenterOffset;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                throw new NotSupportedException("Collider type not supported: " + movingCollider.GetType().FullName);
            }
        }
    }
}
