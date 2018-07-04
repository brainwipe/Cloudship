using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour 
{
	public enum States
	{
		In, ReelIn, Out, ReelOut
	}

	public States State;

	public GameObject ropePrefab;
	public GameObject ClawPrefab;
	GameObject Claw;

	int segments = 20;
	float segmentLength = 4f;
	float reelOutSpeed = 4f;

	Vector3 startPosition;
	Vector3 limitPosition;

	void Start () 
	{
		CreateRope();
		startPosition = transform.localPosition;
		limitPosition = new Vector3(0, -segmentLength, 0);
		
		// TODO Remove
		State = States.ReelOut;
	}
	
	void Update () 
	{
		if (State == States.ReelOut)
		{
			var segmentCount = transform.childCount;
			var count = transform.childCount;
			for(int i=count-1; i > 0; i--)
			{
				var child = transform.GetChild(i);
				var joint = child.GetComponent<HingeJoint>();
				if (joint.connectedAnchor.y > limitPosition.y)
				{
					joint.connectedAnchor = joint.connectedAnchor + new Vector3(0, -Time.deltaTime * reelOutSpeed,0);
					break;
				}
			}
		}
	}

	void CreateRope()
	{
		var segment = GetComponent<Rigidbody>();
		var pos = transform.position;
		for(int i=0; i < segments; i++)
		{
			segment = CreateSegment(segment, pos, i);
			pos = segment.position;
		}
	}

	Rigidbody CreateSegment(Rigidbody previous, Vector3 position, int name)
	{
		var segment = Instantiate(
			ropePrefab, 
			position, 
			Quaternion.identity, 
			transform);
		segment.name = "Rope " + name;
		var hinge = segment.GetComponent<HingeJoint>();
		hinge.connectedBody = previous;
		
		return segment.GetComponent<Rigidbody>();
	}

	public void ReelOut()
	{
		State = States.ReelOut;
	}
}
