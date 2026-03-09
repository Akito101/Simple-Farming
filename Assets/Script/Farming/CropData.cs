using UnityEngine;

[CreateAssetMenu(fileName = "NewCrop", menuName = "Farming/Crop")]
public class CropData : ScriptableObject
{
    [Header("Crop Info")]
    public string cropName;
    public string seedTag;            // ← e.g. "TomatoSeed"
    public Sprite seedSprite;
    public Sprite growingSprite;
    public Sprite readySprite;
    public Sprite harvestSprite;

    [Header("Growth Settings")]
    public int daysToGrow = 3;
    public int harvestAmount = 1;

    [Header("Inventory")]
    public int maxStackSize = 64;
}