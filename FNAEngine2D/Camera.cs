using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Camera
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// A transformation matrix containing info on our position, how much we are rotated and zoomed etc.
        /// </summary>
        private Matrix _transformMatrix;

        /// <summary>
        /// Position of the camera
        /// </summary>
        private Vector2 _location;

        /// <summary>
        /// Size of the camera
        /// </summary>
        private Point _size;

        /// <summary>
        /// Zoom level (1 = no zoom)
        /// </summary>
        private float _zoom = 1f;

        /// <summary>
        /// Rotation of the screen (in radians)
        /// </summary>
        private float _rotation = 0f;

        /// <summary>
        /// Updated
        /// </summary>
        private bool _updated = true;

        /// <summary>
        /// SpriteBatch of the camera
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// ViewPort
        /// </summary>
        private Viewport _viewPort;

        /// <summary>
        /// Position of the view on the screen
        /// </summary>
        private Point _viewLocation = Point.Zero;


        /// <summary>
        /// Bounds of the displayed area
        /// </summary>
        private Rectangle _displayBounds;

        /// <summary>
        /// Game
        /// </summary>
        private Game _game;


        /// <summary>
        /// Layer of the game object
        /// </summary>
        public Layers LayerMask { get; set; } = Layers.All;

        ///// <summary>
        ///// SpriteSortMode
        ///// FrontToBack has weird problem with the text in FrontToBack mode, probablement because of the resolution and the font?
        ///// You should only use Immediate sort mode if you need to change state between every Draw call. 
        ///// Most of the places you used to specify Immediate sort mode, Deferred is now a better choice.
        ///// </summary>
        //public SpriteSortMode SpriteSortMode { get; set; } = SpriteSortMode.Immediate;

        /// <summary>
        /// Blend state of textures
        /// NonPremultiplied by default because we dont premultiply in our ContentManager.
        /// </summary>
        public BlendState BlendState { get; set; } = BlendState.NonPremultiplied;

        /// <summary>
        /// PointWrap is perfect for PixelArts. Use LinearWrap for better texture smoothing with scaling
        /// </summary>
        public SamplerState SamplerState { get; set; } = SamplerState.PointWrap;

        /// <summary>
        /// DepthStencilState
        /// </summary>
        public DepthStencilState DepthStencilState { get; set; } = DepthStencilState.Default;

        /// <summary>
        /// RasterizerState
        /// </summary>
        public RasterizerState RasterizerState { get; set; } = RasterizerState.CullCounterClockwise;

        /// <summary>
        /// Effect
        /// </summary>
        public Effect Effect { get; set; } = null;

        /// <summary>
        /// Camera Index
        /// </summary>
        public int CameraIndex { get; private set; }

        // <summary>
        /// Game
        /// </summary>
        public Game Game
        {
            get { return _game; }
            set
            {
                if (_game != value)
                {
                    _game = value;
                    RecalculteViewPort();
                }
            }

        }

        /// <summary>
        /// Spritebatch of the camera
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get
            {
                return _spriteBatch;
            }
        }

        /// <summary>
        /// Location
        /// </summary>
        public Vector2 Location
        {
            get { return _location; }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    _updated = true;
                    RecalculteDisplayBounds();
                }
            }
        }

        /// <summary>
        /// Location on the screen
        /// </summary>
        public Point ViewLocation
        {
            get { return _viewLocation; }
            set
            {
                if (_viewLocation != value)
                {
                    _viewLocation = value;
                    RecalculteViewPort();
                }
            }
        }

        /// <summary>
        /// Size
        /// </summary>
        public Point Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    _updated = true;
                    RecalculteViewPort();
                    RecalculteDisplayBounds();
                }
            }
        }


        /// <summary>
        /// Zoom level (1 = no zoom)
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                if (_zoom != value)
                {
                    _zoom = value;
                    _updated = true;
                    RecalculteDisplayBounds();
                }
            }
        }

        /// <summary>
        /// Rotation of the screen (in radians)
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    _updated = true;
                }
            }
        }

        /// <summary>
        /// Bounds renderer to screen
        /// </summary>
        public Rectangle DisplayBounds
        {
            get { return _displayBounds; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Camera(int width, int height)
        {
            //Default size
            _size = new Point(width, height);
            RecalculteViewPort();
            RecalculteDisplayBounds();
        }

        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public Camera()
        //{
        //    //Default size
        //    _size = new Point(this.Game.Width, this.Game.Height);
        //    RecalculteViewPort();
        //    RecalculteDisplayBounds();
        //}

        /// <summary>
        /// Check if a rectangle is in the field of view of the camera
        /// </summary>
        public bool IsDisplayed(Vector2 position, float width, float height)
        {
            return (position.X <= _displayBounds.Right)
                    && (position.X + width >= _displayBounds.X)
                    && (position.Y <= _displayBounds.Bottom)
                    && (position.Y + height >= _displayBounds.Y);
        }

        /// <summary>
        /// Check if a rectangle is in the field of view of the camera
        /// </summary>
        public bool IsDisplayed(Rectangle rectangle)
        {
            return _displayBounds.Intersects(rectangle);
        }

        /// <summary>
        /// Begin drawing
        /// </summary>
        public virtual void BeginDraw()
        {
            GraphicsDevice graphicsDevice = _game.GraphicsDevice;

            //Set le ViewPort
            if (graphicsDevice.Viewport.X != _viewPort.X || graphicsDevice.Viewport.Y != _viewPort.Y || graphicsDevice.Viewport.Width != _viewPort.Width || graphicsDevice.Viewport.Height != _viewPort.Height)
                graphicsDevice.Viewport = _viewPort;

            //Creation of the spritebatch...
            if (_spriteBatch == null)
                _spriteBatch = new SpriteBatch(graphicsDevice);

            //FrontToBack has problem if we have multiple layerDepth, the Array.Sort will not always keep the order of drawing even on the same
            //layerDepth. So, i created a DrawingContext that sort correctly the draw calls and we can just keep using SpriteSortMode.Deferred
            //and the layerDepth will be used!
            // You should only use Immediate sort mode if you need to change state between every Draw call. 
            // Most of the places you used to specify Immediate sort mode, Deferred is now a better choice.
            _spriteBatch.Begin(SpriteSortMode.Deferred,
                                this.BlendState,
                                this.SamplerState,
                                this.DepthStencilState,
                                this.RasterizerState,
                                this.Effect,
                                GetMatrix());
        }

        /// <summary>
        /// End of drawing
        /// </summary>
        public virtual void EndDraw()
        {
            _spriteBatch.End();
        }

        /// <summary>
        /// Get the camera matrx
        /// </summary>
        public virtual Matrix GetMatrix()
        {
            if (_updated)
            {
                //The math involved with calculated our _transformMatrix and screenRect is a little intense, so instead of calling the math whenever we need these variables,
                //we'll calculate them once each frame and store them... when someone needs these variables we will simply return the stored variable instead of re cauclating them every time.

                //Calculate the camera transform matrix:
                //Matrix matrix = Matrix.CreateTranslation(new Vector3(-(_location.X - (this.Game.Width / 2)), -(_location.Y - (this.Game.Height / 2)), 0));
                Matrix matrix = Matrix.CreateTranslation(new Vector3(-_location, 0));


                if (_rotation != 0)
                {
                    matrix *= Matrix.CreateRotationZ(_rotation);
                    matrix *= Matrix.CreateTranslation(new Vector3(_game.Width / 2, _game.Height / 2, 0));
                }

                if (_zoom != 1)
                    matrix *= Matrix.CreateScale(new Vector3(_zoom, _zoom, 1));



                //Now combine the camera's matrix with the Resolution Manager's transform matrix to get our final working matrix:
                matrix *= _game.ScaleMatrix;

                //Round the X and Y translation so the camera doesn't jerk as it moves:
                matrix.M41 = (float)Math.Round(matrix.M41, 0);
                matrix.M42 = (float)Math.Round(matrix.M42, 0);

                _transformMatrix = matrix;
                _updated = false;

            }

            return _transformMatrix;

        }

        /// <summary>
        /// Recalculate the view port to use
        /// </summary>
        public void RecalculteViewPort()
        {
            Vector2 scale = Vector2.One;

            if (_game != null)
                scale = _game.ScreenScale;

            _viewPort = new Viewport((int)(_viewLocation.X * scale.X),
                                    (int)(_viewLocation.Y * scale.Y),
                                    (int)(_size.X * scale.X),
                                    (int)(_size.Y * scale.Y));
        }

        /// <summary>
        /// Recalculate the display bounds
        /// </summary>
        private void RecalculteDisplayBounds()
        {
            _displayBounds = new Rectangle((int)_location.X, (int)_location.Y, (int)Math.Ceiling(_size.X / _zoom), (int)Math.Ceiling(_size.Y / _zoom));
        }

    }
}
