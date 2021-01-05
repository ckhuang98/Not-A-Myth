using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChaseState : BaseState
{
    private Enemy _enemy;
    private Transform target;
    public float speed = 3f;

    public ChaseState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override Type Tick()
    {
        //Debug.Log("Chasing");
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) <= 1) {
            return typeof(AttackState);
        }  
        
        return typeof(ChaseState);
    }

}
