using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpWindow : MonoBehaviour 
{
	Text text;

	TutorialStep[] Tutorial;
	int currentTutorialStepIndex = 0;
	float fadeoutStart = 0;

	void Start()
	{
		text = GetComponentInChildren<Text>();
		Tutorial = new[] {
			new TutorialStep("Welcome to the Cloudship Tutorial. Build, collect and fight! Hit [space] for your first task."),
			new TutorialStep("1. To move the camera, hold the right mouse button and move the mouse. [space]"),
			new TutorialStep("2. You need buildings on your Cloudship, press [B] for the build menu. [space]"),
			new TutorialStep("3. Use [W] and [S] to cycle through the buildings. [space]"),
			new TutorialStep("4. Choose a Boiler Chimney. [space]"),
			new TutorialStep("5. Build 2 Boiler Chimneys by clicking on the flat disc of the Cloudship. [space]"),
			new TutorialStep("6. Build a Bridge (the half-arch). A Boiler Chimney and Bridge are the bare minimum. [space]"),
			new TutorialStep("7. Making a building will use up flotsam on the little dial. [space]"),
			new TutorialStep("8. Buildings showing up in red in the menu you cannot afford. [space]"),
			new TutorialStep("9. Delete buildings by dragging them off your Cloudship, you will get the Flotsam back. [space]"),
			new TutorialStep("10. Press B to exit the build menu. [space]"),
			new TutorialStep("11. Tap [W] to increase speed setting, [S] to decrease and [A] and [D] to turn. [space]"),
			new TutorialStep("12. The black lumps over the ground are flotsam. Fly slowly up to one and a hook will drop. [space]"),
			new TutorialStep("13. The hook will drag the Flotsam up to you. [space]"),
			new TutorialStep("14. Build a cannon as soon as you can! [space]"),
			new TutorialStep("Congrats! You have completed the tutorial! [backspace] to go back or [space] to restart."),
		};
		SetTutorialText();
		
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Space))
		{
			if (HelpIsOpen)
			{
				if (currentTutorialStepIndex < Tutorial.Length - 1)
				{
					currentTutorialStepIndex++;	
					SetTutorialText();
				}
				else 
				{
					currentTutorialStepIndex = 0;	
					SetTutorialText();
				}
			}
			else
			{
				fadeoutStart = 0;
				currentTutorialStepIndex = 0;
				SetTutorialText();
				HelpContainer.SetActive(true);
			}
		}
		if (HelpIsOpen && Input.GetKeyUp(KeyCode.Backspace))
		{
			if (currentTutorialStepIndex > 0)
			{
				currentTutorialStepIndex--;	
			}
			else 
			{
				currentTutorialStepIndex = Tutorial.Length - 1;
			}
			SetTutorialText();
		}

		if (currentTutorialStepIndex == Tutorial.Length - 1)
		{
			fadeoutStart += Time.deltaTime;
		}
		else
		{
			fadeoutStart = 0;
		}

		if (fadeoutStart > 8f)
		{
			HelpContainer.SetActive(false);
			fadeoutStart = 0;
		}
	}

	void SetTutorialText()
	{
		text.text = Tutorial[currentTutorialStepIndex].Text;
	}

	public void SetText(string helpText)
	{
		if (!IsTutorialRunning)
		{
			text.text = helpText;
		}
	}

	public bool IsTutorialRunning => currentTutorialStepIndex < Tutorial.Length - 1;

	GameObject HelpContainer => transform.GetChild(0).gameObject;

	bool HelpIsOpen => HelpContainer.activeSelf;

	private class TutorialStep
	{
		public TutorialStep(string text)
		{
			Text = text;
		}

		public string Text { get; set; }
	}
}
