using FNAEngine2D.Desginer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Tileset
    /// </summary>
    public class TileSetRender: GameObject
    {
        /// <summary>
        /// Edit mode overlay
        /// </summary>
        private Color _editModeOverlayColor = Color.White * 0.5f;

        /// <summary>
        /// Edit mode overlay
        /// </summary>
        private Content<Texture2D> _editModeOverlayTexture;

        /// <summary>
        /// Information on tileset
        /// </summary>
        [EditorAttribute(typeof(TileSetUITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public TileSet TileSet { get; set; }


        /// <summary>
        /// Empty constructor
        /// </summary>
        public TileSetRender()
        {
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TileSetRender(TileSet tileSet)
        {
            this.TileSet = tileSet;
        }

        /// <summary>
        /// Loading
        /// </summary>
        public override void Load()
        {
            _editModeOverlayTexture = GameHost.GetContent<Texture2D>("pixel");
            
            if (this.TileSet == null)
                return;

            if (this.Collidable)
            {
                TileSet tileset = this.TileSet;
                Tile[][] tiles = tileset.Tiles;

                int posX = 0;
                for (int x = 0; x < tiles.Length; x++)
                {
                    Tile[] column = tiles[x];

                    if (column != null)
                    {
                        int posY = 0;
                        for (int y = 0; y < column.Length; y++)
                        {
                            if (column[y] != null)
                            {
                                Add(new TileGameObject(new Rectangle(posX, posY, tileset.TileScreenSize, tileset.TileScreenSize))).EnableCollider();
                            }

                            posY += tileset.TileScreenSize;
                        }
                    }

                    posX += tileset.TileScreenSize;
                }
            }

        }


        /// <summary>
        /// Overwride drawing...
        /// </summary>
        public override void Draw()
        {
            if (this.TileSet == null || this.TileSet.Texture == null)
                return;

            TileSet tileset = this.TileSet;
            Tile[][] tiles = tileset.Tiles;
            Texture2D texture = tileset.Texture;

            if(texture == null || tiles == null)
                return;

            Color color = Color.White;
            bool editMode = false;

            if (EditModeHelper.EditMode && !EditModeHelper.IsGameRunning)
            {
                if (EditModeHelper.IsTileSetEditorOpened && EditModeHelper.SelectedGameObject == this)
                {
                    //color = Color.DimGray;
                    editMode = true;
                }
            }

            int posX = 0;
            for (int x = 0; x < tiles.Length; x++)
            {
                Tile[] column = tiles[x];

                if (column != null)
                {
                    int posY = 0;
                    for (int y = 0; y < column.Length; y++)
                    {
                        if (column[y] != null)
                        {
                            DrawingContext.Draw(texture, new Rectangle(posX, posY, tileset.TileScreenSize, tileset.TileScreenSize), new Rectangle(column[y].Col * tileset.TileSize, column[y].Row * tileset.TileSize, tileset.TileSize, tileset.TileSize), color, 0f, Vector2.Zero, SpriteEffects.None, this.Depth);

                            if(editMode)
                                DrawingContext.Draw(_editModeOverlayTexture.Data, new Rectangle(posX, posY, tileset.TileScreenSize, tileset.TileScreenSize), new Rectangle(column[y].Col * tileset.TileSize, column[y].Row * tileset.TileSize, tileset.TileSize, tileset.TileSize), _editModeOverlayColor, 0f, Vector2.Zero, SpriteEffects.None, this.Depth);

                        }

                        posY += tileset.TileScreenSize;
                    }
                }

                posX += tileset.TileScreenSize;
            }

        }


    }
}
