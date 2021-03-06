using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlotsamCollect : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
            var value = GetComponentInParent<Flotsam>().Value;
			GameManager.Instance.PlayerCloudship.Stores.AddFlotsam(value);
			Destroy(transform.parent.gameObject);
		}
	}
}