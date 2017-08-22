using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anticyclone : MonoBehaviour {

	private float pointOfBestSpeedInUnits = 0;
	private float radius = 0;
	public float Diameter = 25;
	public float PointOfBestSpeed = 0.7f;
	public float Speed = 10;
	public GameObject Disc;

	void Start () 
	{
		radius = Diameter / 2;
		pointOfBestSpeedInUnits = radius * PointOfBestSpeed;

		Disc.transform.localScale = new Vector3(Diameter, 0.1f, Diameter);
	}
	
	void Update () 
	{
		var turn = Speed * Time.deltaTime;
		transform.Rotate(new Vector3(0,turn,0));
	}

	public Vector3 GetForceFor(Vector3 cloudshipPosition)
	{
		var heading = cloudshipPosition - transform.position;
        heading.y = 0;
        heading = heading * MagnitudeHat(heading);
        var unitYVector = new Vector3(0, -1 ,0);

        return Vector3.Cross(heading, unitYVector);
	}

	// Returns hat function between 0 and 1, 1 being at the Point of best speed
	private float MagnitudeHat(Vector3 heading)
	{
		var magnitude = heading.magnitude;
		if (magnitude > radius)
		{
			return 0;
		}
	
		if (magnitude < pointOfBestSpeedInUnits)
		{
			return magnitude / pointOfBestSpeedInUnits;
		}
		
		return (radius - magnitude) / (radius - pointOfBestSpeedInUnits);
	}
}
