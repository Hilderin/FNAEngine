using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public static class Texture2DHelper
    {
        /// <summary>
        /// Create a texture
        /// </summary>
        public static Texture2D CreateTexture(int width, int height)
        {
            return new Texture2D(GameHost.InternalGame.GraphicsDevice, width, height, false, SurfaceFormat.Color);
        }

        /// <summary>
        /// Create a transparent texture
        /// </summary>
        public static Texture2D CreateTransparentTexture(int width, int height)
        {
            Texture2D texture = CreateTexture(width, height);

            //Important to cleanup because memory can be reused with artefact or previous textures...
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.Transparent;
            texture.SetData(pixels);

            return texture;
        }

        /// <summary>
        /// Get the color at a coord for a texture
        /// </summary>
        public static Color GetPixel(Texture2D texture, int x, int y)
        {
            Rectangle r = new Rectangle(x, y, 1, 1);
            Color[] colors = new Color[1];

            texture.GetData(0, r, colors, 0, 1);

            return colors[0];
        }

        /// <summary>
        /// Get the colors for a texture
        /// </summary>
        public static Color[] GetPixels(Texture2D texture)
        {
            //Rectangle r = new Rectangle(0, 0, texture.Width, texture.Height);
            Color[] colors = new Color[texture.Width * texture.Height];

            texture.GetData(colors);

            return colors;
        }

        /// <summary>
        /// Set the color at a coord for a texture
        /// </summary>
        public static void SetPixel(Texture2D texture, int x, int y, Color color)
        {
            Rectangle r = new Rectangle(x, y, 1, 1);
            Color[] colors = new Color[1];
            colors[0] = color;

            texture.SetData<Color>(0, r, colors, 0, 1);
        }

        /// <summary>
        /// Set the color at a coord for a texture
        /// </summary>
        public static void SetPixels(Texture2D texture, int x, int y, int width, int height, Color[] colors)
        {
            Rectangle r = new Rectangle(x, y, width, height);

            texture.SetData<Color>(0, r, colors, 0, colors.Length);
        }
    }
}
