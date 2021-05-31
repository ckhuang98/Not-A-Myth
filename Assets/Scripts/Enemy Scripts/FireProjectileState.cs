using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class FireProjectileState : BaseState
{
    private Enemy _enemy;

    //private GameObject damage;
    //private GameObject healing;
    private GameObject projectile;
    //private GameObject otherProjectile;
    private GameObject warning;
    private bool instantiated = false;
    private bool gotAngle = false;
    private bool gotColor = false;
    private string projectileType = "nothing";
    private float pauseTimer = 1f;
    private float angle;
    private Transform target; 
    bool canHeal = false;

    public FireProjectileState(Enemy enemy) : base(enemy.gameObject) {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override Type Tick() {
        if (_enemy.healthAmount > 0){
            //Debug.Log("FIRING");
            _enemy.inAttackState = true;
            var delta_x = transform.position.x - target.position.x;
            var delta_y = transform.position.y - target.position.y;
            if (gotAngle == false) {
                angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
                if (angle < 0.0f) {
                    angle = angle + 360f;
                }
                gotAngle = true;
            }

            ThrowProjectile(angle);
            _enemy.enemyAnimator.SetFloat("ImpAttackHorizontal", _enemy.moveDirections[_enemy.currMoveDirection].x);
            _enemy.enemyAnimator.SetFloat("ImpAttackVertical", _enemy.moveDirections[_enemy.currMoveDirection].y);
            
            if (_enemy.goToWalk == true) {
                instantiated = false;
                gotColor = false;
                gotAngle = false;
                warning.transform.parent = null;
                GameObject.Destroy(warning.gameObject);
                //GameObject.Destroy(otherProjectile.gameObject);
                _enemy.inAttackState = false;
                _enemy.goToWalk = false;
                return typeof(MaintainDistanceState);
            }
        }
        return typeof(FireProjectileState);   
    }

    private void ThrowProjectile(float angle) {
        foreach (GameObject _hammerGiant in _enemy.hammerGiantList) {
            if (_hammerGiant != null) {
                var checkingDistance = Vector3.Distance(transform.position, _hammerGiant.transform.position);
                //var targetDistance = Vector3.Distance(transform.position, hammerTarget.position);
                if (checkingDistance < 20)
                {
                    canHeal = true;
                } 
            }
        }
        foreach (GameObject _swordGiant in _enemy.swordGiantList) {
            if (_swordGiant != null) {
                var checkingDistance = Vector3.Distance(transform.position, _swordGiant.transform.position);
                //var targetDistance = Vector3.Distance(transform.position, hammerTarget.position);
                if (checkingDistance < 20)
                {
                    canHeal = true;
                } 
            }
        }


        if (gotColor == false && canHeal == true) {
            gotColor = true;
            if (UnityEngine.Random.value > 0.5) {
                projectileType = "Healing Projectile";
            } else {
                projectileType = "Imp Damage Projectile";
            }
        } else if (gotColor == false && canHeal == false) {
            gotColor = true;
            projectileType = "Imp Damage Projectile";
        }
        if (instantiated == false) {
            //Debug.Log("Back here");
            instantiated = true;
            warning = GameObject.Instantiate(_enemy.projectileWarning, this.transform) as GameObject;
            //otherProjectile = GameObject.Instantiate(_enemy.impProjectile, this.transform) as GameObject;
            var ps = warning.GetComponent<ParticleSystem>();
            var main = ps.main;
            var em = ps.emission;
            if (projectileType == "Healing Projectile") {
                main.startColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            } else if (projectileType == "Imp Damage Projectile") {
                main.startColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            ps.Play();
            em.enabled = true;
        }
        if (_enemy.doInstantiate == true) {
            projectile = GameObject.Instantiate(_enemy.impProjectile) as GameObject;
            _enemy.doInstantiate = false;
            projectile.tag = projectileType; 
            projectile.transform.position = this.transform.position;
            /*
            if (projectileType == "Healing") {
                //healing = GameObject.Instantiate(_enemy.healingProjectile) as GameObject;
                //healing.transform.position = this.transform.position;
            } else if (projectileType == "Damage") {
                //damage = GameObject.Instantiate(_enemy.damageProjectile) as GameObject;
                //damage.transform.position = this.transform.position;
            }
            */
            // UP
            if (315 > angle && angle > 225) {
                _enemy.currMoveDirection = 0;
            } 
            // RIGHT
            if (225 > angle && angle > 135) {
                _enemy.currMoveDirection = 2;
            } 
            // DOWN
            if (135 > angle && angle > 45) {
                _enemy.currMoveDirection = 4;
            } 
            // LEFT
            if ((45 > angle && angle > 0) || (360 > angle && angle > 315)) {
                _enemy.currMoveDirection = 6;
            }
        }
    }
}

