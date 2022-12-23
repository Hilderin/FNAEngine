using FNAEngine2D.Desginer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private const int CAMERA_SPEED_PIXEL_PER_SECONDS = 300;


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
        public static bool IsGameRunning { get; set; } = false;

        /// <summary>
        /// Selected game object in the designer
        /// </summary>
        public static GameObject SelectedGameObject { get; set; }

        /// <summary>
        /// Indicate we the TileSet editor is opened
        /// </summary>
        public static bool IsTileSetEditorOpened { get { return _tileSetEditor != null; } }

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
        /// Process the Update loop in edit mode
        /// </summary>
        public static void ProcessUpdateEditMode()
        {
            if (!GameHost.InternalGame.IsActive)
                return;

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


            //Mouse left click to place a tile...
            if (Input.MouseLeftDown() || Input.MouseRightDown())
            {
                if (_tileSetEditor != null)
                {
                    Vector2 mousePosition = Input.MousePosition();
                    if(Input.MouseLeftDown())
                        _tileSetEditor.OnMouseLeftClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                    if (Input.MouseRightDown())
                        _tileSetEditor.OnMouseRightClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                }
            }

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
                //We will focus on the designer...
                ShowAllGameForms(_designer.Handle);
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
        /// Set the IsDirty
        /// </summary>
        public static void SetDirty(bool dirty)
        {
            if (_designer != null && !_designer.IsDisposed)
                _designer.SetDirty(dirty);
        }

        /// <summary>
        /// Show tileset editor
        /// </summary>
        public static void ShowTileSetEditor(TileSet tileSet)
        {
            int lastX = Int32.MinValue;
            int lastY = Int32.MinValue;

            if (_tileSetEditor != null && _tileSetEditor.TileSet != tileSet)
            {
                lastX = _tileSetEditor.Left;
                lastY = _tileSetEditor.Top;

                _tileSetEditor.Close();
                _tileSetEditor = null;
            }


            if (_tileSetEditor == null || _tileSetEditor.IsDisposed)
            {
                _tileSetEditor = new TileSetEditor(tileSet);
                _tileSetEditor.Show();
                WindowHelper.LastWindowSwitch = DateTime.MinValue;

                if (lastX != Int32.MinValue)
                {
                    _tileSetEditor.Left = lastX;
                    _tileSetEditor.Top = lastY;
                }

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
            ShowAllGameForms(_tileSetEditor.Handle);
        }

        /// <summary>
        /// Show tileset editor
        /// </summary>
        public static void HideTileSetEditor()
        {
            if (_tileSetEditor != null && !_tileSetEditor.IsDisposed)
            {
                if(!_tileSetEditor.IsClosing)
                    _tileSetEditor.Close();
                _tileSetEditor = null;
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



    }
}
