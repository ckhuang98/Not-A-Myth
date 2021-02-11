using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public Text amount;

    Item item;

    //Update item to the inventory slot in the UI
    public void UpdateItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        if (item.amount != 1)
        {
            amount.text = item.amount.ToString();
        } else
        {
            amount.text = "";
        }
    }

    //Clear inventory slot in the UI
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        amount.text = "";
    }

    //Click on the X icon to remove item from inventory
    public void OnRemoveButton()
    {
        Inventory.instance.Delete(item);
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
