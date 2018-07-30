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

	TrailRenderer trail;
	float timeToStartTrail;
	float waitBeforeTrailStartInSecs = 0.5f;

    void Start()
    {
		Stats.Start(transform.position);
        Destroy(gameObject, maxLifeTime);
		trail = GetComponentInChildren<TrailRenderer>();
		timeToStartTrail = Time.time + waitBeforeTrailStartInSecs;
    }

	void Update()
	{
		if(Stats.On)
		{
			Stats.Update(transform);
		}
		if (Time.time > timeToStartTrail && !trail.emitting)
		{
			trail.emitting = true;
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
		System.Diagnostics.Stopwatch timer;
		public bool On;
		Vector3 startPosition;
		Vector3 endPosition;
		public float Range;
		public float TimeInFlight;

        public float StartElevation { get; internal set; }
        public float Force { get; internal set; }

        public Telemetry()
		{
			endPosition = Vector3.zero;
			timer = new System.Diagnostics.Stopwatch();
		}

		public void Start(Vector3 start)
		{
			timer.Start();
			startPosition = start;
		}

		public void End(Transform cannonball)
		{
			timer.Stop();
			Range = (startPosition - cannonball.position).magnitude;
			TimeInFlight = timer.Elapsed.Seconds;
			Debug.Log($"Cannonball: Range: {Range}, Time In Flight: {TimeInFlight}, Elevation: {StartElevation}");
			On = false;
		}

		public void Update(Transform cannonball)
		{
			if (!On)
			{
				return;
			}
			if (cannonball.position.y < 0 && endPosition == Vector3.zero)
			{
				End(cannonball);
			}
		}
	}
}
