using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator_Idle_Behavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(GameMaster.instance.playerStats.inCombat.Value == false){
            GameMaster.instance.playerStats.speed.Value = GameMaster.instance.playerStats.sprintSpeed.Value;
            //Debug.Log("Out of combat");
        } else{
            GameMaster.instance.playerStats.speed.Value = GameMaster.instance.playerStats.maxSpeed.Value;
            //Debug.Log("In Combat");
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (CombatManager.instance.inputReceived){
           Debug.Log("input receieved!");
           animator.SetTrigger("Attack1");
           CombatManager.instance.InputManager();
           CombatManager.instance.inputReceived = false;
       }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
