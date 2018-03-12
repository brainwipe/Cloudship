using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helm : MonoBehaviour {

	public Rigidbody player;

	public float wheelSpeed = 6f;

	Rigidbody rigidBody;

	void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	void Update () {
		var angles = new Vector3(
			transform.eulerAngles.x,
			transform.eulerAngles.y,
			player.transform.eulerAngles.y * wheelSpeed);

		transform.eulerAngles = angles;
		
		
	}
}
