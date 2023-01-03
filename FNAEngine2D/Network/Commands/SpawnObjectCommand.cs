namespace FNAEngine2D.Network.Commands
{
    [Command(65501)]
    public class SpawnObjectCommand: IClientCommand
    {
        /// <summary>
        /// Content data for the game object to spawn
        /// </summary>
        public GameContentObject Content { get; set; }

        /// <summary>
        /// Serialize
        /// </summary>
        public void Serialize(BinWriter writer)
        {
            writer.WriteObject(Content);
        }

        /// <summary>
        /// Deserialize
        /// </summary>
        public void Deserialize(BinReader reader)
        {
            this.Content = reader.ReadObject<GameContentObject>();
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void ExecuteClient(NetworkClient client)
        {
            client.AddGameObject((NetworkGameObject)client.GameContentManager.CreateGameObject(this.Content));
        }

        /// <summary>
        /// Override of the ToString to help debug
        /// </summary>
        public override string ToString()
        {
            return "SpawnObjectCommand - " + this.Content.ClassName;
        }
    }
}
