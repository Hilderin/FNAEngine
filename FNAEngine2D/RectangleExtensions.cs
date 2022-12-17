using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Extensions for Rectangle
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// Add on x axis
        /// </summary>
        public static Rectangle AddX(this Rectangle rectangle, int x)
        {
            return RectangleHelper.AddX(rectangle, x);
        }

        /// <summary>
        /// Add on y axis
        /// </summary>
        public static Rectangle AddY(this Rectangle rectangle, int y)
        {
            return RectangleHelper.AddY(rectangle, y);
        }

    }
}
