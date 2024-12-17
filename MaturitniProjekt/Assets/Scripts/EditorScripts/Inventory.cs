using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, EditorInputSystem.IEditorActions
{
    private EditorInputSystem inputSystem;
    public GameObject inventoryPanel;
    public GameObject hotBarPanel;

    private void Awake()
    {
        inputSystem = new EditorInputSystem();
        inputSystem.Editor.SetCallbacks(this);
    }
    private void Start()
    {
        PopulateInventory();
    }

    private void OnEnable()
    {
        inputSystem.Editor.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Editor.Disable();
    }

    private void PopulateInventory()
    {
        List<GameObject> inventorySlots = new List<GameObject>();
        foreach (Transform child in inventoryPanel.transform)
        {
            inventorySlots.Add(child.gameObject);
            Debug.Log(child.name);
        }

        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Build");
        foreach (GameObject prefab in prefabs)
        {
            Debug.Log(prefab.name);
        }
        for (int i = 0; i < inventorySlots.Count && i < prefabs.Length; i++)
        {
            GameObject slot = inventorySlots[i];
            Debug.Log(slot.name);
            GameObject prefab = prefabs[i];
            if (slot.TryGetComponent<Image>(out Image slotImage))
            if (slotImage != null)
            {
                Sprite prefabSprite = prefab.GetComponent<SpriteRenderer>().sprite;
                slotImage.sprite = prefabSprite;
            }
        }
    }

    public void OnNumberKeys(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int key = int.Parse(context.control.name);
            if(key > 0){
                key--;
            }
            else if (key == 0)
            {
                key = 9;
            }
            Debug.Log($"Number key pressed: {key}");
        }
    }

    public void OnMovement(InputAction.CallbackContext context) { }
    public void OnLeftClick(InputAction.CallbackContext context) { }
    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnESCAPE(InputAction.CallbackContext context) { }
    public void OnPhoto(InputAction.CallbackContext context) { }
}