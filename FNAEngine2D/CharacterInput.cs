using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Helper class to control a character
    /// </summary>
    public class CharacterInput
    {
        /// <summary>
        /// Linked game object
        /// </summary>
        private GameObject _gameObject;

        /// <summary>
        /// Left key
        /// </summary>
        public Keys LeftKey = Keys.A;

        /// <summary>
        /// Right key
        /// </summary>
        public Keys RightKey = Keys.D;

        /// <summary>
        /// Up key
        /// </summary>
        public Keys UpKey = Keys.W;

        /// <summary>
        /// Down key
        /// </summary>
        public Keys DownKey = Keys.S;

        /// <summary>
        /// Jump key
        /// </summary>
        public Keys JumpKey = Keys.Space;

        /// <summary>
        /// Fire key
        /// </summary>
        public Keys FireKey = Keys.E;


        /// <summary>
        /// Is moving left?
        /// </summary>
        public bool IsLeft { get { return _gameObject.Input.IsKeyDown(LeftKey) && !_gameObject.Input.IsKeyDown(RightKey); } }

        /// <summary>
        /// Is moving right?
        /// </summary>
        public bool IsRight { get { return _gameObject.Input.IsKeyDown(RightKey) && !_gameObject.Input.IsKeyDown(LeftKey); } }

        /// <summary>
        /// Is moving up?
        /// </summary>
        public bool IsUp { get { return _gameObject.Input.IsKeyDown(UpKey) && !_gameObject.Input.IsKeyDown(DownKey); } }

        /// <summary>
        /// Is moving down?
        /// </summary>
        public bool IsDown { get { return _gameObject.Input.IsKeyDown(DownKey) && !_gameObject.Input.IsKeyDown(UpKey); } }

        /// <summary>
        /// Is jumping
        /// </summary>
        public bool IsJump { get { return _gameObject.Input.IsKeyDown(JumpKey); } }

        /// <summary>
        /// Is newly jumping
        /// </summary>
        public bool IsNewJump { get { return _gameObject.Input.IsKeyNewDown(JumpKey); } }

        /// <summary>
        /// Is Firing
        /// </summary>
        public bool IsFire { get { return _gameObject.Input.IsKeyDown(FireKey); } }

        /// <summary>
        /// Is newly Fireing
        /// </summary>
        public bool IsNewFire { get { return _gameObject.Input.IsKeyNewDown(FireKey); } }

        /// <summary>
        /// Constructor
        /// </summary>
        public CharacterInput(GameObject gameObject)
        {
            _gameObject = gameObject;
        }


        /// <summary>
        /// Get the movement Vector2
        /// </summary>
        public Vector2 GetMovement()
        {
            float deltaX = 0f;
            float deltaY = 0f;

            if (this.IsLeft)
                deltaX -= 1f;
            if (this.IsRight)
                deltaX += 1f;

            if (this.IsUp)
                deltaY -= 1f;
            if (this.IsDown)
                deltaY += 1f;

            if (deltaX == 0 && deltaY == 0)
                return Vector2.Zero;

            return VectorHelper.Normalize(deltaX, deltaY);
        }

    }
}
