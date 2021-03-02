using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;
using System.Linq;

public class Enemy : MonoBehaviour
{
    //Variables regarding enemy stats
    Rigidbody2D rb;
    public float healthAmount;
    public float armorAmount;
    //GameObject armorBorderObject;
    public const float maxArmorAmount = 3f;
    private float speed = 1;
    private float alpha;

    public float timer = 0;

    public GameObject shard;
    //Area of Effect
    public GameObject AOE;
    public GameObject fireParticle;
    public GameObject damageProjectile;
    public GameObject healingProjectile;
    
    //Target is the players' current location
    private Transform target;
    public bool inBounds = false;
    public bool hasCircled = false;

    private GameObject[] hammerGiantList;
    private GameObject[] fireImpList;
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
    
    //An array carrying all 8 movement options for the enemy
    /*
    internal Vector3[] moveDirections = new Vector3[] { Vector3.right, Vector3.left, Vector3.up, Vector3.down, 
        Vector3.Normalize(Vector3.left + Vector3.up), Vector3.Normalize(Vector3.left + Vector3.down),
        Vector3.Normalize(Vector3.right + Vector3.up), Vector3.Normalize(Vector3.right + Vector3.down) };
    */

    [SerializeField]
    private GameObject deathSFXObject;

    private ObjectAudioManager audioManager;
    
    internal Vector3[] moveDirections = new Vector3[] { Vector3.up, Vector3.Normalize(Vector3.right + Vector3.up), 
        Vector3.right, Vector3.Normalize(Vector3.right + Vector3.down), Vector3.down,
        Vector3.Normalize(Vector3.left + Vector3.down), Vector3.left, Vector3.Normalize(Vector3.left + Vector3.up) };

    // Start is called before the first frame update
    void Start()
    {
        if (this.tag == "Hammer Giant") {
            healthAmount = 3f;
        } else if (this.tag == "Fire Imp") {
            healthAmount = 25f;
        }
        
        armorAmount = 0f;
        rb = GetComponent<Rigidbody2D>();

        //getting transform component from the Player
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        hammerGiantList = GameObject.FindGameObjectsWithTag("Hammer Giant");
        fireImpList = GameObject.FindGameObjectsWithTag("Fire Imp");
        enemyAmount = hammerGiantList.Length + fireImpList.Length;   
        for (int i = 0; i < moveDirections.Count(); i ++) {
            weightList[i] = 0;
        }
        stateMachine = new StateMachine();
        audioManager = gameObject.GetComponent<ObjectAudioManager>();
        InitializeStateMachine();
        alpha = this.GetComponent<Renderer>().material.color.a;
        /*
        if (this.tag == "Hammer Giant") {
            armorBorderObject = transform.GetChild (4).gameObject;
        } 
        */
    }

    // Update is called once per frame
    void Update() {
        enemyAnimator.SetFloat("Speed", moveDirections[currMoveDirection].sqrMagnitude);
        isDead(PlayerController.gameOver);
        stateMachine.Update();
        DisplayRays();
        /*
        if (this.tag == "Hammer Giant") {
            if (armorAmount <= 0) {
                armorBorderObject.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            } else if (armorAmount > 0) {
                armorBorderObject.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        */
        if (armorAmount > 3) {
            armorAmount = 3;
        } else if (armorAmount < 0) {
            armorAmount = 0;
        }
        alpha = .2f;
        
    }


    // Deal damage to player on contact
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Healing Projectile") && armorAmount < 3 && this.tag == "Hammer Giant") {
            armorAmount += 1;
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
            //playHurtSFX();
            // Vector2 knockback = rb.transform.position - collider.transform.parent.position;
            // //Debug.Log(knockback);
            // rb.AddForce(knockback.normalized * 4000f);

            if (armorAmount > 0) {
                armorAmount -= (collider.transform.parent.parent.GetComponent<PlayerController>().whatIsStrength() * .75f);
            } else {
                if (this.tag == "Hammer Giant") {
                    healthAmount -= collider.transform.parent.parent.GetComponent<PlayerController>().whatIsStrength();
                } else if (this.tag == "Fire Imp") {
                    healthAmount -= (collider.transform.parent.parent.GetComponent<PlayerController>().whatIsStrength() * 8.4f);
                }
                
            }
            
            var thisColor = this.GetComponent<Renderer>().material.color;
            if (thisColor.a < 1f && thisColor.a > 0f) {
                thisColor.a -= .1f;
                this.GetComponent<Renderer>().material.color = thisColor;
                Debug.Log(thisColor.a);
            }
            

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
            if (healthAmount <= 0)
            {
                //playDeathSFX();
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
            { typeof(CircleState), new CircleState(this) },
            { typeof(AttackState), new AttackState(this) },
            { typeof(MaintainDistanceState), new MaintainDistanceState(this) },
            { typeof(FireProjectileState), new FireProjectileState(this) }
        };

        stateMachine.SetStates(states);
    }

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

    public void resetWeightsToZero() {
        for (int i = 0; i < moveDirections.Count(); i ++) {
            weightList[i] = 0;
        }
    }

    public void AreaOfEffect() {
        //Debug.Log("Here");
        doInstantiate = true;
    }

    public void moveToWalk () {
        enemyAnimator.SetTrigger("Walking");
        goToWalk = true;
    }

    private void playHammerImpactSFX()
    {
        audioManager.PlayRandomSoundInGroup("Hammer Impact");
    }

    private void playHammerSwingSFX()
    {
        audioManager.PlayRandomSoundInGroup("Hammer Swing");
    }

    private void playFootstepSFX()
    {
        audioManager.PlayRandomSoundInGroup("Footsteps");
    }

    private void playHurtSFX()
    {
        audioManager.PlayRandomSoundInGroup("Hurt");
    }

    private void playDeathSFX()
    {
        // return gameObject.GetComponent<ObjectAudioManager>().PlayRandomSoundInGroup("Death");

        Vector3 pos = this.gameObject.transform.position;
        GameObject soundSource = Instantiate(deathSFXObject, pos, Quaternion.identity);
        Sound sound = soundSource.GetComponent<ObjectAudioManager>().PlayRandomSoundInGroup("Death");
        Destroy(soundSource, sound.source.clip.length);

    }
}
