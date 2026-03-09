using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Info")]
    public Sprite itemIcon;
    public int maxStackSize = 64;

    private string itemName;
    private bool pickedUp = false;

    void Start()
    {
        itemName = gameObject.tag;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !pickedUp)
        {
            pickedUp = true;
            Debug.Log("Picking up seed with tag: '" + gameObject.tag + "' | itemName: '" + itemName + "'");
            bool success = InventoryManager.Instance.AddItem(itemName, itemIcon, maxStackSize);
            if (success)
                Destroy(gameObject);
            else
                pickedUp = false;
        }
    }
}