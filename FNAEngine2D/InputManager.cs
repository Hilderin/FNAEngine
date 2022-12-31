using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Mamange the input system
    /// </summary>
    public class InputManager 
    {
        private Game _game;

        private KeyboardState _keyboardState = Keyboard.GetState();
        private KeyboardState _lastKeyboardState;

        private MouseState _mouseState;
        private MouseState _lastMouseState;

        private Vector2 _mousePosition;

        /// <summary>
        /// Consumed key pressed
        /// </summary>
        private List<Keys> _consumedKeyPressed = new List<Keys>();

        

        /// <summary>
        /// Constructor
        /// </summary>
        public InputManager(Game game)
        {
            _game = game;
        }

        /// <summary>
        /// Call at each Update
        /// </summary>
        public void Update()
        {
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();

            _mousePosition = (new Vector2(_mouseState.X, _mouseState.Y) / _game.ScreenScale) + _game.MainCamera.Location.Substract(_game.MainCamera.ViewLocation);

            _consumedKeyPressed.Clear();
        }

        /// <summary>
        /// Checks if key is currently pressed.
        /// </summary>
        public bool IsKeyDown(Keys input)
        {
            return _keyboardState.IsKeyDown(input);
        }

        /// <summary>
        /// Checks if key newly down
        /// </summary>
        public bool IsKeyNewDown(Keys input)
        {
            return _keyboardState.IsKeyDown(input) && !_lastKeyboardState.IsKeyDown(input);
        }

        /// <summary>
        /// Checks if key is currently up.
        /// </summary>
        public bool IsKeyUp(Keys input)
        {
            return _keyboardState.IsKeyUp(input);
        }

        /// <summary>
        /// Checks if key was just pressed.
        /// </summary>
        public bool IsKeyPressed(Keys input, bool consume = true)
        {
            if (_keyboardState.IsKeyDown(input) == true && _lastKeyboardState.IsKeyDown(input) == false
                && !_consumedKeyPressed.Contains(input))
            {
                if (consume)
                    ConsumeKeyPressed(input);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Remove the key pressed from the state, we processed it
        /// The goal is to prevent multiple gamepobject to process the keypressed
        /// </summary>
        public void ConsumeKeyPressed(Keys input)
        {
            _consumedKeyPressed.Add(input);
        }

        /// <summary>
        /// Returns whether or not the left mouse button is being pressed.
        /// </summary>
        public bool MouseLeftDown()
        {
            if (_mouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the left mouse button is newlly down
        /// </summary>
        public bool MouseLeftNewDown()
        {
            if (_mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the right mouse button is newlly down
        /// </summary>
        public bool MouseRightNewDown()
        {
            if (_mouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the right mouse button is being pressed.
        /// </summary>
        public bool MouseRightDown()
        {
            if (_mouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the left mouse button was clicked.
        /// </summary>
        public bool MouseLeftClicked()
        {
            if (_mouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the right mouse button was clicked.
        /// </summary>
        public bool MouseRightClicked()
        {
            if (_mouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets mouse coordinates adjusted for virtual resolution and camera position.
        /// </summary>
        public Vector2 MousePosition()
        {
            return _mousePosition;
        }

        ///// <summary>
        ///// Gets the last mouse coordinates adjusted for virtual resolution and camera position.
        ///// </summary>
        //public Vector2 LastMousePositionCamera()
        //{
        //    Vector2 mousePosition = Vector2.Zero;
        //    mousePosition.X = _lastMouseState.X;
        //    mousePosition.Y = _lastMouseState.Y;

        //    return ScreenToWorld(mousePosition);
        //}

        ///// <summary>
        ///// Takes screen coordinates (2D position like where the mouse is on screen) then converts it to world position (where we clicked at in the world). 
        ///// </summary>
        //private Vector2 ScreenToWorld(Vector2 input)
        //{
        //    input.X -= Resolution.VirtualViewportX;
        //    input.Y -= Resolution.VirtualViewportY;

        //    return Vector2.Transform(input, Matrix.Invert(Camera.GetTransformMatrix()));
        //}
    }
}
