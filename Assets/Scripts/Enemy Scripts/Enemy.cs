using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;
using System.Linq;
using EZCameraShake;

public class Enemy : MonoBehaviour
{
    //Variables regarding enemy stats
    Rigidbody2D rb;
    public float healthAmount;
    public float armorAmount;
    //GameObject armorBorderObject;
    public const float maxArmorAmount = 3f;
    private float alpha;
    private float originalY;
    public float floatStrength = 1;

    public float timer = 0;

    public GameObject shard;
    //Area of Effect
    public GameObject AOE;
    public GameObject fireParticle;
    public GameObject damageProjectile;
    public GameObject healingProjectile;
    public GameObject fireTrail;
    public GameObject slash;
    
    //Target is the players' current location
    private Transform target;

    public bool inBounds = false;
    public bool hasCircled = false;

    private GameObject[] hammerGiantList;
    private GameObject[] fireImpList;
    private GameObject[] fireEelList;
    private GameObject[] swordGiantList;
    public static int enemyAmount;
    public Animator enemyAnimator;
    ////////////////////////////////

    StateMachine stateMachine; 

    public float _rayDistance = 5.0f;
    private int layerMask = 1 << 20;
    public RaycastHit2D[] castList = new RaycastHit2D[8];
    public int[] weightList = new int[8];
    internal int currMoveDirection;
    public bool doInstantiate = false;
    public bool goToWalk = false;
    public bool doAttack = false;
    public bool doLungeAttack = false;
    public bool beenHit = false;
    public bool inAttackState = false;

    [SerializeField]
    private GameObject deathSFXObject;

    private Freezer freezer;
    private ObjectAudioManager audioManager;
    
    internal Vector3[] moveDirections = new Vector3[] { Vector3.up, Vector3.Normalize(Vector3.right + Vector3.up), 
        Vector3.right, Vector3.Normalize(Vector3.right + Vector3.down), Vector3.down,
        Vector3.Normalize(Vector3.left + Vector3.down), Vector3.left, Vector3.Normalize(Vector3.left + Vector3.up) };

    // Start is called before the first frame update
    void Start()
    {
        if (this.tag == "Fire Eel") {
            healthAmount = 1f;
        } else if (this.tag == "Fire Imp") {
            healthAmount = .8f;
        } else if (this.tag == "Hammer Giant" || this.tag == "Sword Giant") {
            healthAmount = 1.5f;
        }

        if (this.tag == "Fire Imp") {
            originalY = transform.position.y;
        }
        
        armorAmount = 0f;
        rb = GetComponent<Rigidbody2D>();
        freezer = GameMaster.instance.GetComponent<Freezer>();
        //getting transform component from the Player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        hammerGiantList = GameObject.FindGameObjectsWithTag("Hammer Giant");
        fireImpList = GameObject.FindGameObjectsWithTag("Fire Imp");
        fireEelList = GameObject.FindGameObjectsWithTag("Fire Eel");
        swordGiantList = GameObject.FindGameObjectsWithTag("Sword Giant");

        GameMaster.instance.enemyList.AddRange(hammerGiantList);
        GameMaster.instance.enemyList.AddRange(fireImpList);
        GameMaster.instance.enemyList.AddRange(fireEelList);
        GameMaster.instance.enemyList.AddRange(swordGiantList);

        enemyAmount = hammerGiantList.Length + fireImpList.Length + fireEelList.Length + swordGiantList.Length;
        GameMaster.instance.numOfEnemies = enemyAmount;
        for (int i = 0; i < moveDirections.Count(); i ++) {
            weightList[i] = 0;
        }
        stateMachine = new StateMachine();

        //TODO: Make it so that each individual enemy type has a different type of audio manager for ease of use. Should probably have different scripts for the different enemy types
        if (this.tag == "Fire Eel") {
            audioManager = gameObject.GetComponent<ObjectAudioManager>();
        } else if (this.tag == "Fire Imp") {
            audioManager = gameObject.GetComponent<ObjectAudioManager>();
        } else if (this.tag == "Hammer Giant") {
            audioManager = gameObject.GetComponent<ObjectAudioManager>();
        } else if (this.tag == "Sword Giant") {
            audioManager = gameObject.GetComponent<ObjectAudioManager>();
        }

        InitializeStateMachine();
        alpha = this.GetComponent<Renderer>().material.color.a;
    }

    // Update is called once per frame
    void Update() {
	    if (this.tag == "Fire Imp") { 
            DoFloat();
        }
        //enemyAnimator.SetFloat("Speed", moveDirections[currMoveDirection].sqrMagnitude);
        isDead(GameMaster.instance.getGameOver());
        stateMachine.Update();
        DisplayRays();

        if (armorAmount > 1.5) {
            armorAmount = 1.5f;
        } else if (armorAmount < 0) {
            armorAmount = 0;
        }
        alpha = .2f;
        
    }


    /*
    Purpose: Collision detection for enemy damage or adding armor to the hammer giants.
    Recieves: a Collider2D which is the game object that the enemy came in contact with.
    Returns: nothing.
    */
    void OnTriggerEnter2D(Collider2D collider)
    {
        //Applies armor to the Hammer Giant
        if (collider.CompareTag("Healing Projectile") && armorAmount < 3 && (this.tag == "Hammer Giant" || this.tag == "Sword Giant")) {
            armorAmount += .5f;
            var thisColor = this.GetComponent<Renderer>().material.color;
            if (thisColor.a < 1f && thisColor.a > 0f) {
                thisColor.a += .2f;
                this.GetComponent<Renderer>().material.color = thisColor;
                if (thisColor.a > 1f) {
                    thisColor.a = 1f;
                    this.GetComponent<Renderer>().material.color = thisColor;
                }
            }            
        }
        if (collider.gameObject.name.Equals("SlashSpriteSheet_0") && timer >= .5)
        {
            if (this.tag == "Hammer Giant" && inAttackState == false) {
                enemyAnimator.SetTrigger("HammerHit");
                beenHit = true;
            } else if (this.tag == "Fire Imp" && inAttackState == false) {
                enemyAnimator.SetTrigger("ImpHit");
                beenHit = true;
            } else if (this.tag == "Fire Eel" && inAttackState == false) {
                enemyAnimator.SetTrigger("EelHit");
                beenHit = true;
            } 
            playHurtSFX();
            ////////////////////////////////////////////////////////////////////////////////////
            // Adds knockback to enemy
            // Added by Cary 04/01/21
                Vector2 knockback = rb.transform.position - collider.transform.parent.position;
                rb.AddForce(knockback.normalized * GameMaster.instance.playerStats.knockBackForce.Value);
            ////////////////////////////////////////////////////////////////////////////////////
            if (armorAmount > 0) {
                armorAmount -= (GameMaster.instance.playerStats.attackPower.Value * .3f);
            } else {
                if (this.tag == "Fire Eel") {
                    healthAmount -= (GameMaster.instance.playerStats.attackPower.Value * .01f);
                } else if (this.tag == "Fire Imp") {
                    healthAmount -= (GameMaster.instance.playerStats.attackPower.Value * 0.4f);
                } else if (this.tag == "Hammer Giant" || this.tag == "Sword Giant") {
                    healthAmount -= (GameMaster.instance.playerStats.attackPower.Value * .3f);
                }
                
            }

            GameMaster.instance.playerStats.attackRegenHit.Value = true;
            
            var thisColor = this.GetComponent<Renderer>().material.color;
            if (thisColor.a < 1f && thisColor.a > 0f) {
                thisColor.a -= .1f;
                this.GetComponent<Renderer>().material.color = thisColor;
                Debug.Log(thisColor.a);
            }
            

            timer = 0;
            CameraShaker.Instance.ShakeOnce(2f, 2f, 0.1f, 1f);
            freezer.Freeze();
        }
        // For the dash attack skill
        if(collider.gameObject.name.Equals("DashBox") && timer >= .5){
            playHurtSFX();
            if (armorAmount > 0) {
                armorAmount -= (GameMaster.instance.playerStats.attackPower.Value * .3f);
            } else {
                if (this.tag == "Fire Eel") {
                    healthAmount -= (GameMaster.instance.playerStats.attackPower.Value * .01f);
                } else if (this.tag == "Fire Imp") {
                    healthAmount -= (GameMaster.instance.playerStats.attackPower.Value * 0.4f);
                } else if (this.tag == "Hammer Giant" || this.tag == "Sword Giant") {
                    healthAmount -= (GameMaster.instance.playerStats.attackPower.Value * .3f);
                }
                
            }
            var thisColor = this.GetComponent<Renderer>().material.color;
            if (thisColor.a < 1f && thisColor.a > 0f) {
                thisColor.a -= .1f;
                this.GetComponent<Renderer>().material.color = thisColor;
                Debug.Log(thisColor.a);
            }
            

            timer = 0;
            CameraShaker.Instance.ShakeOnce(2f, 1.5f, 0.1f, 1f);
            freezer.Freeze();
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
            if (healthAmount <= 0)
            {
                playDeathSFX();
                GameMaster.instance.enemyList.Remove(this.gameObject);
                Destroy(this.gameObject);
                enemyAmount -= 1;
                GameMaster.instance.numOfEnemies -= 1;
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
            // { typeof(CircleState), new CircleState(this) },
            { typeof(AttackState), new AttackState(this) },
            { typeof(MaintainDistanceState), new MaintainDistanceState(this) },
            { typeof(FireProjectileState), new FireProjectileState(this) },
            { typeof(LungeAttackState), new LungeAttackState(this) },
            { typeof(EelMaintainDistanceState), new EelMaintainDistanceState(this) },
            { typeof(SwordAttackState), new SwordAttackState(this) }
        };

        stateMachine.SetStates(states);
    }

    /*
    Purpose: Displays rays to visually represent how far the enemies see for wall and NPC detection.
    Recieves: nothing:
    returns: nothing.
    */
    private void DisplayRays()
    {
        for (int i = 0; i < moveDirections.Count(); i ++) {
            var rayColor = Color.green;
            Debug.DrawRay(transform.position, moveDirections[i] * 3.0f, rayColor);
            castList[i] = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), 
                new Vector2(moveDirections[i].x, moveDirections[i].y), 3.0f, layerMask);
        }
        for (int i = 0; i < moveDirections.Count(); i ++) {
            if (castList[i].collider != null) {
                var rayColor = Color.red;
                Debug.DrawRay(transform.position, moveDirections[i] * 3.0f, rayColor);
            }
        }
    }

    // Turns all weights to 0 as optional directions to walk
    public void resetWeightsToZero() {
        for (int i = 0; i < moveDirections.Count(); i ++) {
            weightList[i] = 0;
        }
    }

    //Tell attack state to place AOE
    public void AreaOfEffect() {
        //Debug.Log("Here");
        doInstantiate = true;
    }

    public void ProjectileThrow() {
        doInstantiate = true;
    }

    public void doSlash() {
        doInstantiate = true;
    }

    // Move to walk animations based on enemy tags
    public void moveToWalk () {
        if (this.tag == "Hammer Giant") {
            enemyAnimator.SetTrigger("Walking");
        } else if (this.tag == "Fire Eel") {
            enemyAnimator.SetTrigger("FireEelWalking");
            doLungeAttack = false;
        } else if (this.tag == "Fire Imp") {
            enemyAnimator.SetTrigger("ImpIdle");
        } else if (this.tag == "Sword Giant") {
            enemyAnimator.SetTrigger("SwordWalking");
        }
        goToWalk = true;
    }

    public void HitToWalk () {
        beenHit = false;
        if (this.tag == "Hammer Giant") {
            enemyAnimator.SetTrigger("Walking");
        } else if (this.tag == "Fire Imp") {
            enemyAnimator.SetTrigger("ImpIdle");
        } else if (this.tag == "Fire Eel") {
            enemyAnimator.SetTrigger("FireEelWalking");
        }
    }

    private void DoFloat() {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Math.Sin(Time.time) * floatStrength),
            transform.position.z);
    }

    //Tell lungeattackstate to do the lunge attack
    public void LungeAttackActivation() {
        doLungeAttack = true;
    }

    private void playAttackSFX(){
        audioManager.PlayRandomSoundInGroup("attacks");
    }

    private void playHammerImpactSFX()
    {
        audioManager.PlayRandomSoundInGroup("hammerImpacts");
    }

    private void playHammerSwingSFX()
    {
        audioManager.PlayRandomSoundInGroup("hammerSwings");
    }

    private void playSwordSwingSFX(){
        audioManager.PlayRandomSoundInGroup("swordSwings");
    }

    private void playFootstepSFX()
    {
        audioManager.PlayRandomSoundInGroup("footsteps");
    }

    private void playHurtSFX()
    {
        audioManager.PlayRandomSoundInGroup("hurt");
    }

    private void playGruntSFX()
    {
        audioManager.PlayRandomSoundInGroup("grunts");
    }

    private void playCrackSFX(){
        audioManager.PlayRandomSoundInGroup("cracks");
    }

    private void playDeathSFX()
    {
        // return gameObject.GetComponent<ObjectAudioManager>().PlayRandomSoundInGroup("Death");

        Vector3 pos = this.gameObject.transform.position;
        GameObject soundSource = Instantiate(deathSFXObject, pos, Quaternion.identity);
        Sound sound = soundSource.GetComponent<ObjectAudioManager>().PlayRandomSoundInGroup("death");
        Destroy(soundSource, sound.source.clip.length);

    }
}
