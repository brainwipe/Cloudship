using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance = null;

	// TODO This needs to go in a settings class when there are a few of them
	public static int DrawDistance = 200;

	public Cloudship PlayerCloudship;

	public IWindMaker WindMaker = null;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
		PlayerCloudship = FindObjectOfType<Cloudship>();
		WindMaker = new PerlinPressure();
	}
}
