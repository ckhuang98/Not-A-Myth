using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChaseState : BaseState
{
    private Enemy _enemy;
    //This is the player
    private Transform target;
    public float speed = 3f;
    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    /*
    Purpose: constructor recieves all needed values from enemy class and recieves
    the transform component from the player.
    Recieves: the enemy class from the enemy.cs file
    Returns: nothing
    */
    public ChaseState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    /*
    Purpose: Calls the MoveTowards function in order for the enemy to chase the
    player, If the enemy gets close enough to the player the attack state is started.
    Recieves: nothing
    Returns: the type of the current chase state consistently returned, until the 
    enemy gets close, then the type of the attack state is returned
    */
    public override Type Tick()
    {
        //Debug.Log("Chasing");
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        
        foreach (GameObject __enemy in enemies) {
            if (__enemy != null) {
                float currentDistance = Vector3.Distance(transform.position, __enemy.transform.position);
                if (currentDistance < 2.0f)
                {
                Vector3 dist = transform.position - __enemy.transform.position;
                transform.position += dist * Time.deltaTime;
                } 
            }
        }
        
        

        if (Vector2.Distance(transform.position, target.position) <= 1) {
            return typeof(AttackState);
        }

          
        
        return typeof(ChaseState);
    }
}
