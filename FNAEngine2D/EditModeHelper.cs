using FNAEngine2D.Desginer;
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
    public static class EditModeHelper
    {
        /// <summary>
        /// Camera speed
        /// </summary>
        private const int CAMERA_SPEED_PIXEL_PER_SECONDS = 500;


        /// <summary>
        /// Content designer
        /// </summary>
        private static ContentDesigner _designer = null;

        /// <summary>
        /// Content designer
        /// </summary>
        private static TileSetEditor _tileSetEditor = null;

        /// <summary>
        /// Indicate if the mouse was showned when we displayed the mouse
        /// </summary>
        private static bool _isMouseWasShowned = false;

        /// <summary>
        /// Indicate if we are in edit mode
        /// </summary>
        private static bool _editMode = false;

        /// <summary>
        /// Indicate if we when to run the game in edit mode
        /// </summary>
        private static bool _isGameRunning = false;

        /// <summary>
        /// Location of the player at the beginning of MoveLPlayerMode
        /// </summary>
        private static Vector2 _playerLocationOrigin;

        /// <summary>
        /// SpriteBatch
        /// </summary>
        private static SpriteBatch _spriteBatch;

        /// <summary>
        /// Font to display edit mode
        /// </summary>
        private static Font _font;

        /// <summary>
        /// Text
        /// </summary>
        private static Velentr.Font.Text _text;



        /// <summary>
        /// Selected game object in the designer
        /// </summary>
        public static GameObject SelectedGameObject { get; set; }

        /// <summary>
        /// Indicate we the TileSet editor is opened
        /// </summary>
        public static bool IsTileSetEditorOpened { get { return _tileSetEditor != null && _tileSetEditor.Visible; } }

        /// <summary>
        /// DateTime when the Game or a designer was last activate
        /// </summary>
        public static DateTime LastTimeActivated { get; set; }

        /// <summary>
        /// Player object that can be moved
        /// </summary>
        public static GameObject PlayerObject { get; set; }

        /// <summary>
        /// Moving player mode enabled
        /// </summary>
        public static bool MovePlayerMode { get; set; }

        /// <summary>
        /// Indicate if we are in edit mode
        /// </summary>
        public static bool EditMode
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
        public static bool IsGameRunning
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
                            MouseManager.HideMouse();
                    }
                    else
                    {
                        //Reshowing the mouse...
                        MouseManager.ShowMouse();

                        if (_designer != null && !_designer.IsDisposed)
                        {
                            _designer.UpdatePausePlayUI();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Process the Update loop for DevMode
        /// </summary>
        public static void ProcessUpdateDevMode()
        {
            //We will need the current main windows Win32
            if (GameHost.InternalGame.GameWindowHandle == IntPtr.Zero)
                GameHost.InternalGame.GameWindowHandle = WindowHelper.GetMainWindowHandle();

            if (!GameHost.InternalGame.IsActive)
                return;

            EditModeHelper.LastTimeActivated = DateTime.Now;

            //Reload the content...
            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F5))
            {
                if (EditModeHelper.ConfirmBeforeClose(true))
                {
                    EditModeHelper.Revert();

                    GameHost.InternalGame.RootGameObject.RemoveAll();
                    GameHost.InternalGame.RootGameObject.Load();

                    EditModeHelper.ReloadDesigner();
                }


            }

            //Designer...
            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F12))
            {
                if (EditModeHelper.EditMode)
                {
                    //We must close the designer...
                    EditModeHelper.HideDesigner();
                }
                else
                {
                    //Opening the designer...
                    EditModeHelper.ShowDesigner();

                }
            }

            //Pause/play game
            if (EditModeHelper.EditMode && Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Pause))
            {
                IsGameRunning = !IsGameRunning;
            }

        }


        /// <summary>
        /// Process the Update loop in edit mode
        /// </summary>
        public static void ProcessUpdateEditMode()
        {
            //Cancel selection...
            if (Input.IsKeyDown(Keys.Escape))
            {
                ClearSelection();
            }

            Vector2 mousePosition = Input.MousePosition();

            if (GameHost.InternalGame.IsActive)
            {
                //Game active...
                if (Input.IsKeyDown(Keys.A))
                    //Moving camera to the left
                    GameHost.MainCamera.Location = GameHost.MainCamera.Location.AddX(-CAMERA_SPEED_PIXEL_PER_SECONDS * GameHost.ElapsedGameTimeSeconds);
                else if (Input.IsKeyDown(Keys.D))
                    //Moving camera to the right
                    GameHost.MainCamera.Location = GameHost.MainCamera.Location.AddX(CAMERA_SPEED_PIXEL_PER_SECONDS * GameHost.ElapsedGameTimeSeconds);

                if (Input.IsKeyDown(Keys.W))
                    //Moving camera to the top
                    GameHost.MainCamera.Location = GameHost.MainCamera.Location.AddY(-CAMERA_SPEED_PIXEL_PER_SECONDS * GameHost.ElapsedGameTimeSeconds);
                else if (Input.IsKeyDown(Keys.S))
                    //Moving camera to the down
                    GameHost.MainCamera.Location = GameHost.MainCamera.Location.AddY(CAMERA_SPEED_PIXEL_PER_SECONDS * GameHost.ElapsedGameTimeSeconds);


                //Moving player...
                if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F11))
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
                    if (Input.MouseLeftNewDown())
                    {
                        //Leaving moving player mode...
                        MovePlayerMode = false;
                    }
                    else if (Input.MouseRightNewDown())
                    {
                        //Reset moving player mode...
                        ResetMovePlayerMode();
                    }
                }
                else
                {
                    if (Input.MouseLeftDown() || Input.MouseRightDown())
                    {
                        if (IsTileSetEditorOpened)
                        {
                            if (Input.MouseLeftDown())
                                _tileSetEditor.OnMouseLeftClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                            if (Input.MouseRightDown())
                                _tileSetEditor.OnMouseRightClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                        }
                    }

                    if (Input.MouseLeftNewDown() || Input.MouseRightNewDown())
                    {
                        if (_designer != null && !_designer.IsDisposed)
                        {
                            if (Input.MouseLeftDown())
                                _designer.OnMouseLeftClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                            if (Input.MouseRightDown())
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
        public static void ProcessDrawEditMode()
        {
            if (_spriteBatch == null)
                _spriteBatch = new SpriteBatch(GameHost.InternalGame.GraphicsDevice);
            if(_font == null)
                _font = new Font(FontManager.ROBOTO_REGULAR, 12);


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
            _spriteBatch.DrawString(_text, new Vector2(GameHost.ScreenSize.X - _text.Width - 10, GameHost.ScreenSize.Y - _text.Height), color);
            _spriteBatch.End();
        }


        /// <summary>
        /// Remove the curret selection of object
        /// </summary>
        public static void ClearSelection()
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
        public static bool IsReshowWindowNeeded()
        {
            return DateTime.Now.Subtract(EditModeHelper.LastTimeActivated).TotalMilliseconds > 300;
        }




        /// <summary>
        /// Show the designer
        /// </summary>
        public static void ShowDesigner(IntPtr? focusWindowHandle = null)
        {
            _editMode = true;

            if (_designer == null || _designer.IsDisposed)
            {
                _designer = new ContentDesigner();

                _designer.Show();

                _isMouseWasShowned = MouseManager.IsMouseVisible;

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
                ShowAllGameForms(GameHost.InternalGame.GameWindowHandle);
            else
                ShowAllGameForms(focusWindowHandle.Value);

            //Et on affiche la souris...
            MouseManager.ShowMouse();
        }

        /// <summary>
        /// Hide the designer
        /// </summary>
        public static void HideDesigner()
        {
            _editMode = false;

            if (_designer != null && !_designer.IsDisposed)
            {
                _designer.HidePreview();
                _designer.Hide();
            }

            if (!_isMouseWasShowned)
                MouseManager.HideMouse();
        }

        /// <summary>
        /// Reload the designer
        /// </summary>
        public static void ReloadDesigner()
        {
            if (_designer != null && !_designer.IsDisposed)
                _designer.Reload();
        }

        /// <summary>
        /// Revert all modifications
        /// </summary>
        public static void Revert()
        {
            if (_designer != null && !_designer.IsDisposed)
                _designer.Revert();
        }

        /// <summary>
        /// Set the IsDirty
        /// </summary>
        public static void SetDirty(bool dirty)
        {
            if (_designer != null && !_designer.IsDisposed)
                _designer.SetDirty(dirty);
        }

        /// <summary>
        /// Add the Game Content state in the history
        /// </summary>
        public static void AddHistory()
        {
            if (_designer != null && !_designer.IsDisposed)
                _designer.AddHistory();
        }

        /// <summary>
        /// Show tileset editor
        /// </summary>
        public static void ShowTileSetEditor(TileSetRender gameObject, bool setFocus)
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
        public static void HideTileSetEditor()
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
        private static void ShowAllGameForms(IntPtr focusWindowHandle)
        {

            if (!WindowHelper.IsIntervalOKToSwitchWindow())
                return;


            List<IntPtr> windowHandles = new List<IntPtr>();

            if (_designer != null && !_designer.IsDisposed)
                windowHandles.Add(_designer.Handle);

            if (_tileSetEditor != null && !_tileSetEditor.IsDisposed)
                windowHandles.Add(_tileSetEditor.Handle);

            windowHandles.Add(GameHost.InternalGame.GameWindowHandle);

            WindowHelper.ShowAllWindows(windowHandles, focusWindowHandle);

        }

        /// <summary>
        /// Ask confirmation if changes in designer
        /// </summary>
        internal static bool ConfirmBeforeClose(bool canCancel = true)
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
        internal static bool Quit(bool canCancel = true)
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
        private static void ResetMovePlayerMode()
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
        public static void MoveToObject(GameObject gameObject)
        {
            if (gameObject == null)
                return;


            GameHost.MainCamera.Location = gameObject.Location - new Vector2(GameHost.Width / 2, GameHost.Height / 2);


        }


    }
}
