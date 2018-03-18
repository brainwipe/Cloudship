using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassPlayerDirection : MonoBehaviour {

	public Rigidbody Player;

	void Update () {
		transform.localRotation = Quaternion.LookRotation(Player.velocity);
	}
}
