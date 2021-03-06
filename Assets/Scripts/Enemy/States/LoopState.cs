using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopState : BaseState
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var vectorBetweenEnemyAndPlayer = player.transform.position - enemy.transform.position;
        var loopHeading = Vector3.Cross(vectorBetweenEnemyAndPlayer.normalized, Vector3.up);
        loopHeading.y = 0;
        enemy.Heading = loopHeading;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
