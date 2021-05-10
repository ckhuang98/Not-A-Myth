using UnityEngine;

//Adds the ability to create a new Inventory Item in the Create Asset Menu
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

//Scriptable object blueprint that can be used for any item that can be added to the player's inventory
public class Item : ScriptableObject
{
    //default name
    public string itemName = "New Item";

    //default icon
    public Sprite icon = null;

    //starts in player's inventory
    public bool isDefaultItem = false;

    // maximum stack size
    public int stackSize = 1;

    // currently held
    public int amount = 0;

    // amount to restore health by, if this item does that
    public int restoreHealth = 0;

    // Check that 2 items are the same so that they may be stacked accordingly in the inventory
    // and perhaps other future use cases
    public bool Equals(Item item)
    {
        // properties of items to check if they are equal.
        // More properties will probably be added like attack, defence, modifiers, etc.
        if (this.itemName == item.itemName)
        {
            return true;
        }

        return false;
        
    }

    private void OnDestroy()
    {
        Debug.LogWarning(itemName + " Object Destroyed");
    }

    // Purpose: Use the item by clicking on it in the inventory
    public virtual void Use()
    {
        Debug.Log("Using " + itemName);
        GameObject player = GameObject.FindWithTag("Player");

        switch (itemName)
        {
            //Shard - Give strength to the player
            case "Shard":
                GameMaster.instance.gainStrength();

                Inventory.instance.Remove(this);

                break;

            case "Health Plant":
                player.GetComponent<PlayerController>().restoreHealth(GameMaster.instance.playerStats.healAmount.Value);
                Inventory.instance.Remove(this);
                break;
        }
    }
}
