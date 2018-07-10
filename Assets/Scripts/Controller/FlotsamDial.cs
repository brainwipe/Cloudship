using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotsamDial : MonoBehaviour 
{
	Cloudship player;
	public Transform pointer;
	float pointerSpeed = 0.5f;

	void Start () 
	{
		player = GameManager.Instance.PlayerCloudship;
		
	}
	
	void Update () 
	{
		var targetZEulerAngle = Maths.Rescale(-150, 150, 0, player.MaxFlotsam, player.TotalFlotsam);
		var newZAngle = Mathf.Lerp(pointer.localEulerAngles.z, targetZEulerAngle, Time.deltaTime * pointerSpeed);
		
		if (newZAngle < 0)
		{
			newZAngle = 360 - newZAngle;
		}

		pointer.localEulerAngles = new Vector3(0, 0, newZAngle);
	}


}
