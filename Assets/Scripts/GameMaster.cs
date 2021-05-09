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
    public GMDelegate OnSceneLoad;
    public GMDelegate OnGameRestart;

    public delegate void GMDelegateBool(bool b);

    public GMDelegateBool OnGameOver;

    [SerializeField]
    public CombatManager combatManager;
    public GameObject player;

    public GameObject boss;

    
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
    public List<GameObject> enemyList = new List<GameObject>();
    public int numOfEnemies;

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
        enemyList = new List<GameObject>();
        if(player != null){
            combatManager.player = player.GetComponent<PlayerController>();
        }


        
        OnSceneLoad?.Invoke();
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
        //resumeGame();

        //playerStats.OnStatsChanged += Testing_PlayerStatsUpdated;
        //playerStats.currentHealth.Value += 1;
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

        if (Input.GetKeyDown(KeyCode.Tab))
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
        if(Input.GetKey(KeyCode.LeftShift)){
            if(Input.GetKeyDown(KeyCode.P)){
                loadScene(5);
            }
        }
    }

    public float getSkillPoints(){
        return playerStats.skillPoints.Value;
    }

    public void spendSkillPoints(){
        if(playerStats.skillPoints.Value > 0){
            playerStats.skillPoints.Value--;
        }
    }

    public void pickUpShard(){
        playerStats.currentXp.Value++;
    }

    public void gainStrength(){
        playerStats.attackPower.Value += 0.75f;
    }

    public void gainHealth(){
        if(playerStats.currentHealth.Value < (playerStats.maxHealth.Value - 15)){
            playerStats.maxHealth.Value += 15;
            playerStats.currentHealth.Value += 15;
        } else{
            playerStats.maxHealth.Value += 15;
            playerStats.currentHealth.Value = playerStats.maxHealth.Value;
        }
        
    }

    public void gainSpeed(){
        playerStats.maxSpeed.Value++;
        playerStats.sprintSpeed.Value++;
    }

    public void gainDoubleDash(){
        playerStats.unlockedDoubleDash.Value = true;
    }

    public void gainHealthDash(){
        playerStats.unlockedHealthDash.Value = true;
    }

    public void gainHealthRegen(){
        playerStats.unlockedRegen.Value = true;
    }

    public void gainDashAttack(){
        playerStats.unlockedDashAttack.Value = true;
    }

    public void gainAttackRegen(){
        playerStats.unlockedAttackRegen.Value = true;
    }

    public void gainGroundSmash(){
        playerStats.unlockedGroundSmash.Value = true;
    }

    public void toggleMovementOn(){
        playerStats.toggleMovement.Value = true;
    }

    public void toggleMovementOff(){
        playerStats.toggleMovement.Value = false;
    }

    //Get necessary references to objects in the scene
    void assignReferences()
    {
        //Debug.Log("Assigned Ref");
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GameObject.FindGameObjectWithTag("Boss");
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
        if (showMenu) {
            ui.showPauseMenu();
            ui.menuBackground.SetActive(true);
        }

        ui.updateInventoryUI();

        OnGamePuased?.Invoke();
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        paused = false;
        ui.hideAllPauseMenus();
        ui.menuBackground.SetActive(false);
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
            playerStats.currentHealth.Value = playerStats.maxHealth.Value;
        }
        OnGameRestart?.Invoke();
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
        resumeGame();
    }

    // Load the last scene in the build setting, which is the dev scene
    public void loadDevScene(){
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
        resumeGame();
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
            recordStats();
        }
        
    }
}
