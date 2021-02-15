using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileState : BaseState
{
    private Boss _boss;
    private GameObject FCA;
    private float timer  = 5.0f;
    private bool isCreated = false;

    public ProjectileState(Boss boss) : base (boss.gameObject)
    {
        _boss = boss;
    }
    
    public override Type Tick()
    {
        if (isCreated == false) {
            FCA = GameObject.Instantiate(_boss.fireConeArea) as GameObject;
            FCA.transform.position = new Vector3(0, -1, 1);
            isCreated = true;
        }
        var em = _boss.fireCone.emission;
        Debug.Log("Projectile State!");
        _boss.fireCone.Play();
        em.enabled = true;
        if (timer >= 0.0f) {
            timer -= Time.deltaTime;
        } else {
            Debug.Log("Done");
            //_boss.fireCone.Stop();
            GameObject.Destroy(FCA.gameObject);
            em.enabled = false;
            isCreated = false;
            timer = 5.0f;
            return typeof(IdleState);
        }
        return typeof(ProjectileState);
    }
}