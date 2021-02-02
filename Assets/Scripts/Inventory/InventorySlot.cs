using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;

    Item item;

    //Add item to the inventory slot in the UI
    public void AddItem (Item newItem)
    {
        item = newItem;

        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
    }

    //Clear inventory slot in the UI
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    //Click on the X icon to remove item from inventory
    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    //Click on the item in the UI to use it
    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }
}
