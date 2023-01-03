using System;
using System.Reflection;

namespace FNAEngine2D.Network.Commands
{
    [Command(65501)]
    public class SpawnObjectCommand: IClientCommand
    {
        ///// <summary>
        ///// Content data for the game object to spawn
        ///// </summary>
        //public GameContentObject Content { get; set; }

        /// <summary>
        /// GameObject
        /// </summary>
        public NetworkGameObject GameObject { get; set; }

        /// <summary>
        /// Serialize
        /// </summary>
        public void Serialize(BinWriter writer)
        {
            NetworkObjectAttribute attribute = this.GameObject.GetType().GetCustomAttribute<NetworkObjectAttribute>();

            if (attribute == null)
                throw new InvalidOperationException("Invalid NetworkGameObject to spawn, no NetworkObject attribute found on " + this.GameObject.GetType().FullName);


            writer.Write(attribute.Number);
            writer.Write(this.GameObject.ID);
            writer.Write(this.GameObject.Location);

            if (this.GameObject is INetworkSerializable)
                ((INetworkSerializable)this.GameObject).Serialize(writer);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public void Deserialize(BinReader reader)
        {
            ushort objectNumber = reader.ReadUInt16();

            this.GameObject = NetworkObjectHelper.Create(objectNumber);

            this.GameObject.ID = reader.ReadGuid();
            this.GameObject.Location = reader.ReadVector2();

            if (this.GameObject is INetworkSerializable)
                ((INetworkSerializable)this.GameObject).Deserialize(reader);

        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public virtual void ExecuteClient(NetworkClient client)
        {
            //client.AddGameObject((NetworkGameObject)client.GameContentManager.CreateGameObject(this.Content));
            client.AddGameObject(this.GameObject);
        }

        /// <summary>
        /// Override of the ToString to help debug
        /// </summary>
        public override string ToString()
        {
            return "SpawnObjectCommand - " + this.GameObject;
        }
    }
}
