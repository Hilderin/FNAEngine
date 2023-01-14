using FNAEngine2D.Desginer;
using FNAEngine2D.GameObjects;
using FNAEngine2D.TileSets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using SharpFont.TrueType;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velentr.Font;

namespace FNAEngine2D
{
    /// <summary>
    /// Helper for the edit mode
    /// </summary>
    public class EditModeService
    {
        /// <summary>
        /// Camera speed
        /// </summary>
        private const int CAMERA_SPEED_PIXEL_PER_SECONDS = 500;

        /// <summary>
        /// Content designer
        /// </summary>
        private ContentDesigner _designer = null;

        /// <summary>
        /// Content designer
        /// </summary>
        private TileSetEditor _tileSetEditor = null;

        /// <summary>
        /// Indicate if the mouse was showned when we displayed the mouse
        /// </summary>
        private bool _isMouseWasShowned = false;

        /// <summary>
        /// Indicate if we are in edit mode
        /// </summary>
        private bool _editMode = false;

        /// <summary>
        /// Indicate if we when to run the game in edit mode
        /// </summary>
        private bool _isGameRunning = false;

        /// <summary>
        /// Location of the player at the beginning of MoveLPlayerMode
        /// </summary>
        private Vector2 _playerLocationOrigin;

        /// <summary>
        /// SpriteBatch
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Font to display edit mode
        /// </summary>
        private Font _font;

        /// <summary>
        /// Text
        /// </summary>
        private Velentr.Font.Text _text;

        /// <summary>
        /// Internal game
        /// </summary>
        private Game _game;


        /// <summary>
        /// Content watcher
        /// </summary>
        private ContentWatcher _contentWatcher;


        /// <summary>
        /// Selected game object in the designer
        /// </summary>
        public GameObject SelectedGameObject { get; set; }

        /// <summary>
        /// Indicate we the TileSet editor is opened
        /// </summary>
        public bool IsTileSetEditorOpened { get { return _tileSetEditor != null && _tileSetEditor.Visible; } }

        /// <summary>
        /// DateTime when the Game or a designer was last activate
        /// </summary>
        public DateTime LastTimeActivated { get; set; }

        /// <summary>
        /// Player object that can be moved
        /// </summary>
        public GameObject PlayerObject { get; set; }

        /// <summary>
        /// Moving player mode enabled
        /// </summary>
        public bool MovePlayerMode { get; set; }

        /// <summary>
        /// Game
        /// </summary>
        public Game Game { get { return _game; } }

        /// <summary>
        /// ContentWatcher
        /// </summary>
        public ContentWatcher ContentWatcher { get { return _contentWatcher; } }

        /// <summary>
        /// Indicate if we are in edit mode
        /// </summary>
        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                if (_editMode != value)
                {
                    _editMode = value;

                    IsGameRunning = false;
                    SelectedGameObject = null;

                    if (_editMode)
                    {
                        ShowDesigner();
                    }
                    else
                    {
                        HideDesigner();
                        HideTileSetEditor();
                    }

                }
            }
        }


        /// <summary>
        /// Indicate if we when to run the game in edit mode
        /// </summary>
        public bool IsGameRunning
        {
            get { return _isGameRunning; }
            set
            {
                if (_isGameRunning != value)
                {
                    _isGameRunning = value;
                    if (_isGameRunning)
                    {
                        if (_designer != null && !_designer.IsDisposed)
                        {
                            _designer.HidePreview();
                            _designer.UpdatePausePlayUI();
                        }

                        if (_tileSetEditor != null && !_tileSetEditor.IsDisposed)
                        {
                            _tileSetEditor.HidePreview();
                        }


                        if (!_isMouseWasShowned)
                            _game.Mouse.HideMouse();
                    }
                    else
                    {
                        //Reshowing the mouse...
                        _game.Mouse.ShowMouse();

                        if (_designer != null && !_designer.IsDisposed)
                        {
                            _designer.UpdatePausePlayUI();
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Constructor
        /// </summary>
        public EditModeService(Game game)
        {
            _game = game;

            //Add to the service provider...
            _game.Services.AddService(typeof(EditModeService), this);

            _contentWatcher = new ContentWatcher(game);

        }




        /// <summary>
        /// Process the Update loop for DevMode
        /// </summary>
        public void ProcessUpdateDevMode()
        {

            //Content to reload?
            _contentWatcher.ReloadModifiedContent();


            if (!_game.IsActive)
                return;


            LastTimeActivated = DateTime.Now;

            //Reload the content...
            if (_game.Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                if (ConfirmBeforeClose(true))
                {
                    Revert();

                    _game.RootGameObject.RemoveAll();
                    _game.RootGameObject.DoLoad();

                    ReloadDesigner();
                }


            }

            //Designer...
            if (_game.Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F12))
            {
                if (EditMode)
                {
                    //We must close the designer...
                    HideDesigner();
                }
                else
                {
                    //Opening the designer...
                    ShowDesigner();

                }
            }

            //Pause/play game
            if (EditMode && _game.Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Pause))
            {
                IsGameRunning = !IsGameRunning;
            }

        }


        /// <summary>
        /// Process the Update loop in edit mode
        /// </summary>
        public void ProcessUpdateEditMode()
        {
            //Cancel selection...
            if (_game.Input.IsKeyDown(Keys.Escape))
            {
                ClearSelection();
            }

            Vector2 mousePosition = _game.Input.GetMouseWorldPosition(_game.MainCamera);

            if (_game.IsActive)
            {
                //Game active...
                if (_game.Input.IsKeyDown(Keys.A))
                    //Moving camera to the left
                    _game.MainCamera.Location = _game.MainCamera.Location.AddX(-CAMERA_SPEED_PIXEL_PER_SECONDS * _game.ElapsedGameTimeSeconds);
                else if (_game.Input.IsKeyDown(Keys.D))
                    //Moving camera to the right
                    _game.MainCamera.Location = _game.MainCamera.Location.AddX(CAMERA_SPEED_PIXEL_PER_SECONDS * _game.ElapsedGameTimeSeconds);

                if (_game.Input.IsKeyDown(Keys.W))
                    //Moving camera to the top
                    _game.MainCamera.Location = _game.MainCamera.Location.AddY(-CAMERA_SPEED_PIXEL_PER_SECONDS * _game.ElapsedGameTimeSeconds);
                else if (_game.Input.IsKeyDown(Keys.S))
                    //Moving camera to the down
                    _game.MainCamera.Location = _game.MainCamera.Location.AddY(CAMERA_SPEED_PIXEL_PER_SECONDS * _game.ElapsedGameTimeSeconds);


                //Moving player...
                if (_game.Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F11))
                {
                    if (PlayerObject != null)
                    {
                        if (MovePlayerMode)
                        {
                            //Revert player move mode...
                            ResetMovePlayerMode();
                        }
                        else
                        {
                            //Enable player mode...
                            _playerLocationOrigin = PlayerObject.Location;
                            MovePlayerMode = true;
                        }
                    }
                }


                //Mouse left click to place a tile...
                if (MovePlayerMode)
                {
                    if (_game.Input.IsMouseLeftNewDown())
                    {
                        //Leaving moving player mode...
                        MovePlayerMode = false;
                    }
                    else if (_game.Input.IsMouseRightNewDown())
                    {
                        //Reset moving player mode...
                        ResetMovePlayerMode();
                    }
                }
                else
                {
                    if (_game.Input.IsMouseLeftDown() || _game.Input.IsMouseRightDown())
                    {
                        if (IsTileSetEditorOpened)
                        {
                            if (_game.Input.IsMouseLeftDown())
                                _tileSetEditor.OnMouseLeftClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                            if (_game.Input.IsMouseRightDown())
                                _tileSetEditor.OnMouseRightClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                        }
                    }

                    if (_game.Input.IsMouseLeftNewDown() || _game.Input.IsMouseRightNewDown())
                    {
                        if (_designer != null && !_designer.IsDisposed)
                        {
                            if (_game.Input.IsMouseLeftDown())
                                _designer.OnMouseLeftClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                            if (_game.Input.IsMouseRightDown())
                                _designer.OnMouseRightClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                        }
                    }
                }

            }


            //Even inactive i want to see the preview...
            //Showing preview on mouse cursor...
            string text = "EditMode";
            if (MovePlayerMode)
            {
                //Moving player...
                PlayerObject.Location = mousePosition;
                text += " MovePlayerMode";
            }
            else if (IsTileSetEditorOpened)
            {
                //TileSet mode...
                _tileSetEditor.ShowPreview((int)mousePosition.X, (int)mousePosition.Y);
                text += " TileSetMode";
            }
            else if (_designer != null && !_designer.IsDisposed)
            {
                //Content mode...
                _designer.ShowPreview((int)mousePosition.X, (int)mousePosition.Y);
                text += " ContentEditor";
            }


        }

        /// <summary>
        /// Process the Draw loop in edit mode
        /// </summary>
        public void ProcessDrawEditMode()
        {
            if (_spriteBatch == null)
                _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            if(_font == null)
                _font = new Font(ContentManager.FONT_ROBOTO_REGULAR, 12);


            string text = "EditMode";
            Color color = Color.White;
            if (IsGameRunning)
            {
                //Game running...
                text += " GameRunning";
                color = Color.Green;
            }
            else if (MovePlayerMode)
            {
                //Moving player...
                text += " MovePlayerMode";
                color = Color.HotPink;
            }
            else if (IsTileSetEditorOpened)
            {
                //TileSet mode...
                text += " TileSetMode";
                color = Color.Purple;
            }
            else if (_designer != null && !_designer.IsDisposed)
            {
                //Content mode...
                text += " ContentEditor";
                color = Color.Orange;
            }

            if (_text == null || _text.String != text)
                _text = _font.MakeText(text);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_text, new Vector2(_game.ScreenSize.X - _text.Width - 10, _game.ScreenSize.Y - _text.Height), color);
            _spriteBatch.End();
        }


        /// <summary>
        /// Remove the curret selection of object
        /// </summary>
        public void ClearSelection()
        {
            if (MovePlayerMode)
            {
                //Resetting player position...
                ResetMovePlayerMode();
            }
            if (IsTileSetEditorOpened)
                _tileSetEditor.ClearSelection();
            if (_designer != null && !_designer.IsDisposed)
                _designer.ClearSelection();
        }

        /// <summary>
        /// Check if a reshowing of all window is needed
        /// </summary>
        public bool IsReshowWindowNeeded()
        {
            return DateTime.Now.Subtract(LastTimeActivated).TotalMilliseconds > 300;
        }




        /// <summary>
        /// Show the designer
        /// </summary>
        public void ShowDesigner(IntPtr? focusWindowHandle = null)
        {
            _editMode = true;

            if (_designer == null || _designer.IsDisposed)
            {
                _designer = new ContentDesigner();

                _designer.Show();

                _isMouseWasShowned = _game.Mouse.IsMouseVisible;

                WindowHelper.LastWindowSwitch = DateTime.MinValue;
            }

            //Restore/disply window...
            if (_designer.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                _designer.WindowState = System.Windows.Forms.FormWindowState.Normal;
                WindowHelper.LastWindowSwitch = DateTime.MinValue;
            }

            if (!_designer.Visible)
            {
                _designer.Show();
                WindowHelper.LastWindowSwitch = DateTime.MinValue;
            }


            if (focusWindowHandle == null)
                //We will focus on the game...
                ShowAllGameForms(_game.GameWindowHandle);
            else
                ShowAllGameForms(focusWindowHandle.Value);

            //Et on affiche la souris...
            _game.Mouse.ShowMouse();
        }

        /// <summary>
        /// Hide the designer
        /// </summary>
        public void HideDesigner()
        {
            _editMode = false;

            if (_designer != null && !_designer.IsDisposed)
            {
                _designer.HidePreview();
                _designer.Hide();
            }

            if (!_isMouseWasShowned)
                _game.Mouse.HideMouse();
        }

        /// <summary>
        /// Reload the designer
        /// </summary>
        public void ReloadDesigner()
        {
            if (_designer != null && !_designer.IsDisposed)
                _designer.Reload();
        }

        /// <summary>
        /// Revert all modifications
        /// </summary>
        public void Revert()
        {
            if (_designer != null && !_designer.IsDisposed)
                _designer.Revert();
        }

        /// <summary>
        /// Set the IsDirty
        /// </summary>
        public void SetDirty(bool dirty)
        {
            if (_designer != null && !_designer.IsDisposed)
                _designer.SetDirty(dirty);
        }

        /// <summary>
        /// Add the Game Content state in the history
        /// </summary>
        public void AddHistory()
        {
            if (_designer != null && !_designer.IsDisposed)
                _designer.AddHistory();
        }

        /// <summary>
        /// Show tileset editor
        /// </summary>
        public void ShowTileSetEditor(TileSetRender gameObject, bool setFocus)
        {

            if (_tileSetEditor == null || _tileSetEditor.IsDisposed)
            {
                _tileSetEditor = new TileSetEditor();
                _tileSetEditor.Show();
                WindowHelper.LastWindowSwitch = DateTime.MinValue;
            }

            if (_tileSetEditor != null && (_tileSetEditor.TileSet != gameObject.TileSet || _tileSetEditor.GameObject != gameObject))
            {
                _tileSetEditor.SetGameObject(gameObject);
            }



            //Restore/disply window...
            if (_tileSetEditor.WindowState == System.Windows.Forms.FormWindowState.Minimized)
            {
                _tileSetEditor.WindowState = System.Windows.Forms.FormWindowState.Normal;
                WindowHelper.LastWindowSwitch = DateTime.MinValue;
            }

            if (!_tileSetEditor.Visible)
            {
                _tileSetEditor.Show();
                WindowHelper.LastWindowSwitch = DateTime.MinValue;
            }


            //We will focus on the editor...
            if (setFocus)
                ShowAllGameForms(_tileSetEditor.Handle);
        }

        /// <summary>
        /// Show tileset editor
        /// </summary>
        public void HideTileSetEditor()
        {
            if (_tileSetEditor != null && !_tileSetEditor.IsDisposed)
            {
                _tileSetEditor.HidePreview();
                _tileSetEditor.Hide();
            }
        }

        /// <summary>
        /// Show all game forms
        /// </summary>
        private void ShowAllGameForms(IntPtr focusWindowHandle)
        {

            if (!WindowHelper.IsIntervalOKToSwitchWindow())
                return;


            List<IntPtr> windowHandles = new List<IntPtr>();

            if (_designer != null && !_designer.IsDisposed)
                windowHandles.Add(_designer.Handle);

            if (_tileSetEditor != null && !_tileSetEditor.IsDisposed)
                windowHandles.Add(_tileSetEditor.Handle);

            windowHandles.Add(_game.GameWindowHandle);

            WindowHelper.ShowAllWindows(windowHandles, focusWindowHandle);

        }

        /// <summary>
        /// Ask confirmation if changes in designer
        /// </summary>
        internal bool ConfirmBeforeClose(bool canCancel = true)
        {
            //Check if modification in the designer before closing
            if (_designer != null && !_designer.IsDisposed)
            {
                if (!_designer.ConfirmBeforeClose(canCancel))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// Quit designer
        /// </summary>
        internal bool Quit(bool canCancel = true)
        {

            if (_designer != null && !_designer.IsDisposed)
            {
                //Check if modification in the designer before closing
                if (!ConfirmBeforeClose(canCancel))
                {
                    return false;
                }

                //Close of the content designer
                _designer.Close();
            }

            _designer = null;
            return true;
        }

        /// <summary>
        /// Reset de MovePlayerMode
        /// </summary>
        private void ResetMovePlayerMode()
        {
            if (MovePlayerMode)
            {
                if (PlayerObject != null)
                    PlayerObject.Location = _playerLocationOrigin;
                MovePlayerMode = false;
            }
        }

        /// <summary>
        /// Move to object
        /// </summary>
        public void MoveToObject(GameObject gameObject)
        {
            if (gameObject == null)
                return;


            _game.MainCamera.Location = gameObject.Location - new Vector2(_game.Width / 2, _game.Height / 2);


        }


    }
}
