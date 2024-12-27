using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        Debug.Log("Dropped Object: " + droppedObject.name);
        DraggableItem draggableItem = droppedObject.GetComponent<DraggableItem>();
        draggableItem.parentAfterDrag = transform;
    }
}