using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public TMP_Dropdown resolutionDropdown;
	public LevelChanger levelChanger;

	Resolution[] resolutions;

	

	void Awake()
	{
		Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, true);
	}

	void Start()
	{
		resolutions = Screen.resolutions;
		resolutionDropdown.ClearOptions();

		var options = new List<string>();
		var currentResolutionIndex = 0;
		for(int i=0; i<resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "Hz";
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width &&
			resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}
		
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();
	}

	public void PlayGame()
	{
		PlayerPrefs.SetInt("Load", 0);
		levelChanger.FadeToLevel(1);
	}

	public void LoadGame()
	{
		PlayerPrefs.SetInt("Load", 1);
		levelChanger.FadeToLevel(1);
	}

	public void QuitGame()
	{
		Debug.Log("Quit");
		Application.Quit();
	}

	public void SetGameMode(bool isCreative)
	{
		MenuOutputData.isCreative = true;
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}

	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	public void SetResolution(int resolutionIndex)
	{
		var resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}
}
