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
        
        public Tile(Point center, float size)
        {
            _center = center;
            _size = Mathf.Max(0.01f, Mathf.Min(1f, size));
            _faces = center.GetOrderedFaces();

            _faces.Select(face => Vector3.Lerp(face.CenterPoint.Position, _center.Position, size)).ToList().ForEach(pos => _points.Add(new Point(pos)));
        }
    }
}
