using FNAEngine2D.SpaceTrees;
using Microsoft.Xna.Framework;
using System;

namespace FNAEngine2D.Collisions
{
    /// <summary>
    /// Collider
    /// </summary>
    public abstract class Collider: Component
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
        public virtual Vector2 OrignalMovingLocation { get; set; }

        /// <summary>
        /// Next moving location (can be updated along the way if multiple colliders are found)
        /// </summary>
        public virtual Vector2 MovingLocation { get; set; }

        /// <summary>
        /// Size
        /// </summary>
        public virtual Vector2 Size { get; set; }

        /// <summary>
        /// Link to the data node in the Space2DTree
        /// </summary>
        internal Space2DTreeNodeData<Collider> SpaceTreeDataNode;


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
        public abstract bool Intersects(Collider movingCollider);


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
