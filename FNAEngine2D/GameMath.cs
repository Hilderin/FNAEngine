using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace FNAEngine2D
{
    public static class GameMath
    {

        public const float PiThreeQuarter = (float)Math.PI * 0.75f;
        public const float MinusPiThreeQuarter = (float)Math.PI * -0.75f;
        public const float PiQuarter = (float)Math.PI * 0.25f;
        public const float MinusPiQuarter = (float)Math.PI * -0.25f;


        /// <summary>
        /// Random object
        /// </summary>
        private static Random _random = new Random();

        /// <summary>
        /// Converti un rad en degree
        /// </summary>
        public static float RadToDeg(float radians)
        {
            float degrees = (180 / MathHelper.Pi) * radians % 360;
            //while (degrees > 360)
            //    degrees -= 360;
            //while (degrees < -360)
            //    degrees += 360;
            return degrees;
        }

        /// <summary>
        /// Converti un degree en rad
        /// </summary>
        public static float DegToRad(float angle)
        {
            return (MathHelper.Pi / 180) * angle;
        }

        /// <summary>
        /// Permet d'arroundir une valeur float en int
        /// </summary>
        public static int RoundInt(float value)
        {
            return (int)Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }

        // <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">
        /// The minimum value. If <c>value</c> is less than <c>min</c>, <c>min</c>
        /// will be returned.
        /// </param>
        /// <param name="max">
        /// The maximum value. If <c>value</c> is greater than <c>max</c>, <c>max</c>
        /// will be returned.
        /// </param>
        /// <returns>The clamped value.</returns>
        public static float Clamp(float value, float min, float max)
        {
            // First we check to see if we're greater than the max.
            if (value > max)
                return max;

            // Then we check to see if we're less than the min.
            if (value < min)
                return min;

            // There's no check to see if min > max.
            return value;
        }

        /// <summary>
        /// Permet d'obtenir une valeur random entre 2 floats
        /// </summary>
        public static float RandomFloat(float min, float max)
        {
            return (float)(_random.NextDouble() * (max - min)) + min;
        }

        /// <summary>
        /// Calculate the square value
        /// </summary>
        public static float Sqrt(float value)
        {
            return value * value;
        }

        /// <summary>
        /// Calculate the absolute value
        /// </summary>
        public static float Abs(float value)
        {
            return Math.Abs(value);
        }



        /// <summary>
        /// Return a byte from a float base 1
        /// </summary>
        public static byte Float1ToByte(float value)
        {
            value = value * 255;
            if (value > 255)
                return 255;
            if (value < 0)
                return 0;
            return (byte)value;
        }

        /// <summary>
        /// Return a byte from a float base 1
        /// </summary>
        public static float ByteToFloat1(byte value)
        {
            return value / 255f;
        }

        /// <summary>
        /// Swap 2 values
        /// </summary>
        public static void SwapValues<T>(ref T valueA, ref T valueB)
        {
            T temp = valueA;
            valueA = valueB;
            valueA = temp;
        }


    }
}
