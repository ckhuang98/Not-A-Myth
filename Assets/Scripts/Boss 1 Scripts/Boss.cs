using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Linq;

public class Boss : MonoBehaviour
{
    //Variables regarding enemy stats
    Rigidbody2D rb;
    public float healthAmount;

    private Transform target;

    BossStateMachine stateMachine; 
    private ObjectAudioManager audioManager;

    public GameObject slash;
    public GameObject shockWave;
    public ParticleSystem fireCone;
    public GameObject fireConeArea;

    // Start is called before the first frame update
    void Start()
    {
        healthAmount = 69f;
        rb = GetComponent<Rigidbody2D>();

        //getting transform component from the Player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        stateMachine = new BossStateMachine();
        audioManager = gameObject.GetComponent<ObjectAudioManager>();
        InitializeStateMachine();
        fireCone.GetComponent<ParticleSystem>();
        fireCone.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        // isDead(PlayerController.gameOver);
        stateMachine.Update();
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
            { typeof(IdleState), new IdleState(this) },
            { typeof(HammerState), new HammerState(this) },
            { typeof(SwordState), new SwordState(this) },
            { typeof(ProjectileState), new ProjectileState(this) }
        };
        stateMachine.SetStates(states);
    }
}
