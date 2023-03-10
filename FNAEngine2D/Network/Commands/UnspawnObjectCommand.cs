using System;
using System.Reflection;

namespace FNAEngine2D.Network.Commands
{
    public class UnspawnObjectCommand : ClientCommand
    {
        /// <summary>
        /// Serialize
        /// </summary>
        public override void Serialize(BinWriter writer)
        {
            
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public override void Deserialize(BinReader reader)
        {
            
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public override void ExecuteClient(NetworkClient client)
        {
            client.RemoveGameObject(this.ID);
        }

        /// <summary>
        /// Override of the ToString to help debug
        /// </summary>
        public override string ToString()
        {
            return "UnspawnObjectCommand - " + this.ID;
        }
    }
}
