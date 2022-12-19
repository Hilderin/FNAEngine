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
        /// Get the rectangle centered
        /// </summary>
        public static Rectangle Center(this Rectangle rectangle, int width, int height)
        {
            return RectangleHelper.Center(rectangle, width, height);
        }

        /// <summary>
        /// Get the rectangle centered
        /// </summary>
        public static Rectangle Center(this Rectangle rectangle, float width, float height)
        {
            return RectangleHelper.Center(rectangle, (int)width, (int)height);
        }

        /// <summary>
        /// Get the rectangle centered and align with the bottom
        /// </summary>
        public static Rectangle CenterBottom(this Rectangle rectangle, int width, int height)
        {
            return RectangleHelper.CenterBottom(rectangle, width, height);
        }

        /// <summary>
        /// Get the rectangle centered and align with the bottom
        /// </summary>
        public static Rectangle CenterBottom(this Rectangle rectangle, float width, float height)
        {
            return RectangleHelper.CenterBottom(rectangle, (int)width, (int)height);
        }

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

        /// <summary>
        /// Get the location Vector2
        /// </summary>
        public static Vector2 GetLocation(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X, rectangle.Y);
        }

        /// <summary>
        /// Get the size Vector2
        /// </summary>
        public static Vector2 GetSize(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Width, rectangle.Height);
        }


    }
}
