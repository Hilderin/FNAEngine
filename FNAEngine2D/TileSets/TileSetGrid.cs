using FNAEngine2D.Desginer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace FNAEngine2D.TileSets
{
    /// <summary>
    /// Tileset
    /// </summary>
    public class TileSetGrid: GameObject, IDraw
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
        /// Scale
        /// </summary>
        private Vector2 _scale = Vector2.One;


        /// <summary>
        /// Information on tileset
        /// </summary>
        [Editor(typeof(TileSetUITypeEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public TileSet TileSet { get; set; }


        /// <summary>
        /// Empty constructor
        /// </summary>
        public TileSetGrid()
        {
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public TileSetGrid(TileSet tileSet)
        {
            this.TileSet = tileSet;
        }

        /// <summary>
        /// Loading
        /// </summary>
        protected override void Load()
        {
            _editModeOverlayTexture = GetContent<Texture2D>("pixel");
            
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
                                //If there is a tile right, left, top and bottom, no need for a collider...
                                if (tileset.GetTile(x + 1, y) == null || tileset.GetTile(x - 1, y) == null || tileset.GetTile(x, y - 1) == null || tileset.GetTile(x, y + 1) == null)
                                    Add(new TileGameObject(new Rectangle(posX, posY, tileset.TileScreenSize, tileset.TileScreenSize)));
                            }

                            posY += tileset.TileScreenSize;
                        }
                    }

                    posX += tileset.TileScreenSize;
                }
            }

            _scale = new Vector2((float)this.TileSet.TileScreenSize / this.TileSet.TileSize, (float)this.TileSet.TileScreenSize / this.TileSet.TileSize);

        }


        /// <summary>
        /// Overwride drawing...
        /// </summary>
        public void Draw()
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

            if (this.Game.EditModeService != null && this.Game.EditModeService.EditMode && !this.Game.EditModeService.IsGameRunning)
            {
                if (this.Game.EditModeService.IsTileSetEditorOpened && this.Game.EditModeService.SelectedGameObject == this)
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
                            DrawingContext.Draw(texture, new Vector2(posX, posY), new Rectangle(column[y].Col * tileset.TileSize, column[y].Row * tileset.TileSize, tileset.TileSize, tileset.TileSize), color, 0f, Vector2.Zero, _scale, SpriteEffects.None, this.Depth);

                            if(editMode)
                                DrawingContext.Draw(_editModeOverlayTexture.Data, new Vector2(posX, posY), new Rectangle(column[y].Col * tileset.TileSize, column[y].Row * tileset.TileSize, tileset.TileSize, tileset.TileSize), _editModeOverlayColor, 0f, Vector2.Zero, _scale, SpriteEffects.None, this.Depth);

                        }

                        posY += tileset.TileScreenSize;
                    }
                }

                posX += tileset.TileScreenSize;
            }

        }


        /// <summary>
        /// EnableCollider - We don't want to create a real collider, we will create them with the TileGameObject
        /// </summary>
        public override GameObject EnableCollider()
        {
            return this;
        }

    }
}
