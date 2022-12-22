using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public static class Input //Static classes can easily be accessed anywhere in our codebase. They always stay in memory so you should only do it for universal things like input.
    {
        private static KeyboardState _keyboardState = Keyboard.GetState();
        private static KeyboardState _lastKeyboardState;

        private static MouseState _mouseState;
        private static MouseState _lastMouseState;

        private static Vector2 _mousePosition;

        /// <summary>
        /// Consumed key pressed
        /// </summary>
        private static List<Keys> _consumedKeyPressed = new List<Keys>();

        /// <summary>
        /// Call at each Update
        /// </summary>
        public static void Update()
        {
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();

            _mousePosition = (new Vector2(_mouseState.X, _mouseState.Y) / GameHost.InternalGameHost.ScreenScale) + GameHost.MainCamera.Location;

            _consumedKeyPressed.Clear();
        }

        /// <summary>
        /// Checks if key is currently pressed.
        /// </summary>
        public static bool IsKeyDown(Keys input)
        {
            return _keyboardState.IsKeyDown(input);
        }

        /// <summary>
        /// Checks if key newly down
        /// </summary>
        public static bool IsKeyNewDown(Keys input)
        {
            return _keyboardState.IsKeyDown(input) && !_lastKeyboardState.IsKeyDown(input);
        }

        /// <summary>
        /// Checks if key is currently up.
        /// </summary>
        public static bool IsKeyUp(Keys input)
        {
            return _keyboardState.IsKeyUp(input);
        }

        /// <summary>
        /// Checks if key was just pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys input, bool consume = true)
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
        public static void ConsumeKeyPressed(Keys input)
        {
            _consumedKeyPressed.Add(input);
        }

        /// <summary>
        /// Returns whether or not the left mouse button is being pressed.
        /// </summary>
        public static bool MouseLeftDown()
        {
            if (_mouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the left mouse button is newlly down
        /// </summary>
        public static bool MouseLeftNewDown()
        {
            if (_mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the right mouse button is newlly down
        /// </summary>
        public static bool MouseRightNewDown()
        {
            if (_mouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the right mouse button is being pressed.
        /// </summary>
        public static bool MouseRightDown()
        {
            if (_mouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the left mouse button was clicked.
        /// </summary>
        public static bool MouseLeftClicked()
        {
            if (_mouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the right mouse button was clicked.
        /// </summary>
        public static bool MouseRightClicked()
        {
            if (_mouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets mouse coordinates adjusted for virtual resolution and camera position.
        /// </summary>
        public static Vector2 MousePosition()
        {
            return _mousePosition;
        }

        ///// <summary>
        ///// Gets the last mouse coordinates adjusted for virtual resolution and camera position.
        ///// </summary>
        //public static Vector2 LastMousePositionCamera()
        //{
        //    Vector2 mousePosition = Vector2.Zero;
        //    mousePosition.X = _lastMouseState.X;
        //    mousePosition.Y = _lastMouseState.Y;

        //    return ScreenToWorld(mousePosition);
        //}

        ///// <summary>
        ///// Takes screen coordinates (2D position like where the mouse is on screen) then converts it to world position (where we clicked at in the world). 
        ///// </summary>
        //private static Vector2 ScreenToWorld(Vector2 input)
        //{
        //    input.X -= Resolution.VirtualViewportX;
        //    input.Y -= Resolution.VirtualViewportY;

        //    return Vector2.Transform(input, Matrix.Invert(Camera.GetTransformMatrix()));
        //}
    }
}
