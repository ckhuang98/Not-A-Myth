using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LocateHostState : BaseState
{
    private Enemy _enemy;
    private Transform parent;
    bool movingRight = true;
    bool movingLeft = false;
    private Transform target;
    private float angle;
    private float shootTimer = .5f;
    private GameObject projectile;

    //private GameObject[] fullList;
    public LocateHostState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override Type Tick() {
        if (_enemy.spiritParent != null) {
            if (_enemy.inBounds == true) {
                var delta_x = transform.position.x - target.position.x;
                var delta_y = transform.position.y - target.position.y;
                angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
                if (angle < 0.0f) {
                    angle = angle + 360f;
                }
                if (angle >= 90f && angle <= 270) {
                    FlipRight();
                } else {
                    FlipLeft();
                }

                if (shootTimer >= 0f) {
                    shootTimer -= Time.deltaTime;
                } else {
                    projectile = GameObject.Instantiate(_enemy.damageProjectile) as GameObject;
                    projectile.transform.position = new Vector3(transform.position.x, transform.position.y,
                        transform.position.z);
                    shootTimer = .5f;
                }
            }
        } else {
            return typeof(DeathState);
        }
        
        return typeof(LocateHostState);
    }

    private void FlipLeft() {
        if (movingLeft) {
            return;
        }
        transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        movingLeft = true;
        movingRight = false;
    }

    private void FlipRight() {
        if (movingRight) {
            return;
        }
        transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        movingRight = true;
        movingLeft = false;
    }
    /*
    private void NPCDetection() { 
        foreach (GameObject _hammerGiant in hammerGiants) {
            if (_hammerGiant != null) {
                var currentDistance = Vector3.Distance(transform.position, _hammerGiant.transform.position);
                if (currentDistance < badGuys["Hammer Giant"])
                {
                    badGuys["Hammer Giant"] = currentDistance;
                    closestHammer = _hammerGiant.transform;
                } 
            }
        }

        foreach (GameObject _fireImp in fireImps) {
            if (_fireImp != null) {
                var currentDistance = Vector3.Distance(transform.position, _fireImp.transform.position);
                if (currentDistance < badGuys["Fire Imp"])
                {
                    badGuys["Fire Imp"] = currentDistance;
                    closestImp = _fireImp.transform;
                } 
            }
        }

        foreach(GameObject _fireEel in fireEels) {
            if (_fireEel != null) {
                float currentDistance = Vector3.Distance(transform.position, _fireEel.transform.position);
                if (currentDistance < badGuys["Fire Eel"]) {
                    badGuys["Fire Eel"] = currentDistance;
                    closestImp = _fireEel.transform;
                }
            }
        }

        foreach(GameObject _swordGiant in swordGiants) {
            if (_swordGiant != null) {
                float currentDistance = Vector3.Distance(transform.position, _swordGiant.transform.position);
                if (currentDistance < badGuys["Sword Giant"]) {
                    badGuys["Sword Giant"] = currentDistance;
                    closestHammer = _swordGiant.transform;
                }
            }
        }
    }
    */
}
