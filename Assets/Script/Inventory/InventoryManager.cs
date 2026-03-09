using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Slots")]
    public List<SlotUI> toolbarSlots;
    public List<SlotUI> inventorySlots;

    [Header("UI")]
    public GameObject inventoryPanel;

    private bool inventoryOpen = false;
    private List<SlotUI> allSlots = new List<SlotUI>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Combine all slots into one list for easy searching
        allSlots.AddRange(toolbarSlots);
        allSlots.AddRange(inventorySlots);

        // Initialize all slots as empty
        foreach (SlotUI slot in allSlots)
        {
            slot.ClearSlot();
        }

        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventoryOpen = !inventoryOpen;
            inventoryPanel.SetActive(inventoryOpen);
        }
    }
    public void RemoveItem(string itemName)
    {
        // Check toolbar first
        foreach (SlotUI slot in toolbarSlots)
        {
            if (!slot.IsEmpty && slot.Data.itemName == itemName)
            {
                if (slot.Data.count > 1)
                {
                    slot.Data.count--;
                    slot.SetItem(slot.Data); // Refresh UI
                }
                else
                {
                    slot.ClearSlot(); // Remove completely if last one
                }
                return;
            }
        }

        // Then check inventory
        foreach (SlotUI slot in inventorySlots)
        {
            if (!slot.IsEmpty && slot.Data.itemName == itemName)
            {
                if (slot.Data.count > 1)
                {
                    slot.Data.count--;
                    slot.SetItem(slot.Data);
                }
                else
                {
                    slot.ClearSlot();
                }
                return;
            }
        }
    }

    public int GetItemCount(string itemName)
    {
        foreach (SlotUI slot in toolbarSlots)
        {
            if (!slot.IsEmpty && slot.Data.itemName == itemName)
                return slot.Data.count;
        }

        foreach (SlotUI slot in inventorySlots)
        {
            if (!slot.IsEmpty && slot.Data.itemName == itemName)
                return slot.Data.count;
        }

        return 0; // Item not found
    }

    public bool AddItem(string itemName, Sprite icon, int maxStack)
    {
        // STEP 1: Find existing slot with same item that isn't full
        foreach (SlotUI slot in allSlots)
        {
            if (!slot.IsEmpty && slot.Data.itemName == itemName && slot.Data.count < maxStack)
            {
                slot.AddOne();
                Debug.Log("Stacked " + itemName + " in " + slot.gameObject.name);
                return true;
            }
        }

        // STEP 2: Find first empty slot in toolbar
        foreach (SlotUI slot in toolbarSlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(new ItemData(itemName, icon, 1, maxStack));
                Debug.Log("New item " + itemName + " placed in toolbar: " + slot.gameObject.name);
                return true;
            }
        }

        // STEP 3: Find first empty slot in inventory
        foreach (SlotUI slot in inventorySlots)
        {
            if (slot.IsEmpty)
            {
                slot.SetItem(new ItemData(itemName, icon, 1, maxStack));
                Debug.Log("New item " + itemName + " placed in inventory: " + slot.gameObject.name);
                return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }
}