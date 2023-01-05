using FNAEngine2D.Aseprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FNAEngine2D
{
    /// <summary>
    /// Content Manager du FNAEngine2D
    /// </summary>
    public class ContentManager : Microsoft.Xna.Framework.Content.ContentManager
    {
        /// <summary>
        /// Built in texture for a pixel
        /// </summary>
        public const string TEXTURE_PIXEL = "pixel";

        /// <summary>
        /// Built in texture for a magenta pixel
        /// </summary>
        public const string TEXTURE_PIXEL_MAGENTA = "pixel_magenta";

        /// <summary>
        /// Built in texture for a circle
        /// </summary>
        public const string TEXTURE_CIRCLE = "circle";

        /// <summary>
        /// Embed font: Roboto-Regular
        /// </summary>
        public const string FONT_ROBOTO_REGULAR = "Roboto-Regular";



        /// <summary>
        /// Extensions for textures
        /// </summary>
        internal static string[] TEXTURES_EXTENSIONS = new string[]
        {
            ".aseprite", ".bmp", ".gif", ".jpg", ".jpeg", ".png", ".tga", ".tif", ".tiff"        //, ".dds"
        };


        /// <summary>
        /// Extensions for game content
        /// </summary>
        private static string[] GAMECONTENT_EXTENSIONS = new string[]
        {
            ".json"
        };

        /// <summary>
        /// Extensions for sprite animation
        /// </summary>
        private static string[] SPRITE_EXTENSIONS = new string[]
        {
            ".json"
        };

        /// <summary>
        /// Extensions for sprite animation
        /// </summary>
        private static string[] SPRITEANIMATION_EXTENSIONS = new string[]
        {
            ".aseprite", ".json"
        };

        /// <summary>
        /// Extensions for sound effect
        /// </summary>
        private static string[] SOUNDEFFECT_EXTENSIONS = new string[]
        {
            ".wav"
        };


        /// <summary>
        /// Graphics device
        /// </summary>
        private GraphicsDevice _graphicsDevice;

        /// <summary>
        /// Cache
        /// </summary>
        private static ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Cache
        /// </summary>
        private static ConcurrentDictionary<string, CacheAssetInfo> _cacheFullPathFullAssetName = new ConcurrentDictionary<string, CacheAssetInfo>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Disposable assets
        /// </summary>
        private static List<IDisposable> _disposableAssets = new List<IDisposable>();

        /// <summary>
        /// Disposable objets avec leur nom
        /// </summary>
        private static Dictionary<string, IDisposable> _disposableAssetsPerName = new Dictionary<string, IDisposable>(StringComparer.OrdinalIgnoreCase);
        
        /// <summary>
        /// Root du content folder
        /// </summary>
        private static string _contentFolder;

        /// <summary>
        /// Permet de retourner le path du content folder
        /// </summary>
        public static string ContentFolder
        { 
            get { return _contentFolder; }
            set { _contentFolder = value; }
        }

        
        /// <summary>
        /// Constructeur
        /// </summary>
        static ContentManager()
        {
            string assemblyLocation = Assembly.GetCallingAssembly().Location;
            if (assemblyLocation.IndexOf(@"bin\debug\", StringComparison.OrdinalIgnoreCase) > 0
                || assemblyLocation.IndexOf(@"bin\release\", StringComparison.OrdinalIgnoreCase) > 0)
                _contentFolder = Path.GetFullPath(@"..\..\Content\");
            else
                _contentFolder = Path.GetFullPath(@"Content\");
        }


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
        /// Reload the asset
        /// </summary>
        public void Reload(string fullPath)
        {
            //Asset in the cache???
            if (_cacheFullPathFullAssetName.TryGetValue(fullPath.Replace('\\', '/'), out CacheAssetInfo assetInfo))
            {
                //Reloading...
                if (_cache.TryGetValue(assetInfo.FullAssetName, out object contentObj))
                {
                    //Reloading data...
                    MethodInfo method = typeof(ContentManager).GetMethod(nameof(LoadContent), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
                    MethodInfo generic = method.MakeGenericMethod(assetInfo.Type);

                    object newData = generic.Invoke(this, new object[] { assetInfo.AssetName, String.Empty });
                    contentObj.GetType().GetProperty("Data").SetValue(contentObj, newData);
                }

            }
        }

        /// <summary>
        /// Returns an assent content
        /// </summary>
        public Content<T> GetContent<T>(string assetName)
        {
            //Dans la cache??
            string fullAssetName = typeof(T).Name + "." + assetName;
            object asset = null;
            string fullPath = null;

            //Check the cache...
            if (_cache.TryGetValue(fullAssetName, out asset))
            {
                if (asset is Content<T>)
                {
                    return (Content<T>)asset;
                }
            }

            asset = LoadContent<T>(assetName, ref fullPath);



            //----------
            //Creation of the content...
            Content<T> content = new Content<T>(assetName, (T)asset);


            //On set dans la cache...
            _cache[fullAssetName] = content;
            

            //If we have a full path, we took care of the loading and we can reload the asset...
            if (fullPath != null)
            {
                //In cache...
                _cacheFullPathFullAssetName[fullPath] = new CacheAssetInfo()
                {
                    Type = typeof(T),
                    AssetName = assetName,
                    FullAssetName = fullAssetName
                };


                //And we deal with disposable assets...
                IDisposable disposableResult = asset as IDisposable;
                if (disposableResult != null)
                {
                    lock (_disposableAssets)
                    {
                        //Si on a clearer la cache, on pourrait encore avoir l'objet en mémoire en attendant qu'on reload, on va donc le checker ici...
                        if (_disposableAssetsPerName.TryGetValue(fullAssetName, out IDisposable oldAsset))
                        {
                            oldAsset.Dispose();
                            _disposableAssetsPerName.Remove(fullAssetName);
                        }

                        _disposableAssets.Add(disposableResult);
                        _disposableAssetsPerName[fullAssetName] = disposableResult;
                    }
                }
            }

            return content;

        }

        /// <summary>
        /// Load content
        /// </summary>
        private object LoadContent<T>(string assetName, ref string fullPath)
        {
            object asset;
            //------------------
            //Loading...
            if (typeof(T) == typeof(Texture2D) || typeof(T) == typeof(Texture))
            {
                //Texture...
                asset = LoadTexture<T>(assetName, out fullPath);
            }
            else if (typeof(T) == typeof(GameContent))
            {
                //GameContent...
                asset = LoadGameContent(assetName, out fullPath);
            }
            else if (typeof(T) == typeof(Sprite))
            {
                //Sprite...
                asset = LoadSprite(assetName, out fullPath);
            }
            else if (typeof(T) == typeof(SpriteAnimation))
            {
                //SpriteAnimation...
                asset = LoadSpriteAnimation(assetName, out fullPath);
            }
            else if (typeof(T) == typeof(SoundEffect))
            {
                //SoundEffect...
                asset = LoadSoundEffect(assetName, out fullPath);
            }
            else
            {
                //Fallback sur le ContentManager de base...
                asset = base.Load<T>(assetName);
            }

            return asset;
        }

        /// <summary>
        /// Permet de loader un asset
        /// </summary>
        public override T Load<T>(string assetName)
        {

            Content<T> content = GetContent<T>(assetName);

            return content.Data;

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
        public string GetAssetFullPath(string assetName, string[] acceptedExtensions)
        {
            /* On some platforms, name and slash direction matter.
			 * We store the asset by a /-separating key rather than
			 * how the path to the file was passed to us to avoid
			 * loading "content/asset1.xnb" and "content\\ASSET1.xnb"
			 * as if they were two different files. this matches
			 * stock XNA behavior. The Dictionary will ignore case
			 * differences.
			 */
            foreach (string extension in acceptedExtensions)
            {
                string path = Path.Combine(this.RootDirectory, assetName + extension).Replace('\\', '/');
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
        private T LoadTexture<T>(string assetName, out string fullPath)
        {
            //Built in textures?
            if (typeof(T) == typeof(Texture2D))
            {
                if(!assetName.Contains("\\") && !assetName.Contains("//"))
                {
                    object resObj = Resource.ResourceManager.GetObject(assetName);
                    if (resObj != null && resObj is byte[])
                    {
                        fullPath = assetName;
                        object ret = GetTexture2DFromBytes((byte[])resObj);
                        return (T)ret;
                    }
                }
            }

            try
            {
                //Texture from the disk...
                fullPath = GetAssetFullPath(assetName, TEXTURES_EXTENSIONS);

                using (Stream stream = File.OpenRead(fullPath))
                {
                    if (Path.GetExtension(fullPath).Equals(".aseprite", StringComparison.OrdinalIgnoreCase))
                    {
                        //Aseprite file...
                        AseFile aseFile = new AseFile(stream);

                        //We take the first frame
                        object ret = aseFile.GetFrame(0);
                        return (T)ret;
                    }
                    else
                    {
                        object ret = Texture2D.FromStream(GetGraphicsDevice(), stream);
                        return (T)ret;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadTexture - Error loading '" + assetName + "': " + ex.Message);

                using (MemoryStream ms = new MemoryStream(Resource.pixel_magenta))
                {
                    fullPath = Path.Combine(this.RootDirectory, assetName + ".jpg").Replace('\\', '/');

                    object ret = Texture2D.FromStream(GetGraphicsDevice(), ms);
                    return (T)ret;
                }
            }
        }

        /// <summary>
        /// Load a Texture 2D from an array of bytes
        /// </summary>
        private Texture2D GetTexture2DFromBytes(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                object ret = Texture2D.FromStream(GetGraphicsDevice(), ms);
                return (Texture2D)ret;
            }
        }

        /// <summary>
        /// Load game content file
        /// </summary>
        private GameContent LoadGameContent(string assetName, out string fullPath)
        {
            
            fullPath = Path.Combine(this.RootDirectory, assetName + ".json").Replace('\\', '/');

            try
            {
                if (File.Exists(fullPath))
                    return Deserialize<GameContent>(fullPath);
                else
                    //GameContent does not exists, we dont want to crash, si we can create a empty GameContent from the Editor...
                    return new GameContent();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadGameContent - Error loading '" + assetName + "': " + ex.Message);
                return new GameContent();
            }
            
        }

        /// <summary>
        /// Sprite
        /// </summary>
        private Sprite LoadSprite(string assetName, out string fullPath)
        {
            try
            {
                fullPath = GetAssetFullPath(assetName, SPRITE_EXTENSIONS);
                return Deserialize<Sprite>(fullPath);
            }
            catch (Exception ex)
            {
                fullPath = Path.Combine(this.RootDirectory, assetName + ".png").Replace('\\', '/');
                Debug.WriteLine("LoadSprite - Error loading '" + assetName + "': " + ex.Message);
                return new Sprite("pixel_magenta", 1, 1, 1, 1);
            }
        }

        /// <summary>
        /// SpriteAnimation
        /// </summary>
        private SpriteAnimation LoadSpriteAnimation(string assetName, out string fullPath)
        {
            try
            {
                fullPath = GetAssetFullPath(assetName, SPRITEANIMATION_EXTENSIONS);
                if (Path.GetExtension(fullPath).Equals(".aseprite", StringComparison.OrdinalIgnoreCase))
                {
                    //Aseprite file...
                    using (Stream stream = File.OpenRead(fullPath))
                    {
                        AseFile aseFile = new AseFile(stream);

                        return aseFile.GetSpriteAnimation();
                    }
                }
                else
                {
                    return Deserialize<SpriteAnimation>(fullPath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadSpriteAnimation - Error loading '" + assetName + "': " + ex.Message);

                fullPath = Path.Combine(this.RootDirectory, assetName + ".json").Replace('\\', '/');
                return new SpriteAnimation();
            }
        }

        /// <summary>
        /// Permet de loader une texture
        /// </summary>
        private SoundEffect LoadSoundEffect(string assetName, out string fullPath)
        {

            try
            {

                if (assetName == "empty_sfx")
                {
                    fullPath = "empty_sfx";
                    using (MemoryStream ms = new MemoryStream(Resource.empty_sfx))
                    {
                        return SoundEffect.FromStream(ms);
                    }
                }

                //I will find the file...
                fullPath = GetAssetFullPath(assetName, SOUNDEFFECT_EXTENSIONS);

                //Little hack to bypass the cache of the default ContentManager...
                string tempdid = Guid.NewGuid().ToString();
                string tempAssetName = assetName + tempdid;
                string tempPath = Path.Combine(Path.GetDirectoryName(fullPath), Path.GetFileNameWithoutExtension(fullPath) + tempdid + Path.GetExtension(fullPath));

                try
                {
                    File.Copy(fullPath, tempPath);

                    return base.Load<SoundEffect>(tempAssetName);

                }
                finally
                {
                    try
                    {
                        File.Delete(tempPath);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoadSoundEffect - Error loading '" + assetName + "': " + ex.Message);

                fullPath = Path.Combine(this.RootDirectory, assetName + ".wav").Replace('\\', '/');

                using (MemoryStream ms = new MemoryStream(Resource.empty_sfx))
                {
                    return SoundEffect.FromStream(ms);
                }
            }
        }

        /// <summary>
        /// Deserialize json
        /// </summary>
        private T Deserialize<T>(string fullPath)
        {
            var serializer = new JsonSerializer();
            using (Stream stream = File.OpenRead(fullPath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    using (JsonTextReader jsonReader = new JsonTextReader(reader))
                    {
                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
            }
        }

        /// <summary>
        /// Cache information
        /// </summary>
        private class CacheAssetInfo
        {
            public Type Type;
            public string AssetName;
            public string FullAssetName;
        }

    }
}
