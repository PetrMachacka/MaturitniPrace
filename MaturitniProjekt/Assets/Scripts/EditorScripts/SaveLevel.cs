using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLevel : MonoBehaviour
{
    private GameObject Folder;

    [Serializable]
    public class LevelData
    {
        public string name;
        public List<ObjectData> objects = new List<ObjectData>();
    }

    [Serializable]
    public class ObjectData
    {
        public string name;
        public Vector3 position;
    }

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

        string levelsPath = Path.Combine(Application.persistentDataPath, "Levels");

        if (!Directory.Exists(levelsPath))
        {
            Directory.CreateDirectory(levelsPath);
        }

        string filePath = Path.Combine(levelsPath, PlayerPrefs.GetString("SelectedLevel", "DefaultLevel") + ".json");

        Debug.Log(filePath);
        string existingName = ReadExistingLevelData(filePath);
        Debug.Log(existingName);
        
        LevelData levelData = CollectLevelData(existingName);
        Debug.Log(levelData.objects[0].name);
        WriteLevelDataToFile(filePath, levelData);
    }

    private string ReadExistingLevelData(string filePath)
    {
        string existingName = null;

        if (File.Exists(filePath))
        {
            string existingJson = File.ReadAllText(filePath);
            LevelData existingLevelData = JsonUtility.FromJson<LevelData>(existingJson);
            existingName = existingLevelData.name;
        }

        return existingName;
    }

    private LevelData CollectLevelData(string existingName)
    {
        LevelData levelData = new LevelData
        {
            name = existingName
        };

        foreach (Transform child in Folder.transform)
        {
            ObjectData objectData = new ObjectData
            {
                name = child.gameObject.name.Split('(')[0], // Remove (Clone) from the name
                position = child.position
            };
            levelData.objects.Add(objectData);
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