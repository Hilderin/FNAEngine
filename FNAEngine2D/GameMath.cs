using Microsoft.Xna.Framework;
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


    }
}
