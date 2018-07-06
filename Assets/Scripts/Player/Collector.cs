﻿using System.Collections;
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

	int segments = 12;
	float segmentLength = 8f;
	float reelOutSpeed = 11f;
	float reelInSpeed = 8f;

	Vector3 startPosition;
	Vector3 limitPosition;

	void Start () 
	{
		CreateRope();
		startPosition = transform.localPosition;
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
		var count = transform.childCount;
		var areWeReeledOut = true;
		for(int i=count-1; i > 0; i--)
		{
			var child = transform.GetChild(i);
			var joint = child.GetComponent<HingeJoint>();
			if (joint.connectedAnchor.y > limitPosition.y)
			{
				areWeReeledOut = false;
				var nextInChain = transform.GetChild(i - 1);
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
		var count = transform.childCount;
		var areWeReeledIn = true;
		for(int i=count-2; i > 0; i--)
		{
			var child = transform.GetChild(i);
			var joint = child.GetComponent<HingeJoint>();
			if (joint.connectedAnchor.y < 0)
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
		Claw = Instantiate(ClawPrefab, transform.position, Quaternion.identity, transform);
		var clawHinge = Claw.GetComponent<HingeJoint>();
		clawHinge.connectedBody = segment;
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

	public void ReelIn()
	{
		State = States.ReelIn;
	}
}
