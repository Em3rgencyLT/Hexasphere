using System.Collections.Generic;
using System.Linq;
using Code.Hexasphere;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawHexasphere : MonoBehaviour
{
    [Range(5f,2000f)]
    [SerializeField] private float radius = 10f;
    [Range(1, 15)]
    [SerializeField] private int divisions = 4;
    [Range(0.1f, 1f)]
    [SerializeField] private float hexSize = 1f;

    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private Hexasphere _hexasphere;

    private float _oldRadius;
    private int _oldDivisions;
    private float _oldHexSize;
    private float _lastUpdated;

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        DrawHexasphereMesh();
    }

    private void Update()
    {
        _lastUpdated += Time.deltaTime;
        if (_lastUpdated < 1f) return;
        if (Mathf.Abs(_oldRadius - radius) > 0.001f || _oldDivisions != divisions ||
            Mathf.Abs(_oldHexSize - hexSize) > 0.001f)
        {
            DrawHexasphereMesh();
        }
    }

    private void DrawHexasphereMesh()
    {
        _oldRadius = radius;
        _oldDivisions = divisions;
        _oldHexSize = hexSize;
        _lastUpdated = 0f;
        
        _hexasphere = new Hexasphere(transform.position, radius, divisions, hexSize);
        List<Point> vertexes = new List<Point>();
        List<int> triangleList = new List<int>();
        _hexasphere.Tiles.ForEach(tile =>
        {
            tile.Points.ForEach(point =>
            {
                vertexes.Add(point);
            });
            tile.Faces.ForEach(face =>
            {
                face.Points.ForEach(point =>
                {
                    int vertexIndex = vertexes.FindIndex(vertex => vertex.ID == point.ID);
                    triangleList.Add(vertexIndex);
                });
            });
        });

        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
        _mesh.vertices = vertexes.Select(point => point.Position).ToArray();
        _mesh.triangles = triangleList.ToArray();
        
        _mesh.RecalculateNormals();
    }
}
