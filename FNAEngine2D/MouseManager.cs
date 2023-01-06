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
    public class MouseManager
    {
        /// <summary>
        /// Game
        /// </summary>
        private Game _game;

        /// <summary>
        /// Indique si la souris est visible
        /// </summary>
        public bool IsMouseVisible { get; private set; }

        /// <summary>
        /// Liste des derniers games objects où on était par dessus
        /// </summary>
        private List<IMouseEventHandler> _lastOverGameObjects = new List<IMouseEventHandler>();

        /// <summary>
        /// Objects with mouse left down
        /// </summary>
        private List<IMouseEventHandler> _mouseLeftDownObjects = new List<IMouseEventHandler>();

        /// <summary>
        /// Objects with mouse right down
        /// </summary>
        private List<IMouseEventHandler> _mouseRightDownObjects = new List<IMouseEventHandler>();

        /// <summary>
        /// Constructor
        /// </summary>
        public MouseManager(Game game)
        {
            _game = game;
        }

        /// <summary>
        /// Display the mouse cursor
        /// </summary>
        public void ShowMouse()
        {
            IsMouseVisible = true;

            //Si on a un gamehost, on va le setter...
            if (_game != null && _game.IsInitialized)
                _game.IsMouseVisible = true;
        }


        /// <summary>
        /// Hide the mouse cursor
        /// </summary>
        public void HideMouse()
        {
            IsMouseVisible = false;

            //Si on a un gamehost, on va le setter...
            if (_game != null && _game.IsInitialized)
                _game.IsMouseVisible = false;
        }

        /// <summary>
        /// Process the removal of a game object
        /// </summary>
        internal void RemoveGameObject(GameObject gameObject)
        {
            if (gameObject is IMouseEventHandler)
            {
                _lastOverGameObjects.Remove((IMouseEventHandler)gameObject);
                _mouseLeftDownObjects.Remove((IMouseEventHandler)gameObject);
                _mouseRightDownObjects.Remove((IMouseEventHandler)gameObject);
            }

            //Also remove for childrens...
            gameObject.ForEachChild(o => RemoveGameObject(o));

        }

        /// <summary>
        /// Permet de processer les mouses events
        /// </summary>
        internal void ProcessMouseEvents()
        {

            if (!IsMouseVisible)
                return;

            //We will need the mouse position...
            Vector2 mousePosition = _game.Input.GetMousePosition();

            List<IMouseEventHandler> newOverGameObjects = new List<IMouseEventHandler>(_lastOverGameObjects.Count);

            ProcessGameObject(_game.RootGameObject, mousePosition, newOverGameObjects);

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

                            if (_game.Input.IsMouseLeftDown())
                            {
                                newOverGameObjects[index].HandleMouseEvent(MouseAction.LeftButtonDown);
                                _mouseLeftDownObjects.Add(newOverGameObjects[index]);
                            }
                            if (_game.Input.IsMouseRightDown())
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
                            if (_game.Input.IsMouseLeftNewDown())
                            {
                                _lastOverGameObjects[index].HandleMouseEvent(MouseAction.LeftButtonDown);
                                _mouseLeftDownObjects.Add(_lastOverGameObjects[index]);
                            }
                            if (_game.Input.IsMouseRightNewDown())
                            {
                                _lastOverGameObjects[index].HandleMouseEvent(MouseAction.RightButtonDown);
                                _mouseRightDownObjects.Add(_lastOverGameObjects[index]);
                            }
                        }
                    }
                }

                //Left Clicked... ??
                if (_game.Input.IsMouseLeftClicked())
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
                if (_game.Input.IsMouseRightClicked())
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
        private void ProcessGameObject(GameObject gameObject, Vector2 mousePosition, List<IMouseEventHandler> newOverGameObjects)
        {
            if (gameObject is IMouseEventHandler)
            {
                Camera camera = _game.GetCameraForObject(gameObject);
                
                if (VectorHelper.Intersects(mousePosition + camera.Location.Substract(camera.ViewLocation), gameObject.Bounds))
                {
                    newOverGameObjects.Add((IMouseEventHandler)gameObject);
                }
            }

            gameObject.ForEachChild(o => ProcessGameObject(o, mousePosition, newOverGameObjects));
        }


    }
}
