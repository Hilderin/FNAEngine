﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Velentr.Font;

namespace FNAEngine2D
{
    /// <summary>
    /// Context on the current drawing
    /// </summary>
    internal static class DrawingContext
    {
        private const int INCREMENT_BUFFER = 1024;
        private static DrawInfoComparer _drawInfoComparer = new DrawInfoComparer();


        private static int _bufferLength = 0;
        private static DrawInfo[] _drawings = new DrawInfo[0];

        private static int _drawIndex = -1;


        /// <summary>
        /// Current camera
        /// </summary>
        private static Camera _camera;


        /// <summary>
        /// Current camera
        /// </summary>
        public static Camera Camera  { get { return _camera; } }
        

        /// <summary>
        /// Constructor
        /// </summary>
        static DrawingContext()
        {
            ResizeDrawingsArray();
        }

        /// <summary>
        /// Begin drawing
        /// </summary>
        public static void Begin(Camera camera)
        {
            _drawIndex = -1;
            _camera = camera;
        }

        /// <summary>
        /// Draw a texture
        /// </summary>
        public static void Draw(Texture2D texture,
                         Rectangle destinationRectangle,
                         Rectangle? sourceRectangle,
                         Color color,
                         float rotation,
                         Vector2 origin,
                         SpriteEffects effects,
                         float depth)
        {
            _drawIndex++;
            if (_drawIndex == _bufferLength)
            {
                ResizeDrawingsArray();
            }

            _drawings[_drawIndex].drawType = DrawType.TextureDestinationRectangle;
            _drawings[_drawIndex].drawIndex = _drawIndex;
            _drawings[_drawIndex].texture = texture;
            _drawings[_drawIndex].destinationRectangle = destinationRectangle;
            _drawings[_drawIndex].sourceRectangle = sourceRectangle;
            _drawings[_drawIndex].color = color;
            _drawings[_drawIndex].rotation = rotation;
            _drawings[_drawIndex].origin = origin;
            _drawings[_drawIndex].effects = effects;
            _drawings[_drawIndex].depth = depth;

        }

        /// <summary>
        /// Draw a texture
        /// </summary>
        public static void Draw(Texture2D texture,
                         Vector2 position,
                         Rectangle? sourceRectangle,
                         Color color,
                         float rotation,
                         Vector2 origin,
                         Vector2 scale,
                         SpriteEffects effects,
                         float depth)
        {
            _drawIndex++;
            if (_drawIndex == _bufferLength)
            {
                ResizeDrawingsArray();
            }

            _drawings[_drawIndex].drawType = DrawType.TextureDestinationRectangle;
            _drawings[_drawIndex].drawIndex = _drawIndex;
            _drawings[_drawIndex].texture = texture;
			_drawings[_drawIndex].position = position;
            _drawings[_drawIndex].sourceRectangle = sourceRectangle;
			_drawings[_drawIndex].color = color;
            _drawings[_drawIndex].rotation = rotation;
            _drawings[_drawIndex].origin = origin;
			_drawings[_drawIndex].scale = scale;
            _drawings[_drawIndex].effects = effects;
            _drawings[_drawIndex].depth = depth;

        }

        /// <summary>
        /// DrawString
        /// </summary>
        public static void DrawString(Text text, 
                               Vector2 position, 
                               Color color, 
                               float rotation, 
                               Vector2 origin, 
                               Vector2 scale, 
                               SpriteEffects effects, 
                               float depth)
        {
            _drawIndex++;
            if (_drawIndex == _bufferLength)
            {
                ResizeDrawingsArray();
            }

            _drawings[_drawIndex].drawIndex = _drawIndex;
            _drawings[_drawIndex].text = text;
            _drawings[_drawIndex].color = color;
            _drawings[_drawIndex].position = position;
            _drawings[_drawIndex].scale = scale;
            _drawings[_drawIndex].effects = effects;
            _drawings[_drawIndex].depth = depth;


            //There's a lot more calculation in the full overload of Velentr.Font.DrawString, so, if we can avoid it...
            if (rotation == 0 && origin == Vector2.Zero && scale == Vector2.One && effects == SpriteEffects.None)
            {
                _drawings[_drawIndex].drawType = DrawType.TextNoRotationNoEffect;
            }
            else
            {
                _drawings[_drawIndex].drawType = DrawType.Text;
                _drawings[_drawIndex].rotation = rotation;
                _drawings[_drawIndex].origin = origin;
            }

        }

        /// <summary>
        /// End the drawing
        /// </summary>
        public static void End()
        {
            Array.Sort(_drawings, 0, _drawIndex + 1, _drawInfoComparer);

            _camera.BeginDraw();
            SpriteBatch spriteBatch = _camera.SpriteBatch;

            for (int index = 0; index <= _drawIndex; index++)
            {
                DrawInfo di = _drawings[index];

                switch (di.drawType)
                {
                    case DrawType.TextureVector2:
                        spriteBatch.Draw(di.texture, di.position, di.sourceRectangle, di.color, di.rotation, di.origin, di.scale, di.effects, 0f);
                        break;
                    case DrawType.TextureDestinationRectangle:
                        spriteBatch.Draw(di.texture, di.destinationRectangle, di.sourceRectangle, di.color, di.rotation, di.origin, di.effects, 0f);
                        break;
                    case DrawType.Text:
                        spriteBatch.DrawString(di.text, di.position, di.color, di.rotation, di.origin, di.scale, di.effects, 0f);
                        break;
                    case DrawType.TextNoRotationNoEffect:
                        spriteBatch.DrawString(di.text, di.position, di.color);
                        break;
                }
            }

            _camera.EndDraw();
        }

        /// <summary>
        /// Grow the drawings array
        /// </summary>
        private static void ResizeDrawingsArray()
        {
            int newSize = _bufferLength + INCREMENT_BUFFER;

            Array.Resize(ref _drawings, newSize);

            for (int index = _bufferLength; index < newSize; index++)
                _drawings[index] = new DrawInfo();

            _bufferLength = newSize;


        }

        /// <summary>
        /// Type of drawing
        /// </summary>
        private enum DrawType
        {
            TextureVector2,
            TextureDestinationRectangle,
            Text,
            TextNoRotationNoEffect
        }

        /// <summary>
        /// Information on drawing
        /// </summary>
        private class DrawInfo
        {
            public DrawType drawType;
            public int drawIndex;
            public Texture2D texture;
            public Text text;
            public Vector2 position;
            public Rectangle destinationRectangle;
            public Rectangle? sourceRectangle;
            public Color color;
            public float rotation;
            public Vector2 origin;
            public Vector2 scale;
            public SpriteEffects effects;
            public float depth;

        }

        /// <summary>
        /// Comparer to sort the draw info
        /// </summary>
        private class DrawInfoComparer : IComparer<DrawInfo>
        {
            public unsafe int Compare(DrawInfo i1, DrawInfo i2)
            {
                int compare = i1.depth.CompareTo(i2.depth);
                if (compare != 0)
                    //We want that higher depth be at the beginning of the array
                    return -compare;
                return i1.drawIndex.CompareTo(i2.drawIndex);
            }
        }

    }
}
