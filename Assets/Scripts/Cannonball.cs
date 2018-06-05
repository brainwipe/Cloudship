﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    private float maxLifeTime = 10f;
	private float damage = 10;
	public ITakeDamage originator;

    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

	private void OnTriggerEnter(Collider other)
	{
		var targetRigidbody = other.attachedRigidbody;
		if (!targetRigidbody)
		{
			return;
		}

		var target = targetRigidbody.GetComponentInParent<ITakeDamage>();
		if (target == null || target == originator)
		{
			return;
		}

		target.Damage(damage);
		
		Destroy(gameObject);
	}
}
