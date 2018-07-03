using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour 
{
	public GameObject ropePrefab;
	int segments = 20;

	void Start () {
		CreateRope();
	}
	
	void Update () {
		
	}

	void CreateRope()
	{
		var segment = GetComponent<Rigidbody>();
		var pos = transform.position;
		for(int i=0; i < segments; i++)
		{
			segment = CreateSegment(segment, pos);
			pos = segment.position + new Vector3(0, -4f, 0);
		}
	}

	Rigidbody CreateSegment(Rigidbody previous, Vector3 position)
	{
		var segment = Instantiate(
			ropePrefab, 
			position, 
			Quaternion.identity, 
			transform);
	
		var hinge = segment.GetComponent<HingeJoint>();
		hinge.connectedBody = previous;
		
		return segment.GetComponent<Rigidbody>();
	}
}
