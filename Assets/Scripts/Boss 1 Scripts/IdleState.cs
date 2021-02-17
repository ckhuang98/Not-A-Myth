using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IdleState : BaseState
{
    // This timer controls when to select a new state
    public float timer = 0;
    private Boss _boss;

    public IdleState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
        _boss.animator.Play("Idle");
    }
    
    public override Type Tick()
    {
        if (timer < 3.0f) {
            timer += Time.deltaTime;
        } else {
            var num = UnityEngine.Random.Range(1,4);
            if (num == 1) {
                timer = 0.0f;
                return typeof(HammerState);
            } else if (num == 2) {
                timer = 0.0f;
                return typeof(SwordState);
            } else if (num == 3) {
                timer = 0.0f;
                return typeof(ProjectileState);
                
            }
         }

        //Debug.Log("Idle State!");
        return typeof(IdleState);
    }
}