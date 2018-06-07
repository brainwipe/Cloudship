using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public static bool GameIsPaused = false;
	
	public GameObject pauseMenuUi;
	bool cursorStateWhenPaused;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameIsPaused)
			{
				Cursor.visible = cursorStateWhenPaused;
				Resume();
			}
			else{
				cursorStateWhenPaused = Cursor.visible;
				Cursor.visible = true;
				Pause();
			}
		}
	}

	public void Resume()
	{
		pauseMenuUi.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	public void LoadMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(0);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	void Pause()
	{
		pauseMenuUi.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}
}
