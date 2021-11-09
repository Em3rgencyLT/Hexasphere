using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Hexasphere
{
    public class Face
    {
        private readonly string _id;
        private readonly List<Point> _points;

        public Face(Point point1, Point point2, Point point3, bool trackFaceInPoints = true)
        {
            _id = Guid.NewGuid().ToString();

            float centerX = (point1.Position.x + point2.Position.x + point3.Position.x) / 3;
            float centerY = (point1.Position.y + point2.Position.y + point3.Position.y) / 3;
            float centerZ = (point1.Position.z + point2.Position.z + point3.Position.z) / 3;
            Vector3 center = new Vector3(centerX, centerY, centerZ);

            //Determine correct winding order
            Vector3 normal = GetNormal(point1, point2, point3);
            _points = IsNormalPointingAwayFromOrigin(center, normal) ? 
                new List<Point> {point1, point2, point3} : 
                new List<Point> {point1, point3, point2};

            if (trackFaceInPoints)
            {
                _points.ForEach(point => point.AssignFace(this));
            }
        }

        public string ID => _id;

        public List<Point> Points => _points;

        public List<Point> GetOtherPoints(Point point)
        {
            if (!IsPointPartOfFace(point))
            {
                throw new ArgumentException("Given point must be one of the points on the face!");
            }

            return _points.Where(facePoint => facePoint.ID != point.ID).ToList();
        }

        public bool IsAdjacentToFace(Face face)
        {
            List<string> thisFaceIds = _points.Select(point => point.ID).ToList();
            List<string> otherFaceIds = face.Points.Select(point => point.ID).ToList();
            return thisFaceIds.Intersect(otherFaceIds).ToList().Count == 2;
        }

        public Point GetCenter()
        {
            float centerX = (_points[0].Position.x + _points[1].Position.x + _points[2].Position.x) / 3;
            float centerY = (_points[0].Position.y + _points[1].Position.y + _points[2].Position.y) / 3;
            float centerZ = (_points[0].Position.z + _points[1].Position.z + _points[2].Position.z) / 3;

            return new Point(new Vector3(centerX, centerY, centerZ));
        }

        private bool IsPointPartOfFace(Point point)
        {
            return _points.Any(facePoint => facePoint.ID == point.ID);
        }

        private static Vector3 GetNormal(Point point1, Point point2, Point point3)
        {
            Vector3 side1 = point2.Position - point1.Position;
            Vector3 side2 = point3.Position - point1.Position;

            Vector3 cross = Vector3.Cross(side1, side2);

            return cross / cross.magnitude;
        }

        private static bool IsNormalPointingAwayFromOrigin(Vector3 surface, Vector3 normal)
        {
            //Does adding the normal vector to the center point of the face get you closer or further from the center of the polyhedron?
            return Vector3.Distance(Vector3.zero, surface) < Vector3.Distance(Vector3.zero, surface + normal);
        }
    }
}
