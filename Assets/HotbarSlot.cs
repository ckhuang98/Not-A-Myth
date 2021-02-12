using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public Image icon;
    public Text amount;

    public KeyCode keyCode;

    Item item;

    public void Update()
    {
        if (keyCode != KeyCode.None && Input.GetKeyDown(keyCode)){
            if (item != null)
            {
                item.Use();
            }
        }
    }

    //Update item to the inventory slot in the UI
    public void UpdateItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        if (item.amount != 1)
        {
            amount.text = item.amount.ToString();
        }
        else
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
        amount.text = "";
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
