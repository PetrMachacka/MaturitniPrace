using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    [SerializeField]
    private int reach = 8;
    private Color originalColor;
    private GameObject currentObject;
    private EditorInputSystem editorInputSystem;
    private RaycastHit hit;
    [SerializeField] private GameObject objectPrefab; 

    void Start()
    {
        editorInputSystem = new EditorInputSystem();
    }

    void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 8f))
        {
            HandleRaycastHit(hit);
        }
        else
        {
            RestoreOriginalColor();
        }
    }

    private void HandleRaycastHit(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Edit"))
        {
            if (currentObject != hit.collider.gameObject)
            {
                RestorePreviousObjectColor();
                SelectNewObject(hit.collider.gameObject);
            }
        }
        else
        {
            RestoreOriginalColor();
        }
    }

    private void RestorePreviousObjectColor()
    {
        if (currentObject != null)
        {
            Renderer previousRenderer = currentObject.GetComponent<Renderer>();
            if (previousRenderer != null)
            {
                previousRenderer.material.color = originalColor;
            }
        }
    }

    private void SelectNewObject(GameObject newObject)
    {
        currentObject = newObject;
        Renderer renderer = currentObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalColor = renderer.material.color;
            renderer.material.color = originalColor * 1.5f;
        }
    }

    private void RestoreOriginalColor()
    {
        if (currentObject != null)
        {
            Renderer renderer = currentObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }
            currentObject = null;
        }
    }
    private void PlaceObjectNextToCurrent(RaycastHit hit)
    {
        if (currentObject != null)
        {
            Debug.Log(hit.point.x + " " + currentObject.transform.position.x);
            Vector3 newPosition = currentObject.transform.position;
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
                    break;
                case Vector3 point when Mathf.Approximately(point.y, currentObject.transform.position.y - 0.5f):
                    newPosition += new Vector3(0, -1, 0);
                    break;
            }

            Instantiate(objectPrefab, newPosition, Quaternion.identity);
        }
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
        PlaceObjectNextToCurrent(hit);
    }
    private void OnRightClick()
    {
        BreakObject();
    }
}