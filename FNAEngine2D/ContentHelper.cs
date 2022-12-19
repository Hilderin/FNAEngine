using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FNAEngine2D
{

    /// <summary>
    /// Delegate lancés quand des changements se font
    /// </summary>
    /// <param name="assetName"></param>
    public delegate void ContentChangedHandler(string assetName);

    /// <summary>
    /// Content Manager
    /// </summary>
    public static class ContentHelper
    {

        /// <summary>
        /// Extensions traités
        /// </summary>
        private static readonly string[] EXTENSIONS = new string[] { ".jpg", ".jpeg", ".wav", ".ogg", ".png", ".json" };


        /// <summary>
        /// Event quand des changements se font sur le content
        /// </summary>
        public static event ContentChangedHandler ContentChanged;


        /// <summary>
        /// Root du content folder
        /// </summary>
        private static string _contentFolder;

        /// <summary>
        /// Date et heure du dernier changement de content
        /// </summary>
        private static DateTime _lastUpdatedContent = DateTime.MinValue;

        /// <summary>
        /// Task pour l'update du contenu
        /// </summary>
        private static Task _taskUpdateContent = null;

        /// <summary>
        /// Liste des changements
        /// </summary>
        private static List<string> ListChanges = new List<string>();

        /// <summary>
        /// Indique si du contenu doit être reloadé
        /// </summary>
        public static bool _contentToReload = false;

        /// <summary>
        /// Constructeur
        /// </summary>
        static ContentHelper()
        {
            string assemblyLocation = Assembly.GetCallingAssembly().Location;
            if (assemblyLocation.IndexOf(@"bin\debug\", StringComparison.OrdinalIgnoreCase) > 0
                || assemblyLocation.IndexOf(@"bin\release\", StringComparison.OrdinalIgnoreCase) > 0)
                _contentFolder = Path.GetFullPath(@"..\..\Content\");
            else
                _contentFolder = Path.GetFullPath(@"Content\");
        }

        /// <summary>
        /// Permet de retourner le path du content folder
        /// </summary>
        public static string ContentFolder { get { return _contentFolder; } }


        /// <summary>
        /// Permet de vérifier si du contenu change
        /// </summary>
        public static void StartWatchUpdateContent()
        {
#if DEBUG
            FileSystemWatcher fsw = new FileSystemWatcher(ContentFolder);
            fsw.IncludeSubdirectories = true;

            fsw.Changed += Fsw_Changed;
            fsw.Created += Fsw_Changed;
            fsw.Deleted += Fsw_Changed;
            fsw.Renamed += Fsw_Renamed;

            fsw.EnableRaisingEvents = true;
#endif

        }

        /// <summary>
        /// Permet de reloader le contenu modifié
        /// </summary>
        public static void ReloadModifiedContent()
        {
            if (!_contentToReload)
                return;

            ReloadModifiedContentInternal();

            _contentToReload = false;

        }

        /// <summary>
        /// Permet de reloader le contenu modifié
        /// </summary>
        private static void ReloadModifiedContentInternal()
        {
            List<string> changes;

            lock (ListChanges)
            {
                changes = new List<string>(ListChanges);
                ListChanges.Clear();
            }

            foreach (string fullPath in changes)
            {
                //On clear la cache...
                ContentManager.RemoveFromCache(fullPath);

                if (ContentChanged != null)
                {
                    string assetName = fullPath.Substring(ContentFolder.Length);

                    //Retrait de l'extension...
                    int index = assetName.LastIndexOf('.');
                    if (index > 0)
                    {
                        assetName = assetName.Substring(0, index);

                        ContentChanged(assetName);
                    }
                }
            }

        }

        /// <summary>
        /// Event quand des changements sont lancés
        /// </summary>
        private static void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            ProcessChange(e.FullPath);
        }

        /// <summary>
        /// Event quand des changements sont lancés
        /// </summary>
        private static void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            ProcessChange(e.FullPath);
        }

        /// <summary>
        /// Process le changement
        /// </summary>
        private static void ProcessChange(string fullPath)
        {
            lock (ListChanges)
            {
                _lastUpdatedContent = DateTime.Now;

                //Ajout de l'asset modifié...
                string extension = Path.GetExtension(fullPath).ToLower();

                if (EXTENSIONS.Contains(extension))
                {
                    
                    if (!ListChanges.Contains(fullPath))
                        ListChanges.Add(fullPath);

                    if (_taskUpdateContent == null)
                    {
                        _taskUpdateContent = Task.Factory.StartNew(TaskWaitBeforeForNotification);
                    }
                }
            }
        }

        /// <summary>
        /// Permet d'attendre que les updates soient terminées pour éviter de lancer pleins de refresh de content
        /// </summary>
        private static void TaskWaitBeforeForNotification()
        {
            try
            {
                System.Threading.Thread.Sleep(100);

                //On va surement avoir plus d'un event, on va attendre le dernier event
                while(DateTime.Now.Subtract(_lastUpdatedContent).TotalMilliseconds < 100)
                    System.Threading.Thread.Sleep(10);


                //On indique qu'on a du contenu à reloader
                _contentToReload = true;


            }
            finally
            {
                _taskUpdateContent = null;
            }
        }
    }
}
