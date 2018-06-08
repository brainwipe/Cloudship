using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    private float maxLifeTime = 10f;
	private float damage = 10;
	public IAmAShip owner;

    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

	private void OnTriggerEnter(Collider other)
	{
		var target = other.transform.GetComponentInParent<IAmAShip>();

		if (target == null || target == owner)
		{
			return;
		}

		var damageable = other.attachedRigidbody.GetComponentInParent<ITakeDamage>();
		if (damageable != null)
		{
			damageable.Damage(damage);
		}
		
		Destroy(gameObject);
	}
}
