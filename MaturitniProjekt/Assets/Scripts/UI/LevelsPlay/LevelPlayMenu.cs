using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using Assets.Scripts;
using Steamworks.Data;

public class LevelPlayMenu : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform content;

    void Start()
    {
        Workshop();
    }

    void CreateButton(Steamworks.Ugc.Item item, int index)
    {
        GameObject button = Instantiate(buttonPrefab, content);
        button.name = Path.GetFileNameWithoutExtension(item.Id.ToString());

        RectTransform rectTransform = button.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - (rectTransform.rect.height * (index + 0.5f) + 3 ));
        }

        TextMeshProUGUI[] textComponents = button.GetComponentsInChildren<TextMeshProUGUI>();
        var textMapping = new Dictionary<string, string>
        {
            { "Name", item.Title },
            { "Rating", item.Score.ToString() },
            { "Author", item.Owner.Name },
            { "Date", item.Updated.ToShortDateString() },
            { "Downloads", item.NumSubscriptions.ToString() }
        };

        foreach (var textComponent in textComponents)
        {
            if (textMapping.TryGetValue(textComponent.name, out var text))
            {
            textComponent.text = text;
            }
        }
    }
    public async void Workshop(){
        var result = await SteamManager.GetLevelListWorkshop(WorkshopSearchOptions.SortByDate, 1);
        int index = 0;
        foreach (var item in result.Value.Entries)
        {
            Debug.Log($"Title: {item.Title}, Description: {item.Description}, ID: {item.Id}");
            CreateButton(item, index);
            index++;
            //SteamManager.DownloadByID(item);
        }
    }
}

