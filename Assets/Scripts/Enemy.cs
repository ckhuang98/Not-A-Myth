using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variables regarding enemy stats
    Rigidbody2D rb;
    public float healthAmount;
    public float speed = 1;

    public float timer = 0;

    public GameObject shard;
    
    //Target is the players' current location
    private Transform target;
    private bool inBounds = false;

    bool gameOver;
    private bool ded = false;

    private GameObject[] enemyList;
    public static int enemyAmount;
    // Start is called before the first frame update
    void Start()
    {
        healthAmount = 3f;
        rb = GetComponent<Rigidbody2D>();

        //getting transform component from the Player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        gameOver = target.GetComponent<PlayerController>().gameOver;

        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        enemyAmount = enemyList.Length;
    }

    // Update is called once per frame
    void Update() {
        gameOver = target.GetComponent<PlayerController>().gameOver;
        if (!gameOver) { 
            if (healthAmount <= 0 && ded == false) {
                Destroy(this.gameObject);
                enemyAmount -= 1;
                spawnShard();
                ded = true; //make sure it only checks once
            }
            timer += Time.deltaTime;
            chasePlayer(target);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //check if the player's slash has hit this object (an enemy)
        if (collider.gameObject.name.Equals("SlashSpriteSheet_0") && timer >= .5)
        {
            //if it has, decrease this guys health and lighten the color
            healthAmount -= collider.transform.parent.parent.GetComponent<PlayerController>().whatIsStrength();
            var thisColor = this.GetComponent<Renderer>().material.color;
            thisColor.a -= .1f;
            this.GetComponent<Renderer>().material.color = thisColor;

            timer = 0;
        }
        
        //check for when players view is overlapping with the enemy
        if (collider.gameObject.name.Equals("View"))
        {
            inBounds = true;
        }
        
    }

    //spawn a shard 1/3 of the time an enemy dies. The shard allows the player to gain back some health and gain strength.
    void spawnShard() {
        if(Random.value > .33) {
            GameObject go = (GameObject)Instantiate(shard);
            go.transform.position = this.transform.position;
        }
    }

    /*
    Purpose: This function detects the players location and moves the enemy sprite towards
    the player
    Recieves: The transform component that belongs to the player.
    Returns: nothing
    */
    void chasePlayer(Transform target)
    {
        if (inBounds)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
