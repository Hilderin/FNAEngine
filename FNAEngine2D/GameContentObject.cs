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
    public class GameContentObject: Dictionary<string, object>
    {
        /// <summary>
        /// Class name of the game objet
        /// </summary>
        public string ClassName
        {
            get
            {
                if (this.TryGetValue("ClassName", out var className))
                    return className.ToString();
                return String.Empty;
            }
        }


    }
}
