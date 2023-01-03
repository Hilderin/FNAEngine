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
        /// Number for each types
        /// </summary>
        private static Dictionary<Type, ushort> _numbersPerType = new Dictionary<Type, ushort>();


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
        /// Get an object number from it's type
        /// </summary>
        public static ushort GetObjectNumber(Type type)
        {
            ushort objectNumber;

            if (!_numbersPerType.TryGetValue(type, out objectNumber))
                throw new InvalidOperationException("Unknown object type: " + type.FullName);

            return objectNumber;

        }

        /// <summary>
        /// Load all the object types
        /// </summary>
        private static void LoadObjectTypes()
        {
            List<Type> objects = new List<Type>();

            //FNAEngine2D types...
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string name = assembly.GetName().Name;
                if (name != "mscorlib" && name != "FNA" && name != "netstandard" && name != "Newtonsoft.Json" && !name.StartsWith("System") && !name.StartsWith("Velentr") && !name.StartsWith("SharpFont"))
                    LoadObjectTypes(assembly, objects);
            }

            //We sort so the client and the server will use de same numbers
            objects.Sort((a, b) => a.FullName.CompareTo(b.FullName));

            for (int index = 0; index < objects.Count; index++)
            {
                _objectsPerNumber[index] = objects[index];
                _numbersPerType[objects[index]] = (ushort)index;
            }
        }

        /// <summary>
        /// Load objects types for an assembly
        /// </summary>
        private static void LoadObjectTypes(Assembly assembly, List<Type> objects)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(NetworkGameObject).IsAssignableFrom(type))
                    objects.Add(type);
            }

        }


    }
}
