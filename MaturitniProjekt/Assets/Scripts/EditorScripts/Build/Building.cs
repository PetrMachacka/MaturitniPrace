using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    [SerializeField] private int reach = 8;
    [SerializeField] private Material BrightRed;
    [SerializeField] private GameObject linePrefab;
    private GameObject Folder;
    private GameObject previewFolder;
    private GameObject hitObject;
    private EditorInputSystem editorInputSystem;
    private RaycastHit hit;
    public static GameObject objectPrefab; 
    private GameObject previewBlock;
    private GameObject lastHitObject;
    private bool obstructed = false;
    private float basicRotation = 0;
    // #### CONNECTION VARIABLES ####
    private LayerMask connectionLayerMask = 1 << 8;
    private LayerMask connectedLayserMask = 1 << 9;
    private GameObject lastConnection;
    private GameObject PlayerConnection;
    private GameObject hitConnection;
    private GameObject PlayerLine;
    private bool isDrawingLine = false;
    private GameObject connectionA;
    private GameObject lineFolder;
    private Renderer coloredObject;
    void Start()
    {
        editorInputSystem = new EditorInputSystem();
        Folder = GameObject.Find("Build");
        previewFolder = GameObject.Find("Preview");
        connectionLayerMask = LayerMask.GetMask("Connection");
        connectedLayserMask = LayerMask.GetMask("Connected");
        PlayerConnection = GameObject.Find("PlayerConnection");
        lineFolder = GameObject.Find("Lines");
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            // BUILD MODE
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, reach, LayerMask.GetMask("Edit")))
            {
                HandleRaycastHit(hit);
            }
            else if (previewFolder.transform.childCount > 0)
            {
                ResetPreview();
            }

            // CONNECTION MODE
            Ray connectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            hitConnection = null;
            RaycastHit connectionHit;
            if (Physics.Raycast(connectionRay, out connectionHit, reach, connectionLayerMask | connectedLayserMask))
            {
                hitConnection = connectionHit.transform.gameObject;
                if(lastConnection != connectionHit.transform.gameObject)
                {
                    Debug.Log(connectionHit.transform.gameObject.name);
                    HandleConnectionRaycastHit(connectionHit);
                }
            }
            else
            {
                if (lastConnection != null)
                {
                    lastConnection.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    lastConnection = null;
                }
            }
        }
        if (isDrawingLine && PlayerLine != null)
        {
            LineRenderer lineRenderer = PlayerLine.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(1, PlayerConnection.transform.position);
        }
    }
    private void HandleConnectionRaycastHit(RaycastHit connectionHit)
    {
        GameObject connection = connectionHit.transform.gameObject;
        connection.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        
        lastConnection = connection;
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
                        ToolPreview();
                    }
                    else
                    {
                        BlockPreview(newPosition.Value, previousPreview);
                    }

                }
            }

            lastHitObject = hitObject;
        }
    }
    public void ResetPreview()
    {
        foreach (Transform child in previewFolder.transform)
        {
            Destroy(child.gameObject);
        }
        previewBlock = new GameObject();
        previewBlock.transform.SetParent(previewFolder.transform);
        previewBlock.name = "Reset";
        if (coloredObject != null){
            coloredObject.material.color = coloredObject.material.color / 1.3f;
        }
        coloredObject = null;
    }

    private void ToolPreview()
    {
        if(hitObject != null && lastHitObject != null)
        {
            Renderer renderer = hitObject.transform.parent.GetComponent<Renderer>();
            if (lastHitObject != null && coloredObject != null)
            {
                coloredObject.material.color = coloredObject.material.color / 1.3f;
                coloredObject = null;
            }
            if(objectPrefab.name == "Hammer")
            {
                if (renderer != null)
                {
                    Debug.Log(hitObject.name);
                    Color originalColor = renderer.material.color;
                    renderer.material.color = originalColor * 1.3f;
                    coloredObject = renderer;
                }
            }


        }
    }

    private void BlockPreview(Vector3 newPosition, GameObject previousPreview)
    {
        if (previousPreview != null)
        {
            Destroy(previousPreview);
        }
        Debug.Log(newPosition);
        GameObject newBlockPreview = Instantiate(objectPrefab, newPosition, Quaternion.Euler(0, basicRotation, 0));
        newBlockPreview.transform.SetParent(previewFolder.transform);
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
            if (objectPrefab.GetComponent<Item>().TwoBlocks)
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
        if (newPosition.HasValue)
        {
            newPosition = new Vector3(
            Mathf.Round(newPosition.Value.x * 2) / 2,
            Mathf.Round(newPosition.Value.y * 2) / 2,
            Mathf.Round(newPosition.Value.z * 2) / 2
            );
        }
        return newPosition;
    }

    private void PlaceBlock()
    {
        if (obstructed || previewBlock.name == "Reset") return;

        Vector3? newPosition = previewBlock.transform.position;
        float? newRotation = previewBlock.transform.rotation.eulerAngles.y;
        basicRotation = newRotation.Value;

        if (newPosition != null && Folder != null)
        {
            if(objectPrefab.GetComponent<Item>().isSpawn)
            {
                int spawnCounter = 0;
                foreach (Transform child in Folder.transform)
                {
                    if (child.GetComponent<Item>().isSpawn)
                    {
                        spawnCounter++;
                    }
                }
                if (spawnCounter > (LoadLevel.isCoop ? 1 : 0))
                {
                    Debug.Log("Only one spawn allowed");
                    return;
                }
            }
            GameObject newObject = Instantiate(objectPrefab, newPosition.Value, Quaternion.Euler(0, newRotation.Value, 0));
            newObject.transform.SetParent(Folder.transform);

            if (objectPrefab.GetComponent<Item>().TwoBlocks)
            {
                GameObject cube = BuilidingHelpers.EditingCube(newObject);
                GameObject cube1 = BuilidingHelpers.EditingCube(newObject);
                cube.transform.position = newObject.transform.position + new Vector3(0, 0.5f, 0);
                cube1.transform.position = newObject.transform.position + new Vector3(0, -0.5f, 0);
                return;
            }
            BuilidingHelpers.EditingCube(newObject);
        }

        ResetPreview();
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

    private void UseTool()
    {
        if (hitConnection != null)
        {
            if (PlayerLine == null)
            {
                PlayerLine = BuilidingHelpers.GenerateLine(linePrefab, hitConnection.transform.position, PlayerConnection.transform.position);
                isDrawingLine = true;
                connectionA = hitConnection;
            }
            else if (connectionA != hitConnection)
            {
                bool connectionAIsInput = connectionA.GetComponentInParent<Item>().isInput;
                bool hitConnectionIsInput = hitConnection.GetComponentInParent<Item>().isInput;

                if (connectionAIsInput != hitConnectionIsInput)
                {
                    var connectionAParent = connectionA.GetComponentInParent<Item>();
                    var hitConnectionParent = hitConnection.GetComponentInParent<Item>();

                    if (!connectionAParent.connections.Exists(c => c.connectedObject.GetComponent<Dot>().id == hitConnection.GetComponent<Dot>().id) && 
                    !hitConnectionParent.connections.Exists(c => c.connectedObject.GetComponent<Dot>().id == connectionA.GetComponent<Dot>().id))
                    {
                        Destroy(PlayerLine);
                        GameObject ConnectionLine = BuilidingHelpers.GenerateLine(linePrefab, connectionA.transform.position, hitConnection.transform.position);
                        ConnectionLine.transform.SetParent(lineFolder.transform);
                        Connection connection = new Connection
                        {
                            connectedObject = hitConnectionIsInput ? connectionA : hitConnection,
                            ConnectionLine = ConnectionLine
                        };


                        if (connectionAIsInput)
                        {
                            connectionAParent.connections.Add(connection);
                        }
                        else
                        {
                            hitConnectionParent.connections.Add(connection);
                        }
                        PlayerLine = null;
                        isDrawingLine = false;
                    }
                    else
                    {
                        Destroy(PlayerLine);
                        RemoveExistingConnection(connectionAParent, hitConnection);
                        RemoveExistingConnection(hitConnectionParent, connectionA);
                        PlayerLine = null;
                        isDrawingLine = false;
                    }
                }
            }
        }
        else if (PlayerLine != null)
        {
            Destroy(PlayerLine);

            PlayerLine = null;
            isDrawingLine = false;
        }
    }
    void RemoveExistingConnection(Item parentItem, GameObject connection)
    {
        Connection existingConnection = parentItem.connections.Find(c => c.connectedObject.GetComponent<Dot>().id == connection.GetComponent<Dot>().id);
        if (existingConnection != null)
        {
            Destroy(existingConnection.ConnectionLine);
            parentItem.connections.Remove(existingConnection);
        }
    }
    private void OnRightClick()
    {
        if(objectPrefab.name == "Hammer")
        {
            BreakObject();
        }
    }

    private void OnR()
    {
        Debug.Log("R pressed");
        if (objectPrefab.GetComponent<Item>().Rotating)
        {
            previewBlock.transform.Rotate(0, 90, 0);
        }
    }
}