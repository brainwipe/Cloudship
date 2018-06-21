using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFactory : MonoBehaviour
{
    public Enemy[] EnemyPrefabs;
    Cloudship player;

    float lastSeen;
    
    float intervalSinceLastSeen;

    void Start()
    {
        player = GameManager.Instance.PlayerCloudship;
    }

    void Update()
    {
        var enemies = GetComponentsInChildren<Enemy>();
        if (enemies.Length == 0)
        {
            var location = player.transform.position + new Vector3(1000, 0,0);
            Instantiate(EnemyPrefabs[0], location, Quaternion.identity, transform);
        }
    }
}