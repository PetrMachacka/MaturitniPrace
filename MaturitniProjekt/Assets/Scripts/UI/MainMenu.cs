using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public async void SteamUpload()
    {
        Debug.Log(LoadLevel.selectedGuid);
        await SteamManager.UploadLevelToSteamWorkshopAsync(LoadLevel.selectedGuid);
    }
    public void LevelsButton()
    {
        PlayerPrefs.DeleteKey("NewLevel");
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
