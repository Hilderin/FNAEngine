using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{
    /// <summary>
    /// Command basic
    /// </summary>
    public interface ICommand: INetworkSerializable
    {
        Guid ID { get; set; }
    }
}
