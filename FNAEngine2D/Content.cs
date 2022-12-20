using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Container for a content
    /// </summary>
    public class Content<T>
    {
        /// <summary>
        /// Data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// AssetName
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Content(T data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Content(string assetName, T data)
        {
            this.AssetName = assetName;
            this.Data = data;
        }

    }
}
