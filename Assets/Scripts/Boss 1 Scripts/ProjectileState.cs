using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileState : BaseState
{
    private Boss _boss;
    private GameObject shockWaveAttack;

    public ProjectileState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
    }
    

    public override Type Tick()
    {
        _boss.animator.SetTrigger("Shockwave");
        shockWaveAttack = GameObject.Instantiate(_boss.shockWave) as GameObject;
        shockWaveAttack.transform.position = transform.position;
        Debug.Log("Projectile State!");
        return typeof(IdleState);
    }
}