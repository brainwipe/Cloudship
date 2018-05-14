using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFactory : MonoBehaviour
{
    public Cloudship playerCloudship;
    public Enemy Enemy;

    float lastSeen;
    
    float intervalSinceLastSeen;

    void Start()
    {   
        playerCloudship = GameManager.Instance.PlayerCloudship;
    }

    void Update()
    {
        if (ShouldISpawn())
        {
            Spawn();
        }
    }


    bool ShouldISpawn()
    {
        if (Enemy.ReadyToSpawn)
        {
            return true;
        }
        return true;
    }

    void Spawn()
    {
        Enemy.Reset();
    }
}