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
        /// Number of pixels minimum for distance travelled for calculation
        /// </summary>
        public static float MinimumDistanceCalculation = 5f;

        /// <summary>
        /// Empty collisions
        /// </summary>
        public readonly static List<Collision> EMPTY_COLLISIONS = new List<Collision>();


        /// <summary>
        /// Permet d'obtenir le résultat de la collision entre 2 rectangle
        /// </summary>
        public static bool GetCollisionTravel(Vector2 movingColliderOriginLocation, Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider collidesWith, ref Collision collision)
        {
            Vector2 movingColliderDestinationLocation;
            //We start from the other collider...
            if (collision != null)
                movingColliderDestinationLocation = collision.StopLocation;
            else
                movingColliderDestinationLocation = movingColliderLocation;


            float distance = (movingColliderDestinationLocation - movingColliderOriginLocation).Length();

            //If we are good with the distance, we will not split it...
            if (distance <= MinimumDistanceCalculation)
                return GetCollision(movingColliderLocation, movingColliderSize, collidesWith, ref collision);

            int nbLerp = (int)Math.Ceiling(distance / MinimumDistanceCalculation);
            float incrementAmount = 1f / nbLerp;
            float amount = 0;
            bool collided = false;
            for (int index = 1; index < nbLerp; index++)
            {
                amount += incrementAmount;
                Vector2 movingColliderLocationLerped = Vector2.Lerp(movingColliderOriginLocation, movingColliderDestinationLocation, amount);

                Collision stepCollision = null;
                if (GetCollision(movingColliderLocationLerped, movingColliderSize, collidesWith, ref stepCollision))
                {
                    collided = true;
                    if (stepCollision.StopLocation == movingColliderDestinationLocation)
                    {
                        //Cannot move anymore...
                        collision = stepCollision;
                        return true;
                    }

                    if (stepCollision.StopLocation.X != movingColliderLocationLerped.X)
                    {
                        //Cannot move on X...
                        movingColliderLocation.X = stepCollision.StopLocation.X;
                        movingColliderDestinationLocation.X = stepCollision.StopLocation.X;
                    }
                    if (stepCollision.StopLocation.Y != movingColliderLocationLerped.Y)
                    {
                        //Cannot move on Y...
                        movingColliderLocation.Y = stepCollision.StopLocation.Y;
                        movingColliderDestinationLocation.Y = stepCollision.StopLocation.Y;
                    }
                }

            }

            //Check the last one at destination...
            if (GetCollision(movingColliderDestinationLocation, movingColliderSize, collidesWith, ref collision))
                collided = true;

            return collided;

        }

        /// <summary>
        /// Permet d'obtenir le résultat de la collision entre 2 rectangle
        /// </summary>
        public static Collision GetCollision(Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider collidesWith)
        {
            Collision collision = null;
            GetCollision(movingColliderLocation, movingColliderSize, collidesWith, ref collision);
            return collision;
        }

        /// <summary>
        /// Permet d'obtenir le résultat de la collision entre 2 rectangle
        /// </summary>
        public static bool GetCollision(Vector2 movingColliderLocation, Vector2 movingColliderSize, Collider collidesWith, ref Collision collision)
        {

            //We start from the other collider...
            if (collision != null)
                movingColliderLocation = collision.StopLocation;


            //Collision??
            if (!VectorHelper.Intersects(movingColliderLocation, movingColliderSize, collidesWith.Location, collidesWith.Size))
                return false;

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
                            else if (movingColliderLocation.X <= collidesWithRectRight && movingColliderLocation.X >= collidesWithLocation.X)
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
                                //Vraiment juste en bas...
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

            //Returning collision....
            if (collision != null)
            {
                //Already a collision...
                collision.CollidesWith.Add(collidesWith.GameObject);
                collision.StopLocation = stopLocation;
            }
            else
            {
                //New collision...
                collision = new Collision(collidesWith.GameObject, direction, stopLocation);
            }

            return true;
        }

    }
}
