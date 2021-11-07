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
        private const float PointComparisonAccuracy = 0.000001f;
        
        private static readonly List<Point> Corners = new List<Point>
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

        private List<Face> _faces;
        private List<Face> _icosahedronFaces = new List<Face>
        {
            new Face(Corners[0], Corners[1], Corners[4], false),
            new Face(Corners[1], Corners[9], Corners[4], false),
            new Face(Corners[4], Corners[9], Corners[5], false),
            new Face(Corners[5], Corners[9], Corners[3], false),
            new Face(Corners[2], Corners[3], Corners[7], false),
            new Face(Corners[3], Corners[2], Corners[5], false),
            new Face(Corners[7], Corners[10], Corners[2], false),
            new Face(Corners[0], Corners[8], Corners[10], false),
            new Face(Corners[0], Corners[4], Corners[8], false),
            new Face(Corners[8], Corners[2], Corners[10], false),
            new Face(Corners[8], Corners[4], Corners[5], false),
            new Face(Corners[8], Corners[5], Corners[2], false),
            new Face(Corners[1], Corners[0], Corners[6], false),
            new Face(Corners[11], Corners[1], Corners[6], false),
            new Face(Corners[3], Corners[9], Corners[11], false),
            new Face(Corners[6], Corners[10], Corners[7], false),
            new Face(Corners[3], Corners[11], Corners[7], false),
            new Face(Corners[11], Corners[6], Corners[7], false),
            new Face(Corners[6], Corners[0], Corners[10], false),
            new Face(Corners[9], Corners[1], Corners[11], false)
        };

        public Hexasphere(float radius, int divisions, float hexSize)
        {
            _radius = radius;
            _divisions = divisions;
            _hexSize = hexSize;

            _points = new List<Point>();
            _faces = new List<Face>();
            
            Corners.ForEach(point => CachePoint(point));
            
            _icosahedronFaces.ForEach(icoFace =>
            {
                List<Point> facePoints = icoFace.Points;
                List<Point> previousPoints;
                List<Point> bottomSide = new List<Point> {facePoints[0]};
                List<Point> leftSide = facePoints[0].Subdivide(facePoints[1], divisions, CachePoint);
                List<Point> rightSide = facePoints[0].Subdivide(facePoints[2], divisions, CachePoint);
                for (int i = 1; i <= divisions; i++)
                {
                    previousPoints = bottomSide;
                    bottomSide = leftSide[i].Subdivide(rightSide[i], i, CachePoint);
                    for (int j = 0; j < i; j++)
                    {
                        _faces.Add(new Face(previousPoints[j], bottomSide[j], bottomSide[j+1]));
                        if (j == 0) continue;
                        _faces.Add(new Face(previousPoints[j-1], previousPoints[j], bottomSide[j]));
                    }
                }
            });
        }

        public List<Point> Points => _points;

        public List<Face> Faces => _faces;

        private Point CachePoint(Point point)
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
