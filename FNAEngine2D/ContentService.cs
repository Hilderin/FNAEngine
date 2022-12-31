using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Content service
    /// </summary>
    public class ContentService
    {
        /// <summary>
        /// Game
        /// </summary>
        private Game _game;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContentService(Game game)
        {
            _game = game;

            //Add to the service provider...
            _game.Services.AddService(typeof(ContentService), this);
        }

        /// <summary>
        /// Returns an assent content
        /// </summary>
        public Content<T> GetContent<T>(string assetName)
        {
            return _game.ContentManager.GetContent<T>(assetName);
        }
    }
}
