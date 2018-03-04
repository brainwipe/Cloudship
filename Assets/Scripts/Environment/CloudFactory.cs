using System;
using System.Collections.Generic;
using UnityEngine;

public class CloudFactory : MonoBehaviour
{
    public GameObject cloudPrefab;
    public GameObject[] clouds;
    public int cloudCount =100;
    int sqrCloudSeparation = 20;
    public float cloudSizeUpper = 2f;

    public float cloudSizeLower = 1f;
    
    IWindMaker windMaker;
    Cloudship playerCloudship;
    float windEffect = 0.15f;
    int eliminationDistanceFromPlayer;
    int sqrElimiationDistanceFromPlayer;
    int distanceFromPlayer;
    int sqrDistanceFromPlayer;
    int furthestFromplayer;

    void Awake()
    {
        var drawDistance = GameManager.DrawDistance;
        distanceFromPlayer = drawDistance;
        sqrDistanceFromPlayer = distanceFromPlayer * distanceFromPlayer;
        eliminationDistanceFromPlayer = drawDistance + 40;
        sqrElimiationDistanceFromPlayer = eliminationDistanceFromPlayer * eliminationDistanceFromPlayer;
        furthestFromplayer = distanceFromPlayer + 10;
        clouds = new GameObject[cloudCount];
    }

    void Start()
    {
        playerCloudship = GameManager.Instance.PlayerCloudship;
        windMaker = GameManager.Instance.WindMaker;
        CreateIntialClouds(playerCloudship.Position);
    }

    void FixedUpdate()
    {
        for (int i=0; i < cloudCount; i++)
        {
            var cloud = clouds[i];
            if (cloud == null)
            {
                continue;
            }

            var cycloneForce = windMaker.GetCycloneForce(cloud.transform.position) * Time.deltaTime;
            cloud.transform.position += cycloneForce * windEffect;
            RemoveOldAddNew(playerCloudship.Position, i);
        }
    }

    void RemoveOldAddNew(Vector3 location, int cloudIndex)
    {
        var cloud = clouds[cloudIndex];

        if ((cloud.transform.position - location).sqrMagnitude > sqrElimiationDistanceFromPlayer)
        {
            var newLocation = FindNewLocation(location);
            if (newLocation == null)
            {
                return;
            }
            cloud.transform.position = newLocation.Value;
        }
    }

    void CreateIntialClouds(Vector3 location)
    {
        for(var i=0; i<cloudCount; i++)
        {
            var newLocation = FindInitialLocation(location);
            if (newLocation == null)
            {
                return;
            }
            
            clouds[i] = Instantiate(cloudPrefab, newLocation.Value, Quaternion.identity);
            clouds[i].transform.parent = this.transform;
            float scale = UnityEngine.Random.Range(cloudSizeLower, cloudSizeUpper);
            clouds[i].transform.localScale = new Vector3(scale,scale,scale);
            float yOffset = scale / 2;
            clouds[i].transform.position += new Vector3(0, 6 * yOffset, 0);
        }
    }

    Vector3? FindNewLocation(Vector3 location)
    {
        for (var i=0; i< 100; i++)
        {
            var tryNewLocation = new Vector3(
                UnityEngine.Random.Range(location.x - furthestFromplayer, location.x + furthestFromplayer),
                0,
                UnityEngine.Random.Range(location.z - furthestFromplayer, location.z + furthestFromplayer));

            if (!HasCloudNearby(tryNewLocation) && (location - tryNewLocation).sqrMagnitude > sqrDistanceFromPlayer)
            {
                return tryNewLocation;
            }
        }
        return null;
    }

    Vector3? FindInitialLocation(Vector3 location)
    {
        for (var i=0; i< 100; i++)
        {
            var tryNewLocation = new Vector3(
                UnityEngine.Random.Range(location.x - distanceFromPlayer, location.x + distanceFromPlayer),
                0,
                UnityEngine.Random.Range(location.z - distanceFromPlayer, location.z + distanceFromPlayer));

            if (!HasCloudNearby(tryNewLocation))
            {
                return tryNewLocation;
            }
        }
        return null;
    }

    bool HasCloudNearby(Vector3 location)
    {
        foreach(var cloud in clouds)
        {
            if (cloud == null)
            {
                continue;
            }
            var sqrMag = (location - cloud.transform.position).sqrMagnitude;
            if (sqrMag <= sqrCloudSeparation)
            {
                return true;
            }
        }

        return false;
    }
}