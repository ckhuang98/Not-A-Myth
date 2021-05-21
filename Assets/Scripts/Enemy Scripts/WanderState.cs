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
    private bool safeChoice;
    private float fireTrailTimer = 1f;
    private GameObject fireBall;

    private GameObject[] hammerGiants = GameObject.FindGameObjectsWithTag("Hammer Giant");
    private GameObject[] fireImps = GameObject.FindGameObjectsWithTag("Fire Imp");
    private GameObject[] fireEels = GameObject.FindGameObjectsWithTag("Fire Eel");
    private GameObject[] swordGiants = GameObject.FindGameObjectsWithTag("Sword Giant");

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
        if (_enemy.healthAmount > 0){
            if (_enemy.beenHit == false && _enemy.tag == "Hammer Giant") {
                _enemy.enemyAnimator.SetFloat("Horizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
                _enemy.enemyAnimator.SetFloat("Vertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
                speed = 1f;
            } else if (_enemy.beenHit == true && _enemy.tag == "Hammer Giant") {
                _enemy.enemyAnimator.SetFloat("HammerHitHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
                _enemy.enemyAnimator.SetFloat("HammerHitVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
                speed = .25f;
            }
            //Debug.Log("Wanderin'");

            if (_enemy.tag == "Fire Eel" && _enemy.beenHit == false) {
                _enemy.enemyAnimator.SetFloat("EelWalkHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
                _enemy.enemyAnimator.SetFloat("EelWalkVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
                speed = 1;
            } else if (_enemy.beenHit == true && _enemy.tag == "Fire Eel") {
                _enemy.enemyAnimator.SetFloat("EelHitHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
                _enemy.enemyAnimator.SetFloat("EelHitVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
                speed = .25f;
            } 

            if (_enemy.tag == "Fire Imp" && _enemy.beenHit == false) {
                _enemy.enemyAnimator.SetFloat("ImpIdleHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
                _enemy.enemyAnimator.SetFloat("ImpIdleVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
                speed = 1;
            } else if (_enemy.beenHit == true && _enemy.tag == "Fire Imp") {
                _enemy.enemyAnimator.SetFloat("ImpHitHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
                _enemy.enemyAnimator.SetFloat("ImpHitVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
                speed = .25f;
            }  

            if(_enemy.tag == "Sword Giant") {
                _enemy.enemyAnimator.SetFloat("SwordWalkHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
                _enemy.enemyAnimator.SetFloat("SwordWalkVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
            }

            transform.position += _enemy.moveDirections[_enemy.currMoveDirection] * Time.deltaTime * speed;

            if (decisionTimeCount >= 0)
            {
                decisionTimeCount -= Time.deltaTime;
                
            } else
            {
                if (hasMoved == false) {
                    hasMoved = true;
                }
                decisionTimeCount = UnityEngine.Random.Range(decisionTime.x, decisionTime.y);
                ChooseMoveDirection();
            }
            MoveDirection();
            NPCDetection();
            WallDetection();
            FailSafeDirection();
            //PlaceFire();

            if (_enemy.inBounds == true && (_enemy.tag == "Hammer Giant" || _enemy.tag == "Sword Giant")) {
                _enemy.resetWeightsToZero();
                return typeof(ChaseState);
            }
            if (_enemy.inBounds == true && _enemy.tag == "Fire Eel") {
                _enemy.resetWeightsToZero();
                return typeof(EelMaintainDistanceState);
            }
            if (_enemy.inBounds == true && _enemy.tag == "Fire Imp") {
                _enemy.resetWeightsToZero();
                return typeof(MaintainDistanceState);
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
        //Random Movement at first call
        if (hasMoved == false) {
            int index = Mathf.FloorToInt(UnityEngine.Random.Range(0, _enemy.moveDirections.Length));
            _enemy.weightList[index] = 1;
        } else {

            // UP
            if (_enemy.weightList[0] == 1) {
                choice = (UnityEngine.Random.value > 0.5f);
                _enemy.weightList[0] = 0;
                if (choice == true) {
                    // Turn right up
                    _enemy.weightList[1] = 1;
                }      
                else {
                    // Turn left up
                    _enemy.weightList[7] = 1;
                }
            }

            // RIGHT UP
            if (_enemy.weightList[1] == 1) {
                choice = (UnityEngine.Random.value > 0.5f);
                _enemy.weightList[1] = 0;
                if (choice == true) {
                    // Turn up
                    _enemy.weightList[0] = 1;
                }      
                else {
                    // Turn right
                    _enemy.weightList[2] = 1;
                }
            }

            // RIGHT
            if (_enemy.weightList[2] == 1) {
                choice = (UnityEngine.Random.value > 0.5f);
                _enemy.weightList[2] = 0;
                if (choice == true) {
                    // Turn right up
                    _enemy.weightList[1] = 1;
                }      
                else {
                    // Turn right down
                    _enemy.weightList[3] = 1;
                }
            }

            // RIGHT DOWN
            if (_enemy.weightList[3] == 1) {
                choice = (UnityEngine.Random.value > 0.5f);
                _enemy.weightList[3] = 0;
                if (choice == true) {
                    // Turn right
                    _enemy.weightList[2] = 1;
                }      
                else {
                    // Turn down
                    _enemy.weightList[4] = 1;
                }
            }

            // DOWN
            if (_enemy.weightList[4] == 1) {
                choice = (UnityEngine.Random.value > 0.5f);
                _enemy.weightList[4] = 0;
                if (choice == true) {
                    // Turn down right
                    _enemy.weightList[3] = 1;
                }      
                else {
                    // Turn down left
                    _enemy.weightList[5] = 1;
                }
            }

            // LEFT DOWN
            if (_enemy.weightList[5] == 1) {
                choice = (UnityEngine.Random.value > 0.5f);
                _enemy.weightList[5] = 0;
                if (choice == true) {
                    // Turn left
                    _enemy.weightList[6] = 1;
                }      
                else {
                    // Turn down
                    _enemy.weightList[4] = 1;
                }
            }

            // LEFT
            if (_enemy.weightList[6] == 1) {
                choice = (UnityEngine.Random.value > 0.5f);
                _enemy.weightList[6] = 0;
                if (choice == true) {
                    // Turn left up
                    _enemy.weightList[7] = 1;
                }      
                else {
                    // Turn left down
                    _enemy.weightList[5] = 1;
                }
            }

            // LEFT UP
            if (_enemy.weightList[7] == 1) {
                choice = (UnityEngine.Random.value > 0.5f);
                _enemy.weightList[7] = 0;
                if (choice == true) {
                    // Turn up
                    _enemy.weightList[0] = 1;
                }      
                else {
                    // Turn left
                    _enemy.weightList[6] = 1;
                }
            }
            
        }
    /*
        for (int i = 0; i < _enemy.moveDirections.Count(); i++) {
            if (_enemy.weightList[i] == 1) {
                _enemy.currMoveDirection = i;
            }
        }
        */
        return;
    }

    /*
    Purpose: If a wall is detected within 1.5 pixels away, the enemy will make a
    180 and walk away from wall.
    Recieves: nothing
    Returns: nothing
    */
    private void WallDetection() {
        for (int i = 0; i < _enemy.moveDirections.Count(); i ++) {
            if (_enemy.castList[i].collider != null && (_enemy.castList[i].collider.name == "Walls" 
            || _enemy.castList[i].collider.name == "Passable")) {
                //Debug.Log(_enemy.castList[i].collider.name);                 
                if (_enemy.castList[i].distance <= 1.25) {          
                    var about_face = i;
                    if (about_face >= 4) {
                        about_face -= 4;
                    } else if (about_face < 4) {
                        about_face += 4;
                    }
                    _enemy.weightList[_enemy.currMoveDirection] = 0;
                    _enemy.weightList[about_face] = 1;
                    _enemy.currMoveDirection = about_face;
                }
            }
        }
    }
    
    private void NPCDetection() {
        for (int i = 0; i < _enemy.moveDirections.Count(); i++) {
            if (_enemy.castList[i].collider != null && (_enemy.castList[i].collider.name == "Hammer Giant (Clone)" ||
            _enemy.castList[i].collider.name == "Sword Giant(Clone)" || _enemy.castList[i].collider.name == "Fire Eel(Clone)" ||
            _enemy.castList[i].collider.name == "Imp(Clone)")) {
                if (_enemy.castList[i].distance <= 2) {  
                    _enemy.weightList[i] = -1;
                } else {
                    _enemy.weightList[i] = 0;
                }
            }
        }
    }
    private void FailSafeDirection() {
        if (_enemy.weightList[_enemy.currMoveDirection] == -1) {
            //If the array is at the end or beginning the choice will be made for them
            if (_enemy.currMoveDirection == 7) {
                safeChoice = false;
            } else if (_enemy.currMoveDirection == 0) {
                safeChoice = true;
            } else {
                safeChoice = (UnityEngine.Random.value > 0.5f);
            }
            if (safeChoice == true) {
                for (int i = _enemy.currMoveDirection; i < _enemy.moveDirections.Count(); i++) {
                    if (_enemy.weightList[i] == 0) {
                        _enemy.weightList[_enemy.currMoveDirection] = 0;
                        _enemy.weightList[i] = 1;
                        break;
                    }
                }
            } else if (safeChoice == false) {
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
            }
        }
    }


    /*
    Purpose: If another enemy if detected they will slowly avoid each other. Different
    distances are based on the enemies different sizes.
    Recieves: nothing.
    Returns: nothing
    
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

        foreach(GameObject _swordGiant in swordGiants) {
            if (_swordGiant != null) {
                float currentDistance = Vector3.Distance(transform.position, _swordGiant.transform.position);
                if (currentDistance < 3.0f) {
                    Vector3 dist = transform.position - _swordGiant.transform.position;
                    transform.position += dist *Time.deltaTime;
                }
            }
        }
    }
    */

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
                fireTrailTimer = 1f;
            }
        }
    }
    */
}