using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Flotsam : MonoBehaviour {

	public float Value = 100;

    internal void Attach(Claw claw)
    {
        GetComponentsInChildren<Collider>().Single(c => c.gameObject.tag == "FlotsamNearby").enabled = false;
		transform.parent = claw.transform;
		gameObject.layer = LayerMask.NameToLayer("Default");
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			GameManager.Instance.PlayerCloudship.Collect(Value);
			Destroy(this.gameObject);
		}
	}
}
