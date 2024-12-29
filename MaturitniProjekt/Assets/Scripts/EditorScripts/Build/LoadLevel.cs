using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Scripts;
public class LoadLevel : MonoBehaviour
{
    public static string selectedGuid;
    private string prefabsPath = "Prefabs/Building";
    [SerializeField] private bool EditingCubes = false;
    void Start()
    {
        bool newLevel = PlayerPrefs.GetInt("NewLevelInt", 0) == 1;
        string selectedGuid = PlayerPrefs.GetString("SelectedLevel", "DefaultLevel");
        Debug.Log("Loaded Level GUID: " + selectedGuid);

        string levelsPath = Path.Combine(Application.persistentDataPath, "Levels");

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
        if (!File.Exists(filePath))
        {
            Debug.LogError("Level data file not found: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        GameObject buildFolder = GameObject.Find("Build");
        if (buildFolder == null)
        {
            Debug.LogError("No object named 'Build' in the scene.");
            return;
        }

        foreach (var obj in levelData.objects)
        {
            string prefabPath = Path.Combine(prefabsPath, obj.name);
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab != null)
            {
                GameObject newObject = Instantiate(prefab, obj.position, obj.rotation);
                newObject.transform.SetParent(buildFolder.transform);
                if(EditingCubes)
                {
                    if(prefab.GetComponent<Item>().TwoBlocks)
                    {
                        GameObject cube = BuilidingHelpers.EditingCube(newObject);
                        GameObject cube1 = BuilidingHelpers.EditingCube(newObject);
                        cube.transform.localPosition = new Vector3(0, 0.5f, 0);
                        cube1.transform.localPosition = new Vector3(0, -0.5f, 0);
                        return;
                    }
                    BuilidingHelpers.EditingCube(newObject);
                }
                Debug.Log($"Instantiated {obj.name} at {obj.position}");
            }
            else
            {
                Debug.LogError($"Prefab not found: {prefabPath}");
            }
        }
    }

}