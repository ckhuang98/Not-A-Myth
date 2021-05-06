using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaofEffectTime : MonoBehaviour
{
    private float destroyTime = 10f;
    private float hitTimer = 0.25f;
    public bool canHit = true;
    //public PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyTime >= 0f) {
            destroyTime -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }

        // Hit timer used to properly set 'canHit' boolean
        if (hitTimer >= 0f) {
            hitTimer -= Time.deltaTime;
        } else {
            canHit = false;
        }
    }
    /*
    void OnTriggerStay2D(Collider2D col) {
        stats.speed.Value = 0.5f;
    }
    
    void OnTriggerExit2D(Collider2D col) {
        stats.speed.Value = 5f;
        Debug.Log("Exit");
    }
    */
    

    // Called in PlayerController to determine if AoE circle is new
    public bool CanHit() { return canHit; }
}
