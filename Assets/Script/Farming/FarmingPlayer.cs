using UnityEngine;

public class FarmingPlayer : MonoBehaviour
{
    [Header("All Available Crops")]
    public CropData[] allCrops;

    [Header("Player Tools")]
    public GameObject waterBucket;
    public GameObject pickaxe;

    [Header("Detection")]
    public float detectionRadius = 1.5f; // How close player needs to be to soil

    private CropData selectedCrop;

    void Start()
    {
        HideAllTools();
    }

    void Update()
    {
        UpdateSelectedCrop();
        CheckNearbySoil();   // ← Check every frame not just on click

        if (Input.GetMouseButtonDown(0))
        {
            CheckClick();
        }
    }

    void UpdateSelectedCrop()
    {
        SlotUI selectedSlot = ToolbarManager.Instance.GetSelectedSlot();

        if (selectedSlot == null || selectedSlot.IsEmpty)
        {
            selectedCrop = null;
            return;
        }

        string selectedTag = selectedSlot.Data.itemName;
        selectedCrop = null;

        foreach (CropData crop in allCrops)
        {
            if (crop.seedTag == selectedTag)
            {
                selectedCrop = crop;
                break;
            }
        }
    }

    void CheckNearbySoil()
    {
        // Find all colliders nearby
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        SoilPlot closestSoil = null;
        float closestDist = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            SoilPlot soil = hit.GetComponent<SoilPlot>();
            if (soil != null)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestSoil = soil;
                }
            }
        }

        // Show correct tool based on closest soil state
        if (closestSoil != null)
            UpdateTools(closestSoil);
        else
            HideAllTools(); // No soil nearby, hide everything
    }

    void CheckClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            SoilPlot soil = hit.collider.GetComponent<SoilPlot>();
            if (soil != null)
            {
                soil.Interact(selectedCrop);
            }
        }
    }

    public void UpdateTools(SoilPlot soil)
    {
        HideAllTools();

        if (soil.IsReadyToHarvest)
        {
            pickaxe.SetActive(true);
        }
        else if (soil.NeedsWatering)
        {
            waterBucket.SetActive(true);
        }
    }

    public void HideAllTools()
    {
        if (waterBucket != null) waterBucket.SetActive(false);
        if (pickaxe != null) pickaxe.SetActive(false);
    }
}