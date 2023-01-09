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
        /// <summary>
		/// Represents the value of pi(3.14159274).
		/// </summary>
		public const float Pi = (float)Math.PI;

        /// <summary>
        /// Represents the value of pi divided by two(1.57079637).
        /// </summary>
        public const float PiOver2 = (float)(Math.PI / 2.0);

        /// <summary>
        /// Represents the value of pi divided by four (0.7853982).
        /// </summary>
        public const float PiOver4 = (float)(Math.PI / 4.0);

        /// <summary>
        /// Represents the value of pi divided by minus four (-0.7853982).
        /// </summary>
        public const float MinusPiMinusOver4 = (float)Math.PI * -0.25f;

        /// <summary>
        /// Represents the value of pi times two (6.28318548).
        /// </summary>
        public const float TwoPi = (float)(Math.PI * 2.0);

        /// <summary>
        /// Represents the value of pi multiplied by 0.75 (2.3562).
        /// </summary>
        public const float PiThreeQuarter = (float)Math.PI * 0.75f;

        /// <summary>
        /// Represents the value of pi multiplied by minus 0.75 (-2.3562).
        /// </summary>
        public const float PiMinusThreeQuarter = (float)Math.PI * -0.75f;

        /// <summary>
        /// The value we use to avoid floating point precision issues
        /// http://sandervanrossen.blogspot.com/2009/12/realtime-csg-part-1.html
        /// </summary>
        public const float EPSILON = 0.00001f;

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
        /// Get a random int between 2 values (max is exclusive)
        /// </summary>
        public static int RandomInt(int min, int exlusiveMax)
        {
            return _random.Next(min, exlusiveMax);
        }

        /// <summary>
        /// Get a random int between 2 values (max is exclusive)
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

        /// <summary>
        /// Test if a float is the same as another float
        /// </summary>
        public static bool AreFloatsEqual(float a, float b)
        {
            float diff = a - b;

            float e = EPSILON;

            if (diff < e && diff > -e)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Clamp list indices
        /// Will even work if index is larger/smaller than listSize, so can loop multiple times
        /// </summary>
        public static int ClampListIndex(int index, int listSize)
        {
            index = ((index % listSize) + listSize) % listSize;

            return index;
        }

    }
}
