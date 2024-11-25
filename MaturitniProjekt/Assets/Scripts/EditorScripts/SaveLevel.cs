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

        string filePath = GetFilePath(levelsPath);
        LevelData levelData = CollectLevelData(filePath);
        WriteLevelDataToFile(filePath, levelData);
        PlayerPrefs.DeleteKey("NewLevel");
    }

    private string GetFilePath(string levelsPath)
    {
        string newLevelName = PlayerPrefs.GetString("NewLevel", null);

        Debug.Log(newLevelName);
        string filePath;

        if (!string.IsNullOrEmpty(newLevelName))
        {
            filePath = Path.Combine(levelsPath, Guid.NewGuid().ToString() + ".json");
        }
        else
        {
            string[] levelFiles = Directory.GetFiles(levelsPath, "*.json");
            filePath = Path.Combine(levelsPath, "levelData.json");

            foreach (string levelFile in levelFiles)
            {
                string existingJson = File.ReadAllText(levelFile);
                LevelData existingLevelData = JsonUtility.FromJson<LevelData>(existingJson);

                if (existingLevelData.name == PlayerPrefs.GetString("SelectedLevel", "DefaultLevel"))
                {
                    filePath = levelFile;
                    break;
                }
            }
        }

        return filePath;
    }

    private LevelData CollectLevelData(string filePath)
    {
        LevelData levelData = new LevelData
        {
            name = PlayerPrefs.GetString("NewLevel", "")
        };

        foreach (Transform child in Folder.transform)
        {
            ObjectData objectData = new ObjectData
            {
                name = child.gameObject.name.Split('(')[0],
                position = child.position
            };
            levelData.objects.Add(objectData);
        }

        return levelData;
    }

    private void WriteLevelDataToFile(string filePath, LevelData levelData)
    {
        Debug.Log(levelData.objects[0].name);
        string json = JsonUtility.ToJson(levelData, true);

        Debug.Log(json);
        File.WriteAllText(filePath, json);

        Debug.Log("Level data saved to " + filePath);
    }
}