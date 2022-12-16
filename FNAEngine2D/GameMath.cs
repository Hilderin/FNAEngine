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
        public static double ConvertRadToDeg(double radians)
        {
            double degrees = (180 / Math.PI) * radians;
            while (degrees > 360)
                degrees -= 360;
            while (degrees < -360)
                degrees += 360;
            return (degrees);
        }

        /// <summary>
        /// Converti un degree en rad
        /// </summary>
        public static double ConvertDegToRad(double angle)
        {
            return (Math.PI / 180) * angle;
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
