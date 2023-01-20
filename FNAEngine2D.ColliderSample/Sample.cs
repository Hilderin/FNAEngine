using FNAEngine2D.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.ColliderSample
{
    public class Sample: GameObject
    {
        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            //Top
            Add(new TextureBox("pixel", new Rectangle(0, 0, this.Game.Width, 20), Color.White)).EnableCollider();
            //Right
            Add(new TextureBox("pixel", new Rectangle(this.Game.Width - 20, 0, 20, this.Game.Height), Color.White)).EnableCollider();
            //Bottom
            Add(new TextureBox("pixel", new Rectangle(0, this.Game.Height - 20, this.Game.Width, 20), Color.White)).EnableCollider();
            //Left
            Add(new TextureBox("pixel", new Rectangle(0, 0, 20, this.Game.Height), Color.White)).EnableCollider();

            for(int cpt = 0; cpt < 3000; cpt++)
                Add(new Ball()).TranslateTo(GameMath.RandomFloat(25, this.Game.Width - 25 - 50), GameMath.RandomFloat(25, this.Game.Height - 25 - 50));

            Add(new FPSRender(ContentManager.FONT_ROBOTO_REGULAR, 12, Color.DarkRed));
        }

    }
}
