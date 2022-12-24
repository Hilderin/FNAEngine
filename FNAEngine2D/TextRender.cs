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
        private string _fontName = String.Empty;

        /// <summary>
        /// Size de la font
        /// </summary>
        private int _fontSize = 12;

        /// <summary>
        /// Text
        /// </summary>
        private string _text = String.Empty;

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
        [DefaultValue("")]
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
        [DefaultValue("")]
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
        /// Chargement du contenu
        /// </summary>
        public override void Load()
        {
            if (String.IsNullOrEmpty(_fontName))
                _font = null;
            else
                _font = new Font(_fontName, FontSize);

            //Update the font live so the ContentDesigner will display updated values without the need for updates
            Update();
        }

        /// <summary>
        /// Override de l'update
        /// </summary>
        public override void Update()
        {
            if (_textUpdated)
            {
                if (_font == null || String.IsNullOrEmpty(this.Text))
                    _textCache = null;
                else
                    _textCache = _font.MakeText(this.Text);
                _textUpdated = false;
            }
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            if (_font == null || _textCache == null)
                return;

            int x;
            int y;

            //Calculate horizontal position...
            switch (HorizontalAlignment)
            {
                case TextHorizontalAlignment.Right:
                    //I convert to int because if it's not rounded, the letters will be truncated and missing pixels
                    x = (int)(this.X + this.Width - _textCache.Width);
                    break;
                case TextHorizontalAlignment.Center:
                    //I convert to int because if it's not rounded, the letters will be truncated and missing pixels
                    x = (int)(this.X + (this.Width / 2) - (_textCache.Width / 2));
                    break;
                default:
                    x = (int)this.X;
                    break;
            }

            //Calculate vertical position...
            switch (VerticalAlignment)
            {
                case TextVerticalAlignment.Bottom:
                    //I convert to int because if it's not rounded, the letters will be truncated and missing pixels
                    y = (int)(this.Y + this.Height - _textCache.Height);
                    break;
                case TextVerticalAlignment.Middle:
                    //I convert to int because if it's not rounded, the letters will be truncated and missing pixels
                    //The _textCache.Height is just wrong...
                    //I padded with a +8 we will see later if it's good for every fonts
                    y = (int)(this.Y + (this.Height / 2) - ((FontSize + 8) / 2));
                    break;
                default:
                    y = (int)this.Y;
                    break;
            }

             DrawingContext.DrawString(_textCache, new Vector2(x, y), this.Color, this.Rotation, this.RotationOrigin, Vector2.One, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, this.Depth);
        }

    }
}
