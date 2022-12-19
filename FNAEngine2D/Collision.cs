using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Emplacement de la collision
    /// </summary>
    public enum CollisionDirection
    {
        MovingColliderOnLeft,
        MovingColliderOnRight,
        MovingColliderOnTop,
        MovingColliderOnBottom,
        MovingColliderOver,
        MovingColliderIn,
        Indetermined
    }

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

        /// <summary>
        /// Direction d'où vient la collision
        /// </summary>
        public CollisionDirection Direction;

        /// <summary>
        /// Bounds là où l'objet a faire la collision.
        /// </summary>
        public Vector2 StopLocation;

        /// <summary>
        /// Collision
        /// </summary>
        public Collision(GameObject collidesWith, CollisionDirection direction, Vector2 stopLocation)
        {
            this.CollidesWith.Add(collidesWith);
            this.Direction = direction;
            this.StopLocation = stopLocation;
        }

    }
}
