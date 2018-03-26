using System;
using System.Collections.Generic;
using UnityEngine;

public class CloudFactory : MonoBehaviour
{
    public GameObject cloudPrefab;
    public GameObject[] clouds;
    public int cloudCount =100;
    int sqrCloudSeparation = 10000;
     
    IWindMaker windMaker;
    Cloudship playerCloudship;
    float windEffect = 0.15f;
    float eliminationDistanceFromPlayer;
    float sqrElimiationDistanceFromPlayer;
    float furthestFromplayer;
    float drawDistance;

    void Start()
    {
        drawDistance = GameManager.Instance.DrawDistance;
        eliminationDistanceFromPlayer = drawDistance + 1600;
        sqrElimiationDistanceFromPlayer = eliminationDistanceFromPlayer * eliminationDistanceFromPlayer;
        furthestFromplayer = drawDistance + 400;
        clouds = new GameObject[cloudCount];

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

            if (!HasCloudNearby(tryNewLocation) && (location - tryNewLocation).magnitude > furthestFromplayer)
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
                UnityEngine.Random.Range(location.x - drawDistance, location.x + drawDistance),
                transform.position.y,
                UnityEngine.Random.Range(location.z - drawDistance, location.z + drawDistance));

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