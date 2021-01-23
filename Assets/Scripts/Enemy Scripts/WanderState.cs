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

    internal Vector3 currMoveDirection;

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
        /*
        if (decisionTimeCount >= 0)
        {
            decisionTimeCount -= Time.deltaTime;
            
        } else
        {
            decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);

            ChooseMoveDirection();
        }
        */
        ChooseMoveDirection();

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
        //transform.position += Vector3.up * speed * Time.deltaTime;
        //currMoveDirection = Mathf.FloorToInt(UnityEngine.Random.Range(0, _enemy.moveDirections.Length));
        if (hasMoved == false) {
            var i = Mathf.FloorToInt(UnityEngine.Random.Range(0, _enemy.moveDirections.Length));
            currMoveDirection = _enemy.moveDirections[i];
            transform.position += Vector3.up * speed * Time.deltaTime;
            decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);
            hasMoved = true;
        }

        if (decisionTimeCount >= 0)
        {
            decisionTimeCount -= Time.deltaTime;
            
        }

        //int choice = Mathf.FloorToInt(UnityEngine.Random.Range(1, 2));
        decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);

        // DOWN
        
        if (currMoveDirection == Vector3.down && decisionTimeCount <= 0f) {
            var choice = Mathf.FloorToInt(UnityEngine.Random.Range(1, 2));
            if (choice == 1) {
                currMoveDirection = Vector3.Normalize(Vector3.left + Vector3.down);
                transform.position += Vector3.Normalize(Vector3.left + Vector3.down) * speed * Time.deltaTime;
            }      
            if (choice == 2) {
                currMoveDirection = Vector3.Normalize(Vector3.right + Vector3.down);
                transform.position += Vector3.Normalize(Vector3.right + Vector3.down) * speed * Time.deltaTime;
            }
        }

        // LEFT
        if (currMoveDirection == Vector3.left && decisionTimeCount <= 0f) {
            var choice = Mathf.FloorToInt(UnityEngine.Random.Range(1, 2));
            if (choice == 1) {      // left turn (down & left)
                currMoveDirection = Vector3.Normalize(Vector3.left + Vector3.down);
                transform.position += Vector3.Normalize(Vector3.left + Vector3.down) * speed * Time.deltaTime;
            }
            if (choice == 2) {      // right turn (up & left)
                currMoveDirection = Vector3.Normalize(Vector3.left + Vector3.up);
                transform.position += Vector3.Normalize(Vector3.left + Vector3.up) * speed * Time.deltaTime;
            }
        }

        // UP
        if (currMoveDirection == Vector3.up && decisionTimeCount <= 0f) {
            var choice = Mathf.FloorToInt(UnityEngine.Random.Range(1, 2));
            if (choice == 1) {      // left turn (up & left)
                currMoveDirection = Vector3.Normalize(Vector3.up + Vector3.left);
                transform.position += Vector3.Normalize(Vector3.up + Vector3.left) * speed * Time.deltaTime;
            }
            if (choice == 2) {      // right turn (up & right)
                currMoveDirection = Vector3.Normalize(Vector3.up + Vector3.right);
                transform.position += Vector3.Normalize(Vector3.up + Vector3.right) * speed * Time.deltaTime;
            }
        }

        // RIGHT
        if (currMoveDirection == Vector3.right && decisionTimeCount <= 0f) {
            var choice = Mathf.FloorToInt(UnityEngine.Random.Range(1, 2));
            if (choice == 1) {      // left turn (up & right)
                currMoveDirection = Vector3.Normalize(Vector3.up + Vector3.right);
                transform.position += Vector3.Normalize(Vector3.up + Vector3.right) * speed * Time.deltaTime;
            }
            if (choice == 2) {      // right turn (down & right)
                currMoveDirection = Vector3.Normalize(Vector3.down + Vector3.right);
                transform.position += Vector3.Normalize(Vector3.down + Vector3.right) * speed * Time.deltaTime;
            }
        }

        // RIGHT & UP
        if (currMoveDirection == Vector3.Normalize(Vector3.right + Vector3.up)) {
            var choice = Mathf.FloorToInt(UnityEngine.Random.Range(1, 2));
            if (decisionTimeCount <= 0f) {
                if (choice == 1) {      // left turn (up)
                    currMoveDirection = Vector3.up;
                    transform.position += Vector3.up * speed * Time.deltaTime;
                }
                if (choice == 2) {      // right turn (right)
                    currMoveDirection = Vector3.right;
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }
            }
        }

        // RIGHT & DOWN
        if (currMoveDirection == Vector3.Normalize(Vector3.right + Vector3.down)) {
            var choice = Mathf.FloorToInt(UnityEngine.Random.Range(1, 2));
            if (decisionTimeCount <= 0f) {
                if (choice == 1) {      // right down turn (down)
                    currMoveDirection = Vector3.down;
                    transform.position += Vector3.down * speed * Time.deltaTime;
                }
                if (choice == 2) {      // right down turn (right)
                    currMoveDirection = Vector3.right;
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }
            }
        }

        // LEFT & UP
        if (currMoveDirection == Vector3.Normalize(Vector3.up + Vector3.left)) {
            var choice = Mathf.FloorToInt(UnityEngine.Random.Range(1, 2));
            if (decisionTimeCount <= 0f) {
                if (choice == 1) {      // left turn (left)
                    currMoveDirection = Vector3.left;
                    transform.position += Vector3.left * speed * Time.deltaTime;
                }
                if (choice == 2) {      // right turn (up)
                    currMoveDirection = Vector3.up;
                    transform.position += Vector3.left * speed * Time.deltaTime;
                }    
            }
        }


        // LEFT & DOWN
        if (currMoveDirection == Vector3.Normalize(Vector3.left + Vector3.down)) {
            var choice = Mathf.FloorToInt(UnityEngine.Random.Range(1, 2));
            if (decisionTimeCount <= 0f) {
                if (choice == 1) {      // left down turn (down)
                    currMoveDirection = Vector3.down;
                    transform.position += Vector3.down * speed * Time.deltaTime;
                }
                if (choice == 2) {      // left down turn (left))
                    currMoveDirection = Vector3.left;
                    transform.position += Vector3.left * speed * Time.deltaTime;
                }
            }
        }
    }
}