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
        public string guid;
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

        string newLevelName = PlayerPrefs.GetString("NewLevel", null);
        Debug.Log(PlayerPrefs.GetString("NewLevel", null));
        string filePath;

        if (!string.IsNullOrEmpty(newLevelName))
        {
            filePath = Path.Combine(levelsPath, newLevelName + ".json");
            PlayerPrefs.DeleteKey("NewLevel");
        }
        else
        {
            string[] levelFiles = Directory.GetFiles(levelsPath, "*.json");
            filePath = Path.Combine(levelsPath, "levelData.json");

            foreach (string levelFile in levelFiles)
            {
                string existingJson = File.ReadAllText(levelFile);
                LevelData existingLevelData = JsonUtility.FromJson<LevelData>(existingJson);

                if (existingLevelData.guid == PlayerPrefs.GetString("SelectedLevel", "DefaultLevel"))
                {
                    filePath = levelFile;
                    break;
                }
            }
        }

        string existingGuid = null;

        if (File.Exists(filePath))
        {
            string existingJson = File.ReadAllText(filePath);
            LevelData existingLevelData = JsonUtility.FromJson<LevelData>(existingJson);
            existingGuid = existingLevelData.guid;
        }

        LevelData levelData = new LevelData
        {
            guid = existingGuid ?? Guid.NewGuid().ToString()
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

        Debug.Log(levelData.objects[0].name);
        string json = JsonUtility.ToJson(levelData, true);

        Debug.Log(json);
        File.WriteAllText(filePath, json);

        Debug.Log("Level data saved to " + filePath);
    }
}