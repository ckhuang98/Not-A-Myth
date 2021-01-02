using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject instructions;
    void Start()
    {
        //Assigns the instructions window to the instructions variable.
        instructions = GameObject.FindWithTag("MainMenu_Instructions");

        //SetActive to false so the window doesn't cover the buttons.
        instructions.SetActive(false);
    }

    //Assigned to the Instructions button.
    public void showInstructions(){
        instructions.SetActive(true);
    }

    //Assigned to the close button inside the instruction window.
    public void hideInstructions(){
        instructions.SetActive(false);
    }

    //Assigned to the play button.
    public void playGame(){
        SceneManager.LoadScene("ActualLevel");
    }
}
