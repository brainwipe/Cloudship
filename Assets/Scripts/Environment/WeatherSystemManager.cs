using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class WeatherSystemManager : MonoBehaviour
{
    public GameObject weatherIndicator;

    int numberToCreate = 500;
    int distanceFromPlayer = 20;
    float perlinScale = 30f;

    int indicatorGridWith = 2;

    Dictionary<Vector3, GameObject> indicators = new Dictionary<Vector3, GameObject>();

    public void GenerateWeatherIndicators(Vector3 centre)
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
                    var startDirection = WindDirectionAt(position);
                    indicators[position] = Instantiate(weatherIndicator, position, startDirection);
                }
            }
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

    public Vector3 GetCycloneForce(Vector3 cycloneForce)
    {
        return  WindDirectionAt(cycloneForce) * (Vector3.back * 5);
    }

    public Quaternion WindDirectionAt(Vector3 location)
    {
        float perlinX = location.x / perlinScale;
		float perlinZ = location.z / perlinScale;
		var noise = Mathf.PerlinNoise(perlinX,perlinZ);
        var noiseAngle = noise * 360;
        return Quaternion.AngleAxis(noiseAngle, Vector3.up);
    }
}