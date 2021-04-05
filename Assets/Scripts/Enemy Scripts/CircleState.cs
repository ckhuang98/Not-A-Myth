using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleState : BaseState
{
    private Enemy _enemy;
    private Transform target;
    //private float radius = 3f;
    //private float rotateSpeed = 2f;
    //private float angle;
    //private float speed = 5f;
    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //private Vector2 decisionTime = new Vector2(1, 4);
    //internal float decisionTimeCount = 0f;

    public CircleState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.Find("Player").GetComponent<Transform>();
        //decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);
    }

    public override Type Tick() {
        
        transform.RotateAround(new Vector3(target.position.x, target.position.y, 0f), new Vector3(0f, 0f, 1f), 90f * Time.deltaTime);
        /*
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
        */
        /*
        angle += rotateSpeed * Time.deltaTime;

        var center = new Vector2(target.position.x, target.position.y); 
        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        transform.position =  center + offset; 
        */

    /*
        if (decisionTimeCount >= 0)
        {
            decisionTimeCount -= Time.deltaTime;
            
        } else
        {
            decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);
            return typeof(ChaseState);
        }
    */

        return typeof(CircleState);
    }
}
