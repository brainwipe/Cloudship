using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineVent : MonoBehaviour 
{
	ParticleSystem particles;
	FlyingPhysics flyingPhysics;
	IFly parent;

	public bool IAmLeft;
	
	void Awake() 
	{
		particles = GetComponent<ParticleSystem>();
		flyingPhysics = GetComponentInParent<FlyingPhysics>();
		parent = GetComponentInParent<IFly>();
	}

	void Update () 
	{
		var main = particles.main;
		var emitter = particles.emission;
		if (flyingPhysics.IsAdrift)
		{
			main.startSpeed = 0;
			emitter.rateOverTime = 0.3f;
		}	
		else
		{
			main.startSpeed = Maths.Rescale(0, 8, 0, flyingPhysics.TopSpeed, flyingPhysics.IndicatedAirSpeed);
			emitter.rateOverTime = Maths.Rescale(0.3f, 4, 0, 1f, parent.CommandThrust);
		}

		

		var windEffect = flyingPhysics.CycloneForce * 0.001f;
		var forceOverLifetime = particles.forceOverLifetime;
		forceOverLifetime.x = windEffect.x;
		forceOverLifetime.y = windEffect.y;
		forceOverLifetime.z = windEffect.z; 
	}
}
