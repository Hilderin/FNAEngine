using FNAEngine2D.Aseprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Extensions for texture 2d
    /// </summary>
    public static class Texture2DExtensions
    {
        /// <summary>
        /// Get the color at a coord for a texture
        /// </summary>
        public static Color GetPixel(this Texture2D texture, int x, int y)
        {
            return Texture2DHelper.GetPixel(texture, x, y);
        }

        /// <summary>
        /// Get the colors for a texture
        /// </summary>
        public static Color[] GetPixels(this Texture2D texture)
        {
            return Texture2DHelper.GetPixels(texture);
        }

        /// <summary>
        /// Set the color at a coord for a texture
        /// </summary>
        public static void SetPixel(this Texture2D texture, int x, int y, Color color)
        {
            Texture2DHelper.SetPixel(texture, x, y, color);
        }

        /// <summary>
        /// Set the color at a coord for a texture
        /// </summary>
        public static void SetPixels(this Texture2D texture, int x, int y, int width, int height, Color[] colors)
        {
            Texture2DHelper.SetPixels(texture, x, y, width, height, colors);
        }

    }
}
