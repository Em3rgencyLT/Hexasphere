using System.Collections.Generic;
using System.Linq;
using Code.Hexasphere;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DrawHexasphere : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private Hexasphere _hexasphere;
    
    void Start()
    {
        Mesh mesh = new Mesh();
        _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = mesh;

        _hexasphere = new Hexasphere(transform.position, 10f, 20, 1f);
        mesh.vertices = _hexasphere.Points.Select(point => point.Position).ToArray();

        List<int> triangleList = new List<int>();
        _hexasphere.Faces.ForEach(face =>
        {
            face.Points.ForEach(point =>
            {
                int vertexIndex = _hexasphere.Points.FindIndex(corner => corner.ID == point.ID);
                triangleList.Add(vertexIndex);
            });
        });
        mesh.triangles = triangleList.ToArray();
        
        mesh.RecalculateNormals();
    }

    /*private void Update()
    {
        _hexasphere.Faces.ForEach(face =>
        {
            Debug.DrawRay(face.CenterPoint.Position, face.Normal * 10, Color.red);
        });
    }*/
}
