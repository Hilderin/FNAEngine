using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Graphic settings
    /// </summary>
    public class GraphicSettings
    {
        /// <summary>
        /// Default graphic settings
        /// </summary>
        public static GraphicSettings Default { get; } = new GraphicSettings();

        /// <summary>
        /// Real size of the screen
        /// </summary>
        public Point ScreenSize { get; set; } = new Point(1200, 720);

        /// <summary>
        /// Size of the game
        /// </summary>
        public Point GameSize { get; set; } = new Point(1200, 720);

        /// <summary>
        /// Number of pixels = 1 meter
        /// </summary>
        public float NbPixelPerMeter { get; set; } = 100;

        /// <summary>
        /// Full screen
        /// </summary>
        public bool IsFullscreen { get; set; }

        /// <summary>
        /// Background color
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.Gray;

        

    }
}
