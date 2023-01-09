using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    //And edge between two vertices
    public class Edge
    {
        public Vertex v1;
        public Vertex v2;

        //Is this edge intersecting with another edge?
        public bool isIntersecting = false;

        public Edge(Vertex v1, Vertex v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public Edge(Vector2 v1, Vector2 v2)
        {
            this.v1 = new Vertex(v1);
            this.v2 = new Vertex(v2);
        }

        ////Get vertex in 2d space (assuming x, z)
        //public Vector2 GetVertex2D(Vertex v)
        //{
        //    return new Vector2(v.position.x, v.position.z);
        //}

        //Flip edge
        public void FlipEdge()
        {
            Vertex temp = v1;

            v1 = v2;

            v2 = temp;
        }
    }
}
