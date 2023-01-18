using FNAEngine2D.Renderers;
using FNAEngine2D.Desginer;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace FNAEngine2D.GameObjects
{
    /// <summary>
    /// Text render
    /// </summary>
    public class Label : GameObject
    {
        /// <summary>
        /// Text renderer
        /// </summary>
        private TextRenderer _textRenderer;

        /// <summary>
        /// Rotation
        /// </summary>
        [Category("Text")]
        [BrowsableAttribute(true)]
        [EditorAttribute(typeof(AngleEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue(0f)]
        public float Rotation { get { return _textRenderer.Rotation; } set { _textRenderer.Rotation = value; } }

        /// <summary>
        /// Rotation
        /// </summary>
        [Category("Text")]
        public Vector2 RotationOrigin { get { return _textRenderer.RotationOrigin; } set { _textRenderer.RotationOrigin = value; } }


        /// <summary>
        /// Nom de la font
        /// </summary>
        [Category("Text")]
        [DefaultValue(ContentManager.FONT_ROBOTO_REGULAR)]
        public string FontName { get { return _textRenderer.FontName; } set { _textRenderer.FontName = value; } }

        /// <summary>
        /// Font size
        /// </summary>
        [Category("Text")]
        [DefaultValue(12)]
        public int FontSize { get { return _textRenderer.FontSize; } set { _textRenderer.FontSize = value; } }

        /// <summary>
        /// Alignment horizontal
        /// </summary>
        [Category("Text")]
        [DefaultValue(TextHorizontalAlignment.Left)]
        public TextHorizontalAlignment HorizontalAlignment { get { return _textRenderer.HorizontalAlignment; } set { _textRenderer.HorizontalAlignment = value; } }

        /// <summary>
        /// Alignment vertical
        /// </summary>
        [Category("Text")]
        [DefaultValue(TextVerticalAlignment.Top)]
        public TextVerticalAlignment VerticalAlignment { get { return _textRenderer.VerticalAlignment; } set { _textRenderer.VerticalAlignment = value; } }

        /// <summary>
        /// Couleur
        /// </summary>
        [Category("Text")]
        public Color Color { get { return _textRenderer.Color; } set { _textRenderer.Color = value; } }


        /// <summary>
        /// Texte à afficher
        /// </summary>
        [Category("Text")]
        [DefaultValue("Text")]
        public string Text { get { return _textRenderer.Text; } set { _textRenderer.Text = value; } }

        /// <summary>
        /// Indicate if the location of the text should be rounded so the text is clearer at low resolution
        /// But if set to true and the text is moving, text will not move smoothly
        /// </summary>
        [Category("Layout")]
        [BrowsableAttribute(true)]
        [DefaultValue(false)]
        public bool PixelPerfect { get { return _textRenderer.PixelPerfect; } set { _textRenderer.PixelPerfect = value; } }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Label()
        {
            _textRenderer = new TextRenderer();
        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public Label(string text, string fontName, int fontSize, Vector2 location, Color color)
        {
            _textRenderer = new TextRenderer(text, fontName, fontSize, color);

            //_fontName = fontName;
            //_fontSize = fontSize;

            this.Location = location;
            //this.Color = color;
            //this.Text = text;

        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public Label(string text, string fontName, int fontSize, Rectangle bounds, Color color, TextHorizontalAlignment horizontalAlignment, TextVerticalAlignment verticalAlignment)
        {
            _textRenderer = new TextRenderer(text, fontName, fontSize, color, horizontalAlignment, verticalAlignment);

            //_fontName = fontName;
            //_fontSize = fontSize;

            this.Bounds = bounds;
            //this.Color = color;
            //this.Text = text;

            //HorizontalAlignment = horizontalAlignment;
            //VerticalAlignment = verticalAlignment;

        }

        /// <summary>
        /// Renderer de texture
        /// </summary>
        public Label(string text, string fontName, int fontSize, Rectangle bounds, Color color, TextHorizontalAlignment horizontalAlignment, TextVerticalAlignment verticalAlignment, bool pixelPerfect): this(text, fontName, fontSize, bounds, color, horizontalAlignment, verticalAlignment)
        {
            _textRenderer.PixelPerfect = pixelPerfect;
        }

        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            AddComponent(_textRenderer);
        }


    }
}
