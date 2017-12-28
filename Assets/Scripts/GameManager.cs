using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public Windicators windicators;

	public PressureBalls pressureBalls;

	public Cloudship playerCloudship;

	public CloudFactory cloudFactory;


	void Start () 
	{
		
		
	}

    void Update () 
	{
		windicators.Generate(playerCloudship.transform.position); // Uncomment to show the wind direction arrows
		//pressureBalls.Generate(playerCloudship.transform.position); // Uncomment to show the perlin pressure field
	}
}
