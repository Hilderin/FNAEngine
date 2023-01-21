using System;

namespace FNAEngine2D.Network
{
    /// <summary>
    /// Interface for a command executable on the client
    /// </summary>
    public abstract class ClientCommand : IClientCommand
    {
        /// <summary>
        /// ID de game
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Execute the command
        /// </summary>
        public abstract void ExecuteClient(NetworkClient client);

        /// <summary>
        /// Serialization of the object
        /// </summary>
        public abstract void Serialize(BinWriter writer);

        /// <summary>
        /// Deserialization of the object
        /// </summary>
        public abstract void Deserialize(BinReader reader);
    }
}
