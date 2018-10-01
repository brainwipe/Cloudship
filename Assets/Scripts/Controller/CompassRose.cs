using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassRose : MonoBehaviour {

	CameraMovement cameraMovement;
	
	void Start () 
	{
		cameraMovement = GetComponentInParent<CameraMovement>();
	}
	
	
	void FixedUpdate () 
	{
		transform.localRotation = Quaternion.Euler(0, cameraMovement.transform.eulerAngles.y, 0);
	}
}
