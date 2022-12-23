using FNAEngine2D.Desginer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        /// Process the Update loop in edit mode
        /// </summary>
        public static void ProcessUpdateEditMode()
        {
            if (!GameHost.InternalGameHost.IsActive)
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
                if (TileSetEditorForm.Current != null)
                {
                    Vector2 mousePosition = Input.MousePosition();
                    if(Input.MouseLeftDown())
                        TileSetEditorForm.Current.OnMouseLeftClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                    if (Input.MouseRightDown())
                        TileSetEditorForm.Current.OnMouseRightClickInGame((int)mousePosition.X, (int)mousePosition.Y);
                }
            }

        }

    }
}
