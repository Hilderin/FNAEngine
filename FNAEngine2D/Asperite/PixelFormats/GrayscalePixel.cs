using Microsoft.Xna.Framework;

namespace FNAEngine2D.Aseprite.PixelFormats
{
    public class GrayscalePixel : Pixel
    {
        public byte[] Color { get; private set; }

        public GrayscalePixel(Frame frame, byte[] color) : base(frame)
        {
            Color = color;
        }

        public override Color GetColor()
        {
            //float value = (float)Color[0] / 255;
            //float alpha = (float)Color[1] / 255;

            return new Color(Color[0], Color[0], Color[0], Color[1]);
        }
    }
}
