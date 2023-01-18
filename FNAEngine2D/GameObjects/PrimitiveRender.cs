using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont.Cache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.GameObjects
{
    /// <summary>
    /// Types of primitives
    /// </summary>
    public enum PrimitiveType
    {
        Rectangle,
        RectangleFill,
        CircleFill
    }

    /// <summary>
    /// Render for primitives
    /// </summary>
    public class PrimitiveRender : GameObject, IUpdate, IDraw
    {
        /// <summary>
        /// Texture to renderer
        /// </summary>
        private Content<Texture2D> _texture;

        /// <summary>
        /// Type of primitive to render
        /// </summary>
        private PrimitiveType _primitiveType;

        /// <summary>
        /// Updated ?
        /// </summary>
        private bool _updated = true;

        /// <summary>
        /// Scale
        /// </summary>
        private Vector2 _scale;

        /// <summary>
        /// Scales for each sides of the rectangle
        /// </summary>
        private Vector2 _scaleTopBottom;
        private Vector2 _scaleRightLeft;

        /// <summary>
        /// PrimitiveType
        /// </summary>
        [Category("Layout")]
        public PrimitiveType PrimitiveType
        {
            get { return _primitiveType; }
            set
            {
                if (_primitiveType != value)
                {
                    _primitiveType = value;
                    _updated = true;
                }
            }
        }
        /// <summary>
        /// Color
        /// </summary>
        [Category("Layout")]
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public PrimitiveRender()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrimitiveRender(PrimitiveType primitiveType) : this()
        {
            _primitiveType = primitiveType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrimitiveRender(PrimitiveType primitiveType, Rectangle bounds) : this(primitiveType)
        {
            this.Bounds = bounds;
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public PrimitiveRender(PrimitiveType primitiveType, Rectangle bounds, Color color) : this(primitiveType, bounds)
        {
            this.Color = color;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrimitiveRender(PrimitiveType primitiveType, Vector2 location, Vector2 size, Color color) : this(primitiveType)
        {
            this.Color = color;
            this.Location = location;
            this.Size = size;
        }

        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {

        }

        /// <summary>
        /// On resize...
        /// </summary>
        protected override void OnResized()
        {
            if (_texture != null)
                RecalculateScale();
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update()
        {
            if (_updated)
            {
                //Loading the right texture for the job...
                if (_primitiveType == PrimitiveType.CircleFill)
                {
                    if (_texture == null || _texture.AssetName != ContentManager.TEXTURE_CIRCLE)
                        _texture = GetContent<Texture2D>(ContentManager.TEXTURE_CIRCLE);
                }
                else
                {
                    //Simple pixel will do
                    if (_texture == null || _texture.AssetName != ContentManager.TEXTURE_PIXEL)
                        _texture = GetContent<Texture2D>(ContentManager.TEXTURE_PIXEL);
                }


                RecalculateScale();


                _updated = false;
            }
        }


        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public void Draw()
        {
            if (_texture == null)
                return;


            if (_primitiveType == PrimitiveType.Rectangle)
            {
                //4 renders to draw the 4 sides...

                //top
                DrawingContext.Draw(_texture.Data, this.Location, null, this.Color, 0f, Vector2.Zero, _scaleTopBottom, SpriteEffects.None, this.Depth);
                //bottom
                DrawingContext.Draw(_texture.Data, this.Location.AddY(this.Height - 1), null, this.Color, 0f, Vector2.Zero, _scaleTopBottom, SpriteEffects.None, this.Depth);
                //left
                DrawingContext.Draw(_texture.Data, this.Location, null, this.Color, 0f, Vector2.Zero, _scaleRightLeft, SpriteEffects.None, this.Depth);
                //left
                DrawingContext.Draw(_texture.Data, this.Location.AddX(this.Width - 1), null, this.Color, 0f, Vector2.Zero, _scaleRightLeft, SpriteEffects.None, this.Depth);
            }
            else
            {
                //Fill, we just have to render the texture...
                DrawingContext.Draw(_texture.Data, this.Location, null, this.Color, 0f, Vector2.Zero, _scale, SpriteEffects.None, this.Depth);
            }
        }

        /// <summary>
        /// Recalculate the scale of the texture
        /// </summary>
        private void RecalculateScale()
        {
            if (_primitiveType == PrimitiveType.Rectangle)
            {
                //We need to calculate the scales for each side for the rectangle...
                //The texture is 1x1
                _scaleTopBottom = new Vector2(this.Width, 1);
                _scaleRightLeft = new Vector2(1, this.Height);
            }
            else
            {
                _scale = new Vector2(this.Width / _texture.Data.Width, this.Height / _texture.Data.Height);
            }
        }
    }
}
