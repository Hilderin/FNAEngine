using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Velentr.Font;

namespace FNAEngine2D.GameObjects
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
        /// Timer to calculate time for fps
        /// </summary>
        private Stopwatch _drawTimer = Stopwatch.StartNew();


        /// <summary>
        /// Font name
        /// </summary>
        public string FontName { get; set; } = ContentManager.FONT_ROBOTO_REGULAR;

        /// <summary>
        /// Font size
        /// </summary>
        public int FontSize { get; set; } = 12;

        /// <summary>
        /// Color to display
        /// </summary>
        public Color Color { get; set; } = Color.Yellow;
        
        /// <summary>
        /// Labeler
        /// </summary>
        private Label _label;

        /// <summary>
        /// Game time service
        /// </summary>
        private GameTimeService _gameTimeService;

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

        protected override void Load()
        {
            _gameTimeService = GetService<GameTimeService>();

            _label = this.Add(new Label(GetText(0, 0, 0, 0), this.FontName, this.FontSize, Vector2.Zero, this.Color));
        }

        protected override void Update()
        {
            
            _lastFrameUpdateTimeMillisecondsAverage.Add(_gameTimeService.LastFrameUpdateTimeMilliseconds);
            _lastFrameDrawTimeMillisecondsAverage.Add(_gameTimeService.LastFrameDrawTimeMilliseconds);
            _lastFrameTimeMillisecondsAverage.Add(_gameTimeService.LastFrameTimeMilliseconds);


            _label.Text = GetText(_fpsAverage.GetAverage(), _lastFrameUpdateTimeMillisecondsAverage.GetAverage(), _lastFrameDrawTimeMillisecondsAverage.GetAverage(), _lastFrameTimeMillisecondsAverage.GetAverage());
        }

        protected override void Draw()
        {
            decimal elapsedMilliseconds = ((decimal)_drawTimer.ElapsedTicks / Stopwatch.Frequency);
            _drawTimer.Restart();
            if(elapsedMilliseconds > 0)
                _fpsAverage.Add(1M / elapsedMilliseconds);            
        }

        /// <summary>
        /// Get the text to render
        /// </summary>
        private string GetText(decimal fps, decimal lastFrameUpdateTimeMilliseconds, decimal lastFrameDrawTimeMilliseconds, decimal lastFrameTimeMilliseconds)
        {
            return "FPS: " + Math.Round(fps, 4).ToString() + ", Update: " + Math.Round(lastFrameUpdateTimeMilliseconds, 4) + "ms, Draw: " + Math.Round(lastFrameDrawTimeMilliseconds, 4) + "ms, Total: " + Math.Round(lastFrameTimeMilliseconds, 4) + "ms";
        }

    }
}
