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
        /// <summary>
        /// CollidesWith
        /// </summary>
        public Collider CollidesWith;

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
        public Collision(Collider collidesWith, CollisionDirection direction, Vector2 stopLocation)
        {
            this.CollidesWith = collidesWith;
            this.Direction = direction;
            this.StopLocation = stopLocation;
        }

    }
}
