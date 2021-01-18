using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleState : BaseState
{
    private Enemy _enemy;
    private Transform target;
    private float radius = 3f;
    private float rotateSpeed = 2f;
    private float angle;

    private Vector2 decisionTime = new Vector2(1, 4);
    internal float decisionTimeCount = 0f;

    public CircleState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);
    }

    public override Type Tick() {
        angle += rotateSpeed * Time.deltaTime;

        var center = new Vector2(target.position.x, target.position.y); 
        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        transform.position =  center + offset; 

        if (decisionTimeCount >= 0)
        {
            decisionTimeCount -= Time.deltaTime;
            
        } else
        {
            decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);
            return typeof(ChaseState);
        }

        return typeof(CircleState);
    }
}
