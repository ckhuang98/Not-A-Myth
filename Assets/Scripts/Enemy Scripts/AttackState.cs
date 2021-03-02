using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using Pathfinding;

public class AttackState : BaseState
{
    private Enemy _enemy;
    //How long the enemy will be "attacking"
    private float attackTimeCount = .75f;
    private GameObject AoE;
    private GameObject fireParticles;
    //Area of attack has spawned
    private bool hasSpawned = false;
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
    of time. The moment time is up, the area of attack disapears and the
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
        
        Debug.Log(angle);
        InstantiateAoE(angle);
        _enemy.enemyAnimator.SetFloat("AttackHorzontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
        _enemy.enemyAnimator.SetFloat("AttackVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
        if (_enemy.goToWalk == true) {
            _enemy.goToWalk = false;
            gotAngle = false;
            return typeof(ChaseState);
        }
        //Debug.Log("GOIGN");
        //_enemy.enemyAnimator.Play("AttackHorizontal");
        /*
        if (attackTimeCount >= 0f) {
            attackTimeCount -= Time.deltaTime;
            if (hasSpawned == false) {
                InstantiateAoE();
                hasSpawned = true;
            }
        } else {
            attackTimeCount = .75f;
            hasSpawned = false;
            
            return typeof(ChaseState);
        }
        */

        return typeof(AttackState);
    }

    private void InstantiateAoE(float angle) {
        if (_enemy.doInstantiate == true) {
            AoE = GameObject.Instantiate(_enemy.AOE) as GameObject;
            fireParticles = GameObject.Instantiate(_enemy.fireParticle) as GameObject;
            _enemy.doInstantiate = false; 
            if (315 > angle && angle > 225) {
                Debug.Log("Up Attack");
                AoE.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
                fireParticles.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
            } 

            if (225 > angle && angle > 135) {
                Debug.Log("Right Attack");
                AoE.transform.position = 
                new Vector3(this.transform.position.x + 2, this.transform.position.y - 0.5f, this.transform.position.z);
                fireParticles.transform.position = 
                new Vector3(this.transform.position.x + 2, this.transform.position.y - 0.5f, this.transform.position.z);
            } 
    
            if (135 > angle && angle > 45) {
                Debug.Log("Down attack");
                AoE.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y - 2.75f, this.transform.position.z);
                fireParticles.transform.position = 
                new Vector3(this.transform.position.x, this.transform.position.y - 2.75f, this.transform.position.z);
            } 
    
            if ((45 > angle && angle > 0) || (360 > angle && angle > 315)) {
                Debug.Log("Left Attack");
                AoE.transform.position = 
                new Vector3(this.transform.position.x - 2, this.transform.position.y - 0.5f, this.transform.position.z);
                fireParticles.transform.position = 
                new Vector3(this.transform.position.x - 2, this.transform.position.y - 0.5f, this.transform.position.z);
            }
        }
    }
}