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

    // use the item by clicking on it in the inventory
    public virtual void Use()
    {
        Debug.Log("Using " + name);
    }
}
