using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    [Flags]
    public enum Layers
    {
        Layer1 = 1 << 0,    //1
        Layer2 = 1 << 1,    //2
        Layer3 = 1 << 2,    //4
        Layer4 = 1 << 3,    //8
        Layer5 = 1 << 4,    //16
        Layer6 = 1 << 5,    //32
        Layer7 = 1 << 6,    //64
    }
}
