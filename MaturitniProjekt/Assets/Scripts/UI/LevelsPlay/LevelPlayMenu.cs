using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using Assets.Scripts;
using Steamworks.Data;
using System.Linq;

public class LevelPlayMenu : MonoBehaviour
{
    private int itemsPerPage = 6;
    public GameObject buttonPrefab;
    private int _page = 1;
    void Start()
    {
        LoadWorkshopLevels(_page);
    }
    private void ClearChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }
    void CreateButton(Steamworks.Ugc.Item item, int index)
    {
        GameObject button = Instantiate(buttonPrefab, transform);
        button.name = Path.GetFileNameWithoutExtension(item.Id.ToString());

        RectTransform rectTransform = button.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - (rectTransform.rect.height * (index + 0.5f) + (index * 2.5f) + 6 ));
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
   public async void LoadWorkshopLevels(int page)
    {
        ClearChildren(transform);

        int itemsPerPageSteam = 50;
        int totalItems = (page - 1) * itemsPerPage;
        int steamPage = (totalItems / itemsPerPageSteam) + 1;
        int startIndex = totalItems % itemsPerPageSteam;

        var result = await SteamManager.GetLevelListWorkshop(WorkshopSearchOptions.SortByDate, steamPage);
        if (!result.HasValue)
        {
            Debug.LogError("Failed to get workshop levels.");
            return;
        }

        int index = 0;
        foreach (var item in result.Value.Entries)
        {
            if (index >= startIndex && index < startIndex + itemsPerPage)
            {
                Debug.Log($"Title: {item.Title}, Description: {item.Description}, ID: {item.Id}");
                CreateButton(item, index - startIndex);
            }
            index++;
        }
        Debug.Log($"Total items: {index}");
    }
    public void NextPage()
    {
        _page++;
        LoadWorkshopLevels(_page);
    }
    public void PreviousPage()
    {
        if(_page > 1)
        {
            _page--;
            LoadWorkshopLevels(_page);
        }
    }
}

