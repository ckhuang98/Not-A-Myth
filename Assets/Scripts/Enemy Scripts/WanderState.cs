﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WanderState : BaseState
{
    private Enemy _enemy;
    private float speed = 1f;
    private Vector2 decisionTime = new Vector2(1, 4);
    internal float decisionTimeCount = 0f;

    //An array carrying all 8 movement options for the enemy
    internal Vector3[] moveDirections = new Vector3[] { Vector3.right, Vector3.left,
    Vector3.up, Vector3.down, Vector3.Normalize(Vector3.up + Vector3.right), Vector3.Normalize(Vector3.up + Vector3.left),
    Vector3.Normalize(Vector3.down + Vector3.right), Vector3.Normalize(Vector3.down + Vector3.left) };
    internal int currMoveDirection;

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
        
        transform.position += moveDirections[currMoveDirection] * Time.deltaTime * speed;

        if (decisionTimeCount >= 0)
        {
            decisionTimeCount -= Time.deltaTime;
            
        } else
        {
            decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);

            ChooseMoveDirection();
        }

        if (_enemy.inBounds == true)
        {
            return typeof(ChaseState);
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
        currMoveDirection = Mathf.FloorToInt(UnityEngine.Random.Range(0, moveDirections.Length));
    }

    
}
