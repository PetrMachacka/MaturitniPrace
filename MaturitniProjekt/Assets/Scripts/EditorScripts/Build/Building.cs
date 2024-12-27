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
    private GameObject currentObject;
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
        }
    }

    private void HandleRaycastHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Edit"))
        {
            currentObject = hit.collider.gameObject;
            Vector3? newPosition = GetNewBlockPosition(hit);
            GameObject previousPreview = previewFolder.transform.GetChild(0).gameObject;
            if(newPosition != null && previewFolder != null && previousPreview.transform.position != newPosition)
            {
                obstructed = false;
                GameObject newObject = Instantiate(objectPrefab, newPosition.Value, Quaternion.Euler(0, basicRotation, 0));

                if(objectPrefab.GetComponent<Block>().TwoBlocks){
                    foreach (Transform child in Folder.transform)
                    {
                        if(child.position == newObject.transform.position + new Vector3(0,0.5f,0))
                        {
                            obstructed = true;
                            Debug.Log("Obstructed");
                        }
                    }
                }
                Renderer renderer = newObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    BuilidingHelpers.SetTransparentMaterial(renderer, obstructed);
                }

                foreach (Transform child in newObject.transform)
                {
                    Renderer childRenderer = child.GetComponent<Renderer>();
                    if (childRenderer != null)
                    {
                        BuilidingHelpers.SetTransparentMaterial(childRenderer, obstructed);
                    }
                }
                Destroy(previousPreview);
                newObject.transform.SetParent(previewFolder.transform);
                previewBlock = newObject;
            }
        }
    }
    private Vector3? GetNewBlockPosition(RaycastHit hit)
    {
        Vector3? newPosition = null;
        if (currentObject != null && !PauseMenu.isPaused && objectPrefab != null)
        {
            newPosition = currentObject.transform.position;
            direcions direction = direcions.up;
            switch (hit.point)
            {
                case Vector3 point when Mathf.Approximately(point.x, currentObject.transform.position.x + 0.5f):
                    newPosition += new Vector3(1, 0, 0);
                    break;
                case Vector3 point when Mathf.Approximately(point.x, currentObject.transform.position.x - 0.5f):
                    newPosition += new Vector3(-1, 0, 0);
                    break;
                case Vector3 point when Mathf.Approximately(point.z, currentObject.transform.position.z + 0.5f):
                    newPosition += new Vector3(0, 0, 1);
                    break;
                case Vector3 point when Mathf.Approximately(point.z, currentObject.transform.position.z - 0.5f):
                    newPosition += new Vector3(0, 0, -1);
                    break;
                case Vector3 point when Mathf.Approximately(point.y, currentObject.transform.position.y + 0.5f):
                    newPosition += new Vector3(0, 1, 0);
                    direction = direcions.up;
                    break;
                case Vector3 point when Mathf.Approximately(point.y, currentObject.transform.position.y - 0.5f):
                    newPosition += new Vector3(0, -1, 0);
                    direction = direcions.down;
                    break;
                default:
                    newPosition = null;
                    break;
            }
            if(objectPrefab.GetComponent<Block>().TwoBlocks)
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
    private GameObject EditingCube(GameObject parent)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(parent.transform);
        cube.transform.localPosition = Vector3.zero;
        cube.tag = "Edit";

        cube.GetComponent<Renderer>().enabled = false;
        return cube;
    }
    private void BreakObject()
    {
        if (currentObject != null)
        {
            Destroy(currentObject.transform.parent.gameObject);
        }
    }
    private void OnLeftClick()
    {
        if(obstructed) return;
        if(previewBlock == null) return;
        Vector3? newPosition = previewBlock.transform.position;
        float? newRotation = previewBlock.transform.rotation.eulerAngles.y;
        basicRotation = newRotation.Value;
        if(newPosition != null  && Folder != null)
        {
            GameObject newObject = Instantiate(objectPrefab, newPosition.Value, Quaternion.Euler(0, newRotation.Value, 0));
            newObject.transform.SetParent(Folder.transform);
            if(objectPrefab.GetComponent<Block>().TwoBlocks)
            {
                GameObject cube = EditingCube(newObject);
                GameObject cube1 = EditingCube(newObject);
                cube.transform.localPosition = new Vector3(0, 0.5f, 0);
                cube1.transform.localPosition = new Vector3(0, -0.5f, 0);
                return;
            }
            EditingCube(newObject);
        }
    }
    private void OnRightClick()
    {
        BreakObject();
    }
    private void OnR()
    {
        Debug.Log("R pressed");
        if (objectPrefab.GetComponent<Block>().Rotating)
        {
            previewBlock.transform.Rotate(0, 90, 0);
        }
    }   
}