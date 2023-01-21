using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{
    public class ServerCommandArgs
    {
        public NetworkGameObject GameObject { get; set; }
        
        public Guid ConnectionID { get; private set; }

        public ServerCommandArgs(Guid connectionID)
        {
            this.ConnectionID = connectionID;
        }

        public ServerCommandArgs(Guid connectionID, NetworkGameObject gameObject)
        {
            this.ConnectionID = connectionID;
            this.GameObject = gameObject;
        }
    }
}
