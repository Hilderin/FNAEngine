using Microsoft.Xna.Framework;

namespace FNAEngine2D.Template
{
    public class Ball: GameObject
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
            _rigidBody.SpeedMps = GameMath.RandomFloat(0.1f, 2f);

            //Random movement
            _rigidBody.Movement = new Vector2(GameMath.RandomFloat(-1, 1), GameMath.RandomFloat(-1, 1));
        }

        /// <summary>
        /// Update each frame
        /// </summary>
        protected override void Update()
        {
            if(_rigidBody.Collision != null)
            {
                AdjustMovement(_rigidBody.Collision);

                foreach (GameObject obj in _rigidBody.Collision.CollidesWith)
                {
                    if (obj is Ball)
                    {
                        //Also invert the movement of the ball
                        ((Ball)obj).AdjustMovement(_rigidBody.Collision);
                    }
                }

            }
        }

        /// <summary>
        /// Adjust movement after a collision
        /// </summary>
        private void AdjustMovement(Collision collision)
        {
            //Direction4 direction = _rigidBody.Movement.Direction4();
            Direction4 direction = collision.Direction;
            if (direction == Direction4.Down || direction == Direction4.Up)
                _rigidBody.Movement *= new Vector2(0, -1);
            else
                _rigidBody.Movement *= new Vector2(-1, 0);
        }
    }
}
