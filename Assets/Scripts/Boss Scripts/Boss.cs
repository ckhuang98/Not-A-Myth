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
    public Rigidbody2D rb;
    public float healthAmount;

    public Transform target;
    public Transform bossTransform;
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

    private Freezer freezer;

    public bool attacking = false;
    public bool coneAttack = false;
    public bool goRight = true;
    public bool goLeft = false;

    [SerializeField] private float smoothSpeed = 10f;

    [SerializeField] private Vector3 offSet;

    Vector2 movement;
    Vector2 velocity = new Vector2(1.75f, 1.1f);

    void Awake() {
        fireCone.GetComponent<ParticleSystem>();
        fireCone.Pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthAmount = 50f;

        healthBar.SetMaxValue(healthAmount);

        //getting transform component from the Player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        stateMachine = new BossStateMachine();
        audioManager = gameObject.GetComponent<ObjectAudioManager>();
        InitializeStateMachine();
        freezer = GameMaster.instance.GetComponent<Freezer>();
        GameMaster.instance.playerStats.inBossFight.Value = true;
        
        if(fireCone.isPaused){
            Debug.Log("Paused Particle sys");
            fireCone.Pause();
        }

        movement = bossTransform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        // isDead(PlayerController.gameOver);
        stateMachine.Update();
        isDead(GameMaster.instance.getGameOver());
    }

    void FixedUpdate(){
        if(!attacking){
            // if(goRight && movement.x < 7.5f){
            //     movement.x += 0.05f;
            //     //rb.MovePosition(movement + velocity * Time.fixedDeltaTime);
            //     bossTransform.position = movement;
            // } else{
            //     goRight = false;
            //     goLeft = true;
            // }
            // if(goLeft && movement.x > -7.5f){
            //     movement.x -= 0.05f;
            //     //rb.MovePosition(movement + velocity * Time.fixedDeltaTime);
            //     bossTransform.position = movement;
            // } else{
            //     goRight = true;
            //     goLeft = false;
            // }
            Vector3  desiredPos = target.position + offSet;
            desiredPos.y = transform.position.y;
            transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        } else{
            StartCoroutine(stopMovement());
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
            freezer.Freeze();
            // Vector2 knockback = rb.transform.position - collider.transform.parent.position;
            // //Debug.Log(knockback);
            // rb.AddForce(knockback.normalized * 4000f);
            healthAmount -= GameMaster.instance.playerStats.attackPower.Value;
            audioManager.PlayRandomSoundInGroup("hurt");
            healthBar.SetValue(healthAmount);
            timer = 0;
        }
    }

    void isDead(bool gameOver){
        if (!gameOver) { 
            if (healthAmount <= 0)
            {
                Destroy(this.gameObject);
                GameMaster.instance.setGameOver(true);
            }
            timer += Time.deltaTime; // Temporary
        }
    }

    public void startAnimation(int num){
        StartCoroutine(animationHandler(num));
    }

    public void stopAnimation(){
        // animator.SetBool("SlashCenter", false);
        // animator.SetBool("SlashRight", false);
        // animator.SetBool("SlashLeft", false);
        // animator.SetBool("ConeCenter", false);
        // animator.SetBool("ConeRight", false);
        // animator.SetBool("ConeLeft", false);
        // animator.SetBool("Shockwave", false);
    }

    private IEnumerator animationHandler(int num){
            if(num == 1){
                if (target.position.x > -3 && target.position.x < 3) { // Center
                    animator.SetTrigger("ConeCenter");
                } else if (target.position.x >= 3 && target.position.x < 9) { // Right
                    animator.SetTrigger("ConeRight");
                } else if (target.position.x > -9 && target.position.x <=-3) { // Left
                    animator.SetTrigger("ConeLeft");
                }
            } else if(num == 2){ 
                if (target.position.x > -3 && target.position.x < 3) { // Center
                    animator.SetTrigger("SlashCenter");
                } else if (target.position.x >= 3 && target.position.x < 9) { // Right
                    animator.SetTrigger("SlashRight");
                } else if (target.position.x > -9 && target.position.x <=-3) { // Left
                    animator.SetTrigger("SlashLeft");
                }
            } else if(num == 3){
                animator.SetTrigger("Shockwave");
            }
            yield return new WaitForSeconds(3.0f);
    }

    public IEnumerator stopMovement(){
        attacking = true;
        if(coneAttack == false){
            yield return new WaitForSeconds(1.5f);
        } else{
            yield return new WaitForSeconds(2.5f);
            coneAttack = false;
        }
        
        attacking = false;
    }

    /*******************************************************/
    // Called at the beginnging of an animation's key frame to make sure the attack lines up with animation. 
    /*******************************************************/

    public void targetRight(){
        //targetLastPos = "Right";
    }
    public void targetCenter(){
        targetLastPos = "Center";
    }
    public void targetLeft(){
        //targetLastPos = "Left";
    }
    /*******************************************************/
}
