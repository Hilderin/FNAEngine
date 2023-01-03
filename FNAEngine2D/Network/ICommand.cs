using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{
    public interface ICommand
    {
        /// <summary>
        /// Serialization of the command
        /// </summary>
        void Serialize(BinWriter writer);

        /// <summary>
        /// Deserialization of the command
        /// </summary>
        void Deserialize(BinReader reader);

    }
}
