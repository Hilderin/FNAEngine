namespace FNAEngine2D.Network.Commands
{
    [Command(101)]
    public class SpawnObjectCommand: IClientCommand
    {
        /// <summary>
        /// Content data for the game object to spawn
        /// </summary>
        public GameContentObject Content { get; set; }

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
