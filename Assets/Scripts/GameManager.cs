using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public Windicators windicators;

	public Cloudship playerCloudship;
	

	void Start () 
	{
		windicators.Generate(playerCloudship.transform.position);
	}

    void Update () 
	{
		windicators.UpdateWindicators(playerCloudship.transform.position);
	}
}
