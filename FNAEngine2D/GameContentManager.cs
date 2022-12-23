﻿using Microsoft.Xna.Framework;
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
                }
                else
                {
                    //Create a new one...
                    if(childExist != null)
                        container.Remove(childExist);

                    objToUse = CreateGameObject(childToCreate);
                    isNew = true;
                }

                //Applying properties....
                ApplyGameObjectProperties(objToUse, childToCreate);

                if (isNew)
                {
                    container.Insert(index, objToUse);
                }
                else
                {
                    objToUse.RemoveAll();
                    objToUse.Load();
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
        private static GameObject CreateGameObject(GameContentObject obj)
        {

            Type type = GameObjectTypesLoader.GetGameObjectType(obj.ClassName);

            return (GameObject)Activator.CreateInstance(type);


        }


        /// <summary>
        /// Create the game object...
        /// </summary>
        private static void ApplyGameObjectProperties(GameObject gameObject, GameContentObject obj)
        {

            Type type = gameObject.GetType();

            string json = JsonConvert.SerializeObject(obj);
            object newObj = JsonConvert.DeserializeObject(json, type);

            //Copy values...
            foreach (KeyValuePair<string, object> kv in obj)
            {
                //On skip ClassName...
                if (kv.Key == "ClassName")
                    continue;

                PropertyInfo prop = type.GetProperty(kv.Key);

                if (prop == null || !prop.CanWrite)
                    continue;

                //Copy of value....
                prop.SetValue(gameObject, prop.GetValue(newObj));

            }

            ////Copy values...
            //foreach (KeyValuePair<string, object> kv in obj)
            //{
            //    //On skip ClassName...
            //    if (kv.Key == "ClassName")
            //        continue;

            //    PropertyInfo prop =  type.GetProperty(kv.Key);

            //    if (prop == null || !prop.CanWrite)
            //        continue;

            //    object val;
            //    try
            //    {
            //        //If already the good type, we keep it!
            //        //When we save the content in the ContentDesigner, the prop are not serialized in the GameContentObject, so if we reload the Container,
            //        //we will already have the correct type.
            //        if (kv.Value == null)
            //        {
            //            val = null;
            //        }
            //        else if (kv.Value.GetType() == prop.PropertyType)
            //        {
            //            val = kv.Value;
            //        }
            //        else
            //        {
            //            if (prop.PropertyType.IsEnum)
            //            {
            //                val = Enum.Parse(prop.PropertyType, kv.Value.ToString());
            //            }
            //            else if (prop.PropertyType == typeof(Color))
            //            {
            //                val = CreateColor(kv.Value.ToString());
            //            }
            //            else if (prop.PropertyType == typeof(Vector2))
            //            {
            //                val = CreateVector2(kv.Value.ToString());
            //            }
            //            else
            //            {
            //                val = Convert.ChangeType(kv.Value, prop.PropertyType);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new FormatException("Impossible to convert '" + kv.Value + ", to type: " + prop.PropertyType.Name + ", error: " + ex.ToString());
            //    }


            //    //For some props, we mush do special tricks for already existing objects...
            //    switch (kv.Key)
            //    {
            //        case "X":
            //            gameObject.TranslateX((float)val - gameObject.X);
            //            break;
            //        case "Y":
            //            gameObject.TranslateY((float)val - gameObject.Y);
            //            break;
            //        case "Width":
            //            gameObject.ResizeWidth((float)val - gameObject.Width);
            //            break;
            //        case "Height":
            //            gameObject.ResizeHeight((float)val - gameObject.Height);
            //            break;
            //        default:
            //            prop.SetValue(gameObject, val);
            //            break;
            //    }

            //}

        }


        /// <summary>
        /// Get the GameContentObject from a GameObject
        /// </summary>
        public static GameContentObject GetGameContentObject(GameObject gameObject)
        {
            Type type = gameObject.GetType();


            GameContentObject gameContentObject = new GameContentObject();


            gameContentObject["ClassName"] = type.Name;


            //Copy values...
            foreach(PropertyInfo prop in type.GetProperties())
            {
                object value = prop.GetValue(gameObject);

                BrowsableAttribute browsableAttr = prop.GetCustomAttribute<BrowsableAttribute>();
                if (browsableAttr != null && !browsableAttr.Browsable)
                    //Not visible to the user, we skip..
                    continue;

                DefaultValueAttribute defaultValueAttr = prop.GetCustomAttribute<DefaultValueAttribute>();
                if (defaultValueAttr != null)
                {
                    if (defaultValueAttr.Value == null)
                    {
                        //Default value, we skip..
                        if (value == null)
                            continue;
                    }
                    else if (defaultValueAttr.Value.Equals(value))
                    {
                        //Default value, we skip..
                        continue;
                    }
                }

                gameContentObject[prop.Name] = value;

            }

            //gameContentObject.Props = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(gameObject));

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
