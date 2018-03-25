using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassPlayerDirection : MonoBehaviour {

	public Rigidbody Player;
	public Transform Camera;

	void Update () {
		if (Player.velocity.sqrMagnitude > 0)
		{
			var velocityInTwoAxis = new Vector3(Player.velocity.x, 0, Player.velocity.z);
			transform.localRotation = Quaternion.Inverse(Quaternion.Euler(0,Camera.transform.eulerAngles.y,0)) * Quaternion.LookRotation(velocityInTwoAxis);
		}
	}
}
