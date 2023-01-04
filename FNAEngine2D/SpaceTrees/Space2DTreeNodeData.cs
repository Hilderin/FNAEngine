using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.SpaceTrees
{

    /// <summary>
    /// Internal struc to keep the data in the bucket
    /// </summary>
    internal class Space2DTreeNodeData<T>
    {
        public float X;
        public float Y;
        //public float Width;
        //public float Height;
        public float Right;
        public float Bottom;
        public T Data;
        public Space2DTreeNode<T> ContainerNode { get; set; }

        public Space2DTreeNodeData(float x, float y, float width, float height, T data)
        {
            X = x;
            Y = y;
            //Width = width;
            //Height = height;
            Right = x + width;
            Bottom = y + height;
            Data = data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(float x, float y, float right, float bottom)
        {
            return (x <= Right && right >= X
                    && y <= Bottom && bottom >= Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Intersects(Space2DTreeNodeData<T> dataB)
        {
            return (dataB.X <= Right && dataB.Right >= X
                    && dataB.Y <= Bottom && dataB.Bottom >= Y);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", Right: " + Right + ", Bottom: " + Bottom + ") " + Data.ToString();
        }
    }
}
