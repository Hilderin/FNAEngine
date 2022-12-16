using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public static class VectorHelper
    {
        /// <summary>
        /// Permet de trouver le centre
        /// </summary>
        public static Vector2 Center(Vector2Int size, Vector2Int objectSize)
        {
            return new Vector2((size.X / 2) - (objectSize.X / 2), (size.Y / 2) - (objectSize.Y / 2));
        }

        /// <summary>
        /// Permet de trouver le centre
        /// </summary>
        public static Vector2 Center(int width, int height, int objectWidth, int objectHeight)
        {
            return new Vector2((width / 2) - (objectWidth / 2), (height / 2) - (objectHeight / 2));
        }


        /// <summary>
        /// Permet de faire une rotation sur un point d'origine
        /// </summary>
        public static Vector2 Rotate(Vector2 toRotate, Vector2 origin, float radians)
        {
            //On fait le - car sinon, la rotation va partir dans l'autre sens...
            return Vector2.Transform(toRotate - origin, Matrix.CreateRotationZ(-radians)) + origin;
        }
    }
}
