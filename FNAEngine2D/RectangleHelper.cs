using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public static class RectangleHelper
    {
        /// <summary>
        /// Get the rectangle centered
        /// </summary>
        public static Rectangle Center(Rectangle parentBounds, int width, int height)
        {
            return new Rectangle((parentBounds.Width / 2) - (width / 2)
                                , (parentBounds.Height / 2) - (height / 2)
                                , width
                                , height);
        }

        /// <summary>
        /// Get the rectangle centered and align with the bottom
        /// </summary>
        public static Rectangle CenterBottom(Rectangle parentBounds, int width, int height)
        {
            return new Rectangle(parentBounds.X + (parentBounds.Width / 2) - (width / 2)
                                , parentBounds.Y + (parentBounds.Height - height)
                                , width
                                , height);
        }

        /// <summary>
        /// Get the rectangle centered and align with the Middle
        /// </summary>
        public static Rectangle CenterMiddle(Rectangle parentBounds, int width, int height)
        {
            return new Rectangle(parentBounds.X + (parentBounds.Width / 2) - (width / 2)
                                , parentBounds.Y + (parentBounds.Height / 2) - (height / 2)
                                , width
                                , height);
        }

        /// <summary>
        /// Add on x axis
        /// </summary>
        public static Rectangle AddX(Rectangle rectangle, int x)
        {
            return new Rectangle(rectangle.X + x, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Add on y axis
        /// </summary>
        public static Rectangle AddY(Rectangle rectangle, int y)
        {
            return new Rectangle(rectangle.X, rectangle.Y + y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Add on x and y axis
        /// </summary>
        public static Rectangle AddXY(Rectangle rectangle, int x, int y)
        {
            return new Rectangle(rectangle.X + x, rectangle.Y + y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Add on x and y axis
        /// </summary>
        public static Rectangle Add(Rectangle rectangle, Vector2 vector)
        {
            return new Rectangle(rectangle.X + (int)vector.X, rectangle.Y + (int)vector.Y, rectangle.Width, rectangle.Height);
        }
    }
}
