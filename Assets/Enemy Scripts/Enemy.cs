using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class Enemy : StateMachine
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

    private GameObject[] enemyList;
    public static int enemyAmount;

    ////////////////////////////////

    StateMachine stateMachine;
    EnemyTrigger enemyTrigger;

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
        stateMachine = GetComponent<StateMachine>();
        enemyTrigger = GetComponent<EnemyTrigger>();
        InitializeStateMachine();
        
    }

    // Update is called once per frame
    void Update() {
        gameOver = target.GetComponent<PlayerController>().gameOver;
        if (!gameOver) { 
            if (healthAmount <= 0) {
                Destroy(this.gameObject);
                enemyAmount -= 1;
                spawnShard();
            }
            timer += Time.deltaTime;
            chasePlayer(target);
        }
        stateMachine.Update();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Equals("SlashSpriteSheet_0") && timer >= .5)
        {
            healthAmount -= collider.transform.parent.parent.GetComponent<PlayerController>().whatIsStrength();
            var thisColor = this.GetComponent<Renderer>().material.color;
            thisColor.a -= .1f;
            this.GetComponent<Renderer>().material.color = thisColor;

            timer = 0;
        }
        /*
        //check for when players view is overlapping with the enemy
        if (collider.gameObject.name.Equals("View"))
        {
            inBounds = true;
        }
        */
        
    }

    void spawnShard() {
        if(UnityEngine.Random.value > .33) {
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
            //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            { typeof(WanderState), new WanderState(this, enemyTrigger) },
            { typeof(ChaseState), new ChaseState(this) }
        };

        stateMachine.SetStates(states);
    }

}
