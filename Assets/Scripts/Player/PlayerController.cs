using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.ExceptionServices;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using System;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    [SerializeField] private LayerMask dashLayerMask;
    [SerializeField] private InputActionAsset playerControls;

    public bool bossFight = false;

    private enum State{
        Normal,
        Dashing,
    }

    private State state;

    public float speed = 5.0f;
    public float dashSpeed;

    public Rigidbody2D rb;

    Vector2 movement;
    Vector3 attackDir;

    Vector3 moveDirection;
    Vector3 lastMoveDirection;
    Vector3 dashDirection;

    public Text attackPosition;

    public Animator slashAnimation;

    public Animator playerAnimator;

    float timer = 0f;
    private float fireStacks = 0;

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

    public bool attacked = false;
    public bool canDash = true;

    public GameObject slashCollider;

    public static bool gameOver = false;

    private GameObject restart;
    private GameObject boss;

    private ObjectAudioManager audioManager;
    private GameMaster gameMaster;
    
    Text gameOverText;

    private bool isInvincible = false;

    private SpriteRenderer playerSprite;

    // Start is called before the first frame update
    void Start() {
        //slashAnimation.enabled = false;
        gameMaster = GameMaster.instance;
        currentHealth = gameMaster.getPlayerHealthRecorded() ? gameMaster.getRecordedPlayerHealth() : maxHealth;
        healthBar.SetMaxValue(maxHealth);
        healthBar.SetValue(currentHealth);
        gameOverText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
        Debug.developerConsoleVisible = true;
        CombatManager.instance.canReceiveInput = true;
        state = State.Normal;
        restart = GameObject.FindWithTag("Restart");
        restart.SetActive(false);
        gameOver = false;
        audioManager = gameObject.GetComponent<ObjectAudioManager>();
        boss = GameObject.FindWithTag("Boss");
        if(boss != null){
            bossFight = true;
        }
        playerSprite = this.GetComponent<SpriteRenderer>();
        //slashCollider.GetComponent<Collider>().enabled = false;
        //part = GameObject.Find("Cone Firing").GetComponent<ParticleSystem>();
    }


    // Update is called once per frame
    void Update() {

        if (!gameOver) {

            // Stop character control when mousing over inventory
            // TODO: should stop player control, maybe pause the game, when inventory is open
            // if (EventSystem.current.IsPointerOverGameObject())
            //     return;
            switch (state){
                case State.Normal:

                    movementManager();
                    dashManager();

                    if (Input.GetMouseButtonDown(0)) {

                        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        //Debug.Log(mousePosition);
                        attackDir = (mousePosition - this.transform.position).normalized;
                        timer = 0;
                        //slashAnimation.enabled = true;
                        //Debug.Log(attackDir);
                        playerAnimator.SetFloat("attackDirX", attackDir.x);
                        playerAnimator.SetFloat("attackDirY", attackDir.y);
                        //attack(attackDir);
                    }
                    timer += Time.deltaTime;
                    if (timer >= .5) {
                        var colliders = slashAnimation.GetComponents<PolygonCollider2D>();
                        for (int i = 0; i < colliders.Length; i++) {
                            colliders[i].enabled = false;
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.X)){
                        Debug.Log("death");
                        currentHealth = 0;
                    }

                    hitDetection();
                    ApplyFire();
                    gameIsOver();
                    break;


                case State.Dashing:
                    handleDash();
                    break;
        }
        } else {
            //Stops player from moving after GameOver screen is displayed.
            movement.x = 0;
            movement.y = 0;

            //PUT IDLE ANIMATION HERE IF POSSIBLE
        }



        
    }

    void FixedUpdate() {
        switch(state){
            case State.Normal:
                rb.velocity = movement * speed;
                break;

            case State.Dashing:
                break;
        }
        

        // Dash (teleport) movement using raycast so player can't dash through walls.
        // if (isDashButtonDown && canDash){
        //     float dashAmount = 1f;
        //     Vector3 dashPosition = transform.position + lastMoveDirection * dashAmount;

        //     RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, lastMoveDirection, dashAmount, dashLayerMask);
        //     if(raycastHit2D.collider != null){
        //         dashPosition = raycastHit2D.point;
        //     }

        //     rb.MovePosition(dashPosition);
        //     isDashButtonDown = false;
        // }
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Debug.Log(collision.gameObject.layer);
    //     if(collision.gameObject.name == "Passable"){
    //         Debug.Log(state);
    //         if(state == State.Dashing){
    //             Physics.IgnoreLayerCollision(0, 4, true);
    //         } else{
    //             Physics.IgnoreLayerCollision(0, 4, false);
    //         }
    //     }
    // }

    private void dashManager(){
        if(Input.GetKeyDown(KeyCode.Space) && canDash){
            state = State.Dashing;
            Physics2D.IgnoreLayerCollision(9, 4, true);
            Debug.Log("Dash");
            dashSpeed = 20f;
        }
    }
    private void handleDash(){
        rb.velocity = lastMoveDirection * dashSpeed;
        dashSpeed -= dashSpeed * 5f * Time.deltaTime;
        if(dashSpeed < 2f){
            canDash = false;
            StartCoroutine(DashTimer());
            state = State.Normal;
            Physics2D.IgnoreLayerCollision(9, 4, false);
            Debug.Log("Normal");
        }
    }

    private IEnumerator DashTimer()
     {
         yield return new WaitForSeconds(0.7f);
         canDash = true;
     }

    public void gainStrength() {

        attackStrength += 0.2f;
        StartCoroutine(shardText());

    }

    private IEnumerator shardText(){
        gameOverText.text = "Damage Increased!";
        yield return new WaitForSeconds(1.5f);
        gameOverText.text = "";
    }

    public float whatIsStrength() {
        return attackStrength;
    }

    //restore current health
    public void restoreHealth(int restoreHealthBy = 10)
    {
        currentHealth = Math.Min(currentHealth + restoreHealthBy, maxHealth);
        healthBar.SetValue(currentHealth);
        StartCoroutine(plantText());
    }

    private IEnumerator plantText(){
        gameOverText.text = "Health Healed!";
        yield return new WaitForSeconds(1.5f);
        gameOverText.text = "";
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
            if(attacked){
                lastMoveDirection = attackDir;
                attacked = false;
            }
            playerAnimator.SetFloat("Horizontal", lastMoveDirection.x);
            playerAnimator.SetFloat("Vertical", lastMoveDirection.y);
        }
    }

    // Reduces player's current health and updates the slider value of the health bar.
    public void TakeDamage(int damage){
        playHurtSFX();
        if(!isInvincible){
            currentHealth -= damage;
            StartCoroutine(tempInvincible());
        }
        
        healthBar.SetValue(currentHealth);
    }

    private IEnumerator tempInvincible(){
        isInvincible = true;
        Color alpha = playerSprite.color;
        for(float i = 0; i < 1f; i += 0.2f){
            if(playerSprite.color.a == 0){
                alpha.a = 255;
                playerSprite.color = alpha;
            } else{
                alpha.a = 0;
                playerSprite.color = alpha;
            }
            yield return new WaitForSeconds(0.15f);
        }
        
        alpha.a = 255;
        playerSprite.color = alpha;
        isInvincible = false;
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

        if (withinAggroColliders != null) {
            fireStacks += 0.01f;
            if (healthTimer >= 1) {
                TakeDamage(10);
                healthTimer = 0;
            }
        }
    }

    void ApplyFire() {
        if (fireStacks > 0.0f) {
            if (0.0f <= fireStacks % 1.0f && fireStacks % 1.0f < 0.01f) {
                fireStacks -= 1.0f;
                TakeDamage(1);
            }
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
            gameOverText.text = "Game Over! You Lose!";
            gameOver = true;
            restart.SetActive(true);
        }
        if(bossFight){
            if(boss == null){
                gameOver = true;
                restart.SetActive(true);
            }
        }
        else if(!bossFight){
            if (Enemy.enemyAmount <= 0)
            {
                Debug.Log("Game Over");
                gameOverText.text = "You Win!";
                Color alpha = gameOverText.color;
                alpha.a = 255f;
                gameOverText.color = alpha;
                gameOver = true;
                restart.SetActive(true);
            }
        }
    }
    public void restartScene(){
        SceneManager.LoadScene(0);
    }

    private void playFootstepSFX()
    {
        audioManager.PlayRandomSoundInGroup("Footsteps");
    }

    private void playSlashSFX()
    {
        audioManager.PlayRandomSoundInGroup("Slashes");
    }

    private void playHurtSFX()
    {
        audioManager.PlayRandomSoundInGroup("Hurt");
    }

}
