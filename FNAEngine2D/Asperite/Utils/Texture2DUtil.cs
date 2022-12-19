using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FNAEngine2D.Aseprite.Utils
{
    public class Texture2DUtil
    {
        /// <summary>
        /// Create a texture
        /// </summary>
        public static Texture2D CreateTexture(int width, int height)
        {
            return new Texture2D(GameHost.InternalGameHost.GraphicsDevice, width, height, false, SurfaceFormat.Color);
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
    }
}