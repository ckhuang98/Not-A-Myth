using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FireProjectileState : BaseState
{
    private Enemy _enemy;

    private GameObject damage;
    private GameObject healing;

    public FireProjectileState(Enemy enemy) : base(enemy.gameObject) {
        _enemy = enemy;
    }

    public override Type Tick() {
        Debug.Log("FIRE EVERYTHING!");
        if (UnityEngine.Random.value > 0.5) {
            healing = GameObject.Instantiate(_enemy.healingProjectile) as GameObject;
            healing.transform.position = this.transform.position;
            return typeof(MaintainDistanceState);
        } else {
            damage = GameObject.Instantiate(_enemy.damageProjectile) as GameObject;
            damage.transform.position = this.transform.position;
            return typeof(MaintainDistanceState);
        }
        return typeof(FireProjectileState);
    }
}
