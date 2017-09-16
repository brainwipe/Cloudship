using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeatherSystemFactory : MonoBehaviour 
{
	public GameObject AnticyclonePrefab;
	public GameObject WeatherSystemsParent;

	private List<IWeatherSystemCreationStrategy> creationStrategies = 
		new List<IWeatherSystemCreationStrategy> 
		{
			new Small(), new Medium(), new Large()
		};
	
	internal Anticyclone Create(
		Vector3 location, 
		IWeatherSystemCreationStrategy weatherSystemCreationStrategy)
	{
		var weatherSystem = Instantiate(
			AnticyclonePrefab, 
			location,
			Quaternion.identity, 
			WeatherSystemsParent.transform);
		weatherSystem.tag = "WeatherSystem";
		var cyclone = weatherSystem.GetComponent<Anticyclone>();
		cyclone.Setup(weatherSystemCreationStrategy);
		return cyclone;
	}
	
	internal IWeatherSystemCreationStrategy FindCreationStrategy(int perlinNoiseValue) 
	{ 
		return creationStrategies.First(x => x.Accepts(perlinNoiseValue));
	}
}
