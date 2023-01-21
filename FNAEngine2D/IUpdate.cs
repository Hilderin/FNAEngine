using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public interface IUpdate
    {

        /// <summary>
        /// Order update
        /// </summary>
        float UpdateOrder { get; }

        /// <summary>
        /// Update method
        /// </summary>
        void Update();
    }
}
