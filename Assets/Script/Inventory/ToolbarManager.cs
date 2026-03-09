using UnityEngine;
using System.Collections.Generic;

public class ToolbarManager : MonoBehaviour
{
    public static ToolbarManager Instance;

    [Header("Toolbar Slots")]
    public List<SlotUI> toolbarSlots;

    private int selectedIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Highlight first slot by default
        UpdateHighlight();
    }

    void Update()
    {
        // Press 1-9 to select slot
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedIndex = i;
                UpdateHighlight();
                Debug.Log("Selected slot: " + (selectedIndex + 1));
            }
        }
    }

    void UpdateHighlight()
    {
        for (int i = 0; i < toolbarSlots.Count; i++)
        {
            toolbarSlots[i].SetHighlight(i == selectedIndex);
        }
    }

    public SlotUI GetSelectedSlot()
    {
        return toolbarSlots[selectedIndex];
    }
}