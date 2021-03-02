using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Imp_Script : MonoBehaviour
{
    public float healthAmount;  
    public float timer = 0; 

    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private Transform target;

    private float timeBtwShots;
    public float startTimeBtwShots;

    public GameObject damageProjectile;
    public GameObject healingProjectile;

    // Start is called before the first frame update
    void Start()
    {
        healthAmount = 3f;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        timeBtwShots = startTimeBtwShots;
        
    }

    // Update is called once per frame
    void Update()
    {
        isDead(PlayerController.gameOver);

        if (Vector2.Distance(transform.position, target.position) > stoppingDistance) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        } else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance) {
            transform.position = this.transform.position;
        } else if (Vector2.Distance(transform.position, target.position) < retreatDistance) {
             transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        }

        if (timeBtwShots <= 0) {
            if (Random.value > 0.5) {
                Instantiate(healingProjectile, transform.position, Quaternion.identity);
            } else {
                Instantiate(damageProjectile, transform.position, Quaternion.identity);
            }
            
            timeBtwShots = startTimeBtwShots;
        } else {
            timeBtwShots -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Equals("SlashSpriteSheet_0") && timer >= .5)
        {
            //playHurtSFX();
            // Vector2 knockback = rb.transform.position - collider.transform.parent.position;
            // //Debug.Log(knockback);
            // rb.AddForce(knockback.normalized * 4000f);
            healthAmount -= collider.transform.parent.parent.GetComponent<PlayerController>().whatIsStrength();
            var thisColor = this.GetComponent<Renderer>().material.color;
            thisColor.a -= .1f;
            this.GetComponent<Renderer>().material.color = thisColor;

            timer = 0;
        }      
    }

    void isDead(bool gameOver){
        if (!gameOver) { 
            if (healthAmount <= 0)
            {
                //playDeathSFX();
                Destroy(this.gameObject);
                //enemyAmount -= 1;
                //spawnShard();
            }
            timer += Time.deltaTime; // Temporary
        }
    }
}
