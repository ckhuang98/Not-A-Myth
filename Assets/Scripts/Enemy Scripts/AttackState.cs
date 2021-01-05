using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackState : BaseState
{
    private Enemy _enemy;
    private float attackTimeCount = .75f;
    private GameObject AOE;

    public AttackState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
 
        
    }

    public override Type Tick()
    {
        if (attackTimeCount >= 0f) {
            attackTimeCount -= Time.deltaTime;
        } else {
            attackTimeCount = .75f;
            return typeof(ChaseState);
        }

        return typeof(AttackState);
    }
}