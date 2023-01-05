using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Container for GameContent loaded
    /// </summary>
    public class GameContentContainer: GameObject
    {
        /// <summary>
        /// Asset name
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// GameContent
        /// </summary>
        public Content<GameContent> GameContent { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public GameContentContainer()
        {

        }


        /// <summary>
        /// Constructor
        /// </summary>
        public GameContentContainer(string assetName)
        {
            this.AssetName = assetName;
            
        }

        /// <summary>
        /// Load content
        /// </summary>
        protected override void Load()
        {
            this.GameContent = GetContent<GameContent>(this.AssetName);

            GameContentManager.ReplaceContent(this, this.GameContent.Data);
        }

    }
}
