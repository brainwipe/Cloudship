﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helm : MonoBehaviour {

	public Cloudship player;

	public float wheelSpeed = 6f;

	void Update () {
		player.Turn = Input.GetAxis("Horizontal");

		var angles = new Vector3(
			transform.eulerAngles.x,
			transform.eulerAngles.y,
			player.transform.eulerAngles.y * wheelSpeed);

		transform.eulerAngles = angles;
		
		
	}
}
