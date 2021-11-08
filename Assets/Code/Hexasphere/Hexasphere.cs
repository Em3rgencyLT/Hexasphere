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
        private List<Tile> _tiles;
        private List<Point> _points;
        private static float _tao = Mathf.PI / 2;
        private List<Point> _dividedIcosahedronPoints;
        private const float DefaultSize = 100f;

        private List<Face> _faces;
        private List<Face> _icosahedronFaces;

        public Hexasphere(Vector3 origin, float radius, int divisions, float hexSize)
        {
            _radius = radius;
            _divisions = divisions;
            _hexSize = hexSize;

            _tiles = new List<Tile>();
            _points = new List<Point>();
            _dividedIcosahedronPoints = new List<Point>();
            _faces = new List<Face>();
            
             List<Point> icosahedronCorners = new List<Point>
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
             
            _icosahedronFaces = new List<Face>
            {
                new Face(icosahedronCorners[0], icosahedronCorners[1], icosahedronCorners[4], origin, false),
                new Face(icosahedronCorners[1], icosahedronCorners[9], icosahedronCorners[4], origin, false),
                new Face(icosahedronCorners[4], icosahedronCorners[9], icosahedronCorners[5], origin, false),
                new Face(icosahedronCorners[5], icosahedronCorners[9], icosahedronCorners[3], origin, false),
                new Face(icosahedronCorners[2], icosahedronCorners[3], icosahedronCorners[7], origin, false),
                new Face(icosahedronCorners[3], icosahedronCorners[2], icosahedronCorners[5], origin, false),
                new Face(icosahedronCorners[7], icosahedronCorners[10], icosahedronCorners[2], origin, false),
                new Face(icosahedronCorners[0], icosahedronCorners[8], icosahedronCorners[10], origin, false),
                new Face(icosahedronCorners[0], icosahedronCorners[4], icosahedronCorners[8], origin, false),
                new Face(icosahedronCorners[8], icosahedronCorners[2], icosahedronCorners[10], origin, false),
                new Face(icosahedronCorners[8], icosahedronCorners[4], icosahedronCorners[5], origin, false),
                new Face(icosahedronCorners[8], icosahedronCorners[5], icosahedronCorners[2], origin, false),
                new Face(icosahedronCorners[1], icosahedronCorners[0], icosahedronCorners[6], origin, false),
                new Face(icosahedronCorners[11], icosahedronCorners[1], icosahedronCorners[6], origin, false),
                new Face(icosahedronCorners[3], icosahedronCorners[9], icosahedronCorners[11], origin, false),
                new Face(icosahedronCorners[6], icosahedronCorners[10], icosahedronCorners[7], origin, false),
                new Face(icosahedronCorners[3], icosahedronCorners[11], icosahedronCorners[7], origin, false),
                new Face(icosahedronCorners[11], icosahedronCorners[6], icosahedronCorners[7], origin, false),
                new Face(icosahedronCorners[6], icosahedronCorners[0], icosahedronCorners[10], origin, false),
                new Face(icosahedronCorners[9], icosahedronCorners[1], icosahedronCorners[11], origin, false)
            };
            
            icosahedronCorners.ForEach(point => CachePoint(point));
            
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
                        _faces.Add(new Face(previousPoints[j], bottomSide[j], bottomSide[j+1], origin));
                        if (j == 0) continue;
                        _faces.Add(new Face(previousPoints[j-1], previousPoints[j], bottomSide[j], origin));
                    }
                }
            });
            
            _dividedIcosahedronPoints.ForEach(point =>
            {
                _tiles.Add(new Tile(point, _radius, hexSize, origin));
            });
        }

        public List<Tile> Tiles => _tiles;

        private Point CachePoint(Point point)
        {
            Point existingPoint = _dividedIcosahedronPoints.FirstOrDefault(candidatePoint => Point.IsOverlapping(candidatePoint, point));
            if (existingPoint != null)
            {
                return existingPoint;
            }
            
            _dividedIcosahedronPoints.Add(point);
            return point;
        }
    }
}
