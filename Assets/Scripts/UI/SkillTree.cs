using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Text skillDescription;

    public Text skillPointsText;

    private GameMaster gameMaster;

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
            // if player has required items
            if(GameMaster.instance.getSkillPoints() > 0){
                GameMaster.instance.spendSkillPoints();
                updateSkillPoints();
                GameMaster.instance.gainHealth();
                button.currentUses++;
            }
        }
        updateSkillDescription(button);
    }

    public void increaseStrength(SkillTreeButton button)
    {
        if (button.currentUses < button.maxUses)
        {
            // if player has required items

            if(GameMaster.instance.getSkillPoints() > 0){
                GameMaster.instance.spendSkillPoints();
                updateSkillPoints();
                GameMaster.instance.gainStrength();
                button.currentUses++;
            }
        }
        updateSkillDescription(button);
    }

    public void increaseSpeed(SkillTreeButton button)
    {
        if (button.currentUses < button.maxUses)
        {
            // if player has required items

            if(GameMaster.instance.getSkillPoints() > 0){
                GameMaster.instance.spendSkillPoints();
                updateSkillPoints();
                GameMaster.instance.gainSpeed();
                button.currentUses++;
            }
        }
        updateSkillDescription(button);
    }


    public void doubleDashSkillUpgrade(SkillTreeButton button){
        if (button.currentUses < button.maxUses)
        {
            if(GameMaster.instance.getSkillPoints() > 0 && GameMaster.instance.playerStats.maxSpeed.Value == 8){
                GameMaster.instance.spendSkillPoints();
                updateSkillPoints();
                GameMaster.instance.gainDoubleDash();
                button.currentUses++;
            }
        }
        updateSkillDescription(button);
    }

}
