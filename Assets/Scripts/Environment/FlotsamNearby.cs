using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotsamNearby : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{

		var player = other.gameObject.GetComponentInParent<Cloudship>();
		Debug.Log("reel out");
		if (player != null)
		{
			
			player.ReelOut();
		}
	}

	void OnTriggerExit(Collider other)
	{
		var player = other.gameObject.GetComponentInParent<Cloudship>();
		Debug.Log("reel in");
		if (player != null)
		{
			
			player.ReelIn();
		}
	}
}
