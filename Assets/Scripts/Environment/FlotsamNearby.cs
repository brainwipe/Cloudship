using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotsamNearby : MonoBehaviour 
{


	

	void OnTriggerEnter(Collider other)
	{
		var player = other.gameObject.GetComponentInParent<Cloudship>();
		if (player != null)
		{
			Debug.Log("player over!");
		}
	}

	void OnTriggerExit(Collider other)
	{
		var player = other.gameObject.GetComponentInParent<Cloudship>();
		if (player != null)
		{
			Debug.Log("player leave!");
		}
	}
}
