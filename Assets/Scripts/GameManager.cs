using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour 
{

	public WeatherSystemManager weatherSystemManager;

	public Cloudship playerCloudship;
	private Vector3 positionOfLastUpdate;
	private float sqrDisplacementBetweenUpdates = 220;
	void Start () 
	{
		weatherSystemManager.GenerateWeatherSystems(playerCloudship.transform.position);
	}

    void Update () 
	{
		if (ShouldTheWeatherSystemUpdate(playerCloudship.transform.position))
		{
			positionOfLastUpdate = playerCloudship.transform.position;
			weatherSystemManager.GenerateWeatherSystems(playerCloudship.transform.position);
		}
	}

	// TODO ROLA - premature optimisation? Test without this check
	private bool ShouldTheWeatherSystemUpdate(Vector3 playerCloudshipPosition)
	{
		return ((positionOfLastUpdate - playerCloudshipPosition).sqrMagnitude > 
			sqrDisplacementBetweenUpdates);
	}
}
