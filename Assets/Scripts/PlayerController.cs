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
    public float speed = 5.0f;

    public Rigidbody2D rb;

    Vector2 movement;
    Vector2 lastMoveDirection;

    public Text attackPosition;

    public Animator attackAnimation;

    public Animator playerAnimator;

    float timer = 0f;
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

    public GameObject slashCollider;

    public bool gameOver = false;
    Text gameOverText;
    // Start is called before the first frame update
    void Start() {
        
        currentHealth = maxHealth; //starting health is max, which can be changed in the editor 
        healthBar.SetMaxValue(maxHealth);
        gameOverText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
        
    }


    // Update is called once per frame
    void Update() {

        if (!gameOver) { 

            //handles movement and the animations associated with it
            movementManager();

            //if the player clicks, attack
            attackInDirection();

            //Purpose: Checks to see if a player collides with one of the player.If so the function
            //TakeDamage() is called and players health is reduced by 10 per second.
            hitDetection();

            //checks if the game is over (ie the player has won or lost)
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
    }

    

    public void gainStrength() {

        attackStrength += .25f;

    }

    public void gainHealth(){
        currentHealth += 20;
        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
        healthBar.SetValue(currentHealth);
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

    //plays the attack animation when the player clicks. A separate script (ColliderHandler)
    //handles turning the colliders on for each frame of the animation. After .5 seconds
    //this fucntion will disable all of the colliders. 
    void attackInDirection() {
        if (Input.GetMouseButtonDown(0)) { 
            timer = 0;
            attackAnimation.enabled = true;
            attackAnimation.Play("Attacking", -1, 0f);
        }

        timer += Time.deltaTime;
        if (timer >= .5) {
            var colliders = attackAnimation.GetComponents<PolygonCollider2D>();
            for (int i = 0; i < colliders.Length; i++) {
                colliders[i].enabled = false;
            }
        }

    }



}
