using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeGate : MonoBehaviour
{

    public Gate gate;
    public PlayerStats playerStats;
    private bool gems = false;
    private bool gainedSkill = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerStats.currentXp.Value >= 5 && gems == false) {
            gems = true;
        }
        if(playerStats.skillPoints.Value == 1) {
            gainedSkill = true;
        }
        if(playerStats.skillPoints.Value == 0 && gainedSkill == true) {
            gate.unlocked = true;
        }
        Debug.Log("Gems: " + gems + ", Skill Points Value: " + playerStats.skillPoints.Value + ", GainedSkill: " + gainedSkill);
    }
}
