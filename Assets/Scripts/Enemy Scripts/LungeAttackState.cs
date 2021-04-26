using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LungeAttackState : BaseState
{
    Enemy _enemy;
    private Transform target;
    private float speed = 5;
    private bool gotAngle = false;
    private float angle;

    public LungeAttackState(Enemy enemy) : base (enemy.gameObject) {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    /*
    Purpose: Keeps track of player angle for attack direaction and calls everything to make the eel
    attack.
    Recieves: nothing.
    Returns: the type of the attack state itself consistently and returns
    the type of the ell maintain distance state when the animation is over.
    */
    public override Type Tick() {
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
        LungeAttack();
        _enemy.enemyAnimator.SetFloat("EelAttackHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
        _enemy.enemyAnimator.SetFloat("EelAttackVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
        if (_enemy.doLungeAttack == true) {
            transform.position += _enemy.moveDirections[_enemy.currMoveDirection] * speed * Time.deltaTime;
        }
        if (_enemy.goToWalk == true) {
            _enemy.goToWalk = false;
            gotAngle = false;
            _enemy.inAttackState = false;
            return typeof(EelMaintainDistanceState);
        }
        return typeof(LungeAttackState);
    }

    /*
    Purpose: uses the angle of the player to pick what direction to attack.
    Recieves: nothing.
    Returns: nothing.
    */
    private void LungeAttack() {
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
