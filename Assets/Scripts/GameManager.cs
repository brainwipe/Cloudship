using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance = null;

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
