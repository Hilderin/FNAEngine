using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Container for a content
    /// </summary>
    public class Content<T>
    {
        /// <summary>
        /// Data
        /// </summary>
        public T Data;

        /// <summary>
        /// Constructor
        /// </summary>
        public Content(T data)
        {
            this.Data = data;
        }

    }
}
