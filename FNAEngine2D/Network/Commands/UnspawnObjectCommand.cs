using System;
using System.Reflection;

namespace FNAEngine2D.Network.Commands
{
    public class UnspawnObjectCommand : IClientCommand
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Serialize
        /// </summary>
        public void Serialize(BinWriter writer)
        {
            writer.Write(this.ID);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public void Deserialize(BinReader reader)
        {
            this.ID = reader.ReadGuid();
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public virtual void ExecuteClient(NetworkClient client)
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
