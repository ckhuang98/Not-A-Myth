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
    //Area of attack has spawned
    private bool hasSpawned = false;

    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    /*
    Purpose: constructor recieves all needed values from enemy class.
    Recieves: the enemy class from the enemy.cs file.
    Returns: nothing.
    */
    public AttackState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy; 
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
        if (attackTimeCount >= 0f) {
            attackTimeCount -= Time.deltaTime;
            if (hasSpawned == false) {
                AoE = GameObject.Instantiate(_enemy.AOE) as GameObject;
                AoE.transform.position = this.transform.position;
                hasSpawned = true;
            }
        } else {
            attackTimeCount = .75f;
            GameObject.Destroy(AoE);
            hasSpawned = false;
            return typeof(ChaseState);
        }

        //If the enemy dies the area of effect must be destroyed in case it is
        //permanently.
        if (_enemy.healthAmount <= 0) {
            GameObject.Destroy(AoE);
        }

        return typeof(AttackState);
    }
}