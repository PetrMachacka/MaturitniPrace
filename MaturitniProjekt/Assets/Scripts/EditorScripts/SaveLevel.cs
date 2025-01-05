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
        Debug.Log(levelData.objects[0].name);
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
            isCoop = false,
            objects = new List<ObjectData>()
        };
        int numberOfSpawns = 0;
        List<Vector3> exitingPositions = new List<Vector3>();
        foreach (Transform child in Folder.transform)
        {
            if(exitingPositions.Contains(child.position)) continue;
            List<Vector3> connectionPositions = new List<Vector3>();
            foreach (Connection connection in child.GetComponent<Item>().connections)
            {
                connectionPositions.Add(connection.connectedObject.transform.position);
            }
            if(child.GetComponent<Item>().isSpawn)
            {
                numberOfSpawns++;
                if(numberOfSpawns > 1)
                {
                    levelData.isCoop = true;
                }
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