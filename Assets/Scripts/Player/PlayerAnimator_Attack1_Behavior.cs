using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator_Attack1_Behavior : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameMaster.instance.combatManager.player.attacked = true;
        GameMaster.instance.combatManager.canReceiveInput = true;
        if(GameMaster.instance.combatManager.player.getState() == "Normal"){
            GameMaster.instance.playerStats.speed.Value = 1f;
        }
        if(GameMaster.instance.playerStats.knockBackForce.Value != 300f){
            GameMaster.instance.playerStats.knockBackForce.Value = 300f;
        }
        GameMaster.instance.player.GetComponent<PlayerController>().slashAnimation.Play("SlashAnim1", -1, 0f);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameMaster.instance.combatManager.inputReceived){
            animator.SetTrigger("Attack2");
            GameMaster.instance.combatManager.InputManager();
            GameMaster.instance.combatManager.inputReceived = false;
        }
    }
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        // if (!CombatManager.instance.inputReceived){
        //     CombatManager.instance.StartCoroutine(CombatManager.instance.attackCooldown());
        // }
        
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
