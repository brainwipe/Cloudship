using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFactory : MonoBehaviour
{
    public Enemy[] EnemyPrefabs;
    Cloudship player;

    float lastTimeWeSawAnEnemy;
    public float MinSecsSpawn = 120f;
    public float MaxSecsSpawn = 420f;
    public float WidthOfSpawnCone = 60f;
    public float MinDistance = 3000f;
    public float MaxDistance = 4000f;
    public float SpawnAltitude = 180f;

    void Start()
    {
        player = GameManager.Instance.PlayerCloudship;
    }

    void Update()
    {
        if (IsIttimeForAnEnemy())
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        var location = FindSpawnLocation();
        Instantiate(EnemyPrefabs[0], location, Quaternion.identity, transform);
    }

    Vector3 FindSpawnLocation()
    {
        var bearing = player.transform.eulerAngles.y;
        var randomBearing = Random.Range(bearing - WidthOfSpawnCone, bearing + WidthOfSpawnCone);
        var randomDistance = Random.Range(MinDistance, MaxDistance);

        var positionWithoutY = player.transform.position + (Quaternion.AngleAxis(randomBearing, Vector3.up) * Vector3.forward * randomDistance);
        
        positionWithoutY.y = SpawnAltitude;
        Debug.Log($"Spawn At: Player Bearing: {bearing}, Bearing: {randomBearing}, Distance: {randomDistance}, Position: {positionWithoutY}");
        return positionWithoutY;
    }

    public void ResetTimer()
    {
        lastTimeWeSawAnEnemy = Time.time;
    }

    bool IsIttimeForAnEnemy()
    {
        var durationSinceLastTime = lastTimeWeSawAnEnemy - Time.time;

        if (durationSinceLastTime < MinSecsSpawn)
        {
            return false;
        }
        var probability = Maths.Rescale(0,1,MinSecsSpawn, MaxSecsSpawn, durationSinceLastTime);
        var result = Random.Range(0,1);
        Debug.Log("Duration:" + durationSinceLastTime + " Prob:" + probability + " Result: " + result);
        return result < probability;
    }
}