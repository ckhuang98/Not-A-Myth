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
    private float lungeAttackTimer = 3f;
    private bool movingBack = false;

    public EelMaintainDistanceState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override Type Tick() {
        transform.position += _enemy.moveDirections[_enemy.currMoveDirection] * speed * Time.deltaTime;
        if (_enemy.tag == "Fire Eel") {
            _enemy.enemyAnimator.SetFloat("EelWalkHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
            _enemy.enemyAnimator.SetFloat("EelWalkVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
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
        PlaceFire();

        if (lungeAttackTimer > 0) {
            lungeAttackTimer -= Time.deltaTime;
        } else {
            if (movingBack == true) {
                lungeAttackTimer = 3f;
                _enemy.enemyAnimator.SetTrigger("FireEelAttacking");
                return typeof(LungeAttackState);
            }
        }

        return typeof(EelMaintainDistanceState);
    }

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

    private void findNextDirection() {
        Debug.Log("Stuck Here");
        for (int i = 0; i < _enemy.moveDirections.Count(); i ++) {
            if (_enemy.weightList[i] == 0) {
                _enemy.weightList[i] = 1;
                return;
            }
        }
    }

    private void MaintainDistance() {
        if (Vector2.Distance(transform.position, target.position) <= 5) {
            movingBack = true;
            Debug.Log("Trying to move back");
            var about_face = _enemy.currMoveDirection;
            if (about_face >= 4) {
                about_face -= 4;
            } else if (about_face < 4) {
                about_face += 4;
            }
            _enemy.weightList[about_face] = 1;
            _enemy.currMoveDirection = about_face;
        } else {
            movingBack = false;
        }
    }

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
            

    private void MoveDirection() {
        for (int i = 0; i < _enemy.moveDirections.Count(); i++) {
            //Debug.Log("in da mf loop");
            if (_enemy.weightList[i] == 1) {
                _enemy.currMoveDirection = i;
                //FailSafeDirection();
                //Debug.Log("weight of " + i + " is == 1!");
            }
        }
    }

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
}
