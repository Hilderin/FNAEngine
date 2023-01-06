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
        /// Consumed mouse clicked
        /// </summary>
        private List<MouseButton> _consumedMouseButton = new List<MouseButton>();


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
            _consumedKeyPressed.Clear();
            _consumedMouseButton.Clear();

            _lastKeyboardState = _keyboardState;
            _lastMouseState = _mouseState;

            if (_game.IsActive)
            {                
                _keyboardState = Keyboard.GetState();

                _mouseState = Mouse.GetState();

                _mousePosition = (new Vector2(_mouseState.X, _mouseState.Y) / _game.ScreenScale);
            }
            else
            {
                //Game not active... we don't grab states... We set empty states
                _keyboardState = new KeyboardState();
                _mouseState = new MouseState();
            }

            
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
        /// Checks if key newly up
        /// </summary>
        public bool IsKeyNewUp(Keys input)
        {
            return _keyboardState.IsKeyUp(input) && !_lastKeyboardState.IsKeyUp(input);
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
        public bool IsMouseLeftDown()
        {
            if (_mouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the left mouse button is newlly down
        /// </summary>
        public bool IsMouseLeftNewDown()
        {
            if (_mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the right mouse button is newlly down
        /// </summary>
        public bool IsMouseRightNewDown()
        {
            if (_mouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns whether or not the right mouse button is being pressed.
        /// </summary>
        public bool IsMouseRightDown()
        {
            if (_mouseState.RightButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the left mouse button was clicked.
        /// </summary>
        public bool IsMouseLeftClicked()
        {
            if (_mouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if the right mouse button was clicked.
        /// </summary>
        public bool IsMouseRightClicked()
        {
            if (_mouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets mouse coordinates adjusted for virtual resolution
        /// </summary>
        public Vector2 GetMousePosition()
        {
            return _mousePosition;
        }

        /// <summary>
        /// Gets mouse coordinates adjusted for virtual resolution and main camera position.
        /// </summary>
        public Vector2 GetMouseWorldPosition()
        {
            return GetMouseWorldPosition(_game.MainCamera);
        }

        /// <summary>
        /// Gets mouse coordinates adjusted for virtual resolution and a camera position.
        /// </summary>
        public Vector2 GetMouseWorldPosition(Camera camera)
        {
            return _mousePosition + camera.Location.Substract(camera.ViewLocation);
        }


        /// <summary>
        /// Check if an input map is down
        /// </summary>
        public bool IsMapDown(InputMap inputMap)
        {
            return IsMapDown(inputMap, _keyboardState, _mouseState);
        }

        /// <summary>
        /// Check if an input map is up
        /// </summary>
        public bool IsMapUp(InputMap inputMap)
        {
            return !IsMapDown(inputMap, _keyboardState, _mouseState);
        }

        /// <summary>
        /// Check if an input map is newly down
        /// </summary>
        public bool IsMapNewDown(InputMap inputMap)
        {
            return IsMapDown(inputMap, _keyboardState, _mouseState) && !IsMapDown(inputMap, _lastKeyboardState, _lastMouseState);
        }

        /// <summary>
        /// Check if an input map is newly up
        /// </summary>
        public bool IsMapNewUp(InputMap inputMap)
        {
            return !IsMapDown(inputMap, _keyboardState, _mouseState) && IsMapDown(inputMap, _lastKeyboardState, _lastMouseState);
        }

        /// <summary>
        /// Checks if input map was just pressed.
        /// </summary>
        public bool IsMapPressed(InputMap inputMap, bool consume = true)
        {
            if (IsMapDown(inputMap, _keyboardState, _mouseState) && !IsMapDown(inputMap, _lastKeyboardState, _lastMouseState)
                && !IsMapConsumed(inputMap))
            {
                if (consume)
                    ConsumeMapPressed(inputMap);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Remove the map pressed from the state, we processed it
        /// The goal is to prevent multiple gamepobject to process the MapPressed
        /// </summary>
        public void ConsumeMapPressed(InputMap inputMap)
        {
            if (inputMap.Key != Keys.None)
                _consumedKeyPressed.Add(inputMap.Key);
            if (inputMap.MouseButton != MouseButton.None)
                _consumedMouseButton.Add(inputMap.MouseButton);
        }

        /// <summary>
        /// Check if an input map is down
        /// </summary>
        private bool IsMapDown(InputMap inputMap, KeyboardState keyboardState, MouseState mouseState)
        {
            if (inputMap.Key != Keys.None)
                return keyboardState.IsKeyDown(inputMap.Key);

            switch (inputMap.MouseButton)
            {
                case MouseButton.Left:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                case MouseButton.XButton1:
                    return mouseState.XButton1 == ButtonState.Pressed;
                case MouseButton.XButton2:
                    return mouseState.XButton2 == ButtonState.Pressed;
            }

            return false;
        }

        /// <summary>
        /// Check if an input map consumed?
        /// </summary>
        private bool IsMapConsumed(InputMap inputMap)
        {
            if (inputMap.Key != Keys.None)
                return _consumedKeyPressed.Contains(inputMap.Key);

            if (inputMap.MouseButton != MouseButton.None)
                return _consumedMouseButton.Contains(inputMap.MouseButton);

            return false;
        }
    }
}
