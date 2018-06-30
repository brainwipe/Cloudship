using UnityEngine;
using System.Linq;

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
        sandMesh.normals = RecalculateEdgeMeshNormals(sandMesh.normals);
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

    Vector3[] RecalculateEdgeMeshNormals(Vector3[] sandMeshNormals)
    {
        Mesh edgeMesh = this.GetComponentsInChildren<MeshFilter>().Single(m => m.name == "edgemesh").mesh;
        edgeMesh.vertices = CalculateHeight(edgeMesh.vertices);
        edgeMesh.RecalculateNormals();

        

        return sandMeshNormals;
    }
}