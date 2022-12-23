using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
        /// Reload the form
        /// </summary>
        public void Reload()
        {
            if (_isDirty && _currentContainer != null)
            {
                DialogResult result = MessageBox.Show("Modifications not saved. Do you want to save your modifications?", "Modification", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    cboGameContentContainer.SelectedItem = _currentContainer;
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    if (!Save())
                        return;
                }
            }

            //Fetch all containers...
            _containers = GameHost.RootGameObject.FindAll<GameContentContainer>();


            if (_containers.Count > 0)
                _currentContainer = _containers[0];
            else
                _currentContainer = null;

            cboGameContentContainer.DataSource = _containers;

            ReloadGameObjectsFromContainer();
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
            }

            return true;
        }

        /// <summary>
        /// Reload the form
        /// </summary>
        public void ReloadGameObjectsFromContainer()
        {

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
                    AddGameObject(_gameObjectTypes[lstGameObjectTypes.SelectedItems[0].Index]);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Add a game object on scene
        /// </summary>
        private void AddGameObject(Type gameObjectType)
        {
            try
            {
                if (_currentContainer == null)
                {
                    MessageBox.Show("No Container selected.", "Add impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                GameObject obj = (GameObject)Activator.CreateInstance(gameObjectType);

                _currentContainer.Add(obj);

                ReloadGameObjectsFromContainer();

                cboGameObjects.SelectedItem = obj;

                SetDirty(true);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    RemoveGameObject((GameObject)cboGameObjects.SelectedItem);
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
                            EditModeHelper.ShowTileSetEditor(((TileSetRender)cboGameObjects.SelectedItem).TileSet);
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
            if (_isDirty && _currentContainer != null)
            {
                DialogResult result = MessageBox.Show("Modifications not saved. Do you want to save your modifications?", "Modification", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    cboGameContentContainer.SelectedItem = _currentContainer;
                    return;
                }

                if (result == DialogResult.Yes)
                {
                    if (!Save())
                        return;
                }
            }

            _currentContainer = (GameContentContainer)cboGameContentContainer.SelectedItem;

            ReloadGameObjectsFromContainer();

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
            if(DateTime.Now.Subtract(GameHost.InternalGame.LastTimeActivated).TotalMilliseconds > 300)
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
    }
}
