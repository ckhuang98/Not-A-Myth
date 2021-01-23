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

    //An array carrying all 8 movement options for the enemy
    internal Vector3[] moveDirections = new Vector3[] { Vector3.right, Vector3.left,
    Vector3.up, Vector3.down, Vector3.Normalize(Vector3.up + Vector3.right), Vector3.Normalize(Vector3.up + Vector3.left),
    Vector3.Normalize(Vector3.down + Vector3.right), Vector3.Normalize(Vector3.down + Vector3.left) };
    internal int currMoveDirection;

    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    private float _rayDistance = 1.0f;
    private int layerMask = 1 << 9;
    private RaycastHit2D[] casts = new RaycastHit2D[8];

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
        IsPathBlocked();

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

    private bool IsPathBlocked()
    {
        for (int i = 0; i < moveDirections.Count(); i ++) {
            var rayColor = Color.green;
            Debug.DrawRay(transform.position, moveDirections[i] * _rayDistance, rayColor);
            casts[i] = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), 
                new Vector2(moveDirections[i].x, moveDirections[i].y), _rayDistance, layerMask);
        }
        for (int i = 0; i < moveDirections.Count(); i ++) {
            if (casts[i].collider != null) {
                var rayColor = Color.red;
                Debug.DrawRay(transform.position, moveDirections[i] * _rayDistance, rayColor);
                //return true;
            } 
        }
        
        return false;
    }
}
