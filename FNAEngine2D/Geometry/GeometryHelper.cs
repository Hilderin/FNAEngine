using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace FNAEngine2D.Geometry
{
    /// <summary>
    /// Helper to helper with triangle and geometry
    /// </summary>
    public static class GeometryHelper
    {
        /// <summary>
        ///Where is p in relation to a-b
        /// < 0 -> to the right
        /// = 0 -> on the line
        /// > 0 -> to the left
        /// </summary>
        public static float IsAPointLeftOfVectorOrOnTheLine(Vector2 a, Vector2 b, Vector2 p)
        {
            float determinant = (a.X - p.X) * (b.Y - p.Y) - (a.Y - p.Y) * (b.X - p.X);

            return determinant;
        }

        /// <summary>
        /// Alternative 1. Triangulate with some algorithm - then flip edges until we have a delaunay triangulation
        /// </summary>
        public static List<Triangle> TriangulateByFlippingEdges(List<Vector2> sites)
        {
            //Step 1. Triangulate the points with some algorithm
            //Vector2 to vertex
            List<Vertex> vertices = new List<Vertex>();

            for (int i = 0; i < sites.Count; i++)
            {
                vertices.Add(new Vertex(sites[i]));
            }

            //Triangulate the convex hull of the sites
            List<Triangle> triangles = TriangulatePointsIncremental(vertices);
            //List triangles = TriangulatePoints.TriangleSplitting(vertices);

            //Step 2. Change the structure from triangle to half-edge to make it faster to flip edges
            List<HalfEdge> halfEdges = TransformFromTriangleToHalfEdge(triangles);

            //Step 3. Flip edges until we have a delaunay triangulation
            int safety = 0;

            int flippedEdges = 0;

            while (true)
            {
                safety += 1;

                if (safety > 100000)
                {
                    //Debug.Log("Stuck in endless loop");

                    break;
                }

                bool hasFlippedEdge = false;

                //Search through all edges to see if we can flip an edge
                for (int i = 0; i < halfEdges.Count; i++)
                {
                    HalfEdge thisEdge = halfEdges[i];

                    //Is this edge sharing an edge, otherwise its a border, and then we cant flip the edge
                    if (thisEdge.oppositeEdge == null)
                    {
                        continue;
                    }

                    //The vertices belonging to the two triangles, c-a are the edge vertices, b belongs to this triangle
                    Vector2 aPos = thisEdge.v.position;
                    Vector2 bPos = thisEdge.nextEdge.v.position;
                    Vector2 cPos = thisEdge.prevEdge.v.position;
                    Vector2 dPos = thisEdge.oppositeEdge.nextEdge.v.position;

                    //Use the circle test to test if we need to flip this edge
                    if (IsPointInsideOutsideOrOnCircle(aPos, bPos, cPos, dPos) < 0f)
                    {
                        //Are these the two triangles that share this edge forming a convex quadrilateral?
                        //Otherwise the edge cant be flipped
                        if (IsQuadrilateralConvex(aPos, bPos, cPos, dPos))
                        {
                            //If the new triangle after a flip is not better, then dont flip
                            //This will also stop the algoritm from ending up in an endless loop
                            if (IsPointInsideOutsideOrOnCircle(bPos, cPos, dPos, aPos) < 0f)
                            {
                                continue;
                            }

                            //Flip the edge
                            flippedEdges += 1;

                            hasFlippedEdge = true;

                                FlipEdge(thisEdge);
                        }
                    }
                }

                //We have searched through all edges and havent found an edge to flip, so we have a Delaunay triangulation!
                if (!hasFlippedEdge)
                {
                    //Debug.Log("Found a delaunay triangulation");

                    break;
                }
            }

            //Debug.Log("Flipped edges: " + flippedEdges);

            //Dont have to convert from half edge to triangle because the algorithm will modify the objects, which belongs to the 
            //original triangles, so the triangles have the data we need

            return triangles;
        }

        /// <summary>
        /// Is a triangle in 2d space oriented clockwise or counter-clockwise
        /// https://math.stackexchange.com/questions/1324179/how-to-tell-if-3-connected-points-are-connected-clockwise-or-counter-clockwise
        /// https://en.wikipedia.org/wiki/Curve_orientation
        /// </summary>
        public static bool IsTriangleOrientedClockwise(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            bool isClockWise = true;

            float determinant = p1.X * p2.Y + p3.X * p1.Y + p2.X * p3.Y - p1.X * p3.Y - p3.X * p2.Y - p2.X * p1.Y;

            if (determinant > 0f)
            {
                isClockWise = false;
            }

            return isClockWise;
        }

        /// <summary>
        /// Is a quadrilateral convex? Assume no 3 points are colinear and the shape doesnt look like an hourglass
        /// </summary>
        public static bool IsQuadrilateralConvex(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            bool isConvex = false;

            bool abc = IsTriangleOrientedClockwise(a, b, c);
            bool abd = IsTriangleOrientedClockwise(a, b, d);
            bool bcd = IsTriangleOrientedClockwise(b, c, d);
            bool cad = IsTriangleOrientedClockwise(c, a, d);

            if (abc && abd && bcd & !cad)
            {
                isConvex = true;
            }
            else if (abc && abd && !bcd & cad)
            {
                isConvex = true;
            }
            else if (abc && !abd && bcd & cad)
            {
                isConvex = true;
            }
            //The opposite sign, which makes everything inverted
            else if (!abc && !abd && !bcd & cad)
            {
                isConvex = true;
            }
            else if (!abc && !abd && bcd & !cad)
            {
                isConvex = true;
            }
            else if (!abc && abd && !bcd & !cad)
            {
                isConvex = true;
            }


            return isConvex;
        }


        /// <summary>
        /// From http://totologic.blogspot.se/2014/01/accurate-point-in-triangle-test.html
        /// p is the testpoint, and the other points are corners in the triangle
        /// </summary>
        public static bool IsPointInTriangle(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p)
        {
            bool isWithinTriangle = false;

            //Based on Barycentric coordinates
            float denominator = ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));

            float a = ((p2.Y - p3.Y) * (p.X - p3.X) + (p3.X - p2.X) * (p.Y - p3.Y)) / denominator;
            float b = ((p3.Y - p1.Y) * (p.X - p3.X) + (p1.X - p3.X) * (p.Y - p3.Y)) / denominator;
            float c = 1 - a - b;

            //The point is within the triangle or on the border if 0 <= a <= 1 and 0 <= b <= 1 and 0 <= c <= 1
            //if (a >= 0f && a <= 1f && b >= 0f && b <= 1f && c >= 0f && c <= 1f)
            //{
            //    isWithinTriangle = true;
            //}

            //The point is within the triangle
            if (a > 0f && a < 1f && b > 0f && b < 1f && c > 0f && c < 1f)
            {
                isWithinTriangle = true;
            }

            return isWithinTriangle;
        }

        /// <summary>
        /// http://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/
        /// </summary>
        public static bool AreLinesIntersecting(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints)
        {
            bool isIntersecting = false;

            float denominator = (l2_p2.Y - l2_p1.Y) * (l1_p2.X - l1_p1.X) - (l2_p2.X - l2_p1.X) * (l1_p2.Y - l1_p1.Y);

            //Make sure the denominator is > 0, if not the lines are parallel
            if (denominator != 0f)
            {
                float u_a = ((l2_p2.X - l2_p1.X) * (l1_p1.Y - l2_p1.Y) - (l2_p2.Y - l2_p1.Y) * (l1_p1.X - l2_p1.X)) / denominator;
                float u_b = ((l1_p2.X - l1_p1.X) * (l1_p1.Y - l2_p1.Y) - (l1_p2.Y - l1_p1.Y) * (l1_p1.X - l2_p1.X)) / denominator;

                //Are the line segments intersecting if the end points are the same
                if (shouldIncludeEndPoints)
                {
                    //Is intersecting if u_a and u_b are between 0 and 1 or exactly 0 or 1
                    if (u_a >= 0f && u_a <= 1f && u_b >= 0f && u_b <= 1f)
                    {
                        isIntersecting = true;
                    }
                }
                else
                {
                    //Is intersecting if u_a and u_b are between 0 and 1
                    if (u_a > 0f && u_a < 1f && u_b > 0f && u_b < 1f)
                    {
                        isIntersecting = true;
                    }
                }

            }

            return isIntersecting;
        }

        /// <summary>
        /// Whats the coordinate of an intersection point between two lines in 2d space if we know they are intersecting
        /// http://thirdpartyninjas.com/blog/2008/10/07/line-segment-intersection/
        /// </summary>
        public static Vector2 GetLineLineIntersectionPoint(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2)
        {
            float denominator = (l2_p2.Y - l2_p1.Y) * (l1_p2.X - l1_p1.X) - (l2_p2.X - l2_p1.X) * (l1_p2.Y - l1_p1.Y);

            float u_a = ((l2_p2.X - l2_p1.X) * (l1_p1.Y - l2_p1.Y) - (l2_p2.Y - l2_p1.Y) * (l1_p1.X - l2_p1.X)) / denominator;

            Vector2 intersectionPoint = l1_p1 + u_a * (l1_p2 - l1_p1);

            return intersectionPoint;
        }

        /// <summary>
        /// Is a point to the left, to the right, or on a plane
        /// https://gamedevelopment.tutsplus.com/tutorials/understanding-sutherland-hodgman-clipping-for-physics-engines--gamedev-11917
        /// Notice that the plane normal doesnt have to be normalized
        /// </summary>
        public static float DistanceFromPointToPlane(Vector2 planeNormal, Vector2 planePos, Vector2 pointPos)
        {
            //Positive distance denotes that the point p is on the front side of the plane 
            //Negative means it's on the back side
            float distance = Vector2.Dot(planeNormal, pointPos - planePos);

            return distance;
        }

        /// <summary>
        /// Get the coordinate if we know a ray-plane is intersecting
        /// </summary>
        public static Vector2 GetRayPlaneIntersectionCoordinate(Vector2 planePos, Vector2 planeNormal, Vector2 rayStart, Vector2 rayDir)
        {
            float denominator = Vector2.Dot(-planeNormal, rayDir);

            Vector2 vecBetween = planePos - rayStart;

            float t = Vector2.Dot(vecBetween, -planeNormal) / denominator;

            Vector2 intersectionPoint = rayStart + rayDir * t;

            return intersectionPoint;
        }

        /// <summary>
        /// Is a line-plane intersecting?
        /// </summary>
        public static bool AreLinePlaneIntersecting(Vector2 planeNormal, Vector2 planePos, Vector2 linePos1, Vector2 linePos2)
        {
            bool areIntersecting = false;

            Vector2 lineDir = Vector2.Normalize(linePos1 - linePos2);

            float denominator = Vector2.Dot(-planeNormal, lineDir);

            //No intersection if the line and plane are parallell
            if (denominator > 0.000001f || denominator < -0.000001f)
            {
                Vector2 vecBetween = planePos - linePos1;

                float t = Vector2.Dot(vecBetween, -planeNormal) / denominator;

                Vector2 intersectionPoint = linePos1 + lineDir * t;

                if (IsPointBetweenPoints(linePos1, linePos2, intersectionPoint))
                {
                    areIntersecting = true;
                }
            }

            return areIntersecting;
        }

        /// <summary>
        /// Is a point c between point a and b (we assume all 3 are on the same line)
        /// </summary>
        public static bool IsPointBetweenPoints(Vector2 a, Vector2 b, Vector2 c)
        {
            bool isBetween = false;

            //Entire line segment
            Vector2 ab = b - a;
            //The intersection and the first point
            Vector2 ac = c - a;

            //Need to check 2 things: 
            //1. If the vectors are pointing in the same direction = if the dot product is positive
            //2. If the length of the vector between the intersection and the first point is smaller than the entire line
            if (Vector2.Dot(ab, ac) > 0f && ab.LengthSquared() >= ac.LengthSquared())
            {
                isBetween = true;
            }

            return isBetween;
        }

        /// <summary>
        /// We know a line plane is intersecting and now we want the coordinate of intersection
        /// </summary>
        public static Vector2 GetLinePlaneIntersectionCoordinate(Vector2 planeNormal, Vector2 planePos, Vector2 linePos1, Vector2 linePos2)
        {
            Vector2 vecBetween = planePos - linePos1;

            Vector2 lineDir = Vector2.Normalize(linePos1 - linePos2);

            float denominator = Vector2.Dot(-planeNormal, lineDir);

            float t = Vector2.Dot(vecBetween, -planeNormal) / denominator;

            Vector2 intersectionPoint = linePos1 + lineDir * t;

            return intersectionPoint;
        }

        /// <summary>
        /// The list describing the polygon has to be sorted either clockwise or counter-clockwise because we have to identify its edges
        /// </summary>
        public static bool IsPointInPolygon(List<Vector2> polygonPoints, Vector2 point)
        {
            //Step 1. Find a point outside of the polygon
            //Pick a point with a x position larger than the polygons max x position, which is always outside
            Vector2 maxXPosVertex = polygonPoints[0];

            for (int i = 1; i < polygonPoints.Count; i++)
            {
                if (polygonPoints[i].X > maxXPosVertex.X)
                {
                    maxXPosVertex = polygonPoints[i];
                }
            }

            //The point should be outside so just pick a number to make it outside
            Vector2 pointOutside = maxXPosVertex + new Vector2(10f, 0f);

            //Step 2. Create an edge between the point we want to test with the point thats outside
            Vector2 l1_p1 = point;
            Vector2 l1_p2 = pointOutside;

            //Step 3. Find out how many edges of the polygon this edge is intersecting
            int numberOfIntersections = 0;

            for (int i = 0; i < polygonPoints.Count; i++)
            {
                //Line 2
                Vector2 l2_p1 = polygonPoints[i];

                int iPlusOne = GameMath.ClampListIndex(i + 1, polygonPoints.Count);

                Vector2 l2_p2 = polygonPoints[iPlusOne];

                //Are the lines intersecting?
                if (AreLinesIntersecting(l1_p1, l1_p2, l2_p1, l2_p2, true))
                {
                    numberOfIntersections += 1;
                }
            }

            //Step 4. Is the point inside or outside?
            bool isInside = true;

            //The point is outside the polygon if number of intersections is even or 0
            if (numberOfIntersections == 0 || numberOfIntersections % 2 == 0)
            {
                isInside = false;
            }

            return isInside;
        }

        /// <summary>
        /// Triangulate random points by first generating the convex hull of the points, then triangulate the convex hull
        /// and then add the other points and split the triangle the point is in
        /// </summary>
        public static List<Triangle> TriangulatePoints(List<Vertex> points)
        {
            List<Triangle> triangles = new List<Triangle>();

            //Sort the points along x-axis
            //OrderBy is always soring in ascending order - use OrderByDescending to get in the other order
            points = points.OrderBy(n => n.position.X).ToList();

            //The first 3 vertices are always forming a triangle
            Triangle newTriangle = new Triangle(points[0].position, points[1].position, points[2].position);

            triangles.Add(newTriangle);

            //All edges that form the triangles, so we have something to test against
            List<Edge> edges = new List<Edge>();

            edges.Add(new Edge(newTriangle.v1, newTriangle.v2));
            edges.Add(new Edge(newTriangle.v2, newTriangle.v3));
            edges.Add(new Edge(newTriangle.v3, newTriangle.v1));

            //Add the other triangles one by one
            //Starts at 3 because we have already added 0,1,2
            for (int i = 3; i < points.Count; i++)
            {
                Vector2 currentPoint = points[i].position;

                //The edges we add this loop or we will get stuck in an endless loop
                List<Edge> newEdges = new List<Edge>();

                //Is this edge visible? We only need to check if the midpoint of the edge is visible 
                for (int j = 0; j < edges.Count; j++)
                {
                    Edge currentEdge = edges[j];

                    Vector2 midPoint = (currentEdge.v1.position + currentEdge.v2.position) / 2f;

                    Edge edgeToMidpoint = new Edge(currentPoint, midPoint);

                    //Check if this line is intersecting
                    bool canSeeEdge = true;

                    for (int k = 0; k < edges.Count; k++)
                    {
                        //Dont compare the edge with itself
                        if (k == j)
                        {
                            continue;
                        }

                        if (AreEdgesIntersecting(edgeToMidpoint, edges[k]))
                        {
                            canSeeEdge = false;

                            break;
                        }
                    }

                    //This is a valid triangle
                    if (canSeeEdge)
                    {
                        Edge edgeToPoint1 = new Edge(currentEdge.v1, new Vertex(currentPoint));
                        Edge edgeToPoint2 = new Edge(currentEdge.v2, new Vertex(currentPoint));

                        newEdges.Add(edgeToPoint1);
                        newEdges.Add(edgeToPoint2);

                        Triangle newTri = new Triangle(edgeToPoint1.v1, edgeToPoint1.v2, edgeToPoint2.v1);

                        triangles.Add(newTri);
                    }
                }


                for (int j = 0; j < newEdges.Count; j++)
                {
                    edges.Add(newEdges[j]);
                }
            }


            return triangles;
        }

        /// <summary>
        /// Sort the points along one axis. The first 3 points form a triangle. Consider the next point and connect it with all
        /// previously connected points which are visible to the point. An edge is visible if the center of the edge is visible to the point.
        /// </summary>
        public static List<Triangle> TriangulatePointsIncremental(List<Vertex> points)
        {
            List<Triangle> triangles = new List<Triangle>();

            //Sort the points along x-axis
            //OrderBy is always soring in ascending order - use OrderByDescending to get in the other order
            points = points.OrderBy(n => n.position.X).ToList();

            //The first 3 vertices are always forming a triangle
            Triangle newTriangle = new Triangle(points[0].position, points[1].position, points[2].position);

            triangles.Add(newTriangle);

            //All edges that form the triangles, so we have something to test against
            List<Edge> edges = new List<Edge>();

            edges.Add(new Edge(newTriangle.v1, newTriangle.v2));
            edges.Add(new Edge(newTriangle.v2, newTriangle.v3));
            edges.Add(new Edge(newTriangle.v3, newTriangle.v1));

            //Add the other triangles one by one
            //Starts at 3 because we have already added 0,1,2
            for (int i = 3; i < points.Count; i++)
            {
                Vector2 currentPoint = points[i].position;

                //The edges we add this loop or we will get stuck in an endless loop
                List<Edge> newEdges = new List<Edge>();

                //Is this edge visible? We only need to check if the midpoint of the edge is visible 
                for (int j = 0; j < edges.Count; j++)
                {
                    Edge currentEdge = edges[j];

                    Vector2 midPoint = (currentEdge.v1.position + currentEdge.v2.position) / 2f;

                    Edge edgeToMidpoint = new Edge(currentPoint, midPoint);

                    //Check if this line is intersecting
                    bool canSeeEdge = true;

                    for (int k = 0; k < edges.Count; k++)
                    {
                        //Dont compare the edge with itself
                        if (k == j)
                        {
                            continue;
                        }

                        if (AreEdgesIntersecting(edgeToMidpoint, edges[k]))
                        {
                            canSeeEdge = false;

                            break;
                        }
                    }

                    //This is a valid triangle
                    if (canSeeEdge)
                    {
                        Edge edgeToPoint1 = new Edge(currentEdge.v1, new Vertex(currentPoint));
                        Edge edgeToPoint2 = new Edge(currentEdge.v2, new Vertex(currentPoint));

                        newEdges.Add(edgeToPoint1);
                        newEdges.Add(edgeToPoint2);

                        Triangle newTri = new Triangle(edgeToPoint1.v1, edgeToPoint1.v2, edgeToPoint2.v1);

                        triangles.Add(newTri);
                    }
                }


                for (int j = 0; j < newEdges.Count; j++)
                {
                    edges.Add(newEdges[j]);
                }
            }


            return triangles;
        }

        /// <summary>
        /// Check if 2 edge is intersecting
        /// </summary>
        public static bool AreEdgesIntersecting(Edge edge1, Edge edge2)
        {
            return AreLinesIntersecting(edge1.v1.position, edge1.v2.position, edge2.v1.position, edge2.v2.position, true);
        }

        /// <summary>
        /// From triangle where each triangle has one vertex to half edge
        /// </summary>
        public static List<HalfEdge> TransformFromTriangleToHalfEdge(List<Triangle> triangles)
        {
            //Make sure the triangles have the same orientation
            OrientTrianglesClockwise(triangles);

            //First create a list with all possible half-edges
            List<HalfEdge> halfEdges = new List<HalfEdge>(triangles.Count * 3);

            for (int i = 0; i < triangles.Count; i++)
            {
                Triangle t = triangles[i];

                HalfEdge he1 = new HalfEdge(t.v1);
                HalfEdge he2 = new HalfEdge(t.v2);
                HalfEdge he3 = new HalfEdge(t.v3);

                he1.nextEdge = he2;
                he2.nextEdge = he3;
                he3.nextEdge = he1;

                he1.prevEdge = he3;
                he2.prevEdge = he1;
                he3.prevEdge = he2;

                //The vertex needs to know of an edge going from it
                he1.v.halfEdge = he2;
                he2.v.halfEdge = he3;
                he3.v.halfEdge = he1;

                //The face the half-edge is connected to
                t.halfEdge = he1;

                he1.t = t;
                he2.t = t;
                he3.t = t;

                //Add the half-edges to the list
                halfEdges.Add(he1);
                halfEdges.Add(he2);
                halfEdges.Add(he3);
            }

            //Find the half-edges going in the opposite direction
            for (int i = 0; i < halfEdges.Count; i++)
            {
                HalfEdge he = halfEdges[i];

                Vertex goingToVertex = he.v;
                Vertex goingFromVertex = he.prevEdge.v;

                for (int j = 0; j < halfEdges.Count; j++)
                {
                    //Dont compare with itself
                    if (i == j)
                    {
                        continue;
                    }

                    HalfEdge heOpposite = halfEdges[j];

                    //Is this edge going between the vertices in the opposite direction
                    if (goingFromVertex.position == heOpposite.v.position && goingToVertex.position == heOpposite.prevEdge.v.position)
                    {
                        he.oppositeEdge = heOpposite;

                        break;
                    }
                }
            }


            return halfEdges;
        }

        /// <summary>
        /// Orient triangles so they have the correct orientation
        /// </summary>
        public static void OrientTrianglesClockwise(List<Triangle> triangles)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                Triangle tri = triangles[i];

                if (!IsTriangleOrientedClockwise(tri.v1.position, tri.v2.position, tri.v3.position))
                {
                    tri.ChangeOrientation();
                }
            }
        }

        /// <summary>
        /// Is a point d inside, outside or on the same circle as a, b, c
        /// https://gamedev.stackexchange.com/questions/71328/how-can-i-add-and-subtract-convex-polygons
        /// Returns positive if inside, negative if outside, and 0 if on the circle
        /// </summary>
        public static float IsPointInsideOutsideOrOnCircle(Vector2 aVec, Vector2 bVec, Vector2 cVec, Vector2 dVec)
        {
            //This first part will simplify how we calculate the determinant
            float a = aVec.X - dVec.X;
            float d = bVec.X - dVec.X;
            float g = cVec.X - dVec.X;

            float b = aVec.Y - dVec.Y;
            float e = bVec.Y - dVec.Y;
            float h = cVec.Y - dVec.Y;

            float c = a * a + b * b;
            float f = d * d + e * e;
            float i = g * g + h * h;

            float determinant = (a * e * i) + (b * f * g) + (c * d * h) - (g * e * c) - (h * f * a) - (i * d * b);

            return determinant;
        }

        /// <summary>
        /// Flip an edge
        /// </summary>
        public static void FlipEdge(HalfEdge one)
        {
            //The data we need
            //This edge's triangle
            HalfEdge two = one.nextEdge;
            HalfEdge three = one.prevEdge;
            //The opposite edge's triangle
            HalfEdge four = one.oppositeEdge;
            HalfEdge five = one.oppositeEdge.nextEdge;
            HalfEdge six = one.oppositeEdge.prevEdge;
            //The vertices
            Vertex a = one.v;
            Vertex b = one.nextEdge.v;
            Vertex c = one.prevEdge.v;
            Vertex d = one.oppositeEdge.nextEdge.v;



            //Flip

            //Change vertex
            a.halfEdge = one.nextEdge;
            c.halfEdge = one.oppositeEdge.nextEdge;

            //Change half-edge
            //Half-edge - half-edge connections
            one.nextEdge = three;
            one.prevEdge = five;

            two.nextEdge = four;
            two.prevEdge = six;

            three.nextEdge = five;
            three.prevEdge = one;

            four.nextEdge = six;
            four.prevEdge = two;

            five.nextEdge = one;
            five.prevEdge = three;

            six.nextEdge = two;
            six.prevEdge = four;

            //Half-edge - vertex connection
            one.v = b;
            two.v = b;
            three.v = c;
            four.v = d;
            five.v = d;
            six.v = a;

            //Half-edge - triangle connection
            Triangle t1 = one.t;
            Triangle t2 = four.t;

            one.t = t1;
            three.t = t1;
            five.t = t1;

            two.t = t2;
            four.t = t2;
            six.t = t2;

            //Opposite-edges are not changing!

            //Triangle connection
            t1.v1 = b;
            t1.v2 = c;
            t1.v3 = d;

            t2.v1 = b;
            t2.v2 = d;
            t2.v3 = a;

            t1.halfEdge = three;
            t2.halfEdge = four;
        }
    }
}