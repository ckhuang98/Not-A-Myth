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
    }
    
    public override Type Tick()
    {
        Debug.Log("Idle State!");
        return typeof(IdleState);
    }
}