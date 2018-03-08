using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    private static GameManager instance;

    // TODO This needs to go in a settings class when there are a few of them
    public static int DrawDistance = 200;

	public Cloudship PlayerCloudship;

	public IWindMaker WindMaker = null;

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
		DontDestroyOnLoad(gameObject);
		PlayerCloudship = FindObjectOfType<Cloudship>();
		WindMaker = new PerlinPressure();
	}
}
