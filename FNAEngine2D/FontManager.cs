using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// FontManager
    /// </summary>
    public static class FontManager
    {
        
        /// <summary>
        /// Graphics device
        /// </summary>
        private static Velentr.Font.FontManager _fontManager = null;

        ///// <summary>
        ///// Liste des fonts à loader
        ///// </summary>
        //private static List<Font> _fontsToLoad = new List<Font>();

        /// <summary>
        /// GraphicsDevice
        /// </summary>
        public static void SetGraphicsDevice(GraphicsDevice graphicsDevice)
        {
            _fontManager = new Velentr.Font.FontManager(graphicsDevice);
        }

        ///// <summary>
        ///// Permet d'obtenir des fonts
        ///// </summary>
        //public static Font GetFont(string filename, int fontSize)
        //{
        //    if (_fontManager == null)
        //        throw new InvalidOperationException("GetFont called beforce Initialized");

        //    return new Font(filename, fontSize);
        //}

        ///// <summary>
        ///// Permet de loader les fonts
        ///// </summary>
        //public static void LoadFonts()
        //{
        //    foreach (Font font in _fontsToLoad)
        //        font.LoadFont();

        //    _fontsToLoad.Clear();

        //}

        /// <summary>
        /// Permet d'obtenir des fonts
        /// </summary>
        public static Velentr.Font.Font GetFontVelentr(string filename, int fontSize)
        {
            if (_fontManager == null)
                throw new InvalidOperationException("GetFont called before initialize.");

            //Embbed font?
            if (filename == ContentManager.FONT_ROBOTO_REGULAR)
            {
                return _fontManager.GetTypeface(ContentManager.FONT_ROBOTO_REGULAR, Resource.Roboto_Regular).GetFont(fontSize);
            }
            else
            {
                if (String.IsNullOrEmpty(Path.GetExtension(filename)))
                    filename += ".ttf";

                return _fontManager.GetFont(ContentManager.ContentFolder + filename, fontSize);
            }
        }
    }
}
