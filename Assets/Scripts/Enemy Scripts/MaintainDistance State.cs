using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MaintainDistanceState : BaseState
{
    private Enemy _enemy;
    private Transform target;
    private GameObject[] hammerGiants = GameObject.FindGameObjectsWithTag("Hammer Giant");
    private GameObject[] fireImps = GameObject.FindGameObjectsWithTag("Fire Imp");
    private GameObject[] fireEels = GameObject.FindGameObjectsWithTag("Fire Eel");
    private float speed = 3f;
    private float shiftSpeed = 2f;
    private float stoppingDistance = 4f;
    private float retreatDistance = 2.5f;

    private float timeBtwShots = 2f;
    private float shiftTime = 4f;

    public MaintainDistanceState(Enemy enemy) : base(enemy.gameObject) {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

     /*
    Purpose: Calls all functions in order to successfully maintain distance of the player
    Recieves: nothing
    Returns: the type of the current chase state consistently returned, until the 
    enemy gets close, then the type of the fire projectile state is returned
    */
    public override Type Tick() {
        //Uses movement directions to shift to the left or right of the player.
        transform.position += _enemy.moveDirections[_enemy.currMoveDirection] * Time.deltaTime * shiftSpeed;
        //code to maintain distance
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
        
    //Time for option shift side to side
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

    /*
    Purpose: Uses the angle of the player relative to the enemy, and picks the proper
    shift direction accordingly. This is left or right relative to the player
    Recieves: A float "angle" which is the angle relative to the player and the enemy.
    Returns: nothing
    */
     private void LocatePlayer(float angle) {
        // UP
        if (247.5 < angle && angle < 292.5) { 
            if (UnityEngine.Random.value > 0.5) {
                _enemy.currMoveDirection = 2;
            } else {
                _enemy.currMoveDirection = 6;
            }
        }
        // RIGHT & UP
        if (202.5 < angle && angle < 247.5) {
            if (UnityEngine.Random.value > 0.5) {
                _enemy.currMoveDirection = 3;
            } else {
                _enemy.currMoveDirection = 7;
            }
        }
        // RIGHT
        if (157.5 < angle && angle < 202.5) {
            if (UnityEngine.Random.value > 0.5) {
                _enemy.currMoveDirection = 4;
            } else {
                _enemy.currMoveDirection = 0;
            }
        }
        // DOWN RIGHT
        if (angle > 112.5 && angle < 157.5) {
            if (UnityEngine.Random.value > 0.5) {
                _enemy.currMoveDirection = 5;
            } else {
                _enemy.currMoveDirection = 1;
            }
        }
        // DOWN
        if (angle > 67.5 && angle < 112.5) {
            if (UnityEngine.Random.value > 0.5) {
                _enemy.currMoveDirection = 6;
            } else {
                _enemy.currMoveDirection = 2;
            } 
        }
        //DOWN LEFT
        if (angle > 22.5 && angle < 67.5) {
            if (UnityEngine.Random.value > 0.5) {
                _enemy.currMoveDirection = 7;
            } else {
                _enemy.currMoveDirection = 3;
            }
        }
        // LEFT
        if ((angle > 337.5 && angle < 360) || (angle > 0 && angle < 22.5)) {
            if (UnityEngine.Random.value > 0.5) {
                _enemy.currMoveDirection = 0;
            } else {
                _enemy.currMoveDirection = 4;
            }
        }
        // LEFT & UP
        if (292.5 < angle && angle < 337.5) {
            if (UnityEngine.Random.value > 0.5) {
                _enemy.currMoveDirection = 1;
            } else {
                _enemy.currMoveDirection = 5;
            }
        }
    }

    /*
    Purpose: If another enemy if detected they will slowly avoid each other. Different
    distances are based on the enemies different sizes.
    Recieves: nothing.
    Returns: nothing
    */
    private void NPCDetection() {
        foreach (GameObject _hammerGiant in hammerGiants) {
            if (_hammerGiant != null) {
                float currentDistance = Vector3.Distance(transform.position, _hammerGiant.transform.position);
                if (currentDistance < 2.0f)
                {
                Vector3 dist = transform.position - _hammerGiant.transform.position;
                transform.position += dist * Time.deltaTime;
                } 
            }
        }

        foreach (GameObject _fireImp in fireImps) {
            if (_fireImp != null) {
                float currentDistance = Vector3.Distance(transform.position, _fireImp.transform.position);
                if (currentDistance < 2.0f)
                {
                Vector3 dist = transform.position - _fireImp.transform.position;
                transform.position += dist * Time.deltaTime;
                } 
            }
        }

        foreach(GameObject _fireEel in fireEels) {
            if (_fireEel != null) {
                float currentDistance = Vector3.Distance(transform.position, _fireEel.transform.position);
                if (currentDistance < 3.0f) {
                    Vector3 dist = transform.position - _fireEel.transform.position;
                    transform.position += dist *Time.deltaTime;
                }
            }
        }
    }
}
