using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Definition of a GameObject
    /// </summary>
    public class GameContentObject
    {
        /// <summary>
        /// Class name of the game objet
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Properties
        /// </summary>
        public Dictionary<string, object> Props;

    }
}
