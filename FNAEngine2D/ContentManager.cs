using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Content Manager du FNAEngine2D
    /// </summary>
    public class ContentManager : Microsoft.Xna.Framework.Content.ContentManager
    {

        /// <summary>
        /// Extensions for textures
        /// </summary>
        private static string[] TEXTURES_EXTENSIONS = new string[]
        {
            ".bmp", ".gif", ".jpg", ".jpeg", ".png", ".tga", ".tif", ".tiff"        //, ".dds"
        };


        /// <summary>
        /// Extensions for game content
        /// </summary>
        private static string[] GAMECONTENT_EXTENSIONS = new string[]
        {
            ".json"
        };


        /// <summary>
        /// Graphics device
        /// </summary>
        private GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Cache
        /// </summary>
        private static ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// Disposable assets
        /// </summary>
        private static List<IDisposable> _disposableAssets = new List<IDisposable>();

        /// <summary>
        /// Disposable objets avec leur nom
        /// </summary>
        private static Dictionary<string, IDisposable> _disposableAssetsPerName = new Dictionary<string, IDisposable>();

        /// <summary>
        /// Constructeur
        /// </summary>
        public ContentManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        public ContentManager(IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory)
        {
        }

        /// <summary>
        /// Remove a asset from the cache
        /// </summary>
        public static void RemoveFromCache(string assetName)
        {
            _cache.TryRemove(assetName.Replace("\\", "/"), out var bidon);
            _cache.TryRemove(assetName.Replace("/", "\\"), out bidon);
        }

        /// <summary>
        /// Permet de loader un asset
        /// </summary>
        public override T Load<T>(string assetName)
        {
            //Dans la cache??
            object asset = null;
            if (_cache.TryGetValue(assetName, out asset))
            {
                if (asset is T)
                {
                    return (T)asset;
                }
            }

            //Tentative de lecture sur le disque...
            bool mustAddToDisposable = true;
            if (typeof(T) == typeof(Texture2D) || typeof(T) == typeof(Texture))
            {
                //Texture...
                asset = LoadTexture<T>(assetName);
            }
            else if (typeof(T) == typeof(GameContent))
            {
                //Texture...
                asset = LoadGameContent(assetName);
            }
            else
            {
                //Fallback sur le ContentManager de base...
                asset = base.Load<T>(assetName);

                //Le base va le faire...
                mustAddToDisposable = false;
            }

            //Si on doit checker le disposable, on va le faire. Si ça vient du base.Load, c'est le base.Unload qui va s'en occuper
            if (mustAddToDisposable)
            {
                //On set dans la cache...
                _cache[assetName] = asset;

                IDisposable disposableResult = asset as IDisposable;
                if (disposableResult != null)
                {
                    lock (_disposableAssets)
                    {
                        //Si on a clearer la cache, on pourrait encore avoir l'objet en mémoire en attendant qu'on reload, on va donc le checker ici...
                        if (_disposableAssetsPerName.TryGetValue(assetName, out IDisposable oldAsset))
                        {
                            oldAsset.Dispose();
                            _disposableAssetsPerName.Remove(assetName);
                        }

                        _disposableAssets.Add(disposableResult);
                        _disposableAssetsPerName[assetName] = disposableResult;
                    }
                }
            }

            return (T)asset;

        }


        /// <summary>
        /// Unload du content...
        /// </summary>
        public override void Unload()
        {
            // Look for disposable assets.
            foreach (IDisposable disposable in _disposableAssets)
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            _disposableAssets.Clear();
            _cache.Clear();

            base.Unload();
        }

        /// <summary>
        /// Permet d'obtenir le path de l'asset
        /// </summary>
        private string GetAssetFullPath(string assetName, string[] acceptedExtensions)
        {
            /* On some platforms, name and slash direction matter.
			 * We store the asset by a /-separating key rather than
			 * how the path to the file was passed to us to avoid
			 * loading "content/asset1.xnb" and "content\\ASSET1.xnb"
			 * as if they were two different files. this matches
			 * stock XNA behavior. The Dictionary will ignore case
			 * differences.
			 */
            string filename = assetName.Replace('\\', '/');

            foreach (string extension in acceptedExtensions)
            {
                string path = Path.Combine(this.RootDirectory, filename + extension);
                if (File.Exists(path))
                    return path;
            }

            throw new FileNotFoundException("Asset not found: " + assetName + ", accepted extensions: " + String.Join("; ", acceptedExtensions));
        }

        /// <summary>
        /// Permet d'accéder au GetGraphicsDevice
        /// </summary>
        private GraphicsDevice GetGraphicsDevice()
        {
            if (_graphicsDevice == null)
            {
                IGraphicsDeviceService result = ServiceProvider.GetService(
                    typeof(IGraphicsDeviceService)
                ) as IGraphicsDeviceService;
                if (result == null)
                {
                    throw new ContentLoadException("No Graphics Device Service");
                }
                _graphicsDevice = result.GraphicsDevice;
            }
            return _graphicsDevice;
        }

        /// <summary>
        /// Permet de loader une texture
        /// </summary>
        private T LoadTexture<T>(string assetName)
        {
            string fullPath = GetAssetFullPath(assetName, TEXTURES_EXTENSIONS);

            using (Stream stream = File.OpenRead(fullPath))
            {
                object ret = Texture2D.FromStream(GetGraphicsDevice(), stream);
                return (T)ret;
            }
        }

        /// <summary>
        /// Load game content file
        /// </summary>
        private GameContent LoadGameContent(string assetName)
        {
            string fullPath = GetAssetFullPath(assetName, GAMECONTENT_EXTENSIONS);

            var serializer = new JsonSerializer();
            using (Stream stream = File.OpenRead(fullPath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        return serializer.Deserialize<GameContent>(jsonReader);
                    }
                }
            }
        }

    }
}
