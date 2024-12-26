using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    [SerializeField]
    private int reach = 8;
    private GameObject Folder;
    private GameObject previewFolder;
    private GameObject currentObject;
    private EditorInputSystem editorInputSystem;
    private RaycastHit hit;
    [SerializeField] public static GameObject objectPrefab; 

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
                Debug.Log(hit.collider.name);
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
                GameObject newObject = Instantiate(objectPrefab, newPosition.Value, Quaternion.identity);
                Destroy(previousPreview);
                newObject.transform.SetParent(previewFolder.transform);
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
            Destroy(currentObject);
        }
    }
    private void OnLeftClick()
    {
        Vector3? newPosition = GetNewBlockPosition(hit);
        if(newPosition != null  && Folder != null)
        {
            GameObject newObject = Instantiate(objectPrefab, newPosition.Value, Quaternion.identity);
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
}