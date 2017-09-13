using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	private Transform player;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Start () {
		
	}
	
	void Update () {
	}
}
