using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPlant : MonoBehaviour
{
    public bool pickedUp = false;
    public Item item; //scirptable object healt plant

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Equals("MC Prefab") && pickedUp == false)
        {

            bool wasPickedUp = Inventory.instance.Add(Instantiate(item)); //Returns true if the player can add item to the inventroy
            if (wasPickedUp)
            {
                Debug.Log("Picking up " + item.itemName);
                // collider.GetComponent<PlayerController>().gainStrength();
                this.gameObject.GetComponent<Renderer>().enabled = false;
                Destroy(this.gameObject);
                pickedUp = true;
            }
        }
    }
}
