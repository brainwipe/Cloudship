using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var direction = player.transform.position - enemy.transform.position;
        enemy.transform.rotation = Quaternion.Slerp(
			enemy.transform.rotation,
            Quaternion.LookRotation(direction),
            Time.deltaTime);
		enemy.transform.Translate(Vector3.forward * Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
