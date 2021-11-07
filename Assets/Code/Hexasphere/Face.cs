using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Hexasphere
{
    public class Face
    {
        private string _id;
        private List<Point> _points;
        private Point _centerPoint;

        public Face(Point point1, Point point2, Point point3, bool trackFaceInPoints = true)
        {
            _id = Guid.NewGuid().ToString();

            float centerX = (point1.Position.x + point2.Position.x + point3.Position.x) / 3;
            float centerY = (point1.Position.y + point2.Position.y + point3.Position.y) / 3;
            float centerZ = (point1.Position.z + point2.Position.z + point3.Position.z) / 3;
            _centerPoint = new Point(new Vector3(centerX, centerY, centerZ));

            _points = IsNormalPointingAwayFromPoint(_centerPoint, GetNormal(point1, point2, point3)) ? 
                new List<Point> {point1, point2, point3} : 
                new List<Point> {point1, point3, point2};
            
            if (trackFaceInPoints)
            {
                _points.ForEach(point => point.AssignFace(this));
            }
        }

        public string ID => _id;

        public List<Point> Points => _points;

        public Point CenterPoint => _centerPoint;

        public List<Point> GetOtherPoints(Point point)
        {
            if (!IsPointPartOfFace(point))
            {
                throw new ArgumentException("Given point must be one of the points on the face!");
            }

            return _points.Where(facePoint => facePoint.ID != point.ID).ToList();
        }

        public Point FindThirdPoint(Point point1, Point point2)
        {
            if (!IsPointPartOfFace(point1) || !IsPointPartOfFace(point2))
            {
                throw new ArgumentException("Given point must be one of the points on the face!");
            }
            
            return _points.First(facePoint => facePoint.ID != point1.ID && facePoint.ID != point2.ID);
        }

        public bool IsAdjacentToFace(Face face)
        {
            List<string> thisFaceIds = _points.Select(point => point.ID).ToList();
            List<string> otherFaceIds = face.Points.Select(point => point.ID).ToList();
            return thisFaceIds.Intersect(otherFaceIds).ToList().Count == 2;
        }

        private bool IsPointPartOfFace(Point point)
        {
            return _points.Any(facePoint => facePoint.ID == point.ID);
        }

        public static Vector3 GetNormal(Point point1, Point point2, Point point3)
        {
            Vector3 u = point2.Position - point1.Position;
            Vector3 v = point3.Position - point1.Position;

            float x = u.y * v.z - u.z * v.y;
            float y = u.z * v.x - u.x * v.z;
            float z = u.x * v.y - u.y * v.x;

            return new Vector3(x, y, z);
        }

        public static bool IsNormalPointingAwayFromPoint(Point point, Vector3 normal)
        {
            Vector3 pos = point.Position;
            return pos.x * normal.x >= 0 && pos.y * normal.y >= 0 && pos.z * normal.z >= 0;
        }
    }
}
