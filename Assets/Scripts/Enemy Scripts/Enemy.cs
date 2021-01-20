using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    //Variables regarding enemy stats
    Rigidbody2D rb;
    public float healthAmount;
    private float speed = 1;

    public float timer = 0;

    public GameObject shard;
    //Area of Effect
    public GameObject AOE;
    
    //Target is the players' current location
    private Transform target;
    public bool inBounds = false;

    private GameObject[] enemyList;
    public static int enemyAmount;

    ////////////////////////////////

    StateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        healthAmount = 3f;
        rb = GetComponent<Rigidbody2D>();

        //getting transform component from the Player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        enemyAmount = enemyList.Length;
        stateMachine = new StateMachine();
        InitializeStateMachine();
        
    }

    // Update is called once per frame
    void Update() {
        isDead(PlayerController.gameOver);
        stateMachine.Update();
    }


    // Deal damage to player on contact
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
        
        //check for when players view is overlapping with the enemy
        if (collider.gameObject.name.Equals("View"))
        {
            inBounds = true;
        }
        
        
    }


    //Spawns a chard every 3 enemy deaths for the player to pick up
    //make a public Game Object "shard" and then you can assign a prefab object to that
    //In the future, we sohuld probably use tags instead, so that you don't have to 
    //manually assign the prefab object to the enemy controller script. 
    void spawnShard() {
        //if(UnityEngine.Random.value > .33) {
            GameObject go = (GameObject)Instantiate(shard);
            go.transform.position = this.transform.position;
        //}
    }

    void isDead(bool gameOver){
        if (!gameOver) { 
            if (healthAmount <= 0) {
                Destroy(this.gameObject);
                enemyAmount -= 1;
                spawnShard();
            }
            timer += Time.deltaTime; // Temporary
        }
    }

    /*
    Purpose: Initializes the state machine with all the states attached in
    the enemy scripts folder.
    Recieves: Nothing
    Returns: nothing
    */
    void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            { typeof(WanderState), new WanderState(this) },
            { typeof(ChaseState), new ChaseState(this) },
            { typeof(AttackState), new AttackState(this) }
        };

        stateMachine.SetStates(states);
    }

}
