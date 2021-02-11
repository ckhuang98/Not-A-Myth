using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private GameObject escapeMenu;

    [SerializeField]
    private AudioListener listener;

    private GameObject player;
    private GameObject controlsWindow;
    private GameObject inventoryWindow;
    public InputActionAsset inputActions;
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        paused = false;
        controlsWindow = GameObject.FindWithTag("Controls");
        controlsWindow.SetActive(false);
        escapeMenu = GameObject.FindWithTag("EscapeMenu");
        escapeMenu.SetActive(false);
        player = GameObject.FindWithTag("Player");

        inventoryWindow = GameObject.FindWithTag("Inventory");
        inventoryWindow.SetActive(false);
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
        AudioListener.pause = true;
        inputActions.Disable();
        paused = true;
        showEscapeMenu();
    }

    public void ResumeGame(){
        Time.timeScale = 1;
        AudioListener.pause = false;
        inputActions.Enable();
        paused = false;
        hideAllMenus();
    }

    public void hideAllMenus()
    {
        hideEscapeMenu();
        hideControls();
        hideInventory();
    }

    public void showEscapeMenu(){
        escapeMenu.SetActive(true);
    }

    public void hideEscapeMenu(){
        escapeMenu.SetActive(false);
    }

    public void showControls(){
        controlsWindow.SetActive(true);
    }

    public void hideControls(){
        controlsWindow.SetActive(false);
    }

    public void showInventory(){
        inventoryWindow.SetActive(true);
    }

    public void hideInventory(){
        inventoryWindow.SetActive(false);
    }

    public void exitGame(){
        Application.Quit();
    }
}
