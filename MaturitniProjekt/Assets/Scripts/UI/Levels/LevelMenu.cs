using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class LevelMenu : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform content;

    void Start()
    {
        string levelsFolderPath = Path.Combine(Application.persistentDataPath, "Levels");

        if (Directory.Exists(levelsFolderPath))
        {
            string[] jsonFiles = Directory.GetFiles(levelsFolderPath, "*.json");

            for (int i = 0; i < jsonFiles.Length; i++)
            {
                Debug.Log("Found JSON file: " + jsonFiles[i]);
                CreateButton(jsonFiles[i], i);
            }
        }
        else
        {
            Debug.LogWarning("Levels folder does not exist at path: " + levelsFolderPath);
        }
    }

    void CreateButton(string filePath, int index)
    {
        string json = File.ReadAllText(filePath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        GameObject button = Instantiate(buttonPrefab, content);
        button.name = Path.GetFileNameWithoutExtension(filePath);

        RectTransform rectTransform = button.GetComponent<RectTransform>();
        if (rectTransform != null)  
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.rect.width / 2 + 5, rectTransform.anchoredPosition.y - (index * 40));
        }

        TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = levelData.name;
        }
    }
}

[System.Serializable]
public class LevelData
{
    public string name;
    public List<ObjectData> objects;
}

[System.Serializable]
public class ObjectData
{
    public string name;
    public Vector3 position;
}