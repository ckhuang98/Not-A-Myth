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
    private float angle;
    private bool gotAngle = false;
    private Transform target;

    public FireProjectileState(Enemy enemy) : base(enemy.gameObject) {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override Type Tick() {
        //Debug.Log("FIRING");
        _enemy.inAttackState = true;
        var delta_x = transform.position.x - target.position.x;
        var delta_y = transform.position.y - target.position.y;
        if (gotAngle == false) {
            angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
            if (angle < 0.0f) {
                angle = angle + 360f;
            }
            gotAngle = true;
        }

        ThrowProjectile(angle);
        _enemy.enemyAnimator.SetFloat("ImpAttackHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
        _enemy.enemyAnimator.SetFloat("ImpAttackVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
        
        if (_enemy.goToWalk == true) {
            _enemy.goToWalk = false;
            gotAngle = false;
            _enemy.inAttackState = false;
            return typeof(MaintainDistanceState);
        }
        return typeof(FireProjectileState);   
    }

    private void ThrowProjectile(float angle) {
        if (_enemy.doInstantiate == true) {
            if (UnityEngine.Random.value > 0.5) {
            healing = GameObject.Instantiate(_enemy.healingProjectile) as GameObject;
            healing.transform.position = this.transform.position;
            } else {
                damage = GameObject.Instantiate(_enemy.damageProjectile) as GameObject;
                damage.transform.position = this.transform.position;
            }
            _enemy.doInstantiate = false;
            // UP
            if (315 > angle && angle > 225) {
                _enemy.currMoveDirection = 0;
            } 
            // RIGHT
            if (225 > angle && angle > 135) {
                _enemy.currMoveDirection = 2;
            } 
            // DOWN
            if (135 > angle && angle > 45) {
                _enemy.currMoveDirection = 4;
            } 
            // LEFT
            if ((45 > angle && angle > 0) || (360 > angle && angle > 315)) {
                _enemy.currMoveDirection = 6;
            }
        }
    }
}

