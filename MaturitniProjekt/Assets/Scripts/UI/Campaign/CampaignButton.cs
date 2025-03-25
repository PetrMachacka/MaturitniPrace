using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CampaignButton : MonoBehaviour
{
    public string levelGuid;
    public void PlayLevel()
    {
        Debug.Log("Playing Level GUID: " + levelGuid);
        PlayerPrefs.SetString("SelectedLevel", levelGuid);
        PlayerPrefs.SetInt("SteamDownloads", 2);

        PlayerPrefs.Save();

        SceneManager.LoadScene("PlayLevel");
    }
}
