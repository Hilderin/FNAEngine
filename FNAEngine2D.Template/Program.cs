using FNAEngine2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FNAEngine2D.Template
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the game.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameManager.Run(new Scene(), GraphicSettings.Default);
        }
    }
}
