using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y <= -6.5f) {
            Destroy(this.gameObject);
        }        
    }
    private void FixedUpdate() {
        Vector3 newScale = transform.localScale;
        newScale.x += .006f;
        newScale.y += .003f;
        transform.localScale = newScale;
        
        Vector3 newPos = transform.position;
        newPos.y -= .18f;
        transform.position = newPos;
    }
    /*
    void OnTriggerEnter2D() {
        Debug.Log("touched");
    }
    */
    // private void OnTriggerEnter2D(Collider2D other) {
    //     if(other.CompareTag("Player")){
    //         GameMaster.instance.combatManager.player.TakeDamage(15);
    //     }
    // }
}
