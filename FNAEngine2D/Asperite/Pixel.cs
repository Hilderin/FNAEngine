using Microsoft.Xna.Framework;

namespace FNAEngine2D.Aseprite
{
    public abstract class Pixel
    {
        protected Frame Frame = null;
        public abstract Color GetColor();

        public Pixel(Frame frame)
        {
            Frame = frame;
        }
    }
}

