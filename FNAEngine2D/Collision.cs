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
        Indetermined,
        None
    }

    /// <summary>
    /// Résultat d'une collision
    /// </summary>
    public class Collision
    {
        /// <summary>
        /// CollidesWith
        /// </summary>
        public ColliderRectangle CollidesWith;

        /// <summary>
        /// Direction d'où vient la collision
        /// </summary>
        public CollisionDirection Direction;

        /// <summary>
        /// Collision
        /// </summary>
        public Collision(ColliderRectangle collidesWith, CollisionDirection direction)
        {
            this.CollidesWith = collidesWith;
            this.Direction = direction;
        }

    }
}
