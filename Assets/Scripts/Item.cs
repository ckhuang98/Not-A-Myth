using UnityEngine;

//Adds the ability to create a new Inventory Item in the Create Asset Menu
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

//Scriptable object blueprint that can be used for any item that can be added to the player's inventory
public class Item : ScriptableObject
{
    //default name
    new public string name = "New Item";

    //default icon
    public Sprite icon = null;

    //starts in player's inventory
    public bool isDefaultItem = false;

    // Purpose: Use the item by clicking on it in the inventory
    public virtual void Use()
    {
        Debug.Log("Using " + name);

        switch (name)
        {
            //Shard - Give strength to the player
            case "Shard":
                // GameObject player = GameObject.Find("Player");
                // player.GetComponent<PlayerController>().gainStrength();
                // Inventory.instance.Remove(this);
                break;
        }
    }
}
