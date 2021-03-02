using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MaintainDistanceState : BaseState
{
    private Enemy _enemy;
    private Transform target;

    private float speed = 3f;
    private float shiftSpeed = 2f;
    private float stoppingDistance = 4f;
    private float retreatDistance = 2.5f;

    private float timeBtwShots = 2f;
    private float shiftTime = 4f;

    private bool functionGoing = false;

    public MaintainDistanceState(Enemy enemy) : base(enemy.gameObject) {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override Type Tick() {
        transform.position += _enemy.moveDirections[_enemy.currMoveDirection] * Time.deltaTime * shiftSpeed;
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        } else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance) {
            transform.position = this.transform.position;
        } else if (Vector2.Distance(transform.position, target.position) < retreatDistance) {
             transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        }

        var delta_x = transform.position.x - target.position.x;
        var delta_y = transform.position.y - target.position.y;
        float angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
        if (angle < 0.0f) {
            angle = angle + 360f;
        } 
        
        if (shiftTime >= 0)
        {
            shiftTime -= Time.deltaTime;
            
        } else
        {
            shiftTime = 4f;
            LocatePlayer(angle);
        }


        if (timeBtwShots <= 0) {
            timeBtwShots = 2f;
            return typeof(FireProjectileState);
        } else {
            timeBtwShots -= Time.deltaTime;
        }
        return typeof(MaintainDistanceState);
    }

     private void LocatePlayer(float angle) {
        // UP
        if (247.5 < angle && angle < 292.5) { 
            if (UnityEngine.Random.value > 0.5) {
                Debug.Log("Going right");
                _enemy.currMoveDirection = 2;
            } else {
                Debug.Log("Going left");
                _enemy.currMoveDirection = 6;
            }
        }
        // RIGHT & UP
        if (202.5 < angle && angle < 247.5) {
            if (UnityEngine.Random.value > 0.5) {
                Debug.Log("Going right down");
                _enemy.currMoveDirection = 3;
            } else {
                Debug.Log("Going left up");
                _enemy.currMoveDirection = 7;
            }
        }
        // RIGHT
        if (157.5 < angle && angle < 202.5) {
            if (UnityEngine.Random.value > 0.5) {
                Debug.Log("Going down");
                _enemy.currMoveDirection = 4;
            } else {
                Debug.Log("Going up");
                _enemy.currMoveDirection = 0;
            }
        }
        // DOWN RIGHT
        if (angle > 112.5 && angle < 157.5) {
            if (UnityEngine.Random.value > 0.5) {
                Debug.Log("Going left down");
                _enemy.currMoveDirection = 5;
            } else {
                Debug.Log("Going right up");
                _enemy.currMoveDirection = 1;
            }
        }
        // DOWN
        if (angle > 67.5 && angle < 112.5) {
            if (UnityEngine.Random.value > 0.5) {
                Debug.Log("Going left");
                _enemy.currMoveDirection = 6;
            } else {
                Debug.Log("Going right");
                _enemy.currMoveDirection = 2;
            } 
        }
        //DOWN LEFT
        if (angle > 22.5 && angle < 67.5) {
            if (UnityEngine.Random.value > 0.5) {
                Debug.Log("Going left up");
                _enemy.currMoveDirection = 7;
            } else {
                Debug.Log("Going right down");
                _enemy.currMoveDirection = 3;
            }
        }
        // LEFT
        if ((angle > 337.5 && angle < 360) || (angle > 0 && angle < 22.5)) {
            if (UnityEngine.Random.value > 0.5) {
                Debug.Log("Going up");
                _enemy.currMoveDirection = 0;
            } else {
                Debug.Log("Going down");
                _enemy.currMoveDirection = 4;
            }
        }
        // LEFT & UP
        if (292.5 < angle && angle < 337.5) {
            if (UnityEngine.Random.value > 0.5) {
                Debug.Log("Going right up");
                _enemy.currMoveDirection = 1;
            } else {
                Debug.Log("Going left down");
                _enemy.currMoveDirection = 5;
            }
        }
    }
}
