﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using Pathfinding;

public class SwordAttackState : BaseState
{
    private Enemy _enemy;
    private GameObject slashAttack;
    private GameObject warning;
    private bool gotAngle = false;
    private float angle;
    private Transform target;
    private float horizontal;
    private float vertical;

    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    /*
    Purpose: constructor recieves all needed values from enemy class.
    Recieves: the enemy class from the enemy.cs file.
    Returns: nothing.
    */
    public SwordAttackState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy; 
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    /*
    Purpose: Spawns the area of attack of the enemy for a short amount 
    of time. The moment the animation is over, the area of effect disapears and the
    chase state is started.
    Recieves: nothing
    Returns: the type of the attack state itself consistently and returns
    the type of the chase state when the specified time is up.
    */
    public override Type Tick()
    {
        var delta_x = transform.position.x - target.position.x;
        var delta_y = transform.position.y - target.position.y;
        if (gotAngle == false) {
            angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
            if (angle < 0.0f) {
                angle = angle + 360f;
            }
            gotAngle = true;
        }
        
        _enemy.enemyAnimator.SetFloat("SwordAttackHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
        _enemy.enemyAnimator.SetFloat("SwordAttackVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
        horizontal = _enemy.enemyAnimator.GetFloat("SwordAttackHorizontal");
        vertical = _enemy.enemyAnimator.GetFloat("SwordAttackVertical");
        InstantiateWarning();
        InstantiateSlash();
        if (_enemy.goToWalk == true) {
            _enemy.goToWalk = false;
            gotAngle = false;
            return typeof(ChaseState);
        }

        return typeof(SwordAttackState);
    }

    /*
    Purpose: Instantiates the area of effect, the placement is specified by the players
    angle in four different directions.
    Recieves: a float called "angle" which contains the angle the player is at relative to
    the player.
    Returns: nothng
    */
    private void InstantiateSlash() {
        if (_enemy.doInstantiate == true) {
            slashAttack = GameObject.Instantiate(_enemy.slash, this.transform) as GameObject;
            //slashAttack.GetAngle(angle);
            _enemy.doInstantiate = false; 
            // UP
            if (vertical > 0) {
                slashAttack.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
            } 
            // RIGHT
            if (horizontal > 0) {
                slashAttack.transform.position = 
                new Vector3(this.transform.position.x + 1.5f, this.transform.position.y - 0.1f, this.transform.position.z);
            } 
            // DOWN
            if (vertical < 0) {
                slashAttack.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y - 2.5f, this.transform.position.z);
            } 
            // LEFT
            if (horizontal < 0) {
                slashAttack.transform.position = 
                new Vector3(this.transform.position.x - 1.5f, this.transform.position.y - 0.1f, this.transform.position.z);
            }
        }
    }

    private void InstantiateWarning() {
        if (_enemy.instantiateWarning == true) {
            warning = GameObject.Instantiate(_enemy.slashWarning) as GameObject;
            //Debug.Log("Instantiating");
            _enemy.instantiateWarning = false; 
            // UP
            if (vertical > 0) {
                warning.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
                warning.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            } 
            // RIGHT
            if (horizontal > 0) {
                warning.transform.position = 
                new Vector3(this.transform.position.x + 1.5f, this.transform.position.y - 0.1f, this.transform.position.z);
            } 
            // DOWN
            if (vertical < 0) {
                warning.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y - 2.5f, this.transform.position.z);
                warning.transform.localRotation = Quaternion.Euler(0f, 0f, 270f);
            } 
            // LEFT
            if (horizontal < 0) {
                warning.transform.position = 
                new Vector3(this.transform.position.x - 1.5f, this.transform.position.y - 0.1f, this.transform.position.z);
                warning.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            }
        }
    }
}