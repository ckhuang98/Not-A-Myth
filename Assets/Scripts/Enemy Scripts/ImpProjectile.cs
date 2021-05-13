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
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        hold = new Vector3(playerTarget.position.x, playerTarget.position.y, playerTarget.position.z);
        moveDirection = (hold - transform.position).normalized * projectileSpeed;

        hammerGiants = GameObject.FindGameObjectsWithTag("Hammer Giant");
        hammerTarget = hammerGiants[0].transform;
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hold);
        if (this.tag == "Healing Projectile") {
            sr.color = Color.green;
            if (hammerGiants.Length > 0) {
                transform.position = Vector2.MoveTowards(transform.position, hammerTarget.position, projectileSpeed * Time.deltaTime);
            } 
        
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

        if (col.CompareTag("Hammer Giant") && this.tag == "Healing Projectile") {
            Destroy(this.gameObject);
        }
    }
}
