using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public async void SteamUpload()
    {

        await SteamManager.UploadLevelToSteamWorkshopAsync(PlayerPrefs.GetString("NewLevel"));
    }
    public void LevelsButton()
    {
        PlayerPrefs.DeleteKey("NewLevel");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Levels");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
