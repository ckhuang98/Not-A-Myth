using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator_Attack3_Behavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       GameMaster.instance.playerStats.speed.Value = 0.6f;
       GameMaster.instance.playerStats.knockBackForce.Value = 1200f;
       if(GameMaster.instance.playerStats.unlockedGroundSmash.Value){
            GameMaster.instance.player.GetComponent<PlayerController>().slashAnimation.Play("GroundSmash", -1, 0f);
       } else{
           GameMaster.instance.player.GetComponent<PlayerController>().slashAnimation.Play("SlashAnim3", -1, 0f);
       }
       GameMaster.instance.combatManager.StartCoroutine(GameMaster.instance.combatManager.attackCooldown());
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    // }

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
