using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;
public class Filters : MonoBehaviour
{
    public static bool isCoop = false;
    public static bool isDownloaded = false;
    public static WorkshopSearchOptions searchOption = WorkshopSearchOptions.SortByDate;
    [SerializeField] private GameObject coopButton;
    [SerializeField] private GameObject downloadedButton;
    [SerializeField] private GameObject SortDropdown;
    [SerializeField] private GameObject LevelSelection;
    public async void onSubmit()
    {
        isCoop = coopButton.GetComponent<Checkbox>().isChecked;
        isDownloaded = downloadedButton.GetComponent<Checkbox>().isChecked;
        string dropdown = SortDropdown.GetComponent<Text>().text;
        Debug.Log(dropdown);
        switch (dropdown)
        {
            case "Vote":
                searchOption = WorkshopSearchOptions.sortByVote;
                break;
            case "Date":
                searchOption = WorkshopSearchOptions.SortByDate;
                break;
            case "MadeByFriends":
                searchOption = WorkshopSearchOptions.madeByFriends;
                break;
            case "MostPlayed":
                searchOption = WorkshopSearchOptions.mostPlayed;
                break;
            case "Trending":
                searchOption = WorkshopSearchOptions.trending;
                break;
        }
        Debug.Log(searchOption);
        await LevelSelection.GetComponent<LevelPlayMenu>().LoadWorkshopLevels(PlayerPrefs.GetInt("Page", 1));
    }
}
