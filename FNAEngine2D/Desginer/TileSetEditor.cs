using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAEngine2D.Desginer
{
    public partial class TileSetEditor : Form
    {
        /// <summary>
        /// TileSet that we are editing...
        /// </summary>
        private TileSet _tileSet;

        /// <summary>
        /// Current tile set editing
        /// </summary>
        public TileSet TileSet { get { return _tileSet; } }

        /// <summary>
        /// Current selected col
        /// </summary>
        private int _currentColIndex = -1;

        /// <summary>
        /// Current selected row
        /// </summary>
        private int _currentRowIndex = -1;

        /// <summary>
        /// Indicateur that the form is closing
        /// </summary>
        public bool IsClosing { get; set; }

        /// <summary>
        /// Game object TileSetRenderer
        /// </summary>
        private GameObject _gameObject;

        /// <summary>
        /// Current GameObject
        /// </summary>
        public GameObject GameObject
        {
            get { return _gameObject; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TileSetEditor(TileSet tileSet)
        {
            InitializeComponent();

            _gameObject = EditModeHelper.SelectedGameObject;
            this.Text = "Tile Set Editor - " + _gameObject.DisplayName;

            if (tileSet == null)
                tileSet = new TileSet();

            _tileSet = tileSet;


            //Bind to screen...
            txtTexture.Text = _tileSet.TextureName;
            txtTileSize.Text = _tileSet.TileSize.ToString();
            txtTileScreenSize.Text = _tileSet.TileScreenSize.ToString();

            LoadTileSet(_tileSet.TextureName);

        }

        /// <summary>
        /// The user has click in the game
        /// </summary>
        public void OnMouseLeftClickInGame(int x, int y)
        {
            if (_currentColIndex < 0)
                return;

            SetCurrentTile(x / _tileSet.TileScreenSize, y / _tileSet.TileScreenSize, new Tile(_currentColIndex, _currentRowIndex));
        }

        /// <summary>
        /// The user has click in the game
        /// </summary>
        public void OnMouseRightClickInGame(int x, int y)
        {
            SetCurrentTile(x / _tileSet.TileScreenSize, y / _tileSet.TileScreenSize, null);
        }


        /// <summary>
        /// Set the tile
        /// </summary>
        private void SetCurrentTile(int tileX, int tileY, Tile tile)
        {
            if (tileX < 0 || tileY < 0)
                return;

            if (_tileSet.Tiles == null)
            {
                _tileSet.Tiles = new Tile[tileX + 1][];
            }
            else if(_tileSet.Tiles.Length <= tileX)
            {
                //We need to grow the tiles array...
                Tile[][] newColTiles = new Tile[tileX + 1][];

                //Copy the existing data...
                Array.Copy(_tileSet.Tiles, newColTiles, _tileSet.Tiles.Length);

                _tileSet.Tiles = newColTiles;
            }

            if (_tileSet.Tiles[tileX] == null)
            {
                _tileSet.Tiles[tileX] = new Tile[tileY + 1];
            }
            else if (_tileSet.Tiles[tileX].Length <= tileY)
            {
                //We need to grow the tiles array...
                Tile[] newRowTiles = new Tile[tileY + 1];

                //Copy the existing data...
                Array.Copy(_tileSet.Tiles[tileX], newRowTiles, _tileSet.Tiles[tileX].Length);

                _tileSet.Tiles[tileX] = newRowTiles;

            }

            _tileSet.Tiles[tileX][tileY] = tile;

            EditModeHelper.SetDirty(true);

            _gameObject.RemoveAll();
            _gameObject.Load();

        }

        /// <summary>
        /// Load the tileset
        /// </summary>
        private void LoadTileSet(string assetName)
        {
            try
            {

                if (String.IsNullOrEmpty(assetName))
                {
                    picTileSet.Hide();
                }
                else
                {
                    
                    string fullPath = GameHost.InternalGame.ContentManager.GetAssetFullPath(assetName, ContentManager.TEXTURES_EXTENSIONS);

                    picTileSet.Image = Image.FromFile(fullPath);

                    picTileSet.Show();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Texture validated
        /// </summary>
        private void txtTexture_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtTexture.Text != _tileSet.TextureName)
                {
                    _tileSet.TextureName = txtTexture.Text;
                    LoadTileSet(_tileSet.TextureName);

                    EditModeHelper.SetDirty(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tile size validating
        /// </summary>
        private void txtTileSize_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtTileSize.Text) || !Int32.TryParse(txtTileSize.Text, out var bidon))
                e.Cancel = true;
        }

        /// <summary>
        /// Validating
        /// </summary>
        private void txtTileScreenSize_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtTileSize.Text) || !Int32.TryParse(txtTileSize.Text, out var bidon))
                e.Cancel = true;
        }

        /// <summary>
        /// Tile size validated
        /// </summary>
        private void txtTileSize_Validated(object sender, EventArgs e)
        {
            if (Int32.TryParse(txtTileSize.Text, out var size))
            {
                if (_tileSet.TileSize != size)
                {
                    _tileSet.TileSize = size;

                    EditModeHelper.SetDirty(true);
                }
            }
        }

        /// <summary>
        /// Tile screen size validated
        /// </summary>
        private void txtTileScreenSize_Validated(object sender, EventArgs e)
        {
            if (Int32.TryParse(txtTileScreenSize.Text, out var size))
            {
                if (_tileSet.TileScreenSize != size)
                {
                    _tileSet.TileScreenSize = size;

                    EditModeHelper.SetDirty(true);
                }
            }
        }


        /// <summary>
        /// Form closing
        /// </summary>
        private void TileSetEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IsClosing = true;
        }

        /// <summary>
        /// Form closed
        /// </summary>
        private void TileSetEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            EditModeHelper.HideTileSetEditor();
        }

        /// <summary>
        /// Trapping the enter to change the texture
        /// </summary>
        private void txtTexture_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                txtTexture_Validated(sender, EventArgs.Empty);
            }
        }

        
        /// <summary>
        /// Click on the tileset picture
        /// </summary>
        private void picTileSet_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //Juste avoiding a division by zero
                if (_tileSet.TileSize == 0)
                    return;


                _currentColIndex = e.X / _tileSet.TileSize;
                _currentRowIndex = e.Y / _tileSet.TileSize;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
