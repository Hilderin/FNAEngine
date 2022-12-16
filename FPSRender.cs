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
        /// TextRenderer
        /// </summary>
        private TextRender _textRender;

        /// <summary>
        /// Compteur de frame
        /// </summary>
        private int _cptFrame = 0;

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public FPSRender(string fileName, int fontSize, Color color)
        {
            _textRender = this.Add(new TextRender(String.Empty, fileName, fontSize, new Vector2(0, 0), color));
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            _textRender.Text = (1 / GameHost.GameTime.ElapsedGameTime.TotalSeconds).ToString();
            //_cptFrame++;

            base.Draw();
        }

    }
}
