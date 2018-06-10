using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    private static GameManager instance;

	public GameModes Mode;

  	public float DrawDistance;

	public Cloudship PlayerCloudship;

	public IWindMaker WindMaker = null;

	public Camera Camera;

    public static GameManager Instance => instance;

	public LevelChanger LevelChanger;
    
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
		Mode = new GameModes();

		GameMode(MenuOutputData.isCreative);
	}

    internal void GameMode(bool isCreative)
    {
        if (isCreative)
		{
			Mode.SetCreativeMode();
		}
		else
		{
			Mode.SetSurvivalMode();
		}
    }

    internal void End()
    {
        LevelChanger.FadeToLevel(0);
    }
}
