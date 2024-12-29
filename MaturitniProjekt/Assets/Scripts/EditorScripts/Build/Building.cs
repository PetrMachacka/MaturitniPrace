using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    [SerializeField] private int reach = 8;
    [SerializeField] private Material BrightRed;
    private GameObject Folder;
    private GameObject previewFolder;
    private GameObject hitObject;
    private EditorInputSystem editorInputSystem;
    private RaycastHit hit;
    public static GameObject objectPrefab; 
    private GameObject previewBlock;
    private bool obstructed = false;
    private float basicRotation = 0;
    void Start()
    {
        editorInputSystem = new EditorInputSystem();
        Folder = GameObject.Find("Build");
        previewFolder = GameObject.Find("Preview");
    }

    void Update()
    {
        if(!PauseMenu.isPaused){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, reach))
            {
                HandleRaycastHit(hit);
            }
            else if (previewFolder.transform.childCount > 0)
            {
                ResetPreview();
            }
        }
    }

    private void HandleRaycastHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Edit"))
        {
            hitObject = hit.collider.gameObject;
            Vector3? newPosition = GetNewBlockPosition(hit);
            if (newPosition != null && previewFolder != null)
            {
                GameObject previousPreview = previewFolder.transform.childCount > 0 ? previewFolder.transform.GetChild(0).gameObject : null;
                if (previousPreview == null || previousPreview.transform.position != newPosition)
                {
                    obstructed = false;

                    if (BuilidingHelpers.IsHoldingTool(objectPrefab))
                    {
                        ToolPreview(newPosition.Value, previousPreview);
                    }
                    else
                    {
                        BlockPreview(newPosition.Value, previousPreview);
                    }
                }
            }
        }
    }
    private void ResetPreview()
    {
        foreach (Transform child in previewFolder.transform)
        {
            Destroy(child.gameObject);
        }
        previewBlock = new GameObject();
        previewBlock.transform.SetParent(previewFolder.transform);
    }
    private void ToolPreview(Vector3 newPosition, GameObject previousPreview)
    {
        Debug.Log("Tool preview");
    }
    private void BlockPreview(Vector3 newPosition, GameObject previousPreview)
    {
        GameObject newBlockPreview = Instantiate(objectPrefab, newPosition, Quaternion.Euler(0, basicRotation, 0));

        if (objectPrefab.GetComponent<Item>().TwoBlocks)
        {
            foreach (Transform child in Folder.transform)
            {
                if (child.position == newBlockPreview.transform.position + new Vector3(0, 0.5f, 0))
                {
                    obstructed = true;
                    Debug.Log("Obstructed");
                }
            }
        }

        Renderer blockRenderer = newBlockPreview.GetComponent<Renderer>();
        if (blockRenderer != null)
        {
            BuilidingHelpers.SetTransparentMaterial(blockRenderer, obstructed);
        }

        foreach (Transform child in newBlockPreview.transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                BuilidingHelpers.SetTransparentMaterial(childRenderer, obstructed);
            }
        }

        if (previousPreview != null)
        {
            Destroy(previousPreview);
        }
        newBlockPreview.transform.SetParent(previewFolder.transform);
        previewBlock = newBlockPreview;
    }
    private Vector3? GetNewBlockPosition(RaycastHit hit)
    {
        Vector3? newPosition = null;
        if (hitObject != null && !PauseMenu.isPaused && objectPrefab != null)
        {
            newPosition = hitObject.transform.position;
            direcions direction = direcions.up;
            switch (hit.point)
            {
                case Vector3 point when Mathf.Approximately(point.x, hitObject.transform.position.x + 0.5f):
                    newPosition += new Vector3(1, 0, 0);
                    break;
                case Vector3 point when Mathf.Approximately(point.x, hitObject.transform.position.x - 0.5f):
                    newPosition += new Vector3(-1, 0, 0);
                    break;
                case Vector3 point when Mathf.Approximately(point.z, hitObject.transform.position.z + 0.5f):
                    newPosition += new Vector3(0, 0, 1);
                    break;
                case Vector3 point when Mathf.Approximately(point.z, hitObject.transform.position.z - 0.5f):
                    newPosition += new Vector3(0, 0, -1);
                    break;
                case Vector3 point when Mathf.Approximately(point.y, hitObject.transform.position.y + 0.5f):
                    newPosition += new Vector3(0, 1, 0);
                    direction = direcions.up;
                    break;
                case Vector3 point when Mathf.Approximately(point.y, hitObject.transform.position.y - 0.5f):
                    newPosition += new Vector3(0, -1, 0);
                    direction = direcions.down;
                    break;
                default:
                    newPosition = null;
                    break;
            }
            if(objectPrefab.GetComponent<Item>().TwoBlocks)
            {
                switch (direction)
                {
                    case direcions.up:
                        newPosition += new Vector3(0, 0.5f, 0);
                        break;
                    case direcions.down:
                        newPosition += new Vector3(0, -0.5f, 0);
                        break;
                }
            }
        }
        return newPosition;
    }
    private void BreakObject()
    {
        if (hitObject != null)
        {
            Destroy(hitObject.transform.parent.gameObject);
        }
    }
    private void OnLeftClick()
    {
        if (PauseMenu.isPaused || !objectPrefab) return;
        if (BuilidingHelpers.IsHoldingTool(objectPrefab))
        {
            UseTool();
        }
        else
        {
            PlaceBlock();
        }
    }
    private void PlaceBlock()
    {
        if (obstructed || previewBlock == null) return;

        Vector3? newPosition = previewBlock.transform.position;
        float? newRotation = previewBlock.transform.rotation.eulerAngles.y;
        basicRotation = newRotation.Value;

        if (newPosition != null && Folder != null)
        {
            GameObject newObject = Instantiate(objectPrefab, newPosition.Value, Quaternion.Euler(0, newRotation.Value, 0));
            newObject.transform.SetParent(Folder.transform);

            if (objectPrefab.GetComponent<Item>().TwoBlocks)
            {
                GameObject cube = BuilidingHelpers.EditingCube(newObject);
                GameObject cube1 = BuilidingHelpers.EditingCube(newObject);
                cube.transform.localPosition = new Vector3(0, 0.5f, 0);
                cube1.transform.localPosition = new Vector3(0, -0.5f, 0);
                return;
            }

            BuilidingHelpers.EditingCube(newObject);
        }
    }
    private void UseTool()
    {
        if (hitObject != null)
        {
            Debug.Log("Using tool");
        }
    }
    private void OnRightClick()
    {
        BreakObject();
    }
    private void OnR()
    {
        Debug.Log("R pressed");
        if (objectPrefab.GetComponent<Item>().Rotating)
        {
            previewBlock.transform.Rotate(0, 90, 0);
        }
    }   }