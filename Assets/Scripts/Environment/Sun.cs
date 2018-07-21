using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour 
{
	Vector3 offset;
	Transform cam;

	void Start () 
	{
		cam = Camera.main.transform;
		offset = transform.position - cam.position;	
	}
	
	
	void FixedUpdate () 
	{
		transform.position = cam.position - offset;
	}
}
