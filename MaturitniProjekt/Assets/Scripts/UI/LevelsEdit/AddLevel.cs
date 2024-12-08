using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class AddLevel : MonoBehaviour
{
    public GameObject AddLevelUI;
    public TMP_InputField inputField;
    private string levelName;

    void Start()
    {
        AddLevelUI.SetActive(false);
    }

    public void OpenAddLevelUI()
    {
        AddLevelUI.SetActive(true);
    }

    public void CloseAddLevelUI()
    {
        AddLevelUI.SetActive(false);
    }

    public void ReadInputFieldText()
    {
        levelName = inputField.text;
        Debug.Log(inputField.text);
    }

    public void CreateNewLevel()
    {
        if (string.IsNullOrEmpty(levelName))
        {
            Debug.Log("Level name is null or empty");
            return;
        }

        Debug.Log("Creating new level: " + levelName);

        string newGuid = Guid.NewGuid().ToString();

        TextAsset templateAsset = Resources.Load<TextAsset>("LevelTemplates/BasicTemplate");
        if (templateAsset == null)
        {
            Debug.LogError("BasicTemplate not found in Resources/LevelTemplates.");
            return;
        }

        LevelData templateData = JsonUtility.FromJson<LevelData>(templateAsset.text);

        LevelData newLevelData = new LevelData
        {
            name = levelName,
            objects = templateData.objects
        };

        string json = JsonUtility.ToJson(newLevelData, true);

        string levelsPath = Path.Combine(Application.persistentDataPath, "Levels", newGuid);
        if (!Directory.Exists(levelsPath))
        {
            Directory.CreateDirectory(levelsPath);
        }

        string filePath = Path.Combine(levelsPath, "levelData.json");
        File.WriteAllText(filePath, json);

        PlayerPrefs.SetString("SelectedLevel", newGuid);
        PlayerPrefs.Save();

        SceneManager.LoadScene("EditorScene");
    }

    [Serializable]
    public class LevelData
    {
        public string name;
        public List<ObjectData> objects;
    }

    [Serializable]
    public class ObjectData
    {
        public string name;
        public Vector3 position;
    }
}