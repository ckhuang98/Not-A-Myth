using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HammerState : BaseState
{
    private Boss _boss;
    
    private GameObject FCA;
    private float timer  = 5.0f;
    private bool isCreated = false;

    private Vector3 conePos;

    public HammerState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
        conePos = _boss.fireCone.transform.position;
        if(_boss.targetLastPos == "Left"){
            
        } else if(_boss.targetLastPos == "Right"){

        }
    }
    
    public override Type Tick()
    {
        _boss.attacking = true;
        _boss.coneAttack = true;
        if (isCreated == false) {
            FCA = GameObject.Instantiate(_boss.fireConeArea) as GameObject;

            // if(_boss.targetLastPos == "Center"){
            //     FCA.transform.position = new Vector3(0, -1, 1);
            //     conePos.x = 0;
            //     _boss.fireCone.transform.position = conePos;
            // } else if(_boss.targetLastPos == "Left"){
            //     FCA.transform.position = new Vector3(-3, -1, 1);
            //     conePos.x = -3;
            //     _boss.fireCone.transform.position = conePos;
            // } else if(_boss.targetLastPos == "Right"){
            //     FCA.transform.position = new Vector3(3, -1, 1);
            //     conePos.x = 3;
            //     _boss.fireCone.transform.position = conePos;
            // }

            FCA.transform.position = new Vector3(0, -1, 1);
            conePos.x = transform.position.x;
            _boss.fireCone.transform.position = conePos;
            isCreated = true;
        }
        
        var em = _boss.fireCone.emission;

        // Debug.Log("Hammer State!");
        _boss.fireCone.Play();
        em.enabled = true;
        if (timer >= 0.0f) {
            timer -= Time.deltaTime;
        } else {
            // Debug.Log("Done");
            //_boss.fireCone.Stop();
            GameObject.Destroy(FCA.gameObject);
            em.enabled = false;
            isCreated = false;
            timer = 1.25f;
            return typeof(IdleState);
        }
        return typeof(HammerState);
    }

}