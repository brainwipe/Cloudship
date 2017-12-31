﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : StateMachineBehaviour
{
	protected Enemy enemy;
	protected Cloudship player;
	protected Vector3 defaultHeading;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		defaultHeading = new Vector3(Random.Range(0f,1f), 0f, Random.Range(0f,1f));

		enemy = animator.gameObject.GetComponent<Enemy>();
		player = GameManager.Instance.PlayerCloudship;
	}
}
