namespace FNAEngine2D.Network.Commands
{
    [Command(100)]
    public class SpawnPlayerObjectCommand : IClientCommand
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
            NetworkGameObject obj = (NetworkGameObject)client.GameContentManager.CreateGameObject(this.Content);
            obj.IsLocalPlayer = true;
            client.AddGameObject(obj);
        }

        /// <summary>
        /// Override of the ToString to help debug
        /// </summary>
        public override string ToString()
        {
            return "SpawnPlayerObjectCommand - " + this.Content.ClassName;
        }
    }
}
