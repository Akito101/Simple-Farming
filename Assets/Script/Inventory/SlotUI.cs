using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public Image iconImage;
    public TextMeshProUGUI countText;
    public GameObject highlight;

    public ItemData Data { get; private set; }
    public bool IsEmpty => Data == null;

    private static SlotUI draggedFromSlot;

    void Awake()
    {
        if (highlight != null)
            highlight.SetActive(false);

        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry beginDrag = new EventTrigger.Entry();
        beginDrag.eventID = EventTriggerType.BeginDrag;
        beginDrag.callback.AddListener((data) => OnBeginDrag((PointerEventData)data));
        trigger.triggers.Add(beginDrag);

        EventTrigger.Entry drag = new EventTrigger.Entry();
        drag.eventID = EventTriggerType.Drag;
        drag.callback.AddListener((data) => OnDrag((PointerEventData)data));
        trigger.triggers.Add(drag);

        EventTrigger.Entry endDrag = new EventTrigger.Entry();
        endDrag.eventID = EventTriggerType.EndDrag;
        endDrag.callback.AddListener((data) => OnEndDrag((PointerEventData)data));
        trigger.triggers.Add(endDrag);

        EventTrigger.Entry drop = new EventTrigger.Entry();
        drop.eventID = EventTriggerType.Drop;
        drop.callback.AddListener((data) => OnDrop((PointerEventData)data));
        trigger.triggers.Add(drop);
    }

    // ── Data Methods ──────────────────────────────

    public void SetItem(ItemData data)
    {
        Data = data;
        RefreshUI();
    }

    public void AddOne()
    {
        if (Data == null) return;
        Data.count++;
        RefreshUI();
    }

    public void ClearSlot()
    {
        Data = null;
        RefreshUI();
    }

    void RefreshUI()
    {
        if (Data == null)
        {
            iconImage.sprite = null;
            iconImage.color = new Color(1f, 1f, 1f, 0f);
            countText.text = "";
            countText.enabled = false;
        }
        else
        {
            iconImage.sprite = Data.icon;
            iconImage.color = new Color(1f, 1f, 1f, 1f);
            countText.enabled = Data.count > 1;
            countText.text = Data.count > 1 ? Data.count.ToString() : "";
        }
    }

    // ── Highlight ──────────────────────────────

    public void SetHighlight(bool active)
    {
        if (highlight != null)
            highlight.SetActive(active);
    }

    // ── Click ──────────────────────────────

    public void OnPointerClick(PointerEventData eventData) { }

    // ── Drag Methods ──────────────────────────────

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty) return;
        draggedFromSlot = this;

        // Fade this slot to show its being dragged
        iconImage.color = new Color(1f, 1f, 1f, 0.3f);
        Debug.Log("Dragging: " + Data.itemName + " from " + gameObject.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Nothing needed without drag icon
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore appearance if drop failed
        if (draggedFromSlot != null)
        {
            draggedFromSlot.RefreshUI();
            draggedFromSlot = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggedFromSlot == null || draggedFromSlot == this)
        {
            if (draggedFromSlot != null)
            {
                draggedFromSlot.RefreshUI();
                draggedFromSlot = null;
            }
            return;
        }

        Debug.Log("Swapping " + draggedFromSlot.Data.itemName + " with " + gameObject.name);

        // Swap items
        ItemData temp = Data;

        if (draggedFromSlot.Data != null)
            SetItem(draggedFromSlot.Data);
        else
            ClearSlot();

        if (temp != null)
            draggedFromSlot.SetItem(temp);
        else
            draggedFromSlot.ClearSlot();

        draggedFromSlot = null;
    }
}