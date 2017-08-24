using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeatherSystemManager : MonoBehaviour 
{

	public GameObject AnticyclonePrefab;
 	private List<Anticyclone> weatherSystems = new List<Anticyclone>();

	private int playArea = 100;
	private int maximumNumberOfWeatherSystems = 50;
	private float perlinScale = 0.3f;
	private float overlapAmount = 0.1f;
	public int weatherSystemCount = 0;

	void Start () 
	{
	}

	public void GenerateWeatherSystems(Vector3 position)
    {
		if (weatherSystemCount > maximumNumberOfWeatherSystems)
		{
			return;
		}

		var startX = position.x - (playArea / 2);
		var startZ = position.z - (playArea / 2);

		var endX = position.x + (playArea / 2);
		var endZ = position.z + (playArea / 2);

		var perlinDenominator = playArea * perlinScale;

        for (float x=startX; x < endX; x++)
		{
			for (float z=startZ; z < endZ; z++)
			{
				float perlinX = x / perlinDenominator;
				float perlinZ = z / perlinDenominator;
				int result = (int) (Mathf.PerlinNoise(perlinX,perlinZ) * 10);

				var newWeatherSystemLocation = new Vector3(x, 0f, z);

				int dia = 0; 
				int speed = 0;
				if (result < 6)
				{
					dia = 40;
					speed = 10;
				}
				else
				{
					dia = 20;
					speed = 20;
				}

				if (!HasWeatherSystemThere(newWeatherSystemLocation, dia))
				{
					CreateWeatherSystem(newWeatherSystemLocation, dia, speed, true);
				}
			}
		}
		// TODO ROLA - prune here
    }

	private void CreateWeatherSystem(Vector3 location, int size, int power, bool isClockwise)
	{
		var weatherSystem = Instantiate(AnticyclonePrefab, location, Quaternion.identity);
		weatherSystem.tag = "WeatherSystem";
		var cyclone = weatherSystem.GetComponent<Anticyclone>();
		cyclone.Setup(size, power, isClockwise, overlapAmount);
		weatherSystems.Add(cyclone);
		weatherSystemCount++;
	}

	private bool HasWeatherSystemThere(Vector3 locationToCheck, int diameterOfNewOne)
	{
		foreach(var weatherSystem in weatherSystems)
		{
			var radiusOfNewOne = diameterOfNewOne / 2;
			var distanceBetween = (weatherSystem.radius * (1 - overlapAmount)) + 
				(radiusOfNewOne * (1 - overlapAmount));
			
			var sqrDistanceBetween = distanceBetween * distanceBetween;

			if ((weatherSystem.transform.position - locationToCheck).sqrMagnitude < 
				sqrDistanceBetween)
			{
				return true;
			}
 		}
		return false;
		//return weatherSystems.Any(w => w.IsOverlapTooMuch(locationToCheck));
	}	
}
