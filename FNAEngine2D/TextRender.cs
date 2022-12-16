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
            GameHost.SpriteBatch.DrawString(_textCache, this.Position, this.Color, this.Rotation, this.RotationOrigin, Vector2.One, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
            //_font.DrawString(this.Text, this.Position, this.Color, this.Rotation, this.RotationOrigin);
        }

    }
}
