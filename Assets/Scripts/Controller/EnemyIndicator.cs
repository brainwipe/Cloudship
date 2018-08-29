using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour {

	EnemyFactory enemyFactory;
	Cloudship player;
	Camera playerCamera;
	Vector3 hidePosition = new Vector3(0, -0.08f, 0);
	float showSpeed = 2f;
	
	void Start () 
	{
		transform.localPosition = hidePosition;
		enemyFactory = GameManager.Instance.EnemyFactory;
		player = GameManager.Instance.PlayerCloudship;
		playerCamera = GameManager.Instance.Camera;
	}
	
	void Update () 
	{
		if (enemyFactory.HasAliveEnemies)
		{
			Show();
			Enemy enemy = enemyFactory.GetNearestEnemyToPlayer(); 
			var direction = enemy.Position - player.Position;
			var inTwoDimensions = new Vector3(direction.x, 0, direction.z);
			transform.localRotation = 
				Quaternion.Inverse(Quaternion.Euler(0,playerCamera.transform.eulerAngles.y,0)) * 
				Quaternion.LookRotation(inTwoDimensions);
		}
		else 
		{
			Hide();
		}
	}

    void Hide()
    {
        transform.localPosition = Vector3.Slerp(transform.localPosition, hidePosition, Time.deltaTime * showSpeed);
    }

    void Show()
    {
        transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.zero, Time.deltaTime * showSpeed);
    }
}
