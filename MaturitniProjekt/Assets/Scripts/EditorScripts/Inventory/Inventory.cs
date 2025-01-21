using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Assets.Scripts;
public class Inventory : MonoBehaviour, EditorInputSystem.IEditorActions
{
    private EditorInputSystem inputSystem;
    public GameObject BlocksPanel;
    public GameObject ToolsPanel;
    public GameObject MainInventory;
    public GameObject hotBarPanel;
    public GameObject ToolSlot;
    public Building Building;
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
        PopulatePanel(BlocksPanel, "Prefabs/Building", "BuildingPictures");
        PopulatePanel(ToolsPanel, "Prefabs/Tools", "ToolsPictures");
    }

    private void PopulatePanel(GameObject panel, string prefabFolder, string pictureFolder)
    {
        List<GameObject> inventorySlots = new List<GameObject>();
        foreach (Transform child in panel.transform)
        {
            inventorySlots.Add(child.gameObject);
        }

        GameObject[] prefabs = Resources.LoadAll<GameObject>(prefabFolder);
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            GameObject slot = inventorySlots[i];
            if (i < prefabs.Length)
            {
                GameObject prefab = prefabs[i];
                Texture2D prefabTexture = Resources.Load<Texture2D>($"Prefabs/{pictureFolder}/{prefab.name}");
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
            BuildManager.buildMode = BuildModes.build;
            GameObject LastSlot = hotBarPanel.transform.GetChild(lastInventorySlot).gameObject;
            GameObject hotBarSlot = hotBarPanel.transform.GetChild(key).gameObject;

            Outline lastSlotImage = LastSlot.GetComponent<Outline>();
            Outline hotBarSlotImage = hotBarSlot.GetComponent<Outline>();

            lastSlotImage.enabled = false;
            hotBarSlotImage.enabled = true;
            lastInventorySlot = key;
            if(ToolSlot.transform.childCount > 0)
            {
                Destroy(ToolSlot.transform.GetChild(0).gameObject);
            }
            if(hotBarSlot.transform.childCount > 0)
            {
                var prefabName = hotBarSlotImage.transform.GetChild(0).name;
                GameObject buildingPrefab = Resources.Load<GameObject>($"Prefabs/Building/{prefabName}");
                if (buildingPrefab == null && prefabName != null)
                {
                    buildingPrefab = Resources.Load<GameObject>($"Prefabs/Tools/{prefabName}");
                    GameObject Tool = Instantiate(buildingPrefab, ToolSlot.transform.position, Quaternion.identity);
                    Tool.transform.SetParent(ToolSlot.transform);
                    Tool.transform.localPosition = Vector3.zero;
                    Tool.transform.localRotation = buildingPrefab.transform.rotation;
                    switch (buildingPrefab.name)
                    {
                        case "Drill":
                            BuildManager.buildMode = BuildModes.rotation;
                            break;
                        case "Wrench":
                            BuildManager.buildMode = BuildModes.logic;
                            break;
                        default:
                            break;
                    }
                }
                Building.objectPrefab = buildingPrefab;
            }
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
    public void SelectTools()
    {
        BlocksPanel.SetActive(false);
        ToolsPanel.SetActive(true);
    }
    public void SelectBlocks()
    {
        BlocksPanel.SetActive(true);
        ToolsPanel.SetActive(false);
    }
    public void OnMovement(InputAction.CallbackContext context) { }
    public void OnLeftClick(InputAction.CallbackContext context) { }
    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnESCAPE(InputAction.CallbackContext context) { }
    public void OnPhoto(InputAction.CallbackContext context) { }
    public void OnR(InputAction.CallbackContext context) { }
    public void OnAddPlayer(InputAction.CallbackContext contex) { }
}