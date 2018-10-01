using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WispyCloudTrails : MonoBehaviour 
{
	ParticleSystem wisps;
	IWindMaker windMaker;
	Particle[] particles;

	void Start () 
	{
		windMaker = GameManager.Instance.WindMaker;
		wisps = GetComponent<ParticleSystem>();
		particles = new Particle[wisps.main.maxParticles];
	}
	
	void LateUpdate() 
	{
		var smokeMain = wisps.main;
		var smokeEmitter = wisps.emission;
		var count = wisps.GetParticles(particles);
		for(int i=0; i < count; i++)
		{
			particles[i].velocity = windMaker.GetCycloneForce(particles[i].position) * 0.001f;
		}
		wisps.SetParticles(particles, count);
	}

}
