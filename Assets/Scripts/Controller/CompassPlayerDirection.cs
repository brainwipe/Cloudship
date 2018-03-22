using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassPlayerDirection : MonoBehaviour {

	public Rigidbody Player;
	public Transform Camera;

	void Update () {
		if (Player.velocity.sqrMagnitude > 0)
		{
			transform.localRotation = Quaternion.Inverse(Quaternion.Euler(0,Camera.transform.eulerAngles.y,0)) * Quaternion.LookRotation(Player.velocity);
		}
	}
}
