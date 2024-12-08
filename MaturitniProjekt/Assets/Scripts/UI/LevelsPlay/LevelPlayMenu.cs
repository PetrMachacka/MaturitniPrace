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
        foreach (var textComponent in textComponents)
        {
            Debug.Log(textComponent.name);
            if (textComponent.name == "Name")
            {
                textComponent.text = item.Title;
            }
            if(textComponent.name == "Rating")
            {
                textComponent.text = item.Score.ToString();
            }
            if(textComponent.name == "Author")
            {
                textComponent.text = item.Owner.Name;
            }
            if(textComponent.name == "Date")
            {
                textComponent.text = item.Updated.ToShortDateString();
            }
            if(textComponent.name == "Downloads")
            {
                textComponent.text = item.NumSubscriptions.ToString();
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

