using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System;
using System.Linq;

public class Boss : MonoBehaviour
{
    //Variables regarding enemy stats
    Rigidbody2D rb;
    public float healthAmount;

    public Transform target;

    private float timer;

    BossStateMachine stateMachine; 
    private ObjectAudioManager audioManager;

    public GameObject slash;
    public GameObject shockWave;
    public ParticleSystem fireCone;
    public GameObject fireConeArea;

    public Animator animator;
    public BarScript healthBar;
    public Text speechText;

    // Start is called before the first frame update
    void Start()
    {
        healthAmount = 25f;
        rb = GetComponent<Rigidbody2D>();

        healthBar.SetMaxValue(healthAmount);

        //getting transform component from the Player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        stateMachine = new BossStateMachine();
        audioManager = gameObject.GetComponent<ObjectAudioManager>();
        InitializeStateMachine();
        fireCone.GetComponent<ParticleSystem>();
        fireCone.Pause();
        if(fireCone.isPaused){
            Debug.Log("Paused Particle sys");
            fireCone.Pause();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // isDead(PlayerController.gameOver);
        stateMachine.Update();
        isDead(PlayerController.gameOver);
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Equals("SlashSpriteSheet_0") && timer >= .5)
        {
            // Vector2 knockback = rb.transform.position - collider.transform.parent.position;
            // //Debug.Log(knockback);
            // rb.AddForce(knockback.normalized * 4000f);
            healthAmount -= collider.transform.parent.parent.GetComponent<PlayerController>().whatIsStrength();
            healthBar.SetValue(healthAmount);
            timer = 0;
        }
    }

    void isDead(bool gameOver){
        if (!gameOver) { 
            if (healthAmount <= 0)
            {
                Destroy(this.gameObject);
            }
            timer += Time.deltaTime; // Temporary
        }
    }
}
