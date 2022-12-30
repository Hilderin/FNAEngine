using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Communication
{
    public interface CommunicationChannel
    {
        /// <summary>
        /// Indicate if the communication is opened
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Check if commands available for reading
        /// </summary>
        bool Available { get; }

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
