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
    private UI ui = UI.instance;

    [SerializeField]
    private GameObject player;
    private bool paused = false;
    [SerializeField]
    private bool gameOver = false;

    private bool firstInitialized = false;

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
        assignReferences();
        recordStats();
        ui.setUIForNewScene();
        ui.updateInventoryUI();
    }

    private void Start()
    {
        Cursor.visible = true;
        paused = false;
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            assignReferences();
            recordStats();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            applyStats(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            restartCheckpoint();
        }
    }

    void assignReferences()
    {
        player = GameObject.FindWithTag("Player");
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
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        paused = false;
        ui.hideAllPauseMenus();
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void restartCheckpoint()
    {
        resumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void loadScene(int index)
    {
        if (!paused)
        {
            recordStats();
            SceneManager.LoadScene(index);
        }
    }

    public void loadScene(string name)
    {
        if (!paused)
        {
            recordStats();
            SceneManager.LoadScene(name);
        }
    }

    public bool getGameOver()
    {
        return gameOver;
    }

    public void setGameOver(bool win = false)
    {

        string gameOverMessage = win ? "A Winner Is You!" : "Game Over!";
        ui.showGameOverMenu(gameOverMessage);

        gameOver = true;
    }

    public bool playerStatsRecorded()
    {
        return statsRecorded;
    }

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
                // change the name to avoid (Clone)
                itemCopy.name = it.name;
                recordedInventory.Add(itemCopy);
            }

            statsRecorded = true;
        } else
        {
            recordedPlayerHealth = 0;
            recordedPlayerStrength = 0.0f;
            recordedInventory.Clear();
            statsRecorded = false;
        }
    }

    public void applyStats(bool overrideHealth = false)
    {
        if (statsRecorded)
        {
            if (player != null)
            {
                if (overrideHealth)
                {
                    player.GetComponent<PlayerController>().currentHealth = recordedPlayerHealth;
                }

                player.GetComponent<PlayerController>().attackStrength = recordedPlayerStrength;

                // save inventory
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
                Debug.LogWarning("No player found");
            }
        } else
        {
            player.GetComponent<PlayerController>().currentHealth = 100;
            player.GetComponent<PlayerController>().attackStrength = 1.0f;
            recordStats();
        }
        
    }
}
