using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.TileSets
{
    public class TileGameObject: GameObject
    {
        public TileGameObject()
        {

        }

        public TileGameObject(Rectangle bounds): base(bounds)
        {
            
        }

        protected override void Load()
        {
            this.EnableCollider();
        }

    }
}
