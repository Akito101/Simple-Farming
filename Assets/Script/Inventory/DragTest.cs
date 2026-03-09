using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    void Start()
    {
        // Force add a raycast target
        Image img = GetComponent<Image>();
        if (img != null)
        {
            img.raycastTarget = true;
            Debug.Log("Image found and raycast target set ON for: " + gameObject.name);
        }
        else
        {
            Debug.LogError("NO IMAGE COMPONENT on: " + gameObject.name);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("DRAG STARTED on: " + gameObject.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("DRAGGING!");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("DRAG ENDED!");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check what Unity thinks you clicked on
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            var results = new System.Collections.Generic.List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, results);

            foreach (var result in results)
            {
                Debug.Log("Hit: " + result.gameObject.name + " | depth: " + result.depth);
            }
        }
    }
}