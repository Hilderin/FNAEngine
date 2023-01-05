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
        /// Left
        /// </summary>
        public InputMap Left = InputMap.A;

        /// <summary>
        /// Right key
        /// </summary>
        public InputMap Right = InputMap.D;

        /// <summary>
        /// Up key
        /// </summary>
        public InputMap Up = InputMap.W;

        /// <summary>
        /// Down key
        /// </summary>
        public InputMap Down = InputMap.S;

        /// <summary>
        /// Crouch key
        /// </summary>
        public InputMap Crouch = InputMap.C;

        /// <summary>
        /// Jump key
        /// </summary>
        public InputMap Jump = InputMap.Space;

        /// <summary>
        /// Fire key
        /// </summary>
        public InputMap Fire = InputMap.E;


        /// <summary>
        /// Is moving left?
        /// </summary>
        public bool IsLeftActive() { return _gameObject.Input.IsMapDown(Left) && !_gameObject.Input.IsMapDown(Right); }

        /// <summary>
        /// Is moving right?
        /// </summary>
        public bool IsRightActive() { return _gameObject.Input.IsMapDown(Right) && !_gameObject.Input.IsMapDown(Left); }

        /// <summary>
        /// Is moving up?
        /// </summary>
        public bool IsUpActive() { return _gameObject.Input.IsMapDown(Up) && !_gameObject.Input.IsMapDown(Down); }

        /// <summary>
        /// Is moving down?
        /// </summary>
        public bool IsDownActive() { return _gameObject.Input.IsMapDown(Down) && !_gameObject.Input.IsMapDown(Up); }

        /// <summary>
        /// Is crouching
        /// </summary>
        public bool IsCrouchActive() { return _gameObject.Input.IsMapDown(Crouch); }

        /// <summary>
        /// Is jumping
        /// </summary>
        public bool IsJumpActive() { return _gameObject.Input.IsMapDown(Jump); }

        /// <summary>
        /// Is newly jumping
        /// </summary>
        public bool IsJumpNewlyActive() { return _gameObject.Input.IsMapNewDown(Jump); }

        /// <summary>
        /// Is newly crouching
        /// </summary>
        public bool IsCrouchNewlyActive() { return _gameObject.Input.IsMapNewDown(Crouch); }

        /// <summary>
        /// Is newly not crouching
        /// </summary>
        public bool IsCrouchNewlyInactive() { return _gameObject.Input.IsMapNewUp(Crouch); }

        /// <summary>
        /// Is Firing
        /// </summary>
        public bool IsFireActive() { return _gameObject.Input.IsMapDown(Fire); }

        /// <summary>
        /// Is newly Fireing
        /// </summary>
        public bool IsFireNewlyActive() { return _gameObject.Input.IsMapNewDown(Fire); }

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

            if (this.IsLeftActive())
                deltaX -= 1f;
            if (this.IsRightActive())
                deltaX += 1f;

            if (this.IsUpActive())
                deltaY -= 1f;
            if (this.IsDownActive())
                deltaY += 1f;

            if (deltaX == 0 && deltaY == 0)
                return Vector2.Zero;

            return VectorHelper.Normalize(deltaX, deltaY);
        }


        /// <summary>
        /// Return the mouse position
        /// </summary>
        public Vector2 GetMousePosition()
        { 
            return _gameObject.Input.GetMousePosition(); 
        }

        /// <summary>
        /// Return the mouse position in the world via the Main Camera
        /// </summary>
        public Vector2 GetMouseWorldPosition()
        {
            return _gameObject.Input.GetMouseWorldPosition();
        }

        /// <summary>
        /// Return the mouse position in the world
        /// </summary>
        public Vector2 GetMouseWorldPosition(Camera camera)
        {
            return _gameObject.Input.GetMouseWorldPosition(camera);
        }


    }
}
