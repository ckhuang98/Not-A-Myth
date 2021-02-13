using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileState : BaseState
{
    private Boss _boss;

    public ProjectileState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
    }
    
    public override Type Tick()
    {
        Debug.Log("Projectile State!");
        return typeof(ProjectileState);
    }
}