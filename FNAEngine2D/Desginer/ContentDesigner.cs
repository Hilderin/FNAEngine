﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAEngine2D.Desginer
{
    public partial class ContentDesigner : Form
    {

        /// <summary>
        /// Types of all game object
        /// </summary>
        private List<Type> _gameObjectTypes;

        /// <summary>
        /// Containers on screen
        /// </summary>
        private List<GameContentContainer> _containers;

        /// <summary>
        /// CurrentContainer
        /// </summary>
        private GameContentContainer _currentContainer;

        /// <summary>
        /// GameObjects
        /// </summary>
        private List<GameObject> _gameObjects = new List<GameObject>();

        /// <summary>
        /// Form is dirty?
        /// </summary>
        private bool _isDirty = false;

        /// <summary>
        /// Loading in progress
        /// </summary>
        private bool _loadingSelectedGameObject = true;

        /// <summary>
        /// History
        /// </summary>
        private List<HistoryState> _history = new List<HistoryState>();

        /// <summary>
        /// History index
        /// </summary>
        private int _historyIndex = -1;

        /// <summary>
        /// Preview object for adding
        /// </summary>
        private GameObject _previewAddObject = null;

        /// <summary>
        /// Preview object for editing
        /// </summary>
        private GameObject _previewEditObject = null;


        /// <summary>
        /// Selected game object type
        /// </summary>
        private Type _selectedGameObjectType = null;

        /// <summary>
        /// Texture render for the preview
        /// </summary>
        private TextureRender _previewTextureRender = new TextureRender("pixel", new Microsoft.Xna.Framework.Rectangle(0, 0, 0, 0), Microsoft.Xna.Framework.Color.White * 0.5f);


        /// <summary>
        /// Constructor
        /// </summary>
        public ContentDesigner()
        {
            InitializeComponent();

            //Enabling the edit mode...
            EditModeHelper.EditMode = true;

            btnPausePlay.Text = EditModeHelper.IsGameRunning ? "&Pause" : "&Play";


            //Populate game object types...
            _gameObjectTypes = new List<Type>();
            foreach (Type type in GameObjectTypesLoader.GetGameObjectTypes())
            {
                //Certain types cannot be used in the designer...
                if (type != typeof(TileGameObject) && type != typeof(GameContentContainer))
                {
                    ListViewItem item = new ListViewItem(type.FullName);
                    lstGameObjectTypes.Items.Add(item);
                    _gameObjectTypes.Add(type);
                }
            }

            //Load the components...
            Reload();

        }

        /// <summary>
        /// Showing preview
        /// </summary>
        public void ShowPreview(int x, int y)
        {
            try
            {
                if (_currentContainer == null || _currentContainer.RootGameObject == null)
                    return;

                if (_selectedGameObjectType != null)
                {
                    //Preview object creation...
                    try
                    {
                        if (_previewAddObject == null || _previewAddObject.GetType() != _selectedGameObjectType)
                        {
                            if (_previewAddObject != null)
                                HideAddPreview();

                            _previewAddObject = (GameObject)Activator.CreateInstance(_selectedGameObjectType);

                            _currentContainer.RootGameObject.Add(_previewAddObject);

                        }

                        _previewAddObject.Location = new Microsoft.Xna.Framework.Vector2(x, y);
                    }
                    catch
                    {
                        //Hiding preview and removing selection...
                        HideAddPreview();
                        lstGameObjectTypes.SelectedItems.Clear();

                        throw;
                    }
                }
                
                if(EditModeHelper.SelectedGameObject != null)
                {
                    if (!(EditModeHelper.SelectedGameObject is TileSetRender))
                    {
                        if (_previewEditObject == null)
                        {
                            _previewEditObject = _previewTextureRender;
                            _currentContainer.RootGameObject.Add(_previewEditObject);

                        }

                        //Camera camera = GetCameraForLayer(EditModeHelper.SelectedGameObject.LayerMask);
                        _previewEditObject.Bounds = GetGameObjectBounds(EditModeHelper.SelectedGameObject);
                        _previewEditObject.LayerMask = EditModeHelper.SelectedGameObject.LayerMask;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }

        /// <summary>
        /// Get the bounds of a game object for the preview
        /// </summary>
        private Microsoft.Xna.Framework.Rectangle GetGameObjectBounds(GameObject gameObject)
        {
            if (gameObject.Width != 0 && gameObject.Height != 0)
                return gameObject.Bounds;

            Microsoft.Xna.Framework.Rectangle bounds = gameObject.Bounds;
            UpdateGameObjectBounds(gameObject, ref bounds);
            return bounds;

        }

        /// <summary>
        /// Get the bounds of a game object for the preview
        /// </summary>
        private void UpdateGameObjectBounds(GameObject gameObject, ref Microsoft.Xna.Framework.Rectangle bounds)
        {
            if (gameObject.X < bounds.X)
                bounds.X = (int)gameObject.X;
            if (gameObject.Y < bounds.Y)
                bounds.Y = (int)gameObject.Y;

            if (bounds.Width < gameObject.Width)
                bounds.Width = (int)gameObject.Width;
            if (bounds.Height < gameObject.Height)
                bounds.Height = (int)gameObject.Height;

            if (gameObject.NbChildren > 0)
            {
                for (int index = 0; index < gameObject.NbChildren; index++)
                    UpdateGameObjectBounds(gameObject.Get(index), ref bounds);
            }

        }

        /// <summary>
        /// Get the camera to use for a layermask
        /// </summary>
        private Camera GetCameraForLayer(Layers layerMask)
        {
            if (GameHost.ExtraCameras.Count == 0)
                return GameHost.MainCamera;

            if ((GameHost.MainCamera.LayerMask & layerMask) != 0)
                return GameHost.MainCamera;

            if (GameHost.ExtraCameras.Count > 0)
            {
                foreach (Camera camera in GameHost.ExtraCameras)
                {
                    if ((camera.LayerMask & layerMask) != 0)
                        return camera;
                }
            }

            return GameHost.MainCamera;

        }

        /// <summary>
        /// Hide preview
        /// </summary>
        public void HidePreview()
        {
            HideAddPreview();
            HideEditPreview();
        }

        /// <summary>
        /// Hide preview
        /// </summary>
        private void HideAddPreview()
        {
            if (_previewAddObject != null && _currentContainer != null && _currentContainer.RootGameObject != null)
            {
                _currentContainer.RootGameObject.Remove(_previewAddObject);
            }

            _previewAddObject = null;
        }

        /// <summary>
        /// Hide preview
        /// </summary>
        private void HideEditPreview()
        {
            if (_previewEditObject != null && _currentContainer != null && _currentContainer.RootGameObject != null)
            {
                _currentContainer.RootGameObject.Remove(_previewEditObject);
            }

            _previewEditObject = null;
        }

        /// <summary>
        /// Remove selection
        /// </summary>
        public void ClearSelection()
        {
            if (lstGameObjectTypes.SelectedItems.Count > 0)
            {
                lstGameObjectTypes.SelectedItems[0].Selected = false;
            }
        }

        /// <summary>
        /// The user has click in the game
        /// </summary>
        public void OnMouseLeftClickInGame(int x, int y)
        {
            if (_previewAddObject != null && _selectedGameObjectType != null)
            {
                //We have a selected object, we will add it...
                GameObject newGameObject = AddGameObject(_selectedGameObjectType);

                if (newGameObject != null)
                    newGameObject.TranslateTo(x, y);
            }
            else
            {
                //We dont have anything selected...
                List<GameObject> selectObjects = GetGameObjectsAtCoord(x, y);
                if (selectObjects.Count > 0)
                {
                    //Already selected?
                    if (EditModeHelper.SelectedGameObject != null && selectObjects.Contains(EditModeHelper.SelectedGameObject))
                        return;

                    //Change selection to the last game...
                    cboGameObjects.SelectedItem = selectObjects[selectObjects.Count - 1];
                }
            }

        }

        /// <summary>
        /// The user has click in the game
        /// </summary>
        public void OnMouseRightClickInGame(int x, int y)
        {
            List<GameObject> selectObjects = GetGameObjectsAtCoord(x, y);
            if (selectObjects.Count > 0)
            {
                RemoveGameObject(selectObjects[selectObjects.Count - 1]);
            }
        }

        /// <summary>
        /// Reload the form
        /// </summary>
        public void Reload()
        {
            //if (_isDirty && _currentContainer != null)
            //{
            //    DialogResult result = MessageBox.Show("Modifications not saved. Do you want to save your modifications?", "Modification", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            //    if (result == DialogResult.Cancel)
            //    {
            //        cboGameContentContainer.SelectedItem = _currentContainer;
            //        return;
            //    }

            //    if (result == DialogResult.Yes)
            //    {
            //        if (!Save())
            //            return;
            //    }
            //}

            //Fetch all containers...
            _containers = GameHost.RootGameObject.FindAll<GameContentContainer>();

            _previewEditObject = null;
            _previewAddObject = null;


            if (_containers.Count > 0)
                _currentContainer = _containers[0];
            else
                _currentContainer = null;

            SetDirty(false);

            cboGameContentContainer.DataSource = _containers;
            
            ReloadGameObjectsFromContainer();

            ResetHistory();

            SetTileSetEditorAfterReload();

        }


        /// <summary>
        /// Update the tile set editor after reloading
        /// </summary>
        private void SetTileSetEditorAfterReload()
        {
            //Reopening the TileSetEditor...
            if (EditModeHelper.IsTileSetEditorOpened)
            {
                if (cboGameObjects.SelectedItem is TileSetRender)
                    //Reopenning the tileset editor for the new tileset...
                    EditModeHelper.ShowTileSetEditor((TileSetRender)cboGameObjects.SelectedItem, false);
                else
                    EditModeHelper.HideTileSetEditor();
            }
        }

        /// <summary>
        /// Ask the user to confirm the changes before closing the form
        /// </summary>
        public bool ConfirmBeforeClose(bool canCancel = true)
        {
            if (_isDirty && _currentContainer != null)
            {
                DialogResult result = MessageBox.Show("Modifications not saved. Do you want to save your modifications?", "Modification", canCancel ? MessageBoxButtons.YesNoCancel : MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    return false;
                }

                if (result == DialogResult.Yes)
                {
                    if (!Save())
                    {
                        return false;
                    }
                }
                else
                {
                    //Revert back ti the original...
                    RevertToHistory(_history[0]);
                }
            }

            //Fake that it's not dirty so the question will not be asked again.
            SetDirty(false);

            return true;
        }

        /// <summary>
        /// Reload the form
        /// </summary>
        public void ReloadGameObjectsFromContainer()
        {

            HideEditPreview();

            _gameObjects = new List<GameObject>();
            if (_currentContainer != null)
            {
                //We add only objet in the root of container
                for (int index = 0; index < _currentContainer.NbChildren; index++)
                {
                    _gameObjects.Add(_currentContainer.Get(index));
                }
            }

            try
            {
                _loadingSelectedGameObject = true;
                cboGameObjects.DataSource = _gameObjects;


            }
            finally
            {
                _loadingSelectedGameObject = false;
            }

            if (_gameObjects.Count > 0)
            {
                cboGameContentContainer.SelectedItem = _gameObjects[0];
                propertyGrid.SelectedObject = _gameObjects[0];
                EditModeHelper.SelectedGameObject = _gameObjects[0];
            }
            else
            {
                //No selected object...
                propertyGrid.SelectedObject = null;
                EditModeHelper.SelectedGameObject = null;
            }
        }

        /// <summary>
        /// Add a GameObject by double clicking on it
        /// </summary>
        private void lstGameObjectTypes_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lstGameObjectTypes.SelectedItems.Count > 0)
                {
                    AddGameObject(_gameObjectTypes[lstGameObjectTypes.SelectedItems[0].Index]);
                    AddHistory();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Add a game object on scene
        /// </summary>
        private GameObject AddGameObject(Type gameObjectType)
        {
            try
            {
                if (_currentContainer == null)
                {
                    MessageBox.Show("No Container selected.", "Add impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }


                GameObject obj = (GameObject)Activator.CreateInstance(gameObjectType);

                _currentContainer.Add(obj);

                ReloadGameObjectsFromContainer();

                cboGameObjects.SelectedItem = obj;

                SetTileSetEditorAfterReload();

                SetDirty(true);

                return obj;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

        }

        /// <summary>
        /// Delete a gameobject
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboGameObjects.SelectedItem != null)
                {
                    RemoveGameObject((GameObject)cboGameObjects.SelectedItem);
                    AddHistory();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Add a game object on scene
        /// </summary>
        private void RemoveGameObject(GameObject gameObject)
        {
            try
            {
                if (_currentContainer == null)
                {
                    MessageBox.Show("No Container selected.", "Remove impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                _currentContainer.Remove(gameObject);

                ReloadGameObjectsFromContainer();

                SetTileSetEditorAfterReload();

                SetDirty(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Update the selected game obects
        /// </summary>
        private void cboGameObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGameObjects.SelectedItem != null)
            {
                propertyGrid.SelectedObject = cboGameObjects.SelectedItem;
                EditModeHelper.SelectedGameObject = (GameObject)cboGameObjects.SelectedItem;
                
                if (!_loadingSelectedGameObject)
                {
                    if (EditModeHelper.IsTileSetEditorOpened)
                    {
                        if (cboGameObjects.SelectedItem is TileSetRender)
                            //Reopenning the tileset editor for the new tileset...
                            EditModeHelper.ShowTileSetEditor((TileSetRender)cboGameObjects.SelectedItem, false);
                        else
                            EditModeHelper.HideTileSetEditor();
                    }
                }
            }
            else
            {
                //No selection...
                propertyGrid.SelectedObject = null;
                EditModeHelper.SelectedGameObject = null;
                HideEditPreview();

                if (!_loadingSelectedGameObject)
                {
                    if (EditModeHelper.IsTileSetEditorOpened)
                    {
                        EditModeHelper.HideTileSetEditor();
                    }
                }
            }
        }

        /// <summary>
        /// On property changed
        /// </summary>
        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                if (propertyGrid.SelectedObject != null)
                {
                    ((GameObject)propertyGrid.SelectedObject).RemoveAll();
                    ((GameObject)propertyGrid.SelectedObject).Load();

                    if (e.ChangedItem.PropertyDescriptor.Name == "Name")
                    {
                        try
                        {
                            _loadingSelectedGameObject = true;
                            object oldSelected = cboGameObjects.SelectedItem;
                            cboGameObjects.DataSource = null;
                            cboGameObjects.DataSource = _gameObjects;
                            cboGameObjects.SelectedItem = oldSelected;
                        }
                        finally
                        {
                            _loadingSelectedGameObject = false;
                        }
                    }

                    SetDirty(true);
                    AddHistory();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Save the content
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        /// <summary>
        /// Set the IsDirty
        /// </summary>
        public void SetDirty(bool dirty)
        {

            _isDirty = dirty;

            if (dirty)
                this.Text = this.Text.Replace(" *", String.Empty) + " *";
            else
                this.Text = this.Text.Replace(" *", String.Empty);

        }

        /// <summary>
        /// Save the form
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {

            try
            {
                if (_currentContainer == null)
                {
                    MessageBox.Show("No Container selected.", "Save impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }


                //Saving....
                string fullPath = Path.Combine(GameHost.InternalGame.ContentManager.RootDirectory, _currentContainer.AssetName + ".json").Replace('\\', '/');

                //Disable watching to prevent double reloading...
                ContentWatcher.Enabled = false;

                //Creation of the content...
                GameContent gameContent = _currentContainer.GameContent.Data;
                gameContent.Objects.Clear();
                foreach (GameObject gameObject in _gameObjects)
                {
                    gameContent.Objects.Add(GameContentManager.GetGameContentObject(gameObject));
                }


                //And we serialize...
                var serializer = new JsonSerializer();

                serializer.Formatting = Formatting.Indented;
                serializer.DefaultValueHandling = DefaultValueHandling.Ignore;

                using (Stream stream = File.Create(fullPath))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        using (JsonTextWriter jsonwriter = new JsonTextWriter(writer))
                        {
                            serializer.Serialize(jsonwriter, gameContent);
                        }
                    }
                }

                SetDirty(false);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                ContentWatcher.Enabled = true;
            }
        }

        /// <summary>
        /// Selection changed
        /// </summary>
        private void cboGameContentContainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ConfirmBeforeClose(true))
            {
                //Reselect the container...
                cboGameContentContainer.SelectedItem = _currentContainer;
            }


            _currentContainer = (GameContentContainer)cboGameContentContainer.SelectedItem;

            ReloadGameObjectsFromContainer();

            ResetHistory();

            SetTileSetEditorAfterReload();

        }

        /// <summary>
        /// Closing form
        /// </summary>
        private void ContentDesigner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ConfirmBeforeClose())
                e.Cancel = true;
        }

        /// <summary>
        /// Form closed
        /// </summary>
        private void ContentDesigner_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Disabling the edit mode...
            EditModeHelper.EditMode = false;
        }

        /// <summary>
        /// Resume or pause the game in edit mode
        /// </summary>
        private void btnPausePlay_Click(object sender, EventArgs e)
        {
            EditModeHelper.IsGameRunning = !EditModeHelper.IsGameRunning;
            btnPausePlay.Text = EditModeHelper.IsGameRunning ? "&Pause" : "&Play";

            //Win32.SetForegroundWindow(GameHost.InternalGame.GameWindowHandle);


        }

        /// <summary>
        /// Activation of the window
        /// </summary>
        private void ContentDesigner_Activated(object sender, EventArgs e)
        {
            //Reopening the designer forms and refocusing on ourself...
            if (EditModeHelper.IsReshowWindowNeeded())
                EditModeHelper.ShowDesigner(this.Handle);

        }

        /// <summary>
        /// Process dialog keys
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F12)
            {
                //Hide the designer...
                EditModeHelper.HideDesigner();

                //Refocusing on 
                Win32.SetForegroundWindow(GameHost.InternalGame.GameWindowHandle);

                return true;
            }
            


            return base.ProcessDialogKey(keyData);
        }


        /// <summary>
        /// Update the last time a form is active
        /// </summary>
        private void tmrUpdateLastActive_Tick(object sender, EventArgs e)
        {
            if (Form.ActiveForm != null)
                EditModeHelper.LastTimeActivated = DateTime.Now;
        }

        /// <summary>
        /// Reset the history
        /// </summary>
        private void ResetHistory()
        {
            _history.Clear();
            _historyIndex = -1;

            AddHistory();
        }

        /// <summary>
        /// Add a state in the history
        /// </summary>
        public void AddHistory()
        {
            _historyIndex++;

            //Removing the futur actions...
            while (_history.Count > _historyIndex)
                _history.RemoveAt(_history.Count - 1);

            HistoryState history = new HistoryState();

            GameContent gameContent = new GameContent();
            foreach (GameObject gameObject in _gameObjects)
            {
                gameContent.Objects.Add(GameContentManager.GetGameContentObject(gameObject));
            }

            history.JsonGameContent = JsonConvert.SerializeObject(gameContent);
            history.GameObjectSelectedIndex = cboGameObjects.SelectedIndex;

            _history.Add(history);
        }

        /// <summary>
        /// Undo the last modification
        /// </summary>
        private void btnUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        /// <summary>
        /// Undo the last modification
        /// </summary>
        private void Undo()
        {
            try
            {
                if (_historyIndex < 1)
                    return;

                HistoryState history = _history[_historyIndex - 1];

                _historyIndex--;

                RevertToHistory(history);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Revert all modifications
        /// </summary>
        public void Revert()
        {
            if(_history.Count > 1)
                RevertToHistory(_history[0]);
        }

        /// <summary>
        /// Revert back the content to an history
        /// </summary>
        private void RevertToHistory(HistoryState history)
        {
            //Recreating the game content...
            GameContent gameContent = JsonConvert.DeserializeObject<GameContent>(history.JsonGameContent);


            GameContentManager.ReplaceContent(_currentContainer, gameContent);

            ReloadGameObjectsFromContainer();

            try
            {
                cboGameObjects.SelectedItem = history.GameObjectSelectedIndex;
            }
            catch { }

            SetTileSetEditorAfterReload();
        }

        /// <summary>
        /// Selected game object type changed
        /// </summary>
        private void lstGameObjectTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGameObjectTypes.SelectedItems.Count > 0)
            {
                _selectedGameObjectType = _gameObjectTypes[lstGameObjectTypes.SelectedItems[0].Index];
            }
            else
            {
                _selectedGameObjectType = null;
                HideAddPreview();
            }
        }


        /// <summary>
        /// Clear the selection
        /// </summary>
        private void lnkClearSelection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lstGameObjectTypes.SelectedItems.Clear();
        }



        /// <summary>
        /// Get game objects at a coord
        /// </summary>
        private List<GameObject> GetGameObjectsAtCoord(int x, int y)
        {
            List<GameObject> list = new List<GameObject>();

            if (_currentContainer != null)
            {
                Microsoft.Xna.Framework.Vector2 coord = new Microsoft.Xna.Framework.Vector2(x, y);
                coord -= GameHost.MainCamera.Location.Substract(GameHost.MainCamera.ViewLocation);
                list.AddRange(_currentContainer.FindAll(o => IsObjectAtCoord(o, coord)));
            }

            return list;
        }

        /// <summary>
        /// Check if a object is at a coord
        /// </summary>
        private bool IsObjectAtCoord(GameObject gameObject, Microsoft.Xna.Framework.Vector2 coord)
        {
            if (!_gameObjects.Contains(gameObject))
                return false;

            //We need the camera for the offset location...
            Camera camera = GetCameraForLayer(gameObject.LayerMask);

            return VectorHelper.Intersects(coord + camera.Location, GetGameObjectBounds(gameObject));
        }

        /// <summary>
        /// HistoryState
        /// </summary>
        private class HistoryState
        {
            public int GameObjectSelectedIndex { get; set; }
            public string JsonGameContent { get; set; }
        }

    }
}
