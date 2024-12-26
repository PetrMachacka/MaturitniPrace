using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityEngine.UI.Image image;
    [HideInInspector] public Transform parentAfterDrag;
    private GameObject clone;
    private GameObject originalParent;
    public void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent.parent.gameObject;
        Debug.Log("Begin Drag");
        parentAfterDrag = transform.parent;
        Debug.Log(parentAfterDrag.name);
        if(originalParent.name != "Hotbar")
        {
            clone = Instantiate(gameObject, parentAfterDrag);
            clone.transform.SetSiblingIndex(transform.GetSiblingIndex());
            clone.name = gameObject.name;
        }
        

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

        public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;
        image.raycastTarget = true;

        if (originalParent.name == "Hotbar")
        {
            Destroy(gameObject);
        }

        if (parentAfterDrag.childCount > 1)
        {
            Destroy(parentAfterDrag.GetChild(0).gameObject);
        }
    }
}