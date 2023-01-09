using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public class Triangle
    {
        //Corners
        public Vertex v1;
        public Vertex v2;
        public Vertex v3;

        /// <summary>
        /// If we are using the half edge mesh structure, we just need one half edge
        /// </summary>
        public HalfEdge halfEdge;

        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public Triangle(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            this.v1 = new Vertex(v1);
            this.v2 = new Vertex(v2);
            this.v3 = new Vertex(v3);
        }

        public Triangle(HalfEdge halfEdge)
        {
            this.halfEdge = halfEdge;
        }

        /// <summary>
        /// Change orientation of triangle from cw -> ccw or ccw -> cw
        /// </summary>
        public void ChangeOrientation()
        {
            Vertex temp = this.v1;

            this.v1 = this.v2;

            this.v2 = temp;
        }

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Triangle triangle &&
                   v1.position == triangle.v1.position &&
                   v2.position == triangle.v2.position &&
                   v3.position == triangle.v3.position;
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        public override int GetHashCode()
        {
            int hashCode = 1934698336;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(v1);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(v2);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(v3);
            hashCode = hashCode * -1521134295 + EqualityComparer<HalfEdge>.Default.GetHashCode(halfEdge);
            return hashCode;
        }

        /// <summary>
        /// Compares whether two <see cref="Triangle"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Triangle"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Triangle"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Triangle value1, Triangle value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Compares whether two <see cref="Triangle"/> instances are equal.
        /// </summary>
        /// <param name="value1"><see cref="Triangle"/> instance on the left of the equal sign.</param>
        /// <param name="value2"><see cref="Triangle"/> instance on the right of the equal sign.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        public static bool operator !=(Triangle value1, Triangle value2)
        {
            return !(value1 == value2);
        }
    }
}
