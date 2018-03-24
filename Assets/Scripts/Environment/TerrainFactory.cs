using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TerrainFactory : MonoBehaviour
{
    public static string TerrainTag = "Terrain";

    int chunkSize = 10;
    int chunkRadius;
    Dictionary<Vector3, TerrainChunk> chunks = new Dictionary<Vector3, TerrainChunk>();
    
    Vector3 start;
    float timestampForThisUpdateLoop;

    Cloudship playerCloudship;

    public GameObject chunkPrefab;

    void Awake()
    {
        start = Vector3.zero;
        chunkRadius = GameManager.DrawDistance / chunkSize;
    }

    void Start()
    {   
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
                    var chunkGameObj = Instantiate(chunkPrefab, pos, Quaternion.identity);
                    chunkGameObj.tag = TerrainTag;
                    var chunk = chunkGameObj.GetComponent<TerrainChunk>();
                    chunk.transform.parent = this.transform;
                    chunks.Add(pos, chunk);
                }
                chunks[pos].TimeUpdated = timestampForThisUpdateLoop;
            }
        }

        var outDatedChunks = chunks
        .Where(c => c.Value.TimeUpdated != timestampForThisUpdateLoop)
        .Select(c => c.Value)
        .ToList();
        
        foreach(var removeChunk in outDatedChunks)
        {
            Destroy(removeChunk);
        }

        start = playerCloudship.transform.position;
    }

    bool ShouldWeUpdate(Vector3 playerPosition, Vector3 start)
    {
        var change = playerPosition - start;

        return Mathf.Abs(change.x) >= chunkSize || Mathf.Abs(change.y) >= chunkSize;
    }

    int WorldToChunkConversion(float worldPosition)
    {
        return (int)(Mathf.Floor(worldPosition/chunkSize)) * chunkSize;
    }
}