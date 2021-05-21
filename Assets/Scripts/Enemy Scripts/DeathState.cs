using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using ShapeModule;
//using Pathfinding;

public class DeathState : BaseState
{
    private Enemy _enemy;
    private GameObject AoE;
    private GameObject AoEWarning;
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
    public DeathState(Enemy enemy) : base (enemy.gameObject)
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
        return typeof(DeathState);
    }

}