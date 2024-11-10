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

    void Start()
    {
        string selectedGuid = PlayerPrefs.GetString("SelectedLevel", "DefaultLevel");
        Debug.Log("Loaded Level GUID: " + selectedGuid);

        string levelsPath = Path.Combine(Application.persistentDataPath, "Levels");

        if (!Directory.Exists(levelsPath))
        {
            Debug.LogError("Levels directory not found: " + levelsPath);
            return;
        }

        string[] levelFiles = Directory.GetFiles(levelsPath, "*.json");
        string filePath = null;

        foreach (string levelFile in levelFiles)
        {
            string json = File.ReadAllText(levelFile);
            LevelData levelData = JsonUtility.FromJson<LevelData>(json);

            if (levelData.guid == selectedGuid)
            {
                filePath = levelFile;
                break;
            }
        }

        if (filePath != null)
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