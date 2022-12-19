using Microsoft.Xna.Framework;

namespace FNAEngine2D.Aseprite.PixelFormats
{
    public class RGBAPixel : Pixel
    {
        public byte[] Color { get; private set; }

        public RGBAPixel(Frame frame, byte[] color) : base(frame)
        {
            Color = color;
        }

        public override Color GetColor()
        {
            if (Color.Length == 4)
            {
                return new Color(Color[0], Color[1], Color[2], Color[3]);
            }
            else
            {
                return Microsoft.Xna.Framework.Color.Magenta;
            }
        }
    }
}
