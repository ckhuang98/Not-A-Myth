using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChaseState : BaseState
{
    private Enemy _enemy;
    //This is the player
    private Transform target;
    public float speed = 3f;
    private bool hasCircled = false;
    private GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    /*
    Purpose: constructor recieves all needed values from enemy class and recieves
    the transform component from the player.
    Recieves: the enemy class from the enemy.cs file
    Returns: nothing
    */
    public ChaseState(Enemy enemy) : base (enemy.gameObject)
    {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    /*
    Purpose: Calls the MoveTowards function in order for the enemy to chase the
    player, If the enemy gets close enough to the player the attack state is started.
    Recieves: nothing
    Returns: the type of the current chase state consistently returned, until the 
    enemy gets close, then the type of the attack state is returned
    */
    public override Type Tick()
    { 
        //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime); 
        /*
        var dot = (target.position.x * transform.position.x) * (target.position.y + transform.position.y);
        var det = (target.position.x * transform.position.y) - (target.position.y * transform.position.x);
        float angle = Mathf.Atan2(Mathf.Sin(det), Mathf.Cos(dot)) * 180 / Mathf.PI;
        */
        var delta_x = transform.position.x - target.position.x;
        var delta_y = transform.position.y - target.position.y;
        float angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
        if (angle < 0.0f) {
            angle = angle + 360f;
        }
        
        Debug.Log(angle);

        /*
        var delta_x = transform.position.x - target.position.y;
        var delta_y = transform.position.y - target.position.y;
        float angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
        */

        /*
        dot = x1*x2 + y1*y2      # dot product
        det = x1*y2 - y1*x2      # determinant
        angle = atan2(det, dot)  # atan2(y, x) or atan2(sin, cos)
        */

        ChasePlayer(angle);

        foreach (GameObject __enemy in enemies) {
            if (__enemy != null) {
                float currentDistance = Vector3.Distance(transform.position, __enemy.transform.position);
                if (currentDistance < 2.0f)
                {
                Vector3 dist = transform.position - __enemy.transform.position;
                transform.position += dist * Time.deltaTime;
                } 
            }
        }
    /*
        if (Vector2.Distance(transform.position, target.position) <= 3 && hasCircled == false) {
            hasCircled = true;
            return typeof(CircleState);
        } else if (Vector2.Distance(transform.position, target.position) <= .5 && hasCircled == true) {
            hasCircled = false;
            return typeof(AttackState);
        }
    */
        return typeof(ChaseState);
    }

    private void ChasePlayer(float angle) {

        // DOWN
        if (angle <= 112.5f && angle >= 67.5) {
            //transform.position += Vector3.down * Time.deltaTime * speed;
        }
        
        else if (angle >= 112.5f && angle >= 157.5) {
            //transform.position += Vector3.down * Time.deltaTime * speed;
        }
    }
}

// Enemy coordinates: (e_x, e_y)
// Player coordinates: (p_x, p_y)
// Angle (in radians) between them:

    // float angle = atan2(enemy_y - player_y, enemy_x - player_y) * 180 / PI
