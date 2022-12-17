using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        /// Text
        /// </summary>
        private string _text;

        /// <summary>
        /// Nom de la font
        /// </summary>
        private string _fileName;

        /// <summary>
        /// Font size
        /// </summary>
        private int _fontSize;

        /// <summary>
        /// Texture à renderer
        /// </summary>
        private Font _font;

        /// <summary>
        /// Alignment horizontal
        /// </summary>
        private TextHorizontalAlignment _horizontalAlignment = TextHorizontalAlignment.Left;

        /// <summary>
        /// Alignment vertical
        /// </summary>
        private TextVerticalAlignment _verticalAlignment = TextVerticalAlignment.Top;

        /// <summary>
        /// Couleur
        /// </summary>
        public Color Color;


        /// <summary>
        /// Texte à afficher
        /// </summary>
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
        /// Alignment horizontal
        /// </summary>
        public TextHorizontalAlignment HorizontalAlignment
        {
            get { return _horizontalAlignment; }
            set { _horizontalAlignment = value; }
        }

        /// <summary>
        /// Alignment vertical
        /// </summary>
        public TextVerticalAlignment VerticalAlignment
        {
            get { return _verticalAlignment; }
            set { _verticalAlignment = value; }
        }


        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextRender(string text, string fileName, int fontSize, Vector2 location, Color color)
        {
            _fileName = fileName;
            _fontSize = fontSize;

            this.Location = location;
            this.Color = color;
            this.Text = text;

        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public TextRender(string text, string fileName, int fontSize, Rectangle bounds, Color color, TextHorizontalAlignment horizontalAlignment, TextVerticalAlignment verticalAlignment)
        {
            _fileName = fileName;
            _fontSize = fontSize;

            this.Bounds = bounds;
            this.Color = color;
            this.Text = text;

            _horizontalAlignment = horizontalAlignment;
            _verticalAlignment = verticalAlignment;

        }

        /// <summary>
        /// Chargement du contenu
        /// </summary>
        public override void Load()
        {
            _font = new Font(_fileName, _fontSize);
        }

        /// <summary>
        /// Override de l'update
        /// </summary>
        public override void Update()
        {
            if (_textUpdated)
            {
                _textCache = _font.MakeText(this.Text);
                _textUpdated = false;
            }
        }

        /// <summary>
        /// Permet de dessiner l'objet
        /// </summary>
        public override void Draw()
        {
            int x;
            int y;

            //Calculate horizontal position...
            switch (_horizontalAlignment)
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
                    x = (int)this.Position.X;
                    break;
            }

            //Calculate vertical position...
            switch (_verticalAlignment)
            {
                case TextVerticalAlignment.Bottom:
                    //I convert to int because if it's not rounded, the letters will be truncated and missing pixels
                    y = (int)(this.Y + this.Height - _textCache.Height);
                    break;
                case TextVerticalAlignment.Middle:
                    //I convert to int because if it's not rounded, the letters will be truncated and missing pixels
                    //The _textCache.Height is just wrong...
                    //I padded with a +8 we will see later if it's good for every fonts
                    y = (int)(this.Y + (this.Height / 2) - ((_fontSize + 8) / 2));
                    break;
                default:
                    y = (int)this.Position.Y;
                    break;
            }

            GameHost.SpriteBatch.DrawString(_textCache, new Vector2(x, y), this.Color, this.Rotation, this.RotationOrigin, Vector2.One, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
            //_font.DrawString(this.Text, this.Position, this.Color, this.Rotation, this.RotationOrigin);
        }

    }
}
