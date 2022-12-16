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
    /// FontManager
    /// </summary>
    internal static class FontManager
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

            return _fontManager.GetFont(ContentHelper.ContentFolder + filename, fontSize);
        }
    }
}
