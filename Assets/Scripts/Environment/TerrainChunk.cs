using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TerrainChunk : MonoBehaviour
{
    float perlinScale = 0.04f;
    float heightScale = 60f;

    public float TimeUpdated;
    void Start()
    {
        Mesh sandMesh = this.GetComponentsInChildren<MeshFilter>().Single(m => m.name == "sand").mesh;
        sandMesh.vertices = CalculateHeight(sandMesh.vertices);
        sandMesh.RecalculateBounds();
        sandMesh.RecalculateNormals();
        sandMesh.normals = RecalculateEdgeMeshNormals(sandMesh);
        this.gameObject.AddComponent<MeshCollider>();
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

    Vector3[] RecalculateEdgeMeshNormals(Mesh sandMesh)
    {
        var edgeMesh = GetComponentsInChildren<MeshFilter>()
            .Single(m => m.name.Contains("sandedgemesh"))
            .mesh;

        edgeMesh.vertices = CalculateHeight(edgeMesh.vertices);
        edgeMesh.RecalculateNormals();

        var sandMeshEdgeVertexIndices = sandMesh
            .Edges()
            .NotShared()
            .SelectMany(x => new[] { x.vertexIndex1, x.vertexIndex2})
            .Distinct();
        
        var normals = new List<Vector3>(sandMesh.normals);
        foreach(var vertexIndex in sandMeshEdgeVertexIndices)
        {
            var vertex = sandMesh.vertices[vertexIndex];
            
            var edgeMeshIndexForMatchingVertex = -1;
            for(int i=0; i<edgeMesh.vertices.Length; i++)
            {
                if (edgeMesh.vertices[i] == vertex)
                {
                    edgeMeshIndexForMatchingVertex = i;
                    break;
                }
            }

            if (edgeMeshIndexForMatchingVertex > -1)
            {   
                Debug.Log("Match found, updating");
                normals[vertexIndex] = edgeMesh.normals[edgeMeshIndexForMatchingVertex];
            }
        } 
        return normals.ToArray();
    }
}