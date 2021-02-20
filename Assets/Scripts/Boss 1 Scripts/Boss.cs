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
    public String targetLastPos;

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

    public bool calledAnimationHandler = false;

    void Awake() {
        fireCone.GetComponent<ParticleSystem>();
        fireCone.Pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthAmount = 50f;
        rb = GetComponent<Rigidbody2D>();

        healthBar.SetMaxValue(healthAmount);

        //getting transform component from the Player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        stateMachine = new BossStateMachine();
        audioManager = gameObject.GetComponent<ObjectAudioManager>();
        InitializeStateMachine();
        
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
            { typeof(HammerState), new HammerState(this) },         // Cone of fire
            { typeof(SwordState), new SwordState(this) },           // Slash Attack
            { typeof(ProjectileState), new ProjectileState(this) }  // Shockwave
        };
        stateMachine.SetStates(states);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Equals("SlashSpriteSheet_0") && timer >= .5)
        {
            Debug.Log("Hit");
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
                DestroyImmediate(this.gameObject);
                Destroy(healthBar.gameObject);
            }
            timer += Time.deltaTime; // Temporary
        }
    }

    public void startAnimation(int num){
        StartCoroutine(animationHandler(num));
    }

    public void stopAnimation(){
        animator.SetBool("SlashCenter", false);
        animator.SetBool("SlashRight", false);
        animator.SetBool("SlashLeft", false);
        animator.SetBool("ConeCenter", false);
        animator.SetBool("ConeRight", false);
        animator.SetBool("ConeLeft", false);
        animator.SetBool("Shockwave", false);
    }

    private IEnumerator animationHandler(int num){
            if(num == 1){
                if (target.position.x > -3 && target.position.x < 3) { // Center
                    animator.SetBool("ConeCenter", true);
                } else if (target.position.x >= 3 && target.position.x < 9) { // Right
                    animator.SetBool("ConeRight", true);
                } else if (target.position.x > -9 && target.position.x <=-3) { // Left
                    animator.SetBool("ConeLeft", true);
                }
            } else if(num == 2){ 
                if (target.position.x > -3 && target.position.x < 3) { // Center
                    animator.SetBool("SlashCenter", true);
                } else if (target.position.x >= 3 && target.position.x < 9) { // Right
                    animator.SetBool("SlashRight", true);
                } else if (target.position.x > -9 && target.position.x <=-3) { // Left
                    animator.SetBool("SlashLeft", true);
                }
            } else if(num == 3){
                animator.SetBool("Shockwave", true);
            }
            yield return new WaitForSeconds(3.0f);
    }

    /*******************************************************/
    // Called at the beginnging of an animation's key frame to make sure the attack lines up with animation. 
    /*******************************************************/

    public void targetRight(){
        targetLastPos = "Right";
    }
    public void targetCenter(){
        targetLastPos = "Center";
    }
    public void targetLeft(){
        targetLastPos = "Left";
    }
    /*******************************************************/
}
