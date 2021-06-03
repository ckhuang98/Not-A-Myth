using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXp : MonoBehaviour
{
    public PlayerStats playerStats;

    public GameObject[] yellowHighlight;

    public ParticleEffects particleEffects;


    void Start(){
        resetAll();
    }

    void Update() {
        particleEffects = GameMaster.instance.particleEffects;
        updateProgress(playerStats.currentXp.Value);
    }

    private void resetAll(){
        yellowHighlight[0].SetActive(false);
        yellowHighlight[1].SetActive(false);
        yellowHighlight[2].SetActive(false);
        yellowHighlight[3].SetActive(false);
        yellowHighlight[4].SetActive(false);
    }

    private void updateProgress(float xp){
        for(int i = 0; i < xp; i++){
            if(i < 5){
                yellowHighlight[i].SetActive(true);
                
            }
        }

        if(xp >= 5){
            StartCoroutine(levelUp());
            particleEffects.playLevelUp();
            playerStats.currentXp.Value -= 5;
            playerStats.skillPoints.Value++;
        }
    }

    private IEnumerator levelUp(){
        yellowHighlight[4].SetActive(true);
        UI.instance.playLevelUpSound("LevelUp");
        StartCoroutine(UI.instance.displayerPlayerUpdate("Skill Point Earned. \nPress Tab to Open the Skill Tree!"));
        yield return new WaitForSeconds(0.5f);
        resetAll();
    }

}