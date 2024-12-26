using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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