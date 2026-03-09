using UnityEngine;

[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite icon;
    public int count;
    public int maxStack;

    public ItemData(string name, Sprite icon, int count, int maxStack)
    {
        this.itemName = name;
        this.icon = icon;
        this.count = count;
        this.maxStack = maxStack;
    }

    public bool IsSameItem(string name)
    {
        return itemName == name;
    }
}