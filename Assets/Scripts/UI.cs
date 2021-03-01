using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    //Create Singleton instance of UI
    public static UI instance;
    //To access UI's methods and fields, call UI.instance from any other script

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject controlsMenu;
    [SerializeField]
    private GameObject inventoryMenu;// this is the actual ui that appears
    [SerializeField]
    private InventoryUI inventoryUIComponent;// this is the script that controls the inventory ui
    [SerializeField]
    private GameObject hotbar;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject playerUpdates;


    // Start is called before the first frame update
    void Start()
    {
        inventoryUIComponent = GetComponent<InventoryUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUIForNewScene()
    {
        hideAllPauseMenus();
        hideGameOverMenu();
        playerUpdates.GetComponent<Text>().text = "";
    }

    public void hideAllPauseMenus()
    {
        hidePauseMenu();
        hideControlsMenu();
        hideInventoryMenu();
    }

    // pause menu
    public void showPauseMenu()
    {
        pauseMenu.SetActive(true);
    }
    public void hidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    //controls menu
    public void showControlsMenu()
    {
        controlsMenu.SetActive(true);
    }
    public void hideControlsMenu()
    {
        controlsMenu.SetActive(false);
    }

    //inventory
    public void showInventoryMenu()
    {
        inventoryMenu.SetActive(true);
    }
    public void hideInventoryMenu()
    {
        inventoryMenu.SetActive(false);
    }
    public void updateInventoryUI()
    {
        inventoryUIComponent.UpdateUI();
    }

    //game over
    public void showGameOverMenu(string message = "Game Over!")
    {
        gameOverMenu.SetActive(true);
        gameOverMenu.transform.Find("Message").GetComponent<Text>().text = message;
    }
    public void hideGameOverMenu()
    {
        gameOverMenu.SetActive(false);
    }
    public void setGameOverText(string message)
    {
        gameOverMenu.transform.Find("Message").GetComponent<Text>().text = message;
    }

    //display player update
    public IEnumerator displayerPlayerUpdate(string message = "", float duration = 1.5f)
    {
        playerUpdates.GetComponent<Text>().text = message;
        yield return new WaitForSecondsRealtime(duration); //realtime so it works when game is paused
        playerUpdates.GetComponent<Text>().text = "";
    }

}
