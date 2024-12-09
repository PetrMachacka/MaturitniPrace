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
    private int _page = 1;
    void Start()
    {
        Workshop(_page);
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
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y - (rectTransform.rect.height * (index + 0.5f) + (index * 3f) + 6 ));
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
    public async void Workshop(int page){
        ClearChildren(transform);
        var itemsPerPage = 6;
        var result = await SteamManager.GetLevelListWorkshop(WorkshopSearchOptions.SortByDate, 1);
        int index = 0;
        foreach (var item in result.Value.Entries)
        {
            if(index >= page * itemsPerPage - itemsPerPage && index < page * itemsPerPage)
            {
                Debug.Log($"Title: {item.Title}, Description: {item.Description}, ID: {item.Id}");
                CreateButton(item, index);
            }
            index++;
            //SteamManager.DownloadByID(item);
        }
        Debug.Log(index);
    }
    public void NextPage()
    {
        _page++;
        Workshop(_page);
    }
    public void PreviousPage()
    {
        _page--;
        Workshop(_page);
    }
}

