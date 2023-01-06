using FNAEngine2D;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FNAEngine2D.ParticuleTest
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the game.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GraphicSettings settings = new GraphicSettings();
            settings.BackgroundColor = Color.Black;

            GameManager.Run(new Scene(), settings);
        }
    }
}
