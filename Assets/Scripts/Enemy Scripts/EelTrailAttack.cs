using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelTrailAttack : MonoBehaviour
{
    ParticleSystem ps;
    private GameObject pc;
    PlayerController player;
    float parentHealth;

    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();
    void Start(){
        parentHealth = GetComponentInParent<Enemy>().healthAmount;
        pc = GameObject.FindGameObjectWithTag("Player");
        var trigger = ps.trigger;
        trigger.enabled = true;
        trigger.SetCollider(0, pc.GetComponent<CapsuleCollider2D>());
        player = pc.GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update() {
        if (parentHealth <= 0) {
            this.transform.parent = null;
        }
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
            player.TakeDamage(10);
            //Debug.Log("Hit");
        }

        // re-assign the modified particles back into the particle system
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    }
}
