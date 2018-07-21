using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineVent : MonoBehaviour 
{
	ParticleSystem particles;
	FlyingPhysics flyingPhysics;

	public bool IAmLeft;
	
	void Awake() 
	{
		particles = GetComponent<ParticleSystem>();
		flyingPhysics = GetComponentInParent<FlyingPhysics>();
	}

	void Update () 
	{
		var main = particles.main;
		main.startSpeed = Maths.Rescale(0, 8, 0, flyingPhysics.TopSpeed, flyingPhysics.IndicatedAirSpeed);

		var windEffect = flyingPhysics.CycloneForce * 0.001f;
		var forceOverLifetime = particles.forceOverLifetime;
		forceOverLifetime.x = windEffect.x;
		forceOverLifetime.y = windEffect.y;
		forceOverLifetime.z = windEffect.z; 
	}
}
