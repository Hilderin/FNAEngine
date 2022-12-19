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
            Rectangle r = new Rectangle(x, y, 1, 1);
            Color[] colors = new Color[1];
            
            texture.GetData(0, r, colors, 0, 1);

            return colors[0];
        }

        /// <summary>
        /// Get the colors for a texture
        /// </summary>
        public static Color[] GetPixels(this Texture2D texture)
        {
            //Rectangle r = new Rectangle(0, 0, texture.Width, texture.Height);
            Color[] colors = new Color[texture.Width * texture.Height];

            texture.GetData(colors);

            return colors;
        }

        /// <summary>
        /// Set the color at a coord for a texture
        /// </summary>
        public static void SetPixel(this Texture2D texture, int x, int y, Color color)
        {
            Rectangle r = new Rectangle(x, y, 1, 1);
            Color[] colors = new Color[1];
            colors[0] = color;

            texture.SetData<Color>(0, r, colors, 0, 1);
        }

        /// <summary>
        /// Set the color at a coord for a texture
        /// </summary>
        public static void SetPixels(this Texture2D texture, int x, int y, int width, int height, Color[] colors)
        {
            Rectangle r = new Rectangle(x, y, width, height);
           
            texture.SetData<Color>(0, r, colors, 0, colors.Length);
        }

    }
}
