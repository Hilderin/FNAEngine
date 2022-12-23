using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    public class TileGameObject: GameObject
    {
        public TileGameObject()
        {

        }

        public TileGameObject(Rectangle bounds): base(bounds)
        {
            
        }

        public override void Load()
        {
            this.EnableCollider();
        }

    }
}
