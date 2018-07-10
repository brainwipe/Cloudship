using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassPlayerDirection : MonoBehaviour 
{
	Camera playerCamera;
	Rigidbody player;

	void Start()
	{
		player = GameManager.Instance.PlayerCloudship.GetComponent<Rigidbody>();
		playerCamera = GameManager.Instance.Camera;
	}

	void Update () {
		var velocityInTwoAxis = new Vector3(player.velocity.x, 0, player.velocity.z);
		if (velocityInTwoAxis != Vector3.zero)
		{
			transform.localRotation = Quaternion.Inverse(Quaternion.Euler(0,playerCamera.transform.eulerAngles.y,0)) * Quaternion.LookRotation(velocityInTwoAxis);
		}
	}
}
