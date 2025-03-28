using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class Menu : MonoBehaviour
{
    public GameObject LevelEditSelection;
    public GameObject LevelPlaySelection;
    public GameObject LevelPlayCommunitySelection;
    public GameObject Settings;
    public GameObject CampaignLevels;
    public TextMeshProUGUI text1;
    private void Start()
    {
        Debug.Log(SteamManager.steamId);
        if (SteamManager.steamId != "76561198361913117")
        {
            Debug.Log("Not the owner of the game, setting text to Chimken.");
            text1.text = "Chimken";
        }
        else
        {
            text1.text = "Šimken";
        }

        switch (PlayerPrefs.GetString("OpenMenu"))
        {
            case "PlaySelection":
                LevelPlaySelection.SetActive(true);
                break;
            case "EditSelection":
                LevelEditSelection.SetActive(true);
                break;
            case "CommunityPlaySelection":
                LevelPlaySelection.SetActive(true);
                break;
            case "Settings":
                Settings.SetActive(true);
                break;
            case "CampaignSelection":
                CampaignLevels.SetActive(true);
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void PlaySelection()
    {
        if(LevelPlaySelection.activeSelf)
        {
            LevelPlaySelection.SetActive(false);
        }
        else
        {
            LevelPlaySelection.SetActive(true);
            PlayerPrefs.SetString("OpenMenu", "PlaySelection");
        }
    }
    public void EditSelection()
    {
        if(LevelEditSelection.activeSelf)
        {
            LevelEditSelection.SetActive(false);
        }
        else
        {
            LevelEditSelection.SetActive(true);
            PlayerPrefs.SetString("OpenMenu", "EditSelection");
        }
    }
    public void PlayCommunitySelection()
    {
        if(LevelPlayCommunitySelection.activeSelf)
        {
            LevelPlayCommunitySelection.SetActive(false);
        }
        else if(SteamManager.isSteamActive)
        {
            LevelPlayCommunitySelection.SetActive(true);
            PlayerPrefs.SetString("OpenMenu", "CommunityPlaySelection");
        }
    }
    public void CampaignSelection()
    {
        if(CampaignLevels.activeSelf)
        {
            CampaignLevels.SetActive(false);
        }
        else
        {
            CampaignLevels.SetActive(true);
            PlayerPrefs.SetString("OpenMenu", "CampaignSelection");
        }
    }
    public void SettingsMenu()
    {
        if(Settings.activeSelf)
        {
            Settings.SetActive(false);
        }
        else
        {
            Settings.SetActive(true);
            PlayerPrefs.SetString("OpenMenu", "Settings");
        }
    }

    private void OnExit(){
        switch (PlayerPrefs.GetString("OpenMenu"))
        {
            case "PlaySelection":
                LevelPlaySelection.SetActive(false);
                break;
            case "EditSelection":
                LevelEditSelection.SetActive(false);
                break;
            case "CommunityPlaySelection":
                LevelPlayCommunitySelection.SetActive(false);
                LevelPlaySelection.SetActive(false);
                break;
            case "Settings":
                Settings.SetActive(false);
                break;
        }
    }
}
