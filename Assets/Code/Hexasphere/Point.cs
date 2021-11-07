using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Hexasphere
{
    public class Point
    {
        private string _id;
        private readonly Vector3 _position;
        private List<Face> _faces;

        public Point(Vector3 position)
        {
            _id = Guid.NewGuid().ToString();
            _position = position;
            _faces = new List<Face>();
        }

        public Vector3 Position => _position;

        public string ID => _id;

        public List<Face> Faces => _faces;

        public void AssignFace(Face face)
        {
            this._faces.Add(face);
        }

        public List<Point> Subdivide(Point target, int count, Func<Point, Point> findDuplicatePointIfExists)
        {
            List<Point> segments = new List<Point>();
            segments.Add(this);

            for (int i = 1; i <= count; i++)
            {
                float x = _position.x * (1 - (float) i / count) + target.Position.x * ((float) i / count);
                float y = _position.y * (1 - (float) i / count) + target.Position.y * ((float) i / count);
                float z = _position.z * (1 - (float) i / count) + target.Position.z * ((float) i / count);

                Point newPoint = findDuplicatePointIfExists(new Point(new Vector3(x, y, z)));
                segments.Add(newPoint);
            }

            segments.Add(target);
            return segments;
        }

        public Point ProjectToSphere(float radius, float t)
        {
            float projectionPoint = radius / _position.magnitude;
            float x = _position.x * projectionPoint * t;
            float y = _position.y * projectionPoint * t;
            float z = _position.z * projectionPoint * t;
            return new Point(new Vector3(x, y, z));
        }

        public List<Face> GetOrderedFaces()
        {
            if (_faces.Count == 0) return _faces;
            List<Face> orderedList = new List<Face> {_faces[0]};

            Face currentFace = _faces[0];
            while (orderedList.Count < _faces.Count)
            {
                Face neighbour = _faces.First(face => face.ID != currentFace.ID && face.IsAdjacentToFace(currentFace));
                currentFace = neighbour;
                orderedList.Add(currentFace);
            }

            return orderedList;
        }

        public Face FindCommonFaceExcluding(Point otherPoint, Face excludedFace)
        {
            List<string> faceIds = _faces.Select(face => face.ID).ToList();
            List<string> otherFaceIds = otherPoint.Faces.Select(face => face.ID).ToList();
            string desiredId = faceIds.Intersect(otherFaceIds).First(id => id != excludedFace.ID);
            return _faces.First(face => face.ID == desiredId);
        }

        public override string ToString()
        {
            return $"{_position.x},{_position.y},{_position.z}";
        }

        public string ToJson()
        {
            return $"{{x:{_position.x},y:{_position.y},z:{_position.z}}}";
        }
    }
}
