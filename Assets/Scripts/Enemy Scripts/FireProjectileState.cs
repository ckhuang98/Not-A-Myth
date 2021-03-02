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
    private bool instantiated = false;
    private float pauseTimer = 1f;

    public FireProjectileState(Enemy enemy) : base(enemy.gameObject) {
        _enemy = enemy;
    }

    public override Type Tick() {
        if (instantiated == false) {
            if (UnityEngine.Random.value > 0.5) {
            healing = GameObject.Instantiate(_enemy.healingProjectile) as GameObject;
            healing.transform.position = this.transform.position;
            instantiated = true;
        } else {
            damage = GameObject.Instantiate(_enemy.damageProjectile) as GameObject;
            damage.transform.position = this.transform.position;
            instantiated = true;
        }
        }
        if (pauseTimer > 0) {
            pauseTimer -= Time.deltaTime;
        } else {
            pauseTimer = 1f;
            instantiated = false;
            return typeof(MaintainDistanceState);
        }
        return typeof(FireProjectileState);
        
    }
}
