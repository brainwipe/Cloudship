using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eot : MonoBehaviour {

	Cloudship player;
	public GameObject EotHandle;

	int thrustIndex = 3;

	float eotHandleSpeed = 3f;
	float[] thrustSettings = {
		-1f, -0.5f, -0.3f, 0, 0.3f, 0.5f, 1f
	};

	float[] handlePositions = {
		90f, 60f, 30f, 0, 330f, 300f, 270f
	};

	void Start() {
		player = GameManager.Instance.PlayerCloudship;
	}

	void Update () {
		if (player.Mode == Cloudship.Modes.Drive)
		{
			UpdateThrust(Input.GetButtonUp("Throttle Up"), Input.GetButtonUp("Throttle Down"));
			UpdateEotHandlePosition();
			player.Thrust = thrustSettings[thrustIndex];
		}
	}

	void UpdateEotHandlePosition()
	{
        var target = Quaternion.Euler(0,0,handlePositions[thrustIndex]);
        EotHandle.transform.localRotation = Quaternion.Lerp(EotHandle.transform.localRotation, target, Time.deltaTime * eotHandleSpeed);
	}

	void UpdateThrust(bool up, bool down)
	{
		if (up)
		{
			thrustIndex++;
		}
		else if (down)
		{
			thrustIndex--;
		}

		if (thrustIndex < 0)
		{
			thrustIndex = 0;
		}

		if (thrustIndex > thrustSettings.Length -1)
		{
			thrustIndex = thrustSettings.Length -1;
		}
	}

}
