using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velentr.Font;

namespace FNAEngine2D
{
    /// <summary>
    /// Font
    /// </summary>
    public class Font
    {
        /// <summary>
        /// Font
        /// </summary>
        private Velentr.Font.Font _font;

        /// <summary>
        /// Nom de la font
        /// </summary>
        public string Name;

        /// <summary>
        /// Size
        /// </summary>
        public int Size;

        /// <summary>
        /// Constructeur
        /// </summary>
        internal Font(string name, int size)
        {
            this.Name = name;
            this.Size = size;

            _font = FontManager.GetFontVelentr(this.Name, this.Size);
        }

        /// <summary>
        /// Crée un objet text
        /// </summary>
        public Text MakeText(string text)
        {
            return _font.MakeText(text);
        }

        ///// <summary>
        ///// Render le text
        ///// </summary>
        //public void DrawString(string text, Vector2 position, Color color)
        //{
        //    DrawingContext.DrawString(_font, text, position, color);
        //}

        ///// <summary>
        ///// Render le text
        ///// </summary>
        //public void DrawString(string text, Vector2 position, Color color, float rotation, Vector2 origin, float layerDepth)
        //{
        //    DrawingContext.DrawString(_font, text, position, color, rotation, origin, Vector2.One, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, layerDepth);
        //}

    }
}
