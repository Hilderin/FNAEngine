using Microsoft.Xna.Framework;

namespace FNAEngine2D.Geometry
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
