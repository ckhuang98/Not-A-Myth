using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    public PlayerStats playerStats;
    public ParticleSystem gemPickUp;
    
    private int skillPoints;
    // Start is called before the first frame update
    void Start()
    {
        playerStats.currentXp.OnValueChanged += playGemPickUp;
        gemPickUp = gemPickUp.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playGemPickUp(){
        gemPickUp.Play();
    }
}
