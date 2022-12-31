using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Velentr.Font;

namespace FNAEngine2D
{
    public class DebugInfoRender : GameObject
    {
        /// <summary>
        /// Font name
        /// </summary>
        public string FontName { get; set; } = FontManager.ROBOTO_REGULAR;

        /// <summary>
        /// Font size
        /// </summary>
        public int FontSize { get; set; } = 12;

        /// <summary>
        /// Color to display
        /// </summary>
        public Color Color { get; set; } = Color.Yellow;
        
        /// <summary>
        /// TextRenderer
        /// </summary>
        private TextRender _textRender;

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public DebugInfoRender()
        {
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public DebugInfoRender(string fontName, int fontSize, Color color)
        {
            this.FontName = fontName;
            this.FontSize = fontSize;
            this.Color = color;
        }

        public override void Load()
        {
            _textRender = this.Add(new TextRender(GetText(), this.FontName, this.FontSize, this.Location, this.Color));
        }

        public override void Update()
        {
            _textRender.Text = GetText();
        }

        private string GetText()
        {
            MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            return "Mouse: " + mouseState.X + ", " + mouseState.Y + ", Elapsed: "  + this.ElapsedGameTimeMilliseconds + "ms";
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
