using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EelMaintainDistanceState : BaseState
{
    Enemy _enemy;
    private Transform target;

    public EelMaintainDistanceState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override Type Tick() {

        return typeof(EelMaintainDistanceState);
    }
}
