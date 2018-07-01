using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public static class MeshExtensions
{
    public class Triangle
    {
        public int vertexIndex1;
        public int vertexIndex2;
        public int vertexIndex3;

        public Triangle(int vertexIndex1, int vertexIndex2, int vertexIndex3)
        {
            this.vertexIndex1 = vertexIndex1;
            this.vertexIndex2 = vertexIndex2;
            this.vertexIndex3 = vertexIndex3;
        }

        public int TriangleIndex => vertexIndex1;

        public Edge[] Edges
            => new[] { 
                new Edge(vertexIndex1, vertexIndex2, TriangleIndex),
                new Edge(vertexIndex2, vertexIndex3, TriangleIndex),
                new Edge(vertexIndex3, vertexIndex1, TriangleIndex) };
    }

    public struct Edge
    {
        public int vertexIndex1;
        public int vertexIndex2;
        public int triangleIndex;

        public Edge(int vertexIndex1, int vertexIndex2, int triangleIndex)
        {
            this.vertexIndex1 = vertexIndex1;
            this.vertexIndex2 = vertexIndex2;
            this.triangleIndex = triangleIndex;
        }

        public bool IsSharedWith(Edge other) =>
            (vertexIndex1 == other.vertexIndex2 && 
            vertexIndex2 == other.vertexIndex1);
    }

    public static IEnumerable<Triangle> Triangles(this Mesh mesh)
    {
        int[] trianglesIndices = mesh.triangles;
        
        for (int i=0; i < trianglesIndices.Length; i += 3)
        {
            yield return new Triangle(
                trianglesIndices[i],
                trianglesIndices[i+1],
                trianglesIndices[i+2]
            );
        }
    }

    public static IEnumerable<Edge> Edges(this Mesh mesh)
    {
        var edges = new List<Edge>();

        foreach(var triangle in mesh.Triangles())
        {
            edges.AddRange(triangle.Edges);
        }
        return edges;
    }

    public static int[] VertexIndices(this IEnumerable<Edge> edges) =>
        edges.SelectMany(e => new[] {e.vertexIndex1, e.vertexIndex2}).Distinct().ToArray();
    
    public static IEnumerable<Edge> NotShared(this IEnumerable<Edge> edges)
    {
        List<Edge> result = new List<Edge>(edges);
        for (int i = result.Count-1; i > 0; i--)
         {
             for (int n = i - 1; n >= 0; n--)
             {
                if (result[i].IsSharedWith(result[n]))
                {
                     result.RemoveAt(i);
                     result.RemoveAt(n);
                     i--;
                     break;
                }
             }
         }

        return result;
    }
}