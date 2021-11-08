using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Hexasphere
{
    public class Tile
    {
        private Point _center;
        private float _size;
        private List<Face> _faces;
        private List<Point> _points;
        
        public Tile(Point center, float radius, float size, Vector3 origin)
        {
            _points = new List<Point>();
            _faces = new List<Face>();
            _center = center;
            _size = Mathf.Max(0.01f, Mathf.Min(1f, size));
            List<Face> icosahedronFaces = center.GetOrderedFaces();

            List<Vector3> polygonPoints = icosahedronFaces.Select(face => Vector3.Lerp(_center.Position, face.GetCenter().Position, _size)).ToList();
            polygonPoints.ForEach(pos => _points.Add(new Point(pos).ProjectToSphere(radius, 0.5f)));
            _faces.Add(new Face(_points[0], _points[1], _points[2], origin));
            _faces.Add(new Face(_points[0], _points[2], _points[3], origin));
            _faces.Add(new Face(_points[0], _points[3], _points[4], origin));
            if (_points.Count > 5)
            {
                _faces.Add(new Face(_points[0], _points[4], _points[5], origin));
            }
        }

        public List<Point> Points => _points;
        public List<Face> Faces => _faces;
    }
}
