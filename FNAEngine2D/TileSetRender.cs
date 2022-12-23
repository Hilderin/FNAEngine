﻿using FNAEngine2D.Desginer;
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
        /// Information on tileset
        /// </summary>
        [EditorAttribute(typeof(TileSetEditor), typeof(System.Drawing.Design.UITypeEditor))]
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

            if (GameHost.EditMode)
            {
                if (TileSetEditorForm.Current != null && TileSetEditorForm.Current.GameObject != this)
                    color = Color.DimGray;
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
                            GameHost.SpriteBatch.Draw(texture, new Rectangle(posX, posY, tileset.TileScreenSize, tileset.TileScreenSize), new Rectangle(column[y].Col * tileset.TileSize, column[y].Row * tileset.TileSize, tileset.TileSize, tileset.TileSize), color, 0f, Vector2.Zero, SpriteEffects.None, this.LayerDepth);
                            //GameHost.SpriteBatch.Draw(_texture, new Vector2(posX, posY), new Rectangle(column[y].X * TileSize, column[y].Y * TileSize, TileSize, TileSize), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                        }

                        posY += tileset.TileScreenSize;
                    }
                }

                posX += tileset.TileScreenSize;
            }

        }


    }
}
