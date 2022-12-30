using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FNAEngine2D
{
    /// <summary>
    /// Helper for GameContent management
    /// </summary>
    public static class GameContentManager
    {
        /// <summary>
        /// Game objects per asset names
        /// </summary>
        private static Dictionary<string, List<GameObject>> _gameObjectsPerAssetName = new Dictionary<string, List<GameObject>>();

        /// <summary>
        /// Game objects to reload
        /// </summary>
        private static Dictionary<GameObject, List<string>> _gameObjectsToReload = new Dictionary<GameObject, List<string>>();


        /// <summary>
        /// Static constructor
        /// </summary>
        static GameContentManager()
        {
            
        }


        /// <summary>
        /// Apply game content in the game obect
        /// </summary>
        public static GameObject Apply(GameObject gameObject, string assetName)
        {
            return Apply(gameObject, assetName, false);
        }

        /// <summary>
        /// Apply game content in the game obect
        /// </summary>
        private static GameObject Apply(GameObject gameObject, string assetName, bool fromRefresh)
        {
            var content = GameHost.GetContent<GameContent>(assetName);

            //We all in a container to facilitate the update...
            GameContentContainer container = GetContainer(gameObject, assetName);

            ReplaceContent(container, content.Data);

            if (!fromRefresh)
                AddGameObjectUsage(gameObject, assetName);

            return container;

        }

        /// <summary>
        /// Get the container or create it
        /// </summary>
        private static GameContentContainer GetContainer(GameObject gameObject, string assetName)
        {
            GameContentContainer container = gameObject.Find<GameContentContainer>(o => o.AssetName == assetName);

            if (container != null)
                return container;

            //Not found, we must add it...
            return gameObject.Add(new GameContentContainer() { AssetName = assetName });
        }


        /// <summary>
        /// Apply game content in the game object
        /// </summary>
        public static void ReplaceContent(GameContentContainer container, GameContent gameContent)
        {
            //We have content?
            if (gameContent.Objects == null || gameContent.Objects.Count == 0)
                return;

            //Applying...
            for (int index = 0; index < gameContent.Objects.Count; index++)
            {
                GameContentObject childToCreate = gameContent.Objects[index];

                GameObject childExist = null;
                if(container.NbChildren > index)
                    childExist = container.Get(index);

                GameObject objToUse = null;
                bool isNew = false;

                //This game object is ok??
                if (childExist != null && childExist.GetType().Name == childToCreate.ClassName)
                {
                    objToUse = childExist;

                    //Applying properties....
                    ApplyGameObjectProperties(objToUse, childToCreate);
                }
                else
                {
                    //Create a new one...
                    if(childExist != null)
                        container.Remove(childExist);

                    objToUse = CreateGameObject(childToCreate);
                    isNew = true;
                }

                

                if (isNew)
                {
                    container.Insert(index, objToUse);
                }
                else
                {
                    objToUse.RemoveAll();
                    objToUse.DoLoad();
                }
            }

            //Removing extra objets...
            while (container.NbChildren > gameContent.Objects.Count)
                container.RemoveAt(gameContent.Objects.Count);


        }


        /// <summary>
        /// Update game object if needed
        /// </summary>
        internal static void ReloadModifiedContent(GameObject gameObject)
        {

            if (GameHost.DevelopmentMode)
            {

                try
                {
                    if (_gameObjectsToReload.Count == 0)
                        return;

                    if (_gameObjectsToReload.TryGetValue(gameObject, out List<string> assetNames))
                    {
                        foreach (string assetName in assetNames)
                        {
                            Apply(gameObject, assetName, true);
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                _gameObjectsToReload.Remove(gameObject);
            }

        }

        /// <summary>
        /// Add a gameobject in the usage list
        /// </summary>
        private static void AddGameObjectUsage(GameObject gameObject, string assetName)
        {
            List<GameObject> gameObjects;

            if (!_gameObjectsPerAssetName.TryGetValue(assetName, out gameObjects))
            {
                gameObjects = new List<GameObject>();
                _gameObjectsPerAssetName[assetName] = gameObjects;
            }

            gameObjects.Add(gameObject);

        }

        /// <summary>
        /// Create the game object...
        /// </summary>
        public static GameObject CreateGameObject(GameContentObject contentObj)
        {

            Type type = GameObjectTypesLoader.GetGameObjectType(contentObj.ClassName);

            
            GameObject obj = (GameObject)Activator.CreateInstance(type);

            //Applying properties....
            ApplyGameObjectProperties(obj, contentObj);

            return obj;

        }


        /// <summary>
        /// Create the game object...
        /// </summary>
        private static void ApplyGameObjectProperties(GameObject gameObject, GameContentObject obj)
        {
            if (obj.Props == null)
                return;

            Type type = gameObject.GetType();

            string json = JsonConvert.SerializeObject(obj.Props);
            object newObj = JsonConvert.DeserializeObject(json, type);

            //Copy values...
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (!prop.CanWrite)
                    continue;

                if (prop.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                    continue;

                //Copy of value....
                prop.SetValue(gameObject, prop.GetValue(newObj));

            }

        }

        /// <summary>
        /// Get the GameContentObject from a GameObject
        /// </summary>
        public static GameContentObject GetGameContentObject(GameObject gameObject)
        {
            Type type = gameObject.GetType();


            GameContentObject gameContentObject = new GameContentObject();

            gameContentObject.ClassName = type.Name;

            gameContentObject.Props = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(gameObject, Formatting.None, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            }));

            return gameContentObject;

        }

        /// <summary>
        /// Color creation...
        /// </summary>
        private static Color CreateColor(string value)
        {
            PropertyInfo propColor = typeof(Color).GetProperty(value, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
            if (propColor != null)
                return (Color)propColor.GetValue(null);


            if (value.StartsWith("#"))
            {
                //Hex...
                if (value.Length != 7)
                    throw new FormatException("Invalid color: " + value + ", expected \"r, g, b\" or \"r, g, b, a\" or #RRGGBB, actual: " + value);

                int r = Int32.Parse(value.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                int g = Int32.Parse(value.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                int b = Int32.Parse(value.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

                return new Color(r, g, b);
            }
            else
            {
                //Value separated by comma...
                string[] parts = value.Trim().Replace(" ", String.Empty).Split(',');

                if (parts.Length != 3 && parts.Length != 4)
                    throw new FormatException("Invalid color: " + value + ", expected \"r, g, b\" or \"r, g, b, a\" or #RRGGBB, actual: " + value);

                int r = Int32.Parse(parts[0]);
                int g = Int32.Parse(parts[1]);
                int b = Int32.Parse(parts[2]);
                int a = (parts.Length == 4 ? Int32.Parse(parts[3]) : 1);

                //Création of the color...
                return new Color(r, g, b, a);
            }
        }

        /// <summary>
        /// Vector2 creation...
        /// </summary>
        private static Vector2 CreateVector2(string value)
        {
            string[] parts = value.Trim().Replace(" ", String.Empty).Split(',');

            if (parts.Length != 2)
                throw new FormatException("Invalid Vector2: " + value + ", expected \"x, y\", actual: " + value);

            return new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
        }



        /// <summary>
        /// Changement du content
        /// </summary>
        private static void ContentManager_ContentChanged(string assetName)
        {
            lock (_gameObjectsPerAssetName)
            {
                if (_gameObjectsPerAssetName.TryGetValue(assetName, out List<GameObject> gameObjects))
                {
                    foreach (GameObject gameObject in gameObjects)
                    {
                        List<string> assetNames;
                        if (!_gameObjectsToReload.TryGetValue(gameObject, out assetNames))
                        {
                            assetNames = new List<string>();
                            _gameObjectsToReload[gameObject] = assetNames;
                        }
                        if(!assetNames.Contains(assetName))
                            assetNames.Add(assetName);
                    }
                }
            }
        }
    }
}
