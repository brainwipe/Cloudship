using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anticyclone : MonoBehaviour {


	public float Size = 10;

	public float Speed = 10;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		var turn = Speed * Time.deltaTime;
		transform.Rotate(new Vector3(0,turn,0));
	}
}
