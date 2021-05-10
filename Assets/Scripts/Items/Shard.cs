using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shard : MonoBehaviour
{
    private bool pickedUp = false;
    public Rigidbody2D rb;

    float timeStamp;

    bool moveToPlayer;

    Vector2 direction;

    GameObject player;

    float speed = 2;
    float timer;

    Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(moveToPlayer && timer > 1){
            direction = -(transform.position - player.transform.position).normalized;
            rb.velocity = new Vector2(direction.x, direction.y) * 20f * (Time.deltaTime / timeStamp);
        } else{
            position.y = position.y + 0.001f * Mathf.Sin(speed * Time.time);
            transform.position = position;
        }
    }

    public Item item; //scirptable object shard

    //Only able to be picked up by the player
    //When picked up, the shard is added to the player's inventory and
    //will call the method on the player "gainStength"
    //This allows the player to deal more damage when they pick up a shard
    //The shard destroys itself on pickup, but to make sure, we also check that 
    //the shard has not already been picked up, so that the player can only pick
    //it up once.
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name.Equals("MC Prefab") && pickedUp == false) {

            // Item newItem = Instantiate(item);
            // newItem.name = item.itemName;
            // bool wasPickedUp = Inventory.instance.Add(newItem); //Returns true if the player can add item to the inventroy
            // if (wasPickedUp)
            // {
            //     Debug.Log("Picking up " + newItem.itemName);
            //     // collider.GetComponent<PlayerController>().gainStrength();
            //     var shardObject = this.gameObject;
            //     shardObject.GetComponent<Renderer>().enabled = false;
            //     Destroy(this.gameObject);
            //     pickedUp = true;
            // }
            Destroy(this.gameObject);
            pickedUp = true;
            GameMaster.instance.pickUpShard();
        }

        if(collider.gameObject.name.Equals("ItemMagnet")){
            timeStamp = Time.deltaTime;
            player = GameObject.Find("MC Prefab");
            moveToPlayer = true;
        }
    }
}
