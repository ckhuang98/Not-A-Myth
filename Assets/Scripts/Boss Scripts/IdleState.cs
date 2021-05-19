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
        if(_boss.hammer && _boss.sword && _boss.shock){
            _boss.hammer = false;
            _boss.sword = false;
            _boss.shock = false;
        }
        if(transform.position.y != 6.5){
            // Vector3 temp = transform.position;
            // temp.y = 6.5f;
            // transform.position = temp;
        }
        timer += Time.deltaTime;
        if(timer < 2.7f && timer > 1.5f){
                if(num == 0){
                    num = UnityEngine.Random.Range(1,4);
                }
                if(num == 1 && _boss.hammer == true){
                    if(_boss.sword != true){
                        num = 2;
                    } else {
                        num = 3;
                    }
                } else if(num == 2 && _boss.sword == true){
                    if(_boss.hammer != true){
                        num = 1;
                    } else{
                        num = 3;
                    }
                } else if(num ==3 && _boss.shock == true){
                    if(_boss.hammer != true){
                        num = 1;
                    } else{
                        num = 2;
                    }
                }
                if(calledAnim == false){
                    _boss.startAnimation(num);
                    calledAnim = true;
                }
                
        } else {
            _boss.speechText.text = "";
            if (num == 1) {
                _boss.hammer = true;
                timer = 0.0f;
                num = 0;
                return typeof(HammerState);
                //return typeof(SwordState);
                // return typeof(ProjectileState);
            } else if (num == 2) {
                _boss.sword = true;
                timer = 0.0f;
                num = 0;
                // return typeof(HammerState);
                return typeof(SwordState);
                // return typeof(ProjectileState);
            } else if (num == 3) {
                _boss.shock = true;
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