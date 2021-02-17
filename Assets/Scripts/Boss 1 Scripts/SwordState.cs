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
        bossPos.y += 2;
        slashPos = bossPos;
    }
    
    public override Type Tick()
    {
        _boss.animator.SetTrigger("Slash");
        slashAttack = GameObject.Instantiate(_boss.slash) as GameObject;
        var num = UnityEngine.Random.Range(1,4);
            if (num == 1) {
                slashAttack.transform.position = slashPos;
            } else if (num == 2) {
                slashPos.x += 4;
                slashAttack.transform.position = slashPos;
            } else if (num == 3) {
                slashPos.x -= 4;
                slashAttack.transform.position = slashPos;
            }
        slashPos = bossPos;
        Debug.Log("Sword State!");
        return typeof(IdleState);
    }
}