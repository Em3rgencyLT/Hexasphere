using Code.Hexasphere;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawHexasphere : MonoBehaviour
{
    [Min(5f)]
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
        
        _hexasphere = new Hexasphere(radius, divisions, hexSize);

        _mesh = new Mesh();
        _meshFilter.mesh = _mesh;
        _mesh.vertices = _hexasphere.MeshDetails.Vertices.ToArray();
        _mesh.triangles = _hexasphere.MeshDetails.Triangles.ToArray();
        _mesh.RecalculateNormals();
    }
}
