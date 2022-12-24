using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        ///// <summary>
        ///// Size of the camera
        ///// </summary>
        //private Vector2? _size;

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
        /// </summary>
        public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

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
                }
            }
        }

        ///// <summary>
        ///// Size
        ///// </summary>
        //public Vector2? Size
        //{
        //    get { return _size; }
        //    set
        //    {
        //        if (_size != value)
        //        {
        //            _size = value;
        //            _updated = true;
        //        }
        //    }
        //}


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
        /// Constructor
        /// </summary>
        public Camera()
        {
            
        }

        /// <summary>
        /// Begin drawing
        /// </summary>
        public virtual void BeginDraw()
        {
            if(_spriteBatch == null)
                _spriteBatch = new SpriteBatch(GameHost.InternalGame.GraphicsDevice);

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
                //Matrix matrix = Matrix.CreateTranslation(new Vector3(-(_location.X - (GameHost.Width / 2)), -(_location.Y - (GameHost.Height / 2)), 0));
                Matrix matrix = Matrix.CreateTranslation(new Vector3(-_location, 0));


                if (_rotation != 0)
                {
                    matrix *= Matrix.CreateRotationZ(_rotation);
                    matrix *= Matrix.CreateTranslation(new Vector3(GameHost.Width / 2, GameHost.Height / 2, 0));
                }

                if (_zoom != 1)
                    matrix *= Matrix.CreateScale(new Vector3(_zoom, _zoom, 1));

                

                //Now combine the camera's matrix with the Resolution Manager's transform matrix to get our final working matrix:
                matrix *= GameHost.InternalGame.ScaleMatrix;

                //Round the X and Y translation so the camera doesn't jerk as it moves:
                matrix.M41 = (float)Math.Round(matrix.M41, 0);
                matrix.M42 = (float)Math.Round(matrix.M42, 0);

                _transformMatrix = matrix;
                _updated = false;

            }

            return _transformMatrix;

        }

    }
}
