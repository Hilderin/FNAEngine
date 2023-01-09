using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public class Plane
    {
        public Vector2 pos;

        public Vector2 normal;

        public Plane(Vector2 pos, Vector2 normal)
        {
            this.pos = pos;

            this.normal = normal;
        }
    }
}
