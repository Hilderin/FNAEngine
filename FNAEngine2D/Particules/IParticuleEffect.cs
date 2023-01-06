using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Particules
{
    /// <summary>
    /// Interface for a particule effect
    /// </summary>
    public interface IParticuleEffect
    {
        /// <summary>
        /// Apply the effect
        /// </summary>
        void Apply(Particule particule);

    }
}
