using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameMaster.instance.getGameOver()) {
        //Vector3 targetDirection = Input.mousePosition - this.transform.position;

        //float singleStep = 5 * Time.deltaTime;

        //var newDirection = Vector3.RotateTowards(this.transform.position,  targetDirection, singleStep, 0f);
        //this.transform.rotation = Quaternion.LookRotation(newDirection);    
        //transform.forward = targetDirection;

        var mousePos = Input.mousePosition;
        mousePos.z = 3;
        var objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

}
