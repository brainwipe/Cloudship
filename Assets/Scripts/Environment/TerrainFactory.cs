using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TerrainFactory : MonoBehaviour
{
    public static string TerrainTag = "Terrain";
    Cloudship playerCloudship;

    int chunkRadius;
    Dictionary<Vector3, TerrainChunk> chunks = new Dictionary<Vector3, TerrainChunk>();
    
    float chunkSize;

    public GameObject chunkPrefab;
    public FlotsamFactory flotsamFactory;

    void Start()
    {   
        chunkSize = TerrainChunk.Size;
        chunkRadius = (int)(GameManager.Instance.DrawDistance / chunkSize) + 2;
        playerCloudship = GameManager.Instance.PlayerCloudship;
        RebuildChunks(0,0);
    }

    void Update()
    {
        if (GameManager.Instance.PlayerCloudship == null)
        {
            return;
        }

        int playerXChunk = WorldToChunkConversion(playerCloudship.transform.position.x);
        int playerZChunk = WorldToChunkConversion(playerCloudship.transform.position.z);

        RebuildChunks(playerXChunk, playerZChunk);
    }

    void RebuildChunks(int chunkX, int chunkZ)
    {
        var timestampForThisUpdateLoop = Time.realtimeSinceStartup;

        for(int x = -chunkRadius; x < chunkRadius; x++)
        {
            for(int z = -chunkRadius; z < chunkRadius; z++)
            {
                var pos = new Vector3(
                    x * chunkSize + chunkX,
                    this.transform.position.y,
                    z * chunkSize + chunkZ);

                if (!chunks.ContainsKey(pos))
                {
                    CreateChunk(pos);
                }
                chunks[pos].TimeUpdated = timestampForThisUpdateLoop;
            }
        }

        RemoveOutDatedChunks(timestampForThisUpdateLoop);
    }

    void CreateChunk(Vector3 pos)
    {
        var chunkGameObj = Instantiate(chunkPrefab, pos, Quaternion.identity);
        chunkGameObj.tag = TerrainTag;
        chunkGameObj.transform.parent = this.transform;
        var chunk = chunkGameObj.GetComponent<TerrainChunk>();
        chunks.Add(pos, chunk);

        flotsamFactory.CreateFlotsam(chunk);
    }

    void RemoveOutDatedChunks(float timestampForThisUpdateLoop)
    {
        var outDatedChunks = chunks
        .Where(c => c.Value.TimeUpdated != timestampForThisUpdateLoop)
        .ToList();
        
        foreach(var removeChunk in outDatedChunks)
        {
            chunks.Remove(removeChunk.Key);
            Destroy(removeChunk.Value.gameObject);
        }
    }
    
    int WorldToChunkConversion(float worldPosition)
    {
        return (int)(Mathf.Floor(worldPosition/chunkSize) * chunkSize);
    }

    TerrainChunk FindNearestChunk(Vector3 position)
    {
        var x = (int)(Mathf.Floor(position.x/chunkSize) * chunkSize);
        var z = (int)(Mathf.Floor(position.x/chunkSize) * chunkSize);

        return chunks[new Vector3((float)x, transform.position.y, (float)z)];
    }

    public Flotsam CreateFlotsamAt(Vector3 position)
    {
        var chunk = FindNearestChunk(position);
        return flotsamFactory.CreateFlotsam(chunk, position);
    }
}