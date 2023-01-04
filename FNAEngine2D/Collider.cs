using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Collider
    /// </summary>
    public abstract class Collider: GameComponent
    {

        /// <summary>
        /// Location
        /// </summary>
        public virtual Vector2 Location { get { return this.GameObject.Location; } }

        /// <summary>
        /// Next moving location
        /// </summary>
        public virtual Vector2 MovingLocation { get; set; }

        ///// <summary>
        ///// Size
        ///// </summary>
        //public virtual Vector2 Size { get { return this.GameObject.Size; } }


        /// <summary>
        /// Collider rectangle form
        /// </summary>
        public Collider()
        {
        }

        ///// <summary>
        ///// Collider rectangle form
        ///// </summary>
        //public Collider(GameObject gameObject): base(gameObject)
        //{
        //}

        ///// <summary>
        ///// Collider rectangle form
        ///// </summary>
        //public Collider(Rectangle bounds)
        //{
        //    this.GameObject = new GameObject();
        //    this.GameObject.Bounds = bounds;
        //}

        ///// <summary>
        ///// Collider rectangle form
        ///// </summary>
        //public Collider(int x, int y, int width, int height) : this(new Rectangle(x, y, width, height))
        //{

        //}


        /// <summary>
        /// Check if the collider intersects with a collider
        /// </summary>
        public abstract bool Intersects(Collider movingCollider, out CollisionDirection direction, out Vector2 hitLocation);


        /// <summary>
        /// Adding of a collider
        /// </summary>
        public override void OnAdded()
        {
            this.GameObject.Game.ColliderContainer.Add(this);
        }

        /// <summary>
        /// Removing of a collider
        /// </summary>
        public override void OnRemoved()
        {
            this.GameObject.Game.ColliderContainer.Remove(this);
        }

    }
}
