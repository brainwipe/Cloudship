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
		270f, 300f, 330f, 0, 30f, 60f, 90f
	};

	void Start() {
		player = GameManager.Instance.PlayerCloudship;
	}

	void Update () {
		UpdateThrust(Input.GetButtonUp("Throttle Up"), Input.GetButtonUp("Throttle Down"));
		UpdateEotHandlePosition();

		player.Thrust = thrustSettings[thrustIndex];
	}

	void UpdateEotHandlePosition()
	{
		float lerped = Mathf.LerpAngle(
			EotHandle.transform.localEulerAngles.z, 
			handlePositions[thrustIndex],
			Time.deltaTime * eotHandleSpeed);
		EotHandle.transform.localEulerAngles =  new Vector3(0,0,lerped);
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
