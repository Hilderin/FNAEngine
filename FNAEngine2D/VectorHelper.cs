using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public static class VectorHelper
    {
        /// <summary>
        /// Vector2 NaN
        /// </summary>
        public static Vector2 NaN = VectorHelper.Normalize(0, 0);

        /// <summary>
        /// Permet de trouver le centre
        /// </summary>
        public static Vector2 Center(Point size, Point objectSize)
        {
            return new Vector2((size.X / 2) - (objectSize.X / 2), (size.Y / 2) - (objectSize.Y / 2));
        }

        /// <summary>
        /// Permet de trouver le centre
        /// </summary>
        public static Vector2 Center(Point size, Vector2 objectSize)
        {
            return new Vector2((size.X / 2) - (objectSize.X / 2), (size.Y / 2) - (objectSize.Y / 2));
        }

        /// <summary>
        /// Permet de trouver le centre
        /// </summary>
        public static Vector2 Center(int width, int height, int objectWidth, int objectHeight)
        {
            return new Vector2((width / 2) - (objectWidth / 2), (height / 2) - (objectHeight / 2));
        }


        /// <summary>
        /// Permet de faire une rotation sur un point d'origine
        /// </summary>
        public static Vector2 Rotate(Vector2 toRotate, Vector2 origin, float radians)
        {
            //On fait le - car sinon, la rotation va partir dans l'autre sens...
            return Vector2.Transform(toRotate - origin, Matrix.CreateRotationZ(-radians)) + origin;
        }

        /// <summary>
        /// Check if vector2 in rectangle
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Vector2 vector, Rectangle bounds)
        {
            return (bounds.X <= vector.X && bounds.Right >= vector.X
                    && bounds.Y <= vector.Y && bounds.Bottom >= vector.Y);
        }

        /// <summary>
        /// Check if 2 rectangles reprensented by locations and size intersects
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Intersects(Vector2 locationA, Vector2 sizeA, Vector2 locationB, Vector2 sizeB)
        {
            return (locationB.X <= (locationA.X + sizeA.X) && (locationB.X + sizeB.X) >= locationA.X
                    && locationB.Y <= (locationA.Y + sizeA.Y) && (locationB.Y + sizeB.Y) >= locationA.Y);
        }

        /// <summary>
        /// Check if 2 rectangles reprensented by locations and size intersects
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(Vector2 locationA, Vector2 sizeA, Vector2 locationB, Vector2 sizeB)
        {
            return locationA.X <= locationB.X && (locationA.X + sizeA.X) >= (locationB.X + sizeB.X)
                   && locationA.Y <= locationB.Y && (locationA.Y + sizeA.Y) >= (locationB.Y + sizeB.Y);
        }

        /// <summary>
        /// Add on x axis
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 AddX(Vector2 vector, float x)
        {
            return new Vector2(vector.X + x, vector.Y);
        }

        /// <summary>
        /// Add on y axis
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 AddY(Vector2 vector, float y)
        {
            return new Vector2(vector.X, vector.Y + y);
        }

        /// <summary>
        /// Add on x and y axis
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 AddXY(Vector2 vector, float x, float y)
        {
            return new Vector2(vector.X + x, vector.Y + y);
        }

        /// <summary>
        /// Add a point
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Add(Vector2 vector, Point point)
        {
            return new Vector2(vector.X + point.X, vector.Y + point.Y);
        }

        /// <summary>
        /// Substract a point
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Substract(Vector2 vector, Point point)
        {
            return new Vector2(vector.X - point.X, vector.Y - point.Y);
        }


        /// <summary>
        /// /// <summary>
        /// Creates a Vector2 from an angle and magnitude.
        /// </summary>
        public static Vector2 FromPolar(float radians, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        /// <summary>
        /// Get the angle af a vector 2
        /// </summary>
        public static float ToAngle(Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }

        /// <summary>
		/// Creates a new <see cref="Vector2"/> that contains a normalized values from x and y
		/// </summary>
		public static Vector2 Normalize(float x, float y)
        {
            float val = 1.0f / (float)Math.Sqrt((x * x) + (y * y));
            return new Vector2(x * val, y * val);
        }

        /// <summary>
		/// Creates a new <see cref="Vector2"/> that contains a normalized values from x and y
		/// </summary>
		public static Vector2 Normalize(Vector2 vector)
        {
            return Vector2.Normalize(vector);
        }



        /// <summary>
        /// Calculate the direction from a radians
        /// </summary>
        public static Direction4 GetDirection4(Vector2 vector)
        {
            float radians = vector.ToAngle();

            if (radians >= GameMath.PiThreeQuarter || radians <= GameMath.MinusPiThreeQuarter)
                return Direction4.Left;

            if (radians >= GameMath.MinusPiThreeQuarter && radians <= GameMath.MinusPiQuarter)
                return Direction4.Up;

            if (radians >= GameMath.MinusPiQuarter && radians <= GameMath.PiQuarter)
                return Direction4.Right;

            return Direction4.Down;


        }

        /// <summary>
        /// Calculate the direction from a radians an origin and a destination
        /// </summary>
        public static Direction4 GetDirection4To(Vector2 origin, Vector2 destination)
        {
            float radians = (destination - origin).ToAngle();

            if (radians >= GameMath.PiThreeQuarter || radians <= GameMath.MinusPiThreeQuarter)
                return Direction4.Left;

            if (radians >= GameMath.MinusPiThreeQuarter && radians <= GameMath.MinusPiQuarter)
                return Direction4.Up;

            if (radians >= GameMath.MinusPiQuarter && radians <= GameMath.PiQuarter)
                return Direction4.Right;

            return Direction4.Down;


        }


    }
}
