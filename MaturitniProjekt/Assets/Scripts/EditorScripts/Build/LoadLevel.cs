using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.TextCore.Text;
public class LoadLevel : MonoBehaviour
{
    public class LineConnection
    {
        public GameObject InputObject;
        public Vector3 positionB;
    }
    public enum LoadLevelMode
    {
        Build,
        Play
    }
    public static string selectedGuid;
    private string prefabsPath = "Prefabs/Building";
    [SerializeField] private bool EditingCubes = false;
    public LoadLevelMode loadLevelMode = LoadLevelMode.Build;
    private GameObject lineFolder;
    public static bool isCoop = false;
    public GameObject CharacterA;
    public GameObject CharacterB; 
    public GameObject linePrefab;
    public static List<Vector3> PlayerSpawns = new List<Vector3>();
    void Start()
    {
        PlayerSpawns.Clear();
        string selectedGuid = PlayerPrefs.GetString("SelectedLevel", "DefaultLevel");
        Debug.Log("Loaded Level GUID: " + selectedGuid);
        lineFolder = GameObject.Find("Lines");

        string levelsPath = Application.persistentDataPath;

        if (!Directory.Exists(levelsPath))
        {
            Debug.LogError("Levels directory not found: " + levelsPath);
            return;
        }

        string directoryPath = GetLevelDirectoryPath(levelsPath, selectedGuid);
        if (directoryPath != null)
        {
            string filePath = Path.Combine(directoryPath, "levelData.json");
            LoadLevelData(filePath);
        }
        else
        {
            Debug.LogError("No level directory found with the matching GUID: " + selectedGuid);
        }
    }

    private string GetLevelDirectoryPath(string levelsPath, string selectedGuid)
    {
        int SteamDownloads = PlayerPrefs.GetInt("SteamDownloads");
        if(SteamDownloads == 0){
            levelsPath = Path.Combine(levelsPath, "Levels");
        }
        if(SteamDownloads == 1)
        {
            Debug.Log(SteamManager.steamDownloadPath + "/" + selectedGuid);
            return SteamManager.steamDownloadPath + "/" + selectedGuid;
        }
        if(SteamDownloads == 2){
            levelsPath = Path.Combine(levelsPath, "Campaign");
        }
        string[] directories = Directory.GetDirectories(levelsPath);

        foreach (string directory in directories)
        {
            string directoryName = Path.GetFileName(directory);
            if (directoryName == selectedGuid)
            {
                return directory;
            }
        }
        return null;
    }

    private void LoadLevelData(string filePath)
    {
        Building.objectPrefab = null;
        BuildManager.buildMode = BuildModes.build;
        if (!File.Exists(filePath))
        {
            Debug.LogError("Level data file not found: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        isCoop = levelData.isCoop;
        if(!isCoop && loadLevelMode == LoadLevelMode.Play)
        {
            CharacterB.SetActive(false);
            CharacterA.transform.Find("Camera").GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        }
        GameObject buildFolder = GameObject.Find("Build");
        if (buildFolder == null)
        {
            Debug.LogError("No object named 'Build' in the scene.");
            return;
        }
        List<LineConnection> connections = new List<LineConnection>();
        foreach (var obj in levelData.objects)
        {
            GameObject newObject = null;
            string prefabPath = Path.Combine(prefabsPath, obj.name);
            if(obj.name == "SpawnBlockA")
            {
                PlayerSpawns.Add(obj.position);
            }
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab != null)
            {
                newObject = Instantiate(prefab, obj.position, obj.rotation);
                newObject.transform.SetParent(buildFolder.transform);
                if (prefab.name == "SpawnBlockA" && loadLevelMode == LoadLevelMode.Play) newObject.GetComponent<Renderer>().enabled = false;
                if (EditingCubes)
                {
                    if(prefab.GetComponent<Item>().TwoBlocks)
                    {
                        GameObject cube = BuilidingHelpers.EditingCube(newObject);
                        GameObject cube1 = BuilidingHelpers.EditingCube(newObject);
                        cube.transform.position = newObject.transform.position + new Vector3(0, 0.5f, 0);
                        cube1.transform.position = newObject.transform.position + new Vector3(0, -0.5f, 0);
                        continue;
                    }
                    BuilidingHelpers.EditingCube(newObject);
                }
                //Debug.Log($"Instantiated {obj.name} at {obj.position}");
            }
            else
            {
                Debug.LogError($"Prefab not found: {prefabPath}");
            }
            // Find All Connections
            if(obj.connectionData.Count > 0)
            {
                foreach (var connection in obj.connectionData)
                {
                    LineConnection newConnection = new LineConnection();
                    newConnection.InputObject = newObject.gameObject;
                    newConnection.positionB = connection;
                    connections.Add(newConnection);
                }
            }
        }
        // Connect All Objects
        foreach (var connection in connections)
        {
            if(connection.InputObject.GetComponentInParent<Item>().isInput == false)
            {
                continue;
            }
            Debug.Log(connection.positionB);
            GameObject line = null;
            GameObject Dot = connection.InputObject.transform.Find("ConnectingDot").gameObject;
            if(loadLevelMode == LoadLevelMode.Build)
            {
                line = BuilidingHelpers.GenerateLine(linePrefab, Dot.transform.position, connection.positionB);
                line.transform.SetParent(lineFolder.transform);
            }
            GameObject connectedObject = null;
            foreach (Transform child in buildFolder.transform)
            {
                //Debug.Log(BuilidingHelpers.VectorRound(child.position) + " == " + BuilidingHelpers.VectorRound(connection.positionB));
                if (BuilidingHelpers.VectorRound(child.position)  == BuilidingHelpers.VectorRound(connection.positionB))
                {
                    connectedObject = child.gameObject;
                    break;
                }
            }
            if (connectedObject != null)
            {
                Connection ObjectConnection = new Connection()
                {
                    connectedObject = connectedObject.transform.Find("ConnectedDot").gameObject,
                    ConnectionLine = loadLevelMode == LoadLevelMode.Build ? line : null
                };
                connection.InputObject.GetComponent<Item>().connections.Add(ObjectConnection);
                connectedObject.GetComponent<Item>().connections.Add(ObjectConnection);
            }
        }
        if(loadLevelMode == LoadLevelMode.Play)
        {
            int counter = 0;
            Debug.Log(PlayerSpawns.Count);
            foreach (var spawn in PlayerSpawns)
            {
                if(counter == 0)
                    CharacterA.transform.position = spawn;
                else{
                    CharacterB.transform.position = spawn;
                }
                counter++;
            }
        }
    }

}