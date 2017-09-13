using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : StateMachineBehaviour
{
	protected GameObject enemy;
	protected GameObject player;
	protected Vector3 defaultHeading;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		defaultHeading = new Vector3(Random.Range(0f,1f), 0f, Random.Range(0f,1f));

		enemy = animator.gameObject;
		player = enemy.GetComponent<Enemy>().Player;
	}
}
