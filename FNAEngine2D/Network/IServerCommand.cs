using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{
    public interface IServerCommand : ICommand
    {
        /// <summary>
        /// Execute the command
        /// </summary>
        void ExecuteServer(ServerCommandArgs args);
    }
}
