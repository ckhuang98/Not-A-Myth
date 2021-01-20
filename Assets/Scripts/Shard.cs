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

    public Item item;

    //Only able to be picked up by the player
    //When picked up, the shard will call the method on the player "gainStength"
    //This allows the player to deal more damage when they pick up a shard
    //The shard destroys itself on pickup, but to make sure, we also check that 
    //the shard has not already been picked up, so that the player can only pick
    //it up once. 
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Equals("Player") && pickedUp == false) {

            bool wasPickedUp = Inventory.instance.Add(item);
            if (wasPickedUp)
            {
                Debug.Log("Picking up item " + item.name);
                collider.GetComponent<PlayerController>().gainStrength();
                var shardObject = this.gameObject;
                shardObject.GetComponent<Renderer>().enabled = false;
                Destroy(this.gameObject);
                pickedUp = true;
            }
        }
    }

    void PickUp()
    {
        
    }


}
