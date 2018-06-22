using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    private float maxLifeTime = 10f;
	private float damage = 10;
	public IAmAShip owner;
	public Telemetry Stats;

    void Start()
    {
		Stats.Start = transform.position;
        Destroy(gameObject, maxLifeTime);
    }

	void Update()
	{
		if(Stats.On)
		{
			Stats.Update(transform);
		}
	}
	
	void OnTriggerEnter(Collider other)
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

	[Serializable]
	public class Telemetry
	{
		public bool On;
		public Vector3 Start;
		Vector3 end;
		public float Range;

		public Telemetry()
		{
			end = Vector3.zero;
		}

		public void Update(Transform cannonball)
		{
			if (cannonball.position.y < 0 && end == Vector3.zero)
			{
				end = cannonball.position;
				Range = (Start - end).magnitude;
				Debug.Log("Cannonball Range: " + Range);
			}
		}
	}
}
