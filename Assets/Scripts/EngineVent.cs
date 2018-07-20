﻿using System.Collections;
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
	
	void FixedUpdate() 
	{
		/*
		var main = particles.main;
		//main.startSpeed = Maths.Rescale(5, 8, 0, flyingPhysics.TopSpeed, flyingPhysics.IndicatedAirSpeed);


		var windEffect = flyingPhysics.CycloneForce * 0.001f;
		var forceOverLifetime = particles.forceOverLifetime;
		forceOverLifetime.x = windEffect.x;
		forceOverLifetime.y = windEffect.y;
		forceOverLifetime.z = windEffect.z; */
	}
}
