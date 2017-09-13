using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeatherSystemManager : MonoBehaviour 
{
	public WeatherSystemFactory weatherSystemFactory;

 	private List<Anticyclone> weatherSystems = new List<Anticyclone>();
	private int playArea = 100;
	private float playAreaRadius = 0;
	private int maximumNumberOfWeatherSystems = 50;
	private float perlinScale = 0.3f;
	private float perlinDenominator = 0;
	private int weatherSystemCount = 0;

	void Awake()
	{
		perlinDenominator = playArea * perlinScale;
		playAreaRadius = playArea / 2;
	}

	public void GenerateWeatherSystems(Vector3 position)
    {
		if (weatherSystemCount > maximumNumberOfWeatherSystems)
		{
			return;
		}

		var startX = position.x - playAreaRadius;
		var startZ = position.z - playAreaRadius;

		var endX = position.x + playAreaRadius;
		var endZ = position.z + playAreaRadius;

        for (float x=startX; x < endX; x++)
		{
			for (float z=startZ; z < endZ; z++)
			{
				if (weatherSystemCount > maximumNumberOfWeatherSystems)
				{
					return;
				}

				TryToCreateNewSystem(x, z);
			}
		}
		PruneDistantWeatherSystems(position);
    }

	private void PruneDistantWeatherSystems(Vector3 position)
	{
		var pruneList = new List<Anticyclone>();

		// TODO ROLA - investigate sorting after creation loop to avoid going through this whole list
		foreach(var weatherSystem in weatherSystems)
		{
			var distance = weatherSystem.transform.position - position;
			var furthestDistanceAway = playArea + weatherSystem.radius;
			var sqrFurthestDistanceAway = furthestDistanceAway * furthestDistanceAway;
			if (distance.sqrMagnitude > sqrFurthestDistanceAway)
			{

				pruneList.Add(weatherSystem);
			}
 		}

		weatherSystems = weatherSystems.Except(pruneList).ToList();
		pruneList.ForEach(w => GameObject.Destroy(w.gameObject));
		weatherSystemCount -= pruneList.Count();
	}

	private void TryToCreateNewSystem(float x, float z)
	{
		var newWeatherSystemLocation = new Vector3(x, 0f, z);
		int perlinNoiseValue = FindPerlinNoiseValue(newWeatherSystemLocation);

		var weatherSystemCreationStrategy = weatherSystemFactory.FindCreationStrategy(perlinNoiseValue);

		if (!HasWeatherSystemThere(newWeatherSystemLocation, weatherSystemCreationStrategy))
		{
			weatherSystems.Add(
				weatherSystemFactory.Create(newWeatherSystemLocation, weatherSystemCreationStrategy));
			weatherSystemCount++;
		}
	}

	private int FindPerlinNoiseValue(Vector3 worldPosition)
	{
		float perlinX = worldPosition.x / perlinDenominator;
		float perlinZ = worldPosition.z / perlinDenominator;
		return (int) (Mathf.PerlinNoise(perlinX,perlinZ) * 10);
	}

	private bool HasWeatherSystemThere(Vector3 locationToCheck, 
		IWeatherSystemCreationStrategy weatherSystemCreationStrategy)
	{
		foreach(var weatherSystem in weatherSystems)
		{
			if (weatherSystem.IsTooCloseTo(locationToCheck, weatherSystemCreationStrategy.Radius))
			{
				return true;
			}
 		}
		return false;
	}	
}
