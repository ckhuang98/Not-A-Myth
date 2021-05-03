using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public Text enterInstructions;
    private bool active;

    [SerializeField]
    private List<string> keyNames;
    private bool locked;

    [SerializeField]
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        // if (playerStats == null) playerStats = ScriptableObject.CreateInstance("PlayerStats") as PlayerStats;

        if (enterInstructions == null)
        {
            enterInstructions = GameObject.FindWithTag("Enter Door Text").GetComponent<Text>();
            enterInstructions.text = "";
        }

        active = false;
        locked = keyNames.Count != 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyDown(KeyCode.E)){
            GameMaster.instance.loadScene();
        }

        if (locked){
            foreach(string keyName in playerStats.keys.Value){
                keyNames.Remove(keyName);
            }

            if (keyNames.Count == 0){
                locked = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            showText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hideText();
        }
    }

    void showText()
    {
        active = true;
        if (locked){
            enterInstructions.text = "LOCKED";
        } else {
            enterInstructions.text = "Press E to Enter";
        }
    }

    void hideText()
    {
        active = false;
        enterInstructions.text = "";
    }
}
