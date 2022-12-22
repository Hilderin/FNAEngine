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
        public string AssetName { get; set; }

        public Content<GameContent> GetContent { get; set; }


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
        public override void Load()
        {
            this.GetContent = GameHost.GetContent<GameContent>(this.AssetName);

            GameContentManager.ReplaceContent(this, this.GetContent.Data);
        }

    }
}
