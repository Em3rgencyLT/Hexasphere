using System;
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

        Hexasphere hexasphere = new Hexasphere(1f, 4, 1f);
        mesh.vertices = hexasphere.Points.Select(point => point.Position).ToArray();

        List<int> triangleList = new List<int>();
        hexasphere.Faces.ForEach(face =>
        {
            face.Points.ForEach(point =>
            {
                int vertexIndex = hexasphere.Points.FindIndex(corner => corner.ID == point.ID);
                triangleList.Add(vertexIndex);
            });
        });
        mesh.triangles = triangleList.ToArray();
        
        mesh.RecalculateNormals();
    }
}
