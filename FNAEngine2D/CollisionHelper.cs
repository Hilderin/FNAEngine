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

            if (!collidesWith.Intersects(movingCollider))
                return false;

            //Returning collision....
            if (collision != null)
            {
                //Already a collision...
                collision.CollidesWith.Add(collidesWith.GameObject);

                //With multiple colliders, we will recalculate the next stop location with the updated MovingLocation in the collider.
                Vector2 stopLocation = GetHitLocation(movingCollider, collidesWith);
                collision.StopLocation = stopLocation;
            }
            else
            {
                //New collision...
                Direction4 direction = GetHitDirection(movingCollider, collidesWith);
                Vector2 stopLocation = GetHitLocation(movingCollider, collidesWith);
                collision = new Collision(movingCollider, collidesWith.GameObject, direction, stopLocation);
                //collision.AttemptedLocation = movingCollider.MovingLocation;
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
        /// Check if the collider intersects with a rectangle - TWO RECTANGLES
        /// </summary>
        public static bool Intersects(Vector2 movingColliderLocation, Vector2 movingColliderSize, Vector2 colliderLocation, Vector2 colliderSize)
        {
            return VectorHelper.Intersects(colliderLocation, colliderSize, movingColliderLocation, movingColliderSize);

            //if (!VectorHelper.Intersects(colliderLocation, colliderSize, movingColliderLocation, movingColliderSize))
            //    return false;

            //if (VectorHelper.Contains(movingColliderLocation, movingColliderSize, colliderLocation, colliderSize))
            //{
            //    hitLocation = movingColliderLocation;
            //}
            //else if (VectorHelper.Contains(colliderLocation, colliderSize, movingColliderLocation, movingColliderSize))
            //{
            //    hitLocation = movingColliderLocation;
            //}
            //else
            //{
            //    //Somewhere else...


            //    //En fonction de la direction on va déplacer le StopBounds
            //    switch (GetHitDirection(movingColliderLocation, movingColliderSize, colliderLocation, colliderSize))
            //    {
            //        case Direction4.Right:
            //            //La balle est à gauche
            //            hitLocation = new Vector2(colliderLocation.X - movingColliderSize.X, movingColliderLocation.Y);
            //            break;
            //        case Direction4.Left:
            //            //La balle est à droite
            //            hitLocation = new Vector2(colliderLocation.X + colliderSize.X, movingColliderLocation.Y);
            //            break;
            //        case Direction4.Down:
            //            //La balle est en haut
            //            hitLocation = new Vector2(movingColliderLocation.X, colliderLocation.Y - movingColliderSize.Y);
            //            break;
            //        case Direction4.Up:
            //            //La balle est en bas
            //            hitLocation = new Vector2(movingColliderLocation.X, colliderLocation.Y + colliderSize.Y);
            //            break;
            //        default:
            //            hitLocation = movingColliderLocation;
            //            break;
            //    }

            //}

            //return true;
        }

        /// <summary>
        /// Check if the collider intersects with a circle - CIRCLE WITH A RECTANGLE
        /// </summary>
        public static bool Intersects(Vector2 movingColliderLocation, float movingColliderRadius, Vector2 colliderLocation, Vector2 colliderSize)
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

            //If the distance is less then the half of the rectangle - the radius, it's certain they collide.
            //float penetrationDistance;
            if (distanceX <= (colliderSize.X / 2) - movingColliderRadius
                || distanceY <= (colliderSize.Y / 2) - movingColliderRadius)
            {
                //if (distanceX <= distanceY)
                //    penetrationDistance = distanceX - (colliderSize.X / 2) - movingColliderRadius;
                //else
                //    penetrationDistance = distanceY - (colliderSize.Y / 2) - movingColliderRadius;
                return true;
            }
            else
            {
                //We now need to check for the corners...
                // Find the closest point to the circle within the rectangle
                float closestX = Math.Min(Math.Abs(colliderLocation.X - movingColliderLocation.X), Math.Abs(movingColliderLocation.X - (colliderLocation.X + colliderSize.X)));
                float closestY = Math.Min(Math.Abs(colliderLocation.Y - movingColliderLocation.Y), Math.Abs(movingColliderLocation.Y - (colliderLocation.Y + colliderSize.Y)));

                float distanceToCornerPower2 = (closestX * closestX) + (closestY * closestY);

                if (distanceToCornerPower2 < movingColliderRadius * movingColliderRadius)
                {
                    //penetrationDistance = (float)Math.Sqrt(distanceToCornerPower2);
                    return true;
                }
                else
                {
                    //No collision
                    return false;
                }
            }

            //We are going backwards on the inverted direction.
            //hitLocation = movingColliderLocation + (Vector2.Normalize(movingColliderLocation - previousMovingColliderLocation) * -penetrationDistance);




            //hitLocation = previousMovingColliderLocation;

            //We just need to calculate the direction and the impact point
            //float collisionAngle = (movingColliderLocation - colliderCenter).ToAngle();

            //if (collisionAngle < 0)
            //{
            //    float angleCornerTopLeft = (new Vector2(colliderLocation.X, colliderLocation.Y) - colliderCenter).ToAngle();
            //    if (collisionAngle <= angleCornerTopLeft)
            //    {
            //        direction = Direction4.Right;
            //    }
            //    else
            //    {
            //        float angleCornerTopRight = (new Vector2(colliderLocation.X + colliderSize.X, colliderLocation.Y) - colliderCenter).ToAngle();
            //        if (collisionAngle < angleCornerTopRight)
            //            direction = Direction4.Down;
            //        else
            //            direction = Direction4.Left;
            //    }
            //}
            //else
            //{
            //    float angleCornerBottomLeft = (new Vector2(colliderLocation.X, colliderLocation.Y + colliderSize.Y) - colliderCenter).ToAngle();
            //    if (collisionAngle >= angleCornerBottomLeft)
            //    {
            //        direction = Direction4.Right;
            //    }
            //    else
            //    {
            //        float angleCornerBottomRight = (new Vector2(colliderLocation.X + colliderSize.X, colliderLocation.Y + colliderSize.Y) - colliderCenter).ToAngle();
            //        if (collisionAngle > angleCornerBottomRight)
            //            direction = Direction4.Up;
            //        else
            //            direction = Direction4.Left;
            //    }

            //}


            ////En fonction de la direction on va déplacer le StopBounds
            //switch (direction)
            //{
            //    case Direction4.Right:
            //        //La balle est à gauche
            //        hitLocation = new Vector2(movingColliderLocation.X - movingColliderRadius, movingColliderLocation.Y);
            //        break;
            //    case Direction4.Left:
            //        //La balle est à droite
            //        hitLocation = new Vector2(movingColliderLocation.X + movingColliderRadius, movingColliderLocation.Y);
            //        break;
            //    case Direction4.Down:
            //        //La balle est en haut
            //        hitLocation = new Vector2(movingColliderLocation.X, movingColliderLocation.Y - movingColliderRadius);
            //        break;
            //    case Direction4.Up:
            //        //La balle est en bas
            //        hitLocation = new Vector2(movingColliderLocation.X, movingColliderLocation.Y + movingColliderRadius);
            //        break;
            //    default:
            //        hitLocation = movingColliderLocation;
            //        break;
            //}

            //Hit location......
            //float x = movingColliderLocation.X;
            //float y = movingColliderLocation.Y;
            //float angle;
            //switch (direction)
            //{
            //    case Direction4.Right:
            //        //From the left...
            //        angle = (Vector2.Normalize(movingColliderLocation) - new Vector2(0, 1)).ToAngle();
            //        x = colliderLocation.X - movingColliderRadius;
            //        y = colliderCenter.Y - (float)((colliderCenter.X - colliderLocation.X) * Math.Tan(angle));
            //        break;
            //    case Direction4.Left:
            //        //From the right...
            //        angle = (Vector2.Normalize(movingColliderLocation) - new Vector2(0, 1)).ToAngle();
            //        x = colliderLocation.X + colliderSize.X + movingColliderRadius;
            //        y = colliderCenter.Y - (float)((colliderCenter.X - colliderLocation.X) * Math.Tan(angle));
            //        break;
            //    case Direction4.Down:
            //        //From the top...
            //        angle = (Vector2.Normalize(movingColliderLocation) - new Vector2(1, 0)).ToAngle();
            //        angle = movingColliderLocation.ToAngle();
            //        y = colliderLocation.Y - movingColliderRadius;
            //        x = movingColliderLocation.X - (float)((movingColliderLocation.Y - y) * Math.Tan(angle));

            //        break;
            //    case Direction4.Up:
            //        //From the bottom...
            //        angle = (Vector2.Normalize(movingColliderLocation) - new Vector2(1, 0)).ToAngle();
            //        x = colliderCenter.X - (float)((colliderCenter.Y - colliderLocation.Y) * Math.Tan(angle));
            //        y = colliderLocation.Y + colliderSize.Y - movingColliderRadius;
            //        break;
            //}

            //hitLocation = new Vector2(x, y);

            //return true;
        }


        /// <summary>
        /// Check if the collider intersects with a circle - CIRCLE WITH CIRCLE
        /// </summary>
        public static bool Intersects(Vector2 movingColliderLocation, float movingColliderRadius, Vector2 colliderLocation, float colliderRadius)
        {
            float distance = movingColliderLocation.Distance(colliderLocation);

            //No collision if distance is bigger then the radius of the 2 circles
            if (distance > movingColliderRadius + colliderRadius)
                return false;

            ////Calculating the distance that the moving object penetrated into the other circle.
            ////easy to do with the distance between the 2 circles. We juste need to substract the radius of the other circle.
            //float penetrationDistance = movingColliderRadius - (distance - colliderRadius);

            ////We are going backwards on the inverted direction.
            //hitLocation = movingColliderLocation + (Vector2.Normalize(movingColliderLocation - previousMovingColliderLocation) * -penetrationDistance);

            return true;
        }

        ///// <summary>
        ///// Get direction of the moving collider in relation to another collider 
        ///// </summary>
        //public static Direction4 GetHitDirection(GameObject movingObject, GameObject collidesWithObject)
        //{
        //    return GetHitDirection(movingObject.Location, movingObject.Size, collidesWithObject.Location, collidesWithObject.Size);
        //}

        /// <summary>
        /// Get direction of the moving collider in relation to another collider 
        /// </summary>
        public static Direction4 GetHitDirection(Collider movingCollider, Collider collidesWith)
        {
            return GetHitDirection(movingCollider.MovingLocation, movingCollider.Size, collidesWith.Location, collidesWith.Size);
        }

        /// <summary>
        /// Get direction of the moving collider in relation to another collider 
        /// </summary>
        public static Direction4 GetHitDirection(Vector2 movingColliderLocation, Vector2 movingColliderSize, Vector2 colliderLocation, Vector2 colliderSize)
        {
            float thisRectRight = colliderLocation.X + colliderSize.X;
            float thisRectBottom = colliderLocation.Y + colliderSize.Y;

            float movingColliderRight = movingColliderLocation.X + movingColliderSize.X;
            float movingColliderBottom = movingColliderLocation.Y + movingColliderSize.Y;

            Direction4 direction = Direction4.None;
            if (movingColliderBottom >= colliderLocation.Y && movingColliderBottom <= thisRectBottom)
            {
                //Objet est en haut
                if (movingColliderLocation.Y < colliderLocation.Y)
                {
                    //On va regarder si on va pas prioriser le côté...
                    if (movingColliderRight >= colliderLocation.X && movingColliderRight <= thisRectRight)
                    {
                        if (movingColliderRight - colliderLocation.X < movingColliderBottom - colliderLocation.Y)
                            //On va prioriser la gauche...
                            direction = Direction4.Right;
                        else
                            direction = Direction4.Down;
                    }
                    else if (movingColliderLocation.X <= thisRectRight && movingColliderLocation.X >= colliderLocation.X)
                    {
                        //On a aussi une collision sur la droite...
                        if (thisRectRight - movingColliderLocation.X < movingColliderBottom - colliderLocation.Y)
                            //On va prioriser la droite...
                            direction = Direction4.Left;
                        else
                            direction = Direction4.Down;
                    }
                    else
                    {
                        direction = Direction4.Down;
                    }
                }
            }

            if (direction == Direction4.None)
            {
                if (movingColliderLocation.Y <= thisRectBottom && movingColliderLocation.Y >= colliderLocation.Y)
                {
                    //Objet est en bas
                    if (movingColliderBottom > thisRectBottom)
                    {
                        //On va regarder si on va pas prioriser le côté...
                        if (movingColliderRight >= colliderLocation.X && movingColliderRight <= thisRectRight)
                        {
                            if (movingColliderRight - colliderLocation.X < thisRectBottom - movingColliderLocation.Y)
                                //On va prioriser la gauche...
                                direction = Direction4.Right;
                            else
                                direction = Direction4.Up;
                        }
                        else if (movingColliderLocation.X <= thisRectRight && movingColliderLocation.X >= colliderLocation.X)
                        {
                            //On a aussi une collision sur la droite...
                            if (thisRectRight - movingColliderLocation.X < thisRectBottom - movingColliderLocation.Y)
                                //On va prioriser la droite...
                                direction = Direction4.Left;
                            else
                                direction = Direction4.Up;
                        }
                        else
                        {
                            //Vraiment juste en bas...
                            direction = Direction4.Up;
                        }
                    }
                }

                if (direction == Direction4.None)
                {
                    if (movingColliderRight >= colliderLocation.X && movingColliderRight <= thisRectRight)
                    {
                        //Objet est en left
                        direction = Direction4.Right;
                    }
                    else if (movingColliderLocation.X <= thisRectRight && movingColliderLocation.X >= colliderLocation.X)
                    {
                        //Objet est en right
                        direction = Direction4.Left;
                    }
                }
            }

            return direction;
        }



        /// <summary>
        /// Calculate the hit location between 2 colliders
        /// </summary>
        public static Vector2 GetHitLocation(Collider movingCollider, Collider collidesWith)
        {

            if (movingCollider is ColliderCircle && collidesWith is ColliderCircle)
            {
                //two circles...
                ColliderCircle movingColliderCircle = (ColliderCircle)movingCollider;
                ColliderCircle collidesWithCircle = (ColliderCircle)collidesWith;

                float distance = movingColliderCircle.CenterMovingLocation.Distance(collidesWithCircle.CenterLocation);

                //Calculating the distance that the moving object penetrated into the other circle.
                //easy to do with the distance between the 2 circles. We juste need to substract the radius of the other circle.
                float penetrationDistance = movingColliderCircle.Radius - (distance - collidesWithCircle.Radius);

                //We are going backwards on the inverted direction.
                return movingColliderCircle.MovingLocation + (Vector2.Normalize(movingColliderCircle.MovingLocation - movingColliderCircle.Location) * -(penetrationDistance + 5));

            }
            else
            {
                Vector2 movingColliderLocation = movingCollider.MovingLocation;
                Vector2 movingColliderSize = movingCollider.Size;
                Vector2 colliderLocation = collidesWith.Location;
                Vector2 colliderSize = collidesWith.Size;

                //En fonction de la direction on va déplacer le StopBounds
                switch (GetHitDirection(movingColliderLocation, movingColliderSize, colliderLocation, colliderSize))
                {
                    case Direction4.Right:
                        //La balle est à gauche
                        return new Vector2(colliderLocation.X - movingColliderSize.X, movingColliderLocation.Y);
                    case Direction4.Left:
                        //La balle est à droite
                        return new Vector2(colliderLocation.X + colliderSize.X, movingColliderLocation.Y);
                    case Direction4.Down:
                        //La balle est en haut
                        return new Vector2(movingColliderLocation.X, colliderLocation.Y - movingColliderSize.Y);
                    case Direction4.Up:
                        //La balle est en bas
                        return new Vector2(movingColliderLocation.X, colliderLocation.Y + colliderSize.Y);
                    default:
                        return movingColliderLocation;
                }
            }

        }

    }
}
