using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public Text enterInstructions;
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        if (enterInstructions == null)
        {
            enterInstructions = GameObject.FindWithTag("Enter Door Text").GetComponent<Text>();
        }

        active = false;
        enterInstructions.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetKeyDown(KeyCode.E)){
            GameMaster.instance.loadSceneWithIndex(2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            showText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            hideText();
        }
    }

    void showText()
    {
        active = true;
        enterInstructions.text = "Press E to Enter";
    }

    void hideText()
    {
        active = false;
        enterInstructions.text = "";
    }
}
