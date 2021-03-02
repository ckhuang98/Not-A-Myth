using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{

    public static InventoryUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public Transform itemsParent;
    public GameObject inventoryUI;
    public Transform hotbarParent;

    Inventory inventory;

    InventorySlot[] slots;
    HotbarSlot[] hotbarSlots;

    // Start is called before the first frame update
    void Start()
    {
        //set cached inventory to inventory from the game manager
        inventory = Inventory.instance;

        //add the UpdateUI method to the onItemChangedCallback delegate
        inventory.onItemChangedCallback += UpdateUI;

        //get the inventory slots under the itemsParent in Inventory
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        //get the hotbar slots
        hotbarSlots = hotbarParent.GetComponentsInChildren<HotbarSlot>();

        UpdateUI();
    }

    //Update the inventory slots
    public void UpdateUI()
    {
        if (slots != null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < inventory.items.Count)
                {
                    slots[i].UpdateItem(inventory.items[i]);

                    if (i < hotbarSlots.Length)
                    {
                        hotbarSlots[i].UpdateItem(inventory.items[i]);
                    }
                }
                else
                {
                    slots[i].ClearSlot();

                    if (i < hotbarSlots.Length)
                    {
                        hotbarSlots[i].ClearSlot();
                    }
                }
            }
        }
    }
}
