using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Constructor
        /// </summary>
        public ContentDesigner()
        {
            InitializeComponent();


            //Populate game object types...
            _gameObjectTypes = GameObjectTypesLoader.GetGameObjectTypes();

            foreach (Type type in _gameObjectTypes)
            {
                ListViewItem item = new ListViewItem(type.FullName);
                lstGameObjectTypes.Items.Add(item);
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
            if (_currentContainer != null)
                _gameObjects = _currentContainer.FindAll(o => true);
            else
                _gameObjects = new List<GameObject>();

            cboGameObjects.DataSource = _gameObjects;

            if (_gameObjects.Count > 0)
            {
                cboGameContentContainer.SelectedItem = _gameObjects[0];
                propertyGrid.SelectedObject = _gameObjects[0];
            }
            else
            {
                //No selected object...
                propertyGrid.SelectedObject = null;
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
                propertyGrid.SelectedObject = cboGameObjects.SelectedItem;
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

                        object oldSelected = cboGameObjects.SelectedItem;
                        cboGameObjects.DataSource = null;
                        cboGameObjects.DataSource = _gameObjects;
                        cboGameObjects.SelectedItem = oldSelected;
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
        private void SetDirty(bool dirty)
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
                string fullPath = Path.Combine(GameHost.InternalGameHost.ContentManager.RootDirectory, _currentContainer.AssetName + ".json").Replace('\\', '/');

                //Disable watching to prevent double reloading...
                //ContentWatcher.Enabled = false;

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
            //finally
            //{
            //    ContentWatcher.Enabled = true;
            //}
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
    }
}
