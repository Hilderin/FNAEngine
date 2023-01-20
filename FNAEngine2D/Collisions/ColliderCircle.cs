using Microsoft.Xna.Framework;
using System;

namespace FNAEngine2D.Collisions
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
        /// Current center location
        /// </summary>
        private Vector2 _centerLocation;

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
                _centerLocation = this.Location + value;
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
        /// Center location
        /// </summary>
        public Vector2 CenterLocation { get { return _centerLocation; } }

        /// <summary>
        /// Center location of the moving location
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
            this.GameObject = new EmptyGameObject();
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
        /// Object moved
        /// </summary>
        protected override void OnMoved()
        {
            _centerLocation = this.Location + _centerOffset;

            base.OnMoved();
        }


        /// <summary>
        /// Check if the collider intersects with a rectangle
        /// </summary>
        public override bool Intersects(Collider movingCollider)
        {
            if (movingCollider is ColliderRectangle)
            {
                //With a rectangle...
                //We simulate the previous position
                //TODO: Create a fonction to calculate the correct stopLocation with a circle and a rectangle
                //return CollisionHelper.Intersects(_centerMovingLocation - (movingCollider.MovingLocation - movingCollider.Location), _centerMovingLocation, this.Radius, movingCollider.MovingLocation, movingCollider.Size, ref hitLocation);
                return CollisionHelper.Intersects(_centerLocation, this.Radius, movingCollider.MovingLocation, movingCollider.Size);
            }
            else if (movingCollider is ColliderCircle)
            {
                //With a circle..
                ColliderCircle colliderCircle = (ColliderCircle)movingCollider;
                //if (CollisionHelper.Intersects(colliderCircle.CenterLocation, colliderCircle.CenterMovingLocation, colliderCircle.Radius, _centerMovingLocation, this.Radius, ref hitLocation))
                if (CollisionHelper.Intersects(colliderCircle.CenterMovingLocation, colliderCircle.Radius, _centerLocation, this.Radius))
                {
                    //hitLocation -= ((ColliderCircle)movingCollider).CenterOffset;
                    //Console.WriteLine("collide " + ((NetworkGameObject)this.GameObject).ID + " " + ((NetworkGameObject)movingCollider.GameObject).ID + " " + this.CenterLocation + " " + colliderCircle.CenterMovingLocation);

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
