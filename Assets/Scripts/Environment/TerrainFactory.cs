using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TerrainFactory : MonoBehaviour
{
    public static string TerrainTag = "Terrain";

    int chunkRadius;
    Dictionary<Vector3, TerrainChunk> chunks = new Dictionary<Vector3, TerrainChunk>();
    
    Vector3 start;
    float timestampForThisUpdateLoop;

    Cloudship playerCloudship;

    float chunkSize;

    public GameObject chunkPrefab;
    public GameObject flotsamPrefab;

    void Start()
    {   
        start = Vector3.zero;
        chunkSize = TerrainChunk.Size;
        chunkRadius = (int)(GameManager.Instance.DrawDistance / chunkSize);
        playerCloudship = GameManager.Instance.PlayerCloudship;
        RebuildChunks(0,0);
    }

    void Update()
    {
        if (GameManager.Instance.PlayerCloudship == null ||
            !ShouldWeUpdate(playerCloudship.transform.position, start))
        {
            return;
        }

        int playerXChunk = WorldToChunkConversion(playerCloudship.transform.position.x);
        int playerZChunk = WorldToChunkConversion(playerCloudship.transform.position.z);

        RebuildChunks(playerXChunk, playerZChunk);
    }

    void RebuildChunks(int chunkX, int chunkZ)
    {
        timestampForThisUpdateLoop = Time.realtimeSinceStartup;

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

        RemoveOutDatedChunks();

        start = playerCloudship.transform.position;
    }

    void CreateChunk(Vector3 pos)
    {
        var chunkGameObj = Instantiate(chunkPrefab, pos, Quaternion.identity);
        chunkGameObj.tag = TerrainTag;
        chunkGameObj.transform.parent = this.transform;
        var chunk = chunkGameObj.GetComponent<TerrainChunk>();
        chunks.Add(pos, chunk);

        if(ShouldICreateFlotsam())
        {
            CreateFlotsam(chunk);
        }
    }

    void RemoveOutDatedChunks()
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

    bool ShouldICreateFlotsam()
    {
        return Random.Range(0f,100f) > 70f;
    }

    void CreateFlotsam(TerrainChunk chunk)
    {
        var flotsamPosition = chunk.transform.position;
        var flotsam = Instantiate(flotsamPrefab, flotsamPosition, Quaternion.identity, chunk.transform);
        
        flotsam.transform.localPosition = new Vector3(Random.Range(0f, chunkSize) - (chunkSize / 2), 33f, Random.Range(0f, chunkSize) - (chunkSize / 2));
    }

    bool ShouldWeUpdate(Vector3 playerPosition, Vector3 start)
    {
        var change = playerPosition - start;
        return Mathf.Abs(change.x) >= chunkSize || Mathf.Abs(change.y) >= chunkSize;
    }

    int WorldToChunkConversion(float worldPosition)
    {
        return (int)(Mathf.Floor(worldPosition/chunkSize) * chunkSize);
    }
}