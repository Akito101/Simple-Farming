using UnityEngine;

public class SoilPlot : MonoBehaviour
{
    [Header("Visuals")]
    public SpriteRenderer cropRenderer;
    public GameObject wateredIcon;

    public bool IsReadyToHarvest { get; private set; }
    public bool IsPlanted => isPlanted;
    public bool IsWatered => isWatered;
    public bool IsDead => isDead;
    public bool NeedsWatering => isPlanted && !isWatered && !IsReadyToHarvest && !isDead;

    private CropData currentCrop;
    private int dayPlanted = 0;
    private bool isPlanted = false;
    private bool isWatered = false;
    private bool isDead = false;
    private int missedWaterDays = 0;
    private int wateredDays = 0; // Track how many days it was watered

    void Start()
    {
        DayManager.Instance.OnNewDay += OnNewDay;
        cropRenderer.sprite = null;
        if (wateredIcon != null)
            wateredIcon.SetActive(false);
    }

    void OnDestroy()
    {
        DayManager.Instance.OnNewDay -= OnNewDay;
    }

    public void Interact(CropData selectedCrop)
    {
        if (isDead)
        {
            ResetPlot();
            return;
        }

        if (!isPlanted && selectedCrop != null)
        {
            PlantSeed(selectedCrop);
        }
        else if (NeedsWatering)
        {
            WaterCrop();
        }
        else if (IsReadyToHarvest)
        {
            HarvestCrop();
        }
        else if (isWatered)
        {
            Debug.Log("Already watered today!");
        }
        else if (!isPlanted)
        {
            Debug.Log("No seed selected!");
        }
    }

    void PlantSeed(CropData crop)
    {
        currentCrop = crop;
        dayPlanted = DayManager.Instance.CurrentDay;
        isPlanted = true;
        isWatered = false;
        missedWaterDays = 0;
        wateredDays = 0;
        IsReadyToHarvest = false;

        cropRenderer.sprite = crop.seedSprite;
        InventoryManager.Instance.RemoveItem(crop.seedTag);
        Debug.Log("Planted: " + crop.cropName);
    }

    void WaterCrop()
    {
        isWatered = true;
        wateredDays++;
        cropRenderer.color = new Color(0.6f, 0.8f, 1f);

        if (wateredIcon != null)
            wateredIcon.SetActive(true);

        Debug.Log("Watered! Total watered days: " + wateredDays);
    }

    void HarvestCrop()
    {
        InventoryManager.Instance.AddItem(
            currentCrop.cropName,
            currentCrop.harvestSprite,
            currentCrop.maxStackSize
        );

        Debug.Log("Harvested: " + currentCrop.cropName);
        ResetPlot();
    }

    void ResetPlot()
    {
        currentCrop = null;
        isPlanted = false;
        isWatered = false;
        IsReadyToHarvest = false;
        isDead = false;
        missedWaterDays = 0;
        wateredDays = 0;

        cropRenderer.sprite = null;
        cropRenderer.color = Color.white;

        if (wateredIcon != null)
            wateredIcon.SetActive(false);
    }

    void OnNewDay()
    {
        if (!isPlanted || IsReadyToHarvest) return;

        if (!isWatered)
        {
            missedWaterDays++;
            Debug.Log(currentCrop.cropName + " not watered! Missed days: " + missedWaterDays);

            if (missedWaterDays >= 2)
            {
                KillCrop();
                return;
            }
        }
        else
        {
            // Only grow if watered!
            missedWaterDays = 0;
        }

        // Reset watered state for new day
        isWatered = false;
        cropRenderer.color = Color.white;

        if (wateredIcon != null)
            wateredIcon.SetActive(false);

        // Only check growth based on watered days not total days!
        if (wateredDays >= currentCrop.daysToGrow)
        {
            IsReadyToHarvest = true;
            cropRenderer.sprite = currentCrop.readySprite;
            Debug.Log(currentCrop.cropName + " is ready to harvest!");
        }
        else
        {
            cropRenderer.sprite = currentCrop.growingSprite;
            Debug.Log(currentCrop.cropName + " growing... " + wateredDays + "/" + currentCrop.daysToGrow + " days watered");
        }
    }

    void KillCrop()
    {
        isDead = true;
        cropRenderer.color = new Color(0.3f, 0.3f, 0.3f);

        if (wateredIcon != null)
            wateredIcon.SetActive(false);

        Debug.Log(currentCrop.cropName + " died from lack of water!");

        // Auto destroy after 3 seconds
        Invoke("ResetPlot", 3f);
    }
}