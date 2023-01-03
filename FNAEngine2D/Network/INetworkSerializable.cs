using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{
    public interface INetworkSerializable
    {
        /// <summary>
        /// Serialization of the object
        /// </summary>
        void Serialize(BinWriter writer);

        /// <summary>
        /// Deserialization of the object
        /// </summary>
        void Deserialize(BinReader reader);
    }
}
