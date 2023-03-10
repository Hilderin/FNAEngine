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
    /// Command helper
    /// </summary>
    public static class CommandHelper
    {
        /// <summary>
        /// Header size
        /// </summary>
        private const int HEADER_SIZE = 2;

        /// <summary>
        /// Command per number
        /// </summary>
        private static Type[] _commandsPerNumber = new Type[ushort.MaxValue];

        /// <summary>
        /// Number for each types
        /// </summary>
        private static Dictionary<Type, ushort> _numbersPerType = new Dictionary<Type, ushort>();

        ///// <summary>
        ///// Serializer
        ///// </summary>
        //private static JsonSerializer _serializer = new JsonSerializer();


        /// <summary>
        /// Static constructur
        /// </summary>
        static CommandHelper()
        {
            LoadCommandTypes();
        }

        /// <summary>
        /// Serialize a command
        /// Returns the length written
        /// </summary>
        public static int Serialize(ICommand command, byte[] buffer, int offset)
        {
            //Structure of the data...
            //2 bytes: Command number
            //Command serialized

            Type type = command.GetType();

            ushort commandNumber;
            if(!_numbersPerType.TryGetValue(type, out commandNumber))
                throw new InvalidOperationException("Unknown command type: " + type.FullName);


            //Writing command number...
            buffer[offset] = (byte)commandNumber;
            buffer[offset + 1] = (byte)(commandNumber >> 8);

            BinWriter binWriter = new BinWriter(buffer, offset + HEADER_SIZE);

            binWriter.Write(command.ID);

            command.Serialize(binWriter);

            return binWriter.Position;

            //using (MemoryStream ms = new MemoryStream(buffer, offset + HEADER_SIZE, buffer.Length - offset - HEADER_SIZE))
            //{
            //    using (StreamWriter writer = new StreamWriter(ms))
            //    {

            //        using (var jsonWriter = new JsonTextWriter(writer))
            //        {
            //            _serializer.Serialize(jsonWriter, command);

            //            jsonWriter.Flush();

            //            //Return the total length...
            //            return ((int)ms.Position) + HEADER_SIZE;
            //        }

            //    }


            //}
        }


        /// <summary>
        /// Deserialize a command
        /// </summary>
        public static ICommand Deserialize(byte[] buffer, int offset, int length)
        {
            //Structure of the data...
            //2 bytes: Command number
            //Command serialized
            ushort commandNumber = (ushort)((buffer[1 + offset] << 8) + buffer[offset]);

            if (_commandsPerNumber[commandNumber] == null)
                throw new InvalidOperationException("Unknown command number: " + commandNumber);

            ICommand command = (ICommand)Activator.CreateInstance(_commandsPerNumber[commandNumber]);

            BinReader binReader = new BinReader(buffer, offset + HEADER_SIZE);

            command.ID = binReader.ReadGuid();
            command.Deserialize(binReader);

            return command;

            //using (MemoryStream ms = new MemoryStream(buffer, offset + HEADER_SIZE, length - HEADER_SIZE))
            //{
            //    using (StreamReader reader = new StreamReader(ms))
            //    {
            //        using (var jsonReader = new JsonTextReader(reader))
            //        {
            //            return _serializer.Deserialize(jsonReader, _commandsPerNumber[commandNumber]);
            //        }
            //    }
            //}
        }


        /// <summary>
        /// Load all the command types
        /// </summary>
        private static void LoadCommandTypes()
        {
            List<Type> commands = new List<Type>();
            
            //FNAEngine2D types...
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string name = assembly.GetName().Name;
                if (name != "mscorlib" && name != "FNA" && name != "netstandard" && name != "Newtonsoft.Json" && !name.StartsWith("System") && !name.StartsWith("Velentr") && !name.StartsWith("SharpFont"))
                    LoadCommandTypes(assembly, commands);
            }

            //We sort so the client and the server will use de same numbers
            commands.Sort((a, b) => a.FullName.CompareTo(b.FullName));

            for (int index = 0; index < commands.Count; index++)
            {
                _commandsPerNumber[index] = commands[index];
                _numbersPerType[commands[index]] = (ushort)index;
            }
        }

        /// <summary>
        /// Load command types for an assembly
        /// </summary>
        private static void LoadCommandTypes(Assembly assembly, List<Type> commands)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(ICommand).IsAssignableFrom(type))
                    commands.Add(type);
            }

        }


    }
}
