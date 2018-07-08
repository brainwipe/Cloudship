using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotsamNearby : MonoBehaviour 
{
	void OnTriggerStay(Collider other)
	{
		var player = other.gameObject.GetComponentInParent<Cloudship>();
		if (player != null)
		{
			player.ReelOut();
		}
	}

	void OnTriggerExit(Collider other)
	{
		var player = other.gameObject.GetComponentInParent<Cloudship>();
		if (player != null)
		{
			player.ReelIn();
		}
	}
}
