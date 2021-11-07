using System.Collections.Generic;
using System.Linq;
using Code.Hexasphere;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawHexasphere : MonoBehaviour
{
    private MeshFilter _meshFilter;
    
    void Start()
    {
        Mesh mesh = new Mesh();
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = mesh;

        Hexasphere hexasphere = new Hexasphere(1f, 2, 1f);
        mesh.vertices = hexasphere.Corners.Select(point => point.Position).ToArray();

        List<int> triangleList = new List<int>();
        hexasphere.Faces.ForEach(face =>
        {
            face.Points.ForEach(point =>
            {
                int vertexIndex = hexasphere.Corners.FindIndex(corner => corner.ID == point.ID);
                triangleList.Add(vertexIndex);
            });
        });
        mesh.triangles = triangleList.ToArray();
        
        mesh.RecalculateNormals();
    }
}
