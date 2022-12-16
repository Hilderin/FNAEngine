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
        /// Empty collisions
        /// </summary>
        public readonly static List<Collision> EMPTY_COLLISIONS = new List<Collision>();


        /// <summary>
        /// Permet d'obtenir le résultat de la collision entre 2 rectangle
        /// </summary>
        public static Collision GetCollision(Rectangle movingCollider, Collider collidesWith)
        {
            Rectangle collidesWithRect = collidesWith.Bounds;

            if (!movingCollider.Intersects(collidesWithRect))
                return null;

            CollisionDirection direction = CollisionDirection.Indetermined;

            if (movingCollider.Contains(collidesWithRect))
            {
                direction = CollisionDirection.MovingColliderOver;
            }
            else if (collidesWithRect.Contains(movingCollider))
            {
                direction = CollisionDirection.MovingColliderIn;
            }
            else
            {
                if (movingCollider.Bottom >= collidesWithRect.Top && movingCollider.Bottom <= collidesWithRect.Bottom)
                {
                    //Objet est en haut
                    if (movingCollider.Top < collidesWithRect.Top)
                    {
                        //On va regarder si on va pas prioriser le côté...
                        if (movingCollider.Right >= collidesWithRect.Left && movingCollider.Right <= collidesWithRect.Right)
                        {
                            if (movingCollider.Right - collidesWithRect.Left < movingCollider.Bottom - collidesWithRect.Top)
                                //On va prioriser la gauche...
                                direction = CollisionDirection.MovingColliderOnLeft;
                            else
                                direction = CollisionDirection.MovingColliderOnTop;
                        }
                        else if (movingCollider.Left <= collidesWithRect.Right && movingCollider.Left >= collidesWithRect.Left)
                        {
                            //On a aussi une collision sur la droite...
                            if (collidesWithRect.Right - movingCollider.Left < movingCollider.Bottom - collidesWithRect.Top)
                                //On va prioriser la droite...
                                direction = CollisionDirection.MovingColliderOnRight;
                            else
                                direction = CollisionDirection.MovingColliderOnTop;
                        }
                        else
                        {
                            direction = CollisionDirection.MovingColliderOnTop;
                        }
                    }
                }

                if (direction == CollisionDirection.Indetermined)
                {
                    if (movingCollider.Top <= collidesWithRect.Bottom && movingCollider.Top >= collidesWithRect.Top)
                    {
                        //Objet est en bas
                        if (movingCollider.Bottom > collidesWithRect.Bottom)
                        {
                            //On va regarder si on va pas prioriser le côté...
                            if (movingCollider.Right >= collidesWithRect.Left && movingCollider.Right <= collidesWithRect.Right)
                            {
                                if (movingCollider.Right - collidesWithRect.Left < collidesWithRect.Bottom - movingCollider.Top)
                                    //On va prioriser la gauche...
                                    direction = CollisionDirection.MovingColliderOnLeft;
                                else
                                    direction = CollisionDirection.MovingColliderOnBottom;
                            }

                            if (movingCollider.Left <= collidesWithRect.Right && movingCollider.Left >= collidesWithRect.Left)
                            {
                                //On a aussi une collision sur la droite...
                                if (collidesWithRect.Right - movingCollider.Left < collidesWithRect.Bottom - movingCollider.Top)
                                    //On va prioriser la droite...
                                    direction = CollisionDirection.MovingColliderOnRight;
                                else
                                    direction = CollisionDirection.MovingColliderOnBottom;
                            }
                            else
                            {
                                direction = CollisionDirection.MovingColliderOnBottom;
                            }
                        }
                    }

                    if (direction == CollisionDirection.Indetermined)
                    {
                        if (movingCollider.Right >= collidesWithRect.Left && movingCollider.Right <= collidesWithRect.Right)
                        {
                            //Objet est en left
                            direction = CollisionDirection.MovingColliderOnLeft;
                        }
                        else if (movingCollider.Left <= collidesWithRect.Right && movingCollider.Left >= collidesWithRect.Left)
                        {
                            //Objet est en right
                            direction = CollisionDirection.MovingColliderOnRight;
                        }
                    }
                }

            }

            //En fonction de la direction on va déplacer le StopBounds
            Rectangle stopBounds;
            switch (direction)
            {
                case CollisionDirection.MovingColliderOnLeft:
                    //La balle est à gauche
                    stopBounds = new Rectangle(collidesWithRect.Left - movingCollider.Width, movingCollider.Top, movingCollider.Width, movingCollider.Height);
                    break;
                case CollisionDirection.MovingColliderOnRight:
                    //La balle est à droite
                    stopBounds = new Rectangle(collidesWithRect.Right, movingCollider.Top, movingCollider.Width, movingCollider.Height);
                    break;
                case CollisionDirection.MovingColliderOnTop:
                    //La balle est en haut
                    stopBounds = new Rectangle(movingCollider.Left, collidesWithRect.Top - movingCollider.Height, movingCollider.Width, movingCollider.Height);
                    break;
                case CollisionDirection.MovingColliderOnBottom:
                    //La balle est en bas
                    stopBounds = new Rectangle(movingCollider.Left, collidesWithRect.Bottom, movingCollider.Width, movingCollider.Height);
                    break;
                default:
                    stopBounds = movingCollider;
                    break;
            }


            return new Collision(collidesWith, direction, stopBounds);
        }

    }
}
