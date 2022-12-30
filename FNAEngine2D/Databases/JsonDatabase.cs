using FNAEngine2D;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Databases
{
    /// <summary>
    /// Super simple json database on disk
    /// </summary>
    public class JsonDatabase
    {
        /// <summary>
        /// Serializer
        /// </summary>
        private JsonSerializer _serializer;

        /// <summary>
        /// Root folder
        /// </summary>
        public string RootFolder { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDatabase(string rootFolder)
        {
            this.RootFolder = rootFolder;

            _serializer = new JsonSerializer();
            _serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
        }


        /// <summary>
        /// Save a object in the database
        /// </summary>
        public void Save(Guid id, object obj)
        {
            string folder = Path.Combine(this.RootFolder, obj.GetType().Name);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);


            string path = Path.Combine(folder, id.ToString() + ".json");
            
            using (Stream stream = File.Create(path))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    using (JsonTextWriter jsonwriter = new JsonTextWriter(writer))
                    {
                        _serializer.Serialize(jsonwriter, obj);
                    }
                }
            }
        }

        /// <summary>
        /// Save a object in the database
        /// </summary>
        public T Load<T>(Guid id)
        {
            string path = Path.Combine(this.RootFolder, typeof(T).Name, id.ToString() + ".json");

            if (File.Exists(path))
            {
                using (Stream stream = File.OpenRead(path))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        using (JsonTextReader jsonReader = new JsonTextReader(reader))
                        {
                            return _serializer.Deserialize<T>(jsonReader);
                        }
                    }
                }
            }

            return default(T);
        }

    }
}
