using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using Assets.Scripts;
using Steamworks.Data;
using System.Linq;
using System.Threading.Tasks;

public class LevelPlayMenu : MonoBehaviour
{
    private int itemsPerPage = 6;
    public GameObject buttonPrefab;
    public GameObject downloadedPrefab;
    public TextMeshProUGUI pageText;
    public static int _page = 1;
    private async void Start()
    {
        _page = PlayerPrefs.GetInt("Page", 1);
        await LoadWorkshopLevels(_page);
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
        string name = Path.GetFileNameWithoutExtension(item.Id.ToString());
        GameObject button;
        if(SteamManager.folderNames.Contains(name)){
            button = Instantiate(downloadedPrefab, transform);
        }
        else{
            button = Instantiate(buttonPrefab, transform);
        }
        button.name = name;

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
            { "Downloads", item.NumSubscriptions.ToString() },
            { "Mode", item.HasTag("Coop") ? "Coop" : "Single" }
        };

        foreach (var textComponent in textComponents)
        {
            if (textMapping.TryGetValue(textComponent.name, out var text))
            {
            textComponent.text = text;
            }
        }
    }
   public async Task LoadWorkshopLevels(int page)
    {
        ClearChildren(transform);
        pageText.text = $"{page}";
        int itemsPerPageSteam = 50;
        int totalItems = (page - 1) * itemsPerPage;
        int steamPage = (totalItems / itemsPerPageSteam) + 1;
        int startIndex = totalItems % itemsPerPageSteam;

        var result = await SteamManager.GetLevelListWorkshop(steamPage);
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
                CreateButton(item, index - startIndex);
            }
            index++;
        }
    }
    public async void NextPage()
    {
        _page++;
        PlayerPrefs.SetInt("Page", _page);
        await LoadWorkshopLevels(_page);
    }
    public async void PreviousPage()
    {
        if(_page > 1)
        {
            _page--;
            PlayerPrefs.SetInt("Page", _page);
            await LoadWorkshopLevels(_page);
        }
    }
}

