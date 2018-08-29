using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    public static bool ShowTutorial = true;
	public static bool SetToLoad;

	public GameModes Mode;

  	public float DrawDistance;

	public Cloudship PlayerCloudship;
	public HelpWindow Help;
	public IWindMaker WindMaker = null;
	public Camera Camera;
    public Transform Cannonballs;
	public LevelChanger LevelChanger;
	public TerrainFactory TerrainFactory;
	public EnemyFactory EnemyFactory;

	public Switches Features;

	public bool GameIsPaused = false;
    
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

		if (Help.gameObject.activeSelf)
		{
			Help.gameObject.SetActive(ShowTutorial);
		}
	}

	void Start()
	{
		var buildMenu = FindObjectOfType<BuildMenu>();
		PlayerStart.SetupCloudship(PlayerCloudship, buildMenu);
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

	[Serializable]
	public class Switches
	{
		public bool WindMovement;
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if(SetToLoad)
		{
			var fileManager = GetComponent<FileManager>();
			fileManager.Load();
		}
	}

}
