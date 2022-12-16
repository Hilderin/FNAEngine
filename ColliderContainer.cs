using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Container de collider
    /// </summary>
    public class ColliderContainer
    {
        /// <summary>
        /// Colliders
        /// </summary>
        public List<ColliderRectangle> Colliders { get; private set; } = new List<ColliderRectangle>();

        /// <summary>
        /// Permet d'obtenir la liste des collisions
        /// </summary>
        public IEnumerable<Collision> GetCollisions(Rectangle rectangle, ColliderRectangle currentCollider)
        {
            for (int index = 0; index < this.Colliders.Count; index++)
            {
                if (this.Colliders[index] != currentCollider)
                {
                    
                    if (this.Colliders[index].Intersects(rectangle))
                    {
                        CollisionDirection direction = CalculateDirection(rectangle, this.Colliders[index].Bounds);
                        yield return new Collision(this.Colliders[index], direction);
                    }
                }
            }
        }

        /// <summary>
        /// Permet de trouver l'emplacement du collides with par rapport au currentCollider
        /// </summary>
        private CollisionDirection CalculateDirection(Rectangle currentCollider, Rectangle collidesWith)
        {
            //Il faut que le bottom du collidesWith soit dans le collider
            if (collidesWith.Bottom >= currentCollider.Top && collidesWith.Bottom <= currentCollider.Bottom)
            {
                //Objet est en haut, pas de collider ici
                return CollisionDirection.Top;
            }

            //Il faut que le top du collidesWith soit dans le collider
            if (collidesWith.Top >= currentCollider.Top && collidesWith.Top <= currentCollider.Bottom)
            {
                //Objet est en haut, pas de collider ici
                return CollisionDirection.Bottom;
            }



            bool left = true;
            bool right = true;
            bool top = true;
            bool bottom = true;

            if (collidesWith.Right < currentCollider.Left)
            {
                //Objet est à gauche, pas de collider ici
                left = false;
            }

            if (collidesWith.Left > currentCollider.Right)
            {
                //Objet est à droit, pas de collider ici
                right = false;
            }

            if (collidesWith.Bottom < currentCollider.Top)
            {
                //Objet est en haut, pas de collider ici
                top = false;
            }

            if (collidesWith.Top > currentCollider.Bottom)
            {
                //Objet est en bas, pas de collider ici
                bottom = false;
            }

            if(left)
                return CollisionDirection.Left;
            if (right)
                return CollisionDirection.Right;
            if (top)
                return CollisionDirection.Top;
            if (bottom)
                return CollisionDirection.Bottom;

            return CollisionDirection.None;
        }


    }
}
