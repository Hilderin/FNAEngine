using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Helper pour les collisions
    /// </summary>
    public static class CollisionHelper
    {
        /// <summary>
        /// Permet d'obtenir le résultat de la collision entre 2 rectangle
        /// </summary>
        public static CollisionDirection GetCollision(Rectangle movingCollider, Rectangle collidesWith)
        {
            if (!movingCollider.Intersects(collidesWith))
                return CollisionDirection.None;


            if (movingCollider.Bottom >= collidesWith.Top && movingCollider.Bottom <= collidesWith.Bottom)
            {
                //Objet est en haut, pas de collider ici
                return CollisionDirection.MovingColliderOnTop;
            }

            if (movingCollider.Top <= collidesWith.Bottom && movingCollider.Top >= collidesWith.Top)
            {
                //Objet est en haut, pas de collider ici
                return CollisionDirection.MovingColliderOnBottom;
            }



            bool left = true;
            bool right = true;
            bool top = true;
            bool bottom = true;

            if (collidesWith.Right < movingCollider.Left)
            {
                //Objet est à gauche, pas de collider ici
                left = false;
            }

            if (collidesWith.Left > movingCollider.Right)
            {
                //Objet est à droit, pas de collider ici
                right = false;
            }

            if (collidesWith.Bottom < movingCollider.Top)
            {
                //Objet est en haut, pas de collider ici
                top = false;
            }

            if (collidesWith.Top > movingCollider.Bottom)
            {
                //Objet est en bas, pas de collider ici
                bottom = false;
            }

            if (left)
                return CollisionDirection.MovingColliderOnLeft;
            if (right)
                return CollisionDirection.MovingColliderOnRight;
            if (top)
                return CollisionDirection.MovingColliderOnTop;
            if (bottom)
                return CollisionDirection.MovingColliderOnBottom;

            return CollisionDirection.None;
        }

    }
}
