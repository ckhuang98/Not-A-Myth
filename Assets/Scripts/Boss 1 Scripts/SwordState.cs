using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwordState : BaseState
{
    private Boss _boss;
    private GameObject slashAttack;

    public SwordState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
    }
    
    public override Type Tick()
    {
        slashAttack = GameObject.Instantiate(_boss.slash) as GameObject;
        slashAttack.transform.position = transform.position;
        Debug.Log("Sword State!");
        return typeof(IdleState);
    }
}