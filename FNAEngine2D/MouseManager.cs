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
            if (GameHost.InternalGameHost != null && GameHost.InternalGameHost.IsInitialized)
                GameHost.InternalGameHost.IsMouseVisible = true;
        }


        /// <summary>
        /// Hide the mouse cursor
        /// </summary>
        public static void HideMouse()
        {
            MouseManager.IsMouseVisible = false;

            //Si on a un gamehost, on va le setter...
            if (GameHost.InternalGameHost != null && GameHost.InternalGameHost.IsInitialized)
                GameHost.InternalGameHost.IsMouseVisible = false;
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

            if (gameObject.Childrens.Count > 0)
            {
                for (int index = 0; index < gameObject.Childrens.Count; index++)
                {
                    RemoveGameObject(gameObject.Childrens[index]);
                }
            }

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

            List<IMouseEventHandler> newOverGameObjects = new List<IMouseEventHandler>(_lastOverGameObjects.Count);

            ProcessGameObject(GameHost.InternalGameHost.RootGameObject, mousePosition, newOverGameObjects);

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
                if (VectorHelper.Intersects(mousePosition, gameObject.Bounds))
                {
                    newOverGameObjects.Add((IMouseEventHandler)gameObject);
                }
            }

            if (gameObject.Childrens.Count == 0)
                return;

            for (int index = 0; index < gameObject.Childrens.Count; index++)
                ProcessGameObject(gameObject.Childrens[index], mousePosition, newOverGameObjects);
        }


    }
}
