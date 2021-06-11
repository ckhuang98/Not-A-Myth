using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;
    public GameObject options;
    public GameObject showcase;
    void Start()
    {

        //SetActive to false so the window doesn't cover the buttons.
        credits.SetActive(false);

        options.SetActive(false);

        showcase.SetActive(false);
    }

    //Assigned to the credits button.
    public void showCredits(){
        credits.SetActive(true);
    }

    //Assigned to the close button inside the instruction window.
    public void hideCredits(){
        credits.SetActive(false);
    }

    public void showOptions(){
        options.SetActive(true);
    }

    public void hideOptions(){
        options.SetActive(false);
    }

    public void showShowcase(){
        showcase.SetActive(true);
    }

    public void hideShowcase(){
        showcase.SetActive(false);
    }

    //Assigned to the play button.
    public void playGame(){
        SceneManager.LoadScene(1);
    }

    public void exitGame(){
        Application.Quit();
    }
}
