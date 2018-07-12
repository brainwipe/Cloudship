using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotsamDial : MonoBehaviour 
{
	Cloudship player;
	public Transform pointer;
	public Transform proposedMarker;
	public BuildMenu buildMenu;
	float pointerSpeed = 4f;
	

	void Start () 
	{
		player = GameManager.Instance.PlayerCloudship;
	}
	
	void Update () 
	{
		var targetZEulerAngle = Maths.Rescale(0, 300, 0, player.MaxFlotsam, player.TotalFlotsam);
		var pointerSmoothedAngle = Mathf.Lerp(pointer.localEulerAngles.z, targetZEulerAngle, Time.deltaTime * pointerSpeed);
		pointer.localEulerAngles = new Vector3(0, 0, pointerSmoothedAngle);

		var proposedMarkerLocation = targetZEulerAngle;
		if(player.Mode == Cloudship.Modes.Build)
		{
			var proposedFlotsam = player.TotalFlotsam - buildMenu.SelectedBuilding.FlotsamCost;
			proposedMarkerLocation = Maths.Rescale(0, 300, 0, player.MaxFlotsam, proposedFlotsam);
		}
		
		var smoothedProposedAngle = Mathf.Lerp(proposedMarker.localEulerAngles.z, proposedMarkerLocation, Time.deltaTime * pointerSpeed);
		proposedMarker.localEulerAngles = new Vector3(0, 0, smoothedProposedAngle);
	}
}
