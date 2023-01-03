using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{

    /// <summary>
    /// NetworkObject attribute to specify the unique number of an object
    /// </summary>
    public class NetworkObjectAttribute : Attribute
    {
        /// <summary>
        /// Object Number
        /// </summary>
        public ushort Number { get; private set; }

        /// <summary>
        /// Object number
        /// </summary>
        public NetworkObjectAttribute(ushort number)
        {
            this.Number = number;
        }
    }
}
