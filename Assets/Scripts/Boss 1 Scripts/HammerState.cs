using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HammerState : BaseState
{
    private Boss _boss;

    public HammerState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
    }
    
    public override Type Tick()
    {
        Debug.Log("Hammer State!");
        return typeof(HammerState);
    }
}