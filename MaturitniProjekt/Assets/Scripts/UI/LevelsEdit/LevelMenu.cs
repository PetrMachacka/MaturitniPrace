using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using Assets.Scripts;

public class LevelMenu : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform content;

    void Start()
    {
        
        string levelsFolderPath = Path.Combine(Application.persistentDataPath, "Levels");

        if (Directory.Exists(levelsFolderPath))
        {
            string[] jsonFiles = Directory.GetDirectories(levelsFolderPath);

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

    void CreateButton(string directoryPath, int index)
    {
        string jsonPath = Path.Combine(directoryPath, "levelData.json");
        string json = File.ReadAllText(jsonPath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        GameObject button = Instantiate(buttonPrefab, content);
        button.name = Path.GetFileNameWithoutExtension(directoryPath);

        RectTransform rectTransform = button.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.offsetMin = new Vector2(5, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(-5, rectTransform.offsetMax.y);

            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -((rectTransform.rect.height + 3) * index) - rectTransform.rect.height / 2 - 5);
        }

        TextMeshProUGUI nameHolder = button.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI CoopHolder = button.transform.Find("CoopHolder").GetComponent<TextMeshProUGUI>();
        LevelButtonScript buttonScript = button.GetComponent<LevelButtonScript>();
        if (nameHolder != null)
        {
            nameHolder.text = levelData.name;
            CoopHolder.text = levelData.isCoop ? "Coop" : "Single";
            buttonScript.isCoop = levelData.isCoop;
        }
    }
    public async void Workshop(){
        var result = await SteamManager.GetLevelListWorkshop(WorkshopSearchOptions.SortByDate, 1);

        foreach (var item in result.Value.Entries)
        {
            Debug.Log($"Title: {item.Title}, Description: {item.Description}, ID: {item.Id}");
            SteamManager.DownloadByID(item);
        }
    }
}

