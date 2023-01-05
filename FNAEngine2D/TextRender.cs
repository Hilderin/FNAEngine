using FNAEngine2D.Desginer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Velentr.Font;
using static System.Net.Mime.MediaTypeNames;

namespace FNAEngine2D
{
    /// <summary>
    /// Alignment horizontal
    /// </summary>
    public enum TextHorizontalAlignment
    {
        Left,
        Right,
        Center
    }

    /// <summary>
    /// Alignment vertical
    /// </summary>
    public enum TextVerticalAlignment
    {
        Top,
        Bottom,
        Middle
    }


    /// <summary>
    /// Text render
    /// </summary>
    public class TextRender : GameObject
    {
        /// <summary>
        /// TextUpdated
        /// </summary>
        private bool _textUpdated = true;


        /// <summary>
        /// Text en cache
        /// </summary>
        private Velentr.Font.Text _textCache;


        /// <summary>
        /// Texture à renderer
        /// </summary>
        private Font _font;

        /// <summary>
        /// Nom de la font
        /// </summary>
        private string _fontName = FontManager.ROBOTO_REGULAR;

        /// <summary>
        /// Size de la font
        /// </summary>
        private int _fontSize = 12;

        /// <summary>
        /// Text
        /// </summary>
        private string _text = "Text";

        /// <summary>
        /// Real text location
        /// </summary>
        private Vector2 _textLocation = Vector2.Zero;

        /// <summary>
        /// Indicate if the location of the text should be rounded so the text is clearer at low resolution
        /// But if set to true and the text is moving, text will not move smoothly
        /// </summary>
        private bool _pixelPerfect = false;

        /// <summary>
        /// Rotation
        /// </summary>
        [Category("Text")]
        [BrowsableAttribute(true)]
        [EditorAttribute(typeof(AngleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(0f)]
        public float Rotation { get; set; } = 0f;

        /// <summary>
        /// Rotation
        /// </summary>
        [Category("Text")]
        public Vector2 RotationOrigin { get; set; } = Vector2.Zero;


        /// <summary>
        /// Nom de la font
        /// </summary>
        [Category("Text")]
        [DefaultValue(FontManager.ROBOTO_REGULAR)]
        public string FontName
        {
            get { return _fontName; }
            set
            {
                if (_fontName != value)
                {
                    _fontName = value;
                    _textUpdated = true;
                }
            }
        }

        /// <summary>
        /// Font size
        /// </summary>
        [Category("Text")]
        [DefaultValue(12)]
        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    _textUpdated = true;
                }
            }
        }

        /// <summary>
        /// Alignment horizontal
        /// </summary>
        [Category("Text")]
        [DefaultValue(TextHorizontalAlignment.Left)]
        public TextHorizontalAlignment HorizontalAlignment { get; set; } = TextHorizontalAlignment.Left;

        /// <summary>
        /// Alignment vertical
        /// </summary>
        [Category("Text")]
        [DefaultValue(TextVerticalAlignment.Top)]
        public TextVerticalAlignment VerticalAlignment { get; set; } = TextVerticalAlignment.Top;

        /// <summary>
        /// Couleur
        /// </summary>
        [Category("Text")]
        public Color Color { get; set; } = Color.Black;


        /// <summary>
        /// Texte à afficher
        /// </summary>
        [Category("Text")]
        [DefaultValue("Text")]
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    _textUpdated = true;
                }
            }
        }

        /// <summary>
        /// Indicate if the location of the text should be rounded so the text is clearer at low resolution
        /// But if set to true and the text is moving, text will not move smoothly
        /// </summary>
        [Category("Layout")]
        [BrowsableAttribute(true)]
        [DefaultValue(false)]
        public bool PixelPerfect
        {
            get { return _pixelPerfect; }
            set
            {
                if (_pixelPerfect != value)
                {
                    _pixelPerfect = value;
                    _textUpdated = true;
                }
            }
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TextRender()
        {
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextRender(string text, string fontName, int fontSize, Vector2 location, Color color)
        {
            _fontName = fontName;
            _fontSize = fontSize;

            this.Location = location;
            this.Color = color;
            this.Text = text;

        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextRender(string text, string fontName, int fontSize, Rectangle bounds, Color color, TextHorizontalAlignment horizontalAlignment, TextVerticalAlignment verticalAlignment)
        {
            _fontName = fontName;
            _fontSize = fontSize;

            this.Bounds = bounds;
            this.Color = color;
            this.Text = text;

            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;

        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextRender(string text, string fontName, int fontSize, Rectangle bounds, Color color, TextHorizontalAlignment horizontalAlignment, TextVerticalAlignment verticalAlignment, bool pixelPerfect): this(text, fontName, fontSize, bounds, color, horizontalAlignment, verticalAlignment)
        {
            PixelPerfect = true;
        }

        /// <summary>
        /// Chargement du contenu
        /// </summary>
        protected override void Load()
        {
            if (String.IsNullOrEmpty(_fontName))
                _font = null;
            else
                _font = new Font(_fontName, FontSize);

            //Update the font live so the ContentDesigner will display updated values without the need for updates
            Update();
        }

        /// <summary>
        /// Text moved
        /// </summary>
        protected override void OnMoved()
        {
            RecalculteTextLocation();
        }

        /// <summary>
        /// Text resized
        /// </summary>
        protected override void OnResized()
        {
            RecalculteTextLocation();
        }

        /// <summary>
        /// Override de l'update
        /// </summary>
        protected override void Update()
        {
            if (_textUpdated)
            {
                if (_font == null || String.IsNullOrEmpty(this.Text))
                {
                    _textCache = null;

                    if(this.HorizontalAlignment == TextHorizontalAlignment.Left)
                        this.Width = 0;
                    if (this.VerticalAlignment == TextVerticalAlignment.Top)
                        this.Height = 0;
                }
                else
                {
                    _textCache = _font.MakeText(this.Text);
                    if (this.HorizontalAlignment == TextHorizontalAlignment.Left && this.Width == 0)
                        this.Width = _textCache.Width;
                    if (this.VerticalAlignment == TextVerticalAlignment.Top && this.Height == 0)
                        this.Height = _textCache.Height;
                }

                RecalculteTextLocation();

                _textUpdated = false;
            }
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        protected override void Draw()
        {
            if (_font == null || _textCache == null)
                return;

            DrawingContext.DrawString(_textCache, _textLocation, this.Color, this.Rotation, this.RotationOrigin, Vector2.One, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, this.Depth);
        }

        /// <summary>
        /// Recalculate the text location
        /// </summary>
        private void RecalculteTextLocation()
        {
            if (_font == null || _textCache == null)
                return;

            float x;
            float y;

            //Calculate horizontal position...
            switch (HorizontalAlignment)
            {
                case TextHorizontalAlignment.Right:
                    x = this.X + this.Width - _textCache.Width;
                    break;
                case TextHorizontalAlignment.Center:
                    x = this.X + (this.Width / 2) - (_textCache.Width / 2);
                    break;
                default:
                    x = this.X;
                    break;
            }

            //Calculate vertical position...
            switch (VerticalAlignment)
            {
                case TextVerticalAlignment.Bottom:
                    y = this.Y + this.Height - _textCache.Height;
                    break;
                case TextVerticalAlignment.Middle:
                    //I convert to int because if it's not rounded, the letters will be truncated and missing pixels
                    //The _textCache.Height is just wrong...
                    //I padded with a +8 we will see later if it's good for every fonts
                    y = this.Y + (this.Height / 2) - ((FontSize + 8) / 2);
                    break;
                default:
                    y = this.Y;
                    break;
            }

            if (PixelPerfect)
            {
                x = (int)x;
                y = (int)y;
            }

            _textLocation = new Vector2(x, y);
        }

    }
}
