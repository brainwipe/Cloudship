using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotsamFactory : MonoBehaviour 
{

	public GameObject flotsamPrefab;
    float verticalOffset = 36f;

	public Flotsam CreateFlotsam(TerrainChunk chunk)
    {
        if (!ShouldICreateFlotsam())
        {
            return null;
        }

        var chunkSize = TerrainChunk.Size;
        var globalPosition = 
            chunk.transform.position + 
            new Vector3(Random.Range(0f, chunkSize) - (chunkSize / 2), verticalOffset, Random.Range(0f, chunkSize) - (chunkSize / 2));
        return CreateFlotsam(chunk, globalPosition);
    }

    public Flotsam CreateFlotsam(TerrainChunk chunk, Vector3 globalPosition)
    {
        globalPosition.y = chunk.transform.position.y + verticalOffset;
        var flotsam = Instantiate(flotsamPrefab, globalPosition, Quaternion.identity, transform);
        chunk.flotsam = flotsam;
        return flotsam.GetComponent<Flotsam>();
    }

    bool ShouldICreateFlotsam()
    {
        return Random.Range(0f,100f) > 70f;
    }
}
