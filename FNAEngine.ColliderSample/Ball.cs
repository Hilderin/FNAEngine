using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.ColliderSample
{
    public class Ball: GameObject
    {

        private GameObject _obj;
        private Vector2 _movement = Vector2.Zero;

        public Ball()
        {
            this.Width = 8;
            this.Height = 8;
        }

        public override void Load()
        {
            _obj = Add(new TextureRender("circle", this.Bounds));
            //this.EnableCollider();
            this.AddComponent(new ColliderCircle(new Vector2(this.Width / 2), this.Width / 2));
            _movement = new Vector2(GameMath.RandomFloat(-1, 1), GameMath.RandomFloat(-1, 1));
        }


        public override void Update()
        {
            Vector2 delta = _movement * 3 * this.ElapsedGameTimeSeconds * this.NbPixelPerMeter;
            Vector2 nextLocation = _obj.Location + delta;

            Collision collision = this.GetCollision(nextLocation, null);
            if (collision != null)
            {
                if (collision.Direction == CollisionDirection.MovingColliderOnTop || collision.Direction == CollisionDirection.MovingColliderOnBottom)
                    _movement.Y *= -1;
                else
                    _movement.X *= -1;

                foreach (GameObject obj in collision.CollidesWith)
                {
                    if (obj is Ball)
                    {
                        if (collision.Direction == CollisionDirection.MovingColliderOnTop || collision.Direction == CollisionDirection.MovingColliderOnBottom)
                            ((Ball)obj)._movement.Y *= -1;
                        else
                            ((Ball)obj)._movement.X *= -1;
                    }
                }

                delta = _movement * 3 * this.ElapsedGameTimeSeconds * this.NbPixelPerMeter;
                nextLocation = _obj.Location + delta;
            }
            
                this.Location = nextLocation;
        }
    }
}
