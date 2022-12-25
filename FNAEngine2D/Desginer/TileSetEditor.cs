using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
        private static int _currentColIndex = -1;

        /// <summary>
        /// Current selected row
        /// </summary>
        private static int _currentRowIndex = -1;

        /// <summary>
        /// Current selected col length
        /// </summary>
        private static int _currentColLength = -1;

        /// <summary>
        /// Current selected row length
        /// </summary>
        private static int _currentRowLength = -1;

        /// <summary>
        /// Preview object
        /// </summary>
        private TileSetRender _previewObject = null;

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
        public TileSetEditor()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Set of the current game object that is edited
        /// </summary>
        public void SetGameObject(GameObject gameObject)
        {
            if (gameObject is TileSetRender)
            {
                _gameObject = EditModeHelper.SelectedGameObject;
                this.Text = "Tile Set Editor - " + _gameObject.DisplayName;

                TileSetRender tileSetRender = (TileSetRender)gameObject;
                if (tileSetRender.TileSet == null)
                    tileSetRender.TileSet = new TileSet();

                _tileSet = tileSetRender.TileSet;


                //Bind to screen...
                txtTexture.Text = _tileSet.TextureName;
                txtTileSize.Text = _tileSet.TileSize.ToString();
                txtTileScreenSize.Text = _tileSet.TileScreenSize.ToString();

                LoadTileSet(_tileSet.TextureName);
            }
            else
            {
                this.Text = "Tile Set Editor - No selection";
                _tileSet = null;
                _gameObject = null;
                LoadTileSet(String.Empty);
            }
        }

        /// <summary>
        /// The user has click in the game
        /// </summary>
        public void OnMouseLeftClickInGame(int x, int y)
        {
            if (_currentColIndex < 0)
                return;

            bool updated = SetTiles(x, y, _tileSet);

            if (updated)
            {
                EditModeHelper.SetDirty(true);
                EditModeHelper.AddHistory();
                UpdatePreviewTileSet();

                _gameObject.RemoveAll();
                _gameObject.Load();
            }

        }

        /// <summary>
        /// The user has click in the game
        /// </summary>
        public void OnMouseRightClickInGame(int x, int y)
        {
            if (SetTile(x / _tileSet.TileScreenSize, y / _tileSet.TileScreenSize, null, _tileSet))
            {
                EditModeHelper.SetDirty(true);
                EditModeHelper.AddHistory();
                UpdatePreviewTileSet();

                _gameObject.RemoveAll();
                _gameObject.Load();
            }
        }

        /// <summary>
        /// Showing preview
        /// </summary>
        public void ShowPreview(int x, int y)
        {
            if (_currentColIndex < 0)
                return;

            //Preview object creation...
            if (_previewObject == null)
            {
                _previewObject = new TileSetRender(new TileSet());

                UpdatePreviewTileSet();

                _gameObject.RootGameObject.Add(_previewObject);

            }

            _previewObject.Location = _gameObject.Location;

            //Empty tiles..
            _previewObject.TileSet.Tiles = new Tile[0][];
            //Set tiles..
            SetTiles(x, y, _previewObject.TileSet);


        }

        /// <summary>
        /// Hide preview
        /// </summary>
        public void HidePreview()
        {
            if (_previewObject != null && _gameObject != null && _gameObject.RootGameObject != null)
            {
                _gameObject.RootGameObject.Remove(_previewObject);
            }

            _previewObject = null;
        }

        /// <summary>
        /// Update preview tileset with the same settings then the current tileset
        /// </summary>
        private void UpdatePreviewTileSet()
        {
            if (_previewObject != null)
            {
                _previewObject.TileSet.TileSize = _tileSet.TileSize;
                _previewObject.TileSet.TileScreenSize = _tileSet.TileScreenSize;
                _previewObject.TileSet.TextureName = _tileSet.TextureName;
            }
        }


        /// <summary>
        /// Set the tiles in a tileset
        /// </summary>
        private bool SetTiles(int x, int y, TileSet tileSet)
        {
            bool updated = false;
            for (int col = 0; col < _currentColLength; col++)
            {
                for (int row = 0; row < _currentRowLength; row++)
                {
                    if (SetTile((x / tileSet.TileScreenSize) + col, (y / tileSet.TileScreenSize) + row, new Tile(_currentColIndex + col, _currentRowIndex + row), tileSet))
                        updated = true;
                }
            }

            return updated;
        }

        /// <summary>
        /// Set the tile
        /// </summary>
        private bool SetTile(int tileX, int tileY, Tile tile, TileSet tileSet)
        {
            if (tileX < 0 || tileY < 0)
                return false;

            if (tileSet.Tiles == null)
            {
                tileSet.Tiles = new Tile[tileX + 1][];
            }
            else if(tileSet.Tiles.Length <= tileX)
            {
                //We need to grow the tiles array...
                Tile[][] newColTiles = new Tile[tileX + 1][];

                //Copy the existing data...
                Array.Copy(tileSet.Tiles, newColTiles, tileSet.Tiles.Length);

                tileSet.Tiles = newColTiles;
            }

            if (tileSet.Tiles[tileX] == null)
            {
                tileSet.Tiles[tileX] = new Tile[tileY + 1];
            }
            else if (tileSet.Tiles[tileX].Length <= tileY)
            {
                //We need to grow the tiles array...
                Tile[] newRowTiles = new Tile[tileY + 1];

                //Copy the existing data...
                Array.Copy(tileSet.Tiles[tileX], newRowTiles, tileSet.Tiles[tileX].Length);

                tileSet.Tiles[tileX] = newRowTiles;

            }

            if (!AreTileEguals(tileSet.Tiles[tileX][tileY], tile))
            {
                //Needs to update...
                tileSet.Tiles[tileX][tileY] = tile;
                return true;
            }
            else
            {
                return false;
            }


        }

        /// <summary>
        /// Check if 2 tiles are eguals
        /// </summary>
        private bool AreTileEguals(Tile a, Tile b)
        {
            if (a == null && b == null)
                return true;

            if (a == null || b == null)
                return false;

            return a.Col == b.Col && a.Row == b.Row;
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
                    EditModeHelper.AddHistory();
                    UpdatePreviewTileSet();
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
                    EditModeHelper.AddHistory();
                    UpdatePreviewTileSet();
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
                    EditModeHelper.AddHistory();
                    UpdatePreviewTileSet();
                }
            }
        }

        /// <summary>
        /// To force the update on an Enter
        /// </summary>
        private void txtTileSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                txtTileSize_Validated(sender, EventArgs.Empty);
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
            //Removing the preview object
            if (_previewObject != null && _gameObject != null && _gameObject.RootGameObject != null)
            {
                _gameObject.RootGameObject.Remove(_previewObject);
            }

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
        /// Trapping the enter to change the texture
        /// </summary>
        private void txtTileScreenSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                txtTileScreenSize_Validated(sender, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Click on the tileset picture
        /// </summary>
        private void picTileSet_MouseClick(object sender, MouseEventArgs e)
        {
            
        }


        private void picTileSet_Paint(object sender, PaintEventArgs e)
        {
            if (_currentColIndex < 0)
                return;

            e.Graphics.DrawRectangle(Pens.Aqua, new Rectangle(_currentColIndex * _tileSet.TileSize, _currentRowIndex * _tileSet.TileSize, _currentColLength * _tileSet.TileSize, _currentRowLength * _tileSet.TileSize));
        }

        private void picTileSet_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //Juste avoiding a division by zero
                if (_tileSet.TileSize == 0)
                    return;



                int currentColIndex = e.X / _tileSet.TileSize;
                int currentRowIndex = e.Y / _tileSet.TileSize;

                _currentColIndex = currentColIndex;
                _currentRowIndex = currentRowIndex;
                _currentColLength = 1;
                _currentRowLength = 1;

                picTileSet.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picTileSet_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                //Juste avoiding a division by zero
                if (_tileSet.TileSize == 0)
                    return;

                if (e.Button == MouseButtons.Left)
                {

                    int currentColIndex = e.X / _tileSet.TileSize;
                    int currentRowIndex = e.Y / _tileSet.TileSize;

                    if (currentColIndex < _currentColIndex)
                    {
                        _currentColLength += (_currentColIndex - currentColIndex);
                        _currentColIndex = currentColIndex;
                    }
                    else if (currentColIndex >= _currentColIndex + _currentColLength)
                    {
                        _currentColLength = currentColIndex - _currentColIndex + 1;
                    }

                    if (currentRowIndex < _currentRowIndex)
                    {
                        _currentRowLength += (_currentRowIndex - currentRowIndex);
                        _currentRowIndex = currentRowIndex;
                    }
                    else if (currentRowIndex >= _currentRowIndex + _currentRowLength)
                        _currentRowLength = currentRowIndex - _currentRowIndex + 1;

                    picTileSet.Invalidate();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Form activated
        /// </summary>
        private void TileSetEditor_Activated(object sender, EventArgs e)
        {
            //Reopening the designer forms and refocusing on ourself...
            if (EditModeHelper.IsReshowWindowNeeded())
                EditModeHelper.ShowDesigner(this.Handle);
        }

    }
}
