using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, EditorInputSystem.IEditorActions
{
    private EditorInputSystem inputSystem;
    public GameObject inventoryPanel;
    public GameObject MainInventory;
    public GameObject hotBarPanel;
    [HideInInspector]public static int lastInventorySlot = 0;

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
        }
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Building");
        Debug.Log(prefabs.Length);
        for (int i = 0; i < inventorySlots.Count ; i++)
        {
            GameObject slot = inventorySlots[i];
            if(i < prefabs.Length)
            {
                GameObject prefab = prefabs[i];
                Texture2D prefabTexture = Resources.Load<Texture2D>($"Prefabs/Textures/{prefab.name}");
                if (prefabTexture != null)
                {
                    GameObject textureObject = new GameObject("PrefabTexture");
                    textureObject.transform.SetParent(slot.transform);
                    textureObject.transform.localPosition = Vector3.zero;

                    Image textureImage = textureObject.AddComponent<Image>();
                    textureImage.sprite = Sprite.Create(prefabTexture, new Rect(0, 0, prefabTexture.width, prefabTexture.height), new Vector2(0.5f, 0.5f), 100f);
                    textureImage.rectTransform.sizeDelta = new Vector2(39, 39);
                    textureImage.name = prefab.name;
                    
                    textureImage.rectTransform.localScale = Vector3.one;
                    textureObject.AddComponent<DraggableItem>();
                }
            }
            else
            {
                slot.SetActive(false);
            }
        }
    }

    public void OnNumberKeys(InputAction.CallbackContext context)
    {
        Color originalColor = new Color(247f / 255f, 233f / 255f, 118f / 255f, 154f / 255f);
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
            GameObject LastSlot = hotBarPanel.transform.GetChild(lastInventorySlot).gameObject;
            GameObject hotBarSlot = hotBarPanel.transform.GetChild(key).gameObject;

            Outline lastSlotImage = LastSlot.GetComponent<Outline>();
            Outline hotBarSlotImage = hotBarSlot.GetComponent<Outline>();

            lastSlotImage.enabled = false;
            hotBarSlotImage.enabled = true;
            lastInventorySlot = key;
            var prefabName = hotBarSlotImage.transform.GetChild(0).name;
            GameObject buildingPrefab = Resources.Load<GameObject>($"Prefabs/Building/{prefabName}");
            Building.objectPrefab = buildingPrefab;
        }
    }
    public void OnE(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(MainInventory.activeSelf)
            {
                MainInventory.SetActive(false);
                Time.timeScale = 1f;
                PauseMenu.isPaused = false;
                inputSystem.Editor.ESCAPE.Enable();
            }
            else if(!MainInventory.activeSelf && !PauseMenu.isPaused)
            {
                MainInventory.SetActive(true);
                Time.timeScale = 0f;
                PauseMenu.isPaused = true;
                inputSystem.Editor.ESCAPE.Disable();
            }
        }
    }
    public void OnMovement(InputAction.CallbackContext context) { }
    public void OnLeftClick(InputAction.CallbackContext context) { }
    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnESCAPE(InputAction.CallbackContext context) { }
    public void OnPhoto(InputAction.CallbackContext context) { }
    public void OnR(InputAction.CallbackContext context) { }
}