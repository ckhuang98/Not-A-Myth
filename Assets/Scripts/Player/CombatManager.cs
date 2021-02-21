using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    public static PlayerController player;

    public bool canReceiveInput;
    public bool inputReceived;

    private void Awake() {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Attack(){
        if(canReceiveInput){
            //Debug.Log("Attack!");
            inputReceived = true;
            canReceiveInput = false;
        } else{
            return;
        }
    }

    public void InputManager(){
        if(!canReceiveInput){
            canReceiveInput = true;
        } else{
            canReceiveInput = false;
        }
    }
}
