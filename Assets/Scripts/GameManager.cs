using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    private static GameManager instance;

  	public float DrawDistance;

	public Cloudship PlayerCloudship;

	public IWindMaker WindMaker = null;

	public Camera Camera;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
	{
		if (instance == null)
		{
			instance = this;

		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}

		PlayerCloudship = FindObjectOfType<Cloudship>();
		WindMaker = new PerlinPressure();
		DrawDistance = Camera.farClipPlane + 500;
	}
}
