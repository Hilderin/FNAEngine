using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class FPSRender : GameObject
    {
        public string FontName { get; set; }

        public int FontSize { get; set; }
        
        public Color Color { get; set; }
        
        /// <summary>
        /// TextRenderer
        /// </summary>
        private TextRender _textRender;

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public FPSRender(string fontName, int fontSize, Color color)
        {
            this.FontName = fontName;
            this.FontSize = fontSize;
            this.Color = color;
        }

        public override void Load()
        {
            _textRender = this.Add(new TextRender(String.Empty, this.FontName, this.FontSize, Vector2.Zero, this.Color));
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            _textRender.Text = "FPS: " + Math.Round(1 / GameHost.GameTime.ElapsedGameTime.TotalSeconds, 4).ToString() + ", Update: " + Math.Round(GameHost.LastFrameUpdateTimeMilliseconds, 8) + "ms, Draw: " + Math.Round(GameHost.LastFrameDrawTimeMilliseconds, 8) + "ms, Total: " + Math.Round(GameHost.LastFrameTimeMilliseconds, 8) + "ms";
            
        }

    }
}
