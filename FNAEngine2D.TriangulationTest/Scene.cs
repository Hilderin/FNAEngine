using FNAEngine2D;
using FNAEngine2D.GameObjects;
using FNAEngine2D.Geometry;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Reflection;

namespace FNAEngine2D.TriangulationTest
{

    public class Scene : GameObject, IUpdate
    {

        /// <summary>
        /// Cursor
        /// </summary>
        private GameObject _mouseCursor;

        ///// <summary>
        ///// Line
        ///// </summary>
        //private GameObject _line;

        ///// <summary>
        ///// Text for debugging
        ///// </summary>
        //private TextRender _debugText;

        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            Add(new FPSRender());

            _mouseCursor = Add(new PrimitiveRender(PrimitiveType.RectangleFill, Vector2.Zero, Vector2.One, Color.White));

            //_line = Add(new LineRender());
            ////_line.TranslateTo(this.Game.CenterX, this.Game.CenterY);
            //_line.TranslateTo(100, 100);
            //_line.Size = new Vector2(this.Game.CenterX, this.Game.CenterY);

            //_debugText = Add(new TextRender());
            //_debugText.TranslateTo(800, 0);
            //_debugText.Color = Color.White;

            //var triangleRender = new TriangleRender();
            //triangleRender.Color = Color.Green;
            //triangleRender.Triangle = new Triangle(new Vector2(200, 200), new Vector2(400, 400), new Vector2(100, 600));
            //Add(triangleRender);

            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(100, 100));
            points.Add(new Vector2(400, 400));
            points.Add(new Vector2(200, 600));
            points.Add(new Vector2(1000, 400));
            points.Add(new Vector2(1000, 200));

            List<Vector2> constraints = new List<Vector2>();
            constraints.Add(new Vector2(300, 680));
            constraints.Add(new Vector2(380, 680));
            constraints.Add(new Vector2(400, 300));
            constraints.Add(new Vector2(300, 300));


            //points.AddRange(constraints);

            //List<Triangle> triangles = GeometryHelper.TriangulateByFlippingEdges(points);
            List<Triangle> triangles = ConstrainedDelaunay.GenerateTriangulation(points, constraints);

            foreach (Triangle triangle in triangles)
                Add(new TriangleRender(triangle, new Color(GameMath.RandomFloat(0.5f, 1f), GameMath.RandomFloat(0.5f, 1f), GameMath.RandomFloat(0.5f, 1f)), 1f));

        }

        /// <summary>
        /// Update each frame
        /// </summary>
        public void Update()
        {
            _mouseCursor.Location = this.Input.GetMousePosition();
            //_line.Size = (_line.Location - this.Input.GetMousePosition());

            //_debugText.Text = "IsAPointLeftOfVectorOrOnTheLine: " + GeometryHelper.IsAPointLeftOfVectorOrOnTheLine(_line.Location, _line.Location + _line.Size, _mouseCursor.Location).ToString();





        }
    }
}
