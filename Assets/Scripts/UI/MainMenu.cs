using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
    void Start()
    {

        //SetActive to false so the window doesn't cover the buttons.
        credits.SetActive(false);
    }

    //Assigned to the credits button.
    public void showCredits(){
        credits.SetActive(true);
    }

    //Assigned to the close button inside the instruction window.
    public void hideCredits(){
        credits.SetActive(false);
    }

    //Assigned to the play button.
    public void playGame(){
        SceneManager.LoadScene(1);
    }

    public void exitGame(){
        Application.Quit();
    }
}
