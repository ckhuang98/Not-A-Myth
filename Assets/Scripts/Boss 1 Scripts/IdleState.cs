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

    private bool calledAnim = false;

    public IdleState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
        num = 0;
    }
    
    public override Type Tick()
    {
        timer += Time.deltaTime;
        if(timer < 2.7f && timer > 1.5f){
                if(num == 0){
                    num = UnityEngine.Random.Range(1,4);
                }
                if(num == 1){
                    _boss.speechText.text = "Cone";
                } else if(num == 2){
                    _boss.speechText.text = "Slash";
                } else if(num ==3){
                    _boss.speechText.text = "Shockwave";
                }
                if(calledAnim == false){
                    _boss.startAnimation(num);
                    calledAnim = true;
                }
                
        } else {
            _boss.speechText.text = "";
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
                //return typeof(HammerState);
                //return typeof(SwordState);
                return typeof(ProjectileState);
                
            }
            calledAnim = false;
         }

        //Debug.Log("Idle State!");
        return typeof(IdleState);
    }
}