using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// See: http://wwwimages.adobe.com/www.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf
// Page 333
namespace FNAEngine2D
{
    public static class Texture2DBlender
    {
        public static float Multiply(float b, float s)
        {
            return b * s;
        }

        public static float Screen(float b, float s)
        {
            return b + s - (b * s);
        }

        public static float Overlay(float b, float s)
        {
            return HardLight(s, b);
        }

        public static float Darken(float b, float s)
        {
            return MathHelper.Min(b, s);
        }

        public static float Lighten(float b, float s)
        {
            return MathHelper.Max(b, s);
        }

        // Color Dodge & Color Burn:  http://wwwimages.adobe.com/www.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/adobe_supplement_iso32000_1.pdf
        public static float ColorDodge(float b, float s)
        {
            if (b == 0)
                return 0;
            else if (b >= (1 - s))
                return 1;
            else
                return b / (1 - s);
        }

        public static float ColorBurn(float b, float s)
        {
            if (b == 1)
                return 1;
            else if ((1 - b) >= s)
                return 0;
            else
                return 1 - ((1 - b) / s);
        }

        public static float HardLight(float b, float s)
        {
            if (s <= 0.5)
                return Multiply(b, 2 * s);
            else
                return Screen(b, 2 * s - 1);
        }

        public static float SoftLight(float b, float s)
        {
            if (s <= 0.5)
                return b - (1 - 2 * s) * b * (1 - b);
            else
                return b + (2 * s - 1) * (SoftLightD(b) - b);
        }

        private static float SoftLightD(float x)
        {
            if (x <= 0.25)
                return ((16 * x - 12) * x + 4) * x;
            else
                return GameMath.Sqrt(x);
        }

        public static float Difference(float b, float s)
        {
            return GameMath.Abs(b - s);
        }

        public static float Exclusion(float b, float s)
        {
            return b + s - 2 * b * s;
        }


        public static Texture2D Normal(Texture2D baseLayer, Texture2D layer, float opacity)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                b.A = GameMath.Float1ToByte(b.GetAFloat() * opacity);

                Color c = a.Multiply(1f - b.GetAFloat()).Add(b.Multiply(b.GetAFloat()));
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);

            return newLayer;
        }

        public static Texture2D Multiply(Texture2D baseLayer, Texture2D layer, float opacity)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();

                c.R = GameMath.Float1ToByte((a.GetRFloat()) * (opacity * (2f - b.GetAFloat() * (1f - b.GetRFloat()))));
                c.G = GameMath.Float1ToByte((a.GetGFloat()) * (opacity * (1f - b.GetAFloat() * (1f - b.GetGFloat()))));
                c.B = GameMath.Float1ToByte((a.GetBFloat()) * (opacity * (1f - b.GetAFloat() * (1f - b.GetBFloat()))));
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }


        public static Texture2D Screen(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = a.Add(b).Substract(a.Multiply(b));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D Overlay(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();


                if (a.R < 0.5)
                    c.R = GameMath.Float1ToByte((byte)(2f * a.R * b.R * 255));
                else
                    c.R = GameMath.Float1ToByte(1f - 2f * (1f - b.GetRFloat()) * (1f - a.GetRFloat()));

                if (a.G < 0.5)
                    c.G = GameMath.Float1ToByte(2f * a.G * b.GetGFloat());
                else
                    c.G = GameMath.Float1ToByte(1f - 2f * (1f - b.GetGFloat()) * (1f - a.GetGFloat()));

                if (a.B < 0.5)
                    c.B = GameMath.Float1ToByte(2f * a.GetBFloat() * b.GetBFloat());
                else
                    c.B = GameMath.Float1ToByte(1f - 2f * (1f - b.GetBFloat()) * (1f - a.GetBFloat()));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);

                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));



                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D Darken(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();


                c.R = GameMath.Float1ToByte(MathHelper.Min(a.GetRFloat(), b.GetRFloat()));
                c.G = GameMath.Float1ToByte(MathHelper.Min(a.GetGFloat(), b.GetGFloat()));
                c.B = GameMath.Float1ToByte(MathHelper.Min(a.GetBFloat(), b.GetBFloat()));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D Lighten(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();

                c.R = GameMath.Float1ToByte(Lighten(a.GetRFloat(), b.GetRFloat()));
                c.G = GameMath.Float1ToByte(Lighten(a.GetGFloat(), b.GetGFloat()));
                c.B = GameMath.Float1ToByte(Lighten(a.GetBFloat(), b.GetBFloat()));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D ColorDodge(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();


                c.R = GameMath.Float1ToByte(ColorDodge(a.GetRFloat(), b.GetRFloat()));
                c.G = GameMath.Float1ToByte(ColorDodge(a.GetGFloat(), b.GetGFloat()));
                c.B = GameMath.Float1ToByte(ColorDodge(a.GetBFloat(), b.GetBFloat()));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D ColorBurn(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();

                c.R = GameMath.Float1ToByte(ColorBurn(a.GetRFloat(), b.GetRFloat()));
                c.G = GameMath.Float1ToByte(ColorBurn(a.GetGFloat(), b.GetGFloat()));
                c.B = GameMath.Float1ToByte(ColorBurn(a.GetBFloat(), b.GetBFloat()));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D HardLight(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();

                c.R = GameMath.Float1ToByte(HardLight(a.GetRFloat(), b.GetRFloat()));
                c.G = GameMath.Float1ToByte(HardLight(a.GetGFloat(), b.GetGFloat()));
                c.B = GameMath.Float1ToByte(HardLight(a.GetBFloat(), b.GetBFloat()));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D SoftLight(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();

                c.R = GameMath.Float1ToByte(SoftLight(a.GetRFloat(), b.GetRFloat()));
                c.G = GameMath.Float1ToByte(SoftLight(a.GetGFloat(), b.GetGFloat()));
                c.B = GameMath.Float1ToByte(SoftLight(a.GetBFloat(), b.GetBFloat()));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D Difference(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();

                c.R = GameMath.Float1ToByte(Difference(a.GetRFloat(), b.GetRFloat()));
                c.G = GameMath.Float1ToByte(Difference(a.GetGFloat(), b.GetGFloat()));
                c.B = GameMath.Float1ToByte(Difference(a.GetBFloat(), b.GetBFloat()));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D Exclusion(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color();

                c.R = GameMath.Float1ToByte(Exclusion(a.GetRFloat(), b.GetRFloat()));
                c.G = GameMath.Float1ToByte(Exclusion(a.GetGFloat(), b.GetGFloat()));
                c.B = GameMath.Float1ToByte(Exclusion(a.GetBFloat(), b.GetBFloat()));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }



        public static Texture2D Hue(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                var s = Sat(a);
                var l = Lum(a);

                Color c = SetLum(SetSat(b, s), l);

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D Saturation(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                var s = Sat(b);
                var l = Lum(a);

                Color c = SetLum(SetSat(a, s), l);

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D Color(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = SetLum(b, Lum(a));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c);
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        public static Texture2D Luminosity(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];



                Color c = SetLum(a, Lum(b));

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c); ;
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }


        public static Texture2D Addition(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = a.Add(b);

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c); ;
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }


        public static Texture2D Subtract(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = a.Substract(b);

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c); ;
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }


        public static Texture2D Divide(Texture2D baseLayer, Texture2D layer)
        {
            Texture2D newLayer = Texture2DHelper.CreateTexture(baseLayer.Width, baseLayer.Height);

            Color[] baseLayerColors = baseLayer.GetPixels();
            Color[] layerColors = layer.GetPixels();
            Color[] newLayerColors = new Color[baseLayerColors.Length];

            for (int i = 0; i < baseLayerColors.Length; i++)
            {
                Color a = baseLayerColors[i];
                Color b = layerColors[i];

                Color c = new Color(
                        BlendDivide(a.GetRFloat(), b.GetRFloat()),
                        BlendDivide(a.GetGFloat(), b.GetGFloat()),
                        BlendDivide(a.GetBFloat(), b.GetBFloat())
                        );

                c = a.Multiply(1f - b.GetAFloat()).Add(c.Multiply(b.GetAFloat()));
                //c = ((1f - b.GetAFloat()) * a) + (b.GetAFloat() * c); ;
                c.A = GameMath.Float1ToByte(a.GetAFloat() + b.GetAFloat() * (1f - a.GetAFloat()));

                newLayerColors[i] = c;
            }

            newLayer.SetPixels(0, 0, newLayer.Width, newLayer.Height, newLayerColors);


            return newLayer;
        }

        private static float BlendDivide(float b, float s)
        {
            if (b == 0)
                return 0;
            else if (b >= s)
                return 255;
            else
                return b / s;
        }


        private static double Lum(Color c)
        {
            return (0.3 * c.GetRFloat()) + (0.59 * c.GetGFloat()) + (0.11 * c.GetBFloat());
        }

        private static Color ClipColor(Color c)
        {
            double l = Lum(c);
            float n = Math.Min(c.GetRFloat(), Math.Min(c.GetGFloat(), c.GetBFloat()));
            float x = Math.Max(c.GetRFloat(), Math.Max(c.GetGFloat(), c.GetBFloat()));


            if (n < 0)
            {
                c.R = GameMath.Float1ToByte((float)(l + (((c.GetRFloat() - l) * l) / (l - n))));
                c.G = GameMath.Float1ToByte((float)(l + (((c.GetGFloat() - l) * l) / (l - n))));
                c.B = GameMath.Float1ToByte((float)(l + (((c.GetBFloat() - l) * l) / (l - n))));
            }
            if (x > 1)
            {
                c.R = GameMath.Float1ToByte((float)(l + (((c.GetRFloat() - l) * (1 - l)) / (x - l))));
                c.G = GameMath.Float1ToByte((float)(l + (((c.GetGFloat() - l) * (1 - l)) / (x - l))));
                c.B = GameMath.Float1ToByte((float)(l + (((c.GetBFloat() - l) * (1 - l)) / (x - l))));
            }

            return c;
        }




        private static Color SetLum(Color c, double l)
        {
            double d = l - Lum(c);
            c.R = GameMath.Float1ToByte((float)(c.GetRFloat() + d));
            c.G = GameMath.Float1ToByte((float)(c.GetGFloat() + d));
            c.B = GameMath.Float1ToByte((float)(c.GetBFloat() + d));

            return ClipColor(c);
        }

        private static double Sat(Color c)
        {
            return Math.Max(c.GetRFloat(), Math.Max(c.GetGFloat(), c.GetBFloat())) - Math.Min(c.GetRFloat(), Math.Min(c.GetGFloat(), c.GetBFloat()));
        }

        private static double DMax(double x, double y) { return (x > y) ? x : y; }
        private static double DMin(double x, double y) { return (x < y) ? x : y; }




        private static Color SetSat(Color c, double s)
        {
            char cMin = GetMinComponent(c);
            char cMid = GetMidComponent(c);
            char cMax = GetMaxComponent(c);

            double min = GetComponent(c, cMin);
            double mid = GetComponent(c, cMid);
            double max = GetComponent(c, cMax);


            if (max > min)
            {
                mid = ((mid - min) * s) / (max - min);
                c = SetComponent(c, cMid, (float)mid);
                max = s;
                c = SetComponent(c, cMax, (float)max);
            }
            else
            {
                mid = max = 0;
                c = SetComponent(c, cMax, (float)max);
                c = SetComponent(c, cMid, (float)mid);
            }

            min = 0;
            c = SetComponent(c, cMin, (float)min);

            return c;
        }




        private static float GetComponent(Color c, char component)
        {
            switch (component)
            {
                case 'r': return c.GetRFloat();
                case 'g': return c.GetGFloat();
                case 'b': return c.GetBFloat();
            }

            return 0f;
        }


        private static Color SetComponent(Color c, char component, float value)
        {
            switch (component)
            {
                case 'r': c.R = GameMath.Float1ToByte(value); break;
                case 'g': c.G = GameMath.Float1ToByte(value); break;
                case 'b': c.B = GameMath.Float1ToByte(value); break;
            }

            return c;
        }

        private static char GetMinComponent(Color c)
        {
            var r = new KeyValuePair<char, float>('r', c.GetRFloat());
            var g = new KeyValuePair<char, float>('g', c.GetGFloat());
            var b = new KeyValuePair<char, float>('b', c.GetBFloat());

            return MIN(r, MIN(g, b)).Key;
        }

        private static char GetMidComponent(Color c)
        {
            var r = new KeyValuePair<char, float>('r', c.GetRFloat());
            var g = new KeyValuePair<char, float>('g', c.GetGFloat());
            var b = new KeyValuePair<char, float>('b', c.GetBFloat());

            return MID(r, g, b).Key;
        }

        private static char GetMaxComponent(Color c)
        {
            var r = new KeyValuePair<char, float>('r', c.GetRFloat());
            var g = new KeyValuePair<char, float>('g', c.GetGFloat());
            var b = new KeyValuePair<char, float>('b', c.GetBFloat());

            return MAX(r, MAX(g, b)).Key;
        }

        private static KeyValuePair<char, float> MIN(KeyValuePair<char, float> x, KeyValuePair<char, float> y)
        {
            return (x.Value < y.Value) ? x : y;
        }

        private static KeyValuePair<char, float> MAX(KeyValuePair<char, float> x, KeyValuePair<char, float> y)
        {
            return (x.Value > y.Value) ? x : y;
        }

        private static KeyValuePair<char, float> MID(KeyValuePair<char, float> x, KeyValuePair<char, float> y, KeyValuePair<char, float> z)
        {
            List<KeyValuePair<char, float>> components = new List<KeyValuePair<char, float>>();
            components.Add(x);
            components.Add(z);
            components.Add(y);


            components.Sort((c1, c2) => { return c1.Value.CompareTo(c2.Value); });

            return components[1];
            //return MAX(x, MIN(y, z));
        }
    }
}

