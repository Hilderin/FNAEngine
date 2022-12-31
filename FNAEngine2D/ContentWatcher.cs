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

    ///// <summary>
    ///// Delegate lancés quand des changements se font
    ///// </summary>
    ///// <param name="assetName"></param>
    //internal delegate void ContentChangedHandler(string assetName);

    /// <summary>
    /// Content Manager
    /// </summary>
    public class ContentWatcher
    {

        /// <summary>
        /// Extensions traités
        /// </summary>
        private readonly string[] EXTENSIONS = new string[] { ".jpg", ".jpeg", ".wav", ".ogg", ".png", ".json", ".aseprite" };

        /// <summary>
        /// Game
        /// </summary>
        private Game _game;

        /// <summary>
        /// Date et heure du dernier changement de content
        /// </summary>
        private DateTime _lastUpdatedContent = DateTime.MinValue;

        /// <summary>
        /// Task pour l'update du contenu
        /// </summary>
        private Task _taskUpdateContent = null;

        /// <summary>
        /// Liste des changements
        /// </summary>
        private List<string> ListChanges = new List<string>();

        /// <summary>
        /// Indique si du contenu doit être reloadé
        /// </summary>
        public bool _contentToReload = false;


        /// <summary>
        /// Indicate if we should watch the modifications
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContentWatcher(Game game)
        {
            _game = game;
        }

        /// <summary>
        /// Permet de vérifier si du contenu change
        /// </summary>
        public void StartWatchUpdateContent()
        {
            if (GameManager.DevelopmentMode)
            {
                if (Directory.Exists(ContentManager.ContentFolder))
                {
                    FileSystemWatcher fsw = new FileSystemWatcher(ContentManager.ContentFolder);
                    fsw.IncludeSubdirectories = true;

                    fsw.Changed += Fsw_Changed;
                    fsw.Created += Fsw_Changed;
                    fsw.Deleted += Fsw_Changed;
                    fsw.Renamed += Fsw_Renamed;

                    fsw.EnableRaisingEvents = true;
                }
            }

        }

        /// <summary>
        /// Permet de reloader le contenu modifié
        /// </summary>
        public void ReloadModifiedContent()
        {
            if (!_contentToReload)
                return;

            ReloadModifiedContentInternal();

            _contentToReload = false;

        }

        /// <summary>
        /// Permet de reloader le contenu modifié
        /// </summary>
        private void ReloadModifiedContentInternal()
        {
            List<string> changes;

            lock (ListChanges)
            {
                changes = new List<string>(ListChanges);
                ListChanges.Clear();
            }

            foreach (string fullPath in changes)
            {
                //Reload the asset...
                _game.ContentManager.Reload(fullPath);
            }

        }

        /// <summary>
        /// Event quand des changements sont lancés
        /// </summary>
        private void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            ProcessChange(e.FullPath);
        }

        /// <summary>
        /// Event quand des changements sont lancés
        /// </summary>
        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            ProcessChange(e.FullPath);
        }

        /// <summary>
        /// Process le changement
        /// </summary>
        private void ProcessChange(string fullPath)
        {
            if (!Enabled)
                return;


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
        private void TaskWaitBeforeForNotification()
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
