using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using ShapeModule;
//using Pathfinding;

public class AttackState : BaseState
{
    private Enemy _enemy;
    private GameObject AoE;
    private GameObject AoEWarning;
    private float xAttack;
    private float yAttack;
    //private GameObject fireParticles;
    private bool gotAngle = false;
    //private float angle;
    //private Transform target;
    private float horizontal;
    private float vertical;
    bool inAOEWarning;
    //private ParticleSystem ps;

    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    /*
    Purpose: constructor recieves all needed values from enemy class.
    Recieves: the enemy class from the enemy.cs file.
    Returns: nothing.
    */
    public AttackState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy; 
        
    }

    /*
    Purpose: Spawns the area of attack of the enemy for a short amount 
    of time. The moment the animation is over, the area of effect disapears and the
    chase state is started.
    Recieves: nothing
    Returns: the type of the attack state itself consistently and returns
    the type of the chase state when the specified time is up.
    */
    public override Type Tick()
    {
        if (_enemy.healthAmount > 0) {
            _enemy.inAttackState = true;
            /*
            var delta_x = transform.position.x - target.position.x;
            var delta_y = transform.position.y - target.position.y;
            if (gotAngle == false) {
                angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
                if (angle < 0.0f) {
                    angle = angle + 360f;
                }
                gotAngle = true;
            }
            */
            //Debug.Log("Her");
            if (gotAngle == false) {
                gotAngle = true;
                if (_enemy.attackDir == "Bottom") {
                    xAttack = 0f;
                    yAttack = -1f;
                } else if (_enemy.attackDir == "Right") {
                    xAttack = 1f;
                    yAttack = 0f;
                } else if (_enemy.attackDir == "Top") {
                    xAttack = 0f;
                    yAttack = 1f;
                } else if (_enemy.attackDir == "Left") {
                    xAttack = -1f;
                    yAttack = 0f;
                }
                _enemy.attackDir = "Not Set";
            }
            
            _enemy.enemyAnimator.SetFloat("AttackHorizontal", xAttack);
            _enemy.enemyAnimator.SetFloat("AttackVertical", yAttack);
            //horizontal = _enemy.enemyAnimator.GetFloat("AttackHorizontal");
            //vertical = _enemy.enemyAnimator.GetFloat("AttackVertical");
            if (AoEWarning != null) {
                inAOEWarning = AoEWarning.GetComponent<AOEWarning>().getWarning();
            } else {
                inAOEWarning = false;
            }
            InstantiateWarning();
            InstantiateAoE();
            //Debug.Log("Horizontal: " + horizontal.ToString());
            //Debug.Log("Vertical: " + vertical.ToString());
            
            if (_enemy.goToWalk == true) {
                _enemy.goToWalk = false;
                gotAngle = false;
                _enemy.inAttackState = false;
                return typeof(ChaseState);
            }
        }
        return typeof(AttackState);
    }

    /*
    Purpose: Instantiates the area of effect, the placement is specified by the players
    angle in four different directions.
    Recieves: a float called "angle" which contains the angle the player is at relative to
    the player.
    Returns: nothng
    */
    private void InstantiateAoE() {
        if (_enemy.doInstantiate == true) {
            AoE = GameObject.Instantiate(_enemy.AOE) as GameObject;
            //fireParticles = GameObject.Instantiate(_enemy.fireParticle) as GameObject;
            //fireParticles = GameObject.GetComponent
            //ps = fireParticles.GetComponent<ParticleSystem>();
            var hammerDown = AoE.GetComponent<AreaofEffectTime>().CanHit();
            var target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _enemy.doInstantiate = false; 
            // UP
            if (xAttack == 0f && yAttack == 1f) {
                AoE.transform.position = 
                new Vector3(this.transform.position.x + 0.69f, this.transform.position.y + 2.47f, this.transform.position.z);
                AoE.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
                if (hammerDown == true && inAOEWarning == true) {
                    target.StartCoroutine(target.HammerKnockBack(.2f, 50f, this.transform));
                }
                //fireParticles.transform.position = 
                //new Vector3(this.transform.position.x, this.transform.position.y + 1.5f, this.transform.position.z);
            } 
            // RIGHT
            if (xAttack == 1f && yAttack == 0f) {
                AoE.transform.position = 
                new Vector3(this.transform.position.x + 5.96f, this.transform.position.y - 2.06f, this.transform.position.z);
                AoE.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                if (hammerDown == true && inAOEWarning == true) {
                    target.StartCoroutine(target.HammerKnockBack(.2f, 50f, this.transform));
                }
                //fireParticles.transform.position = 
                //new Vector3(this.transform.position.x + 2, this.transform.position.y - 0.5f, this.transform.position.z);
            } 
            // DOWN
            if (xAttack == 0f && yAttack == -1f) {
                AoE.transform.position = 
                new Vector3(this.transform.position.x - 0.43f, this.transform.position.y - 6.49f, this.transform.position.z);
                if (hammerDown == true && inAOEWarning == true) {
                    target.StartCoroutine(target.HammerKnockBack(.2f, 50f, this.transform));
                }
                //fireParticles.transform.position = 
                //new Vector3(this.transform.position.x - 0.229f, this.transform.position.y - 1.59f, this.transform.position.z);
            } 
            // LEFT
            if (xAttack == -1f && yAttack == 0f) {
                AoE.transform.position = 
                new Vector3(this.transform.position.x - 5.77f, this.transform.position.y - 2.09f, this.transform.position.z);
                AoE.transform.localRotation = Quaternion.Euler(0f, 0f, 270f);
                if (hammerDown == true && inAOEWarning == true) {
                    target.StartCoroutine(target.HammerKnockBack(.2f, 50f, this.transform));
                }
                //fireParticles.transform.position = 
                //new Vector3(this.transform.position.x - 2, this.transform.position.y - 0.5f, this.transform.position.z);
            }
        }
    }

    private void InstantiateWarning() {
        if (_enemy.instantiateWarning == true) {
            AoEWarning = GameObject.Instantiate(_enemy.AOEWarning) as GameObject;
            _enemy.instantiateWarning = false; 
            // UP
            if (xAttack == 0f && yAttack == 1f) {
                //Debug.Log("Up");
                AoEWarning.transform.position = 
                new Vector3(this.transform.position.x + 0.81f, this.transform.position.y + 1.84f, this.transform.position.z);
                AoEWarning.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            } 
            // RIGHT
            if (xAttack == 1f && yAttack == 0f) {
                //Debug.Log("Right");
                AoEWarning.transform.position = 
                new Vector3(this.transform.position.x + 5.21f, this.transform.position.y - 1.75f, this.transform.position.z);
                AoEWarning.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            }
            // DOWN
            if (xAttack == 0f && yAttack == -1f) {
                //Debug.Log("Down");
                AoEWarning.transform.position = 
                new Vector3(this.transform.position.x - 0.38f, this.transform.position.y - 5.62f, this.transform.position.z);
            } 
            // LEFT
            if (xAttack == -1f && yAttack == 0f) {
                //Debug.Log("Left");
                AoEWarning.transform.position = 
                new Vector3(this.transform.position.x - 5.15f, this.transform.position.y - 1.87f, this.transform.position.z);
                AoEWarning.transform.localRotation = Quaternion.Euler(0f, 0f, 270f);
            }
        }
    }
}