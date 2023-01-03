namespace FNAEngine2D.Network
{
    /// <summary>
    /// Interface for a command executable on the client
    /// </summary>
    public interface IClientCommand : ICommand
    {
        /// <summary>
        /// Execute the command
        /// </summary>
        void ExecuteClient(NetworkClient client);


    }
}
