using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{
    /// <summary>
    /// NetworkObjectHelper
    /// </summary>
    public static class NetworkObjectHelper
    {
        
        /// <summary>
        /// Objects per number
        /// </summary>
        private static Type[] _objectsPerNumber = new Type[ushort.MaxValue];

        /// <summary>
        /// Static constructur
        /// </summary>
        static NetworkObjectHelper()
        {
            LoadObjectTypes();
        }

        /// <summary>
        /// Create an instance of an object from it's number
        /// </summary>
        public static NetworkGameObject Create(ushort number)
        {
            if (_objectsPerNumber[number] == null)
                throw new InvalidOperationException("Unknown object number: " + number);

            return (NetworkGameObject)Activator.CreateInstance(_objectsPerNumber[number]);

        }

        /// <summary>
        /// Get an object type from it's number
        /// </summary>
        public static Type GetObjectType(ushort number)
        {
            //if (_objectsPerNumber[number] == null)
            //    throw new InvalidOperationException("Unknown object number: " + number);

            return _objectsPerNumber[number];

        }

        /// <summary>
        /// Load all the object types
        /// </summary>
        private static void LoadObjectTypes()
        {
            
            //FNAEngine2D types...
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string name = assembly.GetName().Name;
                if (name != "mscorlib" && name != "FNA" && name != "netstandard" && name != "Newtonsoft.Json" && !name.StartsWith("System") && !name.StartsWith("Velentr") && !name.StartsWith("SharpFont"))
                    LoadObjectTypes(assembly);
            }

        }

        /// <summary>
        /// Load objects types for an assembly
        /// </summary>
        private static void LoadObjectTypes(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                NetworkObjectAttribute networkObjectAttribute = type.GetCustomAttribute<NetworkObjectAttribute>();
                if (networkObjectAttribute != null)
                {
                    if (_objectsPerNumber[networkObjectAttribute.Number] != null)
                        throw new InvalidOperationException("Multiple objects with the same number: " + type.FullName + " and " + _objectsPerNumber[networkObjectAttribute.Number].FullName);

                    _objectsPerNumber[networkObjectAttribute.Number] = type;
                }
            }

        }


    }
}
