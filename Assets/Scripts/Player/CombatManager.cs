using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public PlayerController player;

    public bool canReceiveInput;
    public bool inputReceived;

    private void Start() {
        
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

    public IEnumerator attackCooldown(){
        canReceiveInput = false;
        Debug.Log("Coroutine called");
        yield return new WaitForSeconds(0.5f);
        canReceiveInput = true;
    }
}
