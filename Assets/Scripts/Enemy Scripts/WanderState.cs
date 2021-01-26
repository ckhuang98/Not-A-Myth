using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class WanderState : BaseState
{
    private Enemy _enemy;
    private float speed = 1f;
    private Vector2 decisionTime = new Vector2(1, 4);
    internal float decisionTimeCount = 0f;
    private bool choice;

    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    bool hasMoved = false;
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
        _enemy.currMoveDirection = ChooseMoveDirection();
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
        //transform.position += _enemy.moveDirections[_enemy.currMoveDirection] * Time.deltaTime * speed;
        
        if (decisionTimeCount >= 0)
        {
            decisionTimeCount -= Time.deltaTime;
            
        } else
        {
            if (hasMoved == false) {
                hasMoved = true;
            }
            decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);
            _enemy.currMoveDirection = ChooseMoveDirection();
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

        if (_enemy.inBounds == true) {
            return typeof(ChaseState);
        }
 
        return typeof(WanderState);
    }

    /*
    Purpose: Changes the current move direction of the enemy to a new one
    Recieves: nothing
    Returns: nothing
    */
    private int ChooseMoveDirection()
    { 
        //Random Movement at first call
        if (hasMoved == false) {
            return Mathf.FloorToInt(UnityEngine.Random.Range(0, _enemy.moveDirections.Length));
        } else { 

            // RIGHT
            if (_enemy.currMoveDirection == 0) {
                choice = (UnityEngine.Random.value > 0.5f);
                if (choice == true) {
                    // Turn right up
                    return 6;
                }      
                else {
                    // Turn right down
                    return 7;
                }
            }

            // LEFT
            if (_enemy.currMoveDirection == 1) {
                choice = (UnityEngine.Random.value > 0.5f);
                if (choice == true) {
                    // Turn left up
                    return 4;
                }      
                else {
                    // Turn left down
                    return 5;
                }
            }

            // UP
            if (_enemy.currMoveDirection == 2) {
                choice = (UnityEngine.Random.value > 0.5f);
                if (choice == true) {
                    // Turn right up
                    return 6;
                }      
                else {
                    // Turn left up
                    return 4;
                }
            }

            // DOWN
            if (_enemy.currMoveDirection == 3) {
                choice = (UnityEngine.Random.value > 0.5f);
                if (choice == true) {
                    // Turn down left
                    return 5;
                }      
                else {
                    // Turn down right
                    return 7;
                }
            }

            // LEFT UP
            if (_enemy.currMoveDirection == 4) {
                choice = (UnityEngine.Random.value > 0.5f);
                if (choice == true) {
                    // Turn up
                    return 2;
                }      
                else {
                    // Turn left
                    return 1;
                }
            }

            // LEFT DOWN
            if (_enemy.currMoveDirection == 5) {
                choice = (UnityEngine.Random.value > 0.5f);
                if (choice == true) {
                    // Turn left
                    return 1;
                }      
                else {
                    // Turn down
                    return 3;
                }
            }

            // RIGHT UP
            if (_enemy.currMoveDirection == 6) {
                choice = (UnityEngine.Random.value > 0.5f);
                if (choice == true) {
                    // Turn right
                    return 0;
                }      
                else {
                    // Turn up
                    return 2;
                }
            }

            // RIGHT DOWN
            if (_enemy.currMoveDirection == 7) {
                choice = (UnityEngine.Random.value > 0.5f);
                if (choice == true) {
                    // Turn right
                    return 0;
                }      
                else {
                    // Turn down
                    return 3;
                }
            }
        }
        return 0;
        
    }
    
}