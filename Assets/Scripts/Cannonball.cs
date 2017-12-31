using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public LayerMask CraftMask;
    private float maxLifeTime = 3f;
	private float damage = 2;
	public ITakeDamage originator;

    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

	private void OnTriggerEnter(Collider other)
	{
		var targetRigidbody = other.GetComponent<Rigidbody>();
		if (!targetRigidbody)
		{
			return;
		}

		var target = targetRigidbody.GetComponent<ITakeDamage>();
		if (target == null || target == originator)
		{
			return;
		}

		// TODO reduce health
		
		Destroy(gameObject);
	}
}
