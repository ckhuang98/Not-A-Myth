using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeButton : MonoBehaviour
{

    public int maxUses = 1;
    public int currentUses = 0;
    public string skillDescription;

    public Image highlight;

    private void Start() {
        highlight.enabled = false;
    }

    private void Update() {
        if(currentUses == maxUses){
            highlight.enabled = true;
        }
    }

}
