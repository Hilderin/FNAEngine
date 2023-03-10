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

        /// <summary>
        /// Get the angle af a vector 2
        /// </summary>
        public static float ToAngle(this Vector2 vector)
        {
            return VectorHelper.ToAngle(vector);
        }

        /// <summary>
        /// Get the distance between the Vector and another vector
        /// </summary>
        public static float Distance(this Vector2 vector, Vector2 target)
        {
            return Vector2.Distance(vector, target);
        }

        /// <summary>
        /// Get the distance squaredbetween the Vector and another vector
        /// </summary>
        public static float DistanceSquared(this Vector2 vector, Vector2 target)
        {
            return Vector2.DistanceSquared(vector, target);
        }


        /// <summary>
        /// Calculate the direction of the vector on 4 directions
        /// </summary>
        public static Direction4 Direction4(this Vector2 vector)
        {
            return VectorHelper.GetDirection4(vector);
        }

        /// <summary>
        /// Calculate the direction from a radians an origin and a destination
        /// </summary>
        public static Direction4 Direction4To(this Vector2 vector, Vector2 destination)
        {
            return VectorHelper.GetDirection4To(vector, destination);
        }

    }
}
