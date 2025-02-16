using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Assets.Scripts;
public class SaveLevel : MonoBehaviour
{
    private GameObject Folder;
    public void Start()
    {
        Folder = GameObject.Find("Build");
    }

    public void Save()
    {
        if (Folder == null)
        {
            Debug.LogError("Build GameObject is not assigned.");
            return;
        }
  
        string directoryPath = FileHelpers.GetFolderPathByGuid(PlayerPrefs.GetString("SelectedLevel", "DefaultLevel"));
        string filePath = Path.Combine(directoryPath, "levelData.json");

        string existingName = ReadExistingLevelData(directoryPath);
        Debug.Log(existingName);
        
        LevelData levelData = CollectLevelData(existingName);
        if(levelData == null) return;
        WriteLevelDataToFile(filePath, levelData);
    }

    private string ReadExistingLevelData(string directoryPath)
    {
        string existingName = null;
        Debug.Log(directoryPath);
        if (Directory.Exists(directoryPath))
        {
            string existingJson = File.ReadAllText(directoryPath + "/levelData.json");
            LevelData existingLevelData = JsonUtility.FromJson<LevelData>(existingJson);
            Debug.Log(existingLevelData);
            existingName = existingLevelData.name;
        }

        return existingName;
    }

    private LevelData CollectLevelData(string existingName)
    {
        LevelData levelData = new LevelData
        {
            name = existingName,
            isCoop = LoadLevel.isCoop,
            objects = new List<ObjectData>()
        };
        int spawnCount = 0;
        int endCount = 0;
        List<Vector3> exitingPositions = new List<Vector3>();
        foreach (Transform child in Folder.transform)
        {
            if(child.gameObject.GetComponent<Item>().isSpawn)
            {
                spawnCount++;
            }
            if(child.gameObject.GetComponent<Item>().isEnd)
            {
                endCount++;
            }
            if(exitingPositions.Contains(child.position)) continue;
            List<Vector3> connectionPositions = new List<Vector3>();
            foreach (Connection connection in child.GetComponent<Item>().connections)
            {
                connectionPositions.Add(connection.connectedObject.transform.position);
            }
            ObjectData objectData = new()
            {
                name = child.gameObject.name.Split('(')[0],
                position = child.position,
                rotation = child.rotation,
                connectionData = connectionPositions
            };
            levelData.objects.Add(objectData);
            exitingPositions.Add(child.position);
        }
        if(spawnCount < 2 && LoadLevel.isCoop)
        {
            Errors.ShowError("Not enough spawn points for coop mode.");
            return null;
        }
        else if(spawnCount > 1 && !LoadLevel.isCoop)
        {
            Errors.ShowError("Only one spawn point is allowed in SinglePlayer.");
            return null;
        }
        else if(spawnCount < 1)
        {
            Errors.ShowError("No spawn points found.");
            return null;
        }
        else if(endCount < 1)
        {
            Errors.ShowError("No end points found.");
            return null;
        }
        else if(endCount > 1)
        {
            Errors.ShowError("Only one end point is allowed.");
            return null;
        }
        return levelData;
    }
    
    private void WriteLevelDataToFile(string filePath, LevelData levelData)
    {
        string json = JsonUtility.ToJson(levelData, true);

        Debug.Log(json);
        File.WriteAllText(filePath, json);

        Debug.Log("Level data saved to " + filePath);
    }
}