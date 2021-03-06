﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : StateMachineBehaviour
{
	protected Enemy enemy;
	protected Cloudship player;
	protected IWindMaker windMaker;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemy = animator.gameObject.GetComponent<Enemy>();
		player = GameManager.Instance.PlayerCloudship;
		windMaker = GameManager.Instance.WindMaker;
	}
}
