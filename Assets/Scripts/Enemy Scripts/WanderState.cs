using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class WanderState : BaseState
{
    private Enemy _enemy;
    private float speed = 3f;
    private Vector2 decisionTime = new Vector2(1, 4);
    internal float decisionTimeCount = 0f;

    internal int currMoveDirection;

    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    /*
    Purpose: constructor recieves all needed values from enemy class, sets the
    first time when to change direction, and sets the first direction to move in
    Recieves: the enemy class from the enemy.cs file
    Returns: nothing
    */
    public WanderState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
        decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);
        ChooseMoveDirection();
    }


    /*
    Purpose: moves the enemy in the current direction chosen and calls the change
    direction function when the move time is up. If the enemy wanders inside the 
    bounds of the player, chase state is started.
    Recieves: nothing
    Returns: the type of the wander state constatntly, until in the players bounds.
    The type of the chase state is returned.
    */
    public override Type Tick()
    {
        //transform.position += _enemy.moveDirections[currMoveDirection] * Time.deltaTime * speed;

        if (decisionTimeCount >= 0)
        {
            decisionTimeCount -= Time.deltaTime;
            
        } else
        {
            decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);

            ChooseMoveDirection();
        }

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

        if (_enemy.inBounds == true)
        {
            return typeof(WanderState);
        }

        for (int i = 0; i < _enemy.moveDirections.Count(); i ++) {
            if (_enemy.castList[i].collider != null) {
                return typeof(ChaseState);
            } 
        }
 
        return typeof(WanderState);
    }

    /*
    Purpose: Changes the current move direction of the enemy to a new one
    Recieves: nothing
    Returns: nothing
    */
    private void ChooseMoveDirection()
    {
        currMoveDirection = Mathf.FloorToInt(UnityEngine.Random.Range(0, _enemy.moveDirections.Length));
    }
}
