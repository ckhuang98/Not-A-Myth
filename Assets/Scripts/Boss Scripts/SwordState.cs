using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SwordState : BaseState
{
    private Boss _boss;
    private GameObject slashAttack;

    Vector3 bossPos;

    Vector3 slashPos;

    public SwordState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
        bossPos = transform.position;
        bossPos.y += 2 ;
        slashPos = bossPos;
    }
    
    public override Type Tick()
    {
        _boss.attacking = true;
        slashAttack = GameObject.Instantiate(_boss.slash) as GameObject;
        // if (_boss.targetLastPos == "Center") { // Center
        //     slashAttack.transform.position = transform.position;
        // } else if (_boss.targetLastPos == "Right") { // Right
        //     slashPos.x += 5;
        //     slashAttack.transform.position = slashPos;
        // } else if (_boss.targetLastPos == "Left") { // Left
        //     slashPos.x -= 5;
        //     slashAttack.transform.position = slashPos;
        // }
        slashAttack.transform.position = transform.position;
        slashPos = bossPos;
        Debug.Log("Sword State!");
        return typeof(IdleState);
    }
}