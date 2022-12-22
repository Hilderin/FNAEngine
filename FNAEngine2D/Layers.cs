using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Flags for Layers
    /// </summary>
    [Flags]
    public enum Layers : uint
    {
        None = 0,           //No layer
        All = 0xFFFFFFFF,   //All layers
        Layer1 = 1 << 0,    //1
        Layer2 = 1 << 1,    //2
        Layer3 = 1 << 2,    //4
        Layer4 = 1 << 3,    //8
        Layer5 = 1 << 4,    //16
        Layer6 = 1 << 5,    //32
        Layer7 = 1 << 6,    //64
        Layer8 = 1 << 9,
        Layer9 = 1 << 10,
        Layer10 = 1 << 11,
        Layer11 = 1 << 12,
        Layer12 = 1 << 13,
        Layer13 = 1 << 14,
        Layer14 = 1 << 15,
        Layer15 = 1 << 16,
        Layer16 =  1 << 17,
        Layer17 = 1 << 18,
        Layer18 = 1 << 19,
        Layer19 = 1 << 20,
        Layer20 = 1 << 21,
        Layer21 = 1 << 22,
        Layer22 = 1 << 23,
        Layer23 = 1 << 24,
        Layer24 = 1 << 25,
        Layer25 = 1 << 26,
        Layer26 = 1 << 27,
        Layer27 = 1 << 28,
        Layer28 = 1 << 29,
        Layer29 = 1 << 30
        
    }
}
