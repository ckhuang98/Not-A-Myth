using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_Projectile : MonoBehaviour
{
    private float damageProjectileSpeed = 10f;
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 moveDirection;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        moveDirection = (target.position - transform.position).normalized * damageProjectileSpeed;
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Destroy(this.gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            Destroy(this.gameObject);
        }

        if (col.CompareTag("Walls")) {
            Destroy(this.gameObject);
        }
    }
    
}
