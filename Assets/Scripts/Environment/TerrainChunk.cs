using UnityEngine;

public class TerrainChunk : MonoBehaviour
{
    float perlinScale = 0.2f;
    float heightScale = 2f;

    public float TimeUpdated;

    void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for(int i=0; i<vertices.Length; i++)
        {
            vertices[i].y = CalculateHeight(vertices[i].x, vertices[i].z);
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        this.gameObject.AddComponent<MeshCollider>();
    }

    float CalculateHeight(float x, float z)
    {
        float xScaled = (x + this.transform.position.x) * perlinScale;
        float zScaled = (z + this.transform.position.z) * perlinScale;

        return Mathf.PerlinNoise(xScaled, zScaled) * heightScale;
    }
}