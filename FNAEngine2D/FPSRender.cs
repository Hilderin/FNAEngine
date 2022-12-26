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
        /// <summary>
        /// Fps average calculator
        /// </summary>
        private AverageCalculator _fpsAverage = new AverageCalculator(60);

        /// <summary>
        /// LastFrameUpdateTimeMilliseconds average calculator
        /// </summary>
        private AverageCalculator _lastFrameUpdateTimeMillisecondsAverage = new AverageCalculator(120);

        /// <summary>
        /// LastFrameDrawTimeMilliseconds average calculator
        /// </summary>
        private AverageCalculator _lastFrameDrawTimeMillisecondsAverage = new AverageCalculator(120);

        /// <summary>
        /// LastFrameTimeMilliseconds average calculator
        /// </summary>
        private AverageCalculator _lastFrameTimeMillisecondsAverage = new AverageCalculator(120);
        

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
        public FPSRender()
        {
        }

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
            _textRender = this.Add(new TextRender(GetText(0, 0, 0, 0), this.FontName, this.FontSize, Vector2.Zero, this.Color));
        }

        public override void Update()
        {
            _fpsAverage.Add(1 / GameHost.GameTime.ElapsedGameTime.TotalSeconds);
            _lastFrameUpdateTimeMillisecondsAverage.Add(GameHost.LastFrameUpdateTimeMilliseconds);
            _lastFrameDrawTimeMillisecondsAverage.Add(GameHost.LastFrameDrawTimeMilliseconds);
            _lastFrameTimeMillisecondsAverage.Add(GameHost.LastFrameTimeMilliseconds);
            



            _textRender.Text = GetText(_fpsAverage.GetAverage(), _lastFrameUpdateTimeMillisecondsAverage.GetAverage(), _lastFrameDrawTimeMillisecondsAverage.GetAverage(), _lastFrameTimeMillisecondsAverage.GetAverage());
        }

        private string GetText(decimal fps, decimal lastFrameUpdateTimeMilliseconds, decimal lastFrameDrawTimeMilliseconds, decimal lastFrameTimeMilliseconds)
        {
            return "FPS: " + Math.Round(fps, 4).ToString() + ", Update: " + Math.Round(lastFrameUpdateTimeMilliseconds, 4) + "ms, Draw: " + Math.Round(lastFrameDrawTimeMilliseconds, 4) + "ms, Total: " + Math.Round(lastFrameTimeMilliseconds, 4) + "ms";
        }

    }
}
