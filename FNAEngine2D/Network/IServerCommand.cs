namespace FNAEngine2D.Network
{
    /// <summary>
    /// Interface for a command executable on the server
    /// </summary>
    public interface IServerCommand
    {
        /// <summary>
        /// Execute the command
        /// </summary>
        void ExecuteServer(ClientWorker cw);
    }
}
