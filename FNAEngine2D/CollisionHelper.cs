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
        public static Collision GetCollision(Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider collidesWith)
        {
           
            //Collision??
            if (!VectorHelper.Intersects(movingColliderLocation, movingColliderSize, collidesWith.Location, collidesWith.Size))
                return null;

            CollisionDirection direction = CollisionDirection.Indetermined;

            Vector2 stopLocation;

            if (VectorHelper.Contains(movingColliderLocation, movingColliderSize, collidesWith.Location, collidesWith.Size))
            {
                direction = CollisionDirection.MovingColliderOver;
                stopLocation = movingColliderLocation;
            }
            else if (VectorHelper.Contains(collidesWith.Location, collidesWith.Size, movingColliderLocation, movingColliderSize))
            {
                direction = CollisionDirection.MovingColliderIn;
                stopLocation = movingColliderLocation;
            }
            else
            {
                //Somewhere else...
                Vector2 collidesWithLocation = collidesWith.Location;
                float collidesWithRectRight = collidesWithLocation.X + collidesWith.Size.X;
                float collidesWithRectBottom = collidesWithLocation.Y + collidesWith.Size.Y;

                float movingColliderRight = movingColliderLocation.X + movingColliderSize.X;
                float movingColliderBottom = movingColliderLocation.Y + movingColliderSize.Y;

                if (movingColliderBottom >= collidesWithLocation.Y && movingColliderBottom <= collidesWithRectBottom)
                {
                    //Objet est en haut
                    if (movingColliderLocation.Y < collidesWithLocation.Y)
                    {
                        //On va regarder si on va pas prioriser le côté...
                        if (movingColliderRight >= collidesWithLocation.X && movingColliderRight <= collidesWithRectRight)
                        {
                            if (movingColliderRight - collidesWithLocation.X < movingColliderBottom - collidesWithLocation.Y)
                                //On va prioriser la gauche...
                                direction = CollisionDirection.MovingColliderOnLeft;
                            else
                                direction = CollisionDirection.MovingColliderOnTop;
                        }
                        else if (movingColliderLocation.X <= collidesWithRectRight && movingColliderLocation.X >= collidesWithLocation.X)
                        {
                            //On a aussi une collision sur la droite...
                            if (collidesWithRectRight - movingColliderLocation.X < movingColliderBottom - collidesWithLocation.Y)
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
                    if (movingColliderLocation.Y <= collidesWithRectBottom && movingColliderLocation.Y >= collidesWithLocation.Y)
                    {
                        //Objet est en bas
                        if (movingColliderBottom > collidesWithRectBottom)
                        {
                            //On va regarder si on va pas prioriser le côté...
                            if (movingColliderRight >= collidesWithLocation.X && movingColliderRight <= collidesWithRectRight)
                            {
                                if (movingColliderRight - collidesWithLocation.X < collidesWithRectBottom - movingColliderLocation.Y)
                                    //On va prioriser la gauche...
                                    direction = CollisionDirection.MovingColliderOnLeft;
                                else
                                    direction = CollisionDirection.MovingColliderOnBottom;
                            }

                            if (movingColliderLocation.X <= collidesWithRectRight && movingColliderLocation.X >= collidesWithLocation.X)
                            {
                                //On a aussi une collision sur la droite...
                                if (collidesWithRectRight - movingColliderLocation.X < collidesWithRectBottom - movingColliderLocation.Y)
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
                        if (movingColliderRight >= collidesWithLocation.X && movingColliderRight <= collidesWithRectRight)
                        {
                            //Objet est en left
                            direction = CollisionDirection.MovingColliderOnLeft;
                        }
                        else if (movingColliderLocation.X <= collidesWithRectRight && movingColliderLocation.X >= collidesWithLocation.X)
                        {
                            //Objet est en right
                            direction = CollisionDirection.MovingColliderOnRight;
                        }
                    }
                }

                //En fonction de la direction on va déplacer le StopBounds
                switch (direction)
                {
                    case CollisionDirection.MovingColliderOnLeft:
                        //La balle est à gauche
                        stopLocation = new Vector2(collidesWithLocation.X - movingColliderSize.X, movingColliderLocation.Y);
                        break;
                    case CollisionDirection.MovingColliderOnRight:
                        //La balle est à droite
                        stopLocation = new Vector2(collidesWithRectRight, movingColliderLocation.Y);
                        break;
                    case CollisionDirection.MovingColliderOnTop:
                        //La balle est en haut
                        stopLocation = new Vector2(movingColliderLocation.X, collidesWithLocation.Y - movingColliderSize.Y);
                        break;
                    case CollisionDirection.MovingColliderOnBottom:
                        //La balle est en bas
                        stopLocation = new Vector2(movingColliderLocation.X, collidesWithRectBottom);
                        break;
                    default:
                        stopLocation = movingColliderLocation;
                        break;
                }

            }

            

            return new Collision(collidesWith, direction, stopLocation);
        }

    }
}
