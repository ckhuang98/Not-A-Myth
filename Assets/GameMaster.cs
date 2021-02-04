using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private GameObject escapeMenu;

    private GameObject player;
    public InputActionAsset inputActions;
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        paused = false;
        escapeMenu = GameObject.FindWithTag("EscapeMenu");
        escapeMenu.SetActive(false);
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(paused == false){
                PauseGame();
            } else{
                ResumeGame();
            }
        }
    }

    void PauseGame(){
        Time.timeScale = 0;
        inputActions.Disable();
        paused = true;
        escapeMenu.SetActive(true);
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        inputActions.Enable();
        paused = false;
        escapeMenu.SetActive(false);
    }
}
