using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anticyclone : MonoBehaviour {

	public float radius = 0;
	public float diameter = 20;
	private float pointOfBestSpeed = 0.7f;
	private float radiusOfBestSpeedInUnits = 0;
	private float overlapAmount = 0f;
	public float sqrNotOverlappedAmountInUnits = 0;
	private float speed = 0; // 10 - 100
	private float speedDenominator = 100f;
	private bool isClockwise = true;
	
	// TODO ROLA - remove when putting in clouds
	public GameObject Disc;

	void Start () 
	{
		Disc.transform.localScale = new Vector3(diameter, 0.1f, diameter);
	}

	internal void Setup(int diameter, int speed, bool isClockwise, float overlapAmout)
    {
		this.diameter = diameter;
		this.speed = speed;
		this.isClockwise = isClockwise;
		
		radius = diameter / 2;
		radiusOfBestSpeedInUnits = radius * pointOfBestSpeed;
		sqrNotOverlappedAmountInUnits = (radius * (1 - overlapAmount)) * (radius * (1 - overlapAmount));
    }

	void Update () 
	{
		var turn = speed * Time.deltaTime;
		if (!isClockwise)
		{
			turn = -turn;
		}
		transform.Rotate(new Vector3(0,turn,0));
	}

	internal Vector3 GetForceFor(Vector3 cloudshipPosition)
	{
		var heading = cloudshipPosition - transform.position;
        heading.y = 0;
        heading = heading * MagnitudeHat(heading) * (speed / speedDenominator);
        var unitYVector = new Vector3(0, -1 ,0);

        return Vector3.Cross(heading, unitYVector);
	}
/* 
	internal bool IsOverlapTooMuch(Vector3 other)
	{
		Debug.Log("Is too close: " + (transform.position - other).sqrMagnitude + " < " + sqrNotOverlappedAmountInUnits);
		return (transform.position - other).sqrMagnitude < sqrNotOverlappedAmountInUnits;
	}
*/
	// Returns hat function between 0 and 1, 1 being at the Point of best speed
	private float MagnitudeHat(Vector3 heading)
	{
		var magnitude = heading.magnitude;
		if (magnitude > radius)
		{
			return 0;
		}
	
		if (magnitude < radiusOfBestSpeedInUnits)
		{
			return magnitude / radiusOfBestSpeedInUnits;
		}
		
		return (radius - magnitude) / (radius - radiusOfBestSpeedInUnits);
	}
}
