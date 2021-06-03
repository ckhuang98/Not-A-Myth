using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpProjectile : MonoBehaviour
{
    private float projectileSpeed = 10f;
    private Rigidbody2D rb;
    private Transform playerTarget;
    private Vector2 moveDirection;
    private Vector3 hold;

    private Transform hammerTarget;
    private GameObject[] hammerGiants;
    private GameObject[] swordGiants;
    private SpriteRenderer sr;
    private Vector2 healMoveDirection;
    private bool lookForHeal = true;
    // Start is called before the first frame update
    void Awake() {
        hammerGiants = GameObject.FindGameObjectsWithTag("Hammer Giant");
        swordGiants = GameObject.FindGameObjectsWithTag("Sword Giant");
        if (hammerGiants.Length <= 0 && swordGiants.Length > 0) {
            hammerTarget = swordGiants[0].transform;
        } else if (swordGiants.Length <= 0 && hammerGiants.Length > 0) {
            hammerTarget = hammerGiants[0].transform;
        } else if (swordGiants.Length > 0 && hammerGiants.Length > 0) {
            hammerTarget = hammerGiants[0].transform;
        } else if (swordGiants.Length <= 0 && hammerGiants.Length <= 0) {
            lookForHeal = false;
        }

        if (lookForHeal == true) {
            foreach (GameObject _hammerGiant in hammerGiants) {
                if (_hammerGiant != null) {
                    var checkingDistance = Vector3.Distance(transform.position, _hammerGiant.transform.position);
                    var targetDistance = Vector3.Distance(transform.position, hammerTarget.position);
                    if (checkingDistance < targetDistance)
                    {
                        hammerTarget = _hammerGiant.transform;
                    } 
                }
            }

            foreach (GameObject _swordGiant in swordGiants) {
                if (_swordGiant != null) {
                    var checkingDistance = Vector3.Distance(transform.position, _swordGiant.transform.position);
                    var targetDistance = Vector3.Distance(transform.position, hammerTarget.position);
                    if (checkingDistance < targetDistance)
                    {
                        hammerTarget = _swordGiant.transform;
                    } 
                }
            }
        }
        
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        hold = new Vector3(playerTarget.position.x, playerTarget.position.y, playerTarget.position.z);
        moveDirection = (hold - transform.position).normalized * projectileSpeed;
        
        //hammerTarget = hammerGiants[0].transform;
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hold);
        if (this.tag == "Healing Projectile" && lookForHeal == true) {
            sr.color = Color.green;

            if (hammerGiants.Length > 0 || swordGiants.Length > 0) {
                //transform.position = Vector2.MoveTowards(transform.position, hammerTarget.position, projectileSpeed * Time.deltaTime);
                healMoveDirection = (hammerTarget.transform.position - transform.position).normalized * projectileSpeed;
                rb.velocity = new Vector2(healMoveDirection.x, healMoveDirection.y);
            } 
        } else if (this.tag == "Imp Damage Projectile") {
            sr.color = Color.red;
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
            Destroy(this.gameObject, 3f);
        }
    }

    void SetType(string t) {
        this.tag = t;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player") && this.tag == "Imp Damage Projectile") {
            Destroy(this.gameObject);
        }

        if (col.CompareTag("Walls") && this.tag == "Imp Damage Projectile") {
            Destroy(this.gameObject);
        }

        if ((col.CompareTag("Hammer Giant") || col.CompareTag("Sword Giant")) && this.tag == "Healing Projectile") {
            Destroy(this.gameObject);
        }
    }
}
