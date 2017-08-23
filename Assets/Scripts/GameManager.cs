using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject AnticyclonePrefab;

	private int playAreaX;
	private int playAreaZ;

	private GameObject[] weatherSystems;

	private float perlinScale = 0.1f;
	private int numberOfWeatherSystems = 20;
	private float sqrWeatherMinSeparation = 250;

	void Start () {
		playAreaX = 100;
		playAreaZ = 100;
		GenerateWeatherSystems();
	}

    void Update () {
		
	}

	private void GenerateWeatherSystems()
    {
		weatherSystems = new GameObject[numberOfWeatherSystems];
		var weatherSystemCount = 0;

        for (float x=0; x < playAreaX; x++)
		{
			for (float z=0; z < playAreaZ; z++)
			{
				float perlinX = x / (playAreaX * perlinScale);
				float perlinZ = z / (playAreaZ * perlinScale);
				int result = (int) (Mathf.PerlinNoise(perlinX,perlinZ) * 10);
				if (result < 8)
				{
					continue;
				}

				var newWeatherSystemLocation = new Vector3(x, 0f, z );

				if (weatherSystemCount == 0)
				{
					CreateWeatherSystem(newWeatherSystemLocation, weatherSystemCount);
					weatherSystemCount++;
					continue;
				}
				
				if (!HasWeatherSystemThere(newWeatherSystemLocation))
				{
					CreateWeatherSystem(newWeatherSystemLocation, weatherSystemCount);
					weatherSystemCount++;
				}
			}
		}
    }

	private void CreateWeatherSystem(Vector3 location, int index)
	{
		if (index >= weatherSystems.Length)
		{
			return;
		}
		weatherSystems[index] = Instantiate(AnticyclonePrefab, location, Quaternion.identity);
		weatherSystems[index].tag = "WeatherSystem";
	}

	private bool HasWeatherSystemThere(Vector3 locationToCheck)
	{
		foreach(var weatherSystem in weatherSystems)
		{
			if (weatherSystem == null)
			{
				continue;
			}

			var rawDifference = weatherSystem.transform.position - locationToCheck;
			if (rawDifference.sqrMagnitude < sqrWeatherMinSeparation)
			{
				return true;
			}
		}

		return false;
	}
}
