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

    }
}
