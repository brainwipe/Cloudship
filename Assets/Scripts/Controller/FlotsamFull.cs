using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotsamFull : MonoBehaviour 
{

	public float InX;

	Cloudship player;
	float sproing = 5f;

	void Start () 
	{
		player = GameManager.Instance.PlayerCloudship;
	}
	
	
	void Update () 
	{
		var total = player.Stores.TotalFlotsam;
		var max = player.Stores.MaxFlotsam;

		float proposedX = InX;
		if (total == max)
		{
			proposedX = 0;
		}
		var smoothed = Mathf.Lerp(transform.localPosition.x, proposedX, Time.deltaTime * sproing);
		transform.localPosition =  new Vector3(smoothed, 0, 0);
	}
}
