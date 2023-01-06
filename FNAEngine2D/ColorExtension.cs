using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FNAEngine2D
{
    /// <summary>
    /// Extension on colors
    /// </summary>
    public static class ColorExtension
    {

        /// <summary>
        /// Get R in a float based 1
        /// </summary>
        public static float GetRFloat(this Color color)
        {
            return GameMath.ByteToFloat1(color.R);
        }

        /// <summary>
        /// Get G in a float based 1
        /// </summary>
        public static float GetGFloat(this Color color)
        {
            return GameMath.ByteToFloat1(color.G);
        }

        /// <summary>
        /// Get B in a float based 1
        /// </summary>
        public static float GetBFloat(this Color color)
        {
            return GameMath.ByteToFloat1(color.B);
        }

        /// <summary>
        /// Get A in a float based 1
        /// </summary>
        public static float GetAFloat(this Color color)
        {
            return GameMath.ByteToFloat1(color.A);
        }

        ///// <summary>
        ///// Get R in a float based 1
        ///// </summary>
        //public static void SetRFloat(this Color color, float value)
        //{
        //    color.R = GameMath.Float1ToByte(value);
        //}

        ///// <summary>
        ///// Get G in a float based 1
        ///// </summary>
        //public static void SetGFloat(this Color color, float value)
        //{
        //    color.G = GameMath.Float1ToByte(value);
        //}

        ///// <summary>
        ///// Get B in a float based 1
        ///// </summary>
        //public static void SetBFloat(this Color color, float value)
        //{
        //    color.B = GameMath.Float1ToByte(value);
        //}

        ///// <summary>
        ///// Get A in a float based 1
        ///// </summary>
        //public static void SetAFloat(this Color color, float value)
        //{
        //    color.A = GameMath.Float1ToByte(value);
        //}


        /// <summary>
        /// Add 2 colors
        /// </summary>
        public static Color Add(this Color color, Color color2)
        {
            return new Color(GameMath.Float1ToByte(color.GetRFloat() + color2.GetRFloat())
                            , GameMath.Float1ToByte(color.GetGFloat() + color2.GetGFloat())
                            , GameMath.Float1ToByte(color.GetBFloat() + color2.GetBFloat())
                            , GameMath.Float1ToByte(color.GetAFloat() + color2.GetAFloat()));
        }

        /// <summary>
        /// Substract 2 colors
        /// </summary>
        public static Color Substract(this Color color, Color color2)
        {
            return new Color(GameMath.Float1ToByte(color.GetRFloat() - color2.GetRFloat())
                            , GameMath.Float1ToByte(color.GetGFloat() - color2.GetGFloat())
                            , GameMath.Float1ToByte(color.GetBFloat() - color2.GetBFloat())
                            , GameMath.Float1ToByte(color.GetAFloat() - color2.GetAFloat()));
        }

        /// <summary>
        /// Multiply a color to value
        /// </summary>
        public static Color Multiply(this Color color, float value)
        {
            if (value == 1f)
                return color;

            return new Color(GameMath.Float1ToByte(color.GetRFloat() * value)
                            , GameMath.Float1ToByte(color.GetGFloat() * value)
                            , GameMath.Float1ToByte(color.GetBFloat() * value)
                            , GameMath.Float1ToByte(color.GetAFloat() * value));
        }


        /// <summary>
        /// Multiply 2 colors
        /// </summary>
        public static Color Multiply(this Color color, Color color2)
        {
            return new Color(GameMath.Float1ToByte(color.GetRFloat() * color2.GetRFloat())
                            , GameMath.Float1ToByte(color.GetGFloat() * color2.GetGFloat())
                            , GameMath.Float1ToByte(color.GetBFloat() * color2.GetBFloat())
                            , GameMath.Float1ToByte(color.GetAFloat() * color2.GetAFloat()));
        }

    }
}
