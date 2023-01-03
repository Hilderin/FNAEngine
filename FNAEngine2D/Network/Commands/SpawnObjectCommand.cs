using System;
using System.Reflection;

namespace FNAEngine2D.Network.Commands
{
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
            writer.Write(NetworkObjectHelper.GetObjectNumber(this.GameObject.GetType()));
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
