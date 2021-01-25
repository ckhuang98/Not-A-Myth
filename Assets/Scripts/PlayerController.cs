using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Video;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using System;
using TMPro;

public class PlayerController : MonoBehaviour {

    [SerializeField] private LayerMask dashLayerMask;

    public float speed = 5.0f;

    public Rigidbody2D rb;

    Vector2 movement;
    Vector3 attackDir;

    Vector3 moveDirection;
    Vector3 lastMoveDirection;

    public Text attackPosition;

    public Animator attackAnimation;

    public Animator playerAnimator;

    float timer = 0f;
    public int numOfClicks = 0;
    float lastClickedTime = 0f;
    float maxComboDelay = 0.9f;
    bool endOfCombo = false;

    float healthTimer = 0f;
    public float attackStrength = 1f;

    private Collider2D withinAggroColliders;
    public float agroRange = 0;

    private int layerMask = 1 << 8;

    ////////////////////////////////////
    // Variables for the health bar.
    public BarScript healthBar;
    public int maxHealth;
    public int currentHealth;
    ///////////////////////////////////

    public bool isDashButtonDown = false;

    public GameObject slashCollider;

    public static bool gameOver = false;
    Text gameOverText;
    // Start is called before the first frame update
    void Start() {
        //attackAnimation.enabled = false;
        currentHealth = maxHealth;
        healthBar.SetMaxValue(maxHealth);
        gameOverText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
        Debug.developerConsoleVisible = true;
        //slashCollider.GetComponent<Collider>().enabled = false;
    }


    // Update is called once per frame
    void Update() {

        if (!gameOver) { 

            movementManager();
            if(Time.time - lastClickedTime > maxComboDelay){
                numOfClicks = 0;
            }
            if (Input.GetMouseButtonDown(0)) {
                speed = 0;
                lastClickedTime = Time.time;
                numOfClicks++;
                
                numOfClicks = Mathf.Clamp(numOfClicks, 0, 3);


                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Debug.Log(mousePosition);
                attackDir = (mousePosition - this.transform.position).normalized;
                timer = 0;
                attackAnimation.enabled = true;
                //Debug.Log(attackDir);
                attack(attackDir);
            }
            if(Input.GetKeyDown(KeyCode.Space)){
                isDashButtonDown = true;
            }
            timer += Time.deltaTime;
            if (timer >= .5) {
                speed = 5;
                var colliders = attackAnimation.GetComponents<PolygonCollider2D>();
                for (int i = 0; i < colliders.Length; i++) {
                    colliders[i].enabled = false;
                }
                attackAnimation.SetBool("LeftMouseDown", false);

                playerAnimator.SetBool("Attack1", false);
                playerAnimator.SetBool("Attack2", false);
                playerAnimator.SetBool("Attack3", false);
            }

            hitDetection();

            gameIsOver();
        } else {
            //Stops player from moving after GameOver screen is displayed.
            movement.x = 0;
            movement.y = 0;

            //PUT IDLE ANIMATION HERE IF POSSIBLE
        }



        
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        // Dash movement using raycast so player can't dash through walls.
        if (isDashButtonDown){
            float dashAmount = 1f;
            Vector3 dashPosition = transform.position + lastMoveDirection * dashAmount;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, lastMoveDirection, dashAmount, dashLayerMask);
            if(raycastHit2D.collider != null){
                dashPosition = raycastHit2D.point;
            }

            rb.MovePosition(dashPosition);
            isDashButtonDown = false;
        }
    }

    void attack(Vector3 attackDir) {
        Debug.Log(attackDir);
        playerAnimator.SetFloat("attackDirX", attackDir.x);
        playerAnimator.SetFloat("attackDirY", attackDir.y);
        if(numOfClicks == 1){
            playerAnimator.SetBool("Attack1", true);
        } else if(numOfClicks == 2){
            playerAnimator.SetBool("Attack2", true);
            // Debug.Log("Combo2!");
        } else if(numOfClicks != 2 && numOfClicks < 2){
            playerAnimator.SetBool("Attack1", false);
            numOfClicks = 0;
        } else if(endOfCombo){
            // Debug.Log("Combo Finised!");
            playerAnimator.SetBool("Attack1", false);
            playerAnimator.SetBool("Attack2", false);
            playerAnimator.SetBool("Attack3", false);
            numOfClicks = 0;
            endOfCombo = false;
        } else if(numOfClicks >= 3){
            playerAnimator.SetBool("Attack3", true);
            // Debug.Log("Combo3!");
            endOfCombo = true;
        } else if(numOfClicks != 3 && numOfClicks < 3){
            playerAnimator.SetBool("Attack2", false);
            numOfClicks = 0;
        } 


        var temp = attackDir;
        //slashCollider.GetComponent<Collider>().enabled = true;
        //attackPosition.transform.position = Input.mousePosition;
        //attackPosition.text = "" + attackDir;
        //attackAnimation.Play("Attacking", -1, 0f);
        attackAnimation.SetBool("LeftMouseDown", true);
    }

    public void gainStrength() {

        attackStrength += 1;

    }

    public float whatIsStrength() {
        return attackStrength;
    }

    // Handles the player movements and animations.
    void movementManager(){
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Updates the horizontal and vertical variable 
        // within the player animator to determine what animations plays
        playerAnimator.SetFloat("Horizontal", movement.x);
        playerAnimator.SetFloat("Vertical", movement.y);

        // Keeps track of the last direction the player moved to 
        // update the correct idle animation.
        if(movement.x != 0 || movement.y !=0){
            lastMoveDirection = movement;
        }

        // Updates the speed variable in animator to determine whether
        // idle or running animations should be playing.
        playerAnimator.SetFloat("Speed", movement.sqrMagnitude);
        
        // When switching back to idle, update the animator's 
        // Horizontal and Vertical variables with the lastMoveDirection
        // to play the correct animations
        if(movement.sqrMagnitude <= 0.01f){
            playerAnimator.SetFloat("Horizontal", lastMoveDirection.x);
            playerAnimator.SetFloat("Vertical", lastMoveDirection.y);
        }
    }

    // Reduces player's current health and updates the slider value of the health bar.
    void TakeDamage(int damage){
        currentHealth -= damage;
        healthBar.SetValue(currentHealth);
    }

    /*
    Purpose: Checks to see if a player collides with one of the player. If so the function
    TakeDamage() is called and players health is reduced by 10 per second.
    Recieves: nothing.
    Returns: nothing.
    */
    void hitDetection()
    {
        withinAggroColliders = Physics2D.OverlapBox(transform.position, transform.localScale, 1f, layerMask);
        healthTimer += Time.deltaTime;

        //Tracks whether the enmemy has collided with the player. After initial
        //detection it will take damage from player per second
        if (withinAggroColliders != null && healthTimer >= 1)
        {
            TakeDamage(10);
            healthTimer = 0;
        }
    }

    /*
    Purpose: Checks for whether the players' health is at 0 or if all enemies have been cleared out.
    A different message is displayed based on result. If either variable reaches 0 gameOver variable 
    becomes true. 
    Recieves: nothing.
    Returns: nothing.
    */
    void gameIsOver()
    {
        if (currentHealth <= 0)
        {
            Color alpha = gameOverText.color;
            alpha.a = 255f;
            gameOverText.color = alpha;
            gameOver = true;

        }

        if (Enemy.enemyAmount <= 0)
        {
            gameOverText.text = "You Win!";
            Color alpha = gameOverText.color;
            alpha.a = 255f;
            gameOverText.color = alpha;
            gameOver = true;
        }
    }
}
