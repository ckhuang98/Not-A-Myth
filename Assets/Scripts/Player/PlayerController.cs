using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using System;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class PlayerController : MonoBehaviour {

    [SerializeField] private LayerMask dashLayerMask;

    public bool bossFight = false;

    private enum State{
        Normal,
        Dashing,
    }

    private State state;

    public float dashSpeed;

    public Rigidbody2D rb;

    Vector2 movement;
    Vector3 attackDir;

    Vector3 lastMoveDirection;

    public Animator slashAnimation;

    public Animator playerAnimator;

    float timer = 0f;

    float healthTimer = 0f;

    private Collider2D withinAggroColliders;
    public float agroRange = 0;

    private int layerMask = 1 << 8;

    ////////////////////////////////////
    // Variables for fire damage.
    bool onFire = false;
    float fireAddTimer = 0f;
    float fireRemoveTimer = 0f;
    float fireDamageTimer = 0f;
    public int fireStacks = 0;
    public int maxFireStacks = 10;
    public Image fireAlert;
    ////////////////////////////////////

    ////////////////////////////////////
    // Variables for the health bar.
    public PlayerStats stats;
    public BarScript healthBar;
    public int maxHealth;
    public int currentHealth;
    ///////////////////////////////////

    ///////////////////////////////////
    public bool attacked = false;
    public bool canDash = true;
    public bool canDashTwice = false;
    private bool inAoE = false;

    ///////////////////////////////////

    public GameObject slashCollider;

    private PlayerAudioManager playerAudioManager;

    public bool isInvincible = false;

    bool beingKnockedback = false;

    private SpriteRenderer playerSprite;

    private GameMaster gameMaster;

    private Freezer freezer;

    [SerializeField]
    private GameObject dashBox;

    public GameObject magnet;

    public bool dashMeterFull = true;
    public bool dashMeterEmpty = false;

    public LookAt lookAt;

    public DashVFX dashVFX;

    public ParticleEffects particleEffects;
    // Start is called before the first frame update
    void Start() {
        //slashAnimation.enabled = false;
        gameMaster = GameMaster.instance;
        freezer = gameMaster.GetComponent<Freezer>();
        // currentHealth = maxHealth;
        // attackStrength = 1f;
        gameMaster.applyStats(true); //sets currentHealth, attackStrength, and Inventory
        Debug.developerConsoleVisible = true;

        if (stats == null) stats = ScriptableObject.CreateInstance("PlayerStats") as PlayerStats;

        GameMaster.instance.combatManager.canReceiveInput = true;
        state = State.Normal;
        playerAudioManager = gameObject.GetComponent<PlayerAudioManager>();
        playerSprite = this.GetComponent<SpriteRenderer>();

        healthBar = UI.instance.GetComponentInChildren<BarScript>();
        // healthBar.SetMaxValue(maxHealth);
        // healthBar.SetValue(currentHealth);

        //slashCollider.GetComponent<Collider>().enabled = false;
        //part = GameObject.Find("Cone Firing").GetComponent<ParticleSystem>();
        fireAlert = this.GetComponentInChildren<Image>();
        fireAlert.fillMethod = Image.FillMethod.Vertical;
        fireAlert.fillAmount = 0f;

        // Grabs the hitbox used for dash attack and disable it until player unlock the skill
        dashBox.SetActive(false);
    }   


    // Update is called once per frame
    void Update() {

        // test take damage
        if (Input.GetKeyDown(KeyCode.G))
		{
            GameMaster.instance.loadScene();
		}
        if (Input.GetKeyDown(KeyCode.H)){
            TakeDamage(25);
        }

        //magnet.transform.position = new Vector2(transform.position.x, transform.position.y);

        if (!gameMaster.getGameOver() && Time.timeScale == 1) {

            // Health Regen skill
            if(stats.inCombat.Value == false && stats.unlockedRegen.Value == true && stats.inBossFight.Value == false){
                StartCoroutine(regenHealth());
            }

            // Stop character control when mousing over inventory
            // TODO: should stop player control, maybe pause the game, when inventory is open
            // if (EventSystem.current.IsPointerOverGameObject())
            //     return;
            switch (state){
                case State.Normal:
                    // Dash attack Skill
                    if(stats.unlockedDashAttack.Value){
                        dashBox.SetActive(false);
                    }
                    movementManager();
                    dashManager();

                    if (Input.GetMouseButtonDown(0)) {
                        // if(!CombatManager.instance.canReceiveInput){
                        //     CombatManager.instance.InputManager();
                        // }
                        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        //Debug.Log(mousePosition);
                        attackDir = (mousePosition - this.transform.position);
                        timer = 0;
                        //slashAnimation.enabled = true;
                        playerAnimator.SetFloat("attackDirX", attackDir.x);
                        playerAnimator.SetFloat("attackDirY", attackDir.y);
                        //attack(attackDir);
                        GameMaster.instance.combatManager.Attack();
                    }
                    timer += Time.deltaTime;
                    if (timer >= .5) {
                        var colliders = slashAnimation.GetComponents<PolygonCollider2D>();
                        for (int i = 0; i < colliders.Length; i++) {
                            colliders[i].enabled = false;
                        }
                    }
                    // if (Input.GetKeyDown(KeyCode.L)){
                    //     Debug.Log("death");
                    //     currentHealth = 0;
                    // }

                    hitDetection();
                    handleFire();
                    checkIfPlayerIsDead();
                    break;


                case State.Dashing:
                    handleDash();
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
                        GameMaster.instance.combatManager.Attack();
                    }
                    // Dash attack Skill
                    if(stats.unlockedDashAttack.Value){
                        dashBox.SetActive(true);
                    }
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
                //Debug.Log(stats.speed.Value);
                if(stats.toggleMovement.Value){
                    rb.AddForce(movement.normalized * stats.speed.Value);
                } else{
                    if (inAoE == false && !beingKnockedback) {
                        rb.velocity = movement.normalized * stats.speed.Value;
                    } else if(!beingKnockedback){
                        rb.velocity = (movement.normalized * stats.speed.Value) * 0.5f;
                    }
                }
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

    private void dashManager(){
        if(stats.unlockedDoubleDash.Value){
           canDashTwice = true; 
        }
        if(Input.GetKeyDown(KeyCode.Space) && canDash){
            dashVFX.playDashVFX(lastMoveDirection.normalized);
            state = State.Dashing;
            Physics2D.IgnoreLayerCollision(9, 4, true);
            dashSpeed = stats.sprintSpeed.Value * 4;
        }

    }
    private void handleDash(){
        dashMeterEmpty = true;
        rb.velocity = lastMoveDirection.normalized * dashSpeed;
        
        
        dashSpeed -= dashSpeed * stats.maxSpeed.Value * Time.deltaTime;
        if(stats.unlockedDashMovement.Value){
            movementManager();
        }
        if(dashSpeed < 3f){
            canDash = false;
            StartCoroutine(DashTimer());
            state = State.Normal;
            Physics2D.IgnoreLayerCollision(9, 4, false);
        } else{
            if(stats.unlockedDoubleDash.Value && canDashTwice && Input.GetKeyDown(KeyCode.Space)){
                dashSpeed = stats.sprintSpeed.Value * 4f;
                rb.velocity = lastMoveDirection.normalized * dashSpeed;
                //dashVFX.playDashVFX(lastMoveDirection.normalized);
                dashSpeed -= dashSpeed * stats.maxSpeed.Value * Time.deltaTime;
                canDashTwice = false;
            }
        }
    }

    private IEnumerator DashTimer()
     {
        dashMeterEmpty = false;
        yield return new WaitForSeconds(stats.dashCooldown.Value);
        canDash = true;
     }



    public String getState(){
        if(state == State.Normal){
            return "Normal";
        } else if(state == State.Dashing){
            return "Dashing";
        }
        return "";
    }

    //restore current health
    public void restoreHealth(float restoreHealthBy)
    {
        stats.currentHealth.Value = Math.Min(stats.currentHealth.Value + restoreHealthBy, stats.maxHealth.Value);
        particleEffects.playHeal();
        // healthBar.SetValue(currentHealth);
        //StartCoroutine(UI.instance.displayerPlayerUpdate("Health Restored"));
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

    public void playSlashAnim1(){
        slashAnimation.Play("SlashAnim1", -1, 0f);
    }

    public void playSlashAnim2(){
        slashAnimation.Play("SlashAnim2", -1, 0f);
    }

    public void playSlashAnim3(){
        slashAnimation.Play("SlashAnim3", -1, 0f);
    }

    public void cameraLungeLeft(){
        stats.cameraOffsetX.Value -= 0.05f;
    }

    public void cameraLungeRight(){
        stats.cameraOffsetX.Value += 0.05f;
    }

    public void cameraLungeUp(){
        stats.cameraOffsetY.Value += 0.05f;
    }

    public void cameraLungeDown(){
        stats.cameraOffsetY.Value -= 0.05f;
        Debug.Log(stats.cameraOffsetY.Value);
    }

    public void cameraReset(){
        stats.cameraOffsetX.Value = 0.0f;
        stats.cameraOffsetY.Value = 0.5f;
    }

    // Reduces player's current health and updates the slider value of the health bar.
    public void TakeDamage(int damage){
        if(!isInvincible){
            playerAudioManager.playHurtSFX();
            stats.currentHealth.Value -= damage;
            CameraShaker.Instance.ShakeOnce(3f, 3f, 0.1f, 1f);
            StartCoroutine(tempInvincible());
            if(stats.unlockedHealthDash.Value){
                StartCoroutine(healthDashWindow(damage));
            }
            if(stats.unlockedAttackRegen.Value){
                StartCoroutine(attackRegenWindow(damage));
            }
            
        }
        
        // healthBar.SetValue(currentHealth);
    }

    // Separate from TakeDamage(), because fire damage should not
    // have a camera shake, or allow temporary invincibility 
    public void TakeFireDamage(int damage) {
        playerAudioManager.playSizzleSFX();
        stats.currentHealth.Value -= damage;
        // healthBar.SetValue(currentHealth);
    }


    // (Skill) Restores health if you dash right after getting hit by an attack. 
    private IEnumerator healthDashWindow(int damage){
        float tempTimer = 0;
        while(!Input.GetKeyDown(KeyCode.Space))
        {
            tempTimer += Time.deltaTime;
            yield return null;
        }
        if(tempTimer < 0.5f){
            stats.currentHealth.Value += damage;
        }
    }

    private IEnumerator attackRegenWindow(int damage){
        float tempTimer = 0;
        while(!stats.attackRegenHit.Value)
        {
            tempTimer += Time.deltaTime;
            yield return null;
        }
        if(tempTimer < 1f){
            stats.currentHealth.Value += damage;
        }
        stats.attackRegenHit.Value = false;
    }

    private IEnumerator regenHealth(){
        float threshold = stats.maxHealth.Value * 0.4f;
        Debug.Log(threshold);
        while(stats.inCombat.Value == false && stats.currentHealth.Value < threshold){
            if(stats.currentHealth.Value <= threshold - 2.0f){
                stats.currentHealth.Value += 0.001f;
            } else{
                stats.currentHealth.Value = threshold;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    // (Skill) Restores your health up to 40 percent when out of combat.
    

    // Co-routine that is called when the player is hit. Grants temporary invinciblility for 0.2 seconds. 
    private IEnumerator tempInvincible(){
        isInvincible = true;
        Color alpha = playerSprite.color;
        for(float i = 0; i < 1f; i += 0.2f){
            if(playerSprite.color.a == 0.5f){
                alpha.a = 255;
                playerSprite.color = alpha;
            } else{
                alpha.a = 0.5f;
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

            // If one of the colliders is an AoE circle (from a giant)
            if (withinAggroColliders.CompareTag("Fire Giant AoE")) {
                inAoE = true;
                /*
                fireAddTimer += Time.deltaTime;
                fireRemoveTimer = Mathf.Max(0f, fireRemoveTimer - Time.deltaTime);
            
                // If AoE is new (< 0.25 secs after creation), 10 dmg for hammer swing
                */
                var AoE = withinAggroColliders.GetComponent<AreaofEffectTime>();
                if (AoE.CanHit() && healthTimer >= 1) {
                    
                    TakeDamage(10);
                    healthTimer = 0;
                }
                /*
                // Increment fireStacks every 0.1 secs (onFire = T if stacks == 10)
                if (fireAddTimer >= 0.1f) {
                    if (fireStacks != maxFireStacks) { fireStacks += 1; }
                    if (fireStacks == maxFireStacks) { onFire = true; }
                    fireAddTimer = 0f;
                }
                */
            }
            

            if (withinAggroColliders.CompareTag("EnemySlash")) {
                fireAddTimer += Time.deltaTime;
                fireRemoveTimer = Mathf.Max(0f, fireRemoveTimer - Time.deltaTime);

                if (fireAddTimer >= 0.1f) {
                    if (fireStacks != maxFireStacks) { fireStacks = 10; }
                    if (fireStacks == maxFireStacks) { onFire = true; }
                    fireAddTimer = 0f;
                }
            }

            // // If player collides with boss slash attack, 15 damage
            if (withinAggroColliders.CompareTag("Boss Slash") && !isInvincible) { 
                TakeDamage(15);
                StartCoroutine(HammerKnockBack(GameMaster.instance.enemyKnockbackDuration, GameMaster.instance.enemyKnockbackPower, GameMaster.instance.boss.transform));
            }

            // // If the player collides with boss shockwave attack, 10 damage
            if (withinAggroColliders.CompareTag("Shockwave") && !isInvincible) { 
                TakeDamage(15); 
                StartCoroutine(HammerKnockBack(GameMaster.instance.enemyKnockbackDuration, GameMaster.instance.enemyKnockbackPower, GameMaster.instance.boss.transform));
            }

            if (withinAggroColliders.CompareTag("Fireball")) { if(!isInvincible) lookAt.updateTarget(withinAggroColliders.transform.position);TakeDamage(10); }

            if (withinAggroColliders.CompareTag("Eel Tendril")) { if(!isInvincible) lookAt.updateTarget(withinAggroColliders.transform.position);TakeDamage(10); }

            if (withinAggroColliders.CompareTag("EnemySlash")) { if(!isInvincible) lookAt.updateTarget(withinAggroColliders.transform.position);TakeDamage(10); }
            

            /* EXAMPLE for other types of damage

             // if one of the colliders is (something else) (from some other enemy)
             if (withinAggroColliders.CompareTag("Slowing Projectile")) {
                 TakeDamage(5);
                 Slow_Player(2);    // just an example function
            }
            */
            
        }

        // Note: This is likely not where this condition should be!
        //       I ran into problems attempting to put this condition
        //       in  (withinAggroColliders != null) condition.
        //
        // If not within an AoE circle, adjust timers, stacks, onFire
        else {
            //stats.speed.Value = 5f;
            inAoE = false;
            fireRemoveTimer += Time.deltaTime;
            fireAddTimer = Mathf.Max(0f, fireAddTimer - Time.deltaTime);
                    
            // If not in circle, decrement stacks (onFire = F if stacks == 0)
            if (fireRemoveTimer >= 0.25) {
                if (fireStacks != 0) { fireStacks -= 1; }
                if (fireStacks == 0) { onFire = false; }
                fireRemoveTimer = 0f;
            }
        }
        
    }

    // Purpose: To handle fire indicator above player
    void handleFire() {
        fireDamageTimer += Time.deltaTime;
        fireAlert.fillAmount = fireStacks / 10f;
        if (onFire) { ApplyFire(); }
    }

    // Purpose: damaging the player at a certain rate
    void ApplyFire() {
        if (fireDamageTimer >= 0.5f) {
            TakeFireDamage(1);
            fireDamageTimer = 0f;
        }
    }

    /*
    Purpose: Checks for whether the players' health is at 0 or if all enemies have been cleared out.
    A different message is displayed based on result. If either variable reaches 0 gameOver variable 
    becomes true. 
    Recieves: nothing.
    Returns: nothing.
    */
    void checkIfPlayerIsDead()
    {
        if (!gameMaster.getGameOver())
        {
            if (currentHealth <= 0)
            {
                gameMaster.setGameOver();
                return;
            }

            if (stats.currentHealth.Value <= 0)
            {
                gameMaster.setGameOver();
                return;
            }
        }
    }

    // private void playFootstepSFX()
    // {
    //     objectAudioManager.PlayRandomSoundInGroup("footsteps");
    // }

    // private void playSlashSFX()
    // {
    //     objectAudioManager.PlayRandomSoundInGroup("slashes");
    // }

    // private void playHurtSFX()
    // {
    //     objectAudioManager.PlayRandomSoundInGroup("hurt");
    // }

    // private void playSizzleSFX()
    // {
    //     objectAudioManager.PlayRandomSoundInGroup("sizzle");
    // }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Imp Damage Projectile")) {
            Vector3 temp = col.transform.position;
            lookAt.updateTarget(temp);
            TakeDamage(10);
        }
    }

    public IEnumerator HammerKnockBack(float duration, float power, Transform obj) {
        float timer = 0;
        beingKnockedback = true;
        while (timer < duration) {
            timer += Time.deltaTime;
            Vector2 direction = (obj.transform.position - this.transform.position).normalized;
            rb.AddForce(-direction * power);
            yield return null;
        }
        beingKnockedback = false;
        //yield return 0;
    }

    public IEnumerator HammerKnockBack(float power, Transform obj) {
        Vector2 direction = (obj.transform.position - this.transform.position).normalized;
        rb.AddForce(-direction * power);
        yield return 0;
    }
}
