using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    private bool pickedUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Check if this shard has collided with the player and increase their health and strength if so
    void OnTriggerEnter2D(Collider2D collider) { 
        if (collider.gameObject.name.Equals("Player") && pickedUp == false) {
            collider.GetComponent<PlayerController>().gainStrength(); //calls the player's gain strength function
            collider.GetComponent<PlayerController>().gainHealth(); //calls the player's gain health function
            var shardObject = this.gameObject;
            shardObject.GetComponent<Renderer>().enabled = false; //makes the shard invisible 
            Destroy(shardObject); //destroys the shard
            pickedUp = true; //just to make sure the shard isn't picked up multiple times
        }
        
    }


}
