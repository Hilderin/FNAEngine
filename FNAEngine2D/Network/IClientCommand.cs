using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{
    public interface IClientCommand: ICommand
    {
        /// <summary>
        /// Execute the command
        /// </summary>
        void ExecuteClient(NetworkClient client);
    }
}
