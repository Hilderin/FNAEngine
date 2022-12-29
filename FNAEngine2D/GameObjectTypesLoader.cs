using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public static class GameObjectTypesLoader
    {

        /// <summary>
        /// Cache for types
        /// </summary>
        private static List<Type> _types = new List<Type>();

        /// <summary>
        /// Cache for types
        /// </summary>
        private static Dictionary<string, Type> _cacheTypes = new Dictionary<string, Type>();


        /// <summary>
        /// Contructor
        /// </summary>
        static GameObjectTypesLoader()
        {
            LoadGameObjectTypes();
        }

        /// <summary>
        /// Return all the game object types
        /// </summary>
        private static void LoadGameObjectTypes()
        {
            _types = new List<Type>();

            //FNAEngine2D types...
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string name = assembly.GetName().Name;
                if (name != "mscorlib" && name != "FNA" && name != "netstandard" && name != "Newtonsoft.Json" && !name.StartsWith("System") && !name.StartsWith("Velentr") && !name.StartsWith("SharpFont"))
                    LoadGameObjectTypes(assembly, _types);
            }

            _types.Sort((a, b) => a.FullName.CompareTo(b.FullName));

            //Cache the types...
            foreach (Type type in _types)
                _cacheTypes[type.Name] = type;

        }

        /// <summary>
        /// Load game object types from an assembly
        /// </summary>
        private static void LoadGameObjectTypes(Assembly assembly, List<Type> list)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(GameObject).IsAssignableFrom(type) && type != typeof(GameObject))
                    list.Add(type);
            }
        }

        /// <summary>
        /// Return the game object types
        /// </summary>
        public static List<Type> GetGameObjectTypes()
        {
            return _types;
        }

        /// <summary>
        /// Get game object type from class name
        /// </summary>
        public static Type GetGameObjectType(string className)
        {
            if (_cacheTypes.TryGetValue(className, out var type))
                return type;

            throw new InvalidCastException("GameObject not found '" + className + "'.");

            //Type type;

            //if (!_cacheTypes.TryGetValue(className, out type))
            //{
            //    foreach (Assembly assembly in _assemblies)
            //    {
            //        type = assembly.GetType(className, false);

            //        if (type != null)
            //            break;

            //        foreach (Type assemblyType in assembly.GetTypes())
            //        {
            //            if (assemblyType.Name == className)
            //            {
            //                type = assemblyType;
            //                break;
            //            }
            //        }

            //        if (type != null)
            //            break;
            //    }

            //    _cacheTypes[className] = type;

            //}


            //if (type == null)
            //    throw new InvalidCastException("Type not found '" + className + "'.");

            ////Is it a GameObject?
            //if (!typeof(GameObject).IsAssignableFrom(type))
            //    throw new InvalidCastException("Type '" + className + "' is not a GameObject.");

            //return type;


        }

    }
}
