using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordState : BaseState
{
    private Boss _boss;

    public SwordState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
    }
    
    public override Type Tick()
    {
        Debug.Log("Sword State!");
        return typeof(SwordState);
    }
}