using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator_Attack2_Behavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CombatManager.instance.player.attacked = true;
        CombatManager.instance.canReceiveInput = true;
        GameMaster.instance.playerStats.speed.Value = 0.8f;
        GameMaster.instance.playerStats.knockBackForce.Value = 600f;
        CombatManager.instance.player.slashAnimation.Play("SlashAnim2", -1, 0f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (CombatManager.instance.inputReceived){
            animator.SetTrigger("Attack3");
            CombatManager.instance.InputManager();
            CombatManager.instance.inputReceived = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CombatManager.instance.player.attacked = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
