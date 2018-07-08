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

	int segments = 13;
	float segmentLength = 8f;
	float reelOutSpeed = 15f;
	float reelInSpeed = 12f;

	Vector3 limitPosition;

	void Start () 
	{
		CreateRope();
		limitPosition = new Vector3(0, -segmentLength, 0);
	}
	
	void Update () 
	{
		if (State == States.ReelOut)
		{
			ReelOutUpdate();
		}
		else if (State == States.ReelIn)
		{
			ReelInUpdate();
		}
	}

	void ReelOutUpdate()
	{
		var areWeReeledOut = true;
		var children = gameObject.GetComponentsInChildren<Transform>();
		for(int i=children.Length-1; i > 1; i--)
		{ 
			var child = children[i];
			var joint = child.gameObject.GetComponent<Joint>();
			if (joint != null && joint.connectedAnchor.y > limitPosition.y)
			{
				areWeReeledOut = false;
				var nextInChain = children[i-1];
				var renderer = nextInChain.GetComponent<Renderer>();
				if (renderer != null)
				{
					renderer.enabled = true;
				}
				joint.connectedAnchor = joint.connectedAnchor + new Vector3(0, -Time.deltaTime * reelOutSpeed,0);
				break;
			}
		}

		if (areWeReeledOut)
		{
			State = States.Out;
		}
	}

	void ReelInUpdate()
	{
		var children = gameObject.GetComponentsInChildren<Transform>();
		
		var areWeReeledIn = true;
		for(int i=0; i < children.Length-1; i++)
		{
			var child = children[i];
			var joint = child.GetComponent<Joint>();
			if (joint != null && joint.connectedAnchor.y < 0)
			{
				areWeReeledIn = false;
				joint.connectedAnchor = joint.connectedAnchor + new Vector3(0, Time.deltaTime * reelInSpeed,0);
				break;
			}
			else
			{
				var renderer = child.GetComponent<Renderer>();
				if (renderer != null)
				{
					renderer.enabled = false;
				}
			}
		}

		if (areWeReeledIn)
		{
			State = States.In;
		}
	}

	void CreateRope()
	{
		var segment = GetComponent<Rigidbody>();
		for(int i=0; i < segments; i++)
		{
			segment = CreateSegment(segment, transform.position, i);
		}
		Claw = Instantiate(ClawPrefab, transform.position, Quaternion.identity, segment.transform);
		var clawHinge = Claw.GetComponent<Joint>();
		clawHinge.connectedBody = segment;
	}

	Rigidbody CreateSegment(Rigidbody previous, Vector3 position, int name)
	{
		var segment = Instantiate(
			ropePrefab, 
			position, 
			Quaternion.identity, 
			previous.transform);
		segment.name = "Rope " + name;
		var hinge = segment.GetComponent<Joint>();
		hinge.connectedBody = previous;
		
		return segment.GetComponent<Rigidbody>();
	}

	public void ReelOut()
	{
		State = States.ReelOut;
	}

	public void ReelIn()
	{
		State = States.ReelIn;
	}
}
