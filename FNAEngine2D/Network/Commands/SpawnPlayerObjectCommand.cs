namespace FNAEngine2D.Network.Commands
{
    [Command(65500)]
    public class SpawnPlayerObjectCommand : SpawnObjectCommand
    {
        /// <summary>
        /// Execute the command
        /// </summary>
        public override void ExecuteClient(NetworkClient client)
        {
            this.GameObject.IsLocalPlayer = true;
            client.AddGameObject(this.GameObject);
        }

        /// <summary>
        /// Override of the ToString to help debug
        /// </summary>
        public override string ToString()
        {
            return "SpawnPlayerObjectCommand - " + this.GameObject;
        }
    }
}
