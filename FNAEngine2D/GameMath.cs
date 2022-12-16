﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public static class GameMath
    {
        /// <summary>
        /// Converti un rad en degree
        /// </summary>
        public static float RadToDeg(float radians)
        {
            float degrees = (180 / MathHelper.Pi) * radians;
            while (degrees > 360)
                degrees -= 360;
            while (degrees < -360)
                degrees += 360;
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

    }
}
