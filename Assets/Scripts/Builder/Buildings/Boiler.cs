using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : MonoBehaviour 
{
	public ParticleSystem smoke;
	Building building;
	FlyingPhysics flyingPhysics;
	IFly parent;

	void Start () 
	{
		flyingPhysics = GetComponentInParent<FlyingPhysics>();
		parent = GetComponentInParent<IFly>();		
		building = GetComponent<Building>();

		var smokeEmitter = smoke.emission;
		smokeEmitter.enabled = !building.InMenu;
	}
	
	void FixedUpdate() 
	{
		if (building.InMenu)
		{
			return;
		}

		var smokeMain = smoke.main;
		var smokeEmitter = smoke.emission;

		smokeMain.startSpeed = Maths.Rescale(6, 10, 0, 1, Mathf.Abs(parent.CommandThrust));
		smokeEmitter.rateOverTime = Maths.Rescale(0.3f, 12, 0, 1f, Mathf.Abs(parent.CommandThrust));

		var windEffect = flyingPhysics.CycloneForce * 0.002f;
		var forceOverLifetime = smoke.forceOverLifetime;
		forceOverLifetime.x = windEffect.x;
		forceOverLifetime.y = windEffect.y;
		forceOverLifetime.z = windEffect.z; 
	}
}
