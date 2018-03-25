using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    float perlinScale = 0.01f;
    float heightScale = 1f;

    public float TimeUpdated;

    void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for(int i=0; i<vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(transform.TransformPoint(vertices[i]));
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        this.gameObject.AddComponent<MeshCollider>();
    }

    float CalculateHeight(Vector3 vertex)
    {
        float xScaled =vertex.x * perlinScale;
        float zScaled =vertex.z * perlinScale;

        return Mathf.PerlinNoise(xScaled, zScaled) * heightScale;
    }
}