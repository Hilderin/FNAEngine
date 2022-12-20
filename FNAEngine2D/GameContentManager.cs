using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
        /// Cache for types
        /// </summary>
        private static Dictionary<string, Type> _cacheTypes = new Dictionary<string, Type>();

        /// <summary>
        /// Assemblies to check
        /// </summary>
        private static List<Assembly> _assemblies = new List<Assembly>();

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
            //FNAEngine2D assembly...
            _assemblies.Add(typeof(GameContentManager).Assembly);

            //Game assembly
            _assemblies.Add(Assembly.GetEntryAssembly());

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
        public static void ReplaceContent(GameObject gameObject, GameContent gameContent)
        {
            //We have content?
            if (gameContent.Objects == null || gameContent.Objects.Count == 0)
                return;

            //Applying...
            for (int index = 0; index < gameContent.Objects.Count; index++)
            {
                GameContentObject childToCreate = gameContent.Objects[index];

                GameObject childExist = null;
                if(gameObject.NbChildren > index)
                    childExist = gameObject.Get(index);

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
                        gameObject.Remove(childExist);

                    objToUse = CreateGameObject(childToCreate);
                    isNew = true;
                }

                //Applying properties....
                ApplyGameObjectProperties(objToUse, childToCreate);

                if (isNew)
                {
                    gameObject.Insert(index, objToUse);
                }
                else
                {
                    objToUse.RemoveAll();
                    objToUse.Load();
                }
            }

            //Removing extra objets...
            while (gameObject.NbChildren > gameContent.Objects.Count)
                gameObject.RemoveAt(gameContent.Objects.Count);


        }


        /// <summary>
        /// Update game object if needed
        /// </summary>
        internal static void ReloadModifiedContent(GameObject gameObject)
        {
#if DEBUG

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
#endif
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

            Type type = GetGameObjectType(obj.ClassName);

            return (GameObject)Activator.CreateInstance(type);


        }


        /// <summary>
        /// Create the game object...
        /// </summary>
        private static void ApplyGameObjectProperties(GameObject gameObject, GameContentObject obj)
        {

            Type type = gameObject.GetType();

            //Copy values...
            foreach (KeyValuePair<string, object> kv in obj)
            {
                //On skip ClassName...
                if (kv.Key == "ClassName")
                    continue;

                PropertyInfo prop =  type.GetProperty(kv.Key);

                if (prop == null || !prop.CanWrite)
                    continue;

                object val;
                try
                {
                    if (prop.PropertyType.IsEnum)
                    {
                        val = Enum.Parse(prop.PropertyType, kv.Value.ToString());
                    }
                    else if (prop.PropertyType == typeof(Color))
                    {
                        val = CreateColor(kv.Value.ToString());
                    }
                    else
                    {
                        val = Convert.ChangeType(kv.Value, prop.PropertyType);
                    }
                }
                catch (Exception ex)
                {
                    throw new FormatException("Impossible to convert '" + kv.Value + ", to type: " + prop.PropertyType.Name + ", error: " + ex.ToString());
                }


                //For some props, we mush do special tricks for already existing objects...
                switch (kv.Key)
                {
                    case "X":
                        gameObject.TranslateX((float)val - gameObject.X);
                        break;
                    case "Y":
                        gameObject.TranslateY((float)val - gameObject.Y);
                        break;
                    case "Width":
                        gameObject.ResizeWidth((float)val - gameObject.Width);
                        break;
                    case "Height":
                        gameObject.ResizeHeight((float)val - gameObject.Height);
                        break;
                    default:
                        prop.SetValue(gameObject, val);
                        break;
                }

            }

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
                if(value.Length != 7)
                    throw new FormatException("Invalid color: " + value + ", expected \"r, g, b\" or \"r, g, b, a\" or #RRGGBB.");

                int r = Int32.Parse(value.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                int g = Int32.Parse(value.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                int b = Int32.Parse(value.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

                return new Color(r, g, b, 1);
            }
            else
            {
                //Value separated by comma...
                string[] parts = value.Trim().Replace(" ", String.Empty).Split(',');

                if (parts.Length != 3 && parts.Length != 4)
                    throw new FormatException("Invalid color: " + value + ", expected \"r, g, b\" or \"r, g, b, a\" or #RRGGBB.");

                int r = Int32.Parse(parts[0]);
                int g = Int32.Parse(parts[1]);
                int b = Int32.Parse(parts[2]);
                int a = (parts.Length == 4 ? Int32.Parse(parts[3]) : 1);

                //Création of the color...
                return new Color(r, g, b, a);
            }
        }

        /// <summary>
        /// Get game object type from class name
        /// </summary>
        private static Type GetGameObjectType(string className)
        {
            Type type;

            if (!_cacheTypes.TryGetValue(className, out type))
            {
                foreach (Assembly assembly in _assemblies)
                {
                    type = assembly.GetType(className, false);

                    if (type != null)
                        break;

                    foreach (Type assemblyType in assembly.GetTypes())
                    {
                        if (assemblyType.Name == className)
                        {
                            type = assemblyType;
                            break;
                        }
                    }

                    if (type != null)
                        break;
                }

                _cacheTypes[className] = type;

            }


            if (type == null)
                throw new InvalidCastException("Type not found '" + className + "'.");

            //Is it a GameObject?
            if(!typeof(GameObject).IsAssignableFrom(type))
                throw new InvalidCastException("Type '" + className + "' is not a GameObject.");

            return type;

            
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
