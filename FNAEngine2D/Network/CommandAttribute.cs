using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{
    
    /// <summary>
    /// Command attribute
    /// </summary>
    public class CommandAttribute: Attribute
    {
        /// <summary>
        /// Command Number
        /// </summary>
        public short Number { get; private set; }

        /// <summary>
        /// Command number
        /// </summary>
        public CommandAttribute(short number)
        {
            this.Number = number;
        }
    }
}
