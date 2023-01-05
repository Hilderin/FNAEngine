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
        /// Added on the scene?
        /// </summary>
        private bool _added = false;

        /// <summary>
        /// Location
        /// </summary>
        public Vector2 Location { get; set; }

        /// <summary>
        /// Next moving location
        /// </summary>
        public virtual Vector2 MovingLocation { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        public virtual Vector2 Size { get; set; }


        /// <summary>
        /// Collider rectangle form
        /// </summary>
        public Collider()
        {
        }

        /// <summary>
        /// On load
        /// </summary>
        protected override void Load()
        {
        }

        /// <summary>
        /// Called when the game object has moved
        /// </summary>
        protected override void OnMoved()
        {
            UpdateLocationAndSize();
        }

        /// <summary>
        /// Called when the game object is resized
        /// </summary>
        protected override void OnResized()
        {
            UpdateLocationAndSize();
        }

        /// <summary>
        /// Update the location and size of the collider from the game object
        /// </summary>
        private void UpdateLocationAndSize()
        {
            if (!_added)
                return;

            if (this.Location != this.GameObject.Location || this.Size != this.GameObject.Size)
            {
                this.Location = this.GameObject.Location;
                this.Size = this.GameObject.Size;
                this.GameObject.Game.ColliderContainer.Update(this);
            }
        }


        /// <summary>
        /// Check if the collider intersects with a collider
        /// </summary>
        public abstract bool Intersects(Collider movingCollider, ref CollisionDirection direction, ref Vector2 hitLocation);


        /// <summary>
        /// Adding of a collider
        /// </summary>
        protected override void OnAdded()
        {
            
            this.Location = this.GameObject.Location;
            this.Size = this.GameObject.Size;

            this.GameObject.Game.ColliderContainer.Add(this);

            _added = true;

        }

        /// <summary>
        /// Removing of a collider
        /// </summary>
        protected override void OnRemoved()
        {
            this.GameObject.Game.ColliderContainer.Remove(this);
            _added = false;
        }

    }
}
