﻿using Microsoft.Xna.Framework.Input;
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

        /// <summary>
        /// Indique si la souris est visible
        /// </summary>
        public static bool IsMouseVisible { get; private set; }

        /// <summary>
        /// Call at each Update
        /// </summary>
        public static void Update()
        {
            _lastKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();
        }

        /// <summary>
        /// Checks if key is currently pressed.
        /// </summary>
        public static bool IsKeyDown(Keys input)
        {
            return _keyboardState.IsKeyDown(input);
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
        public static bool IsKeyPressed(Keys input)
        {
            if (_keyboardState.IsKeyDown(input) == true && _lastKeyboardState.IsKeyDown(input) == false)
                return true;
            else
                return false;
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
            if (_mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
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
        /// Display the mouse cursor
        /// </summary>
        public static void ShowMouse()
        {
            Input.IsMouseVisible = true;

            //Si on a un gamehost, on va le setter...
            if (GameHost.InternalGameHost != null && GameHost.InternalGameHost.IsInitialized)
                GameHost.InternalGameHost.IsMouseVisible = true;
        }


        /// <summary>
        /// Hide the mouse cursor
        /// </summary>
        public static void HideMouse()
        {
            Input.IsMouseVisible = false;

            //Si on a un gamehost, on va le setter...
            if (GameHost.InternalGameHost != null && GameHost.InternalGameHost.IsInitialized)
                GameHost.InternalGameHost.IsMouseVisible = false;
        }


        ///// <summary>
        ///// Gets mouse coordinates adjusted for virtual resolution and camera position.
        ///// </summary>
        //public static Vector2 MousePositionCamera()
        //{
        //    Vector2 mousePosition = Vector2.Zero;
        //    mousePosition.X = _mouseState.X;
        //    mousePosition.Y = _mouseState.Y;

        //    return ScreenToWorld(mousePosition);
        //}

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
