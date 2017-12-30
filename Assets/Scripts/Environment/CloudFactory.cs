using System;
using System.Collections.Generic;
using UnityEngine;

public class CloudFactory : MonoBehaviour
{
    public GameObject cloudPrefab;
    public GameObject[] clouds;
    public int cloudCount = 20;
    public int distanceFromPlayer = 20;
    public int sqrDistanceFromPlayer = 400;
    public int furthestFromplayer = 25;
    public int sqrFurthestFromPlayer = 625;
    public int sqrElimiationDistanceFromPlayer = 800;
    public int sqrCloudSeparation = 10;

    IWindMaker windMaker;
    Cloudship playerCloudship;
    float windEffect = 0.15f;
    
    public void Start()
    {
        playerCloudship = GameManager.Instance.PlayerCloudship;
        windMaker = GameManager.Instance.WindMaker;
        CreateIntialClouds(playerCloudship.Position);
    }

    public void FixedUpdate()
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

    private void RemoveOldAddNew(Vector3 location, int cloudIndex)
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

    private void CreateIntialClouds(Vector3 location)
    {
        clouds = new GameObject[cloudCount];
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

    private Vector3? FindNewLocation(Vector3 location)
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

    private Vector3? FindInitialLocation(Vector3 location)
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

    private bool HasCloudNearby(Vector3 location)
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