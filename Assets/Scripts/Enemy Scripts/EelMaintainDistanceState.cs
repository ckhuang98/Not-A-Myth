using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EelMaintainDistanceState : BaseState
{
    Enemy _enemy;
    private Transform target;
    private float speed = 2f;
    private float fireTrailTimer = .5f;
    private GameObject[] hammerGiants = GameObject.FindGameObjectsWithTag("Hammer Giant");
    private GameObject[] fireImps = GameObject.FindGameObjectsWithTag("Fire Imp");
    private GameObject[] fireEels = GameObject.FindGameObjectsWithTag("Fire Eel");
    private GameObject fireBall;
    private GameObject walls = GameObject.Find("Walls");
    private bool choice;
    private bool stop = false;
    private bool attackDistance = false;
    private float lungeAttackTimer = 3f;
    private bool movingBack = false;

    public EelMaintainDistanceState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    /*
    Purpose: Calls all functions in order to successfully maintain distance of the player
    Recieves: nothing
    Returns: the type of the current chase state consistently returned, until the 
    enemy gets close, then the type of the attack state is returned
    */
    public override Type Tick() {
        if (_enemy.healthAmount > 0){
            //Debug.Log("I'm here");
            if (stop == false) {
                transform.position += _enemy.moveDirections[_enemy.currMoveDirection] * speed * Time.deltaTime;
            }
            
            if (_enemy.tag == "Fire Eel" && _enemy.beenHit == false) {
                _enemy.enemyAnimator.SetFloat("EelWalkHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
                _enemy.enemyAnimator.SetFloat("EelWalkVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
                speed = 1;
            } else if (_enemy.beenHit == true && _enemy.tag == "Fire Eel") {
                _enemy.enemyAnimator.SetFloat("EelHitHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
                _enemy.enemyAnimator.SetFloat("EelHitVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
                speed = .25f;
            } 

            var delta_x = transform.position.x - target.position.x;
            var delta_y = transform.position.y - target.position.y;
            float angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
            if (angle < 0.0f) {
                angle = angle + 360f;
            }
            if (movingBack == false) {
                LocatePlayer(angle); 
                FailSafeDirection();
            }
            WallDetection();  
            MaintainDistance();
            MoveDirection();   
            NPCDetection();
            FailSafeDirection();
            //PlaceFire();

            if (lungeAttackTimer > 0) {
                lungeAttackTimer -= Time.deltaTime;
            } else {
                if (attackDistance == true && _enemy.beenHit == false) {
                    lungeAttackTimer = 3f;
                    _enemy.enemyAnimator.SetTrigger("FireEelAttacking");
                    return typeof(LungeAttackState);
                }
            }
        }

        return typeof(EelMaintainDistanceState);
    }

    /*
    Purpose: Uses the angle of the player relative to the enemy, and picks the proper
    move direction accordingly.
    Recieves: A float "angle" which is the angle relative to the player and the enemy.
    Returns: nothing
    */
    private void LocatePlayer(float angle) {
        // transform.position += _enemy.moveDirections[i] * speed * Time.deltaTime;

        // UP
        if (247.5 < angle && angle < 292.5) {  
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[0] = 1;
 
        }
        // RIGHT & UP
        if (202.5 < angle && angle < 247.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[1] = 1;
        }
        // RIGHT
        if (157.5 < angle && angle < 202.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[2] = 1;
    
        }
        // DOWN RIGHT
        if (angle > 112.5 && angle < 157.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[3] = 1;
  
        }
        // DOWN
        if (angle > 67.5 && angle < 112.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
                _enemy.weightList[4] = 1; 
        }
        //DOWN LEFT
        if (angle > 22.5 && angle < 67.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
           _enemy.weightList[5] = 1;
        }
        // LEFT
        if ((angle > 337.5 && angle < 360) || (angle > 0 && angle < 22.5)) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[6] = 1;
        }
        // LEFT & UP
        if (292.5 < angle && angle < 337.5) {
            _enemy.weightList[_enemy.currMoveDirection] = 0;
            _enemy.weightList[7] = 1;
        }
    }
    
    /*
    Purpose: If a wall is detected within 1.5 pixels away, the enemy will make a
    180 and walk away from wall.
    Recieves: nothing
    Returns: nothing
    */
    private void WallDetection() {
        // Adjust weight list: -1 for wall, 0 for non-wall
        for (int i = 0; i < _enemy.moveDirections.Count(); i ++) {
            //_enemy.weightList[i] = 0;
            if (_enemy.castList[i].collider != null) {
                if (_enemy.castList[i].distance <= 1.5) {  
                    _enemy.weightList[i] = -1;
                } else {
                    _enemy.weightList[i] = 0;
                }
            }
        }
    }
    /*
    private void findNextDirection() {
        for (int i = 0; i < _enemy.moveDirections.Count(); i ++) {
            if (_enemy.weightList[i] == 0) {
                _enemy.weightList[i] = 1;
                return;
            }
        }
    }
    */

    /*
    Purpose: makes sure the enemy is in between the specified distances  of the player. If it
    gets too close the eel will do a 180, if its too far it will chase, otherwise it stops.
    Recieves: nothing
    Returns: nothing
    */
    private void MaintainDistance() {
        if (Vector2.Distance(transform.position, target.position) <= 2.5) {
            attackDistance = true;
            movingBack = true;
            stop = false;
            var about_face = _enemy.currMoveDirection;
            if (about_face >= 4) {
                about_face -= 4;
            } else if (about_face < 4) {
                about_face += 4;
            }
            _enemy.weightList[about_face] = 1;
            _enemy.currMoveDirection = about_face;
        } else if (Vector2.Distance(transform.position, target.position) >= 3.75) {
            stop = false;
            movingBack = false;
            attackDistance = false;
        } else {
            attackDistance = true;
            stop = true;
            transform.position = this.transform.position;
        }
    }

    /*
    Purpose: If the current move direction finds an obstacle and becomes a weight of
    -1. FialSafeDirection finds the next best direction to take.
    Recieves: nothing
    Returns: nothing
    */
    private void FailSafeDirection() {
        if (_enemy.weightList[_enemy.currMoveDirection] == -1) {
            if (_enemy.currMoveDirection == 7) {
                choice = false;
            } else if (_enemy.currMoveDirection == 0) {
                choice = true;
            } else {
                choice = (UnityEngine.Random.value > 0.5f);
            }
            if (choice == true) {
                for (int i = _enemy.currMoveDirection; i < _enemy.moveDirections.Count(); i++) {
                    if (_enemy.weightList[i] == 0) {
                        _enemy.weightList[_enemy.currMoveDirection] = 0;
                        _enemy.weightList[i] = 1;
                        break;
                    }
                }
            } else if (choice == false) {
                for (int i = _enemy.currMoveDirection; i >= 0; i--) {
                    if (_enemy.weightList[i] == 0) {
                        _enemy.weightList[_enemy.currMoveDirection] = 0;
                        _enemy.weightList[i] = 1;
                        break;
                    }
                }
            }
        }
    }
            
    /*
    Purpose: sets the current move direction to the direction with a weight of 1
    Recieves: nothing
    Returns: nothign
    */
    private void MoveDirection() {
        for (int i = 0; i < _enemy.moveDirections.Count(); i++) {
            if (_enemy.weightList[i] == 1) {
                _enemy.currMoveDirection = i;
                //FailSafeDirection();
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

    /*
    Purpose: Drops a fireball every half second
    Recieves: nothing
    Returns: nothing
    */
    /*
    private void PlaceFire() {
        if (_enemy.tag == "Fire Eel") {
            if (fireTrailTimer > 0) {
                fireTrailTimer -= Time.deltaTime;
            } else {
                fireBall = GameObject.Instantiate(_enemy.fireTrail) as GameObject;
                fireBall.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
                fireTrailTimer = .5f;
            }
        }
    }
    */
}
