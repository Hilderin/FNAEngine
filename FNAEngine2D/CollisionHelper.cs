using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private const float PiThreeQuarter = (float)Math.PI * 0.75f;
        private const float MinusPiThreeQuarter = (float)Math.PI * -0.75f;
        private const float PiQuarter = (float)Math.PI * 0.25f;
        private const float MinusPiQuarter = (float)Math.PI * -0.25f;

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
        public static bool GetCollisionTravel(Collider movingCollider, Collider collidesWith, ref Collision collision)
        {
            Vector2 movingColliderOriginLocation = movingCollider.Location;
            Vector2 movingColliderDestinationLocation;
            //We start from the other collider...
            if (collision != null)
                movingColliderDestinationLocation = collision.StopLocation;
            else
                movingColliderDestinationLocation = movingCollider.MovingLocation;


            float distance = (movingColliderDestinationLocation - movingColliderOriginLocation).Length();

            //If we are good with the distance, we will not split it...
            if (distance <= MinimumDistanceCalculation)
                return GetCollision(movingCollider, collidesWith, ref collision);

            int nbLerp = (int)Math.Ceiling(distance / MinimumDistanceCalculation);
            float incrementAmount = 1f / nbLerp;
            float amount = 0;
            bool collided = false;
            Collision lastCollision = null;

            for (int index = 1; index < nbLerp; index++)
            {
                amount += incrementAmount;
                Vector2 movingColliderLocationLerped = Vector2.Lerp(movingColliderOriginLocation, movingColliderDestinationLocation, amount);
                movingCollider.MovingLocation = movingColliderLocationLerped;
                Collision stepCollision = null;
                if (GetCollision(movingCollider, collidesWith, ref stepCollision))
                {
                    if (stepCollision.StopLocation == movingColliderLocationLerped)
                    {
                        //Cannot move anymore...
                        collision = stepCollision;
                        return true;
                    }

                    if (stepCollision.StopLocation.X != movingColliderLocationLerped.X)
                    {
                        //Cannot move on X...
                        movingColliderDestinationLocation.X = stepCollision.StopLocation.X;
                    }

                    if (stepCollision.StopLocation.Y != movingColliderLocationLerped.Y)
                    {
                        //Cannot move on Y...
                        movingColliderDestinationLocation.Y = stepCollision.StopLocation.Y;
                    }


                    collided = true;
                    lastCollision = stepCollision;
                }

            }


            //Check the last one at destination...
            movingCollider.MovingLocation = movingColliderDestinationLocation;
            if (GetCollision(movingCollider, collidesWith, ref collision))
                return true;

            collision = lastCollision;
            return collided;

        }

        /// <summary>
        /// Get collision between a rectangle and a collider
        /// </summary>
        public static Collision GetCollision(Collider movingCollider, Collider collidesWith)
        {
            Collision collision = null;
            GetCollision(movingCollider, collidesWith, ref collision);
            return collision;
        }

        /// <summary>
        /// Get collision between a rectangle and a collider
        /// </summary>
        public static bool GetCollision(Collider movingCollider, Collider collidesWith, ref Collision collision)
        {

            //We start from the other collider...
            if (collision != null)
                movingCollider.MovingLocation = collision.StopLocation;

            CollisionDirection direction = CollisionDirection.Indetermined;
            Vector2 stopLocation = Vector2.Zero;

            if (!collidesWith.Intersects(movingCollider, ref direction, ref stopLocation))
                return false;

            //if (movingCollider is ColliderRectangle)
            //{
            //    if (!collidesWith.Intersects(movingColliderLocation, ((ColliderRectangle)movingCollider).Size, out direction, out stopLocation))
            //        return false;
            //}
            //else if (movingCollider is ColliderCircle)
            //{
            //    if (!collidesWith.Intersects(movingColliderLocation, ((ColliderCircle)movingCollider).Radius, out direction, out stopLocation))
            //        return false;
            //}
            //else
            //{
            //    throw new NotSupportedException("Collider type not supported: " + movingCollider.GetType().FullName);
            //}


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


        ///// <summary>
        ///// Get collision between a circle and a collider
        ///// </summary>
        //public static Collision GetCollision(Vector2 movingColliderLocation, float movingColliderRadius, Collider collidesWith)
        //{
        //    Collision collision = null;
        //    GetCollision(movingColliderLocation, movingColliderRadius, collidesWith, ref collision);
        //    return collision;
        //}






        /// <summary>
        /// Calculate the direction from a radians
        /// </summary>
        public static CollisionDirection GetCollisionDirection(Vector2 movingColliderLocation, Vector2 colliderLocation)
        {
            float radians = (colliderLocation - movingColliderLocation).ToAngle();

            if (radians >= PiThreeQuarter || radians <= MinusPiThreeQuarter)
                return CollisionDirection.MovingColliderOnRight;

            if (radians >= MinusPiThreeQuarter && radians <= MinusPiQuarter)
                return CollisionDirection.MovingColliderOnTop;

            if (radians >= MinusPiQuarter && radians <= PiQuarter)
                return CollisionDirection.MovingColliderOnLeft;

            return CollisionDirection.MovingColliderOnBottom;


        }

        /// <summary>
        /// Check if the collider intersects with a rectangle
        /// </summary>
        public static bool Intersects(Vector2 movingColliderLocation, Vector2 movingColliderSize, Vector2 colliderLocation, Vector2 colliderSize, ref CollisionDirection direction, ref Vector2 hitLocation)
        {
            if (!VectorHelper.Intersects(colliderLocation, colliderSize, movingColliderLocation, movingColliderSize))
                return false;

            if (VectorHelper.Contains(movingColliderLocation, movingColliderSize, colliderLocation, colliderSize))
            {
                direction = CollisionDirection.MovingColliderOver;
                hitLocation = movingColliderLocation;
            }
            else if (VectorHelper.Contains(colliderLocation, colliderSize, movingColliderLocation, movingColliderSize))
            {
                direction = CollisionDirection.MovingColliderIn;
                hitLocation = movingColliderLocation;
            }
            else
            {
                //Somewhere else...
                Vector2 thisLocation = colliderLocation;
                float thisRectRight = thisLocation.X + colliderSize.X;
                float thisRectBottom = thisLocation.Y + colliderSize.Y;

                float movingColliderRight = movingColliderLocation.X + movingColliderSize.X;
                float movingColliderBottom = movingColliderLocation.Y + movingColliderSize.Y;

                if (movingColliderBottom >= thisLocation.Y && movingColliderBottom <= thisRectBottom)
                {
                    //Objet est en haut
                    if (movingColliderLocation.Y < thisLocation.Y)
                    {
                        //On va regarder si on va pas prioriser le côté...
                        if (movingColliderRight >= thisLocation.X && movingColliderRight <= thisRectRight)
                        {
                            if (movingColliderRight - thisLocation.X < movingColliderBottom - thisLocation.Y)
                                //On va prioriser la gauche...
                                direction = CollisionDirection.MovingColliderOnLeft;
                            else
                                direction = CollisionDirection.MovingColliderOnTop;
                        }
                        else if (movingColliderLocation.X <= thisRectRight && movingColliderLocation.X >= thisLocation.X)
                        {
                            //On a aussi une collision sur la droite...
                            if (thisRectRight - movingColliderLocation.X < movingColliderBottom - thisLocation.Y)
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
                    if (movingColliderLocation.Y <= thisRectBottom && movingColliderLocation.Y >= thisLocation.Y)
                    {
                        //Objet est en bas
                        if (movingColliderBottom > thisRectBottom)
                        {
                            //On va regarder si on va pas prioriser le côté...
                            if (movingColliderRight >= thisLocation.X && movingColliderRight <= thisRectRight)
                            {
                                if (movingColliderRight - thisLocation.X < thisRectBottom - movingColliderLocation.Y)
                                    //On va prioriser la gauche...
                                    direction = CollisionDirection.MovingColliderOnLeft;
                                else
                                    direction = CollisionDirection.MovingColliderOnBottom;
                            }
                            else if (movingColliderLocation.X <= thisRectRight && movingColliderLocation.X >= thisLocation.X)
                            {
                                //On a aussi une collision sur la droite...
                                if (thisRectRight - movingColliderLocation.X < thisRectBottom - movingColliderLocation.Y)
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
                        if (movingColliderRight >= thisLocation.X && movingColliderRight <= thisRectRight)
                        {
                            //Objet est en left
                            direction = CollisionDirection.MovingColliderOnLeft;
                        }
                        else if (movingColliderLocation.X <= thisRectRight && movingColliderLocation.X >= thisLocation.X)
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
                        hitLocation = new Vector2(thisLocation.X - movingColliderSize.X, movingColliderLocation.Y);
                        break;
                    case CollisionDirection.MovingColliderOnRight:
                        //La balle est à droite
                        hitLocation = new Vector2(thisRectRight, movingColliderLocation.Y);
                        break;
                    case CollisionDirection.MovingColliderOnTop:
                        //La balle est en haut
                        hitLocation = new Vector2(movingColliderLocation.X, thisLocation.Y - movingColliderSize.Y);
                        break;
                    case CollisionDirection.MovingColliderOnBottom:
                        //La balle est en bas
                        hitLocation = new Vector2(movingColliderLocation.X, thisRectBottom);
                        break;
                    default:
                        hitLocation = movingColliderLocation;
                        break;
                }

            }

            return true;
        }

        /// <summary>
        /// Check if the collider intersects with a circle
        /// </summary>
        public static bool Intersects(Vector2 movingColliderLocation, float movingColliderRadius, Vector2 colliderLocation, Vector2 colliderSize, ref CollisionDirection direction, ref Vector2 hitLocation)
        {
            //Maybe a collision?
            Vector2 colliderCenter = new Vector2(colliderLocation.X + (colliderSize.X / 2), colliderLocation.Y + (colliderSize.Y / 2));
            float distanceX = Math.Abs(colliderCenter.X - movingColliderLocation.X);
            float distanceY = Math.Abs(colliderCenter.Y - movingColliderLocation.Y);

            //If the distance is greater then the half of the rectangle + the radius, there's no way the rectangle and circle can collide.
            //event on the corners...
            if (distanceX > (colliderSize.X / 2) + movingColliderRadius)
                return false;
            if (distanceY > (colliderSize.Y / 2) + movingColliderRadius)
                return false;

            //If the distance is less then the half of the rectangle - the radius, it's certain the collide.
            bool collide = false;
            if (distanceX <= (colliderSize.X / 2) - movingColliderRadius)
            {
                collide = true;
            }
            else if (distanceY <= (colliderSize.Y / 2) - movingColliderRadius)
            {
                collide = true;
            }
            else
            {
                //We now need to check for the corners...
                // Find the closest point to the circle within the rectangle
                float closestX = Math.Min(Math.Abs(colliderLocation.X - movingColliderLocation.X), Math.Abs(movingColliderLocation.X - (colliderLocation.X + colliderSize.X)));
                float closestY = Math.Min(Math.Abs(colliderLocation.Y - movingColliderLocation.Y), Math.Abs(movingColliderLocation.Y - (colliderLocation.Y + colliderSize.Y)));

                float distanceToCornerSquared = (closestX * closestX) + (closestY * closestY);

                if (distanceToCornerSquared < movingColliderRadius * movingColliderRadius)
                    collide = true;
            }

            if (!collide)
                return false;

            //We just need to calculate the direction and the impact point
            float collisionAngle = (movingColliderLocation - colliderCenter).ToAngle();

            if (collisionAngle < 0)
            {
                float angleCornerTopLeft = (new Vector2(colliderLocation.X, colliderLocation.Y) - colliderCenter).ToAngle();
                if (collisionAngle <= angleCornerTopLeft)
                {
                    direction = CollisionDirection.MovingColliderOnLeft;
                }
                else
                {
                    float angleCornerTopRight = (new Vector2(colliderLocation.X + colliderSize.X, colliderLocation.Y) - colliderCenter).ToAngle();
                    if (collisionAngle < angleCornerTopRight)
                        direction = CollisionDirection.MovingColliderOnTop;
                    else
                        direction = CollisionDirection.MovingColliderOnRight;
                }
            }
            else
            {
                float angleCornerBottomLeft = (new Vector2(colliderLocation.X, colliderLocation.Y + colliderSize.Y) - colliderCenter).ToAngle();
                if (collisionAngle >= angleCornerBottomLeft)
                {
                    direction = CollisionDirection.MovingColliderOnLeft;
                }
                else
                {
                    float angleCornerBottomRight = (new Vector2(colliderLocation.X + colliderSize.X, colliderLocation.Y + colliderSize.Y) - colliderCenter).ToAngle();
                    if (collisionAngle > angleCornerBottomRight)
                        direction = CollisionDirection.MovingColliderOnBottom;
                    else
                        direction = CollisionDirection.MovingColliderOnRight;
                }

            }


            //En fonction de la direction on va déplacer le StopBounds
            switch (direction)
            {
                case CollisionDirection.MovingColliderOnLeft:
                    //La balle est à gauche
                    hitLocation = new Vector2(movingColliderLocation.X - movingColliderRadius, movingColliderLocation.Y);
                    break;
                case CollisionDirection.MovingColliderOnRight:
                    //La balle est à droite
                    hitLocation = new Vector2(movingColliderLocation.X + movingColliderRadius, movingColliderLocation.Y);
                    break;
                case CollisionDirection.MovingColliderOnTop:
                    //La balle est en haut
                    hitLocation = new Vector2(movingColliderLocation.X, movingColliderLocation.Y - movingColliderRadius);
                    break;
                case CollisionDirection.MovingColliderOnBottom:
                    //La balle est en bas
                    hitLocation = new Vector2(movingColliderLocation.X, movingColliderLocation.Y + movingColliderRadius);
                    break;
                default:
                    hitLocation = movingColliderLocation;
                    break;
            }

            //Hit location......
            //float x = movingColliderLocation.X;
            //float y = movingColliderLocation.Y;
            //float angle;
            //switch (direction)
            //{
            //    case CollisionDirection.MovingColliderOnLeft:
            //        //From the left...
            //        angle = (Vector2.Normalize(movingColliderLocation) - new Vector2(0, 1)).ToAngle();
            //        x = colliderLocation.X - movingColliderRadius;
            //        y = colliderCenter.Y - (float)((colliderCenter.X - colliderLocation.X) * Math.Tan(angle));
            //        break;
            //    case CollisionDirection.MovingColliderOnRight:
            //        //From the right...
            //        angle = (Vector2.Normalize(movingColliderLocation) - new Vector2(0, 1)).ToAngle();
            //        x = colliderLocation.X + colliderSize.X + movingColliderRadius;
            //        y = colliderCenter.Y - (float)((colliderCenter.X - colliderLocation.X) * Math.Tan(angle));
            //        break;
            //    case CollisionDirection.MovingColliderOnTop:
            //        //From the top...
            //        angle = (Vector2.Normalize(movingColliderLocation) - new Vector2(1, 0)).ToAngle();
            //        angle = movingColliderLocation.ToAngle();
            //        y = colliderLocation.Y - movingColliderRadius;
            //        x = movingColliderLocation.X - (float)((movingColliderLocation.Y - y) * Math.Tan(angle));

            //        break;
            //    case CollisionDirection.MovingColliderOnBottom:
            //        //From the bottom...
            //        angle = (Vector2.Normalize(movingColliderLocation) - new Vector2(1, 0)).ToAngle();
            //        x = colliderCenter.X - (float)((colliderCenter.Y - colliderLocation.Y) * Math.Tan(angle));
            //        y = colliderLocation.Y + colliderSize.Y - movingColliderRadius;
            //        break;
            //}

            //hitLocation = new Vector2(x, y);

            return true;
        }


        /// <summary>
        /// Check if the collider intersects with a circle
        /// </summary>
        public static bool Intersects(Vector2 movingColliderLocation, float movingColliderRadius, Vector2 colliderLocation, float colliderRadius, ref CollisionDirection direction, ref Vector2 hitLocation)
        {
            float distance = movingColliderLocation.Distance(colliderLocation);

            //No collision if distance is bigger then the radius of the 2 circles
            if (distance > movingColliderRadius + colliderRadius)
                return false;

            direction = GetCollisionDirection(movingColliderLocation, colliderLocation);

            hitLocation = movingColliderLocation + (Vector2.Normalize(movingColliderLocation - colliderLocation) * movingColliderRadius);

            return true;
        }


    }
}
