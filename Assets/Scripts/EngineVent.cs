using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineVent : MonoBehaviour 
{
	FlyingPhysics flyingPhysics;
	public ParticleSystem left;
	public ParticleSystem right;

	void Awake() 
	{
		flyingPhysics = GetComponentInParent<FlyingPhysics>();
	}
	
	void Update() 
	{
		
		/*var main = particles.main;
		main.startSpeed = Maths.Rescale(5, 8, 0, flyingPhysics.TopSpeed, flyingPhysics.IndicatedAirSpeed);*/


		var windEffect = flyingPhysics.CycloneForce * 0.001f;
		var forceOverLifetime = left.forceOverLifetime;
		forceOverLifetime.x = windEffect.x;
		forceOverLifetime.y = windEffect.y;
		forceOverLifetime.z = windEffect.z; 

		forceOverLifetime = right.forceOverLifetime;
		forceOverLifetime.x = windEffect.x;
		forceOverLifetime.y = windEffect.y;
		forceOverLifetime.z = windEffect.z; 
	}
}
