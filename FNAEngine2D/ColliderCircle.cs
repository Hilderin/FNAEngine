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
        /// Offset du center
        /// </summary>
        private Vector2 _centerOffset;

        /// <summary>
        /// Size
        /// </summary>
        private Vector2 _size;

        /// <summary>
        /// Radius
        /// </summary>
        private float _radius;

        /// <summary>
        /// Offset location from the game object for the center
        /// </summary>
        public virtual Vector2 CenterOffset
        {
            get { return _centerOffset; }
            set
            {
                _centerOffset = value;
                _centerMovingLocation = _movingLocation + value;
            }
        }

        /// <summary>
        /// Next moving location
        /// </summary>
        public override Vector2 MovingLocation
        {
            get { return _movingLocation; }
            set
            {
                _movingLocation = value;
                _centerMovingLocation = value + _centerOffset;
            }
        }

        /// <summary>
        /// Offset location from the game object for the center
        /// </summary>
        public Vector2 CenterMovingLocation { get { return _centerMovingLocation; } }

        /// <summary>
        /// Size
        /// </summary>
        public override Vector2 Size { get { return _size; } }

        /// <summary>
        /// Radius
        /// </summary>
        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                _size = new Vector2(value * 2, value * 2);
            }
        }

        /// <summary>
        /// Collider rectangle form
        /// </summary>
        public ColliderCircle(Vector2 location, Vector2 centerOffset, float radius)
        {
            this.GameObject = new GameObject();
            this.GameObject.Location = location;
            this.Location = location;
            this.MovingLocation = location;

            this.CenterOffset = centerOffset;
            this.Radius = radius;
        }

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
        public override bool Intersects(Collider movingCollider, ref CollisionDirection direction, ref Vector2 hitLocation)
        {
            if (movingCollider is ColliderRectangle)
            {
                //With another rectangle...
                return CollisionHelper.Intersects(_centerMovingLocation, this.Radius, movingCollider.MovingLocation, movingCollider.Size, ref direction, ref hitLocation);
            }
            else if (movingCollider is ColliderCircle)
            {
                //With a circle..
                if (CollisionHelper.Intersects(((ColliderCircle)movingCollider).CenterMovingLocation, ((ColliderCircle)movingCollider).Radius, _centerMovingLocation, this.Radius, ref direction, ref hitLocation))
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
