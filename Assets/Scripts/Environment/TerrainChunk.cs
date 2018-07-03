using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TerrainChunk : MonoBehaviour
{
    public const float Size = 1000f;
    static RemovalContext removalContextInstance;

    float perlinScale = 0.04f;
    float heightScale = 60f;

    
    public GameObject flotsam;
    public float TimeUpdated;

    void Start()
    {
        Mesh sandMesh = this.GetComponentsInChildren<MeshFilter>().Single(m => m.name == "sand").mesh;
        sandMesh.vertices = CalculateHeight(sandMesh.vertices);
        sandMesh.RecalculateBounds();
        sandMesh.RecalculateNormals();
        RemoveOuterMostEdge(sandMesh);
        this.gameObject.AddComponent<MeshCollider>();
    }

    void OnDestroy()
    {
        if (flotsam != null)
        {
            Destroy(flotsam);
        }
    }

    Vector3[] CalculateHeight(Vector3[] vertices)
    {
        for(int i=0; i<vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(transform.TransformPoint(vertices[i]));
        }
        return vertices;
    }

    float CalculateHeight(Vector3 vertex)
    {
        float xScaled =vertex.x * perlinScale;
        float zScaled =vertex.z * perlinScale;

        return Mathf.PerlinNoise(xScaled, zScaled) * heightScale;
    }

    RemovalContext FindRemovalItems(Mesh sandMesh)
    {
        var edgeVertexIndices = sandMesh
            .Edges()
            .NotShared()
            .VertexIndices();

        var context = new RemovalContext();
        var triangles = new List<int>(sandMesh.triangles);

        for(int i=0; i<triangles.Count(); i+=3)
        {
            if (edgeVertexIndices.Contains(triangles[i]) || 
                edgeVertexIndices.Contains(triangles[i+1]) ||
                edgeVertexIndices.Contains(triangles[i+2]))
            {
                context.AddTriangleIndex(i);
                context.AddTriangleIndex(i+1);
                context.AddTriangleIndex(i+2);
            }
        }
        return context;
    }

    void RemoveOuterMostEdge(Mesh sandMesh)
    {
        if (removalContextInstance == null)
        {
            removalContextInstance = FindRemovalItems(sandMesh);
        }

        var triangles = new List<int>(sandMesh.triangles);
        foreach(var triangleIndex in removalContextInstance.TriangleIndices)
        {
            triangles.RemoveAt(triangleIndex);
        }
        sandMesh.triangles = triangles.ToArray();
    }

    class RemovalContext
    {
        List<int> triangleIndices;
        public int[] TriangleIndices => triangleIndices.OrderByDescending(x => x).ToArray();

        public RemovalContext()
        {
            triangleIndices = new List<int>();
        }

        public void AddTriangleIndex(int index)
        {
            triangleIndices.Add(index);
        }
    }
}