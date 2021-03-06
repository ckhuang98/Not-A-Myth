using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Text skillDescription;

    public void clearSkillDescription()
    {
        skillDescription.text = "";
    }
    public void updateSkillDescription(SkillTreeButton button)
    {
        skillDescription.text = button.skillDescription + "\n" + button.currentUses + "/" + button.maxUses;
    }

    public void increaseTotalHealth(SkillTreeButton button)
    {
        if (button.currentUses < button.maxUses)
        {
            // if player has required items

            button.currentUses++;
        }
        updateSkillDescription(button);
    }

    public void increaseStrength(SkillTreeButton button)
    {
        if (button.currentUses < button.maxUses)
        {
            // if player has required items

            button.currentUses++;
        }
        updateSkillDescription(button);
    }

    public void increaseSpeed(SkillTreeButton button)
    {
        if (button.currentUses < button.maxUses)
        {
            // if player has required items

            button.currentUses++;
        }
        updateSkillDescription(button);
    }

    public void mysterySkillUpgrade(SkillTreeButton button)
    {
        if (button.currentUses < button.maxUses)
        {
            // if player has required items

            button.currentUses++;
        }
        updateSkillDescription(button);
    }
}
