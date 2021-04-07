using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    //Create Singleton instance of GameMaster
    public static GameMaster instance;
    //To access GameMaster's methods and fields, call GameMaster.instance from any other script

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField]
    private UI ui;

    // Events
    public delegate void GMDelegate();
    public GMDelegate OnGamePuased;
    public GMDelegate OnGameResumed;

    public delegate void GMDelegateBool(bool b);

    public GMDelegateBool OnGameOver;

    [SerializeField]
    private GameObject player;
    private int skillPoints = 0;
    private int numOfShards = 0;

    private bool paused = false;

    [SerializeField]
    private bool gameOver = false;

    private bool statsRecorded = false;


    //player stats to be saved
    public int recordedPlayerHealth;
    public float recordedPlayerStrength;
    public List<Item> recordedInventory = new List<Item>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // OnSceneLoaded: Called every time a new scene is loaded (before Start)
    // Treat like a Start method for every time a new scene is loaded
    // since the GameManager carries over into all scenes
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ui = UI.instance;
        gameOver = false;
        assignReferences();
        ui.setUIForNewScene();
        ui.updateInventoryUI();
    }


    public PlayerStats playerStats;

    public void Testing_PlayerStatsUpdated()
	{
        Debug.Log("GameMaster: Player Stats Updated");
	}


    private void Start()
    {
        Cursor.visible = true;
        paused = false;

        playerStats.OnStatsChanged += Testing_PlayerStatsUpdated;
        playerStats.currentHealth.Value += 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                resumeGame();
            } else
            {
                pauseGame();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (paused)
            {
                resumeGame();
            } else
            {
                pauseGame(false);
                ui.hideHotbar();
                ui.showSkillTree();
            }
        }
    }

    public int getSkillPoints(){
        return skillPoints;
    }

    public void spendSkillPoints(){
        if(skillPoints > 0){
            skillPoints--;
        }
    }

    public void pickUpShard(){
        numOfShards++;
        if(numOfShards == 5){
            numOfShards = 0;
            skillPoints++;
        }
    }

    public void gainStrength(){
        player.GetComponent<PlayerController>().gainStrength();
    }

    public void gainHealth(){
        player.GetComponent<PlayerController>().gainHealth();
    }

    public void gainSpeed(){
        player.GetComponent<PlayerController>().gainSpeed();
    }

    public void gainFireResistance(){
        
    }

    public void gainDoubleDash(){
        player.GetComponent<PlayerController>().gainDoubleDash();
    }

    public void gainKnockback(){
        player.GetComponent<PlayerController>().gainKnockback();
    }

    //Get necessary references to objects in the scene
    void assignReferences()
    {
        Debug.Log("Assigned Ref");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject getPlayer()
    {
        return player;
    }

    public bool isPaused()
    {
        return paused;
    }

    void pauseGame(bool showMenu = true)
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        paused = true;
        if (showMenu) ui.showPauseMenu();
        ui.updateInventoryUI();

        OnGamePuased?.Invoke();
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        paused = false;
        ui.hideAllPauseMenus();
        ui.showHotbar();

        // OnGameResumed?.Invoke();
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void restartCheckpoint(bool fullHealth = false)
    {
        resumeGame();
        if (fullHealth) //set player's health to max. Used for when restarting checkpoint after death
        {
            recordedPlayerHealth = player.GetComponent<PlayerController>().maxHealth;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Load next scene

    public void loadScene(){
        if (!paused)
        {
            recordStats();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Load the scene with the given build index
    public void loadScene(int index)
    {
        if (!paused)
        {
            recordStats();
            SceneManager.LoadScene(index);
        }
    }

    // Load the scene witht the given name
    public void loadScene(string name)
    {
        if (!paused)
        {
            recordStats();
            SceneManager.LoadScene(name);
        }
    }

    public void loadMainMenuScene(){
        Destroy(gameObject);
        SceneManager.LoadScene(0);
    }

    // Game over
    public bool getGameOver()
    {
        return gameOver;
    }

    public void setGameOver(bool win = false)
    {
        // string gameOverMessage = win ? "A Winner Is You!" : "Game Over!";
        // ui.showGameOverMenu(gameOverMessage);

        OnGameOver?.Invoke(win);

        gameOver = true;
    }

    //Player stats
    public bool checkPlayerStatsRecorded()
    {
        return statsRecorded;
    }

    //record the necessary stats associated with the player and inventory
    void recordStats()
    {
        if (player != null)
        {
            recordedPlayerHealth = player.GetComponent<PlayerController>().currentHealth;
            recordedPlayerStrength = player.GetComponent<PlayerController>().attackStrength;

            // save inventory
            recordedInventory.Clear();
            foreach (Item it in Inventory.instance.items)
            {
                Item itemCopy = Instantiate(it) as Item;
                // change the name to avoid (Clone) in the object name
                itemCopy.name = it.name;
                recordedInventory.Add(itemCopy);
            }

            statsRecorded = true;
        } else
        {
            Debug.LogWarning("No instance of Player found");
            recordedPlayerHealth = 100;
            recordedPlayerStrength = 1.0f;
            recordedInventory.Clear();
            statsRecorded = false;
        }
    }

    // Apply the stats to the player and inventory
    public void applyStats(bool overrideHealth = false)
    {
        if (statsRecorded)
        {
            if (player != null)
            {
                if (overrideHealth)
                {
                    player.GetComponent<PlayerController>().currentHealth = recordedPlayerHealth;
                } else
                {
                    player.GetComponent<PlayerController>().currentHealth = player.GetComponent<PlayerController>().maxHealth;
                }

                player.GetComponent<PlayerController>().attackStrength = recordedPlayerStrength;

                // load inventory
                Inventory.instance.items.Clear();
                foreach (Item it in recordedInventory)
                {
                    Item itemCopy = Instantiate(it) as Item;
                    // change the name to avoid (Clone)
                    itemCopy.name = it.name;
                    Inventory.instance.items.Add(itemCopy);
                }

                ui.updateInventoryUI();

                statsRecorded = true;
            }
            else
            {
                Debug.LogWarning("No instance of Player found");
            }
        } else
        {
            player.GetComponent<PlayerController>().currentHealth = player.GetComponent<PlayerController>().maxHealth;
            player.GetComponent<PlayerController>().attackStrength = 1.0f; //TODO: don't hard code attackStrength default value
            recordStats();
        }
        
    }
}
