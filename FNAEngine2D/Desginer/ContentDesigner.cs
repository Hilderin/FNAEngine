using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        /// Constructor
        /// </summary>
        public ContentDesigner()
        {
            InitializeComponent();


            Reload();


            _gameObjectTypes = GameObjectTypesLoader.GetGameObjectTypes();

            foreach (Type type in _gameObjectTypes)
            {
                ListViewItem item = new ListViewItem(type.FullName);
                lstGameObjectTypes.Items.Add(item);
            }

            //propertyGrid.SelectedObject = _currentContainer;
        }

        /// <summary>
        /// Reload the form
        /// </summary>
        public void Reload()
        {
            _containers = GameHost.RootGameObject.FindAll<GameContentContainer>();

            if (_containers.Count > 0)
                _currentContainer = _containers[0];


            cboGameContentContainer.DataSource = _containers;

            ReloadFromContainer();
        }

        /// <summary>
        /// Reload the form
        /// </summary>
        public void ReloadFromContainer()
        {
            _gameObjects = _currentContainer.FindAll(o => true);
            cboGameObjects.DataSource = _gameObjects;
        }

        /// <summary>
        /// Add a GameObject by double clicking on it
        /// </summary>
        private void lstGameObjectTypes_DoubleClick(object sender, EventArgs e)
        {
            if (lstGameObjectTypes.SelectedItems.Count > 0)
                AddGameObject(_gameObjectTypes[lstGameObjectTypes.SelectedItems[0].Index]);
        }

        /// <summary>
        /// Add a game object on scene
        /// </summary>
        private void AddGameObject(Type gameObjectType)
        {
            GameObject obj = (GameObject)Activator.CreateInstance(gameObjectType);

            _currentContainer.Add(obj);

            ReloadFromContainer();

            cboGameObjects.SelectedItem = obj;

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
            if (propertyGrid.SelectedObject != null)
            {
                ((GameObject)propertyGrid.SelectedObject).RemoveAll();
                ((GameObject)propertyGrid.SelectedObject).Load();

                if (e.ChangedItem.PropertyDescriptor.Name == "Name")
                {
                    
                    object oldSelected = cboGameObjects.SelectedItem;
                    cboGameObjects.DataSource = null;
                    cboGameObjects.DataSource = _gameObjects;
                    cboGameObjects.SelectedItem = propertyGrid.SelectedObject;
                }
            }
        }
    }
}
