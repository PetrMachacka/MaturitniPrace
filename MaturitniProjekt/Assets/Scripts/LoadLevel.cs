using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    [System.Serializable]
    public class LevelData
    {
        public string guid;
        public List<ObjectData> objects;
    }

    [System.Serializable]
    public class ObjectData
    {
        public string name;
        public Vector3 position;
    }
    public static string selectedGuid;
    private string prefabsPath = "Prefabs/Building";

    void Start()
    {
        bool newLevel = PlayerPrefs.GetInt("NewLevelInt", 0) == 1;
        selectedGuid = PlayerPrefs.GetString("SelectedLevel", "DefaultLevel");
        Debug.Log("Loaded Level GUID: " + selectedGuid);

        string levelsPath = Path.Combine(Application.persistentDataPath, "Levels");

        if (!Directory.Exists(levelsPath))
        {
            Debug.LogError("Levels directory not found: " + levelsPath);
            return;
        }

        string filePath = GetLevelFilePath(levelsPath, newLevel, selectedGuid);
        if (filePath != null)
        {
            LoadLevelData(filePath);
        }
        else
        {
            Debug.LogError("No level file found with the matching GUID: " + selectedGuid);
        }
    }

    private string GetLevelFilePath(string levelsPath, bool newLevel, string selectedGuid)
    {
        if (newLevel)
        {
            PlayerPrefs.DeleteKey("NewLevelInt");
            return Path.Combine(Application.persistentDataPath, "LevelData.json");
        }
        else
        {
            string[] levelFiles = Directory.GetFiles(levelsPath, "*.json");

            foreach (string levelFile in levelFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(levelFile);
                if (fileName == selectedGuid)
                {
                    return levelFile;
                }
            }
        }

        return null;
    }

    private void LoadLevelData(string filePath)
    {
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
                GameObject newObject = Instantiate(prefab, obj.position, Quaternion.identity);
                newObject.transform.SetParent(buildFolder.transform);
                Debug.Log($"Instantiated {obj.name} at {obj.position}");
            }
            else
            {
                Debug.LogError($"Prefab not found: {prefabPath}");
            }
        }
    }
}