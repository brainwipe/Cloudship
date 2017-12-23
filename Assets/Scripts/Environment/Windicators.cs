using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Windicators : MonoBehaviour
{
    public GameObject weatherIndicator;

    public WeatherSystemManager weatherSystemManager;

    int distanceFromPlayer = 20;
    float sqrDisplacementBetweenUpdates = 100;
    private Vector3 positionOfLastUpdate;

    int indicatorGridWith = 2;
    Dictionary<Vector3, GameObject> indicators = new Dictionary<Vector3, GameObject>();
    
    public void Generate(Vector3 centre)
    {
        int startX = (int)centre.x - distanceFromPlayer;
        int endX = (int)centre.x + distanceFromPlayer;
        int startZ = (int)centre.z - distanceFromPlayer;
        int endZ = (int)centre.z + distanceFromPlayer;

        for (int x = startX; x < endX; x++)
        {
            for (int z = startZ; z < endZ; z++)
            {
                var position = new Vector3(x, 0, z);
                if (MakeNewIndicator(position) && !AlreadyIndicatorThere(position))
                {
                    var startDirection = weatherSystemManager.WindDirectionAt(position);
                    indicators[position] = Instantiate(weatherIndicator, position, startDirection);
                }
            }
        }
    }

    internal void UpdateWindicators(Vector3 position)
    {
        if (ShouldTheWeatherSystemUpdate(position))
		{
			positionOfLastUpdate = position;
			Generate(position);
		}
    }

    private bool AlreadyIndicatorThere(Vector3 position)
    {
        return indicators.ContainsKey(position);
    }

    private bool MakeNewIndicator(Vector3 position)
    {
        return (position.x % indicatorGridWith == 0) && 
        (position.z % indicatorGridWith == 0);
    }

    private bool ShouldTheWeatherSystemUpdate(Vector3 playerCloudshipPosition)
	{
		return ((positionOfLastUpdate - playerCloudshipPosition).sqrMagnitude > 
			sqrDisplacementBetweenUpdates);
	}

}
