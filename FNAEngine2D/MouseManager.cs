using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Manage the mouse
    /// </summary>
    public static class MouseManager
    {

        /// <summary>
        /// Indique si la souris est visible
        /// </summary>
        public static bool IsMouseVisible { get; private set; }

        /// <summary>
        /// Liste des derniers games objects où on était par dessus
        /// </summary>
        private static List<IMouseEventHandler> _lastOverGameObjects = new List<IMouseEventHandler>();

        /// <summary>
        /// Objects with mouse left down
        /// </summary>
        private static List<IMouseEventHandler> _mouseLeftDownObjects = new List<IMouseEventHandler>();

        /// <summary>
        /// Objects with mouse right down
        /// </summary>
        private static List<IMouseEventHandler> _mouseRightDownObjects = new List<IMouseEventHandler>();

        /// <summary>
        /// Display the mouse cursor
        /// </summary>
        public static void ShowMouse()
        {
            MouseManager.IsMouseVisible = true;

            //Si on a un gamehost, on va le setter...
            if (GameHost.InternalGame != null && GameHost.InternalGame.IsInitialized)
                GameHost.InternalGame.IsMouseVisible = true;
        }


        /// <summary>
        /// Hide the mouse cursor
        /// </summary>
        public static void HideMouse()
        {
            MouseManager.IsMouseVisible = false;

            //Si on a un gamehost, on va le setter...
            if (GameHost.InternalGame != null && GameHost.InternalGame.IsInitialized)
                GameHost.InternalGame.IsMouseVisible = false;
        }

        /// <summary>
        /// Process the removal of a game object
        /// </summary>
        internal static void RemoveGameObject(GameObject gameObject)
        {
            if (gameObject is IMouseEventHandler)
            {
                _lastOverGameObjects.Remove((IMouseEventHandler)gameObject);
                _mouseLeftDownObjects.Remove((IMouseEventHandler)gameObject);
                _mouseRightDownObjects.Remove((IMouseEventHandler)gameObject);
            }

            //Also remove for childrens...
            gameObject.ForEach(o => RemoveGameObject(o));

        }

        /// <summary>
        /// Permet de processer les mouses events
        /// </summary>
        internal static void ProcessMouseEvents()
        {

            if (!IsMouseVisible)
                return;

            //We will need the mouse position...
            Vector2 mousePosition = Input.MousePosition();

            //We remove the main camera because it's been added, we will rehad it in the IsObjectAtCoord...
            mousePosition -= GameHost.MainCamera.Location.Substract(GameHost.MainCamera.ViewLocation);

            List<IMouseEventHandler> newOverGameObjects = new List<IMouseEventHandler>(_lastOverGameObjects.Count);

            ProcessGameObject(GameHost.InternalGame.RootGameObject, mousePosition, newOverGameObjects);

            if (newOverGameObjects.Count > 0 || _lastOverGameObjects.Count > 0)
            {
                //Have something to do...

                //Process the enters....
                if (newOverGameObjects.Count > 0)
                {
                    for (int index = 0; index < newOverGameObjects.Count; index++)
                    {
                        if (!_lastOverGameObjects.Contains(newOverGameObjects[index]))
                        {
                            //Enter...
                            newOverGameObjects[index].HandleMouseEvent(MouseAction.Enter);

                            if (Input.MouseLeftDown())
                            {
                                newOverGameObjects[index].HandleMouseEvent(MouseAction.LeftButtonDown);
                                _mouseLeftDownObjects.Add(newOverGameObjects[index]);
                            }
                            if (Input.MouseRightDown())
                            {
                                newOverGameObjects[index].HandleMouseEvent(MouseAction.RightButtonDown);
                                _mouseRightDownObjects.Add(newOverGameObjects[index]);
                            }
                        }
                    }
                }

                //Process the leave....
                if (_lastOverGameObjects.Count > 0)
                {
                    for (int index = 0; index < _lastOverGameObjects.Count; index++)
                    {
                        if (!newOverGameObjects.Contains(_lastOverGameObjects[index]))
                        {
                            //Leave...
                            _lastOverGameObjects[index].HandleMouseEvent(MouseAction.Leave);
                        }
                        else
                        {
                            //Object was already there...
                            if (Input.MouseLeftNewDown())
                            {
                                _lastOverGameObjects[index].HandleMouseEvent(MouseAction.LeftButtonDown);
                                _mouseLeftDownObjects.Add(_lastOverGameObjects[index]);
                            }
                            if (Input.MouseRightNewDown())
                            {
                                _lastOverGameObjects[index].HandleMouseEvent(MouseAction.RightButtonDown);
                                _mouseRightDownObjects.Add(_lastOverGameObjects[index]);
                            }
                        }
                    }
                }

                //Left Clicked... ??
                if (Input.MouseLeftClicked())
                {
                    for (int index = 0; index < newOverGameObjects.Count; index++)
                    {
                        //Up...
                        newOverGameObjects[index].HandleMouseEvent(MouseAction.LeftButtonUp);
                    }

                    for (int index = 0; index < _mouseLeftDownObjects.Count; index++)
                    {
                        //Clicked...
                        _mouseLeftDownObjects[index].HandleMouseEvent(MouseAction.LeftButtonClicked);
                    }
                }

                //Right Clicked... ??
                if (Input.MouseRightClicked())
                {
                    for (int index = 0; index < newOverGameObjects.Count; index++)
                    {
                        //Up...
                        newOverGameObjects[index].HandleMouseEvent(MouseAction.RightButtonUp);
                    }

                    for (int index = 0; index < _mouseRightDownObjects.Count; index++)
                    {
                        //Clicked...
                        _mouseRightDownObjects[index].HandleMouseEvent(MouseAction.RightButtonClicked);
                    }
                }


                //Keep last...
                _lastOverGameObjects = newOverGameObjects;
            }

        }

        /// <summary>
        /// Process the game objects
        /// </summary>
        private static void ProcessGameObject(GameObject gameObject, Vector2 mousePosition, List<IMouseEventHandler> newOverGameObjects)
        {
            if (gameObject is IMouseEventHandler)
            {
                Camera camera = GameHost.GetCameraForObject(gameObject);
                
                if (VectorHelper.Intersects(mousePosition + camera.Location, gameObject.Bounds))
                {
                    newOverGameObjects.Add((IMouseEventHandler)gameObject);
                }
            }

            gameObject.ForEach(o => ProcessGameObject(o, mousePosition, newOverGameObjects));
        }


    }
}
