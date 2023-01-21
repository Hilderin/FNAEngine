using FNAEngine2D.Physics;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace FNAEngine2D.Network.Commands
{
    public class NewConnectionCommand : ClientCommand
    {
        /// <summary>
        /// Connection id
        /// </summary>
        public Guid ConnectionID { get; set; }


        /// <summary>
        /// Serialize
        /// </summary>
        public override void Serialize(BinWriter writer)
        {
            writer.Write(this.ConnectionID);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public override void Deserialize(BinReader reader)
        {
            this.ConnectionID = reader.ReadGuid();
        }



        /// <summary>
        /// Execute
        /// </summary>
        public override void ExecuteClient(NetworkClient client)
        {
            client.ConnectionID = this.ConnectionID;

            if (client.OnConnected != null)
                client.OnConnected();
        }


        /// <summary>
        /// Override of the ToString to help debug
        /// </summary>
        public override string ToString()
        {
            return "NewConnectionCommand - ConnectionID: " + this.ConnectionID;
        }
    }
}
