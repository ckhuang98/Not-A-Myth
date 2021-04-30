using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySlash : MonoBehaviour
{
    private float timer = 5f;
    public Animator slashAnimator;
    private GameObject SG;
    private Enemy SGS;

    /*
    private float angle;
    private Transform target;
    internal Vector3[] moveDirections = new Vector3[] { Vector3.up, Vector3.Normalize(Vector3.right + Vector3.up), 
        Vector3.right, Vector3.Normalize(Vector3.right + Vector3.down), Vector3.down,
        Vector3.Normalize(Vector3.left + Vector3.down), Vector3.left, Vector3.Normalize(Vector3.left + Vector3.up) };
    */
    // Start is called before the first frame update
    void Start()
    {
        SGS = transform.parent.GetComponent<Enemy>();
        /*
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        var delta_x = transform.position.x - target.position.x;
        var delta_y = transform.position.y - target.position.y;
        angle = Mathf.Atan2(delta_y, delta_x) * 180 / Mathf.PI;
        if (angle < 0.0f) {
            angle = angle + 360f;
        }  
        */
       
    }

    // Update is called once per frame
    void Update()
    {
        slashAnimator.SetFloat("SlashAttackVertical", SGS.moveDirections[SGS.currMoveDirection].y);
        slashAnimator.SetFloat("SlashAttackHorizontal", SGS.moveDirections[SGS.currMoveDirection].x);
        //ChooseDirection(angle);
        
    }

    public void RemoveSlash() {
        Destroy(this.gameObject);
    }
    /*
    private void ChooseDirection(float angle) {
        // UP
        if (315 > angle && angle > 225) {
            slashAnimator.SetFloat("SlashAttackVertical", moveDirections[0].y);
        } 
        // RIGHT
        if (225 > angle && angle > 135) {
            slashAnimator.SetFloat("SlashAttackHorizontal", moveDirections[2].x);
        } 
        // DOWN
        if (135 > angle && angle > 45) {
            slashAnimator.SetFloat("SlashAttackVertical", moveDirections[4].y);
        } 
        // LEFT
        if ((45 > angle && angle > 0) || (360 > angle && angle > 315)) {
            slashAnimator.SetFloat("SlashAttackHorizontal", moveDirections[6].x);
        }
    }
    */
}
