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
                if (CanIMakeANewIndicator(position))
                {
                    var rotation = weatherSystemManager.WindDirectionAt(position);
                    
                    var magnitude = weatherSystemManager.WindMagnitudeAt(position) * 1.2f;
                    var newScale = new Vector3(magnitude, 0.5f, magnitude) ;

                    if (!AlreadyIndicatorThere(position))
                    {
                        indicators[position] = Instantiate(weatherIndicator, position, rotation);
                        indicators[position].transform.localScale = newScale;
                        SetColour(indicators[position], rotation);
                    }
                }
            }
        }
    }

    private void SetColour(GameObject indicator, Quaternion rotation)
    {
        var vectorAxis = Vector3.up;
        float angle;
        rotation.ToAngleAxis(out angle, out vectorAxis);
        angle = Mathf.Abs(angle);
        Debug.Log(angle);
        float h = angle / 360f;
        var color = Color.HSVToRGB(h, 1f, 1f);

        Renderer rend = indicator.GetComponent<Renderer>();
        rend.material.color = color;
    }

    internal void UpdateWindicators(Vector3 position)
    {
        if (ShouldIMakeMoreWindicators(position))
		{
			positionOfLastUpdate = position;
			Generate(position);
		}
    }

    private bool AlreadyIndicatorThere(Vector3 position)
    {
        return indicators.ContainsKey(position);
    }

    private bool CanIMakeANewIndicator(Vector3 position)
    {
        return (position.x % indicatorGridWith == 0) && 
        (position.z % indicatorGridWith == 0);
    }

    private bool ShouldIMakeMoreWindicators(Vector3 playerCloudshipPosition)
	{
		return ((positionOfLastUpdate - playerCloudshipPosition).sqrMagnitude > 
			sqrDisplacementBetweenUpdates);
	}

}
