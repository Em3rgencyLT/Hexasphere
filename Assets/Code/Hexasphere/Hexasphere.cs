using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Hexasphere
{
    public class Hexasphere
    {
        private float _radius;
        private int _divisions;
        private float _hexSize;
        private static float _tao = Mathf.PI / 2;
        private List<Point> _points;
        private const float DefaultSize = 100f;
        private const float PointComparisonAccuracy = 0.00001f;
        
        private static readonly List<Point> _corners = new List<Point>
        {
            new Point(new Vector3(DefaultSize, _tao * DefaultSize, 0f)),
            new Point(new Vector3(-DefaultSize, _tao * DefaultSize, 0f)),
            new Point(new Vector3(DefaultSize, -_tao * DefaultSize, 0f)),
            new Point(new Vector3(-DefaultSize, -_tao * DefaultSize, 0f)),
            new Point(new Vector3(0, DefaultSize, _tao * DefaultSize)),
            new Point(new Vector3(0, -DefaultSize, _tao * DefaultSize)),
            new Point(new Vector3(0, DefaultSize, -_tao * DefaultSize)),
            new Point(new Vector3(0, -DefaultSize, -_tao * DefaultSize)),
            new Point(new Vector3(_tao * DefaultSize, 0f, DefaultSize)),
            new Point(new Vector3(-_tao * DefaultSize, 0f, DefaultSize)),
            new Point(new Vector3(_tao * DefaultSize, 0f, -DefaultSize)),
            new Point(new Vector3(-_tao * DefaultSize, 0f, -DefaultSize))
        };

        private List<Face> _faces = new List<Face>
        {
            new Face(_corners[0], _corners[1], _corners[4], false),
            new Face(_corners[1], _corners[9], _corners[4], false),
            new Face(_corners[4], _corners[9], _corners[5], false),
            new Face(_corners[5], _corners[9], _corners[3], false),
            new Face(_corners[2], _corners[3], _corners[7], false),
            new Face(_corners[3], _corners[2], _corners[5], false),
            new Face(_corners[7], _corners[10], _corners[2], false),
            new Face(_corners[0], _corners[8], _corners[10], false),
            new Face(_corners[0], _corners[4], _corners[8], false),
            new Face(_corners[8], _corners[2], _corners[10], false),
            new Face(_corners[8], _corners[4], _corners[5], false),
            new Face(_corners[8], _corners[5], _corners[2], false),
            new Face(_corners[1], _corners[0], _corners[6], false),
            new Face(_corners[11], _corners[1], _corners[6], false),
            new Face(_corners[3], _corners[9], _corners[11], false),
            new Face(_corners[6], _corners[10], _corners[7], false),
            new Face(_corners[3], _corners[11], _corners[7], false),
            new Face(_corners[11], _corners[6], _corners[7], false),
            new Face(_corners[6], _corners[0], _corners[10], false),
            new Face(_corners[9], _corners[1], _corners[11], false)
        };

        public Hexasphere(float radius, int divisions, float hexSize)
        {
            _radius = radius;
            _divisions = divisions;
            _hexSize = hexSize;

            _points = new List<Point>();
        }

        public List<Face> Faces => _faces;

        public List<Point> Corners => _corners;

        public Point GetPointFromCacheIfExists(Point point)
        {
            Point existingPoint = _points.FirstOrDefault(candidatePoint => 
                Mathf.Abs(candidatePoint.Position.x - point.Position.x) <= PointComparisonAccuracy &&
                Mathf.Abs(candidatePoint.Position.y - point.Position.y) <= PointComparisonAccuracy &&
                Mathf.Abs(candidatePoint.Position.z - point.Position.z) <= PointComparisonAccuracy);
            if (existingPoint != null)
            {
                return existingPoint;
            }
            
            _points.Add(point);
            return point;
        }
    }
}
