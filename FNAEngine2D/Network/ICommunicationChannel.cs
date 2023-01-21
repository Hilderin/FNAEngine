using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Network
{

    /// <summary>
    /// Status of a channel
    /// </summary>
    public enum ChannelState
    {
        NotConnected,
        Connecting,
        Connected,
        Error,
        Disconnected
    }




    public interface ICommunicationChannel
    {
        /// <summary>
        /// Indicate the state of the channel
        /// </summary>
        ChannelState State { get; }

        /// <summary>
        /// Error detail
        /// </summary>
        string Error { get; }

        /// <summary>
        /// Check if commands available for reading
        /// </summary>
        bool Available { get; }

        /// <summary>
        /// Connect to the server
        /// </summary>
        void Connect(Action actionAfterConnect);

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Send data
        /// </summary>
        void Send(ICommand command);

        /// <summary>
        /// Read the next data
        /// </summary>
        ICommand ReadObject();

        /// <summary>
        /// Read the next data
        /// </summary>
        T Read<T>() where T : ICommand;

        /// <summary>
        /// Wait and read the next data
        /// </summary>
        ICommand WaitNextObject();

        /// <summary>
        /// Wait and read the next data
        /// </summary>
        T WaitNext<T>() where T : ICommand;

        /// <summary>
        /// Action on error
        /// </summary>
        Action<Exception> OnError { get; set; }
    }
}
