using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using Pathfinding;

public class AttackState : BaseState
{
    private Enemy _enemy;
    private GameObject AoE;
    private GameObject fireParticles;
    private bool gotAngle = false;
    private float angle;
    private Transform target;

    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    /*
    Purpose: constructor recieves all needed values from enemy class.
    Recieves: the enemy class from the enemy.cs file.
    Returns: nothing.
    */
    public AttackState(Enemy enemy) : base (enemy.gameObject)
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
        
        InstantiateAoE(angle);
        _enemy.enemyAnimator.SetFloat("AttackHorzontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
        _enemy.enemyAnimator.SetFloat("AttackVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
        if (_enemy.goToWalk == true) {
            _enemy.goToWalk = false;
            gotAngle = false;
            return typeof(ChaseState);
        }

        return typeof(AttackState);
    }

    /*
    Purpose: Instantiates the area of effect, the placement is specified by the players
    angle in four different directions.
    Recieves: a float called "angle" which contains the angle the player is at relative to
    the player.
    Returns: nothng
    */
    private void InstantiateAoE(float angle) {
        if (_enemy.doInstantiate == true) {
            AoE = GameObject.Instantiate(_enemy.AOE) as GameObject;
            fireParticles = GameObject.Instantiate(_enemy.fireParticle) as GameObject;
            _enemy.doInstantiate = false; 
            // UP
            if (315 > angle && angle > 225) {
                AoE.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
                fireParticles.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
            } 
            // RIGHT
            if (225 > angle && angle > 135) {
                AoE.transform.position = 
                new Vector3(this.transform.position.x + 2, this.transform.position.y - 0.5f, this.transform.position.z);
                fireParticles.transform.position = 
                new Vector3(this.transform.position.x + 2, this.transform.position.y - 0.5f, this.transform.position.z);
            } 
            // DOWN
            if (135 > angle && angle > 45) {
                AoE.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y - 2.75f, this.transform.position.z);
                fireParticles.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y - 2.75f, this.transform.position.z);
            } 
            // LEFT
            if ((45 > angle && angle > 0) || (360 > angle && angle > 315)) {
                AoE.transform.position = 
                new Vector3(this.transform.position.x - 2, this.transform.position.y - 0.5f, this.transform.position.z);
                fireParticles.transform.position = 
                new Vector3(this.transform.position.x - 2, this.transform.position.y - 0.5f, this.transform.position.z);
            }
        }
    }
}