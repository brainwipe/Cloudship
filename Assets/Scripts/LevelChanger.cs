using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour 
{

	public Animator animator;

	int levelToChangeTo;

	void Update () {
		if (Input.GetKey(KeyCode.P))	
		{
			FadeToLevel(0);
		}
	}

	public void FadeToLevel(int levelIndex)
	{
		levelToChangeTo = levelIndex;
		animator.SetTrigger("FadeOut");
	}

	public void ChangeLevel()
	{
		SceneManager.LoadScene(levelToChangeTo);
	}

}
