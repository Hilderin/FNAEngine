using FNAEngine2D.Collisions;
using FNAEngine2D.Physics;
using FNAEngine2D.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.ColliderSample
{
    public class Ball : GameObject, IUpdate
    {
        /// <summary>
        /// Rigid body that will handle movement
        /// </summary>
        private RigidBody _rigidBody;

        /// <summary>
        /// Constructor
        /// </summary>
        public Ball()
        {
            this.Width = 4;
            this.Height = 4;
        }

        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            //Add a primitive render to draw the circle
            Add(new PrimitiveRender(PrimitiveType.CircleFill, this.Bounds, Color.Green));

            //Enable a circle collider at the center of our object
            this.EnableColliderCircle();

            //Rigid body with gravity
            _rigidBody = AddComponent<RigidBody>();
            _rigidBody.UseGravity = false;
            _rigidBody.SpeedMps = GameMath.RandomFloat(0.2f, 5);

            //Random movement
            _rigidBody.Movement = new Vector2(GameMath.RandomFloat(-1, 1), GameMath.RandomFloat(-1, 1));
        }

        /// <summary>
        /// Update each frame
        /// </summary>
        public void Update()
        {
            if (_rigidBody.Collision != null)
            {

                AdjustMovement(_rigidBody.Collision);

                //foreach (GameObject obj in _rigidBody.Collision.CollidesWith)
                //{
                //    if (obj is Ball)
                //    {
                //        //Also invert the movement of the ball
                //        ((Ball)obj).AdjustMovement(_rigidBody.Collision);
                //    }
                //}

                
            }
        }

        /// <summary>
        /// Adjust movement after a collision
        /// </summary>
        private void AdjustMovement(Collision collision)
        {
            Vector2 direction = _rigidBody.Movement;

            foreach (GameObject obj in _rigidBody.Collision.CollidesWith)
            {
                if (obj is Ball)
                {
                    direction -= VectorHelper.Normalize(obj.Location - this.Location);
                }
                else
                {
                    if (collision.Direction == Direction4.Down || collision.Direction == Direction4.Up)
                        direction *= new Vector2(1, -1);
                    else
                        direction *= new Vector2(-1, 1);
                }
            }

            _rigidBody.Movement = direction;



        }
    }
}
