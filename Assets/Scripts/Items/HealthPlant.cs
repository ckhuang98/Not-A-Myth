using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlant : MonoBehaviour
{
    float timeStamp;
    public Rigidbody2D rb;
    bool moveToPlayer;

    Vector2 direction;

    GameObject player;    
    public bool pickedUp = false;
    public Item item; //scirptable object healt plant

    void Update(){
        if(moveToPlayer && Inventory.instance.count < 5){
            direction = -(transform.position - player.transform.position).normalized;
            rb.velocity = new Vector2(direction.x, direction.y) * 10f * (Time.deltaTime / timeStamp);
        } else{
            rb.velocity = new Vector2(0,0);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Equals("MC Prefab") && pickedUp == false)
        {
            bool wasPickedUp = false;
            
            wasPickedUp = Inventory.instance.Add(Instantiate(item)); //Returns true if the player can add item to the inventroy
            
            if (wasPickedUp)
            {
                
                Debug.Log(Inventory.instance.count);
                // collider.GetComponent<PlayerController>().gainStrength();
                this.gameObject.GetComponent<Renderer>().enabled = false;
                Destroy(this.gameObject);
                pickedUp = true;
            }
        }
        if(collider.gameObject.name.Equals("ItemMagnet") && Inventory.instance.count < 5){
                timeStamp = Time.deltaTime;
                player = GameObject.Find("MC Prefab");
                moveToPlayer = true;
        }
    }
}
