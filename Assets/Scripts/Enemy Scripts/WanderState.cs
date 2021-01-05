using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WanderState : BaseState
{
    private Enemy _enemy;
    private float speed = 3f;
    private Vector2 decisionTime = new Vector2(1, 4);
    internal float decisionTimeCount = 0f;

    internal Vector3[] moveDirections = new Vector3[] { Vector3.right, Vector3.left,
    Vector3.up, Vector3.down };
    internal int currMoveDirection;

    public WanderState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
        decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);
        ChooseMoveDirection();
    }

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

    private void ChooseMoveDirection()
    {
        currMoveDirection = Mathf.FloorToInt(UnityEngine.Random.Range(0, moveDirections.Length));
    }

    
}
