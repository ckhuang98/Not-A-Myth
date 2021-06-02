using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    public PlayerStats playerStats;
    public ParticleSystem gemPS;

    public ParticleSystem healPS;
    public ParticleSystem levelUpPS1;
    public ParticleSystem levelUpPS2;
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.EmissionModule em = gemPS.emission;
        em.enabled = true;
        em = levelUpPS1.emission;
        em.enabled = true;
        em = levelUpPS2.emission;
        em.enabled = true;
        playerStats.currentXp.OnValueChanged += playGemPickUP;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void playGemPickUP(){
            gemPS.Play();
    }

    public void playHeal(){
        healPS.Play();
    }
    public void playLevelUp(){
        levelUpPS1.Play();
        levelUpPS2.Play();
    }
}
