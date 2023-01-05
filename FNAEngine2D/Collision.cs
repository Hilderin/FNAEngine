using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
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
        ///// <summary>
        ///// CollidesWith
        ///// </summary>
        //public List<Collider> CollidesWith = new List<Collider>(1);

        /// <summary>
        /// CollidesWith
        /// </summary>
        public List<GameObject> CollidesWith = new List<GameObject>(1);

        ///// <summary>
        ///// Location where the object attempted to move
        ///// </summary>
        //public Vector2 AttemptedLocation;

        /// <summary>
        /// Location where the object has been stopped
        /// </summary>
        public Vector2 StopLocation;

        public Direction4 Direction;

        /// <summary>
        /// Collision
        /// </summary>
        public Collision(GameObject collidesWith, Direction4 direction, Vector2 stopLocation)
        {
            this.CollidesWith.Add(collidesWith);
            this.Direction = direction;
            this.StopLocation = stopLocation;
        }

    }
}
