using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineVent : MonoBehaviour 
{
	FlyingPhysics flyingPhysics;
	ParticleSystem particles;

	void Awake() 
	{
		particles = GetComponentInChildren<ParticleSystem>();
		flyingPhysics = GetComponentInParent<FlyingPhysics>();
	}
	
	
	void FixedUpdate () 
	{
		var main = particles.main;
		//main.startSpeed = Maths.Rescale(3, 7, 0, flyingPhysics.TopSpeed, flyingPhysics.IndicatedAirspeed);

		var forceOverLifetime = particles.forceOverLifetime;
		Debug.Log(flyingPhysics.CycloneForce);
		forceOverLifetime.xMultiplier = flyingPhysics.CycloneForce.x;
		forceOverLifetime.yMultiplier = flyingPhysics.CycloneForce.y;
		forceOverLifetime.zMultiplier = flyingPhysics.CycloneForce.z;
	}
}
