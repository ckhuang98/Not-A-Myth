﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttack : MonoBehaviour
{
     ParticleSystem ps;
     private GameObject pc;
     PlayerController player;
     
     private GameObject boss;

    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        pc = GameObject.FindGameObjectWithTag("Player");
        player = pc.GetComponent<PlayerController>();
        boss = GameObject.FindGameObjectWithTag("Boss");
    }

    void OnParticleTrigger()
    {
        // get the particles which matched the trigger conditions this frame
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
        // iterate through the particles which entered the trigger and make them red
        for (int i = 0; i < numEnter; i++)
        {
            //ParticleSystem.Particle p = enter[i];
            //p.startColor = new Color32(255, 0, 0, 255);
            //enter[i] = p;
            if(!player.isInvincible){
                player.TakeDamage(15);
                player.StartCoroutine(player.HammerKnockBack(GameMaster.instance.enemyKnockbackDuration, GameMaster.instance.enemyKnockbackPower, boss.transform));
            }
            
        }

        // re-assign the modified particles back into the particle system
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }
}