using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject AnticyclonePrefab;

	private int playArea;
 	private List<GameObject> weatherSystems;

	private float perlinScale = 0.1f;
	private float sqrWeatherMinSeparation = 250;

	void Start () {
		playArea = 100;
		GenerateWeatherSystems(new Vector3(50,0,50));
	}

    void Update () {
		
	}

	private void GenerateWeatherSystems(Vector3 position)
    {
		weatherSystems = new List<GameObject>();
		var startX = position.x - (playArea / 2);
		var startZ = position.z - (playArea / 2);

		var endX = position.x + (playArea / 2);
		var endZ = position.z + (playArea / 2);

		var perlinDenominator = (playArea * perlinScale);

        for (float x=startX; x < endX; x++)
		{
			for (float z=startZ; z < endZ; z++)
			{
				float perlinX = x / perlinDenominator;
				float perlinZ = z / perlinDenominator;
				int result = (int) (Mathf.PerlinNoise(perlinX,perlinZ) * 10);
				if (result < 8)
				{
					continue;
				}

				var newWeatherSystemLocation = new Vector3(x, 0f, z);

				if (!weatherSystems.Any())
				{
					CreateWeatherSystem(newWeatherSystemLocation);
					continue;
				}
				
				if (!HasWeatherSystemThere(newWeatherSystemLocation))
				{
					CreateWeatherSystem(newWeatherSystemLocation);
				}
			}
		}
    }

	private void CreateWeatherSystem(Vector3 location)
	{
		var weatherSystem = Instantiate(AnticyclonePrefab, location, 
		Quaternion.identity);
		weatherSystem.tag = "WeatherSystem";
		weatherSystems.Add(weatherSystem);
	}

	private bool HasWeatherSystemThere(Vector3 locationToCheck)
	{
		return weatherSystems.Any(w => 
		(w.transform.position - locationToCheck).sqrMagnitude < sqrWeatherMinSeparation);
	}
}
