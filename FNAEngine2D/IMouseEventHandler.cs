using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public interface IMouseEventHandler
    {

        void HandleMouseEvent(MouseAction action);
    }
}
