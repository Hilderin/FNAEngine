using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FNAEngine2D.Collisions
{
    ///// <summary>
    ///// Emplacement de la collision
    ///// </summary>
    //public enum Direction4
    //{
    //    Right,
    //    Left,
    //    Down,
    //    Up,
    //    MovingColliderOver,
    //    MovingColliderIn,
    //    Indetermined
    //}

    /// <summary>
    /// Résultat d'une collision
    /// </summary>
    public class Collision
    {
        /// <summary>
        /// CollidesWith
        /// </summary>
        public List<GameObject> CollidesWith = new List<GameObject>(1);

        /// <summary>
        /// Location where the object has been stopped
        /// </summary>
        public Vector2 StopLocation;

        /// <summary>
        /// Direction
        /// </summary>
        public Direction4 Direction;

        /// <summary>
        /// Moving collider
        /// </summary>
        public Collider MovingCollider { get; set; }

        /// <summary>
        /// Collision
        /// </summary>
        public Collision(Collider movingCollider, GameObject collidesWith, Direction4 direction, Vector2 stopLocation)
        {
            this.MovingCollider = movingCollider;
            this.CollidesWith.Add(collidesWith);
            this.Direction = direction;
            this.StopLocation = stopLocation;
        }

    }
}
