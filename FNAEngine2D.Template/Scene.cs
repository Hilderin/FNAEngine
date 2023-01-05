using Microsoft.Xna.Framework;

namespace FNAEngine2D.Template
{
    public class Scene: GameObject
    {
        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            Add(new FPSRender());

            Add(new TextRender("Template", ContentManager.FONT_ROBOTO_REGULAR, 32, this.Game.Rectangle, Color.White, TextHorizontalAlignment.Center, TextVerticalAlignment.Middle));
        }

    }
}
