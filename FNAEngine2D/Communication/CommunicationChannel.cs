using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Communication
{

    /// <summary>
    /// Status of a channel
    /// </summary>
    public enum ChannelState
    {
        NotConnected,
        Connected,
        Connecting,
        Error,
        Disconnected
    }




    public interface CommunicationChannel
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
        void Send(object data);

        /// <summary>
        /// Read the next data
        /// </summary>
        object ReadObject();

        /// <summary>
        /// Read the next data
        /// </summary>
        T Read<T>();

        /// <summary>
        /// Wait and read the next data
        /// </summary>
        object WaitNextObject();

        /// <summary>
        /// Wait and read the next data
        /// </summary>
        T WaitNext<T>();
    }
}
