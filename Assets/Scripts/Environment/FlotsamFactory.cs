using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotsamFactory : MonoBehaviour {

	public GameObject flotsamPrefab;

	public void CreateFlotsam(TerrainChunk chunk)
    {
        if (!ShouldICreateFlotsam())
        {
            return;
        }

        var flotsamPosition = chunk.transform.position;
        var flotsam = Instantiate(flotsamPrefab, flotsamPosition, Quaternion.identity, transform);
        chunk.flotsam = flotsam;
        
        var chunkSize = TerrainChunk.Size;
        var offset = new Vector3(Random.Range(0f, chunkSize) - (chunkSize / 2), 36f, Random.Range(0f, chunkSize) - (chunkSize / 2));
        flotsam.transform.position += offset;

    }

    bool ShouldICreateFlotsam()
    {
        return Random.Range(0f,100f) > 70f;
    }
}
