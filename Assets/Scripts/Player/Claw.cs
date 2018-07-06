using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour {

	void OnCollisionEnter(Collision collisionInfo)
	{
		if (collisionInfo.gameObject.tag == "Flotsam")
		{
			var flotsam = collisionInfo.gameObject.GetComponentInParent<Flotsam>();
			flotsam.Attach(this);
			GetComponentInParent<Collector>().ReelIn();
		}
	}
}
