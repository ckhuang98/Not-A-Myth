using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Singleton for inventory
    public static Inventory instance;

    //The inventory exists within the GameManager object
    //and it must be the only instance of inventory at all times
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
    }

    //delegate called for calling subscibed methods when inventory is updated
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

   //The amount of items that can be carried in the inventory
    public int space = 20;

    //The actual List of inventory items
    public List<Item> items = new List<Item>();

    //Purpose: Add the item to the inventory list if it is not a default item and there is room in the inventory for it
    public bool Add(Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space)
            {
                Debug.Log("Not Enough Room");
                return false;
            }

            items.Add(item);

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }

        return true;
    }

    //Purpose: Remove the item from the inventory list
    public void Remove (Item item)
    {
        items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
