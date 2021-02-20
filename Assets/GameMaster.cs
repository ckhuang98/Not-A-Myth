using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    // Singleton for inventory
    public static GameMaster instance;

    //The inventory exists within the GameManager object
    //and it must be the only instance of inventory at all times
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameMaster found!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    private AudioListener listener;
    private GameObject player;
    [SerializeField]
    private GameObject escapeMenu;
    private GameObject controlsWindow;
    private GameObject inventoryWindow;
    public InputActionAsset inputActions;
    private bool paused;

    private int recordedPlayerHealth;
    private bool playerHealthRecorded = false;

    private bool firstInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Cursor.visible = true;
        paused = false;
        
        assignReferences();
        controlsWindow.SetActive(false);
        escapeMenu.SetActive(false);
        inventoryWindow.SetActive(false);

        firstInitialized = true;
    }

    void assignReferences()
    {
        player = GameObject.FindWithTag("Player");
        listener = player.GetComponent<AudioListener>();

        controlsWindow = GameObject.FindWithTag("Controls");
        escapeMenu = GameObject.FindWithTag("EscapeMenu");
        inventoryWindow = GameObject.FindWithTag("Inventory");
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

        if (Input.GetKeyDown(KeyCode.N) && !paused)
        {
            recordPlayerHealth();
            SceneManager.LoadScene(2);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (firstInitialized)
        {
            //get the new player object and associated components
            player = GameObject.FindWithTag("Player");
            listener = player.GetComponent<AudioListener>();
        }
    }

    public bool getPaused()
    {
        return paused;
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

    //Get the player's health at the current time
    void recordPlayerHealth()
    {
        if (player != null)
        {
            recordedPlayerHealth = player.GetComponent<PlayerController>().currentHealth;
            playerHealthRecorded = true;
        } else
        {
            playerHealthRecorded = false;
        }
    }

    //Return what the player's health was when last recorded
    public int getRecordedPlayerHealth()
    {
        return recordedPlayerHealth;
    }

    //Get whether the player's health has been recorded yet or not
    public bool getPlayerHealthRecorded()
    {
        return playerHealthRecorded;
    }
}
