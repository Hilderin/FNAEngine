using FNAEngine2D.SpaceTrees;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.Space2DTreeTest
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {

                List<Data> datas = new List<Data>();

                for (int cpt = 0; cpt < 1000; cpt++)
                    datas.Add(new Data(GameMath.RandomFloat(0, 10000), GameMath.RandomFloat(0, 10000), GameMath.RandomFloat(0, 100), GameMath.RandomFloat(0, 100), "Data " + cpt));

                Space2DTree<Data> spaceTree = new Space2DTree<Data>();

                //Insertion...
                Stopwatch timer = Stopwatch.StartNew();
                foreach (Data data in datas)
                    spaceTree.Add(data.X, data.Y, data.Width, data.Heigth, data);
                timer.Stop();
                Console.WriteLine("Insertion time: " + timer.ElapsedMilliseconds);


                //Search creations...
                List<Data> searchs = new List<Data>();
                for (int cpt = 0; cpt < 1000; cpt++)
                    searchs.Add(new Data(GameMath.RandomFloat(0, 10000), GameMath.RandomFloat(0, 10000), GameMath.RandomFloat(0, 100), GameMath.RandomFloat(0, 100), "Search " + cpt));


                //Search with tree...
                timer.Restart();
                int resultCountTree = 0;
                foreach(Data search in searchs)
                {
                    var result = spaceTree.GetValues(search.X, search.Y, search.Width, search.Heigth);
                    resultCountTree += result.Count;
                }
                timer.Stop();
                Console.WriteLine("Search time tree: " + timer.ElapsedMilliseconds + ", total count: " + resultCountTree);


                //Serach with list...
                timer.Restart();

                //Search with tree...
                timer.Restart();
                resultCountTree = 0;
                foreach (Data search in searchs)
                {
                    resultCountTree += spaceTree.Search(search.X, search.Y, search.Width, search.Heigth).Count();
                }
                timer.Stop();
                Console.WriteLine("Search time tree enumerator: " + timer.ElapsedMilliseconds + ", total count: " + resultCountTree);


                //Serach with list...
                timer.Restart();


                int resultCountList = 0;
                foreach (Data search in searchs)
                {
                    resultCountList += datas.Where(d => search.Intersects(ref d)).Count();
                }
                timer.Stop();
                Console.WriteLine("Search time list: " + timer.ElapsedMilliseconds + ", total count: " + resultCountTree);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }
            Console.ReadKey();
        }


        /// <summary>
        /// Simple Data test class
        /// </summary>
        private class Data
        {
            public float X;
            public float Y;
            public float Width;
            public float Heigth;
            public string Name;

            public Data(float x, float y, float width, float heigth, string name)
            {
                X = x;
                Y = y;
                Width = width;
                Heigth = heigth;
                Name = name;
            }

            public override string ToString()
            {
                return "(" + X + ", " + Y + ") " + Name;
            }

            /// <summary>
            /// Check if 2 rectangles reprensented by locations and size intersects
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Intersects(ref Data dataB)
            {
                return (dataB.X <= (this.X + this.Width) && (dataB.X + dataB.Width) >= this.X
                        && dataB.Y <= (this.Y + this.Heigth) && (dataB.Y + dataB.Heigth) >= this.Y);
            }
        }

    }
}
