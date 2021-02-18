using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IdleState : BaseState
{
    // This timer controls when to select a new state
    public float timer = 0;
    private Boss _boss;
    private int num;

    public IdleState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
        _boss.animator.Play("Idle");
        num = 0;
    }
    
    public override Type Tick()
    {
        if (timer < 3.0f) {
            timer += Time.deltaTime;
            if(timer < 3.0f && timer > 2.0f){
                if(num == 0){
                    num = UnityEngine.Random.Range(1,4);
                }
                if(num == 1){
                    _boss.speechText.text = "Eat This!";
                } else if(num == 2){
                    _boss.speechText.text = "Take That!";
                } else if(num ==3){
                    _boss.speechText.text = "Dodge This!";
                }
            }
        } else {
            if (num == 1) {
                timer = 0.0f;
                num = 0;
                return typeof(HammerState);
                //return typeof(SwordState);
                // return typeof(ProjectileState);
            } else if (num == 2) {
                timer = 0.0f;
                num = 0;
                // return typeof(HammerState);
                return typeof(SwordState);
                // return typeof(ProjectileState);
            } else if (num == 3) {
                timer = 0.0f;
                num = 0;
                // return typeof(HammerState);
                //return typeof(SwordState);
                return typeof(ProjectileState);
                
            }
         }

        //Debug.Log("Idle State!");
        return typeof(IdleState);
    }
}