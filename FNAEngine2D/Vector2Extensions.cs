using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Extensions for Vector2
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Add on x axis
        /// </summary>
        public static Vector2 AddX(this Vector2 vector, float x)
        {
            return VectorHelper.AddX(vector, x);
        }

        /// <summary>
        /// Add on y axis
        /// </summary>
        public static Vector2 AddY(this Vector2 vector, float y)
        {
            return VectorHelper.AddY(vector, y);
        }

        /// <summary>
        /// Add on x and y axis
        /// </summary>
        public static Vector2 AddXY(this Vector2 vector, float x, float y)
        {
            return VectorHelper.AddXY(vector, x, y);
        }

        /// <summary>
        /// Add a point
        /// </summary>
        public static Vector2 Add(this Vector2 vector, Point point)
        {
            return VectorHelper.Add(vector, point);
        }

        /// <summary>
        /// Substract a point
        /// </summary>
        public static Vector2 Substract(this Vector2 vector, Point point)
        {
            return VectorHelper.Substract(vector, point);
        }

    }
}
