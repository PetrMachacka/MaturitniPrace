using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    [System.Serializable]
    public class ObjectData
    {
        public string name;
        public Vector3 position;
    }

    [System.Serializable]
    public class LevelData
    {
        public string guid;
        public List<ObjectData> objects;
    }

private string prefabsPath = "Prefabs/Building";
private string levelTemplatePath = "LevelTemplates";

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

    string filePath = null;
    string json = null;

    if (newLevel)
    {
        TextAsset levelTemplate = Resources.Load<TextAsset>(Path.Combine(levelTemplatePath, "LevelData"));
        if (levelTemplate != null)
        {
            json = levelTemplate.text;
            PlayerPrefs.DeleteKey("NewLevelInt");
        }
        else
        {
            Debug.LogError("Level template not found in Resources/LevelTemplates.");
            return;
        }
    }
    else
    {
        string[] levelFiles = Directory.GetFiles(levelsPath, "*.json");

        foreach (string levelFile in levelFiles)
        {
            json = File.ReadAllText(levelFile);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            if (levelData.guid == selectedGuid)
            {
                filePath = levelFile;
                break;
            }
        }
    }

    if (json != null)
    {
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
            }
            else
            {
                Debug.LogError($"Prefab not found: {prefabPath}");
            }
        }
    }
    else
    {
        Debug.LogError("No level file found with the matching GUID: " + selectedGuid);
    }
}
}