using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
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

            if (movingCollider.Contains(collidesWith))
                return CollisionDirection.MovingColliderOver;

            if (collidesWith.Contains(movingCollider))
                return CollisionDirection.MovingColliderIn;

            if (movingCollider.Bottom >= collidesWith.Top && movingCollider.Bottom <= collidesWith.Bottom)
            {
                //Objet est en haut
                if (movingCollider.Top < collidesWith.Top)
                {
                    //On va regarder si on va pas prioriser le côté...
                    if (movingCollider.Right >= collidesWith.Left && movingCollider.Right <= collidesWith.Right)
                    {
                        if (movingCollider.Right - collidesWith.Left < movingCollider.Bottom - collidesWith.Top)
                            //On va prioriser la gauche...
                            return CollisionDirection.MovingColliderOnLeft;
                    }

                    if (movingCollider.Left <= collidesWith.Right && movingCollider.Left >= collidesWith.Left)
                    {
                        //On a aussi une collision sur la droite...
                        if (collidesWith.Right - movingCollider.Left < movingCollider.Bottom - collidesWith.Top)
                            //On va prioriser la droite...
                            return CollisionDirection.MovingColliderOnRight;
                    }

                    return CollisionDirection.MovingColliderOnTop;
                }
            }

            if (movingCollider.Top <= collidesWith.Bottom && movingCollider.Top >= collidesWith.Top)
            {
                //Objet est en bas
                if (movingCollider.Bottom > collidesWith.Bottom)
                {
                    //On va regarder si on va pas prioriser le côté...
                    if (movingCollider.Right >= collidesWith.Left && movingCollider.Right <= collidesWith.Right)
                    {
                        if(movingCollider.Right - collidesWith.Left < collidesWith.Bottom - movingCollider.Top)
                            //On va prioriser la gauche...
                            return CollisionDirection.MovingColliderOnLeft;
                    }
                    
                    if (movingCollider.Left <= collidesWith.Right && movingCollider.Left >= collidesWith.Left)
                    {
                        //On a aussi une collision sur la droite...
                        if(collidesWith.Right - movingCollider.Left < collidesWith.Bottom - movingCollider.Top)
                            //On va prioriser la droite...
                            return CollisionDirection.MovingColliderOnRight;
                    }

                    return CollisionDirection.MovingColliderOnBottom;
                }
            }

            if (movingCollider.Right >= collidesWith.Left && movingCollider.Right <= collidesWith.Right)
            {
                //Objet est en left
                return CollisionDirection.MovingColliderOnLeft;
            }

            if (movingCollider.Left <= collidesWith.Right && movingCollider.Left >= collidesWith.Left)
            {
                //Objet est en right
                return CollisionDirection.MovingColliderOnRight;
            }



            //bool left = true;
            //bool right = true;
            //bool top = true;
            //bool bottom = true;

            //if (collidesWith.Right < movingCollider.Left)
            //{
            //    //Objet est à gauche, pas de collider ici
            //    left = false;
            //}

            //if (collidesWith.Left > movingCollider.Right)
            //{
            //    //Objet est à droit, pas de collider ici
            //    right = false;
            //}

            //if (collidesWith.Bottom < movingCollider.Top)
            //{
            //    //Objet est en haut, pas de collider ici
            //    top = false;
            //}

            //if (collidesWith.Top > movingCollider.Bottom)
            //{
            //    //Objet est en bas, pas de collider ici
            //    bottom = false;
            //}

            //if (left)
            //    return CollisionDirection.MovingColliderOnLeft;
            //if (right)
            //    return CollisionDirection.MovingColliderOnRight;
            //if (top)
            //    return CollisionDirection.MovingColliderOnTop;
            //if (bottom)
            //    return CollisionDirection.MovingColliderOnBottom;

            return CollisionDirection.Indetermined;
        }

    }
}
