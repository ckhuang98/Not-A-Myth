using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Text skillDescription;

    public Text skillPointsText;

    private GameMaster gameMaster;

    private bool h1, h2, h3, s1, s2, s3, a1, a2, a3 = false;

    public void Start(){}

    public void clearSkillDescription()
    {
        skillDescription.text = "";
    }
    public void updateSkillDescription(SkillTreeButton button)
    {
        skillDescription.text = button.skillDescription + "\n" + button.currentUses + "/" + button.maxUses;
    }

    public void updateSkillPoints(){
        skillPointsText.text = "Skill Points: " + GameMaster.instance.getSkillPoints();
    }

    public void increaseTotalHealth(SkillTreeButton button)
    {
        if (button.currentUses < button.maxUses)
        {
            if(button.ToString() == "Increase Max Health Button 1 (SkillTreeButton)"){
                if(GameMaster.instance.getSkillPoints() > 0){
                GameMaster.instance.spendSkillPoints();
                updateSkillPoints();
                GameMaster.instance.gainHealth();
                button.currentUses++;
                h1 = true;
                }
            } else if(h1){
                if(button.ToString() == "Increase Max Health Button 2 (SkillTreeButton)"){
                    if(GameMaster.instance.getSkillPoints() > 0){
                        GameMaster.instance.spendSkillPoints();
                        updateSkillPoints();
                        GameMaster.instance.gainHealth();
                        button.currentUses++;
                        h2 = true;
                    }
                } else{
                    if(GameMaster.instance.getSkillPoints() > 0){
                        GameMaster.instance.spendSkillPoints();
                        updateSkillPoints();
                        GameMaster.instance.gainHealth();
                        button.currentUses++;
                        h3 = true;
                    }
                }
            }
            // if player has required items
            
        }
        updateSkillDescription(button);
    }

    public void increaseStrength(SkillTreeButton button)
    {
        if (button.currentUses < button.maxUses){

            if(button.ToString() == "Increase Max Strength Button 1 (SkillTreeButton)"){
                if(GameMaster.instance.getSkillPoints() > 0){
                    GameMaster.instance.spendSkillPoints();
                    updateSkillPoints();
                    GameMaster.instance.gainStrength();
                    button.currentUses++;
                    a1 = true;
                }
            } else if(a1){
                if(button.ToString() == "Increase Max Strength Button 2 (SkillTreeButton)"){
                    if(GameMaster.instance.getSkillPoints() > 0){
                        GameMaster.instance.spendSkillPoints();
                        updateSkillPoints();
                        GameMaster.instance.gainStrength();
                        button.currentUses++;
                        a2 = true;
                    }               
                } else{
                    if(GameMaster.instance.getSkillPoints() > 0){
                        GameMaster.instance.spendSkillPoints();
                        updateSkillPoints();
                        GameMaster.instance.gainStrength();
                        button.currentUses++;
                        a3 = true;
                    }                     
                }
            }
        }
        updateSkillDescription(button);
    }

    public void increaseSpeed(SkillTreeButton button)
    {
        if (button.currentUses < button.maxUses)
        {
            if(button.ToString() == "Increase Max Speed Button 1 (SkillTreeButton)"){
                if(GameMaster.instance.getSkillPoints() > 0){
                    GameMaster.instance.spendSkillPoints();
                    updateSkillPoints();
                    GameMaster.instance.gainSpeed();
                    button.currentUses++;
                    s1 = true;
                }
            } else if(s1){
                if(button.ToString() == "Increase Max Speed Button 2 (SkillTreeButton)"){
                    if(GameMaster.instance.getSkillPoints() > 0){
                        GameMaster.instance.spendSkillPoints();
                        updateSkillPoints();
                        GameMaster.instance.gainSpeed();
                        button.currentUses++;
                        s3 = true;
                    }               
                } else{
                    if(GameMaster.instance.getSkillPoints() > 0){
                        GameMaster.instance.spendSkillPoints();
                        updateSkillPoints();
                        GameMaster.instance.gainSpeed();
                        button.currentUses++;
                        s2 = true;
                    }                     
                }
            }
        }
        updateSkillDescription(button);
    }


    public void doubleDashSkillUpgrade(SkillTreeButton button){
        if (button.currentUses < button.maxUses)
        {
            if(GameMaster.instance.getSkillPoints() > 0 && (s1 && s2 && s3)){
                GameMaster.instance.spendSkillPoints();
                updateSkillPoints();
                GameMaster.instance.gainDoubleDash();
                button.currentUses++;
            }
        }
        updateSkillDescription(button);
    }

    public void healthDashSkillUpgrade(SkillTreeButton button){
        if (button.currentUses < button.maxUses)
        {
            Debug.Log(h2 + " " + s2);
            if(GameMaster.instance.getSkillPoints() > 0 && (h2 & s2)){
                GameMaster.instance.spendSkillPoints();
                updateSkillPoints();
                GameMaster.instance.gainHealthDash();
                button.currentUses++;
            }
        }
        updateSkillDescription(button);
    }

}
