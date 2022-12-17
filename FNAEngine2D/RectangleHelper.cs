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
        /// Permet de trouver le rectangle au center
        /// </summary>
        public static Rectangle Center(Rectangle parentBounds, int width, int height)
        {
            return new Rectangle((parentBounds.Width / 2) - (width / 2)
                                , (parentBounds.Height / 2) - (height / 2)
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
    }
}
