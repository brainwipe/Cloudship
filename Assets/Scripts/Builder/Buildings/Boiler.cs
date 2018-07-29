using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : MonoBehaviour 
{
	Building building;
	ParticleSystem particles;
	FlyingPhysics flyingPhysics;
	IFly parent;

	void Start () 
	{
		particles = GetComponentInChildren<ParticleSystem>();
		flyingPhysics = GetComponentInParent<FlyingPhysics>();
		parent = GetComponentInParent<IFly>();		
		building = GetComponent<Building>();

		var emitter = particles.emission;
		emitter.enabled = !building.InMenu;
	}
	
	void FixedUpdate() 
	{
		if (building.InMenu)
		{
			return;
		}

		var main = particles.main;
		var emitter = particles.emission;

		main.startSpeed = Maths.Rescale(0, 3, 0, 1, Mathf.Abs(parent.CommandThrust));
		emitter.rateOverTime = Maths.Rescale(0.3f, 4, 0, 1f, Mathf.Abs(parent.CommandThrust));

		var windEffect = flyingPhysics.CycloneForce * 0.002f;
		var forceOverLifetime = particles.forceOverLifetime;
		forceOverLifetime.x = windEffect.x;
		forceOverLifetime.y = windEffect.y;
		forceOverLifetime.z = windEffect.z; 
	}
}
